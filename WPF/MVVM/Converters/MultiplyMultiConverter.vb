Imports System.Globalization
Imports System.Windows.Data

Namespace Common.MVVM.Converters

    Public Class MultiplyMultiConverter
        Implements IMultiValueConverter

        Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
            Dim R = If(parameter Is Nothing, 1, CType(parameter, Double))
            For I = 0 To values.Length - 1
                R *= CType(values(I), Double)
            Next
            Return R
        End Function

        Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
            Throw New NotSupportedException()
        End Function

    End Class

End Namespace
