Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions

Namespace Common

    Public Module CommonExtensions

#Region "String Group"
        <Extension()>
        Public Function RegexEscape(str As String) As String
            Return Regex.Escape(str)
        End Function

        <Extension()>
        Public Function RegexIsMatch(input As String, pattern As String) As Boolean
            Return Regex.IsMatch(input, pattern)
        End Function

        <Extension()>
        Public Function RegexIsMatch(input As String, pattern As String, options As RegexOptions) As Boolean
            Return Regex.IsMatch(input, pattern, options)
        End Function

        <Extension()>
        Public Function RegexIsMatch(input As String, pattern As String, options As RegexOptions, matchTimeout As TimeSpan) As Boolean
            Return Regex.IsMatch(input, pattern, options, matchTimeout)
        End Function

        <Extension()>
        Public Function RegexMatch(input As String, pattern As String) As Match
            Return Regex.Match(input, pattern)
        End Function

        <Extension()>
        Public Function RegexMatch(input As String, pattern As String, options As RegexOptions) As Match
            Return Regex.Match(input, pattern, options)
        End Function

        <Extension()>
        Public Function RegexMatch(input As String, pattern As String, options As RegexOptions, matchTimeout As TimeSpan) As Match
            Return Regex.Match(input, pattern, options, matchTimeout)
        End Function

        <Extension()>
        Public Function RegexMatches(input As String, pattern As String) As MatchCollection
            Return Regex.Matches(input, pattern)
        End Function

        <Extension()>
        Public Function RegexMatches(input As String, pattern As String, options As RegexOptions) As MatchCollection
            Return Regex.Matches(input, pattern, options)
        End Function

        <Extension()>
        Public Function RegexMatches(input As String, pattern As String, options As RegexOptions, matchTimeout As TimeSpan) As MatchCollection
            Return Regex.Matches(input, pattern, options, matchTimeout)
        End Function

        <Extension()>
        Public Function RegexReplace(input As String, pattern As String, evaluator As MatchEvaluator) As String
            Return Regex.Replace(input, pattern, evaluator)
        End Function

        <Extension()>
        Public Function RegexReplace(input As String, pattern As String, replacement As String) As String
            Return Regex.Replace(input, pattern, replacement)
        End Function

        <Extension()>
        Public Function RegexReplace(input As String, pattern As String, evaluator As MatchEvaluator, options As RegexOptions) As String
            Return Regex.Replace(input, pattern, evaluator, options)
        End Function

        <Extension()>
        Public Function RegexReplace(input As String, pattern As String, replacement As String, options As RegexOptions) As String
            Return Regex.Replace(input, pattern, replacement, options)
        End Function

        <Extension()>
        Public Function RegexReplace(input As String, pattern As String, replacement As String, options As RegexOptions, matchTimeout As TimeSpan) As String
            Return Regex.Replace(input, pattern, replacement, options, matchTimeout)
        End Function

        <Extension()>
        Public Function RegexReplace(input As String, pattern As String, evaluator As MatchEvaluator, options As RegexOptions, matchTimeout As TimeSpan) As String
            Return Regex.Replace(input, pattern, evaluator, options, matchTimeout)
        End Function

        <Extension()>
        Public Function RegexSplit(input As String, pattern As String) As String()
            Return Regex.Split(input, pattern)
        End Function

        <Extension()>
        Public Function RegexSplit(input As String, pattern As String, options As RegexOptions) As String()
            Return Regex.Split(input, pattern, options)
        End Function

        <Extension()>
        Public Function RegexSplit(input As String, pattern As String, options As RegexOptions, matchTimeout As TimeSpan) As String()
            Return Regex.Split(input, pattern, options, matchTimeout)
        End Function

        <Extension()>
        Public Function RegexUnescape(str As String) As String
            Return Regex.Unescape(str)
        End Function
#End Region

#Region "Math Group"
        <Extension()>
        Public Function GetLeastCommonMultiple(ByVal Self As IEnumerable(Of Integer)) As Integer
            Dim Res = 0
            Dim First = True

            For Each I In Self
                If First Then
                    Res = I
                    First = False
                Else
                    Res = Utilities.Math.LeastCommonMultiple(Res, I)
                End If
            Next

            Return Res
        End Function

        <Extension()>
        Public Function GetLeastCommonMultiple(ByVal Self As IEnumerable(Of Long)) As Long
            Dim Res = 0L
            Dim First = True

            For Each I In Self
                If First Then
                    Res = I
                    First = False
                Else
                    Res = Utilities.Math.LeastCommonMultiple(Res, I)
                End If
            Next

            Return Res
        End Function
        <Extension()>
        Public Function GetGreatestCommonDivisor(ByVal Self As IEnumerable(Of Integer)) As Integer
            Dim Res = 0
            Dim First = True

            For Each I In Self
                If First Then
                    Res = I
                    First = False
                Else
                    Res = Utilities.Math.GreatestCommonDivisor(Res, I)
                End If
            Next

            Return Res
        End Function

        <Extension()>
        Public Function GetGreatestCommonDivisor(ByVal Self As IEnumerable(Of Long)) As Long
            Dim Res = 0L
            Dim First = True

            For Each I In Self
                If First Then
                    Res = I
                    First = False
                Else
                    Res = Utilities.Math.GreatestCommonDivisor(Res, I)
                End If
            Next

            Return Res
        End Function
