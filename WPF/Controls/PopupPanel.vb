Namespace Common.Controls

    Public Class PopupPanel
        Inherits Panel

        Protected Overrides Function MeasureOverride(ByVal AvailableSize As Size) As Size
            Dim MaxWidth = 0.0
            Dim MaxHeight = 0.0

            For Each C As UIElement In Me.Children
                C.Measure(AvailableSize)
                Dim Sz As Size = C.DesiredSize

                If MaxHeight < Sz.Height Then
                    MaxHeight = Sz.Height
                End If
                If MaxWidth < Sz.Width Then
                    MaxWidth = Sz.Width
                End If
            Next

            Return New Size(MaxWidth, MaxHeight)
        End Function

        Protected Overrides Function ArrangeOverride(FinalSize As Size) As Size
            For Each C As UIElement In Me.Children
                Dim Popup = TryCast(C, Popup)
                Dim Rect As Rect?

                If Popup IsNot Nothing Then
                    Rect = Popup.ArrangeCallBack?.Invoke(Me, FinalSize, Popup.DesiredSize)
                End If

                If Not Rect.HasValue Then
                    Rect = New Rect(FinalSize)
                End If

                C.Arrange(Rect.Value)
            Next

            Return FinalSize
        End Function

    End Class

End Namespace
