Namespace Common

    Public Class PartialReadOnlyDictionary(Of TKey, TValue)
        Inherits BaseDictionary(Of TKey, TValue)

        Public Sub New(ByVal BaseDic As IDictionary(Of TKey, TValue))
            Me.BaseDic = BaseDic
        End Sub

        Public Sub LockCurrentElements()
            For Each K In Me.Keys
                Me.LockedKeys.Add(K)
            Next
        End Sub

        Public Sub ResetLock()
            Me.LockedKeys.Clear()
        End Sub

        Public Overrides ReadOnly Property Count As Integer
            Get
                Return Me.BaseDic.Count
            End Get
        End Property

        Default Public Overrides Property Item(key As TKey) As TValue
            Get
                Return Me.BaseDic.Item(key)
            End Get
            Set(value As TValue)
                Verify.False(Me.LockedKeys.Contains(key), "Key is locked.")
                Me.BaseDic.Item(key) = value
            End Set
        End Property

        Public Overrides ReadOnly Property Keys As ICollection(Of TKey)
            Get
                Return Me.BaseDic.Keys
            End Get
        End Property

        Public Overrides ReadOnly Property Values As ICollection(Of TValue)
            Get
                Return Me.BaseDic.Values
            End Get
        End Property

        Public Overrides Sub Add(key As TKey, value As TValue)
            Me.BaseDic.Add(key, value)
        End Sub

        Public Overrides Sub Clear()
            Dim CurrentState = Me.BaseDic.Where(Function(KV) Me.LockedKeys.Contains(KV.Key)).ToArray()
            Me.BaseDic.Clear()
            For Each KV In CurrentState
                Me.BaseDic.Add(KV.Key, KV.Value)
            Next
        End Sub

        Public Overrides Function ContainsKey(key As TKey) As Boolean
            Return Me.BaseDic.ContainsKey(key)
        End Function

        Public Overrides Function Remove(key As TKey) As Boolean
            Verify.False(Me.LockedKeys.Contains(key), "Key is locked.")
            Return Me.BaseDic.Remove(key)
        End Function

        Public Overrides Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean
            Return Me.BaseDic.TryGetValue(key, value)
        End Function

        Protected Overrides Function IEnumerator_1_GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
            Return Me.GetEnumerator()
        End Function

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
            Return Me.BaseDic.GetEnumerator()
        End Function

        Private ReadOnly BaseDic As IDictionary(Of TKey, TValue)
        Private ReadOnly LockedKeys As HashSet(Of TKey) = New HashSet(Of TKey)()

    End Class

End Namespace
