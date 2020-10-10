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
            this.Start = Start;
            this.Step = Step;
            this.End = (End - Utilities.Math.PosMod(End - Start, Step)) + Step;
        }

        public int Start { get; }

        public int End { get; }

        public int Step { get; }

        public int this[int Index]
        {
            get
            {
                int Res;

                Res = this.Start + (Index * this.Step);

                if (Res >= this.End || Res < this.Start)
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
                return (this.End - this.Start) / this.Step;
            }
        }

        public int IndexOf(int item)
        {
            if (this.Contains(item))
                return (item - this.Start) / this.Step;
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(int Item)
        {
            return Item >= this.Start && Item < this.End && ((Item - this.Start) % this.Step) == 0;
        }

        public void CopyTo(int[] Array, int ArrayIndex)
        {
            for (var I = this.Start; I < this.End; I += this.Step)
            {
                Array[ArrayIndex] = I;
                ArrayIndex += 1;
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (var I = this.Start; I < this.End; I += this.Step)
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
