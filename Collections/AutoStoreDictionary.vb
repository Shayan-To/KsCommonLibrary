Public Class AutoStoreDictionary
    Implements IDictionary(Of String, String),
               IFormattable,
               IDisposable

    Public Sub New(ByVal Path As String)
        Me.New(IO.File.Open(Path, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.Read))
    End Sub

    Public Sub New(ByVal Stream As IO.Stream, Optional ByVal LeaveOpen As Boolean = False)
        Me.Stream = Stream
        Me.LeaveOpen = LeaveOpen

        If Stream.Length = 0 Then
            Me.Dic = New Concurrent.ConcurrentDictionary(Of String, String)()
            Exit Sub
        End If

        Dim BinaryData = Stream.ReadToEnd()
        Dim Data = New String(Text.Encoding.UTF8.GetChars(BinaryData))
        Me.Dic = New Concurrent.ConcurrentDictionary(Of String, String)(2, Utilities.Serialization.DicFromStirng(Data), StringComparer.CurrentCulture)
    End Sub

    Private Sub Collection_Changed()
        Me.TaskDelayer.RunTask(TaskDelayerRunningMode.Delayed)
    End Sub

    Private Sub Store()
        Dim SerializedData As String

        SerializedData = Utilities.Serialization.DicToStirng(Me.Dic)

        Dim SerializedBinaryData = Text.Encoding.UTF8.GetBytes(SerializedData)
        Me.Stream.Position = 0
        Me.Stream.Write(SerializedBinaryData, 0, SerializedBinaryData.Length)
        Me.Stream.SetLength(SerializedBinaryData.Length)
        Me.Stream.Flush()
    End Sub

#Region "Dictionary Implementation"
    Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of String, String)).Count
        Get
            Return Me.Dic.Count
        End Get
    End Property

    Private ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of String, String)).IsReadOnly
        Get
            Return DirectCast(Me.Dic, ICollection(Of KeyValuePair(Of String, String))).IsReadOnly
        End Get
    End Property

    Default Public Property Item(ByVal Key As String) As String Implements IDictionary(Of String, String).Item
        Get
            Return Me.Dic.Item(Key)
        End Get
        Set(ByVal Value As String)
            Me.Dic.Item(Key) = Value
            Me.Collection_Changed()
        End Set
    End Property

    Public ReadOnly Property Keys As ICollection(Of String) Implements IDictionary(Of String, String).Keys
        Get
            Return Me.Dic.Keys
        End Get
    End Property

    Public ReadOnly Property Values As ICollection(Of String) Implements IDictionary(Of String, String).Values
        Get
            Return Me.Dic.Values
        End Get
    End Property

    Public Sub Add(ByVal Item As KeyValuePair(Of String, String)) Implements ICollection(Of KeyValuePair(Of String, String)).Add
        Me.Dic.Add(Item.Key, Item.Value)
        Me.Collection_Changed()
    End Sub

    Public Sub Add(ByVal Key As String, ByVal Value As String) Implements IDictionary(Of String, String).Add
        Me.Dic.Add(Key, Value)
        Me.Collection_Changed()
    End Sub

    Public Sub Clear() Implements ICollection(Of KeyValuePair(Of String, String)).Clear
        Me.Dic.Clear()
        Me.Collection_Changed()
    End Sub

    Public Sub CopyTo(array() As KeyValuePair(Of String, String), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of String, String)).CopyTo
        DirectCast(Me.Dic, ICollection(Of KeyValuePair(Of String, String))).CopyTo(array, arrayIndex)
    End Sub

    Public Function Contains(item As KeyValuePair(Of String, String)) As Boolean Implements ICollection(Of KeyValuePair(Of String, String)).Contains
        Return DirectCast(Me.Dic, ICollection(Of KeyValuePair(Of String, String))).Contains(item)
    End Function

    Public Function ContainsKey(key As String) As Boolean Implements IDictionary(Of String, String).ContainsKey
        Return Me.Dic.ContainsKey(key)
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.Dic.GetEnumerator()
    End Function

    Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of String, String)) Implements IEnumerable(Of KeyValuePair(Of String, String)).GetEnumerator
        Return Me.Dic.GetEnumerator()
    End Function

    Public Function Remove(item As KeyValuePair(Of String, String)) As Boolean Implements ICollection(Of KeyValuePair(Of String, String)).Remove
        Dim R = False
        R = DirectCast(Me.Dic, ICollection(Of KeyValuePair(Of String, String))).Remove(item)
        Me.Collection_Changed()
        Return R
    End Function

    Public Function Remove(key As String) As Boolean Implements IDictionary(Of String, String).Remove
        Dim R = False
        R = Me.Dic.Remove(key)
        Me.Collection_Changed()
        Return R
    End Function

    Public Function TryGetValue(key As String, ByRef value As String) As Boolean Implements IDictionary(Of String, String).TryGetValue
        Return Me.Dic.TryGetValue(key, value)
    End Function

    Public Overrides Function ToString() As String
        Return Me.ToString("", Utilities.Text.CurruntFormatProvider)
    End Function

    Public Overloads Function ToString(ByVal format As String, ByVal formatProvider As IFormatProvider) As String Implements System.IFormattable.ToString
        Dim R = New Text.StringBuilder("{"c)
        Dim Bl = True
        For Each KV In Me
            If Bl Then
                Bl = False
            Else
                R.Append(", ")
            End If

            R.Append(KV.Key).Append(" : ").Append(String.Format(formatProvider, "{0:" & format & "}", KV.Value))
        Next

        Return R.Append("}"c).ToString()
    End Function
#End Region

#Region "IDisposable Support"
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me._IsDisposed Then
            Me._IsDisposed = True

            If disposing Then
                ' Dispose managed state (managed objects).
                Me.TaskDelayer.Dispose()
            End If

            ' Set large fields to null.
            Me.Dic = Nothing
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub
#End Region

#Region "IsDisposed Property"
    Private _IsDisposed As Boolean

    Public ReadOnly Property IsDisposed As Boolean
        Get
            Return Me._IsDisposed
        End Get
    End Property
#End Region

    ' ConcurrentDicionary has a weired API, so casting it as an IDictionary to use it with ease.
    Private Dic As IDictionary(Of String, String) ' Concurrent.ConcurrentDicionary(Of String, String)
    Private ReadOnly Stream As IO.Stream
    Private ReadOnly LeaveOpen As Boolean
    Private ReadOnly TaskDelayer As TaskDelayer = New TaskDelayer(AddressOf Me.Store, 10000)

End Class
