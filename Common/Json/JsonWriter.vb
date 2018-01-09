Namespace Common

    Public Class JsonWriter

        Private Sub WriteEscaped(ByVal S As String)
            Dim PrevStart = 0
            Dim I = 0
            For I = 0 To S.Length - 1
                Dim Ch = S.Chars(I)
                Dim ECh = Ch
                If EscapeDic.TryGetValue(Ch, ECh) Then
                    Me.Builder.Append(S, PrevStart, I - PrevStart).Append("\"c).Append(ECh)
                    PrevStart = I + 1
                ElseIf Char.IsControl(Ch) Then
                    Me.Builder.Append(S, PrevStart, I - PrevStart).Append("\u").Append(Convert.ToString(Strings.AscW(Ch), 16).PadLeft(4, "0"c))
                    PrevStart = I + 1
                End If
            Next

            Me.Builder.Append(S, PrevStart, I - PrevStart)
        End Sub

        Private Sub WriteNewLine()
            Me.Builder.AppendLine()
            For I = 0 To Me.CurrentIndent - 1
                Me.Builder.Append(Me.IndentString)
            Next
        End Sub

        Private Sub WriteSeparator(Optional ByVal NewLineRequired As Boolean = False)
            If Me.HasValueBefore Then
                Me.Builder.Append(","c)
                If Not NewLineRequired And Not Me.MultiLine And Me.AddSpaces Then
                    Me.Builder.Append(" "c)
                End If
            End If
            If Me.HasKeyBefore And Me.AddSpaces Then
                Me.Builder.Append(" "c)
            End If

            If (Me.MultiLine And Not Me.HasKeyBefore) Or NewLineRequired Then
                If Me.State <> WriterState.Begin Then
                    Me.WriteNewLine()
                End If
            End If
        End Sub

        Public Sub WriteValue(ByVal Value As String, ByVal Quoted As Boolean)
            Verify.False(Me.State = WriterState.End, "Cannot write after write is finished.")
            Verify.True((Me.State = WriterState.Dictionary).Implies(Me.HasKeyBefore), $"Cannot write a value in place of a key in a dictionary. Use {NameOf(Me.WriteKey)} instead.")

            Me.WriteSeparator()

            If Quoted Then
                Me.Builder.Append(""""c)
                Me.WriteEscaped(Value)
                Me.Builder.Append(""""c)
            Else
                Me.Builder.Append(Value)
            End If

            If Me.State = WriterState.Begin Then
                Me.State = WriterState.End
            End If
            Me.HasKeyBefore = False
            Me.HasValueBefore = True
        End Sub

        Public Sub WriteKey(ByVal Name As String)
            Verify.False(Me.State = WriterState.End, "Cannot write after write is finished.")
            Verify.True(Me.State = WriterState.Dictionary, "Cannot write a key outside a dictionary.")
            Verify.False(Me.HasKeyBefore, "Cannot write a key immediately after another.")

            Me.WriteSeparator()

            Me.Builder.Append(""""c)
            Me.WriteEscaped(Name)
            Me.Builder.Append(""":")

            Me.HasValueBefore = False
            Me.HasKeyBefore = True
        End Sub

        Public Function OpenList(Optional ByVal MultiLine As Boolean = False) As Opening
            Verify.False(Me.State = WriterState.End, "Cannot write after write is finished.")
            Verify.True((Me.State = WriterState.Dictionary).Implies(Me.HasKeyBefore), $"Cannot write a value in place of a key in a dictionary. Use {NameOf(Me.WriteKey)} instead.")

            Dim R = New Opening(Me, "]"c, If(Me.State = WriterState.Begin, WriterState.End, Me.State), Me.MultiLine)

            Me.WriteSeparator(Me.OpeningBraceOnNewLine And MultiLine)
            Me.Builder.Append("["c)

            Me.MultiLine = MultiLine
            Me.HasValueBefore = False
            Me.HasKeyBefore = False
            Me.State = WriterState.List

            If MultiLine Then
                Me.CurrentIndent += 1
            End If

            Return R
        End Function

        Public Function OpenDictionary(Optional ByVal MultiLine As Boolean = False) As Opening
            Verify.False(Me.State = WriterState.End, "Cannot write after write is finished.")
            Verify.True((Me.State = WriterState.Dictionary).Implies(Me.HasKeyBefore), $"Cannot write a value in place of a key in a dictionary. Use {NameOf(Me.WriteKey)} instead.")

            Dim R = New Opening(Me, "}"c, If(Me.State = WriterState.Begin, WriterState.End, Me.State), Me.MultiLine)

            Me.WriteSeparator(Me.OpeningBraceOnNewLine And MultiLine)
            Me.Builder.Append("{"c)

            Me.MultiLine = MultiLine
            Me.HasValueBefore = False
            Me.HasKeyBefore = False
            Me.State = WriterState.Dictionary

            If MultiLine Then
                Me.CurrentIndent += 1
            End If

            Return R
        End Function

        Private Sub CloseOpening(ByVal ClosingChar As Char, ByVal PreviousState As WriterState, ByVal PreviousMultiline As Boolean)
            Verify.False(Me.HasKeyBefore, "Cannot close while a key is pending its value.")

            If Me.MultiLine Then
                Me.CurrentIndent -= 1
                Me.WriteNewLine()
            End If

            Me.Builder.Append(ClosingChar)

            Me.MultiLine = PreviousMultiline
            Me.State = PreviousState
            Me.HasValueBefore = True
        End Sub

        Public Sub Reset()
            Me.Builder.Clear()
            Me.State = WriterState.Begin
            Me.HasValueBefore = False
            Me.HasKeyBefore = False
            Me.MultiLine = False
            Me.CurrentIndent = 0
        End Sub

        Public Overrides Function ToString() As String
            Verify.True(Me.State = WriterState.End, "Write must be finished before JSON string can be got.")
            Return Me.Builder.ToString()
        End Function

        Private Shared ReadOnly EscapeDic As Dictionary(Of Char, Char) =
                (Function() New Dictionary(Of Char, Char) From {
                         {""""c, """"c},
                         {"/"c, "/"c},
                         {"\"c, "\"c},
                         {Strings.ChrW(&H8), "b"c},
                         {Strings.ChrW(&HC), "f"c},
                         {Strings.ChrW(&HA), "n"c},
                         {Strings.ChrW(&HD), "r"c},
                         {Strings.ChrW(&H9), "t"c}
                     }).Invoke()

        Private ReadOnly Builder As Text.StringBuilder = New Text.StringBuilder()
        Private HasValueBefore As Boolean
        Private HasKeyBefore As Boolean
        Private MultiLine As Boolean
        Private CurrentIndent As Integer
        Private State As WriterState = WriterState.Begin

#Region "IndentString Property"
        Private _IndentString As String = "  "

        Public Property IndentString As String
            Get
                Return Me._IndentString
            End Get
            Set(ByVal Value As String)
                Me._IndentString = Value
            End Set
        End Property
#End Region

#Region "OpeningBraceOnNewLine Property"
        Private _OpeningBraceOnNewLine As Boolean = False

        Public Property OpeningBraceOnNewLine As Boolean
            Get
                Return Me._OpeningBraceOnNewLine
            End Get
            Set(ByVal Value As Boolean)
                Me._OpeningBraceOnNewLine = Value
            End Set
        End Property
#End Region

#Region "AddSpaces Property"
        Private _AddSpaces As Boolean = True

        Public Property AddSpaces As Boolean
            Get
                Return Me._AddSpaces
            End Get
            Set(ByVal Value As Boolean)
                Me._AddSpaces = Value
            End Set
        End Property
#End Region

        Public Structure Opening
            Implements IDisposable

            Friend Sub New(ByVal Writer As JsonWriter, ByVal ClosingChar As Char, ByVal PreviousState As WriterState, ByVal PreviousMultiline As Boolean)
                Me.Writer = Writer
                Me.ClosingChar = ClosingChar
                Me.PreviousState = PreviousState
                Me.PreviousMultiline = PreviousMultiline
            End Sub

            Private Sub Dispose() Implements IDisposable.Dispose
                Me.Writer.CloseOpening(Me.ClosingChar, Me.PreviousState, Me.PreviousMultiline)
            End Sub

            Public Sub Close()
                Me.Dispose()
            End Sub

            Private ReadOnly ClosingChar As Char
            Private ReadOnly PreviousState As WriterState
            Private ReadOnly PreviousMultiline As Boolean
            Private ReadOnly Writer As JsonWriter

        End Structure

        Friend Enum WriterState

            Begin
            Dictionary
            List
            [End]

        End Enum

    End Class

End Namespace
