Imports System.ComponentModel
Imports System.Runtime.InteropServices

Namespace Common.Win32

    Public MustInherit Class Common

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        Friend Shared Sub VerifyError()
            Dim ErrorCode = Marshal.GetLastWin32Error()
            If ErrorCode <> 0 Then
                Throw New Win32Exception(ErrorCode)
            End If
        End Sub

        Friend Shared Sub ThrowError()
            Throw New Win32Exception()
        End Sub

    End Class

End Namespace
