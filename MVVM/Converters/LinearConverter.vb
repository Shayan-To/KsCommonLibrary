Imports System.Globalization
Imports System.Windows.Data

Namespace MVVM.Converters

    Public Class LinearConverter
        Implements IValueConverter

        Public Function Convert(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.Convert
            Return Me.A * CType(Value, Double) + Me.B
        End Function

        Public Function ConvertBack(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Return (CType(Value, Double) - Me.B) / Me.A
        End Function

        Public Property A As Double
        Public Property B As Double

    End Class

End Namespace
