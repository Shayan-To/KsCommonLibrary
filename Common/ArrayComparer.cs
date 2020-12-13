using System.Collections.Generic;
using System;

namespace Ks
{
    namespace Common
    {
        public class ArrayComparer<T> : IComparer<T[]>, IEqualityComparer<T[]>
        {
            public ArrayComparer() : this(System.Collections.Generic.Comparer<T>.Default, System.Collections.Generic.EqualityComparer<T>.Default)
            {
            }

            public ArrayComparer(IComparer<T> Comparer) : this(Comparer, System.Collections.Generic.EqualityComparer<T>.Default)
            {
            }

            public ArrayComparer(IComparer<T> Comparer, IEqualityComparer<T> EqualityComparer)
            {
                this.Comparer = Comparer;
                this.EqualityComparer = EqualityComparer;
            }

            public int Compare(T[] x, T[] y)
            {
                var loopTo = Math.Min(x.Length, y.Length) - 1;
                for (int I = 0; I <= loopTo; I++)
                {
                    var T = this.Comparer.Compare(x[I], y[I]);
                    if (T != 0)
                        return T;
                }
                return x.Length - y.Length;
            }

            public bool Equals(T[] x, T[] y)
            {
                var loopTo = Math.Min(x.Length, y.Length) - 1;
                for (int I = 0; I <= loopTo; I++)
                {
                    if (!this.EqualityComparer.Equals(x[I], y[I]))
                        return false;
                }
                return x.Length == y.Length;
            }

            public int GetHashCode(T[] obj)
            {
                var R = unchecked((int)0xFAB43DC8);
                var loopTo = obj.Length - 1;
                for (int I = 0; I <= loopTo; I++)
                    R = Utilities.CombineHashCodes(R, this.EqualityComparer.GetHashCode(obj[I]));
                return R;
            }

            private readonly IComparer<T> Comparer;
            private readonly IEqualityComparer<T> EqualityComparer;
        }
    }
}
