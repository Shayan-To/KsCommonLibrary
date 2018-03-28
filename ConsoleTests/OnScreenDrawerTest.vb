Imports System.Drawing

Public Class OnScreenDrawerTest

    <InteractiveRunnable(True)>
    Public Shared Sub Start()
        Dim Dispatcher = New Dispatcher()
        Dispatcher.SetSynchronizationContext()
        Dispatcher.Invoke(Sub()
                              Dim Drawer = OnScreenDrawer.ForScreen()
                              Dim D As OnScreenDrawer.Drawing
                              Dim P As OnScreenDrawer.DrawingPart

                              Drawer.StartDrawing()
                              D = New OnScreenDrawer.Drawing()
                              D.Interval = 200
                              P = New OnScreenDrawer.DrawingPart(100, 100)
                              P.Graphics.FillEllipse(Brushes.Red, 0, 0, 100, 100)
                              D.Parts.Add(P)
                              Drawer.Drawings.Add(D)
                          End Sub)
        Dispatcher.Run()
    End Sub

End Class
