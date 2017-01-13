Namespace Common.NonUI

    Public MustInherit Class NonUIElement
        Inherits FrameworkElement

        Private Shared ReadOnly Collapsed As Object = Visibility.Collapsed

        Shared Sub New()
            VisibilityProperty.OverrideMetadata(GetType(NonUIElement), New FrameworkPropertyMetadata(Collapsed, Nothing, Function(D, B) Collapsed))
        End Sub

        Protected Overrides Function MeasureOverride(availableSize As Size) As Size
            Return New Size()
        End Function

    End Class

End Namespace
