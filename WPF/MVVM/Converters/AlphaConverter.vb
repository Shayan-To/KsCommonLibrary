Imports System.Globalization
Imports System.Windows.Data
Imports System.Windows.Media

Namespace Common.MVVM.Converters

    Public Class AlphaConverter
        Implements IValueConverter

        Public Function Convert(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim B = TryCast(Value, SolidColorBrush)
            Dim C As Color
            If B IsNot Nothing Then
                C = B.Color
            Else
                C = CType(Value, Color)
            End If
            Dim P = CByte(255)
            If Parameter IsNot Nothing Then
                P = CType(Parameter, Byte)
            End If
            C = Color.FromArgb(P, C.R, C.G, C.B)
            If TargetType = GetType(Color) Then
                Return C
            End If
            Return New SolidColorBrush(C)
        End Function

        Public Function ConvertBack(ByVal Value As Object, ByVal TargetType As Type, ByVal Parameter As Object, ByVal Culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Dim B = TryCast(Value, SolidColorBrush)
            Dim C As Color
            If B IsNot Nothing Then
                C = B.Color
            Else
                C = CType(Value, Color)
            End If
            Dim P = CByte(255)
            If Parameter IsNot Nothing Then
                P = CType(Parameter, Byte)
            End If
            C = Color.FromArgb(P, C.R, C.G, C.B)
            If TargetType = GetType(Color) Then
                Return C
            End If
            Return New SolidColorBrush(C)
        End Function

    End Class

End Namespace
