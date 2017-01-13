Imports System.Collections.Specialized

Namespace Common

    Public Interface IOrderedDictionary(Of TKey, TValue)
        Inherits IOrderedDictionary,
                 IList,
                 IList(Of KeyValuePair(Of TKey, TValue)),
                 IDictionary(Of TKey, TValue),
                 IDictionary

        Overloads Sub Insert(index As Integer, key As TKey, value As TValue)

    End Interface

End Namespace
