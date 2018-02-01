Namespace Common

    Public Class XmlWriter
        Implements IDisposable

        Public Sub New(ByVal Output As IO.TextWriter, Optional ByVal LeaveOpen As Boolean = False)
            Me.Out = Output
            Me.LeaveOpen = LeaveOpen
            Me.Reset()
        End Sub

        Public Sub New(ByVal StringBuilder As Text.StringBuilder)
            Me.New(New IO.StringWriter(StringBuilder))
        End Sub

        Private Function NeedsEscaping(ByVal S As String) As Boolean
            For I = 0 To S.Length - 1
                Dim Ch = S.Chars(I)
                If EscapeDic.ContainsKey(Ch) Or Char.IsControl(Ch) Or (Char.IsWhiteSpace(Ch) And Ch <> " "c) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Private Sub WriteEscaped(ByVal S As String)
            Dim PrevStart = 0
            Dim I = 0
            For I = 0 To S.Length - 1
                Dim Ch = S.Chars(I)
                Dim Esc As String = Nothing
                If EscapeDic.TryGetValue(Ch, Esc) Then
                    Me.Out.Write(S.Substring(PrevStart, I - PrevStart))
                    Me.Out.Write(Esc)
                    PrevStart = I + 1
                ElseIf Char.IsControl(Ch) Or (Char.IsWhiteSpace(Ch) And Ch <> " "c) Then
                    Me.Out.Write(S.Substring(PrevStart, I - PrevStart))
                    Me.Out.Write("&#x")
                    Me.Out.Write(Convert.ToString(Strings.AscW(Ch), 16))
                    Me.Out.Write(";")
                    PrevStart = I + 1
                End If
            Next
            Me.Out.Write(S.Substring(PrevStart, I - PrevStart))
        End Sub

        Private Sub WriteNewLine(ByVal IsAttribute As Boolean, Optional ByVal AfterAttributes As Boolean = False)
            Assert.True(AfterAttributes.Implies(IsAttribute))

            Me.Out.WriteLine()
            For I = 0 To Me.CurrentIndent - 2
                Me.Out.Write(Me.IndentString)
            Next

            If IsAttribute Then
                If Not AfterAttributes Then
                    If Me.AttributeDynamicIndent Then
                        Me.Out.Write(New String(" "c, Me.AttributeIndent))
                    Else
                        Me.Out.Write(Me.IndentString)
                    End If
                End If
            Else
                If Me.CurrentIndent <> 0 Then
                    Me.Out.Write(Me.IndentString)
                End If
            End If
        End Sub

        Private Sub GetOutOfTag()
            If Me.IsTagOpen Then
                If Me.AttributeEachOnNewLine Then
                    Me.WriteNewLine(True, True)
                End If
                Me.Out.Write(">")
            End If

            Me.IsTagOpen = False
            Me.TagJustOpened = False
        End Sub

        Public Function OpenTag(ByVal Name As String, Optional ByVal MultiLine As Boolean = False, Optional ByVal MultiLineAttributes As Boolean = False) As Opening
            Verify.False(Me.State = WriterState.End, "Cannot write after write is finished.")
            Verify.True(MultiLineAttributes.Implies(MultiLine), "Cannot have multi-line attributes on a non-multi-line tag.")

            Verify.FalseArg(Me.NeedsEscaping(Name), NameOf(Name), "Name contains unacceptable characters.")

            Dim R = New Opening(Me, Name, Me.TagIdCounter,
                                If(Me.State = WriterState.Begin, WriterState.End, Me.State), Me.MultiLine, Me.TagId)

            Me.TagId = Me.TagIdCounter
            Me.TagIdCounter += 1

            If Me.State = WriterState.Begin Then
                If Me.AddXmlDeclaration Then
                    Me.Out.Write("<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>")
                    Me.WriteNewLine(False)
                End If
            End If

            Me.GetOutOfTag()

            If Me.MultiLine Then
                Me.WriteNewLine(False)
            End If

            Dim T = $"<{Name}"
            Me.Out.Write(T)

            Me.AttributeIndent = T.Length + 1

            Me.MultiLine = MultiLine
            Me.MultiLineAttributes = MultiLineAttributes

            Me.IsTagOpen = True
            Me.TagJustOpened = True

            Me.HasValueBefore = False
            Me.State = WriterState.Document

            If MultiLine Then
                Me.CurrentIndent += 1
            End If

            Return R
        End Function

        Public Sub WriteAttribute(ByVal Key As String, ByVal Value As String)
            Verify.False(Me.State = WriterState.Begin, "Cannot write before a tag is opened.")
            Verify.False(Me.State = WriterState.End, "Cannot write after write is finished.")
            Verify.True(Me.IsTagOpen, "Cannot write an attribute when the tag is cloned.")

            Verify.FalseArg(Me.NeedsEscaping(Key), NameOf(Key), "Key contains unacceptable characters.")

            If Me.MultiLineAttributes Then
                If Not Me.TagJustOpened Or Me.AttributeEachOnNewLine Then
                    Me.WriteNewLine(True)
                Else
                    Me.Out.Write(" "c)
                End If
            Else
                Me.Out.Write(" "c)
            End If

            Me.Out.Write(Key)
            Me.Out.Write("=""")
            Me.WriteEscaped(Value)
            Me.Out.Write(""""c)

            Me.TagJustOpened = False
        End Sub

        Public Sub WriteValue(ByVal Value As String)
            Verify.False(Me.State = WriterState.Begin, "Cannot write before a tag is opened.")
            Verify.False(Me.State = WriterState.End, "Cannot write after write is finished.")
            Verify.False(Me.HasValueBefore, "Cannot write two consecutive values.")

            Me.GetOutOfTag()

            If Me.MultiLine Then
                Me.WriteNewLine(False)
            End If
            Me.WriteEscaped(Value)

            Me.HasValueBefore = True
        End Sub

        Private Sub CloseOpening(ByVal Name As String, ByVal TagId As Integer,
                                 ByVal PreviousState As WriterState, ByVal PreviousMultiline As Boolean, ByVal PreviousTagId As Integer)
            Verify.False(Me.State = WriterState.Begin, "Cannot write before a tag is opened.")
            Verify.False(Me.State = WriterState.End, "Cannot write after write is finished.")
            Verify.True(Me.TagId = TagId, $"{Me.TagId} {TagId}Invalid closing of tag. Nested structure is not respected.")

            If Me.MultiLine Then
                Me.CurrentIndent -= 1
            End If

            If Me.IsTagOpen Then
                Me.Out.Write(" />")
                Me.IsTagOpen = False
            Else
                Me.GetOutOfTag()

                If Me.MultiLine Then
                    Me.WriteNewLine(False)
                End If

                Me.Out.Write($"</{Name}>")
            End If

            Me.TagId = PreviousTagId
            Me.MultiLine = PreviousMultiline
            Me.State = PreviousState

            Me.HasValueBefore = False
        End Sub

        Private Sub Reset()
            Me.State = WriterState.Begin

            Me.IsTagOpen = False
            Me.TagJustOpened = False

            Me.MultiLine = False
            Me.MultiLineAttributes = False

            Me.CurrentIndent = 0
            Me.AttributeIndent = 0

            Me.HasValueBefore = False
        End Sub

        Private Shared ReadOnly EscapeDic As Dictionary(Of Char, String) =
                (Function() New Dictionary(Of Char, String) From {
                         {""""c, "&quot;"},
                         {"'"c, "&apos;"},
                         {"&"c, "&amp;"},
                         {"<"c, "&lt;"},
                         {">"c, "&gt;"}
                     }).Invoke()

        Private ReadOnly Out As IO.TextWriter
        Private ReadOnly LeaveOpen As Boolean

        Private TagId As Integer
        Private TagIdCounter As Integer = 0
        Private IsTagOpen As Boolean
        Private TagJustOpened As Boolean
        Private MultiLineAttributes As Boolean
        Private AttributeIndent As Integer
        Private CurrentIndent As Integer
        Private MultiLine As Boolean
        Private HasValueBefore As Boolean
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

