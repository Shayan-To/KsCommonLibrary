Namespace Controls

    Public Class SimplePanel
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

        Protected Overrides Function ArrangeOverride(ByVal FinalSize As Size) As Size
            For Each C As UIElement In Me.Children
                C.Arrange(New Rect(FinalSize))
            Next

            Return FinalSize
        End Function

    End Class

End Namespace
