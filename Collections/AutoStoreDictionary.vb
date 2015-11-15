Imports System.Windows.Threading

Public Class AutoStoreDictionary
    Implements IDictionary(Of String, String),
               IFormattable,
               IDisposable

    Public Sub New(ByVal Path As String)
        Me.New(IO.File.Open(Path, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.Read))
    End Sub

    Public Sub New(ByVal Stream As IO.Stream)
        'Me.Dispatcher = Dispatcher.CurrentDispatcher
        Me.Stream = Stream

        If Stream.Length = 0 Then
            Me.Dic = New Dictionary(Of String, String)()
            Exit Sub
        End If
        Dim BinaryData = New Byte(CInt(Stream.Length - 1)) {}
        If Stream.ReadAll(BinaryData, 0, BinaryData.Length) <> BinaryData.Length Then
            Throw New Exception("Invalid strem object...")
        End If

        Dim Data = New String(Text.Encoding.UTF8.GetChars(BinaryData))
        Me.Dic = Utilities.Serialization.DicFromStirng(Data)
    End Sub

    Private Sub Collection_Changed()
        If Me.IsWritePending Then
            Exit Sub
        End If
        'SyncLock Me.LockObject
        Me.IsWritePending = True
        'End SyncLock

        Dim Thread = New Threading.Thread(AddressOf Me.Store)
        Thread.IsBackground = True
        Thread.Start()
    End Sub

    Private Sub Store()
        Threading.Thread.Sleep(StoreDelay)

        SyncLock Me.SerializationLockObject
            'SyncLock Me.LockObject
            Me.IsWritePending = False
            'End SyncLock
            'Me.Dispatcher.BeginInvoke()

            Dim SerializedData As String

            SyncLock Me.DicLockObject
                SerializedData = Utilities.Serialization.DicToStirng(Me.Dic)
            End SyncLock

            Dim SerializedBinaryData = Text.Encoding.UTF8.GetBytes(SerializedData)
            Me.Stream.Seek(0, IO.SeekOrigin.Begin)
            Me.Stream.Write(SerializedBinaryData, 0, SerializedBinaryData.Length)
            Me.Stream.SetLength(SerializedBinaryData.Length)
            Me.Stream.Flush()
        End SyncLock
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
            SyncLock Me.DicLockObject
                Me.Dic.Item(Key) = Value
            End SyncLock
            Me.Collection_Changed()
        End Set
    End Property

    Private ReadOnly Property Int_Keys As ICollection(Of String) Implements IDictionary(Of String, String).Keys
        Get
            Return Me.Dic.Keys
        End Get
    End Property

    Public ReadOnly Property Keys As Dictionary(Of String, String).KeyCollection
        Get
            Return Me.Dic.Keys
        End Get
    End Property

    Private ReadOnly Property Int_Values As ICollection(Of String) Implements IDictionary(Of String, String).Values
        Get
            Return Me.Dic.Values
        End Get
    End Property

    Public ReadOnly Property Values As Dictionary(Of String, String).ValueCollection
        Get
            Return Me.Dic.Values
        End Get
    End Property

    Public Sub Add(ByVal Item As KeyValuePair(Of String, String)) Implements ICollection(Of KeyValuePair(Of String, String)).Add
        SyncLock Me.DicLockObject
            Me.Dic.Add(Item.Key, Item.Value)
        End SyncLock
        Me.Collection_Changed()
    End Sub

    Public Sub Add(ByVal Key As String, ByVal Value As String) Implements IDictionary(Of String, String).Add
        SyncLock Me.DicLockObject
            Me.Dic.Add(Key, Value)
        End SyncLock
        Me.Collection_Changed()
    End Sub

    Public Sub Clear() Implements ICollection(Of KeyValuePair(Of String, String)).Clear
        SyncLock Me.DicLockObject
            Me.Dic.Clear()
        End SyncLock
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

    Private Function Int_GetEnumerator() As IEnumerator(Of KeyValuePair(Of String, String)) Implements IEnumerable(Of KeyValuePair(Of String, String)).GetEnumerator
        Return Me.Dic.GetEnumerator()
    End Function

    Public Function GetEnumerator() As Dictionary(Of String, String).Enumerator
        Return Me.Dic.GetEnumerator()
    End Function

    Public Function Remove(item As KeyValuePair(Of String, String)) As Boolean Implements ICollection(Of KeyValuePair(Of String, String)).Remove
        Dim R = False
        SyncLock Me.DicLockObject
            R = DirectCast(Me.Dic, ICollection(Of KeyValuePair(Of String, String))).Remove(item)
        End SyncLock
        Me.Collection_Changed()
        Return R
    End Function

    Public Function Remove(key As String) As Boolean Implements IDictionary(Of String, String).Remove
        Dim R = False
        SyncLock Me.DicLockObject
            R = Me.Dic.Remove(key)
        End SyncLock
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
            If disposing Then
                Me.Stream.Dispose()
            End If

            Me.Dic = Nothing
        End If
        Me._IsDisposed = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub

#Region "IsDisposed Property"
    Private _IsDisposed As Boolean

    Public ReadOnly Property IsDisposed As Boolean
        Get
            Return Me._IsDisposed
        End Get
    End Property
#End Region
#End Region

    Private Const StoreDelay As Integer = 10000

    Private Dic As Dictionary(Of String, String)
    Private ReadOnly DicLockObject As Object = New Object()
    Private ReadOnly SerializationLockObject As Object = New Object()
    Private IsWritePending As Boolean = False
    'Private ReadOnly Dispatcher As Dispatcher
    Private ReadOnly Stream As IO.Stream

End Class
