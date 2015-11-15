Namespace MVVM

    Public Class WorldedObject(Of TWorld)
        Inherits BindableBase

        Public Sub New(ByVal World As TWorld)
            Me._World = World
        End Sub

#Region "World Property"
        Private ReadOnly _World As TWorld

        Public ReadOnly Property World As TWorld
            Get
                Return Me._World
            End Get
        End Property
#End Region

    End Class

End Namespace
