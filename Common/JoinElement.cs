namespace Ks
{
    namespace Common
    {
        public struct JoinElement<T1, T2, TKey>
        {
            public JoinElement(TKey Key, JoinDirection Direction, T1 Item1, T2 Item2)
            {
                this._Key = Key;
                this._Direction = Direction;
                this._Item1 = Item1;
                this._Item2 = Item2;
            }

            private readonly T1 _Item1;

            public T1 Item1
            {
                get
                {
                    return this._Item1;
                }
            }

            private readonly T2 _Item2;

            public T2 Item2
            {
                get
                {
                    return this._Item2;
                }
            }

            private readonly TKey _Key;

            public TKey Key
            {
                get
                {
                    return this._Key;
                }
            }

            private readonly JoinDirection _Direction;

            public JoinDirection Direction
            {
                get
                {
                    return this._Direction;
                }
            }
        }
    }
}
