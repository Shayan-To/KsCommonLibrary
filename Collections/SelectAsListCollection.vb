Public Class SelectAsListCollection(Of TIn, TOut)
    Inherits BaseReadOnlyList(Of TOut)

    Public Sub New(ByVal List As IReadOnlyList(Of TIn), ByVal Func As Func(Of TIn, TOut))
        Me.List = List
        Me.Func = Func
    End Sub

    Public Overrides ReadOnly Property Count As Integer
        Get
            Return Me.List.Count
        End Get
    End Property

    Default Public Overrides ReadOnly Property Item(ByVal Index As Integer) As TOut
        Get
            Return Me.Func.Invoke(Me.List.Item(Index))
        End Get
    End Property

    Public Overrides Iterator Function GetEnumerator() As IEnumerator(Of TOut)
        For Each I In Me.List
            Yield Me.Func.Invoke(I)
        Next
    End Function

    Protected ReadOnly List As IReadOnlyList(Of TIn)
    Protected ReadOnly Func As Func(Of TIn, TOut)

End Class
