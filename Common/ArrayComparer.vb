Namespace Common

    Public Class ArrayComparer(Of T)
        Implements IComparer(Of T()),
                   IEqualityComparer(Of T())

        Public Sub New()
            Me.New(Generic.Comparer(Of T).Default, Generic.EqualityComparer(Of T).Default)
        End Sub

        Public Sub New(ByVal Comparer As IComparer(Of T))
            Me.New(Comparer, Generic.EqualityComparer(Of T).Default)
        End Sub

        Public Sub New(ByVal Comparer As IComparer(Of T), ByVal EqualityComparer As IEqualityComparer(Of T))
            Me.Comparer = Comparer
            Me.EqualityComparer = EqualityComparer
        End Sub

        Public Function Compare(x As T(), y As T()) As Integer Implements IComparer(Of T()).Compare
            For I As Integer = 0 To Math.Min(x.Length, y.Length) - 1
                Dim T = Me.Comparer.Compare(x(I), y(I))
                If T <> 0 Then
                    Return T
                End If
            Next
            Return x.Length - y.Length
        End Function

        Public Overloads Function Equals(x As T(), y As T()) As Boolean Implements IEqualityComparer(Of T()).Equals
            For I As Integer = 0 To Math.Min(x.Length, y.Length) - 1
                If Not Me.EqualityComparer.Equals(x(I), y(I)) Then
                    Return False
                End If
            Next
            Return x.Length = y.Length
        End Function

        Public Overloads Function GetHashCode(obj As T()) As Integer Implements IEqualityComparer(Of T()).GetHashCode
            Dim R = &HFAB43DC8
            For I As Integer = 0 To obj.Length - 1
                R = Utilities.CombineHashCodes(R, Me.EqualityComparer.GetHashCode(obj(I)))
            Next
            Return R
        End Function

        Private ReadOnly Comparer As IComparer(Of T)
        Private ReadOnly EqualityComparer As IEqualityComparer(Of T)

    End Class

End Namespace
