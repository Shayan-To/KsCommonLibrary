Public Class MultiDictionary(Of TKey, TValue)
    Implements IReadOnlyDictionary(Of TKey, MultiDictionaryList(Of TKey, TValue))

    Private Sub IncrementVersion()
        If Me.Version = 255 Then
            Me.Version = 0
        Else
            Me.Version = CByte(Me.Version + 1)
        End If
    End Sub

    Friend Sub ReportKeyEmpty(ByVal List As MultiDictionaryList(Of TKey, TValue))
        Assert.True(Me.Dic.Remove(List.Key))
        Me.IncrementVersion()
    End Sub

    Friend Sub ReportKeyFilled(ByVal List As MultiDictionaryList(Of TKey, TValue))
        Me.Dic.Add(List.Key, List.List)
        Me.IncrementVersion()
    End Sub

    Public Sub Clear()
        Me.Dic.Clear()
        Me.IncrementVersion()
    End Sub

    Public ReadOnly Property Count As Integer Implements IReadOnlyCollection(Of KeyValuePair(Of TKey, MultiDictionaryList(Of TKey, TValue))).Count
        Get
            Return Me.Dic.Count
        End Get
    End Property

    Default Public ReadOnly Property Item(key As TKey) As MultiDictionaryList(Of TKey, TValue) Implements IReadOnlyDictionary(Of TKey, MultiDictionaryList(Of TKey, TValue)).Item
        Get
            Dim L As List(Of TValue) = Nothing
            If Me.Dic.TryGetValue(key, L) Then
                Return New MultiDictionaryList(Of TKey, TValue)(Me, key, L)
            End If
            Return New MultiDictionaryList(Of TKey, TValue)(Me, key, Nothing)
        End Get
    End Property

    Public ReadOnly Property Keys As IEnumerable(Of TKey) Implements IReadOnlyDictionary(Of TKey, MultiDictionaryList(Of TKey, TValue)).Keys
        Get
            Return Me.Dic.Keys
        End Get
    End Property

    Public ReadOnly Iterator Property Values As IEnumerable(Of MultiDictionaryList(Of TKey, TValue)) Implements IReadOnlyDictionary(Of TKey, MultiDictionaryList(Of TKey, TValue)).Values
        Get
            For Each KV In Me.Dic
                Yield New MultiDictionaryList(Of TKey, TValue)(Me, KV.Key, KV.Value)
            Next
        End Get
    End Property

    Public Function ContainsKey(key As TKey) As Boolean Implements IReadOnlyDictionary(Of TKey, MultiDictionaryList(Of TKey, TValue)).ContainsKey
        Return Me.Dic.ContainsKey(key)
    End Function

    Public Iterator Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, MultiDictionaryList(Of TKey, TValue)))
        For Each KV In Me.Dic
            Yield New KeyValuePair(Of TKey, MultiDictionaryList(Of TKey, TValue))(KV.Key, New MultiDictionaryList(Of TKey, TValue)(Me, KV.Key, KV.Value))
        Next
    End Function

    Private Function IEnumerable_1_GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, MultiDictionaryList(Of TKey, TValue))) Implements IEnumerable(Of KeyValuePair(Of TKey, MultiDictionaryList(Of TKey, TValue))).GetEnumerator
        Return Me.GetEnumerator()
    End Function

    Public Function TryGetValue(key As TKey, ByRef value As MultiDictionaryList(Of TKey, TValue)) As Boolean Implements IReadOnlyDictionary(Of TKey, MultiDictionaryList(Of TKey, TValue)).TryGetValue
        Dim L As List(Of TValue) = Nothing
        If Me.Dic.TryGetValue(key, L) Then
            value = New MultiDictionaryList(Of TKey, TValue)()
            Return True
        End If
        Return False
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function

    Friend Version As Byte
    Friend ReadOnly Dic As Dictionary(Of TKey, List(Of TValue)) = New Dictionary(Of TKey, List(Of TValue))()

End Class
