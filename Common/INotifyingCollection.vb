Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports System.Diagnostics.Contracts
Imports Ks.Common

Public Delegate Sub NotifyCollectionChangedEventHandler(Of T)(sender As Object, e As NotifyCollectionChangedEventArgs(Of T))

Public Class NotifyCollectionChangedEventArgs(Of T)
    Inherits NotifyCollectionChangedEventArgs

    Public Sub New(action As NotifyCollectionChangedAction)
        MyBase.New(action)

        InitializeAdd(action, Nothing, -1)
    End Sub

    Public Sub New(action As NotifyCollectionChangedAction, changedItem As T)
        MyBase.New(action, changedItem)

        If action = NotifyCollectionChangedAction.Reset Then
            InitializeAdd(action, Nothing, -1)
        Else
            InitializeAddOrRemove(action, New T() {changedItem}, -1)
        End If
    End Sub

    Public Sub New(action As NotifyCollectionChangedAction, changedItem As T, index As Integer)
        MyBase.New(action, changedItem, index)

        If action = NotifyCollectionChangedAction.Reset Then
            InitializeAdd(action, Nothing, -1)
        Else
            InitializeAddOrRemove(action, New T() {changedItem}, index)
        End If
    End Sub

    Public Sub New(action As NotifyCollectionChangedAction, changedItems As IList(Of T))
        MyBase.New(action, DirectCast(changedItems, IList))

        If action = NotifyCollectionChangedAction.Reset Then
            InitializeAdd(action, Nothing, -1)
        Else
            InitializeAddOrRemove(action, changedItems, -1)
        End If
    End Sub

    Public Sub New(action As NotifyCollectionChangedAction, changedItems As IList(Of T), startingIndex As Integer)
        MyBase.New(action, DirectCast(changedItems, IList), startingIndex)

        If action = NotifyCollectionChangedAction.Reset Then
            InitializeAdd(action, Nothing, -1)
        Else
            InitializeAddOrRemove(action, changedItems, startingIndex)
        End If
    End Sub

    Public Sub New(action As NotifyCollectionChangedAction, newItem As T, oldItem As T)
        MyBase.New(action, newItem, oldItem)

        InitializeMoveOrReplace(action, New T() {newItem}, New T() {oldItem}, -1, -1)
    End Sub

    Public Sub New(action As NotifyCollectionChangedAction, newItem As T, oldItem As T, index As Integer)
        MyBase.New(action, newItem, oldItem, index)

        Dim oldStartingIndex As Integer = index

        InitializeMoveOrReplace(action, New T() {newItem}, New T() {oldItem}, index, oldStartingIndex)
    End Sub

    Public Sub New(action As NotifyCollectionChangedAction, newItems As IList(Of T), oldItems As IList(Of T))
        MyBase.New(action, DirectCast(newItems, IList), DirectCast(oldItems, IList))

        InitializeMoveOrReplace(action, newItems, oldItems, -1, -1)
    End Sub

    Public Sub New(action As NotifyCollectionChangedAction, newItems As IList(Of T), oldItems As IList(Of T), startingIndex As Integer)
        MyBase.New(action, DirectCast(newItems, IList), DirectCast(oldItems, IList), startingIndex)

        InitializeMoveOrReplace(action, newItems, oldItems, startingIndex, startingIndex)
    End Sub

    Public Sub New(action As NotifyCollectionChangedAction, changedItem As T, index As Integer, oldIndex As Integer)
        MyBase.New(action, changedItem, index, oldIndex)

        Dim changedItems As T() = New T() {changedItem}
        InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex)
    End Sub

    Public Sub New(action As NotifyCollectionChangedAction, changedItems As IList(Of T), index As Integer, oldIndex As Integer)
        MyBase.New(action, DirectCast(changedItems, IList), index, oldIndex)

        InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex)
    End Sub

    Public Shared Function FromNotifyCollectionChangedEventArgs(ByVal E As NotifyCollectionChangedEventArgs) As NotifyCollectionChangedEventArgs(Of T)
        Select Case E.Action
            Case NotifyCollectionChangedAction.Move
                Return New NotifyCollectionChangedEventArgs(Of T)(E.Action,
                                                                  E.NewItems.ToGeneric().Select(Function(A) DirectCast(A, T)).ToList(),
                                                                  E.NewStartingIndex,
                                                                  E.OldStartingIndex)
            Case NotifyCollectionChangedAction.Replace
                Return New NotifyCollectionChangedEventArgs(Of T)(E.Action,
                                                                  E.NewItems.ToGeneric().Select(Function(A) DirectCast(A, T)).ToList(),
                                                                  E.OldItems.ToGeneric().Select(Function(A) DirectCast(A, T)).ToList(),
                                                                  E.NewStartingIndex)
            Case NotifyCollectionChangedAction.Reset
                Return New NotifyCollectionChangedEventArgs(Of T)(E.Action)
            Case NotifyCollectionChangedAction.Add
                Return New NotifyCollectionChangedEventArgs(Of T)(E.Action,
                                                                  E.NewItems.ToGeneric().Select(Function(A) DirectCast(A, T)).ToList(),
                                                                  E.NewStartingIndex)
            Case NotifyCollectionChangedAction.Remove
                Return New NotifyCollectionChangedEventArgs(Of T)(E.Action,
                                                                  E.OldItems.ToGeneric().Select(Function(A) DirectCast(A, T)).ToList(),
                                                                  E.OldStartingIndex)
        End Select
        Debug.Assert(False)
        Return Nothing
    End Function

    Public Shared Function FromNotifyCollectionChangedEventArgsNoIndex(ByVal E As NotifyCollectionChangedEventArgs) As NotifyCollectionChangedEventArgs(Of T)
        Select Case E.Action
            Case NotifyCollectionChangedAction.Move
                Return New NotifyCollectionChangedEventArgs(Of T)(E.Action,
                                                                  E.NewItems.ToGeneric().Select(Function(A) DirectCast(A, T)).ToList())
            Case NotifyCollectionChangedAction.Replace
                Return New NotifyCollectionChangedEventArgs(Of T)(E.Action,
                                                                  E.NewItems.ToGeneric().Select(Function(A) DirectCast(A, T)).ToList(),
                                                                  E.OldItems.ToGeneric().Select(Function(A) DirectCast(A, T)).ToList())
            Case NotifyCollectionChangedAction.Reset
                Return New NotifyCollectionChangedEventArgs(Of T)(E.Action)
            Case NotifyCollectionChangedAction.Add
                Return New NotifyCollectionChangedEventArgs(Of T)(E.Action,
                                                                  E.NewItems.ToGeneric().Select(Function(A) DirectCast(A, T)).ToList())
            Case NotifyCollectionChangedAction.Remove
                Return New NotifyCollectionChangedEventArgs(Of T)(E.Action,
                                                                  E.OldItems.ToGeneric().Select(Function(A) DirectCast(A, T)).ToList())
        End Select

        Debug.Assert(False)
        Return Nothing
    End Function

    Private Sub InitializeAddOrRemove(action As NotifyCollectionChangedAction, changedItems As IList(Of T), startingIndex As Integer)
        If action = NotifyCollectionChangedAction.Add Then
            InitializeAdd(action, changedItems, startingIndex)
        ElseIf action = NotifyCollectionChangedAction.Remove Then
            InitializeRemove(action, changedItems, startingIndex)
        End If
    End Sub

    Private Sub InitializeAdd(action As NotifyCollectionChangedAction, newItems As IList(Of T), newStartingIndex As Integer)
        _NewItems = If((newItems Is Nothing), Nothing, New List(Of T)(newItems).AsReadOnly())
    End Sub

    Private Sub InitializeRemove(action As NotifyCollectionChangedAction, oldItems As IList(Of T), oldStartingIndex As Integer)
        _OldItems = If((oldItems Is Nothing), Nothing, New List(Of T)(oldItems).AsReadOnly())
    End Sub

    Private Sub InitializeMoveOrReplace(action As NotifyCollectionChangedAction, newItems As IList(Of T), oldItems As IList(Of T), startingIndex As Integer, oldStartingIndex As Integer)
        InitializeAdd(action, newItems, startingIndex)
        InitializeRemove(action, oldItems, oldStartingIndex)
    End Sub

