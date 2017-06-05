Namespace Common.MVVM

    <AttributeUsage(AttributeTargets.Class, Inherited:=False)>
    Public Class ViewModelMetadataAttribute
        Inherits Attribute

        Public Sub New(ByVal ViewType As Type)
            Me._ViewType = ViewType
            Verify.True(GetType(Window).IsAssignableFrom(ViewType) Or GetType(Page).IsAssignableFrom(ViewType), "ViewType must be a Window or a Page.")
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
        Private _IsSingleInstance_Set As Boolean = False

        Public Property IsSingleInstance As Boolean
            Get
                Return Me._IsSingleInstance
            End Get
            Set(ByVal Value As Boolean)
                If Me._IsSingleInstance_Set Then
                    Throw New InvalidOperationException("Cannot set the IsSingleInstance property multiple times.")
                End If
                Me._IsSingleInstance_Set = True
                Me._IsSingleInstance = Value
            End Set
        End Property
#End Region

#Region "KsApplicationType Property"
        Private _KsApplicationType As CNullable(Of Type)

        Public Property KsApplicationType As Type
            Get
                Return Me._KsApplicationType.Value
            End Get
            Set(ByVal Value As Type)
                If Me._KsApplicationType.HasValue Then
                    Throw New InvalidOperationException("Cannot set the KsApplicationType property multiple times.")
                End If
                Me._KsApplicationType = Value
            End Set
        End Property
#End Region

    End Class

End Namespace
