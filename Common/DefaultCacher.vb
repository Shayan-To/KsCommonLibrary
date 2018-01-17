Namespace Common

    Public MustInherit Class DefaultCacher(Of T As New)

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        Public Shared Sub SetValue(ByVal Value As T)
            _Value.Value = Value
        End Sub

        Private Shared ReadOnly _Value As Threading.ThreadLocal(Of T) = New Threading.ThreadLocal(Of T)(Function() New T())

        Public Shared ReadOnly Property Value As T
            Get
                Return _Value.Value
            End Get
        End Property

    End Class

End Namespace
