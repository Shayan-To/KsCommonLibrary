Imports System.Collections.Specialized
Imports Ks.Common

Public Class ConcurrentOrderedDictionary(Of TKey, TValue)
    Inherits BaseDictionary(Of TKey, TValue)
    Implements IOrderedDictionary(Of TKey, TValue)

    Public Sub New()
        Me.New(New OrderedDictionary(Of TKey, TValue)())
    End Sub

    Public Sub New(ByVal BaseDictionary As OrderedDictionary(Of TKey, TValue))
        Me.New(BaseDictionary, New Object())
    End Sub

    Public Sub New(ByVal BaseDictionary As OrderedDictionary(Of TKey, TValue), ByVal LockObject As Object)
        Me.BaseDic = BaseDictionary
        Me.LockObject = LockObject
    End Sub

    Public Overrides ReadOnly Property Count As Integer
        Get
            SyncLock Me.LockObject
                Return Me.BaseDic.Count
            End SyncLock
        End Get
    End Property

    Default Public Overrides Property Item(key As TKey) As TValue
        Get
            SyncLock Me.LockObject
                Return Me.BaseDic.Item(key)
            End SyncLock
        End Get
        Set(value As TValue)
            SyncLock Me.LockObject
                Me.BaseDic.Item(key) = value
            End SyncLock
        End Set
    End Property

    Public Overrides ReadOnly Property Keys As ICollection(Of TKey)
        Get
            Return DirectCast(Me.KeysList, ICollection(Of TKey))
        End Get
    End Property

    Public Overrides ReadOnly Property Values As ICollection(Of TValue)
        Get
            Return DirectCast(Me.ValuesList, ICollection(Of TValue))
        End Get
    End Property

    Public ReadOnly Property KeysList As IReadOnlyList(Of TKey)
        Get
            SyncLock Me.LockObject
                Static R As IReadOnlyList(Of TKey) = New ConcurrentList(Of TKey)(DirectCast(Me.BaseDic.KeysList, IList(Of TKey)), Me.LockObject)
                Return R
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property ValuesList As IReadOnlyList(Of TValue)
        Get
            SyncLock Me.LockObject
                Static R As IReadOnlyList(Of TValue) = Me.KeysList.SelectAsList(Function(K) Me.BaseDic.Item(K))
                Return R
            End SyncLock
        End Get
    End Property

    Public Property ItemAt(index As Integer) As KeyValuePair(Of TKey, TValue) Implements IList(Of KeyValuePair(Of TKey, TValue)).Item
        Get
            SyncLock Me.LockObject
                Return Me.BaseDic.ItemAt(index)
            End SyncLock
        End Get
        Set(value As KeyValuePair(Of TKey, TValue))
            SyncLock Me.LockObject
                Me.BaseDic.ItemAt(index) = value
            End SyncLock
        End Set
    End Property

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

    Public Overrides Sub Add(key As TKey, value As TValue)
        SyncLock Me.LockObject
            Me.BaseDic.Add(key, value)
        End SyncLock
    End Sub

    Public Overrides Sub Clear() Implements IList.Clear, IList(Of KeyValuePair(Of TKey, TValue)).Clear
        SyncLock Me.LockObject
            Me.BaseDic.Clear()
        End SyncLock
    End Sub

    Public Sub Insert(index As Integer, key As TKey, value As TValue) Implements IOrderedDictionary(Of TKey, TValue).Insert
        SyncLock Me.LockObject
            Me.BaseDic.Insert(index, key, value)
        End SyncLock
    End Sub

    Public Sub RemoveAt(index As Integer) Implements IOrderedDictionary.RemoveAt, IList.RemoveAt, IList(Of KeyValuePair(Of TKey, TValue)).RemoveAt
        SyncLock Me.LockObject
            Me.BaseDic.RemoveAt(index)
        End SyncLock
    End Sub

    Public Overrides Function ContainsKey(key As TKey) As Boolean
        SyncLock Me.LockObject
            Return Me.BaseDic.ContainsKey(key)
        End SyncLock
    End Function

    Public Function IndexOf(key As TKey) As Integer
        SyncLock Me.LockObject
            Return Me.BaseDic.IndexOf(key)
        End SyncLock
    End Function

    Public Overrides Function Remove(key As TKey) As Boolean
        SyncLock Me.LockObject
            Return Me.BaseDic.Remove(key)
        End SyncLock
    End Function

    Public Overrides Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean
        SyncLock Me.LockObject
            Return Me.BaseDic.TryGetValue(key, value)
        End SyncLock
    End Function

    Public Iterator Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
        SyncLock Me.LockObject
            For Each KV In Me.BaseDic
                Yield KV
            Next
        End SyncLock
    End Function

    Protected Overrides Function IEnumerator_1_GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
        Return Me.GetEnumerator()
    End Function

#Region "Junk"
    Private Property IList_ItemAt(index As Integer) As Object Implements IList.Item, IOrderedDictionary.Item
        Get
            Return Me.ItemAt(index)
        End Get
        Set(value As Object)
            Me.ItemAt(index) = DirectCast(value, KeyValuePair(Of TKey, TValue))
        End Set
    End Property

    Private Function IList_Add(value As Object) As Integer Implements IList.Add
        SyncLock Me.LockObject
            Me.ICollection_Add(DirectCast(value, KeyValuePair(Of TKey, TValue)))
            Return Me.Count - 1
        End SyncLock
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

    Private Function IList_IndexOf(item As KeyValuePair(Of TKey, TValue)) As Integer Implements IList(Of KeyValuePair(Of TKey, TValue)).IndexOf
        Dim R As Integer
        Dim T As TValue

        SyncLock Me.LockObject
            R = Me.IndexOf(item.Key)
            T = Me.ItemAt(R).Value
        End SyncLock

        If R = -1 Then
            Return -1
        End If
        If Not Object.Equals(item.Value, T) Then
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
#End Region

    Private ReadOnly BaseDic As OrderedDictionary(Of TKey, TValue)
    Private ReadOnly LockObject As Object

End Class
