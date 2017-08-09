Namespace Common

    Public Class ConsoleListChoiceReader(Of T)

        Public Sub New(ByVal List As IEnumerable(Of T))
            Me.New(List, Function(O) O.ToString())
        End Sub

        Public Sub New(ByVal List As IEnumerable(Of T), ByVal Selector As Func(Of T, String))
            Me.List = List.AsCachedList()
            Me.Selector = Selector
        End Sub

        Public Function ReadChoice() As T
            Dim Choices = New List(Of T)()

            Do
                Choices.Clear()
                For I = 0 To PageLength - 1
                    Dim Tmp As T = Nothing
                    If Not Me.List.TryGetValue(Me.ChoiceOffset + I, Tmp) Then
                        Exit For
                    End If
                    Choices.Add(Tmp)
                Next

                Dim PrevPagePossible = Me.ChoiceOffset - PageLength >= 0
                Dim NextPagePossible = Me.List.TryGetValue(Me.ChoiceOffset + PageLength, Nothing)

                Console.WriteLine()
                Console.WriteLine()
                For I = 0 To Choices.Count - 1
                    ConsoleUtilities.WriteColored($"{I + 1,4} : {Me.Selector.Invoke(Choices.Item(I))}", ConsoleColor.Cyan)
                    Console.WriteLine()
                Next
                Console.WriteLine()
                Dim S = "<- Previous page  |  Page {0:D2}  |  Next page ->  |  (Q)uit".Split("|"c)
                If Not PrevPagePossible Then
                    S(0) = New String(" "c, S(0).Length)
                End If
                S(1) = String.Format(S(1), Me.ChoiceOffset \ PageLength + 1)
                If Not NextPagePossible Then
                    S(2) = New String(" "c, S(2).Length)
                End If
                ConsoleUtilities.WriteColored(S.Aggregate(Function(A, B) A & "|" & B))
                Console.WriteLine()
                Console.WriteLine()
                ConsoleUtilities.WriteColored("Select your choice:", ConsoleColor.Green)

                Do
                    Dim Key = Console.ReadKey(True).Key

                    If Key = ConsoleKey.LeftArrow And PrevPagePossible Then
                        ConsoleUtilities.WriteColored(" <-")
                        Console.WriteLine()
                        Me.ChoiceOffset -= PageLength
                        Exit Do
                    End If
                    If Key = ConsoleKey.RightArrow And NextPagePossible Then
                        ConsoleUtilities.WriteColored(" ->")
                        Console.WriteLine()
                        Me.ChoiceOffset += PageLength
                        Exit Do
                    End If

                    If Key = ConsoleKey.Q Then
                        ConsoleUtilities.WriteColored(" Q")
                        Console.WriteLine()
                        Return Nothing
                    End If

                    Dim N = 0
                    If ConsoleKey.D1 <= Key And Key <= ConsoleKey.D9 Then
                        N = Key - ConsoleKey.D0
                    End If
                    If ConsoleKey.NumPad1 <= Key And Key <= ConsoleKey.NumPad9 Then
                        N = Key - ConsoleKey.NumPad0
                    End If

                    If 1 <= N And N <= Choices.Count Then
                        ConsoleUtilities.WriteColored($" {N.ToStringInv()}")
                        Console.WriteLine()

                        Return Choices.Item(N - 1)
                    End If
                Loop
            Loop
        End Function

        Const PageLength = 9

        Private ChoiceOffset As Integer = 0

        Private ReadOnly List As EnumerableCacher(Of T)
        Private ReadOnly Selector As Func(Of T, String)

    End Class

End Namespace
