Imports System.Globalization
Imports System.Windows.Data

Namespace Common.MVVM.Converters

    Public Class SubstringConverter
        Implements IValueConverter

        Public Function Convert(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim P = DirectCast(Parameter, String).Split(","c).Select(Function(S) Integer.Parse(S.Trim())).ToArray()
            Verify.True(P.Length = 2)
            Return CType(Value, String).Substring(P(0), P(1))
        End Function

        Public Function ConvertBack(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotSupportedException()
        End Function

    End Class

End Namespace
