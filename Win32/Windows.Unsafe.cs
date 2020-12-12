using System;
using System.Runtime.InteropServices;

namespace Ks.Common.Win32
{
    partial class Windows
    {
        public static class Unsafe
        {

            /// BOOL WINAPI GetWindowPlacement(
            /// _In_    HWND            hWnd,
            /// _Inout_ WINDOWPLACEMENT *lpwndpl
            /// );
            /// <summary>
            /// Retrieves the show state and the restored, minimized, and maximized positions of the specified window.
            /// </summary>
            /// <param name="hWnd">
            /// hWnd [in]
            /// Type: HWND
            /// A handle to the window.
            /// </param>
            /// <param name="lpwndpl">
            /// lpwndpl [in, out]
            /// Type: WINDOWPLACEMENT*
            /// A pointer to the WINDOWPLACEMENT structure that receives the show state and position information. Before calling GetWindowPlacement, set the length member to sizeof(WINDOWPLACEMENT). GetWindowPlacement fails if lpwndpl-> length is not set correctly.
            /// </param>
            /// <returns>
            /// Type: BOOL
            /// If the function succeeds, the return value is nonzero.
            /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
            /// </returns>
            /// <remarks>
            /// The flags member of WINDOWPLACEMENT retrieved by this function is always zero. If the window identified by the hWnd parameter is maximized, the showCmd member is SW_SHOWMAXIMIZED. If the window is minimized, showCmd is SW_SHOWMINIMIZED. Otherwise, it is SW_SHOWNORMAL.
            /// The length member of WINDOWPLACEMENT must be set to sizeof(WINDOWPLACEMENT). If this member is not set correctly, the function returns FALSE. For additional remarks on the proper use of window placement coordinates, see WINDOWPLACEMENT.
            /// </remarks>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement lpwndpl);

            /// BOOL WINAPI EnumWindows(
            /// _In_ WNDENUMPROC lpEnumFunc,
            /// _In_ LPARAM      lParam
            /// );
            /// <summary>
            /// Enumerates all top-level windows on the screen by passing the handle to each window, in turn, to an application-defined callback function. EnumWindows continues until the last top-level window is enumerated or the callback function returns FALSE.
            /// </summary>
            /// <param name="lpEnumFunc">
            /// lpEnumFunc [in]
            /// Type: WNDENUMPROC
            /// A pointer to an application-defined callback function. For more information, see EnumWindowsProc.
            /// </param>
            /// <param name="lParam">
            /// lParam [in]
            /// Type: LPARAM
            /// An application-defined value to be passed to the callback function.
            /// </param>
            /// <returns>
            /// Type: BOOL
            /// If the function succeeds, the return value is nonzero.
            /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
            /// If EnumWindowsProc returns zero, the return value is also zero. In this case, the callback function should call SetLastError to obtain a meaningful error code to be returned to the caller of EnumWindows.
            /// </returns>
            /// <remarks>
            /// The EnumWindows function does not enumerate child windows, with the exception of a few top-level windows owned by the system that have the WS_CHILD style.
            /// This function is more reliable than calling the GetWindow function in a loop. An application that calls GetWindow to perform this task risks being caught in an infinite loop or referencing a handle to a window that has been destroyed.
            /// Note  For Windows 8 and later, EnumWindows enumerates only top-level windows of desktop apps.
            /// </remarks>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

            /// BOOL WINAPI GetWindowRect(
            /// _In_  HWND   hWnd,
            /// _Out_ LPRECT lpRect
            /// );
            /// <summary>
            /// Retrieves the dimensions of the bounding rectangle of the specified window. The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
            /// </summary>
            /// <param name="hWnd">
            /// hWnd [in]
            /// Type: HWND
            /// A handle to the window.
            /// </param>
            /// <param name="lpRect">
            /// lpRect [out]
            /// Type: LPRECT
            /// A pointer to a RECT structure that receives the screen coordinates of the upper-left and lower-right corners of the window.
            /// </param>
            /// <returns>
            /// Type: BOOL
            /// If the function succeeds, the return value is nonzero.
            /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
            /// </returns>
            /// <remarks>
            /// In conformance with conventions for the RECT structure, the bottom-right coordinates of the returned rectangle are exclusive. In other words, the pixel at (right, bottom) lies immediately outside the rectangle.
            /// </remarks>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

