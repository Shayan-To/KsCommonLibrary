Imports System.Drawing
Imports System.Windows.Forms

Namespace Common

    Public Class TestingVisualizer

        Public Sub New()
            Me.FormTop = 0
            Me.FormLeft = 0
            Me.FormMargin = 35
            Me.FormWidth = 900
            Me.FormHeight = 600

            Me.XStart = 0
            Me.XLength = 1
            Me.YStart = 0
            Me.YLength = 1
        End Sub

        Public Sub Initialize(ByVal BorderRectangleColor As Color)
            'Me.Form?.Dispose()
            Dim Bl = Me.Form Is Nothing
            If Bl Then
                Me.Form = New Form()
            End If
            With Me.Form
                .ClientSize = New Size(Me.FormWidth + Me.FormMargin * 2,
                                   Me.FormHeight + Me.FormMargin * 2)
                .Location = New Point(Me.FormLeft, Me.FormTop)
                .StartPosition = FormStartPosition.Manual
            End With
            If Bl Then
                Me.Form.Show()
                Me.Graphics = Graphics.FromHwnd(Me.Form.Handle)
            End If

            Me.FIntervals(Orientation.X) = New Interval(FormMargin, FormWidth)
            Me.FIntervals(Orientation.Y) = New Interval(FormMargin + FormHeight, -FormHeight)
            Me.Intervals(Orientation.X) = New Interval(XStart, XLength)
            Me.Intervals(Orientation.Y) = New Interval(YStart, YLength)

            Me.BorderRectangleColor = BorderRectangleColor

            Me.Clear()
        End Sub

        Public Sub Clear()
            Dim FIX = Me.FIntervals(Orientation.X)
            Dim FIY = Me.FIntervals(Orientation.Y)

            Me.Graphics.Clear(Me.Form.BackColor)
            Using Pen = New Pen(ConvertColor(BorderRectangleColor))
                Me.Graphics.DrawLine(Pen, CreatePoint(FIX.Start, FIY.Start, Orientation.X), CreatePoint(FIX.Start + FIX.Length, FIY.Start, Orientation.X))
                Me.Graphics.DrawLine(Pen, CreatePoint(FIX.Start, FIY.Start, Orientation.X), CreatePoint(FIX.Start, FIY.Start + FIY.Length, Orientation.X))
                Me.Graphics.DrawLine(Pen, CreatePoint(FIX.Start + FIX.Length, FIY.Start + FIY.Length, Orientation.X), CreatePoint(FIX.Start + FIX.Length, FIY.Start, Orientation.X))
                Me.Graphics.DrawLine(Pen, CreatePoint(FIX.Start + FIX.Length, FIY.Start + FIY.Length, Orientation.X), CreatePoint(FIX.Start, FIY.Start + FIY.Length, Orientation.X))
            End Using
        End Sub

        Public Sub DrawFunction(ByVal Color As Color, ByVal Ys As IReadOnlyList(Of Double), ByVal XStart As Double, XStep As Double)
            Me.DrawFunction(Color, Ys.SelectAsList(Function(Y, I) (XStart + I * XStep, Y)))
        End Sub

        Public Sub DrawFunction(ByVal Color As Color, ByVal Points As IReadOnlyList(Of (X As Double, Y As Double)))
            Dim FIX = Me.Intervals(Orientation.X)
            Dim FPoints = New Point(Points.Count - 1) {}

            For I As Integer = 0 To FPoints.Length - 1
                Dim T = Points.Item(I)
                Dim P = ConvertPoint(T.X, T.Y, Orientation.X)
                FPoints(I) = P
            Next

            Using Pen = New Pen(ConvertColor(Color))
                Me.Graphics.DrawLines(Pen, FPoints)
            End Using
        End Sub

        Public Sub DrawFunction(ByVal Color As Color, ByVal Func As Func(Of Double, Double))
            Dim FIX = Me.Intervals(Orientation.X)
            Dim N = 1000
            Dim Points = New Point(N) {}

            For I As Integer = 0 To N
                Dim X = FIX.Start + FIX.Length / N * I
                Dim P = ConvertPoint(X, Func.Invoke(X), Orientation.X)
                Points(I) = P
            Next

            Using Pen = New Pen(ConvertColor(Color))
                Me.Graphics.DrawLines(Pen, Points)
            End Using
        End Sub

        Public Sub DrawSlope(ByVal Color As Color, ByVal Value As Double)
            Dim IX = Me.Intervals(Orientation.X)
            Dim IY = Me.Intervals(Orientation.Y)
            Dim FIX = Me.FIntervals(Orientation.X)
            Dim FIY = Me.FIntervals(Orientation.Y)

            Value /= (IY.Length / FIY.Length) / (IX.Length / FIX.Length)

            Dim A = Math.Atan(Value)
            Dim X = FIX.Start
            Dim Y = FIY.Start + FIY.Length / 2
            Dim X2 = X + 100 * Math.Cos(A)
            Dim Y2 = Y + 100 * Math.Sin(A)

            Using Pen = New Pen(ConvertColor(Color))
                Me.Graphics.DrawLine(Pen,
                                 CreatePoint(X, Y, Orientation.X),
                                 CreatePoint(X2, Y2, Orientation.X))
            End Using
        End Sub

        Public Sub DrawPoint(ByVal Color As Color, ByVal Value As Double, ByVal Orientation As Orientation, Optional ByVal Caption As String = "")
            Dim FIO = Me.FIntervals(1 - Orientation)
            Dim FV = ConvertValue(Value, Orientation)

            Dim Col = ConvertColor(Color)
            Using Pen = New Pen(ConvertColor(Color))
                Me.Graphics.DrawLine(Pen,
                                 Me.CreatePoint(FV, FIO.Start, Orientation),
                                 Me.CreatePoint(FV, FIO.Start + FIO.Length, Orientation))
            End Using
            Using Brush = New SolidBrush(ConvertColor(Color))
                Me.Graphics.DrawString(Caption, SystemFonts.DefaultFont, Brush,
                                   CreatePoint(FV, FIO.Start, Orientation))
            End Using
        End Sub

        Private Function CreatePoint(ByVal X As Double, ByVal Y As Double, ByVal Orientation As Orientation) As Point
            If Orientation = Orientation.X Then
                Return New Point(CInt(X), CInt(Y))
            Else
                Return New Point(CInt(Y), CInt(X))
            End If
        End Function

        Private Function CreateSize(ByVal Width As Double, ByVal Height As Double, ByVal Orientation As Orientation) As Size
            If Orientation = Orientation.X Then
                Return New Size(CInt(Width), CInt(Height))
            Else
                Return New Size(CInt(Height), CInt(Width))
            End If
        End Function

        Private Function ConvertValue(ByVal Value As Double, ByVal Orientation As Orientation) As Double
            Dim I = Me.Intervals(Orientation)
            Dim FI = Me.FIntervals(Orientation)
            Return (Value - I.Start) / I.Length * FI.Length + FI.Start
        End Function

        Private Function ConvertPoint(ByVal X As Double, ByVal Y As Double, ByVal Orientation As Orientation) As Point
            Return CreatePoint(ConvertValue(X, Orientation), ConvertValue(Y, Orientation Xor Orientation.Y), Orientation)
        End Function

        Private Function ConvertSize(ByVal Width As Double, ByVal Height As Double, ByVal Orientation As Orientation) As Size
            Return CreateSize(ConvertValue(Width, Orientation), ConvertValue(Height, Orientation Xor Orientation.Y), Orientation)
        End Function

        Private Function ConvertColor(ByVal Color As Color) As Drawing.Color
            Select Case Color
                Case Color.Transparent
                    Return Drawing.Color.Transparent
                Case Color.Black
                    Return Drawing.Color.Black
                Case Color.White
                    Return Drawing.Color.White
                Case Color.Red
                    Return Drawing.Color.Red
                Case Color.Green
                    Return Drawing.Color.Green
                Case Color.Blue
                    Return Drawing.Color.Blue
                Case Color.Cyan
                    Return Drawing.Color.Cyan
                Case Color.Magenta
                    Return Drawing.Color.Magenta
                Case Color.Yellow
                    Return Drawing.Color.Yellow
                Case Color.Gray
                    Return Drawing.Color.Gray
                Case Color.Orange
                    Return Drawing.Color.Orange
            End Select
            Verify.Fail()
            Return Nothing
        End Function

