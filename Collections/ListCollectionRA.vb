Imports System.Collections.Specialized
Imports System.Runtime.Serialization
Imports Ks.Common

Public Class ListCollectionRA(Of T)
    Inherits ListCollectionRA(Of T, List(Of T))

    Public Sub New()
        MyBase.New(Function() As List(Of T)
                       Return New List(Of T)()
                   End Function)
    End Sub

End Class

Public Class ListCollectionRA(Of T, List As IList(Of T))
    Implements INotifyingCollection(Of List),
               IList(Of List),
               ISerializable

    Private ReadOnly InnerList As List(Of List),
                     ListSeeder As Func(Of List)

    Public Sub New(ByVal ListSeeder As Func(Of List))
        Me.ListSeeder = ListSeeder
        Me.InnerList = New List(Of List)()
    End Sub

    'Public Sub New()
    '    Me.New(Function() As List
    '               Return DirectCast(GetType(List).GetConstructor(New Type() {}).Invoke(New Object() {}), List)
    '           End Function)
    'End Sub

    'Public Sub New(ByVal Type As Type)
    '    Me.New(Function() As List
    '               Return DirectCast(Type.GetConstructor(New Type() {}).Invoke(New Object() {}), List)
    '           End Function)

    '    If Not GetType(List).IsAssignableFrom(Type) Then
    '        Throw New ArgumentException("Type has to implement the interface List.")
    '    End If
    'End Sub

    'Public Sub New(ByVal Constructor As Reflection.ConstructorInfo, ParamArray ByVal Parameters As Object())
    '    Me.New(Function() As List
    '               Return DirectCast(Constructor.Invoke(Parameters), List)
    '           End Function)

    '    If Not GetType(List).IsAssignableFrom(Constructor.DeclaringType) Then
    '        Throw New ArgumentException("Type has to implement the interface List.")
    '    End If
    '    Parameters = DirectCast(Parameters.Clone(), Object())
    'End Sub

    Default Public Property Item(ByVal Index As Integer) As List Implements IList(Of List).Item
        Get
            Me.EnsureFits(Index)
            Return Me.InnerList.Item(Index)
        End Get
#Region "Not Supported"
        Set(ByVal Value As List)
            Throw New NotSupportedException()
        End Set
#End Region
    End Property

#Region "ISerializable Logic"
    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        '_Title = info.GetString("Title")
    End Sub

    Protected Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData

    End Sub
#End Region

#Region "CollectionChanged Event"
    Public Event CollectionChanged As NotifyCollectionChangedEventHandler(Of List) Implements INotifyCollectionChanged(Of List).CollectionChanged
    Private Event Nongeneric_CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

    Protected Overridable Sub OnCollectionChanged(ByVal E As NotifyCollectionChangedEventArgs(Of List))
        RaiseEvent CollectionChanged(Me, E)
        RaiseEvent Nongeneric_CollectionChanged(Me, E)
    End Sub
#End Region

    Private Function InstantiateList() As List
        Return Me.ListSeeder.Invoke()
    End Function

    Private Sub EnsureFits(ByVal Index As Integer)
        Dim T As List,
            NewItems As List(Of List)

        If Me.InnerList.Count <= Index Then
            NewItems = New List(Of List)()

            For I As Integer = Me.InnerList.Count To Index
                T = Me.InstantiateList()
                Me.InnerList.Add(T)
                NewItems.Add(T)
            Next

            Me.OnCollectionChanged(New NotifyCollectionChangedEventArgs(Of List)(NotifyCollectionChangedAction.Add, NewItems))
        End If
    End Sub

    Public Function GetEnumerator() As List(Of List).Enumerator
        Return Me.InnerList.GetEnumerator()
    End Function

    Private Function GetEnumerator_Interface() As IEnumerator(Of List) Implements IEnumerable(Of List).GetEnumerator
        Return Me.InnerList.GetEnumerator()
    End Function

#Region "Trivial Implementations"
    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function

    Public ReadOnly Property Count As Integer Implements ICollection(Of List).Count
        Get
            Return Me.InnerList.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of List).IsReadOnly
        Get
            Return True
        End Get
    End Property

    Public Sub CopyTo(ByVal Array As List(), ByVal ArrayIndex As Integer) Implements ICollection(Of List).CopyTo
        Me.InnerList.CopyTo(Array, ArrayIndex)
    End Sub
#End Region

#Region "Not Supported"
    Public Function IndexOf(ByVal item As List) As Integer Implements IList(Of List).IndexOf
        Throw New NotSupportedException()
    End Function

    Public Sub Insert(ByVal index As Integer, ByVal item As List) Implements IList(Of List).Insert
        Throw New NotSupportedException()
    End Sub

    Public Sub RemoveAt(ByVal index As Integer) Implements IList(Of List).RemoveAt
        Throw New NotSupportedException()
    End Sub

    Public Sub Add(ByVal item As List) Implements ICollection(Of List).Add
        Throw New NotSupportedException()
    End Sub

    Public Sub Clear() Implements ICollection(Of List).Clear
        Throw New NotSupportedException()
    End Sub

    Public Function Contains(ByVal item As List) As Boolean Implements ICollection(Of List).Contains
        Throw New NotSupportedException()
    End Function

    Public Function Remove(ByVal item As List) As Boolean Implements ICollection(Of List).Remove
        Throw New NotSupportedException()
    End Function
#End Region

End Class
