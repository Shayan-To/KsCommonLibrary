Namespace Common

    Public Class Assert

        <DebuggerHidden()>
        Public Shared Sub NonNull(Of T As Class)(ByVal O As T, Optional ByVal Name As String = Nothing)
            If O Is Nothing Then
                If Name IsNot Nothing Then
                    Fail(String.Format("Object reference '{0}' not set to an instance of an object.", Name))
                Else
                    Fail("Object reference not set to an instance of an object.")
                End If
            End If
        End Sub

        <DebuggerHidden()>
        Public Shared Sub [True](ByVal T As Boolean, Optional ByVal Message As String = Nothing)
            If Not T Then
                Fail(Message)
            End If
        End Sub

        <DebuggerHidden()>
        Public Shared Sub [False](ByVal T As Boolean, Optional ByVal Message As String = Nothing)
            If T Then
                Fail(Message)
            End If
        End Sub

        <DebuggerHidden()>
        Public Shared Sub Fail(Optional ByVal Message As String = Nothing)
            Throw New AssertionException(Message)
        End Sub

    End Class

End Namespace
