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

            Public Shared Function GetWindowPlacement(hWnd As IntPtr) As Unsafe.WindowPlacement
                Dim R = New Unsafe.WindowPlacement().InitNew()
                Unsafe.GetWindowPlacement(hWnd, R)
                Common.VerifyError()
                Return R
            End Function

        End Class

        Public Shared Function GetWindowState(Hwnd As IntPtr) As Unsafe.WindowShowState
            Return Helpers.GetWindowPlacement(Hwnd).ShowCmd
        End Function

        Public Shared Function GetAllWindows() As IntPtr()
            Dim L = New List(Of IntPtr)()
            Unsafe.EnumWindows(Function(hWnd, lParam)
                                   L.Add(hWnd)
                                   Return True
                               End Function, IntPtr.Zero)
            Common.VerifyError()
            Return L.ToArray()
        End Function

        Public Shared Function GetWindowRect(hWnd As IntPtr) As Rect
            Dim R As Rect = Nothing
            Unsafe.GetWindowRect(hWnd, R)
            Common.VerifyError()
            Return R
        End Function

        Public Shared Function GetForegroundWindow() As IntPtr
            Dim R = Unsafe.GetForegroundWindow()
            Common.VerifyError()
            Return R
        End Function

        Public Shared Function GetWindowText(hWnd As IntPtr) As String
            Dim Length = Unsafe.GetWindowTextLength(hWnd)
            Dim R = New Text.StringBuilder(Length + 2)

            Dim T = Unsafe.GetWindowText(hWnd, R, R.Capacity)

            Common.VerifyError()
            Assert.True(T = Length)

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

        ''' <summary>
        ''' Documentation from https://msdn.microsoft.com/en-us/library/windows/desktop/ms646267(v=vs.85).aspx.
        ''' Values are extracted from WinUser.h (line 424).
        ''' </summary>
        Public Enum KeyFlags

            ''' #define KF_EXTENDED 0x0100
            ''' <summary>
            ''' Manipulates the extended key flag.
            ''' </summary>
            Extended = &H100
            ''' #define KF_DLGMODE 0x0800
            ''' <summary>
            ''' Manipulates the dialog mode flag, which indicates whether a dialog box is active.
            ''' </summary>
            DlgMode = &H800
            ''' #define KF_MENUMODE 0x1000
            ''' <summary>
            ''' Manipulates the menu mode flag, which indicates whether a menu is active.
            ''' </summary>
            MenuMode = &H1000
            ''' #define KF_ALTDOWN 0x2000
            ''' <summary>
            ''' Manipulates the ALT key flag, which indicates whether the ALT key is pressed.
            ''' </summary>
            AltDown = &H2000
            ''' #define KF_REPEAT 0x4000
            ''' <summary>
            ''' Manipulates the repeat count.
            ''' </summary>
            Repeat = &H4000
            ''' #define KF_UP 0x8000
            ''' <summary>
            ''' Manipulates the transition state flag.
            ''' </summary>
            Up = &H8000

        End Enum

        ''' typedef struct tagPOINT {
        '''   LONG x;
        '''   LONG y;
        ''' } POINT, *PPOINT;
        ''' <summary>
        ''' The POINT structure defines the x- and y- coordinates of a point.
        ''' </summary>
        Public Structure Point

            Public Sub New(ByVal X As Integer, ByVal Y As Integer)
                Me.X = X
                Me.Y = Y
            End Sub

            Public Sub New(ByVal Point As Drawing.Point)
                Me.New(Point.X, Point.Y)
            End Sub

            ''' <summary>
            ''' The x-coordinate of the point.
            ''' </summary>
            Public X As Integer
            ''' <summary>
            ''' The y-coordinate of the point.
            ''' </summary>
            Public Y As Integer

        End Structure

        ''' typedef struct _RECT {
        '''   LONG left;
        '''   LONG top;
        '''   LONG right;
        '''   LONG bottom;
        ''' } RECT, *PRECT;
        ''' <summary>
        ''' The RECT structure defines the coordinates of the upper-left and lower-right corners of a rectangle.
        ''' </summary>
        ''' <remarks>
        ''' By convention, the right and bottom edges of the rectangle are normally considered exclusive. In other words, the pixel whose coordinates are ( right, bottom ) lies immediately outside of the rectangle. For example, when RECT is passed to the FillRect function, the rectangle is filled up to, but not including, the right column and bottom row of pixels. This structure is identical to the RECTL structure.
        ''' </remarks>
        Public Structure Rect

            ''' <summary>
            ''' The x-coordinate of the upper-left corner of the rectangle.
            ''' </summary>
            Public Left As Integer
            ''' <summary>
            ''' The y-coordinate of the upper-left corner of the rectangle.
            ''' </summary>
            Public Top As Integer
            ''' <summary>
            ''' The x-coordinate of the lower-right corner of the rectangle.
            ''' </summary>
            Public Right As Integer
            ''' <summary>
            ''' The y-coordinate of the lower-right corner of the rectangle.
            ''' </summary>
            Public Bottom As Integer

        End Structure

    End Class

End Namespace
