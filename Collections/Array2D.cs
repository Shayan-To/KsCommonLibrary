namespace Ks.Common
{
    public class Array2D<T>
    {
        public Array2D(int Width, int Height)
        {
            this.Array = new T[Width, Height];
        }

        public T this[int I, int J]
        {
            get => this.Array[I, J];
            set => this.Array[I, J] = value;
        }

        private readonly T[,] Array;
    }
}
