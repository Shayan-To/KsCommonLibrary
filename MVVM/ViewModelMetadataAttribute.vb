Namespace MVVM

    <AttributeUsage(AttributeTargets.Class, Inherited:=False)>
    Public Class ViewModelMetadataAttribute
        Inherits Attribute

        Public Sub New(ByVal ViewType As Type)
            Me._ViewType = ViewType
        End Sub

#Region "ViewType Property"
        Private ReadOnly _ViewType As Type

        Public ReadOnly Property ViewType As Type
            Get
                Return Me._ViewType
            End Get
        End Property
#End Region

#Region "IsSingleInstance Property"
        Private _IsSingleInstance As Boolean = False

        Public Property IsSingleInstance As Boolean
            Get
                Return Me._IsSingleInstance
            End Get
            Set(ByVal Value As Boolean)
                Me._IsSingleInstance = Value
            End Set
        End Property
#End Region

    End Class

End Namespace
