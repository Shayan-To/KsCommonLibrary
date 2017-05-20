Namespace Common

    Public Class TaskDeduplicator

        Public Sub New(ByVal Task As Func(Of Task))
            Me.Task = Task
        End Sub

        Public Async Sub Run()
            If Me.IsTaskGoingOn Then
                Me.IsTaskPending = True
                Exit Sub
            End If

            Me.IsTaskGoingOn = True
            Do
                Me.IsTaskPending = False
                Await Me.Task.Invoke()
            Loop While Me.IsTaskPending
            Me.IsTaskGoingOn = False

            Me.TaskDoneTaskSource?.SetResult(Nothing)
            Me.TaskDoneTaskSource = Nothing
        End Sub

        Public Function WaitTillDoneAsync() As Task
            If Not Me.IsTaskGoingOn Then
                Return Threading.Tasks.Task.FromResult(Of Void)(Nothing)
            End If
            Me.TaskDoneTaskSource = New TaskCompletionSource(Of Void)()
            Return Me.TaskDoneTaskSource.Task
        End Function

        Private TaskDoneTaskSource As TaskCompletionSource(Of Void)
        Private IsTaskGoingOn As Boolean
        Private IsTaskPending As Boolean
        Private ReadOnly Task As Func(Of Task)

    End Class

End Namespace
