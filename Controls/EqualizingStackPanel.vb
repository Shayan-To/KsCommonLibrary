Namespace Controls

    Public Class EqualizingStackPanel
        Inherits Panel

        ' As Orientation.Horizontal is 0 (which makes no changes to the Orientation when got XOred with it), then the default for writing the Measure and Arrange methods will be the horizontal orientation.

        Protected Overrides Function MeasureOverride(ByVal AvailableSize As Size) As Size
            Dim ChildCount = Me.Children.Count

            If ChildCount = 0 Then
                Return Size.Empty
            End If

            Dim MaxHeight = 0.0
            Dim MaxWidth = 0.0
            AvailableSize = Me.CreateSize(Me.Dimension(AvailableSize, Orientation.Horizontal) / ChildCount,
                                          Me.Dimension(AvailableSize, Orientation.Vertical))

            For Each C As UIElement In Me.Children
                C.Measure(AvailableSize)
                Dim Sz = C.DesiredSize
                MaxWidth = Math.Max(MaxWidth, Me.Dimension(Sz, Orientation.Horizontal))
                MaxHeight = Math.Max(MaxHeight, Me.Dimension(Sz, Orientation.Vertical))
            Next

            Return Me.CreateSize(MaxWidth * ChildCount, MaxHeight)
        End Function

        Protected Overrides Function ArrangeOverride(ByVal FinalSize As Size) As Size
            Dim ChildCount = Me.Children.Count
            Dim OrigFinalSize = FinalSize

            Dim Width = Me.Dimension(FinalSize, Orientation.Horizontal) / ChildCount
            FinalSize = Me.CreateSize(Width, Me.Dimension(FinalSize, Orientation.Vertical))
            Dim X = 0.0

            For Each C As UIElement In Me.Children
                C.Arrange(New Rect(Me.CreatePoint(X, 0), FinalSize))
                X += Width
            Next

            Return OrigFinalSize
        End Function

        Private Function CreateSize(ByVal Width As Double, ByVal Height As Double) As Size
            If Me.OrientationCache = Orientation.Horizontal Then
                Return New Size(Width, Height)
            Else
                Return New Size(Height, Width)
            End If
        End Function

        Private Function CreatePoint(ByVal X As Double, ByVal Y As Double) As Point
            If Me.OrientationCache = Orientation.Horizontal Then
                Return New Point(X, Y)
            Else
                Return New Point(Y, X)
            End If
        End Function

        Private ReadOnly Property Dimension(ByVal Size As Size, ByVal Orientation As Orientation) As Double
            Get
                Orientation = Orientation Xor Me.OrientationCache
                If Orientation = Orientation.Horizontal Then
                    Return Size.Width
                Else
                    Return Size.Height
                End If
            End Get
        End Property

#Region "Orientation Property"
        Public Shared ReadOnly OrientationProperty As DependencyProperty = DependencyProperty.Register("Orientation", GetType(Orientation), GetType(EqualizingStackPanel), New PropertyMetadata(Orientation.Vertical, AddressOf Orientation_Changed, AddressOf Orientation_Coerce))

        Private Shared Function Orientation_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = DirectCast(D, EqualizingStackPanel)

            'Dim Value = DirectCast(BaseValue, Orientation)

            Return BaseValue
        End Function

        Private Shared Sub Orientation_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, EqualizingStackPanel)

            'Dim OldValue = DirectCast(E.OldValue, Orientation)
            Dim NewValue = DirectCast(E.NewValue, Orientation)

            Self.OrientationCache = NewValue

            Self.InvalidateMeasure()
        End Sub

        Public Property Orientation As Orientation
            Get
                Return DirectCast(Me.GetValue(OrientationProperty), Orientation)
            End Get
            Set(ByVal value As Orientation)
                Me.SetValue(OrientationProperty, value)
            End Set
        End Property
#End Region

        Private OrientationCache As Orientation = Orientation.Vertical

    End Class

End Namespace
