Namespace Common

    Public Class NullAcceptingDic

        Public Shared Function Create(Of TKey As Class, TValue)(ByVal Dic As IDictionary(Of TKey, TValue)) As NullAcceptingDic(Of TKey, TValue)
            Return New NullAcceptingDic(Of TKey, TValue)(Dic)
        End Function

    End Class

    Public Class NullAcceptingDic(Of TKey As Class, TValue)
        Implements IDictionary(Of TKey, TValue),
               IDictionary

        Public Sub New(ByVal Dic As IDictionary(Of TKey, TValue))
            Me.Dic = Dic
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Count, ICollection.Count
            Get
                If Me.NullValue.HasValue Then
                    Return Me.Dic.Count + 1
                End If
                Return Me.Dic.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).IsReadOnly, IDictionary.IsReadOnly
            Get
                Return Me.Dic.IsReadOnly
            End Get
        End Property

        Default Public Property Item(ByVal Key As TKey) As TValue Implements IDictionary(Of TKey, TValue).Item
            Get
                If Key Is Nothing Then
                    If Not Me.NullValue.HasValue Then
                        Throw New KeyNotFoundException()
                    End If
                    Return Me.NullValue.Value
                End If
                Return Me.Dic.Item(Key)
            End Get
            Set(ByVal Value As TValue)
                If Key Is Nothing Then
                    If Me.Dic.IsReadOnly Then
                        Throw New NotSupportedException("Collection is read only.")
                    End If
                    Me.NullValue = Value
                    Exit Property
                End If
                Me.Dic.Item(Key) = Value
            End Set
        End Property

        Public ReadOnly Property Keys As ICollection(Of TKey) Implements IDictionary(Of TKey, TValue).Keys
            Get
                ' ToDo Cache the MergedCollection. Why is the dic cached, but not the collection??
                If Not Me.NullValue.HasValue Then
                    Return Me.Dic.Keys
                End If
                Me.KeyList(0) = Nothing
                Return New MergedCollection(Of TKey)(Me.KeyList, Me.Dic.Keys)
            End Get
        End Property

        Public ReadOnly Property Values As ICollection(Of TValue) Implements IDictionary(Of TKey, TValue).Values
            Get
                ' ToDo Cache the MergedCollection. Why is the dic cached, but not the collection??
                If Not Me.NullValue.HasValue Then
                    Return Me.Dic.Values
                End If
                Me.ValueList(0) = Me.NullValue.Value
                Return New MergedCollection(Of TValue)(Me.ValueList, Me.Dic.Values)
            End Get
        End Property

        Private Property IDictionary_Item(key As Object) As Object Implements IDictionary.Item
            Get
                Return Me.Item(DirectCast(key, TKey))
            End Get
            Set(value As Object)
                Me.Item(DirectCast(key, TKey)) = DirectCast(value, TValue)
            End Set
        End Property

        Private ReadOnly Property IDictionary_Keys As ICollection Implements IDictionary.Keys
            Get
                Return DirectCast(Me.Keys, MergedCollection(Of TKey))
            End Get
        End Property

        Private ReadOnly Property IDictionary_Values As ICollection Implements IDictionary.Values
            Get
                Return DirectCast(Me.Values, MergedCollection(Of TValue))
            End Get
        End Property

        Private ReadOnly Property IsFixedSize As Boolean Implements IDictionary.IsFixedSize
            Get
                Return False
            End Get
        End Property

        Private ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Private ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
            Get
                Return False
            End Get
        End Property

        Private Sub Add(ByVal Item As KeyValuePair(Of TKey, TValue)) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Add
            If Item.Key Is Nothing Then
                If Me.Dic.IsReadOnly Then
                    Throw New NotSupportedException()
                End If
                Me.NullValue = Item.Value
                Exit Sub
            End If
            Me.Dic.Add(Item)
        End Sub

        Public Sub Add(ByVal Key As TKey, ByVal Value As TValue) Implements IDictionary(Of TKey, TValue).Add
            If Key Is Nothing Then
                If Me.Dic.IsReadOnly Then
                    Throw New NotSupportedException()
                End If

                Me.NullValue = Value
                Exit Sub
            End If
            Me.Dic.Add(Key, Value)
        End Sub

        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Clear, IDictionary.Clear
            If Me.Dic.IsReadOnly Then
                Throw New NotSupportedException()
            End If
            Me.NullValue = Nothing
            Me.Dic.Clear()
        End Sub

        Public Sub CopyTo(ByVal Array() As KeyValuePair(Of TKey, TValue), ByVal ArrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).CopyTo
            If Me.NullValue.HasValue Then
                Array(ArrayIndex) = New KeyValuePair(Of TKey, TValue)(Nothing, Me.NullValue.Value)
                ArrayIndex += 1
            End If
            Me.Dic.CopyTo(Array, ArrayIndex)
        End Sub

        Private Function Contains(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Contains
            If item.Key Is Nothing Then
                Return Me.NullValue.HasValue AndAlso Object.Equals(Me.NullValue.Value, item.Value)
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
            If Me.NullValue.HasValue Then
                Return Me.GetEnumeratorWithNull()
            End If
            Return Me.Dic.GetEnumerator()
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function

        Private Function Remove(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Remove
            If item.Key Is Nothing Then
                If Me.Dic.IsReadOnly Then
                    Throw New NotSupportedException()
                End If
                If Not Me.NullValue.HasValue Then
                    Return False
                End If
                If Object.Equals(Me.NullValue.Value, item.Value) Then
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

        Private Function Contains(key As Object) As Boolean Implements IDictionary.Contains
            Return Me.Contains(DirectCast(key, TKey))
        End Function

        Private Sub Add(key As Object, value As Object) Implements IDictionary.Add
            Me.Add(DirectCast(key, TKey), DirectCast(value, TValue))
        End Sub

        Private Function IDictionary_GetEnumerator() As IDictionaryEnumerator Implements IDictionary.GetEnumerator
            Throw New NotSupportedException()
        End Function

        Private Sub Remove(key As Object) Implements IDictionary.Remove
            Me.Remove(DirectCast(key, TKey))
        End Sub

        Private Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo

        End Sub

        Private ReadOnly Dic As IDictionary(Of TKey, TValue)
        Private NullValue As CNullable(Of TValue)
        Private ReadOnly KeyList As TKey() = New TKey(0) {}
        Private ReadOnly ValueList As TValue() = New TValue(0) {}

    End Class

End Namespace
