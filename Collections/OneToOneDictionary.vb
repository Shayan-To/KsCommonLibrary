Public Class OneToOneDictionary(Of TKey, TValue)
    Implements IDictionary(Of TKey, TValue),
               IDictionary

    Public Sub New(ByVal KeySelector As Func(Of TValue, TKey))
        Me.New(New Dictionary(Of TKey, TValue)(), KeySelector)
    End Sub

    Public Sub New(ByVal BaseDictionary As IDictionary(Of TKey, TValue), ByVal KeySelector As Func(Of TValue, TKey))
        Me.BaseDictionary = BaseDictionary
        Me.KeySelector = KeySelector
    End Sub

#Region "BaseDictionary"
#Region "Junk"
    Private ReadOnly Property IsReadOnly As Boolean Implements IDictionary.IsReadOnly, IDictionary(Of TKey, TValue).IsReadOnly
        Get
            Return False
        End Get
    End Property

    Private Property IDictionary_Item(key As Object) As Object Implements IDictionary.Item
        Get
            Return Me.IDictionary_Item(DirectCast(key, TKey))
        End Get
        Set(value As Object)
            Me.IDictionary_Item(DirectCast(key, TKey)) = DirectCast(value, TValue)
        End Set
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

    Private Sub ICollection_Add(item As KeyValuePair(Of TKey, TValue)) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Add
        Me.IDictionary_Add(item.Key, item.Value)
    End Sub

    Private Sub IDictionary_Add(key As Object, value As Object) Implements IDictionary.Add
        Me.IDictionary_Add(DirectCast(key, TKey), DirectCast(value, TValue))
    End Sub

    Private Function ICollection_Remove(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Remove
        Dim V As TValue = Nothing
        If Not Me.TryGetValue(item.Key, V) Then
            Return False
        End If
        If Not Object.Equals(V, item.Value) Then
            Return False
        End If
        Return Me.RemoveKey(item.Key)
    End Function

    Private Sub IDictionary_Remove(key As Object) Implements IDictionary.Remove
        Me.RemoveKey(DirectCast(key, TKey))
    End Sub

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function

    Private Function ICollection_Contains(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Contains
        Dim V As TValue = Nothing
        If Not Me.TryGetValue(item.Key, V) Then
            Return False
        End If
        Return Object.Equals(V, item.Value)
    End Function

    Private Function IDictionary_Contains(key As Object) As Boolean Implements IDictionary.Contains
        Return Me.ContainsKey(DirectCast(key, TKey))
    End Function
#End Region

    Private ReadOnly Property IDictionary_Keys As ICollection Implements IDictionary.Keys
        Get
            Return DirectCast(Me.Keys, ICollection)
        End Get
    End Property

    Private ReadOnly Property IDictionary_Values As ICollection Implements IDictionary.Values
        Get
            Return DirectCast(Me.Values, ICollection)
        End Get
    End Property

    Public Sub CopyTo(array() As KeyValuePair(Of TKey, TValue), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).CopyTo
        Verify.True(arrayIndex + Me.Count <= array.Length, "Array is not large enough.")
        For Each I In Me
            array(arrayIndex) = I
            arrayIndex += 1
        Next
    End Sub

    Private Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
        Verify.True(index + Me.Count <= array.GetLength(0), "Array is not large enough.")
        For Each I In Me
            array.SetValue(I, index)
            index += 1
        Next
    End Sub

    Private Function IDictionary_GetEnumerator() As IDictionaryEnumerator Implements IDictionary.GetEnumerator
        Return New DictionaryEnumerator(Of TKey, TValue, IEnumerator(Of KeyValuePair(Of TKey, TValue)))(Me.GetEnumerator())
    End Function
#End Region

    Public ReadOnly Property Count As Integer Implements ICollection.Count, IDictionary(Of TKey, TValue).Count
        Get
            Return Me.BaseDictionary.Count
        End Get
    End Property

    Default Public ReadOnly Property Item(key As TKey) As TValue
        Get
            Return Me.BaseDictionary.Item(key)
        End Get
    End Property

    Private Property IDictionary_Item(key As TKey) As TValue Implements IDictionary(Of TKey, TValue).Item
        Get
            Return Me.BaseDictionary.Item(key)
        End Get
        Set(value As TValue)
            Throw New NotSupportedException()
        End Set
    End Property

    Public ReadOnly Property Keys As ICollection(Of TKey) Implements IDictionary(Of TKey, TValue).Keys
        Get
            Return Me.BaseDictionary.Keys
        End Get
    End Property

    Public ReadOnly Property Values As ICollection(Of TValue) Implements IDictionary(Of TKey, TValue).Values
        Get
            Return Me.BaseDictionary.Values
        End Get
    End Property

    ' ToDo Keys and Values may have not implemented ICollection and cause problems when using IDictionary.

    Private Sub IDictionary_Add(key As TKey, value As TValue) Implements IDictionary(Of TKey, TValue).Add
        Throw New NotSupportedException()
    End Sub

    Public Sub Add(ByVal Value As TValue)
        Me.BaseDictionary.Add(Me.KeySelector.Invoke(Value), Value)
    End Sub

    ''' <summary>
    ''' Sets or adds the provided value.
    ''' </summary>
    Public Overridable Sub [Set](ByVal Value As TValue)
        Me.BaseDictionary.Item(Me.KeySelector.Invoke(Value)) = Value
    End Sub

    Public Sub Clear() Implements IDictionary.Clear, IDictionary(Of TKey, TValue).Clear
        Me.BaseDictionary.Clear()
    End Sub

    Public Function ContainsKey(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).ContainsKey
        Return Me.BaseDictionary.ContainsKey(key)
    End Function

    Public Function RemoveKey(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).Remove
        Return Me.BaseDictionary.Remove(key)
    End Function

    Public Function RemoveValue(ByVal Value As TValue) As Boolean
        Return Me.BaseDictionary.Remove(Me.KeySelector.Invoke(Value))
    End Function

    Public Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean Implements IDictionary(Of TKey, TValue).TryGetValue
        Return Me.BaseDictionary.TryGetValue(key, value)
    End Function

    Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IEnumerable(Of KeyValuePair(Of TKey, TValue)).GetEnumerator
        Return Me.BaseDictionary.GetEnumerator()
    End Function

    Private ReadOnly BaseDictionary As IDictionary(Of TKey, TValue)
    Private ReadOnly KeySelector As Func(Of TValue, TKey)

End Class
