Imports System.Windows.Markup

Public Class AddChildList(Of T)
    Inherits List(Of T)
    Implements IAddChild

    Public Sub AddChild(value As Object) Implements IAddChild.AddChild
        Me.Add(DirectCast(value, T))
    End Sub

    Public Sub AddText(text As String) Implements IAddChild.AddText
        If text.Trim().Length <> 0 Then
            Throw New ArgumentException()
        End If
    End Sub

End Class
