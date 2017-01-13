Imports System.Collections.Specialized

Namespace Common

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

        Public Sub New(ByVal List As IReadOnlyList(Of TIn), ByVal Func As Func(Of TIn, Integer, TOut))
            MyBase.New(List, Func)

            Dim Notifying = TryCast(List, INotifyCollectionChanged)
            If Notifying IsNot Nothing Then
                AddHandler Notifying.CollectionChanged, AddressOf Me.List_CollectionChanged
            End If
        End Sub

        Private Sub List_CollectionChanged(ByVal Sender As Object, ByVal E As NotifyCollectionChangedEventArgs)
            Dim NE As NotifyCollectionChangedEventArgs(Of TOut)

            If Me.Func IsNot Nothing Then
                Select Case E.Action
                    Case NotifyCollectionChangedAction.Move
                        NE = NotifyCollectionChangedEventArgs(Of TOut).CreateMove(E.NewItems.CastAsList(Of TIn).SelectAsList(Me.Func),
                                                                                  E.NewStartingIndex,
                                                                                  E.OldStartingIndex)
                    Case NotifyCollectionChangedAction.Replace
                        NE = NotifyCollectionChangedEventArgs(Of TOut).CreateReplace(E.NewItems.CastAsList(Of TIn).SelectAsList(Me.Func),
                                                                                     E.OldItems.CastAsList(Of TIn).SelectAsList(Me.Func),
                                                                                     E.NewStartingIndex)
                    Case NotifyCollectionChangedAction.Reset
                        NE = NotifyCollectionChangedEventArgs(Of TOut).CreateReset()
                    Case NotifyCollectionChangedAction.Add
                        NE = NotifyCollectionChangedEventArgs(Of TOut).CreateAdd(E.NewItems.CastAsList(Of TIn).SelectAsList(Me.Func),
                                                                                 E.NewStartingIndex)
                    Case NotifyCollectionChangedAction.Remove
                        NE = NotifyCollectionChangedEventArgs(Of TOut).CreateRemove(E.OldItems.CastAsList(Of TIn).SelectAsList(Me.Func),
                                                                                    E.OldStartingIndex)
                    Case Else
                        Throw New InvalidOperationException("Action not supported.")
                End Select
            Else
                Select Case E.Action
                    Case NotifyCollectionChangedAction.Move
                        NE = NotifyCollectionChangedEventArgs(Of TOut).CreateMove(E.NewItems.CastAsList(Of TIn).SelectAsList(Me.FuncIndexed),
                                                                                  E.NewStartingIndex,
                                                                                  E.OldStartingIndex)
                    Case NotifyCollectionChangedAction.Replace
                        NE = NotifyCollectionChangedEventArgs(Of TOut).CreateReplace(E.NewItems.CastAsList(Of TIn).SelectAsList(Me.FuncIndexed),
                                                                                     E.OldItems.CastAsList(Of TIn).SelectAsList(Me.FuncIndexed),
                                                                                     E.NewStartingIndex)
                    Case NotifyCollectionChangedAction.Reset
                        NE = NotifyCollectionChangedEventArgs(Of TOut).CreateReset()
                    Case NotifyCollectionChangedAction.Add
                        NE = NotifyCollectionChangedEventArgs(Of TOut).CreateAdd(E.NewItems.CastAsList(Of TIn).SelectAsList(Me.FuncIndexed),
                                                                                 E.NewStartingIndex)
                    Case NotifyCollectionChangedAction.Remove
                        NE = NotifyCollectionChangedEventArgs(Of TOut).CreateRemove(E.OldItems.CastAsList(Of TIn).SelectAsList(Me.FuncIndexed),
                                                                                    E.OldStartingIndex)
                    Case Else
                        Throw New InvalidOperationException("Action not supported.")
                End Select
            End If

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

End Namespace
