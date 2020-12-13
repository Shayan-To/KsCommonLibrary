using System;

namespace Ks
{
    namespace Common
    {
        public class ArraySerializer<T> : Serializer<T[]>
        {
            public ArraySerializer() : base(nameof(Array))
            {
            }

            public override void SetT(FormatterSetProxy Formatter, T[] Obj)
            {
                Formatter.Set(nameof(Obj.Length), Obj.Length);
                var loopTo = Obj.Length - 1;
                for (int I = 0; I <= loopTo; I++)
                    Formatter.Set(null, Obj[I]);
            }

            public override T[] GetT(FormatterGetProxy Formatter)
            {
                int Length = default(int);
                Length = Formatter.Get<int>(nameof(Length));

                var R = new T[Length - 1 + 1];
                var loopTo = Length - 1;
                for (int I = 0; I <= loopTo; I++)
                    R[I] = Formatter.Get<T>(null);

                return R;
            }
        }
    }
}
