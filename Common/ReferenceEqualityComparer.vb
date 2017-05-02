Namespace Common

    Public Class ReferenceEqualityComparer(Of T As Class)
        Inherits EqualityComparer(Of T)

        Public Overrides Function Equals(x As T, y As T) As Boolean
            Return x Is y
        End Function

        Public Overrides Function GetHashCode(obj As T) As Integer
            Return Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj)
        End Function

#Region "Instance Property"
        Private Shared ReadOnly _Instance As ReferenceEqualityComparer(Of T) = New ReferenceEqualityComparer(Of T)()

        Public Shared ReadOnly Property Instance As ReferenceEqualityComparer(Of T)
            Get
                Return _Instance
            End Get
        End Property
#End Region

    End Class

End Namespace
