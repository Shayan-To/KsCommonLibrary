Public Class CsvData

    Public Shared Function Parse(ByVal Str As String, Optional ByVal HasHeaders As Boolean = True, Optional ByVal Delimiter As Char = ","c, Optional ByVal NormalizeLineEndings As Boolean = True) As CsvData
        Dim Res = New CsvData()
        CsvParser.Instance.ParseCsv(Res, Str, HasHeaders, Delimiter, NormalizeLineEndings)
        Return Res
    End Function

    Public Sub ParseIn(ByVal Str As String, Optional ByVal HasHeaders As Boolean = True, Optional ByVal Delimiter As Char = ","c, Optional ByVal NormalizeLineEndings As Boolean = True)
        CsvParser.Instance.ParseCsv(Me, Str, HasHeaders, Delimiter, NormalizeLineEndings)
    End Sub

    Public Overrides Function ToString() As String
        Return Me.ToString(True)
    End Function

    Private Shared Sub WriteField(ByVal Field As String, ByVal UseQuotes As Boolean, ByVal Out As Text.StringBuilder)
        If Field.StartsWith("""") Or Field.Contains(ControlChars.Cr) Or Field.Contains(ControlChars.Lf) Then
            UseQuotes = True
        End If

        If Not UseQuotes Then
            Out.Append(Field)
        End If

        Out.Append(""""c)

        For Each C In Field
            If C = """"c Then
                Out.Append("""""")
            Else
                Out.Append(C)
            End If
        Next

        Out.Append(""""c)
    End Sub

    Public Overloads Function ToString(Optional ByVal UseQuotes As Boolean = True, Optional ByVal Delimiter As Char = ","c) As String
        Dim Res = New Text.StringBuilder()

        If Me.HasHeaders Then
            Dim Bl = True
            For Each C In Me.Columns
                If Bl Then
                    Bl = False
                Else
                    Res.Append(","c)
                End If
                WriteField(C.HeaderName, UseQuotes, Res)
            Next
            Res.AppendLine()
        End If

        Dim ColsCount = Me.Columns.Count

        For Each E In Me.Entries
            Dim Bl = True
            For I As Integer = 0 To ColsCount - 1
                If Bl Then
                    Bl = False
                Else
                    Res.Append(","c)
                End If
                WriteField(E.Item(I), UseQuotes, Res)
            Next
            Res.AppendLine()
        Next

        Return Res.ToString()
    End Function

    Public Sub Clear()
        Me.Entries.Clear()
        Me.Columns.Clear()
    End Sub

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
