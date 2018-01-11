Namespace Common

    Public Class JsonDynamicList
        Inherits JsonDynamicBase
        Implements IReadOnlyList(Of JsonDynamicBase), IList(Of JsonDynamicBase), IList

        Public Sub Insert(index As Integer, item As JsonDynamicBase) Implements IList(Of JsonDynamicBase).Insert
            Me.Base.Insert(index, item)
        End Sub

        Public Sub RemoveAt(index As Integer) Implements IList(Of JsonDynamicBase).RemoveAt, IList.RemoveAt
            Me.Base.RemoveAt(index)
        End Sub

        Public Sub Clear() Implements ICollection(Of JsonDynamicBase).Clear, IList.Clear
            Me.Base.Clear()
        End Sub

        Default Public Property Item(index As Integer) As JsonDynamicBase Implements IList(Of JsonDynamicBase).Item
            Get
                Return Me.Base.Item(index)
            End Get
            Set(value As JsonDynamicBase)
                Me.Base.Item(index) = value
            End Set
        End Property

        Public ReadOnly Property Count As Integer Implements IReadOnlyCollection(Of JsonDynamicBase).Count, ICollection.Count, ICollection(Of JsonDynamicBase).Count
            Get
                Return Me.Base.Count
            End Get
        End Property

        Protected Function IEnumerable_1_GetEnumerator() As IEnumerator(Of JsonDynamicBase) Implements IEnumerable(Of JsonDynamicBase).GetEnumerator
            Return Me.GetEnumerator()
        End Function

        Public Function GetEnumerator() As List(Of JsonDynamicBase).Enumerator
            Return Me.Base.GetEnumerator()
        End Function

        Public Overridable Function IndexOf(item As JsonDynamicBase) As Integer Implements IList(Of JsonDynamicBase).IndexOf
            For I As Integer = 0 To Me.Count - 1
                If Object.Equals(item, Me.Item(I)) Then
                    Return I
                End If
            Next
            Return -1
        End Function

        Public Overridable Sub CopyTo(array() As JsonDynamicBase, arrayIndex As Integer) Implements ICollection(Of JsonDynamicBase).CopyTo
            Me.CopyTo(DirectCast(array, Array), arrayIndex)
        End Sub

        Protected Overridable Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
            Verify.TrueArg(array.Rank = 1, NameOf(array), "Array's rank must be 1.")
            Verify.TrueArg(index + Me.Count <= array.Length, NameOf(array), "Array does not have enough length to copy the collection.")
            For I As Integer = 0 To Me.Count - 1
                array.SetValue(Me.Item(I), index)
                index += 1
            Next
        End Sub

        Public Overridable Function Remove(item As JsonDynamicBase) As Boolean Implements ICollection(Of JsonDynamicBase).Remove
            Dim I = Me.IndexOf(item)
            If I = -1 Then
                Return False
            End If

            Me.RemoveAt(I)
            Return True
        End Function

#Region "Junk"
        Private Property IList_Item(index As Integer) As Object Implements IList.Item
            Get
                Return Me.Item(index)
            End Get
            Set(value As Object)
                Me.Item(index) = DirectCast(value, JsonDynamicBase)
            End Set
        End Property

        Private ReadOnly Property IReadOnlyList_Item(index As Integer) As JsonDynamicBase Implements IReadOnlyList(Of JsonDynamicBase).Item
            Get
                Return Me.Item(index)
            End Get
        End Property

        Protected Overridable ReadOnly Property IList_IsReadOnly As Boolean Implements ICollection(Of JsonDynamicBase).IsReadOnly, IList.IsReadOnly
            Get
                Return False
            End Get
        End Property

        Private ReadOnly Property IList_IsFixedSize As Boolean Implements IList.IsFixedSize
            Get
                Return False
            End Get
        End Property

        Private ReadOnly Property IList_SyncRoot As Object Implements ICollection.SyncRoot
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Private ReadOnly Property IList_IsSynchronized As Boolean Implements ICollection.IsSynchronized
            Get
                Return False
            End Get
        End Property

        Public Function Contains(item As JsonDynamicBase) As Boolean Implements ICollection(Of JsonDynamicBase).Contains
            Return Me.IndexOf(item) <> -1
        End Function

        Private Function IList_Add(value As Object) As Integer Implements IList.Add
            Me.Add(DirectCast(value, JsonDynamicBase))
            Return Me.Count - 1
        End Function

        Private Function IList_Contains(value As Object) As Boolean Implements IList.Contains
            Return Me.Contains(DirectCast(value, JsonDynamicBase))
        End Function

        Private Function IList_IndexOf(value As Object) As Integer Implements IList.IndexOf
            Return Me.IndexOf(DirectCast(value, JsonDynamicBase))
        End Function

        Private Sub IList_Insert(index As Integer, value As Object) Implements IList.Insert
            Me.Insert(index, DirectCast(value, JsonDynamicBase))
        End Sub

        Private Sub IList_Remove(value As Object) Implements IList.Remove
            Me.Remove(DirectCast(value, JsonDynamicBase))
        End Sub

        Public Overridable Sub Add(item As JsonDynamicBase) Implements ICollection(Of JsonDynamicBase).Add
            Me.Insert(Me.Count, item)
        End Sub

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.IEnumerable_1_GetEnumerator()
        End Function
#End Region

        Private ReadOnly Base As List(Of JsonDynamicBase) = New List(Of JsonDynamicBase)()

    End Class

End Namespace
