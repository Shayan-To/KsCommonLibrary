﻿using System.Collections.Generic;
using System;
using Ks.Common;
using Microsoft.VisualBasic.CompilerServices;

namespace Ks.ConsoleTests
{
    public class LongestCommonSubsequenceTest
    {
        [InteractiveRunnable(true)]
        public static void Start()
        {
            var Path1 = Console.ReadLine();
            var Path2 = Console.ReadLine();

            var File1 = System.IO.File.ReadAllLines(Path1, System.Text.Encoding.UTF8);
            var File2 = System.IO.File.ReadAllLines(Path2, System.Text.Encoding.UTF8);
            var Comparer = new StringComparer();

            var LCS = Utilities.Algorithm.GetLongestCommonSubsequence(File1, File2, Comparer);

            var I = 0;
            var J = 0;
            foreach (var L in LCS.Append((File1.Length, File2.Length)))
            {
                var loopTo = L.Index1 - 1;
                for (I = I; I <= loopTo; I++)
                {
                    ConsoleUtilities.WriteColored(File1[I], ConsoleColor.Red, ConsoleColor.White);
                    Console.WriteLine();
                }

                var loopTo1 = L.Index2 - 1;
                for (J = J; J <= loopTo1; J++)
                {
                    ConsoleUtilities.WriteColored(File2[J], ConsoleColor.Green, ConsoleColor.Black);
                    Console.WriteLine();
                }
                if (I == File1.Length)
                    break;
                // They can be non-equal, as the comparer trims.
                if (Operators.CompareString(File1[I], File2[J], TextCompare: false) == 0)
                    ConsoleUtilities.WriteColored(File2[J], ConsoleColor.Black, ConsoleColor.White);
                else
                    ConsoleUtilities.WriteColored(File2[J], ConsoleColor.Blue, ConsoleColor.White);
                Console.WriteLine();
                I += 1;
                J += 1;
            }

            Console.WriteLine();
        }

        public class StringComparer : EqualityComparer<string>
        {
            public override bool Equals(string x, string y)
            {
                return Operators.CompareString(x.Trim(), y.Trim(), TextCompare: false) == 0; // x.GetHashCode() = y.GetHashCode()
            }

            public override int GetHashCode(string obj)
            {
                return obj.Trim().GetHashCode();
            }
        }
    }
}
