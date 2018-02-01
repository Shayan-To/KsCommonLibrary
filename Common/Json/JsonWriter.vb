Namespace Common

    Public Class JsonWriter
        Implements IDisposable

        Public Sub New(ByVal Output As IO.TextWriter, Optional ByVal LeaveOpen As Boolean = False)
            Me.Out = Output
            Me.LeaveOpen = LeaveOpen
            Me.Reset()
        End Sub

        Public Sub New(ByVal StringBuilder As Text.StringBuilder)
            Me.New(New IO.StringWriter(StringBuilder))
        End Sub

        Private Sub WriteEscaped(ByVal S As String)
            Dim PrevStart = 0
            Dim I = 0
            For I = 0 To S.Length - 1
                Dim Ch = S.Chars(I)
                Dim ECh = Ch
                If EscapeDic.TryGetValue(Ch, ECh) Then
                    Me.Out.Write(S.Substring(PrevStart, I - PrevStart))
                    Me.Out.Write("\"c)
                    Me.Out.Write(ECh)
                    PrevStart = I + 1
                ElseIf Char.IsControl(Ch) Then
                    Me.Out.Write(S.Substring(PrevStart, I - PrevStart))
                    Me.Out.Write("\u")
                    Me.Out.Write(Convert.ToString(Strings.AscW(Ch), 16).PadLeft(4, "0"c))
                    PrevStart = I + 1
                End If
            Next
            Me.Out.Write(S.Substring(PrevStart, I - PrevStart))
        End Sub

        Private Sub WriteNewLine()
            Me.Out.WriteLine()
            For I = 0 To Me.CurrentIndent - 1
                Me.Out.Write(Me.IndentString)
            Next
        End Sub

        Private Sub WriteSeparator(Optional ByVal NewLineRequired As Boolean = False)
            If Me.HasValueBefore Then
                Me.Out.Write(","c)
                If Not NewLineRequired And Not Me.MultiLine And Me.AddSpaces Then
                    Me.Out.Write(" "c)
                End If
            End If
            If Me.HasKeyBefore And Me.AddSpaces Then
                Me.Out.Write(" "c)
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
                Me.Out.Write(""""c)
                Me.WriteEscaped(Value)
                Me.Out.Write(""""c)
            Else
                Me.Out.Write(Value)
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

            Me.Out.Write(""""c)
            Me.WriteEscaped(Name)
            Me.Out.Write(""":")

            Me.HasValueBefore = False
            Me.HasKeyBefore = True
        End Sub

        Public Function OpenList(Optional ByVal MultiLine As Boolean = False) As Opening
            Verify.False(Me.State = WriterState.End, "Cannot write after write is finished.")
            Verify.True((Me.State = WriterState.Dictionary).Implies(Me.HasKeyBefore), $"Cannot write a value in place of a key in a dictionary. Use {NameOf(Me.WriteKey)} instead.")

            Dim R = New Opening(Me, "]"c, If(Me.State = WriterState.Begin, WriterState.End, Me.State), Me.MultiLine)

            Me.WriteSeparator(Me.OpeningBraceOnNewLine And MultiLine)
            Me.Out.Write("["c)

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
            Me.Out.Write("{"c)

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
            Verify.False(Me.State = WriterState.End, "Cannot write after write is finished.")

            If Me.MultiLine Then
                Me.CurrentIndent -= 1
                Me.WriteNewLine()
            End If

            Me.Out.Write(ClosingChar)

            Me.MultiLine = PreviousMultiline
            Me.State = PreviousState
            Me.HasValueBefore = True
        End Sub

        Private Sub Reset()
            Me.State = WriterState.Begin
            Me.HasValueBefore = False
            Me.HasKeyBefore = False
            Me.MultiLine = False
            Me.CurrentIndent = 0
        End Sub

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

        Private ReadOnly Out As IO.TextWriter
        Private ReadOnly LeaveOpen As Boolean

        Private HasValueBefore As Boolean
        Private HasKeyBefore As Boolean
        Private MultiLine As Boolean
        Private CurrentIndent As Integer
        Private State As WriterState = WriterState.Begin

#Region "IDisposable Support"
        Protected Overridable Sub Dispose(Disposing As Boolean)
            If Not Me.IsDisposed Then
                If Disposing Then
                    If Not Me.LeaveOpen Then
                        Me.Out.Dispose()
                    End If
                End If
            End If
            Me._IsDisposed = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Me.Dispose(True)
        End Sub

#Region "IsDisposed Read-Only Property"
        Private _IsDisposed As Boolean

        <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Advanced)>
        Public ReadOnly Property IsDisposed As Boolean
            Get
                Return Me._IsDisposed
            End Get
        End Property
#End Region
#End Region

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
