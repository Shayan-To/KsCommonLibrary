Friend Class CsvParser

    Public Sub ParseCsv(ByVal Csv As CsvData, ByVal Str As String, Optional ByVal HasHeaders As Boolean = True, Optional ByVal Delimiter As Char = ","c, Optional ByVal NormalizeLineEndings As Boolean = True)
        Me.Str = Str.ToCharArray()
        Me.Index = 0
        Me.NormalizeLineEndings = True
        Me.DelimiterChar = Delimiter
        Me.Delimiter = Delimiter
        Me.Csv = Csv

        Me.ParseCsv(HasHeaders)
    End Sub

    Private Shared Sub EnsureColumns(ByVal Data As CsvData, ByVal I As Integer)
        Do Until I < Data.Columns.Count
            Data.Columns.Add()
        Loop
    End Sub

    Private Sub ParseCsv(ByVal HasHeaders As Boolean)
        Dim Res = Me.Csv
        Res.Clear()

        Res.HasHeaders = HasHeaders
        If HasHeaders Then
            Do
                Dim T = Me.ReadToken()
                Dim D = T
                Dim ShouldPeek = True

                ' If a delimiter happens right after another, then an empty field should be considered between them.
                If T Is NewLine Or T Is Nothing Or T Is Delimiter Then
                    D = ""
                    ShouldPeek = False
                End If

                Res.Columns.Add(D)

                If ShouldPeek Then
                    T = Me.PeekToken()
                End If

                If T Is NewLine Or T Is Nothing Or T Is Delimiter Then
                    If ShouldPeek Then
                        Me.ReadToken()
                    End If
                    If T Is Delimiter Then
                        Continue Do
                    Else
                        Exit Do
                    End If
                End If

                Throw New ArgumentException("Invalid CSV.")
            Loop
        End If

        Dim Entry As CsvEntry = Nothing
        Dim IsLastEntryEmpty As Boolean
        Dim I As Integer
        Dim NewEntry = True

        Do
            If NewEntry Then
                Entry = Res.Entries.Add()
                I = 0
                IsLastEntryEmpty = True
                NewEntry = False
            End If

            Dim T = Me.ReadToken()
            Dim D = T
            Dim ShouldPeek = True

            ' The last empty line. We have to remove the last entry from the data.
            If T Is Nothing And IsLastEntryEmpty Then
                Exit Do
            End If

            ' If a delimiter happens right after another, then an empty field should be considered between them.
            If T Is Nothing Or T Is NewLine Or T Is Delimiter Then
                D = ""
                ShouldPeek = False
            End If

            EnsureColumns(Res, I)
            Entry.Item(I) = D
            I += 1
            IsLastEntryEmpty = False

            If ShouldPeek Then
                T = Me.PeekToken()
            End If

            If T Is Nothing Then
                Exit Do
            End If
            If T Is NewLine Or T Is Delimiter Then
                If ShouldPeek Then
                    Me.ReadToken()
                End If
                If T Is NewLine Then
                    NewEntry = True
                End If
                Continue Do
            End If

            Throw New ArgumentException("Invalid CSV.")
        Loop

        If IsLastEntryEmpty Then
            Res.Entries.Remove(Res.Entries.Count - 1)
        End If
    End Sub

    Private Function ReadToken(ByVal T As String) As Boolean
        If Me.PeekToken() = T Then
            Me.ReadToken()
            Return True
        End If
        Return False
    End Function

    Private Function PeekToken() As String
        Dim I = Me.Index
        Dim R = Me.ReadToken()
        Me.Index = I
        Return R
    End Function

    ''' <summary>
    ''' Possible Values:
    ''' Nothing
    ''' NewLine
    ''' ","
    ''' Field
    ''' </summary>
    Private Function ReadToken() As String
        If Me.IsFinished() Then
            Return Nothing
        End If

        If Me.ReadChar(ControlChars.Cr) Then
            Me.ReadChar(ControlChars.Lf)
            Return NewLine
        End If

        If Me.ReadChar(ControlChars.Lf) Then
            Return NewLine
        End If

        If Me.ReadChar(Me.DelimiterChar) Then
            Return Me.Delimiter
        End If

        Dim Res = New Text.StringBuilder()

        If Me.ReadChar(""""c) Then
            Do
                If Me.IsFinished() Then
                    Throw New ArgumentException("Invalid CSV.")
                End If

                If Me.ReadChar(""""c) Then
                    If Not Me.ReadChar(""""c) Then
                        Exit Do
                    End If
                    Res.Append(""""c)
                    Continue Do
                End If

                If Me.NormalizeLineEndings AndAlso Me.ReadChar(ControlChars.Cr) Then
                    Me.ReadChar(ControlChars.Lf)
                    Res.Append(NewLine)
                    Continue Do
                End If
                If Me.NormalizeLineEndings AndAlso Me.ReadChar(ControlChars.Lf) Then
                    Res.Append(NewLine)
                    Continue Do
                End If

                Res.Append(Me.ReadChar())
            Loop
        Else
            Do
                Dim T = Me.PeekChar()
                If Not T.HasValue Or T = Me.DelimiterChar Or T = ControlChars.Cr Or T = ControlChars.Lf Then
                    Exit Do
                End If
                Res.Append(Me.ReadChar())
            Loop
        End If

        Return Res.ToString()
    End Function

    Private Function ReadChar(ByVal Ch As Char) As Boolean
        If Me.PeekChar() = Ch Then
            Me.ReadChar()
            Return True
        End If
        Return False
    End Function

    Private Function ReadChar() As Char?
        If Me.IsFinished() Then
            Return Nothing
        End If
        Dim R = Me.Str(Index)
        Index += 1
        Return R
    End Function

    Private Function PeekChar() As Char?
        If Me.IsFinished() Then
            Return Nothing
        End If
        Return Me.Str(Index)
    End Function

    Private Function IsFinished() As Boolean
        Return Me.Index = Me.Str.Length
    End Function

#Region "Instance Property"
    Private Shared ReadOnly _Instance As CsvParser = New CsvParser()

    Public Shared ReadOnly Property Instance As CsvParser
        Get
            Return _Instance
        End Get
    End Property
#End Region

    Private Shared NewLine As String = ControlChars.CrLf

    Private Delimiter As String
    Private DelimiterChar As Char
    Private NormalizeLineEndings As Boolean
    Private Str As Char()
    Private Index As Integer
    Private Csv As CsvData

End Class
