Public Class BitArray
    Inherits BaseList(Of Boolean)

    Public Sub New(ByVal Count As Integer)
        Me._Count = Count
        Me._Bytes = New Byte((Count >> 3) + If((Count And 7) = 0, 0, 1) - 1) {}
    End Sub

    Public Overrides Sub Insert(index As Integer, item As Boolean)
        Throw New NotSupportedException()
    End Sub

    Public Overrides Sub RemoveAt(index As Integer)
        Throw New NotSupportedException()
    End Sub

    Public Overrides Sub Clear()
        Array.Clear(Me._Bytes, 0, Me._Bytes.Length)
    End Sub

    Private Function LastByteBitCount() As Integer
        Dim R = Me.Count And 7
        If R = 0 Then
            R = 8
        End If
        Return R
    End Function

    Public Sub [Not]()
        For I = 0 To Me._Bytes.Length - 1
            Me._Bytes(I) = Not Me._Bytes(I)
        Next
    End Sub

    ''' <param name="Amount">Shift left if positive, right if negative.</param>
    Public Sub Shift(ByVal Amount As Integer)
        For I = 0 To Me._Bytes.Length - 1
            Me._Bytes(I) = Not Me._Bytes(I)
        Next
        Me._Bytes(Me._Bytes.Length - 1) = CByte(Me._Bytes(Me._Bytes.Length - 1) And ((1 << Me.LastByteBitCount()) - 1))
    End Sub

    Protected Overrides Function IEnumerable_1_GetEnumerator() As IEnumerator(Of Boolean)
        Return Me.GetEnumerator()
    End Function

    Public Iterator Function GetEnumerator() As IEnumerator(Of Boolean)
        For I = 0 To Me._Bytes.Length - 2
            Dim B = Me._Bytes(I)
            For J = 0 To 7
                Yield ((B >> J) And 1) = 1
            Next
        Next

        If True Then
            Dim B = Me._Bytes(Me._Bytes.Length - 1)

            For J = 0 To Me.LastByteBitCount() - 1
                Yield ((B >> J) And 1) = 1
            Next
        End If
    End Function

    Public Sub SetOne(ByVal Index As Integer)
        Verify.True(0 <= Index And Index < Me.Count, "Index out of range.")

        Dim B As Integer = Me._Bytes(Index >> 3)
        Dim I = Index And 7
        B = B Or (1 << I)
        Me._Bytes(Index >> 3) = CByte(B)
    End Sub

    Public Sub SetZero(ByVal Index As Integer)
        Verify.True(0 <= Index And Index < Me.Count, "Index out of range.")

        Dim B As Integer = Me._Bytes(Index >> 3)
        Dim I = Index And 7
        B = B And Not (1 << I)
        Me._Bytes(Index >> 3) = CByte(B)
    End Sub

    Default Public Overrides Property Item(index As Integer) As Boolean
        Get
            Verify.True(0 <= index And index < Me.Count, "Index out of range.")

            Dim B = Me._Bytes(index >> 3)
            Dim I = index And 7
            Return ((B >> I) And 1) = 1
        End Get
        Set(value As Boolean)
            Verify.True(0 <= index And index < Me.Count, "Index out of range.")

            Dim B As Integer = Me._Bytes(index >> 3)
            Dim I = index And 7
            B = (B And Not (1 << I)) Or (If(value, 1, 0) << I)
            Me._Bytes(index >> 3) = CByte(B)
        End Set
    End Property

    Public Property [Byte](Index As Integer) As Byte
        Get
            Return Me._Bytes(Index)
        End Get
        Set(Value As Byte)
            If Index = Me._Bytes.Length - 1 Then
                Value = CByte(Value And ((1 << Me.LastByteBitCount()) - 1))
            End If
            Me._Bytes(Index) = Value
        End Set
    End Property

#Region "BytesCount Property"
    Public ReadOnly Property BytesCount As Integer
        Get
            Return Me._Bytes.Count
        End Get
    End Property
#End Region

#Region "Count Property"
    Private ReadOnly _Count As Integer

    Public Overrides ReadOnly Property Count As Integer
        Get
            Return Me._Count
        End Get
    End Property
#End Region

    Private _Bytes As Byte()

End Class
