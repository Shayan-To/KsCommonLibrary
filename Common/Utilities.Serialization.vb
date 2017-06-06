Imports System.Runtime.CompilerServices
Imports System.Text
Imports Media = System.Windows.Media
Imports Reflect = System.Reflection
Imports SIO = System.IO

Namespace Common

    Partial Class Utilities

        Public Class Serialization

            Private Sub New()
                Throw New NotSupportedException()
            End Sub

            Public Shared Function HexToColor(ByVal Hex As String) As Media.Color
                If Hex.StartsWith("#") Then
                    Hex = Hex.Substring(1)
                End If
                If Hex.Length = 8 Then
                    Return Media.Color.FromArgb(Byte.Parse(Hex.Substring(0, 2), Globalization.NumberStyles.HexNumber),
                                                Byte.Parse(Hex.Substring(2, 2), Globalization.NumberStyles.HexNumber),
                                                Byte.Parse(Hex.Substring(4, 2), Globalization.NumberStyles.HexNumber),
                                                Byte.Parse(Hex.Substring(6, 2), Globalization.NumberStyles.HexNumber))
                End If
                If Hex.Length = 6 Then
                    Return Media.Color.FromRgb(Byte.Parse(Hex.Substring(0, 2), Globalization.NumberStyles.HexNumber),
                                               Byte.Parse(Hex.Substring(2, 2), Globalization.NumberStyles.HexNumber),
                                               Byte.Parse(Hex.Substring(4, 2), Globalization.NumberStyles.HexNumber))
                End If

                Throw New ArgumentException("Invalid hex color.")
            End Function

            Public Shared Function ColorToHex(ByVal Color As Media.Color) As String
                Return String.Concat("#",
                                     Color.A.ToString("X2"),
                                     Color.R.ToString("X2"),
                                     Color.G.ToString("X2"),
                                     Color.B.ToString("X2"))
            End Function

            Private Shared Function EscapeChar(ByVal Ch As Char, ByVal EscapeChars As String) As String
                Select Case Ch
                    Case ControlChars.Cr
                        Return "\r"
                    Case ControlChars.Lf
                        Return "\n"
                    Case Else
                        If EscapeChars.Contains(Ch) Then
                            Return "\"c & Ch
                        End If
                        Return Ch
                End Select
            End Function

            Private Shared Function UnescapeChar(ByVal Ch As Char, ByVal EscapeChars As String) As Char
                Select Case Ch
                    Case "r"c
                        Return ControlChars.Cr
                    Case "n"c
                        Return ControlChars.Lf
                    Case Else
                        If EscapeChars.Contains(Ch) Then
                            Return Ch
                        End If
                        Throw New Exception("Invalid escape character.")
                End Select
            End Function

            Public Shared Function ListToString(ByVal List As IEnumerable(Of String)) As String
                Dim Res = New StringBuilder("{")
                Dim Bl = True

                For Each Str As String In List
                    If Bl Then
                        Bl = False
                    Else
                        Res.Append(",")
                    End If

                    For Each Ch In Str
                        Res.Append(EscapeChar(Ch, ",\{}"))
                    Next
                Next

                Return Res.Append("}").ToString()
            End Function

            Public Shared Function ListFromString(ByVal Str As String) As List(Of String)
                Dim Res = New List(Of String)()
                Dim R = New StringBuilder()

                For I As Integer = 0 To Str.Length - 1
                    Dim Ch = Str.Chars(I)

                    If I = 0 Then
                        If Ch <> "{"c Then
                            Throw New Exception("Invalid list string.")
                        End If
                        Continue For
                    End If

                    If Ch = "\"c Then
                        I += 1
                        If I = Str.Length Then
                            Throw New Exception("Invalid list string.")
                        End If

                        R.Append(UnescapeChar(Str.Chars(I), ",\{}"))
                        Continue For
                    End If

                    If Ch = ","c Or Ch = "}"c Then
                        Res.Add(R.ToString())
                        R.Clear()

                        If Ch = "}"c And I <> Str.Length - 1 Then
                            Throw New Exception("Invalid list string.")
                        End If

                        Continue For
                    End If

                    R.Append(Ch)
                Next

                Return Res
            End Function

            Public Shared Function ListToStringMultiline(ByVal List As IEnumerable(Of String)) As String
                Dim Res = New StringBuilder()

                For Each Str As String In List
                    For Each Ch In Str
                        Res.Append(EscapeChar(Ch, "\"))
                    Next

                    Res.AppendLine()
                Next

                Return Res.ToString()
            End Function

            Public Shared Function ListFromStringMultiline(ByVal Str As String) As List(Of String)
                Dim Res = New List(Of String)()
                Dim R = New StringBuilder()

                For I As Integer = 0 To Str.Length - 1
                    Dim Ch = Str.Chars(I)

                    If Ch = "\"c Then
                        I += 1
                        If I = Str.Length Then
                            Verify.Fail("Invalid list string.")
                        End If

                        R.Append(UnescapeChar(Str.Chars(I), "\"))
                        Continue For
                    End If

                    If Ch = ControlChars.Cr Or Ch = ControlChars.Lf Then
                        If (Ch = ControlChars.Cr And I + 1 < Str.Length) AndAlso Str.Chars(I + 1) = ControlChars.Lf Then
                            I += 1
                        End If

                        Res.Add(R.ToString())
                        R.Clear()

                        If I + 1 = Str.Length Then
                            Return Res
                        End If

                        Continue For
                    End If

                    R.Append(Ch)
                Next

                If Str.Length = 0 Then
                    Return Res
                End If

                Verify.Fail("Invalid list string.")
                Return Nothing
            End Function

            Public Shared Function DicToString(ByVal Dic As IDictionary(Of String, String)) As String
                Dim Res = New StringBuilder("{")
                Dim Bl = True

                For Each KV In Dic
                    If Bl Then
                        Bl = False
                    Else
                        Res.Append(",")
                    End If

                    For Each Ch In KV.Key
                        Res.Append(EscapeChar(Ch, ",\{}:"))
                    Next
                    Res.Append(":"c)
                    For Each Ch In KV.Value
                        Res.Append(EscapeChar(Ch, ",\{}"))
                    Next
                Next

                Return Res.Append("}").ToString()
            End Function

            Public Shared Function DicFromString(ByVal Str As String) As OrderedDictionary(Of String, String)
                Dim Res = New OrderedDictionary(Of String, String)()
                Dim R = New StringBuilder()
                Dim Key As String = Nothing

                For I As Integer = 0 To Str.Length - 1
                    Dim Ch = Str.Chars(I)

                    If I = 0 Then
                        If Ch <> "{"c Then
                            Throw New Exception("Invalid dictionary string.")
                        End If
                        Continue For
                    End If

                    If Ch = "\"c Then
                        I += 1
                        If I = Str.Length Then
                            Throw New Exception("Invalid dictionary string.")
                        End If

                        R.Append(UnescapeChar(Str.Chars(I), ",\{}:"))
                        Continue For
                    End If

                    If Ch = ":"c Then
                        Key = R.ToString()
                        R.Clear()

                        Continue For
                    End If

                    If Ch = ","c Or Ch = "}"c Then
                        If Key Is Nothing Then
                            Throw New Exception("Invalid dictionary string.")
                        End If
                        Res.Add(Key, R.ToString())
                        R.Clear()
                        Key = Nothing

                        If Ch = "}"c And I <> Str.Length - 1 Then
                            Throw New Exception("Invalid dictionary string.")
                        End If

                        Continue For
                    End If

                    R.Append(Ch)
                Next

                Return Res
            End Function

            Public Shared Function DicToStringMultiline(ByVal Dic As IDictionary(Of String, String)) As String
                Dim Res = New StringBuilder()

                For Each KV In Dic
                    For Each Ch In KV.Key
                        Res.Append(EscapeChar(Ch, "\:"))
                    Next
                    Res.Append(":"c)
                    For Each Ch In KV.Value
                        Res.Append(EscapeChar(Ch, "\"))
                    Next

                    Res.AppendLine()
                Next

                Return Res.ToString()
            End Function

            Public Shared Function DicFromStringMultiline(ByVal Str As String) As OrderedDictionary(Of String, String)
                Dim Res = New OrderedDictionary(Of String, String)()
                Dim R = New StringBuilder()
                Dim Key As String = Nothing

                For I As Integer = 0 To Str.Length - 1
                    Dim Ch = Str.Chars(I)

                    If Ch = "\"c Then
                        I += 1
                        Verify.False(I = Str.Length, "Invalid dictionary string.")

                        R.Append(UnescapeChar(Str.Chars(I), "\:"))
                        Continue For
                    End If

                    If Ch = ":"c And Key Is Nothing Then
                        Key = R.ToString()
                        R.Clear()

                        Continue For
                    End If

                    If Ch = ControlChars.Cr Or Ch = ControlChars.Lf Then
                        If (Ch = ControlChars.Cr And I + 1 < Str.Length) AndAlso Str.Chars(I + 1) = ControlChars.Lf Then
                            I += 1
                        End If

                        Verify.False(Key Is Nothing, "Invalid dictionary string.")
                        Res.Add(Key, R.ToString())
                        R.Clear()
                        Key = Nothing

                        If I + 1 = Str.Length Then
                            Return Res
                        End If

                        Continue For
                    End If

                    R.Append(Ch)
                Next

                Verify.Fail("Invalid dictionary string.")
                Return Nothing
            End Function

        End Class

    End Class

End Namespace
