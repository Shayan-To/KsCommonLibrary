using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Collections.Specialized;

namespace Ks.Common
{
    public class ComplementingCollectionMaster<T> : INotifyingCollection<T>
    {
        public ComplementingCollectionMaster()
        {
            this._Collection1 = new ChildComplementCollection<T>(this);
            this._Collection2 = new ChildComplementCollection<T>(this);
        }

        private readonly ChildComplementCollection<T> _Collection1;

        public INotifyingCollection<T> Collection1
        {
            get
            {
                return this._Collection1;
            }
        }

        private readonly ChildComplementCollection<T> _Collection2;

        public INotifyingCollection<T> Collection2
        {
            get
            {
                return this._Collection2;
            }
        }

        public event NotifyCollectionChangedEventHandler<T> CollectionChanged;
        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add => this.INotifyCollectionChanged_CollectionChanged += value;
            remove => this.INotifyCollectionChanged_CollectionChanged -= value;
        }
        private event NotifyCollectionChangedEventHandler INotifyCollectionChanged_CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs<T> E)
        {
            CollectionChanged?.Invoke(this, E);
            INotifyCollectionChanged_CollectionChanged?.Invoke(this, E);
        }

        public int Count
        {
            get
            {
                return this._Collection1.Count + this._Collection2.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if ((array.Length - arrayIndex) < this.Count)
            {
                throw new ArgumentException("The number of elements in the source System.Collections.Generic.ICollection`1 is greater than the available space from arrayIndex to the end of the destination array.");
            }

            this._Collection1.CopyTo(array, arrayIndex);
            this._Collection2.CopyTo(array, arrayIndex + this._Collection1.Count);
        }

        public bool Contains(T item)
        {
            return this._Collection1.Contains(item) || this._Collection2.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this._Collection1.Concat(this._Collection2).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal void AddC(T Item, ChildComplementCollection<T> C1, ChildComplementCollection<T> C2 = null)
        {
            if (C2 == null)
            {
                if (C1 == this._Collection1)
                {
                    C2 = this._Collection2;
                }
                else
                {
                    C2 = this._Collection1;
                }
            }

            if (!C2.RemoveI(Item))
            {
                throw new ArgumentException();
            }

            C1.AddI(Item);

            C1.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateAdd(Item));
            C2.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateRemove(Item));
        }

        internal void ClearC(ChildComplementCollection<T> C1, ChildComplementCollection<T> C2 = null)
        {
            IList<T> C1Clone;

            if (C2 == null)
            {
                if (C1 == this._Collection1)
                {
                    C2 = this._Collection2;
                }
                else
                {
                    C2 = this._Collection1;
                }
            }

            C1Clone = new T[C1.Count];
            C1.CopyTo(C1Clone);

            C1.ClearI();

            foreach (var I in C1Clone)
            {
                C2.AddI(I);
                C2.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateAdd(I));
            }

            C1.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateReset());
        }

        internal bool RemoveC(T Item, ChildComplementCollection<T> C1, ChildComplementCollection<T> C2 = null)
        {
            if (C2 == null)
            {
                if (C1 == this._Collection1)
                {
                    C2 = this._Collection2;
                }
                else
                {
                    C2 = this._Collection1;
                }
            }

            if (!C1.RemoveI(Item))
            {
                throw new ArgumentException();
            }

            C2.AddI(Item);

            C1.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateRemove(Item));
            C2.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateAdd(Item));

            return true;
        }

        public void Add(T item)
        {
            if (this._Collection1.Contains(item) || this._Collection2.Contains(item))
            {
                return;
            }

            this._Collection1.Add(item);
            this.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateAdd(item));
        }

        public void Clear()
        {
            this._Collection1.Clear();
            this._Collection2.Clear();
            this.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateReset());
        }

        public bool Remove(T item)
        {
            if (this._Collection1.Remove(item) || this._Collection2.Remove(item))
            {
                this.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateRemove(item));
                return true;
            }
            return false;
        }
    }

    internal class ChildComplementCollection<T> : INotifyingCollection<T>
    {
        internal ChildComplementCollection(ComplementingCollectionMaster<T> Master)
        {
            this.InnerList = new List<T>();
            this.Master = Master;
        }

        private readonly List<T> InnerList;

        public event NotifyCollectionChangedEventHandler<T> CollectionChanged;
        private event NotifyCollectionChangedEventHandler INotifyCollectionChanged_CollectionChanged;
        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add => this.INotifyCollectionChanged_CollectionChanged += value;
            remove => this.INotifyCollectionChanged_CollectionChanged -= value;
        }

        protected internal virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs<T> E)
        {
            CollectionChanged?.Invoke(this, E);
            INotifyCollectionChanged_CollectionChanged?.Invoke(this, E);
        }

        public ComplementingCollectionMaster<T> Master { get; }

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
                return false;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.InnerList.CopyTo(array, arrayIndex);
        }

        public bool Contains(T item)
        {
            return this.InnerList.Contains(item);
        }

        public List<T>.Enumerator GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal void AddI(T item)
        {
            this.InnerList.Add(item);
        }

        internal void ClearI()
        {
            this.InnerList.Clear();
        }

        internal bool RemoveI(T item)
        {
            return this.InnerList.Remove(item);
        }

        public void Add(T Item)
        {
            this.Master.AddC(Item, this);
        }

        public void Clear()
        {
            this.Master.ClearC(this);
        }

        public bool Remove(T Item)
        {
            return this.Master.RemoveC(Item, this);
        }
    }
}
