Imports System.Windows.Markup
Imports System.Windows.Media
Imports Ks.Common

Namespace Controls

    Public Class ResizableCanvas
        Inherits Panel

        Protected Overrides Function MeasureOverride(availableSize As Size) As Size
            Dim Size = New Size(Double.PositiveInfinity, Double.PositiveInfinity)

            For Each C As UIElement In Me.Children
                C.Measure(Size)
            Next

            Return New Size()
        End Function

        Protected Overrides Function ArrangeOverride(FinalSize As Size) As Size
            Dim Padding = Me.Padding
            Dim Padding1 = New Point(Padding.Left, Padding.Top)
            Dim Padding2 = New Point(Padding.Right, Padding.Bottom)

            Dim Client = New Rect(Padding1,
                                  (-Padding2.ToVector() + FinalSize.ToVector()).ToPoint())

            Dim TRect = New Rect(FinalSize)
            Dim Padding1InShape = TRect.ToLocal01(Padding1)
            Dim Padding2InShape = TRect.ToLocal01(Padding2)
            TRect = New Rect(Client.Size)
            Padding1InShape = TRect.FromLocal01(Padding1InShape)
            Padding2InShape = TRect.FromLocal01(Padding2InShape)

            Dim ShapeRectangle = Me.ShapeRectangle
            ShapeRectangle.Location += Padding1InShape.ToVector()
            ShapeRectangle.Size = (ShapeRectangle.Size.ToVector() - Padding1InShape.ToVector() - Padding2InShape.ToVector()).ToSizeSafe()

            Dim ViewingRectangle = Me.ViewingRectangle
            ViewingRectangle.Intersect(ShapeRectangle)

            If Me.KeepAspectRatio Then
                Dim RB = ViewingRectangle.GetSmallestBoundOf(Client.Size)
                If Not RB.Item2.HasValue Then
                    Console.WriteLine("Error. " & Utilities.CompactStackTrace(5))
                    Return FinalSize
                End If
                ViewingRectangle = RB.Item1
                If Not ShapeRectangle.Contains(ViewingRectangle) Then
                    If RB.Item2.Value Then
                        Dim B1 = ViewingRectangle.Top < ShapeRectangle.Top
                        Dim B2 = ViewingRectangle.Bottom > ShapeRectangle.Bottom
                        If B1 Xor B2 Then
                            If B1 Then
                                ViewingRectangle.Y = ShapeRectangle.Y
                            Else
                                ViewingRectangle.Y = ShapeRectangle.Y + ShapeRectangle.Height - ViewingRectangle.Height
                            End If
                        End If
                    Else
                        Dim B1 = ViewingRectangle.Left < ShapeRectangle.Left
                        Dim B2 = ViewingRectangle.Right > ShapeRectangle.Right
                        If B1 Xor B2 Then
                            If B1 Then
                                ViewingRectangle.X = ShapeRectangle.X
                            Else
                                ViewingRectangle.X = ShapeRectangle.X + ShapeRectangle.Width - ViewingRectangle.Width
                            End If
                        End If
                    End If
                End If
                'If Not ShapeRectangle.Contains(ViewingRectangle) Then
                '    RB = ShapeRectangle.GetLargestFitOf(Client.Size)
                '    ViewingRectangle = RB.Item1
                'End If
            End If

            For Each C As UIElement In Me.Children
                'If Double.IsNaN(Left) Then
                '    Left = 0
                'End If
                'If Double.IsNaN(Top) Then
                '    Top = 0
                'End If

                'If TypeOf C Is Control Then
                '    If DirectCast(C, Control).Tag Is "Back" Then
                '        'Stop
                '    End If
                'End If

                Dim Pt = New Point(GetX(C), GetY(C))
                Dim Sz = New Vector(GetW(C), GetH(C))

                'If Not ViewingRectangle.IntersectsWith(New Rect(Pt, Sz)) Then
                '    Continue For
                'End If

                Dim Pt1 = Pt
                Pt1 = ViewingRectangle.ToLocal01(Pt1)
                Pt1 = Client.FromLocal01(Pt1)

                If Sz = New Vector() Then
                    C.Arrange(New Rect(Pt1, C.DesiredSize))
                Else
                    Dim Pt2 = Pt + Sz
                    Pt2 = ViewingRectangle.ToLocal01(Pt2)
                    Pt2 = Client.FromLocal01(Pt2)
                    C.Arrange(New Rect(Pt1, Pt2))
                End If
            Next

            Return FinalSize
        End Function

#Region "ShapeRectangle Property"
        Public Shared ReadOnly ShapeRectangleProperty As DependencyProperty = DependencyProperty.Register("ShapeRectangle", GetType(Rect), GetType(ResizableCanvas), New PropertyMetadata(New Rect(0, 0, 1, 1), AddressOf ShapeRectangle_Changed))

        Private Shared Sub ShapeRectangle_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, ResizableCanvas)
            Self.InvalidateArrange()
        End Sub

        Public Property ShapeRectangle As Rect
            Get
                Return DirectCast(Me.GetValue(ShapeRectangleProperty), Rect)
            End Get
            Set(value As Rect)
                Me.SetValue(ShapeRectangleProperty, value)
            End Set
        End Property
#End Region

