Public Class InserstionSorter(Of T)

    Public Sub New(ByVal Comparer As IComparer(Of T))
        Me.Comparer = Comparer
    End Sub

    Public Sub New()
        Me.New(Generic.Comparer(Of T).Default)
    End Sub

    Public Sub AddItem(ByVal Item As T)
        Dim I = Me.List.First
        Do
            If Me.Comparer.Compare(I.Value, Item) > 0 Then

            End If
        Loop
    End Sub

    Private ReadOnly Comparer As IComparer(Of T)
    Private ReadOnly List As LinkedList(Of T) = New LinkedList(Of T)()

End Class
