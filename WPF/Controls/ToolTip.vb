Namespace Controls

    Public Class ToolTip
        Inherits Windows.Controls.ToolTip

        ' ToDo Make the popup be above the mouse using the CustomPopupPlacementCallback and Placement properties.

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(ToolTip), New FrameworkPropertyMetadata(GetType(ToolTip)))
            TextBlock.KsLanguageProperty.AddOwner(GetType(ToolTip), New FrameworkPropertyMetadata(AddressOf KsLanguage_Changed))
        End Sub

        Private Shared Sub KsLanguage_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, ToolTip)
            Self.Update()
        End Sub

        Private Async Sub Update()
            Dim Text = Me.Text
            If Text Is Nothing Then
                Exit Sub
            End If

            Dim Lang = TextBlock.GetKsLanguage(Me)
            Dim OText = If(Lang?.Translation(Text), Text)

            Me.Visibility = If(OText.Length <> 0, Visibility.Visible, Visibility.Collapsed)
            Me.Content = OText

            ' There is a bug that the content changes, but the UI does not show the change. Let the translation for a language be empty and non-empty for another. Switching to the non-empty one will show an empty tool tip.
            ' ToDo This is a very bad workaround. Correct it.
            Await Task.Delay(1)
            Me.Content = ""
            Await Task.Delay(1)
            Me.Content = OText
        End Sub

#Region "Text Property"
        Public Shared ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register("Text", GetType(String), GetType(ToolTip), New PropertyMetadata(Nothing, AddressOf Text_Changed, AddressOf Text_Coerce))

        Private Shared Function Text_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = DirectCast(D, ToolTip)

            'Dim Value = DirectCast(BaseValue, String)

            Return BaseValue
        End Function

        Private Shared Sub Text_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, ToolTip)

            'Dim OldValue = DirectCast(E.OldValue, String)
            'Dim NewValue = DirectCast(E.NewValue, String)

            Self.Update()
        End Sub

        Public Property Text As String
            Get
                Return DirectCast(Me.GetValue(TextProperty), String)
            End Get
            Set(ByVal value As String)
                Me.SetValue(TextProperty, value)
            End Set
        End Property
#End Region

    End Class

End Namespace
