Public Class TextWriterWrapper
    Inherits IO.TextWriter

    Public Sub New(ByVal TextWriter As IO.TextWriter)
        Me.Base = TextWriter
    End Sub

#Region "Writes Group"
    Public Overrides Sub Write(value As String)
        Me.Base.Write(value)
    End Sub

    Public Overrides Sub Write(buffer() As Char, index As Integer, count As Integer)
        Me.Base.Write(buffer, index, count)
    End Sub

    Public Overrides Sub Write(value As Char)
        Me.Base.Write(value)
    End Sub

    Public Overrides Sub Write(buffer() As Char)
        Me.Base.Write(buffer)
    End Sub

    Public Overrides Sub Write(format As String, arg0 As Object)
        Me.Base.Write(format, arg0)
    End Sub

    Public Overrides Sub Write(format As String, arg0 As Object, arg1 As Object)
        Me.Base.Write(format, arg0, arg1)
    End Sub

    Public Overrides Sub Write(format As String, arg0 As Object, arg1 As Object, arg2 As Object)
        Me.Base.Write(format, arg0, arg1, arg2)
    End Sub

    Public Overrides Sub Write(format As String, ParamArray arg() As Object)
        Me.Base.Write(format, arg)
    End Sub

    Public Overrides Sub Write(value As Boolean)
        Me.Base.Write(value)
    End Sub

    Public Overrides Sub Write(value As Decimal)
        Me.Base.Write(value)
    End Sub

    Public Overrides Sub Write(value As Double)
        Me.Base.Write(value)
    End Sub

    Public Overrides Sub Write(value As Integer)
        Me.Base.Write(value)
    End Sub

    Public Overrides Sub Write(value As Long)
        Me.Base.Write(value)
    End Sub

    Public Overrides Sub Write(value As Object)
        Me.Base.Write(value)
    End Sub

    Public Overrides Sub Write(value As Single)
        Me.Base.Write(value)
    End Sub

    Public Overrides Sub Write(value As UInteger)
        Me.Base.Write(value)
    End Sub

    Public Overrides Sub Write(value As ULong)
        Me.Base.Write(value)
    End Sub
#End Region

#Region "WriteLines Group"
    Public Overrides Sub WriteLine()
        Me.Base.WriteLine()
    End Sub

    Public Overrides Sub WriteLine(buffer() As Char)
        Me.Base.WriteLine(buffer)
    End Sub

    Public Overrides Sub WriteLine(buffer() As Char, index As Integer, count As Integer)
        Me.Base.WriteLine(buffer, index, count)
    End Sub

    Public Overrides Sub WriteLine(format As String, arg0 As Object)
        Me.Base.WriteLine(format, arg0)
    End Sub

    Public Overrides Sub WriteLine(format As String, arg0 As Object, arg1 As Object)
        Me.Base.WriteLine(format, arg0, arg1)
    End Sub

    Public Overrides Sub WriteLine(format As String, arg0 As Object, arg1 As Object, arg2 As Object)
        Me.Base.WriteLine(format, arg0, arg1, arg2)
    End Sub

    Public Overrides Sub WriteLine(format As String, ParamArray arg() As Object)
        Me.Base.WriteLine(format, arg)
    End Sub

    Public Overrides Sub WriteLine(value As Boolean)
        Me.Base.WriteLine(value)
    End Sub

    Public Overrides Sub WriteLine(value As Char)
        Me.Base.WriteLine(value)
    End Sub

    Public Overrides Sub WriteLine(value As Decimal)
        Me.Base.WriteLine(value)
    End Sub

    Public Overrides Sub WriteLine(value As Double)
        Me.Base.WriteLine(value)
    End Sub

    Public Overrides Sub WriteLine(value As Integer)
        Me.Base.WriteLine(value)
    End Sub

    Public Overrides Sub WriteLine(value As Long)
        Me.Base.WriteLine(value)
    End Sub

    Public Overrides Sub WriteLine(value As Object)
        Me.Base.WriteLine(value)
    End Sub

    Public Overrides Sub WriteLine(value As Single)
        Me.Base.WriteLine(value)
    End Sub

    Public Overrides Sub WriteLine(value As String)
        Me.Base.WriteLine(value)
    End Sub

    Public Overrides Sub WriteLine(value As UInteger)
        Me.Base.WriteLine(value)
    End Sub

    Public Overrides Sub WriteLine(value As ULong)
        Me.Base.WriteLine(value)
    End Sub
#End Region

#Region "Asyncs Group"
    Public Overrides Function WriteAsync(buffer() As Char, index As Integer, count As Integer) As Task
        Return Me.Base.WriteAsync(buffer, index, count)
    End Function

    Public Overrides Function WriteAsync(value As Char) As Task
        Return Me.Base.WriteAsync(value)
    End Function

    Public Overrides Function WriteAsync(value As String) As Task
        Return Me.Base.WriteAsync(value)
    End Function

    Public Overrides Function WriteLineAsync() As Task
        Return Me.Base.WriteLineAsync()
    End Function

    Public Overrides Function WriteLineAsync(buffer() As Char, index As Integer, count As Integer) As Task
        Return Me.Base.WriteLineAsync(buffer, index, count)
    End Function

    Public Overrides Function WriteLineAsync(value As Char) As Task
        Return Me.Base.WriteLineAsync(value)
    End Function

    Public Overrides Function WriteLineAsync(value As String) As Task
        Return Me.Base.WriteLineAsync(value)
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
        Return String.Concat(NameOf(TextWriterWrapper), "{", Me.Base.ToString(), "}")
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

    Private ReadOnly Base As IO.TextWriter

End Class
