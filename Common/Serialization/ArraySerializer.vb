Public Class ArraySerializer(Of T)
    Inherits Serializer(Of T())

    Public Sub New()
        MyBase.New(NameOf(Array))
    End Sub

    Public Overrides Sub SetT(Formatter As FormatterSetProxy, Obj As T())
        Formatter.Set(NameOf(Obj.Length), Obj.Length)
        For I As Integer = 0 To Obj.Length - 1
            Formatter.Set(Nothing, Obj(I))
        Next
    End Sub

    Public Overrides Function GetT(Formatter As FormatterGetProxy) As T()
        Dim Length As Integer
        Length = Formatter.Get(Of Integer)(NameOf(Length))

        Dim R = New T(Length - 1) {}
        For I As Integer = 0 To Length - 1
            R(I) = Formatter.Get(Of T)(Nothing)
        Next

        Return R
    End Function

End Class
