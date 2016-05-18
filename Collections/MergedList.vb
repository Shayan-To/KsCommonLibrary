Public Class MergedList(Of T)
    Implements IList(Of T)

    Public Sub New(ByVal Lists As IEnumerable(Of IList(Of T)))
        Me.Lists = Lists.ToArray()
    End Sub

    Public Sub New(ParamArray ByVal Lists As IList(Of T)())
        Me.New(DirectCast(Lists, IEnumerable(Of IList(Of T))))
    End Sub

    Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count
        Get
            Return Me.Lists.Sum(Function(L) L.Count)
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
        Get
            Return True
        End Get
    End Property

    Default Public Property Item(Index As Integer) As T Implements IList(Of T).Item
        Get
            For Each L In Me.Lists
                Dim Count As Integer = L.Count
                If Index < Count Then
                    Return L.Item(Index)
                End If
                Index -= Count
            Next
            Throw New ArgumentOutOfRangeException(NameOf(Index))
        End Get
        Set(Value As T)
            Throw New NotSupportedException()
        End Set
    End Property

    Public Sub Add(item As T) Implements ICollection(Of T).Add
        Throw New NotSupportedException()
    End Sub

    Public Sub Clear() Implements ICollection(Of T).Clear
        Throw New NotSupportedException()
    End Sub

    Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
        For Each L In Me.Lists
            L.CopyTo(array, arrayIndex)
            arrayIndex += L.Count
        Next
    End Sub

    Public Sub Insert(index As Integer, item As T) Implements IList(Of T).Insert
        Throw New NotSupportedException()
    End Sub

    Public Sub RemoveAt(index As Integer) Implements IList(Of T).RemoveAt
        Throw New NotSupportedException()
    End Sub

    Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
        Throw New NotSupportedException()
    End Function

    Public Iterator Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        For Each L In Me.Lists
            For Each I In L
                Yield I
            Next
        Next
    End Function

    Public Function IndexOf(item As T) As Integer Implements IList(Of T).IndexOf
        Dim I = 0
        For Each L In Me.Lists
            For Each It In L
                If It.Equals(item) Then
                    Return I
                End If
                I += 1
            Next
        Next
        Return -1
    End Function

    Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
        Throw New NotSupportedException()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function

    Private ReadOnly Lists As IList(Of T)()

End Class
