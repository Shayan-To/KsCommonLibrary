using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using Ks.Common.Geometry;

namespace Ks.Common
{
    public static class CommonExtensions
    {
        #region String Group
        public static string RegexEscape(this string str)
        {
            return Regex.Escape(str);
        }

        public static bool RegexIsMatch(this string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }

        public static bool RegexIsMatch(this string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        public static bool RegexIsMatch(this string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
        {
            return Regex.IsMatch(input, pattern, options, matchTimeout);
        }

        public static Match RegexMatch(this string input, string pattern)
        {
            return Regex.Match(input, pattern);
        }

        public static Match RegexMatch(this string input, string pattern, RegexOptions options)
        {
            return Regex.Match(input, pattern, options);
        }

        public static Match RegexMatch(this string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
        {
            return Regex.Match(input, pattern, options, matchTimeout);
        }

        public static MatchCollection RegexMatches(this string input, string pattern)
        {
            return Regex.Matches(input, pattern);
        }

        public static MatchCollection RegexMatches(this string input, string pattern, RegexOptions options)
        {
            return Regex.Matches(input, pattern, options);
        }

        public static MatchCollection RegexMatches(this string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
        {
            return Regex.Matches(input, pattern, options, matchTimeout);
        }

        public static string RegexReplace(this string input, string pattern, MatchEvaluator evaluator)
        {
            return Regex.Replace(input, pattern, evaluator);
        }

        public static string RegexReplace(this string input, string pattern, string replacement)
        {
            return Regex.Replace(input, pattern, replacement);
        }

        public static string RegexReplace(this string input, string pattern, MatchEvaluator evaluator, RegexOptions options)
        {
            return Regex.Replace(input, pattern, evaluator, options);
        }

        public static string RegexReplace(this string input, string pattern, string replacement, RegexOptions options)
        {
            return Regex.Replace(input, pattern, replacement, options);
        }

        public static string RegexReplace(this string input, string pattern, string replacement, RegexOptions options, TimeSpan matchTimeout)
        {
            return Regex.Replace(input, pattern, replacement, options, matchTimeout);
        }

        public static string RegexReplace(this string input, string pattern, MatchEvaluator evaluator, RegexOptions options, TimeSpan matchTimeout)
        {
            return Regex.Replace(input, pattern, evaluator, options, matchTimeout);
        }

        public static string[] RegexSplit(this string input, string pattern)
        {
            return Regex.Split(input, pattern);
        }

        public static string[] RegexSplit(this string input, string pattern, RegexOptions options)
        {
            return Regex.Split(input, pattern, options);
        }

        public static string[] RegexSplit(this string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
        {
            return Regex.Split(input, pattern, options, matchTimeout);
        }

        public static string RegexUnescape(this string str)
        {
            return Regex.Unescape(str);
        }

        public static string CorrectLineEndings(this string S)
        {
            return S.RegexReplace(@"\r\n|\r|\n", Environment.NewLine);
        }

        public static string TruncateEnd(this string s, int length, string ellipsis = "...")
        {
            if (s.Length <= length)
            {
                return s;
            }
            length -= ellipsis.Length;
            return s[..length] + ellipsis;
        }

        public static string TruncateStart(this string s, int length, string ellipsis = "...")
        {
            if (s.Length <= length)
            {
                return s;
            }
            length -= ellipsis.Length;
            return ellipsis + s.Substring(s.Length - length, length);
        }

        public static string TruncateMid(this string s, int length, string ellipsis = "...")
        {
            if (s.Length <= length)
            {
                return s;
            }
            length -= ellipsis.Length;
            var halfLength = length / 2;
            return s[..(length - halfLength)] + ellipsis + s.Substring(s.Length - halfLength, halfLength);
        }
        #endregion

        #region Math Group
        public static int GetLeastCommonMultiple(this IEnumerable<int> Self)
        {
            var Res = 0;
            var First = true;

            foreach (var I in Self)
            {
                if (First)
                {
                    Res = I;
                    First = false;
                }
                else
                {
                    Res = Utilities.Math.LeastCommonMultiple(Res, I);
                }
            }

            return Res;
        }

        public static long GetLeastCommonMultiple(this IEnumerable<long> Self)
        {
            var Res = 0L;
            var First = true;

            foreach (var I in Self)
            {
                if (First)
                {
                    Res = I;
                    First = false;
                }
                else
                {
                    Res = Utilities.Math.LeastCommonMultiple(Res, I);
                }
            }

            return Res;
        }
        public static int GetGreatestCommonDivisor(this IEnumerable<int> Self)
        {
            var Res = 0;
            var First = true;

            foreach (var I in Self)
            {
                if (First)
                {
                    Res = I;
                    First = false;
                }
                else
                {
                    Res = Utilities.Math.GreatestCommonDivisor(Res, I);
                }
            }

            return Res;
        }

        public static long GetGreatestCommonDivisor(this IEnumerable<long> Self)
        {
            var Res = 0L;
            var First = true;

            foreach (var I in Self)
            {
                if (First)
                {
                    Res = I;
                    First = false;
                }
                else
                {
                    Res = Utilities.Math.GreatestCommonDivisor(Res, I);
                }
            }

            return Res;
        }
        #endregion

        #region CollectionUtils Group
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerable<T> Self)
        {
            foreach (var I in Self)
            {
                yield return I;
            }
        }

        public static int? FastCount<T>(this IEnumerable<T> Self)
        {
            if (Self is ICollection<T> CollectionT)
            {
                return CollectionT.Count;
            }
            if (Self is ICollection Collection)
            {
                return Collection.Count;
            }
            return default;
        }

        public static IEnumerable<T> PadBegin<T>(this IEnumerable<T> Self, int Count, T PaddingElement = default)
        {
            var Cnt = Self.Count();
            if (Cnt >= Count)
            {
                return Self;
            }

            return Self.PadBeginImpl(Cnt, Count, PaddingElement);
        }

        private static IEnumerable<T> PadBeginImpl<T>(this IEnumerable<T> Self, int SelfCount, int Count, T PaddingElement)
        {
            for (; SelfCount < Count; SelfCount++)
            {
                yield return PaddingElement;
            }

            foreach (var I in Self)
            {
                yield return I;
            }
        }

        public static IEnumerable<T> PadEnd<T>(this IEnumerable<T> Self, int Count, T PaddingElement = default)
        {
            var Cnt = Self.FastCount();
            if (Cnt.HasValue)
            {
                if (Cnt.Value >= Count)
                {
                    return Self;
                }
            }
            return Self.PadEndImpl(Count, PaddingElement);
        }

        private static IEnumerable<T> PadEndImpl<T>(this IEnumerable<T> Self, int Count, T PaddingElement = default)
        {
            var Cnt = 0;
            foreach (var I in Self)
            {
                yield return I;
                Cnt += 1;
            }

            for (; Cnt < Count; Cnt++)
            {
                yield return PaddingElement;
            }
        }

        public static void ReverseSelf<T>(this IList<T> Self, int Index = 0, int Count = -1)
        {
            if (Count == -1)
            {
                Count = Self.Count - Index;
            }

            var Complement = Count + (2 * Index) - 1;
            var MaxI = Index + (Count / 2);
            for (var I = Index; I < MaxI; I++)
            {
                var C = Self[I];
                Self[I] = Self[Complement - I];
                Self[Complement - I] = C;
            }
        }

        public static IEnumerable<T> Reverse<T>(this IReadOnlyList<T> self)
        {
            for (var i = self.Count - 1; i >= 0; i -= 1)
            {
                yield return self[i];
            }
        }

        public static StringAsListCollection AsList(this string Self)
        {
            return new StringAsListCollection(Self);
        }

        public static bool Equals<T1, T2>(this IEnumerable<T1> Self, IEnumerable<T2> Other, Func<T1, T2, bool> Comparison)
        {
            using var Enum1 = Self.GetEnumerator();
            using var Enum2 = Other.GetEnumerator();
            while (true)
            {
                if (!Enum1.MoveNext())
                {
                    return !Enum2.MoveNext();
                }

                if (!Enum2.MoveNext())
                {
                    return false;
                }

                if (!Comparison.Invoke(Enum1.Current, Enum2.Current))
                {
                    return false;
                }
            }
        }

        public static IEnumerable<(TKey Key, EnumerableCacher<T> Items)> SplitByKeyChange<T, TKey>(this IEnumerable<T> self, Func<T, TKey> keySelector)
        {
            return self.SplitByKeyChange(keySelector, EqualityComparer<TKey>.Default);
        }

        public static IEnumerable<(TKey Key, EnumerableCacher<T> Items)> SplitByKeyChange<T, TKey>(this IEnumerable<T> self, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            self = self.GetEnumerator().ToEnumerable();
            var prevElement = self.First();
            var key = keySelector.Invoke(prevElement);
            var done = false;

            IEnumerable<T> GetSplit()
            {
                foreach (var i in self)
                {
                    yield return prevElement;
                    prevElement = i;

                    var oldKey = key;
                    key = keySelector.Invoke(i);

                    if (!comparer.Equals(key, oldKey))
                    {
                        yield break;
                    }
                }
                done = true;
            }

            while (!done)
            {
                var split = GetSplit().AsCachedList();
                yield return (key, split);
                split.DrainEnumerable();
            }
        }

        public static IEnumerable<(T Beginning, IEnumerable<T> Items, T Ending)> SplitByElement<T>(this IEnumerable<T> self, Func<T, bool> pred)
        {
            return self.SplitByElement(pred, s => s);
        }

        public static IEnumerable<(TSplit Beginning, IEnumerable<T> Items, TSplit Ending)> SplitByElement<T, TSplit>(this IEnumerable<T> self, Func<T, bool> pred, Func<T, TSplit> splitterSelector)
        {
            var list = self.AsCachedList();

            var prev = 0;
            var beginning = splitterSelector.Invoke(default);

            var i = 0;
            foreach (var l in list)
            {
                if (pred.Invoke(l))
                {
                    var split = splitterSelector.Invoke(l);
                    yield return (beginning, list.Skip(prev).Take(i - prev), split);
                    prev = i + 1;
                    beginning = split;
                }
                i += 1;
            }

            yield return (beginning, list.Skip(prev).Take(i - prev), splitterSelector.Invoke(default));
        }

        public static IEnumerable<TResult> SelectSome<T, TResult>(this IEnumerable<T> self, Func<T, (TResult Result, bool Continue)> selector)
        {
            foreach (var i in self)
            {
                var t = selector.Invoke(i);
                yield return t.Result;
                if (!t.Continue)
                {
                    break;
                }
            }
        }

        public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> self)
        {
            while (self.MoveNext())
            {
                yield return self.Current;
            }
        }

        public static IEnumerable<T> TakeLast2<T>(this IEnumerable<T> self, int count)
        {
            if (count <= 0)
            {
                yield break;
            }

            var buffer = new Queue<T>(count);

            foreach (var i in self)
            {
                if (buffer.Count == count)
                {
                    buffer.Dequeue();
                }
                buffer.Enqueue(i);
            }

            while (buffer.Count != 0)
            {
                yield return buffer.Dequeue();
            }
        }

        public static IEnumerable<T> SkipLast2<T>(this IEnumerable<T> self, int count)
        {
            if (count <= 0)
            {
                foreach (var i in self)
                {
                    yield return i;
                }
                yield break;
            }

            var buffer = new Queue<T>(count);

            foreach (var i in self)
            {
                if (buffer.Count == count)
                {
                    yield return buffer.Dequeue();
                }
                buffer.Enqueue(i);
            }
        }

        public static IEnumerable<TRes> ZipNeighbors<T, TRes>(this IEnumerable<T> self, Func<T, T, TRes> func)
        {
            var bl = true;
            var prev = default(T);
            foreach (var i in self)
            {
                if (bl)
                {
                    bl = false;
                    prev = i;
                    continue;
                }
                yield return func.Invoke(prev, i);
                prev = i;
            }
        }

        public static TAggregate Aggregate<TAggregate, T>(this IEnumerable<T> Self, TAggregate Seed, Func<TAggregate, T, int, TAggregate> Func)
        {
            var Ind = 0;
            foreach (var I in Self)
            {
                Seed = Func.Invoke(Seed, I, Ind);
                Ind += 1;
            }
            return Seed;
        }

        public static T Aggregate<T>(this IEnumerable<T> Self, Func<T, T, T> Func, T EmptyValue)
        {
            var R = EmptyValue;
            var Bl = true;
            foreach (var I in Self)
            {
                if (Bl)
                {
                    R = I;
                    Bl = false;
                    continue;
                }
                R = Func.Invoke(R, I);
            }
            return R;
        }

        public static IEnumerable<T> Cumulate<T>(this IEnumerable<T> self, Func<T, T, T> func)
        {
            var bl = true;
            var v = default(T);
            foreach (var i in self)
            {
                if (bl)
                {
                    v = i;
                    bl = false;
                }
                else
                {
                    v = func.Invoke(v, i);
                }
                yield return v;
            }
        }

        public static IEnumerable<TCumulate> Cumulate<T, TCumulate>(this IEnumerable<T> self, TCumulate seed, Func<TCumulate, T, TCumulate> func)
        {
            var v = seed;
            foreach (var i in self)
            {
                v = func.Invoke(v, i);
                yield return v;
            }
        }

        public static int IndexOf<T>(this IList<T> Self, Func<T, bool> Predicate, int StartIndex = 0)
        {
            return Self.IndexOf((i, _) => Predicate.Invoke(i), StartIndex);
        }

        public static int IndexOf<T>(this IList<T> Self, Func<T, int, bool> Predicate, int StartIndex = 0)
        {
            for (var I = StartIndex; I < Self.Count; I++)
            {
                if (Predicate.Invoke(Self[I], I))
                {
                    return I;
                }
            }
            return -1;
        }

        public static int LastIndexOf<T>(this IList<T> Self, Func<T, bool> Predicate, int StartIndex = -1)
        {
            return Self.LastIndexOf((i, _) => Predicate.Invoke(i), StartIndex);
        }

        // ToDo: Set the fallback value to StartIndex before the loop.
        public static int LastIndexOf<T>(this IList<T> Self, Func<T, int, bool> Predicate, int StartIndex = -1)
        {
            for (var I = (StartIndex != -1) ? StartIndex : (Self.Count - 1); I >= 0; I--)
            {
                if (Predicate.Invoke(Self[I], I))
                {
                    return I;
                }
            }
            return -1;
        }

        public static void Move<T>(this IList<T> Self, int OldIndex, int NewIndex)
        {
            var Item = Self[OldIndex];
            for (var I = OldIndex; I < NewIndex; I++)
            {
                Self[I] = Self[I + 1];
            }

            for (var I = OldIndex; I > NewIndex; I--)
            {
                Self[I] = Self[I - 1];
            }

            Self[NewIndex] = Item;
        }

        public static void Swap<T>(this IList<T> self, int i1, int i2)
        {
            if (i1 == i2)
            {
                return;
            }

            var c = self[i1];
            self[i1] = self[i2];
            self[i2] = c;
        }

        public static void RemoveRange<T>(this IList<T> Self, int StartIndex, int Length = -1)
        {
            if (Length == -1)
            {
                Length = Self.Count - StartIndex;
            }
            else if (Length == 0)
            {
                return;
            }

            Verify.TrueArg(Length > 0, nameof(Length), "Length must be a non-negative number.");
            Verify.True((StartIndex + Length) <= Self.Count, "The given range must be inside the list.");
            for (var I = StartIndex; I < Self.Count - Length; I++)
            {
                Self[I] = Self[I + Length];
            }

            for (var I = Self.Count - 1; I >= Self.Count - Length; I--)
            {
                Self.RemoveAt(I);
            }
        }

        public static int RemoveWhere<T>(this IList<T> Self, Func<T, bool> Predicate, int StartIndex = 0, int Length = -1)
        {
            if (Length == -1)
            {
                Length = Self.Count - StartIndex;
            }
            else if (Length == 0)
            {
                return 0;
            }

            Verify.TrueArg(Length > 0, nameof(Length), "Length must be a non-negative number.");
            Verify.True((StartIndex + Length) <= Self.Count, "The given range must be inside the list.");

            var Count = 0;
            var I = StartIndex;
            for (var J = StartIndex; J < StartIndex + Length; J++)
            {
                if (!Predicate.Invoke(Self[J]))
                {
                    Self[I] = Self[J];
                    I += 1;
                }
                else
                {
                    Count += 1;
                }
            }

            Self.RemoveRange(I, Count);

            return Count;
        }

        public static byte[] Resize(this byte[] Array, int Length)
        {
            if (Length != Array.Length)
            {
                var R = new byte[Length];
                System.Array.Copy(Array, 0, R, 0, Math.Min(Length, Array.Length));
                Array = R;
            }

            return Array;
        }

        public static byte[] Resize(this byte[] Array, int Offset, int Length)
        {
            if ((Length != Array.Length) | (Offset != 0))
            {
                var R = new byte[Length];
                System.Array.Copy(Array, Offset, R, 0, Math.Min(Length, Array.Length - Offset));
                Array = R;
            }

            return Array;
        }

        public static T[] Subarray<T>(this T[] Self, int Start, int Count)
        {
            var Res = new T[Count];
            Array.Copy(Self, Start, Res, 0, Count);
            return Res;
        }

        public static ReadOnlySubList<T> ReadOnlySubList<T>(this IReadOnlyList<T> self, int index, int count = -1)
        {
            if (count == -1)
            {
                count = self.Count - index;
            }
            return new ReadOnlySubList<T>(self, index, count);
        }

        public static SubList<T> SubList<T>(this IList<T> self, int index, int count = -1)
        {
            if (count == -1)
            {
                count = self.Count - index;
            }
            return new SubList<T>(self, index, count);
        }

        public static (int Index, int Count) BinarySearch<T>(this IReadOnlyList<T> Self, T Value)
        {
            return Self.BinarySearch(Value, Comparer<T>.Default);
        }

        public static (int Index, int Count) BinarySearch<T>(this IReadOnlyList<T> Self, T Value, IComparer<T> Comp)
        {
            return Self.BinarySearch(Value, new Comparison<T, T>(Comp.Compare));
        }

        public static (int Index, int Count) BinarySearch<T>(this IReadOnlyList<T> Self, T Value, Comparison<T> Comp)
        {
            return Self.BinarySearch(Value, new Comparison<T, T>(Comp));
        }

        /// <summary>
        /// Gets the interval in which Value resides in inside a sorted list.
        /// </summary>
        /// <param name="Value">The value to look for.</param>
        /// <returns>
        /// Start index being the index of fist occurrance of Value, and length being the count of its occurrances.
        /// If no occurrance of Value is found, start index will be at the first element larger than Value.
        /// </returns>
        public static (int Index, int Count) BinarySearch<T, TValue>(this IReadOnlyList<T> Self, TValue Value, Comparison<T, TValue> Comp)
        {
            var Count = Utilities.Math.LeastPowerOfTwoOnMin(Self.Count + 1) / 2;
            var Offset1 = -1;

            while (Count > 0)
            {
                if ((Offset1 + Count) < Self.Count)
                {
                    var C = Comp.Invoke(Self[Offset1 + Count], Value);
                    if (C < 0)
                    {
                        Offset1 += Count;
                    }
                    else if (C == 0)
                    {
                        break;
                    }
                }
                Count /= 2;
            }

            var Offset2 = Offset1;
            if (Count > 0)
            {
                // This should have been done in the ElseIf block in the previous loop before the Exit statement.
                Offset2 += Count;

                while (Count > 1)
                {
                    Count /= 2;
                    if ((Offset1 + Count) < Self.Count)
                    {
                        if (Comp.Invoke(Self[Offset1 + Count], Value) < 0)
                        {
                            Offset1 += Count;
                        }
                    }
                    if ((Offset2 + Count) < Self.Count)
                    {
                        if (Comp.Invoke(Self[Offset2 + Count], Value) <= 0)
                        {
                            Offset2 += Count;
                        }
                    }
                }
            }

            return (Offset1 + 1, Offset2 - Offset1);
        }

        public static StringSplitEnumerator EnumerateSplit(this string Str, StringSplitOptions Options, params char[] Chars)
        {
            return new StringSplitEnumerator(Str, Options, Chars);
        }

        public static IReadOnlyList<T> CastAsList<T>(this IList Self)
        {
            if (Self is IReadOnlyList<T> R)
            {
                return R;
            }

            return new CastAsListCollection<T>(Self);
        }

        public static SelectAsListCollection<TIn, TOut> SelectAsList<TIn, TOut>(this IReadOnlyList<TIn> Self, Func<TIn, TOut> Func)
        {
            return new SelectAsListCollection<TIn, TOut>(Self, Func);
        }

        public static SelectAsListCollection<TIn, TOut> SelectAsList<TIn, TOut>(this IReadOnlyList<TIn> Self, Func<TIn, int, TOut> Func)
        {
            return new SelectAsListCollection<TIn, TOut>(Self, Func);
        }

        public static SelectAsNotifyingListCollection<TIn, TOut> SelectAsNotifyingList<TIn, TOut>(this IReadOnlyList<TIn> Self, Func<TIn, TOut> Func)
        {
            return new SelectAsNotifyingListCollection<TIn, TOut>(Self, Func);
        }

        public static SelectAsNotifyingListCollection<TIn, TOut> SelectAsNotifyingList<TIn, TOut>(this IReadOnlyList<TIn> Self, Func<TIn, int, TOut> Func)
        {
            return new SelectAsNotifyingListCollection<TIn, TOut>(Self, Func);
        }

        public static IEnumerable<T> DistinctNeighbors<T>(this IEnumerable<T> Self)
        {
            return DistinctNeighbors(Self, EqualityComparer<T>.Default);
        }

        public static IEnumerable<T> DistinctNeighbors<T>(this IEnumerable<T> Self, IEqualityComparer<T> Comparer)
        {
            var Bl = true;
            var P = default(T);

            foreach (var I in Self)
            {
                if (Bl)
                {
                    yield return I;
                    P = I;
                    Bl = false;
                }
                else if (!Comparer.Equals(P, I))
                {
                    yield return I;
                    P = I;
                }
            }
        }

        public static T RandomElement<T>(this IEnumerable<T> Self)
        {
            var Rnd = DefaultCacher<Random>.Value;

            if (Self is IList<T>)
            {
                var L = (IList<T>) Self;
                return L[Rnd.Next(L.Count)];
            }

            if (Self is IList)
            {
                var L = (IList) Self;
                return (T) L[Rnd.Next(L.Count)];
            }

            return Self.ElementAt(Rnd.Next(Self.Count()));
        }

        public static T[] RandomElements<T>(this IEnumerable<T> Self, int Count)
        {
            var Res = new T[Count];
            var Cnt = 0;

            var Rand = DefaultCacher<Random>.Value;

            foreach (var I in Self)
            {
                Cnt += 1;
                if (Cnt <= Count)
                {
                    Res[Cnt - 1] = I;
                    if (Cnt == Count)
                    {
                        Res.RandomizeSelfOrder();
                    }
                }
                else if (Rand.Next(Cnt) < Count)
                {
                    Res[Rand.Next(Count)] = I;
                }
            }

            return Res;
        }

        public static IEnumerable<T> RandomPick<T>(this IEnumerable<T> Self, double Ratio)
        {
            var Rand = DefaultCacher<Random>.Value;

            foreach (var I in Self)
            {
                if (Rand.NextDouble() < Ratio)
                {
                    yield return I;
                }
            }
        }

        public static IEnumerable<T> RandomPick<T>(this IEnumerable<T> Self, Ratio Ratio)
        {
            var Rand = DefaultCacher<Random>.Value;

            foreach (var I in Self)
            {
                if (Rand.Next(Ratio.Denumenator) < Ratio.Numerator)
                {
                    yield return I;
                }
            }
        }

        public static void RandomizeSelfOrder<T>(this IList<T> Self, Random Random = null)
        {
            if (Random == null)
            {
                Random = DefaultCacher<Random>.Value;
            }

            for (var I = 1; I < Self.Count; I++)
            {
                var J = Random.Next(I + 1);
                Self.Swap(I, J);
            }
        }

        public static IEnumerable<T> RandomizeOrder<T>(this IEnumerable<T> Self, Random Random = null)
        {
            var Arr = Self.ToArray();
            Arr.RandomizeSelfOrder(Random);
            return Arr;
        }

        public static void CopyTo<T>(this IEnumerable<T> Self, IList<T> Destination, int Index = 0, int Count = -1)
        {
            // If Destination.Count - Index < Self.Count() Then
            // Throw New ArgumentException("There is not enough space on the destination to copy the collection.")
            // End If
            Verify.TrueArg(Count >= -1, "Count");

            if (Count == -1)
            {
                foreach (var I in Self)
                {
                    Destination[Index] = I;
                    Index += 1;
                }
            }
            else if (Count > 0)
            {
                foreach (var I in Self)
                {
                    Destination[Index] = I;
                    Index += 1;
                    Count -= 1;
                    if (Count == 0)
                    {
                        break;
                    }
                }
            }
        }

        public static void AddRange<T>(this IList<T> Self, IEnumerable<T> Items)
        {
            foreach (var I in Items)
            {
                Self.Add(I);
            }
        }

        [Obsolete("This method is dangerous.", false)]
        public static void AddRange(this IList Self, IEnumerable Items)
        {
            foreach (var I in Items)
            {
                Self.Add(I);
            }
        }

        public static void Sort<T>(this IList<T> Self)
        {
            DefaultCacher<MergeSorter<T>>.Value.Sort(Self);
        }

        public static void Sort<T>(this IList<T> Self, IComparer<T> Comparer)
        {
            DefaultCacher<MergeSorter<T>>.Value.Sort(Self, Comparer);
        }

        public static void Sort<T>(this IList<T> Self, Comparison<T, T> Comparer)
        {
            DefaultCacher<MergeSorter<T>>.Value.Sort(Self, Comparer);
        }

        public static void Sort<T, TKey>(this IList<T> Self, Func<T, TKey> KeySelector, Comparison<TKey, TKey> Comparer)
        {
            DefaultCacher<MergeSorter<T>>.Value.Sort(Self, (a, b) => Comparer.Invoke(KeySelector.Invoke(a), KeySelector.Invoke(b)));
        }

        public static void Sort<T, TKey>(this IList<T> Self, Func<T, TKey> KeySelector, IComparer<TKey> Comparer)
        {
            Self.Sort(KeySelector, Comparer.Compare);
        }

        public static void Sort<T, TKey>(this IList<T> Self, Func<T, TKey> KeySelector)
        {
            Self.Sort(KeySelector, Comparer<TKey>.Default.Compare);
        }

        public static string AllToString<T>(this IEnumerable<T> Self)
        {
            var Res = new System.Text.StringBuilder("{");

            var Bl = false;
            foreach (var I in Self)
            {
                if (Bl)
                {
                    Res.Append(", ");
                }

                Bl = true;

                Res.Append(I);
            }

            return Res.Append('}').ToString();
        }

        public static ReadOnlyListWrapper<T> AsReadOnly<T>(this IList<T> Self)
        {
            return new ReadOnlyListWrapper<T>(Self);
        }

        public static System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> Self)
        {
            return new System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue>(Self);
        }

        public static ReadOnlyCollectionWrapper<T> AsReadOnly<T>(this ICollection<T> Self)
        {
            return new ReadOnlyCollectionWrapper<T>(Self);
        }

        public static IEnumerable<T> AppendElement<T>(this IEnumerable<T> Self, T Element)
        {
            foreach (var I in Self)
            {
                yield return I;
            }

            yield return Element;
        }

        public static IEnumerable<T> PrependElement<T>(this IEnumerable<T> Self, T Element)
        {
            yield return Element;
            foreach (var I in Self)
            {
                yield return I;
            }
        }

        public static IEnumerable<T> ConcatBack<T>(this IEnumerable<T> Self, IEnumerable<T> Other)
        {
            foreach (var I in Other)
            {
                yield return I;
            }
            foreach (var I in Self)
            {
                yield return I;
            }
        }

        public static bool Any(this IEnumerable<bool> Self)
        {
            foreach (var I in Self)
            {
                if (I)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool All(this IEnumerable<bool> Self)
        {
            foreach (var I in Self)
            {
                if (!I)
                {
                    return false;
                }
            }
            return true;
        }

        public static T MaxOrDefault<T>(this IEnumerable<T> Self, Func<T, int, double?> Selector)
        {
            var M = default(double?);
            var R = default(T);
            var Ind = 0;
            foreach (var I in Self)
            {
                var V = Selector.Invoke(I, Ind);
                if (V.HasValue & (!M.HasValue | (M > V)))
                {
                    M = V;
                    R = I;
                }
                Ind += 1;
            }
            return R;
        }

        public static T MinOrDefault<T>(this IEnumerable<T> Self, Func<T, int, double?> Selector)
        {
            var M = default(double?);
            var R = default(T);
            var Ind = 0;
            foreach (var I in Self)
            {
                var V = Selector.Invoke(I, Ind);
                if (V.HasValue & (!M.HasValue | (M > V)))
                {
                    M = V;
                    R = I;
                }
                Ind += 1;
            }
            return R;
        }

        public static T MaxOrDefault<T>(this IEnumerable<T> Self, Func<T, int, long?> Selector)
        {
            var M = default(long?);
            var R = default(T);
            var Ind = 0;
            foreach (var I in Self)
            {
                var V = Selector.Invoke(I, Ind);
                if (V.HasValue & (!M.HasValue | (M > V)))
                {
                    M = V;
                    R = I;
                }
                Ind += 1;
            }
            return R;
        }

        public static T MinOrDefault<T>(this IEnumerable<T> Self, Func<T, int, long?> Selector)
        {
            var M = default(long?);
            var R = default(T);
            var Ind = 0;
            foreach (var I in Self)
            {
                var V = Selector.Invoke(I, Ind);
                if (V.HasValue & (!M.HasValue | (M > V)))
                {
                    M = V;
                    R = I;
                }
                Ind += 1;
            }
            return R;
        }

        public static T PeekOrDefault<T>(this IPushPop<T> Self)
        {
            if (Self.CanPop())
            {
                return Self.Peek();
            }

            return default;
        }

        public static EnumerableCacher<T> AsCachedList<T>(this IEnumerable<T> Self)
        {
            return new EnumerableCacher<T>(Self);
        }

        public static IEnumerable<T> CatchExceptions<T>(this IEnumerable<T> self)
        {
            var e = self.GetEnumerator();
            while (true)
            {
                CNullable<T> c = default;
                try
                {
                    if (!e.MoveNext())
                    {
                        break;
                    }
                    c = e.Current;
                }
                catch
                { }

                if (c.HasValue)
                {
                    yield return c.Value;
                }
            }
        }
        #endregion

        #region Geometry Group
        #region Elementary
        public static Vector ToVector(this Size Self)
        {
            return new Vector(Self.Width, Self.Height);
        }

        public static Vector ToVector(this Point Self)
        {
            return new Vector(Self.X, Self.Y);
        }

        public static Size ToSize(this Vector Self)
        {
            return new Size(Self.X, Self.Y);
        }

        public static Size ToSizeSafe(this Vector Self)
        {
            if (Self.X < 0)
            {
                Self.X = 0;
            }

            if (Self.Y < 0)
            {
                Self.Y = 0;
            }

            return new Size(Self.X, Self.Y);
        }

        public static Point ToPoint(this Vector Self)
        {
            return new Point(Self.X, Self.Y);
        }
        #endregion

        #region RectangleFitting
        public static (Rect, bool?) GetLargestFitOf(this Rect Self, Size Size)
        {
            bool? Bl = null;

            if (Self.IsEmpty)
            {
                return (Rect.Empty, Bl);
            }

            if ((Size.Width == 0) & (Size.Height == 0))
            {
                return (new Rect(Self.Location + (Self.Size.ToVector() / 2), new Size()), Bl);
            }

            if (Size.Width == 0)
            {
                if (Self.Height == 0)
                {
                    return (new Rect(Self.Location + (Self.Size.ToVector() / 2), new Size()), Bl);
                }

                Bl = false;
            }
            if (Size.Height == 0)
            {
                if (Self.Width == 0)
                {
                    return (new Rect(Self.Location + (Self.Size.ToVector() / 2), new Size()), Bl);
                }

                Bl = true;
            }
            if ((Self.Width == 0) | (Self.Height == 0))
            {
                return (new Rect(Self.Location + (Self.Size.ToVector() / 2), new Size()), Bl);
            }

            if (!Bl.HasValue)
            {
                var R1 = Self.Width / Self.Height;
                var R2 = Size.Width / Size.Height;
                Bl = R1 < R2;
            }

            var Loc = Self.Location;
            if (Bl.Value)
            {
                var Sz = new Size(Self.Width, (Self.Width / Size.Width) * Size.Height);
                Loc += new Vector(0, (Self.Height - Sz.Height) / 2);
                return (new Rect(Loc, Sz), Bl);
            }
            else
            {
                var Sz = new Size((Self.Height / Size.Height) * Size.Width, Self.Height);
                Loc += new Vector((Self.Width - Sz.Width) / 2, 0);
                return (new Rect(Loc, Sz), Bl);
            }
        }

        public static (Rect, bool?) GetSmallestBoundOf(this Rect Self, Size Size)
        {
            bool? Bl = null;

            if (Self.IsEmpty)
            {
                return (Rect.Empty, Bl);
            }

            if (Self.Size == new Size())
            {
                return (new Rect(Self.Location, new Size()), Bl);
            }

            if (Self.Width == 0)
            {
                if (Size.Height == 0)
                {
                    return (Rect.Empty, Bl);
                }

                Bl = false;
            }
            if (Self.Height == 0)
            {
                if (Size.Width == 0)
                {
                    return (Rect.Empty, Bl);
                }

                Bl = true;
            }
            if ((Size.Width == 0) | (Size.Height == 0))
            {
                return (Rect.Empty, Bl);
            }

            if (!Bl.HasValue)
            {
                var R1 = Self.Width / Self.Height;
                var R2 = Size.Width / Size.Height;
                Bl = R1 > R2;
            }

            var Loc = Self.Location;
            if (Bl.Value)
            {
                var Sz = new Size(Self.Width, (Self.Width / Size.Width) * Size.Height);
                Loc += new Vector(0, (Self.Height - Sz.Height) / 2);
                return (new Rect(Loc, Sz), Bl);
            }
            else
            {
                var Sz = new Size((Self.Height / Size.Height) * Size.Width, Self.Height);
                Loc += new Vector((Self.Width - Sz.Width) / 2, 0);
                return (new Rect(Loc, Sz), Bl);
            }
        }

        public static Rect GetInnerBoundedSquare(this Rect Self)
        {
            throw new NotImplementedException();
        }

        public static Rect GetOuterBoundingSquare(this Rect Self)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ChangeCoordinateSystem Logic
        public static Point FromLocal(this Rect Self, Point Point)
        {
            return Point + Self.Location.ToVector();
        }

        public static Point ToLocal(this Rect Self, Point Point)
        {
            return Point - Self.Location.ToVector();
        }

        public static Point FromLocal01(this Rect Self, Point Point)
        {
            Point = new Point(Point.X * Self.Width, Point.Y * Self.Height);
            Point += Self.Location.ToVector();
            return Point;
        }

        public static Point ToLocal01(this Rect Self, Point Point)
        {
            Point -= Self.Location.ToVector();
            Point = new Point(Point.X / Self.Width, Point.Y / Self.Height);
            return Point;
        }
        #endregion

        public static Point GetCenter(this Rect Self)
        {
            return Self.Location + (Self.Size.ToVector() / 2);
        }

        public static void MoveCenter(this Rect Self, Point Center)
        {
            Self.Location = Center - (Self.Size.ToVector() / 2);
        }
        #endregion

        #region Reflection Group
        /// <summary>
        /// First element is the given type.
        /// </summary>
        public static IEnumerable<Type> GetBaseTypes(this Type Self)
        {
            do
            {
                yield return Self;
                Self = Self.BaseType;
            } while (Self != null);
        }

        public static Type GetGenericTypeDef(this Type self)
        {
            return self.IsGenericType ? self.GetGenericTypeDefinition() : self;
        }

        public static IEnumerable<System.Reflection.Assembly> GetRecursiveReferencedAssemblies(this System.Reflection.Assembly Assembly)
        {
            var res = new HashSet<System.Reflection.Assembly>() { Assembly };

            IEnumerable<System.Reflection.Assembly> recurse(System.Reflection.Assembly assembly)
            {
                var referenceds = assembly.GetAllReferencedAssemblies().Where(res.Add).AsCachedList();
                return referenceds.Concat(referenceds.SelectMany(recurse));
            }

            return recurse(Assembly).PrependElement(Assembly);
        }

        public static IEnumerable<System.Reflection.Assembly> GetAllReferencedAssemblies(this System.Reflection.Assembly Assembly)
        {
            return Assembly.GetReferencedAssemblies().Select(System.Reflection.Assembly.Load);
        }

        public static object CreateInstance(this Type Self)
        {
            return Self.GetConstructor(Utilities.Typed<Type>.EmptyArray).Invoke(Utilities.Typed<object>.EmptyArray);
        }

        public static object CreateInstance<T1>(this Type Self, T1 Arg1)
        {
            return Self.GetConstructor(new[] { typeof(T1) }).Invoke(new object[] { Arg1 });
        }

        public static object CreateInstance<T1, T2>(this Type Self, T1 Arg1, T2 Arg2)
        {
            return Self.GetConstructor(new[] { typeof(T1), typeof(T2) }).Invoke(new object[] { Arg1, Arg2 });
        }

        public static object CreateInstance<T1, T2, T3>(this Type Self, T1 Arg1, T2 Arg2, T3 Arg3)
        {
            return Self.GetConstructor(new[] { typeof(T1), typeof(T2), typeof(T3) }).Invoke(new object[] { Arg1, Arg2, Arg3 });
        }

        public static object CreateInstance<T1, T2, T3, T4>(this Type Self, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4)
        {
            return Self.GetConstructor(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }).Invoke(new object[] { Arg1, Arg2, Arg3, Arg4 });
        }

        public static object RunStaticMethod(this Type Self, string Name)
        {
            return Self.GetMethod(Name, Utilities.Typed<Type>.EmptyArray).Invoke(null, Utilities.Typed<object>.EmptyArray);
        }

        public static object RunStaticMethod<T1>(this Type Self, string Name, T1 Arg1)
        {
            return Self.GetMethod(Name, new[] { typeof(T1) }).Invoke(null, new object[] { Arg1 });
        }

        public static object RunStaticMethod<T1, T2>(this Type Self, string Name, T1 Arg1, T2 Arg2)
        {
            return Self.GetMethod(Name, new[] { typeof(T1), typeof(T2) }).Invoke(null, new object[] { Arg1, Arg2 });
        }

        public static object RunStaticMethod<T1, T2, T3>(this Type Self, string Name, T1 Arg1, T2 Arg2, T3 Arg3)
        {
            return Self.GetMethod(Name, new[] { typeof(T1), typeof(T2), typeof(T3) }).Invoke(null, new object[] { Arg1, Arg2, Arg3 });
        }

        public static object RunStaticMethod<T1, T2, T3, T4>(this Type Self, string Name, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4)
        {
            return Self.GetMethod(Name, new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }).Invoke(null, new object[] { Arg1, Arg2, Arg3, Arg4 });
        }

        public static object RunMethod(this object Self, string Name)
        {
            return Self.GetType().GetMethod(Name, Utilities.Typed<Type>.EmptyArray).Invoke(Self, Utilities.Typed<object>.EmptyArray);
        }

        public static object RunMethod<T1>(this object Self, string Name, T1 Arg1)
        {
            return Self.GetType().GetMethod(Name, new[] { typeof(T1) }).Invoke(Self, new object[] { Arg1 });
        }

        public static object RunMethod<T1, T2>(this object Self, string Name, T1 Arg1, T2 Arg2)
        {
            return Self.GetType().GetMethod(Name, new[] { typeof(T1), typeof(T2) }).Invoke(Self, new object[] { Arg1, Arg2 });
        }

        public static object RunMethod<T1, T2, T3>(this object Self, string Name, T1 Arg1, T2 Arg2, T3 Arg3)
        {
            return Self.GetType().GetMethod(Name, new[] { typeof(T1), typeof(T2), typeof(T3) }).Invoke(Self, new object[] { Arg1, Arg2, Arg3 });
        }

        public static object RunMethod<T1, T2, T3, T4>(this object Self, string Name, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4)
        {
            return Self.GetType().GetMethod(Name, new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }).Invoke(Self, new object[] { Arg1, Arg2, Arg3, Arg4 });
        }

        public static T GetStaticFieldValue<T>(this Type Self, string Name)
        {
            return (T) Self.GetField(Name).GetValue(null);
        }

        public static object GetStaticFieldValue(this Type Self, string Name)
        {
            return Self.GetField(Name).GetValue(null);
        }

        public static T GetFieldValue<T>(this object Self, string Name)
        {
            return (T) Self.GetType().GetField(Name).GetValue(Self);
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this System.Reflection.MemberInfo self, bool inherit)
        {
            return self.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this System.Reflection.Assembly self, bool inherit)
        {
            return self.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }

        public static Attribute GetCustomAttribute(this System.Reflection.MemberInfo Self, Type AttributeType, bool Inherit)
        {
            var Usage = AttributeType.GetCustomAttributes<AttributeUsageAttribute>(true).SingleOrDefault();
            if (Usage != null && Usage.AllowMultiple)
            {
                throw new ArgumentException("The attribute should not allow multiple.");
            }

            return (Attribute) Self.GetCustomAttributes(AttributeType, Inherit).SingleOrDefault();
        }

        public static TAttribute GetCustomAttribute<TAttribute>(this System.Reflection.Assembly Self, bool Inherit) where TAttribute : Attribute
        {
            var AttributeType = typeof(TAttribute);
            var Usage = AttributeType.GetCustomAttributes<AttributeUsageAttribute>(true).SingleOrDefault();
            if (Usage != null && Usage.AllowMultiple)
            {
                throw new ArgumentException("The attribute should not allow multiple.");
            }

            return Self.GetCustomAttributes<TAttribute>(Inherit).SingleOrDefault();
        }

        public static Attribute GetCustomAttribute(this System.Reflection.Assembly Self, Type AttributeType, bool Inherit)
        {
            var Usage = AttributeType.GetCustomAttributes<AttributeUsageAttribute>(true).SingleOrDefault();
            if (Usage != null && Usage.AllowMultiple)
            {
                throw new ArgumentException("The attribute should not allow multiple.");
            }

            return (Attribute) Self.GetCustomAttributes(AttributeType, Inherit).SingleOrDefault();
        }

        public static IEnumerable<(Type Type, TAttribute Attribute)> WithCustomAttribute<TAttribute>(this IEnumerable<Type> Types, bool Inherit) where TAttribute : Attribute
        {
            var Type = typeof(TAttribute);

            var Usage = Type.GetCustomAttributes<AttributeUsageAttribute>(true).SingleOrDefault();
            if (Usage != null)
            {
                if (Usage.AllowMultiple)
                {
                    throw new ArgumentException("The attribute should not allow multiple.");
                }

                if ((Usage.ValidOn & (AttributeTargets.Class | AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.GenericParameter | AttributeTargets.Interface | AttributeTargets.Module | AttributeTargets.Struct)) == 0)
                {
                    throw new ArgumentException("The attribute is not valid on types.");
                }
            }

            foreach (var T in Types)
            {
                var Attribute = T.GetCustomAttributes<TAttribute>(Inherit).SingleOrDefault();
                if (Attribute != null)
                {
                    yield return (T, Attribute);
                }
            }
        }

        public static IEnumerable<(System.Reflection.MethodInfo Method, TAttribute Attribute)> WithCustomAttribute<TAttribute>(this IEnumerable<System.Reflection.MethodInfo> Methods, bool Inherit) where TAttribute : Attribute
        {
            var Type = typeof(TAttribute);

            var Usage = Type.GetCustomAttributes<AttributeUsageAttribute>(true).SingleOrDefault();
            if (Usage != null)
            {
                if (Usage.AllowMultiple)
                {
                    throw new ArgumentException("The attribute should not allow multiple.");
                }

                if ((Usage.ValidOn & AttributeTargets.Method) != AttributeTargets.Method)
                {
                    throw new ArgumentException("The attribute is not valid on methods.");
                }
            }

            foreach (var M in Methods)
            {
                var Attribute = M.GetCustomAttributes<TAttribute>(Inherit).SingleOrDefault();
                if (Attribute != null)
                {
                    yield return (M, Attribute);
                }
            }
        }

        public static System.Reflection.MemberInfo GetAccessedMemberInfo(this LambdaExpression self)
        {
            var parameter = self.Parameters.Single();

            var exp = self.Body;
            if (exp.NodeType == ExpressionType.Convert || exp.NodeType == ExpressionType.ConvertChecked)
            {
                exp = ((UnaryExpression) exp).Operand;
            }

            if (exp.NodeType == ExpressionType.MemberAccess || exp.NodeType == ExpressionType.Index)
            {
                var access = (MemberExpression) exp;
                Verify.True(access.Expression == parameter, "Expression must consist of a single member access.");
                return access.Member;
            }

            throw new NotSupportedException();
        }
        #endregion

        #region IO Group
        public static int ReadAll(this System.IO.Stream Self, byte[] Buffer, int Offset, int Length)
        {
            return Utilities.IO.ReadAll((B, O, L) => Self.Read(B, O, L), Buffer, Offset, Length);
        }

        public static byte[] ReadToEnd(this System.IO.Stream Self, Action<int> ProgressCallback = null)
        {
            // Dim Length = -1
            // If Self.CanSeek Then
            // Length = Self.Length - Self.Position
            // End If
            // ToDo Use Length to optimize this code.

            var Arrs = new List<byte[]>();

            const int BufLength = 8192;

            var TotalN = 0;
            int N;
            byte[] Buf;

            while (true)
            {
                Buf = new byte[BufLength];
                N = 0;
                while (true)
                {
                    var T = Self.Read(Buf, N, Buf.Length - N);
                    if (T == 0)
                    {
                        break;
                    }

                    TotalN += T;
                    N += T;
                    ProgressCallback?.Invoke(TotalN);
                }
                if (N != BufLength)
                {
                    break;
                }

                Arrs.Add(Buf);
            }

            var Res = new byte[TotalN];
            var Offset = 0;
            foreach (var A in Arrs)
            {
                A.CopyTo(Res, Offset);
                Offset += BufLength;
            }
            Array.Copy(Buf, 0, Res, Offset, N);

            return Res;
        }

        public static int Write(this System.IO.Stream Self, System.IO.Stream Stream, long Start = -1, long Length = -1, Action<int> ProgressCallback = null)
        {
            var Buffer = new byte[65536];

            if (Start != -1)
            {
                Stream.Seek(Start, System.IO.SeekOrigin.Begin);
            }

            var TotalN = 0;
            while (true)
            {
                int N;
                if (Length == -1)
                {
                    N = Stream.Read(Buffer, 0, Buffer.Length);
                }
                else
                {
                    N = Stream.Read(Buffer, 0, (int) Math.Min(Length, Buffer.Length));
                }

                if (N == 0)
                {
                    break;
                }

                Self.Write(Buffer, 0, N);
                TotalN += N;
                ProgressCallback?.Invoke(TotalN);
                if (Length != -1)
                {
                    Length -= N;
                    if (Length == 0)
                    {
                        break;
                    }
                }
            }

            return TotalN;
        }

        public static string GetRegexMatch(this System.IO.TextReader Self, Regex Regex)
        {
            var Text = new System.Text.StringBuilder();
            var Buffer = new char[256];

            while (!Regex.IsMatch(Text.ToString()))
            {
                var N = Self.Read(Buffer, 0, Buffer.Length);
                if (N == 0)
                {
                    break;
                }

                Text.Append(Buffer, 0, N);
            }

            return Regex.Match(Text.ToString())?.Value ?? null;
        }

        public static string GetRegexMatch(this System.IO.TextReader Self, string RegexPattern)
        {
            return Self.GetRegexMatch(new Regex(RegexPattern, RegexOptions.Singleline));
        }
        #endregion

        public static byte[] ComputeHash(this System.Security.Cryptography.HashAlgorithm Self, byte[] Data, int Index, int Length, byte[] Result = null)
        {
            using var Stream = Result == null ? new System.IO.MemoryStream() : new System.IO.MemoryStream(Result);
            using (var CryptoStream = new System.Security.Cryptography.CryptoStream(Stream, Self, System.Security.Cryptography.CryptoStreamMode.Write))
            {
                CryptoStream.Write(Data, Index, Length);
            }
            return Stream.ToArray();
        }

        public static byte[] ComputeHash(this System.Security.Cryptography.HashAlgorithm Self, byte[] Data, byte[] Result = null)
        {
            return Self.ComputeHash(Data, 0, Data.Length, Result);
        }

        public static byte[] ComputeHash(this System.Security.Cryptography.HashAlgorithm Self, string Data, byte[] Result = null)
        {
            return Self.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Data), Result);
        }

