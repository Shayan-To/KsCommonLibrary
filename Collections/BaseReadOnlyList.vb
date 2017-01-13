Namespace Common

    Public MustInherit Class BaseReadOnlyList(Of T)
        Implements IReadOnlyList(Of T), IList(Of T), IList

        Public MustOverride ReadOnly Property Count As Integer Implements IReadOnlyCollection(Of T).Count, ICollection.Count, ICollection(Of T).Count

        Default Public MustOverride ReadOnly Property Item(ByVal Index As Integer) As T Implements IReadOnlyList(Of T).Item

        Public MustOverride Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function

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

#Region "Junk"
        Private Property IList_1_Item(index As Integer) As T Implements IList(Of T).Item
            Get
                Return Me.Item(index)
            End Get
            Set(value As T)
                Throw New NotSupportedException()
            End Set
        End Property

        Private Property IList_Item(index As Integer) As Object Implements IList.Item
            Get
                Return Me.Item(index)
            End Get
            Set(value As Object)
                Throw New NotSupportedException()
            End Set
        End Property

        Private ReadOnly Property IList_IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly, IList.IsReadOnly
            Get
                Return True
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
                Return True
            End Get
        End Property

        Private Sub IList_Insert(index As Integer, item As T) Implements IList(Of T).Insert
            Throw New NotSupportedException()
        End Sub

        Private Sub IList_RemoveAt(index As Integer) Implements IList(Of T).RemoveAt, IList.RemoveAt
            Throw New NotSupportedException()
        End Sub

        Private Sub IList_Add(item As T) Implements ICollection(Of T).Add
            Throw New NotSupportedException()
        End Sub

        Private Sub IList_Clear() Implements ICollection(Of T).Clear, IList.Clear
            Throw New NotSupportedException()
        End Sub

        Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
            Return Me.IndexOf(item) <> -1
        End Function

        Private Function IList_Remove(item As T) As Boolean Implements ICollection(Of T).Remove
            Throw New NotSupportedException()
        End Function

        Private Function IList_Add(value As Object) As Integer Implements IList.Add
            Throw New NotSupportedException()
        End Function

        Private Function IList_Contains(value As Object) As Boolean Implements IList.Contains
            Return Me.Contains(DirectCast(value, T))
        End Function

        Private Function IList_IndexOf(value As Object) As Integer Implements IList.IndexOf
            Return Me.IndexOf(DirectCast(value, T))
        End Function

        Private Sub IList_Insert(index As Integer, value As Object) Implements IList.Insert
            Throw New NotSupportedException()
        End Sub

        Private Sub IList_Remove(value As Object) Implements IList.Remove
            Throw New NotSupportedException()
        End Sub
#End Region

    End Class

End Namespace
