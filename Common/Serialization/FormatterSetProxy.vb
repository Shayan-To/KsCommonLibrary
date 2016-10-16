Public Class FormatterSetProxy

    Public Sub New(ByVal Formatter As Formatter)
        Me._Formatter = Formatter
    End Sub

    Protected Friend Sub [Set](Of T)(ByVal Name As String, ByVal Obj As T)
        Me.Formatter.Set(Name, Obj)
    End Sub

    Protected Friend Sub [Set](ByVal Name As String, ByVal Obj As Object)
        Me.Formatter.Set(Name, Obj)
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
