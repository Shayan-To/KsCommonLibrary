using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using FsCheck.Xunit;

using Ks.Common;

using Xunit;

using Assert = Xunit.Assert;

namespace Ks.Tests.Common
{
    public class Sorters_Tests
    {
        [Property()]
        public void Merge(int[] List1, int[] List2)
        {
            var Clone = new int[List1.Length + List2.Length];
            List1.CopyTo(Clone, 0);
            List2.CopyTo(Clone, List1.Length);

            Array.Sort(List1);
            Array.Sort(List2);

            var List = new int[List1.Length + List2.Length];
            MergeSorter<int>.Merge(List, 0, List1, 0, List1.Length, List2, 0, List2.Length, Comparer<int>.Default);

            Array.Sort(Clone);

            Assert.Equal(List, Clone);
        }

        [Property()]
        public void MergeSort(int[] List)
        {
            var Clone = new int[List.Length];
            List.CopyTo(Clone, 0);

            MergeSorter<int>.Instance.Sort(List);
            Array.Sort(Clone);

            Assert.Equal(List, Clone);
        }

        [Property()]
        public void QuickSort(int[] List)
        {
            var Clone = new int[List.Length];
            List.CopyTo(Clone, 0);

            QuickSorter<int>.Instance.Sort(List);
            Array.Sort(Clone);

            Assert.Equal(List, Clone);
        }
    }
}
