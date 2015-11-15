Public Class Utf8Encoding

    Public Shared Function Encode(ByVal CharsArray As Char(), ByVal CharsIndex As Integer, ByVal CharsLenght As Integer,
                                  ByVal BytesArray As Byte(), ByVal BytesIndex As Integer, ByVal BytesLength As Integer,
                                  ByVal BeginningOfFile As Boolean,
                                  ByRef CharsRead As Integer, ByRef BytesWritten As Integer) As Boolean
        CharsLenght += CharsIndex
        BytesLength += BytesIndex

        Dim CharsInitialIndex = CharsIndex
        Dim BytesInitialIndex = BytesIndex
        Dim T = New Byte(5) {}
        If BeginningOfFile Then
            CharsIndex -= 1
        End If
        For CharsIndex = CharsIndex To CharsLenght - 1
            If BytesIndex = BytesLength Then
                Exit For
            End If

            Dim Ch As Integer
            If BeginningOfFile And CharsIndex = CharsInitialIndex - 1 Then
                Ch = &HFEFF
            Else
                Ch = Strings.AscW(CharsArray(CharsIndex))
            End If

            If Ch < 128 Then
                BytesArray(BytesIndex) = CByte(Ch)
                BytesIndex += 1
                Continue For
            End If

            Dim I = 0

            Do
                T(I) = CByte((2 << 6) Or (Ch And ((1 << 6) - 1)))
                Ch >>= 6
                I += 1
                ' If we are ready, we will have I + 1 bytes for the character.
                ' So, there will be I + 2 bits occupied by the preamble of the first byte.
                ' And 8 - I - 2 = 6 - I bits will be remaining.
            Loop Until Ch < (1 << (6 - I))

            ' We are having 6 - I bits remaining.
            ' So we have to make 7 - I zeros at the end of the byte.
            T(I) = CByte((255 Xor ((1 << (7 - I)) - 1)) Or Ch)
            I += 1

            If BytesIndex + I >= BytesLength Then
                Exit For
            End If

            For I = I - 1 To 0 Step -1
                BytesArray(BytesIndex) = T(I)
                BytesIndex += 1
            Next
        Next
        ' 12345678
        ' 76543210

        CharsRead = CharsIndex - CharsInitialIndex
        BytesWritten = BytesIndex - BytesInitialIndex
        Return True
    End Function

    Public Shared Function Decode(ByVal BytesArray As Byte(), ByVal BytesIndex As Integer, ByVal BytesLength As Integer,
                                  ByVal CharsArray As Char(), ByVal CharsIndex As Integer, ByVal CharsLength As Integer,
                                  ByVal BeginningOfFile As Boolean,
                                  ByRef BytesRead As Integer, ByRef CharsWritten As Integer) As Boolean
        CharsLength += CharsIndex
        BytesLength += BytesIndex

        Dim CharsInitialIndex = CharsIndex
        Dim BytesInitialIndex = BytesIndex

        For BytesIndex = BytesIndex To BytesLength - 1
            If CharsIndex = CharsLength Then
                Exit For
            End If

            Dim B = BytesArray(BytesIndex)

            If (B >> 7) = 0 Then
                CharsArray(CharsIndex) = Strings.ChrW(B)
                CharsIndex += 1
                Continue For
            End If

            If ((B >> 6) And 1) = 0 Then
                Return False
            End If

            Dim I = 5
            Do Until ((B >> I) And 1) = 0
                I -= 1
                If I = 0 Then
                    Return False
                End If
            Loop

            I = 7 - I

            If BytesIndex + I >= BytesLength Then
                Exit For
            End If

            Dim Ch As Integer = (B << (I + 1)) >> (I + 1)

            For J As Integer = 1 To I - 1
                BytesIndex += 1
                B = BytesArray(BytesIndex)
                If (B >> 6) <> 2 Then
                    Return False
                End If
                Ch = (Ch << 6) Or ((B << 2) >> 2)
            Next

            ' Exclude Byte Order Mark (BOM).
            If Not (BeginningOfFile And CharsIndex = CharsInitialIndex And Ch = &HFEFF) Then
                CharsArray(CharsIndex) = Strings.ChrW(Ch)
                CharsIndex += 1
            End If
        Next
        ' 12345678
        ' 76543210

        BytesRead = BytesIndex - BytesInitialIndex
        CharsWritten = CharsIndex - CharsInitialIndex
        Return True
    End Function

End Class
