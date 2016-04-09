Public MustInherit Class BaseList(Of T)
    Implements IReadOnlyList(Of T), IList(Of T), IList

    Public MustOverride Sub Insert(index As Integer, item As T) Implements IList(Of T).Insert

    Public MustOverride Sub RemoveAt(index As Integer) Implements IList(Of T).RemoveAt, IList.RemoveAt

    Public MustOverride Sub Clear() Implements ICollection(Of T).Clear, IList.Clear

    Default Public MustOverride Property Item(index As Integer) As T Implements IList(Of T).Item

    Public MustOverride ReadOnly Property Count As Integer Implements IReadOnlyCollection(Of T).Count, ICollection.Count, ICollection(Of T).Count

    Protected MustOverride Function IEnumerable_1_GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator

    Public Overridable Function IndexOf(item As T) As Integer Implements IList(Of T).IndexOf
        For I As Integer = 0 To Me.Count - 1
            If Object.Equals(item, Me.Item(I)) Then
                Return I
            End If
        Next
        Return -1
    End Function

    Public Overridable Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
        Verify.True(arrayIndex + Me.Count <= array.Length, "Array is not large enough.")
        For I As Integer = 0 To Me.Count - 1
            array(arrayIndex) = Me.Item(I)
            arrayIndex += 1
        Next
    End Sub

    Protected Overridable Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
        Verify.True(index + Me.Count <= array.GetLength(0), "Array is not large enough.")
        For I As Integer = 0 To Me.Count - 1
            array.SetValue(Me.Item(I), index)
            index += 1
        Next
    End Sub

    Public Overridable Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
        Dim I = Me.IndexOf(item)
        If I = -1 Then
            Return False
        End If

        Me.RemoveAt(I)
        Return True
    End Function

#Region "Junk"
    Private Property IList_Item(index As Integer) As Object Implements IList.Item
        Get
            Return Me.Item(index)
        End Get
        Set(value As Object)
            Me.Item(index) = DirectCast(value, T)
        End Set
    End Property

    Private ReadOnly Property IReadOnlyList_Item(index As Integer) As T Implements IReadOnlyList(Of T).Item
        Get
            Return Me.Item(index)
        End Get
    End Property

    Protected Overridable ReadOnly Property IList_IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly, IList.IsReadOnly
        Get
            Return False
        End Get
    End Property

    Private ReadOnly Property IList_IsFixedSize As Boolean Implements IList.IsFixedSize
        Get
            Return False
        End Get
    End Property

    Private ReadOnly Property IList_SyncRoot As Object Implements ICollection.SyncRoot
        Get
            Throw New NotSupportedException()
        End Get
    End Property

    Private ReadOnly Property IList_IsSynchronized As Boolean Implements ICollection.IsSynchronized
        Get
            Return False
        End Get
    End Property

    Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
        Return Me.IndexOf(item) <> -1
    End Function

    Private Function IList_Add(value As Object) As Integer Implements IList.Add
        Me.Add(DirectCast(value, T))
        Return Me.Count - 1
    End Function

    Private Function IList_Contains(value As Object) As Boolean Implements IList.Contains
        Return Me.Contains(DirectCast(value, T))
    End Function

    Private Function IList_IndexOf(value As Object) As Integer Implements IList.IndexOf
        Return Me.IndexOf(DirectCast(value, T))
    End Function

    Private Sub IList_Insert(index As Integer, value As Object) Implements IList.Insert
        Me.Insert(index, DirectCast(value, T))
    End Sub

    Private Sub IList_Remove(value As Object) Implements IList.Remove
        Me.Remove(DirectCast(value, T))
    End Sub

    Public Overridable Sub Add(item As T) Implements ICollection(Of T).Add
        Me.Insert(Me.Count, item)
    End Sub

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.IEnumerable_1_GetEnumerator()
    End Function
#End Region

End Class
