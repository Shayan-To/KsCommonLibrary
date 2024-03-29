using System.Drawing;

using Ks.Common;

namespace Ks.Tests
{
    public static class OnScreenDrawerTest
    {
        [InteractiveRunnable(true)]
        public static void Start()
        {
            using var Dispatcher = new Dispatcher();
            Dispatcher.SetSynchronizationContext();
            Dispatcher.Invoke(() =>
            {
                var Drawer = OnScreenDrawer.ForScreen();
                var D = new OnScreenDrawer.Drawing(200, 100, 100);
                D.Graphics.FillEllipse(Brushes.Red, 0, 0, 100, 100);
                Drawer.Drawings.Add(D);
                D.Show();
            });
            Dispatcher.Run();
        }
    }
}
