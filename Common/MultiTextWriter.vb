Public Class MultiTextWriter
    Inherits IO.TextWriter

    Public Sub New(ByVal TextWriters As IEnumerable(Of IO.TextWriter))
        Me.Bases = TextWriters.ToArray()
    End Sub

    Public Sub New(ParamArray ByVal TextWriters As IO.TextWriter())
        Me.New(DirectCast(TextWriters, IEnumerable(Of IO.TextWriter)))
    End Sub

#Region "Writes Group"
    Public Overrides Sub Write(value As String)
        For Each B In Me.Bases
            B.Write(value)
        Next
    End Sub

    Public Overrides Sub Write(buffer() As Char, index As Integer, count As Integer)
        For Each B In Me.Bases
            B.Write(buffer, index, count)
        Next
    End Sub

    Public Overrides Sub Write(value As Char)
        For Each B In Me.Bases
            B.Write(value)
        Next
    End Sub

    Public Overrides Sub Write(buffer() As Char)
        For Each B In Me.Bases
            B.Write(buffer)
        Next
    End Sub

    Public Overrides Sub Write(format As String, arg0 As Object)
        For Each B In Me.Bases
            B.Write(format, arg0)
        Next
    End Sub

    Public Overrides Sub Write(format As String, arg0 As Object, arg1 As Object)
        For Each B In Me.Bases
            B.Write(format, arg0, arg1)
        Next
    End Sub

    Public Overrides Sub Write(format As String, arg0 As Object, arg1 As Object, arg2 As Object)
        For Each B In Me.Bases
            B.Write(format, arg0, arg1, arg2)
        Next
    End Sub

    Public Overrides Sub Write(format As String, ParamArray arg() As Object)
        For Each B In Me.Bases
            B.Write(format, arg)
        Next
    End Sub

    Public Overrides Sub Write(value As Boolean)
        For Each B In Me.Bases
            B.Write(value)
        Next
    End Sub

    Public Overrides Sub Write(value As Decimal)
        For Each B In Me.Bases
            B.Write(value)
        Next
    End Sub

    Public Overrides Sub Write(value As Double)
        For Each B In Me.Bases
            B.Write(value)
        Next
    End Sub

    Public Overrides Sub Write(value As Integer)
        For Each B In Me.Bases
            B.Write(value)
        Next
    End Sub

    Public Overrides Sub Write(value As Long)
        For Each B In Me.Bases
            B.Write(value)
        Next
    End Sub

    Public Overrides Sub Write(value As Object)
        For Each B In Me.Bases
            B.Write(value)
        Next
    End Sub

    Public Overrides Sub Write(value As Single)
        For Each B In Me.Bases
            B.Write(value)
        Next
    End Sub

    Public Overrides Sub Write(value As UInteger)
        For Each B In Me.Bases
            B.Write(value)
        Next
    End Sub

    Public Overrides Sub Write(value As ULong)
        For Each B In Me.Bases
            B.Write(value)
        Next
    End Sub
#End Region

#Region "WriteLines Group"
    Public Overrides Sub WriteLine()
        For Each B In Me.Bases
            B.WriteLine()
        Next
    End Sub

    Public Overrides Sub WriteLine(buffer() As Char)
        For Each B In Me.Bases
            B.WriteLine(buffer)
        Next
    End Sub

    Public Overrides Sub WriteLine(buffer() As Char, index As Integer, count As Integer)
        For Each B In Me.Bases
            B.WriteLine(buffer, index, count)
        Next
    End Sub

    Public Overrides Sub WriteLine(format As String, arg0 As Object)
        For Each B In Me.Bases
            B.WriteLine(format, arg0)
        Next
    End Sub

    Public Overrides Sub WriteLine(format As String, arg0 As Object, arg1 As Object)
        For Each B In Me.Bases
            B.WriteLine(format, arg0, arg1)
        Next
    End Sub

    Public Overrides Sub WriteLine(format As String, arg0 As Object, arg1 As Object, arg2 As Object)
        For Each B In Me.Bases
            B.WriteLine(format, arg0, arg1, arg2)
        Next
    End Sub

    Public Overrides Sub WriteLine(format As String, ParamArray arg() As Object)
        For Each B In Me.Bases
            B.WriteLine(format, arg)
        Next
    End Sub

    Public Overrides Sub WriteLine(value As Boolean)
        For Each B In Me.Bases
            B.WriteLine(value)
        Next
    End Sub

    Public Overrides Sub WriteLine(value As Char)
        For Each B In Me.Bases
            B.WriteLine(value)
        Next
    End Sub

    Public Overrides Sub WriteLine(value As Decimal)
        For Each B In Me.Bases
            B.WriteLine(value)
        Next
    End Sub

    Public Overrides Sub WriteLine(value As Double)
        For Each B In Me.Bases
            B.WriteLine(value)
        Next
    End Sub

    Public Overrides Sub WriteLine(value As Integer)
        For Each B In Me.Bases
            B.WriteLine(value)
        Next
    End Sub

    Public Overrides Sub WriteLine(value As Long)
        For Each B In Me.Bases
            B.WriteLine(value)
        Next
    End Sub

    Public Overrides Sub WriteLine(value As Object)
        For Each B In Me.Bases
            B.WriteLine(value)
        Next
    End Sub

    Public Overrides Sub WriteLine(value As Single)
        For Each B In Me.Bases
            B.WriteLine(value)
        Next
    End Sub

    Public Overrides Sub WriteLine(value As String)
        For Each B In Me.Bases
            B.WriteLine(value)
        Next
    End Sub

    Public Overrides Sub WriteLine(value As UInteger)
        For Each B In Me.Bases
            B.WriteLine(value)
        Next
    End Sub

    Public Overrides Sub WriteLine(value As ULong)
        For Each B In Me.Bases
            B.WriteLine(value)
        Next
    End Sub
