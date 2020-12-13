using System.Collections.Generic;
using System;

namespace Ks
{
    namespace Common
    {
        public class InsertionSortList<T> : BaseList<T>
        {
            public InsertionSortList() : this(System.Collections.Generic.Comparer<T>.Default)
            {
            }

            public InsertionSortList(IComparer<T> Comparer)
            {
                this.Comparer = Comparer;
            }

            public override void Insert(int index, T item)
            {
                throw new NotSupportedException();
            }

            public override void Add(T item)
            {
                throw new NotImplementedException();
                var I = this.List.First;
                do
                {
                    if (this.Comparer.Compare(I.Value, item) > 0)
                    {
                    }
                }
                while (true);
            }

            public override int IndexOf(T item)
            {
                throw new NotImplementedException();
            }

            public override void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            public override void Clear()
            {
                throw new NotImplementedException();
            }

            protected override IEnumerator<T> _GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public IEnumerator<T> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public override T this[int index]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public override int Count
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            private readonly IComparer<T> Comparer;
            private readonly LinkedList<T> List = new LinkedList<T>();
        }
    }
}
