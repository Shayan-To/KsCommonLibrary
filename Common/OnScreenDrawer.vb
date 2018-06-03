Imports System.Drawing

Namespace Common

    Public Class OnScreenDrawer

        Private Sub New(ByVal Graphics As Graphics)
            Me.Graphics = Graphics
            Me._IsDrawing = True
        End Sub

        Public Shared Function ForScreen() As OnScreenDrawer
            Return ForWindowHandle(IntPtr.Zero)
        End Function

        Public Shared Function ForWindowHandle(ByVal Handle As IntPtr) As OnScreenDrawer
            Return ForGraphics(Graphics.FromHwnd(Handle))
        End Function

        Public Shared Function ForGraphics(ByVal Graphics As Graphics) As OnScreenDrawer
            Return New OnScreenDrawer(Graphics)
        End Function

        Private Sub AddToGroup(ByVal Interval As TimeSpan, ByVal Drawing As Drawing)
            Dim Group As DrawingGroup = Nothing
            If Not Me.Groups.TryGetValue(Interval, Group) Then
                Group = New DrawingGroup(Me, Interval)
                Me.Groups.Item(Interval) = Group

                If Me.IsDrawing Then
                    Group.StartDrawing()
                End If
            End If

            Group.Drawings.Add(Drawing)
        End Sub

        Private Sub RemoveFromGroup(ByVal Interval As TimeSpan, ByVal Drawing As Drawing)
            Dim Group = Me.Groups.Item(Interval)
            Assert.True(Group.Drawings.Remove(Drawing))
            If Group.Drawings.Count = 0 Then
                Group.StopDrawing()
                Me.Groups.Remove(Interval)
            End If
        End Sub

        Private Sub ReportIntervalChange(ByVal Drawing As Drawing, ByVal OldInterval As TimeSpan)
            If Drawing.IsVisible Then
                Me.RemoveFromGroup(OldInterval, Drawing)
                Me.AddToGroup(Drawing.Interval, Drawing)
            End If
        End Sub

        Private Sub ReportIsVisibleChange(ByVal Drawing As Drawing)
            If Drawing.IsVisible Then
                Me.AddToGroup(Drawing.Interval, Drawing)
            Else
                Me.RemoveFromGroup(Drawing.Interval, Drawing)
            End If
        End Sub

        Private Sub ReportDrawingGotIn(ByVal Drawing As Drawing)
            Drawing.SetParent(Me)
            If Drawing.IsVisible Then
                Me.AddToGroup(Drawing.Interval, Drawing)
            End If
        End Sub

        Private Sub ReportDrawingGotOut(ByVal Drawing As Drawing)
            Drawing.SetParent(Nothing)
            If Drawing.IsVisible Then
                Me.RemoveFromGroup(Drawing.Interval, Drawing)
            End If
        End Sub

        Private Sub StartDrawing()
            Verify.False(Me.IsDrawing, "Already drawing.")
            Me._IsDrawing = True
            For Each D In Me.Groups.Values
                D.StartDrawing()
            Next
        End Sub

        Private Sub StopDrawing()
            Me._IsDrawing = False
            For Each D In Me.Groups.Values
                D.StopDrawing()
            Next
        End Sub

#Region "Drawings Read-Only Property"
        Private ReadOnly _Drawings As DrawingsCollection = New DrawingsCollection(Me)

        Public ReadOnly Property Drawings As IList(Of Drawing)
            Get
                Return Me._Drawings
            End Get
        End Property
#End Region

#Region "IsDrawing Property"
        Private _IsDrawing As Boolean

        Public Property IsDrawing As Boolean
            Get
                Return Me._IsDrawing
            End Get
            Set(ByVal Value As Boolean)
                If Value <> Me._IsDrawing Then
                    If Value Then
                        Me.StartDrawing()
                    Else
                        Me.StopDrawing()
                    End If
                End If
            End Set
        End Property
