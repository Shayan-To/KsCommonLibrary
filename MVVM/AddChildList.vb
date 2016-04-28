Imports System.Windows.Markup

Public Class AddChildList(Of T)
    Inherits NotifyingList(Of T)
    Implements IAddChild

    Public Sub AddChild(value As Object) Implements IAddChild.AddChild
        Me.Add(DirectCast(value, T))
    End Sub

    Public Sub AddText(text As String) Implements IAddChild.AddText
        Throw New NotSupportedException()
    End Sub

End Class
