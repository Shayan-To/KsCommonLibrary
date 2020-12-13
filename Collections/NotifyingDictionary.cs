// Namespace Common

// Public Class NotifyingDictionary(Of TKey, TValue)
// Implements IDictionary(Of TKey, TValue),
// INotifyCollectionChanged(Of KeyValuePair(Of TKey, TValue))

// Public Sub New(ByVal Dic As IDictionary(Of TKey, TValue))
// Me.Dic = Dic
// End Sub

// Public Sub Add(item As KeyValuePair(Of TKey, TValue)) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Add

// End Sub

// Public Sub Clear() Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Clear

// End Sub

// Public Function Contains(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Contains

// End Function

// Public Sub CopyTo(array() As KeyValuePair(Of TKey, TValue), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).CopyTo

// End Sub

// Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Count
// Get

// End Get
// End Property

// Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).IsReadOnly
// Get

// End Get
// End Property

// Public Function Remove(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Remove

// End Function

// Public Sub Add1(key As TKey, value As TValue) Implements IDictionary(Of TKey, TValue).Add

// End Sub

// Public Function ContainsKey(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).ContainsKey

// End Function

// Default Public Property Item(key As TKey) As TValue Implements IDictionary(Of TKey, TValue).Item
// Get
// Return Me.Dic.Item(key)
// End Get
// Set(value As TValue)
// If Me.Dic.ContainsKey(key) Then

// End If
// End Set
// End Property

// Public ReadOnly Property Keys As ICollection(Of TKey) Implements IDictionary(Of TKey, TValue).Keys
// Get
// Return Me.Dic.Keys
// End Get
// End Property

// Public Function Remove(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).Remove
// Dim value As TValue

// If Me.Dic.TryGetValue(key, value) Then
// Me.Dic.Remove(key)
// Me.OnCollectionChanged(New NotifyCollectionChangedEventArgs(Of KeyValuePair(Of TKey, TValue))(Specialized.NotifyCollectionChangedAction.Remove, New KeyValuePair(Of TKey, TValue)(key, value)))
// Return True
// End If

// Return False
// End Function

// Public Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean Implements IDictionary(Of TKey, TValue).TryGetValue
// Return Me.Dic.TryGetValue(key, value)
// End Function

// Public ReadOnly Property Values As ICollection(Of TValue) Implements IDictionary(Of TKey, TValue).Values
// Get
// Return Me.Dic.Values
// End Get
// End Property

// Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IEnumerable(Of KeyValuePair(Of TKey, TValue)).GetEnumerator
// Return Me.Dic.GetEnumerator()
// End Function

// Private Function GetEnumerator_NonGeneric() As IEnumerator Implements IEnumerable.GetEnumerator
// Return Me.GetEnumerator()
// End Function

// Public Event CollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs(Of KeyValuePair(Of TKey, TValue))) Implements INotifyCollectionChanged(Of KeyValuePair(Of TKey, TValue)).CollectionChanged

// Private Event CollectionChanged_NonGeneric(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs) Implements Specialized.INotifyCollectionChanged.CollectionChanged

// Protected Overridable Sub OnCollectionChanged(ByVal E As NotifyCollectionChangedEventArgs(Of KeyValuePair(Of TKey, TValue)))
// RaiseEvent CollectionChanged(Me, E)
// RaiseEvent CollectionChanged_NonGeneric(Me, E)
// End Sub

// Private ReadOnly Dic As IDictionary(Of TKey, TValue)

// End Class

// End Namespace

