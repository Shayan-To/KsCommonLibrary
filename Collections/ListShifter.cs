using System.Collections;
using System.Collections.Generic;

namespace Ks.Common
{
    public class ListShifter<T> : IList<T>
    {
        private readonly IList<T> InnerList;
        private readonly int Shift;

        public ListShifter(IList<T> List, int Shift)
        {
            this.InnerList = List;
            this.Shift = Shift;
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
                return this.InnerList.IsReadOnly;
            }
        }

        public T this[int index]
        {
            get
            {
                return this.InnerList[index + this.Shift];
            }
            set
            {
                this.InnerList[index + this.Shift] = value;
            }
        }

        public void Add(T item)
        {
            this.InnerList.Add(item);
        }

        public void Clear()
        {
            this.InnerList.Clear();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.InnerList.CopyTo(array, arrayIndex);
        }

        public void Insert(int index, T item)
        {
            this.InnerList.Insert(index + this.Shift, item);
        }

        public void RemoveAt(int index)
        {
            this.InnerList.RemoveAt(index + this.Shift);
        }

        public bool Contains(T item)
        {
            return this.InnerList.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return this.InnerList.IndexOf(item) - this.Shift;
        }

        public bool Remove(T item)
        {
            return this.InnerList.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
