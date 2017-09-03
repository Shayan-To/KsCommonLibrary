Namespace Common

    Public Class ConsoleUtilities

        Private Sub New()
            Throw New NotSupportedException
        End Sub

        Public Const DefaultBackColor As ConsoleColor = ConsoleColor.White
        Public Const DefaultForeColor As ConsoleColor = ConsoleColor.Black

        Public Shared Sub Initialize(Optional ByVal SetInvariantCulture As Boolean = True)
            If SetInvariantCulture Then
                Threading.Thread.CurrentThread.CurrentCulture = Globalization.CultureInfo.InvariantCulture
                Threading.Thread.CurrentThread.CurrentUICulture = Globalization.CultureInfo.InvariantCulture
            End If

            Console.ForegroundColor = DefaultForeColor
            Console.BackgroundColor = DefaultBackColor
            Console.Clear()
        End Sub

        Public Shared Sub WriteColored(ByVal Value As String, Optional ByVal BackColor As ConsoleColor = DefaultBackColor, Optional ByVal ForeColor As ConsoleColor = DefaultForeColor)
            Dim PBackColor = Console.BackgroundColor
            Dim PForeColor = Console.ForegroundColor
            Console.BackgroundColor = BackColor
            Console.ForegroundColor = ForeColor
            Console.Write(Value)
            Console.BackgroundColor = PBackColor
            Console.ForegroundColor = PForeColor
        End Sub

        Public Shared Function ReadYesNo(ByVal Prompt As String) As Boolean
            WriteColored(Prompt, ConsoleColor.Green)

            Dim K As ConsoleKey
            Do
                K = Console.ReadKey(True).Key
            Loop Until K = ConsoleKey.Y Or K = ConsoleKey.N

            Dim Res = K = ConsoleKey.Y

            WriteColored(If(Res, " Y", " N"))
            Console.WriteLine()

            Return Res
        End Function

        Public Shared Sub WriteExceptionData(ByVal Ex As Exception)
            Dim Bl = True
            Do
                WriteColored($"{If(Bl, "Exception", "Inner exception")}: ".PadRight(0, "-"c), ConsoleColor.Yellow)
                Bl = False
                Console.WriteLine()
                WriteColored($"Type: {Ex.GetType().FullName}")
                WriteColored($"Message: {Ex.Message}")
                WriteColored($"StackTrace: {Ex.StackTrace}")
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
