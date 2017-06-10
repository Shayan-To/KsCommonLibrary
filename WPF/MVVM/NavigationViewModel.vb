Namespace Common.MVVM

    Public MustInherit Class NavigationViewModel
        Inherits ViewModel

        Public Sub New(ByVal KsApplication As KsApplication)
            MyBase.New(KsApplication)
            If Not GetType(INavigationView).IsAssignableFrom(Me.Metadata.ViewType) Then
                Throw New InvalidOperationException("Navigation views must implement INavigationView.")
            End If
        End Sub

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub NavigateTo(ByVal ViewModel As ViewModel, Optional ByVal AddToStack As Boolean = True, Optional ByVal ForceToStack As Boolean = False)
            Me.KsApplicationBase.NavigateTo(Me, ViewModel, AddToStack, ForceToStack)
        End Sub

    End Class

End Namespace
