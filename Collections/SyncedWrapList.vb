''' <summary>
''' Beware that GetEnumerator is not synchronized in this collection.
''' </summary>
Public Class SyncedWrapList(Of T)
    Inherits BaseList(Of T)

    Public Sub New()
        Me.New(New List(Of T)(), New Object())
    End Sub

    Public Sub New(ByVal BaseList As IList(Of T))
        Me.New(BaseList, New Object())
    End Sub

    Public Sub New(ByVal BaseList As IList(Of T), ByVal LockObject As Object)
        Me._BaseList = BaseList
        Me._LockObject = LockObject
    End Sub

    Public Overrides ReadOnly Property Count As Integer
        Get
            SyncLock Me._LockObject
                Return Me._BaseList.Count
            End SyncLock
        End Get
    End Property

    Default Public Overrides Property Item(index As Integer) As T
        Get
            SyncLock Me._LockObject
                Return Me._BaseList.Item(index)
            End SyncLock
        End Get
        Set(value As T)
            SyncLock Me._LockObject
                Me._BaseList.Item(index) = value
            End SyncLock
        End Set
    End Property

    Public Overrides Sub Clear()
        SyncLock Me._LockObject
            Me._BaseList.Clear()
        End SyncLock
    End Sub

    Public Overrides Sub Insert(index As Integer, item As T)
        SyncLock Me._LockObject
            Me._BaseList.Insert(index, item)
        End SyncLock
    End Sub

    Public Overrides Sub RemoveAt(index As Integer)
        SyncLock Me._LockObject
            Me._BaseList.RemoveAt(index)
        End SyncLock
    End Sub

    Protected Overrides Function IEnumerable_1_GetEnumerator() As IEnumerator(Of T)
        Return Me._BaseList.GetEnumerator()
    End Function

    Public Function GetEnumerator() As IEnumerator(Of T)
        Return Me._BaseList.GetEnumerator()
    End Function

#Region "LockObject Property"
    Private ReadOnly _LockObject As Object

    Public ReadOnly Property LockObject As Object
        Get
            Return Me._LockObject
        End Get
    End Property
#End Region

#Region "BaseList Property"
    Private ReadOnly _BaseList As IList(Of T)

    Public ReadOnly Property BaseList As IList(Of T)
        Get
            Return Me._BaseList
        End Get
    End Property
#End Region

End Class
