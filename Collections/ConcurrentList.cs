using System.Collections.Generic;

namespace Ks.Common
{
    public class ConcurrentList<T> : BaseList<T>
    {
        public ConcurrentList() : this(new List<T>(), new object())
        {
        }

        public ConcurrentList(IList<T> BaseList) : this(BaseList, new object())
        {
        }

        public ConcurrentList(IList<T> BaseList, object LockObject)
        {
            this.BaseList = BaseList;
            this.LockObject = LockObject;
        }

        public override int Count
        {
            get
            {
                lock (this.LockObject)
                {
                    return this.BaseList.Count;
                }
            }
        }

        public override T this[int index]
        {
            get
            {
                lock (this.LockObject)
                {
                    return this.BaseList[index];
                }
            }
            set
            {
                lock (this.LockObject)
                {
                    this.BaseList[index] = value;
                }
            }
        }

        public override void Clear()
        {
            lock (this.LockObject)
            {
                this.BaseList.Clear();
            }
        }

        public override void Insert(int index, T item)
        {
            lock (this.LockObject)
            {
                this.BaseList.Insert(index, item);
            }
        }

        public override void RemoveAt(int index)
        {
            lock (this.LockObject)
            {
                this.BaseList.RemoveAt(index);
            }
        }

        protected override IEnumerator<T> _GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (this.LockObject)
            {
                foreach (var I in this.BaseList)
                {
                    yield return I;
                }
            }
        }

        public object LockObject { get; }

        public IList<T> BaseList { get; }
    }
}
