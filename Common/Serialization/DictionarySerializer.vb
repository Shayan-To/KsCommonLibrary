Public Class DictionarySerializer(Of TKey, TValue)
    Inherits Serializer(Of IEnumerable(Of KeyValuePair(Of TKey, TValue)))

    Public Sub New()
        MyBase.New(NameOf(Dictionary(Of Object, Object)))
    End Sub

    Public Overrides Sub SetT(Formatter As FormatterSetProxy, Obj As IEnumerable(Of KeyValuePair(Of TKey, TValue)))
        Formatter.Set(NameOf(Obj.Count), Obj.Count())
        For Each I In Obj
            Formatter.Set(Nothing, I)
        Next
    End Sub

    Public Overrides Function GetT(Formatter As FormatterGetProxy) As IEnumerable(Of KeyValuePair(Of TKey, TValue))
        Dim Count As Integer
        Count = Formatter.Get(Of Integer)(NameOf(Count))

        Dim R = New Dictionary(Of TKey, TValue)(Count)
        For I As Integer = 0 To Count - 1
            Dim T = Formatter.Get(Of KeyValuePair(Of TKey, TValue))(Nothing)
            R.Add(T.Key, T.Value)
        Next

        Return R
    End Function

End Class

Public Class KeyValuePairSerializer(Of TKey, TValue)
    Inherits Serializer(Of KeyValuePair(Of TKey, TValue))

    Public Sub New()
        MyBase.New(NameOf(KeyValuePair(Of Object, Object)))
    End Sub

    Public Overrides Sub SetT(Formatter As FormatterSetProxy, Obj As KeyValuePair(Of TKey, TValue))
        Formatter.Set(NameOf(Obj.Key), Obj.Key)
        Formatter.Set(NameOf(Obj.Value), Obj.Value)
    End Sub

    Public Overrides Function GetT(Formatter As FormatterGetProxy) As KeyValuePair(Of TKey, TValue)
        Dim Key As TKey
        Key = Formatter.Get(Of TKey)(NameOf(Key))
        Dim Value As TValue
        Value = Formatter.Get(Of TValue)(NameOf(Value))

        Return New KeyValuePair(Of TKey, TValue)(Key, Value)
    End Function

End Class
