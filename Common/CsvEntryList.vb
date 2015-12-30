﻿Public Class CsvEntryList
    Implements IReadOnlyList(Of CsvEntry)

    Friend Sub New(ByVal Parent As CsvData)
        Me._Parent = Parent
    End Sub

    Friend Sub ReportColumnInsert(ByVal Index As Integer)
        For Each E In Me.List
            If Index < E.Data.Count Then
                E.Data.Insert(Index, "")
            End If
        Next
    End Sub

    Friend Sub ReportColumnRemove(ByVal Index As Integer)
        For Each E In Me.List
            If Index < E.Data.Count Then
                E.Data.RemoveAt(Index)
            End If
        Next
    End Sub

    Friend Sub ReportColumnMove(ByVal OldIndex As Integer, ByVal NewIndex As Integer)
        For Each E In Me.List
            If OldIndex < E.Data.Count Then
                Dim C = E.Data.Item(OldIndex)
                E.Data.RemoveAt(OldIndex)
                If NewIndex < E.Data.Count Then
                    E.Data.Insert(NewIndex, C)
                Else
                    E.Item(NewIndex) = C
                End If
            ElseIf NewIndex < E.Data.Count Then
                E.Data.Insert(NewIndex, "")
            End If
        Next
    End Sub

#Region "Obvious Implementation"
    Public Function GetEnumerator() As List(Of CsvEntry).Enumerator
        Return Me.List.GetEnumerator()
    End Function

    Private Function IEnumerable_1_GetEnumerator() As IEnumerator(Of CsvEntry) Implements IEnumerable(Of CsvEntry).GetEnumerator
        Return Me.GetEnumerator()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function

    Public Function Insert(ByVal Index As Integer) As CsvEntry
        Dim R = New CsvEntry(Me.Parent)
        Me.List.Insert(Index, R)
        Return R
    End Function

    Public Function Add() As CsvEntry
        Dim R = New CsvEntry(Me.Parent)
        Me.List.Add(R)
        Return R
    End Function

    Public Sub Remove(ByVal Index As Integer)
        Me.List.Item(Index).Detach()
        Me.List.RemoveAt(Index)
    End Sub

    Public Sub Remove(ByVal Entry As CsvEntry)
        Verify.TrueArg(Entry.Parent Is Me.Parent, "Entry", "Given entry is not part of this csv data.")
        Entry.Detach()
        Me.List.Remove(Entry)
    End Sub

    Public Sub Move(ByVal Index As Integer, ByVal NewIndex As Integer)
        Dim T = Me.List.Item(Index)
        Me.List.RemoveAt(Index)
        Me.List.Insert(NewIndex, T)
    End Sub

    Public Sub Move(ByVal Entry As CsvEntry, ByVal NewIndex As Integer)
        Verify.TrueArg(Entry.Parent Is Me.Parent, "Entry", "Given entry is not part of this csv data.")
        Me.List.Remove(Entry)
        Me.List.Insert(NewIndex, Entry)
    End Sub

    Public Sub Clear()
        For Each E In Me.List
            E.Detach()
        Next
        Me.List.Clear()
    End Sub

    Public Function IndexOf(ByVal Entry As CsvEntry) As Integer
        Verify.TrueArg(Entry.Parent Is Me.Parent, "Entry", "Given entry is not part of this csv data.")
        Return Me.List.IndexOf(Entry)
    End Function

    Public Function Contains(ByVal Entry As CsvEntry) As Boolean
        Verify.TrueArg(Entry.Parent Is Me.Parent, "Entry", "Given entry is not part of this csv data.")
        Return Me.List.Contains(Entry)
    End Function

    Public ReadOnly Property Count As Integer Implements IReadOnlyCollection(Of CsvEntry).Count
        Get
            Return Me.List.Count
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal Index As Integer) As CsvEntry Implements IReadOnlyList(Of CsvEntry).Item
        Get
            Return Me.List.Item(Index)
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

    Private ReadOnly List As List(Of CsvEntry) = New List(Of CsvEntry)()

End Class
