Public Class Assert

    Public Shared Sub NonNull(Of T As Class)(ByVal O As T, ByVal Name As String)
        If O Is Nothing Then
            Throw New AssertionException(String.Format("Object reference '{0}' not set to an instance of an object.", Name))
        End If
    End Sub

    Public Shared Sub [True](ByVal T As Boolean, Optional ByVal Message As String = Nothing)
        If Not T Then
            Throw New AssertionException(Message)
        End If
    End Sub

    Public Shared Sub [False](ByVal T As Boolean, Optional ByVal Message As String = Nothing)
        If T Then
            Throw New AssertionException(Message)
        End If
    End Sub

End Class
