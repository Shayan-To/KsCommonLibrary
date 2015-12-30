Public Class CsvColumn

    Friend Sub New(ByVal Parent As CsvData)
        Me._Parent = Parent
        Me.Parent.Columns.ReportHeaderNameChanged(Me, Nothing, "")
    End Sub

    Friend Sub Detach()
        Me.IsRemoved = True
        Me.Parent.Columns.ReportHeaderNameChanged(Me, Me._HeaderName, Nothing)
    End Sub

#Region "HeaderName Property"
    Friend _HeaderName As String = ""

    Public Property HeaderName As String
        Get
            Verify.False(Me.IsRemoved, "Header is detached.")
            Verify.True(Me.Parent.HasHeaders, "The CSV does not have headers.")
            Return Me._HeaderName
        End Get
        Set(ByVal Value As String)
            Verify.False(Me.IsRemoved, "Header is detached.")
            Verify.True(Me.Parent.HasHeaders, "The CSV does not have headers.")
            If Value Is Nothing Then
                Value = ""
            End If
            If Me._HeaderName <> Value Then
                Dim OldValue = Me._HeaderName
                Me._HeaderName = Value
                Me.Parent.Columns.ReportHeaderNameChanged(Me, OldValue, Value)
            End If
        End Set
    End Property
#End Region

#Region "Index Property"
    Private _Index As Integer

    Public Property Index As Integer
        Get
            Verify.False(Me.IsRemoved, "Header is detached.")
            Return Me._Index
        End Get
        Friend Set(ByVal Value As Integer)
            Me._Index = Value
        End Set
    End Property
#End Region

#Region "Parent Property"
    Private ReadOnly _Parent As CsvData

    Public ReadOnly Property Parent As CsvData
        Get
            Verify.False(Me.IsRemoved, "Header is detached.")
            Return Me._Parent
        End Get
    End Property
#End Region

    Private IsRemoved As Boolean = False

End Class
