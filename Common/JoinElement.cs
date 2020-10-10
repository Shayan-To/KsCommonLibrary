namespace Ks.Common
{
    public struct JoinElement<T1, T2, TKey>
    {
        public JoinElement(TKey Key, JoinDirection Direction, T1 Item1, T2 Item2)
        {
            this.Key = Key;
            this.Direction = Direction;
            this.Item1 = Item1;
            this.Item2 = Item2;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public TKey Key { get; }

        public JoinDirection Direction { get; }
    }
}
