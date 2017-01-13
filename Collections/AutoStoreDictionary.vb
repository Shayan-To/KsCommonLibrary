Imports System.Collections.Specialized

Namespace Common

    Public Class AutoStoreDictionary
        Inherits BaseDictionary(Of String, String)
        Implements IFormattable,
                   IDisposable,
                   IOrderedDictionary(Of String, String)

        Public Sub New(ByVal Path As String)
            Me.New(IO.File.Open(Path, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.Read))
        End Sub

        Public Sub New(ByVal Stream As IO.Stream, Optional ByVal LeaveOpen As Boolean = False)
            Verify.True(Stream.CanRead And Stream.CanWrite And Stream.CanSeek, "Stream must have read, write and seek capabilities.")

            Me.Stream = Stream
            Me._LeaveOpen = LeaveOpen

            If Stream.Length = 0 Then
                Me.BaseDic = New ConcurrentOrderedDictionary(Of String, String)()
                Exit Sub
            End If

            Dim BinaryData = Stream.ReadToEnd()
            Dim Data = New String(Text.Encoding.UTF8.GetChars(BinaryData))

            ' For backward compatibility.
            Dim T As OrderedDictionary(Of String, String)
            If Data.Chars(0) = "{"c Then
                T = Utilities.Serialization.DicFromString(Data)
            Else
                T = Utilities.Serialization.DicFromStringMultiline(Data)
            End If

            Me.BaseDic = New ConcurrentOrderedDictionary(Of String, String)(T, StringComparer.InvariantCulture)
        End Sub

        Private Sub Collection_Changed()
            Me.TaskDelayer.RunTask(TaskDelayerRunningMode.Delayed)
        End Sub

        Private Sub Store()
            Dim SerializedData As String

            SerializedData = Utilities.Serialization.DicToStringMultiline(Me.BaseDic)

            Dim SerializedBinaryData = Text.Encoding.UTF8.GetBytes(SerializedData)
            Me.Stream.Position = 0
            Me.Stream.Write(SerializedBinaryData, 0, SerializedBinaryData.Length)
            Me.Stream.SetLength(SerializedBinaryData.Length)
            Me.Stream.Flush()
        End Sub

        Public Overrides ReadOnly Property Count As Integer
            Get
                Return Me.BaseDic.Count
            End Get
        End Property

        Default Public Overrides Property Item(key As String) As String
            Get
                Return Me.BaseDic.Item(key)
            End Get
            Set(value As String)
                Me.BaseDic.Item(key) = value
                Me.Collection_Changed()
            End Set
        End Property

        Public Overrides ReadOnly Property Keys As ICollection(Of String)
            Get
                Return DirectCast(Me.KeysList, ICollection(Of String))
            End Get
        End Property

        Public Overrides ReadOnly Property Values As ICollection(Of String)
            Get
                Return DirectCast(Me.ValuesList, ICollection(Of String))
            End Get
        End Property

        Public ReadOnly Property KeysList As IReadOnlyList(Of String)
            Get
                Return Me.BaseDic.KeysList
            End Get
        End Property

        Public ReadOnly Property ValuesList As IReadOnlyList(Of String)
            Get
                Return Me.BaseDic.ValuesList
            End Get
        End Property

        Public Property ItemAt(index As Integer) As KeyValuePair(Of String, String) Implements IList(Of KeyValuePair(Of String, String)).Item
            Get
                Return Me.BaseDic.ItemAt(index)
            End Get
            Set(value As KeyValuePair(Of String, String))
                Me.BaseDic.ItemAt(index) = value
                Me.Collection_Changed()
            End Set
        End Property

        Private ReadOnly Property IList_IsReadOnly As Boolean Implements IList.IsReadOnly
            Get
                Return False
            End Get
        End Property

        Private ReadOnly Property IList_IsFixedSize As Boolean Implements IList.IsFixedSize
            Get
                Return False
            End Get
        End Property

        Public Overrides Sub Add(key As String, value As String)
            Me.BaseDic.Add(key, value)
            Me.Collection_Changed()
        End Sub

        Public Overrides Sub Clear() Implements IList.Clear, IList(Of KeyValuePair(Of String, String)).Clear
            Me.BaseDic.Clear()
            Me.Collection_Changed()
        End Sub

        Public Sub Insert(index As Integer, key As String, value As String) Implements IOrderedDictionary(Of String, String).Insert
            Me.BaseDic.Insert(index, key, value)
            Me.Collection_Changed()
        End Sub

        Public Sub RemoveAt(index As Integer) Implements IOrderedDictionary.RemoveAt, IList.RemoveAt, IList(Of KeyValuePair(Of String, String)).RemoveAt
            Me.BaseDic.RemoveAt(index)
            Me.Collection_Changed()
        End Sub

        Public Overrides Function ContainsKey(key As String) As Boolean
            Return Me.BaseDic.ContainsKey(key)
        End Function

        Public Function IndexOf(key As String) As Integer
            Return Me.BaseDic.IndexOf(key)
        End Function

        Public Overrides Function Remove(key As String) As Boolean
            Return Me.BaseDic.Remove(key)
            Me.Collection_Changed()
        End Function

        Public Overrides Function TryGetValue(key As String, ByRef value As String) As Boolean
            Return Me.BaseDic.TryGetValue(key, value)
        End Function

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of String, String))
            Return Me.BaseDic.GetEnumerator()
        End Function

        Protected Overrides Function IEnumerator_1_GetEnumerator() As IEnumerator(Of KeyValuePair(Of String, String))
            Return Me.GetEnumerator()
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

