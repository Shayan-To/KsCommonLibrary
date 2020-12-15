using System.Diagnostics;
using System.Collections.Generic;
using System;

namespace Ks
{
    namespace Common.Win32
    {
        public abstract partial class Windows
        {

            public static class Helpers
            {

                public static bool SetWindowPos(IntPtr hwnd, Unsafe.SetWindowPosInsertAfter hWndInsertAfter, int X, int Y, int cx, int cy, Unsafe.SetWindowPosFlags uFlags)
                {
                    return Unsafe.SetWindowPos(hwnd, new IntPtr((int)hWndInsertAfter), X, Y, cx, cy, uFlags);
                }

                public static Unsafe.WindowPlacement GetWindowPlacement(IntPtr hWnd)
                {
                    var R = new Unsafe.WindowPlacement().InitNew();
                    Unsafe.GetWindowPlacement(hWnd, out R);
                    Common.VerifyError();
                    return R;
                }
            }

            public static Unsafe.WindowShowState GetWindowState(IntPtr Hwnd)
            {
                return Helpers.GetWindowPlacement(Hwnd).ShowCmd;
            }

            public static IntPtr[] GetAllWindows()
            {
                var L = new List<IntPtr>();
                Unsafe.EnumWindows((hWnd, lParam) =>
                {
                    L.Add(hWnd);
                    return true;
                }, IntPtr.Zero);
                Common.VerifyError();
                return L.ToArray();
            }

            public static Rect GetWindowRect(IntPtr hWnd)
            {
                var R = default(Rect);
                Unsafe.GetWindowRect(hWnd, out R);
                Common.VerifyError();
                return R;
            }

            public static IntPtr GetForegroundWindow()
            {
                var R = Unsafe.GetForegroundWindow();
                Common.VerifyError();
                return R;
            }

            public static string GetWindowText(IntPtr hWnd)
            {
                var Length = Unsafe.GetWindowTextLength(hWnd);
                var R = new System.Text.StringBuilder(Length + 2);

                var T = Unsafe.GetWindowText(hWnd, R, R.Capacity);

                Common.VerifyError();
                Assert.True(T == Length);

                return R.ToString();
            }

            public static IntPtr FindWindow(string lpClassName, string lpWindowName)
            {
                var R = Unsafe.FindWindow(lpClassName, lpWindowName);
                Common.VerifyError();
                return R;
            }

            public static (uint ProcessId, uint ThreadId) GetWindowThreadProcessId(IntPtr hwnd)
            {
                var ProcessId = 0u;
                var ThreadId = Unsafe.GetWindowThreadProcessId(hwnd, out ProcessId);
                Common.VerifyError();
                return (ProcessId, ThreadId);
            }

            public static Process GetWindowProcess(IntPtr hwnd)
            {
                return Process.GetProcessById((int)GetWindowThreadProcessId(hwnd).ProcessId);
            }

            public static IntPtr WindowFromPoint(Point p)
            {
                var R = Unsafe.WindowFromPoint(p);
                Common.VerifyError();
                return R;
            }

            public static IntPtr GetAncestorWindow(IntPtr hwnd, AncestorKind Kind)
            {
                IntPtr R = default;

                if (Kind == AncestorKind.ParentOrOwner)
                    R = Unsafe.GetParent(hwnd);
                else if ((GetAncestorConstant <= (uint)Kind) & ((uint)Kind < GetWindowConstant))
                    R = Unsafe.GetAncestor(hwnd, (Unsafe.GetAncestorFlags)((uint)Kind - GetAncestorConstant));
                else if (GetWindowConstant <= (long)Kind)
                    R = Unsafe.GetWindow(hwnd, (Unsafe.GetWindowCommand)((uint)Kind - GetWindowConstant));
                else
                    Verify.FailArg(nameof(Kind), "Invalid kind.");

                Common.VerifyError();
                return R;
            }

            public static void SetTopMost(IntPtr hwnd, bool TopMost)
            {
                Helpers.SetWindowPos(hwnd, TopMost ? Unsafe.SetWindowPosInsertAfter.TopMost : Unsafe.SetWindowPosInsertAfter.NoTopMost, 0, 0, 0, 0, Unsafe.SetWindowPosFlags.NoMove | Unsafe.SetWindowPosFlags.NoSize | Unsafe.SetWindowPosFlags.ShowWindow);
                Common.VerifyError();
            }

            private const uint GetAncestorConstant = 100;
            private const uint GetWindowConstant = 200;

            public enum AncestorKind : uint
            {

                /// <summary>
            /// Retrieve the parent or the owner window.
            /// </summary>
                ParentOrOwner = 0,

                /// <summary>
            /// Retrieve the parent window. This does not include the owner.
            /// </summary>
                Parent = GetAncestorConstant + 1,

                /// <summary>
            /// Retrieve the root window by walking the chain of parent windows.
            /// </summary>
                Root = GetAncestorConstant + 2,

                /// <summary>
            /// Retrieve the owned root window by walking the chain of parent/owner windows.
            /// </summary>
                RootOwner = GetAncestorConstant + 3,