#End Region

#Region "Asyncs Group"
    Public Overrides Function WriteAsync(buffer() As Char, index As Integer, count As Integer) As Task
        Return Task.WhenAll(Me.Bases.Select(Function(B) B.WriteAsync(buffer, index, count)))
    End Function

    Public Overrides Function WriteAsync(value As Char) As Task
        Return Task.WhenAll(Me.Bases.Select(Function(B) B.WriteAsync(value)))
    End Function

    Public Overrides Function WriteAsync(value As String) As Task
        Return Task.WhenAll(Me.Bases.Select(Function(B) B.WriteAsync(value)))
    End Function

    Public Overrides Function WriteLineAsync() As Task
        Return Task.WhenAll(Me.Bases.Select(Function(B) B.WriteLineAsync()))
    End Function

    Public Overrides Function WriteLineAsync(buffer() As Char, index As Integer, count As Integer) As Task
        Return Task.WhenAll(Me.Bases.Select(Function(B) B.WriteLineAsync(buffer, index, count)))
    End Function

    Public Overrides Function WriteLineAsync(value As Char) As Task
        Return Task.WhenAll(Me.Bases.Select(Function(B) B.WriteLineAsync(value)))
    End Function

    Public Overrides Function WriteLineAsync(value As String) As Task
        Return Task.WhenAll(Me.Bases.Select(Function(B) B.WriteLineAsync(value)))
    End Function
#End Region

    Public Overrides Sub Close()
        For Each B In Me.Bases
            B.Close()
        Next
    End Sub

    Public Overrides Function CreateObjRef(requestedType As Type) As Runtime.Remoting.ObjRef
        Throw New NotSupportedException()
    End Function

    Protected Overrides Sub Dispose(disposing As Boolean)
        For Each B In Me.Bases
            B.Dispose()
        Next
    End Sub

    Public Overrides Sub Flush()
        For Each B In Me.Bases
            B.Flush()
        Next
    End Sub

    Public Overrides Function FlushAsync() As Task
        Return Task.WhenAll(Me.Bases.Select(Function(B) B.FlushAsync()))
    End Function

    Public Overrides Function InitializeLifetimeService() As Object
        Throw New NotSupportedException()
    End Function

    Public Overrides Function ToString() As String
        Dim Res = New Text.StringBuilder()
        Res.Append(NameOf(MultiTextWriter)).Append("{"c)

        Dim Bl = False
        For Each B In Me.Bases
            If Bl Then
                Res.Append(","c)
            End If
            Bl = True
            Res.Append(B.ToString())
        Next

        Res.Append("}"c)
        Return Res.ToString()
    End Function

    Public Overrides ReadOnly Property Encoding As Text.Encoding
        Get
            Return Me.Bases.FirstOrDefault()?.Encoding
        End Get
    End Property

    Public Overrides Property NewLine As String
        Get
            Return Me.Bases.FirstOrDefault()?.NewLine
        End Get
        Set(value As String)
            For Each B In Me.Bases
                B.NewLine = value
            Next
        End Set
    End Property

    Public Overrides ReadOnly Property FormatProvider As IFormatProvider
        Get
            Return Me.Bases.FirstOrDefault()?.FormatProvider
        End Get
    End Property

    Private ReadOnly Bases As IO.TextWriter()

End Class
