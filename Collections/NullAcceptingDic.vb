Public Class NullAcceptingDic

    Public Shared Function Create(Of TKey As Class, TValue)(ByVal Dic As IDictionary(Of TKey, TValue)) As NullAcceptingDic(Of TKey, TValue)
        Return New NullAcceptingDic(Of TKey, TValue)(Dic)
    End Function

End Class

Public Class NullAcceptingDic(Of TKey As Class, TValue)
    Implements IDictionary(Of TKey, TValue)

    Public Sub New(ByVal Dic As IDictionary(Of TKey, TValue))
        Me.Dic = Dic
    End Sub

    Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Count
        Get
            If Me.NullValue.HasValue Then
                Return Me.Dic.Count + 1
            End If
            Return Me.Dic.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).IsReadOnly
        Get
            Return Me.Dic.IsReadOnly
        End Get
    End Property

    Default Public Property Item(key As TKey) As TValue Implements IDictionary(Of TKey, TValue).Item
        Get
            If key Is Nothing Then
                If Not Me.NullValue.HasValue Then
                    Throw New KeyNotFoundException()
                End If
                Return Me.NullValue.Value
            End If
            Return Me.Dic.Item(key)
        End Get
        Set(Value As TValue)
            If key Is Nothing Then
                If Me.Dic.IsReadOnly Then
                    Throw New NotSupportedException()
                End If
                Me.NullValue = Value
                Exit Property
            End If
            Me.Dic.Item(key) = Value
        End Set
    End Property

    Public ReadOnly Property Keys As ICollection(Of TKey) Implements IDictionary(Of TKey, TValue).Keys
        Get
            If Not Me.NullValue.HasValue Then
                Return Me.Dic.Keys
            End If
            Me.KeyList(0) = Nothing
            Return New MergedCollection(Of TKey)(Me.Dic.Keys, Me.KeyList)
        End Get
    End Property

    Public ReadOnly Property Values As ICollection(Of TValue) Implements IDictionary(Of TKey, TValue).Values
        Get
            If Not Me.NullValue.HasValue Then
                Return Me.Dic.Values
            End If
            Me.ValueList(0) = Me.NullValue.Value
            Return New MergedCollection(Of TValue)(Me.Dic.Values, Me.ValueList)
        End Get
    End Property

    Public Sub Add(item As KeyValuePair(Of TKey, TValue)) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Add
        If item.Key Is Nothing Then
            If Me.Dic.IsReadOnly Then
                Throw New NotSupportedException()
            End If
            Me.NullValue = item.Value
            Exit Sub
        End If
        Me.Dic.Add(item)
    End Sub

    Public Sub Add(key As TKey, value As TValue) Implements IDictionary(Of TKey, TValue).Add
        If key Is Nothing Then
            If Me.Dic.IsReadOnly Then
                Throw New NotSupportedException()
            End If
            Me.NullValue = value
            Exit Sub
        End If
        Me.Dic.Add(key, value)
    End Sub

    Public Sub Clear() Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Clear
        If Me.Dic.IsReadOnly Then
            Throw New NotSupportedException()
        End If
        Me.NullValue = Nothing
        Me.Dic.Clear()
    End Sub

    Public Sub CopyTo(array() As KeyValuePair(Of TKey, TValue), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).CopyTo
        If Me.NullValue.HasValue Then
            array(arrayIndex) = New KeyValuePair(Of TKey, TValue)(Nothing, Me.NullValue.Value)
            arrayIndex += 1
        End If
        Me.Dic.CopyTo(array, arrayIndex)
    End Sub

    Public Function Contains(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Contains
        If item.Key Is Nothing Then
            Return Me.NullValue.HasValue AndAlso Me.NullValue.Value.Equals(item.Value)
        End If
        Return Me.Dic.Contains(item)
    End Function

    Public Function ContainsKey(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).ContainsKey
        If key Is Nothing Then
            Return Me.NullValue.HasValue
        End If
        Return Me.Dic.ContainsKey(key)
    End Function

    Private Iterator Function GetEnumeratorWithNull() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
        Yield New KeyValuePair(Of TKey, TValue)(Nothing, Me.NullValue.Value)
        For Each KV In Me.Dic
            Yield KV
        Next
    End Function

    Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IEnumerable(Of KeyValuePair(Of TKey, TValue)).GetEnumerator
        If Not Me.NullValue.HasValue Then
            Return Me.Dic.GetEnumerator()
        End If
        Return Me.GetEnumeratorWithNull()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function

    Public Function Remove(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Remove
        If item.Key Is Nothing Then
            If Me.Dic.IsReadOnly Then
                Throw New NotSupportedException()
            End If
            If Not Me.NullValue.HasValue Then
                Return False
            End If
            If Me.NullValue.Value.Equals(item.Value) Then
                Me.NullValue = Nothing
                Return True
            End If
            Return False
        End If
        Return Me.Dic.Remove(item)
    End Function

    Public Function Remove(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).Remove
        If key Is Nothing Then
            If Me.Dic.IsReadOnly Then
                Throw New NotSupportedException()
            End If
            If Not Me.NullValue.HasValue Then
                Return False
            End If
            Me.NullValue = Nothing
            Return True
        End If
        Return Me.Dic.Remove(key)
    End Function

    Public Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean Implements IDictionary(Of TKey, TValue).TryGetValue
        If key Is Nothing Then
            If Me.NullValue.HasValue Then
                value = Me.NullValue.Value
                Return True
            End If
            Return False
        End If

        Return Me.Dic.TryGetValue(key, value)
    End Function

    Private ReadOnly Dic As IDictionary(Of TKey, TValue)
    Private NullValue As CNullable(Of TValue)
    Private ReadOnly KeyList As TKey() = New TKey(0) {}
    Private ReadOnly ValueList As TValue() = New TValue(0) {}

End Class
