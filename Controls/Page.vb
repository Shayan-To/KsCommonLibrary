Imports Ks.Common.MVVM

Namespace Controls

    Public Class Page
        Inherits Windows.Controls.ContentControl

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(Page), New FrameworkPropertyMetadata(GetType(Page)))
        End Sub

#Region "ParentView Property"
        Private _ParentView As INavigationView

        Friend Property ParentView As INavigationView
            Get
                Return Me._ParentView
            End Get
            Set(ByVal Value As INavigationView)
                Me._ParentView = Value
            End Set
        End Property
#End Region

    End Class

End Namespace