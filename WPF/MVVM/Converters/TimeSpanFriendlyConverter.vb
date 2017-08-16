Imports System.Globalization
Imports System.Windows.Data

Namespace Common.MVVM.Converters

    Public Class TimeSpanFriendlyConverter
        Implements IValueConverter

        Public Function Convert(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.Convert
            Return Utilities.Representation.GetFriendlyTimeSpan(CType(Value, TimeSpan), TimeSpan.FromMinutes(CType(Parameter, Double)))
        End Function

        Public Function ConvertBack(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            ' ToDo Implement the reverse conversion.
            Throw New NotSupportedException()
        End Function

    End Class

End Namespace
