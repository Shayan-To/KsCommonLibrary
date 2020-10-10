using System.Linq;
using System.Collections.Generic;

namespace Ks.Common
{
    public class ListSerializer<T> : Serializer<IEnumerable<T>>
    {
        public ListSerializer() : base(nameof(List<object>))
        {
        }

        public override void SetT(FormatterSetProxy Formatter, IEnumerable<T> Obj)
        {
            Formatter.Set(nameof(Enumerable.Count), Obj.Count());
            foreach (var I in Obj)
            {
                Formatter.Set(null, I);
            }
        }

        public override IEnumerable<T> GetT(FormatterGetProxy Formatter)
        {
            var Count = default(int);
            Count = Formatter.Get<int>(nameof(Count));

            var R = new List<T>(Count);
            for (var I = 0; I < Count; I++)
            {
                R.Add(Formatter.Get<T>(null));
            }

            return R;
        }
    }
}
