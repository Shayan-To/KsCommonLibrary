using System.Collections.Generic;
using System.Collections;
using System;

namespace Ks
{
    namespace Common
    {
        public abstract class BaseReadOnlyList<T> : IReadOnlyList<T>, IList<T>, IList
        {
            public abstract int Count { get; private set; }

            public abstract T this[int Index] { get; private set; }

            public abstract IEnumerator<T> GetEnumerator();

            private IEnumerator IEnumerable_GetEnumerator()
            {
                return this.GetEnumerator();
            }

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

            private T IList_1_Item
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

            private object IList_Item
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

            private bool IList_IsReadOnly
            {
                get
                {
                    return true;
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
                    return true;
                }
            }

            private void IList_Insert(int index, T item)
            {
                throw new NotSupportedException();
            }

            private void IList_RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            private void IList_Add(T item)
            {
                throw new NotSupportedException();
            }

            private void IList_Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(T item)
            {
                return this.IndexOf(item) != -1;
            }

            private bool IList_Remove(T item)
            {
                throw new NotSupportedException();
            }

            private int IList_Add(object value)
            {
                throw new NotSupportedException();
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
                throw new NotSupportedException();
            }

            private void IList_Remove(object value)
            {
                throw new NotSupportedException();
            }
        }
    }
}
