Namespace Common

    Public Class NullExpandingDictionary

        Public Shared Function Create(Of TKey, TValue)(ByVal Dic As IDictionary(Of TKey, TValue)) As NullExpandingDictionary(Of TKey, TValue)
            Return New NullExpandingDictionary(Of TKey, TValue)(Dic)
        End Function

    End Class

    Public Class NullExpandingDictionary(Of TKey, TValue)
        Inherits BaseDictionary(Of TKey, TValue)

        Public Sub New(ByVal Dic As IDictionary(Of TKey, TValue))
            Me.Dic = Dic
        End Sub

        Public Sub New()
            Me.New(New Dictionary(Of TKey, TValue)())
        End Sub

        Public Overrides ReadOnly Property Count As Integer
            Get
                Return Me.Dic.Count
            End Get
        End Property

        Protected Overrides ReadOnly Property IsReadOnly As Boolean
            Get
                Return Me.Dic.IsReadOnly
            End Get
        End Property

        Default Public Overrides Property Item(key As TKey) As TValue
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

        Public Overrides ReadOnly Property Keys As ICollection(Of TKey)
            Get
                Return Me.Dic.Keys
            End Get
        End Property

        Public Overrides ReadOnly Property Values As ICollection(Of TValue)
            Get
                Return Me.Dic.Values
            End Get
        End Property

        Public Overrides Sub Add(key As TKey, value As TValue)
            Me.Dic.Add(key, value)
        End Sub

        Public Overrides Sub Clear()
            Me.Dic.Clear()
        End Sub

        Public Overrides Function ContainsKey(key As TKey) As Boolean
            Return Me.Dic.ContainsKey(key)
        End Function

        Public Overrides Function Remove(key As TKey) As Boolean
            Return Me.Dic.Remove(key)
        End Function

        Public Overrides Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean
            value = Me.Item(key)
            Return True
        End Function

        Protected Overrides Function IEnumerator_1_GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
            Return Me.GetEnumerator()
        End Function

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
            Return Me.Dic.GetEnumerator()
        End Function

        Private ReadOnly Dic As IDictionary(Of TKey, TValue)

    End Class

End Namespace
