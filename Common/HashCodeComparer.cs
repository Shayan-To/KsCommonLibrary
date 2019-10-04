using System.Collections.Generic;
using System.Collections;

namespace Ks
{
    public class HashCodeComparer<T> : IComparer, IComparer<T>, IEqualityComparer, IEqualityComparer<T>
    {
        public int Compare(T x, T y)
        {
            return x.GetHashCode().CompareTo(y.GetHashCode());
        }

        public new int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        public new bool Equals(T x, T y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        private int Compare(object x, object y)
        {
            return this.Compare((T)x, (T)y);
        }

        private new int GetHashCode(object obj)
        {
            return this.GetHashCode((T)obj);
        }

        private new bool Equals(object x, object y)
        {
            var TX = (T)x;
            var TY = (T)y;

            if (TX == null | TY == null)
                return x == null & y == null;

            return this.Equals(TX, TY);
        }
    }
}
