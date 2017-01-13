Namespace Common

    Public Class NullExpandingDic

        Public Shared Function Create(Of TKey, TValue)(ByVal Dic As IDictionary(Of TKey, TValue)) As NullExpandingDic(Of TKey, TValue)
            Return New NullExpandingDic(Of TKey, TValue)(Dic)
        End Function

    End Class

    Public Class NullExpandingDic(Of TKey, TValue)
        Implements IDictionary(Of TKey, TValue)

        Public Sub New(ByVal Dic As IDictionary(Of TKey, TValue))
            Me.Dic = Dic
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Count
            Get
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
                Dim R As TValue
                If Me.Dic.TryGetValue(key, R) Then
                    Return R
                End If
                Return Nothing
            End Get
            Set(value As TValue)
                Me.Dic.Item(key) = value
            End Set
        End Property

        Public ReadOnly Property Keys As ICollection(Of TKey) Implements IDictionary(Of TKey, TValue).Keys
            Get
                Return Me.Dic.Keys
            End Get
        End Property

        Public ReadOnly Property Values As ICollection(Of TValue) Implements IDictionary(Of TKey, TValue).Values
            Get
                Return Me.Dic.Values
            End Get
        End Property

        Public Sub Add(item As KeyValuePair(Of TKey, TValue)) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Add
            Me.Dic.Add(item)
        End Sub

        Public Sub Add(key As TKey, value As TValue) Implements IDictionary(Of TKey, TValue).Add
            Me.Dic.Add(key, value)
        End Sub

        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Clear
            Me.Dic.Clear()
        End Sub

        Public Sub CopyTo(array() As KeyValuePair(Of TKey, TValue), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).CopyTo
            Me.Dic.CopyTo(array, arrayIndex)
        End Sub

        Public Function Contains(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Contains
            Return Me.Dic.Contains(item)
        End Function

        Public Function ContainsKey(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).ContainsKey
            Return Me.Dic.ContainsKey(key)
        End Function

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IEnumerable(Of KeyValuePair(Of TKey, TValue)).GetEnumerator
            Return Me.Dic.GetEnumerator()
        End Function

        Public Function Remove(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Remove
            Return Me.Dic.Remove(item)
        End Function

        Public Function Remove(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).Remove
            Return Me.Dic.Remove(key)
        End Function

        Public Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean Implements IDictionary(Of TKey, TValue).TryGetValue
            value = Me.Item(key)
            Return True
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.Dic.GetEnumerator()
        End Function

        Private ReadOnly Dic As IDictionary(Of TKey, TValue)

    End Class

End Namespace
