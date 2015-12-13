Imports System.Runtime.CompilerServices

Public Module CommonExtensions

#Region "CollectionUtils Group"
    <Extension()>
    Public Function EnumerateSplit(ByVal Str As String, ByVal Options As StringSplitOptions, ParamArray ByVal Chars As Char()) As StringSplitEnumerator
        Return New StringSplitEnumerator(Str, Options, Chars)
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
        Dim List1 As IList(Of T),
            List As IList

        If TypeOf Self Is IList(Of T) Then
            List1 = DirectCast(Self, IList(Of T))
            Return List1.Item(Utilities.Math.GetStaticRandom().Next(List1.Count))
        End If
        If TypeOf Self Is IList Then
            List = DirectCast(Self, IList)
            Return DirectCast(List.Item(Utilities.Math.GetStaticRandom().Next(List.Count)), T)
        End If

        Return Self.ElementAt(Utilities.Math.GetStaticRandom().Next(Self.Count()))
    End Function

    <Extension>
    Public Sub CopyTo(Of T)(ByVal Self As IEnumerable(Of T), ByVal Destination As IList(Of T), Optional ByVal Index As Integer = 0)
        If Destination.Count - Index < Self.Count() Then
            Throw New ArgumentException("There is not enough space on the destination to copy the collection.")
        End If

        For Each I As T In Self
            Destination.Item(Index) = I
            Index += 1
        Next
    End Sub

    <Extension>
    Public Sub AddRange(Of T)(ByVal Self As IList(Of T), ByVal Items As IEnumerable(Of T))
        For Each I As T In Items
            Self.Add(I)
        Next
    End Sub

    <Extension>
    Public Sub Sort(Of T)(ByVal Self As IList(Of T))
        Dim Sorter As MergeSorter(Of T)

        Sorter = New MergeSorter(Of T)()

        Sorter.Sort(Self)
    End Sub

    <Extension>
    Public Sub Sort(Of T)(ByVal Self As IList(Of T), ByVal Comparer As IComparer(Of T))
        Dim Sorter As MergeSorter(Of T)

        Sorter = New MergeSorter(Of T)()

        Sorter.Sort(Self, Comparer)
    End Sub

    <Extension()>
    Public Function AllToString(Of T)(ByVal Collection As IEnumerable(Of T)) As String
        Dim Res As Text.StringBuilder,
            Enumerator As IEnumerator(Of T)

        Res = New Text.StringBuilder("{")

        Enumerator = Collection.GetEnumerator()

        If Enumerator.MoveNext() Then
            Res.Append(Enumerator.Current)
        End If

        Do While Enumerator.MoveNext()
            Res.Append(", ").Append(Enumerator.Current)
        Loop

        Return Res.Append("}").ToString()
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
    Private Function GetCustomAttributeInternal(Of TAttribute As Attribute)(ByVal Self As Reflection.MemberInfo, Optional ByVal Inherit As Boolean = True) As TAttribute
        Return DirectCast(Self.GetCustomAttributeInternal(GetType(TAttribute), Inherit), TAttribute)
    End Function

    <Extension()>
    Private Function GetCustomAttributeInternal(ByVal Self As Reflection.MemberInfo, ByVal AttributeType As Type, Optional ByVal Inherit As Boolean = True) As Attribute
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
    Public Sub Write(ByVal Self As IO.Stream, ByVal Stream As IO.Stream)
        Dim Buffer As Byte()

        Buffer = New Byte(65535) {}

        Do
            Dim N = Stream.Read(Buffer, 0, Buffer.Length)
            If N = 0 Then
                Exit Do
            End If
            Self.Write(Buffer, 0, N)
        Loop
    End Sub
#End Region

End Module
