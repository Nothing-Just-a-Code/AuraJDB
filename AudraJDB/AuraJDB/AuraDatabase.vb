Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.IO

''' <summary>
''' Main class for handling database interactions, including reading and writing data.
''' </summary>
Public Class AuraJDB
    Private _filePath As String
    Private _inMemoryJson As JObject
    ''' <summary>
    ''' Constructor to initialize the file path.
    ''' </summary>
    ''' <param name="filePath">The path to the database file.</param>
    Public Sub New(filePath As String)
        _filePath = $"{filePath}.adb"
        If File.Exists(_filePath) Then
            _inMemoryJson = JObject.Parse(File.ReadAllText(_filePath))
        Else
            _inMemoryJson = New JObject()
        End If
    End Sub


    ''' <summary>
    ''' Writes a key-value pair into the database at the specified path. It updates the value if the key already exists.
    ''' </summary>
    Public Sub Write(path As AuraDataPath, key As String, value As Object)
        SyncLock _inMemoryJson
            ' Navigate or create the nested path
            Dim segments As String() = path.GetPath().Split("."c)
            Dim current As JObject = _inMemoryJson

            For Each segment In segments
                If current(segment) Is Nothing OrElse current(segment).Type <> JTokenType.Object Then
                    current(segment) = New JObject()
                End If
                current = CType(current(segment), JObject)
            Next

            ' Convert AuraList to JArray if needed
            If TypeOf value Is AuraList Then
                Dim list As New JArray()
                For Each item In TryCast(value, AuraList).ToList()
                    list.Add(JToken.FromObject(item))
                Next
                current(key) = list
            Else
                current(key) = JToken.FromObject(value)
            End If

            ' Write updated JSON back to the file immediately
            File.WriteAllText(_filePath, _inMemoryJson.ToString(Formatting.Indented))
        End SyncLock
    End Sub

    ''' <summary>
    ''' Deletes a specific key at a given path in the database.
    ''' </summary>
    ''' <param name="path">The path to the data in the database.</param>
    ''' <param name="key">The key to delete from the path.</param>
    Public Sub DeleteKey(path As AuraDataPath, key As String)
        SyncLock _inMemoryJson
            ' Navigate through the JSON object to the correct path
            Dim segments As String() = path.GetPath().Split("."c)
            Dim current As JObject = _inMemoryJson

            ' Traverse the path
            For Each segment As String In segments
                If current(segment) Is Nothing OrElse current(segment).Type <> JTokenType.Object Then
                    ' If the segment doesn't exist, exit (nothing to delete)
                    Return
                End If
                current = CType(current(segment), JObject)
            Next

            ' Remove the specific key
            If current.ContainsKey(key) Then
                current.Remove(key)
                ' Write the updated JSON back to the file
                File.WriteAllText(_filePath, _inMemoryJson.ToString(Formatting.Indented))
            End If
        End SyncLock
    End Sub

    ''' <summary>
    ''' Deletes an entire path (branch) from the database.
    ''' </summary>
    ''' <param name="path">The path to the data in the database.</param>
    Public Sub DeletePath(path As AuraDataPath)
        SyncLock _inMemoryJson
            ' Navigate through the JSON object to the correct path
            Dim segments As String() = path.GetPath().Split("."c)
            Dim current As JObject = _inMemoryJson

            ' Traverse to the parent of the path to be deleted
            For i As Integer = 0 To segments.Length - 2
                If current(segments(i)) Is Nothing OrElse current(segments(i)).Type <> JTokenType.Object Then
                    ' If the segment doesn't exist, exit (nothing to delete)
                    Return
                End If
                current = CType(current(segments(i)), JObject)
            Next

            ' Remove the entire path (branch)
            If current.ContainsKey(segments(segments.Length - 1)) Then
                current.Remove(segments(segments.Length - 1))
                ' Write the updated JSON back to the file
                File.WriteAllText(_filePath, _inMemoryJson.ToString(Formatting.Indented))
            End If
        End SyncLock
    End Sub



    ''' <summary>
    ''' Reads data from a specified path and returns an AuraList if the data is a list of strings.
    ''' </summary>
    ''' <param name="path">The path in the database.</param>
    ''' <returns>An AuraList containing the data, or Nothing if not found.</returns>
    Public Function Read(path As AuraDataPath) As AuraList
        ' Fetch the serialized data from the database (for simplicity, assuming this is in JSON format)
        Dim serializedData As String = GetDataFromDatabase(path.GetPath())

        ' Try to deserialize into an AuraList (if it exists in the database as a List of Strings)
        Try
            ' Deserialize the data into a List(Of String)
            Dim listData As List(Of String) = JsonConvert.DeserializeObject(Of List(Of String))(serializedData)

            ' Return an AuraList instance filled with the deserialized data
            Dim auraList As New AuraList()
            For Each item In listData
                auraList.Add(item)
            Next

            Return auraList
        Catch ex As Exception
            ' If there was an error, return Nothing (or handle the error as needed)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Searches the database and returns the first AuraObject where the specified key equals the given value.
    ''' </summary>
    Public Function Search(key As String, value As String) As AuraObject
        Dim root As JObject = JObject.Parse(File.ReadAllText(_filePath))
        Dim result As JObject = FindMatchingObject(root, key, value)
        If result IsNot Nothing Then
            Return New AuraObject(result)
        End If
        Return Nothing
    End Function

    Private Function FindMatchingObject(token As JToken, key As String, value As String) As JObject
        If TypeOf token Is JObject Then
            Dim obj As JObject = CType(token, JObject)
            If obj.TryGetValue(key, StringComparison.OrdinalIgnoreCase, Nothing) AndAlso obj(key).ToString() = value Then
                Return obj
            End If

            For Each prop As JProperty In obj.Properties()
                Dim result = FindMatchingObject(prop.Value, key, value)
                If result IsNot Nothing Then Return result
            Next
        ElseIf TypeOf token Is JArray Then
            For Each item In token.Children()
                Dim result = FindMatchingObject(item, key, value)
                If result IsNot Nothing Then Return result
            Next
        End If

        Return Nothing
    End Function


    ''' <summary>
    ''' Helper function to save serialized data to the file.
    ''' </summary>
    ''' <param name="path">The path in the database.</param>
    ''' <param name="data">The serialized data to save.</param>
    Private Sub SaveToDatabase(path As String, data As String)
        Dim jsonData As String = GetDatabaseContent()
        Dim parsedJson As Dictionary(Of String, Object) = If(String.IsNullOrEmpty(jsonData), New Dictionary(Of String, Object)(), JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(jsonData))
        parsedJson(path) = data
        File.WriteAllText(_filePath, JsonConvert.SerializeObject(parsedJson))
    End Sub

    ''' <summary>
    ''' Helper function to get the current content of the database.
    ''' </summary>
    ''' <returns>The JSON content of the database as a string.</returns>
    Private Function GetDatabaseContent() As String
        If Not File.Exists(_filePath) Then
            Return "{}" ' Return empty JSON object if file doesn't exist
        End If
        Return File.ReadAllText(_filePath)
    End Function

    ''' <summary>
    ''' Helper function to retrieve data from the database.
    ''' </summary>
    ''' <param name="path">The path to fetch data from.</param>
    ''' <returns>The serialized data from the database, or an empty string if not found.</returns>
    Private Function GetDataFromDatabase(path As String) As String
        Dim jsonData As String = GetDatabaseContent()
        Dim parsedJson As Dictionary(Of String, Object) = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(jsonData)

        If parsedJson.ContainsKey(path) Then
            Return parsedJson(path).ToString()
        Else
            Return String.Empty ' Return an empty string if path doesn't exist
        End If
    End Function

    ''' <summary>
    ''' Get a single item in your database by using its path and key.
    ''' </summary>
    ''' <param name="path">Path of your Database value (not database file) Ex.: 'Data.Players' is the Path.</param>
    ''' <param name="key">Key of your item which you want to get. Ex.: 'playername:jason' where playername is key and jason is value.</param>
    ''' <returns></returns>
    Public Function GetSingleItem(path As AuraDataPath, key As String) As Object
        ' Load the entire JSON from the database
        Dim rootJson As JObject
        If File.Exists(_filePath) Then
            rootJson = JObject.Parse(File.ReadAllText(_filePath))
        Else
            rootJson = New JObject()
        End If

        ' Navigate through the path
        Dim segments As String() = path.GetPath().Split("."c)
        Dim current As JObject = rootJson

        For Each segment In segments
            If current(segment) Is Nothing OrElse current(segment).Type <> JTokenType.Object Then
                ' If the segment is not found, return Nothing
                Return Nothing
            End If
            current = CType(current(segment), JObject)
        Next

        ' Now that we've navigated to the correct path, try to get the value for the key
        If current.ContainsKey(key) Then
            Return current(key).ToObject(Of Object)() ' Return the value
        Else
            Return Nothing ' If the key doesn't exist, return Nothing
        End If
    End Function

End Class
