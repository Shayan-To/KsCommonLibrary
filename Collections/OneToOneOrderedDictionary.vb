Imports System.Collections.Specialized

Public Class OneToOneOrderedDictionary(Of TKey, TValue)
    Implements IOrderedDictionary(Of TKey, TValue)

    ' ToDo Get an equality comparer and use it on the dic, and also the list operations.

    Public Sub New(ByVal KeySelector As Func(Of TValue, TKey))
        Me.KeySelector = KeySelector
    End Sub

    Public Overridable ReadOnly Property Values As ICollection(Of TValue) Implements IDictionary(Of TKey, TValue).Values
        Get
            Return Me._Values
        End Get
    End Property

    Public Overridable Property ItemAt(index As Integer) As TValue
        Get
            Return Me._Items.Item(index)
        End Get
        Set(value As TValue)
            Dim PKey = Me._Keys.Item(index)
            Assert.True(Me._Dic.Remove(PKey))

            Me._Dic.Add(Me.KeySelector.Invoke(value), value)
            Me._Items.Item(index) = value
        End Set
    End Property

    ''' <summary>
    ''' Sets or adds the provided value. The value will be added to the end of the collection if not present.
    ''' </summary>
    ''' <returns>True if the collection was expanded, and false otherwise, when the key was already in the collection.</returns>
    Public Overridable Function [Set](ByVal Value As TValue) As Boolean
        Dim Key = Me.KeySelector.Invoke(Value)
        Dim PValue As TValue = Nothing

        If Me._Dic.TryGetValue(Key, PValue) Then
            Me._Items.Item(Me._Items.IndexOf(PValue)) = Value
            Me._Dic.Item(Key) = Value
            Return False
        End If

        Me._Items.Add(Value)
        Me._Dic.Add(Key, Value)
        Return True
    End Function

    Public Overridable Sub Clear() Implements IList.Clear, IDictionary.Clear, IDictionary(Of TKey, TValue).Clear
        Me._Items.Clear()
        Me._Dic.Clear()
    End Sub

    Public Overridable Sub Insert(ByVal Index As Integer, ByVal Value As TValue)
        Dim Key = Me.KeySelector.Invoke(Value)
        Me._Dic.Add(Key, Value)
        Me._Items.Insert(Index, Value)
    End Sub

    Public Overridable Sub RemoveAt(index As Integer) Implements IOrderedDictionary.RemoveAt, IList.RemoveAt, IList(Of KeyValuePair(Of TKey, TValue)).RemoveAt
        Dim Key = Me._Keys.Item(index)
        Assert.True(Me._Dic.Remove(Key))
        Me._Items.RemoveAt(index)
    End Sub

    Public Overridable Function RemoveKey(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).Remove
        Dim Value As TValue = Nothing
        If Not Me._Dic.TryGetValue(key, Value) Then
            Return False
        End If

        Assert.True(Me._Dic.Remove(key))
        Assert.True(Me._Items.Remove(Value))
        Return True
    End Function

    Public Overridable Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IDictionary(Of TKey, TValue).GetEnumerator
        Return Me._Items.Select(Function(V) New KeyValuePair(Of TKey, TValue)(Me.KeySelector.Invoke(V), V)).GetEnumerator()
    End Function

#Region "Obvious"
    Default Public Overridable ReadOnly Property Item(key As TKey) As TValue
        Get
            Return Me._Dic.Item(key)
        End Get
    End Property

    Private Property IDictionary_Item(key As TKey) As TValue Implements IDictionary(Of TKey, TValue).Item
        Get
            Return Me.Item(key)
        End Get
        Set(value As TValue)
            Throw New NotSupportedException()
        End Set
    End Property

    Public Overridable ReadOnly Property Count As Integer Implements IList(Of KeyValuePair(Of TKey, TValue)).Count, ICollection.Count
        Get
            Return Me._Items.Count
        End Get
    End Property

    Public Overridable ReadOnly Property Keys As ICollection(Of TKey) Implements IDictionary(Of TKey, TValue).Keys
        Get
            Return Me._Keys
        End Get
    End Property

    Private Sub IDictionary_Add(key As TKey, value As TValue) Implements IDictionary(Of TKey, TValue).Add
        Throw New NotSupportedException()
    End Sub

    Public Sub Insert(index As Integer, key As TKey, value As TValue) Implements IOrderedDictionary(Of TKey, TValue).Insert
        Throw New NotSupportedException()
    End Sub

    Public Overridable Function ContainsKey(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).ContainsKey
        Return Me._Dic.ContainsKey(key)
    End Function

    Public Overridable Function IndexOf(key As TKey) As Integer
        Return Me._Items.IndexOf(Me._Dic.Item(key))
    End Function

    Public Overridable Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean Implements IDictionary(Of TKey, TValue).TryGetValue
        Return Me._Dic.TryGetValue(key, value)
    End Function
