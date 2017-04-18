Namespace Common

    Public Class DebugWriter
        Inherits IO.TextWriter

        Private Sub New()

        End Sub

#Region "Writes Group"
        Public Overrides Sub Write(value As String)
            Debug.Write(value)
        End Sub

        Public Overrides Sub Write(buffer() As Char)
            Me.Write(New String(buffer))
        End Sub

        Public Overrides Sub Write(buffer() As Char, index As Integer, count As Integer)
            Me.Write(New String(buffer, index, count))
        End Sub

        Public Overrides Sub Write(format As String, arg0 As Object)
            Me.Write(String.Format(Me.FormatProvider, format, arg0))
        End Sub

        Public Overrides Sub Write(format As String, arg0 As Object, arg1 As Object)
            Me.Write(String.Format(Me.FormatProvider, format, arg0, arg1))
        End Sub

        Public Overrides Sub Write(format As String, arg0 As Object, arg1 As Object, arg2 As Object)
            Me.Write(String.Format(Me.FormatProvider, format, arg0, arg1, arg2))
        End Sub

        Public Overrides Sub Write(format As String, ParamArray arg() As Object)
            Me.Write(String.Format(Me.FormatProvider, format, arg))
        End Sub

        Public Overrides Sub Write(value As Char)
            Me.Write(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub Write(value As Boolean)
            Me.Write(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub Write(value As Decimal)
            Me.Write(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub Write(value As Double)
            Me.Write(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub Write(value As Integer)
            Me.Write(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub Write(value As Long)
            Me.Write(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub Write(value As Object)
            Me.Write(String.Format(Me.FormatProvider, "{0}", value))
        End Sub

        Public Overrides Sub Write(value As Single)
            Me.Write(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub Write(value As UInteger)
            Me.Write(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub Write(value As ULong)
            Me.Write(value.ToString(Me.FormatProvider))
        End Sub
#End Region

#Region "WriteLines Group"
        Public Overrides Sub WriteLine(value As String)
            Debug.WriteLine(value)
        End Sub

        Public Overrides Sub WriteLine()
            Me.WriteLine("")
        End Sub

        Public Overrides Sub WriteLine(buffer() As Char)
            Me.WriteLine(New String(buffer))
        End Sub

        Public Overrides Sub WriteLine(buffer() As Char, index As Integer, count As Integer)
            Me.WriteLine(New String(buffer, index, count))
        End Sub

        Public Overrides Sub WriteLine(format As String, arg0 As Object)
            Me.WriteLine(String.Format(Me.FormatProvider, format, arg0))
        End Sub

        Public Overrides Sub WriteLine(format As String, arg0 As Object, arg1 As Object)
            Me.WriteLine(String.Format(Me.FormatProvider, format, arg0, arg1))
        End Sub

        Public Overrides Sub WriteLine(format As String, arg0 As Object, arg1 As Object, arg2 As Object)
            Me.WriteLine(String.Format(Me.FormatProvider, format, arg0, arg1, arg2))
        End Sub

        Public Overrides Sub WriteLine(format As String, ParamArray arg() As Object)
            Me.WriteLine(String.Format(Me.FormatProvider, format, arg))
        End Sub

        Public Overrides Sub WriteLine(value As Boolean)
            Me.WriteLine(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As Char)
            Me.WriteLine(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As Decimal)
            Me.WriteLine(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As Double)
            Me.WriteLine(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As Integer)
            Me.WriteLine(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As Long)
            Me.WriteLine(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As Object)
            Me.WriteLine(String.Format(Me.FormatProvider, "{0}", value))
        End Sub

        Public Overrides Sub WriteLine(value As Single)
            Me.WriteLine(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As UInteger)
            Me.WriteLine(value.ToString(Me.FormatProvider))
        End Sub

        Public Overrides Sub WriteLine(value As ULong)
            Me.WriteLine(value.ToString(Me.FormatProvider))
        End Sub
#End Region

#Region "Asyncs Group"
        Public Overrides Function WriteAsync(buffer() As Char, index As Integer, count As Integer) As Task
            Me.Write(buffer, index, count)
            Return Task.FromResult(Of Void)(Nothing)
        End Function

        Public Overrides Function WriteAsync(value As Char) As Task
            Me.Write(value)
            Return Task.FromResult(Of Void)(Nothing)
        End Function

        Public Overrides Function WriteAsync(value As String) As Task
            Me.Write(value)
            Return Task.FromResult(Of Void)(Nothing)
        End Function

        Public Overrides Function WriteLineAsync() As Task
            Me.WriteLine()
            Return Task.FromResult(Of Void)(Nothing)
        End Function

        Public Overrides Function WriteLineAsync(buffer() As Char, index As Integer, count As Integer) As Task
            Me.WriteLine(buffer, index, count)
            Return Task.FromResult(Of Void)(Nothing)
        End Function

        Public Overrides Function WriteLineAsync(value As Char) As Task
            Me.WriteLine(value)
            Return Task.FromResult(Of Void)(Nothing)
        End Function

        Public Overrides Function WriteLineAsync(value As String) As Task
            Me.WriteLine(value)
            Return Task.FromResult(Of Void)(Nothing)
        End Function
#End Region

        Public Overrides Sub Close()

        End Sub

        Public Overrides Function CreateObjRef(requestedType As Type) As Runtime.Remoting.ObjRef
            Return Nothing
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)

        End Sub

        Public Overrides Sub Flush()

        End Sub

        Public Overrides Function FlushAsync() As Task
            Me.Flush()
            Return Task.FromResult(Of Void)(Nothing)
        End Function

        Public Overrides Function InitializeLifetimeService() As Object
            Return Nothing
        End Function

        Public Overrides Function ToString() As String
            Return NameOf(DebugWriter)
        End Function

        Public Overrides ReadOnly Property Encoding As Text.Encoding
            Get
                Return Text.Encoding.UTF8
            End Get
        End Property

        Public Overrides Property NewLine As String
            Get
                Return Environment.NewLine
            End Get
            Set(value As String)
                Throw New NotSupportedException()
            End Set
        End Property

        Public Overrides ReadOnly Property FormatProvider As IFormatProvider
            Get
                Return Globalization.CultureInfo.InvariantCulture
            End Get
        End Property

#Region "Instance Shared Read-Only Property"
        Private Shared ReadOnly _Instance As DebugWriter = New DebugWriter()

        Public Shared ReadOnly Property Instance As DebugWriter
            Get
                Return _Instance
            End Get
        End Property
#End Region

    End Class

End Namespace
