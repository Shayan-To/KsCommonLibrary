Namespace Common

    Public Class Closable
        Implements IDisposable

        Public Sub New(ByVal CloseOperation As Action)
            Me.CloseOperation = CloseOperation
        End Sub

        Public Sub Close()
            Me.CloseOperation.Invoke()
        End Sub

        Private Sub Dispose() Implements IDisposable.Dispose
            Me.Close()
        End Sub

        Private ReadOnly CloseOperation As Action

    End Class

End Namespace