#End Region

#Region "Other-Calling"
    Public Sub Add(ByVal Value As TValue)
        Me.Insert(Me.Count, Value)
    End Sub

    Private ReadOnly Property IList_IsReadOnly As Boolean Implements IList.IsReadOnly
        Get
            Return False
        End Get
    End Property

    Private ReadOnly Property IList_IsFixedSize As Boolean Implements IList.IsFixedSize
        Get
            Return False
        End Get
    End Property

    Private Property IList_ItemAt(index As Integer) As Object Implements IList.Item, IOrderedDictionary.Item
        Get
            Return Me.IList_1_ItemAt(index)
        End Get
        Set(value As Object)
            Me.IList_1_ItemAt(index) = DirectCast(value, KeyValuePair(Of TKey, TValue))
        End Set
    End Property

    Private Property IList_1_ItemAt(index As Integer) As KeyValuePair(Of TKey, TValue) Implements IList(Of KeyValuePair(Of TKey, TValue)).Item
        Get
            Dim Value = Me.ItemAt(index)
            Return New KeyValuePair(Of TKey, TValue)(Me.KeySelector.Invoke(Value), Value)
        End Get
        Set(value As KeyValuePair(Of TKey, TValue))
            Throw New NotSupportedException()
        End Set
    End Property

    Private Function IList_Add(value As Object) As Integer Implements IList.Add
        Me.ICollection_Add(DirectCast(value, KeyValuePair(Of TKey, TValue)))
        Return Me.Count - 1
    End Function

    Private Sub IList_Insert(index As Integer, item As KeyValuePair(Of TKey, TValue)) Implements IList(Of KeyValuePair(Of TKey, TValue)).Insert
        Me.Insert(index, item.Key, item.Value)
    End Sub

    Private Sub IList_Insert(index As Integer, value As Object) Implements IList.Insert
        Me.IList_Insert(index, DirectCast(value, KeyValuePair(Of TKey, TValue)))
    End Sub

    Private Sub IOrderedDictionary_Insert(index As Integer, key As Object, value As Object) Implements IOrderedDictionary.Insert
        Me.Insert(index, DirectCast(key, TKey), DirectCast(value, TValue))
    End Sub

    Private Sub IList_Remove(value As Object) Implements IList.Remove
        Me.ICollection_Remove(DirectCast(value, KeyValuePair(Of TKey, TValue)))
    End Sub

    Public Function RemoveValue(ByVal Value As TValue) As Boolean
        Return Me.RemoveKey(Me.KeySelector.Invoke(Value))
    End Function

    Private Function IList_IndexOf(item As KeyValuePair(Of TKey, TValue)) As Integer Implements IList(Of KeyValuePair(Of TKey, TValue)).IndexOf
        Dim R = Me.IndexOf(item.Key)
        If R = -1 Then
            Return -1
        End If
        If Not Object.Equals(item.Value, Me.ItemAt(R)) Then
            Return -1
        End If
        Return R
    End Function

    Private Function IList_IndexOf(value As Object) As Integer Implements IList.IndexOf
        Return Me.IList_IndexOf(DirectCast(value, KeyValuePair(Of TKey, TValue)))
    End Function

    Private Function IOrderedDictionary_GetEnumerator() As IDictionaryEnumerator Implements IOrderedDictionary.GetEnumerator
        Return Me.IDictionary_GetEnumerator()
    End Function

    Private Function IList_Contains(value As Object) As Boolean Implements IList.Contains
        Return Me.ICollection_Contains(DirectCast(value, KeyValuePair(Of TKey, TValue)))
    End Function

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
#End Region

    Private ReadOnly _Dic As Dictionary(Of TKey, TValue) = New Dictionary(Of TKey, TValue)()
    Private ReadOnly _Items As List(Of TValue) = New List(Of TValue)()
    Private ReadOnly _Keys As IList(Of TKey) = Me._Items.SelectAsList(Function(V) Me.KeySelector.Invoke(V))
    Private ReadOnly _Values As IList(Of TValue) = Me._Items.AsReadOnly()
    Private ReadOnly KeySelector As Func(Of TValue, TKey)

End Class
