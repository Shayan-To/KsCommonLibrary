using System.Collections.Generic;
using System.Collections;
using System;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace Ks
{
    namespace Common
    {

        // ToDo Is this replacable by CreateInstance...?

        public class ListCollectionRA<T> : ListCollectionRA<T, List<T>>
        {
            public ListCollectionRA() : base(() =>
            {
                return new List<T>();
            })
            {
            }
        }

        public class ListCollectionRA<T, TList> : INotifyingCollection<TList>, IList<TList>, ISerializable where TList : IList<T>
        {
            private readonly List<TList> InnerList;
            private readonly Func<TList> ListSeeder;

            public ListCollectionRA(Func<TList> ListSeeder)
            {
                this.ListSeeder = ListSeeder;
                this.InnerList = new List<TList>();
            }

            public ListCollectionRA() : this(() => (TList)typeof(TList).CreateInstance())
            { }

            public TList this[int Index]
            {
                get
                {
                    this.EnsureFits(Index);
                    return this.InnerList[Index];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            protected ListCollectionRA(SerializationInfo info, StreamingContext context)
            {
                if ((info == null))
                    throw new ArgumentNullException(nameof(info));
            }

            protected void GetObjectData(SerializationInfo info, StreamingContext context)
            {
            }

            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                this.GetObjectData(info, context);
            }

            public event NotifyCollectionChangedEventHandler<TList> CollectionChanged;
            private event NotifyCollectionChangedEventHandler INotifyCollectionChanged_CollectionChanged;
            event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
            {
                add => this.INotifyCollectionChanged_CollectionChanged += value;
                remove => this.INotifyCollectionChanged_CollectionChanged -= value;
            }

            protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs<TList> E)
            {
                CollectionChanged?.Invoke(this, E);
                INotifyCollectionChanged_CollectionChanged?.Invoke(this, E);
            }

            private TList InstantiateList()
            {
                return this.ListSeeder.Invoke();
            }

            private void EnsureFits(int Index)
            {
                TList T;
                List<TList> NewItems;

                if (this.InnerList.Count <= Index)
                {
                    NewItems = new List<TList>();
                    for (var I = this.InnerList.Count; I <= Index; I++)
                    {
                        T = this.InstantiateList();
                        this.InnerList.Add(T);
                        NewItems.Add(T);
                    }

                    this.OnCollectionChanged(NotifyCollectionChangedEventArgs<TList>.CreateAdd(NewItems));
                }
            }

            public List<TList>.Enumerator GetEnumerator()
            {
                return this.InnerList.GetEnumerator();
            }

            IEnumerator<TList> IEnumerable<TList>.GetEnumerator()
            {
                return this.InnerList.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public int Count
            {
                get
                {
                    return this.InnerList.Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            public void CopyTo(TList[] Array, int ArrayIndex)
            {
                this.InnerList.CopyTo(Array, ArrayIndex);
            }

            public int IndexOf(TList item)
            {
                throw new NotSupportedException();
            }

            public void Insert(int index, TList item)
            {
                throw new NotSupportedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            public void Add(TList item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(TList item)
            {
                throw new NotSupportedException();
            }

            public bool Remove(TList item)
            {
                throw new NotSupportedException();
            }
        }
    }
}
