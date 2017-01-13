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

        Public Shared Sub WriteExceptionData(ByVal Ex As Exception)
            Do
                Console.BackgroundColor = ConsoleColor.Yellow
                Console.WriteLine("Exception: --------------------------------------------------")
                Console.BackgroundColor = ConsoleColor.White
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
                Console.BackgroundColor = ConsoleColor.Green
                Console.Write("Press any key to continue...")
                Console.BackgroundColor = ConsoleColor.White
                Console.ReadKey(True)
                Console.WriteLine()
            End If
        End Sub

    End Class

End Namespace
