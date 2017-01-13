Namespace Common

    Public NotInheritable Class VTuple

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        Public Shared Function Create(Of T1)(ByVal Item1 As T1) As VTuple(Of T1)
            Return New VTuple(Of T1)(Item1)
        End Function

        Public Shared Function Create(Of T1, T2)(ByVal Item1 As T1, ByVal Item2 As T2) As VTuple(Of T1, T2)
            Return New VTuple(Of T1, T2)(Item1, Item2)
        End Function

        Public Shared Function Create(Of T1, T2, T3)(ByVal Item1 As T1, ByVal Item2 As T2, ByVal Item3 As T3) As VTuple(Of T1, T2, T3)
            Return New VTuple(Of T1, T2, T3)(Item1, Item2, Item3)
        End Function

        Public Shared Function Create(Of T1, T2, T3, T4)(ByVal Item1 As T1, ByVal Item2 As T2, ByVal Item3 As T3, ByVal Item4 As T4) As VTuple(Of T1, T2, T3, T4)
            Return New VTuple(Of T1, T2, T3, T4)(Item1, Item2, Item3, Item4)
        End Function

        Public Shared Function Create(Of T1, T2, T3, T4, T5)(ByVal Item1 As T1, ByVal Item2 As T2, ByVal Item3 As T3, ByVal Item4 As T4, ByVal Item5 As T5) As VTuple(Of T1, T2, T3, T4, T5)
            Return New VTuple(Of T1, T2, T3, T4, T5)(Item1, Item2, Item3, Item4, Item5)
        End Function

    End Class

    Public Structure VTuple(Of T1)
        Implements IComparable(Of VTuple(Of T1)),
               IStructuralComparable,
               IStructuralEquatable

        Public Sub New(ByVal Item1 As T1)
            Me._Item1 = Item1
        End Sub

        Public Overrides Function ToString() As String
            Return String.Concat("VTuple{", Me._Item1, "}")
        End Function

        Public Function CompareTo(ByVal Other As VTuple(Of T1)) As Integer Implements IComparable(Of VTuple(Of T1)).CompareTo
            Return Me.CompareTo(Other, Comparer(Of Object).Default)
        End Function

        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            Return Me.Equals(Obj, EqualityComparer(Of Object).Default)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.GetHashCode(EqualityComparer(Of Object).Default)
        End Function

        Public Function CompareTo(ByVal Other As Object, ByVal Comparer As IComparer) As Integer Implements IStructuralComparable.CompareTo
            Dim O = DirectCast(Other, VTuple(Of T1))

            Return Comparer.Compare(Me._Item1, O._Item1)
        End Function

        Public Overloads Function Equals(ByVal Other As Object, ByVal Comparer As IEqualityComparer) As Boolean Implements IStructuralEquatable.Equals
            If Not TypeOf Other Is VTuple(Of T1) Then
                Return False
            End If
            Dim O = DirectCast(Other, VTuple(Of T1))
            Return Comparer.Equals(Me._Item1, O._Item1)
        End Function

        Public Overloads Function GetHashCode(ByVal Comparer As IEqualityComparer) As Integer Implements IStructuralEquatable.GetHashCode
            Return Comparer.GetHashCode(Me._Item1)
        End Function

        Public Shared Operator >(ByVal Left As VTuple(Of T1), ByVal Right As VTuple(Of T1)) As Boolean
            Return Left.CompareTo(Right) > 0
        End Operator

        Public Shared Operator <(ByVal Left As VTuple(Of T1), ByVal Right As VTuple(Of T1)) As Boolean
            Return Left.CompareTo(Right) < 0
        End Operator

        Public Shared Operator >=(ByVal Left As VTuple(Of T1), ByVal Right As VTuple(Of T1)) As Boolean
            Return Left.CompareTo(Right) >= 0
        End Operator

        Public Shared Operator <=(ByVal Left As VTuple(Of T1), ByVal Right As VTuple(Of T1)) As Boolean
            Return Left.CompareTo(Right) <= 0
        End Operator

        Public Shared Operator =(ByVal Left As VTuple(Of T1), ByVal Right As VTuple(Of T1)) As Boolean
            Return Left.Equals(Right)
        End Operator

        Public Shared Operator <>(ByVal Left As VTuple(Of T1), ByVal Right As VTuple(Of T1)) As Boolean
            Return Left.Equals(Right)
        End Operator