#End Region

#Region "CollectionUtils Group"
        <Extension()>
        Public Sub Reverse(Of T)(ByVal Self As IList(Of T))
#Disable Warning BC40008 ' Type or member is obsolete
            Self.Reverse(0)
#Enable Warning BC40008 ' Type or member is obsolete
        End Sub

        <Obsolete(),
         Extension()>
        Public Sub Reverse(Of T)(ByVal Self As IList(Of T), ByVal Index As Integer, Optional ByVal Count As Integer = -1)
            If Count = -1 Then
                Count = Self.Count + Index
            Else
                Count += 2 * Index
            End If
            For I As Integer = Index To Index + (Count - 2 * Index) \ 2 - 1
                Dim C = Self.Item(I)
                Self.Item(I) = Self.Item(Count - 1 - I)
                Self.Item(Count - 1 - I) = Self.Item(I)
            Next
        End Sub

        <Extension()>
        Public Function AsList(ByVal Self As String) As StringAsListCollection
            Return New StringAsListCollection(Self)
        End Function

        <Extension()>
        Public Function Equals(Of T1, T2)(ByVal Self As IEnumerable(Of T1), ByVal Other As IEnumerable(Of T2), ByVal Comparison As Func(Of T1, T2, Boolean)) As Boolean
            Using Enum1 = Self.GetEnumerator(),
                  Enum2 = Other.GetEnumerator()
                Do
                    If Not Enum1.MoveNext() Then
                        Return Not Enum2.MoveNext()
                    End If
                    If Not Enum2.MoveNext() Then
                        Return False
                    End If
                    If Not Comparison.Invoke(Enum1.Current, Enum2.Current) Then
                        Return False
                    End If
                Loop
            End Using
        End Function

        <Extension()>
        Public Function Aggregate(Of TAggregate, T)(ByVal Self As IEnumerable(Of T), ByVal Seed As TAggregate, ByVal Func As Func(Of TAggregate, T, Integer, TAggregate)) As TAggregate
            Dim Ind = 0
            For Each I In Self
                Seed = Func.Invoke(Seed, I, Ind)
                Ind += 1
            Next
            Return Seed
        End Function

        <Extension()>
        Public Function Aggregate(Of T)(ByVal Self As IEnumerable(Of T), ByVal Func As Func(Of T, T, T), ByVal EmptyValue As T) As T
            Dim R As T = Nothing
            Dim Bl = True
            For Each I In Self
                If Bl Then
                    R = I
                    Bl = False
                    Continue For
                End If
                R = Func.Invoke(R, I)
            Next
            Return R
        End Function

        <Extension()>
        Public Function IndexOf(Of T)(ByVal Self As IList(Of T), ByVal Predicate As Func(Of T, Integer, Boolean), Optional ByVal StartIndex As Integer = 0) As Integer
            For I = StartIndex To Self.Count - 1
                If Predicate.Invoke(Self.Item(I), I) Then
                    Return I
                End If
            Next
            Return -1
        End Function

        <Extension()>
        Public Function LastIndexOf(Of T)(ByVal Self As IList(Of T), ByVal Predicate As Func(Of T, Integer, Boolean), Optional ByVal StartIndex As Integer = -1) As Integer
            For I = If(StartIndex <> -1, StartIndex, Self.Count - 1) To 0 Step -1
                If Predicate.Invoke(Self.Item(I), I) Then
                    Return I
                End If
            Next
            Return -1
        End Function

        <Extension()>
        Public Sub Move(Of T)(ByVal Self As IList(Of T), ByVal OldIndex As Integer, ByVal NewIndex As Integer)
            Dim Item = Self.Item(OldIndex)
            For I As Integer = OldIndex To NewIndex - 1
                Self.Item(I) = Self.Item(I + 1)
            Next
            For I As Integer = OldIndex To NewIndex + 1 Step -1
                Self.Item(I) = Self.Item(I - 1)
            Next
            Self.Item(NewIndex) = Item
        End Sub

        <Extension()>
        Public Function Resize(ByVal Array() As Byte, ByVal Length As Integer) As Byte()
            If Length <> Array.Length Then
                Dim R = New Byte(Length - 1) {}
                System.Array.Copy(Array, 0, R, 0, Math.Min(Length, Array.Length))
                Array = R
            End If

            Return Array
        End Function

        <Extension()>
        Public Function Resize(ByVal Array() As Byte, ByVal Offset As Integer, ByVal Length As Integer) As Byte()
            If Length <> Array.Length Or Offset <> 0 Then
                Dim R = New Byte(Length - 1) {}
                System.Array.Copy(Array, Offset, R, 0, Math.Min(Length, Array.Length - Offset))
                Array = R
            End If

            Return Array
        End Function

        <Extension()>
        Public Function BinarySearch(Of T)(ByVal Self As IReadOnlyList(Of T), ByVal Value As T) As VTuple(Of Integer, Integer)
            Return Self.BinarySearch(Value, Comparer(Of T).Default)
        End Function

        ''' <summary>
        ''' Gets the interval in which Value resides in inside a sorted list.
        ''' </summary>
        ''' <param name="Value">The value to look for.</param>
        ''' <returns>
        ''' The tuple (start index, length).
        ''' Start index being the index of fist occurrance of Value, and length being the count of its occurrances.
        ''' If no occurrance of Value has been found, start index will be at the first element larger than Value.
        ''' </returns>
        <Extension()>
        Public Function BinarySearch(Of T)(ByVal Self As IReadOnlyList(Of T), ByVal Value As T, ByVal Comp As IComparer(Of T)) As VTuple(Of Integer, Integer)
            Dim Count = Utilities.Math.LeastPowerOfTwoOnMin(Self.Count + 1) \ 2
            Dim Offset1 = -1

            Do While Count > 0
                If Offset1 + Count < Self.Count Then
                    Dim C = Comp.Compare(Self.Item(Offset1 + Count), Value)
                    If C < 0 Then
                        Offset1 += Count
                    ElseIf C = 0 Then
                        Exit Do
                    End If
                End If
                Count \= 2
            Loop

            Dim Offset2 = Offset1
            If Count > 0 Then
                ' This should have been done in the ElseIf block in the previous loop before the Exit statement.
                Offset2 += Count

                Do While Count > 1
                    Count \= 2
                    If Offset1 + Count < Self.Count Then
                        If Comp.Compare(Self.Item(Offset1 + Count), Value) < 0 Then
                            Offset1 += Count
                        End If
                    End If
                    If Offset2 + Count < Self.Count Then
                        If Comp.Compare(Self.Item(Offset2 + Count), Value) <= 0 Then
                            Offset2 += Count
                        End If
                    End If
                Loop
            End If

            Return VTuple.Create(Offset1 + 1, Offset2 - Offset1)
        End Function

        <Extension()>
        Public Function EnumerateSplit(ByVal Str As String, ByVal Options As StringSplitOptions, ParamArray ByVal Chars As Char()) As StringSplitEnumerator
            Return New StringSplitEnumerator(Str, Options, Chars)
        End Function

        <Extension()>
        Public Function CastAsList(Of T)(ByVal Self As IList) As IReadOnlyList(Of T)
            Dim R = TryCast(Self, IReadOnlyList(Of T))
            If R IsNot Nothing Then
                Return R
            End If
            Return New CastAsListCollection(Of T)(Self)
        End Function

        <Extension()>
        Public Function SelectAsList(Of TIn, TOut)(ByVal Self As IReadOnlyList(Of TIn), ByVal Func As Func(Of TIn, TOut)) As SelectAsListCollection(Of TIn, TOut)
            Return New SelectAsListCollection(Of TIn, TOut)(Self, Func)
        End Function

        <Extension()>
        Public Function SelectAsList(Of TIn, TOut)(ByVal Self As IReadOnlyList(Of TIn), ByVal Func As Func(Of TIn, Integer, TOut)) As SelectAsListCollection(Of TIn, TOut)
            Return New SelectAsListCollection(Of TIn, TOut)(Self, Func)
        End Function

        <Extension()>
        Public Function SelectAsNotifyingList(Of TIn, TOut)(ByVal Self As IReadOnlyList(Of TIn), ByVal Func As Func(Of TIn, TOut)) As SelectAsNotifyingListCollection(Of TIn, TOut)
            Return New SelectAsNotifyingListCollection(Of TIn, TOut)(Self, Func)
        End Function

        <Extension()>
        Public Function SelectAsNotifyingList(Of TIn, TOut)(ByVal Self As IReadOnlyList(Of TIn), ByVal Func As Func(Of TIn, Integer, TOut)) As SelectAsNotifyingListCollection(Of TIn, TOut)
            Return New SelectAsNotifyingListCollection(Of TIn, TOut)(Self, Func)
        End Function

        <Extension()>
        Public Function DistinctNeighbours(Of T)(ByVal Self As IEnumerable(Of T)) As IEnumerable(Of T)
            Return DistinctNeighbours(Self, EqualityComparer(Of T).Default)
        End Function

        <Extension()>
        Public Iterator Function DistinctNeighbours(Of T)(ByVal Self As IEnumerable(Of T), ByVal Comparer As IEqualityComparer(Of T)) As IEnumerable(Of T)
            Dim Bl = True
            Dim P As T = Nothing

            For Each I In Self
                If Bl Then
                    Yield I
                    P = I
                    Bl = False
                ElseIf Not Comparer.Equals(P, I) Then
                    Yield I
                    P = I
                End If
            Next
        End Function

        <Extension>
        Public Function RandomElement(Of T)(ByVal Self As IEnumerable(Of T)) As T
            Dim Rnd = DefaultCacher(Of Random).Value

            If TypeOf Self Is IList(Of T) Then
                Dim L = DirectCast(Self, IList(Of T))
                Return L.Item(Rnd.Next(L.Count))
            End If

            If TypeOf Self Is IList Then
                Dim L = DirectCast(Self, IList)
                Return DirectCast(L.Item(Rnd.Next(L.Count)), T)
            End If

            Return Self.ElementAt(Rnd.Next(Self.Count()))
        End Function

        <Extension>
        Public Sub CopyTo(Of T)(ByVal Self As IEnumerable(Of T), ByVal Destination As IList(Of T), Optional ByVal Index As Integer = 0, Optional ByVal Count As Integer = -1)
            'If Destination.Count - Index < Self.Count() Then
            '    Throw New ArgumentException("There is not enough space on the destination to copy the collection.")
            'End If
            Verify.TrueArg(Count >= -1, "Count")

            If Count = -1 Then
                For Each I In Self
                    Destination.Item(Index) = I
                    Index += 1
                Next
            Else
                If Count > 0 Then
                    For Each I In Self
                        Destination.Item(Index) = I
                        Index += 1
                        Count -= 1
                        If Count = 0 Then
                            Exit For
                        End If
                    Next
                End If
            End If
        End Sub

        <Extension>
        Public Sub AddRange(Of T)(ByVal Self As IList(Of T), ByVal Items As IEnumerable(Of T))
            For Each I As T In Items
                Self.Add(I)
            Next
        End Sub

        <Extension>
        Public Sub AddRange(ByVal Self As IList, ByVal Items As IEnumerable)
            For Each I In Items
                Self.Add(I)
            Next
        End Sub

        <Extension>
        Public Sub Sort(Of T)(ByVal Self As IList(Of T))
            DefaultCacher(Of MergeSorter(Of T)).Value.Sort(Self)
        End Sub

        <Extension>
        Public Sub Sort(Of T)(ByVal Self As IList(Of T), ByVal Comparer As IComparer(Of T))
            DefaultCacher(Of MergeSorter(Of T)).Value.Sort(Self, Comparer)
        End Sub

        <Extension()>
        Public Function AllToString(Of T)(ByVal Self As IEnumerable(Of T)) As String
            Dim Res = New Text.StringBuilder("{")

            Dim Bl = False
            For Each I In Self
                If Bl Then
                    Res.Append(", ")
                End If
                Bl = True

                Res.Append(I)
            Next

            Return Res.Append("}").ToString()
        End Function

        <Extension()>
        Public Function AsReadOnly(Of T)(ByVal Self As IList(Of T)) As ReadOnlyListWrapper(Of T)
            Return New ReadOnlyListWrapper(Of T)(Self)
        End Function

        <Extension()>
        Public Function AsReadOnly(Of TKey, TValue)(ByVal Self As IDictionary(Of TKey, TValue)) As ObjectModel.ReadOnlyDictionary(Of TKey, TValue)
            Return New ObjectModel.ReadOnlyDictionary(Of TKey, TValue)(Self)
        End Function

        <Extension()>
        Public Function AsReadOnly(Of T)(ByVal Self As ICollection(Of T)) As ReadOnlyCollectionWrapper(Of T)
            Return New ReadOnlyCollectionWrapper(Of T)(Self)
        End Function

        <Extension()>
        Public Iterator Function Concat(Of T)(ByVal Self As IEnumerable(Of T), ByVal Element As T) As IEnumerable(Of T)
            For Each I In Self
                Yield I
            Next
            Yield Element
        End Function

        <Extension()>
        Public Iterator Function ConcatBacked(Of T)(ByVal Self As IEnumerable(Of T), ByVal Element As T) As IEnumerable(Of T)
            Yield Element
            For Each I In Self
                Yield I
            Next
        End Function

        <Extension()>
        Public Function Any(ByVal Self As IEnumerable(Of Boolean)) As Boolean
            For Each I In Self
                If I Then
                    Return True
                End If
            Next
            Return False
        End Function

        <Extension()>
        Public Function All(ByVal Self As IEnumerable(Of Boolean)) As Boolean
            For Each I In Self
                If Not I Then
                    Return False
                End If
            Next
            Return True
        End Function

        <Extension()>
        Public Function MaxOrDefault(Of T)(ByVal Self As IEnumerable(Of T), ByVal Selector As Func(Of T, Double?)) As T
            Dim M As Double? = Nothing
            Dim R As T = Nothing
            For Each I In Self
                Dim V = Selector.Invoke(I)
                If V.HasValue And Not M >= V Then
                    M = V
                    R = I
                End If
            Next
            Return R
        End Function

        <Extension()>
        Public Function MinOrDefault(Of T)(ByVal Self As IEnumerable(Of T), ByVal Selector As Func(Of T, Double?)) As T
            Dim M As Double? = Nothing
            Dim R As T = Nothing
            For Each I In Self
                Dim V = Selector.Invoke(I)
                If V.HasValue And Not M <= V Then
                    M = V
                    R = I
                End If
            Next
            Return R
        End Function

        <Extension()>
        Public Function MaxOrDefault(Of T)(ByVal Self As IEnumerable(Of T), ByVal Selector As Func(Of T, Long?)) As T
            Dim M As Long? = Nothing
            Dim R As T = Nothing
            For Each I In Self
                Dim V = Selector.Invoke(I)
                If V.HasValue And If(M > V, True) Then
                    M = V
                    R = I
                End If
            Next
            Return R
        End Function

        <Extension()>
        Public Function MinOrDefault(Of T)(ByVal Self As IEnumerable(Of T), ByVal Selector As Func(Of T, Long?)) As T
            Dim M As Long? = Nothing
            Dim R As T = Nothing
            For Each I In Self
                Dim V = Selector.Invoke(I)
                If V.HasValue And If(M > V, True) Then
                    M = V
                    R = I
                End If
            Next
            Return R
        End Function
#End Region

#Region "Geometry Group"
#Region "Elementary"
        <Extension>
        Public Function ToVector(ByVal Self As Size) As Vector
            Return New Vector(Self.Width, Self.Height)
        End Function

        <Extension>
        Public Function ToVector(ByVal Self As Point) As Vector
            Return New Vector(Self.X, Self.Y)
        End Function

        <Extension>
        Public Function ToSize(ByVal Self As Vector) As Size
            Return New Size(Self.X, Self.Y)
        End Function

        <Extension>
        Public Function ToSizeSafe(ByVal Self As Vector) As Size
            If Self.X < 0 Then
                Self.X = 0
            End If
            If Self.Y < 0 Then
                Self.Y = 0
            End If
            Return New Size(Self.X, Self.Y)
        End Function

        <Extension>
        Public Function ToPoint(ByVal Self As Vector) As Point
            Return New Point(Self.X, Self.Y)
        End Function
#End Region

#Region "RectangleFitting"
        <Extension>
        Public Function GetLargestFitOf(ByVal Self As Rect, ByVal Size As Size) As VTuple(Of Rect, Boolean?)
            Dim Bl As Boolean?

            If Self.IsEmpty Then
                Return VTuple.Create(Rect.Empty, Bl)
            End If
            If Size.Width = 0 And Size.Height = 0 Then
                Return VTuple.Create(New Rect(Self.Location + Self.Size.ToVector() / 2, New Size()), Bl)
            End If
            If Size.Width = 0 Then
                If Self.Height = 0 Then
                    Return VTuple.Create(New Rect(Self.Location + Self.Size.ToVector() / 2, New Size()), Bl)
                End If
                Bl = False
            End If
            If Size.Height = 0 Then
                If Self.Width = 0 Then
                    Return VTuple.Create(New Rect(Self.Location + Self.Size.ToVector() / 2, New Size()), Bl)
                End If
                Bl = True
            End If
            If Self.Width = 0 Or Self.Height = 0 Then
                Return VTuple.Create(New Rect(Self.Location + Self.Size.ToVector() / 2, New Size()), Bl)
            End If

            If Not Bl.HasValue Then
                Dim R1 = Self.Width / Self.Height
                Dim R2 = Size.Width / Size.Height
                Bl = R1 < R2
            End If

            Dim Loc = Self.Location
            If Bl.Value Then
                Dim Sz = New Size(Self.Width, Self.Width / Size.Width * Size.Height)
                Loc += New Vector(0, (Self.Height - Sz.Height) / 2)
                Return VTuple.Create(New Rect(Loc, Sz), Bl)
            Else
                Dim Sz = New Size(Self.Height / Size.Height * Size.Width, Self.Height)
                Loc += New Vector((Self.Width - Sz.Width) / 2, 0)
                Return VTuple.Create(New Rect(Loc, Sz), Bl)
            End If
        End Function

        <Extension()>
        Public Function GetSmallestBoundOf(ByVal Self As Rect, ByVal Size As Size) As VTuple(Of Rect, Boolean?)
            Dim Bl As Boolean?

            If Self.IsEmpty Then
                Return VTuple.Create(Rect.Empty, Bl)
            End If
            If Self.Size = New Size() Then
                Return VTuple.Create(New Rect(Self.Location, New Size()), Bl)
            End If
            If Self.Width = 0 Then
                If Size.Height = 0 Then
                    Return VTuple.Create(Rect.Empty, Bl)
                End If
                Bl = False
            End If
            If Self.Height = 0 Then
                If Size.Width = 0 Then
                    Return VTuple.Create(Rect.Empty, Bl)
                End If
                Bl = True
            End If
            If Size.Width = 0 Or Size.Height = 0 Then
                Return VTuple.Create(Rect.Empty, Bl)
            End If

            If Not Bl.HasValue Then
                Dim R1 = Self.Width / Self.Height
                Dim R2 = Size.Width / Size.Height
                Bl = R1 > R2
            End If

            Dim Loc = Self.Location
            If Bl.Value Then
                Dim Sz = New Size(Self.Width, Self.Width / Size.Width * Size.Height)
                Loc += New Vector(0, (Self.Height - Sz.Height) / 2)
                Return VTuple.Create(New Rect(Loc, Sz), Bl)
            Else
                Dim Sz = New Size(Self.Height / Size.Height * Size.Width, Self.Height)
                Loc += New Vector((Self.Width - Sz.Width) / 2, 0)
                Return VTuple.Create(New Rect(Loc, Sz), Bl)
            End If
        End Function

        <Extension>
        Public Function GetInnerBoundedSquare(ByVal Self As Rect) As Rect
            Throw New NotImplementedException()
        End Function

        <Extension>
        Public Function GetOuterBoundingSquare(ByVal Self As Rect) As Rect
            Throw New NotImplementedException()
        End Function
#End Region

#Region "ChangeCoordinateSystem Logic"
        <Extension>
        Public Function FromLocal(ByVal Self As Rect, ByVal Point As Point) As Point
            Return Point + Self.Location.ToVector()
        End Function

        <Extension>
        Public Function ToLocal(ByVal Self As Rect, ByVal Point As Point) As Point
            Return Point - Self.Location.ToVector()
        End Function

        <Extension>
        Public Function FromLocal01(ByVal Self As Rect, ByVal Point As Point) As Point
            Point = New Point(Point.X * Self.Width, Point.Y * Self.Height)
            Point += Self.Location.ToVector()
            Return Point
        End Function

        <Extension>
        Public Function ToLocal01(ByVal Self As Rect, ByVal Point As Point) As Point
            Point -= Self.Location.ToVector()
            Point = New Point(Point.X / Self.Width, Point.Y / Self.Height)
            Return Point
        End Function
#End Region

        <Extension>
        Public Function GetCenter(ByVal Self As Rect) As Point
            Return Self.Location + Self.Size.ToVector() / 2
        End Function

        <Extension>
        Public Sub MoveCenter(ByVal Self As Rect, ByVal Center As Point)
            Self.Location = Center - Self.Size.ToVector() / 2
        End Sub
#End Region

#Region "Reflection Group"
        ''' <summary>
        ''' First element is the given type.
        ''' </summary>
        <Extension()>
        Public Iterator Function GetBaseTypes(ByVal Self As Type) As IEnumerable(Of Type)
            Do
                Yield Self
                Self = Self.BaseType
            Loop Until Self Is Nothing
        End Function

        <Extension()>
        Public Function GetRecursiveReferencedAssemblies(ByVal Assembly As Reflection.Assembly) As IEnumerable(Of Reflection.Assembly)
            Dim Helper = CecilHelper.Instance
            Return Helper.GetRawRecursiveReferencedAssemblies(Helper.Convert(Assembly)).Select(Function(A) Helper.Convert(A))
        End Function

        <Extension()>
        Public Function GetAllReferencedAssemblies(ByVal Assembly As Reflection.Assembly) As IEnumerable(Of Reflection.Assembly)
            Dim Helper = CecilHelper.Instance
            Return Helper.GetRawReferencedAssemblyNames(Helper.Convert(Assembly)).Select(Function(A) Helper.Convert(A))
        End Function

        <Extension()>
        Public Function CreateInstance(ByVal Self As Type) As Object
            Return Self.GetConstructor(Utilities.Typed(Of Type).EmptyArray).Invoke(Utilities.Typed(Of Object).EmptyArray)
        End Function

        <Extension()>
        Public Function CreateInstance(Of T1)(ByVal Self As Type, ByVal Arg1 As T1) As Object
            Return Self.GetConstructor({GetType(T1)}).Invoke({Arg1})
        End Function

        <Extension()>
        Public Function CreateInstance(Of T1, T2)(ByVal Self As Type, ByVal Arg1 As T1, ByVal Arg2 As T2) As Object
            Return Self.GetConstructor({GetType(T1), GetType(T2)}).Invoke({Arg1, Arg2})
        End Function

        <Extension()>
        Public Function CreateInstance(Of T1, T2, T3)(ByVal Self As Type, ByVal Arg1 As T1, ByVal Arg2 As T2, ByVal Arg3 As T3) As Object
            Return Self.GetConstructor({GetType(T1), GetType(T2), GetType(T3)}).Invoke({Arg1, Arg2, Arg3})
        End Function

        <Extension()>
        Public Function CreateInstance(Of T1, T2, T3, T4)(ByVal Self As Type, ByVal Arg1 As T1, ByVal Arg2 As T2, ByVal Arg3 As T3, ByVal Arg4 As T4) As Object
            Return Self.GetConstructor({GetType(T1), GetType(T2), GetType(T3), GetType(T4)}).Invoke({Arg1, Arg2, Arg3, Arg4})
        End Function

        <Extension()>
        Public Function RunSharedMethod(ByVal Self As Type, ByVal Name As String) As Object
            Return Self.GetMethod(Name, Utilities.Typed(Of Type).EmptyArray).Invoke(Nothing, Utilities.Typed(Of Object).EmptyArray)
        End Function

        <Extension()>
        Public Function RunSharedMethod(Of T1)(ByVal Self As Type, ByVal Name As String, ByVal Arg1 As T1) As Object
            Return Self.GetMethod(Name, {GetType(T1)}).Invoke(Nothing, {Arg1})
        End Function

        <Extension()>
        Public Function RunSharedMethod(Of T1, T2)(ByVal Self As Type, ByVal Name As String, ByVal Arg1 As T1, ByVal Arg2 As T2) As Object
            Return Self.GetMethod(Name, {GetType(T1), GetType(T2)}).Invoke(Nothing, {Arg1, Arg2})
        End Function

        <Extension()>
        Public Function RunSharedMethod(Of T1, T2, T3)(ByVal Self As Type, ByVal Name As String, ByVal Arg1 As T1, ByVal Arg2 As T2, ByVal Arg3 As T3) As Object
            Return Self.GetMethod(Name, {GetType(T1), GetType(T2), GetType(T3)}).Invoke(Nothing, {Arg1, Arg2, Arg3})
        End Function

        <Extension()>
        Public Function RunSharedMethod(Of T1, T2, T3, T4)(ByVal Self As Type, ByVal Name As String, ByVal Arg1 As T1, ByVal Arg2 As T2, ByVal Arg3 As T3, ByVal Arg4 As T4) As Object
            Return Self.GetMethod(Name, {GetType(T1), GetType(T2), GetType(T3), GetType(T4)}).Invoke(Nothing, {Arg1, Arg2, Arg3, Arg4})
        End Function

        <Extension()>
        Public Function RunMethod(ByVal Self As Object, ByVal Name As String) As Object
            Return Self.GetType().GetMethod(Name, Utilities.Typed(Of Type).EmptyArray).Invoke(Self, Utilities.Typed(Of Object).EmptyArray)
        End Function

        <Extension()>
        Public Function RunMethod(Of T1)(ByVal Self As Object, ByVal Name As String, ByVal Arg1 As T1) As Object
            Return Self.GetType().GetMethod(Name, {GetType(T1)}).Invoke(Self, {Arg1})
        End Function

        <Extension()>
        Public Function RunMethod(Of T1, T2)(ByVal Self As Object, ByVal Name As String, ByVal Arg1 As T1, ByVal Arg2 As T2) As Object
            Return Self.GetType().GetMethod(Name, {GetType(T1), GetType(T2)}).Invoke(Self, {Arg1, Arg2})
        End Function

        <Extension()>
        Public Function RunMethod(Of T1, T2, T3)(ByVal Self As Object, ByVal Name As String, ByVal Arg1 As T1, ByVal Arg2 As T2, ByVal Arg3 As T3) As Object
            Return Self.GetType().GetMethod(Name, {GetType(T1), GetType(T2), GetType(T3)}).Invoke(Self, {Arg1, Arg2, Arg3})
        End Function

        <Extension()>
        Public Function RunMethod(Of T1, T2, T3, T4)(ByVal Self As Object, ByVal Name As String, ByVal Arg1 As T1, ByVal Arg2 As T2, ByVal Arg3 As T3, ByVal Arg4 As T4) As Object
            Return Self.GetType().GetMethod(Name, {GetType(T1), GetType(T2), GetType(T3), GetType(T4)}).Invoke(Self, {Arg1, Arg2, Arg3, Arg4})
        End Function

        <Extension()>
        Public Function GetSharedFieldValue(Of T)(ByVal Self As Type, ByVal Name As String) As T
            Return DirectCast(Self.GetField(Name).GetValue(Nothing), T)
        End Function

        <Extension()>
        Public Function GetFieldValue(Of T)(ByVal Self As Object, ByVal Name As String) As T
            Return DirectCast(Self.GetType().GetField(Name).GetValue(Self), T)
        End Function

        <Extension()>
        Private Function GetCustomAttributeInternal(Of TAttribute As Attribute)(ByVal Self As Reflection.MemberInfo, Optional ByVal Inherit As Boolean = True) As TAttribute
            Return DirectCast(Self.GetCustomAttributeInternal(GetType(TAttribute), Inherit), TAttribute)
        End Function

        <Extension()>
        Private Function GetCustomAttributeInternal(ByVal Self As Reflection.MemberInfo, ByVal AttributeType As Type, Optional ByVal Inherit As Boolean = True) As Attribute
            Return DirectCast(Self.GetCustomAttributes(AttributeType, Inherit).FirstOrDefault(), Attribute)
        End Function

        <Extension()>
        Private Function GetCustomAttributeInternal(Of TAttribute As Attribute)(ByVal Self As Reflection.Assembly, Optional ByVal Inherit As Boolean = True) As TAttribute
            Return DirectCast(Self.GetCustomAttributeInternal(GetType(TAttribute), Inherit), TAttribute)
        End Function

        <Extension()>
        Private Function GetCustomAttributeInternal(ByVal Self As Reflection.Assembly, ByVal AttributeType As Type, Optional ByVal Inherit As Boolean = True) As Attribute
            Return DirectCast(Self.GetCustomAttributes(AttributeType, Inherit).FirstOrDefault(), Attribute)
        End Function

        <Extension()>
        Public Function GetCustomAttribute(Of TAttribute As Attribute)(ByVal Self As Reflection.MemberInfo, Optional ByVal Inherit As Boolean = True) As TAttribute
            Dim AttributeType = GetType(TAttribute)
            Dim Usage = AttributeType.GetCustomAttributeInternal(Of AttributeUsageAttribute)()
            If Usage IsNot Nothing AndAlso Usage.AllowMultiple Then
                Throw New ArgumentException("The attribute should not allow multiple.")
            End If

            Return Self.GetCustomAttributeInternal(Of TAttribute)(Inherit)
        End Function

        <Extension()>
        Public Function GetCustomAttribute(ByVal Self As Reflection.MemberInfo, ByVal AttributeType As Type, Optional ByVal Inherit As Boolean = True) As Attribute
            Dim Usage = AttributeType.GetCustomAttributeInternal(Of AttributeUsageAttribute)()
            If Usage IsNot Nothing AndAlso Usage.AllowMultiple Then
                Throw New ArgumentException("The attribute should not allow multiple.")
            End If

            Return Self.GetCustomAttributeInternal(AttributeType, Inherit)
        End Function

        <Extension()>
        Public Function GetCustomAttribute(Of TAttribute As Attribute)(ByVal Self As Reflection.Assembly, Optional ByVal Inherit As Boolean = True) As TAttribute
            Dim AttributeType = GetType(TAttribute)
            Dim Usage = AttributeType.GetCustomAttributeInternal(Of AttributeUsageAttribute)()
            If Usage IsNot Nothing AndAlso Usage.AllowMultiple Then
                Throw New ArgumentException("The attribute should not allow multiple.")
            End If

            Return Self.GetCustomAttributeInternal(Of TAttribute)(Inherit)
        End Function

        <Extension()>
        Public Function GetCustomAttribute(ByVal Self As Reflection.Assembly, ByVal AttributeType As Type, Optional ByVal Inherit As Boolean = True) As Attribute
            Dim Usage = AttributeType.GetCustomAttributeInternal(Of AttributeUsageAttribute)()
            If Usage IsNot Nothing AndAlso Usage.AllowMultiple Then
                Throw New ArgumentException("The attribute should not allow multiple.")
            End If

            Return Self.GetCustomAttributeInternal(AttributeType, Inherit)
        End Function

        <Extension()>
        Public Iterator Function WithCustomAttribute(Of TAttribute As Attribute)(ByVal Types As IEnumerable(Of Type)) As IEnumerable(Of VTuple(Of Type, TAttribute))
            Dim Type = GetType(TAttribute)

            Dim Usage = Type.GetCustomAttributeInternal(Of AttributeUsageAttribute)()
            If Usage IsNot Nothing Then
                If Usage.AllowMultiple Then
                    Throw New ArgumentException("The attribute should not allow multiple.")
                End If
                If (Usage.ValidOn And (AttributeTargets.Class Or AttributeTargets.Delegate Or AttributeTargets.Enum Or AttributeTargets.GenericParameter Or AttributeTargets.Interface Or AttributeTargets.Module Or AttributeTargets.Struct)) = 0 Then
                    Throw New ArgumentException("The attribute is not valid on types.")
                End If
            End If

            For Each T In Types
                Dim Attribute = T.GetCustomAttributeInternal(Of TAttribute)()
                If Attribute IsNot Nothing Then
                    Yield VTuple.Create(T, Attribute)
                End If
            Next
        End Function

        <Extension()>
        Public Iterator Function WithCustomAttribute(Of TAttribute As Attribute)(ByVal Methods As IEnumerable(Of Reflection.MethodInfo)) As IEnumerable(Of VTuple(Of Reflection.MethodInfo, TAttribute))
            Dim Type = GetType(TAttribute)

            Dim Usage = Type.GetCustomAttributeInternal(Of AttributeUsageAttribute)()
            If Usage IsNot Nothing Then
                If Usage.AllowMultiple Then
                    Throw New ArgumentException("The attribute should not allow multiple.")
                End If
                If (Usage.ValidOn And AttributeTargets.Method) <> AttributeTargets.Method Then
                    Throw New ArgumentException("The attribute is not valid on methods.")
                End If
            End If

            For Each M In Methods
                Dim Attribute = M.GetCustomAttributeInternal(Of TAttribute)()
                If Attribute IsNot Nothing Then
                    Yield VTuple.Create(M, Attribute)
                End If
            Next
        End Function
