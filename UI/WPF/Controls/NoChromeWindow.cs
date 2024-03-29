using System;
using System.Windows;
using System.Windows.Interop;

namespace Ks.Common.Controls
{
    public class NoChromeWindow : System.Windows.Window
    {
        static NoChromeWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NoChromeWindow), new FrameworkPropertyMetadata(typeof(NoChromeWindow)));
            ResizeModeProperty.OverrideMetadata(typeof(NoChromeWindow), new FrameworkPropertyMetadata(ResizeMode.NoResize, null, (D, T) => ResizeMode.NoResize));
            WindowStyleProperty.OverrideMetadata(typeof(NoChromeWindow), new FrameworkPropertyMetadata(WindowStyle.None, null, (D, T) => WindowStyle.None));
        }

        public NoChromeWindow()
        {
            var Helper = new WindowInteropHelper(this);
            var Handle = Helper.EnsureHandle();
            var HandleSource = HwndSource.FromHwnd(Handle);
            // var HandleSource = HwndSource.FromVisual(this);
            HandleSource.AddHook(this.WindowProcess);
        }

        private IntPtr WindowProcess(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            return msg switch
            {
                0x0084 /* NCHitTest */ => this.NCHitTest(wParam, lParam, ref handled),
                _ => default,
            };
        }

        private IntPtr NCHitTest(IntPtr wParam, IntPtr lParam, ref bool Handled)
        {
            var MousePosition = System.Windows.Input.Mouse.GetPosition(this);
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var InputElement = this.InputHitTest(MousePosition);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            return default;
        }
    }
}
