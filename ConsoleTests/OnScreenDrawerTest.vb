Imports System.Drawing

Public Class OnScreenDrawerTest

    <InteractiveRunnable(True)>
    Public Shared Sub Start()
        Dim Dispatcher = New Dispatcher()
        Dispatcher.SetSynchronizationContext()
        Dispatcher.Invoke(Sub()
                              Dim Drawer = OnScreenDrawer.ForScreen()
                              Dim D = New OnScreenDrawer.Drawing(200, 100, 100)
                              D.Graphics.FillEllipse(Brushes.Red, 0, 0, 100, 100)
                              Drawer.Drawings.Add(D)
                              D.Show()
                          End Sub)
        Dispatcher.Run()
    End Sub

End Class
