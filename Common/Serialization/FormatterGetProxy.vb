Namespace Common

    Public Class FormatterGetProxy

        Public Sub New(ByVal Formatter As Formatter)
            Me._Formatter = Formatter
        End Sub

        Protected Friend Function [Get](Of T)(ByVal Name As String) As T
            Return Me.Formatter.Get(Of T)(Name)
        End Function

        Protected Friend Function [Get](ByVal Name As String) As Object
            Return Me.Formatter.Get(Name)
        End Function

        Protected Friend Sub [Get](Of T)(ByVal Name As String, ByVal Obj As T)
            Me.Formatter.Get(Name, Obj)
        End Sub

        Protected Friend Sub [Get](ByVal Name As String, ByVal Obj As Object)
            Me.Formatter.Get(Name, Obj)
        End Sub

#Region "Formatter Property"
        Private ReadOnly _Formatter As Formatter

        Public ReadOnly Property Formatter As Formatter
            Get
                Return Me._Formatter
            End Get
        End Property
#End Region

    End Class

End Namespace