#End Region

#Region "IO Group"
        <Extension()>
        Public Function ReadAll(ByVal Self As IO.Stream, ByVal Buffer As Byte(), ByVal Offset As Integer, ByVal Length As Integer) As Integer
            Return Utilities.IO.ReadAll(Function(B, O, L) Self.Read(B, O, L), Buffer, Offset, Length)
        End Function

        <Extension()>
        Public Function ReadToEnd(ByVal Self As IO.Stream) As Byte()
            'Dim Length = -1
            'If Self.CanSeek Then
            '    Length = Self.Length - Self.Position
            'End If
            ' ToDo Use Length to optimize this code.

            Dim Arrs = New List(Of Byte())()

            Dim BufLength = 8192

            Dim N As Integer
            Dim Buf As Byte()

            Do
                Buf = New Byte(BufLength - 1) {}
                N = Self.ReadAll(Buf, 0, Buf.Length)
                If N < Buf.Length Then
                    Exit Do
                End If
                Arrs.Add(Buf)
            Loop

            Dim Res = New Byte(Arrs.Count * BufLength + N - 1) {}
            Dim Offset = 0
            For Each A In Arrs
                A.CopyTo(Res, Offset)
                Offset += BufLength
            Next
            Array.Copy(Buf, 0, Res, Offset, N)

            Return Res
        End Function

        <Extension()>
        Public Function Write(ByVal Self As IO.Stream, ByVal Stream As IO.Stream, Optional ByVal Length As Integer = -1) As Integer
            Dim Buffer As Byte()

            Buffer = New Byte(65535) {}

            Dim N As Integer
            Dim Total = 0
            Do
                If Length = -1 Then
                    N = Stream.Read(Buffer, 0, Buffer.Length)
                Else
                    N = Stream.Read(Buffer, 0, Math.Min(Length, Buffer.Length))
                End If

                If N = 0 Then
                    Exit Do
                End If

                Self.Write(Buffer, 0, N)
                Total += N

                If Length <> -1 Then
                    Length -= N
                    If Length = 0 Then
                        Exit Do
                    End If
                End If
            Loop

            Return Total
        End Function
#End Region

        <Extension()>
        Public Function NothingIfEmpty(Of T As ICollection)(ByVal Self As T) As T
            If Self.Count = 0 Then
                Return Nothing
            End If
            Return Self
        End Function

        <Extension()>
        Public Function NothingIfEmpty(ByVal Self As String) As String
            If Self.Length = 0 Then
                Return Nothing
            End If
            Return Self
        End Function

        <Extension()>
        Public Function Implies(ByVal B1 As Boolean, ByVal B2 As Boolean) As Boolean
            Return Not B1 Or B2
        End Function

    End Module

End Namespace
