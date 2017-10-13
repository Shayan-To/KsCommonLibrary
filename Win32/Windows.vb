Imports System.ComponentModel

Namespace Common.Win32

    Partial Public MustInherit Class Windows

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        Public MustInherit Class Helpers

            Private Sub New()
                Throw New NotSupportedException()
            End Sub

            Public Shared Function SetWindowPos(hwnd As IntPtr, hWndInsertAfter As Unsafe.SetWindowPosInsertAfter, X As Integer, Y As Integer, cx As Integer, cy As Integer, uFlags As Unsafe.SetWindowPosFlags) As Boolean
                Return Unsafe.SetWindowPos(hwnd, New IntPtr(hWndInsertAfter), X, Y, cx, cy, uFlags)
            End Function

        End Class

        Public Shared Function GetForegroundWindow() As IntPtr
            Dim R = Unsafe.GetForegroundWindow()
            Common.VerifyError()
            Return R
        End Function

        Public Shared Function GetWindowText(hWnd As IntPtr) As String
            Dim Length = Unsafe.GetWindowTextLength(hWnd)
            Dim R = New Text.StringBuilder(Length + 2)

            Dim T = Unsafe.GetWindowText(hWnd, R, R.Capacity)

            Assert.True(T = Length)
            Common.VerifyError()

            Return R.ToString()
        End Function

        Public Shared Function FindWindow(lpClassName As String, lpWindowName As String) As IntPtr
            Dim R = Unsafe.FindWindow(lpClassName, lpWindowName)
            Common.VerifyError()
            Return R
        End Function

        Public Shared Function GetWindowThreadProcessId(hwnd As IntPtr) As (ProcessId As UInteger, ThreadId As UInteger)
            Dim ProcessId As UInteger = 0
            Dim ThreadId = Unsafe.GetWindowThreadProcessId(hwnd, ProcessId)
            Common.VerifyError()
            Return (ProcessId, ThreadId)
            Dim a = (a:=1, 2)
        End Function

        Public Shared Function GetWindowProcess(hwnd As IntPtr) As Process
            Return Process.GetProcessById(CInt(GetWindowThreadProcessId(hwnd).ProcessId))
        End Function

        Public Shared Function WindowFromPoint(p As Point) As IntPtr
            Dim R = Unsafe.WindowFromPoint(p)
            Common.VerifyError()
            Return R
        End Function

        Public Shared Function GetAncestorWindow(hwnd As IntPtr, Kind As AncestorKind) As IntPtr
            Dim R As IntPtr

            If Kind = AncestorKind.ParentOrOwner Then
                R = Unsafe.GetParent(hwnd)
            ElseIf GetAncestorConstant <= Kind And Kind < GetWindowConstant Then
                R = Unsafe.GetAncestor(hwnd, CType(Kind - GetAncestorConstant, Unsafe.GetAncestorFlags))
            ElseIf GetWindowConstant <= Kind Then
                R = Unsafe.GetWindow(hwnd, CType(Kind - GetWindowConstant, Unsafe.GetWindowCommand))
            Else
                Verify.FailArg(NameOf(Kind), "Invalid kind.")
            End If

            Common.VerifyError()
            Return R
        End Function

        Public Shared Sub SetTopMost(hwnd As IntPtr, TopMost As Boolean)
            Helpers.SetWindowPos(hwnd,
                                 If(TopMost, Unsafe.SetWindowPosInsertAfter.TopMost, Unsafe.SetWindowPosInsertAfter.NoTopMost),
                                 0, 0, 0, 0,
                                 Unsafe.SetWindowPosFlags.NoMove Or Unsafe.SetWindowPosFlags.NoSize Or Unsafe.SetWindowPosFlags.ShowWindow)
            Common.VerifyError()
        End Sub

        Private Const GetAncestorConstant = 100
        Private Const GetWindowConstant = 200

        Public Enum AncestorKind As UInteger

            ''' <summary>
            ''' Retrieve the parent or the owner window.
            ''' </summary>
            ParentOrOwner = 0

            ''' <summary>
            ''' Retrieve the parent window. This does not include the owner.
            ''' </summary>
            Parent = GetAncestorConstant + 1

            ''' <summary>
            ''' Retrieve the root window by walking the chain of parent windows.
            ''' </summary>
            Root = GetAncestorConstant + 2

            ''' <summary>
            ''' Retrieve the owned root window by walking the chain of parent/owner windows.
            ''' </summary>
            RootOwner = GetAncestorConstant + 3

            ''' <summary>
            ''' Retrieve the window of the same type that is highest in the Z order.
            ''' If the specified window is a topmost window, the handle identifies a topmost window.
            ''' If the specified window is a top-level window, the handle identifies a top-level window.
            ''' If the specified window is a child window, the handle identifies a sibling window.
            ''' </summary>
            HwndFirst = GetWindowConstant + 0

            ''' <summary>
            ''' Retrieve the window of the same type that is lowest in the Z order.
            ''' If the specified window is a topmost window, the handle identifies a topmost window.
            ''' If the specified window is a top-level window, the handle identifies a top-level window.
            ''' If the specified window is a child window, the handle identifies a sibling window.
            ''' </summary>
            HwndLast = GetWindowConstant + 1

            ''' <summary>
            ''' Retrieve the window below the specified window in the Z order.
            ''' If the specified window is a topmost window, the handle identifies a topmost window.
            ''' If the specified window is a top-level window, the handle identifies a top-level window.
            ''' If the specified window is a child window, the handle identifies a sibling window.
            ''' </summary>
            HwndNext = GetWindowConstant + 2

            ''' <summary>
            ''' Retrieve the window above the specified window in the Z order.
            ''' If the specified window is a topmost window, the handle identifies a topmost window.
            ''' If the specified window is a top-level window, the handle identifies a top-level window.
            ''' If the specified window is a child window, the handle identifies a sibling window.
            ''' </summary>
            HwndPrev = GetWindowConstant + 3

            ''' <summary>
            ''' Retrieve the specified window's owner window, if any. For more information, see Owned Windows.
            ''' </summary>
            Owner = GetWindowConstant + 4

            ''' <summary>
            ''' Retrieve the child window at the top of the Z order, if the specified window is a parent window;
            ''' otherwise, the retrieved handle is NULL.
            ''' The function examines only child windows of the specified window. It does not examine descendant windows.
            ''' </summary>
            Child = GetWindowConstant + 5

            ''' <summary>
            ''' Retrieve the enabled popup window owned by the specified window (the search uses the first such window found using GW_HWNDNEXT);
            ''' otherwise, if there are no enabled popup windows, the retrieved handle is that of the specified window.
            ''' </summary>
            EnabledPopup = GetWindowConstant + 6

        End Enum

        Public Structure Point

            Public Sub New(ByVal X As Integer, ByVal Y As Integer)
                Me.X = X
                Me.Y = Y
            End Sub

            Public X, Y As Integer

        End Structure

    End Class

End Namespace