#Region "Junk"
        Private Property IList_ItemAt(index As Integer) As Object Implements IList.Item, IOrderedDictionary.Item
            Get
                Return Me.ItemAt(index)
            End Get
            Set(value As Object)
                Me.ItemAt(index) = DirectCast(value, KeyValuePair(Of String, String))
            End Set
        End Property

        Private Function IList_Add(value As Object) As Integer Implements IList.Add
            Me.ICollection_Add(DirectCast(value, KeyValuePair(Of String, String)))
            Return Me.Count - 1
        End Function

        Private Sub IList_Insert(index As Integer, item As KeyValuePair(Of String, String)) Implements IList(Of KeyValuePair(Of String, String)).Insert
            Me.Insert(index, item.Key, item.Value)
        End Sub

        Private Sub IList_Insert(index As Integer, value As Object) Implements IList.Insert
            Me.IList_Insert(index, DirectCast(value, KeyValuePair(Of String, String)))
        End Sub

        Private Sub IOrderedDictionary_Insert(index As Integer, key As Object, value As Object) Implements IOrderedDictionary.Insert
            Me.Insert(index, DirectCast(key, String), DirectCast(value, String))
        End Sub

        Private Sub IList_Remove(value As Object) Implements IList.Remove
            Me.ICollection_Remove(DirectCast(value, KeyValuePair(Of String, String)))
        End Sub

        Private Function IList_IndexOf(item As KeyValuePair(Of String, String)) As Integer Implements IList(Of KeyValuePair(Of String, String)).IndexOf
            Dim R = Me.IndexOf(item.Key)
            Dim T = Me.ItemAt(R).Value

            If R = -1 Then
                Return -1
            End If
            If Not Object.Equals(item.Value, T) Then
                Return -1
            End If
            Return R
        End Function

        Private Function IList_IndexOf(value As Object) As Integer Implements IList.IndexOf
            Return Me.IList_IndexOf(DirectCast(value, KeyValuePair(Of String, String)))
        End Function

        Private Function IOrderedDictionary_GetEnumerator() As IDictionaryEnumerator Implements IOrderedDictionary.GetEnumerator
            Return Me.IDictionary_GetEnumerator()
        End Function

        Private Function IList_Contains(value As Object) As Boolean Implements IList.Contains
            Return Me.ICollection_Contains(DirectCast(value, KeyValuePair(Of String, String)))
        End Function
#End Region

#Region "IDisposable Support"
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me._IsDisposed Then
                Me._IsDisposed = True

                ' This is more critical to be given over to the non-reliable GC.
                Me.TaskDelayer.Dispose()

                If disposing Then
                    ' Dispose managed state (managed objects).
                    If Not Me.LeaveOpen Then
                        Me.Stream.Dispose()
                    End If
                End If

                ' Set large fields to null.
                Me.BaseDic = Nothing
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Me.Dispose(True)
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

#Region "LeaveOpen Property"
        Private ReadOnly _LeaveOpen As Boolean

        Public ReadOnly Property LeaveOpen As Boolean
            Get
                Return Me._LeaveOpen
            End Get
        End Property
#End Region

        Private BaseDic As ConcurrentOrderedDictionary(Of String, String)
        Private ReadOnly Stream As IO.Stream
        Private ReadOnly TaskDelayer As TaskDelayer = New TaskDelayer(AddressOf Me.Store, TimeSpan.FromSeconds(10))

    End Class

End Namespace
