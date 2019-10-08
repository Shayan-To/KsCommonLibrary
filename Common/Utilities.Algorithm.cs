using System.Collections.Generic;
using System;

namespace Ks
{
    namespace Common
    {
        partial class Utilities
        {
            public class Algorithm
            {

                /// <param name="Func">N must exist that N >= 0, and Func(I) = True if and only if I >= N.</param>
            /// <param name="MaxX">Func(MaxIndex) must equal True.</param>
            /// <returns>Index of first true.</returns>
                public static int BinarySearch(Func<int, bool> Func, int MaxX = -1)
                {
                    if (MaxX == -1)
                    {
                        MaxX = 8;
                        while (!Func.Invoke(MaxX))
                            MaxX <<= 1;
                    }
                    else
                    {
                        Verify.True(Func.Invoke(MaxX));
                        MaxX = Math.LeastPowerOfTwoOnMin(MaxX);
                    }

                    var X = -1;
                    while (MaxX > 1)
                    {
                        MaxX >>= 1;
                        if (!Func.Invoke(X + MaxX))
                            X += MaxX;
                    }

                    return X + 1;
                }

                /// <param name="Func">N must exist that N >= 0, and Func(I) = True if and only if I >= N.</param>
            /// <param name="End">Func(End) must equal True.</param>
            /// <returns>Index of first true.</returns>
                public static int BinarySearch(Func<int, bool> Func, bool Foreward, int Start, int? End = default(int?))
                {
                    if (Foreward)
                    {
                        var R = BinarySearch(I => Func(I + Start), (End - Start) ?? -1);
                        return R + Start;
                    }
                    else
                    {
                        var R = BinarySearch(I => Func(Start - I), (Start - End) ?? -1);
                        return Start - R;
                    }
                }

                /// <param name="Func">N must exist that N >= 0, and Func(I) = True if and only if I >= N.</param>
            /// <param name="End">Func(End) must equal True. Func will never be called on End.</param>
            /// <returns>Index of first true.</returns>
                public static int BinarySearchIn(Func<int, bool> Func, int Start, int End)
                {
                    if (Start <= End)
                    {
                        var R = BinarySearch(I => ((I + Start) >= End) ? true : Func(I + Start), End - Start);
                        return R + Start;
                    }
                    else
                    {
                        var R = BinarySearch(I => ((Start - I) <= End) ? true : Func(Start - I), Start - End);
                        return Start - R;
                    }
                }

                /// <param name="Func">N must exist that N >= 0, and Func(I) = True if and only if I >= N.</param>
            /// <param name="MaxX">Func(MaxIndex) must equal True.</param>
            /// <returns>Some X that Func(X) = True and |X - N| &lt; MaxError (N is from doc of Func).</returns>
                public static double BinarySearch(Func<double, bool> Func, double MaxError, double MaxX = double.NaN)
                {
                    Verify.True(MaxError > 0);
                    if (double.IsNaN(MaxX))
                    {
                        MaxX = 8 * MaxError;
                        while (!Func.Invoke(MaxX))
                            MaxX *= 2;
                    }
                    else
                        Verify.True(Func.Invoke(MaxX));

                    var X = 0.0;
                    while (MaxX > MaxError)
                    {
                        MaxX /= 2;
                        if (!Func.Invoke(X + MaxX))
                            X += MaxX;
                    }

                    return X + MaxError;
                }

                /// <summary>
            /// Returns a list of (I, J) where List1[I] = List2[J] and the list is [one of] the longest possible list[s].
            /// </summary>
                public static IReadOnlyList<(int Index1, int Index2)> GetLongestCommonSubsequence<T>(IReadOnlyList<T> List1, IReadOnlyList<T> List2)
                {
                    return GetLongestCommonSubsequence(List1, List2, EqualityComparer<T>.Default);
                }

                /// <summary>
            /// Returns a list of (I, J) where List1[I] = List2[J] and the list is [one of] the longest possible list[s].
            /// </summary>
                public static IReadOnlyList<(int Index1, int Index2)> GetLongestCommonSubsequence<T>(IReadOnlyList<T> List1, IReadOnlyList<T> List2, IEqualityComparer<T> Comparer)
                {
                    return GetLongestCommonSubsequence(List1, List2, (A, B) => Comparer.Equals(A, B) ? 1 : -1);
                }

