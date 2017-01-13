Namespace Common

    Public Class MergedCollection(Of T)
        Implements ICollection(Of T),
                   ICollection

        Public Sub New(ByVal Collections As IEnumerable(Of ICollection(Of T)))
            Me.Collections = Collections.ToArray()
        End Sub

        Public Sub New(ParamArray ByVal Collections As ICollection(Of T)())
            Me.New(DirectCast(Collections, IEnumerable(Of ICollection(Of T))))
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count
            Get
                Return Me.Collections.Sum(Function(L) L.Count)
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
            Get
                Return True
            End Get
        End Property

        Private ReadOnly Property ICollection_Count As Integer Implements ICollection.Count
            Get
                Return Me.Count
            End Get
        End Property

        Public ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
            Get
                Return False
            End Get
        End Property

        Public Sub Add(item As T) Implements ICollection(Of T).Add
            Throw New NotSupportedException()
        End Sub

        Public Sub Clear() Implements ICollection(Of T).Clear
            Throw New NotSupportedException()
        End Sub

        Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
            For Each L In Me.Collections
                L.CopyTo(array, arrayIndex)
                arrayIndex += L.Count
            Next
        End Sub

        Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
            For Each L In Me.Collections
                If L.Contains(item) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Iterator Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
            For Each L In Me.Collections
                For Each I In L
                    Yield I
                Next
            Next
        End Function

        Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
            Throw New NotSupportedException()
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function

        Public Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
            For Each L In Me.Collections
                For Each I In L
                    array.SetValue(I, index)
                    index += 1
                Next
            Next
        End Sub

        Private ReadOnly Collections As ICollection(Of T)()

    End Class

End Namespace
