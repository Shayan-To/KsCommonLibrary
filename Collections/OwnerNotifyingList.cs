using System;
using System.Collections.Generic;

namespace Ks.Common
{
    public class OwnerNotifyingList<T> : BaseList<T>
    {
        public OwnerNotifyingList(Action<NotifyCollectionChangedEventArgs<T>> ChangedDelegate)
        {
            this.Base = new BaseList(ChangedDelegate);
        }

        public override T this[int index]
        {
            get => this.Base[index];
            set => this.Base[index] = value;
        }

        public override int Count
        {
            get
            {
                return this.Base.Count;
            }
        }

        public override void Insert(int index, T item)
        {
            this.Base.Insert(index, item);
        }

        public override void RemoveAt(int index)
        {
            this.Base.RemoveAt(index);
        }

        public override void Clear()
        {
            this.Base.Clear();
        }

        protected override IEnumerator<T> _GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Base.GetEnumerator();
        }

        private readonly BaseList Base;

        private class BaseList : NotifyingList<T>
        {
            public BaseList(Action<NotifyCollectionChangedEventArgs<T>> ChangedDelegate)
            {
                this.ChangedDelegate = ChangedDelegate;
            }

            protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs<T> E)
            {
                this.ChangedDelegate.Invoke(E);
            }

            private readonly Action<NotifyCollectionChangedEventArgs<T>> ChangedDelegate;
        }
    }
}
