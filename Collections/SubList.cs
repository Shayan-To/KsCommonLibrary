using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
{
    public class SubList<T> : BaseList<T>
    {

        public SubList(IList<T> list, int index, int count)
        {
            Verify.True(index + count <= list.Count, "The given range does not fit inside the given list.");
            this.List = list;
            this.OffsetIndex = index;
            this._Count = count;
        }

        public override T this[int index]
        {
            get
            {
                Verify.TrueArg(0 <= index && index < this.Count, nameof(index), "Index out of range.");
                return this.List[this.OffsetIndex + index];
            }
            set
            {
                Verify.TrueArg(0 <= index && index < this.Count, nameof(index), "Index out of range.");
                this.List[this.OffsetIndex + index] = value;
            }
        }

        public override int Count => this._Count;

        public override void Insert(int index, T item)
        {
            Verify.TrueArg(0 <= index && index < this.Count, nameof(index), "Index out of range.");
            this.List.Insert(this.OffsetIndex + index, item);
            this._Count += 1;
        }

        public override void RemoveAt(int index)
        {
            Verify.TrueArg(0 <= index && index < this.Count, nameof(index), "Index out of range.");
            this.List.RemoveAt(this.OffsetIndex + index);
            this._Count -= 1;
        }

        public override void Clear()
        {
            for (; this._Count > 0; this._Count -= 1)
            {
                this.List.RemoveAt(this.OffsetIndex + this._Count - 1);
            }
        }

        protected override IEnumerator<T> _GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.List.Skip(this.OffsetIndex).Take(this.Count).GetEnumerator();
        }

        private readonly IList<T> List;
        private readonly int OffsetIndex;
        private int _Count;
    }
}
