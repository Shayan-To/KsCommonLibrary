Imports System.Globalization
Imports System.Windows.Data

Namespace MVVM.Converters

    Public Class ThicknessAverageConverter
        Implements IValueConverter

        Public Function Convert(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim Th = CType(Value, Thickness)
            Return (Th.Left + Th.Top + Th.Right + Th.Bottom) / 4
        End Function

        Public Function ConvertBack(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Dim V = CType(Value, Double)
            Return New Thickness(V)
        End Function

    End Class

End Namespace
