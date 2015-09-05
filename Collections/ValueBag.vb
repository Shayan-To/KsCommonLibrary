Public Class ValueBag(Of T)
    Inherits Dictionary(Of String, T)
    Implements IFormattable

    Public Overrides Function ToString() As String
        Return Me.ToString("", Utilities.CurruntFormatProvider)
    End Function

    Public Overloads Function ToString(ByVal format As String, ByVal formatProvider As System.IFormatProvider) As String Implements System.IFormattable.ToString
        Dim R = New Text.StringBuilder("{"c)
        Dim Bl = True
        For Each KV In Me
            If Bl Then
                Bl = False
            Else
                R.Append(", ")
            End If

            R.Append(KV.Key).Append(" : ").Append(String.Format(formatProvider, "{0:" & format & "}", KV.Value))
        Next

        Return R.Append("}"c).ToString()
    End Function

End Class
