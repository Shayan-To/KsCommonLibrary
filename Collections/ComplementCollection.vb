Imports System.Collections.Specialized
Imports Ks.Common

Public Class ComplementingCollectionMaster(Of T)
    Implements INotifyingCollection(Of T)

    Public Sub New()
        Me._Collection1 = New ChildComplementCollection(Of T)(Me)
        Me._Collection2 = New ChildComplementCollection(Of T)(Me)
    End Sub

#Region "Collection1 Property"
    Private ReadOnly _Collection1 As ChildComplementCollection(Of T)

    Public ReadOnly Property Collection1 As INotifyingCollection(Of T)
        Get
            Return Me._Collection1
        End Get
    End Property
#End Region

#Region "Collection2 Property"
    Private ReadOnly _Collection2 As ChildComplementCollection(Of T)

    Public ReadOnly Property Collection2 As INotifyingCollection(Of T)
        Get
            Return Me._Collection2
        End Get
    End Property
#End Region

#Region "CollectionChanged Event"
    Private Event NonGeneric_CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged
    Public Event CollectionChanged As NotifyCollectionChangedEventHandler(Of T) Implements INotifyCollectionChanged(Of T).CollectionChanged

    Protected Sub OnCollectionChanged(ByVal E As NotifyCollectionChangedEventArgs(Of T))
        RaiseEvent CollectionChanged(Me, E)
        RaiseEvent NonGeneric_CollectionChanged(Me, E)
    End Sub
#End Region

#Region "Reading Logic"
    Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count
        Get
            Return Me._Collection1.Count + Me._Collection2.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
        Get
            Return False
        End Get
    End Property

    Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
        If array.Length - arrayIndex < Me.Count Then
            Throw New ArgumentException("The number of elements in the source System.Collections.Generic.ICollection`1 is greater than the available space from arrayIndex to the end of the destination array.")
        End If
        Me._Collection1.CopyTo(array, arrayIndex)
        Me._Collection2.CopyTo(array, arrayIndex + Me._Collection1.Count)
    End Sub

    Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
        Return Me._Collection1.Contains(item) OrElse
               Me._Collection2.Contains(item)
    End Function

    Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Return Me._Collection1.Concat(Me._Collection2).GetEnumerator()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function
#End Region

#Region "ChildOperation Logic"
    Friend Sub AddC(ByVal Item As T, ByVal C1 As ChildComplementCollection(Of T), Optional C2 As ChildComplementCollection(Of T) = Nothing)
        If C2 Is Nothing Then
            If C1 Is Me._Collection1 Then
                C2 = Me._Collection2
            Else
                C2 = Me._Collection1
            End If
        End If

        If Not C2.RemoveI(Item) Then
            Throw New ArgumentException()
        End If
        C1.AddI(Item)

        C1.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateAdd(Item))
        C2.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateRemove(Item))
    End Sub

    Friend Sub ClearC(ByVal C1 As ChildComplementCollection(Of T), Optional C2 As ChildComplementCollection(Of T) = Nothing)
        Dim C1Clone As IList(Of T)

        If C2 Is Nothing Then
            If C1 Is Me._Collection1 Then
                C2 = Me._Collection2
            Else
                C2 = Me._Collection1
            End If
        End If

        C1Clone = New T(C1.Count - 1) {}
        C1.CopyTo(C1Clone)

        C1.ClearI()

        For Each I As T In C1Clone
            C2.AddI(I)
            C2.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateAdd(I))
        Next

        C1.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateReset())
    End Sub

    Friend Function RemoveC(ByVal Item As T, ByVal C1 As ChildComplementCollection(Of T), Optional C2 As ChildComplementCollection(Of T) = Nothing) As Boolean
        If C2 Is Nothing Then
            If C1 Is Me._Collection1 Then
                C2 = Me._Collection2
            Else
                C2 = Me._Collection1
            End If
        End If

        If Not C1.RemoveI(Item) Then
            Throw New ArgumentException()
        End If
        C2.AddI(Item)

        C1.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateRemove(Item))
        C2.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateAdd(Item))

        Return True
    End Function
#End Region

    Public Sub Add(item As T) Implements ICollection(Of T).Add
        If Me._Collection1.Contains(item) OrElse Me._Collection2.Contains(item) Then
            Exit Sub
        End If

        Me._Collection1.Add(item)
        Me.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateAdd(item))
    End Sub

    Public Sub Clear() Implements ICollection(Of T).Clear
        Me._Collection1.Clear()
        Me._Collection2.Clear()
        Me.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateReset())
    End Sub

    Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
        If Me._Collection1.Remove(item) OrElse
           Me._Collection2.Remove(item) Then
            Me.OnCollectionChanged(NotifyCollectionChangedEventArgs(Of T).CreateRemove(item))
            Return True
        End If
        Return False
    End Function

