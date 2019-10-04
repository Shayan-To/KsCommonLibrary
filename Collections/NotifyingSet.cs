// ToDo

// Imports System.Collections.Specialized

// Namespace Common

// Public Class ObservableDictionary(Of T)
// Implements ISet(Of T),
// INotifyCollectionChanged

// Private ReadOnly InnerSet As ISet(Of T)

// Public Sub New()
// Me.InnerSet = New SortedSet(Of T)()
// End Sub

// Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count
// Get
// Return Me.InnerSet.Count
// End Get
// End Property

// Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
// Get
// Return Me.InnerSet.IsReadOnly
// End Get
// End Property

// Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

// Protected Sub OnCollectionChanged(ByVal E As NotifyCollectionChangedEventArgs)
// RaiseEvent CollectionChanged(Me, E)
// End Sub

// Public Sub Clear() Implements ICollection(Of T).Clear
// Me.InnerSet.Clear()
// End Sub

// Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
// Me.InnerSet.CopyTo(array, arrayIndex)
// End Sub

// Public Sub ExceptWith(other As IEnumerable(Of T)) Implements ISet(Of T).ExceptWith
// Me.InnerSet.ExceptWith(other)
// End Sub

// Public Sub IntersectWith(other As IEnumerable(Of T)) Implements ISet(Of T).IntersectWith
// Me.InnerSet.IntersectWith(other)
// End Sub

// Public Sub SymmetricExceptWith(other As IEnumerable(Of T)) Implements ISet(Of T).SymmetricExceptWith
// Me.InnerSet.SymmetricExceptWith(other)
// End Sub

// Public Sub UnionWith(other As IEnumerable(Of T)) Implements ISet(Of T).UnionWith
// Me.InnerSet.UnionWith(other)
// End Sub

// Private Sub ICollection_Add(item As T) Implements ICollection(Of T).Add
// Me.InnerSet.Add(item)
// End Sub

// Public Function Add(item As T) As Boolean Implements ISet(Of T).Add
// Return Me.InnerSet.Add(item)
// End Function

// Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
// Return Me.InnerSet.Contains(item)
// End Function

// Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
// Return Me.InnerSet.GetEnumerator()
// End Function

// Public Function IsProperSubsetOf(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).IsProperSubsetOf
// Return Me.InnerSet.IsProperSubsetOf(other)
// End Function

// Public Function IsProperSupersetOf(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).IsProperSupersetOf
// Return Me.InnerSet.IsProperSupersetOf(other)
// End Function

// Public Function IsSubsetOf(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).IsSubsetOf
// Return Me.InnerSet.IsSubsetOf(other)
// End Function

// Public Function IsSupersetOf(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).IsSupersetOf
// Return Me.InnerSet.IsSupersetOf(other)
// End Function

// Public Function Overlaps(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).Overlaps
// Return Me.InnerSet.Overlaps(other)
// End Function

// Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
// Return Me.InnerSet.Remove(item)
// End Function

// Public Function SetEquals(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).SetEquals
// Return Me.InnerSet.SetEquals(other)
// End Function

// Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
// Return Me.InnerSet.GetEnumerator()
// End Function

// End Class

// End Namespace

