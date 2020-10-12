using System.Collections;
using System.Collections.Generic;

namespace Ks
{
    public class HashCodeComparer<T> : IComparer, IComparer<T>, IEqualityComparer, IEqualityComparer<T>
    {
        public int Compare(T x, T y)
        {
            return x.GetHashCode().CompareTo(y.GetHashCode());
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        public bool Equals(T x, T y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        int IComparer.Compare(object x, object y)
        {
            return this.Compare((T) x, (T) y);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return this.GetHashCode((T) obj);
        }

        bool IEqualityComparer.Equals(object x, object y)
        {
            var TX = (T) x;
            var TY = (T) y;

            if (TX == null | TY == null)
            {
                return x == null & y == null;
            }

            return this.Equals(TX, TY);
        }
    }
}
