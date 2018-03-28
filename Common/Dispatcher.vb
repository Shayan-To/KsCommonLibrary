Imports System.Collections.Concurrent
Imports System.Threading

Namespace Common

    Public Class Dispatcher
        Implements IDisposable

        Private Sub RunAction(ByVal AI As (Action As Action, Id As Integer))
            Dim WaitingData As (WaitHandle As EventWaitHandle, Exception As Exception) = Nothing
            Me.WaitingActions.TryGetValue(AI.Id, WaitingData)

            Try
                AI.Action.Invoke()
            Catch Ex As Exception
                If WaitingData.WaitHandle Is Nothing Then
                    Me.OnExceptionUnhandled(New ExceptionUnhandledEventArgs(Ex))
                Else
                    WaitingData = (WaitingData.WaitHandle, Ex)
                End If
            End Try

            If WaitingData.WaitHandle IsNot Nothing Then
                Me.WaitingActions.Item(AI.Id) = WaitingData
                WaitingData.WaitHandle.Set()
            End If
        End Sub

        Public Sub Invoke(ByVal [Delegate] As Action)
            Verify.False(Me.IsShuttingDown, "Cannot add an action when the dispatcher is shutting down.")

            Dim Id = 0
            SyncLock Me.LockObject
                Me.CurrentId += 1
                Id = Me.CurrentId
            End SyncLock

            Me.Queue.Add(([Delegate], Id))
        End Sub

        Public Sub InvokeAndWait(ByVal [Delegate] As Action)
            Verify.False(Me.IsShuttingDown, "Cannot add an action when the dispatcher is shutting down.")
            Verify.True(Me.IsDispatching, "Cannot wait for invocation when the dispatcher is not running.")

            Dim Id = 0
            SyncLock Me.LockObject
                Me.CurrentId += 1
                Id = Me.CurrentId
            End SyncLock

            Dim WaitingData As (WaitHandle As EventWaitHandle, Exception As Exception)

            Using WaitHandle = New EventWaitHandle(False, EventResetMode.ManualReset)
                WaitingData = (WaitHandle, Nothing)
                Me.WaitingActions.Add(Id, WaitingData)

                Me.Queue.Add(([Delegate], Id))

                WaitHandle.WaitOne()
            End Using

            WaitingData = Me.WaitingActions.Item(Id)
            Assert.True(Me.WaitingActions.Remove(Id))

            If WaitingData.Exception IsNot Nothing Then
                Throw New InvocationException("Action invocation threw an exception. See InnerException for more details.", WaitingData.Exception)
            End If
        End Sub

        Public Sub SetSynchronizationContext()
            If Me.Thread Is Nothing Then
                Me._Thread = Thread.CurrentThread
            End If

            Verify.True(Me.IsSameThread(), $"You can only call `{NameOf(Me.SetSynchronizationContext)}` from the thread of the dispatcher.")

            SynchronizationContext.SetSynchronizationContext(New DispatcherSynchronizationContext(Me))
        End Sub

        Public Sub DoActions()
            If Me.Thread Is Nothing Then
                Me._Thread = Thread.CurrentThread
            End If

            Verify.True(Me.IsSameThread(),
                        If(Me.IsDispatching,
                           $"You can only call `{NameOf(Me.DoActions)}` from the same thread the dispatcher is running on.",
                           "The dispatcher cannot be run from two different threads."))

            Dim IsRunning = Me.IsRunning
            Me._IsRunning = True

            Dim List = New List(Of (Action As Action, Id As Integer))()
            Do
                Dim AI As (Action As Action, Id As Integer) = Nothing
                If Not Me.Queue.TryTake(AI) Then
                    Exit Do
                End If
                List.Add(AI)
            Loop

            For Each AI In List
                Me.RunAction(AI)
            Next

            Me._IsRunning = IsRunning
        End Sub

        Public Sub Run()
            Verify.False(Me.IsShutDown, "The dispatcher has already been shut down.")
            Verify.False(Me.IsRunning, $"The dispatcher is already running something on {If(Me.IsSameThread(), "the same", "another")} thread.")

            If Me.Thread IsNot Nothing Then
                Verify.True(Me.IsSameThread(), "The dispatcher cannot be run from two different threads.")
            End If

            Me._Thread = Thread.CurrentThread

            Me._IsDispatching = True
            Me._IsRunning = True

            For Each AI In Me.Queue.GetConsumingEnumerable()
                Me.RunAction(AI)

                If Me.IsShuttingDown Then
                    Me.DoActions()
                    Exit For
                End If
            Next

            Me._IsDispatching = False
            Me._IsRunning = False
            Me._IsShutDown = True

            Me.OnDispatcherShutDown()
        End Sub

        Public Sub ShutDown()
            Me._IsShuttingDown = True
        End Sub

        Private Function IsSameThread() As Boolean
            Return Me.Thread.ManagedThreadId = Thread.CurrentThread.ManagedThreadId
        End Function

