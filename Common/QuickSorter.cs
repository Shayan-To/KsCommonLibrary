using System;
using System.Collections.Generic;

namespace Ks.Common
{
    public class QuickSorter<T>
    {
        public QuickSorter() : this(DefaultCacher<Random>.Value)
        {
        }

        public QuickSorter(Random Random)
        {
            this.Random = Random;
        }

        private void Sort(int Start, int Length)
        {
            if (Length < 2)
            {
                return;
            }

            var Ptr1 = Start;
            var Ptr2 = (Start + Length) - 1;

            var PivotPtr = Start + this.Random.Next(Length);
            var Pivot = this.List[PivotPtr];

            while (Ptr1 < Ptr2)
            {
                // L[PivotPtr] is never moved, as it is always >= and <= to Pivot.
                while ((Ptr1 < Ptr2) & (this.Comparer.Compare(this.List[Ptr1], Pivot) <= 0))
                {
                    Ptr1 += 1;
                }

                while ((Ptr1 < Ptr2) & (this.Comparer.Compare(this.List[Ptr2], Pivot) >= 0))
                {
                    Ptr2 -= 1;
                }

                var C = this.List[Ptr1];
                this.List[Ptr1] = this.List[Ptr2];
                this.List[Ptr2] = C;
            }

            Assert.True(Ptr1 == Ptr2);

            // Ptr always stands on the first I that L[I] > Pivot.
            // If no such I exists (meaning that all items are <= Pivot), it will stand at the end.
            // (**)^

            // We want to swap L[PivotPtr] with L[Ptr], so that we can put it out of the ranges we sort recursively.

            // Ptr is our split point.
            // Items to its left are <= Pivot, and items to its right are >= Pivot.
            // But PivotPtr can be anywhere in the list.

            // Now consider these cases:

            // 0. PivotPtr = Ptr. We can always swap L[Ptr] and L[PivotPtr] in this case!
            // 1. PivotPtr is to the right of the split point.
            // This means that the exception point above (**) has not happened and L[Ptr] > Pivot.
            // So we can swap L[Ptr] and L[PivotPtr].
            // 2. Pivot is to the left of the split point, and the exception point above (**) has happened.
            // This means that L[Ptr] <= Pivot.
            // This time also we can swap L[Ptr] and L[PivotPtr].
            // 3. Pivot is to the left of the split point, without the exception point above (**).
            // This means that L[Ptr] > Pivot, and we cannot swap them (as PivotPtr is within the small ones).
            // In this case Ptr > Start, and we know that L[Ptr - 1] <= Pivot.
            // So we can swap L[Ptr - 1] and L[PivotPtr].

            if ((PivotPtr < Ptr1) & (this.Comparer.Compare(Pivot, this.List[Ptr1]) < 0))
            {
                var C = this.List[Ptr1 - 1];
                this.List[Ptr1 - 1] = this.List[PivotPtr];
                this.List[PivotPtr] = C;

                PivotPtr = Ptr1 - 1;
            }
            else
            {
                var C = this.List[Ptr1];
                this.List[Ptr1] = this.List[PivotPtr];
                this.List[PivotPtr] = C;

                PivotPtr = Ptr1;
            }

            this.Sort(Start, PivotPtr - Start); // [Start, PivotPtr)
            this.Sort(PivotPtr + 1, (Start + Length) - (PivotPtr + 1)); // [PivotPtr + 1, Start + Length)
        }

        public void Sort(IList<T> List)
        {
            this.Sort(List, System.Collections.Generic.Comparer<T>.Default);
        }

        public void Sort(IList<T> List, IComparer<T> Comparer)
        {
            this.List = List;
            this.Comparer = Comparer;
            this.Sort(0, List.Count);
        }

        public static QuickSorter<T> Instance
        {
            get
            {
                return DefaultCacher<QuickSorter<T>>.Value;
            }
        }

        private Random Random;
        private IList<T> List;
        private IComparer<T> Comparer;
    }
}
