Public Structure MultiDictionaryList(Of TKey, TValue)
    Implements IList(Of TValue),
               IList

    Public Sub New(ByVal Parent As MultiDictionary(Of TKey, TValue), ByVal Key As TKey, ByVal List As List(Of TValue))
        Me._Parent = Parent
        Me._Key = Key
        Me.List = List
        Me.Version = Me.Parent.Version
    End Sub

    Private Sub CheckChanges()
        If Me.Version <> Me.Parent.Version Then
            Dim T As List(Of TValue) = Nothing
            If Me.Parent.Dic.TryGetValue(Me.Key, T) Then
                Me.List = T
            Else
                Me.List = Nothing
            End If
            Me.Version = Me.Parent.Version
        End If
    End Sub

    Private Sub BeforeInp()
        Me.CheckChanges()
        If Me.List Is Nothing Then
            Me.List = New List(Of TValue)()
            Me.Parent.ReportKeyFilled(Me)
        End If
    End Sub

    Private Sub AfterOut()
        Me.CheckChanges()
        If Me.List.Count = 0 Then
            Me.List = Nothing
            Me.Parent.ReportKeyEmpty(Me)
        End If
    End Sub

    Public Sub Add(item As TValue) Implements ICollection(Of TValue).Add
        Me.CheckChanges()
        Me.BeforeInp()
        Me.List.Add(item)
    End Sub

    Public Sub Insert(index As Integer, item As TValue) Implements IList(Of TValue).Insert
        Me.CheckChanges()
        Me.BeforeInp()
        Me.List.Insert(index, item)
    End Sub

    Public Sub Clear() Implements ICollection(Of TValue).Clear, IList.Clear
        Me.CheckChanges()
        If Me.List IsNot Nothing Then
            Me.List.Clear()
            Me.AfterOut()
        End If
    End Sub

    Public Sub RemoveAt(index As Integer) Implements IList(Of TValue).RemoveAt, IList.RemoveAt
        Me.CheckChanges()
        If Me.List Is Nothing Then
            Throw New ArgumentOutOfRangeException("index")
        End If
        Me.List.RemoveAt(index)
        Me.AfterOut()
    End Sub

    Public Function Remove(item As TValue) As Boolean Implements ICollection(Of TValue).Remove
        Me.CheckChanges()
        If Me.List Is Nothing Then
            Return False
        End If
        Dim R = Me.List.Remove(item)
        Me.AfterOut()
        Return R
    End Function

    Public Function Contains(item As TValue) As Boolean Implements ICollection(Of TValue).Contains
        Me.CheckChanges()
        If Me.List Is Nothing Then
            Return False
        End If
        Return Me.List.Contains(item)
    End Function

    Public Function IndexOf(item As TValue) As Integer Implements IList(Of TValue).IndexOf
        Me.CheckChanges()
        If Me.List Is Nothing Then
            Return -1
        End If
        Return If(Me.List, EmptyList).IndexOf(item)
    End Function

    Public Sub CopyTo(array() As TValue, arrayIndex As Integer) Implements ICollection(Of TValue).CopyTo
        Me.CheckChanges()
        If Me.List IsNot Nothing Then
            Me.List.CopyTo(array, arrayIndex)
        End If
    End Sub

    Private Sub IList_CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
        Me.CheckChanges()
        If Me.List IsNot Nothing Then
            DirectCast(Me.List, IList).CopyTo(array, index)
        End If
    End Sub

    Public Function GetEnumerator() As List(Of TValue).Enumerator
        Me.CheckChanges()
        Return If(Me.List, EmptyList).GetEnumerator()
    End Function

    Default Public Property Item(index As Integer) As TValue Implements IList(Of TValue).Item
        Get
            Me.CheckChanges()
            If Me.List Is Nothing Then
                Throw New ArgumentOutOfRangeException("index")
            End If
            Return Me.List.Item(index)
        End Get
        Set(value As TValue)
            Me.CheckChanges()
            If Me.List Is Nothing Then
                Throw New ArgumentOutOfRangeException("index")
            End If
            Me.List.Item(index) = value
        End Set
    End Property

    Public ReadOnly Property Count As Integer Implements ICollection(Of TValue).Count, ICollection.Count
        Get
            Me.CheckChanges()
            If Me.List Is Nothing Then
                Return 0
            End If
            Return Me.List.Count
        End Get
    End Property


#Region "Obvious Implementations"
    ' The following methods call the ones from above.

    Private Function IList_Contains(value As Object) As Boolean Implements IList.Contains
        Return Me.Contains(DirectCast(value, TValue))
    End Function

    Private Function IList_IndexOf(value As Object) As Integer Implements IList.IndexOf
        Return Me.IndexOf(DirectCast(value, TValue))
    End Function

    Private Function IEnumerable_1_GetEnumerator() As IEnumerator(Of TValue) Implements IEnumerable(Of TValue).GetEnumerator
        Return Me.GetEnumerator()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function

    Private Function IList_Add(value As Object) As Integer Implements IList.Add
        Me.Add(DirectCast(value, TValue))
        Return Me.Count - 1
    End Function

    Private Sub IList_Insert(index As Integer, value As Object) Implements IList.Insert
        Me.Insert(index, DirectCast(value, TValue))
    End Sub

    Private Sub IList_Remove(value As Object) Implements IList.Remove
        Me.Remove(DirectCast(value, TValue))
    End Sub

    Private Property IList_Item(index As Integer) As Object Implements IList.Item
        Get
            Return Me.Item(index)
        End Get
        Set(value As Object)
            Me.Item(index) = DirectCast(value, TValue)
        End Set
    End Property

    Private ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of TValue).IsReadOnly, IList.IsReadOnly
        Get
            Return False
        End Get
    End Property

    Private ReadOnly Property IsFixedSize As Boolean Implements IList.IsFixedSize
        Get
            Return False
        End Get
    End Property

    Private ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
        Get
            Throw New NotSupportedException()
        End Get
    End Property

    Private ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Parent Property"
    Private ReadOnly _Parent As MultiDictionary(Of TKey, TValue)

    Public ReadOnly Property Parent As MultiDictionary(Of TKey, TValue)
        Get
            Return Me._Parent
        End Get
    End Property
#End Region

#Region "Key Property"
    Private ReadOnly _Key As TKey

    Public ReadOnly Property Key As TKey
        Get
            Return Me._Key
        End Get
    End Property
#End Region

    Private Version As Byte
    Friend List As List(Of TValue)
    Private Shared ReadOnly EmptyList As List(Of TValue) = New List(Of TValue)()

End Structure
