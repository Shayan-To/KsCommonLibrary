Public Structure SerializationArrayChunk(Of T)

    Public Sub New(ByVal Array As T())
        Me.New(Array, 0, Array.Length)
    End Sub

    Public Sub New(ByVal Array As T(), ByVal StartIndex As Integer, ByVal Length As Integer)
        Me._Array = Array
        Me._StartIndex = StartIndex
        Me._Length = Length
    End Sub

#Region "Array Property"
    Private ReadOnly _Array As T()

    Public ReadOnly Property Array As T()
        Get
            Return Me._Array
        End Get
    End Property
#End Region

#Region "StartIndex Property"
    Private ReadOnly _StartIndex As Integer

    Public ReadOnly Property StartIndex As Integer
        Get
            Return Me._StartIndex
        End Get
    End Property
#End Region

#Region "Length Property"
    Private ReadOnly _Length As Integer

    Public ReadOnly Property Length As Integer
        Get
            Return Me._Length
        End Get
    End Property
#End Region

End Structure
