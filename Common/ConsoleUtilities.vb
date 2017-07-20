Namespace Common

    Public Class ConsoleUtilities

        Private Sub New()
            Throw New NotSupportedException
        End Sub

        Public Shared Sub Initialize()
            Console.ForegroundColor = ConsoleColor.Black
            Console.BackgroundColor = ConsoleColor.White
            Console.Clear()
        End Sub

        Public Shared Sub WriteColored(ByVal Value As String, ByVal Color As ConsoleColor)
            Dim PrevColor = Console.BackgroundColor
            Console.BackgroundColor = Color
            Console.Write(Value)
            Console.BackgroundColor = PrevColor
        End Sub

        Public Shared Function ReadYesNo(ByVal Prompt As String) As Boolean
            WriteColored(Prompt, ConsoleColor.Green)

            Dim K As ConsoleKey
            Do
                K = Console.ReadKey(True).Key
            Loop Until K = ConsoleKey.Y Or K = ConsoleKey.N

            Dim Res = K = ConsoleKey.Y

            Console.WriteLine(If(Res, " Y", " N"))

            Return Res
        End Function

        Public Shared Sub WriteExceptionData(ByVal Ex As Exception)
            Do
                WriteColored("Exception: --------------------------------------------------", ConsoleColor.Yellow)
                Console.WriteLine()
                Console.WriteLine("Type: " & Ex.GetType().FullName)
                Console.WriteLine("Message: " & Ex.Message)
                Console.WriteLine("StackTrace: " & Ex.StackTrace)
                Console.WriteLine()
                Ex = Ex.InnerException
            Loop Until Ex Is Nothing
        End Sub

        Public Shared Sub WriteTypes(ByVal Types As IEnumerable(Of Type))
            For Each T In Types
                Console.WriteLine(T.FullName & ControlChars.Tab & T.Attributes.ToString())
            Next
        End Sub

        Public Shared Sub Pause()
            If Environment.UserInteractive Then
                WriteColored("Press any key to continue...", ConsoleColor.Green)
                Console.ReadKey(True)
                Console.WriteLine()
            End If
        End Sub

    End Class

End Namespace
