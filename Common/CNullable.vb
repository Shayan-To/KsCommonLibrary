Public Structure CNullable(Of T)

    Public Sub New(ByVal Value As T)
        Me._Value = Value
        Me._HasValue = True
    End Sub

    Public Shared Widening Operator CType(ByVal O As T) As CNullable(Of T)
        Return New CNullable(Of T)(O)
    End Operator

    Public Shared Narrowing Operator CType(ByVal O As CNullable(Of T)) As T
        Return O.Value
    End Operator

#Region "Value Property"
    Private ReadOnly _Value As T

    Public ReadOnly Property Value As T
        Get
            If Not Me.HasValue Then
                Throw New InvalidOperationException("There is no value to get.")
            End If
            Return Me._Value
        End Get
    End Property
#End Region

#Region "HasValue Property"
    Private ReadOnly _HasValue As Boolean

    Public ReadOnly Property HasValue As Boolean
        Get
            Return Me._HasValue
        End Get
    End Property
#End Region

End Structure
