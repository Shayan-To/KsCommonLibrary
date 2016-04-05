Public Class OneToOneOrderedFreezableDictionary(Of TKey, TValue)
    Inherits OneToOneOrderedDictionary(Of TKey, TValue)

    Public Sub New(ByVal KeySelector As Func(Of TValue, TKey))
        MyBase.New(KeySelector)
    End Sub

#Region "Freezable"
    Public Sub Freeze()
        If Me.FreezeCalled Then
            Exit Sub
        End If
        Me.FreezeCalled = True
        Me.OnFreezing()
        Me._IsFreezed = True
    End Sub

    Protected Overridable Sub OnFreezing()

    End Sub

    Protected Sub VerifyWrite()
        Verify.False(Me._IsFreezed, "Cannot change a freezed object.")
    End Sub

#Region "IsFreezed Property"
    Private _IsFreezed As Boolean

    Public ReadOnly Property IsFreezed As Boolean
        Get
            Return Me._IsFreezed
        End Get
    End Property
#End Region

    Private FreezeCalled As Boolean
#End Region

    Public Overrides Sub Add(Value As TValue)
        Me.VerifyWrite()
        MyBase.Add(Value)
    End Sub

    Public Overrides Function [Set](Value As TValue) As Boolean
        Me.VerifyWrite()
        Return MyBase.Set(Value)
    End Function

    Public Overrides Sub Clear()
        Me.VerifyWrite()
        MyBase.Clear()
    End Sub

    Public Overrides Sub Insert(Index As Integer, Value As TValue)
        Me.VerifyWrite()
        MyBase.Insert(Index, Value)
    End Sub

    Public Overrides Sub RemoveAt(index As Integer)
        Me.VerifyWrite()
        MyBase.RemoveAt(index)
    End Sub

    Public Overrides Function RemoveKey(key As TKey) As Boolean
        Me.VerifyWrite()
        Return MyBase.RemoveKey(key)
    End Function

End Class
