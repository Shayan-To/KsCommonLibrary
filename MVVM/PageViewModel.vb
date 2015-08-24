Namespace MVVM

    Public Class PageViewModel
        Inherits ViewModelBase

        Public Sub New(ByVal Page As Page)
            MyBase.New(False)

            Me._Page = Page
            Page.DataContext = Me
        End Sub

        Public Sub New()

        End Sub

#Region "Page Property"
        Private ReadOnly _Page As Page

        Public ReadOnly Property Page As Page
            Get
                Return Me._Page
            End Get
        End Property
#End Region

    End Class

End Namespace
