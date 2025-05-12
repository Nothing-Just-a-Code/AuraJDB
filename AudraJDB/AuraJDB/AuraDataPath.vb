''' <summary>
''' Helper class to manage dynamic paths.
''' </summary>
Public Class AuraDataPath
    Private _path As String

    ''' <summary>
    ''' Constructor to initialize the path.
    ''' </summary>
    Public Sub New()
        _path = ""
    End Sub

    ''' <summary>
    ''' Adds a new segment to the current path.
    ''' </summary>
    ''' <param name="segment">The path segment to add.</param>
    ''' <returns>The updated AuraDataPath instance.</returns>
    Public Function AddPath(segment As String) As AuraDataPath
        If String.IsNullOrEmpty(_path) Then
            _path = segment
        Else
            _path = $"{_path}.{segment}"
        End If
        Return Me
    End Function

    ''' <summary>
    ''' Gets the full path.
    ''' </summary>
    ''' <returns>The full path as a string.</returns>
    Public Function GetPath() As String
        Return _path
    End Function
End Class
