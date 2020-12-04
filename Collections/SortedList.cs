using System;
using System.Collections.Generic;

namespace Ks.Common
{
    public class SortedList<T> : BaseList<T>
    {

        public SortedList(IComparer<T> comparer)
            : this(comparer.Compare)
        {
        }

        public SortedList(Comparison<T> comparison)
        {
            this.Comparison = comparison;
        }

        public override T this[int index]
        {
            get => this.List[index];
            set => throw new NotSupportedException();
        }

        public override int Count => this.List.Count;

        public override void Clear()
        {
            this.List.Clear();
        }

        public int AddWithIndex(T item)
        {
            var (index, count) = this.BinarySearch(item);
            this.List.Insert(index + count, item);
            return index + count;
        }

        public void Replace(int oldItemIndex, T newItem)
        {
            var (itemIndex, itemCount) = this.BinarySearch(newItem);
            var insertIndex = itemIndex + itemCount;
            insertIndex -= oldItemIndex < insertIndex ? 1 : 0;
            this.List.Move(oldItemIndex, insertIndex);
            this.List[insertIndex] = newItem;
        }

        public void Replace(T oldItem, T newItem)
        {
            this.Replace(this.BinarySearch(oldItem).Index, newItem);
        }

        public override void Add(T item)
        {
            this.AddWithIndex(item);
        }

        public override void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public override void RemoveAt(int index)
        {
            this.List.RemoveAt(index);
        }

        public override int IndexOf(T item)
        {
            var (index, count) = this.BinarySearch(item);
            if (count == 0)
            {
                return -1;
            }
            return index;
        }

        public (int Index, int Count) BinarySearch(T item)
        {
            return this.List.BinarySearch(item, this.Comparison);
        }

        protected override IEnumerator<T> _GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public List<T>.Enumerator GetEnumerator()
        {
            return this.List.GetEnumerator();
        }

        private readonly List<T> List = new List<T>();
        private readonly Comparison<T> Comparison;

    }
}
