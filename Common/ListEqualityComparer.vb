Namespace Common

    Public Class ListEqualityComparer(Of T)
        Inherits EqualityComparer(Of IList(Of T))

        Public Sub New(ByVal EqualityComparer As IEqualityComparer(Of T))
            Me.EqualityComparer = EqualityComparer
        End Sub

        Public Sub New()
            Me.New(Generic.EqualityComparer(Of T).Default)
        End Sub

        Public Overrides Function Equals(x As IList(Of T), y As IList(Of T)) As Boolean
            If x.Count <> y.Count Then
                Return False
            End If
            For I As Integer = 0 To x.Count - 1
                If Me.EqualityComparer.Equals(x.Item(I), y.Item(I)) Then
                    Return False
                End If
            Next
            Return True
        End Function

        Public Overrides Function GetHashCode(obj As IList(Of T)) As Integer
            Dim Hash = 0
            For I As Integer = 0 To obj.Count - 1
                Hash = Utilities.CombineHashCodes(Hash, I, Me.EqualityComparer.GetHashCode(obj.Item(I)))
            Next
            Return Hash
        End Function

        Private ReadOnly EqualityComparer As IEqualityComparer(Of T)

    End Class

End Namespace