#Region "Item1 Property"
        Private ReadOnly _Item1 As T1

        Public ReadOnly Property Item1 As T1
            Get
                Return Me._Item1
            End Get
        End Property
#End Region

    End Structure

    Public Structure VTuple(Of T1, T2)
        Implements IComparable(Of VTuple(Of T1, T2)),
               IStructuralComparable,
               IStructuralEquatable

        Public Sub New(ByVal Item1 As T1, ByVal Item2 As T2)
            Me._Item1 = Item1
            Me._Item2 = Item2
        End Sub

        Public Overrides Function ToString() As String
            Return String.Concat("VTuple{", Me._Item1, ", ", Me._Item2, "}")
        End Function

        Public Function CompareTo(ByVal Other As VTuple(Of T1, T2)) As Integer Implements IComparable(Of VTuple(Of T1, T2)).CompareTo
            Return Me.CompareTo(Other, Comparer(Of Object).Default)
        End Function

        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            Return Me.Equals(Obj, EqualityComparer(Of Object).Default)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.GetHashCode(EqualityComparer(Of Object).Default)
        End Function

        Public Function CompareTo(ByVal Other As Object, ByVal Comparer As IComparer) As Integer Implements IStructuralComparable.CompareTo
            Dim O = DirectCast(Other, VTuple(Of T1, T2))

            Dim C = Comparer.Compare(Me._Item1, O._Item1)
            If C <> 0 Then
                Return C
            End If

            Return Comparer.Compare(Me._Item2, O._Item2)
        End Function

        Public Overloads Function Equals(ByVal Other As Object, ByVal Comparer As IEqualityComparer) As Boolean Implements IStructuralEquatable.Equals
            If Not TypeOf Other Is VTuple(Of T1, T2) Then
                Return False
            End If
            Dim O = DirectCast(Other, VTuple(Of T1, T2))
            Return Comparer.Equals(Me._Item1, O._Item1) AndAlso
               Comparer.Equals(Me._Item2, O._Item2)
        End Function

        Public Overloads Function GetHashCode(ByVal Comparer As IEqualityComparer) As Integer Implements IStructuralEquatable.GetHashCode
            Return Utilities.CombineHashCodes(Comparer.GetHashCode(Me._Item1),
                                         Comparer.GetHashCode(Me._Item2))
        End Function

        Public Shared Operator >(ByVal Left As VTuple(Of T1, T2), ByVal Right As VTuple(Of T1, T2)) As Boolean
            Return Left.CompareTo(Right) > 0
        End Operator

        Public Shared Operator <(ByVal Left As VTuple(Of T1, T2), ByVal Right As VTuple(Of T1, T2)) As Boolean
            Return Left.CompareTo(Right) < 0
        End Operator

        Public Shared Operator >=(ByVal Left As VTuple(Of T1, T2), ByVal Right As VTuple(Of T1, T2)) As Boolean
            Return Left.CompareTo(Right) >= 0
        End Operator

        Public Shared Operator <=(ByVal Left As VTuple(Of T1, T2), ByVal Right As VTuple(Of T1, T2)) As Boolean
            Return Left.CompareTo(Right) <= 0
        End Operator

        Public Shared Operator =(ByVal Left As VTuple(Of T1, T2), ByVal Right As VTuple(Of T1, T2)) As Boolean
            Return Left.Equals(Right)
        End Operator

        Public Shared Operator <>(ByVal Left As VTuple(Of T1, T2), ByVal Right As VTuple(Of T1, T2)) As Boolean
            Return Left.Equals(Right)
        End Operator

