Public Class Verify

    Public Shared Sub NonNullArg(Of T As Class)(ByVal O As T, Optional ByVal Name As String = Nothing, Optional ByVal Message As String = Nothing)
        If O Is Nothing Then
            If Name Is Nothing Then
                Throw New ArgumentNullException()
            ElseIf Message Is Nothing
                Throw New ArgumentNullException(Name)
            Else
                Throw New ArgumentNullException(Name, Message)
            End If
        End If
    End Sub

    Public Shared Sub TrueArg(ByVal T As Boolean, Optional ByVal Name As String = Nothing, Optional ByVal Message As String = Nothing)
        If Not T Then
            If Name Is Nothing And Message Is Nothing Then
                Throw New ArgumentException()
            Else
                Throw New ArgumentException(Message, Name)
            End If
        End If
    End Sub

    Public Shared Sub FalseArg(ByVal T As Boolean, Optional ByVal Name As String = Nothing, Optional ByVal Message As String = Nothing)
        If T Then
            If Name Is Nothing And Message Is Nothing Then
                Throw New ArgumentException()
            Else
                Throw New ArgumentException(Message, Name)
            End If
        End If
    End Sub

    Public Shared Sub NonNull(Of T As Class)(ByVal O As T, ByVal Name As String)
        If O Is Nothing Then
            Throw New NullReferenceException(String.Format("Object reference '{0}' not set to an instance of an object.", Name))
        End If
    End Sub

    Public Shared Sub [True](ByVal T As Boolean, Optional ByVal Message As String = Nothing)
        If Not T Then
            Throw New InvalidOperationException(Message)
        End If
    End Sub

    Public Shared Sub [False](ByVal T As Boolean, Optional ByVal Message As String = Nothing)
        If T Then
            Throw New InvalidOperationException(Message)
        End If
    End Sub

End Class
