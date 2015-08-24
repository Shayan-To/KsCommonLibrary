Imports System.Runtime.CompilerServices

Public Module CommonExtensions

    <Extension>
    Public Function RandomElement(Of T)(ByVal Self As IEnumerable(Of T)) As T
        Dim List1 As IList(Of T),
            List As IList

        If TypeOf Self Is IList(Of T) Then
            List1 = DirectCast(Self, IList(Of T))
            Return List1.Item(Utilities.GetStaticRandom().Next(List1.Count))
        End If
        If TypeOf Self Is IList Then
            List = DirectCast(Self, IList)
            Return DirectCast(List.Item(Utilities.GetStaticRandom().Next(List.Count)), T)
        End If

        Return Self.ElementAt(Utilities.GetStaticRandom().Next(Self.Count()))
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

    <Extension()>
    Public Function ToGeneric(ByVal Self As IEnumerable) As IEnumerable(Of Object)
        Return Self.Cast(Of Object)()
    End Function

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

    <Extension>
    Public Function GetCenter(ByVal Self As Rect) As Point
        Return Self.Location + Self.Size.ToVector() / 2
    End Function

    <Extension>
    Public Sub MoveCenter(ByVal Self As Rect, ByVal Center As Point)
        Self.Location = Center - Self.Size.ToVector() / 2
    End Sub

End Module
