Public MustInherit Class Serializer

    Public Sub New(ByVal Id As String)
        Me._Id = Id
    End Sub

    Public MustOverride Function CanSerializeType(ByVal Type As Type) As Boolean
    Public MustOverride Sub [Set](ByVal Formatter As FormatterSetProxy, ByVal Obj As Object)

    Public Overridable Function [Get](ByVal Formatter As FormatterGetProxy) As Object
        Throw New NotSupportedException()
    End Function

    Public Overridable Sub [Get](ByVal Formatter As FormatterGetProxy, ByVal Obj As Object)
        Throw New NotSupportedException()
    End Sub

#Region "Id Property"
    Private ReadOnly _Id As String

    Public ReadOnly Property Id As String
        Get
            Return Me._Id
        End Get
    End Property
#End Region

    Public Shared Function Create(ByVal Id As String, ByVal CanSerialize As Func(Of Type, Boolean), ByVal [Get] As Func(Of FormatterGetProxy, Object), ByVal Get2 As Action(Of FormatterGetProxy, Object), ByVal [Set] As Action(Of FormatterSetProxy, Object)) As Serializer
        Return New DelegateSerializer(Id, CanSerialize, [Get], Get2, [Set])
    End Function

    Private Class DelegateSerializer
        Inherits Serializer

        Public Sub New(ByVal Id As String, ByVal CanSerialize As Func(Of Type, Boolean), ByVal [Get] As Func(Of FormatterGetProxy, Object), ByVal Get2 As Action(Of FormatterGetProxy, Object), ByVal [Set] As Action(Of FormatterSetProxy, Object))
            MyBase.New(Id)
            Me.CanSerializeDelegate = CanSerialize
            Me.GetDelegate = [Get]
            Me.GetDelegate2 = Get2
            Me.SetDelegate = [Set]
        End Sub

        Public Overrides Function CanSerializeType(ByVal Type As Type) As Boolean
            Return Me.CanSerializeDelegate.Invoke(Type)
        End Function

        Public Overrides Sub [Set](Formatter As FormatterSetProxy, Obj As Object)
            Me.SetDelegate.Invoke(Formatter, Obj)
        End Sub

        Public Overrides Function [Get](Formatter As FormatterGetProxy) As Object
            If Me.GetDelegate Is Nothing Then
                Throw New NotSupportedException()
            End If
            Return Me.GetDelegate.Invoke(Formatter)
        End Function

        Public Overrides Sub [Get](Formatter As FormatterGetProxy, Obj As Object)
            If Me.GetDelegate2 Is Nothing Then
                Throw New NotSupportedException()
            End If
            Me.GetDelegate2.Invoke(Formatter, Obj)
        End Sub

        Private ReadOnly CanSerializeDelegate As Func(Of Type, Boolean)
        Private ReadOnly GetDelegate As Func(Of FormatterGetProxy, Object)
        Private ReadOnly GetDelegate2 As Action(Of FormatterGetProxy, Object)
        Private ReadOnly SetDelegate As Action(Of FormatterSetProxy, Object)

    End Class

End Class

Public MustInherit Class Serializer(Of T)
    Inherits Serializer

    Public Sub New(ByVal Id As String)
        MyBase.New(Id)
    End Sub

    Public MustOverride Sub SetT(ByVal Formatter As FormatterSetProxy, ByVal Obj As T)

    Public Overridable Function GetT(ByVal Formatter As FormatterGetProxy) As T
        Throw New NotSupportedException()
    End Function

    Public Overridable Sub GetT(ByVal Formatter As FormatterGetProxy, ByVal Obj As T)
        Throw New NotSupportedException()
    End Sub

    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)>
    Public Overrides Function CanSerializeType(ByVal Type As Type) As Boolean
        Return Type = GetType(T)
    End Function

    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)>
    Public NotOverridable Overrides Function [Get](Formatter As FormatterGetProxy) As Object
        Return Me.GetT(Formatter)
    End Function

    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)>
    Public NotOverridable Overrides Sub [Get](Formatter As FormatterGetProxy, Obj As Object)
        Me.GetT(Formatter, DirectCast(Obj, T))
    End Sub

    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)>
    Public NotOverridable Overrides Sub [Set](Formatter As FormatterSetProxy, Obj As Object)
        Me.SetT(Formatter, DirectCast(Obj, T))
    End Sub

    Public Shared Shadows Function Create(ByVal Id As String, ByVal [Get] As Func(Of FormatterGetProxy, T), ByVal Get2 As Action(Of FormatterGetProxy, T), ByVal [Set] As Action(Of FormatterSetProxy, T)) As Serializer(Of T)
        Return New DelegateSerializer(Id, [Get], Get2, [Set])
    End Function

    Private Class DelegateSerializer
        Inherits Serializer(Of T)

        Public Sub New(ByVal Id As String, ByVal [Get] As Func(Of FormatterGetProxy, T), ByVal Get2 As Action(Of FormatterGetProxy, T), ByVal [Set] As Action(Of FormatterSetProxy, T))
            MyBase.New(Id)
            Me.GetDelegate = [Get]
            Me.SetDelegate = [Set]
            Me.GetDelegate2 = Get2
        End Sub

        Public Overrides Sub SetT(Formatter As FormatterSetProxy, Obj As T)
            Me.SetDelegate.Invoke(Formatter, Obj)
        End Sub

        Public Overrides Function GetT(Formatter As FormatterGetProxy) As T
            If Me.GetDelegate Is Nothing Then
                Throw New NotSupportedException()
            End If
            Return Me.GetDelegate.Invoke(Formatter)
        End Function

        Public Overrides Sub GetT(Formatter As FormatterGetProxy, Obj As T)
            If Me.GetDelegate2 Is Nothing Then
                Throw New NotSupportedException()
            End If
            Me.GetDelegate2.Invoke(Formatter, Obj)
        End Sub

        Private ReadOnly GetDelegate As Func(Of FormatterGetProxy, T)
        Private ReadOnly GetDelegate2 As Action(Of FormatterGetProxy, T)
        Private ReadOnly SetDelegate As Action(Of FormatterSetProxy, T)

    End Class

End Class
