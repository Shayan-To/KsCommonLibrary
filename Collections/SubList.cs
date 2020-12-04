using System.Collections.Generic;

namespace Ks.Common
{
    public class SubList<T> : BaseReadOnlyList<T>
    {

        public SubList(IReadOnlyList<T> list, int index, int count)
        {
            Verify.True(index + count <= list.Count, "The given range does not fit inside the given list.");
            this.List = list;
            this.OffsetIndex = index;
            this.Count = count;
        }

        public override T this[int index]
        {
            get
            {
                Verify.TrueArg(index < this.Count, nameof(index), "Index out of range.");
                return this.List[this.OffsetIndex + index];
            }
        }

        public override int Count { get; }

        public override IEnumerator<T> GetEnumerator()
        {
            return this.List.GetEnumerator();
        }

        private readonly IReadOnlyList<T> List;
        private readonly int OffsetIndex;
    }
}