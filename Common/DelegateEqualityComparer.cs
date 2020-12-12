using System.Collections.Generic;
using System;

namespace Ks.Common
{
    public class DelegateEqualityComparer<T> : EqualityComparer<T>
    {
        public DelegateEqualityComparer(Func<T, T, bool> EqualsDelegate, Func<T, int> GetHashCodeDelegate)
        {
            this.EqualsDelegate = EqualsDelegate;
            this.GetHashCodeDelegate = GetHashCodeDelegate;
        }

        public override bool Equals(T x, T y)
        {
            return this.EqualsDelegate.Invoke(x, y);
        }

        public override int GetHashCode(T obj)
        {
            return this.GetHashCodeDelegate.Invoke(obj);
        }

        private readonly Func<T, T, bool> EqualsDelegate;
        private readonly Func<T, int> GetHashCodeDelegate;
    }
}
