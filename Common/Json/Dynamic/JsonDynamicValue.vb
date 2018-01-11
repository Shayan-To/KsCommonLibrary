Namespace Common

    Public Class JsonDynamicValue
        Inherits JsonDynamicBase

        Public Sub New(ByVal Value As String, ByVal IsString As Boolean)
            Me._Value = Value
            Me._IsString = IsString
        End Sub

        Public Shared Widening Operator CType(ByVal Value As String) As JsonDynamicValue
            Return New JsonDynamicValue(Value, True)
        End Operator

        Public Shared Widening Operator CType(ByVal Value As JsonDynamicValue) As String
            Verify.True(Value.IsString, "Value is not a string.")
            Return Value.Value
        End Operator

        Public Shared Widening Operator CType(ByVal Value As Integer) As JsonDynamicValue
            Return New JsonDynamicValue(Value.ToStringInv(), False)
        End Operator

        Public Shared Widening Operator CType(ByVal Value As JsonDynamicValue) As Integer
            Verify.False(Value.IsString, "Value is a string.")
            Return Value.GetInteger()
        End Operator

        Public Shared Widening Operator CType(ByVal Value As Double) As JsonDynamicValue
            Return New JsonDynamicValue(Value.ToStringInv(), False)
        End Operator

        Public Shared Widening Operator CType(ByVal Value As JsonDynamicValue) As Double
            Verify.False(Value.IsString, "Value is a string.")
            Return Value.GetDouble()
        End Operator

        Public Shared Widening Operator CType(ByVal Value As Boolean) As JsonDynamicValue
            Return New JsonDynamicValue(If(Value, "true", "false"), False)
        End Operator

        Public Shared Widening Operator CType(ByVal Value As JsonDynamicValue) As Boolean
            Verify.False(Value.IsString, "Value is a string.")
            Return Value.GetBoolean()
        End Operator

        Public Shared Operator =(ByVal L As JsonDynamicValue, ByVal R As JsonDynamicValue) As Boolean
            Return L.Value = R.Value And L.IsString = R.IsString
        End Operator

        Public Shared Operator <>(ByVal L As JsonDynamicValue, ByVal R As JsonDynamicValue) As Boolean
            Return Not L = R
        End Operator

        Public Overrides Function Equals(obj As Object) As Boolean
            Dim T = TryCast(obj, JsonDynamicValue)
            If T Is Nothing Then
                Return False
            End If
            Return Me = T
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Utilities.CombineHashCodes(Me.Value.GetHashCode(), Me.IsString.GetHashCode())
        End Function

        Public Overrides Function ToString() As String
            Return If(Me.IsString, """", "") & Me.Value & If(Me.IsString, """", "")
        End Function

#Region "Value Read-Only Property"
        Private ReadOnly _Value As String

        Public ReadOnly Property Value As String
            Get
                Return Me._Value
            End Get
        End Property
#End Region

#Region "IsString Read-Only Property"
        Private ReadOnly _IsString As Boolean

        Public ReadOnly Property IsString As Boolean
            Get
                Return Me._IsString
            End Get
        End Property
#End Region

        Friend Const [True] = "true"
        Friend Const [False] = "false"

    End Class

End Namespace
