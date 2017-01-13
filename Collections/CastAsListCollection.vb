Namespace Common

    Public Class CastAsListCollection(Of T)
        Inherits BaseReadOnlyList(Of T)

        Public Sub New(ByVal List As IList)
            Me.List = List
        End Sub

        Public Overrides ReadOnly Property Count As Integer
            Get
                Return Me.List.Count
            End Get
        End Property

        Default Public Overrides ReadOnly Property Item(ByVal Index As Integer) As T
            Get
                Return DirectCast(Me.List.Item(Index), T)
            End Get
        End Property

        Public Overrides Function GetEnumerator() As IEnumerator(Of T)
            Return Me.List.Cast(Of T).GetEnumerator()
        End Function

        Private ReadOnly List As IList

    End Class

End Namespace