#Region "FormWidth Property"
        Private _FormWidth As Integer

        Public Property FormWidth As Integer
            Get
                Return Me._FormWidth
            End Get
            Set(ByVal Value As Integer)
                Me._FormWidth = Value
            End Set
        End Property
#End Region

#Region "FormHeight Property"
        Private _FormHeight As Integer

        Public Property FormHeight As Integer
            Get
                Return Me._FormHeight
            End Get
            Set(ByVal Value As Integer)
                Me._FormHeight = Value
            End Set
        End Property
#End Region

#Region "FormMargin Property"
        Private _FormMargin As Integer

        Public Property FormMargin As Integer
            Get
                Return Me._FormMargin
            End Get
            Set(ByVal Value As Integer)
                Me._FormMargin = Value
            End Set
        End Property
#End Region

#Region "FormLeft Property"
        Private _FormLeft As Integer

        Public Property FormLeft As Integer
            Get
                Return Me._FormLeft
            End Get
            Set(ByVal Value As Integer)
                Me._FormLeft = Value
            End Set
        End Property
#End Region

#Region "FormTop Property"
        Private _FormTop As Integer

        Public Property FormTop As Integer
            Get
                Return Me._FormTop
            End Get
            Set(ByVal Value As Integer)
                Me._FormTop = Value
            End Set
        End Property
#End Region

#Region "XStart Property"
        Private _XStart As Double

        Public Property XStart As Double
            Get
                Return Me._XStart
            End Get
            Set(ByVal Value As Double)
                Me._XStart = Value
            End Set
        End Property
#End Region

#Region "XLength Property"
        Private _XLength As Double

        Public Property XLength As Double
            Get
                Return Me._XLength
            End Get
            Set(ByVal Value As Double)
                Me._XLength = Value
            End Set
        End Property
#End Region

#Region "YStart Property"
        Private _YStart As Double

        Public Property YStart As Double
            Get
                Return Me._YStart
            End Get
            Set(ByVal Value As Double)
                Me._YStart = Value
            End Set
        End Property
#End Region

#Region "YLength Property"
        Private _YLength As Double

        Public Property YLength As Double
            Get
                Return Me._YLength
            End Get
            Set(ByVal Value As Double)
                Me._YLength = Value
            End Set
        End Property
#End Region

        Private BorderRectangleColor As Color
        Private Graphics As Graphics
        Private Form As Form
        Private ReadOnly FIntervals As Interval() = New Interval(1) {}
        Private ReadOnly Intervals As Interval() = New Interval(1) {}

        Private Structure Interval

            Public Sub New(ByVal Start As Double, ByVal Length As Double)
                Me.Start = Start
                Me.Length = Length
            End Sub

            Public ReadOnly Start As Double
            Public ReadOnly Length As Double

        End Structure

        Public Enum Orientation As Integer

            X = 0
            Y = 1

        End Enum

        Public Enum Color

            Transparent
            Black
            White
            Red
            Green
            Blue
            Cyan
            Magenta
            Yellow
            Gray
            Orange

        End Enum

    End Class

End Namespace
