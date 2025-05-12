''' <summary>
''' Custom class to handle collections of strings, offering easy add and remove functionality.
''' </summary>
Public Class AuraList : Inherits List(Of Object)
    Sub New()
        MyBase.New()
    End Sub
    ''' <summary>
    ''' Adds an item to the AuraList.
    ''' </summary>
    ''' <param name="item">The item to add.</param>
    Public Overloads Sub Add(item As Object)
        MyBase.Add(item)
    End Sub

    ''' <summary>
    ''' Removes an item from the AuraList.
    ''' </summary>
    ''' <param name="item">The item to remove.</param>
    Public Overloads Sub Remove(item As Object)
        MyBase.Remove(item)
    End Sub

    ''' <summary>
    ''' Gets all items as a List of Strings.
    ''' </summary>
    ''' <returns>A List of Strings.</returns>
    Public Function GetItems() As List(Of Object)
        Return MyBase.ToList()
    End Function

    ''' <summary>
    ''' Converts the AuraList to a JSON-compatible list.
    ''' </summary>
    ''' <returns>A List of Strings.</returns>
    Public Function ToList() As List(Of Object)
        Return MyBase.ToList()
    End Function
End Class
