Namespace Common

    ' ToDo Change this to use StopWatch.

    Public Class TaskDelayer
        Implements IDisposable

        ''' <param name="Task">The task that should be invoked. This call will be taken place on some background thread.</param>
        Public Sub New(ByVal Task As Action, ByVal MinDelay As TimeSpan, Optional ByVal MaxDelay As TimeSpan = Nothing, Optional ByVal InactivityTime As TimeSpan = Nothing)
            Me._Task = Task
            Me._MinDelay = MinDelay
            Me._MaxDelay = If(MaxDelay = TimeSpan.Zero, TimeSpan.MaxValue, MaxDelay)
            Me._InactivityTime = InactivityTime

            Me.TaskThread = New Threading.Thread(AddressOf Me.TaskThreadProcedure) With {.IsBackground = True}
            Me.TaskThread.Start()
        End Sub

        Public Sub RunTask(ByVal RunningMode As TaskDelayerRunningMode)
            ' First Check that IsTaskPending is REALLY true. We have to lock to make that sure.
            If Me.IsTaskPending Then
                SyncLock Me.WaitLockObject
                    If Me.IsTaskPending Then
                        If RunningMode = TaskDelayerRunningMode.Instant Then
                            Me.IsInstantSet = True
                            Me.DelayWaitHandle.Set()
                        Else
                            ' As setting LastActivityTime can at most delay the run of a task, it is only important to have it at the time of deciding whether to run the task.
                            Dim Now = DateTime.UtcNow
                            If Me.LastActivityTime > Now Then
                                ' This usually means that the system's time has changed.
                                ' The simplest thing to do is to set instant.
                                Me.IsInstantSet = True
                                Me.DelayWaitHandle.Set()
                            Else
                                Me.LastActivityTime = Now
                            End If
                        End If
                        Exit Sub
                    End If
                End SyncLock
            End If

            ' If IsTaskPending is REALLY false, then the other thread is for sure locked at TaskWaitHandle.
            ' So we safely can set it to true.
            Me.IsTaskPending = True
            Me.FirstActivityTime = DateTime.UtcNow
            Me.LastActivityTime = Me.FirstActivityTime

            If RunningMode = TaskDelayerRunningMode.Instant Then
                Me.IsInstantSet = True
                ' We set the wait handle before waiting on it. The wait will then immediately return.
                Me.DelayWaitHandle.Set()
            End If
            Me.TaskWaitHandle.Set()
        End Sub

        Private Sub TaskThreadProcedure()
            ' ToDo Prove that we need two wait handles. (Have done it once.)
            Do
                Me.TaskWaitHandle.WaitOne()

                If Not Me.IsTaskPending Then
                    Assert.True(Me.IsDisposed)
                    Exit Do
                End If

                Do
                    Dim WaitTime = TimeSpan.Zero
                    Dim ShouldRunTask = False

                    ' Here we will decide whether to run the task or not.
                    ' So as LastActivityTime can set that to a later time, we have to take the last LastActivityTime into account.
                    ' Every call to RunTask has to have an effect, either to start a pending task or to set an activity time (or set instant).
                    ' So if RunTask has determined that IsTaskPending is true, it must set its activity time as it cannot set out a new pending task.
                    SyncLock Me.WaitLockObject
                        ShouldRunTask = Me.IsInstantSet

                        Dim Now = DateTime.UtcNow
                        Do
                            If ShouldRunTask Then
                                Exit Do
                            End If

                            ' These " - Now"s were at the end. To avoid overflows, moved them a step back.

                            WaitTime = Me.FirstActivityTime - Now + Me.MinDelay
                            If WaitTime > TimeEpsilon Then
                                Exit Do
                            End If

                            Dim MaxWaitTime = Me.FirstActivityTime - Now + Me.MaxDelay
                            WaitTime = Me.LastActivityTime - Now + Me.InactivityTime

                            If WaitTime <= TimeEpsilon Or MaxWaitTime <= TimeEpsilon Then
                                ShouldRunTask = True
                                Exit Do
                            End If

                            If WaitTime > MaxWaitTime Then
                                ' We know that next time the task must surely be done, so we set instant to avoid next-time unnecessary calculation.
                                Me.IsInstantSet = True
                                WaitTime = MaxWaitTime
                            End If

                            Exit Do
                        Loop

                        If ShouldRunTask Then
                            ' ToDo Isn't DelayWaitHandle always reset at this point?
                            Me.DelayWaitHandle.Reset()
                            Me.IsTaskPending = False
                            Me.IsInstantSet = False
                        End If
                    End SyncLock

                    If ShouldRunTask Then
                        Me.Task.Invoke()
                        Exit Sub
                    Else
                        Me.DelayWaitHandle.WaitOne(WaitTime)
                    End If
                Loop
            Loop
        End Sub

        Protected Overridable Sub Dispose(ByVal Disposing As Boolean)
            If Not Me.IsDisposed Then
                Me._IsDisposed = True

                If Disposing Then
                    ' Dispose managed state (managed objects).
                End If

                ' Free unmanaged resources (unmanaged objects) and override Finalize() below.

                ' The other thread is necessarily waiting in one of the wait handles (or soon will be).

                ' ... If it is waiting in DelayWaitHandle, a task is pending and must be done, so we set instant.
                If Me.IsTaskPending Then
                    ' We have to ensure that when a task is set to be done, it is always necessarily done.
                    Me.IsInstantSet = True
                    Me.DelayWaitHandle.Set()
                End If

                ' ... Otherwise, we set the TaskWaitHandle without setting IsTaskPending, so the thread understands that object is being disposed.
                Me.TaskWaitHandle.Set()
                ' ... And the important part, joining the thread so that everything is done before Dispose method returns.
                Me.TaskThread.Join()

                Me.TaskWaitHandle.Dispose()
                Me.DelayWaitHandle.Dispose()

                ' Set large fields to null.
            End If
        End Sub

#Region "IDisposable Support"
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

#Region "Task Property"
        Private ReadOnly _Task As Action

        Public ReadOnly Property Task As Action
            Get
                Return Me._Task
            End Get
        End Property
#End Region

#Region "MinDelay Property"
        Private ReadOnly _MinDelay As TimeSpan

        Public ReadOnly Property MinDelay As TimeSpan
            Get
                Return Me._MinDelay
            End Get
        End Property
#End Region

#Region "MaxDelay Property"
        Private ReadOnly _MaxDelay As TimeSpan

        Public ReadOnly Property MaxDelay As TimeSpan
            Get
                Return Me._MaxDelay
            End Get
        End Property
#End Region

#Region "InactivityTime Property"
        Private ReadOnly _InactivityTime As TimeSpan

        Public ReadOnly Property InactivityTime As TimeSpan
            Get
                Return Me._InactivityTime
            End Get
        End Property
#End Region

        Private IsTaskPending As Boolean = False
        Private IsInstantSet As Boolean = False
        Private FirstActivityTime As DateTime
        Private LastActivityTime As DateTime

        Private Shared ReadOnly TimeEpsilon As TimeSpan = TimeSpan.FromMilliseconds(30)

        Private ReadOnly WaitLockObject As Object = New Object()
        Private ReadOnly TaskWaitHandle As Threading.EventWaitHandle = New Threading.EventWaitHandle(False, Threading.EventResetMode.AutoReset)
        Private ReadOnly DelayWaitHandle As Threading.EventWaitHandle = New Threading.EventWaitHandle(False, Threading.EventResetMode.AutoReset)
        Private ReadOnly TaskThread As Threading.Thread

    End Class

    Public Enum TaskDelayerRunningMode

        Instant
        Delayed

    End Enum

End Namespace
