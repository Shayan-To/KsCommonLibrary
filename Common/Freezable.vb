Public Class Freezable
    Inherits MVVM.BindableBase

    Public Sub Freeze()
        If Me.FreezeCalled Then
            Exit Sub
        End If
        Me.FreezeCalled = True
        Me.OnFreezing()
        Me._IsFreezed = True
    End Sub

    Protected Overridable Sub OnFreezing()

    End Sub

    Protected Sub VerifyWrite()
        Verify.False(Me._IsFreezed, "Cannot change a freezed object.")
    End Sub

#Region "IsFreezed Property"
    Private _IsFreezed As Boolean

    Public ReadOnly Property IsFreezed As Boolean
        Get
            Return Me._IsFreezed
        End Get
    End Property
#End Region

    Private FreezeCalled As Boolean

End Class
