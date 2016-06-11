Namespace MVVM

    Public Class NavigationFrame
        Implements IList(Of ViewModel)

        Public Sub New(ByVal List As IEnumerable(Of ViewModel))
            Me.List = List.ToArray()
        End Sub

#Region "Trivial"
        Public ReadOnly Property Count As Integer Implements ICollection(Of ViewModel).Count
            Get
                Return Me.List.Length
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of ViewModel).IsReadOnly
            Get
                Return True
            End Get
        End Property

        Public Sub CopyTo(Array() As ViewModel, Index As Integer) Implements ICollection(Of ViewModel).CopyTo
            Me.List.CopyTo(Array, Index)
        End Sub

        Public Function Contains(item As ViewModel) As Boolean Implements ICollection(Of ViewModel).Contains
            Return Me.IndexOf(item) <> -1
        End Function

        Public Function GetEnumerator() As IEnumerator(Of ViewModel) Implements IEnumerable(Of ViewModel).GetEnumerator
            Return DirectCast(Me.List, IEnumerable(Of ViewModel)).GetEnumerator()
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.List.GetEnumerator()
        End Function

        Default Public Property Item(index As Integer) As ViewModel Implements IList(Of ViewModel).Item
            Get
                Return Me.List(index)
            End Get
#End Region

#Region "NotSupported"
            Set(value As ViewModel)
                Throw New NotSupportedException()
            End Set
        End Property

        Public Sub Add(item As ViewModel) Implements ICollection(Of ViewModel).Add
            Throw New NotSupportedException()
        End Sub

        Public Sub Clear() Implements ICollection(Of ViewModel).Clear
            Throw New NotSupportedException()
        End Sub

        Public Sub Insert(index As Integer, item As ViewModel) Implements IList(Of ViewModel).Insert
            Throw New NotSupportedException()
        End Sub

        Public Sub RemoveAt(index As Integer) Implements IList(Of ViewModel).RemoveAt
            Throw New NotSupportedException()
        End Sub

        Public Function Remove(item As ViewModel) As Boolean Implements ICollection(Of ViewModel).Remove
            Throw New NotSupportedException()
        End Function
#End Region

        Public Function IndexOf(ByVal Frame As ViewModel) As Integer Implements IList(Of ViewModel).IndexOf
            For I As Integer = 0 To Me.Count - 1
                If Me.Item(I) Is Frame Then
                    Return I
                End If
            Next
            Return -1
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            Dim Frame = TryCast(obj, NavigationFrame)
            If obj Is Nothing Then
                Return False
            End If
            Return Me = Frame
        End Function

        Public Shared Operator =(ByVal Left As NavigationFrame, ByVal Right As NavigationFrame) As Boolean
            If Left.List.Length <> Right.List.Length Then
                Return False
            End If

            For I As Integer = 0 To Left.List.Length - 1
                If Left.List(I) IsNot Right.List(I) Then
                    Return False
                End If
            Next

            Return True
        End Operator

        Public Shared Operator <>(ByVal Left As NavigationFrame, ByVal Right As NavigationFrame) As Boolean
            Return Not Left = Right
        End Operator

        Private ReadOnly List As ViewModel()

    End Class

End Namespace
