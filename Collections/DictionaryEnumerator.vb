Namespace Common

    Public Structure DictionaryEnumerator(Of TKey, TValue, T As IEnumerator(Of KeyValuePair(Of TKey, TValue)))
        Implements IEnumerator(Of KeyValuePair(Of TKey, TValue)), IDictionaryEnumerator

        Public Sub New(ByVal BaseEnumerator As T)
            Me.BaseEnumerator = BaseEnumerator
        End Sub

        Public ReadOnly Property Current As KeyValuePair(Of TKey, TValue) Implements IEnumerator(Of KeyValuePair(Of TKey, TValue)).Current
            Get
                Return Me.BaseEnumerator.Current
            End Get
        End Property

        Public ReadOnly Property Entry As DictionaryEntry Implements IDictionaryEnumerator.Entry
            Get
                Dim Current = Me.Current
                Return New DictionaryEntry(Current.Key, Current.Value)
            End Get
        End Property

        Public ReadOnly Property Key As Object Implements IDictionaryEnumerator.Key
            Get
                Return Me.Current.Key
            End Get
        End Property

        Public ReadOnly Property Value As Object Implements IDictionaryEnumerator.Value
            Get
                Return Me.Current.Value
            End Get
        End Property

        Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
            Get
                Return Me.Current
            End Get
        End Property

        Public Sub Dispose() Implements IDisposable.Dispose
            Me.BaseEnumerator.Dispose()
        End Sub

        Public Sub Reset() Implements IEnumerator.Reset
            Me.BaseEnumerator.Reset()
        End Sub

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            Return Me.BaseEnumerator.MoveNext()
        End Function

        Private ReadOnly BaseEnumerator As T

    End Structure

End Namespace
