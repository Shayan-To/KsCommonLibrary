Public Class SaveDelayer

    Public Sub New(ByVal Stream As IO.Stream, Optional ByVal Encoding As Text.Encoding = Nothing)
        If Encoding Is Nothing Then
            Encoding = Text.Encoding.UTF8
        End If

        Me._Stream = Stream
        Me._Encoding = Encoding
    End Sub

    Private Sub DoSave()
        Me.Stream.Position = 0
        Dim N = Me.Stream.Write(Me.InStream, Me.Length)
        Me.Stream.SetLength(N)
    End Sub

    Public Sub Save(ByVal Stream As IO.Stream, ByVal Length As Integer, ByVal RunningMode As TaskDelayerRunningMode)
        Me.InStream = Stream
        Me.Length = Length
        Me.TaskDelayer.RunTask(RunningMode)
    End Sub

    Public Sub Save(ByVal Stream As IO.Stream, ByVal RunningMode As TaskDelayerRunningMode)
        Me.Save(Stream, -1, RunningMode)
    End Sub

    Public Sub Save(ByVal Buffer As Byte(), ByVal Index As Integer, ByVal Length As Integer, ByVal RunningMode As TaskDelayerRunningMode)
        Dim Strm = New IO.MemoryStream(Buffer, Index, Length)
        Me.Save(Strm, RunningMode)
    End Sub

    Public Sub Save(ByVal Buffer As Byte(), ByVal RunningMode As TaskDelayerRunningMode)
        Me.Save(Buffer, 0, Buffer.Length, RunningMode)
    End Sub

    Public Sub Save(ByVal Str As String, ByVal RunningMode As TaskDelayerRunningMode)
        Me.Save(Me.Encoding.GetBytes(Str), RunningMode)
    End Sub

#Region "Stream Property"
    Private ReadOnly _Stream As IO.Stream

    Public ReadOnly Property Stream As IO.Stream
        Get
            Return Me._Stream
        End Get
    End Property
#End Region

#Region "Encoding Property"
    Private ReadOnly _Encoding As Text.Encoding

    Public ReadOnly Property Encoding As Text.Encoding
        Get
            Return Me._Encoding
        End Get
    End Property
#End Region

    Private ReadOnly TaskDelayer As TaskDelayer = New TaskDelayer(AddressOf Me.DoSave, 10000)
    Private InStream As IO.Stream
    Private Length As Integer

End Class
