Imports System.ComponentModel
Imports System.Runtime.InteropServices

Namespace Common.Win32

    Public MustInherit Class Files

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        <DllImport("Kernel32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Private Shared Function CreateHardLinkW(ByVal lpFileName As String, ByVal lpExistingFileName As String, ByVal lpSecurityAttributes As IntPtr) As Boolean
        End Function

        Public Shared Sub CreateHardLink(ByVal FileName As String, ByVal ExistingFileName As String)
            Dim R = CreateHardLinkW(FileName, ExistingFileName, IntPtr.Zero)
            If Not R Then
                Common.ThrowError()
            End If
        End Sub

    End Class

End Namespace
