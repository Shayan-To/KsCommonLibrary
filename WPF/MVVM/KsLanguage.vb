Namespace MVVM

    Public Class KsLanguage
        Implements IDisposable

        Public Sub New(ByVal Stream As IO.Stream)
            Me.Stream = Stream

            Stream.Position = 0
            Me.Csv = CsvData.Parse(New String(Text.Encoding.UTF8.GetChars(Stream.ReadToEnd())), False)

            Verify.True(Me.Csv.Columns.Count <= 2, "Invalid language file.")
            Do While Me.Csv.Columns.Count < 2
                Me.Csv.Columns.Add()
            Loop

            Dim HeaderSet = New HashSet(Of String)()
            Dim I = 0
            Do While I < Me.Csv.Entries.Count
                Dim E = Me.Csv.Entries.Item(I)
                Dim Key = E.Item(0).ToLower()
                Dim Value = E.Item(1)

                If Key.Length = 0 And Value.Length = 0 Then
                    Exit Do
                End If

                Verify.True(HeaderSet.Add(Key), "Duplicate language property.")

                Select Case Key
                    Case NameOf(Me.Id).ToLower()
                        Me._Id = Value
                    Case NameOf(Me.Name).ToLower()
                        Me._Name = Value
                    Case NameOf(Me.NativeName).ToLower()
                        Me._NativeName = Value
                    Case NameOf(Me.Direction).ToLower()
                        Value = Value.ToLower()
                        Verify.True(Value = NameOf(FlowDirection.LeftToRight).ToLower() Or
                                    Value = NameOf(FlowDirection.RightToLeft).ToLower() Or
                                    Value = "rtl" Or Value = "ltr")
                        Me._Direction = If(Value = "ltr" Or Value = NameOf(FlowDirection.LeftToRight).ToLower(),
                                           FlowDirection.LeftToRight,
                                           FlowDirection.RightToLeft)
                    Case Else
                        Verify.Fail("Invalid language property.")
                End Select

                I += 1
            Loop

            If Me.Id Is Nothing Then
                Me._Id = ""
            End If
            If Me.Name Is Nothing And Me.NativeName Is Nothing Then
                Me._Name = "Default"
            End If

            If I = Me.Csv.Entries.Count Then
                Dim E = Me.Csv.Entries.LastOrDefault()
                If E Is Nothing OrElse Not (E.Item(0).Length = 0 And E.Item(1).Length = 0) Then
                    Me.Csv.Entries.Add()
                End If
            End If

            I += 1
            Do While I < Me.Csv.Entries.Count
                Dim E = Me.Csv.Entries.Item(I)
                Dim Key = E.Item(0)
                Dim Value = E.Item(1)

                Me.Dictionary.Add(Key, Value)

                I += 1
            Loop

            Me.TaskDelayer.RunTask(TaskDelayerRunningMode.Delayed)
        End Sub

        Public ReadOnly Property Translation(ByVal Text As String) As String
            Get
                If Text Is Nothing OrElse Text.Length = 0 Then
                    Return Text
                End If

                Dim R As String = Nothing

                If Me.Dictionary.TryGetValue(Text, R) Then
                    Return R
                End If

                Me.Dictionary.Add(Text, Text)

                SyncLock Me.LockObject
                    Dim E = Me.Csv.Entries.Add()
                    E.Item(0) = Text
                    E.Item(1) = Text
                End SyncLock

                Me.TaskDelayer.RunTask(TaskDelayerRunningMode.Delayed)

                Return Text
            End Get
        End Property

        Private Sub DoStore()
            Dim Str As String
            SyncLock Me.LockObject
                Dim HeaderSize = 0
                Do Until Me.Csv.Entries.Count = HeaderSize
                    Dim H = Me.Csv.Entries.Item(HeaderSize)

                    If H.Item(0).Length = 0 And H.Item(1).Length = 0 Then
                        Exit Do
                    End If
                    HeaderSize += 1
                Loop

                Assert.True(HeaderSize <= 4)
                Do While HeaderSize < 4
                    Me.Csv.Entries.Insert(0)
                    HeaderSize += 1
                Loop

                Dim E = Me.Csv.Entries.Item(0)
                E.Item(0) = NameOf(Me.Id)
                E.Item(1) = Me.Id

                E = Me.Csv.Entries.Item(1)
                E.Item(0) = NameOf(Me.Name)
                E.Item(1) = Me.Name

                E = Me.Csv.Entries.Item(2)
                E.Item(0) = NameOf(Me.NativeName)
                E.Item(1) = Me.NativeName

                E = Me.Csv.Entries.Item(3)
                E.Item(0) = NameOf(Me.Direction)
                E.Item(1) = If(Me.Direction = FlowDirection.LeftToRight, NameOf(FlowDirection.LeftToRight), NameOf(FlowDirection.RightToLeft))

                Str = Me.Csv.ToString()
            End SyncLock

            Dim Bytes = Text.Encoding.UTF8.GetBytes(Str)
            Me.Stream.Position = 0
            Me.Stream.Write(Bytes, 0, Bytes.Length)
            Me.Stream.SetLength(Bytes.Length)
            Me.Stream.Flush()
        End Sub

#Region "IDisposable Support"
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me._IsDisposed Then
                Me._IsDisposed = True

                If disposing Then
                    ' Dispose managed state (managed objects).
                    Me.TaskDelayer.Dispose()
                End If

                ' Set large fields to null.
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
        End Sub
#End Region

#Region "Id Property"
        Private ReadOnly _Id As String

        Public ReadOnly Property Id As String
            Get
                Return Me._Id
            End Get
        End Property
#End Region

#Region "Name Property"
        Private ReadOnly _Name As String

        Public ReadOnly Property Name As String
            Get
                Return Me._Name
            End Get
        End Property
#End Region

#Region "NativeName Property"
        Private ReadOnly _NativeName As String

        Public ReadOnly Property NativeName As String
            Get
                Return Me._NativeName
            End Get
        End Property
#End Region

#Region "Direction Property"
        Private ReadOnly _Direction As FlowDirection

        Public ReadOnly Property Direction As FlowDirection
            Get
                Return Me._Direction
            End Get
        End Property
#End Region

#Region "IsDisposed Property"
        Private _IsDisposed As Boolean

        Public ReadOnly Property IsDisposed As Boolean
            Get
                Return Me._IsDisposed
            End Get
        End Property
#End Region

        Private ReadOnly LockObject As Object = New Object()

        Private ReadOnly Dictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)()
        Private ReadOnly Csv As CsvData
        Private ReadOnly TaskDelayer As TaskDelayer = New TaskDelayer(AddressOf Me.DoStore, 10000)
        Private ReadOnly Stream As IO.Stream

    End Class

End Namespace
