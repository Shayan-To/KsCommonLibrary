Namespace Common

    Public Class ReadOnlyCollectionWrapper(Of T)
        Implements ICollection,
                   ICollection(Of T),
                   IReadOnlyCollection(Of T)

        Public Sub New(ByVal Collection As ICollection(Of T))
            Me.Collection = Collection
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection.Count, ICollection(Of T).Count, IReadOnlyCollection(Of T).Count
            Get
                Return Me.Collection.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
            Get
                Return True
            End Get
        End Property

        Public ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
            Get
                Return True
            End Get
        End Property

        Public ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public Sub Add(item As T) Implements ICollection(Of T).Add
            Throw New NotSupportedException()
        End Sub

        Public Sub Clear() Implements ICollection(Of T).Clear
            Throw New NotSupportedException()
        End Sub

        Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
            Me.Collection.CopyTo(array, arrayIndex)
        End Sub

        Public Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
            For Each I In Me.Collection
                array.SetValue(I, index)
                index += 1
            Next
        End Sub

        Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
            Return Me.Collection.Contains(item)
        End Function

        Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.Collection.GetEnumerator()
        End Function

        Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
            Throw New NotSupportedException()
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
            Return Me.Collection.GetEnumerator()
        End Function

        Private ReadOnly Collection As ICollection(Of T)

    End Class

End Namespace
