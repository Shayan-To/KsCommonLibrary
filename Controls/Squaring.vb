Namespace Controls

    Public Class Squaring
        Inherits ContentControl

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(Squaring), New FrameworkPropertyMetadata(GetType(Squaring)))
        End Sub

        Protected Overrides Function MeasureOverride(constraint As Size) As Size
            Dim Size = New Size(constraint.Width, constraint.Width)
            If constraint.Height < constraint.Width Then
                Size = New Size(constraint.Height, constraint.Height)
            End If

            Dim Content = Me.Content
            If Content IsNot Nothing Then
                Content.Measure(Size)
            End If

            If Double.IsInfinity(Size.Width) Then
                If Content IsNot Nothing Then
                    Size = Content.DesiredSize
                    If Size.Width > Size.Height Then
                        Size = New Size(Size.Width, Size.Width)
                    Else
                        Size = New Size(Size.Height, Size.Height)
                    End If
                Else
                    Size = New Size()
                End If
            End If

            Return Size
        End Function

        Protected Overrides Function ArrangeOverride(arrangeBounds As Size) As Size
            Dim Content = Me.Content
            If Content Is Nothing Then
                Return arrangeBounds
            End If

            Dim Rect As Rect
            If arrangeBounds.Height < arrangeBounds.Width Then
                Rect = New Rect((arrangeBounds.Width - arrangeBounds.Height) / 2, 0, arrangeBounds.Height, arrangeBounds.Height)
            Else
                Rect = New Rect(0, (arrangeBounds.Height - arrangeBounds.Width) / 2, arrangeBounds.Width, arrangeBounds.Width)
            End If

            Content.Arrange(Rect)

            Return arrangeBounds
        End Function

    End Class

End Namespace
