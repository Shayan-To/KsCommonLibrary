Namespace MVVM

    Public Class WindowViewModel
        Inherits ViewModelBase

        Public Sub New(ByVal Window As Window)
            MyBase.New(False)

            Me._Window = Window
            Window.DataContext = Me
        End Sub

        Public Sub New()
            Me._Window = Nothing
        End Sub

#Region "Window Property"
        Private ReadOnly _Window As Window

        Public ReadOnly Property Window As Window
            Get
                Return Me._Window
            End Get
        End Property
#End Region

    End Class

End Namespace
