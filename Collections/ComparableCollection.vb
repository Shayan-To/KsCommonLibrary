Public Class ComparableCollection(Of T)
    Inherits List(Of T)
    Implements IComparable(Of ComparableCollection(Of T)), 
               IStructuralComparable, 
               IStructuralEquatable

    Public Sub New()

    End Sub

    Public Sub New(ByVal Capacity As Integer)
        MyBase.New(Capacity)
    End Sub

    Public Sub New(ByVal Collection As IEnumerable(Of T))
        MyBase.New(Collection)
    End Sub

    Public Function CompareTo(ByVal Other As ComparableCollection(Of T)) As Integer Implements System.IComparable(Of ComparableCollection(Of T)).CompareTo
        Return Me.CompareTo(Other, Collections.Generic.Comparer(Of Object).Default)
    End Function

    Public Overrides Function Equals(ByVal Obj As Object) As Boolean
        Return Me.Equals(Obj, Collections.Generic.EqualityComparer(Of Object).Default)
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return Me.GetHashCode(Collections.Generic.EqualityComparer(Of Object).Default)
    End Function

    Public Function CompareTo(ByVal Other As Object, ByVal Comparer As System.Collections.IComparer) As Integer Implements System.Collections.IStructuralComparable.CompareTo
        Dim O = DirectCast(Other, ComparableCollection(Of T))

        For I As Integer = 0 To Math.Min(Me.Count, O.Count) - 1
            Dim C = Comparer.Compare(Me.Item(I), O.Item(I))
            If C <> 0 Then
                Return C
            End If
        Next

        Return Me.Count - O.Count
    End Function

    Public Overloads Function Equals(ByVal Other As Object, ByVal Comparer As System.Collections.IEqualityComparer) As Boolean Implements System.Collections.IStructuralEquatable.Equals
        If Not TypeOf Other Is ComparableCollection(Of T) Then
            Return False
        End If
        Dim O = DirectCast(Other, ComparableCollection(Of T))

        If Me.Count <> O.Count Then
            Return False
        End If

        For I As Integer = 0 To Me.Count - 1
            If Not Comparer.Equals(Me.Item(I), O.Item(I)) Then
                Return False
            End If
        Next

        Return True
    End Function

    Public Overloads Function GetHashCode(ByVal Comparer As System.Collections.IEqualityComparer) As Integer Implements System.Collections.IStructuralEquatable.GetHashCode
        Dim Bl = True
        Dim R = 0
        For Each I As T In Me
            If Bl Then
                R = Comparer.GetHashCode(I)
                Bl = False
            Else
                R = Utilities.CombineHashCodes(R, Comparer.GetHashCode(I))
            End If
        Next
        Return R
    End Function

    Public Function Clone() As ComparableCollection(Of T)
        Return New ComparableCollection(Of T)(Me)
    End Function

    Public Shared Operator >(ByVal Left As ComparableCollection(Of T), ByVal Right As ComparableCollection(Of T)) As Boolean
        Return Left.CompareTo(Right) > 0
    End Operator

    Public Shared Operator <(ByVal Left As ComparableCollection(Of T), ByVal Right As ComparableCollection(Of T)) As Boolean
        Return Left.CompareTo(Right) < 0
    End Operator

    Public Shared Operator >=(ByVal Left As ComparableCollection(Of T), ByVal Right As ComparableCollection(Of T)) As Boolean
        Return Left.CompareTo(Right) >= 0
    End Operator

    Public Shared Operator <=(ByVal Left As ComparableCollection(Of T), ByVal Right As ComparableCollection(Of T)) As Boolean
        Return Left.CompareTo(Right) <= 0
    End Operator

    Public Shared Operator =(ByVal Left As ComparableCollection(Of T), ByVal Right As ComparableCollection(Of T)) As Boolean
        Return Left.Equals(Right)
    End Operator

    Public Shared Operator <>(ByVal Left As ComparableCollection(Of T), ByVal Right As ComparableCollection(Of T)) As Boolean
        Return Left.Equals(Right)
    End Operator

End Class
