using System.Collections.Generic;
using System.Collections;
using System;

namespace Ks
{
    namespace Common
    {
        public abstract class BaseList<T> : IReadOnlyList<T>, IList<T>, IList
        {
            public abstract void Insert(int index, T item);

            public abstract void RemoveAt(int index);

            public abstract void Clear();

            public abstract T this[int index] { get; set; }

            public abstract int Count { get; private set; }

            protected abstract IEnumerator<T> IEnumerable_1_GetEnumerator();

            public virtual int IndexOf(T item)
            {
                var loopTo = this.Count - 1;
                for (int I = 0; I <= loopTo; I++)
                {
                    if (object.Equals(item, this[I]))
                        return I;
                }
                return -1;
            }

            public virtual void CopyTo(T[] array, int arrayIndex)
            {
                this.CopyTo((Array)array, arrayIndex);
            }

            protected virtual void CopyTo(Array array, int index)
            {
                Verify.TrueArg(array.Rank == 1, nameof(array), "Array's rank must be 1.");
                Verify.TrueArg((index + this.Count) <= array.Length, nameof(array), "Array does not have enough length to copy the collection.");
                var loopTo = this.Count - 1;
                for (int I = 0; I <= loopTo; I++)
                {
                    array.SetValue(this[I], index);
                    index += 1;
                }
            }

            public virtual bool Remove(T item)
            {
                var I = this.IndexOf(item);
                if (I == -1)
                    return false;

                this.RemoveAt(I);
                return true;
            }

            private object IList_Item
            {
                get
                {
                    return this[index];
                }
                set
                {
                    this[index] = (T)value;
                }
            }

            private T IReadOnlyList_Item
            {
                get
                {
                    return this[index];
                }
            }

            protected virtual bool IList_IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            private bool IList_IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            private object IList_SyncRoot
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            private bool IList_IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            public bool Contains(T item)
            {
                return this.IndexOf(item) != -1;
            }

            private int IList_Add(object value)
            {
                this.Add((T)value);
                return this.Count - 1;
            }

            private bool IList_Contains(object value)
            {
                return this.Contains((T)value);
            }

            private int IList_IndexOf(object value)
            {
                return this.IndexOf((T)value);
            }

            private void IList_Insert(int index, object value)
            {
                this.Insert(index, (T)value);
            }

            private void IList_Remove(object value)
            {
                this.Remove((T)value);
            }

            public virtual void Add(T item)
            {
                this.Insert(this.Count, item);
            }

            private IEnumerator IEnumerable_GetEnumerator()
            {
                return this.IEnumerable_1_GetEnumerator();
            }
        }
    }
}
