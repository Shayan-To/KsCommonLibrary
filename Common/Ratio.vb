Namespace Common

    Public Structure Ratio

        Private Sub New(ByVal Numerator As Integer, ByVal Denumenator As Integer, ByVal NoSimplify As Void)
            Me._Numerator = Numerator
            Me._Denumenator = Denumenator
        End Sub

        Public Sub New(ByVal Numerator As Integer, ByVal Denumenator As Integer)
            Verify.True(Denumenator <> 0, "Division by zero.")
            Dim GCD = Utilities.Math.GreatestCommonDivisor(Numerator, Denumenator)
            If Denumenator < 0 Then
                Numerator = -Numerator
                Denumenator = -Denumenator
            End If
            Me._Numerator = Numerator \ GCD
            Me._Denumenator = Denumenator \ GCD
        End Sub

        Public Sub New(ByVal Num As Integer)
            Me.New(Num, 1, Nothing)
        End Sub

        Public Shared Operator +(ByVal Left As Ratio, ByVal Right As Ratio) As Ratio
            Dim GCD = Utilities.Math.GreatestCommonDivisor(Left.Denumenator, Right.Denumenator)
            If GCD = 0 Then
                If Left = Zero Then
                    Return Right
                Else
                    Return Left
                End If
            End If
            Return New Ratio(Left.Numerator * (Right.Denumenator \ GCD) + Right.Numerator * (Left.Denumenator \ GCD),
                             (Left.Denumenator \ GCD) * Right.Denumenator)
        End Operator

        Public Shared Operator -(ByVal Right As Ratio) As Ratio
            Return New Ratio(-Right.Numerator, Right.Denumenator, Nothing)
        End Operator

        Public Shared Operator -(ByVal Left As Ratio, ByVal Right As Ratio) As Ratio
            Return Left + -Right
        End Operator

        Public Shared Operator *(ByVal Left As Ratio, ByVal Right As Ratio) As Ratio
            Dim GCD1 = Utilities.Math.GreatestCommonDivisor(Left.Numerator, Right.Denumenator)
            Dim GCD2 = Utilities.Math.GreatestCommonDivisor(Left.Denumenator, Right.Numerator)
            If GCD1 = 0 Or GCD2 = 0 Then
                Return Zero
            End If
            Return New Ratio((Left.Numerator \ GCD1) * (Right.Numerator \ GCD2),
                             (Left.Denumenator \ GCD2) * (Right.Denumenator \ GCD1),
                             Nothing)
        End Operator

        Public Shared Operator /(ByVal Left As Ratio, ByVal Right As Ratio) As Ratio
            Verify.False(Right.Numerator = 0, "Division by zero.")
            If Right.Numerator < 0 Then
                Return Left * New Ratio(-Right.Denumenator, -Right.Numerator, Nothing)
            Else
                Return Left * New Ratio(Right.Denumenator, Right.Numerator, Nothing)
            End If
        End Operator

        Public Shared Operator <(ByVal Left As Ratio, ByVal Right As Ratio) As Boolean
            If Left.Denumenator = Zero Or Right.Denumenator = Zero Then
                Return Left.Numerator < Right.Numerator
            End If
            Return Left.Numerator * Right.Denumenator < Right.Numerator * Left.Denumenator
        End Operator

        Public Shared Operator >(ByVal Left As Ratio, ByVal Right As Ratio) As Boolean
            Return Right < Left
        End Operator

        Public Shared Operator <=(ByVal Left As Ratio, ByVal Right As Ratio) As Boolean
            Return Not Right < Left
        End Operator

        Public Shared Operator >=(ByVal Left As Ratio, ByVal Right As Ratio) As Boolean
            Return Not Left < Right
        End Operator

        Public Shared Operator =(ByVal Left As Ratio, ByVal Right As Ratio) As Boolean
            If Left.Denumenator = Zero Or Right.Denumenator = Zero Then
                Return Left.Numerator = Right.Numerator
            End If
            Return Left.Numerator = Right.Numerator And Right.Denumenator = Left.Denumenator
        End Operator

        Public Shared Operator <>(ByVal Left As Ratio, ByVal Right As Ratio) As Boolean
            Return Not Left = Right
        End Operator

        Public Shared Operator +(ByVal Left As Integer, ByVal Right As Ratio) As Ratio
            Return New Ratio(Left) + Right
        End Operator

        Public Shared Operator -(ByVal Left As Integer, ByVal Right As Ratio) As Ratio
            Return New Ratio(Left) - Right
        End Operator

        Public Shared Operator *(ByVal Left As Integer, ByVal Right As Ratio) As Ratio
            Return New Ratio(Left) * Right
        End Operator

        Public Shared Operator /(ByVal Left As Integer, ByVal Right As Ratio) As Ratio
            Return New Ratio(Left) / Right
        End Operator

        Public Shared Operator <(ByVal Left As Integer, ByVal Right As Ratio) As Boolean
            Return New Ratio(Left) < Right
        End Operator

        Public Shared Operator >(ByVal Left As Integer, ByVal Right As Ratio) As Boolean
            Return New Ratio(Left) > Right
        End Operator

        Public Shared Operator <=(ByVal Left As Integer, ByVal Right As Ratio) As Boolean
            Return New Ratio(Left) <= Right
        End Operator

        Public Shared Operator >=(ByVal Left As Integer, ByVal Right As Ratio) As Boolean
            Return New Ratio(Left) >= Right
        End Operator

        Public Shared Operator =(ByVal Left As Integer, ByVal Right As Ratio) As Boolean
            Return New Ratio(Left) = Right
        End Operator

        Public Shared Operator <>(ByVal Left As Integer, ByVal Right As Ratio) As Boolean
            Return New Ratio(Left) <> Right
        End Operator

        Public Shared Operator +(ByVal Left As Ratio, ByVal Right As Integer) As Ratio
            Return Left + New Ratio(Right)
        End Operator

        Public Shared Operator -(ByVal Left As Ratio, ByVal Right As Integer) As Ratio
            Return Left - New Ratio(Right)
        End Operator

        Public Shared Operator *(ByVal Left As Ratio, ByVal Right As Integer) As Ratio
            Return Left * New Ratio(Right)
        End Operator

        Public Shared Operator /(ByVal Left As Ratio, ByVal Right As Integer) As Ratio
            Return Left / New Ratio(Right)
        End Operator

        Public Shared Operator <(ByVal Left As Ratio, ByVal Right As Integer) As Boolean
            Return Left < New Ratio(Right)
        End Operator

        Public Shared Operator >(ByVal Left As Ratio, ByVal Right As Integer) As Boolean
            Return Left > New Ratio(Right)
        End Operator

        Public Shared Operator <=(ByVal Left As Ratio, ByVal Right As Integer) As Boolean
            Return Left <= New Ratio(Right)
        End Operator

        Public Shared Operator >=(ByVal Left As Ratio, ByVal Right As Integer) As Boolean
            Return Left >= New Ratio(Right)
        End Operator

        Public Shared Operator =(ByVal Left As Ratio, ByVal Right As Integer) As Boolean
            Return Left = New Ratio(Right)
        End Operator

        Public Shared Operator <>(ByVal Left As Ratio, ByVal Right As Integer) As Boolean
            Return Left <> New Ratio(Right)
        End Operator

        Public Shared Narrowing Operator CType(ByVal Value As Ratio) As Integer
            Verify.True(Value.Denumenator = 1, "Not an integer.")
            Return Value.Numerator
        End Operator

        Public Shared Narrowing Operator CType(ByVal Value As Ratio) As Double
            Return Value.Numerator / Value.Denumenator
        End Operator

        Public Shared Widening Operator CType(ByVal Value As Integer) As Ratio
            Return New Ratio(Value)
        End Operator

        Public Function Floor() As Integer
            Return Me.Numerator \ Me.Denumenator
        End Function

        Public Overrides Function ToString() As String
            Return Me.Numerator & If(Me.Denumenator <> 1, "/" & Me.Denumenator, "")
        End Function

#Region "Numerator Property"
        Private ReadOnly _Numerator As Integer

        Public ReadOnly Property Numerator As Integer
            Get
                Return Me._Numerator
            End Get
        End Property
#End Region

#Region "Denumenator Property"
        Private ReadOnly _Denumenator As Integer

        Public ReadOnly Property Denumenator As Integer
            Get
                If Me._Denumenator = 0 And Me._Numerator = 0 Then
                    Return 1
                End If
                Return Me._Denumenator
            End Get
        End Property
#End Region

        Public Shared ReadOnly Zero As Ratio

    End Structure

End Namespace
