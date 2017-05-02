Public Structure JoinElement(Of T1, T2, TKey)

    Public Sub New(ByVal Key As TKey, ByVal Direction As JoinDirection, ByVal Item1 As T1, ByVal Item2 As T2)
        Me._Key = Key
        Me._Direction = Direction
        Me._Item1 = Item1
        Me._Item2 = Item2
    End Sub

#Region "Item1 Read-Only Property"
    Private ReadOnly _Item1 As T1

    Public ReadOnly Property Item1 As T1
        Get
            Return Me._Item1
        End Get
    End Property
#End Region

#Region "Item2 Read-Only Property"
    Private ReadOnly _Item2 As T2

    Public ReadOnly Property Item2 As T2
        Get
            Return Me._Item2
        End Get
    End Property
#End Region

#Region "Key Read-Only Property"
    Private ReadOnly _Key As TKey

    Public ReadOnly Property Key As TKey
        Get
            Return Me._Key
        End Get
    End Property
#End Region

#Region "Direction Read-Only Property"
    Private ReadOnly _Direction As JoinDirection

    Public ReadOnly Property Direction As JoinDirection
        Get
            Return Me._Direction
        End Get
    End Property
#End Region

End Structure
