Public Class HashCodeComparer(Of T)
    Implements IComparer, IComparer(Of T), IEqualityComparer, IEqualityComparer(Of T)

    Public Function Compare(x As T, y As T) As Integer Implements IComparer(Of T).Compare
        Return x.GetHashCode().CompareTo(y.GetHashCode())
    End Function

    Public Overloads Function GetHashCode(obj As T) As Integer Implements IEqualityComparer(Of T).GetHashCode
        Return obj.GetHashCode()
    End Function

    Public Overloads Function Equals(x As T, y As T) As Boolean Implements IEqualityComparer(Of T).Equals
        Return x.GetHashCode() = y.GetHashCode()
    End Function

    Private Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
        Return Me.Compare(DirectCast(x, T), DirectCast(y, T))
    End Function

    Private Overloads Function GetHashCode(obj As Object) As Integer Implements IEqualityComparer.GetHashCode
        Return Me.GetHashCode(DirectCast(obj, T))
    End Function

    Private Overloads Function Equals(x As Object, y As Object) As Boolean Implements IEqualityComparer.Equals
        Dim TX = DirectCast(x, T)
        Dim TY = DirectCast(y, T)

        If TX Is Nothing Or TY Is Nothing Then
            Return x Is Nothing And y Is Nothing
        End If

        Return Me.Equals(TX, TY)
    End Function

End Class
