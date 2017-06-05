Imports System.Runtime.CompilerServices
Imports Ks.Common.Controls

Namespace Common.MVVM

    Public Module MvvmExtensions

        <Extension()>
        Public Function IsNavigation(ByVal Self As ViewModel) As Boolean
            Return TypeOf Self Is NavigationViewModel
        End Function

        <Extension()>
        Friend Function GetNavigationView(ByVal Self As ViewModel) As INavigationView
            Return DirectCast(Self.View, INavigationView)
        End Function

        <Extension()>
        Friend Sub SetView(ByVal Navigation As ViewModel, ByVal ViewModel As ViewModel)
            Navigation.GetNavigationView().SetContent(DirectCast(ViewModel?.View, Page))
        End Sub

        <Extension()>
        Friend Sub SetContent(ByVal NavigationView As INavigationView, ByVal View As Page)
            Dim Prev = NavigationView.Content
            If Prev Is View Then
                Exit Sub
            End If

            If Prev IsNot Nothing Then
                Prev.ParentView = Nothing
            End If

            If View IsNot Nothing Then
                If View.ParentView IsNot Nothing Then
                    View.ParentView.Content = Nothing
                End If
                View.ParentView = NavigationView
            End If

            NavigationView.Content = View
        End Sub

    End Module

End Namespace
