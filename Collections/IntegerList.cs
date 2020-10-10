using System.Collections.Generic;
using System.Collections;
using System;
using System.Runtime.CompilerServices;

namespace Ks.Common
{
        public class IntegerList : IList<int>
        {

            /// <param name="Start">The start of the sequence, inclusive.</param>
        /// <param name="End">The end of the sequence, exclusive.</param>
        /// <param name="Step">The step by which the sequence goes.</param>
            public IntegerList(int Start, int End, int Step = 1)
            {
                this._Start = Start;
                this._Step = Step;
                this._End = (End - Utilities.Math.PosMod(End - Start, Step)) + Step;
            }

            private readonly int _Start;

            public int Start
            {
                get
                {
                    return this._Start;
                }
            }

            private readonly int _End;

            public int End
            {
                get
                {
                    return this._End;
                }
            }

            private readonly int _Step;

            public int Step
            {
                get
                {
                    return this._Step;
                }
            }

            public int this[int Index]
            {
                get
                {
                    int Res;

                    Res = this._Start + (Index * this._Step);

                    if (Res >= this._End || Res < this._Start)
                        throw new ArgumentOutOfRangeException(nameof(Index));

                    return Res;
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            public int Count
            {
                get
                {
                    return (this._End - this._Start) / this._Step;
                }
            }

            public int IndexOf(int item)
            {
                if (this.Contains(item))
                    return (item - this._Start) / this._Step;
                return -1;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Contains(int Item)
            {
                return Item >= this._Start && Item < this._End && ((Item - this._Start) % this._Step) == 0;
            }

            public void CopyTo(int[] Array, int ArrayIndex)
            {
                for (var I = this._Start; I < this._End; I += this._Step)
                {
                    Array[ArrayIndex] = I;
                    ArrayIndex += 1;
                }
            }

            public IEnumerator<int> GetEnumerator()
            {
                for (var I = this._Start; I < this._End; I += this._Step)
                    yield return I;
            }

            public bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public void Insert(int index, int item)
            {
                throw new NotSupportedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            public void Add(int item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Remove(int item)
            {
                throw new NotSupportedException();
            }
        }
    }