#Region "Item1 Property"
        Private ReadOnly _Item1 As T1

        Public ReadOnly Property Item1 As T1
            Get
                Return Me._Item1
            End Get
        End Property
#End Region

#Region "Item2 Property"
        Private ReadOnly _Item2 As T2

        Public ReadOnly Property Item2 As T2
            Get
                Return Me._Item2
            End Get
        End Property
#End Region

    End Structure

    Public Structure VTuple(Of T1, T2, T3)
        Implements IComparable(Of VTuple(Of T1, T2, T3)),
               IStructuralComparable,
               IStructuralEquatable

        Public Sub New(ByVal Item1 As T1, ByVal Item2 As T2, ByVal Item3 As T3)
            Me._Item1 = Item1
            Me._Item2 = Item2
            Me._Item3 = Item3
        End Sub

        Public Overrides Function ToString() As String
            Return String.Concat("VTuple{", Me._Item1, ", ", Me._Item2, ", ", Me._Item3, "}")
        End Function

        Public Function CompareTo(ByVal Other As VTuple(Of T1, T2, T3)) As Integer Implements IComparable(Of VTuple(Of T1, T2, T3)).CompareTo
            Return Me.CompareTo(Other, Comparer(Of Object).Default)
        End Function

        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            Return Me.Equals(Obj, EqualityComparer(Of Object).Default)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.GetHashCode(EqualityComparer(Of Object).Default)
        End Function

        Public Function CompareTo(ByVal Other As Object, ByVal Comparer As IComparer) As Integer Implements IStructuralComparable.CompareTo
            Dim O = DirectCast(Other, VTuple(Of T1, T2, T3))

            Dim C = Comparer.Compare(Me._Item1, O._Item1)
            If C <> 0 Then
                Return C
            End If

            C = Comparer.Compare(Me._Item2, O._Item2)
            If C <> 0 Then
                Return C
            End If

            Return Comparer.Compare(Me._Item3, O._Item3)
        End Function

        Public Overloads Function Equals(ByVal Other As Object, ByVal Comparer As IEqualityComparer) As Boolean Implements IStructuralEquatable.Equals
            If Not TypeOf Other Is VTuple(Of T1, T2, T3) Then
                Return False
            End If
            Dim O = DirectCast(Other, VTuple(Of T1, T2, T3))
            Return Comparer.Equals(Me._Item1, O._Item1) AndAlso
               Comparer.Equals(Me._Item2, O._Item2) AndAlso
               Comparer.Equals(Me._Item3, O._Item3)
        End Function

        Public Overloads Function GetHashCode(ByVal Comparer As IEqualityComparer) As Integer Implements IStructuralEquatable.GetHashCode
            Return Utilities.CombineHashCodes(Comparer.GetHashCode(Me._Item1),
                                         Comparer.GetHashCode(Me._Item2),
                                         Comparer.GetHashCode(Me._Item3))
        End Function

        Public Shared Operator >(ByVal Left As VTuple(Of T1, T2, T3), ByVal Right As VTuple(Of T1, T2, T3)) As Boolean
            Return Left.CompareTo(Right) > 0
        End Operator

        Public Shared Operator <(ByVal Left As VTuple(Of T1, T2, T3), ByVal Right As VTuple(Of T1, T2, T3)) As Boolean
            Return Left.CompareTo(Right) < 0
        End Operator

        Public Shared Operator >=(ByVal Left As VTuple(Of T1, T2, T3), ByVal Right As VTuple(Of T1, T2, T3)) As Boolean
            Return Left.CompareTo(Right) >= 0
        End Operator

        Public Shared Operator <=(ByVal Left As VTuple(Of T1, T2, T3), ByVal Right As VTuple(Of T1, T2, T3)) As Boolean
            Return Left.CompareTo(Right) <= 0
        End Operator

        Public Shared Operator =(ByVal Left As VTuple(Of T1, T2, T3), ByVal Right As VTuple(Of T1, T2, T3)) As Boolean
            Return Left.Equals(Right)
        End Operator

        Public Shared Operator <>(ByVal Left As VTuple(Of T1, T2, T3), ByVal Right As VTuple(Of T1, T2, T3)) As Boolean
            Return Left.Equals(Right)
        End Operator

