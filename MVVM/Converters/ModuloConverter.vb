Imports System.Globalization
Imports System.Windows.Data
Imports System.Windows.Media

Namespace MVVM.Converters

    Public Class ModuloConverter
        Implements IValueConverter

        Public Function Convert(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim A = CType(Value, Integer)
            Dim B = CType(Parameter, Integer)

            Return A Mod B
        End Function

        Public Function ConvertBack(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Return Value
        End Function

    End Class

End Namespace
