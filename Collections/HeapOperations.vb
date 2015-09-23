Public Class HeapOperations(Of T)

    Public Sub New(ByVal Comparer As IComparer(Of T))
        Me.Comparer = Comparer
    End Sub

    Public Sub New()
        Me.New(Generic.Comparer(Of T).Default)
    End Sub

    Public Sub Heapify(ByVal List As IList(Of T), ByVal Count As Integer, ByVal Index As Integer)
        Do
            Dim MinIndex = LeftChildren(Index)
            If MinIndex >= Count Then
                Exit Do
            End If

            Dim Min = List.Item(MinIndex)
            Dim I = RightChildren(Index)
            Dim T As T
            If I < Count Then
                T = List.Item(I)
                If Me.Comparer.Compare(T, Min) < 0 Then
                    MinIndex = I
                    Min = T
                End If
            End If

            T = List.Item(Index)
            If Me.Comparer.Compare(Min, T) < 0 Then
                List.Item(Index) = Min
                List.Item(MinIndex) = T

                Index = MinIndex
            Else
                Exit Do
            End If
        Loop
    End Sub

    Public Sub MakeHeap(ByVal List As IList(Of T), ByVal Count As Integer)
        For I As Integer = Parent(Count - 1) To 0 Step -1
            Me.Heapify(List, Count, I)
        Next
    End Sub

    Public Sub BubbleUp(ByVal List As IList(Of T), ByVal Count As Integer, ByVal Index As Integer)
        Do
            If Index = 0 Then
                Exit Do
            End If

            Dim T = List.Item(Index)
            Dim PI = Parent(Index)
            Dim P = List.Item(PI)

            If Me.Comparer.Compare(P, T) > 0 Then
                List.Item(PI) = T
                List.Item(Index) = P
                Index = PI
            Else
                Exit Do
            End If
        Loop
    End Sub

    Public Shared Function Parent(ByVal Index As Integer) As Integer
        Return (Index - 1) \ 2
    End Function

    Public Shared Function LeftChildren(ByVal Index As Integer) As Integer
        Return 2 * Index + 1
    End Function

    Public Shared Function RightChildren(ByVal Index As Integer) As Integer
        Return 2 * Index + 2
    End Function

    Private ReadOnly Comparer As IComparer(Of T)

End Class
