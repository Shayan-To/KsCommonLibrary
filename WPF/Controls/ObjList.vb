Namespace Common.Controls

    Public Class ObjList
        Inherits BaseList(Of Obj)
        Implements Markup.IAddChild

        Public Sub New(ByVal Parent As TextBlock)
            Me._Parent = Parent
        End Sub

        Private Sub GotIn(ByVal Obj As Obj)
            Obj.ReportParent(Me.Parent)
            Me.Parent.ReportObjGotIn(Obj)
        End Sub

        Private Sub WentOut(ByVal Obj As Obj)
            Me.Parent.ReportObjWentOut(Obj)
        End Sub

        Public Overrides ReadOnly Property Count As Integer
            Get
                Return Me.List.Count
            End Get
        End Property

        Default Public Overrides Property Item(index As Integer) As Obj
            Get
                Return Me.List.Item(index)
            End Get
            Set(value As Obj)
                Me.WentOut(Me.List.Item(index))
                Me.List.Item(index) = value
                Me.GotIn(value)
                Me.Parent.ReportObjChanged()
            End Set
        End Property

        Public Overrides Sub Clear()
            For Each O In Me.List
                Me.WentOut(O)
            Next
            Me.List.Clear()
            Me.Parent.ReportObjChanged()
        End Sub

        Public Overrides Sub Insert(index As Integer, item As Obj)
            Me.List.Insert(index, item)
            Me.GotIn(item)
            Me.Parent.ReportObjChanged()
        End Sub

        Public Overrides Sub RemoveAt(index As Integer)
            Me.WentOut(Me.List.Item(index))
            Me.List.RemoveAt(index)
            Me.Parent.ReportObjChanged()
        End Sub

        Protected Overrides Function IEnumerable_1_GetEnumerator() As IEnumerator(Of Obj)
            Return Me.List.GetEnumerator()
        End Function

        Public Function GetEnumerator() As List(Of Obj).Enumerator
            Return Me.List.GetEnumerator()
        End Function

        Public Sub AddChild(value As Object) Implements Markup.IAddChild.AddChild
            Me.Add(DirectCast(value, Obj))
        End Sub

        Public Sub AddText(text As String) Implements Markup.IAddChild.AddText
            Throw New NotSupportedException()
        End Sub

#Region "Parent Property"
        Private ReadOnly _Parent As TextBlock

        Public ReadOnly Property Parent As TextBlock
            Get
                Return Me._Parent
            End Get
        End Property
#End Region

        Private ReadOnly List As List(Of Obj) = New List(Of Obj)()

    End Class

End Namespace
