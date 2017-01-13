Imports System.Collections.Specialized

Namespace Common

    Public Delegate Sub NotifyCollectionChangedEventHandler(Of T)(sender As Object, e As NotifyCollectionChangedEventArgs(Of T))

    Public Class NotifyCollectionChangedEventArgs(Of T)
        Inherits NotifyCollectionChangedEventArgs

#Disable Warning BC40000 ' Type or member is obsolete
        <Obsolete("Use the Create methods instead.")>
        Public Sub New(action As NotifyCollectionChangedAction)
            MyBase.New(action)

            InitializeAdd(action, Nothing, -1)
        End Sub

        Public Shared Function CreateReset() As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Reset)
        End Function

        ''' <summary>
        ''' For Add, Remove, or Reset.
        ''' </summary>
        <Obsolete("Use the Create methods instead.")>
        Public Sub New(action As NotifyCollectionChangedAction, changedItem As T)
            MyBase.New(action, changedItem)

            If action = NotifyCollectionChangedAction.Reset Then
                InitializeAdd(action, Nothing, -1)
            Else
                InitializeAddOrRemove(action, New T() {changedItem}, -1)
            End If
        End Sub

        Public Shared Function CreateAdd(changedItem As T) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Add, changedItem)
        End Function

        Public Shared Function CreateRemove(changedItem As T) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Remove, changedItem)
        End Function

        ''' <summary>
        ''' For Add, Remove, or Reset.
        ''' </summary>
        <Obsolete("Use the Create methods instead.")>
        Public Sub New(action As NotifyCollectionChangedAction, changedItem As T, index As Integer)
            MyBase.New(action, changedItem, index)

            If action = NotifyCollectionChangedAction.Reset Then
                InitializeAdd(action, Nothing, -1)
            Else
                InitializeAddOrRemove(action, New T() {changedItem}, index)
            End If
        End Sub

        Public Shared Function CreateAdd(changedItem As T, index As Integer) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Add, changedItem, index)
        End Function

        Public Shared Function CreateRemove(changedItem As T, index As Integer) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Remove, changedItem, index)
        End Function

        ''' <summary>
        ''' For Add, Remove, or Reset.
        ''' </summary>
        <Obsolete("Use the Create methods instead.")>
        Public Sub New(action As NotifyCollectionChangedAction, changedItems As IList(Of T))
            MyBase.New(action, DirectCast(changedItems, IList))

            If action = NotifyCollectionChangedAction.Reset Then
                InitializeAdd(action, Nothing, -1)
            Else
                InitializeAddOrRemove(action, changedItems, -1)
            End If
        End Sub

        Public Shared Function CreateAdd(changedItems As IList(Of T)) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Add, changedItems)
        End Function

        Public Shared Function CreateRemove(changedItems As IList(Of T)) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Remove, changedItems)
        End Function

        ''' <summary>
        ''' For Add, Remove, or Reset.
        ''' </summary>
        <Obsolete("Use the Create methods instead.")>
        Public Sub New(action As NotifyCollectionChangedAction, changedItems As IList(Of T), startingIndex As Integer)
            MyBase.New(action, DirectCast(changedItems, IList), startingIndex)

            If action = NotifyCollectionChangedAction.Reset Then
                InitializeAdd(action, Nothing, -1)
            Else
                InitializeAddOrRemove(action, changedItems, startingIndex)
            End If
        End Sub

        Public Shared Function CreateAdd(changedItems As IList(Of T), startingIndex As Integer) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Add, changedItems, startingIndex)
        End Function

        Public Shared Function CreateRemove(changedItems As IList(Of T), startingIndex As Integer) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Remove, changedItems, startingIndex)
        End Function

        ''' <summary>
        ''' For Move or Replace.
        ''' </summary>
        <Obsolete("Use the Create methods instead.")>
        Public Sub New(action As NotifyCollectionChangedAction, newItem As T, oldItem As T)
            MyBase.New(action, newItem, oldItem)

            InitializeMoveOrReplace(action, New T() {newItem}, New T() {oldItem}, -1, -1)
        End Sub

        Public Shared Function CreateReplace(newItem As T, oldItem As T) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Replace, newItem, oldItem)
        End Function

        ''' <summary>
        ''' For Move or Replace.
        ''' </summary>
        <Obsolete("Use the Create methods instead.")>
        Public Sub New(action As NotifyCollectionChangedAction, newItem As T, oldItem As T, index As Integer)
            MyBase.New(action, newItem, oldItem, index)

            InitializeMoveOrReplace(action, New T() {newItem}, New T() {oldItem}, index, index)
        End Sub

        Public Shared Function CreateReplace(newItem As T, oldItem As T, index As Integer) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Replace, newItem, oldItem, index)
        End Function

        ''' <summary>
        ''' For Move or Replace.
        ''' </summary>
        <Obsolete("Use the Create methods instead.")>
        Public Sub New(action As NotifyCollectionChangedAction, newItems As IList(Of T), oldItems As IList(Of T))
            MyBase.New(action, DirectCast(newItems, IList), DirectCast(oldItems, IList))

            InitializeMoveOrReplace(action, newItems, oldItems, -1, -1)
        End Sub

        Public Shared Function CreateMove(changedItem As T) As NotifyCollectionChangedEventArgs(Of T)
            Dim Items = New T() {changedItem}
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Move, Items, Items)
        End Function

        Public Shared Function CreateMove(changedItems As IList(Of T)) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Move, changedItems, changedItems)
        End Function

        Public Shared Function CreateReplace(newItems As IList(Of T), oldItems As IList(Of T)) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Replace, newItems, oldItems)
        End Function

        ''' <summary>
        ''' For Move or Replace.
        ''' </summary>
        <Obsolete("Use the Create methods instead.")>
        Public Sub New(action As NotifyCollectionChangedAction, newItems As IList(Of T), oldItems As IList(Of T), startingIndex As Integer)
            MyBase.New(action, DirectCast(newItems, IList), DirectCast(oldItems, IList), startingIndex)

            InitializeMoveOrReplace(action, newItems, oldItems, startingIndex, startingIndex)
        End Sub

        Public Shared Function CreateReplace(newItems As IList(Of T), oldItems As IList(Of T), startingIndex As Integer) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Replace, newItems, oldItems, startingIndex)
        End Function

        ''' <summary>
        ''' For Move or Replace.
        ''' </summary>
        <Obsolete("Use the Create methods instead.")>
        Public Sub New(action As NotifyCollectionChangedAction, changedItem As T, index As Integer, oldIndex As Integer)
            MyBase.New(action, changedItem, index, oldIndex)

            Dim changedItems = New T() {changedItem}
            InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex)
        End Sub

        Public Shared Function CreateMove(changedItem As T, index As Integer, oldIndex As Integer) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Move, changedItem, index, oldIndex)
        End Function

        ''' <summary>
        ''' For Move or Replace.
        ''' </summary>
        <Obsolete("Use the Create methods instead.")>
        Public Sub New(action As NotifyCollectionChangedAction, changedItems As IList(Of T), index As Integer, oldIndex As Integer)
            MyBase.New(action, DirectCast(changedItems, IList), index, oldIndex)

            InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex)
        End Sub

        Public Shared Function CreateMove(changedItems As IList(Of T), index As Integer, oldIndex As Integer) As NotifyCollectionChangedEventArgs(Of T)
            Return New NotifyCollectionChangedEventArgs(Of T)(NotifyCollectionChangedAction.Move, changedItems, index, oldIndex)
        End Function
