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
                Return Content.DesiredSize
            End If
            Return New Size()
        End Function

        Protected Overrides Function ArrangeOverride(ByVal FinalSize As Size) As Size
            Me.Content?.Arrange(New Rect(FinalSize))
            Return FinalSize
        End Function

        Protected Overrides Function GetVisualChild(index As Integer) As Windows.Media.Visual
            Verify.TrueArg(index = 0 And Me.Content IsNot Nothing, NameOf(index))
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

            Dim OldValue = DirectCast(E.OldValue, UIElement)
            Dim NewValue = DirectCast(E.NewValue, UIElement)

            If OldValue IsNot Nothing Then
                Self.RemoveVisualChild(OldValue)
                Self.RemoveLogicalChild(OldValue)
            End If
            If NewValue IsNot Nothing Then
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