Namespace Common

    Public Class OwnerNotifyingList(Of T)
        Inherits BaseList(Of T)

        Public Sub New(ByVal ChangedDelegate As Action(Of NotifyCollectionChangedEventArgs(Of T)))
            Me.Base = New OwnerNotifyingList(ChangedDelegate)
        End Sub

        Default Public Overrides Property Item(index As Integer) As T
            Get
                Return Me.Base.Item(index)
            End Get
            Set(value As T)
                Me.Base.Item(index) = value
            End Set
        End Property

        Public Overrides ReadOnly Property Count As Integer
            Get
                Return Me.Base.Count
            End Get
        End Property

        Public Overrides Sub Insert(index As Integer, item As T)
            Me.Base.Insert(index, item)
        End Sub

        Public Overrides Sub RemoveAt(index As Integer)
            Me.Base.RemoveAt(index)
        End Sub

        Public Overrides Sub Clear()
            Me.Base.Clear()
        End Sub

        Protected Overrides Function IEnumerable_1_GetEnumerator() As IEnumerator(Of T)
            Return Me.GetEnumerator()
        End Function

        Public Function GetEnumerator() As IEnumerator(Of T)
            Return Me.Base.GetEnumerator()
        End Function

        Private ReadOnly Base As OwnerNotifyingList

        Public Class OwnerNotifyingList
            Inherits NotifyingList(Of T)

            Public Sub New(ByVal ChangedDelegate As Action(Of NotifyCollectionChangedEventArgs(Of T)))
                Me.ChangedDelegate = ChangedDelegate
            End Sub

            Protected Overrides Sub OnCollectionChanged(E As NotifyCollectionChangedEventArgs(Of T))
                Me.ChangedDelegate.Invoke(E)
            End Sub

            Private ReadOnly ChangedDelegate As Action(Of NotifyCollectionChangedEventArgs(Of T))

        End Class

    End Class

End Namespace
