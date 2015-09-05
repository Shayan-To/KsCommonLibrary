Imports System.Windows.Markup

Namespace Controls

    <ContentProperty("Content")>
    Public Class Squaring
        Inherits FrameworkElement

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(Squaring), New FrameworkPropertyMetadata(GetType(Squaring)))
        End Sub

        Protected Overrides Function MeasureOverride(constraint As Size) As Size
            Dim Size = New Size(constraint.Width, constraint.Width)
            If constraint.Height < constraint.Width Then
                Size = New Size(constraint.Height, constraint.Height)
            End If

            Dim Content = Me.Content
            If Content IsNot Nothing Then
                Content.Measure(Size)
            End If

            If Double.IsInfinity(Size.Width) Then
                If Content IsNot Nothing Then
                    Size = Content.DesiredSize
                    If Size.Width > Size.Height Then
                        Size = New Size(Size.Width, Size.Width)
                    Else
                        Size = New Size(Size.Height, Size.Height)
                    End If
                Else
                    Size = New Size()
                End If
            End If

            Return Size
        End Function

        Protected Overrides Function ArrangeOverride(arrangeBounds As Size) As Size
            Dim Content = Me.Content
            If Content Is Nothing Then
                Return arrangeBounds
            End If

            Dim Rect As Rect
            If arrangeBounds.Height < arrangeBounds.Width Then
                Rect = New Rect((arrangeBounds.Width - arrangeBounds.Height) / 2, 0, arrangeBounds.Height, arrangeBounds.Height)
            Else
                Rect = New Rect(0, (arrangeBounds.Height - arrangeBounds.Width) / 2, arrangeBounds.Width, arrangeBounds.Width)
            End If

            Content.Arrange(Rect)

            Return arrangeBounds
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
            Dim Self = DirectCast(D, Squaring)

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
