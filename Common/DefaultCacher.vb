Namespace Common

    Public Class DefaultCacher(Of T As New)

        Private Shared ReadOnly _Value As T = New T()

        Public Shared ReadOnly Property Value As T
            Get
                Return _Value
            End Get
        End Property

    End Class

End Namespace
