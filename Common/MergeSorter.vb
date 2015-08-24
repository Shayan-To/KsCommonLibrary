Friend Class MergeSorter(Of T)

    Private List, Temp As IList(Of T),
            Comparer As IComparer(Of T)

    Private Sub Merge(ByVal Start As Integer, ByVal Mid As Integer, ByVal [End] As Integer)
        Dim A, B, P, Size As Integer

        If Me.Comparer.Compare(Me.List.Item(Mid - 1), Me.List.Item(Mid)) <= 0 Then
            Exit Sub
        End If

        Size = Mid - Start

        For I As Integer = 0 To Size - 1
            Me.Temp.Item(I) = Me.List.Item(I + Start)
        Next

        P = Start
        A = 0
        B = Mid

        Do While A < Size AndAlso B < [End]
            If Me.Comparer.Compare(Me.Temp.Item(A), Me.List.Item(B)) <= 0 Then
                Me.List.Item(P) = Me.Temp.Item(A)
                A += 1
            Else
                Me.List.Item(P) = Me.List.Item(B)
                B += 1
            End If
            P += 1
        Loop

        Do While A < Size
            Me.List.Item(P) = Me.Temp.Item(A)
            A += 1
            P += 1
        Loop
    End Sub

    Private Sub Sort(ByVal Start As Integer, ByVal [End] As Integer)
        Dim Mid As Integer

        If [End] - Start < 2 Then
            Exit Sub
        End If

        Mid = (Start + [End]) \ 2

        Me.Sort(Start, Mid)
        Me.Sort(Mid, [End])
        Me.Merge(Start, Mid, [End])
    End Sub

    Public Sub Sort(ByVal List As IList(Of T), ByVal Comparer As IComparer(Of T))
        Dim Length, Size, I As Integer

        Me.List = List
        Me.Temp = New T(List.Count) {}
        Me.Comparer = Comparer

        Length = 1
        Size = List.Count
        Do While Length < Size
            For I = 0 To Size - 2 * Length Step Length * 2
                Me.Merge(I, I + Length, I + Length * 2)
            Next

            If I + Length < Size Then
                Me.Merge(I, I + Length, Size)
            End If

            Length *= 2
        Loop

        'Me.Sort(0, List.Count)

        Me.Temp = Nothing
        Me.List = Nothing
    End Sub

    Public Sub Sort(ByVal List As IList(Of T))
        Me.Sort(List, Generic.Comparer(Of T).Default)
    End Sub

End Class
