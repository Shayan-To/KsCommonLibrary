Imports System.Collections.Specialized

Namespace Common

    Public Class OrderedDictionary(Of TKey, TValue)
        Inherits BaseDictionary(Of TKey, TValue)
        Implements IOrderedDictionary(Of TKey, TValue)

        ' ToDo Do the current done changes (in this commit) to the other ordered dictionaries.

        Public Sub New()
            Me.New(EqualityComparer(Of TKey).Default)
        End Sub

        Public Sub New(ByVal Comparer As IEqualityComparer(Of TKey))
            Me.Comparer = Comparer
            Me._Dic = New Dictionary(Of TKey, TValue)(Comparer)
        End Sub

        Public Overrides ReadOnly Property Count As Integer Implements IList(Of KeyValuePair(Of TKey, TValue)).Count
            Get
                Return Me._Keys.Count
            End Get
        End Property

        ''' <summary>
        ''' When setting, adds the value to the end of collection if not present.
        ''' </summary>
        Default Public Overrides Property Item(key As TKey) As TValue
            Get
                Return Me._Dic.Item(key)
            End Get
            Set(value As TValue)
                If Not Me._Dic.ContainsKey(key) Then
                    Me._Keys.Add(key)
                End If
                Me._Dic.Item(key) = value
            End Set
        End Property

        Public Overrides ReadOnly Property Keys As ICollection(Of TKey)
            Get
                Return DirectCast(Me.KeysList, ICollection(Of TKey))
            End Get
        End Property

        Public Overrides ReadOnly Property Values As ICollection(Of TValue)
            Get
                Return DirectCast(Me.ValuesList, ICollection(Of TValue))
            End Get
        End Property

        Public ReadOnly Property KeysList As IReadOnlyList(Of TKey)
            Get
                Static R As IReadOnlyList(Of TKey) = Me._Keys.AsReadOnly()
                Return R
            End Get
        End Property

        Public ReadOnly Property ValuesList As IReadOnlyList(Of TValue)
            Get
                Static R As IReadOnlyList(Of TValue) = Me._Keys.SelectAsList(Function(K) Me._Dic.Item(K))
                Return R
            End Get
        End Property

        Public Property ItemAt(index As Integer) As KeyValuePair(Of TKey, TValue) Implements IList(Of KeyValuePair(Of TKey, TValue)).Item
            Get
                Dim Key = Me._Keys.Item(index)
                Return New KeyValuePair(Of TKey, TValue)(Key, Me._Dic.Item(Key))
            End Get
            Set(value As KeyValuePair(Of TKey, TValue))
                Dim Key = Me._Keys.Item(index)

                Assert.True(Me._Dic.Remove(Key))

                Me._Dic.Add(value.Key, value.Value)
                Me._Keys.Item(index) = value.Key
            End Set
        End Property

        Private ReadOnly Property IList_IsReadOnly As Boolean Implements IList.IsReadOnly
            Get
                Return False
            End Get
        End Property

        Private ReadOnly Property IList_IsFixedSize As Boolean Implements IList.IsFixedSize
            Get
                Return False
            End Get
        End Property

        Public Overrides Sub Add(key As TKey, value As TValue)
            Me._Keys.Add(key)
            Me._Dic.Add(key, value)
        End Sub

        Public Overrides Sub Clear() Implements IList.Clear, IList(Of KeyValuePair(Of TKey, TValue)).Clear
            Me._Keys.Clear()
            Me._Dic.Clear()
        End Sub

        Public Sub Insert(index As Integer, key As TKey, value As TValue) Implements IOrderedDictionary(Of TKey, TValue).Insert
            Me._Keys.Insert(index, key)
            Me._Dic.Add(key, value)
        End Sub

        Public Sub RemoveAt(index As Integer) Implements IOrderedDictionary.RemoveAt, IList.RemoveAt, IList(Of KeyValuePair(Of TKey, TValue)).RemoveAt
            Dim Key = Me._Keys.Item(index)
            Me._Keys.RemoveAt(index)
            Assert.True(Me._Dic.Remove(Key))
        End Sub

        Public Overrides Function ContainsKey(key As TKey) As Boolean
            Return Me._Dic.ContainsKey(key)
        End Function

        Public Function IndexOf(key As TKey) As Integer
            For I As Integer = 0 To Me._Keys.Count - 1
                If Me.Comparer.Equals(key, Me._Keys.Item(I)) Then
                    Return I
                End If
            Next
            Return -1
        End Function

        Public Overrides Function Remove(key As TKey) As Boolean
            If Not Me._Dic.Remove(key) Then
                Return False
            End If
            Dim I = Me.IndexOf(key)
            Assert.True(I <> -1)
            Me._Keys.RemoveAt(I)
            Return True
        End Function

        Public Overrides Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean
            Return Me._Dic.TryGetValue(key, value)
        End Function

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
            Return Me._Keys.Select(Function(K) New KeyValuePair(Of TKey, TValue)(K, Me._Dic.Item(K))).GetEnumerator()
        End Function

        Protected Overrides Function IEnumerator_1_GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
            Return Me.GetEnumerator()
        End Function

#Region "Junk"
        Private Property IList_ItemAt(index As Integer) As Object Implements IList.Item, IOrderedDictionary.Item
            Get
                Return Me.ItemAt(index)
            End Get
            Set(value As Object)
                Me.ItemAt(index) = DirectCast(value, KeyValuePair(Of TKey, TValue))
            End Set
        End Property

        Private Function IList_Add(value As Object) As Integer Implements IList.Add
            Me.ICollection_Add(DirectCast(value, KeyValuePair(Of TKey, TValue)))
            Return Me.Count - 1
        End Function

        Private Sub IList_Insert(index As Integer, item As KeyValuePair(Of TKey, TValue)) Implements IList(Of KeyValuePair(Of TKey, TValue)).Insert
            Me.Insert(index, item.Key, item.Value)
        End Sub

        Private Sub IList_Insert(index As Integer, value As Object) Implements IList.Insert
            Me.IList_Insert(index, DirectCast(value, KeyValuePair(Of TKey, TValue)))
        End Sub

        Private Sub IOrderedDictionary_Insert(index As Integer, key As Object, value As Object) Implements IOrderedDictionary.Insert
            Me.Insert(index, DirectCast(key, TKey), DirectCast(value, TValue))
        End Sub

        Private Sub IList_Remove(value As Object) Implements IList.Remove
            Me.ICollection_Remove(DirectCast(value, KeyValuePair(Of TKey, TValue)))
        End Sub

        Private Function IList_IndexOf(item As KeyValuePair(Of TKey, TValue)) As Integer Implements IList(Of KeyValuePair(Of TKey, TValue)).IndexOf
            Dim R = Me.IndexOf(item.Key)
            If R = -1 Then
                Return -1
            End If
            If Not Object.Equals(item.Value, Me.ItemAt(R).Value) Then
                Return -1
            End If
            Return R
        End Function

        Private Function IList_IndexOf(value As Object) As Integer Implements IList.IndexOf
            Return Me.IList_IndexOf(DirectCast(value, KeyValuePair(Of TKey, TValue)))
        End Function

        Private Function IOrderedDictionary_GetEnumerator() As IDictionaryEnumerator Implements IOrderedDictionary.GetEnumerator
            Return Me.IDictionary_GetEnumerator()
        End Function

        Private Function IList_Contains(value As Object) As Boolean Implements IList.Contains
            Return Me.ICollection_Contains(DirectCast(value, KeyValuePair(Of TKey, TValue)))
        End Function
#End Region

        Private ReadOnly Comparer As IEqualityComparer(Of TKey)
        Private ReadOnly _Dic As Dictionary(Of TKey, TValue)
        Private ReadOnly _Keys As List(Of TKey) = New List(Of TKey)()

    End Class

End Namespace
