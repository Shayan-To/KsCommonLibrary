using System;
using System.Collections;
using System.Collections.Generic;

namespace Ks.Common
{
    public abstract class BaseList<T> : IReadOnlyList<T>, IList<T>, IList
    {
        public abstract void Insert(int index, T item);

        public abstract void RemoveAt(int index);

        public abstract void Clear();

        public abstract T this[int index] { get; set; }

        public abstract int Count { get; }

        protected abstract IEnumerator<T> _GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this._GetEnumerator();
        }
        public virtual int IndexOf(T item)
        {
            for (var I = 0; I < this.Count; I++)
            {
                if (object.Equals(item, this[I]))
                {
                    return I;
                }
            }
            return -1;
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            this.CopyTo((Array) array, arrayIndex);
        }

        protected virtual void CopyTo(Array array, int index)
        {
            Verify.TrueArg(array.Rank == 1, nameof(array), "Array's rank must be 1.");
            Verify.TrueArg((index + this.Count) <= array.Length, nameof(array), "Array does not have enough length to copy the collection.");
            for (var I = 0; I < this.Count; I++)
            {
                array.SetValue(this[I], index);
                index += 1;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo(array, index);
        }

        public virtual bool Remove(T item)
        {
            var I = this.IndexOf(item);
            if (I == -1)
            {
                return false;
            }

            this.RemoveAt(I);
            return true;
        }

        object IList.this[int index]
        {
            get => this[index];
            set => this[index] = (T) value;
        }

        T IReadOnlyList<T>.this[int index] => this[index];

        protected virtual bool IList_IsReadOnly => false;

        bool IList.IsReadOnly => this.IList_IsReadOnly;

        bool ICollection<T>.IsReadOnly => this.IList_IsReadOnly;

        bool IList.IsFixedSize => false;

        object ICollection.SyncRoot => throw new NotSupportedException();

        bool ICollection.IsSynchronized => false;

        public bool Contains(T item)
        {
            return this.IndexOf(item) != -1;
        }

        int IList.Add(object value)
        {
            this.Add((T) value);
            return this.Count - 1;
        }

        bool IList.Contains(object value)
        {
            return this.Contains((T) value);
        }

        int IList.IndexOf(object value)
        {
            return this.IndexOf((T) value);
        }

        void IList.Insert(int index, object value)
        {
            this.Insert(index, (T) value);
        }

        void IList.Remove(object value)
        {
            this.Remove((T) value);
        }

        public virtual void Add(T item)
        {
            this.Insert(this.Count, item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._GetEnumerator();
        }
    }
}
