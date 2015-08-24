Namespace MVVM

    Public Class WorldedObject(Of T)
        Inherits BindableBase

        Public Sub New(ByVal World As T)
            Me._World = World
        End Sub

#Region "World Property"
        Private ReadOnly _World As T

        Public ReadOnly Property World As T
            Get
                Return Me._World
            End Get
        End Property
#End Region

    End Class

End Namespace
