Namespace Common

    Public Class SelectAsListCollection(Of TIn, TOut)
        Inherits BaseReadOnlyList(Of TOut)

        Public Sub New(ByVal List As IReadOnlyList(Of TIn), ByVal Func As Func(Of TIn, TOut))
            Me.List = List
            Me.Func = Func
        End Sub

        Public Sub New(ByVal List As IReadOnlyList(Of TIn), ByVal Func As Func(Of TIn, Integer, TOut))
            Me.List = List
            Me.FuncIndexed = Func
        End Sub

        Protected Function GetMappedElement(ByVal Inp As TIn, ByVal Index As Integer) As TOut
            If Me.Func IsNot Nothing Then
                Return Me.Func.Invoke(Inp)
            End If
            Return Me.FuncIndexed.Invoke(Inp, Index)
        End Function

        Public Overrides ReadOnly Property Count As Integer
            Get
                Return Me.List.Count
            End Get
        End Property

        Default Public Overrides ReadOnly Property Item(ByVal Index As Integer) As TOut
            Get
                Return Me.GetMappedElement(Me.List.Item(Index), Index)
            End Get
        End Property

        Public Overrides Iterator Function GetEnumerator() As IEnumerator(Of TOut)
            Dim Ind = 0
            For Each I In Me.List
                Yield Me.GetMappedElement(I, Ind)
                Ind += 1
            Next
        End Function

        Protected ReadOnly List As IReadOnlyList(Of TIn)
        Protected ReadOnly Func As Func(Of TIn, TOut)
        Protected ReadOnly FuncIndexed As Func(Of TIn, Integer, TOut)

    End Class

End Namespace
