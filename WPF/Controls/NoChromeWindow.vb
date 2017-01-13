Imports System.Windows.Input
Imports System.Windows.Interop

Namespace Common.Controls

    Public Class NoChromeWindow
        Inherits System.Windows.Window

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(NoChromeWindow), New FrameworkPropertyMetadata(GetType(NoChromeWindow)))
            ResizeModeProperty.OverrideMetadata(GetType(NoChromeWindow), New FrameworkPropertyMetadata(ResizeMode.NoResize, Nothing, Function(D, T) ResizeMode.NoResize))
            WindowStyleProperty.OverrideMetadata(GetType(NoChromeWindow), New FrameworkPropertyMetadata(WindowStyle.None, Nothing, Function(D, T) WindowStyle.None))
        End Sub

        Public Sub New()
            Dim Helper = New WindowInteropHelper(Me)
            Dim Handle = Helper.EnsureHandle()
            Dim HandleSource = HwndSource.FromHwnd(Handle)
            'Dim HandleSource = HwndSource.FromVisual(Me)
            HandleSource.AddHook(AddressOf Me.WindowProcess)
        End Sub

        Private Function WindowProcess(ByVal hwnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr, ByRef handled As Boolean) As IntPtr
            Select Case msg
                Case &H0084 ' NCHitTest
                    Return Me.NCHitTest(wParam, lParam, handled)
            End Select
        End Function

        Private Function NCHitTest(ByVal wParam As IntPtr, ByVal lParam As IntPtr, ByRef Handled As Boolean) As IntPtr
            Dim MousePosition = Input.Mouse.GetPosition(Me)
            Dim InputElement = Me.InputHitTest(MousePosition)

        End Function

    End Class

End Namespace