                /// <summary>
            /// Retrieve the window of the same type that is highest in the Z order.
            /// If the specified window is a topmost window, the handle identifies a topmost window.
            /// If the specified window is a top-level window, the handle identifies a top-level window.
            /// If the specified window is a child window, the handle identifies a sibling window.
            /// </summary>
                HwndFirst = GetWindowConstant + 0,

                /// <summary>
            /// Retrieve the window of the same type that is lowest in the Z order.
            /// If the specified window is a topmost window, the handle identifies a topmost window.
            /// If the specified window is a top-level window, the handle identifies a top-level window.
            /// If the specified window is a child window, the handle identifies a sibling window.
            /// </summary>
                HwndLast = GetWindowConstant + 1,

                /// <summary>
            /// Retrieve the window below the specified window in the Z order.
            /// If the specified window is a topmost window, the handle identifies a topmost window.
            /// If the specified window is a top-level window, the handle identifies a top-level window.
            /// If the specified window is a child window, the handle identifies a sibling window.
            /// </summary>
                HwndNext = GetWindowConstant + 2,

                /// <summary>
            /// Retrieve the window above the specified window in the Z order.
            /// If the specified window is a topmost window, the handle identifies a topmost window.
            /// If the specified window is a top-level window, the handle identifies a top-level window.
            /// If the specified window is a child window, the handle identifies a sibling window.
            /// </summary>
                HwndPrev = GetWindowConstant + 3,

                /// <summary>
            /// Retrieve the specified window's owner window, if any. For more information, see Owned Windows.
            /// </summary>
                Owner = GetWindowConstant + 4,

                /// <summary>
            /// Retrieve the child window at the top of the Z order, if the specified window is a parent window;
            /// otherwise, the retrieved handle is NULL.
            /// The function examines only child windows of the specified window. It does not examine descendant windows.
            /// </summary>
                Child = GetWindowConstant + 5,

                /// <summary>
            /// Retrieve the enabled popup window owned by the specified window (the search uses the first such window found using GW_HWNDNEXT);
            /// otherwise, if there are no enabled popup windows, the retrieved handle is that of the specified window.
            /// </summary>
                EnabledPopup = GetWindowConstant + 6
            }

            /// <summary>
        /// Documentation from https://msdn.microsoft.com/en-us/library/windows/desktop/ms646267(v=vs.85).aspx.
        /// Values are extracted from WinUser.h (line 424).
        /// </summary>
            public enum KeyFlags
            {

                /// #define KF_EXTENDED 0x0100
            /// <summary>
            /// Manipulates the extended key flag.
            /// </summary>
                Extended = 0x100,
                /// #define KF_DLGMODE 0x0800
            /// <summary>
            /// Manipulates the dialog mode flag, which indicates whether a dialog box is active.
            /// </summary>
                DlgMode = 0x800,
                /// #define KF_MENUMODE 0x1000
            /// <summary>
            /// Manipulates the menu mode flag, which indicates whether a menu is active.
            /// </summary>
                MenuMode = 0x1000,
                /// #define KF_ALTDOWN 0x2000
            /// <summary>
            /// Manipulates the ALT key flag, which indicates whether the ALT key is pressed.
            /// </summary>
                AltDown = 0x2000,
                /// #define KF_REPEAT 0x4000
            /// <summary>
            /// Manipulates the repeat count.
            /// </summary>
                Repeat = 0x4000,
                /// #define KF_UP 0x8000
            /// <summary>
            /// Manipulates the transition state flag.
            /// </summary>
                Up = 0x8000
            }

            /// typedef struct tagPOINT {
        /// LONG x;
        /// LONG y;
        /// } POINT, *PPOINT;
        /// <summary>
        /// The POINT structure defines the x- and y- coordinates of a point.
        /// </summary>
            public struct Point
            {
                public Point(int X, int Y)
                {
                    this.X = X;
                    this.Y = Y;
                }

                public Point(System.Drawing.Point Point) : this(Point.X, Point.Y)
                {
                }

                /// <summary>
            /// The x-coordinate of the point.
            /// </summary>
                public int X;
                /// <summary>
            /// The y-coordinate of the point.
            /// </summary>
                public int Y;
            }

            /// typedef struct _RECT {
        /// LONG left;
        /// LONG top;
        /// LONG right;
        /// LONG bottom;
        /// } RECT, *PRECT;
        /// <summary>
        /// The RECT structure defines the coordinates of the upper-left and lower-right corners of a rectangle.
        /// </summary>
        /// <remarks>
        /// By convention, the right and bottom edges of the rectangle are normally considered exclusive. In other words, the pixel whose coordinates are ( right, bottom ) lies immediately outside of the rectangle. For example, when RECT is passed to the FillRect function, the rectangle is filled up to, but not including, the right column and bottom row of pixels. This structure is identical to the RECTL structure.
        /// </remarks>
            public struct Rect
            {

                /// <summary>
            /// The x-coordinate of the upper-left corner of the rectangle.
            /// </summary>
                public int Left;
                /// <summary>
            /// The y-coordinate of the upper-left corner of the rectangle.
            /// </summary>
                public int Top;
                /// <summary>
            /// The x-coordinate of the lower-right corner of the rectangle.
            /// </summary>
                public int Right;
                /// <summary>
            /// The y-coordinate of the lower-right corner of the rectangle.
            /// </summary>
                public int Bottom;
            }
        }
    }
}
