Public Class CsvEntry

    Friend Sub New(ByVal Parent As CsvData)
        Me._Parent = Parent
    End Sub

    Friend Sub Detach()
        Me.IsRemoved = True
    End Sub

    Default Public Property Item(ByVal Index As Integer) As String
        Get
            Verify.False(Me.IsRemoved, "Entry is detached.")
            Verify.TrueArg(Index < Me.Parent.Columns.Count, "Index was outside range.")
            If Index >= Me.Data.Count Then
                Return ""
            End If
            Return Me.Data(Index)
        End Get
        Set(ByVal Value As String)
            Verify.False(Me.IsRemoved, "Entry is detached.")
            Verify.TrueArg(Index < Me.Parent.Columns.Count, "Index was outside range.")
            If Value Is Nothing Then
                Value = ""
            End If
            If Value.Length = 0 And Index >= Me.Data.Count Then
                Exit Property
            End If
            Do Until Index < Me.Data.Count
                Me.Data.Add("")
            Loop
            Me.Data(Index) = Value
        End Set
    End Property

    Default Public Property Item(ByVal HeaderName As String) As String
        Get
            Verify.False(Me.IsRemoved, "Entry is detached.")
            Return Me.Item(Me.Parent.Columns.Item(HeaderName).Index)
        End Get
        Set(ByVal Value As String)
            Verify.False(Me.IsRemoved, "Entry is detached.")
            Me.Item(Me.Parent.Columns.Item(HeaderName).Index) = Value
        End Set
    End Property

    Default Public Property Item(ByVal Column As CsvColumn) As String
        Get
            Verify.False(Me.IsRemoved, "Entry is detached.")
            Verify.TrueArg(Column.Parent Is Me.Parent, "Column", "Given column is not part of this csv data.")
            Return Me.Item(Column.Index)
        End Get
        Set(ByVal Value As String)
            Verify.False(Me.IsRemoved, "Entry is detached.")
            Verify.TrueArg(Column.Parent Is Me.Parent, "Column", "Given column is not part of this csv data.")
            Me.Item(Column.Index) = Value
        End Set
    End Property

#Region "Parent Property"
    Private ReadOnly _Parent As CsvData

    Public ReadOnly Property Parent As CsvData
        Get
            Verify.False(Me.IsRemoved, "Entry is detached.")
            Return Me._Parent
        End Get
    End Property
#End Region

    Friend ReadOnly Data As List(Of String) = New List(Of String)()
    Private IsRemoved As Boolean = False

End Class
