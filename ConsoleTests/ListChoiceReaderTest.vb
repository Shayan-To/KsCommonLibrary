Public Class ListChoiceReaderTest

    <InteractiveRunnable(True)>
    Public Shared Sub Start()
        Dim Rand = New Random()
        Dim L = Utilities.Collections.Range(45).Select(Of Integer?)(Function(I) Rand.Next())
        Dim ChoiceReader = New ConsoleListChoiceReader(Of Integer?)(L)

        Do
            Dim I = ChoiceReader.ReadChoice()
            If Not I.HasValue Then
                Exit Do
            End If
            Console.WriteLine($"{I} was chosen!!")
        Loop
    End Sub

End Class
