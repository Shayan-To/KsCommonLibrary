Namespace Common

    Public Class ListSerializer(Of T)
        Inherits Serializer(Of IEnumerable(Of T))

        Public Sub New()
            MyBase.New(NameOf(List(Of Object)))
        End Sub

        Public Overrides Sub SetT(Formatter As FormatterSetProxy, Obj As IEnumerable(Of T))
            Formatter.Set(NameOf(Obj.Count), Obj.Count())
            For Each I In Obj
                Formatter.Set(Nothing, I)
            Next
        End Sub

        Public Overrides Function GetT(Formatter As FormatterGetProxy) As IEnumerable(Of T)
            Dim Count As Integer
            Count = Formatter.Get(Of Integer)(NameOf(Count))

            Dim R = New List(Of T)(Count)
            For I As Integer = 0 To Count - 1
                R.Add(Formatter.Get(Of T)(Nothing))
            Next

            Return R
        End Function

    End Class

End Namespace
