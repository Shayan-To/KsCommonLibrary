Namespace Common

    Public Class JsonDynamicDictionary
        Inherits JsonDynamicBase
        Implements IReadOnlyDictionary(Of String, JsonDynamicBase),
               IDictionary(Of String, JsonDynamicBase),
               IDictionary

        Public ReadOnly Property Count As Integer Implements ICollection.Count, IDictionary(Of String, JsonDynamicBase).Count, IReadOnlyDictionary(Of String, JsonDynamicBase).Count
            Get
                Return Me.Base.Count
            End Get
        End Property

        Protected Overridable ReadOnly Property IsReadOnly As Boolean Implements IDictionary.IsReadOnly, IDictionary(Of String, JsonDynamicBase).IsReadOnly
            Get
                Return False
            End Get
        End Property

#Region "Junk"
        Private Property IDictionary_Item(key As Object) As Object Implements IDictionary.Item
            Get
                Return Me.Item(DirectCast(key, String))
            End Get
            Set(value As Object)
                Me.Item(DirectCast(key, String)) = DirectCast(value, JsonDynamicBase)
            End Set
        End Property

        Private ReadOnly Property IReadOnlyDictionary_Item(key As String) As JsonDynamicBase Implements IReadOnlyDictionary(Of String, JsonDynamicBase).Item
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

        Private Function IReadOnlyDictionary_TryGetValue(key As String, ByRef value As JsonDynamicBase) As Boolean Implements IReadOnlyDictionary(Of String, JsonDynamicBase).TryGetValue
            Return Me.TryGetValue(key, value)
        End Function

        Protected Sub ICollection_Add(item As KeyValuePair(Of String, JsonDynamicBase)) Implements ICollection(Of KeyValuePair(Of String, JsonDynamicBase)).Add
            Me.Add(item.Key, item.Value)
        End Sub

        Private Sub IDictionary_Add(key As Object, value As Object) Implements IDictionary.Add
            Me.Add(DirectCast(key, String), DirectCast(value, JsonDynamicBase))
        End Sub

        Protected Function ICollection_Remove(item As KeyValuePair(Of String, JsonDynamicBase)) As Boolean Implements ICollection(Of KeyValuePair(Of String, JsonDynamicBase)).Remove
            Dim V As JsonDynamicBase = Nothing
            If Not Me.TryGetValue(item.Key, V) Then
                Return False
            End If
            If Not Object.Equals(V, item.Value) Then
                Return False
            End If
            Return Me.Remove(item.Key)
        End Function

        Private Sub IDictionary_Remove(key As Object) Implements IDictionary.Remove
            Me.Remove(DirectCast(key, String))
        End Sub

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function

        Protected Function ICollection_Contains(item As KeyValuePair(Of String, JsonDynamicBase)) As Boolean Implements ICollection(Of KeyValuePair(Of String, JsonDynamicBase)).Contains
            Dim V As JsonDynamicBase = Nothing
            If Not Me.TryGetValue(item.Key, V) Then
                Return False
            End If
            Return Object.Equals(V, item.Value)
        End Function

        Private Function IDictionary_Contains(key As Object) As Boolean Implements IDictionary.Contains
            Return Me.ContainsKey(DirectCast(key, String))
        End Function

        Private Function IReadOnlyDictionary_ContainsKey(key As String) As Boolean Implements IReadOnlyDictionary(Of String, JsonDynamicBase).ContainsKey
            Return Me.ContainsKey(key)
        End Function
#End Region

        Default Public Property Item(key As String) As JsonDynamicBase Implements IDictionary(Of String, JsonDynamicBase).Item
            Get
                Return Me.Base.Item(key)
            End Get
            Set(value As JsonDynamicBase)
                Me.Base.Item(key) = value
            End Set
        End Property

        Public Property ItemValue(key As String) As JsonDynamicValue
            Get
                Return DirectCast(Me.Base.Item(key), JsonDynamicValue)
            End Get
            Set(value As JsonDynamicValue)
                Me.Base.Item(key) = value
            End Set
        End Property

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

        Private ReadOnly Property IReadOnlyDictionary_Keys As IEnumerable(Of String) Implements IReadOnlyDictionary(Of String, JsonDynamicBase).Keys
            Get
                Return Me.Keys
            End Get
        End Property

        Private ReadOnly Property IReadOnlyDictionary_Values As IEnumerable(Of JsonDynamicBase) Implements IReadOnlyDictionary(Of String, JsonDynamicBase).Values
            Get
                Return Me.Values
            End Get
        End Property

        Public ReadOnly Property Keys As ICollection(Of String) Implements IDictionary(Of String, JsonDynamicBase).Keys
            Get
                Return Me.Base.Keys
            End Get
        End Property

        Public ReadOnly Property Values As ICollection(Of JsonDynamicBase) Implements IDictionary(Of String, JsonDynamicBase).Values
            Get
                Return Me.Base.Values
            End Get
        End Property

        Public Sub Add(key As String, value As JsonDynamicBase) Implements IDictionary(Of String, JsonDynamicBase).Add
            Me.Base.Add(key, value)
        End Sub

        Public Sub Clear() Implements IDictionary.Clear, IDictionary(Of String, JsonDynamicBase).Clear
            Me.Base.Clear()
        End Sub

        Public Overridable Sub CopyTo(array() As KeyValuePair(Of String, JsonDynamicBase), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of String, JsonDynamicBase)).CopyTo
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

        Public Function ContainsKey(key As String) As Boolean Implements IDictionary(Of String, JsonDynamicBase).ContainsKey
            Return Me.Base.ContainsKey(key)
        End Function

        Public Function Remove(key As String) As Boolean Implements IDictionary(Of String, JsonDynamicBase).Remove
            Return Me.Base.Remove(key)
        End Function

        Public Function TryGetValue(key As String, ByRef value As JsonDynamicBase) As Boolean Implements IDictionary(Of String, JsonDynamicBase).TryGetValue
            Return Me.Base.TryGetValue(key, value)
        End Function

        Protected Overridable Function IDictionary_GetEnumerator() As IDictionaryEnumerator Implements IDictionary.GetEnumerator
            Return New DictionaryEnumerator(Of String, JsonDynamicBase, IEnumerator(Of KeyValuePair(Of String, JsonDynamicBase)))(Me.GetEnumerator())
        End Function

        Protected Function IEnumerator_1_GetEnumerator() As IEnumerator(Of KeyValuePair(Of String, JsonDynamicBase)) Implements IEnumerable(Of KeyValuePair(Of String, JsonDynamicBase)).GetEnumerator
            Return Me.GetEnumerator()
        End Function

        Public Function GetEnumerator() As Dictionary(Of String, JsonDynamicBase).Enumerator
            Return Me.Base.GetEnumerator()
        End Function

        Private ReadOnly Base As Dictionary(Of String, JsonDynamicBase) = New Dictionary(Of String, JsonDynamicBase)()

    End Class

End Namespace
