Public Class ConcurrentList(Of T)
    Inherits BaseList(Of T)

    Public Sub New()
        Me.New(New List(Of T)())
    End Sub

    Public Sub New(ByVal BaseList As IList(Of T))
        Me.New(BaseList, New Object())
    End Sub

    Public Sub New(ByVal BaseList As IList(Of T), ByVal LockObject As Object)
        Me.BaseList = BaseList
        Me.LockObject = LockObject
    End Sub

    Public Overrides ReadOnly Property Count As Integer
        Get
            SyncLock Me.LockObject
                Return Me.BaseList.Count
            End SyncLock
        End Get
    End Property

    Default Public Overrides Property Item(index As Integer) As T
        Get
            SyncLock Me.LockObject
                Return Me.BaseList.Item(index)
            End SyncLock
        End Get
        Set(value As T)
            SyncLock Me.LockObject
                Me.BaseList.Item(index) = value
            End SyncLock
        End Set
    End Property

    Public Overrides Sub Clear()
        SyncLock Me.LockObject
            Me.BaseList.Clear()
        End SyncLock
    End Sub

    Public Overrides Sub Insert(index As Integer, item As T)
        SyncLock Me.LockObject
            Me.BaseList.Insert(index, item)
        End SyncLock
    End Sub

    Public Overrides Sub RemoveAt(index As Integer)
        SyncLock Me.LockObject
            Me.BaseList.RemoveAt(index)
        End SyncLock
    End Sub

    Protected Overrides Function IEnumerable_1_GetEnumerator() As IEnumerator(Of T)
        Return Me.GetEnumerator()
    End Function

    Public Iterator Function GetEnumerator() As IEnumerator(Of T)
        SyncLock Me.LockObject
            For Each I In Me.BaseList
                Yield I
            Next
        End SyncLock
    End Function

    Private ReadOnly BaseList As IList(Of T)
    Private ReadOnly LockObject As Object

End Class
