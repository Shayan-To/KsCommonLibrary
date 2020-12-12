using System.Collections.Generic;

namespace Ks.Common
{
    public class ReferenceEqualityComparer<T> : EqualityComparer<T> where T : class
    {
        public override bool Equals(T x, T y)
        {
            return x == y;
        }

        public override int GetHashCode(T obj)
        {
            return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
        }

        private static readonly ReferenceEqualityComparer<T> _Instance = new ReferenceEqualityComparer<T>();

        public static ReferenceEqualityComparer<T> Instance
        {
            get
            {
                return _Instance;
            }
        }
    }
}
