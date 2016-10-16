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

    Public Shared Function TryInteger(ByVal Str As String, ByRef Value As Integer) As Boolean
        Return Integer.TryParse(Str, Globalization.NumberStyles.Integer, Globalization.CultureInfo.InvariantCulture, Value)
    End Function

    Public Shared Function TryLong(ByVal Str As String, ByRef Value As Long) As Boolean
        Return Long.TryParse(Str, Globalization.NumberStyles.Integer, Globalization.CultureInfo.InvariantCulture, Value)
    End Function

    Public Shared Function TryShort(ByVal Str As String, ByRef Value As Short) As Boolean
        Return Short.TryParse(Str, Globalization.NumberStyles.Integer, Globalization.CultureInfo.InvariantCulture, Value)
    End Function

    Public Shared Function TryByte(ByVal Str As String, ByRef Value As Byte) As Boolean
        Return Byte.TryParse(Str, Globalization.NumberStyles.Integer, Globalization.CultureInfo.InvariantCulture, Value)
    End Function

    Public Shared Function TryUInteger(ByVal Str As String, ByRef Value As UInteger) As Boolean
        Return UInteger.TryParse(Str, Globalization.NumberStyles.Integer, Globalization.CultureInfo.InvariantCulture, Value)
    End Function

    Public Shared Function TryULong(ByVal Str As String, ByRef Value As ULong) As Boolean
        Return ULong.TryParse(Str, Globalization.NumberStyles.Integer, Globalization.CultureInfo.InvariantCulture, Value)
    End Function

    Public Shared Function TryUShort(ByVal Str As String, ByRef Value As UShort) As Boolean
        Return UShort.TryParse(Str, Globalization.NumberStyles.Integer, Globalization.CultureInfo.InvariantCulture, Value)
    End Function

    Public Shared Function TrySByte(ByVal Str As String, ByRef Value As SByte) As Boolean
        Return SByte.TryParse(Str, Globalization.NumberStyles.Integer, Globalization.CultureInfo.InvariantCulture, Value)
    End Function

    Public Shared Function TryDouble(ByVal Str As String, ByRef Value As Double) As Boolean
        Return Double.TryParse(Str, Globalization.NumberStyles.Float Or Globalization.NumberStyles.AllowThousands, Globalization.CultureInfo.InvariantCulture, Value)
    End Function

    Public Shared Function TrySingle(ByVal Str As String, ByRef Value As Single) As Boolean
        Return Single.TryParse(Str, Globalization.NumberStyles.Float Or Globalization.NumberStyles.AllowThousands, Globalization.CultureInfo.InvariantCulture, Value)
    End Function

    Public Shared Function TryDecimal(ByVal Str As String, ByRef Value As Decimal) As Boolean
        Return Decimal.TryParse(Str, Globalization.NumberStyles.Number, Globalization.CultureInfo.InvariantCulture, Value)
    End Function

    Public Shared Function TryBoolean(ByVal Str As String, ByRef Value As Boolean) As Boolean
        Return Boolean.TryParse(Str, Value)
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
