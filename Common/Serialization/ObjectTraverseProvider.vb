Namespace Common

    Public MustInherit Class ObjectTraverseProvider

        Protected MustOverride Function GetSetPropertiesOverride(ByVal Handler As PropertyTraverseHandler) As IEnumerable(Of Void)

    End Class


    Public Class PropertyTraverseHandler



    End Class

    Public Structure PropertyTraverseCurrent

        Public Sub New(ByVal Handler As PropertyTraverseHandler)
            Me._Handler = Handler
        End Sub

#Region "Handler Property"
        Private ReadOnly _Handler As PropertyTraverseHandler

        Public ReadOnly Property Handler As PropertyTraverseHandler
            Get
                Return Me._Handler
            End Get
        End Property
#End Region

    End Structure

    Public Interface PropertyTraverseProcessor

        Sub Process(Of T)(ByVal Prop As PropertyTraverseHandler)

    End Interface

End Namespace
