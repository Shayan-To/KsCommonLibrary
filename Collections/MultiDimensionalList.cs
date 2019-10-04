﻿using System.Collections.Generic;
using System.Collections;

namespace Ks
{
    namespace Common
    {
        public class MultiDimensionalList<T> : IEnumerable<T>
        {
            public MultiDimensionalList(params int[] Lengths)
            {
                this._Lengths = Lengths;
                this._Lengths_RO = Lengths.AsReadOnly();
                var Length = 1;
                foreach (var L in Lengths)
                    Length *= L;
                this.Arr = new T[Length - 1 + 1];
            }

            public IEnumerator<T> GetEnumerator()
            {
                return ((IList<T>)this.Arr).GetEnumerator();
            }

            private IEnumerator IEnumerable_GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private int GetIndex(params int[] Indexes)
            {
                Verify.True(Indexes.Length == this._Lengths.Length);
                var Index = 0;
                var loopTo = Indexes.Length - 1;
                for (int I = 0; I <= loopTo; I++)
                {
                    Verify.True((0 <= Indexes[I]) & (Indexes[I] < this._Lengths[I]));
                    Index *= this._Lengths[I];
                    Index += Indexes[I];
                }
                return Index;
            }

            public T Item
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
            private readonly IReadOnlyList<int> _Lengths_RO;

            public IReadOnlyList<int> Lengths
            {
                get
                {
                    return this._Lengths_RO;
                }
            }

            public int Lengths
            {
                get
                {
                    return this._Lengths[Index];
                }
            }

            private readonly T[] Arr;
        }
    }
}
