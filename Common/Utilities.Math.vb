Imports System.Runtime.CompilerServices
Imports System.Text
Imports Media = System.Windows.Media
Imports Reflect = System.Reflection
Imports SIO = System.IO

Namespace Common

    Partial Class Utilities

        Public Class Math

            Private Sub New()
                Throw New NotSupportedException()
            End Sub

#Region "ModAdditions Logic"
            ''' <summary>
            ''' Calculates the non-negative reminder of the two numbers specified.
            ''' </summary>
            ''' <param name="A">The dividend</param>
            ''' <param name="B">The divisor</param>
            ''' <returns>
            ''' The reminder, R, of A divided by B. If it is a negative number, A + Abs(B) will be returned.
            ''' The result is always between 0 and B - 1.
            ''' </returns>
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Public Shared Function NonNegMod(ByVal A As Integer, ByVal B As Integer) As Integer
                A = A Mod B
                If A >= 0 Then
                    Return A
                End If
                If B > 0 Then
                    Return A + B
                End If
                Return A - B
            End Function

            ''' <summary>
            ''' Calculates the positive reminder of the two numbers specified.
            ''' </summary>
            ''' <param name="A">The dividend</param>
            ''' <param name="B">The divisor</param>
            ''' <returns>
            ''' The reminder, R, of A divided by B. If it is not a positive number, A + Abs(B) will be returned.
            ''' The result is always between 1 and B.
            ''' </returns>
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Public Shared Function PosMod(ByVal A As Integer, ByVal B As Integer) As Integer
                A = A Mod B
                If A > 0 Then
                    Return A
                End If
                If B > 0 Then
                    Return A + B
                End If
                Return A - B
            End Function
