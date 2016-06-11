Public Class MultiDimensionalList(Of T)
    Implements IEnumerable(Of T)

    Public Sub New(ParamArray ByVal Lengths As Integer())
        Me._Lengths = Lengths
        Me._Lengths_RO = Lengths.AsReadOnly()
        Dim Length = 1
        For Each L In Lengths
            Length *= L
        Next
        Me.Arr = New T(Length - 1) {}
    End Sub

    Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Return DirectCast(Me.Arr, IList(Of T)).GetEnumerator()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function

    Private Function GetIndex(ParamArray ByVal Indexes As Integer()) As Integer
        Verify.True(Indexes.Length = Me._Lengths.Length)
        Dim Index = 0
        For I As Integer = 0 To Indexes.Length - 1
            Verify.True(0 <= Indexes(I) And Indexes(I) < Me._Lengths(I))
            Index *= Me._Lengths(I)
            Index += Indexes(I)
        Next
        Return Index
    End Function

#Region "Item Property"
    Public Property Item(ParamArray ByVal Indexes As Integer()) As T
        Get
            Return Me.Arr(Me.GetIndex(Indexes))
        End Get
        Set(ByVal Value As T)
            Me.Arr(Me.GetIndex(Indexes)) = Value
        End Set
    End Property
#End Region

#Region "Lengths Property"
    Private ReadOnly _Lengths As Integer()
    Private ReadOnly _Lengths_RO As IReadOnlyList(Of Integer)

    Public ReadOnly Property Lengths As IReadOnlyList(Of Integer)
        Get
            Return Me._Lengths_RO
        End Get
    End Property

    Public ReadOnly Property Lengths(ByVal Index As Integer) As Integer
        Get
            Return Me._Lengths(Index)
        End Get
    End Property
#End Region

    Private ReadOnly Arr As T()

End Class
