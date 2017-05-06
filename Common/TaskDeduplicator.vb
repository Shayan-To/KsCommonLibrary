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
        End Sub

        Private IsTaskGoingOn As Boolean
        Private IsTaskPending As Boolean
        Private ReadOnly Task As Func(Of Task)

    End Class

End Namespace
