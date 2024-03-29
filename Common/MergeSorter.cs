using System.Collections.Generic;

namespace Ks.Common
{
    public class MergeSorter<T>
    {
        private void Merge(int Start, int Mid, int End)
        {
            if (Start == Mid || this.Comparer.Invoke(this.List[Mid - 1], this.List[Mid]) <= 0)
            {
                return;
            }

            for (var I = 0; I < Mid - Start; I++)
            {
                this.Temp[I] = this.List[I + Start];
            }

            Merge(this.List, Start, this.Temp, 0, Mid - Start, this.List, Mid, End - Mid, this.Comparer);
        }

        public static void Merge(IList<T> BaseList, int BaseIndex, IList<T> List1, int Index1, int Length1, IList<T> List2, int Index2, int Length2, Comparison<T, T> Comparer)
        {
            var End1 = Index1 + Length1;
            var End2 = Index2 + Length2;

            while ((Index1 < End1) & (Index2 < End2))
            {
                if (Comparer.Invoke(List1[Index1], List2[Index2]) <= 0)
                {
                    BaseList[BaseIndex] = List1[Index1];
                    Index1 += 1;
                }
                else
                {
                    BaseList[BaseIndex] = List2[Index2];
                    Index2 += 1;
                }
                BaseIndex += 1;
            }

            while (Index1 < End1)
            {
                BaseList[BaseIndex] = List1[Index1];
                Index1 += 1;
                BaseIndex += 1;
            }
            while (Index2 < End2)
            {
                BaseList[BaseIndex] = List2[Index2];
                Index2 += 1;
                BaseIndex += 1;
            }
        }

        public static void Merge(IList<T> BaseList, int BaseIndex, IList<T> List1, int Index1, int Length1, IList<T> List2, int Index2, int Length2, IComparer<T> Comparer)
        {
            Merge(BaseList, BaseIndex, List1, Index1, Length1, List2, Index2, Length2, Comparer.Compare);
        }

        public static void Merge(IList<T> BaseList, int BaseIndex, IList<T> List1, int Index1, int Length1, IList<T> List2, int Index2, int Length2)
        {
            Merge(BaseList, BaseIndex, List1, Index1, Length1, List2, Index2, Length2, Comparer<T>.Default.Compare);
        }

        private void SortRecursive(int Start, int End)
        {
            int Mid;

            if ((End - Start) < 2)
            {
                return;
            }

            Mid = (Start + End) / 2;

            this.SortRecursive(Start, Mid);
            this.SortRecursive(Mid, End);
            this.Merge(Start, Mid, End);
        }

        public void Sort(IList<T> List, Comparison<T, T> Comparer)
        {
            this.List = List;
            this.Temp = new T[List.Count + 1];
            this.Comparer = Comparer;

            var Length = 1;
            var Size = List.Count;
            while (Length < Size)
            {
                var Length2 = Length * 2;

                var I = 0;
                for (; I <= Size - Length2; I += Length2)
                {
                    this.Merge(I, I + Length, I + Length2);
                }

                if ((I + Length) < Size)
                {
                    this.Merge(I, I + Length, Size);
                }

                Length = Length2;
            }

            this.Temp = null;
            this.List = null;
        }

        public void Sort(IList<T> List, IComparer<T> Comparer)
        {
            this.Sort(List, Comparer.Compare);
        }

        public void Sort(IList<T> List)
        {
            this.Sort(List, Comparer<T>.Default.Compare);
        }

        public static MergeSorter<T> Instance => DefaultCacher<MergeSorter<T>>.Value;

        private IList<T> List, Temp;
        private Comparison<T, T> Comparer;
    }
}
