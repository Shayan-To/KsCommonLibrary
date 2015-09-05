Public Class DefaultCacher(Of T As New)

    Private Shared ReadOnly _Cached As T = New T()

    Public Shared ReadOnly Property Cached As T
        Get
            Return _Cached
        End Get
    End Property

End Class
