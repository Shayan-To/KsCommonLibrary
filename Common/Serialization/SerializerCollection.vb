Public Class SerializerCollection
    Inherits OneToOneOrderedDictionary(Of String, Serializer)

    Public Sub New()
        MyBase.New(Function(S) S.Id)
    End Sub

    Friend Sub LockCurrentElements()
        For Each I In Me.Values
            Me.LockedItems.Add(I)
        Next
    End Sub

    Public Overrides Sub Clear()
        Dim State = Me.LockedItems.ToArray()
        MyBase.Clear()
        For Each I In State
            Me.Add(I)
        Next
    End Sub

    Public Overrides Sub RemoveAt(index As Integer)
        Verify.False(Me.LockedItems.Contains(Me.ItemAt(index)), "Serializer is locked.")
        MyBase.RemoveAt(index)
    End Sub

    Public Overrides Function RemoveKey(key As String) As Boolean
        Verify.False(Me.LockedItems.Contains(Me.Item(key)), "Serializer is locked.")
        Return MyBase.RemoveKey(key)
    End Function

    Public Overrides Function [Set](Value As Serializer) As Boolean
        Verify.False(Me.LockedItems.Contains(Value), "Serializer is locked.")
        Return MyBase.Set(Value)
    End Function

    Public Overrides Property ItemAt(index As Integer) As Serializer
        Get
            Return MyBase.ItemAt(index)
        End Get
        Set(value As Serializer)
            Verify.False(Me.LockedItems.Contains(value), "Serializer is locked.")
            MyBase.ItemAt(index) = value
        End Set
    End Property

    Private ReadOnly LockedItems As HashSet(Of Serializer) = New HashSet(Of Serializer)()

End Class