End Class

Friend Class ChildComplementCollection(Of T)
    Implements INotifyingCollection(Of T)

    Friend Sub New(ByVal Master As ComplementingCollectionMaster(Of T))
        Me.InnerList = New List(Of T)()
        Me._Master = Master
    End Sub

    Private ReadOnly InnerList As List(Of T)

#Region "CollectionChanged Event"
    Private Event NonGeneric_CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged
    Public Event CollectionChanged As NotifyCollectionChangedEventHandler(Of T) Implements INotifyCollectionChanged(Of T).CollectionChanged

    Protected Friend Sub OnCollectionChanged(ByVal E As NotifyCollectionChangedEventArgs(Of T))
        RaiseEvent CollectionChanged(Me, E)
        RaiseEvent NonGeneric_CollectionChanged(Me, E)
    End Sub
#End Region

#Region "Master Property"
    Private ReadOnly _Master As ComplementingCollectionMaster(Of T)

    Public ReadOnly Property Master As ComplementingCollectionMaster(Of T)
        Get
            Return Me._Master
        End Get
    End Property
#End Region

#Region "Reading Logic"
    Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count
        Get
            Return Me.InnerList.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
        Get
            Return False
        End Get
    End Property

    Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
        Me.InnerList.CopyTo(array, arrayIndex)
    End Sub

    Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
        Return Me.InnerList.Contains(item)
    End Function

    Public Function GetEnumerator() As List(Of T).Enumerator
        Return Me.InnerList.GetEnumerator()
    End Function

    Private Function GetEnumerator_Interface() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Return Me.InnerList.GetEnumerator()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function
#End Region

#Region "DirectOperations Logic"
    Friend Sub AddI(item As T)
        Me.InnerList.Add(item)
    End Sub

    Friend Sub ClearI()
        Me.InnerList.Clear()
    End Sub

    Friend Function RemoveI(item As T) As Boolean
        Return Me.InnerList.Remove(item)
    End Function
#End Region

    Public Sub Add(ByVal Item As T) Implements ICollection(Of T).Add
        Me._Master.AddC(Item, Me)
    End Sub

    Public Sub Clear() Implements ICollection(Of T).Clear
        Me._Master.ClearC(Me)
    End Sub

    Public Function Remove(ByVal Item As T) As Boolean Implements ICollection(Of T).Remove
        Return Me._Master.RemoveC(Item, Me)
    End Function

End Class

'Friend Class ComplementCollection(Of T)
'    Inherits Collection(Of T)
'    Implements INotifyCollectionChanged

'    Private ReadOnly InnerList As List(Of T)

'    Private Sub New(ByVal ObservingCollection As NotifyingCollection(Of T), ByVal MasterCollection As IEnumerable(Of T))
'        Me._ObservingCollection = ObservingCollection
'        Me._MasterCollection = MasterCollection
'        InnerList = New List(Of T)()
'    End Sub

'#Region "ObservingCollection Property"
'    Private ReadOnly _ObservingCollection As NotifyingCollection(Of T)

'    Public ReadOnly Property ObservingCollection As NotifyingCollection(Of T)
'        Get
'            Return Me._ObservingCollection
'        End Get
'    End Property
'#End Region

'#Region "MasterCollection Property"
'    Private ReadOnly _MasterCollection As IEnumerable(Of T)
'    Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

'    Public ReadOnly Property MasterCollection As IEnumerable(Of T)
'        Get
'            Return Me._MasterCollection
'        End Get
'    End Property
'#End Region

'    Private Sub OnObservingCollectionChanged(ByVal Sender As Object, ByVal E As NotifyCollectionChangedEventArgs)
'        Select Case E.Action
'            Case NotifyCollectionChangedAction.Add

'            Case NotifyCollectionChangedAction.Remove

'            Case NotifyCollectionChangedAction.Replace

'            Case NotifyCollectionChangedAction.Reset

'        End Select
'    End Sub

'    Protected Overrides Sub ClearItems()

'    End Sub

'    Protected Overrides Sub InsertItem(index As Integer, item As T)

'    End Sub

'    Protected Overridable Sub MoveItem(oldIndex As Integer, newIndex As Integer)

'    End Sub

'    Protected Overridable Sub OnCollectionChanged(e As NotifyCollectionChangedEventArgs)

'    End Sub

'    Protected Overrides Sub RemoveItem(index As Integer)

'    End Sub

'    Protected Overrides Sub SetItem(index As Integer, item As T)

'    End Sub

'End Class
