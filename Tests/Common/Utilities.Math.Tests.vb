﻿Namespace Common

    Partial Class Utilities_Tests

        Public Class Math

            <[Property]()>
            Public Sub SquareRootConsistencyCheck(ByVal N As Integer)
                If N < 0 Then
                    Assert.Throws(Of ArgumentException)(Sub() Utilities.Math.SquareRoot(N))
                    Exit Sub
                End If

                Dim T = Utilities.Math.SquareRoot(N)
                Assert.Equal(N, T.Root * T.Root + T.Reminder)
                Assert.False(T.Root < 0, "Root must be non-negative.")
                Assert.False(T.Reminder < 0, "Root must be non-negative.")
                Assert.True(T.Reminder < (T.Root + 1) * (T.Root + 1) - T.Root * T.Root, "Reminder is too large.")
            End Sub

            <[Property]()>
            Public Sub SquareRootLConsistencyCheck(ByVal N As Long)
                If N < 0 Then
                    Assert.Throws(Of ArgumentException)(Sub() Utilities.Math.SquareRootL(N))
                    Exit Sub
                End If

                Dim T = Utilities.Math.SquareRootL(N)
                Assert.Equal(N, T.Root * T.Root + T.Reminder)
                Assert.False(T.Root < 0, "Root must be non-negative.")
                Assert.False(T.Reminder < 0, "Root must be non-negative.")
                Assert.True(T.Reminder < (T.Root + 1) * (T.Root + 1) - T.Root * T.Root, "Reminder is too large.")
            End Sub

        End Class

    End Class

End Namespace