#End Region

        Private ReadOnly Groups As Dictionary(Of TimeSpan, DrawingGroup) = New Dictionary(Of TimeSpan, DrawingGroup)()
        Private ReadOnly Graphics As Graphics

        Private Class DrawingsCollection
            Inherits BaseList(Of Drawing)

            Public Sub New(ByVal Parent As OnScreenDrawer)
                Me.Parent = Parent
            End Sub

            Private Sub OnChanged()

            End Sub

            Private Sub ItemGotIn(ByVal Item As Drawing)
                Me.Parent.ReportDrawingGotIn(Item)
            End Sub

            Private Sub ItemGotOut(ByVal Item As Drawing)
                Me.Parent.ReportDrawingGotOut(Item)
            End Sub

            Public Overrides Sub Clear()
                For Each I In Me
                    Me.ItemGotOut(I)
                Next
                Me.Base.Clear()
                Me.OnChanged()
            End Sub

            Public Overrides Sub Insert(index As Integer, item As Drawing)
                Me.ItemGotIn(item)
                Me.Base.Insert(index, item)
                Me.OnChanged()
            End Sub

            Public Overrides Sub RemoveAt(index As Integer)
                Me.ItemGotOut(Me.Base.Item(index))
                Me.Base.RemoveAt(index)
                Me.OnChanged()
            End Sub

            Protected Overrides Function IEnumerable_1_GetEnumerator() As IEnumerator(Of Drawing)
                Return Me.GetEnumerator()
            End Function

            Public Function GetEnumerator() As List(Of Drawing).Enumerator
                Return Me.Base.GetEnumerator()
            End Function

            Default Public Overrides Property Item(index As Integer) As Drawing
                Get
                    Return Me.Base.Item(index)
                End Get
                Set(value As Drawing)
                    Me.ItemGotOut(Me.Base.Item(index))
                    Me.ItemGotIn(value)
                    Me.Base.Item(index) = value
                    Me.OnChanged()
                End Set
            End Property

            Public Overrides ReadOnly Property Count As Integer
                Get
                    Return Me.Base.Count
                End Get
            End Property

            Private ReadOnly Base As List(Of Drawing) = New List(Of Drawing)()
            Private ReadOnly Parent As OnScreenDrawer

        End Class

        Private Class DrawingGroup

            Public Sub New(ByVal Parent As OnScreenDrawer, ByVal Interval As TimeSpan)
                Me.Parent = Parent
                Me.Interval = Interval
            End Sub

            Public Async Sub StartDrawing()
                Assert.True(Me.Parent.IsDrawing)

                If Me.IsDrawing Then
                    Exit Sub
                End If
                Me.IsDrawing = True

                Dim Graphics = Me.Parent.Graphics

                Do
                    Await Task.Delay(Me.Interval)
                    If Not Me.IsDrawing Then
                        Exit Do
                    End If

                    For Each D In Me.Drawings
                        Assert.True(D.Interval = Me.Interval)
                        D.Draw(Graphics)
                    Next
                Loop

                Me.IsDrawing = False
            End Sub

            Public Sub StopDrawing()
                Me.IsDrawing = False
            End Sub

#Region "Drawings Read-Only Property"
            Private ReadOnly _Drawings As List(Of Drawing) = New List(Of Drawing)()

            Public ReadOnly Property Drawings As List(Of Drawing)
                Get
                    Return Me._Drawings
                End Get
            End Property
#End Region

            Private IsDrawing As Boolean
            Private ReadOnly Parent As OnScreenDrawer
            Private ReadOnly Interval As TimeSpan

        End Class

        Public Class Drawing

            Public Sub New(ByVal IntervalMillis As Integer, ByVal Width As Integer, ByVal Height As Integer)
                Me.New(IntervalMillis, 0, 0, Width, Height)
            End Sub

            Public Sub New(ByVal IntervalMillis As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer)
                Me.New(TimeSpan.FromMilliseconds(IntervalMillis), New Rectangle(X, Y, Width, Height))
            End Sub

            Public Sub New(ByVal Interval As TimeSpan, ByVal Bounds As Rectangle)
                Me._Interval = Interval
                Me._Bounds = Bounds
                Me.Bitmap = New Bitmap(Bounds.Width, Bounds.Height)
                Me._Graphics = Graphics.FromImage(Me.Bitmap)
            End Sub

            Friend Sub SetParent(ByVal Parent As OnScreenDrawer)
                Verify.True(Parent Is Nothing Xor Me.Parent Is Nothing, "Cannot add a drawing to two drawers.")
                Me.Parent = Parent
            End Sub

            Private Sub RecreateBitmap()
                Me._Graphics.Dispose()
                Me.Bitmap.Dispose()
                Me.Bitmap = New Bitmap(Me.Bounds.Width, Me.Bounds.Height)
                Me._Graphics = Graphics.FromImage(Me.Bitmap)
            End Sub

            Public Sub Show()
                Me._IsVisible = True
                Me.Parent?.ReportIsVisibleChange(Me)
            End Sub

            Public Sub Hide()
                Me._IsVisible = False
                Me.Parent?.ReportIsVisibleChange(Me)
            End Sub

            Friend Sub Draw(ByVal Graphics As Graphics)
                Graphics.DrawImageUnscaled(Me.Bitmap, Me.Bounds)
            End Sub

#Region "Interval Property"
            Private _Interval As TimeSpan

            Public Property Interval As TimeSpan
                Get
                    Return Me._Interval
                End Get
                Set(ByVal Value As TimeSpan)
                    Dim Old = Me._Interval
                    Me._Interval = Value
                    Me.Parent?.ReportIntervalChange(Me, Old)
                End Set
            End Property
#End Region

#Region "Bounds Property"
            Private _Bounds As Rectangle

            Public Property Bounds As Rectangle
                Get
                    Return Me._Bounds
                End Get
                Set(ByVal Value As Rectangle)
                    Dim ShouldRecreate = Me._Bounds.Width <> Value.Width Or Me._Bounds.Height <> Value.Height
                    Me._Bounds = Value
                    If ShouldRecreate Then
                        Me.RecreateBitmap()
                    End If
                End Set
            End Property
#End Region

#Region "Graphics Read-Only Property"
            Private _Graphics As Graphics

            Public ReadOnly Property Graphics As Graphics
                Get
                    Return Me._Graphics
                End Get
            End Property
#End Region

#Region "IsVisible Property"
            Private _IsVisible As Boolean

            Public Property IsVisible As Boolean
                Get
                    Return Me._IsVisible
                End Get
                Set(ByVal Value As Boolean)
                    If Me._IsVisible <> Value Then
                        If Value Then
                            Me.Show()
                        Else
                            Me.Hide()
                        End If
                    End If
                End Set
            End Property
#End Region

            Private Parent As OnScreenDrawer
            Private Bitmap As Bitmap

        End Class

    End Class

End Namespace
