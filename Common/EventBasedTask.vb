Imports System.Runtime.CompilerServices

Namespace Common

    <Obsolete("Use " + NameOf(TaskCompletionSource(Of Void)) + " instead.", True)>
    Public Class EventBasedTask
        Implements INotifyCompletion

        Public Function GetAwaiter() As EventBasedTask
            Return Me
        End Function

        Public Sub GetResult()
            Verify.True(Me.IsCompleted)
        End Sub

        Public Sub SetComplete()
            If Not Me._IsCompleted Then
                Me._IsCompleted = True
                Me.CompletedAction?.Invoke()
            End If
        End Sub

        Public Sub OnCompleted(continuation As Action) Implements INotifyCompletion.OnCompleted
            Verify.True(Me.CompletedAction Is Nothing)
            Me.CompletedAction = continuation
        End Sub

#Region "IsCompleted Property"
        Private _IsCompleted As Boolean

        Public ReadOnly Property IsCompleted As Boolean
            Get
                Return Me._IsCompleted
            End Get
        End Property
#End Region

        Private CompletedAction As Action

    End Class

End Namespace