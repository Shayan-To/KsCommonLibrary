Namespace Common

    Public Class WordReader

        Public Sub New(ByVal TextReader As IO.TextReader)
            Me._TextReader = TextReader
        End Sub

        Public Function ReadWord() As String
            Do
                If Me.Index = Me.BufferSize Then
                    Me.BufferSize = Me.TextReader.Read(Me.Buffer, 0, Me.Buffer.Length)
                    If Me.BufferSize = 0 Then
                        Return Nothing
                    End If
                    Me.Index = 0
                End If

                If Not Char.IsWhiteSpace(Me.Buffer(Me.Index)) Then
                    Exit Do
                End If

                Me.Index += 1
            Loop

            Dim Res = ""
            Dim StartIndex = Me.Index
            Do
                If Me.Index = Me.BufferSize Then
                    If StartIndex <> Me.Index Then
                        Assert.True(StartIndex < Me.Index)
                        Res &= New String(Me.Buffer, StartIndex, Me.Index - StartIndex)
                    End If

                    Me.BufferSize = Me.TextReader.Read(Me.Buffer, 0, Me.Buffer.Length)
                    If Me.BufferSize = 0 Then
                        Exit Do
                    End If
                    Me.Index = 0
                End If

                If Char.IsWhiteSpace(Me.Buffer(Me.Index)) Then
                    Res &= New String(Me.Buffer, StartIndex, Me.Index - StartIndex)
                    Exit Do
                End If

                Me.Index += 1
            Loop

            Return Res
        End Function

#Region "TextReader Read-Only Property"
        Private ReadOnly _TextReader As IO.TextReader

        Public ReadOnly Property TextReader As IO.TextReader
            Get
                Return Me._TextReader
            End Get
        End Property
#End Region

        Private ReadOnly Buffer As Char() = New Char(4095) {}
        Private BufferSize As Integer = 0
        Private Index As Integer

    End Class

End Namespace
