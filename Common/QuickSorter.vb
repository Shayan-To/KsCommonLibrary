Namespace Common

    Public Class QuickSorter(Of T)

        Public Sub New()
            Me.New(DefaultCacher(Of Random).Value)
        End Sub

        Public Sub New(ByVal Random As Random)
            Me.Random = Random
        End Sub

        Private Sub Sort(ByVal Start As Integer, ByVal Length As Integer)
            If Length < 2 Then
                Exit Sub
            End If

            Dim Ptr1 = Start
            Dim Ptr2 = Start + Length - 1

            Dim PivotPtr = Start + Me.Random.Next(Length)
            Dim Pivot = Me.List.Item(PivotPtr)

            Do While Ptr1 < Ptr2
                ' L[PivotPtr] is never moved, as it is always >= and <= to Pivot.
                Do While Ptr1 < Ptr2 And Me.Comparer.Compare(Me.List.Item(Ptr1), Pivot) <= 0
                    Ptr1 += 1
                Loop
                Do While Ptr1 < Ptr2 And Me.Comparer.Compare(Me.List.Item(Ptr2), Pivot) >= 0
                    Ptr2 -= 1
                Loop

                Dim C = Me.List.Item(Ptr1)
                Me.List.Item(Ptr1) = Me.List.Item(Ptr2)
                Me.List.Item(Ptr2) = C
            Loop

            Assert.True(Ptr1 = Ptr2)

            ' Ptr always stands on the first I that L[I] > Pivot.
            ' If no such I exists (meaning that all items are <= Pivot), it will stand at the end.
            ' (**)^

            ' We want to swap L[PivotPtr] with L[Ptr], so that we can put it out of the ranges we sort recursively.

            ' Ptr is our split point.
            ' Items to its left are <= Pivot, and items to its right are >= Pivot.
            ' But PivotPtr can be anywhere in the list.

            ' Now consider these cases:

            ' 0. PivotPtr = Ptr. We can always swap L[Ptr] and L[PivotPtr] in this case!
            ' 1. PivotPtr is to the right of the split point.
            '    This means that the exception point above (**) has not happened and L[Ptr] > Pivot.
            '    So we can swap L[Ptr] and L[PivotPtr].
            ' 2. Pivot is to the left of the split point, and the exception point above (**) has happened.
            '    This means that L[Ptr] <= Pivot.
            '    This time also we can swap L[Ptr] and L[PivotPtr].
            ' 3. Pivot is to the left of the split point, without the exception point above (**).
            '    This means that L[Ptr] > Pivot, and we cannot swap them (as PivotPtr is within the small ones).
            '    In this case Ptr > Start, and we know that L[Ptr - 1] <= Pivot.
            '    So we can swap L[Ptr - 1] and L[PivotPtr].

            If PivotPtr < Ptr1 And Me.Comparer.Compare(Pivot, Me.List.Item(Ptr1)) < 0 Then
                Dim C = Me.List.Item(Ptr1 - 1)
                Me.List.Item(Ptr1 - 1) = Me.List.Item(PivotPtr)
                Me.List.Item(PivotPtr) = C

                PivotPtr = Ptr1 - 1
            Else
                Dim C = Me.List.Item(Ptr1)
                Me.List.Item(Ptr1) = Me.List.Item(PivotPtr)
                Me.List.Item(PivotPtr) = C

                PivotPtr = Ptr1
            End If

            Me.Sort(Start, PivotPtr - Start) ' [Start, PivotPtr)
            Me.Sort(PivotPtr + 1, Start + Length - (PivotPtr + 1)) ' [PivotPtr + 1, Start + Length)
        End Sub

        Public Sub Sort(ByVal List As IList(Of T))
            Me.Sort(List, Generic.Comparer(Of T).Default)
        End Sub

        Public Sub Sort(ByVal List As IList(Of T), ByVal Comparer As IComparer(Of T))
            Me.List = List
            Me.Comparer = Comparer
            Me.Sort(0, List.Count)
        End Sub

#Region "Instance Property"
        Public Shared ReadOnly Property Instance As QuickSorter(Of T)
            Get
                Return DefaultCacher(Of QuickSorter(Of T)).Value
            End Get
        End Property
#End Region

        Private Random As Random
        Private List As IList(Of T)
        Private Comparer As IComparer(Of T)

    End Class

End Namespace
