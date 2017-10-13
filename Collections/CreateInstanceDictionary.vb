Namespace Common

    Public NotInheritable Class CreateInstanceDictionary

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        Public Shared Function Create(Of TKey, TValue)(ByVal Dic As IDictionary(Of TKey, TValue), ByVal Creator As Func(Of TKey, TValue)) As CreateInstanceDictionary(Of TKey, TValue)
            Return New CreateInstanceDictionary(Of TKey, TValue)(Dic, Creator)
        End Function

        Public Shared Function Create(Of TKey, TValue)(ByVal Creator As Func(Of TKey, TValue)) As CreateInstanceDictionary(Of TKey, TValue)
            Return New CreateInstanceDictionary(Of TKey, TValue)(Creator)
        End Function

        <Obsolete("Use other overloads.", True)>
        Public Shared Function Create(Of TKey, TValue)(ByVal Dic As IDictionary(Of TKey, TValue), ByVal Creator As Func(Of TValue)) As CreateInstanceDictionary(Of TKey, TValue)
            Return New CreateInstanceDictionary(Of TKey, TValue)(Dic, Function(K) Creator.Invoke())
        End Function

        <Obsolete("Use other overloads.", True)>
        Public Shared Function Create(Of TKey, TValue)(ByVal Creator As Func(Of TValue)) As CreateInstanceDictionary(Of TKey, TValue)
            Return New CreateInstanceDictionary(Of TKey, TValue)(Function(K) Creator.Invoke())
        End Function

        Public Shared Function Create(Of TKey, TValue As New)(ByVal Dic As IDictionary(Of TKey, TValue)) As CreateInstanceDictionary(Of TKey, TValue)
            Return New CreateInstanceDictionary(Of TKey, TValue)(Dic, Function() New TValue())
        End Function

        Public Shared Function Create(Of TKey, TValue As New)() As CreateInstanceDictionary(Of TKey, TValue)
            Return New CreateInstanceDictionary(Of TKey, TValue)(Function() New TValue())
        End Function

    End Class

    Public Class CreateInstanceDictionary(Of TKey, TValue)
        Inherits BaseDictionary(Of TKey, TValue)

        Public Sub New(ByVal Dic As IDictionary(Of TKey, TValue), ByVal Creator As Func(Of TKey, TValue))
            Me.Dic = Dic
            Me.Creator = Creator
        End Sub

        Public Sub New(ByVal Creator As Func(Of TKey, TValue))
            Me.New(New Dictionary(Of TKey, TValue)(), Creator)
        End Sub

        Public Overrides Sub Clear()
            Me.Dic.Clear()
        End Sub

        Public Overrides ReadOnly Property Count As Integer
            Get
                Return Me.Dic.Count
            End Get
        End Property

        Public Overrides Sub Add(key As TKey, value As TValue)
            Me.Dic.Add(key, value)
        End Sub

        Public Overrides Function ContainsKey(key As TKey) As Boolean
            Return Me.Dic.ContainsKey(key)
        End Function

        Default Public Overrides Property Item(key As TKey) As TValue
            Get
                Dim V As TValue
                If Not Me.Dic.TryGetValue(key, V) Then
                    V = Me.Creator.Invoke(key)
                    Me.Dic.Add(key, V)
                End If
                Return V
            End Get
            Set(ByVal value As TValue)
                Me.Dic.Item(key) = value
            End Set
        End Property

        Public Overrides ReadOnly Property Keys As ICollection(Of TKey)
            Get
                Return Me.Dic.Keys
            End Get
        End Property

        Public Overrides Function Remove(key As TKey) As Boolean
            Return Me.Dic.Remove(key)
        End Function

        Public Overrides Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean
            Return Me.Dic.TryGetValue(key, value)
        End Function

        Public Overrides ReadOnly Property Values As ICollection(Of TValue)
            Get
                Return Me.Dic.Values
            End Get
        End Property

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
            Return Me.Dic.GetEnumerator()
        End Function

        Protected Overrides Function IEnumerator_1_GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
            Return Me.GetEnumerator()
        End Function

        Private ReadOnly Creator As Func(Of TKey, TValue)
        Private ReadOnly Dic As IDictionary(Of TKey, TValue)

    End Class

End Namespace
