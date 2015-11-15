Imports System.Windows.Media
Imports Ks.Common

Namespace Controls

    Public Class ResizableCanvas
        Inherits Panel

        Private Function GetViewingArea(ByVal Size As Size) As Rect?
            Dim ViewingRectangle = Me.ViewingRectangle
            'ViewingRectangle.Intersect(ShapeRectangle)

            If Me.KeepAspectRatio Then
                Dim RB = ViewingRectangle.GetSmallestBoundOf(Me.GetClientArea(Size).Size)
                If Not RB.Item2.HasValue Then
                    Console.WriteLine("Error. " & Utilities.Debug.CompactStackTrace(5))
                    Return Nothing
                End If
                ViewingRectangle = RB.Item1
                'If Not ShapeRectangle.Contains(ViewingRectangle) Then
                '    If RB.Item2.Value Then
                '        Dim B1 = ViewingRectangle.Top < ShapeRectangle.Top
                '        Dim B2 = ViewingRectangle.Bottom > ShapeRectangle.Bottom
                '        If B1 Xor B2 Then
                '            If B1 Then
                '                ViewingRectangle.Y = ShapeRectangle.Y
                '            Else
                '                ViewingRectangle.Y = ShapeRectangle.Y + ShapeRectangle.Height - ViewingRectangle.Height
                '            End If
                '        End If
                '    Else
                '        Dim B1 = ViewingRectangle.Left < ShapeRectangle.Left
                '        Dim B2 = ViewingRectangle.Right > ShapeRectangle.Right
                '        If B1 Xor B2 Then
                '            If B1 Then
                '                ViewingRectangle.X = ShapeRectangle.X
                '            Else
                '                ViewingRectangle.X = ShapeRectangle.X + ShapeRectangle.Width - ViewingRectangle.Width
                '            End If
                '        End If
                '    End If
                'End If
                ''If Not ShapeRectangle.Contains(ViewingRectangle) Then
                ''    RB = ShapeRectangle.GetLargestFitOf(Client.Size)
                ''    ViewingRectangle = RB.Item1
                ''End If
            End If

            Return ViewingRectangle
        End Function

        Private Function GetClientArea(ByVal Size As Size) As Rect
            Dim Padding = Me.Padding
            Dim Padding1 = New Point(Padding.Left, Padding.Top)
            Dim Padding2 = New Point(Padding.Right, Padding.Bottom)

            Return New Rect(Padding1,
                            (-Padding2.ToVector() + Size.ToVector()).ToPoint())
        End Function

        Protected Overrides Function MeasureOverride(ByVal AvailableSize As Size) As Size
            Dim InfSize = New Size(Double.PositiveInfinity, Double.PositiveInfinity)

            Dim Client = Me.GetClientArea(AvailableSize)

            Dim ViewingRectangleQ = Me.GetViewingArea(AvailableSize)
            If Not ViewingRectangleQ.HasValue Then
                Return AvailableSize
            End If
            Dim ViewingRectangle = ViewingRectangleQ.Value

            Dim Sz0 = New Point()

            Sz0 = ViewingRectangle.ToLocal01(Sz0)
            Sz0 = Client.FromLocal01(Sz0)

            For Each C As UIElement In Me.Children
                Dim Sz = New Vector(GetW(C), GetH(C))

                If Sz = New Vector() Then
                    C.Measure(InfSize)
                Else
                    Dim Szz = Sz.ToPoint()

                    Szz = ViewingRectangle.ToLocal01(Szz)
                    Szz = Client.FromLocal01(Szz)

                    Dim Size = (Szz - Sz0).ToSize()
                    If Double.IsNaN(Size.Width) Or Double.IsNaN(Size.Height) Then
                        Size = New Size()
                    End If

                    C.Measure(Size)
                End If
            Next

            Return New Size()
        End Function

        Protected Overrides Function ArrangeOverride(FinalSize As Size) As Size
            Dim Client = Me.GetClientArea(FinalSize)

            Dim ViewingRectangleQ = Me.GetViewingArea(FinalSize)
            If Not ViewingRectangleQ.HasValue Then
                Return FinalSize
            End If
            Dim ViewingRectangle = ViewingRectangleQ.Value

            For Each C As UIElement In Me.Children
                Dim Pt = New Point(GetX(C), GetY(C))
                Dim Sz = New Vector(GetW(C), GetH(C))

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

        ' ToDo Support Padding and MaxViewingRectangle.

#Region "ViewingRectangle Property"
        Public Shared ReadOnly ViewingRectangleProperty As DependencyProperty = DependencyProperty.Register("ViewingRectangle", GetType(Rect), GetType(ResizableCanvas), New PropertyMetadata(New Rect(0, 0, 1, 1), AddressOf ViewingRectangle_Changed))

        Private Shared Sub ViewingRectangle_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, ResizableCanvas)
            Self.InvalidateMeasure()
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

        '#Region "MaxViewingRectangle Property"
        '        Public Shared ReadOnly MaxViewingRectangleProperty As DependencyProperty = DependencyProperty.Register("MaxViewingRectangle", GetType(Rect?), GetType(ResizableCanvas), New PropertyMetadata(New Rect?(), AddressOf MaxViewingRectangle_Changed))

        '        Private Shared Sub MaxViewingRectangle_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
        '            Dim Self = DirectCast(D, ResizableCanvas)

        '            Dim OldValue = DirectCast(E.OldValue, Rect?)
        '            Dim NewValue = DirectCast(E.NewValue, Rect?)
        '        End Sub

        '        Public Property MaxViewingRectangle As Rect?
        '            Get
        '                Return DirectCast(Me.GetValue(MaxViewingRectangleProperty), Rect?)
        '            End Get
        '            Set(ByVal value As Rect?)
        '                Me.SetValue(MaxViewingRectangleProperty, value)
        '            End Set
        '        End Property
        '#End Region

#Region "KeepAspectRatio Property"
        Public Shared ReadOnly KeepAspectRatioProperty As DependencyProperty = DependencyProperty.Register("KeepAspectRatio", GetType(Boolean), GetType(ResizableCanvas), New PropertyMetadata(True, AddressOf KeepAspectRatio_Changed))

        Private Shared Sub KeepAspectRatio_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, ResizableCanvas)
            Self.InvalidateMeasure()
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
            Self.InvalidateMeasure()
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
                C.InvalidateMeasure()
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

        Private Shared Sub X_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim C = TryCast(VisualTreeHelper.GetParent(D), ResizableCanvas)
            If C IsNot Nothing Then
                C.InvalidateMeasure()
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
                C.InvalidateMeasure()
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
                C.InvalidateMeasure()
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
