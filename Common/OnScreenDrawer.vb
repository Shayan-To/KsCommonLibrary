Imports System.Drawing

Namespace Common

    Public Class OnScreenDrawer

        Private Sub New(ByVal Graphics As Graphics)
            Me.Graphics = Graphics
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

        Public Sub StartDrawing()
            Verify.False(Me.IsDrawing, "Already drawing.")
            Me._IsDrawing = True
            For Each D In Me.Drawings
                D.StartDrawing()
            Next
        End Sub

        Public Sub StopDrawing()
            Me._IsDrawing = False
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

        Private ReadOnly Graphics As Graphics

        Private Class DrawingsCollection
            Inherits BaseList(Of Drawing)

            Public Sub New(ByVal Parent As OnScreenDrawer)
                Me.Parent = Parent
            End Sub

            Private Sub OnChanged()

            End Sub

            Private Sub ItemGotOut(ByVal Item As Drawing)
                Item.SetParent(Nothing)
            End Sub

            Private Sub ItemGotIn(ByVal Item As Drawing)
                Item.SetParent(Me.Parent)
                If Me.Parent.IsDrawing Then
                    Item.StartDrawing()
                End If
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

        Public Class Drawing

            Friend Sub SetParent(ByVal Parent As OnScreenDrawer)
                Verify.True(Parent Is Nothing Xor Me.Parent Is Nothing, "You cannot add one drawing to two different OnDisplayDrawer's.")
                Me.Parent = Parent
                Me.IsDrawing = False
            End Sub

            Friend Async Sub StartDrawing()
                Assert.True(Me.Parent IsNot Nothing)
                Assert.True(Me.Parent.IsDrawing)

                If Me.IsDrawing Then
                    Exit Sub
                End If
                Me.IsDrawing = True

                Do
                    Await Task.Delay(Me.Interval)
                    If Not Me.Parent.IsDrawing Or Not Me.IsDrawing Then
                        Assert.True((Not Me.IsDrawing).Implies(Me.Parent Is Nothing))
                        Exit Do
                    End If

                    For Each P In Me.Parts
                        P.Draw(Me.Parent.Graphics)
                    Next
                Loop

                Me.IsDrawing = False
            End Sub

#Region "Interval Property"
            Private _Interval As Integer

            Public Property Interval As Integer
                Get
                    Return Me._Interval
                End Get
                Set(ByVal Value As Integer)
                    Me._Interval = Value
                End Set
            End Property
#End Region

#Region "Parts Read-Only Property"
            Private ReadOnly _Parts As List(Of DrawingPart) = New List(Of DrawingPart)()

            Public ReadOnly Property Parts As List(Of DrawingPart)
                Get
                    Return Me._Parts
                End Get
            End Property
#End Region

            Private IsDrawing As Boolean
            Private Parent As OnScreenDrawer

        End Class

        Public Class DrawingPart

            Public Sub New(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer)
                Me.New(New Rectangle(X, Y, Width, Height))
            End Sub

            Public Sub New(ByVal Width As Integer, ByVal Height As Integer)
                Me.New(0, 0, Width, Height)
            End Sub

            Public Sub New(ByVal Bounds As Rectangle)
                Me._Bounds = Bounds
                Me.Bitmap = New Bitmap(Bounds.Width, Bounds.Height)
                Me._Graphics = Graphics.FromImage(Me.Bitmap)
            End Sub

            Private Sub RecreateBitmap()
                Me._Graphics.Dispose()
                Me.Bitmap.Dispose()
                Me.Bitmap = New Bitmap(Me.Bounds.Width, Me.Bounds.Height)
                Me._Graphics = Graphics.FromImage(Me.Bitmap)
            End Sub

            Public Sub Show()
                Me.IsVisible = True
            End Sub

            Public Sub Hide()
                Me.IsVisible = False
            End Sub

            Friend Sub Draw(ByVal Graphics As Graphics)
                If Me.IsVisible Then
                    Graphics.DrawImageUnscaled(Me.Bitmap, Me.Bounds)
                End If
            End Sub

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
                    Me._IsVisible = Value
                End Set
            End Property
#End Region

            Friend Bitmap As Bitmap

        End Class

    End Class

End Namespace