        public static T NothingIfEmpty<T>(this T Self) where T : ICollection
        {
            if (Self.Count == 0)
            {
                return default;
            }

            return Self;
        }

        public static string NothingIfEmpty(this string Self)
        {
            if (Self.Length == 0)
            {
                return null;
            }

            return Self;
        }

        public static string GetCSharpString(this AccessModifier accessModifier)
        {
            return accessModifier switch
            {
                AccessModifier.Public => "public",
                AccessModifier.Private => "private",
                AccessModifier.Protected => "protected",
                AccessModifier.Internal => "internal",
                AccessModifier.ProtectedInternal => "protected internal",
                AccessModifier.PrivateProtected => "private protected",
                _ => throw new NotSupportedException(),
            };
        }

        public static bool Implies(this bool B1, bool B2)
        {
            return !B1 | B2;
        }

        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kv, out TKey key, out TValue value)
        {
            key = kv.Key;
            value = kv.Value;
        }

        public static void Deconstruct<T>(this T[] arr, out T e0)
        {
            Verify.True(arr.Length == 1);
            e0 = arr[0];
        }

        public static void Deconstruct<T>(this T[] arr, out T e0, out T e1)
        {
            Verify.True(arr.Length == 2);
            e0 = arr[0];
            e1 = arr[1];
        }

        public static void Deconstruct<T>(this T[] arr, out T e0, out T e1, out T e2)
        {
            Verify.True(arr.Length == 3);
            e0 = arr[0];
            e1 = arr[1];
            e2 = arr[2];
        }

        public static void Deconstruct<T>(this T[] arr, out T e0, out T e1, out T e2, out T e3)
        {
            Verify.True(arr.Length == 4);
            e0 = arr[0];
            e1 = arr[1];
            e2 = arr[2];
            e3 = arr[3];
        }

        public static void Deconstruct<T>(this T[] arr, out T e0, out T e1, out T e2, out T e3, out T e4)
        {
            Verify.True(arr.Length == 5);
            e0 = arr[0];
            e1 = arr[1];
            e2 = arr[2];
            e3 = arr[3];
            e4 = arr[4];
        }
    }

    public delegate int Comparison<in T1, in T2>(T1 x, T2 y);

}
