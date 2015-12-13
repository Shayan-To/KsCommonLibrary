Public Class MergedCollection(Of T)
    Implements ICollection(Of T)

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
        Throw New NotSupportedException()
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

    Private ReadOnly Collections As ICollection(Of T)()

End Class
