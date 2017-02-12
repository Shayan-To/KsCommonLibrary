﻿Imports System.Globalization
Imports System.Windows.Data

Namespace Common.MVVM.Converters

    Public Class BooleanToVisibilityConverter
        Implements IValueConverter

        Public Function Convert(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.Convert
            If CType(Parameter, String) = "~" Xor CType(Value, Boolean) Then
                Return Visibility.Visible
            Else
                Return Visibility.Collapsed
            End If
        End Function

        Public Function ConvertBack(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Return (CType(Parameter, String) = "~") Xor CType(Value, Visibility) = Visibility.Visible
        End Function

    End Class

End Namespace