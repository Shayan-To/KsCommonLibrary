namespace Ks.Common
{
    public struct SerializationArrayChunk<T>
    {
        public SerializationArrayChunk(T[] Array) : this(Array, 0, Array.Length)
        {
        }

        public SerializationArrayChunk(T[] Array, int StartIndex, int Length)
        {
            this.Array = Array;
            this.StartIndex = StartIndex;
            this.Length = Length;
        }

        public T[] Array { get; }

        public int StartIndex { get; }

        public int Length { get; }
    }
}
