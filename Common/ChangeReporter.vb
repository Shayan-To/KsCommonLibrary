Imports System.Collections.Specialized
Imports System.ComponentModel

Public Class ChangeReporter

    Dim A As New List(Of Object)()

    Public Sub Add(ByVal Obj As Object)
        Dim PropObj As INotifyPropertyChanged,
            CollecObj As INotifyCollectionChanged,
            Enumerable As IEnumerable

        If Obj.GetType().Name = "NotifyingCollection`1" Then
            A.Add(Obj)
        End If

        PropObj = TryCast(Obj, INotifyPropertyChanged)
        If PropObj IsNot Nothing Then
            AddHandler PropObj.PropertyChanged, AddressOf Me.OnPropertyChanged
        End If

        CollecObj = TryCast(Obj, INotifyCollectionChanged)
        If CollecObj IsNot Nothing Then
            AddHandler CollecObj.CollectionChanged, AddressOf Me.OnCollectionChanged
        End If

        Enumerable = TryCast(Obj, IEnumerable)
        If Enumerable IsNot Nothing Then
            For Each O As Object In Enumerable
                Me.Add(O)
            Next
        End If
    End Sub

    Public Sub Remove(ByVal Obj As Object)
        Dim PropObj As INotifyPropertyChanged,
            CollecObj As INotifyCollectionChanged,
            Enumerable As IEnumerable

        PropObj = TryCast(Obj, INotifyPropertyChanged)
        If PropObj IsNot Nothing Then
            RemoveHandler PropObj.PropertyChanged, AddressOf Me.OnPropertyChanged
        End If

        CollecObj = TryCast(Obj, INotifyCollectionChanged)
        If CollecObj IsNot Nothing Then
            RemoveHandler CollecObj.CollectionChanged, AddressOf Me.OnCollectionChanged
        End If

        Enumerable = TryCast(Obj, IEnumerable)
        If Enumerable IsNot Nothing Then
            For Each O As Object In Enumerable
                Me.Remove(O)
            Next
        End If
    End Sub

    Private Sub OnPropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        Me.OnObjectChanged()
    End Sub

    Private Sub OnCollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
        If e.Action <> NotifyCollectionChangedAction.Move Then
            If e.OldItems IsNot Nothing Then
                For Each O As Object In e.OldItems
                    Me.Remove(O)
                Next
            End If
            If e.NewItems IsNot Nothing Then
                For Each O As Object In e.NewItems
                    Me.Add(O)
                Next
            End If
        End If
        Me.OnObjectChanged()
    End Sub

#Region "ObjectChanged Event"
    Public Event ObjectChanged As EventHandler(Of EventArgs)

    Protected Overridable Sub OnObjectChanged()
        RaiseEvent ObjectChanged(Me, EventArgs.Empty)
    End Sub
#End Region

End Class
