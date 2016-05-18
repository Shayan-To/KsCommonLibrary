Imports System.Runtime.CompilerServices

Public Class IntegerList
    Implements IList(Of Integer)

    ''' <param name="Start">The start of the sequence, inclusive.</param>
    ''' <param name="End">The end of the sequence, exclusive.</param>
    ''' <param name="Step">The step by which the sequence goes.</param>
    Public Sub New(ByVal Start As Integer, ByVal [End] As Integer, Optional ByVal [Step] As Integer = 1)
        Me._Start = Start
        Me._Step = [Step]
        Me._End = [End] - Utilities.Math.PosMod([End] - Start, [Step]) + [Step]
    End Sub

#Region "Start Property"
    Private ReadOnly _Start As Integer

    Public ReadOnly Property Start As Integer
        Get
            Return Me._Start
        End Get
    End Property
#End Region

#Region "End Property"
    Private ReadOnly _End As Integer

    Public ReadOnly Property [End] As Integer
        Get
            Return Me._End
        End Get
    End Property
#End Region

#Region "Step Property"
    Private ReadOnly _Step As Integer

    Public ReadOnly Property [Step] As Integer
        Get
            Return Me._Step
        End Get
    End Property
#End Region

    Default Public Property Item(ByVal Index As Integer) As Integer Implements IList(Of Integer).Item
        Get
            Dim Res As Integer

            Res = Me._Start + Index * Me._Step

            If Res >= Me._End OrElse Res < Me._Start Then
                Throw New ArgumentOutOfRangeException(NameOf(Index))
            End If

            Return Res
        End Get
#Region "Not Supported"
        Set(ByVal Value As Integer)
            Throw New NotSupportedException()
        End Set
#End Region
    End Property

    Public ReadOnly Property Count As Integer Implements ICollection(Of Integer).Count
        Get
            Return (Me._End - Me._Start) \ Me._Step
        End Get
    End Property

    Public Function IndexOf(item As Integer) As Integer Implements IList(Of Integer).IndexOf
        If Me.Contains(item) Then
            Return (item - Me._Start) \ Me._Step
        End If
        Return -1
    End Function

#If VBC_VER >= 11.0 Then
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Contains(ByVal Item As Integer) As Boolean Implements ICollection(Of Integer).Contains
#Else
    Public Function Contains(ByVal Item As Integer) As Boolean Implements ICollection(Of Integer).Contains
#End If
        Return Item >= Me._Start AndAlso
               Item < Me._End AndAlso
               (Item - Me._Start) Mod Me._Step = 0
    End Function

    Public Sub CopyTo(ByVal Array() As Integer, ByVal ArrayIndex As Integer) Implements ICollection(Of Integer).CopyTo
        For I As Integer = Me._Start To Me._End Step Me._Step
            Array(ArrayIndex) = I
            ArrayIndex += 1
        Next
    End Sub

#If VBC_VER >= 11.0 Then
    Public Iterator Function GetEnumerator() As IEnumerator(Of Integer) Implements IEnumerable(Of Integer).GetEnumerator
        For I As Integer = Me._Start To Me._End - 1 Step Me._Step
            Yield I
        Next
    End Function
#Else
    Public Function GetEnumerator() As IEnumerator(Of Integer) Implements IEnumerable(Of Integer).GetEnumerator
        Throw New NotImplementedException()
    End Function
#End If

#Region "Trivial Implementations"
    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of Integer).IsReadOnly
        Get
            Return True
        End Get
    End Property

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function
#End Region

#Region "Not Supported"
    Public Sub Insert(ByVal index As Integer, ByVal item As Integer) Implements IList(Of Integer).Insert
        Throw New NotSupportedException()
    End Sub

    Public Sub RemoveAt(ByVal index As Integer) Implements IList(Of Integer).RemoveAt
        Throw New NotSupportedException()
    End Sub

    Public Sub Add(ByVal item As Integer) Implements ICollection(Of Integer).Add
        Throw New NotSupportedException()
    End Sub

    Public Sub Clear() Implements ICollection(Of Integer).Clear
        Throw New NotSupportedException()
    End Sub

    Public Function Remove(ByVal item As Integer) As Boolean Implements ICollection(Of Integer).Remove
        Throw New NotSupportedException()
    End Function
#End Region

End Class