#Region "Item1 Property"
        Private ReadOnly _Item1 As T1

        Public ReadOnly Property Item1 As T1
            Get
                Return Me._Item1
            End Get
        End Property
#End Region

#Region "Item2 Property"
        Private ReadOnly _Item2 As T2

        Public ReadOnly Property Item2 As T2
            Get
                Return Me._Item2
            End Get
        End Property
#End Region

#Region "Item3 Property"
        Private ReadOnly _Item3 As T3

        Public ReadOnly Property Item3 As T3
            Get
                Return Me._Item3
            End Get
        End Property
#End Region

    End Structure

    Public Structure VTuple(Of T1, T2, T3, T4)
        Implements IComparable(Of VTuple(Of T1, T2, T3, T4)),
               IStructuralComparable,
               IStructuralEquatable

        Public Sub New(ByVal Item1 As T1, ByVal Item2 As T2, ByVal Item3 As T3, ByVal Item4 As T4)
            Me._Item1 = Item1
            Me._Item2 = Item2
            Me._Item3 = Item3
            Me._Item4 = Item4
        End Sub

        Public Overrides Function ToString() As String
            Return String.Concat("VTuple{", Me._Item1, ", ", Me._Item2, ", ", Me._Item3, ", ", Me._Item4, "}")
        End Function

        Public Function CompareTo(ByVal Other As VTuple(Of T1, T2, T3, T4)) As Integer Implements IComparable(Of VTuple(Of T1, T2, T3, T4)).CompareTo
            Return Me.CompareTo(Other, Comparer(Of Object).Default)
        End Function

        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            Return Me.Equals(Obj, EqualityComparer(Of Object).Default)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.GetHashCode(EqualityComparer(Of Object).Default)
        End Function

        Public Function CompareTo(ByVal Other As Object, ByVal Comparer As IComparer) As Integer Implements IStructuralComparable.CompareTo
            Dim O = DirectCast(Other, VTuple(Of T1, T2, T3, T4))

            Dim C = Comparer.Compare(Me._Item1, O._Item1)
            If C <> 0 Then
                Return C
            End If

            C = Comparer.Compare(Me._Item2, O._Item2)
            If C <> 0 Then
                Return C
            End If

            C = Comparer.Compare(Me._Item3, O._Item3)
            If C <> 0 Then
                Return C
            End If

            Return Comparer.Compare(Me._Item4, O._Item4)
        End Function

        Public Overloads Function Equals(ByVal Other As Object, ByVal Comparer As IEqualityComparer) As Boolean Implements IStructuralEquatable.Equals
            If Not TypeOf Other Is VTuple(Of T1, T2, T3, T4) Then
                Return False
            End If
            Dim O = DirectCast(Other, VTuple(Of T1, T2, T3, T4))
            Return Comparer.Equals(Me._Item1, O._Item1) AndAlso
               Comparer.Equals(Me._Item2, O._Item2) AndAlso
               Comparer.Equals(Me._Item3, O._Item3) AndAlso
               Comparer.Equals(Me._Item4, O._Item4)
        End Function

        Public Overloads Function GetHashCode(ByVal Comparer As IEqualityComparer) As Integer Implements IStructuralEquatable.GetHashCode
            Return Utilities.CombineHashCodes(Comparer.GetHashCode(Me._Item1),
                                         Comparer.GetHashCode(Me._Item2),
                                         Comparer.GetHashCode(Me._Item3),
                                         Comparer.GetHashCode(Me._Item4))
        End Function

        Public Shared Operator >(ByVal Left As VTuple(Of T1, T2, T3, T4), ByVal Right As VTuple(Of T1, T2, T3, T4)) As Boolean
            Return Left.CompareTo(Right) > 0
        End Operator

        Public Shared Operator <(ByVal Left As VTuple(Of T1, T2, T3, T4), ByVal Right As VTuple(Of T1, T2, T3, T4)) As Boolean
            Return Left.CompareTo(Right) < 0
        End Operator

        Public Shared Operator >=(ByVal Left As VTuple(Of T1, T2, T3, T4), ByVal Right As VTuple(Of T1, T2, T3, T4)) As Boolean
            Return Left.CompareTo(Right) >= 0
        End Operator

        Public Shared Operator <=(ByVal Left As VTuple(Of T1, T2, T3, T4), ByVal Right As VTuple(Of T1, T2, T3, T4)) As Boolean
            Return Left.CompareTo(Right) <= 0
        End Operator

        Public Shared Operator =(ByVal Left As VTuple(Of T1, T2, T3, T4), ByVal Right As VTuple(Of T1, T2, T3, T4)) As Boolean
            Return Left.Equals(Right)
        End Operator

        Public Shared Operator <>(ByVal Left As VTuple(Of T1, T2, T3, T4), ByVal Right As VTuple(Of T1, T2, T3, T4)) As Boolean
            Return Not Left.Equals(Right)
        End Operator

