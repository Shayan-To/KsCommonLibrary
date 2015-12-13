Public Class Freezable

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
        If Me._IsFreezed Then
            Throw New InvalidOperationException("Cannot change a freezed object.")
        End If
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
