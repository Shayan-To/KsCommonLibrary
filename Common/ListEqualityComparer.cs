﻿using System.Collections.Generic;

namespace Ks
{
    namespace Common
    {
        public class ListEqualityComparer<T> : EqualityComparer<IList<T>>
        {
            public ListEqualityComparer(IEqualityComparer<T> EqualityComparer)
            {
                this.EqualityComparer = EqualityComparer;
            }

            public ListEqualityComparer() : this(System.Collections.Generic.EqualityComparer<T>.Default)
            {
            }

            public override bool Equals(IList<T> x, IList<T> y)
            {
                if (x.Count != y.Count)
                    return false;
                var loopTo = x.Count - 1;
                for (int I = 0; I <= loopTo; I++)
                {
                    if (this.EqualityComparer.Equals(x[I], y[I]))
                        return false;
                }
                return true;
            }

            public override int GetHashCode(IList<T> obj)
            {
                var Hash = 0;
                var loopTo = obj.Count - 1;
                for (int I = 0; I <= loopTo; I++)
                    Hash = Utilities.CombineHashCodes(Hash, I, this.EqualityComparer.GetHashCode(obj[I]));
                return Hash;
            }

            private readonly IEqualityComparer<T> EqualityComparer;
        }
    }
}