#Region "AttributeDynamicIndent Property"
        Private _AttributeDynamicIndent As Boolean = True

        Public Property AttributeDynamicIndent As Boolean
            Get
                Return Me._AttributeDynamicIndent
            End Get
            Set(ByVal Value As Boolean)
                Me._AttributeDynamicIndent = Value
            End Set
        End Property
#End Region

#Region "AttributeEachOnNewLine Read-Only Property"
        Private ReadOnly _AttributeEachOnNewLine As Boolean = False

        Public ReadOnly Property AttributeEachOnNewLine As Boolean
            Get
                Return Me._AttributeEachOnNewLine
            End Get
        End Property
#End Region

#Region "AddXmlDeclaration Property"
        Private _AddXmlDeclaration As Boolean = True

        Public Property AddXmlDeclaration As Boolean
            Get
                Return Me._AddXmlDeclaration
            End Get
            Set(ByVal Value As Boolean)
                Me._AddXmlDeclaration = Value
            End Set
        End Property
#End Region

        Public Structure Opening
            Implements IDisposable

            Friend Sub New(ByVal Writer As XmlWriter, ByVal Name As String, ByVal TagId As Integer, ByVal PreviousState As WriterState, ByVal PreviousMultiline As Boolean, ByVal PreviousTagId As Integer)
                Me.Writer = Writer
                Me.Name = Name
                Me.TagId = TagId
                Me.PreviousState = PreviousState
                Me.PreviousMultiline = PreviousMultiline
                Me.PreviousTagId = PreviousTagId
            End Sub

            Private Sub Dispose() Implements IDisposable.Dispose
                Me.Writer.CloseOpening(Me.Name, Me.TagId, Me.PreviousState, Me.PreviousMultiline, Me.PreviousTagId)
            End Sub

            Public Sub Close()
                Me.Dispose()
            End Sub

            Private ReadOnly Name As String
            Private ReadOnly TagId As Integer
            Private ReadOnly PreviousState As WriterState
            Private ReadOnly PreviousMultiline As Boolean
            Private ReadOnly PreviousTagId As Integer
            Private ReadOnly Writer As XmlWriter

        End Structure

        Friend Enum WriterState

            Begin
            Document
            [End]

        End Enum

    End Class

End Namespace
