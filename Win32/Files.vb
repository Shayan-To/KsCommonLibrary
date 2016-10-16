Imports System.ComponentModel
Imports System.Runtime.InteropServices

Namespace Win32

    Public NotInheritable Class Files

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        <DllImport("Kernel32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Private Shared Function CreateHardLinkW(ByVal lpFileName As String, ByVal lpExistingFileName As String, ByVal lpSecurityAttributes As IntPtr) As Boolean
        End Function

        Public Shared Sub CreateHardLink(ByVal FileName As String, ByVal ExistingFileName As String)
            Dim R = CreateHardLinkW(FileName, ExistingFileName, IntPtr.Zero)
            If Not R Then
                ThrowError()
            End If
        End Sub

        Private Shared Sub VerifyError()
            Dim ErrorCode = Marshal.GetLastWin32Error()
            If ErrorCode <> 0 Then
                Throw New Win32Exception(ErrorCode)
            End If
        End Sub

        Private Shared Sub ThrowError()
            Throw New Win32Exception()
        End Sub

    End Class

End Namespace
