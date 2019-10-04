Imports System.Runtime.InteropServices

Namespace Common.Win32


    ' Open tabs:
    '  Hooks Overview (Windows)                      https://msdn.microsoft.com/en-us/library/windows/desktop/ms644959(v=vs.85).aspx#wh_keyboard_llhook
    '  KeyboardProc callback function (Windows)      https://msdn.microsoft.com/en-us/library/windows/desktop/ms644984(v=vs.85).aspx
    '  LowLevelMouseProc callback function (Windows)      https://msdn.microsoft.com/en-us/library/windows/desktop/ms644986(v=vs.85).aspx
    '  MouseProc callback function (Windows)         https://msdn.microsoft.com/en-us/library/windows/desktop/ms644988(v=vs.85).aspx
    '  LowLevelKeyboardProc callback function (Windows)      https://msdn.microsoft.com/en-us/library/windows/desktop/ms644985(v=vs.85).aspx
    '  SetWindowsHookEx function (Windows)           https://msdn.microsoft.com/en-us/library/windows/desktop/ms644990(v=vs.85).aspx
    '  Hooks (Windows)                               https://msdn.microsoft.com/en-us/library/windows/desktop/ms632589(v=vs.85).aspx
    '  Using Hooks (Windows)                         https://msdn.microsoft.com/en-us/library/windows/desktop/ms644960(v=vs.85).aspx
    '  CallNextHookEx function (Windows)             https://msdn.microsoft.com/en-us/library/windows/desktop/ms644974(v=vs.85).aspx
    '  UnhookWindowsHookEx function (Windows)        https://msdn.microsoft.com/en-us/library/windows/desktop/ms644993(v=vs.85).aspx
    '  KBDLLHOOKSTRUCT structure (Windows)           https://msdn.microsoft.com/en-us/library/windows/desktop/ms644967(v=vs.85).aspx
    '  MSLLHOOKSTRUCT structure (Windows)            https://msdn.microsoft.com/en-us/library/windows/desktop/ms644970(v=vs.85).aspx
    '  Raw Input (Windows)                           https://msdn.microsoft.com/en-us/library/ms645536(VS.85).aspx
    '  MOUSEHOOKSTRUCT structure (Windows)           https://msdn.microsoft.com/en-us/library/windows/desktop/ms644968(v=vs.85).aspx
    '  About Raw Input (Windows)                     https://msdn.microsoft.com/en-us/library/ms645543(v=vs.85).aspx
    '  Using Raw Input (Windows)                     https://msdn.microsoft.com/en-us/library/ms645546(v=vs.85).aspx
    '  KF_ALTDOWN - Bing                             https://www.bing.com/search?q=KF_ALTDOWN&pc=MOZI&form=MOZCON
    '  pinvoke.net: KeyFlags (Enums)                 https://www.pinvoke.net/default.aspx/Enums/KeyFlags.html
    '  About Keyboard Input (Windows)                https://msdn.microsoft.com/en-us/library/windows/desktop/ms646267(v=vs.85).aspx


    ''' <summary>
    ''' Documentation is available here: https://msdn.microsoft.com/en-us/library/windows/desktop/ms644959(v=vs.85).aspx
    ''' </summary>
    ''' <remarks>
    ''' From remarks section of SetWindowsHookEx.
    ''' SetWindowsHookEx can be used to inject a DLL into another process. A 32-bit DLL cannot be injected into a 64-bit process, and a 64-bit DLL cannot be injected into a 32-bit process. If an application requires the use of hooks in other processes, it is required that a 32-bit application call SetWindowsHookEx to inject a 32-bit DLL into 32-bit processes, and a 64-bit application call SetWindowsHookEx to inject a 64-bit DLL into 64-bit processes. The 32-bit and 64-bit DLLs must have different names.
    ''' Because hooks run in the context of an application, they must match the "bitness" of the application. If a 32-bit application installs a global hook on 64-bit Windows, the 32-bit hook is injected into each 32-bit process (the usual security boundaries apply). In a 64-bit process, the threads are still marked as "hooked." However, because a 32-bit application must run the hook code, the system executes the hook in the hooking app's context; specifically, on the thread that called SetWindowsHookEx. This means that the hooking application must continue to pump messages or it might block the normal functioning of the 64-bit processes.
    ''' If a 64-bit application installs a global hook on 64-bit Windows, the 64-bit hook is injected into each 64-bit process, while all 32-bit processes use a callback to the hooking application.
    ''' To hook all applications on the desktop of a 64-bit Windows installation, install a 32-bit global hook and a 64-bit global hook, each from appropriate processes, and be sure to keep pumping messages in the hooking application to avoid blocking normal functioning. If you already have a 32-bit global hooking application and it doesn't need to run in each application's context, you may not need to create a 64-bit version.
    ''' An error may occur if the hMod parameter is NULL and the dwThreadId parameter is zero or specifies the identifier of a thread created by another process.
    ''' Calling the CallNextHookEx function to chain to the next hook procedure is optional, but it is highly recommended; otherwise, other applications that have installed hooks will not receive hook notifications and may behave incorrectly as a result. You should call CallNextHookEx unless you absolutely need to prevent the notification from being seen by other applications.
    ''' Before terminating, an application must call the UnhookWindowsHookEx function to free system resources associated with the hook.
    ''' The scope of a hook depends on the hook type. Some hooks can be set only with global scope; others can also be set for only a specific thread, as shown in the following table.
    ''' Hook                 Scope
    ''' WH_CALLWNDPROC       Thread or global
    ''' WH_CALLWNDPROCRET    Thread or global
    ''' WH_CBT               Thread or global
    ''' WH_DEBUG             Thread or global
    ''' WH_FOREGROUNDIDLE    Thread or global
    ''' WH_GETMESSAGE        Thread or global
    ''' WH_JOURNALPLAYBACK   Global only
    ''' WH_JOURNALRECORD     Global only
    ''' WH_KEYBOARD          Thread or global
    ''' WH_KEYBOARD_LL       Global only
    ''' WH_MOUSE             Thread or global
    ''' WH_MOUSE_LL          Global only
    ''' WH_MSGFILTER         Thread or global
    ''' WH_SHELL             Thread or global
    ''' WH_SYSMSGFILTER      Global only
    ''' For a specified hook type, thread hooks are called first, then global hooks. Be aware that the WH_MOUSE, WH_KEYBOARD, WH_JOURNAL*, WH_SHELL, and low-level hooks can be called on the thread that installed the hook rather than the thread processing the hook. For these hooks, it is possible that both the 32-bit and 64-bit hooks will be called if a 32-bit hook is ahead of a 64-bit hook in the hook chain.
    ''' The global hooks are a shared resource, and installing one affects all applications in the same desktop as the calling thread. All global hook functions must be in libraries. Global hooks should be restricted to special-purpose applications or to use as a development aid during application debugging. Libraries that no longer need a hook should remove its hook procedure.
    ''' Windows Store app development If dwThreadId is zero, then window hook DLLs are not loaded in-process for the Windows Store app processes and the Windows Runtime broker process unless they are installed by either UIAccess processes (accessibility tools). The notification is delivered on the installer's thread for these hooks:
    '''     WH_JOURNALPLAYBACK
    '''     WH_JOURNALRECORD
    '''     WH_KEYBOARD
    '''     WH_KEYBOARD_LL
    '''     WH_MOUSE
    '''     WH_MOUSE_LL
    ''' This behavior is similar to what happens when there is an architecture mismatch between the hook DLL and the target application process, for example, when the hook DLL is 32-bit and the application process 64-bit.
    ''' </remarks>
    Public MustInherit Class Hooks

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        ''' LRESULT CALLBACK LowLevelKeyboardProc(
        '''   _In_ int    nCode,
        '''   _In_ WPARAM wParam,
        '''   _In_ LPARAM lParam
        ''' );
        ''' <summary>
        ''' An application-defined or library-defined callback function used with the SetWindowsHookEx function. The system calls this function every time a new keyboard input event is about to be posted into a thread input queue.
        ''' Note  When this callback function is called in response to a change in the state of a key, the callback function is called before the asynchronous state of the key is updated. Consequently, the asynchronous state of the key cannot be determined by calling GetAsyncKeyState from within the callback function.
        ''' The HOOKPROC type defines a pointer to this callback function. LowLevelKeyboardProc is a placeholder for the application-defined or library-defined function name.
        ''' </summary>
        ''' <param name="nCode">
        ''' nCode [in]
        ''' Type: int
        ''' A code the hook procedure uses to determine how to process the message. If nCode is less than zero, the hook procedure must pass the message to the CallNextHookEx function without further processing and should return the value returned by CallNextHookEx. This parameter can be one of the following values.
        ''' <para />
        ''' HC_ACTION = 0
        ''' The wParam and lParam parameters contain information about a keyboard message.
        ''' </param>
        ''' <param name="wParam">
        ''' wParam [in]
        ''' Type: WPARAM
        ''' The identifier of the keyboard message. This parameter can be one of the following messages: WM_KEYDOWN, WM_KEYUP, WM_SYSKEYDOWN, or WM_SYSKEYUP.
        ''' </param>
        ''' <param name="lParam">
        ''' lParam [in]
        ''' Type: LPARAM
        ''' A pointer to a KBDLLHOOKSTRUCT structure.
        ''' </param>
        ''' <returns>
        ''' Type: LRESULT
        ''' If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx.
        ''' If nCode is greater than or equal to zero, and the hook procedure did not process the message, it is highly recommended that you call CallNextHookEx and return the value it returns; otherwise, other applications that have installed WH_KEYBOARD_LL hooks will not receive hook notifications and may behave incorrectly as a result. If the hook procedure processed the message, it may return a nonzero value to prevent the system from passing the message to the rest of the hook chain or the target window procedure.
        ''' </returns>
        ''' <remarks>
        ''' An application installs the hook procedure by specifying the WH_KEYBOARD_LL hook type and a pointer to the hook procedure in a call to the SetWindowsHookEx function.
        ''' This hook is called in the context of the thread that installed it. The call is made by sending a message to the thread that installed the hook. Therefore, the thread that installed the hook must have a message loop.
        ''' The keyboard input can come from the local keyboard driver or from calls to the keybd_event function. If the input comes from a call to keybd_event, the input was "injected". However, the WH_KEYBOARD_LL hook is not injected into another process. Instead, the context switches back to the process that installed the hook and it is called in its original context. Then the context switches back to the application that generated the event.
        ''' The hook procedure should process a message in less time than the data entry specified in the LowLevelHooksTimeout value in the following registry key:
        ''' HKEY_CURRENT_USER\Control Panel\Desktop
        ''' The value is in milliseconds. If the hook procedure times out, the system passes the message to the next hook. However, on Windows 7 and later, the hook is silently removed without being called. There is no way for the application to know whether the hook is removed.
        ''' Note  Debug hooks cannot track this type of low level keyboard hooks. If the application must use low level hooks, it should run the hooks on a dedicated thread that passes the work off to a worker thread and then immediately returns. In most cases where the application needs to use low level hooks, it should monitor raw input instead. This is because raw input can asynchronously monitor mouse and keyboard messages that are targeted for other threads more effectively than low level hooks can. For more information on raw input, see Raw Input.
        ''' </remarks>
        Public Delegate Function LowLevelKeyboardProc(nCode As Integer, wParam As UIntPtr, lParam As IntPtr) As IntPtr

        ''' LRESULT CALLBACK KeyboardProc(
        '''   _In_ int    code,
        '''   _In_ WPARAM wParam,
        '''   _In_ LPARAM lParam
        ''' );
        ''' <summary>
        ''' An application-defined or library-defined callback function used with the SetWindowsHookEx function. The system calls this function whenever an application calls the GetMessage or PeekMessage function and there is a keyboard message (WM_KEYUP or WM_KEYDOWN) to be processed.
        ''' The HOOKPROC type defines a pointer to this callback function. KeyboardProc is a placeholder for the application-defined or library-defined function name.
        ''' </summary>
        ''' <param name="nCode">
        ''' code [in]
        ''' Type: int
        ''' A code the hook procedure uses to determine how to process the message. If code is less than zero, the hook procedure must pass the message to the CallNextHookEx function without further processing and should return the value returned by CallNextHookEx. This parameter can be one of the following values.
        ''' <para />
        ''' HC_ACTION = 0
        ''' The wParam and lParam parameters contain information about a keystroke message.
        ''' <para />
        ''' HC_NOREMOVE = 3
        ''' The wParam and lParam parameters contain information about a keystroke message, and the keystroke message has not been removed from the message queue. (An application called the PeekMessage function, specifying the PM_NOREMOVE flag.)
        ''' </param>
        ''' <param name="wParam">
        ''' wParam [in]
        ''' Type: WPARAM
        ''' The virtual-key code of the key that generated the keystroke message.
        ''' </param>
        ''' <param name="lParam">
        ''' lParam [in]
        ''' Type: LPARAM
        ''' The repeat count, scan code, extended-key flag, context code, previous key-state flag, and transition-state flag. For more information about the lParam parameter, see Keystroke Message Flags. The following table describes the bits of this value.
        ''' <para />
        ''' Bits    Description
        ''' 0-15    The repeat count. The value is the number of times the keystroke is repeated as a result of the user's holding down the key.
        ''' 16-23   The scan code. The value depends on the OEM.
        ''' 24      Indicates whether the key is an extended key, such as a function key or a key on the numeric keypad. The value is 1 if the key is an extended key; otherwise, it is 0.
        ''' 25-28   Reserved.
        ''' 29      The context code. The value is 1 if the ALT key is down; otherwise, it is 0.
        ''' 30      The previous key state. The value is 1 if the key is down before the message is sent; it is 0 if the key is up.
        ''' 31      The transition state. The value is 0 if the key is being pressed and 1 if it is being released.
        ''' </param>
        ''' <returns>
        ''' Type: LRESULT
        ''' If code is less than zero, the hook procedure must return the value returned by CallNextHookEx.
        ''' If code is greater than or equal to zero, and the hook procedure did not process the message, it is highly recommended that you call CallNextHookEx and return the value it returns; otherwise, other applications that have installed WH_KEYBOARD hooks will not receive hook notifications and may behave incorrectly as a result. If the hook procedure processed the message, it may return a nonzero value to prevent the system from passing the message to the rest of the hook chain or the target window procedure.
        ''' </returns>
        ''' <remarks>
        ''' An application installs the hook procedure by specifying the WH_KEYBOARD hook type and a pointer to the hook procedure in a call to the SetWindowsHookEx function.
        ''' This hook may be called in the context of the thread that installed it. The call is made by sending a message to the thread that installed the hook. Therefore, the thread that installed the hook must have a message loop.
        ''' </remarks>
        Public Delegate Function KeyboardProc(nCode As Integer, wParam As UIntPtr, lParam As IntPtr) As IntPtr

        ''' LRESULT CALLBACK LowLevelMouseProc(
        '''   _In_ int    nCode,
        '''   _In_ WPARAM wParam,
        '''   _In_ LPARAM lParam
        ''' );
        ''' <summary>
        ''' An application-defined or library-defined callback function used with the SetWindowsHookEx function. The system calls this function every time a new mouse input event is about to be posted into a thread input queue.
        ''' The HOOKPROC type defines a pointer to this callback function. LowLevelMouseProc is a placeholder for the application-defined or library-defined function name.
        ''' </summary>
        ''' <param name="nCode">
        ''' nCode [in]
        ''' Type: int
        ''' A code the hook procedure uses to determine how to process the message. If nCode is less than zero, the hook procedure must pass the message to the CallNextHookEx function without further processing and should return the value returned by CallNextHookEx. This parameter can be one of the following values.
        ''' <para />
        ''' HC_ACTION = 0
        ''' The wParam and lParam parameters contain information about a mouse message.
        ''' </param>
        ''' <param name="wParam">
        ''' wParam [in]
        ''' Type: WPARAM
        ''' The identifier of the mouse message. This parameter can be one of the following messages: WM_LBUTTONDOWN, WM_LBUTTONUP, WM_MOUSEMOVE, WM_MOUSEWHEEL, WM_MOUSEHWHEEL, WM_RBUTTONDOWN, or WM_RBUTTONUP.
        ''' </param>
        ''' <param name="lParam">
        ''' lParam [in]
        ''' Type: LPARAM
        ''' A pointer to an MSLLHOOKSTRUCT structure.
        ''' </param>
        ''' <returns>
        ''' Type: LRESULT
        ''' If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx.
        ''' If nCode is greater than or equal to zero, and the hook procedure did not process the message, it is highly recommended that you call CallNextHookEx and return the value it returns; otherwise, other applications that have installed WH_MOUSE_LL hooks will not receive hook notifications and may behave incorrectly as a result. If the hook procedure processed the message, it may return a nonzero value to prevent the system from passing the message to the rest of the hook chain or the target window procedure.
        ''' </returns>
        ''' <remarks>
        ''' An application installs the hook procedure by specifying the WH_MOUSE_LL hook type and a pointer to the hook procedure in a call to the SetWindowsHookEx function.
        ''' This hook is called in the context of the thread that installed it. The call is made by sending a message to the thread that installed the hook. Therefore, the thread that installed the hook must have a message loop.
        ''' The mouse input can come from the local mouse driver or from calls to the mouse_event function. If the input comes from a call to mouse_event, the input was "injected". However, the WH_MOUSE_LL hook is not injected into another process. Instead, the context switches back to the process that installed the hook and it is called in its original context. Then the context switches back to the application that generated the event.
        ''' The hook procedure should process a message in less time than the data entry specified in the LowLevelHooksTimeout value in the following registry key:
        ''' HKEY_CURRENT_USER\Control Panel\Desktop
        ''' The value is in milliseconds. If the hook procedure times out, the system passes the message to the next hook. However, on Windows 7 and later, the hook is silently removed without being called. There is no way for the application to know whether the hook is removed.
        ''' Note  Debug hooks cannot track this type of low level mouse hooks. If the application must use low level hooks, it should run the hooks on a dedicated thread that passes the work off to a worker thread and then immediately returns. In most cases where the application needs to use low level hooks, it should monitor raw input instead. This is because raw input can asynchronously monitor mouse and keyboard messages that are targeted for other threads more effectively than low level hooks can. For more information on raw input, see Raw Input.
        ''' </remarks>
        Public Delegate Function LowLevelMouseProc(nCode As Integer, wParam As UIntPtr, lParam As IntPtr) As IntPtr

        ''' LRESULT CALLBACK MouseProc(
        '''   _In_ int    nCode,
        '''   _In_ WPARAM wParam,
        '''   _In_ LPARAM lParam
        ''' );
        ''' <summary>
        ''' An application-defined or library-defined callback function used with the SetWindowsHookEx function. The system calls this function whenever an application calls the GetMessage or PeekMessage function and there is a mouse message to be processed.
        ''' The HOOKPROC type defines a pointer to this callback function. MouseProc is a placeholder for the application-defined or library-defined function name.
        ''' </summary>
        ''' <param name="nCode">
        ''' nCode [in]
        ''' Type: int
        ''' A code that the hook procedure uses to determine how to process the message. If nCode is less than zero, the hook procedure must pass the message to the CallNextHookEx function without further processing and should return the value returned by CallNextHookEx. This parameter can be one of the following values.
        ''' <para />
        ''' HC_ACTION = 0
        ''' The wParam and lParam parameters contain information about a mouse message.
        ''' <para />
        ''' HC_NOREMOVE = 3
        ''' The wParam and lParam parameters contain information about a mouse message, and the mouse message has not been removed from the message queue. (An application called the PeekMessage function, specifying the PM_NOREMOVE flag.)
        ''' </param>
        ''' <param name="wParam">
        ''' wParam [in]
        ''' Type: WPARAM
        ''' The identifier of the mouse message.
        ''' </param>
        ''' <param name="lParam">
        ''' lParam [in]
        ''' Type: LPARAM
        ''' A pointer to a MOUSEHOOKSTRUCT structure.
        ''' </param>
        ''' <returns>
        ''' Type: LRESULT
        ''' If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx.
        ''' If nCode is greater than or equal to zero, and the hook procedure did not process the message, it is highly recommended that you call CallNextHookEx and return the value it returns; otherwise, other applications that have installed WH_MOUSE hooks will not receive hook notifications and may behave incorrectly as a result. If the hook procedure processed the message, it may return a nonzero value to prevent the system from passing the message to the target window procedure.
        ''' </returns>
        ''' <remarks>
        ''' An application installs the hook procedure by specifying the WH_MOUSE hook type and a pointer to the hook procedure in a call to the SetWindowsHookEx function.
        ''' The hook procedure must not install a WH_JOURNALPLAYBACK callback function.
        ''' This hook may be called in the context of the thread that installed it. The call is made by sending a message to the thread that installed the hook. Therefore, the thread that installed the hook must have a message loop.
        ''' </remarks>
        Public Delegate Function MouseProc(nCode As Integer, wParam As UIntPtr, lParam As IntPtr) As IntPtr

        ''' The following table describes the layout of this value.
        ''' Bits  Description
        ''' 0     Specifies whether the key is an extended key, such as a function key or a key on the numeric keypad. The value is 1 if the key is an extended key; otherwise, it is 0.
        ''' 1     Specifies whether the event was injected from a process running at lower integrity level. The value is 1 if that is the case; otherwise, it is 0. Note that bit 4 is also set whenever bit 1 is set.
        ''' 2-3   Reserved.
        ''' 4     Specifies whether the event was injected. The value is 1 if that is the case; otherwise, it is 0. Note that bit 1 is not necessarily set when bit 4 is set.
        ''' 5     The context code. The value is 1 if the ALT key is pressed; otherwise, it is 0.
        ''' 6     Reserved.
        ''' 7     The transition state. The value is 0 if the key is pressed and 1 if it is being released.
        Public Enum LowLevelKeyHookFlags As UInteger

            ''' LLKHF_EXTENDED = (KF_EXTENDED >> 8)
            ''' <summary>
            ''' Test the extended-key flag.
            ''' </summary>
            Extended = (Windows.KeyFlags.Extended >> 8)
            ''' LLKHF_LOWER_IL_INJECTED = 0x00000002
            ''' <summary>
            ''' Test the event-injected (from a process running at lower integrity level) flag.
            ''' </summary>
            LowerILInjected = &H2
            ''' LLKHF_INJECTED = 0x00000010
            ''' <summary>
            ''' Test the event-injected (from any process) flag.
            ''' </summary>
            Injected = &H10
            ''' LLKHF_ALTDOWN = (KF_ALTDOWN >> 8)
            ''' <summary>
            ''' Test the context code.
            ''' </summary>
            AltDown = (Windows.KeyFlags.AltDown >> 8)
            ''' LLKHF_UP = (KF_UP >> 8)
            ''' <summary>
            ''' Test the transition-state flag.
            ''' </summary>
            Up = (Windows.KeyFlags.Up >> 8)

        End Enum

        ''' typedef struct tagKBDLLHOOKSTRUCT {
        '''   DWORD     vkCode;
        '''   DWORD     scanCode;
        '''   DWORD     flags;
        '''   DWORD     time;
        '''   ULONG_PTR dwExtraInfo;
        ''' } KBDLLHOOKSTRUCT, *PKBDLLHOOKSTRUCT, *LPKBDLLHOOKSTRUCT;
        ''' <summary>
        ''' Contains information about a low-level keyboard input event.
        ''' </summary>
        Public Structure LowLevelKeyboardHookData

            ''' vkCode
            ''' 
            ''' Type: DWORD
            ''' 
            ''' A virtual-key code. The code must be a value in the range 1 to 254.
            ''' scanCode
            ''' 
            ''' Type: DWORD
            ''' 
            ''' A hardware scan code for the key.
            ''' flags
            ''' 
            '''     Type: DWORD
            ''' 
            '''     The extended-key flag, event-injected flags, context code, and transition-state flag. This member is specified as follows. An application can use the following values to test the keystroke flags. Testing LLKHF_INJECTED (bit 4) will tell you whether the event was injected. If it was, then testing LLKHF_LOWER_IL_INJECTED (bit 1) will tell you whether or not the event was injected from a process running at lower integrity level.
            '''      
            ''' time
            ''' 
            '''     Type: DWORD
            ''' 
            '''     The time stamp for this message, equivalent to what GetMessageTime would return for this message.
            ''' dwExtraInfo
            ''' 
            '''     Type: ULONG_PTR
            ''' 
            '''     Additional information associated with the message.

        End Structure

        ''' typedef struct tagMSLLHOOKSTRUCT {
        '''   POINT     pt;
        '''   DWORD     mouseData;
        '''   DWORD     flags;
        '''   DWORD     time;
        '''   ULONG_PTR dwExtraInfo;
        ''' } MSLLHOOKSTRUCT, *PMSLLHOOKSTRUCT, *LPMSLLHOOKSTRUCT;
        ''' <summary>
        ''' Contains information about a low-level mouse input event.
        ''' </summary>
        Public Structure LowLevelMouseHookData

            ''' pt
            ''' 
            '''     Type: POINT
            ''' 
            '''     The x- and y-coordinates of the cursor, in per-monitor-aware screen coordinates.
            ''' mouseData
            ''' 
            '''     Type: DWORD
            ''' 
            '''     If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta. The low-order word is reserved. A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user. One wheel click is defined as WHEEL_DELTA, which is 120.
            ''' 
            '''     If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, and the low-order word is reserved. This value can be one or more of the following values. Otherwise, mouseData is not used.
            '''     Value	Meaning
            ''' 
            ''' XBUTTON1 = 0x0001
            ''' The first X button was pressed or released.
            ''' 
            ''' XBUTTON2 = 0x0002
            ''' The second X button was pressed or released.
            ''' 
            '''      
            ''' flags
            ''' 
            '''     Type: DWORD
            ''' 
            '''     The event-injected flags. An application can use the following values to test the flags. Testing LLMHF_INJECTED (bit 0) will tell you whether the event was injected. If it was, then testing LLMHF_LOWER_IL_INJECTED (bit 1) will tell you whether or not the event was injected from a process running at lower integrity level.
            '''     Value	Meaning
            ''' 
            ''' LLMHF_INJECTED = 0x00000001
            ''' Test the event-injected (from any process) flag.
            ''' 
            ''' LLMHF_LOWER_IL_INJECTED = 0x00000002
            ''' Test the event-injected (from a process running at lower integrity level) flag.
            ''' 
            '''      
            ''' time
            ''' 
            '''     Type: DWORD
            ''' 
            '''     The time stamp for this message.
            ''' dwExtraInfo
            ''' 
            '''     Type: ULONG_PTR
            ''' 
            '''     Additional information associated with the message.

        End Structure

        ''' typedef struct tagMOUSEHOOKSTRUCT {
        '''   POINT     pt;
        '''   HWND      hwnd;
        '''   UINT      wHitTestCode;
        '''   ULONG_PTR dwExtraInfo;
        ''' } MOUSEHOOKSTRUCT, *PMOUSEHOOKSTRUCT, *LPMOUSEHOOKSTRUCT;
        ''' <summary>
        ''' Contains information about a mouse event passed to a WH_MOUSE hook procedure, MouseProc.
        ''' </summary>
        Public Structure MouseHookData

            ''' pt
            ''' 
            '''     Type: POINT
            ''' 
            '''     The x- and y-coordinates of the cursor, in screen coordinates.
            ''' hwnd
            ''' 
            '''     Type: HWND
            ''' 
            '''     A handle to the window that will receive the mouse message corresponding to the mouse event.
            ''' wHitTestCode
            ''' 
            '''     Type: UINT
            ''' 
            '''     The hit-test value. For a list of hit-test values, see the description of the WM_NCHITTEST message.
            ''' dwExtraInfo
            ''' 
            '''     Type: ULONG_PTR
            ''' 
            '''     Additional information associated with the message.

        End Structure

    End Class

End Namespace
