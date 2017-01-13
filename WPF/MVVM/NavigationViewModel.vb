Namespace Common.MVVM

    Public MustInherit Class NavigationViewModel
        Inherits ViewModel

        Public Sub New(ByVal KsApplication As KsApplication)
            MyBase.New(KsApplication)
            If Not GetType(INavigationView).IsAssignableFrom(Metadata.ViewType) Then
                Throw New InvalidOperationException("Navigation views must implement INavigationView.")
            End If
        End Sub

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub NavigateTo(ByVal ViewModel As ViewModel, Optional ByVal AddToStack As Boolean = True, Optional ByVal ForceToStack As Boolean = False)
            Me.KsApplicationBase.NavigateTo(Me, ViewModel, AddToStack, ForceToStack)
        End Sub

        Friend ReadOnly Property NavigationView As INavigationView
            Get
                Return DirectCast(Me.View, INavigationView)
            End Get
        End Property

    End Class

End Namespace
