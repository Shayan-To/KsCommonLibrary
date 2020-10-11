using System;

namespace Ks.Common
{
    public class ArraySerializer<T> : Serializer<T[]>
    {
        public ArraySerializer() : base(nameof(Array))
        {
        }

        public override void SetT(FormatterSetProxy Formatter, T[] Obj)
        {
            Formatter.Set(nameof(Obj.Length), Obj.Length);
            for (var I = 0; I < Obj.Length; I++)
            {
                Formatter.Set(null, Obj[I]);
            }
        }

        public override T[] GetT(FormatterGetProxy Formatter)
        {
            int Length = Formatter.Get<int>(nameof(Length));

            var R = new T[Length];
            for (var I = 0; I < Length; I++)
            {
                R[I] = Formatter.Get<T>(null);
            }

            return R;
        }
    }
}
