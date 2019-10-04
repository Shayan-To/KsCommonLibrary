using System.Linq;
using System.Collections.Generic;
using System;

namespace Ks
{
    namespace Common
    {
        public class ConsoleListChoiceReader<T>
        {
            public ConsoleListChoiceReader(IEnumerable<T> List) : this(List, O => O.ToString())
            {
            }

            public ConsoleListChoiceReader(IEnumerable<T> List, Func<T, string> Selector)
            {
                this.List = List.AsCachedList();
                this.Selector = Selector;
            }

            public T ReadChoice()
            {
                var Choices = new List<T>();

                do
                {
                    Choices.Clear();
                    for (var I = 0; I <= PageLength - 1; I++)
                    {
                        T Tmp = default(T);
                        if (!this.List.TryGetValue(this.ChoiceOffset + I, ref Tmp))
                            break;
                        Choices.Add(Tmp);
                    }

                    var PrevPagePossible = (this.ChoiceOffset - PageLength) >= 0;
                    var argValue = default(T);
                    var NextPagePossible = this.List.TryGetValue(this.ChoiceOffset + PageLength, ref argValue);
                    Console.WriteLine();
                    Console.WriteLine();
                    var loopTo = Choices.Count - 1;
                    for (var I = 0; I <= loopTo; I++)
                    {
                        ConsoleUtilities.WriteColored($"{I + 1,4} : {this.Selector.Invoke(Choices[I])}", ConsoleColor.Cyan);
                        Console.WriteLine();
                    }

                    Console.WriteLine();
                    var S = "<- Previous page  |  Page {0:D2}  |  Next page ->  |  (Q)uit".Split('|');
                    if (!PrevPagePossible)
                        S[0] = new string(' ', S[0].Length);
                    S[1] = string.Format(S[1], (this.ChoiceOffset / PageLength) + 1);
                    if (!NextPagePossible)
                        S[2] = new string(' ', S[2].Length);
                    ConsoleUtilities.WriteColored(S.Aggregate((A, B) => A + "|" + B));
                    Console.WriteLine();
                    Console.WriteLine();
                    ConsoleUtilities.WriteColored("Select your choice:", ConsoleColor.Green);
                    do
                    {
                        var Key = Console.ReadKey(true).Key;
                        if (((int)Key == (int)ConsoleKey.LeftArrow) & PrevPagePossible)
                        {
                            ConsoleUtilities.WriteColored(" <-");
                            Console.WriteLine();
                            this.ChoiceOffset -= PageLength;
                            break;
                        }

                        if (((int)Key == (int)ConsoleKey.RightArrow) & NextPagePossible)
                        {
                            ConsoleUtilities.WriteColored(" ->");
                            Console.WriteLine();
                            this.ChoiceOffset += PageLength;
                            break;
                        }

                        if ((int)Key == (int)ConsoleKey.Q)
                        {
                            ConsoleUtilities.WriteColored(" Q");
                            Console.WriteLine();
                            return default(T);
                        }

                        var N = 0;
                        if (((int)ConsoleKey.D1 <= (int)Key) & ((int)Key <= (int)ConsoleKey.D9))
                            N = (int)Key - (int)ConsoleKey.D0;
                        if (((int)ConsoleKey.NumPad1 <= (int)Key) & ((int)Key <= (int)ConsoleKey.NumPad9))
                            N = (int)Key - (int)ConsoleKey.NumPad0;
                        if ((1 <= N) & (N <= Choices.Count))
                        {
                            ConsoleUtilities.WriteColored($" {N.ToStringInv()}");
                            Console.WriteLine();
                            return Choices[N - 1];
                        }
                    }
                    while (true);
                }
                while (true);
            }

            const int PageLength = 9;

            private int ChoiceOffset = 0;

            private readonly EnumerableCacher<T> List;
            private readonly Func<T, string> Selector;
        }
    }
}