                /// <summary>
            /// Returns a list of (I, J) where List1[I] = List2[J] and the list is [one of] the longest possible list[s].
            /// </summary>
                public static IReadOnlyList<(int Index1, int Index2)> GetLongestCommonSubsequence<T>(IReadOnlyList<T> List1, IReadOnlyList<T> List2, Func<T, T, int> ValueFunction)
                {
                    // We use dynamic programming.

                    // The value of the most valuable common subsequence of List1[0..m] and List2[0..n] is max of:
                    // - ValueFunction(List1[m], List2[n]) + The value of the most valuable common subsequence of List1[0..(m - 1)] and List2[0..(n - 1)].
                    // - The value of the most valuable common subsequence of List1[0..m] and List2[0..(n - 1)].
                    // - The value of the most valuable common subsequence of List1[0..(m - 1)] and List2[0..n].

                    // We do it from the other end so that we can have the result without reversing it.

                    var M = List1.Count;
                    var N = List2.Count;

                    // The tuple is (Length, Mode). See below.
                    var Dyn = new (int, int)[M - 1 + 1, N - 1 + 1];

                    // Mode:
                    // 1 -> Did equal?
                    // 2 -> First index has +1?
                    // 4 -> Second index has +1?

                    for (var I = M - 1; I >= 0; I--)
                    {
                        for (var J = N - 1; J >= 0; J--)
                        {
                            var Length = ValueFunction.Invoke(List1[I], List2[J]);
                            var Mode = 1;

                            if ((I != (M - 1)) & (J != (N - 1)))
                            {
                                Length += Dyn[I + 1, J + 1].Item1;
                                Mode += 2 + 4;
                            }

                            if (I != (M - 1))
                            {
                                var L = Dyn[I + 1, J].Item1;
                                if (L > Length)
                                {
                                    Length = L;
                                    Mode = 2;
                                }
                            }

                            if (J != (N - 1))
                            {
                                var L = Dyn[I, J + 1].Item1;
                                if (L > Length)
                                {
                                    Length = L;
                                    Mode = 4;
                                }
                            }

                            Dyn[I, J] = (Length, Mode);
                        }
                    }

                    var Res = new List<(int, int)>();
                    do
                    {
                        var I = 0;
                        var J = 0;
                        var Cur = default((int, int));
                        do
                        {
                            Cur = Dyn[I, J];
                            if ((Cur.Item2 & 1) == 1)
                                Res.Add((I, J));
                            if ((Cur.Item2 & 2) == 2)
                                I += 1;
                            if ((Cur.Item2 & 4) == 4)
                                J += 1;
                            Assert.True(((Cur.Item2 & (2 + 4)) == (2 + 4)).Implies((Cur.Item2 & 1) == 1));
                        }
                        while ((Cur.Item2 & (2 + 4)) != 0);
                        break;
                    }
                    while (true);

                    return Res.AsReadOnly();
                }

                public static int GetEditDistance<T>(IList<T> L1, IList<T> L2)
                {
                    var Comparer = EqualityComparer<T>.Default;
                    return GetEditDistance(L1, L2, (A, B) => Comparer.Equals(A, B));
                }

                public static int GetEditDistance<T>(IList<T> L1, IList<T> L2, Func<T, T, bool> EqualityComparer)
                {
                    // We use dynamic programming.

                    // The edit distance of L1[0..m] and L2[0..n] is min of:
                    // - If L1[m] = L2[n] Then: The edit distance of L1[0..(m - 1)] and L2[0..(n - 1)].
                    // - Remove L1[m] from L1[0..m]: 1 + The edit distance of L1[0..(m - 1)] and L2[0..n].
                    // - Add L2[n] to the end of L1[0..m]: 1 + The edit distance of L1[0..m] and L2[0..(n - 1)].
                    // - Replace L1[m] with L2[n]: 1 + The edit distance of L1[0..(m - 1)] and L2[0..(n - 1)].

                    // ToDo If the first case happens, do we need to proceed and check the other 3? Or it is proven to be <= others?
                    // ToDo Output the operations as well.

                    var Dyn = new int[L1.Count + 1, L2.Count + 1];
                    for (var I = 0; I <= L1.Count; I++)
                    {
                        for (var J = 0; J <= L2.Count; J++)
                        {
                            var Cost = int.MaxValue;

                            // If the last ones are equal, boh are removed.
                            if ((I != 0) & (J != 0))
                            {
                                if (EqualityComparer.Invoke(L1[I - 1], L2[J - 1]))
                                {
                                    var C = Dyn[I - 1, J - 1];
                                    if (C < Cost)
                                        Cost = C;
                                }
                            }

                            // Remove the last one.
                            if (I != 0)
                            {
                                var C = 1 + Dyn[I - 1, J];
                                if (C < Cost)
                                    Cost = C;
                            }

                            // Add the last of second to here. Now the lasts are equal and are both remove.
                            if (J != 0)
                            {
                                var C = 1 + Dyn[I, J - 1];
                                if (C < Cost)
                                    Cost = C;
                            }

                            // Replace the last with the last of second. Now the lasts are equal and are both removed.
                            if ((I != 0) & (J != 0))
                            {
                                var C = 1 + Dyn[I - 1, J - 1];
                                if (C < Cost)
                                    Cost = C;
                            }

                            if (Cost == int.MaxValue)
                                Cost = 0;

                            Dyn[I, J] = Cost;
                        }
                    }

                    return Dyn[L1.Count, L2.Count];
                }
            }
        }
    }
}
