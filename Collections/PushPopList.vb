Public Interface IPushPop(Of T)

    Sub Push(ByVal Item As T)
    Function Pop() As T
    Function Peek() As T

End Interface

Public Class PushPopList(Of T)
    Inherits List(Of T)
    Implements IPushPop(Of T)

    Public Sub Push(Item As T) Implements IPushPop(Of T).Push
        Me.Add(Item)
    End Sub

    Public Function Pop() As T Implements IPushPop(Of T).Pop
        Dim I = Me.Count - 1
        Dim R = Me.Item(I)
        Me.RemoveAt(I)
        Return R
    End Function

    Public Function Peek() As T Implements IPushPop(Of T).Peek
        Return Me.Item(Me.Count - 1)
    End Function

End Class
