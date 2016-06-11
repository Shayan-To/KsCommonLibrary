Imports Ks.Common.MVVM

Namespace Controls

    <ComponentModel.DesignTimeVisible(False)>
    Public Class Page
        Inherits Windows.Controls.ContentControl

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(Page), New FrameworkPropertyMetadata(GetType(Page)))
        End Sub

#Region "Title Property"
        Public Shared ReadOnly TitleProperty As DependencyProperty = DependencyProperty.Register("Title", GetType(String), GetType(Page), New PropertyMetadata(Nothing))

        Public Property Title As String
            Get
                Return DirectCast(Me.GetValue(TitleProperty), String)
            End Get
            Set(ByVal value As String)
                Me.SetValue(TitleProperty, value)
            End Set
        End Property
#End Region

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