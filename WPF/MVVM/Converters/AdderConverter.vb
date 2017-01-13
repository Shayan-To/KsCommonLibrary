Imports System.Globalization
Imports System.Windows.Data

Namespace Common.MVVM.Converters

    Public Class AdderConverter
        Implements IValueConverter

        Public Function Convert(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.Convert
            Return CType(Value, Double) + CType(Parameter, Double)
        End Function

        Public Function ConvertBack(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Return CType(Value, Double) - CType(Parameter, Double)
        End Function
    End Class

End Namespace
