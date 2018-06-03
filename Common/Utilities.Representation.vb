﻿Imports System.Runtime.CompilerServices
Imports System.Text
Imports Media = System.Windows.Media
Imports Reflect = System.Reflection
Imports SIO = System.IO

Namespace Common

    Partial Class Utilities

        Public Class Representation

            Public Shared ReadOnly BinaryPrefixes As IReadOnlyList(Of (Prefix As String, Multiplier As Integer, AcceptInterval As Interval)) =
                {("", 1024, New Interval(0, True, 1000, False)),
                 ("K", 1024, New Interval(0.9, True, 1000, False)),
                 ("M", 1024, New Interval(0.9, True, 1000, False)),
                 ("G", 1024, New Interval(0.9, True, 1000, False)),
                 ("T", 1024, New Interval(0.9, True, 1000, False)),
                 ("P", 1024, New Interval(0.9, True, 1000, False)),
                 ("E", 1024, New Interval(0.9, True, 1000, False)),
                 ("Z", 1024, New Interval(0.9, True, Double.PositiveInfinity, True))}.AsReadOnly()

            Public Shared ReadOnly MetricPrefixes As IReadOnlyList(Of (Prefix As String, Multiplier As Integer, AcceptInterval As Interval)) =
                {("a", 1000, New Interval(0, True, 1000, False)),
                 ("f", 1000, New Interval(1, True, 1000, False)),
                 ("p", 1000, New Interval(1, True, 1000, False)),
                 ("n", 1000, New Interval(1, True, 1000, False)),
                 ("μ", 1000, New Interval(1, True, 1000, False)),
                 ("m", 1000, New Interval(1, True, 10, False)),
                 ("c", 10, New Interval(1, True, 10, False)),
                 ("d", 10, New Interval(1, True, 10, False)),
                 ("", 10, New Interval(1, True, 10, False)),
                 ("da", 10, New Interval(1, True, 10, False)),
                 ("h", 10, New Interval(1, True, 10, False)),
                 ("k", 10, New Interval(1, True, 1000, False)),
                 ("M", 1000, New Interval(1, True, 1000, False)),
                 ("G", 1000, New Interval(1, True, 1000, False)),
                 ("T", 1000, New Interval(1, True, 1000, False)),
                 ("P", 1000, New Interval(1, True, 1000, False)),
                 ("E", 1000, New Interval(1, True, Double.PositiveInfinity, True))}.AsReadOnly()

            Public Shared Function GetPrefixedRepresentation(ByVal Value As Double,
                                                             ByVal Prefixes As IReadOnlyList(Of (Prefix As String, Multiplier As Integer, AcceptInterval As Interval))) As (Value As Double, Prefix As String)
                Dim I = 0
                For I = 0 To Prefixes.Count - 1
                    If Prefixes.Item(I).Prefix = "" Then
                        Exit For
                    End If
                Next

                Do
                    Dim Prefix = Prefixes.Item(I)
                    Dim C = Prefix.AcceptInterval.Compare(System.Math.Abs(Value))
                    If C < 0 Then
                        I -= 1
                        Value *= Prefix.Multiplier
                    ElseIf C > 0 Then
                        I += 1
                        Prefix = Prefixes.Item(I)
                        Value /= Prefix.Multiplier
                    Else
                        Exit Do
                    End If
                Loop

                Return (Value, Prefixes.Item(I).Prefix)
            End Function

            Public Shared Function GetFriendlyTimeSpan(ByVal Time As TimeSpan, ByVal MaxError As TimeSpan) As String
                Dim Units = {("ms", TimeSpan.FromMilliseconds(1)),
                             ("s", TimeSpan.FromSeconds(1)),
                             ("min", TimeSpan.FromMinutes(1)),
                             ("h", TimeSpan.FromHours(1)),
                             ("d", TimeSpan.FromDays(1))}
                ' We want to be able to show values using a max error value, so we have to remove the unnecessary units.
                ' So we will keep the units that are greater than or equal to MaxError.
                ' But these are not enough, as they may not show the value with needed precision if no unit is equal to MaxError.
                ' We will use in that case A'shari numbers. See below.

                ' We want the units that are greater than or equal to Error.
                ' And if the last unit is less that Error, we have no choice but to use it alone. So Func is true for the last unit.
                Dim Start = Algorithm.BinarySearchIn(Function(X) Units(X).Item2 >= MaxError, 0, Units.Length - 1)
                ' The Units with false should not be used.
                ' And from the remaining ones, we will use at most Count of them.
                Dim Count = 2

                Dim Res = New StringBuilder()
                Dim Ticks = Time.Ticks
                ' We will start from the greatest unit, and pick at most Count of them with non-zero Value.
                ' ToDo If ticks is negative, the greatest unit will always be non-zero, even if not necessary. (-1 days 23 hours, instead of -1 hours)
                Dim I = 0
                For I = Units.Length - 1 To Start + 1 Step -1
                    Dim Unit = Units(I)
                    Dim UnitTicks = Unit.Item2.Ticks
                    Dim Value = Math.FloorDiv(Ticks, UnitTicks)
                    Ticks -= Value * UnitTicks

                    If Value <> 0 Then
                        If Count = 1 Then
                            Exit For
                        End If
                        If Res.Length <> 0 Then
                            Res.Append(" ")
                        End If
                        Res.Append(Value).Append(Unit.Item1.ToLowerInvariant())
                        Count -= 1
                    End If
                Next

                ' A block for limiting the scope of variables.
                Do
                    Dim Unit = Units(I)
                    Dim UnitTicks = Unit.Item2.Ticks

                    ' Just like the units, we have to check whether (0.01 U) is a good unit or not. (Division will move us upwards in the list.)
                    ' We will find units less than or equal to MaxError (the bad ones + equals), and choose the first of them (plus the good ones of course).
                    '
                    ' And we cannot optimize it by assuming we can do any number of digits when not at Start.
                    ' A unit is something about 20-100 times smaller than the previous one, so maybe we are forced to use only one digit.
                    Dim Prec = 100
                    Do While UnitTicks \ Prec <= MaxError.Ticks
                        Prec \= 10
                        If Prec = 0 Then
                            ' We will end up here only if the unit equals the error.
                            Exit Do
                        End If
                    Loop
                    Prec = If(Prec = 0, 1, Prec * 10)
                    If Prec > 100 Then
                        Prec = 100
                    End If

                    ' We assume (Unit / Prec) to be another unit, but print the result divided by Prec.
                    Dim Value = System.Math.Round(Ticks / (UnitTicks \ Prec))
                    If Value <> 0 Or Res.Length = 0 Then
                        If Res.Length <> 0 Then
                            Res.Append(" ")
                        End If
                        Res.Append(Value / Prec).Append(Unit.Item1.ToLowerInvariant())
                    End If

                    Exit Do
                Loop

                Return Res.ToString()
            End Function

            Public Shared Function GetTimeStamp(Optional ByVal Time As DateTime? = Nothing, Optional ByVal Compact As Boolean = False) As String
                Static Builder As StringBuilder = New StringBuilder()

                Dim Now As DateTime
                If Time.HasValue Then
                    Now = Time.Value
                Else
                    Now = DateTime.Now
                End If

                If Compact Then
                    Builder.Clear() _
                      .Append(Now.Year.ToStringInv().PadLeft(4, "0"c)) _
                      .Append(Now.Month.ToStringInv().PadLeft(2, "0"c)) _
                      .Append(Now.Day.ToStringInv().PadLeft(2, "0"c)) _
                      .Append(Now.Hour.ToStringInv().PadLeft(2, "0"c)) _
                      .Append(Now.Minute.ToStringInv().PadLeft(2, "0"c)) _
                      .Append(Now.Second.ToStringInv().PadLeft(2, "0"c)) _
                      .Append(Now.Millisecond.ToStringInv().PadLeft(3, "0"c))
                Else
                    Builder.Clear() _
                      .Append(Now.Year.ToStringInv().PadLeft(4, "0"c)) _
                      .Append("-"c) _
                      .Append(Now.Month.ToStringInv().PadLeft(2, "0"c)) _
                      .Append("-"c) _
                      .Append(Now.Day.ToStringInv().PadLeft(2, "0"c)) _
                      .Append(" "c) _
                      .Append(Now.Hour.ToStringInv().PadLeft(2, "0"c)) _
                      .Append(":"c) _
                      .Append(Now.Minute.ToStringInv().PadLeft(2, "0"c)) _
                      .Append(":"c) _
                      .Append(Now.Second.ToStringInv().PadLeft(2, "0"c)) _
                      .Append("."c) _
                      .Append(Now.Millisecond.ToStringInv().PadLeft(3, "0"c))
                End If

                Dim Stamp = Builder.ToString()
                Return Stamp
            End Function

            Public Shared Function Hour24To12(ByVal Hour As Integer) As (Hour As Integer, IsPm As Boolean)
                Dim IsPm = False

                If Hour >= 12 Then
                    Hour -= 12
                    IsPm = True
                End If
                If Hour = 0 Then
                    Hour = 12
                End If

                Return (Hour, IsPm)
            End Function

            Public Shared Function Hour12To24(ByVal Hour As Integer, ByVal IsPm As Boolean) As Integer
                If Hour = 12 Then
                    Hour = 0
                End If
                If IsPm Then
                    Hour += 12
                End If

                Return Hour
            End Function

        End Class

    End Class

End Namespace
