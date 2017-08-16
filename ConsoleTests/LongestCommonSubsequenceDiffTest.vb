Public Class LongestCommonSubsequenceTest

    <InteractiveRunnable(True)>
    Public Shared Sub Start()
        Dim Path1 = Console.ReadLine()
        Dim Path2 = Console.ReadLine()

        Dim File1 = IO.File.ReadAllLines(Path1, Text.Encoding.UTF8)
        Dim File2 = IO.File.ReadAllLines(Path2, Text.Encoding.UTF8)
        Dim Comparer = New StringComparer()

        Dim LCS = Utilities.Algorithm.GetLongestCommonSubsequence(File1, File2, Comparer)

        Dim I = 0
        Dim J = 0
        For Each L In LCS.Append((File1.Length, File2.Length))
            For I = I To L.Index1 - 1
                ConsoleUtilities.WriteColored(File1(I), ConsoleColor.Red, ConsoleColor.White)
                Console.WriteLine()
            Next
            For J = J To L.Index2 - 1
                ConsoleUtilities.WriteColored(File2(J), ConsoleColor.Green, ConsoleColor.Black)
                Console.WriteLine()
            Next
            If I = File1.Length Then
                Exit For
            End If
            ' They can be non-equal, as the comparer trims.
            If File1(I) = File2(J) Then
                ConsoleUtilities.WriteColored(File2(J), ConsoleColor.Black, ConsoleColor.White)
            Else
                ConsoleUtilities.WriteColored(File2(J), ConsoleColor.Blue, ConsoleColor.White)
            End If
            Console.WriteLine()
            I += 1
            J += 1
        Next

        Console.WriteLine()
    End Sub

    Public Class StringComparer
        Inherits EqualityComparer(Of String)

        Public Overrides Function Equals(x As String, y As String) As Boolean
            Return x.Trim() = y.Trim() 'x.GetHashCode() = y.GetHashCode()
        End Function

        Public Overrides Function GetHashCode(obj As String) As Integer
            Return obj.Trim().GetHashCode()
        End Function

    End Class

End Class
