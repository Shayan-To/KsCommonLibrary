Namespace Geometry

    Public Structure Line

        Public Sub New(ByVal X1 As Double, ByVal Y1 As Double, ByVal X2 As Double, ByVal Y2 As Double)
            Me._P1 = New Vector(X1, Y1)
            Me._P2 = New Vector(X2, Y2)
        End Sub

        Public Sub New(ByVal P1 As Vector, ByVal P2 As Vector)
            Me._P1 = P1
            Me._P2 = P2
        End Sub

        Public Shared Operator +(ByVal A As Line, ByVal B As Vector) As Line
            Return New Line(A.P1 + B, A.P2 + B)
        End Operator

        Public Shared Operator -(ByVal A As Line, ByVal B As Vector) As Line
            Return New Line(A._P1 - B, A._P2 - B)
        End Operator

        Public Function BoundingRectangle() As Rectangle
            Return Rectangle.FromCorners(Me._P1, Me._P2)
        End Function

        Public Overrides Function ToString() As String
            Return String.Concat("Line{(", Me._P1.X, ", ", Me._P1.Y, ")-(", Me._P2.X, ", ", Me._P2.Y, ")End")
        End Function

#Region "P1 Property"
        Private ReadOnly _P1 As Vector
        Public ReadOnly Property P1 As Vector
            Get
                Return Me._P1
            End Get
        End Property
#End Region

#Region "P2 Property"
        Private ReadOnly _P2 As Vector
        Public ReadOnly Property P2 As Vector
            Get
                Return Me._P2
            End Get
        End Property
#End Region

    End Structure

End Namespace
