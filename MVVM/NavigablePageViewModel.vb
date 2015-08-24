Namespace MVVM

    Public Class NavigablePageViewModel(Of T As NavigationWindowViewModel)
        Inherits NavigablePageViewModelBase

        Public Sub New(ByVal NavigationWindow As T, ByVal Page As Page)
            MyBase.New(NavigationWindow, Page)
        End Sub

        Public Sub New(ByVal NavigationWindow As T)
            MyBase.New(NavigationWindow)
        End Sub

#Region "NavigationWindow Property"
        Public Shadows ReadOnly Property NavigationWindow As T
            Get
                Return DirectCast(MyBase.NavigationWindow, T)
            End Get
        End Property
#End Region

    End Class

End Namespace
