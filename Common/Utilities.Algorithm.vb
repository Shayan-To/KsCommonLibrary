Imports System.Runtime.CompilerServices
Imports System.Text
Imports Media = System.Windows.Media
Imports Reflect = System.Reflection
Imports SIO = System.IO

Namespace Common

    Partial Class Utilities

        Public Class Algorithm

            ''' <param name="Func">N must exist that N >= 0, and Func(I) = True if and only if I >= N.</param>
            ''' <param name="MaxX">Func(MaxIndex) must equal True.</param>
            ''' <returns>Index of first true.</returns>
            Public Shared Function BinarySearch(ByVal Func As Func(Of Integer, Boolean), Optional ByVal MaxX As Integer = -1) As Integer
                If MaxX = -1 Then
                    MaxX = 8
                    Do Until Func.Invoke(MaxX)
                        MaxX <<= 1
                    Loop
                Else
                    Verify.True(Func.Invoke(MaxX))
                    MaxX = Math.LeastPowerOfTwoOnMin(MaxX)
                End If

                Dim X = -1
                Do While MaxX > 1
                    MaxX >>= 1
                    If Not Func.Invoke(X + MaxX) Then
                        X += MaxX
                    End If
                Loop

                Return X + 1
            End Function

            ''' <param name="Func">N must exist that N >= 0, and Func(I) = True if and only if I >= N.</param>
            ''' <param name="End">Func(End) must equal True.</param>
            ''' <returns>Index of first true.</returns>
            Public Shared Function BinarySearch(ByVal Func As Func(Of Integer, Boolean), ByVal Foreward As Boolean, ByVal Start As Integer, Optional ByVal [End] As Integer? = Nothing) As Integer
                If Foreward Then
                    Dim R = BinarySearch(Function(I) Func(I + Start), If([End] - Start, -1))
                    Return R + Start
                Else
                    Dim R = BinarySearch(Function(I) Func(Start - I), If(Start - [End], -1))
                    Return Start - R
                End If
            End Function

            ''' <param name="Func">N must exist that N >= 0, and Func(I) = True if and only if I >= N.</param>
            ''' <param name="End">Func(End) must equal True. Func will never be called on End.</param>
            ''' <returns>Index of first true.</returns>
            Public Shared Function BinarySearchIn(ByVal Func As Func(Of Integer, Boolean), ByVal Start As Integer, ByVal [End] As Integer) As Integer
                If Start <= [End] Then
                    Dim R = BinarySearch(Function(I) If(I + Start >= [End], True, Func(I + Start)), [End] - Start)
                    Return R + Start
                Else
                    Dim R = BinarySearch(Function(I) If(Start - I <= [End], True, Func(Start - I)), Start - [End])
                    Return Start - R
                End If
            End Function

            ''' <param name="Func">N must exist that N >= 0, and Func(I) = True if and only if I >= N.</param>
            ''' <param name="MaxX">Func(MaxIndex) must equal True.</param>
            ''' <returns>Some X that Func(X) = True and |X - N| &lt; MaxError (N is from doc of Func).</returns>
            Public Shared Function BinarySearch(ByVal Func As Func(Of Double, Boolean), ByVal MaxError As Double, Optional ByVal MaxX As Double = Double.NaN) As Double
                Verify.True(MaxError > 0)
                If Double.IsNaN(MaxX) Then
                    MaxX = 8 * MaxError
                    Do Until Func.Invoke(MaxX)
                        MaxX *= 2
                    Loop
                Else
                    Verify.True(Func.Invoke(MaxX))
                End If

                Dim X = 0.0
                Do While MaxX > MaxError
                    MaxX /= 2
                    If Not Func.Invoke(X + MaxX) Then
                        X += MaxX
                    End If
                Loop

                Return X + MaxError
            End Function

            ''' <summary>
            ''' Returns a list of (I, J) where List1[I] = List2[J] and the list is [one of] the longest possible list[s].
            ''' </summary>
            Public Shared Function GetLongestCommonSubsequence(Of T)(ByVal List1 As IReadOnlyList(Of T), ByVal List2 As IReadOnlyList(Of T)) As IReadOnlyList(Of (Index1 As Integer, Index2 As Integer))
                Return GetLongestCommonSubsequence(List1, List2, EqualityComparer(Of T).Default)
            End Function

            ''' <summary>
            ''' Returns a list of (I, J) where List1[I] = List2[J] and the list is [one of] the longest possible list[s].
            ''' </summary>
            Public Shared Function GetLongestCommonSubsequence(Of T)(ByVal List1 As IReadOnlyList(Of T), ByVal List2 As IReadOnlyList(Of T), ByVal Comparer As IEqualityComparer(Of T)) As IReadOnlyList(Of (Index1 As Integer, Index2 As Integer))
                Return GetLongestCommonSubsequence(List1, List2, Function(A, B) If(Comparer.Equals(A, B), 1, 0))
            End Function

            ''' <summary>
            ''' Returns a list of (I, J) where List1[I] = List2[J] and the list is [one of] the longest possible list[s].
            ''' </summary>
            Public Shared Function GetLongestCommonSubsequence(Of T)(ByVal List1 As IReadOnlyList(Of T), ByVal List2 As IReadOnlyList(Of T), ByVal ValueFunction As Func(Of T, T, Integer)) As IReadOnlyList(Of (Index1 As Integer, Index2 As Integer))
                ' We use dynamic programming.

                ' The value of the most valuable common subsequence of List1[0..m] and List2[0..n] is max of:
                ' - ValueFunction(List1[m], List2[n]) + The value of the most valuable common subsequence of List1[0..(m - 1)] and List2[0..(n - 1)].
                ' - The value of the most valuable common subsequence of List1[0..m] and List2[0..(n - 1)].
                ' - The value of the most valuable common subsequence of List1[0..(m - 1)] and List2[0..n].

                ' We do it from the other end so that we can have the result without reversing it.

                Dim M = List1.Count
                Dim N = List2.Count

                ' The tuple is (Length, Mode). See below.
                Dim Dyn = New(Integer, Integer)(M - 1, N - 1) {}

                ' Mode:
                ' 1 -> Did equal?
                ' 2 -> First index has +1?
                ' 4 -> Second index has +1?

                For I = M - 1 To 0 Step -1
                    For J = N - 1 To 0 Step -1
                        Dim Length = ValueFunction.Invoke(List1.Item(I), List2.Item(J))
                        Dim Mode = 1

                        If I <> M - 1 And J <> N - 1 Then
                            Length += Dyn(I + 1, J + 1).Item1
                            Mode += 2 + 4
                        End If

                        If I <> M - 1 Then
                            Dim L = Dyn(I + 1, J).Item1
                            If L > Length Then
                                Length = L
                                Mode = 2
                            End If
                        End If

                        If J <> N - 1 Then
                            Dim L = Dyn(I, J + 1).Item1
                            If L > Length Then
                                Length = L
                                Mode = 4
                            End If
                        End If

                        Dyn(I, J) = (Length, Mode)
                    Next
                Next

                Dim Res = New List(Of (Integer, Integer))()
                Do ' Block for variable scopes
                    Dim I = 0
                    Dim J = 0
                    Dim Cur As (Integer, Integer)
                    Do
                        Cur = Dyn(I, J)
                        If (Cur.Item2 And 1) = 1 Then
                            Res.Add((I, J))
                        End If
                        If (Cur.Item2 And 2) = 2 Then
                            I += 1
                        End If
                        If (Cur.Item2 And 4) = 4 Then
                            J += 1
                        End If

                        Assert.True(((Cur.Item2 And (2 + 4)) = 2 + 4).Implies((Cur.Item2 And 1) = 1))
                    Loop Until (Cur.Item2 And (2 + 4)) = 0

                    Exit Do
                Loop

                Return Res.AsReadOnly()
            End Function

        End Class

    End Class

End Namespace
