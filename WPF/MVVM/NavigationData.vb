Namespace Common.MVVM

    Public Structure NavigationData

        Public Sub New(ByVal Frame As NavigationFrame, Optional ByVal AddToStack As Boolean = True, Optional ByVal ForceToStack As Boolean = False)
            Me._Frame = Frame
            Me._AddToStack = AddToStack
            Me._ForceToStack = ForceToStack
        End Sub

#Region "Frame Read-Only Property"
        Private ReadOnly _Frame As NavigationFrame

        Public ReadOnly Property Frame As NavigationFrame
            Get
                Return Me._Frame
            End Get
        End Property
#End Region

#Region "AddToStack Read-Only Property"
        Private ReadOnly _AddToStack As Boolean

        Public ReadOnly Property AddToStack As Boolean
            Get
                Return Me._AddToStack
            End Get
        End Property
#End Region

#Region "ForceToStack Read-Only Property"
        Private ReadOnly _ForceToStack As Boolean

        Public ReadOnly Property ForceToStack As Boolean
            Get
                Return Me._ForceToStack
            End Get
        End Property
#End Region

    End Structure

End Namespace
