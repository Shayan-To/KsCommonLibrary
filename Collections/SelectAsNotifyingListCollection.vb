Imports System.Collections.Specialized

Public Class SelectAsNotifyingListCollection(Of TIn, TOut)
    Implements IReadOnlyList(Of TOut),
               INotifyCollectionChanged(Of TOut)

    Public Sub New(ByVal List As IReadOnlyList(Of TIn), ByVal Func As Func(Of TIn, TOut))
        Me.OrigList = List
        Me.Func = Func
        Me.List = New List(Of TOut)(List.Select(Func))

        Dim Notifying = TryCast(List, INotifyCollectionChanged)
        If Notifying IsNot Nothing Then
            AddHandler Notifying.CollectionChanged, AddressOf Me.List_CollectionChanged
        End If
    End Sub

    Private Sub List_CollectionChanged(ByVal Sender As Object, ByVal E As NotifyCollectionChangedEventArgs)
        Select Case E.Action
            Case NotifyCollectionChangedAction.Move
                For I As Integer = 1 To E.NewItems.Count
                    Me.List.RemoveAt(E.OldStartingIndex)
                Next

                Dim Ind = E.NewStartingIndex
                For I As Integer = 1 To E.NewItems.Count
                    Me.List.Insert(Ind, Me.Func.Invoke(DirectCast(E.NewItems.Item(I), TIn)))
                    Ind += 1
                Next
            Case NotifyCollectionChangedAction.Replace
                Me.List.Item(E.NewStartingIndex) = Me.Func.Invoke(E.NewItems.Cast(Of TIn)().Single())
            Case NotifyCollectionChangedAction.Reset
                Me.List.Clear()
                Me.List.AddRange(Me.OrigList.Select(Me.Func))
            Case NotifyCollectionChangedAction.Add
                Dim Ind = E.NewStartingIndex
                For I As Integer = 1 To E.NewItems.Count
                    Me.List.Insert(Ind, Me.Func.Invoke(DirectCast(E.NewItems.Item(I), TIn)))
                    Ind += 1
                Next
            Case NotifyCollectionChangedAction.Remove
                For I As Integer = 1 To E.OldItems.Count
                    Me.List.RemoveAt(E.OldStartingIndex)
                Next
        End Select

        Me.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of TOut).FromNotifyCollectionChangedEventArgs(E))
    End Sub

    Public ReadOnly Property Count As Integer Implements IReadOnlyCollection(Of TOut).Count
        Get
            Return Me.List.Count
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal Index As Integer) As TOut Implements IReadOnlyList(Of TOut).Item
        Get
            Return Me.List.Item(Index)
        End Get
    End Property

    Public Function GetEnumerator() As IEnumerator(Of TOut) Implements IEnumerable(Of TOut).GetEnumerator
        Return Me.List.GetEnumerator()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.List.GetEnumerator()
    End Function

#Region "CollectionChanged Event"
    Public Event CollectionChanged As NotifyCollectionChangedEventHandler(Of TOut) Implements INotifyCollectionChanged(Of TOut).CollectionChanged
    Private Event INotifyCollectionChanged_CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

    Protected Overridable Sub OnCollectionChanged(ByVal E As NotifyCollectionChangedEventArgs(Of TOut))
        RaiseEvent CollectionChanged(Me, E)
        RaiseEvent INotifyCollectionChanged_CollectionChanged(Me, E)
    End Sub
#End Region

    Private ReadOnly List As List(Of TOut)
    Private ReadOnly OrigList As IReadOnlyList(Of TIn)
    Private ReadOnly Func As Func(Of TIn, TOut)

End Class
