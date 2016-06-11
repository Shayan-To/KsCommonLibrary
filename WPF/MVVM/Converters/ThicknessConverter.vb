Imports System.Globalization
Imports System.Windows.Data

Namespace MVVM.Converters

    Public Class ThicknessConverter
        Implements IValueConverter

        Public Function Convert(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim V = CType(Value, Double)
            Dim P = 1.0
            If Parameter IsNot Nothing Then
                P = CType(Parameter, Double)
            End If
            Return New Thickness(P * V * Me.Coefficients.Left,
                                 P * V * Me.Coefficients.Top,
                                 P * V * Me.Coefficients.Right,
                                 P * V * Me.Coefficients.Bottom)
        End Function

        Public Function ConvertBack(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Dim Th = CType(Value, Thickness)
            Dim V As Double?

            ' ToDo Correct the double equality checks. (It should somehow just check the significant digits, and not the order of magnitude...)

            If Me.Coefficients.Left <> 0 Then
                V = Th.Left / Me.Coefficients.Left
            End If
            If Me.Coefficients.Top <> 0 Then
                If V.HasValue And V.Value <> Th.Top / Me.Coefficients.Top Then
                    Return 0
                End If
                V = Th.Top / Me.Coefficients.Top
            End If
            If Me.Coefficients.Right <> 0 Then
                If V.HasValue And V.Value <> Th.Right / Me.Coefficients.Right Then
                    Return 0
                End If
                V = Th.Right / Me.Coefficients.Right
            End If
            If Me.Coefficients.Bottom <> 0 Then
                If V.HasValue And V.Value <> Th.Bottom / Me.Coefficients.Bottom Then
                    Return 0
                End If
                V = Th.Bottom / Me.Coefficients.Bottom
            End If
            If Not V.HasValue Then
                Return 0
            End If
            Dim P = 1.0
            If Parameter IsNot Nothing Then
                P = CType(Parameter, Double)
            End If
            Return V.Value * P
        End Function

        Public Property Coefficients As Thickness

    End Class

End Namespace
