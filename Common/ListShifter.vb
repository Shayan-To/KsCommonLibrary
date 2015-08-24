Public Class ListShifter(Of T)
    Implements IList(Of T)

    Private ReadOnly InnerList As IList(Of T),
                     Shift As Integer

    Public Sub New(ByVal List As IList(Of T), ByVal Shift As Integer)
        Me.InnerList = List
        Me.Shift = Shift
    End Sub

    Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count
        Get
            Return Me.InnerList.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
        Get
            Return Me.InnerList.IsReadOnly
        End Get
    End Property

    Default Public Property Item(index As Integer) As T Implements IList(Of T).Item
        Get
            Return Me.InnerList.Item(index + Me.Shift)
        End Get
        Set(value As T)
            Me.InnerList.Item(index + Me.Shift) = value
        End Set
    End Property

    Public Sub Add(item As T) Implements ICollection(Of T).Add
        Me.InnerList.Add(item)
    End Sub

    Public Sub Clear() Implements ICollection(Of T).Clear
        Me.InnerList.Clear()
    End Sub

    Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
        Me.InnerList.CopyTo(array, arrayIndex)
    End Sub

    Public Sub Insert(index As Integer, item As T) Implements IList(Of T).Insert
        Me.InnerList.Insert(index + Me.Shift, item)
    End Sub

    Public Sub RemoveAt(index As Integer) Implements IList(Of T).RemoveAt
        Me.InnerList.RemoveAt(index + Me.Shift)
    End Sub

    Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
        Return Me.InnerList.Contains(item)
    End Function

    Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Return Me.InnerList.GetEnumerator()
    End Function

    Public Function IndexOf(item As T) As Integer Implements IList(Of T).IndexOf
        Return Me.InnerList.IndexOf(item) - Me.Shift
    End Function

    Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
        Return Me.InnerList.Remove(item)
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function
End Class
