Namespace Common

    Public MustInherit Class BaseDictionary(Of TKey, TValue)
        Implements IReadOnlyDictionary(Of TKey, TValue),
                   IDictionary(Of TKey, TValue),
                   IDictionary

        Public MustOverride ReadOnly Property Count As Integer Implements ICollection.Count, IDictionary(Of TKey, TValue).Count, IReadOnlyDictionary(Of TKey, TValue).Count

        Protected Overridable ReadOnly Property IsReadOnly As Boolean Implements IDictionary.IsReadOnly, IDictionary(Of TKey, TValue).IsReadOnly
            Get
                Return False
            End Get
        End Property

#Region "Junk"
        Private Property IDictionary_Item(key As Object) As Object Implements IDictionary.Item
            Get
                Return Me.Item(DirectCast(key, TKey))
            End Get
            Set(value As Object)
                Me.Item(DirectCast(key, TKey)) = DirectCast(value, TValue)
            End Set
        End Property

        Private ReadOnly Property IReadOnlyDictionary_Item(key As TKey) As TValue Implements IReadOnlyDictionary(Of TKey, TValue).Item
            Get
                Return Me.Item(key)
            End Get
        End Property

        Private ReadOnly Property IDictionary_IsFixedSize As Boolean Implements IDictionary.IsFixedSize
            Get
                Return False
            End Get
        End Property

        Private ReadOnly Property ICollection_IsSynchronized As Boolean Implements ICollection.IsSynchronized
            Get
                Return False
            End Get
        End Property

        Private ReadOnly Property ICollection_SyncRoot As Object Implements ICollection.SyncRoot
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Private Function IReadOnlyDictionary_TryGetValue(key As TKey, ByRef value As TValue) As Boolean Implements IReadOnlyDictionary(Of TKey, TValue).TryGetValue
            Return Me.TryGetValue(key, value)
        End Function

        Protected Sub ICollection_Add(item As KeyValuePair(Of TKey, TValue)) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Add
            Me.Add(item.Key, item.Value)
        End Sub

        Private Sub IDictionary_Add(key As Object, value As Object) Implements IDictionary.Add
            Me.Add(DirectCast(key, TKey), DirectCast(value, TValue))
        End Sub

        Protected Function ICollection_Remove(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Remove
            Dim V As TValue = Nothing
            If Not Me.TryGetValue(item.Key, V) Then
                Return False
            End If
            If Not Object.Equals(V, item.Value) Then
                Return False
            End If
            Return Me.Remove(item.Key)
        End Function

        Private Sub IDictionary_Remove(key As Object) Implements IDictionary.Remove
            Me.Remove(DirectCast(key, TKey))
        End Sub

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.IEnumerator_1_GetEnumerator()
        End Function

        Protected Function ICollection_Contains(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Contains
            Dim V As TValue = Nothing
            If Not Me.TryGetValue(item.Key, V) Then
                Return False
            End If
            Return Object.Equals(V, item.Value)
        End Function

        Private Function IDictionary_Contains(key As Object) As Boolean Implements IDictionary.Contains
            Return Me.ContainsKey(DirectCast(key, TKey))
        End Function

        Private Function IReadOnlyDictionary_ContainsKey(key As TKey) As Boolean Implements IReadOnlyDictionary(Of TKey, TValue).ContainsKey
            Return Me.ContainsKey(key)
        End Function
#End Region

        Default Public MustOverride Property Item(key As TKey) As TValue Implements IDictionary(Of TKey, TValue).Item

        Protected Overridable ReadOnly Property IDictionary_Keys As ICollection Implements IDictionary.Keys
            Get
                Return DirectCast(Me.Keys, ICollection)
            End Get
        End Property

        Protected Overridable ReadOnly Property IDictionary_Values As ICollection Implements IDictionary.Values
            Get
                Return DirectCast(Me.Values, ICollection)
            End Get
        End Property

        Private ReadOnly Property IReadOnlyDictionary_Keys As IEnumerable(Of TKey) Implements IReadOnlyDictionary(Of TKey, TValue).Keys
            Get
                Return Me.Keys
            End Get
        End Property

        Private ReadOnly Property IReadOnlyDictionary_Values As IEnumerable(Of TValue) Implements IReadOnlyDictionary(Of TKey, TValue).Values
            Get
                Return Me.Values
            End Get
        End Property

        Public MustOverride ReadOnly Property Keys As ICollection(Of TKey) Implements IDictionary(Of TKey, TValue).Keys

        Public MustOverride ReadOnly Property Values As ICollection(Of TValue) Implements IDictionary(Of TKey, TValue).Values

        Public MustOverride Sub Add(key As TKey, value As TValue) Implements IDictionary(Of TKey, TValue).Add

        Public MustOverride Sub Clear() Implements IDictionary.Clear, IDictionary(Of TKey, TValue).Clear

        Public Overridable Sub CopyTo(array() As KeyValuePair(Of TKey, TValue), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).CopyTo
            Me.CopyTo(DirectCast(array, Array), arrayIndex)
        End Sub

        Protected Overridable Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
            Verify.TrueArg(array.Rank = 1, NameOf(array), "Array's rank must be 1.")
            Verify.TrueArg(index + Me.Count <= array.Length, NameOf(array), "Array does not have enough length to copy the collection.")
            For Each I In Me
                array.SetValue(I, index)
                index += 1
            Next
        End Sub

        Public MustOverride Function ContainsKey(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).ContainsKey

        Public MustOverride Function Remove(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).Remove

        Public MustOverride Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean Implements IDictionary(Of TKey, TValue).TryGetValue

        Protected Overridable Function IDictionary_GetEnumerator() As IDictionaryEnumerator Implements IDictionary.GetEnumerator
            Return New DictionaryEnumerator(Of TKey, TValue, IEnumerator(Of KeyValuePair(Of TKey, TValue)))(Me.IEnumerator_1_GetEnumerator())
        End Function

        Protected MustOverride Function IEnumerator_1_GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IEnumerable(Of KeyValuePair(Of TKey, TValue)).GetEnumerator

    End Class

End Namespace
