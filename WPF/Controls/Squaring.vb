Namespace Controls

    Public Class Squaring
        Inherits ContentControl

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(Squaring), New FrameworkPropertyMetadata(GetType(Squaring)))
        End Sub

        Protected Overrides Function MeasureOverride(ByVal Constraint As Size) As Size
            Dim Dimension = Math.Min(Constraint.Width, Constraint.Height)
            Dim Size = New Size(Dimension, Dimension)

            Dim Content = Me.Content
            Content?.Measure(Size)

            If Me.FillAvailableSpace Then
                If Double.IsPositiveInfinity(Dimension) Then
                    Return New Size()
                End If
                Return Size
            End If

            Dimension = 0.0
            If Content IsNot Nothing Then
                Size = Content.DesiredSize
                Dimension = Math.Max(Size.Width, Size.Height)
            End If

            Return New Size(Dimension, Dimension)
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

#Region "FillAvailableSpace Property"
        Public Shared ReadOnly FillAvailableSpaceProperty As DependencyProperty = DependencyProperty.Register("FillAvailableSpace", GetType(Boolean), GetType(Squaring), New PropertyMetadata(True, AddressOf FillAvailableSpace_Changed, AddressOf FillAvailableSpace_Coerce))

        Private Shared Function FillAvailableSpace_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = DirectCast(D, Squaring)

            'Dim Value = DirectCast(BaseValue, Boolean)

            Return BaseValue
        End Function

        Private Shared Sub FillAvailableSpace_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, Squaring)

            'Dim OldValue = DirectCast(E.OldValue, Boolean)
            'Dim NewValue = DirectCast(E.NewValue, Boolean)

            Self.InvalidateMeasure()
        End Sub

        Public Property FillAvailableSpace As Boolean
            Get
                Return DirectCast(Me.GetValue(FillAvailableSpaceProperty), Boolean)
            End Get
            Set(ByVal value As Boolean)
                Me.SetValue(FillAvailableSpaceProperty, value)
            End Set
        End Property
#End Region

    End Class

End Namespace
