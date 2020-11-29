using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Ks.Common;

namespace Ks.Tests
{
    public static class LongestCommonSubsequenceTest
    {
        [InteractiveRunnable(true)]
        public static void Start()
        {
            var Path1 = Console.ReadLine() ?? throw new EndOfStreamException();
            var Path2 = Console.ReadLine() ?? throw new EndOfStreamException();

            var File1 = System.IO.File.ReadAllLines(Path1, System.Text.Encoding.UTF8);
            var File2 = System.IO.File.ReadAllLines(Path2, System.Text.Encoding.UTF8);
            var Comparer = new StringComparer();

            var LCS = Utilities.Algorithm.GetLongestCommonSubsequence(File1, File2, Comparer);

            var I = 0;
            var J = 0;
            foreach (var L in LCS.Append((Index1: File1.Length, Index2: File2.Length)))
            {
                for (; I < L.Index1; I++)
                {
                    ConsoleUtilities.WriteColored(File1[I], ConsoleColor.Red, ConsoleColor.White);
                    Console.WriteLine();
                }

                for (; J < L.Index2; J++)
                {
                    ConsoleUtilities.WriteColored(File2[J], ConsoleColor.Green, ConsoleColor.Black);
                    Console.WriteLine();
                }
                if (I == File1.Length)
                {
                    break;
                }
                // They can be non-equal, as the comparer trims.
                if (File1[I] == File2[J])
                {
                    ConsoleUtilities.WriteColored(File2[J], ConsoleColor.Black, ConsoleColor.White);
                }
                else
                {
                    ConsoleUtilities.WriteColored(File2[J], ConsoleColor.Blue, ConsoleColor.White);
                }

                Console.WriteLine();
                I += 1;
                J += 1;
            }

            Console.WriteLine();
        }

        public class StringComparer : EqualityComparer<string>
        {
            public override bool Equals(string? x, string? y)
            {
                return x?.Trim() == y?.Trim(); // x.GetHashCode() = y.GetHashCode()
            }

            public override int GetHashCode(string obj)
            {
                return obj.Trim().GetHashCode();
            }
        }
    }
}