#Enable Warning BC40000 ' Type or member is obsolete

        Public Shared Function FromNotifyCollectionChangedEventArgs(ByVal E As NotifyCollectionChangedEventArgs) As NotifyCollectionChangedEventArgs(Of T)
            Select Case E.Action
                Case NotifyCollectionChangedAction.Move
                    Return CreateMove(E.NewItems.Cast(Of T)().ToArray(),
                                  E.NewStartingIndex,
                                  E.OldStartingIndex)
                Case NotifyCollectionChangedAction.Replace
                    Return CreateReplace(E.NewItems.Cast(Of T)().ToArray(),
                                     E.OldItems.Cast(Of T)().ToArray(),
                                     E.NewStartingIndex)
                Case NotifyCollectionChangedAction.Reset
                    Return CreateReset()
                Case NotifyCollectionChangedAction.Add
                    Return CreateAdd(E.NewItems.Cast(Of T)().ToArray(),
                                 E.NewStartingIndex)
                Case NotifyCollectionChangedAction.Remove
                    Return CreateRemove(E.OldItems.Cast(Of T)().ToArray(),
                                    E.OldStartingIndex)
            End Select

            Assert.Fail()
            Return Nothing
        End Function

        Public Shared Function FromNotifyCollectionChangedEventArgsNoIndex(ByVal E As NotifyCollectionChangedEventArgs) As NotifyCollectionChangedEventArgs(Of T)
            Select Case E.Action
                Case NotifyCollectionChangedAction.Move
                    Return CreateMove(E.NewItems.Cast(Of T)().ToArray())
                Case NotifyCollectionChangedAction.Replace
                    Return CreateReplace(E.NewItems.Cast(Of T)().ToArray(),
                                     E.OldItems.Cast(Of T)().ToArray())
                Case NotifyCollectionChangedAction.Reset
                    Return CreateReset()
                Case NotifyCollectionChangedAction.Add
                    Return CreateAdd(E.NewItems.Cast(Of T)().ToArray())
                Case NotifyCollectionChangedAction.Remove
                    Return CreateRemove(E.OldItems.Cast(Of T)().ToArray())
            End Select

            Assert.Fail()
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

