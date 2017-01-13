Namespace Common

    Public Class Verify

        <DebuggerHidden()>
        Public Shared Sub NonNullArg(Of T As Class)(ByVal O As T, Optional ByVal Name As String = Nothing, Optional ByVal Message As String = Nothing)
            If O Is Nothing Then
                If Name Is Nothing Then
                    Throw New ArgumentNullException()
                ElseIf Message Is Nothing Then
                    Throw New ArgumentNullException(Name)
                Else
                    Throw New ArgumentNullException(Name, Message)
                End If
            End If
        End Sub

        <DebuggerHidden()>
        Public Shared Sub TrueArg(ByVal T As Boolean, Optional ByVal Name As String = Nothing, Optional ByVal Message As String = Nothing)
            If Not T Then
                If Name Is Nothing And Message Is Nothing Then
                    Throw New ArgumentException()
                Else
                    Throw New ArgumentException(Message, Name)
                End If
            End If
        End Sub

        <DebuggerHidden()>
        Public Shared Sub RangeArg(Of T As IComparable(Of T))(ByVal Start As T, ByVal V As T, ByVal [End] As T, Optional ByVal Name As String = Nothing, Optional ByVal Message As String = Nothing)
            If Start.CompareTo(V) > 0 OrElse V.CompareTo([End]) > 0 Then
                If Name Is Nothing Then
                    Throw New ArgumentOutOfRangeException()
                ElseIf Message Is Nothing Then
                    Throw New ArgumentOutOfRangeException(Name, V, String.Format("Argument must be between '{0}' and '{1}'.", Start, [End]))
                Else
                    Throw New ArgumentOutOfRangeException(Name, V, Message)
                End If
            End If
        End Sub

        <DebuggerHidden()>
        Public Shared Sub FalseArg(ByVal T As Boolean, Optional ByVal Name As String = Nothing, Optional ByVal Message As String = Nothing)
            If T Then
                If Name Is Nothing And Message Is Nothing Then
                    Throw New ArgumentException()
                Else
                    Throw New ArgumentException(Message, Name)
                End If
            End If
        End Sub

        <DebuggerHidden()>
        Public Shared Sub NonNull(Of T As Class)(ByVal O As T, Optional ByVal Name As String = Nothing)
            If O Is Nothing Then
                If Name Is Nothing Then
                    Throw New NullReferenceException(String.Format("Object reference '{0}' not set to an instance of an object.", Name))
                Else
                    Throw New NullReferenceException()
                End If
            End If
        End Sub

        <DebuggerHidden()>
        Public Shared Sub [True](ByVal T As Boolean, Optional ByVal Message As String = Nothing)
            If Not T Then
                Throw New InvalidOperationException(Message)
            End If
        End Sub

        <DebuggerHidden()>
        Public Shared Sub [False](ByVal T As Boolean, Optional ByVal Message As String = Nothing)
            If T Then
                Throw New InvalidOperationException(Message)
            End If
        End Sub

        <DebuggerHidden()>
        Public Shared Sub Fail(Optional ByVal Message As String = Nothing)
            Throw New InvalidOperationException(Message)
        End Sub

        <DebuggerHidden()>
        Public Shared Sub FailArg(Optional ByVal Name As String = Nothing, Optional ByVal Message As String = Nothing)
            If Name Is Nothing And Message Is Nothing Then
                Throw New ArgumentException()
            Else
                Throw New ArgumentException(Message, Name)
            End If
        End Sub

    End Class

End Namespace
