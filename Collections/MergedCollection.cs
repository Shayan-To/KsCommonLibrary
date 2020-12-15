using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
{
    public class MergedCollection<T> : ICollection<T>, ICollection
    {
        public MergedCollection(IEnumerable<ICollection<T>> Collections)
        {
            this.Collections = Collections.ToArray();
        }

        public MergedCollection(params ICollection<T>[] Collections) : this((IEnumerable<ICollection<T>>) Collections)
        {
        }

        public int Count => this.Collections.Sum(L => L.Count);

        public bool IsReadOnly => true;

        int ICollection.Count => this.Count;

        public object SyncRoot => throw new NotSupportedException();

        public bool IsSynchronized => false;

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var L in this.Collections)
            {
                L.CopyTo(array, arrayIndex);
                arrayIndex += L.Count;
            }
        }

        public bool Contains(T item)
        {
            foreach (var L in this.Collections)
            {
                if (L.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var L in this.Collections)
            {
                foreach (var I in L)
                {
                    yield return I;
                }
            }
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            foreach (var L in this.Collections)
            {
                foreach (var I in L)
                {
                    array.SetValue(I, index);
                    index += 1;
                }
            }
        }

        private readonly ICollection<T>[] Collections;
    }
}
