Public MustInherit Class Application

    Private Sub New()
        Throw New NotSupportedException()
    End Sub

    Public Shared Sub Main()
        ConsoleUtilities.Initialize()
        InteractiveRunnableAttribute.RunTestMethods()
        ConsoleUtilities.Pause()
    End Sub

End Class
