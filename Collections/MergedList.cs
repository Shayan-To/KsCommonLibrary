using System;
using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
{
    public class MergedList<T> : BaseReadOnlyList<T>
    {
        public MergedList(IEnumerable<IReadOnlyList<T>> Lists)
        {
            this.Lists = Lists.ToArray();
        }

        public MergedList(params IReadOnlyList<T>[] Lists) : this((IEnumerable<IReadOnlyList<T>>) Lists)
        {
        }

        public override int Count => this.Lists.Sum(L => L.Count);

        public override T this[int Index]
        {
            get
            {
                foreach (var L in this.Lists)
                {
                    var Count = L.Count;
                    if (Index < Count)
                    {
                        return L[Index];
                    }

                    Index -= Count;
                }
                throw new ArgumentOutOfRangeException(nameof(Index));
            }
        }

        public override void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var L in this.Lists)
            {
                L.CopyTo(array, arrayIndex);
                arrayIndex += L.Count;
            }
        }

        public override int IndexOf(T item)
        {
            var I = 0;
            foreach (var L in this.Lists)
            {
                foreach (var It in L)
                {
                    if (It.Equals(item))
                    {
                        return I;
                    }

                    I += 1;
                }
            }
            return -1;
        }

        public override IEnumerator<T> GetEnumerator()
        {
            foreach (var L in this.Lists)
            {
                foreach (var I in L)
                {
                    yield return I;
                }
            }
        }

        private readonly IReadOnlyList<T>[] Lists;
    }
}
