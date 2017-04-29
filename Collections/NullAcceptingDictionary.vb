Namespace Common

    Public Class NullAcceptingDictionary

        Public Shared Function Create(Of TKey As Class, TValue)(ByVal Dic As IDictionary(Of TKey, TValue)) As NullAcceptingDictionary(Of TKey, TValue)
            Return New NullAcceptingDictionary(Of TKey, TValue)(Dic)
        End Function

    End Class

    Public Class NullAcceptingDictionary(Of TKey As Class, TValue)
        Inherits BaseDictionary(Of TKey, TValue)

        Public Sub New(ByVal Dic As IDictionary(Of TKey, TValue))
            Me.Dic = Dic
            Me.KeysCollection = New MergedCollection(Of TKey)(New TKey(0) {Nothing}, Me.Dic.Keys)
            Me.ValuesCollection = New MergedCollection(Of TValue)(Me.NullValue, Me.Dic.Values)
        End Sub

        Public Sub New()
            Me.New(New Dictionary(Of TKey, TValue)())
        End Sub

        Protected Overrides ReadOnly Property IsReadOnly As Boolean
            Get
                Return Me.Dic.IsReadOnly
            End Get
        End Property

        Public Overrides ReadOnly Property Count As Integer
            Get
                If Me.HasNullValue Then
                    Return Me.Dic.Count + 1
                End If
                Return Me.Dic.Count
            End Get
        End Property

        Default Public Overrides Property Item(key As TKey) As TValue
            Get
                If key Is Nothing Then
                    If Not Me.HasNullValue Then
                        Throw New KeyNotFoundException()
                    End If
                    Return Me.NullValue(0)
                End If
                Return Me.Dic.Item(key)
            End Get
            Set(value As TValue)
                If key Is Nothing Then
                    If Me.IsReadOnly Then
                        Throw New NotSupportedException("Collection is read only.")
                    End If
                    Me.NullValue(0) = value
                    Me.HasNullValue = True
                    Exit Property
                End If
                Me.Dic.Item(key) = value
            End Set
        End Property

        Public Overrides ReadOnly Property Keys As ICollection(Of TKey)
            Get
                If Not Me.HasNullValue Then
                    Return Me.Dic.Keys
                End If
                Return Me.KeysCollection
            End Get
        End Property

        Public Overrides ReadOnly Property Values As ICollection(Of TValue)
            Get
                If Not Me.HasNullValue Then
                    Return Me.Dic.Values
                End If
                Return Me.ValuesCollection
            End Get
        End Property

        Protected Overrides Sub CopyTo(ByVal Array As Array, ByVal ArrayIndex As Integer)
            If Me.HasNullValue Then
                Array.SetValue(New KeyValuePair(Of TKey, TValue)(Nothing, Me.NullValue(0)), ArrayIndex)
                ArrayIndex += 1
            End If

            Dim Dic = TryCast(Me.Dic, IDictionary)
            If Dic IsNot Nothing Then
                Dic.CopyTo(Array, ArrayIndex)
                Exit Sub
            End If

            For Each KV In Dic
                Array.SetValue(KV, ArrayIndex)
                ArrayIndex += 1
            Next
        End Sub

        Public Overrides Sub Add(key As TKey, value As TValue)
            If key Is Nothing Then
                If Me.IsReadOnly Then
                    Throw New NotSupportedException()
                End If
                Me.NullValue(0) = value
                Me.HasNullValue = True
                Exit Sub
            End If
            Me.Dic.Add(key, value)
        End Sub

        Public Overrides Sub Clear()
            If Me.IsReadOnly Then
                Throw New NotSupportedException()
            End If
            Me.NullValue(0) = Nothing
            Me.HasNullValue = False
            Me.Dic.Clear()
        End Sub

        Public Overrides Function ContainsKey(key As TKey) As Boolean
            If key Is Nothing Then
                Return Me.HasNullValue
            End If
            Return Me.Dic.ContainsKey(key)
        End Function

        Public Overrides Function Remove(key As TKey) As Boolean
            If key Is Nothing Then
                If Me.IsReadOnly Then
                    Throw New NotSupportedException()
                End If
                If Not Me.HasNullValue Then
                    Return False
                End If
                Me.NullValue(0) = Nothing
                Me.HasNullValue = False
                Return True
            End If
            Return Me.Dic.Remove(key)
        End Function

        Public Overrides Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean
            If key Is Nothing Then
                If Me.HasNullValue Then
                    value = Me.NullValue(0)
                    Return True
                End If
                Return False
            End If

            Return Me.Dic.TryGetValue(key, value)
        End Function

        Private Iterator Function GetEnumeratorWithNull() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
            Yield New KeyValuePair(Of TKey, TValue)(Nothing, Me.NullValue(0))
            For Each KV In Me.Dic
                Yield KV
            Next
        End Function

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
            If Me.HasNullValue Then
                Return Me.GetEnumeratorWithNull()
            End If
            Return Me.Dic.GetEnumerator()
        End Function

        Protected Overrides Function IEnumerator_1_GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
            Return Me.GetEnumerator()
        End Function

        Private ReadOnly Dic As IDictionary(Of TKey, TValue)
        Private HasNullValue As Boolean
        Private ReadOnly NullValue As TValue() = New TValue(0) {}

        Private ReadOnly KeysCollection As MergedCollection(Of TKey)
        Private ReadOnly ValuesCollection As MergedCollection(Of TValue)

    End Class

End Namespace
