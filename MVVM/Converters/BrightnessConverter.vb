Imports System.Globalization
Imports System.Windows.Data
Imports System.Windows.Media

Namespace MVVM.Converters

    Public Class BrightnessConverter
        Inherits DependencyObject
        Implements IValueConverter

        Public Function Convert(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim B = TryCast(Value, SolidColorBrush)
            Dim C As Color
            If B IsNot Nothing Then
                C = B.Color
            Else
                C = CType(Value, Color)
            End If

            Dim P = 0.0
            If Parameter IsNot Nothing Then
                P = CType(Parameter, Double)
            End If

            C = Me.Convert(C, P)

            If TargetType = GetType(Color) Then
                Return C
            End If
            Return New SolidColorBrush(C)
        End Function

        Public Function ConvertBack(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Dim B = TryCast(Value, SolidColorBrush)
            Dim C As Color
            If B IsNot Nothing Then
                C = B.Color
            Else
                C = CType(Value, Color)
            End If

            Dim P = 0.0
            If Parameter IsNot Nothing Then
                P = CType(Parameter, Double)
            End If

            C = Me.Convert(C, -P)

            If TargetType = GetType(Color) Then
                Return C
            End If
            Return New SolidColorBrush(C)
        End Function

        Private Shared Function GetByte(ByVal N As Integer) As Byte
            If N < 0 Then
                N = 0
            End If
            If N > 255 Then
                N = 255
            End If
            Return CByte(N)
        End Function

        Public Function Convert(ByVal C As Color, Optional ByVal Parameter As Double = 0.0) As Color
            Dim P = CInt(Parameter * 255 * Me.Coeff)
            Return Color.FromArgb(C.A, GetByte(C.R + P), GetByte(C.G + P), GetByte(C.B + P))
        End Function

        Private Sub CalculateCoeff()
            Dim Background = Me.Background
            Dim Foreground = Me.Foreground
            Dim BG = (Background.ScR + Background.ScG + Background.ScB) / 3
            Dim FG = (Foreground.ScR + Foreground.ScG + Foreground.ScB) / 3

            Me.Coeff = BG - FG
        End Sub

#Region "Background Property"
        Public Shared ReadOnly BackgroundProperty As DependencyProperty = DependencyProperty.Register("Background", GetType(Color), GetType(BrightnessConverter), New PropertyMetadata(Colors.White, AddressOf Background_Changed))

        Private Shared Sub Background_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, BrightnessConverter)
            Self.CalculateCoeff()
        End Sub

        Public Property Background As Color
            Get
                Return DirectCast(Me.GetValue(BackgroundProperty), Color)
            End Get
            Set(value As Color)
                Me.SetValue(BackgroundProperty, value)
            End Set
        End Property
#End Region

#Region "Foreground Property"
        Public Shared ReadOnly ForegroundProperty As DependencyProperty = DependencyProperty.Register("Foreground", GetType(Color), GetType(BrightnessConverter), New PropertyMetadata(Colors.Black, AddressOf Foreground_Changed))

        Private Shared Sub Foreground_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, BrightnessConverter)
            Self.CalculateCoeff()
        End Sub

        Public Property Foreground As Color
            Get
                Return DirectCast(Me.GetValue(ForegroundProperty), Color)
            End Get
            Set(value As Color)
                Me.SetValue(ForegroundProperty, value)
            End Set
        End Property
#End Region

        Private Coeff As Double = 1

    End Class

End Namespace
