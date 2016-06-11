Imports System.Globalization
Imports System.Windows.Data

Namespace MVVM.Converters

    Public Class ScaleInverseConverter
        Implements IValueConverter

        Public Function Convert(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.Convert
            Return CType(Parameter, Double) / CType(Value, Double)
        End Function

        Public Function ConvertBack(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Return CType(Parameter, Double) / CType(Value, Double)
        End Function
    End Class

End Namespace
