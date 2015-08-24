Imports System.Globalization
Imports System.Windows.Data

Namespace MVVM.Converters

    Public Class NumberPaddingConverter
        Implements IValueConverter

        Public Function Convert(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.Convert
            Return String.Format(Culture, "{0}", Value).PadLeft(CType(Parameter, Integer), "0"c)
        End Function

        Public Function ConvertBack(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Dim S = String.Format(Culture, "{0}", Value)
            For I As Integer = 0 To S.Length - 1
                If S.Chars(I) <> "0"c Then
                    Return S.Substring(I)
                End If
            Next
            Return "0"
        End Function
    End Class

End Namespace
