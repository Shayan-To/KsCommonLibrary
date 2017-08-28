Namespace Common

    Public Class InsertionSortList(Of T)
        Inherits BaseList(Of T)

        Public Sub New()
            Me.New(Generic.Comparer(Of T).Default)
        End Sub

        Public Sub New(ByVal Comparer As IComparer(Of T))
            Me.Comparer = Comparer
        End Sub

        Public Overrides Sub Insert(index As Integer, item As T)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Add(item As T)
            Throw New NotImplementedException()
            Dim I = Me.List.First
            Do
                If Me.Comparer.Compare(I.Value, item) > 0 Then

                End If
            Loop
        End Sub

        Public Overrides Function IndexOf(item As T) As Integer
            Throw New NotImplementedException()
        End Function

        Public Overrides Sub RemoveAt(index As Integer)
            Throw New NotImplementedException()
        End Sub

        Public Overrides Sub Clear()
            Throw New NotImplementedException()
        End Sub

        Protected Overrides Function IEnumerable_1_GetEnumerator() As IEnumerator(Of T)
            Return Me.GetEnumerator()
        End Function

        Public Function GetEnumerator() As IEnumerator(Of T)
            Throw New NotImplementedException()
        End Function

        Default Public Overrides Property Item(index As Integer) As T
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As T)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Overrides ReadOnly Property Count As Integer
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Private ReadOnly Comparer As IComparer(Of T)
        Private ReadOnly List As LinkedList(Of T) = New LinkedList(Of T)()

    End Class

End Namespace
