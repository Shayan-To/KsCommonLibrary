Imports Ks.Common.MVVM

Namespace Controls

    Public Class ContentPage
        Inherits Page
        Implements INavigationView

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(ContentPage), New FrameworkPropertyMetadata(GetType(ContentPage)))
        End Sub

        Private Property INavigationView_Content As Page Implements INavigationView.Content
            Get
                Return DirectCast(Me.Content, Page)
            End Get
            Set(ByVal Value As Page)
                Me.Content = Value
            End Set
        End Property

    End Class

End Namespace