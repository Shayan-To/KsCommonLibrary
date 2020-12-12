using System;
using System.Runtime.InteropServices;

namespace Ks.Common.Win32
{
    partial class Hooks
    {
        public static class Unsafe
        {

            /// HHOOK WINAPI SetWindowsHookEx(
            /// _In_ int       idHook,
            /// _In_ HOOKPROC  lpfn,
            /// _In_ HINSTANCE hMod,
            /// _In_ DWORD     dwThreadId
            /// );
            /// <summary>
            /// Installs an application-defined hook procedure into a hook chain. You would install a hook procedure to monitor the system for certain types of events. These events are associated either with a specific thread or with all threads in the same desktop as the calling thread.
            /// </summary>
            /// <param name="idHook">
            /// idHook [in]
            /// Type: int
            /// The type of hook procedure to be installed. This parameter can be one of the following values.
            /// <para />
            /// This parameter must equal <see cref="HookType.Keyboard"/>.
            /// </param>
            /// <param name="lpfn">
            /// lpfn [in]
            /// Type: HOOKPROC
            /// A pointer to the hook procedure. If the dwThreadId parameter is zero or specifies the identifier of a thread created by a different process, the lpfn parameter must point to a hook procedure in a DLL. Otherwise, lpfn can point to a hook procedure in the code associated with the current process.
            /// </param>
            /// <param name="hMod">
            /// hMod [in]
            /// Type: HINSTANCE
            /// A handle to the DLL containing the hook procedure pointed to by the lpfn parameter. The hMod parameter must be set to NULL if the dwThreadId parameter specifies a thread created by the current process and if the hook procedure is within the code associated with the current process.
            /// </param>
            /// <param name="dwThreadId">
            /// dwThreadId [in]
            /// Type: DWORD
            /// The identifier of the thread with which the hook procedure is to be associated. For desktop apps, if this parameter is zero, the hook procedure is associated with all existing threads running in the same desktop as the calling thread. For Windows Store apps, see the Remarks section.
            /// </param>
            /// <returns>
            /// Type: HHOOK
            /// If the function succeeds, the return value is the handle to the hook procedure.
            /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
            /// </returns>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern IntPtr SetWindowsHookEx(HookType idHook, KeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

            /// HHOOK WINAPI SetWindowsHookEx(
            /// _In_ int       idHook,
            /// _In_ HOOKPROC  lpfn,
            /// _In_ HINSTANCE hMod,
            /// _In_ DWORD     dwThreadId
            /// );
            /// <summary>
            /// Installs an application-defined hook procedure into a hook chain. You would install a hook procedure to monitor the system for certain types of events. These events are associated either with a specific thread or with all threads in the same desktop as the calling thread.
            /// </summary>
            /// <param name="idHook">
            /// idHook [in]
            /// Type: int
            /// The type of hook procedure to be installed. This parameter can be one of the following values.
            /// <para />
            /// This parameter must equal <see cref="HookType.KeyboardLL"/>.
            /// </param>
            /// <param name="lpfn">
            /// lpfn [in]
            /// Type: HOOKPROC
            /// A pointer to the hook procedure. If the dwThreadId parameter is zero or specifies the identifier of a thread created by a different process, the lpfn parameter must point to a hook procedure in a DLL. Otherwise, lpfn can point to a hook procedure in the code associated with the current process.
            /// </param>
            /// <param name="hMod">
            /// hMod [in]
            /// Type: HINSTANCE
            /// A handle to the DLL containing the hook procedure pointed to by the lpfn parameter. The hMod parameter must be set to NULL if the dwThreadId parameter specifies a thread created by the current process and if the hook procedure is within the code associated with the current process.
            /// </param>
            /// <param name="dwThreadId">
            /// dwThreadId [in]
            /// Type: DWORD
            /// The identifier of the thread with which the hook procedure is to be associated. For desktop apps, if this parameter is zero, the hook procedure is associated with all existing threads running in the same desktop as the calling thread. For Windows Store apps, see the Remarks section.
            /// </param>
            /// <returns>
            /// Type: HHOOK
            /// If the function succeeds, the return value is the handle to the hook procedure.
            /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
            /// </returns>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern IntPtr SetWindowsHookEx(HookType idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

