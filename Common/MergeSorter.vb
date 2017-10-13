Namespace Common

    Public Class MergeSorter(Of T)

        Private Sub Merge(ByVal Start As Integer, ByVal Mid As Integer, ByVal [End] As Integer)
            If Start = Mid OrElse Me.Comparer.Compare(Me.List.Item(Mid - 1), Me.List.Item(Mid)) <= 0 Then
                Exit Sub
            End If

            For I As Integer = 0 To Mid - Start - 1
                Me.Temp.Item(I) = Me.List.Item(I + Start)
            Next

            Merge(Me.List, Start,
                  Me.Temp, 0, Mid - Start,
                  Me.List, Mid, [End] - Mid,
                  Me.Comparer)
        End Sub

        Public Shared Sub Merge(ByVal BaseList As IList(Of T), ByVal BaseIndex As Integer,
                                ByVal List1 As IList(Of T), ByVal Index1 As Integer, ByVal Length1 As Integer,
                                ByVal List2 As IList(Of T), ByVal Index2 As Integer, ByVal Length2 As Integer,
                                ByVal Comparer As IComparer(Of T))
            Dim End1 = Index1 + Length1
            Dim End2 = Index2 + Length2

            Do While Index1 < End1 And Index2 < End2
                If Comparer.Compare(List1.Item(Index1), List2.Item(Index2)) <= 0 Then
                    BaseList.Item(BaseIndex) = List1.Item(Index1)
                    Index1 += 1
                Else
                    BaseList.Item(BaseIndex) = List2.Item(Index2)
                    Index2 += 1
                End If
                BaseIndex += 1
            Loop

            Do While Index1 < End1
                BaseList.Item(BaseIndex) = List1.Item(Index1)
                Index1 += 1
                BaseIndex += 1
            Loop
            Do While Index2 < End2
                BaseList.Item(BaseIndex) = List2.Item(Index2)
                Index2 += 1
                BaseIndex += 1
            Loop
        End Sub

        Private Sub SortRecursive(ByVal Start As Integer, ByVal [End] As Integer)
            Dim Mid As Integer

            If [End] - Start < 2 Then
                Exit Sub
            End If

            Mid = (Start + [End]) \ 2

            Me.SortRecursive(Start, Mid)
            Me.SortRecursive(Mid, [End])
            Me.Merge(Start, Mid, [End])
        End Sub

        Public Sub Sort(ByVal List As IList(Of T), ByVal Comparer As IComparer(Of T))
            Me.List = List
            Me.Temp = New T(List.Count) {}
            Me.Comparer = Comparer

            Dim Length = 1
            Dim Size = List.Count
            Do While Length < Size
                Dim Length2 = Length * 2

                Dim I = 0
                For I = 0 To Size - Length2 Step Length2
                    Me.Merge(I, I + Length, I + Length2)
                Next

                If I + Length < Size Then
                    Me.Merge(I, I + Length, Size)
                End If

                Length = Length2
            Loop

            Me.Temp = Nothing
            Me.List = Nothing
        End Sub

        Public Sub Sort(ByVal List As IList(Of T))
            Me.Sort(List, Generic.Comparer(Of T).Default)
        End Sub

#Region "Instance Property"
        Public Shared ReadOnly Property Instance As MergeSorter(Of T)
            Get
                Return DefaultCacher(Of MergeSorter(Of T)).Value
            End Get
        End Property
#End Region

        Private List, Temp As IList(Of T)
        Private Comparer As IComparer(Of T)

    End Class

End Namespace
