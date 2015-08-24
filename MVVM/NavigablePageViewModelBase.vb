Namespace MVVM

    Public Class NavigablePageViewModelBase
        Inherits PageViewModel

        Public Sub New(ByVal NavigationWindow As NavigationWindowViewModel, ByVal Page As Page)
            MyBase.New(Page)
            Me._NavigationWindow = NavigationWindow
        End Sub

        Public Sub New(ByVal NavigationWindow As NavigationWindowViewModel)
            Me._NavigationWindow = NavigationWindow
        End Sub

#Region "NavigatingTo Event"
        Public Event NavigatingTo As EventHandler(Of EventArgs)

        Protected Friend Overridable Sub OnNavigatingTo(ByVal E As EventArgs)
            RaiseEvent NavigatingTo(Me, E)
        End Sub
#End Region

#Region "NavigatedTo Event"
        Public Event NavigatedTo As EventHandler(Of EventArgs)

        Protected Friend Overridable Sub OnNavigatedTo(ByVal E As EventArgs)
            RaiseEvent NavigatedTo(Me, E)
        End Sub
#End Region

#Region "NavigatingFrom Event"
        Public Event NavigatingFrom As EventHandler(Of EventArgs)

        Protected Friend Overridable Sub OnNavigatingFrom(ByVal E As EventArgs)
            RaiseEvent NavigatingFrom(Me, E)
        End Sub
#End Region

#Region "NavigatedFrom Event"
        Public Event NavigatedFrom As EventHandler(Of EventArgs)

        Protected Friend Overridable Sub OnNavigatedFrom(ByVal E As EventArgs)
            RaiseEvent NavigatedFrom(Me, E)
        End Sub
#End Region

#Region "NavigationWindow Property"
        Private ReadOnly _NavigationWindow As NavigationWindowViewModel

        Public ReadOnly Property NavigationWindow As NavigationWindowViewModel
            Get
                Return Me._NavigationWindow
            End Get
        End Property
#End Region

    End Class

End Namespace