            /// HHOOK WINAPI SetWindowsHookEx(
            /// _In_ int       idHook,
            /// _In_ HOOKPROC  lpfn,
            /// _In_ HINSTANCE hMod,
            /// _In_ DWORD     dwThreadId
            /// );
            /// <summary>
            /// Installs an application-defined hook procedure into a hook chain. You would install a hook procedure to monitor the system for certain types of events. These events are associated either with a specific thread or with all threads in the same desktop as the calling thread.
            /// </summary>
            /// <param name="idHook">
            /// idHook [in]
            /// Type: int
            /// The type of hook procedure to be installed. This parameter can be one of the following values.
            /// <para />
            /// This parameter must equal <see cref="HookType.Mouse"/>.
            /// </param>
            /// <param name="lpfn">
            /// lpfn [in]
            /// Type: HOOKPROC
            /// A pointer to the hook procedure. If the dwThreadId parameter is zero or specifies the identifier of a thread created by a different process, the lpfn parameter must point to a hook procedure in a DLL. Otherwise, lpfn can point to a hook procedure in the code associated with the current process.
            /// </param>
            /// <param name="hMod">
            /// hMod [in]
            /// Type: HINSTANCE
            /// A handle to the DLL containing the hook procedure pointed to by the lpfn parameter. The hMod parameter must be set to NULL if the dwThreadId parameter specifies a thread created by the current process and if the hook procedure is within the code associated with the current process.
            /// </param>
            /// <param name="dwThreadId">
            /// dwThreadId [in]
            /// Type: DWORD
            /// The identifier of the thread with which the hook procedure is to be associated. For desktop apps, if this parameter is zero, the hook procedure is associated with all existing threads running in the same desktop as the calling thread. For Windows Store apps, see the Remarks section.
            /// </param>
            /// <returns>
            /// Type: HHOOK
            /// If the function succeeds, the return value is the handle to the hook procedure.
            /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
            /// </returns>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern IntPtr SetWindowsHookEx(HookType idHook, MouseProc lpfn, IntPtr hMod, uint dwThreadId);

            /// HHOOK WINAPI SetWindowsHookEx(
            /// _In_ int       idHook,
            /// _In_ HOOKPROC  lpfn,
            /// _In_ HINSTANCE hMod,
            /// _In_ DWORD     dwThreadId
            /// );
            /// <summary>
            /// Installs an application-defined hook procedure into a hook chain. You would install a hook procedure to monitor the system for certain types of events. These events are associated either with a specific thread or with all threads in the same desktop as the calling thread.
            /// </summary>
            /// <param name="idHook">
            /// idHook [in]
            /// Type: int
            /// The type of hook procedure to be installed. This parameter can be one of the following values.
            /// <para />
            /// This parameter must equal <see cref="HookType.MouseLL"/>.
            /// </param>
            /// <param name="lpfn">
            /// lpfn [in]
            /// Type: HOOKPROC
            /// A pointer to the hook procedure. If the dwThreadId parameter is zero or specifies the identifier of a thread created by a different process, the lpfn parameter must point to a hook procedure in a DLL. Otherwise, lpfn can point to a hook procedure in the code associated with the current process.
            /// </param>
            /// <param name="hMod">
            /// hMod [in]
            /// Type: HINSTANCE
            /// A handle to the DLL containing the hook procedure pointed to by the lpfn parameter. The hMod parameter must be set to NULL if the dwThreadId parameter specifies a thread created by the current process and if the hook procedure is within the code associated with the current process.
            /// </param>
            /// <param name="dwThreadId">
            /// dwThreadId [in]
            /// Type: DWORD
            /// The identifier of the thread with which the hook procedure is to be associated. For desktop apps, if this parameter is zero, the hook procedure is associated with all existing threads running in the same desktop as the calling thread. For Windows Store apps, see the Remarks section.
            /// </param>
            /// <returns>
            /// Type: HHOOK
            /// If the function succeeds, the return value is the handle to the hook procedure.
            /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
            /// </returns>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern IntPtr SetWindowsHookEx(HookType idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

            /// BOOL WINAPI UnhookWindowsHookEx(
            /// _In_ HHOOK hhk
            /// );
            /// <summary>
            /// Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
            /// </summary>
            /// <param name="hhk">
            /// hhk [in]
            /// Type: HHOOK
            /// A handle to the hook to be removed. This parameter is a hook handle obtained by a previous call to SetWindowsHookEx.
            /// </param>
            /// <returns>
            /// Type: BOOL
            /// If the function succeeds, the return value is nonzero.
            /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
            /// </returns>
            /// <remarks>
            /// The hook procedure can be in the state of being called by another thread even after UnhookWindowsHookEx returns. If the hook procedure is not being called concurrently, the hook procedure is removed immediately before UnhookWindowsHookEx returns.
            /// </remarks>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern bool UnhookWindowsHookEx(IntPtr hhk);