#Region "ItemsGotIn Property"
        Public ReadOnly Property ItemsGotIn As IList(Of T)
            Get
                If Me.Action = NotifyCollectionChangedAction.Add Or Action = NotifyCollectionChangedAction.Replace Then
                    Return Me.NewItems
                End If
                Return Utilities.Typed(Of T).EmptyArray
            End Get
        End Property
#End Region

#Region "ItemsWentOut Property"
        Public ReadOnly Property ItemsWentOut As IList(Of T)
            Get
                If Me.Action = NotifyCollectionChangedAction.Remove Or Action = NotifyCollectionChangedAction.Replace Then
                    Return Me.OldItems
                End If
                Return Utilities.Typed(Of T).EmptyArray
            End Get
        End Property
#End Region

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

    Public Class NotifyingList(Of T)
        Inherits BaseList(Of T)
        Implements INotifyingCollection(Of T)

        Public Sub New(ByVal List As IList(Of T))
            Me.BaseList = List
        End Sub

        Public Sub New()
            Me.New(New List(Of T)())
        End Sub

        Public Overrides Sub Clear()
            Me.BaseList.Clear()
            Me.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateReset())
        End Sub

        Public Overrides Sub Insert(index As Integer, item As T)
            Me.BaseList.Insert(index, item)
            Me.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateAdd(item, index))
        End Sub

        Public Overrides Sub RemoveAt(index As Integer)
            Dim OldItem = Me.BaseList.Item(index)
            Me.BaseList.RemoveAt(index)
            Me.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateRemove(OldItem, index))
        End Sub

        Public Overridable Sub Move(ByVal OldIndex As Integer, NewIndex As Integer)
            Dim Item = Me.BaseList.Item(OldIndex)
            Me.BaseList.Move(OldIndex, NewIndex)
            Me.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateMove(Item, NewIndex, OldIndex))
        End Sub

        Public Overridable Sub SetFrom(ByVal Collection As IEnumerable(Of T))
            Me.BaseList.Clear()
            Me.BaseList.AddRange(Collection)
            Me.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateReset())
        End Sub

        Protected Overrides Function IEnumerable_1_GetEnumerator() As IEnumerator(Of T)
            Return Me.BaseList.GetEnumerator()
        End Function

        Public Function GetEnumerator() As IEnumerator(Of T)
            Return Me.BaseList.GetEnumerator()
        End Function

        Public Overrides ReadOnly Property Count As Integer
            Get
                Return Me.BaseList.Count
            End Get
        End Property

        Default Public Overrides Property Item(index As Integer) As T
            Get
                Return Me.BaseList.Item(index)
            End Get
            Set(value As T)
                Dim OldItem = Me.BaseList.Item(index)
                Me.BaseList.Item(index) = value
                Me.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateReplace(value, OldItem, index))
            End Set
        End Property

#Region "CollectionChanged Event"
        Public Shadows Event CollectionChanged As NotifyCollectionChangedEventHandler(Of T) Implements INotifyCollectionChanged(Of T).CollectionChanged
        Private Event INotifyCollectionChanged_CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

        Protected Overridable Sub OnCollectionChanged(ByVal E As NotifyCollectionChangedEventArgs(Of T))
            RaiseEvent INotifyCollectionChanged_CollectionChanged(Me, E)
            RaiseEvent CollectionChanged(Me, E)
        End Sub
#End Region

        Private ReadOnly BaseList As IList(Of T)

    End Class

End Namespace
