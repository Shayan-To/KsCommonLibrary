Imports System.Collections.Specialized

Public Class SelectAsNotifyingListCollection(Of TIn, TOut)
    Inherits SelectAsListCollection(Of TIn, TOut)
    Implements INotifyCollectionChanged(Of TOut)

    Public Sub New(ByVal List As IReadOnlyList(Of TIn), ByVal Func As Func(Of TIn, TOut))
        MyBase.New(List, Func)

        Dim Notifying = TryCast(List, INotifyCollectionChanged)
        If Notifying IsNot Nothing Then
            AddHandler Notifying.CollectionChanged, AddressOf Me.List_CollectionChanged
        End If
    End Sub

    Private Sub List_CollectionChanged(ByVal Sender As Object, ByVal E As NotifyCollectionChangedEventArgs)
        Dim NE As NotifyCollectionChangedEventArgs(Of TOut)

        Select Case E.Action
            Case NotifyCollectionChangedAction.Move
                NE = New NotifyCollectionChangedEventArgs(Of TOut)(NotifyCollectionChangedAction.Move,
                                                                   E.NewItems.CastAsList(Of TIn).SelectAsList(Me.Func),
                                                                   E.NewStartingIndex,
                                                                   E.OldStartingIndex)
            Case NotifyCollectionChangedAction.Replace
                NE = New NotifyCollectionChangedEventArgs(Of TOut)(NotifyCollectionChangedAction.Replace,
                                                                   E.NewItems.CastAsList(Of TIn).SelectAsList(Me.Func),
                                                                   E.OldItems.CastAsList(Of TIn).SelectAsList(Me.Func),
                                                                   E.NewStartingIndex)
            Case NotifyCollectionChangedAction.Reset
                NE = New NotifyCollectionChangedEventArgs(Of TOut)(NotifyCollectionChangedAction.Reset)
            Case NotifyCollectionChangedAction.Add
                NE = New NotifyCollectionChangedEventArgs(Of TOut)(NotifyCollectionChangedAction.Add,
                                                                   E.NewItems.CastAsList(Of TIn).SelectAsList(Me.Func),
                                                                   E.NewStartingIndex)
            Case NotifyCollectionChangedAction.Remove
                NE = New NotifyCollectionChangedEventArgs(Of TOut)(NotifyCollectionChangedAction.Remove,
                                                                   E.OldItems.CastAsList(Of TIn).SelectAsList(Me.Func),
                                                                   E.OldStartingIndex)
            Case Else
                Throw New InvalidOperationException("Action not supported.")
        End Select

        Me.OnCollectionChanged(NE)
    End Sub

#Region "CollectionChanged Event"
    Public Event CollectionChanged As NotifyCollectionChangedEventHandler(Of TOut) Implements INotifyCollectionChanged(Of TOut).CollectionChanged
    Private Event INotifyCollectionChanged_CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

    Protected Overridable Sub OnCollectionChanged(ByVal E As NotifyCollectionChangedEventArgs(Of TOut))
        RaiseEvent CollectionChanged(Me, E)
        RaiseEvent INotifyCollectionChanged_CollectionChanged(Me, E)
    End Sub
#End Region

End Class
