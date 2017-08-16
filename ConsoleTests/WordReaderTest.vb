Public Class WordReaderTest

    <InteractiveRunnable(True)>
    Public Shared Sub Start()
        Dim Reader = New WordReader(Console.In)

        Do
            Dim W = Reader.ReadWord()
            Console.WriteLine(W)
        Loop
    End Sub

End Class
