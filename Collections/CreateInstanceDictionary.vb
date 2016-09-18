Public NotInheritable Class CreateInstanceDictionary

    Private Sub New()
        Throw New NotSupportedException()
    End Sub

    Public Shared Function Create(Of TKey, TValue As New)(ByVal Dic As IDictionary(Of TKey, TValue)) As CreateInstanceDictionary(Of TKey, TValue)
        Return New CreateInstanceDictionary(Of TKey, TValue)(Dic)
    End Function

End Class

Public Class CreateInstanceDictionary(Of TKey, TValue As New)
    Implements IDictionary(Of TKey, TValue)

    Public Sub New(ByVal Dic As IDictionary(Of TKey, TValue))
        Me.Dic = Dic
    End Sub

    Public Sub New()
        Me.New(New Dictionary(Of TKey, TValue)())
    End Sub

    Public Sub Add(ByVal item As KeyValuePair(Of TKey, TValue)) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Add
        Me.Dic.Add(item)
    End Sub

    Public Sub Clear() Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Clear
        Me.Dic.Clear()
    End Sub

    Public Function Contains(ByVal item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Contains
        Return Me.Dic.Contains(item)
    End Function

    Public Sub CopyTo(ByVal array() As KeyValuePair(Of TKey, TValue), ByVal arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).CopyTo
        Me.Dic.CopyTo(array, arrayIndex)
    End Sub

    Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Count
        Get
            Return Me.Dic.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).IsReadOnly
        Get
            Return False
        End Get
    End Property

    Public Function Remove(ByVal item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Remove
        Return Me.Dic.Remove(item)
    End Function

    Public Sub Add(ByVal key As TKey, ByVal value As TValue) Implements IDictionary(Of TKey, TValue).Add
        Me.Dic.Add(key, value)
    End Sub

    Public Function ContainsKey(ByVal key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).ContainsKey
        Return Me.Dic.ContainsKey(key)
    End Function

    Default Public Property Item(ByVal key As TKey) As TValue Implements IDictionary(Of TKey, TValue).Item
        Get
            Dim V As TValue
            If Not Me.Dic.TryGetValue(key, V) Then
                V = New TValue()
                Me.Dic.Add(key, V)
            End If
            Return V
        End Get
        Set(ByVal value As TValue)
            Me.Dic.Item(key) = value
        End Set
    End Property

    Public ReadOnly Property Keys As ICollection(Of TKey) Implements IDictionary(Of TKey, TValue).Keys
        Get
            Return Me.Dic.Keys
        End Get
    End Property

    Public Function Remove(ByVal key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).Remove
        Return Me.Dic.Remove(key)
    End Function

    Public Function TryGetValue(ByVal key As TKey, ByRef value As TValue) As Boolean Implements IDictionary(Of TKey, TValue).TryGetValue
        Return Me.Dic.TryGetValue(key, value)
    End Function

    Public ReadOnly Property Values As ICollection(Of TValue) Implements IDictionary(Of TKey, TValue).Values
        Get
            Return Me.Dic.Values
        End Get
    End Property

    Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IEnumerable(Of System.Collections.Generic.KeyValuePair(Of TKey, TValue)).GetEnumerator
        Return Me.Dic.GetEnumerator()
    End Function

    Private Function GetEnumerator_NonGeneric() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function

    Private ReadOnly Dic As IDictionary(Of TKey, TValue)

End Class
