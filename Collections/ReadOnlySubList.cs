using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
{
    public class ReadOnlySubList<T> : BaseReadOnlyList<T>
    {

        public ReadOnlySubList(IReadOnlyList<T> list, int index, int count)
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
            return this.List.Skip(this.OffsetIndex).Take(this.Count).GetEnumerator();
        }

        private readonly IReadOnlyList<T> List;
        private readonly int OffsetIndex;
    }
}
