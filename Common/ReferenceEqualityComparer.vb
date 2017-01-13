Namespace Common

    Public Class ReferenceEqualityComparer(Of T As Class)
        Implements IEqualityComparer(Of T)

        Public Overloads Function Equals(x As T, y As T) As Boolean Implements IEqualityComparer(Of T).Equals
            Return x Is y
        End Function

        Public Overloads Function GetHashCode(obj As T) As Integer Implements IEqualityComparer(Of T).GetHashCode
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
