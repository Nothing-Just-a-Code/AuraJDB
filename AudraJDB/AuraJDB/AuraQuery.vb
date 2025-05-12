
Imports Newtonsoft.Json.Linq

''' <summary>
''' Provides LINQ-style filtering and selection on arrays.
''' </summary>
Public Class AuraQuery
    Private ReadOnly _items As IEnumerable(Of JObject)

    Public Sub New(array As JArray)
        _items = array.OfType(Of JObject)()
    End Sub

    ''' <summary>
    ''' Filters array items using a predicate function.
    ''' </summary>
    Public Function Where(predicate As Func(Of JObject, Boolean)) As AuraQuery
        Dim filtered = _items.Where(predicate).ToList()
        Return New AuraQuery(New JArray(filtered))
    End Function

    ''' <summary>
    ''' Projects each filtered JObject into a custom type.
    ''' </summary>
    Public Function SelectItems(Of T)(selector As Func(Of JObject, T)) As List(Of T)
        Return _items.Select(selector).ToList()
    End Function

    ''' <summary>
    ''' Selects the first item that matches the condition and applies a projection.
    ''' </summary>
    Public Function SelectSingleItem(Of T)(predicate As Func(Of JObject, Boolean), selector As Func(Of JObject, T)) As T
        Dim item = _items.FirstOrDefault(predicate)
        If item IsNot Nothing Then
            Return selector(item)
        End If
        Return Nothing
    End Function
End Class