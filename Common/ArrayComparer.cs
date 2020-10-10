using System.Collections.Generic;
using System;

namespace Ks.Common
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
            var count = Math.Min(x.Length, y.Length);
            for (var I = 0; I < count; I++)
            {
                var T = this.Comparer.Compare(x[I], y[I]);
                if (T != 0)
                {
                    return T;
                }
            }
            return x.Length - y.Length;
        }

        public bool Equals(T[] x, T[] y)
        {
            var count = Math.Min(x.Length, y.Length);
            for (var I = 0; I < count; I++)
            {
                if (!this.EqualityComparer.Equals(x[I], y[I]))
                {
                    return false;
                }
            }
            return x.Length == y.Length;
        }

        public int GetHashCode(T[] obj)
        {
            var R = unchecked((int) 0xFAB43DC8);
            for (var I = 0; I < obj.Length; I++)
            {
                R = Utilities.CombineHashCodes(R, this.EqualityComparer.GetHashCode(obj[I]));
            }

            return R;
        }

        private readonly IComparer<T> Comparer;
        private readonly IEqualityComparer<T> EqualityComparer;
    }
}