#Region "IDisposable Support"
        Protected Overridable Sub Dispose(Disposing As Boolean)
            If Not Me._IsDisposed Then
                If Disposing Then
                    DirectCast(Me.WaitingActions, IDisposable).Dispose()
                    Me.Queue.Dispose()
                End If
            End If
            Me._IsDisposed = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
        End Sub
#End Region

#Region "ExceptionUnhandled Event"
        Public Event ExceptionUnhandled As EventHandler(Of ExceptionUnhandledEventArgs)

        Protected Overridable Sub OnExceptionUnhandled(ByVal E As ExceptionUnhandledEventArgs)
            RaiseEvent ExceptionUnhandled(Me, E)
        End Sub
#End Region

#Region "DispatcherShutDown Event"
        Public Event DispatcherShutDown As EventHandler

        Protected Overridable Sub OnDispatcherShutDown()
            RaiseEvent DispatcherShutDown(Me, EventArgs.Empty)
        End Sub
#End Region

#Region "IsDispatching Read-Only Property"
        Private _IsDispatching As Boolean

        Public ReadOnly Property IsDispatching As Boolean
            Get
                Return Me._IsDispatching
            End Get
        End Property
#End Region

#Region "IsRunning Read-Only Property"
        Private _IsRunning As Boolean

        Public ReadOnly Property IsRunning As Boolean
            Get
                Return Me._IsRunning
            End Get
        End Property
#End Region

#Region "IsShutDown Read-Only Property"
        Private _IsShutDown As Boolean

        Public ReadOnly Property IsShutDown As Boolean
            Get
                Return Me._IsShutDown
            End Get
        End Property
#End Region

#Region "IsShuttingDown Read-Only Property"
        Private _IsShuttingDown As Boolean

        Public ReadOnly Property IsShuttingDown As Boolean
            Get
                Return Me._IsShuttingDown
            End Get
        End Property
#End Region

#Region "Current Shared Read-Only Property"
        Public Shared ReadOnly Property Current As Dispatcher
            Get
                Return TryCast(SynchronizationContext.Current, DispatcherSynchronizationContext)?.Dispatcher
            End Get
        End Property
#End Region

#Region "Thread Read-Only Property"
        Private _Thread As Thread

        Public ReadOnly Property Thread As Thread
            Get
                Return Me._Thread
            End Get
        End Property
#End Region

#Region "IsDisposed Read-Only Property"
        Private _IsDisposed As Boolean

        Public ReadOnly Property IsDisposed As Boolean
            Get
                Return Me._IsDisposed
            End Get
        End Property
#End Region

        Private ReadOnly LockObject As Object = New Object()
        Private CurrentId As Integer
        Private ReadOnly WaitingActions As IDictionary(Of Integer, (WaitHandle As EventWaitHandle, Exception As Exception)) = New ConcurrentDictionary(Of Integer, (WaitHandle As EventWaitHandle, Exception As Exception))(2, 8)
        Private ReadOnly Queue As BlockingCollection(Of (Action As Action, Id As Integer)) = New BlockingCollection(Of (Action As Action, Id As Integer))()

        Private Class DispatcherSynchronizationContext
            Inherits SynchronizationContext

            Public Sub New(ByVal Dispatcher As Dispatcher)
                Me.Dispatcher = Dispatcher
            End Sub

            Public Overrides Sub Post(d As SendOrPostCallback, state As Object)
                Me.Dispatcher.Invoke(Sub() d.Invoke(state))
            End Sub

            Public Overrides Sub Send(d As SendOrPostCallback, state As Object)
                Me.Dispatcher.InvokeAndWait(Sub() d.Invoke(state))
            End Sub

            Public Overrides Function CreateCopy() As SynchronizationContext
                Return New DispatcherSynchronizationContext(Me.Dispatcher)
            End Function

            Public ReadOnly Dispatcher As Dispatcher

        End Class

    End Class

    Public Class ExceptionUnhandledEventArgs
        Inherits EventArgs

        Public Sub New(ByVal Exception As Exception)
            Me._Exception = Exception
        End Sub

#Region "Exception Read-Only Property"
        Private ReadOnly _Exception As Exception

        Public ReadOnly Property Exception As Exception
            Get
                Return Me._Exception
            End Get
        End Property
#End Region

    End Class

End Namespace
