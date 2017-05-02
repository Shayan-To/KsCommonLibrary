Namespace Common

    Public Class DelegateEqualityComparer(Of T)
        Inherits EqualityComparer(Of T)

        Public Sub New(ByVal EqualsDelegate As Func(Of T, T, Boolean), ByVal GetHashCodeDelegate As Func(Of T, Integer))
            Me.EqualsDelegate = EqualsDelegate
            Me.GetHashCodeDelegate = GetHashCodeDelegate
        End Sub

        Public Overrides Function Equals(x As T, y As T) As Boolean
            Return Me.EqualsDelegate.Invoke(x, y)
        End Function

        Public Overrides Function GetHashCode(obj As T) As Integer
            Return Me.GetHashCodeDelegate.Invoke(obj)
        End Function

        Private ReadOnly EqualsDelegate As Func(Of T, T, Boolean)
        Private ReadOnly GetHashCodeDelegate As Func(Of T, Integer)

    End Class

End Namespace