            /// LRESULT WINAPI CallNextHookEx(
            /// _In_opt_ HHOOK  hhk,
            /// _In_     int    nCode,
            /// _In_     WPARAM wParam,
            /// _In_     LPARAM lParam
            /// );
            /// <summary>
            /// Passes the hook information to the next hook procedure in the current hook chain. A hook procedure can call this function either before or after processing the hook information.
            /// </summary>
            /// <param name="hhk">
            /// hhk [in, optional]
            /// Type: HHOOK
            /// This parameter is ignored.
            /// </param>
            /// <param name="nCode">
            /// nCode [in]
            /// Type: int
            /// The hook code passed to the current hook procedure. The next hook procedure uses this code to determine how to process the hook information.
            /// </param>
            /// <param name="wParam">
            /// wParam [in]
            /// Type: WPARAM
            /// The wParam value passed to the current hook procedure. The meaning of this parameter depends on the type of hook associated with the current hook chain.
            /// </param>
            /// <param name="lParam">
            /// lParam [in]
            /// Type: LPARAM
            /// The lParam value passed to the current hook procedure. The meaning of this parameter depends on the type of hook associated with the current hook chain.
            /// </param>
            /// <returns>
            /// Type: LRESULT
            /// This value is returned by the next hook procedure in the chain. The current hook procedure must also return this value. The meaning of the return value depends on the hook type. For more information, see the descriptions of the individual hook procedures.
            /// </returns>
            /// <remarks>
            /// Hook procedures are installed in chains for particular hook types. CallNextHookEx calls the next hook in the chain.
            /// Calling CallNextHookEx is optional, but it is highly recommended; otherwise, other applications that have installed hooks will not receive hook notifications and may behave incorrectly as a result. You should call CallNextHookEx unless you absolutely need to prevent the notification from being seen by other applications.
            /// </remarks>
            [DllImport("User32.dll", SetLastError = true)]
            public static extern bool CallNextHookEx(IntPtr hhk, int nCode, UIntPtr wParam, IntPtr lParam);

            public enum HookType : int
            {

                /// WH_CALLWNDPROC = 4
                /// <summary>
                /// Installs a hook procedure that monitors messages before the system sends them to the destination window procedure. For more information, see the CallWndProc hook procedure.
                /// </summary>
                CallWndProc = 4,
                /// WH_CALLWNDPROCRET = 12
                /// <summary>
                /// Installs a hook procedure that monitors messages after they have been processed by the destination window procedure. For more information, see the CallWndRetProc hook procedure.
                /// </summary>
                CallWndProcRet = 12,
                /// WH_CBT = 5
                /// <summary>
                /// Installs a hook procedure that receives notifications useful to a CBT application. For more information, see the CBTProc hook procedure.
                /// </summary>
                Cbt = 5,
                /// WH_DEBUG = 9
                /// <summary>
                /// Installs a hook procedure useful for debugging other hook procedures. For more information, see the DebugProc hook procedure.
                /// </summary>
                Debug = 9,
                /// WH_FOREGROUNDIDLE = 11
                /// <summary>
                /// Installs a hook procedure that will be called when the application's foreground thread is about to become idle. This hook is useful for performing low priority tasks during idle time. For more information, see the ForegroundIdleProc hook procedure.
                /// </summary>
                ForegroundIdle = 11,
                /// WH_GETMESSAGE = 3
                /// <summary>
                /// Installs a hook procedure that monitors messages posted to a message queue. For more information, see the GetMsgProc hook procedure.
                /// </summary>
                GetMessage = 3,
                /// WH_JOURNALPLAYBACK = 1
                /// <summary>
                /// Installs a hook procedure that posts messages previously recorded by a WH_JOURNALRECORD hook procedure. For more information, see the JournalPlaybackProc hook procedure.
                /// </summary>
                JournalPlayback = 1,
                /// WH_JOURNALRECORD = 0
                /// <summary>
                /// Installs a hook procedure that records input messages posted to the system message queue. This hook is useful for recording macros. For more information, see the JournalRecordProc hook procedure.
                /// </summary>
                JournalRecord = 0,
                /// WH_KEYBOARD = 2
                /// <summary>
                /// Installs a hook procedure that monitors keystroke messages. For more information, see the KeyboardProc hook procedure.
                /// </summary>
                Keyboard = 2,
                /// WH_KEYBOARD_LL = 13
                /// <summary>
                /// Installs a hook procedure that monitors low-level keyboard input events. For more information, see the LowLevelKeyboardProc hook procedure.
                /// </summary>
                KeyboardLL = 13,
                /// WH_MOUSE = 7
                /// <summary>
                /// Installs a hook procedure that monitors mouse messages. For more information, see the MouseProc hook procedure.
                /// </summary>
                Mouse = 7,
                /// WH_MOUSE_LL = 14
                /// <summary>
                /// Installs a hook procedure that monitors low-level mouse input events. For more information, see the LowLevelMouseProc hook procedure.
                /// </summary>
                MouseLL = 14,
                /// WH_MSGFILTER = -1
                /// <summary>
                /// Installs a hook procedure that monitors messages generated as a result of an input event in a dialog box, message box, menu, or scroll bar. For more information, see the MessageProc hook procedure.
                /// </summary>
                MsgFilter = -1,
                /// WH_SHELL = 10
                /// <summary>
                /// Installs a hook procedure that receives notifications useful to shell applications. For more information, see the ShellProc hook procedure.
                /// </summary>
                Shell = 10,
                /// WH_SYSMSGFILTER = 6
                /// <summary>
                /// Installs a hook procedure that monitors messages generated as a result of an input event in a dialog box, message box, menu, or scroll bar. The hook procedure monitors these messages for all applications in the same desktop as the calling thread. For more information, see the SysMsgProc hook procedure.
                /// </summary>
                SysMsgFilter = 6
            }
        }
    }
}
