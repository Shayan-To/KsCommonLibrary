Imports System.Runtime.CompilerServices

Public NotInheritable Class ParseInv

    Private Sub New()
        Throw New NotSupportedException()
    End Sub

    Public Shared Function [Integer](ByVal Str As String) As Integer
        Return Integer.Parse(Str, Globalization.CultureInfo.InvariantCulture)
    End Function

    Public Shared Function [Long](ByVal Str As String) As Long
        Return Long.Parse(Str, Globalization.CultureInfo.InvariantCulture)
    End Function

    Public Shared Function [Short](ByVal Str As String) As Short
        Return Short.Parse(Str, Globalization.CultureInfo.InvariantCulture)
    End Function

    Public Shared Function [Byte](ByVal Str As String) As Byte
        Return Byte.Parse(Str, Globalization.CultureInfo.InvariantCulture)
    End Function

    Public Shared Function [UInteger](ByVal Str As String) As UInteger
        Return UInteger.Parse(Str, Globalization.CultureInfo.InvariantCulture)
    End Function

    Public Shared Function [ULong](ByVal Str As String) As ULong
        Return ULong.Parse(Str, Globalization.CultureInfo.InvariantCulture)
    End Function

    Public Shared Function [UShort](ByVal Str As String) As UShort
        Return UShort.Parse(Str, Globalization.CultureInfo.InvariantCulture)
    End Function

    Public Shared Function [SByte](ByVal Str As String) As SByte
        Return SByte.Parse(Str, Globalization.CultureInfo.InvariantCulture)
    End Function

    Public Shared Function [Double](ByVal Str As String) As Double
        Return Double.Parse(Str, Globalization.CultureInfo.InvariantCulture)
    End Function

    Public Shared Function [Single](ByVal Str As String) As Single
        Return Single.Parse(Str, Globalization.CultureInfo.InvariantCulture)
    End Function

    Public Shared Function [Decimal](ByVal Str As String) As Decimal
        Return Decimal.Parse(Str, Globalization.CultureInfo.InvariantCulture)
    End Function

    Public Shared Function [Boolean](ByVal Str As String) As Boolean
        Return Boolean.Parse(Str)
    End Function

End Class

Public Module ConversionExtensions

    <Extension()>
    Public Function ToStringInv(ByVal Self As Integer) As String
        Return Self.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Public Function ToStringInv(ByVal Self As Long) As String
        Return Self.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Public Function ToStringInv(ByVal Self As Short) As String
        Return Self.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Public Function ToStringInv(ByVal Self As Byte) As String
        Return Self.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Public Function ToStringInv(ByVal Self As UInteger) As String
        Return Self.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Public Function ToStringInv(ByVal Self As ULong) As String
        Return Self.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Public Function ToStringInv(ByVal Self As UShort) As String
        Return Self.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Public Function ToStringInv(ByVal Self As SByte) As String
        Return Self.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Public Function ToStringInv(ByVal Self As Double) As String
        Return Self.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Public Function ToStringInv(ByVal Self As Single) As String
        Return Self.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Public Function ToStringInv(ByVal Self As Decimal) As String
        Return Self.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Public Function ToStringInv(ByVal Self As Boolean) As String
        Return Self.ToString()
    End Function

End Module
