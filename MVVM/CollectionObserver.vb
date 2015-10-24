Imports System.Collections.Specialized

Namespace MVVM

    Public Class CollectionObserver(Of T)

        Public Sub New()

        End Sub

        Private Sub Collection_CollectionChanged(ByVal Sender As Object, ByVal E As NotifyCollectionChangedEventArgs)
            Select Case E.Action
                Case NotifyCollectionChangedAction.Add
                    Dim I = E.NewStartingIndex
                    For Each El In E.NewItems
                        Me.OnElementMoved(New ElementMovedEventArgs(Of T)(DirectCast(El, T), -1, I))
                        I += 1
                    Next
                Case NotifyCollectionChangedAction.Remove
                    Dim I = E.OldStartingIndex
                    For Each El In E.OldItems
                        Me.OnElementMoved(New ElementMovedEventArgs(Of T)(DirectCast(El, T), I, -1))
                    Next
                Case NotifyCollectionChangedAction.Move
                    Me.OnElementMoved(New ElementMovedEventArgs(Of T)(DirectCast(E.NewItems(0), T), E.OldStartingIndex, E.NewStartingIndex))
                Case NotifyCollectionChangedAction.Replace
                    Me.OnElementMoved(New ElementMovedEventArgs(Of T)(DirectCast(E.OldItems(0), T), E.OldStartingIndex, -1))
                    Me.OnElementMoved(New ElementMovedEventArgs(Of T)(DirectCast(E.NewItems(0), T), -1, E.NewStartingIndex))
                Case NotifyCollectionChangedAction.Reset
                    Me.OnCollectionReset()
            End Select
        End Sub

        Private ReadOnly _Collection_CollectionChanged As NotifyCollectionChangedEventHandler = New NotifyCollectionChangedEventHandler(AddressOf Me.Collection_CollectionChanged)

#Region "ElementMoved Event"
        Public Event ElementMoved As EventHandler(Of ElementMovedEventArgs(Of T))

        Protected Sub OnElementMoved(ByVal E As ElementMovedEventArgs(Of T))
            RaiseEvent ElementMoved(Me, E)
        End Sub
#End Region

#Region "CollectionReset Event"
        Public Event CollectionReset As EventHandler

        Protected Sub OnCollectionReset()
            RaiseEvent CollectionReset(Me, EventArgs.Empty)
        End Sub
#End Region

#Region "Collection Property"
        Private _Collection As IEnumerable(Of T)

        Public Property Collection As IEnumerable(Of T)
            Get
                Return Me._Collection
            End Get
            Set(ByVal Value As IEnumerable(Of T))
                Dim Obs = TryCast(Me._Collection, INotifyCollectionChanged)
                If Obs IsNot Nothing Then
                    RemoveHandler Obs.CollectionChanged, Me._Collection_CollectionChanged
                End If

                Me._Collection = Value

                Obs = TryCast(Me._Collection, INotifyCollectionChanged)
                If Obs IsNot Nothing Then
                    AddHandler Obs.CollectionChanged, Me._Collection_CollectionChanged
                End If
            End Set
        End Property
#End Region

    End Class

    Public Class ElementMovedEventArgs(Of T)
        Inherits EventArgs

        Public Sub New(ByVal Element As T, ByVal OldIndex As Integer, ByVal NewIndex As Integer)
            Me._Element = Element
            Me._OldIndex = OldIndex
            Me._NewIndex = NewIndex
        End Sub

#Region "Element Property"
        Private ReadOnly _Element As T

        Public ReadOnly Property Element As T
            Get
                Return Me._Element
            End Get
        End Property
#End Region

#Region "NewIndex Property"
        Private ReadOnly _NewIndex As Integer

        Public ReadOnly Property NewIndex As Integer
            Get
                Return Me._NewIndex
            End Get
        End Property
#End Region

#Region "OldIndex Property"
        Private ReadOnly _OldIndex As Integer

        Public ReadOnly Property OldIndex As Integer
            Get
                Return Me._OldIndex
            End Get
        End Property
#End Region

    End Class

End Namespace
