Namespace Geometry

    Public Structure Rectangle

        Private Sub New(ByVal Line As Line)
            Me.Line = Line
        End Sub

        Public Shared Function FromCorners(ByVal P1 As Vector, ByVal P2 As Vector) As Rectangle
            Return FromCorners(P1.X, P1.Y, P2.X, P2.Y)
        End Function

        Public Shared Function FromCorners(ByVal X1 As Double, ByVal Y1 As Double, ByVal X2 As Double, ByVal Y2 As Double) As Rectangle
            Dim C As Double

            If X1 > X2 Then
                C = X1
                X1 = X2
                X2 = C
            End If
            If Y1 > Y2 Then
                C = Y1
                Y1 = Y2
                Y2 = C
            End If

            Return New Rectangle(New Line(X1, Y1, X2, Y2))
        End Function

        Public Shared Function FromCornerSize(ByVal TopLeftCorner As Vector, ByVal Size As Vector) As Rectangle
            Return New Rectangle(New Line(TopLeftCorner, TopLeftCorner + Size))
        End Function

        Public Shared Function FromCenterSize(ByVal Center As Vector, ByVal Size As Vector) As Rectangle
            Size = Size / 2
            Return New Rectangle(New Line(Center - Size, Center + Size))
        End Function

        Public Shared Function BoundingRectangle(ByVal ParamArray Rectangles As Rectangle()) As Rectangle
            Return BoundingRectangle(DirectCast(Rectangles, IEnumerable(Of Rectangle)))
        End Function

        Public Shared Function BoundingRectangle(ByVal Rectangles As IEnumerable(Of Rectangle)) As Rectangle
            Dim MinX, MinY, MaxX, MaxY As Double

            Dim Bl = True

            For Each R As Rectangle In Rectangles
                If R.Size = New Vector() Then
                    Continue For
                End If
                If Bl Then
                    MinX = R.Line.P1.X
                    MinY = R.Line.P1.Y
                    MaxX = MinX
                    MaxY = MinY

                    Bl = False
                End If
                If R.Line.P1.X < R.Line.P2.X Then
                    MinX = Math.Min(R.Line.P1.X, MinX)
                    MaxX = Math.Max(R.Line.P2.X, MaxX)
                Else
                    MinX = Math.Min(R.Line.P2.X, MinX)
                    MaxX = Math.Max(R.Line.P1.X, MaxX)
                End If
                If R.Line.P1.Y < R.Line.P2.Y Then
                    MinY = Math.Min(R.Line.P1.Y, MinY)
                    MaxY = Math.Max(R.Line.P2.Y, MaxY)
                Else
                    MinY = Math.Min(R.Line.P2.Y, MinY)
                    MaxY = Math.Max(R.Line.P1.Y, MaxY)
                End If
            Next

            If Bl Then
                Return New Rectangle()
            End If
            Return Rectangle.FromCorners(MinX, MinY, MaxX, MaxY)
        End Function

        Public Function InnerBoundingSquare() As Rectangle
            Dim Size = Me.Size

            If Size.X = Size.Y Then
                Return Me
            End If
            Dim Corner = Me.TopLeftCorner

            If Size.X < Size.Y Then
                Corner.Y += (Size.Y - Size.X) / 2
                Size = New Vector(Size.X, Size.X)
            Else
                Corner.X += (Size.X - Size.Y) / 2
                Size = New Vector(Size.Y, Size.Y)
            End If

            Return Rectangle.FromCornerSize(Corner, Size)
        End Function

        Public Function OuterBoundingSquare() As Rectangle
            Dim Size = Me.Size

            If Size.X = Size.Y Then
                Return Me
            End If
            Dim Corner = Me.TopLeftCorner

            If Size.X > Size.Y Then
                Corner.Y += (Size.Y - Size.X) / 2
                Size = New Vector(Size.X, Size.X)
            Else
                Corner.X += (Size.X - Size.Y) / 2
                Size = New Vector(Size.Y, Size.Y)
            End If

            Return Rectangle.FromCornerSize(Corner, Size)
        End Function

        Public Function IsInside(ByVal P As Vector) As Boolean
            Return Me.Line.P1.X < P.X And P.X < Me.Line.P2.X And
                   Me.Line.P1.Y < P.Y And P.Y < Me.Line.P2.Y
        End Function

        Public Shared Operator +(ByVal A As Rectangle, ByVal B As Vector) As Rectangle
            Return New Rectangle(A.Line + B)
        End Operator

        Public Shared Operator -(ByVal A As Rectangle, ByVal B As Vector) As Rectangle
            Return New Rectangle(A.Line - B)
        End Operator

        Public Overrides Function ToString() As String
            Return String.Concat("Rectangle{(", Me.Line.P1.X, ", ", Me.Line.P1.Y, ")-(",
                                 Me.Line.P2.X, ", ", Me.Line.P2.Y, ")End")
        End Function

        Public ReadOnly Property TopLeftCorner As Vector
            Get
                Return Me.Line.P1
            End Get
        End Property

        Public ReadOnly Property BottomRightCorner As Vector
            Get
                Return Me.Line.P2
            End Get
        End Property

        Public ReadOnly Property TopRightCorner As Vector
            Get
                Return Me.Line.P1 + New Vector(Me.Size.X, 0)
            End Get
        End Property

        Public ReadOnly Property BottomLeftCorner As Vector
            Get
                Return Me.Line.P1 + New Vector(0, Me.Size.Y)
            End Get
        End Property

        Public ReadOnly Property Center As Vector
            Get
                Return (Me.Line.P1 + Me.Line.P2) / 2
            End Get
        End Property

        Public ReadOnly Property Size As Vector
            Get
                Return Me.Line.P2 - Me.Line.P1
            End Get
        End Property

        Private ReadOnly Line As Line

    End Structure

End Namespace
