Public Class CsvData

    Public Shared Function ParseCSV(ByVal Str As String, Optional ByVal HasHeaders As Boolean = True, Optional ByVal Delimiter As Char = ","c, Optional ByVal NormalizeLineEndings As Boolean = True) As CsvData
        Return CsvParser.Instance.ParseCSV(Str, HasHeaders, Delimiter, NormalizeLineEndings)
    End Function

#Region "HasHeaders Property"
    Private _HasHeaders As Boolean = True

    Public Property HasHeaders As Boolean
        Get
            Return Me._HasHeaders
        End Get
        Set(ByVal Value As Boolean)
            If Me._HasHeaders <> Value Then
                Me._HasHeaders = Value
                Me.Columns.ReportHasHeadersChanged(Value)
            End If
        End Set
    End Property
#End Region

#Region "Columns Property"
    Private ReadOnly _Columns As CsvColumnList = New CsvColumnList(Me)

    Public ReadOnly Property Columns As CsvColumnList
        Get
            Return Me._Columns
        End Get
    End Property
#End Region

#Region "Entries Property"
    Private ReadOnly _Entries As CsvEntryList = New CsvEntryList(Me)

    Public ReadOnly Property Entries As CsvEntryList
        Get
            Return Me._Entries
        End Get
    End Property
#End Region

End Class
