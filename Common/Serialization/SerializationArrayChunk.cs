namespace Ks.Common
{
    public struct SerializationArrayChunk<T>
    {
        public SerializationArrayChunk(T[] Array) : this(Array, 0, Array.Length)
        {
        }

        public SerializationArrayChunk(T[] Array, int StartIndex, int Length)
        {
            this._Array = Array;
            this._StartIndex = StartIndex;
            this._Length = Length;
        }

        private readonly T[] _Array;

        public T[] Array
        {
            get
            {
                return this._Array;
            }
        }

        private readonly int _StartIndex;

        public int StartIndex
        {
            get
            {
                return this._StartIndex;
            }
        }

        private readonly int _Length;

        public int Length
        {
            get
            {
                return this._Length;
            }
        }
    }
}
