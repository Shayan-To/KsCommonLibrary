using System;
using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
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

            while (true)
            {
                Choices.Clear();
                for (var I = 0; I < PageLength; I++)
                {
                    if (!this.List.TryGetValue(this.ChoiceOffset + I, out var Tmp))
                    {
                        break;
                    }

                    Choices.Add(Tmp);
                }

                var PrevPagePossible = (this.ChoiceOffset - PageLength) >= 0;
                var NextPagePossible = this.List.TryGetValue(this.ChoiceOffset + PageLength, out var argValue);
                Console.WriteLine();
                Console.WriteLine();
                for (var I = 0; I < Choices.Count; I++)
                {
                    ConsoleUtilities.WriteColored($"{I + 1,4} : {this.Selector.Invoke(Choices[I])}", ConsoleColor.Cyan);
                    Console.WriteLine();
                }

                Console.WriteLine();
                var S = "<- Previous page  |  Page {0:D2}  |  Next page ->  |  (Q)uit".Split('|');
                if (!PrevPagePossible)
                {
                    S[0] = new string(' ', S[0].Length);
                }

                S[1] = string.Format(S[1], (this.ChoiceOffset / PageLength) + 1);
                if (!NextPagePossible)
                {
                    S[2] = new string(' ', S[2].Length);
                }

                ConsoleUtilities.WriteColored(S.Aggregate((A, B) => A + "|" + B));
                Console.WriteLine();
                Console.WriteLine();
                ConsoleUtilities.WriteColored("Select your choice:", ConsoleColor.Green);
                while (true)
                {
                    var Key = Console.ReadKey(true).Key;
                    if ((Key == ConsoleKey.LeftArrow) & PrevPagePossible)
                    {
                        ConsoleUtilities.WriteColored(" <-");
                        Console.WriteLine();
                        this.ChoiceOffset -= PageLength;
                        break;
                    }

                    if ((Key == ConsoleKey.RightArrow) & NextPagePossible)
                    {
                        ConsoleUtilities.WriteColored(" ->");
                        Console.WriteLine();
                        this.ChoiceOffset += PageLength;
                        break;
                    }

                    if (Key == ConsoleKey.Q)
                    {
                        ConsoleUtilities.WriteColored(" Q");
                        Console.WriteLine();
                        return default;
                    }

                    var N = 0;
                    if ((ConsoleKey.D1 <= Key) & (Key <= ConsoleKey.D9))
                    {
                        N = Key - ConsoleKey.D0;
                    }

                    if ((ConsoleKey.NumPad1 <= Key) & (Key <= ConsoleKey.NumPad9))
                    {
                        N = Key - ConsoleKey.NumPad0;
                    }

                    if ((1 <= N) & (N <= Choices.Count))
                    {
                        ConsoleUtilities.WriteColored($" {N.ToStringInv()}");
                        Console.WriteLine();
                        return Choices[N - 1];
                    }
                }
            }
        }

        private const int PageLength = 9;

        private int ChoiceOffset = 0;

        private readonly EnumerableCacher<T> List;
        private readonly Func<T, string> Selector;
    }
}
