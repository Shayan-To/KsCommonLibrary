Imports System.Globalization
Imports System.Windows.Data

Namespace Common.MVVM

    Public Class TranslationConverter
        Implements IValueConverter

        Private Function GetLanguage() As KsLanguage
            Return KsApplication.Current?.Language
        End Function

        Public Function Convert(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim Lang = Me.GetLanguage()
            Dim Text = CType(Value, String)
            Return If(Lang?.Translation(Text), Text)
        End Function

        Public Function ConvertBack(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotSupportedException()
        End Function

    End Class

End Namespace