            /// HWND WINAPI GetForegroundWindow(void);
            /// <summary>
            /// Retrieves a handle to the foreground window (the window with which the user is currently working). The system assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads.
            /// </summary>
            /// <returns>
            /// Type: HWND
            /// The return value is a handle to the foreground window. The foreground window can be NULL in certain circumstances, such as when a window is losing activation.
            /// </returns>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern IntPtr GetForegroundWindow();

            /// int WINAPI GetWindowTextLength(
            /// _In_ HWND hWnd
            /// );
            /// <summary>
            /// Retrieves the length, in characters, of the specified window's title bar text (if the window has a title bar). If the specified window is a control, the function retrieves the length of the text within the control. However, GetWindowTextLength cannot retrieve the length of the text of an edit control in another application.
            /// </summary>
            /// <param name="hWnd">
            /// hWnd [in]
            /// Type: HWND
            /// A handle to the window or control.
            /// </param>
            /// <returns>
            /// Type: int
            /// If the function succeeds, the return value is the length, in characters, of the text. Under certain conditions, this value may actually be greater than the length of the text. For more information, see the following Remarks section.
            /// If the window has no text, the return value is zero. To get extended error information, call GetLastError.
            /// </returns>
            /// <remarks>
            /// If the target window is owned by the current process, GetWindowTextLength causes a WM_GETTEXTLENGTH message to be sent to the specified window or control.
            /// Under certain conditions, the GetWindowTextLength function may return a value that is larger than the actual length of the text. This occurs with certain mixtures of ANSI and Unicode, and is due to the system allowing for the possible existence of double-byte character set (DBCS) characters within the text. The return value, however, will always be at least as large as the actual length of the text; you can thus always use it to guide buffer allocation. This behavior can occur when an application uses both ANSI functions and common dialogs, which use Unicode. It can also occur when an application uses the ANSI version of GetWindowTextLength with a window whose window procedure is Unicode, or the Unicode version of GetWindowTextLength with a window whose window procedure is ANSI. For more information on ANSI and ANSI functions, see Conventions for Function Prototypes.
            /// To obtain the exact length of the text, use the WM_GETTEXT, LB_GETTEXT, or CB_GETLBTEXT messages, or the GetWindowText function.
            /// </remarks>
            [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern int GetWindowTextLength(IntPtr hWnd);

            /// int WINAPI GetWindowText(
            /// _In_  HWND   hWnd,
            /// _Out_ LPTSTR lpString,
            /// _In_  int    nMaxCount
            /// );
            /// <summary>
            /// Copies the text of the specified window's title bar (if it has one) into a buffer. If the specified window is a control, the text of the control is copied. However, GetWindowText cannot retrieve the text of a control in another application.
            /// </summary>
            /// <param name="hWnd">
            /// hWnd [in]
            /// Type: HWND
            /// A handle to the window or control containing the text.
            /// </param>
            /// <param name="lpString">
            /// lpString [out]
            /// Type: LPTSTR
            /// The buffer that will receive the text. If the string is as long or longer than the buffer, the string is truncated and terminated with a null character.
            /// </param>
            /// <param name="nMaxCount">
            /// nMaxCount [in]
            /// Type: int
            /// The maximum number of characters to copy to the buffer, including the null character. If the text exceeds this limit, it is truncated.
            /// </param>
            /// <returns>
            /// Type: int
            /// If the function succeeds, the return value is the length, in characters, of the copied string, not including the terminating null character. If the window has no title bar or text, if the title bar is empty, or if the window or control handle is invalid, the return value is zero. To get extended error information, call GetLastError.
            /// This function cannot retrieve the text of an edit control in another application.
            /// </returns>
            /// <remarks>
            /// If the target window is owned by the current process, GetWindowText causes a WM_GETTEXT message to be sent to the specified window or control. If the target window is owned by another process and has a caption, GetWindowText retrieves the window caption text. If the window does not have a caption, the return value is a null string. This behavior is by design. It allows applications to call GetWindowText without becoming unresponsive if the process that owns the target window is not responding. However, if the target window is not responding and it belongs to the calling application, GetWindowText will cause the calling application to become unresponsive.
            /// To retrieve the text of a control in another process, send a WM_GETTEXT message directly instead of calling GetWindowText.
            /// </remarks>
            [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

            /// <summary>
            /// Retrieves a handle to a window whose class name and window name match the specified strings. The function searches child windows, beginning with the one following the specified child window. This function does not perform a case-sensitive search.
            /// </summary>
            /// HWND WINAPI FindWindowEx(
            /// _In_opt_ HWND    hwndParent,
            /// _In_opt_ HWND    hwndChildAfter,
            /// _In_opt_ LPCTSTR lpszClass,
            /// _In_opt_ LPCTSTR lpszWindow
            /// );
            /// <param name="hwndParent">
            /// hwndParent [in, optional]
            /// Type: HWND
            /// A handle to the parent window whose child windows are to be searched.
            /// If hwndParent is NULL, the function uses the desktop window as the parent window. The function searches among windows that are child windows of the desktop.
            /// If hwndParent is HWND_MESSAGE, the function searches all message-only windows.
            /// </param>
            /// <param name="hwndChildAfter">
            /// hwndChildAfter [in, optional]
            /// Type: HWND
            /// A handle to a child window. The search begins with the next child window in the Z order. The child window must be a direct child window of hwndParent, not just a descendant window.
            /// If hwndChildAfter is NULL, the search begins with the first child window of hwndParent.
            /// Note that if both hwndParent and hwndChildAfter are NULL, the function searches all top-level and message-only windows.
            /// </param>
            /// <param name="lpszClass">
            /// lpszClass [in, optional]
            /// Type: LPCTSTR
            /// The class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. The atom must be placed in the low-order word of lpszClass; the high-order word must be zero.
            /// If lpszClass is a string, it specifies the window class name. The class name can be any name registered with RegisterClass or RegisterClassEx, or any of the predefined control-class names, or it can be MAKEINTATOM(0x8000). In this latter case, 0x8000 is the atom for a menu class. For more information, see the Remarks section of this topic.
            /// </param>
            /// <param name="lpszWindow">
            /// lpszWindow [in, optional]
            /// Type: LPCTSTR
            /// The window name (the window's title). If this parameter is NULL, all window names match.
            /// </param>
            /// <returns>
            /// Type: HWND
            /// If the function succeeds, the return value is a handle to the window that has the specified class and window names.
            /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
            /// </returns>
            /// <remarks>
            /// If the lpszWindow parameter is not NULL, FindWindowEx calls the GetWindowText function to retrieve the window name for comparison. For a description of a potential problem that can arise, see the Remarks section of GetWindowText.
            /// An application can call this function in the following way.
            /// FindWindowEx( NULL, NULL, MAKEINTATOM(0x8000), NULL );
            /// Note that 0x8000 is the atom for a menu class. When an application calls this function, the function checks whether a context menu is being displayed that the application created.
            /// </remarks>
            [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

            /// <summary>
            /// Retrieves a handle to the top-level window whose class name and window name match the specified strings. This function does not search child windows. This function does not perform a case-sensitive search.
            /// To search child windows, beginning with a specified child window, use the FindWindowEx function.
            /// </summary>
            /// HWND WINAPI FindWindow(
            /// _In_opt_ LPCTSTR lpClassName,
            /// _In_opt_ LPCTSTR lpWindowName
            /// );
            /// <param name="lpClassName">
            /// lpClassName [in, optional]
            /// Type: LPCTSTR
            /// The class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. The atom must be in the low-order word of lpClassName; the high-order word must be zero.
            /// If lpClassName points to a string, it specifies the window class name. The class name can be any name registered with RegisterClass or RegisterClassEx, or any of the predefined control-class names.
            /// If lpClassName is NULL, it finds any window whose title matches the lpWindowName parameter.
            /// </param>
            /// <param name="lpWindowName">
            /// lpWindowName [in, optional]
            /// Type: LPCTSTR
            /// The window name (the window's title). If this parameter is NULL, all window names match.
            /// </param>
            /// <returns>
            /// Type: HWND
            /// If the function succeeds, the return value is a handle to the window that has the specified class name and window name.
            /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
            /// </returns>
            /// <remarks>
            /// If the lpWindowName parameter is not NULL, FindWindow calls the GetWindowText function to retrieve the window name for comparison. For a description of a potential problem that can arise, see the Remarks for GetWindowText.
            /// </remarks>
            [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            /// <summary>
            /// Retrieves a handle to the window that contains the specified point.
            /// </summary>
            /// HWND WINAPI WindowFromPoint(
            /// _In_ POINT Point
            /// );
            /// <param name="p">
            /// Point [in]
            /// Type: POINT
            /// The point to be checked.
            /// </param>
            /// <returns>
            /// Type: HWND
            /// The return value is a handle to the window that contains the point. If no window exists at the given point, the return value is NULL. If the point is over a static text control, the return value is a handle to the window under the static text control.
            /// </returns>
            /// <remarks>
            /// The WindowFromPoint function does not retrieve a handle to a hidden or disabled window, even if the point is within the window. An application should use the ChildWindowFromPoint function for a nonrestrictive search.
            /// </remarks>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern IntPtr WindowFromPoint(Point p);

            /// <summary>
            /// Retrieves the handle to the ancestor of the specified window.
            /// </summary>
            /// HWND WINAPI GetAncestor(
            /// _In_ HWND hwnd,
            /// _In_ UINT gaFlags
            /// );
            /// <param name="hwnd">
            /// hwnd [in]
            /// Type: HWND
            /// A handle to the window whose ancestor is to be retrieved. If this parameter is the desktop window, the function returns NULL.
            /// </param>
            /// <param name="gaFlags">
            /// gaFlags [in]
            /// Type: UINT
            /// The ancestor to be retrieved.
            /// </param>
            /// <returns>
            /// Type: HWND
            /// The return value is the handle to the ancestor window.
            /// </returns>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags gaFlags);

            /// <summary>
            /// Retrieves a handle to a window that has the specified relationship (Z-Order or owner) to the specified window.
            /// </summary>
            /// HWND WINAPI GetWindow(
            /// _In_ HWND hWnd,
            /// _In_ UINT uCmd
            /// );
            /// <param name="hwnd">
            /// hWnd [in]
            /// Type: HWND
            /// A handle to a window. The window handle retrieved is relative to this window, based on the value of the uCmd parameter.
            /// </param>
            /// <param name="uCmd">
            /// uCmd [in]
            /// Type: UINT
            /// The relationship between the specified window and the window whose handle is to be retrieved. This parameter can be one of the following values.
            /// </param>
            /// <returns>
            /// Type: HWND
            /// If the function succeeds, the return value is a window handle. If no window exists with the specified relationship to the specified window, the return value is NULL. To get extended error information, call GetLastError.
            /// </returns>
            /// <remarks>
            /// The EnumChildWindows function is more reliable than calling GetWindow in a loop. An application that calls GetWindow to perform this task risks being caught in an infinite loop or referencing a handle to a window that has been destroyed.
            /// </remarks>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern IntPtr GetWindow(IntPtr hwnd, GetWindowCommand uCmd);

            /// <summary>
            /// Retrieves a handle to the specified window's parent or owner.
            /// To retrieve a handle to a specified ancestor, use the GetAncestor function.
            /// </summary>
            /// HWND WINAPI GetParent(
            /// _In_ HWND hWnd
            /// );
            /// <param name="hwnd">
            /// hWnd [in]
            /// Type: HWND
            /// A handle to the window whose parent window handle is to be retrieved.
            /// </param>
            /// <returns>
            /// Type: HWND
            /// If the window is a child window, the return value is a handle to the parent window. If the window is a top-level window with the WS_POPUP style, the return value is a handle to the owner window.
            /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
            /// This function typically fails for one of the following reasons:
            /// The window is a top-level window that is unowned or does not have the WS_POPUP style.
            /// The owner window has WS_POPUP style.
            /// </returns>
            /// <remarks>
            /// To obtain a window's owner window, instead of using GetParent, use GetWindow with the GW_OWNER flag. To obtain the parent window and not the owner, instead of using GetParent, use GetAncestor with the GA_PARENT flag.
            /// </remarks>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern IntPtr GetParent(IntPtr hwnd);

            /// DWORD WINAPI GetWindowThreadProcessId(
            /// _In_      HWND    hWnd,
            /// _Out_opt_ LPDWORD lpdwProcessId
            /// );
            /// <summary>
            /// Retrieves the identifier of the thread that created the specified window and, optionally, the identifier of the process that created the window.
            /// </summary>
            /// <param name="hwnd">
            /// hWnd [in]
            /// Type: HWND
            /// A handle to the window.
            /// </param>
            /// <param name="lpdwProcessId">
            /// lpdwProcessId [out, optional]
            /// Type: LPDWORD
            /// A pointer to a variable that receives the process identifier. If this parameter is not NULL, GetWindowThreadProcessId copies the identifier of the process to the variable; otherwise, it does not.
            /// </param>
            /// <returns>
            /// Type: DWORD
            /// The return value is the identifier of the thread that created the window.
            /// </returns>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint lpdwProcessId);

            /// BOOL WINAPI SetWindowPos(
            /// _In_     HWND hWnd,
            /// _In_opt_ HWND hWndInsertAfter,
            /// _In_     int  X,
            /// _In_     int  Y,
            /// _In_     int  cx,
            /// _In_     int  cy,
            /// _In_     UINT uFlags
            /// );
            /// <summary>
            /// Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.
            /// </summary>
            /// <param name="hwnd">
            /// hWnd [in]
            /// Type: HWND
            /// A handle to the window.
            /// </param>
            /// <param name="hWndInsertAfter">
            /// hWndInsertAfter [in, optional]
            /// Type: HWND
            /// A handle to the window to precede the positioned window in the Z order. This parameter must be a window handle or one of the following values.
            /// For more information about how this parameter is used, see the following Remarks section.
            /// </param>
            /// <param name="X">
            /// X [in]
            /// Type: int
            /// The new position of the left side of the window, in client coordinates.
            /// </param>
            /// <param name="Y">
            /// Y [in]
            /// Type: int
            /// The new position of the top of the window, in client coordinates.
            /// </param>
            /// <param name="cx">
            /// cx [in]
            /// Type: int
            /// The new width of the window, in pixels.
            /// </param>
            /// <param name="cy">
            /// cy [in]
            /// Type: int
            /// The new height of the window, in pixels.
            /// </param>
            /// <param name="uFlags">
            /// uFlags [in]
            /// Type: UINT
            /// The window sizing and positioning flags. This parameter can be a combination of the following values.
            /// </param>
            /// <returns>
            /// Type: BOOL
            /// If the function succeeds, the return value is nonzero.
            /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
            /// </returns>
            /// <remarks>
            /// As part of the Vista re-architecture, all services were moved off the interactive desktop into Session 0. hwnd and window manager operations are only effective inside a session and cross-session attempts to manipulate the hwnd will fail. For more information, see The Windows Vista Developer Story: Application Compatibility Cookbook.
            /// If you have changed certain window data using SetWindowLong, you must call SetWindowPos for the changes to take effect. Use the following combination for uFlags: SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED.
            /// A window can be made a topmost window either by setting the hWndInsertAfter parameter to HWND_TOPMOST and ensuring that the SWP_NOZORDER flag is not set, or by setting a window's position in the Z order so that it is above any existing topmost windows. When a non-topmost window is made topmost, its owned windows are also made topmost. Its owners, however, are not changed.
            /// If neither the SWP_NOACTIVATE nor SWP_NOZORDER flag is specified (that is, when the application requests that a window be simultaneously activated and its position in the Z order changed), the value specified in hWndInsertAfter is used only in the following circumstances.
            /// Neither the HWND_TOPMOST nor HWND_NOTOPMOST flag is specified in hWndInsertAfter.
            /// The window identified by hWnd is not the active window.
            /// An application cannot activate an inactive window without also bringing it to the top of the Z order. Applications can change an activated window's position in the Z order without restrictions, or it can activate a window and then move it to the top of the topmost or non-topmost windows.
            /// If a topmost window is repositioned to the bottom (HWND_BOTTOM) of the Z order or after any non-topmost window, it is no longer topmost. When a topmost window is made non-topmost, its owners and its owned windows are also made non-topmost windows.
            /// A non-topmost window can own a topmost window, but the reverse cannot occur. Any window (for example, a dialog box) owned by a topmost window is itself made a topmost window, to ensure that all owned windows stay above their owner.
            /// If an application is not in the foreground, and should be in the foreground, it must call the SetForegroundWindow function.
            /// To use SetWindowPos to bring a window to the top, the process that owns the window must have SetForegroundWindow permission.
            /// </remarks>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

            /// BOOL CALLBACK EnumWindowsProc(
            /// _In_ HWND   hwnd,
            /// _In_ LPARAM lParam
            /// );
            /// <summary>
            /// An application-defined callback function used with the EnumWindows or EnumDesktopWindows function. It receives top-level window handles. The WNDENUMPROC type defines a pointer to this callback function. EnumWindowsProc is a placeholder for the application-defined function name.
            /// </summary>
            /// <param name="hwnd">
            /// hwnd [in]
            /// A handle to a top-level window.
            /// </param>
            /// <param name="lParam">
            /// lParam [in]
            /// The application-defined value given in EnumWindows or EnumDesktopWindows.
            /// </param>
            /// <returns>
            /// To continue enumeration, the callback function must return TRUE; to stop enumeration, it must return FALSE.
            /// </returns>
            /// <remarks>
            /// An application must register this callback function by passing its address to EnumWindows or EnumDesktopWindows.
            /// </remarks>
            public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

            public enum SetWindowPosInsertAfter
            {

                /// <summary>
                /// Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows.
                /// </summary>
                Bottom = 1,
                /// <summary>
                /// Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.
                /// </summary>
                NoTopMost = -2,
                /// <summary>
                /// Places the window at the top of the Z order.
                /// </summary>
                Top = 0,
                /// <summary>
                /// Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.
                /// </summary>
                TopMost = -1
            }

            public enum SetWindowPosFlags : uint
            {

                /// <summary>
                /// If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
                /// </summary>
                Async‏WindowPos = 0x4000,
                /// <summary>
                /// Prevents generation of the WM_SYNCPAINT message.
                /// </summary>
                DefereRase = 0x2000,
                /// <summary>
                /// Draws a frame (defined in the window's class description) around the window.
                /// </summary>
                DrawFrame = 0x20,
                /// <summary>
                /// Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
                /// </summary>
                FrameChanged = 0x20,
                /// <summary>
                /// Hides the window.
                /// </summary>
                HideWindow = 0x80,
                /// <summary>
                /// Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
                /// </summary>
                NoActivate = 0x10,
                /// <summary>
                /// Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
                /// </summary>
                NoCopyBits = 0x100,
                /// <summary>
                /// Retains the current position (ignores X and Y parameters).
                /// </summary>
                NoMove = 0x2,
                /// <summary>
                /// Does not change the owner window's position in the Z order.
                /// </summary>
                NoOwnerZOrder = 0x200,
                /// <summary>
                /// Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
                /// </summary>
                NoRedraw = 0x8,
                /// <summary>
                /// Same as the SWP_NOOWNERZORDER flag.
                /// </summary>
                NoReposition = 0x200,
                /// <summary>
                /// Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
                /// </summary>
                NoSendChanging = 0x400,
                /// <summary>
                /// Retains the current size (ignores the cx and cy parameters).
                /// </summary>
                NoSize = 0x1,
                /// <summary>
                /// Retains the current Z order (ignores the hWndInsertAfter parameter).
                /// </summary>
                NoZOrder = 0x4,
                /// <summary>
                /// Displays the window.
                /// </summary>
                ShowWindow = 0x40
            }

            public enum GetWindowCommand : uint
            {

                /// <summary>
                /// The retrieved handle identifies the window of the same type that is highest in the Z order.
                /// If the specified window is a topmost window, the handle identifies a topmost window. If the specified window is a top-level window, the handle identifies a top-level window. If the specified window is a child window, the handle identifies a sibling window.
                /// </summary>
                HwndFirst = 0,
                /// <summary>
                /// The retrieved handle identifies the window of the same type that is lowest in the Z order.
                /// If the specified window is a topmost window, the handle identifies a topmost window. If the specified window is a top-level window, the handle identifies a top-level window. If the specified window is a child window, the handle identifies a sibling window.
                /// </summary>
                HwndLast = 1,
                /// <summary>
                /// The retrieved handle identifies the window below the specified window in the Z order.
                /// If the specified window is a topmost window, the handle identifies a topmost window. If the specified window is a top-level window, the handle identifies a top-level window. If the specified window is a child window, the handle identifies a sibling window.
                /// </summary>
                HwndNext = 2,
                /// <summary>
                /// The retrieved handle identifies the window above the specified window in the Z order.
                /// If the specified window is a topmost window, the handle identifies a topmost window. If the specified window is a top-level window, the handle identifies a top-level window. If the specified window is a child window, the handle identifies a sibling window.
                /// </summary>
                HwndPrev = 3,
                /// <summary>
                /// The retrieved handle identifies the specified window's owner window, if any. For more information, see Owned Windows.
                /// </summary>
                Owner = 4,
                /// <summary>
                /// The retrieved handle identifies the child window at the top of the Z order, if the specified window is a parent window; otherwise, the retrieved handle is NULL. The function examines only child windows of the specified window. It does not examine descendant windows.
                /// </summary>
                Child = 5,
                /// <summary>
                /// The retrieved handle identifies the enabled popup window owned by the specified window (the search uses the first such window found using GW_HWNDNEXT); otherwise, if there are no enabled popup windows, the retrieved handle is that of the specified window.
                /// </summary>
                EnabledPopup = 6
            }

            public enum GetAncestorFlags : uint
            {

                /// <summary>
                /// Retrieves the parent window. This does not include the owner, as it does with the GetParent function.
                /// </summary>
                Parent = 1,
                /// <summary>
                /// Retrieves the root window by walking the chain of parent windows.
                /// </summary>
                Root = 2,
                /// <summary>
                /// Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent.
                /// </summary>
                RootOwner = 3
            }

            /// typedef struct tagWINDOWPLACEMENT {
            /// UINT  length;
            /// UINT  flags;
            /// UINT  showCmd;
            /// POINT ptMinPosition;
            /// POINT ptMaxPosition;
            /// RECT  rcNormalPosition;
            /// } WINDOWPLACEMENT, *PWINDOWPLACEMENT, *LPWINDOWPLACEMENT;
            /// <summary>
            /// Contains information about the placement of a window on the screen.
            /// </summary>
            /// <remarks>
            /// If the window is a top-level window that does not have the WS_EX_TOOLWINDOW window style, then the coordinates represented by the following members are in workspace coordinates: ptMinPosition, ptMaxPosition, and rcNormalPosition. Otherwise, these members are in screen coordinates.
            /// Workspace coordinates differ from screen coordinates in that they take the locations and sizes of application toolbars (including the taskbar) into account. Workspace coordinate (0,0) is the upper-left corner of the workspace area, the area of the screen not being used by application toolbars.
            /// The coordinates used in a WINDOWPLACEMENT structure should be used only by the GetWindowPlacement and SetWindowPlacement functions. Passing workspace coordinates to functions which expect screen coordinates (such as SetWindowPos) will result in the window appearing in the wrong location. For example, if the taskbar is at the top of the screen, saving window coordinates using GetWindowPlacement and restoring them using SetWindowPos causes the window to appear to "creep" up the screen.
            /// </remarks>
            public struct WindowPlacement
            {
                public static readonly uint ActualLength = (uint) Marshal.SizeOf<WindowPlacement>();

                public void Init()
                {
                    this.Length = ActualLength;
                }

                public WindowPlacement InitNew()
                {
                    this.Length = ActualLength;
                    return this;
                }

                /// <summary>
                /// Type: UINT
                /// The length of the structure, in bytes. Before calling the GetWindowPlacement or SetWindowPlacement functions, set this member to sizeof(WINDOWPLACEMENT).
                /// GetWindowPlacement and SetWindowPlacement fail if this member is not set correctly.
                /// </summary>
                public uint Length;
                /// <summary>
                /// Type: UINT
                /// The flags that control the position of the minimized window and the method by which the window is restored. This member can be one or more of the following values.
                /// </summary>
                public WindowPlacementFlags Flags;
                /// <summary>
                /// Type: UINT
                /// The current show state of the window. This member can be one of the following values.
                /// </summary>
                public WindowShowState ShowCmd;
                /// <summary>
                /// Type: POINT
                /// The coordinates of the window's upper-left corner when the window is minimized.
                /// </summary>
                public Point PtMinPosition;
                /// <summary>
                /// Type: POINT
                /// The coordinates of the window's upper-left corner when the window is maximized.
                /// </summary>
                public Point PtMaxPosition;
                /// <summary>
                /// Type: RECT
                /// The window's coordinates when the window is in the restored position.
                /// </summary>
                public Rect RcNormalPosition;
            }

            public enum WindowShowState : uint
            {

                /// <summary>
                /// Hides the window and activates another window.
                /// </summary>
                Hide = 0,
                /// <summary>
                /// Maximizes the specified window.
                /// </summary>
                Maximize = 3,
                /// <summary>
                /// Minimizes the specified window and activates the next top-level window in the z-order.
                /// </summary>
                Minimize = 6,
                /// <summary>
                /// Activates and displays the window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window.
                /// </summary>
                Restore = 9,
                /// <summary>
                /// Activates the window and displays it in its current size and position.
                /// </summary>
                Show = 5,
                /// <summary>
                /// Activates the window and displays it as a maximized window.
                /// </summary>
                ShowMaximized = 3,
                /// <summary>
                /// Activates the window and displays it as a minimized window.
                /// </summary>
                ShowMinimized = 2,
                /// <summary>
                /// Displays the window as a minimized window.
                /// This value is similar to SW_SHOWMINIMIZED, except the window is not activated.
                /// </summary>
                ShowMinNoActive = 7,
                /// <summary>
                /// Displays the window in its current size and position.
                /// This value is similar to SW_SHOW, except the window is not activated.
                /// </summary>
                ShowNA = 8,
                /// <summary>
                /// Displays a window in its most recent size and position.
                /// This value is similar to SW_SHOWNORMAL, except the window is not activated.
                /// </summary>
                ShowNoActivate = 4,
                /// <summary>
                /// Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.
                /// </summary>
                Shownormal = 1
            }

            public enum WindowPlacementFlags : uint
            {

                /// <summary>
                /// If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
                /// </summary>
                CWindowPlacement = 0x4,
                /// <summary>
                /// The restored window will be maximized, regardless of whether it was maximized before it was minimized. This setting is only valid the next time the window is restored. It does not change the default restoration behavior.
                /// This flag is only valid when the SW_SHOWMINIMIZED value is specified for the showCmd member.
                /// </summary>
                RestoreToMaximized = 0x2,
                /// <summary>
                /// The coordinates of the minimized window may be specified.
                /// This flag must be specified if the coordinates are set in the ptMinPosition member.
                /// </summary>
                SetMinPosition = 0x1
            }
        }
    }
}
