using System.Collections;
using System.Collections.Generic;

namespace Ks.Common
{
    public class MultiDimensionalList<T> : IEnumerable<T>
    {
        public MultiDimensionalList(params int[] Lengths)
        {
            this._Lengths = Lengths;
            this.Lengths = Lengths.AsReadOnly();
            var Length = 1;
            foreach (var L in Lengths)
            {
                Length *= L;
            }

            this.Arr = new T[Length];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IList<T>) this.Arr).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private int GetIndex(params int[] Indexes)
        {
            Verify.True(Indexes.Length == this._Lengths.Length);
            var Index = 0;
            for (var I = 0; I < Indexes.Length; I++)
            {
                Verify.True((0 <= Indexes[I]) & (Indexes[I] < this._Lengths[I]));
                Index *= this._Lengths[I];
                Index += Indexes[I];
            }
            return Index;
        }

        public T this[params int[] Indexes]
        {
            get
            {
                return this.Arr[this.GetIndex(Indexes)];
            }
            set
            {
                this.Arr[this.GetIndex(Indexes)] = value;
            }
        }

        private readonly int[] _Lengths;

        public IReadOnlyList<int> Lengths { get; }

        private readonly T[] Arr;
    }
}
