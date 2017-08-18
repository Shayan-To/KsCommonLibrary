Namespace Common

    Public Class Sorters_Tests

        <[Property]()>
        Public Sub Merge(ByVal List1 As Integer(), ByVal List2 As Integer())
            Dim Clone = New Integer(List1.Length + List2.Length - 1) {}
            List1.CopyTo(Clone, 0)
            List2.CopyTo(Clone, List1.Length)

            Array.Sort(List1)
            Array.Sort(List2)

            Dim List = New Integer(List1.Length + List2.Length - 1) {}
            MergeSorter(Of Integer).Merge(List, 0,
                                          List1, 0, List1.Length,
                                          List2, 0, List2.Length,
                                          Comparer(Of Integer).Default)

            Array.Sort(Clone)

            Assert.Equal(List, Clone)
        End Sub

        <[Property]()>
        Public Sub MergeSort(ByVal List As Integer())
            Dim Clone = New Integer(List.Length - 1) {}
            List.CopyTo(Clone, 0)

            MergeSorter(Of Integer).Instance.Sort(List)
            Array.Sort(Clone)

            Assert.Equal(List, Clone)
        End Sub

        <[Property]()>
        Public Sub QuickSort(ByVal List As Integer())
            Dim Clone = New Integer(List.Length - 1) {}
            List.CopyTo(Clone, 0)

            QuickSorter(Of Integer).Instance.Sort(List)
            Array.Sort(Clone)

            Assert.Equal(List, Clone)
        End Sub

    End Class

End Namespace