#Region "Item1 Property"
        Private ReadOnly _Item1 As T1

        Public ReadOnly Property Item1 As T1
            Get
                Return Me._Item1
            End Get
        End Property
#End Region

#Region "Item2 Property"
        Private ReadOnly _Item2 As T2

        Public ReadOnly Property Item2 As T2
            Get
                Return Me._Item2
            End Get
        End Property
#End Region

#Region "Item3 Property"
        Private ReadOnly _Item3 As T3

        Public ReadOnly Property Item3 As T3
            Get
                Return Me._Item3
            End Get
        End Property
#End Region

#Region "Item4 Property"
        Private ReadOnly _Item4 As T4

        Public ReadOnly Property Item4 As T4
            Get
                Return Me._Item4
            End Get
        End Property
#End Region

    End Structure

    Public Structure VTuple(Of T1, T2, T3, T4, T5)
        Implements IComparable(Of VTuple(Of T1, T2, T3, T4, T5)),
               IStructuralComparable,
               IStructuralEquatable

        Public Sub New(ByVal Item1 As T1, ByVal Item2 As T2, ByVal Item3 As T3, ByVal Item4 As T4, ByVal Item5 As T5)
            Me._Item1 = Item1
            Me._Item2 = Item2
            Me._Item3 = Item3
            Me._Item4 = Item4
            Me._Item5 = Item5
        End Sub

        Public Overrides Function ToString() As String
            Return String.Concat("VTuple{", Me._Item1, ", ", Me._Item2, ", ", Me._Item3, ", ", Me._Item4, ", ", Me._Item5, "}")
        End Function

        Public Function CompareTo(ByVal Other As VTuple(Of T1, T2, T3, T4, T5)) As Integer Implements IComparable(Of VTuple(Of T1, T2, T3, T4, T5)).CompareTo
            Return Me.CompareTo(Other, Comparer(Of Object).Default)
        End Function

        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            Return Me.Equals(Obj, EqualityComparer(Of Object).Default)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.GetHashCode(EqualityComparer(Of Object).Default)
        End Function

        Public Function CompareTo(ByVal Other As Object, ByVal Comparer As IComparer) As Integer Implements IStructuralComparable.CompareTo
            Dim O = DirectCast(Other, VTuple(Of T1, T2, T3, T4, T5))

            Dim C = Comparer.Compare(Me._Item1, O._Item1)
            If C <> 0 Then
                Return C
            End If

            C = Comparer.Compare(Me._Item2, O._Item2)
            If C <> 0 Then
                Return C
            End If

            C = Comparer.Compare(Me._Item3, O._Item3)
            If C <> 0 Then
                Return C
            End If

            C = Comparer.Compare(Me._Item4, O._Item4)
            If C <> 0 Then
                Return C
            End If

            Return Comparer.Compare(Me._Item5, O._Item5)
        End Function

        Public Overloads Function Equals(ByVal Other As Object, ByVal Comparer As IEqualityComparer) As Boolean Implements IStructuralEquatable.Equals
            If Not TypeOf Other Is VTuple(Of T1, T2, T3, T4, T5) Then
                Return False
            End If
            Dim O = DirectCast(Other, VTuple(Of T1, T2, T3, T4, T5))
            Return Comparer.Equals(Me._Item1, O._Item1) AndAlso
               Comparer.Equals(Me._Item2, O._Item2) AndAlso
               Comparer.Equals(Me._Item3, O._Item3) AndAlso
               Comparer.Equals(Me._Item4, O._Item4) AndAlso
               Comparer.Equals(Me._Item5, O._Item5)
        End Function

        Public Overloads Function GetHashCode(ByVal Comparer As IEqualityComparer) As Integer Implements IStructuralEquatable.GetHashCode
            Return Utilities.CombineHashCodes(Comparer.GetHashCode(Me._Item1),
                                         Comparer.GetHashCode(Me._Item2),
                                         Comparer.GetHashCode(Me._Item3),
                                         Comparer.GetHashCode(Me._Item4),
                                         Comparer.GetHashCode(Me._Item5))
        End Function

        Public Shared Operator >(ByVal Left As VTuple(Of T1, T2, T3, T4, T5), ByVal Right As VTuple(Of T1, T2, T3, T4, T5)) As Boolean
            Return Left.CompareTo(Right) > 0
        End Operator

        Public Shared Operator <(ByVal Left As VTuple(Of T1, T2, T3, T4, T5), ByVal Right As VTuple(Of T1, T2, T3, T4, T5)) As Boolean
            Return Left.CompareTo(Right) < 0
        End Operator

        Public Shared Operator >=(ByVal Left As VTuple(Of T1, T2, T3, T4, T5), ByVal Right As VTuple(Of T1, T2, T3, T4, T5)) As Boolean
            Return Left.CompareTo(Right) >= 0
        End Operator

        Public Shared Operator <=(ByVal Left As VTuple(Of T1, T2, T3, T4, T5), ByVal Right As VTuple(Of T1, T2, T3, T4, T5)) As Boolean
            Return Left.CompareTo(Right) <= 0
        End Operator

        Public Shared Operator =(ByVal Left As VTuple(Of T1, T2, T3, T4, T5), ByVal Right As VTuple(Of T1, T2, T3, T4, T5)) As Boolean
            Return Left.Equals(Right)
        End Operator

        Public Shared Operator <>(ByVal Left As VTuple(Of T1, T2, T3, T4, T5), ByVal Right As VTuple(Of T1, T2, T3, T4, T5)) As Boolean
            Return Not Left.Equals(Right)
        End Operator

#Region "Item1 Property"
        Private ReadOnly _Item1 As T1

        Public ReadOnly Property Item1 As T1
            Get
                Return Me._Item1
            End Get
        End Property
#End Region

#Region "Item2 Property"
        Private ReadOnly _Item2 As T2

        Public ReadOnly Property Item2 As T2
            Get
                Return Me._Item2
            End Get
        End Property
#End Region

#Region "Item3 Property"
        Private ReadOnly _Item3 As T3

        Public ReadOnly Property Item3 As T3
            Get
                Return Me._Item3
            End Get
        End Property
#End Region

#Region "Item4 Property"
        Private ReadOnly _Item4 As T4

        Public ReadOnly Property Item4 As T4
            Get
                Return Me._Item4
            End Get
        End Property
#End Region

#Region "Item5 Property"
        Private ReadOnly _Item5 As T5

        Public ReadOnly Property Item5 As T5
            Get
                Return Me._Item5
            End Get
        End Property
#End Region

    End Structure

End Namespace
