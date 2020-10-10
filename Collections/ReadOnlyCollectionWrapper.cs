using System.Collections.Generic;
using System.Collections;
using System;

namespace Ks.Common
{
        public class ReadOnlyCollectionWrapper<T> : ICollection, ICollection<T>, IReadOnlyCollection<T>
        {
            public ReadOnlyCollectionWrapper(ICollection<T> Collection)
            {
                this.Collection = Collection;
            }

            public int Count
            {
                get
                {
                    return this.Collection.Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            public bool IsSynchronized
            {
                get
                {
                    return true;
                }
            }

            public object SyncRoot
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

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
                this.Collection.CopyTo(array, arrayIndex);
            }

            public void CopyTo(Array array, int index)
            {
                foreach (var I in this.Collection)
                {
                    array.SetValue(I, index);
                    index += 1;
                }
            }

            public bool Contains(T item)
            {
                return this.Collection.Contains(item);
            }

            public IEnumerator GetEnumerator()
            {
                return this.Collection.GetEnumerator();
            }

            public bool Remove(T item)
            {
                throw new NotSupportedException();
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                return this.Collection.GetEnumerator();
            }

            private readonly ICollection<T> Collection;
        }
    }
