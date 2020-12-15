using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ks.Common
{
    public class CastAsListCollection<T> : BaseReadOnlyList<T>
    {
        public CastAsListCollection(IList List)
        {
            this.List = List;
        }

        public override int Count => this.List.Count;

        public override T this[int Index] => (T) this.List[Index];

        public override IEnumerator<T> GetEnumerator()
        {
            return this.List.Cast<T>().GetEnumerator();
        }

        private readonly IList List;
    }
}
