Namespace Common.MVVM

    Public Class NavigationEventArgs
        Inherits EventArgs

        Public Sub New(ByVal NavigationType As NavigationType)
            Me._NavigationType = NavigationType
        End Sub

#Region "NavigationType Read-Only Property"
        Private ReadOnly _NavigationType As NavigationType

        Public ReadOnly Property NavigationType As NavigationType
            Get
                Return Me._NavigationType
            End Get
        End Property
#End Region

    End Class

End Namespace
