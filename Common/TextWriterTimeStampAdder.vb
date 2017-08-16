Namespace Common

    Public Class TextWriterTimeStampAdder
        Inherits IO.TextWriter

        Public Sub New(ByVal TextWriter As IO.TextWriter)
            Me.Base = TextWriter
        End Sub

        Private Function GetTimeStamp() As String
            Return Utilities.Representation.GetTimeStamp(False) & " :: "
        End Function

#Region "Writes Group"
        Public Overrides Sub Write(value As String)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(buffer() As Char, index As Integer, count As Integer)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(value As Char)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(buffer() As Char)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(format As String, arg0 As Object)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(format As String, arg0 As Object, arg1 As Object)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(format As String, arg0 As Object, arg1 As Object, arg2 As Object)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(format As String, ParamArray arg() As Object)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(value As Boolean)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(value As Decimal)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(value As Double)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(value As Integer)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(value As Long)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(value As Object)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(value As Single)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(value As UInteger)
            Throw New NotSupportedException()
        End Sub

        Public Overrides Sub Write(value As ULong)
            Throw New NotSupportedException()
        End Sub
#End Region

#Region "WriteLines Group"
        Public Overrides Sub WriteLine(buffer() As Char, index As Integer, count As Integer)
            If buffer Is Nothing Then
                Me.Base.WriteLine(Me.GetTimeStamp())
                If Me.AutoFlush Then
                    Me.Base.Flush()
                End If
                Exit Sub
            End If

            Dim Stamp = Me.GetTimeStamp()
            Dim StampLength = Stamp.Length
            Me.Base.Write(Stamp)

            Dim SI = index
            Dim Bl = False
            For I As Integer = index To index + count
                Dim Ch As Char
                If I < index + count Then
                    Ch = buffer(I)
                Else
                    Ch = ControlChars.Lf
                End If
                If Ch = ControlChars.Cr Or Ch = ControlChars.Lf Then
                    If Bl Then
                        Me.Base.Write(New String(" "c, StampLength))
                    End If
                    Bl = True
                    Me.Base.WriteLine(buffer, SI, I - SI)

                    If I < index + count - 1 AndAlso
                       (Ch = ControlChars.Cr And buffer(I + 1) = ControlChars.Lf) Then
                        I += 1
                    End If
                    SI = I + 1
                End If
            Next

            If Me.AutoFlush Then
                Me.Base.Flush()
            End If
        End Sub

        Public Overrides Sub WriteLine(value As Object)
            Dim Formattable = TryCast(value, IFormattable)
            If Formattable IsNot Nothing Then
                Me.WriteLine(Formattable.ToString(Nothing, Me.Base.FormatProvider))
            Else
                Me.WriteLine(value?.ToString())
            End If
        End Sub

        Public Overrides Sub WriteLine()
            Me.WriteLine(Nothing, 0, 0)
        End Sub

        Public Overrides Sub WriteLine(buffer() As Char)
            Me.WriteLine(buffer, 0, buffer.Length)
        End Sub

        Public Overrides Sub WriteLine(value As String)
            Me.WriteLine(value?.ToCharArray(), 0, If(value?.Length, 0))
        End Sub

        Public Overrides Sub WriteLine(format As String, arg0 As Object)
            Me.WriteLine(String.Format(format, arg0))
        End Sub

        Public Overrides Sub WriteLine(format As String, arg0 As Object, arg1 As Object)
            Me.WriteLine(String.Format(format, arg0, arg1))
        End Sub

        Public Overrides Sub WriteLine(format As String, arg0 As Object, arg1 As Object, arg2 As Object)
            Me.WriteLine(String.Format(format, arg0, arg1, arg2))
        End Sub

        Public Overrides Sub WriteLine(format As String, ParamArray arg() As Object)
            Me.WriteLine(String.Format(format, arg))
        End Sub

        Public Overrides Sub WriteLine(value As Boolean)
            Me.WriteLine(value.ToString(Me.Base.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As Char)
            Me.WriteLine(value.ToString(Me.Base.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As Decimal)
            Me.WriteLine(value.ToString(Me.Base.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As Double)
            Me.WriteLine(value.ToString(Me.Base.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As Integer)
            Me.WriteLine(value.ToString(Me.Base.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As Long)
            Me.WriteLine(value.ToString(Me.Base.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As Single)
            Me.WriteLine(value.ToString(Me.Base.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As UInteger)
            Me.WriteLine(value.ToString(Me.Base.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As ULong)
            Me.WriteLine(value.ToString(Me.Base.FormatProvider))
        End Sub
#End Region

#Region "Asyncs Group"
        Public Overrides Function WriteAsync(buffer() As Char, index As Integer, count As Integer) As Task
            Throw New NotSupportedException()
        End Function

        Public Overrides Function WriteAsync(value As Char) As Task
            Throw New NotSupportedException()
        End Function

        Public Overrides Function WriteAsync(value As String) As Task
            Throw New NotSupportedException()
        End Function

        Public Overrides Async Function WriteLineAsync(buffer() As Char, index As Integer, count As Integer) As Task
            If buffer Is Nothing Then
                Await Me.Base.WriteLineAsync(Me.GetTimeStamp())
                If Me.AutoFlush Then
                    Await Me.Base.FlushAsync()
                End If
                Exit Function
            End If

            Dim Stamp = Me.GetTimeStamp()
            Dim StampLength = Stamp.Length
            Await Me.Base.WriteAsync(Stamp)

            Dim SI = index
            Dim Bl = False
            For I As Integer = index To index + count
                Dim Ch As Char
                If I < index + count Then
                    Ch = buffer(I)
                Else
                    Ch = ControlChars.Lf
                End If
                If Ch = ControlChars.Cr Or Ch = ControlChars.Lf Then
                    If Bl Then
                        Await Me.Base.WriteAsync(New String(" "c, StampLength))
                    End If
                    Bl = True
                    Await Me.Base.WriteLineAsync(buffer, SI, I - SI)

                    If I < index + count - 1 AndAlso
                       (Ch = ControlChars.Cr And buffer(I + 1) = ControlChars.Lf) Then
                        I += 1
                    End If
                    SI = I + 1
                End If
            Next

            If Me.AutoFlush Then
                Await Me.Base.FlushAsync()
            End If
        End Function

        Public Overrides Async Function WriteLineAsync() As Task
            Await Me.WriteLineAsync(Nothing, 0, 0)
        End Function

        Public Overrides Async Function WriteLineAsync(value As Char) As Task
            Await Me.WriteLineAsync(value.ToString(Me.Base.FormatProvider))
        End Function

        Public Overrides Async Function WriteLineAsync(value As String) As Task
            Await Me.WriteLineAsync(value?.ToCharArray(), 0, If(value?.Length, 0))
        End Function
#End Region

        Public Overrides Sub Close()
            Me.Base.Close()
        End Sub

        Public Overrides Function CreateObjRef(requestedType As Type) As Runtime.Remoting.ObjRef
            Return Me.Base.CreateObjRef(requestedType)
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)
            Me.Base.Dispose()
        End Sub

        Public Overrides Sub Flush()
            Me.Base.Flush()
        End Sub

        Public Overrides Function FlushAsync() As Task
            Return Me.Base.FlushAsync()
        End Function

        Public Overrides Function InitializeLifetimeService() As Object
            Return Me.Base.InitializeLifetimeService()
        End Function

        Public Overrides Function ToString() As String
            Return String.Concat(NameOf(TextWriterTimeStampAdder), "{", Me.Base.ToString(), "}")
        End Function

        Public Overrides ReadOnly Property Encoding As Text.Encoding
            Get
                Return Me.Base.Encoding
            End Get
        End Property

        Public Overrides Property NewLine As String
            Get
                Return Me.Base.NewLine
            End Get
            Set(value As String)
                Me.Base.NewLine = value
            End Set
        End Property

        Public Overrides ReadOnly Property FormatProvider As IFormatProvider
            Get
                Return Me.Base.FormatProvider
            End Get
        End Property

#Region "AutoFlush Property"
        Private _AutoFlush As Boolean = True

        Public Property AutoFlush As Boolean
            Get
                Return Me._AutoFlush
            End Get
            Set(ByVal Value As Boolean)
                Me._AutoFlush = Value
            End Set
        End Property
#End Region

        Private ReadOnly Base As IO.TextWriter

    End Class

End Namespace
