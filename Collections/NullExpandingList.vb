'Public Class NullExpandingList

'    Public Shared Function Create(Of T)(ByVal List As IList(Of T)) As NullExpandingList(Of T)
'        Return New NullExpandingList(Of T)(List)
'    End Function

'End Class

Public Class NullExpandingList(Of T)
    Implements IList(Of T)

    Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count
        Get
            Return Me.List.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
        Get
            Return False
        End Get
    End Property

    Default Public Property Item(Index As Integer) As T Implements IList(Of T).Item
        Get
            If Index >= Me.List.Count Then
                Return Nothing
            End If
            Return Me.List.Item(Index)
        End Get
        Set(Value As T)
            For I As Integer = Me.List.Count To Index
                Me.List.Add(Nothing)
            Next
            Me.List.Item(Index) = Value
        End Set
    End Property

    Public Sub Add(item As T) Implements ICollection(Of T).Add
        Me.List.Add(item)
    End Sub

    Public Sub Clear() Implements ICollection(Of T).Clear
        Me.List.Clear()
    End Sub

    Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
        Me.List.CopyTo(array, arrayIndex)
    End Sub

    Public Sub Insert(index As Integer, item As T) Implements IList(Of T).Insert
        Me.List.Insert(index, item)
    End Sub

    Public Sub RemoveAt(index As Integer) Implements IList(Of T).RemoveAt
        Me.List.RemoveAt(index)
    End Sub

    Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
        Return Me.List.Contains(item)
    End Function

    Public Function GetEnumerator() As List(Of T).Enumerator
        Return Me.List.GetEnumerator()
    End Function

    Public Function IEnumerable_1_GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Return Me.List.GetEnumerator()
    End Function

    Public Function IndexOf(item As T) As Integer Implements IList(Of T).IndexOf
        Return Me.List.IndexOf(item)
    End Function

    Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
        Return Me.List.Remove(item)
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.List.GetEnumerator()
    End Function

    Private ReadOnly List As List(Of T) = New List(Of T)()

End Class