#Region "NewItems Property"
    Private _NewItems As IList(Of T)

    Public Shadows ReadOnly Property NewItems As IList(Of T)
        Get
            Return Me._NewItems
        End Get
    End Property
#End Region

#Region "OldItems Property"
    Private _OldItems As IList(Of T)

    Public Shadows ReadOnly Property OldItems As IList(Of T)
        Get
            Return Me._OldItems
        End Get
    End Property
#End Region

End Class

Public Interface INotifyCollectionChanged(Of T)
    Inherits INotifyCollectionChanged

    Shadows Event CollectionChanged As NotifyCollectionChangedEventHandler(Of T)

End Interface

Public Interface INotifyingCollection(Of T)
    Inherits INotifyCollectionChanged(Of T),
             ICollection(Of T)

End Interface

Public Class NotifyingCollection(Of T)
    Inherits ObservableCollection(Of T)
    Implements INotifyingCollection(Of T)

    Public Shadows Event CollectionChanged As NotifyCollectionChangedEventHandler(Of T) Implements INotifyCollectionChanged(Of T).CollectionChanged

    Protected Overrides Sub OnCollectionChanged(ByVal E As NotifyCollectionChangedEventArgs)
        Dim E2 As NotifyCollectionChangedEventArgs(Of T)
        E2 = NotifyCollectionChangedEventArgs(Of T).FromNotifyCollectionChangedEventArgs(E)
        RaiseEvent CollectionChanged(Me, E2)
        ' ToDo Correct this E to E2.
        MyBase.OnCollectionChanged(E2)
    End Sub

End Class
