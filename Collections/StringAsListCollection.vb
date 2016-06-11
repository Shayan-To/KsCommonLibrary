Public Class StringAsListCollection
    Inherits BaseReadOnlyList(Of Char)

    Public Sub New(ByVal Str As String)
        Me.Base = Str
    End Sub

    Public Overrides ReadOnly Property Count As Integer
        Get
            Return Me.Base.Length
        End Get
    End Property

    Default Public Overrides ReadOnly Property Item(Index As Integer) As Char
        Get
            Return Me.Base.Chars(Index)
        End Get
    End Property

    Public Overrides Function GetEnumerator() As IEnumerator(Of Char)
        Return Me.Base.GetEnumerator()
    End Function

    Private ReadOnly Base As String

End Class
