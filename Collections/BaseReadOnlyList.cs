using System;
using System.Collections;
using System.Collections.Generic;

namespace Ks.Common
{
    public abstract class BaseReadOnlyList<T> : IReadOnlyList<T>, IList<T>, IList
    {
        public abstract int Count { get; }

        public abstract T this[int Index] { get; }

        // ToDo Make this protected _GetEnumerable
        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
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

        T IList<T>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return true;
            }
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            return this.IndexOf(item) != -1;
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException();
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
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }
    }
}
