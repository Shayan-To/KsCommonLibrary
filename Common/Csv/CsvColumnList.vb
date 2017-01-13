Namespace Common

    Public Class CsvColumnList
        Implements IReadOnlyList(Of CsvColumn)

        Friend Sub New(ByVal Parent As CsvData)
            Me._Parent = Parent
        End Sub

        Friend Sub ReportHeaderNameChanged(ByVal Column As CsvColumn, ByVal OldName As String, ByVal NewName As String)
            If NewName IsNot Nothing Then
                Me.Names.Item(NewName).Add(Column)
            End If
            If OldName IsNot Nothing Then
                Assert.True(Me.Names.Item(OldName).Remove(Column))
            End If
        End Sub

        Friend Sub ReportHasHeadersChanged(ByVal HasHeaders As Boolean)
            If HasHeaders Then
                Me.Names.Clear()
                For Each C In Me.List
                    C._HeaderName = ""
                Next
            End If
        End Sub

        Private Sub UpdateIndexes(ByVal Start As Integer, Optional ByVal [End] As Integer = -1)
            If [End] = -1 Then
                [End] = Me.Count
            End If
            For Start = Start To [End] - 1
                Me.List.Item(Start).Index = Start
            Next
        End Sub

        Public Function Insert(ByVal Index As Integer, ByVal HeaderName As String) As CsvColumn
            Verify.True(Me.Parent.HasHeaders, "The CSV does not have headers.")

            Dim R = Me.Insert(Index)
            R.HeaderName = HeaderName
            Return R
        End Function

        Public Function Insert(ByVal Index As Integer) As CsvColumn
            Dim R = New CsvColumn(Me.Parent)

            Me.List.Insert(Index, R)
            Me.UpdateIndexes(Index)

            Me.Parent.Entries.ReportColumnInsert(Index)

            Return R
        End Function

        Public Sub Remove(ByVal Column As CsvColumn)
            Verify.TrueArg(Column.Parent Is Me.Parent, "Column", "Given column is not part of this csv data.")
            Dim Index = Column.Index

            Me.List.RemoveAt(Index)

            Column.Detach()

            Me.Parent.Entries.ReportColumnRemove(Index)
            Me.UpdateIndexes(Index)
        End Sub

        Public Sub Clear()
            For Each C In Me.List
                C.Detach()
            Next
            Me.List.Clear()
            Me.Parent.Entries.ReportColumnsClear()
        End Sub

        Public Sub Move(ByVal Column As CsvColumn, ByVal NewIndex As Integer)
            Verify.TrueArg(Column.Parent Is Me.Parent, "Column", "Given column is not part of this csv data.")
            Dim OldIndex = Column.Index
            Assert.True(Me.List.Item(OldIndex) Is Column)

            If OldIndex = NewIndex Then
                Exit Sub
            End If

            Me.List.Move(OldIndex, NewIndex)

            Dim A = NewIndex
            Dim B = OldIndex
            If A > B Then
                Dim C = A
                A = B
                B = C
            End If
            Me.UpdateIndexes(A, B + 1)

            Me.Parent.Entries.ReportColumnMove(OldIndex, NewIndex)
        End Sub

#Region "Obvious Implementations"
        Public Function Add() As CsvColumn
            Return Me.Insert(Me.Count)
        End Function

        Public Function Add(ByVal HeaderName As String) As CsvColumn
            Return Me.Insert(Me.Count, HeaderName)
        End Function

        Public Sub Remove(ByVal Index As Integer)
            Me.Remove(Me.Item(Index))
        End Sub

        Public Sub Remove(ByVal HeaderName As String)
            Me.Remove(Me.Item(HeaderName))
        End Sub

        Public Sub Move(ByVal Index As Integer, ByVal NewIndex As Integer)
            Me.Move(Me.Item(Index), NewIndex)
        End Sub

        Public Sub Move(ByVal HeaderName As String, ByVal NewIndex As Integer)
            Me.Move(Me.Item(HeaderName), NewIndex)
        End Sub

        Public Function GetEnumerator() As List(Of CsvColumn).Enumerator
            Return Me.List.GetEnumerator()
        End Function

        Private Function IEnumerable_1_GetEnumerator() As IEnumerator(Of CsvColumn) Implements IEnumerable(Of CsvColumn).GetEnumerator
            Return Me.GetEnumerator()
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function

        Default Public ReadOnly Property Item(ByVal Index As Integer) As CsvColumn Implements IReadOnlyList(Of CsvColumn).Item
            Get
                Return Me.List.Item(Index)
            End Get
        End Property

        Default Public ReadOnly Property Item(ByVal HeaderName As String) As CsvColumn
            Get
                Verify.True(Me.Parent.HasHeaders, "The CSV does not have headers.")
                Return Me.Names.Item(HeaderName).SingleOrDefault()
            End Get
        End Property

        Public ReadOnly Property Count As Integer Implements IReadOnlyCollection(Of CsvColumn).Count
            Get
                Return Me.List.Count
            End Get
        End Property
#End Region

#Region "Parent Property"
        Private ReadOnly _Parent As CsvData

        Public ReadOnly Property Parent As CsvData
            Get
                Return Me._Parent
            End Get
        End Property
#End Region

        Private ReadOnly List As List(Of CsvColumn) = New List(Of CsvColumn)()
        Private ReadOnly Names As MultiDictionary(Of String, CsvColumn) = New MultiDictionary(Of String, CsvColumn)()

    End Class

End Namespace
