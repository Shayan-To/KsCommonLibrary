Namespace Common

    Public Class Array2D(Of T)

        Public Sub New(ByVal Width As Integer, ByVal Height As Integer)
            Me.Array = New T(Width - 1, Height - 1) {}
        End Sub

        Default Public Property Item(ByVal I As Integer, ByVal J As Integer) As T
            Get
                Return Me.Array(I, J)
            End Get
            Set(ByVal Value As T)
                Me.Array(I, J) = Value
            End Set
        End Property

        Private ReadOnly Array As T(,)

    End Class

End Namespace
