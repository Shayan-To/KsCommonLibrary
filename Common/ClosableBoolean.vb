Public Structure ClosableBoolean
    Implements IDisposable

    Public Shared Widening Operator CType(ByVal O As ClosableBoolean) As Boolean
        Return O._Value
    End Operator

    Public Shared Widening Operator CType(ByVal O As Boolean) As ClosableBoolean
        Return New ClosableBoolean() With {._Value = O}
    End Operator

    Public Shared Operator Not(ByVal O As ClosableBoolean) As Boolean
        Return Not O._Value
    End Operator

    Public Shared Operator And(ByVal O1 As ClosableBoolean, ByVal O2 As Boolean) As Boolean
        Return O1._Value And O2
    End Operator

    Public Shared Operator Or(ByVal O1 As ClosableBoolean, ByVal O2 As Boolean) As Boolean
        Return O1._Value Or O2
    End Operator

    Public Shared Operator Xor(ByVal O1 As ClosableBoolean, ByVal O2 As Boolean) As Boolean
        Return O1._Value Xor O2
    End Operator

    Public Shared Operator And(ByVal O1 As Boolean, ByVal O2 As ClosableBoolean) As Boolean
        Return O1 And O2._Value
    End Operator

    Public Shared Operator Or(ByVal O1 As Boolean, ByVal O2 As ClosableBoolean) As Boolean
        Return O1 Or O2._Value
    End Operator

    Public Shared Operator Xor(ByVal O1 As Boolean, ByVal O2 As ClosableBoolean) As Boolean
        Return O1 Xor O2._Value
    End Operator

    Public Shared Operator IsTrue(ByVal O As ClosableBoolean) As Boolean
        Return O._Value
    End Operator

    Public Shared Operator IsFalse(ByVal O As ClosableBoolean) As Boolean
        Return Not O._Value
    End Operator

    Public Sub Dispose() Implements IDisposable.Dispose
        Me._Value = False
    End Sub

#Region "Value Property"
    Private _Value As Boolean

    Public ReadOnly Property Value As Boolean
        Get
            Return Me._Value
        End Get
    End Property
#End Region

End Structure
