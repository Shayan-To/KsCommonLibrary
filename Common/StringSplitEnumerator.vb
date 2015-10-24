Public Class StringSplitEnumerator
    Implements IEnumerator(Of String)

    Public Sub New(ByVal Str As String, ByVal Options As StringSplitOptions, ParamArray ByVal Chars As Char())
        Me._String = Str
        Me._Chars = DirectCast(Chars.Clone(), Char())
        Array.Sort(Me._Chars)
        Me._Options = Options
        Me.Reset()
    End Sub

    Public Function GetEnumerator() As StringSplitEnumerator
        Return Me
    End Function

    Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
        If Me.Index = Me._String.Length Then
            Return False
        End If

        Dim Start = Me.Index
        Me.SkipText()
        Me._Current = Me._String.Substring(Start, Me.Index - Start)
        Me.SkipDelimiter()

        Return True
    End Function

    Public Sub Reset() Implements IEnumerator.Reset
        Me.Index = 0
        Me.GiveRest = False
        If Me._Options = StringSplitOptions.RemoveEmptyEntries Then
            Me.SkipDelimiter()
        End If
    End Sub

    Public Sub UnifyRest()
        Me.GiveRest = True
    End Sub

    Private Sub SkipDelimiter()
        If Me._Options = StringSplitOptions.RemoveEmptyEntries Then
            Do While Me.Index <> Me._String.Length AndAlso
                     Array.BinarySearch(Me._Chars, Me._String.Chars(Me.Index)) <> -1
                Me.Index += 1
            Loop
        End If
    End Sub

    Private Sub SkipText()
        If Me.GiveRest Then
            Me.Index = Me._String.Length
        End If
        Do While Me.Index <> Me._String.Length AndAlso
                 Array.BinarySearch(Me._Chars, Me._String.Chars(Me.Index)) = -1
            Me.Index += 1
        Loop
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose

    End Sub

#Region "Current Property"
    Private _Current As String

    Public ReadOnly Property Current As String Implements IEnumerator(Of String).Current
        Get
            Return Me._Current
        End Get
    End Property

    Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
        Get
            Return Me._Current
        End Get
    End Property
#End Region

#Region "String Property"
    Private ReadOnly _String As String

    Public ReadOnly Property [String] As String
        Get
            Return Me._String
        End Get
    End Property
#End Region

#Region "Chars Property"
    Private ReadOnly _Chars As Char()

    Public ReadOnly Property Chars(ByVal Index As Integer) As Char
        Get
            Return Me._Chars(Index)
        End Get
    End Property
#End Region

#Region "Options Property"
    Private ReadOnly _Options As StringSplitOptions

    Public ReadOnly Property Options As StringSplitOptions
        Get
            Return Me._Options
        End Get
    End Property
#End Region

    Private Index As Integer
    Private GiveRest As Boolean

End Class
