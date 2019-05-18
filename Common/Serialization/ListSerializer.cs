using System.Linq;
using System.Collections.Generic;

namespace Ks
{
    namespace Common
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
                    Formatter.Set(null, I);
            }

            public override IEnumerable<T> GetT(FormatterGetProxy Formatter)
            {
                int Count = default(int);
                Count = Formatter.Get<int>(nameof(Count));

                var R = new List<T>(Count);
                var loopTo = Count - 1;
                for (int I = 0; I <= loopTo; I++)
                    R.Add(Formatter.Get<T>(null));

                return R;
            }
        }
    }
}
