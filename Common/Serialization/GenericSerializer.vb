Namespace Common

    Public MustInherit Class GenericSerializer
        Inherits Serializer

        Public Sub New(ByVal Id As String, ByVal SerializerType As Type)
            MyBase.New(Id)
            Verify.True(SerializerType.IsGenericTypeDefinition)
            Me._SerializerType = SerializerType
        End Sub

        Protected MustOverride Function GetTypeArguments(ByVal Type As Type) As Type()

        Private Function GetSerializer(ByVal TypeArguments As Type()) As Serializer
            Dim S As Serializer = Nothing
            If Me.Cache.TryGetValue(TypeArguments, S) Then
                Return S
            End If

            S = DirectCast(Me.SerializerType.MakeGenericType(TypeArguments).CreateInstance(), Serializer)

            Do Until Me.Cache.Count < CacheLimit
                Me.Cache.RemoveAt(0)
            Loop

            Me.Cache.Add(TypeArguments, S)
            Return S
        End Function

        Public NotOverridable Overrides Function CanSerializeType(ByVal Type As Type) As Boolean
            Return Me.GetTypeArguments(Type) IsNot Nothing
        End Function

        Public NotOverridable Overrides Sub [Set](Formatter As FormatterSetProxy, Obj As Object)
            Dim TypeArguments = Me.GetTypeArguments(Obj.GetType())
            Dim S = Me.GetSerializer(TypeArguments)

            Dim IsSingle = TypeArguments.Length = 1
            Formatter.Set(NameOf(IsSingle), IsSingle)

            If IsSingle Then
                Formatter.Set(NameOf(TypeArguments), TypeArguments(0))
            Else
                Formatter.Set(NameOf(TypeArguments), TypeArguments)
            End If
            S.Set(Formatter, Obj)
        End Sub

        Public NotOverridable Overrides Function [Get](Formatter As FormatterGetProxy) As Object
            Dim TypeArguments As Type()
            Dim IsSingle As Boolean
            IsSingle = Formatter.Get(Of Boolean)(NameOf(IsSingle))
            If IsSingle Then
                TypeArguments = {Formatter.Get(Of Type)(NameOf(TypeArguments))}
            Else
                TypeArguments = Formatter.Get(Of Type())(NameOf(TypeArguments))
            End If

            Dim S = Me.GetSerializer(TypeArguments)
            Return S.Get(Formatter)
        End Function

        Public NotOverridable Overrides Sub [Get](Formatter As FormatterGetProxy, Obj As Object)
            Throw New NotSupportedException()
        End Sub

#Region "SerializerType Property"
        Private ReadOnly _SerializerType As Type

        Public ReadOnly Property SerializerType As Type
            Get
                Return Me._SerializerType
            End Get
        End Property
#End Region

        Private Const CacheLimit As Integer = 10

        Private ReadOnly Cache As OrderedDictionary(Of Type(), Serializer) = New OrderedDictionary(Of Type(), Serializer)(New ArrayComparer(Of Type)())

        Public Shared Shadows Function Create(ByVal Id As String, ByVal SerializerType As Type, ByVal GetTypeArgument As Func(Of Type, Type())) As GenericSerializer
            Return New DelegateGenericSerializer(Id, SerializerType, GetTypeArgument)
        End Function

        Public Class DelegateGenericSerializer
            Inherits GenericSerializer

            Public Sub New(ByVal Id As String, ByVal SerializerType As Type, ByVal GetTypeArgument As Func(Of Type, Type()))
                MyBase.New(Id, SerializerType)
                Me.GetTypeArgumentDelegate = GetTypeArgument
            End Sub

            Protected Overrides Function GetTypeArguments(Type As Type) As Type()
                Return Me.GetTypeArgumentDelegate.Invoke(Type)
            End Function

            Private ReadOnly GetTypeArgumentDelegate As Func(Of Type, Type())

        End Class

    End Class

End Namespace
