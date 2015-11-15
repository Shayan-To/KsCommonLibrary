Imports System.Windows.Markup

Namespace Controls

    <ContentProperty("Content")>
    Public Class ContentControl
        Inherits Control

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(ContentControl), New FrameworkPropertyMetadata(GetType(ContentControl)))
        End Sub

        Protected Overrides Function MeasureOverride(ByVal AvailableSize As Size) As Size
            Dim Content = Me.Content
            If Content IsNot Nothing Then
                Content.Measure(AvailableSize)
            End If

            If Content IsNot Nothing Then
                Return Content.DesiredSize
            Else
                Return New Size()
            End If
        End Function

        Protected Overrides Function ArrangeOverride(ByVal FinalSize As Size) As Size
            Dim Content = Me.Content

            If Content IsNot Nothing Then
                Content.Arrange(New Rect(New Point(), FinalSize))
            End If

            Return FinalSize
        End Function

        Protected Overrides Function GetVisualChild(index As Integer) As Windows.Media.Visual
            If index <> 0 Or Me.Content Is Nothing Then
                Throw New ArgumentException()
            End If

            Return Me.Content
        End Function

        Protected Overrides ReadOnly Property VisualChildrenCount As Integer
            Get
                Return If(Me.Content Is Nothing, 0, 1)
            End Get
        End Property

        Protected Overrides ReadOnly Iterator Property LogicalChildren As IEnumerator
            Get
                If Me.Content IsNot Nothing Then
                    Yield Me.Content
                End If
            End Get
        End Property

#Region "Content Property"
        Public Shared ReadOnly ContentProperty As DependencyProperty = DependencyProperty.Register("Content", GetType(UIElement), GetType(Squaring), New PropertyMetadata(Nothing, AddressOf Content_Changed))

        Public Shared Sub Content_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, ContentControl)

            If E.OldValue IsNot Nothing Then
                Dim OldValue = DirectCast(E.OldValue, UIElement)
                Self.RemoveVisualChild(OldValue)
                Self.RemoveLogicalChild(OldValue)
            End If
            If E.NewValue IsNot Nothing Then
                Dim NewValue = DirectCast(E.NewValue, UIElement)
                Self.AddVisualChild(NewValue)
                Self.AddLogicalChild(NewValue)
            End If

            Self.InvalidateArrange()
        End Sub

        Public Property Content As UIElement
            Get
                Return DirectCast(Me.GetValue(ContentProperty), UIElement)
            End Get
            Set(value As UIElement)
                Me.SetValue(ContentProperty, value)
            End Set
        End Property
#End Region

    End Class

End Namespace