#Region "ViewingRectangle Property"
        Public Shared ReadOnly ViewingRectangleProperty As DependencyProperty = DependencyProperty.Register("ViewingRectangle", GetType(Rect), GetType(ResizableCanvas), New PropertyMetadata(New Rect(0, 0, 1, 1), AddressOf ViewingRectangle_Changed))

        Private Shared Sub ViewingRectangle_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, ResizableCanvas)
            Self.InvalidateArrange()
        End Sub

        Public Property ViewingRectangle As Rect
            Get
                Return DirectCast(Me.GetValue(ViewingRectangleProperty), Rect)
            End Get
            Set(value As Rect)
                Me.SetValue(ViewingRectangleProperty, value)
            End Set
        End Property
#End Region

#Region "KeepAspectRatio Property"
        Public Shared ReadOnly KeepAspectRatioProperty As DependencyProperty = DependencyProperty.Register("KeepAspectRatio", GetType(Boolean), GetType(ResizableCanvas), New PropertyMetadata(True, AddressOf KeepAspectRatio_Changed))

        Private Shared Sub KeepAspectRatio_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, ResizableCanvas)
            Self.InvalidateArrange()
        End Sub

        Public Property KeepAspectRatio As Boolean
            Get
                Return DirectCast(Me.GetValue(KeepAspectRatioProperty), Boolean)
            End Get
            Set(value As Boolean)
                Me.SetValue(KeepAspectRatioProperty, value)
            End Set
        End Property
#End Region

#Region "Padding Property"
        Public Shared ReadOnly PaddingProperty As DependencyProperty = DependencyProperty.Register("Padding", GetType(Thickness), GetType(ResizableCanvas), New PropertyMetadata(New Thickness(), AddressOf Padding_Changed))

        Private Shared Sub Padding_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, ResizableCanvas)
            Self.InvalidateArrange()
        End Sub

        Public Property Padding As Thickness
            Get
                Return DirectCast(Me.GetValue(PaddingProperty), Thickness)
            End Get
            Set(value As Thickness)
                Me.SetValue(PaddingProperty, value)
            End Set
        End Property
#End Region

#Region "Y Property"
        Public Shared ReadOnly YProperty As DependencyProperty = DependencyProperty.RegisterAttached("Y", GetType(Double), GetType(ResizableCanvas), New PropertyMetadata(0.0, AddressOf Y_Changed))

        Private Shared Sub Y_Changed(ByVal D As System.Windows.DependencyObject, ByVal E As System.Windows.DependencyPropertyChangedEventArgs)
            Dim C = TryCast(VisualTreeHelper.GetParent(D), ResizableCanvas)
            If C IsNot Nothing Then
                C.InvalidateArrange()
            End If
        End Sub

        Public Shared Function GetY(ByVal O As UIElement) As Double
            Return DirectCast(O.GetValue(YProperty), Double)
        End Function

        Public Shared Sub SetY(ByVal O As UIElement, ByVal Value As Double)
            O.SetValue(YProperty, Value)
        End Sub
#End Region

#Region "X Property"
        Public Shared ReadOnly XProperty As DependencyProperty = DependencyProperty.RegisterAttached("X", GetType(Double), GetType(ResizableCanvas), New PropertyMetadata(0.0, AddressOf X_Changed))

        Private Shared Sub X_Changed(ByVal D As System.Windows.DependencyObject, ByVal E As System.Windows.DependencyPropertyChangedEventArgs)
            Dim C = TryCast(VisualTreeHelper.GetParent(D), ResizableCanvas)
            If C IsNot Nothing Then
                C.InvalidateArrange()
            End If
        End Sub

        Public Shared Function GetX(ByVal O As UIElement) As Double
            Return DirectCast(O.GetValue(XProperty), Double)
        End Function

        Public Shared Sub SetX(ByVal O As UIElement, ByVal Value As Double)
            O.SetValue(XProperty, Value)
        End Sub
#End Region

#Region "H Property"
        Public Shared ReadOnly HProperty As DependencyProperty = DependencyProperty.RegisterAttached("H", GetType(Double), GetType(ResizableCanvas), New PropertyMetadata(0.0, AddressOf H_Changed))

        Private Shared Sub H_Changed(ByVal D As System.Windows.DependencyObject, ByVal E As System.Windows.DependencyPropertyChangedEventArgs)
            Dim C = TryCast(VisualTreeHelper.GetParent(D), ResizableCanvas)
            If C IsNot Nothing Then
                C.InvalidateArrange()
            End If
        End Sub

        Public Shared Function GetH(ByVal O As UIElement) As Double
            Return DirectCast(O.GetValue(HProperty), Double)
        End Function

        Public Shared Sub SetH(ByVal O As UIElement, ByVal Value As Double)
            O.SetValue(HProperty, Value)
        End Sub
#End Region

#Region "W Property"
        Public Shared ReadOnly WProperty As DependencyProperty = DependencyProperty.RegisterAttached("W", GetType(Double), GetType(ResizableCanvas), New PropertyMetadata(0.0, AddressOf W_Changed))

        Private Shared Sub W_Changed(ByVal D As System.Windows.DependencyObject, ByVal E As System.Windows.DependencyPropertyChangedEventArgs)
            Dim C = TryCast(VisualTreeHelper.GetParent(D), ResizableCanvas)
            If C IsNot Nothing Then
                C.InvalidateArrange()
            End If
        End Sub

        Public Shared Function GetW(ByVal O As UIElement) As Double
            Return DirectCast(O.GetValue(WProperty), Double)
        End Function

        Public Shared Sub SetW(ByVal O As UIElement, ByVal Value As Double)
            O.SetValue(WProperty, Value)
        End Sub
#End Region

    End Class

End Namespace
