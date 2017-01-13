Namespace Common

    Public Class ReadOnlyListWrapper(Of T)
        Inherits BaseReadOnlyList(Of T)

        Public Sub New(ByVal List As IList(Of T))
            Me.List = List
        End Sub

        Public Sub New(ByVal List As IReadOnlyList(Of T))
            Me.ROList = List
        End Sub

        Public Overrides ReadOnly Property Count As Integer
            Get
                If Me.List IsNot Nothing Then
                    Return Me.List.Count
                Else
                    Return Me.ROList.Count
                End If
            End Get
        End Property

        Default Public Overrides ReadOnly Property Item(Index As Integer) As T
            Get
                If Me.List IsNot Nothing Then
                    Return Me.List.Item(Index)
                Else
                    Return Me.ROList.Item(Index)
                End If
            End Get
        End Property

        Public Overrides Function GetEnumerator() As IEnumerator(Of T)
            If Me.List IsNot Nothing Then
                Return Me.List.GetEnumerator()
            Else
                Return Me.ROList.GetEnumerator()
            End If
        End Function

        Private ReadOnly List As IList(Of T)
        Private ReadOnly ROList As IReadOnlyList(Of T)

    End Class

End Namespace
