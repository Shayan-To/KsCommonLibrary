Namespace Common

    Public NotInheritable Class CreateInstanceList

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        Public Shared Function Create(Of T As New)(ByVal List As IList(Of T)) As CreateInstanceList(Of T)
            Return New CreateInstanceList(Of T)(List)
        End Function

    End Class

    Public Class CreateInstanceList(Of T As New)
        Implements IList(Of T)

        Public Sub New(ByVal List As IList(Of T))
            Me.List = List
        End Sub

        Public Sub Add(ByVal item As T) Implements ICollection(Of T).Add
            Me.List.Add(item)
        End Sub

        Public Sub Clear() Implements ICollection(Of T).Clear
            Me.List.Clear()
        End Sub

        Public Function Contains(ByVal item As T) As Boolean Implements ICollection(Of T).Contains
            Return Me.List.Contains(item)
        End Function

        Public Sub CopyTo(ByVal array() As T, ByVal arrayIndex As Integer) Implements ICollection(Of T).CopyTo
            Me.List.CopyTo(array, arrayIndex)
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count
            Get
                Return Me.List.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal Item As T) As Boolean Implements ICollection(Of T).Remove
            Return Me.List.Remove(Item)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal Value As T) Implements IList(Of T).Insert
            Me.List.Insert(Index, Value)
        End Sub

        Public Sub Insert(ByVal Index As Integer)
            Me.List.Insert(Index, New T())
        End Sub

        Default Public Property Item(ByVal Index As Integer) As T Implements IList(Of T).Item
            Get
                Dim V As T
                If Index > Me.List.Count Then
                    Throw New ArgumentOutOfRangeException("Index has to be inside the list range or immidiately after the end of it.")
                ElseIf Index = Me.List.Count Then
                    V = New T()
                    Me.List.Add(V)
                Else
                    V = Me.List.Item(Index)
                End If
                Return V
            End Get
            Set(ByVal Value As T)
                If Index = Me.List.Count Then
                    Me.List.Add(Value)
                Else
                    Me.List.Item(Index) = Value
                End If
            End Set
        End Property

        Public Sub RemoveAt(ByVal Index As Integer) Implements IList(Of T).RemoveAt
            Me.List.RemoveAt(Index)
        End Sub

        Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
            Return Me.List.GetEnumerator()
        End Function

        Private Function GetEnumerator_NonGeneric() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function

        Public Function IndexOf(Item As T) As Integer Implements IList(Of T).IndexOf
            Return Me.List.IndexOf(Item)
        End Function

        Private ReadOnly List As IList(Of T)

    End Class

End Namespace
