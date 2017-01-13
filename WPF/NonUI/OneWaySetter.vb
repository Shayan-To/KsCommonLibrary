Namespace Common.NonUI

    Public Class OneWaySetter
        Inherits NonUIElement

#Region "Input Property"
        Public Shared ReadOnly InputProperty As DependencyProperty = DependencyProperty.Register("Input", GetType(Object), GetType(OneWaySetter), New PropertyMetadata(Nothing, AddressOf Input_Changed, AddressOf Input_Coerce))

        Private Shared Function Input_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = DirectCast(D, OneWaySetter)

            'Dim Value = DirectCast(BaseValue, Object)

            Return BaseValue
        End Function

        Private Shared Sub Input_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, OneWaySetter)

            'Dim OldValue = DirectCast(E.OldValue, Object)
            Dim NewValue = DirectCast(E.NewValue, Object)

            Self.Output = NewValue
        End Sub

        Public Property Input As Object
            Get
                Return DirectCast(Me.GetValue(InputProperty), Object)
            End Get
            Set(ByVal value As Object)
                Me.SetValue(InputProperty, value)
            End Set
        End Property
#End Region

#Region "Output Property"
        Public Shared ReadOnly OutputProperty As DependencyProperty = DependencyProperty.Register("Output", GetType(Object), GetType(OneWaySetter), New PropertyMetadata(Nothing, AddressOf Output_Changed, AddressOf Output_Coerce))

        Private Shared Function Output_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = DirectCast(D, OneWaySetter)

            'Dim Value = DirectCast(BaseValue, Object)

            Return BaseValue
        End Function

        Private Shared Async Sub Output_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, OneWaySetter)

            'Dim OldValue = DirectCast(E.OldValue, Object)
            'Dim NewValue = DirectCast(E.NewValue, Object)

            Await Task.Delay(1)

            Self.Output = Self.Input
        End Sub

        Public Property Output As Object
            Get
                Return DirectCast(Me.GetValue(OutputProperty), Object)
            End Get
            Set(ByVal value As Object)
                Me.SetValue(OutputProperty, value)
            End Set
        End Property
#End Region

    End Class

End Namespace
