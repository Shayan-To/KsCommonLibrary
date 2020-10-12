using System.Collections.Generic;

namespace Ks.Common
{
    public class HeapOperations<T>
    {
        public HeapOperations(IComparer<T> Comparer)
        {
            this.Comparer = Comparer;
        }

        public HeapOperations() : this(System.Collections.Generic.Comparer<T>.Default)
        {
        }

        public void Heapify(IList<T> List, int Count, int Index)
        {
            while (true)
            {
                var MinIndex = LeftChild(Index);
                if (MinIndex >= Count)
                {
                    break;
                }

                var Min = List[MinIndex];
                var I = RightChild(Index);
                var T = default(T);
                if (I < Count)
                {
                    T = List[I];
                    if (this.Comparer.Compare(T, Min) < 0)
                    {
                        MinIndex = I;
                        Min = T;
                    }
                }

                T = List[Index];
                if (this.Comparer.Compare(Min, T) < 0)
                {
                    List[Index] = Min;
                    List[MinIndex] = T;
                    Index = MinIndex;
                }
                else
                {
                    break;
                }
            }
        }

        public void MakeHeap(IList<T> List, int Count)
        {
            for (var I = Parent(Count - 1); I >= 0; I--)
            {
                this.Heapify(List, Count, I);
            }
        }

        public void BubbleUp(IList<T> List, int Count, int Index)
        {
            while (true)
            {
                if (Index == 0)
                {
                    break;
                }

                var T = List[Index];
                var PI = Parent(Index);
                var P = List[PI];
                if (this.Comparer.Compare(P, T) > 0)
                {
                    List[PI] = T;
                    List[Index] = P;
                    Index = PI;
                }
                else
                {
                    break;
                }
            }
        }

        public static int Parent(int Index)
        {
            return (Index - 1) / 2;
        }

        public static int LeftChild(int Index)
        {
            return (2 * Index) + 1;
        }

        public static int RightChild(int Index)
        {
            return (2 * Index) + 2;
        }

        private readonly IComparer<T> Comparer;
    }
}
