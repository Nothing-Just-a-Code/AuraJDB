Imports Newtonsoft.Json.Linq


''' <summary>
''' Represents a JSON object in the AuraJDB database.
''' </summary>
Public Class AuraObject
    Private _jObject As JObject

    Public Sub New(jObject As JObject)
        _jObject = jObject
    End Sub

    ''' <summary>
    ''' Gets the value for the given property name as a string.
    ''' </summary>
    Public Function GetString(key As String) As String
        Return _jObject(key)?.ToString()
    End Function

    ''' <summary>
    ''' Gets the raw Newtonsoft JObject (if needed).
    ''' </summary>
    Public Function Raw() As JObject
        Return _jObject
    End Function

    ''' <summary>
    ''' Indexer to get any value directly.
    ''' </summary>
    Default Public ReadOnly Property Item(key As String) As JToken
        Get
            Return _jObject(key)
        End Get
    End Property
    Public Function GetValue(Of T)(token As JToken) As T
        If GetType(T) Is GetType(AuraList) AndAlso token.Type = JTokenType.Array Then
            Dim list As New AuraList()

            ' Iterate through each item in the JArray
            For Each Itemm In token
                ' Check the actual type of the item and add it to the AuraList
                Select Case Itemm.Type
                    Case JTokenType.String
                        list.Add(Itemm.ToString()) ' Add as String
                    Case JTokenType.Integer
                        list.Add(Itemm.ToObject(Of Integer)()) ' Add as Integer
                    Case JTokenType.Boolean
                        list.Add(Itemm.ToObject(Of Boolean)()) ' Add as Boolean
                    Case Else
                        list.Add(Itemm.ToObject(Of Object)()) ' Default to Object if type is unknown
                End Select
            Next

            Return CType(CType(list, Object), T)
        End If

        ' For non-AuraList values, convert the token to the expected type
        Return token.ToObject(Of T)()
    End Function


End Class
