Namespace Common

    Public Class Sorters_Tests

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
