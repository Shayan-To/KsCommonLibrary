using System;
using System.Collections.Generic;

namespace Ks.Common
{
    partial class Utilities
    {
        public static class Collections
        {
            public static IEnumerable<T> GatherRecursiveDistinct<T>(IEnumerable<T> bases, Func<T, IEnumerable<T>> getChildren)
            {
                var set = new HashSet<T>();

                foreach (var i in process(default, true))
                {
                    yield return i;
                }

                IEnumerable<T> process(T item, bool isRoot = false)
                {
                    if (!isRoot && !set.Add(item))
                    {
                        yield break;
                    }

                    if (!isRoot)
                    {
                        yield return item;
                    }

                    foreach (var child in isRoot ? bases : getChildren(item))
                    {
                        foreach (var i in process(child))
                        {
                            yield return i;
                        }
                    }
                }
            }

            public static IEnumerable<T> GatherRecursiveAll<T>(IEnumerable<T> items, Func<T, IEnumerable<T>> getChildren)
            {
                foreach (var item in items)
                {
                    foreach (var i in GatherRecursiveAll(item, getChildren))
                    {
                        yield return i;
                    }
                }
            }

            public static IEnumerable<T> GatherRecursiveAll<T>(T item, Func<T, IEnumerable<T>> getChildren)
            {
                yield return item;
                foreach (var child in getChildren(item))
                {
                    foreach (var i in GatherRecursiveAll(child, getChildren))
                    {
                        yield return i;
                    }
                }
            }

            public static IEnumerable<T> Concat<T>(IEnumerable<IEnumerable<T>> Collections)
            {
                foreach (var L in Collections)
                {
                    foreach (var I in L)
                    {
                        yield return I;
                    }
                }
            }

            public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] Collections)
            {
                return Concat((IEnumerable<IEnumerable<T>>) Collections);
            }

            public static IEnumerable<int> Range(int Start, int Length, int Step = 1)
            {
                Verify.TrueArg(Step != 0, "Step");
                var End = Start + Length;
                for (; Start != End; Start += Step)
                {
                    yield return Start;
                }
            }

            public static IEnumerable<int> Range(int Length)
            {
                return Range(0, Length);
            }

            public static IEnumerable<T> Repeat<T>(T I1, int Count)
            {
                for (var I = 0; I < Count; I++)
                {
                    yield return I1;
                }
            }

            public static IEnumerable<Void> InfiniteEnumerable()
            {
                while (true)
                {
                    yield return default;
                }
            }

            public static IEnumerable<T> Generate<T>(T initialValue, Func<T, T> generator)
            {
                while (true)
                {
                    yield return initialValue;
                    initialValue = generator.Invoke(initialValue);
                }
            }

            public static JoinElement<T, T, TKey>[] Join<T, TKey>(IEnumerable<T> Items1, IEnumerable<T> Items2, Func<T, TKey> KeySelector, JoinDirection JoinType)
            {
                return Join(Items1, Items2, KeySelector, KeySelector, JoinType, EqualityComparer<TKey>.Default);
            }

            public static JoinElement<T, T, TKey>[] Join<T, TKey>(IEnumerable<T> Items1, IEnumerable<T> Items2, Func<T, TKey> KeySelector, JoinDirection JoinType, IEqualityComparer<TKey> Comparer)
            {
                return Join(Items1, Items2, KeySelector, KeySelector, JoinType, Comparer);
            }

            public static JoinElement<T1, T2, TKey>[] Join<T1, T2, TKey>(IEnumerable<T1> Items1, IEnumerable<T2> Items2, Func<T1, TKey> KeySelector1, Func<T2, TKey> KeySelector2, JoinDirection JoinType)
            {
                return Join(Items1, Items2, KeySelector1, KeySelector2, JoinType, EqualityComparer<TKey>.Default);
            }

            public static JoinElement<T1, T2, TKey>[] Join<T1, T2, TKey>(IEnumerable<T1> Items1, IEnumerable<T2> Items2, Func<T1, TKey> KeySelector1, Func<T2, TKey> KeySelector2, JoinDirection JoinType, IEqualityComparer<TKey> Comparer)
            {
                var Dic = new Dictionary<TKey, T2>(Comparer);
                foreach (var I in Items2)
                {
                    Dic.Add(KeySelector2.Invoke(I), I);
                }

                var Res = new List<JoinElement<T1, T2, TKey>>();

                foreach (var I1 in Items1)
                {
                    var Key = KeySelector1.Invoke(I1);
                    if (Dic.TryGetValue(Key, out var I2))
                    {
                        Res.Add(new JoinElement<T1, T2, TKey>(Key, JoinDirection.Both, I1, I2));
                        Dic.Remove(Key);
                    }
                    else if ((JoinType & JoinDirection.Left) == JoinDirection.Left)
                    {
                        Res.Add(new JoinElement<T1, T2, TKey>(Key, JoinDirection.Left, I1, default));
                    }
                }

                if ((JoinType & JoinDirection.Right) == JoinDirection.Right)
                {
                    foreach (var KI2 in Dic)
                    {
                        Res.Add(new JoinElement<T1, T2, TKey>(KI2.Key, JoinDirection.Left, default, KI2.Value));
                    }
                }

                return Res.ToArray();
            }

            public static IEnumerable<T> InEnumerable<T>()
            {
                yield break;
            }

            public static IEnumerable<T> InEnumerable<T>(T I1)
            {
                yield return I1;
            }

            public static IEnumerable<T> InEnumerable<T>(T I1, T I2)
            {
                yield return I1;
                yield return I2;
            }

            public static IEnumerable<T> InEnumerable<T>(T I1, T I2, T I3)
            {
                yield return I1;
                yield return I2;
                yield return I3;
            }

            public static IEnumerable<T> InEnumerable<T>(T I1, T I2, T I3, T I4)
            {
                yield return I1;
                yield return I2;
                yield return I3;
                yield return I4;
            }

            public static IEnumerable<T> InEnumerable<T>(T I1, T I2, T I3, T I4, T I5)
            {
                yield return I1;
                yield return I2;
                yield return I3;
                yield return I4;
                yield return I5;
            }

            public static IEnumerable<T> InEnumerable<T>(T I1, T I2, T I3, T I4, T I5, T I6)
            {
                yield return I1;
                yield return I2;
                yield return I3;
                yield return I4;
                yield return I5;
                yield return I6;
            }

            public static IEnumerable<T> InEnumerable<T>(T I1, T I2, T I3, T I4, T I5, T I6, T I7)
            {
                yield return I1;
                yield return I2;
                yield return I3;
                yield return I4;
                yield return I5;
                yield return I6;
                yield return I7;
            }

            public static IEnumerable<T> InEnumerable<T>(T I1, T I2, T I3, T I4, T I5, T I6, T I7, T I8)
            {
                yield return I1;
                yield return I2;
                yield return I3;
                yield return I4;
                yield return I5;
                yield return I6;
                yield return I7;
                yield return I8;
            }
        }
    }
}
