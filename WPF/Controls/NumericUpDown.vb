'Imports System.Collections.Specialized

Imports Ks.Common.MVVM

Namespace Common.Controls

    Public Class NumericUpDown
        Inherits Control

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(NumericUpDown), New FrameworkPropertyMetadata(GetType(NumericUpDown)))
        End Sub

#Region "Increment Command"
        Private _IncrementCommand As DelegateCommand = New DelegateCommand(AddressOf Me.OnIncrementCommand)

        Private Sub OnIncrementCommand()
            Me.Value += Me.Step
        End Sub

        Public ReadOnly Property IncrementCommand As DelegateCommand
            Get
                Return Me._IncrementCommand
            End Get
        End Property
#End Region

#Region "Decrement Command"
        Private _DecrementCommand As DelegateCommand = New DelegateCommand(AddressOf Me.OnDecrementCommand)

        Private Sub OnDecrementCommand()
            Me.Value -= Me.Step
        End Sub

        Public ReadOnly Property DecrementCommand As DelegateCommand
            Get
                Return Me._DecrementCommand
            End Get
        End Property
#End Region

#Region "Step Property"
        Public Shared ReadOnly StepProperty As DependencyProperty = DependencyProperty.Register("Step", GetType(Double), GetType(NumericUpDown), New PropertyMetadata(1.0, AddressOf Step_Changed, AddressOf Step_Coerce))

        Private Shared Function Step_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            Dim Self = DirectCast(D, NumericUpDown)

            Dim Value = DirectCast(BaseValue, Double)

            Return BaseValue
        End Function

        Private Shared Sub Step_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, NumericUpDown)

            Dim OldValue = DirectCast(E.OldValue, Double)
            Dim NewValue = DirectCast(E.NewValue, Double)
        End Sub

        Public Property [Step] As Double
            Get
                Return DirectCast(Me.GetValue(StepProperty), Double)
            End Get
            Set(ByVal value As Double)
                Me.SetValue(StepProperty, value)
            End Set
        End Property
#End Region

#Region "Value Property"
        Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Double), GetType(NumericUpDown), New PropertyMetadata(0.0, AddressOf Value_Changed, AddressOf Value_Coerce))

        Private Shared Function Value_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            Dim Self = DirectCast(D, NumericUpDown)

            Dim Value = DirectCast(BaseValue, Double)

            Return BaseValue
        End Function

        Private Shared Sub Value_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, NumericUpDown)

            Dim OldValue = DirectCast(E.OldValue, Double)
            Dim NewValue = DirectCast(E.NewValue, Double)
        End Sub

        Public Property Value As Double
            Get
                Return DirectCast(Me.GetValue(ValueProperty), Double)
            End Get
            Set(ByVal value As Double)
                Me.SetValue(ValueProperty, value)
            End Set
        End Property
#End Region

        Private IsDirty As Boolean

    End Class

End Namespace
