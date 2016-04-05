Public Class TaskDelayer
    Implements IDisposable

    ''' <param name="Task">The task that should be invoked. This call will be taken place on some background thread.</param>
    Public Sub New(ByVal Task As Action, ByVal Delay As Integer)
        Me.Task = Task
        Me.Delay = Delay

        Me.TaskThread = New Threading.Thread(AddressOf Me.TaskThreadProcedure) With {.IsBackground = True}
        Me.TaskThread.Start()
    End Sub

    Public Sub RunTask(ByVal RunningMode As TaskDelayerRunningMode)
        If Me.IsTaskPending Then
            SyncLock Me.WaitLockObject
                If Me.IsTaskPending Then
                    If RunningMode = TaskDelayerRunningMode.Instant Then
                        Me.DelayWaitHandle.Set()
                    End If
                    Exit Sub
                End If
            End SyncLock
        End If
        Me.IsTaskPending = True

        If RunningMode = TaskDelayerRunningMode.Instant Then
            Me.DelayWaitHandle.Set()
        End If
        Me.TaskWaitHandle.Set()
    End Sub

    Private Sub TaskThreadProcedure()
        Do
            Me.TaskWaitHandle.WaitOne()

            If Not Me.IsTaskPending Then
                Assert.True(Me._IsDisposed)
                Exit Do
            End If

            Me.DelayWaitHandle.WaitOne(Delay)

            SyncLock Me.WaitLockObject
                Me.DelayWaitHandle.Reset()
                Me.IsTaskPending = False
            End SyncLock

            Task.Invoke()
        Loop
    End Sub

#Region "IDisposable Support"
    Protected Overridable Sub Dispose(ByVal Disposing As Boolean)
        If Not Me._IsDisposed Then
            Me._IsDisposed = True

            If Disposing Then
                ' Dispose managed state (managed objects).
            End If

            ' Free unmanaged resources (unmanaged objects) and override Finalize() below.
            If Me.IsTaskPending Then
                Me.DelayWaitHandle.Set()
            End If

            Me.TaskWaitHandle.Set()
            Me.TaskThread.Join()

            Me.TaskWaitHandle.Dispose()
            Me.DelayWaitHandle.Dispose()

            ' Set large fields to null.
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Me.Dispose(False)
        MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Me.Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#Region "IsDisposed Property"
    Private _IsDisposed As Boolean

    Public ReadOnly Property IsDisposed As Boolean
        Get
            Return Me._IsDisposed
        End Get
    End Property
#End Region

    Private ReadOnly Task As Action
    Private ReadOnly Delay As Integer

    Private IsTaskPending As Boolean = False

    Private ReadOnly WaitLockObject As Object = New Object()
    Private ReadOnly TaskWaitHandle As Threading.EventWaitHandle = New Threading.EventWaitHandle(False, Threading.EventResetMode.AutoReset)
    Private ReadOnly DelayWaitHandle As Threading.EventWaitHandle = New Threading.EventWaitHandle(False, Threading.EventResetMode.AutoReset)
    Private ReadOnly TaskThread As Threading.Thread

End Class

Public Enum TaskDelayerRunningMode

    Instant
    Delayed

End Enum

'Public Class TaskDelayer(Of TParams)
'    Inherits TaskDelayer

'    Public Sub New(ByVal Task As Action(Of TParams), ByVal Delay As Integer)
'        MyBase.New(AddressOf Me.TaskProc, Delay)
'        Me.Task = Task
'    End Sub

'    Private Sub TaskProc()
'        Me.Task.Invoke(Me.Params)
'    End Sub

'    Private Shadows Sub DoTask()

'    End Sub

'    Public Shadows Sub DoTask(ByVal Params As TParams)
'        Me.Params = Params

'        MyBase.DoTask()
'    End Sub

'    Private ReadOnly Task As Action(Of TParams)
'    Private Params As TParams

'End Class