#End Region

            Public Shared Function Power(ByVal A As Integer, ByVal B As Integer) As Integer
                Dim R = 1
                Do Until B = 0
                    If (B And 1) = 1 Then
                        R *= A
                    End If
                    A *= A
                    B >>= 1
                Loop
                Return R
            End Function

            Public Shared Function PowerL(ByVal A As Long, ByVal B As Long) As Long
                Dim R = 1L
                Do Until B = 0
                    If (B And 1) = 1 Then
                        R *= A
                    End If
                    A *= A
                    B >>= 1
                Loop
                Return R
            End Function

            Public Shared Function SquareRoot(ByVal A As Integer) As Integer
                Dim ARev = 0
                Dim T = A
                Do Until T = 0
                    ARev = (ARev << 2) Or (T And 3)
                    T >>= 2
                Loop

                Dim Reminder = 0
                Dim Root = 0
                Do Until A = 0
                    Reminder = (Reminder << 2) Or (ARev And 3)

                    ARev >>= 2
                    A >>= 2

                    Root <<= 1
                    Dim Root2 = (Root << 1) Or 1

                    If Reminder >= Root2 Then
                        Root = Root Or 1
                        Reminder -= Root2
                    End If
                Loop

                Return Root
            End Function

            Public Shared Function SquareRootL(ByVal A As Long) As Long
                Dim ARev = 0L
                Dim T = A
                Do Until T = 0
                    ARev = (ARev << 2) Or (T And 3)
                    T >>= 2
                Loop

                Dim Reminder = 0L
                Dim Root = 0L
                Do Until A = 0
                    Reminder = (Reminder << 2) Or (ARev And 3)

                    ARev >>= 2
                    A >>= 2

                    Root <<= 1
                    Dim Root2 = (Root << 1) Or 1

                    If Reminder >= Root2 Then
                        Root = Root Or 1
                        Reminder -= Root2
                    End If
                Loop

                Return Root
            End Function

            Public Shared Function LeastPowerOfTwoOnMin(ByVal Min As Integer) As Integer
                If Min < 1 Then
                    Return 1
                End If

                ' If Min is a power of two, we should return Min, otherwise, Min * 2
                Dim T = (Min - 1) And Min
                If T = 0 Then
                    Return Min
                End If
                Min = T

                Do
                    T = (Min - 1) And Min
                    If T = 0 Then
                        Return Min << 1
                    End If
                    Min = T
                Loop
            End Function

            Public Shared Function LeastPowerOfTwoOnMin(ByVal Min As Long) As Long
                If Min < 1 Then
                    Return 1
                End If

                ' If Min is a power of two, we should return Min, otherwise, Min * 2
                Dim T = (Min - 1) And Min
                If T = 0 Then
                    Return Min
                End If
                Min = T

                Do
                    T = (Min - 1) And Min
                    If T = 0 Then
                        Return Min << 1
                    End If
                    Min = T
                Loop
            End Function

            Public Shared Function FloorDiv(ByVal A As Integer, ByVal B As Integer) As Integer
                If B < 0 Then
                    A = -A
                    B = -B
                End If
                If A >= 0 Or A Mod B = 0 Then
                    Return A \ B
                End If
                Return A \ B - 1
            End Function

            Public Shared Function FloorDiv(ByVal A As Long, ByVal B As Long) As Long
                If B < 0 Then
                    A = -A
                    B = -B
                End If
                If A >= 0 Or A Mod B = 0 Then
                    Return A \ B
                End If
                Return A \ B - 1
            End Function

            Public Shared Function CeilDiv(ByVal A As Integer, ByVal B As Integer) As Integer
                If B < 0 Then
                    A = -A
                    B = -B
                End If
                If A < 0 Or A Mod B = 0 Then
                    Return A \ B
                End If
                Return A \ B + 1
            End Function

            Public Shared Function CeilDiv(ByVal A As Long, ByVal B As Long) As Long
                If B < 0 Then
                    A = -A
                    B = -B
                End If
                If A < 0 Or A Mod B = 0 Then
                    Return A \ B
                End If
                Return A \ B + 1
            End Function

            Public Shared Function GreatestCommonDivisor(ByVal A As Integer, ByVal B As Integer) As Integer
                If B < 0 Then
                    B = -B
                End If
                If A < 0 Then
                    A = -A
                End If
                Do Until B = 0
                    Dim C = A Mod B
                    A = B
                    B = C
                Loop
                Return A
            End Function

            Public Shared Function GreatestCommonDivisor(ByVal A As Long, ByVal B As Long) As Long
                If B < 0 Then
                    B = -B
                End If
                If A < 0 Then
                    A = -A
                End If
                Do Until B = 0
                    Dim C = A Mod B
                    A = B
                    B = C
                Loop
                Return A
            End Function

            Public Shared Function LeastCommonMultiple(ByVal A As Integer, ByVal B As Integer) As Integer
                Return (A \ GreatestCommonDivisor(A, B)) * B
            End Function

            Public Shared Function LeastCommonMultiple(ByVal A As Long, ByVal B As Long) As Long
                Return (A \ GreatestCommonDivisor(A, B)) * B
            End Function

            Public Shared Function LogarithmIntegral(ByVal N As Long, ByVal Base As Long) As (Log As Integer, Reminder As Long)
                Dim Reminder = 0L
                Dim Power = 1L
                Dim Log = 0
                Do Until N = 0
                    Reminder += (N Mod Base) * Power
                    N = N \ Base
                    Power *= Base
                    Log += 1
                Loop
                Return (Log, Reminder)
            End Function

            Public Shared Function IsOfIntegralType(O As Object) As Boolean
                Return TypeOf O Is Byte Or TypeOf O Is UShort Or TypeOf O Is UInteger Or TypeOf O Is ULong Or
                       TypeOf O Is SByte Or TypeOf O Is Short Or TypeOf O Is Integer Or TypeOf O Is Long Or
                       TypeOf O Is Single Or TypeOf O Is Double
            End Function

            Public Shared Function ConvertToBase(ByVal N As Long, ByVal Digits As Char(), Optional ByVal NegativeSign As Char = "-"c) As String
                Dim IsNegative = N < 0
                If IsNegative Then
                    N = -N
                End If

                Dim Res = New List(Of Char)()
                Dim Base = Digits.Length

                Do Until N = 0
                    Res.Add(Digits(CInt(N Mod Base)))
                    N \= Base
                Loop

                If IsNegative Then
                    Res.Add(NegativeSign)
                End If

                Res.Reverse()

                Return New String(Res.ToArray())
            End Function

            Public Shared Function ConvertFromBase(ByVal N As String, ByVal Digits As Char(), Optional ByVal NegativeSign As Char = "-"c) As Long
                Dim I = 0
                Dim IsNegative = N.Chars(I) = NegativeSign
                If IsNegative Then
                    I += 1
                End If

                Dim Res = 0L
                Dim Base = Digits.Length

                For I = I To N.Length - 1
                    Dim T = Array.IndexOf(Digits, N.Chars(I))
                    Verify.False(T = -1, $"Invalid digit at index {I}.")
                    Res = Res * Base + T
                Next

                If IsNegative Then
                    Res = -Res
                End If

                Return Res
            End Function

            Public Shared Function ConvertToBaseU(ByVal N As ULong, ByVal Digits As Char()) As String
                Dim Res = New List(Of Char)()
                Dim Base = CUInt(Digits.Length)

                Do Until N = 0
                    Res.Add(Digits(CInt(N Mod Base)))
                    N \= Base
                Loop

                Res.Reverse()

                Return New String(Res.ToArray())
            End Function

            Public Shared Function ConvertFromBaseU(ByVal N As String, ByVal Digits As Char()) As ULong
                Dim I = 0

                Dim Res = 0UL
                Dim Base = CUInt(Digits.Length)

                For I = I To N.Length - 1
                    Dim T = Array.IndexOf(Digits, N.Chars(I))
                    Verify.False(T = -1, $"Invalid digit at index {I}.")
                    Res = Res * Base + CUInt(T)
                Next

                Return Res
            End Function

            Private Shared ReadOnly Digits As Char()() =
                (Function()
                     Dim D = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                     Return Collections.Range(2, D.Length - 2).Select(Function(I) D.Substring(0, I).ToCharArray()).ToArray()
                 End Function).Invoke()

            Public Shared Function ConvertToBase(ByVal N As Long, ByVal Base As Integer) As String
                Return ConvertToBase(N, Digits(Base))
            End Function

            Public Shared Function ConvertFromBase(ByVal N As String, ByVal Base As Integer) As Long
                Return ConvertFromBase(N, Digits(Base))
            End Function

            Public Shared Function ConvertToBaseU(ByVal N As ULong, ByVal Base As Integer) As String
                Return ConvertToBaseU(N, Digits(Base))
            End Function

            Public Shared Function ConvertFromBaseU(ByVal N As String, ByVal Base As Integer) As ULong
                Return ConvertFromBaseU(N, Digits(Base))
            End Function

        End Class

    End Class

End Namespace
