Namespace Controls

    Public Class ListBox
        Inherits Windows.Controls.ListBox

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(ListBox), New FrameworkPropertyMetadata(GetType(ListBox)))
            SelectedItemProperty.OverrideMetadata(GetType(ListBox), New FrameworkPropertyMetadata(Nothing, AddressOf SelectedItem_Coerce))
            ItemsSourceProperty.OverrideMetadata(GetType(ListBox), New FrameworkPropertyMetadata(AddressOf ItemsSource_Changed))
        End Sub

        Private Shared Sub ItemsSource_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, ListBox)

            'Dim NewValue = DirectCast(E.NewValue, IEnumerable)

            Self.CoerceValue(SelectedItemProperty)
        End Sub

        Private Shared Function SelectedItem_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            Dim Self = DirectCast(D, ListBox)

            Dim Value = DirectCast(BaseValue, Object)

            If Self.ItemsSource Is Nothing Then
                Return BaseValue
            End If

            If Value Is Nothing OrElse Not Self.ItemsSource.Cast(Of Object)().Contains(Value) Then
                Return Nothing
            End If

            Return BaseValue
        End Function

    End Class

End Namespace
