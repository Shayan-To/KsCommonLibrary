Imports System.Runtime.CompilerServices
Imports System.Text
Imports Media = System.Windows.Media
Imports Reflect = System.Reflection
Imports SIO = System.IO

Namespace Common

    Partial Class Utilities

        Public Class Text

            Private Sub New()
                Throw New NotSupportedException()
            End Sub

#Region "Escape Support Logic"
            Private Shared ReadOnly EscapeDic As Dictionary(Of Char, Char) =
                (Function() New Dictionary(Of Char, Char) From {
                         {"0"c, Strings.ChrW(&H0)},
                         {""""c, Strings.ChrW(&H27)},
                         {"'"c, Strings.ChrW(&H22)},
                         {"?"c, Strings.ChrW(&H3F)},
                         {"\"c, Strings.ChrW(&H5C)},
                         {"a"c, Strings.ChrW(&H7)},
                         {"b"c, Strings.ChrW(&H8)},
                         {"f"c, Strings.ChrW(&HC)},
                         {"n"c, Strings.ChrW(&HA)},
                         {"r"c, Strings.ChrW(&HD)},
                         {"t"c, Strings.ChrW(&H9)},
                         {"v"c, Strings.ChrW(&HB)}
                     }).Invoke()

            Public Shared Function IsBinaryDigit(ByVal C As Char) As Boolean
                Return C = "0"c OrElse C = "1"c
            End Function

            Public Shared Function IsDecimalDigit(ByVal C As Char) As Boolean
                Return "0"c <= C AndAlso C <= "9"c
            End Function

            Public Shared Function IsOctalDigit(ByVal C As Char) As Boolean
                Return "0"c <= C AndAlso C <= "7"c
            End Function

            Public Shared Function IsHexadecimalDigit(ByVal C As Char) As Boolean
                Return ("0"c <= C AndAlso C <= "9"c) OrElse ("A"c <= C AndAlso C <= "F"c) OrElse ("a"c <= C AndAlso C <= "f"c)
            End Function

            Public Shared Function CEscapeC(ByVal Input As String, Optional ByVal DoesThrow As Boolean = True) As Char
                Dim T1, T2 As Char

                If Input.Length = 0 Then
                    Throw New ArgumentException()
                End If

                T2 = Input.Chars(0)

                If T2 <> "\" Then
                    If DoesThrow AndAlso Input.Length <> 1 Then
                        Throw New ArgumentException("Invalid escaped character.")
                    End If
                    Return T2
                End If

                T1 = Input.Chars(1)

                If EscapeDic.TryGetValue(T1, T2) Then
                    If DoesThrow AndAlso Input.Length <> 2 Then
                    End If
                    Return T2
                End If

                If T1 = "x"c Then
                    If Input.Length <> 4 Then
                        If DoesThrow Then
                            Throw New ArgumentException("Invalid escaped string.")
                        Else
                            Return T1
                        End If
                    End If
                    If Not (IsHexadecimalDigit(Input.Chars(2)) AndAlso
                            IsHexadecimalDigit(Input.Chars(3))) Then
                        If DoesThrow Then
                            Throw New ArgumentException("Invalid escaped string.")
                        Else
                            Return T1
                        End If
                    End If

                    Return Strings.ChrW(Convert.ToInt32(Input.Substring(2, 2), 16))
                End If

                If T1 = "u"c Then
                    If Input.Length <> 6 Then
                        If DoesThrow Then
                            Throw New ArgumentException("Invalid escaped string.")
                        Else
                            Return T1
                        End If
                    End If
                    If Not (IsHexadecimalDigit(Input.Chars(2)) AndAlso
                            IsHexadecimalDigit(Input.Chars(3)) AndAlso
                            IsHexadecimalDigit(Input.Chars(4)) AndAlso
                            IsHexadecimalDigit(Input.Chars(5))) Then
                        If DoesThrow Then
                            Throw New ArgumentException("Invalid escaped string.")
                        Else
                            Return T1
                        End If
                    End If

                    Return Strings.ChrW(Convert.ToInt32(Input.Substring(2, 4), 16))
                End If

                If T1 = "U"c Then
                    If Input.Length <> 10 Then
                        If DoesThrow Then
                            Throw New ArgumentException("Invalid escaped string.")
                        Else
                            Return T1
                        End If
                    End If
                    If Not (IsHexadecimalDigit(Input.Chars(2)) AndAlso
                            IsHexadecimalDigit(Input.Chars(3)) AndAlso
                            IsHexadecimalDigit(Input.Chars(4)) AndAlso
                            IsHexadecimalDigit(Input.Chars(5)) AndAlso
                            IsHexadecimalDigit(Input.Chars(6)) AndAlso
                            IsHexadecimalDigit(Input.Chars(7)) AndAlso
                            IsHexadecimalDigit(Input.Chars(8)) AndAlso
                            IsHexadecimalDigit(Input.Chars(9))) Then
                        If DoesThrow Then
                            Throw New ArgumentException("Invalid escaped string.")
                        Else
                            Return T1
                        End If
                    End If

                    Return Strings.ChrW(Convert.ToInt32(Input.Substring(2, 8), 16))
                End If

                If IsOctalDigit(T1) Then
                    If Input.Length <> 4 Then
                        If DoesThrow Then
                            Throw New ArgumentException("Invalid escaped string.")
                        Else
                            Return T1
                        End If
                    End If
                    If Not (IsOctalDigit(Input.Chars(2)) AndAlso
                            IsOctalDigit(Input.Chars(3))) Then
                        If DoesThrow Then
                            Throw New ArgumentException("Invalid escaped string.")
                        Else
                            Return T1
                        End If
                    End If

                    Return Strings.ChrW(Convert.ToInt32(Input.Substring(1, 3), 8))
                End If

                If DoesThrow Then
                    Throw New ArgumentException("Invalid escaped string.")
                Else
                    Return T1
                End If
            End Function

            Public Shared Function CEscape(ByVal Input As String, Optional ByVal DoesThrow As Boolean = True) As String
                Dim T1, T2 As Char

                Dim Res = New StringBuilder()

                For I As Integer = 0 To Input.Length - 1
                    T2 = Input.Chars(I)

                    If T2 = "\"c Then
                        I += 1

                        If I = Input.Length Then
                            If DoesThrow Then
                                Throw New ArgumentException("Invalid escaped string.")
                            Else
                                Exit For
                            End If
                        End If

                        T1 = Input.Chars(I)

                        If EscapeDic.TryGetValue(T1, T2) Then
                            Res.Append(T2)
                            Continue For
                        End If

                        If T1 = "x"c Then
                            If I + 2 >= Input.Length Then
                                If DoesThrow Then
                                    Throw New ArgumentException("Invalid escaped string.")
                                Else
                                    Res.Append(T1)
                                    Continue For
                                End If
                            End If
                            If Not (IsHexadecimalDigit(Input.Chars(I + 1)) AndAlso
                                    IsHexadecimalDigit(Input.Chars(I + 2))) Then
                                If DoesThrow Then
                                    Throw New ArgumentException("Invalid escaped string.")
                                Else
                                    Res.Append(T1)
                                    Continue For
                                End If
                            End If

                            Res.Append(Strings.ChrW(Convert.ToInt32(Input.Substring(I + 1, 2), 16)))
                            I += 2
                            Continue For
                        End If

                        If T1 = "u"c Then
                            If I + 4 >= Input.Length Then
                                If DoesThrow Then
                                    Throw New ArgumentException("Invalid escaped string.")
                                Else
                                    Res.Append(T1)
                                    Continue For
                                End If
                            End If
                            If Not (IsHexadecimalDigit(Input.Chars(I + 1)) AndAlso
                                    IsHexadecimalDigit(Input.Chars(I + 2)) AndAlso
                                    IsHexadecimalDigit(Input.Chars(I + 3)) AndAlso
                                    IsHexadecimalDigit(Input.Chars(I + 4))) Then
                                If DoesThrow Then
                                    Throw New ArgumentException("Invalid escaped string.")
                                Else
                                    Res.Append(T1)
                                    Continue For
                                End If
                            End If

                            Res.Append(Strings.ChrW(Convert.ToInt32(Input.Substring(I + 1, 4), 16)))
                            I += 4
                            Continue For
                        End If

                        If T1 = "U"c Then
                            If I + 8 >= Input.Length Then
                                If DoesThrow Then
                                    Throw New ArgumentException("Invalid escaped string.")
                                Else
                                    Res.Append(T1)
                                    Continue For
                                End If
                            End If
                            If Not (IsHexadecimalDigit(Input.Chars(I + 1)) AndAlso
                                    IsHexadecimalDigit(Input.Chars(I + 2)) AndAlso
                                    IsHexadecimalDigit(Input.Chars(I + 3)) AndAlso
                                    IsHexadecimalDigit(Input.Chars(I + 4)) AndAlso
                                    IsHexadecimalDigit(Input.Chars(I + 5)) AndAlso
                                    IsHexadecimalDigit(Input.Chars(I + 6)) AndAlso
                                    IsHexadecimalDigit(Input.Chars(I + 7)) AndAlso
                                    IsHexadecimalDigit(Input.Chars(I + 8))) Then
                                If DoesThrow Then
                                    Throw New ArgumentException("Invalid escaped string.")
                                Else
                                    Res.Append(T1)
                                    Continue For
                                End If
                            End If

                            Res.Append(Strings.ChrW(Convert.ToInt32(Input.Substring(I + 1, 8), 16)))
                            I += 8
                            Continue For
                        End If

                        If IsOctalDigit(T1) Then
                            If I + 2 >= Input.Length Then
                                If DoesThrow Then
                                    Throw New ArgumentException("Invalid escaped string.")
                                Else
                                    Res.Append(T1)
                                    Continue For
                                End If
                            End If
                            If Not (IsOctalDigit(Input.Chars(I + 1)) AndAlso
                                    IsOctalDigit(Input.Chars(I + 2))) Then
                                If DoesThrow Then
                                    Throw New ArgumentException("Invalid escaped string.")
                                Else
                                    Res.Append(T1)
                                    Continue For
                                End If
                            End If

                            Res.Append(Strings.ChrW(Convert.ToInt32(Input.Substring(I, 3), 8)))
                            I += 2
                            Continue For
                        End If

                        If DoesThrow Then
                            Throw New ArgumentException("Invalid escaped string.")
                        Else
                            Res.Append(T1)
                            Continue For
                        End If
                    End If
                    Res.Append(T2)
                Next

                Return Res.ToString()
            End Function
#End Region

            Public Shared Function EscapeNewLine(ByVal Str As String) As String
                Dim Res = New StringBuilder()

                For Each Ch In Str
                    Select Case Ch
                        Case ControlChars.Cr
                            Res.Append("\r")
                        Case ControlChars.Lf
                            Res.Append("\n")
                        Case "\"c
                            Res.Append("\\")
                        Case Else
                            Res.Append(Ch)
                    End Select
                Next

                Return Res.ToString()
            End Function

            Public Shared Function UnescapeNewLine(ByVal Str As String) As String
                Dim Res = New StringBuilder()

                For I As Integer = 0 To Str.Length - 1
                    Dim Ch = Str.Chars(I)

                    If Ch = "\"c Then
                        I += 1
                        If I = Str.Length Then
                            Throw New Exception("Invalid list string.")
                        End If
                        Ch = Str.Chars(I)

                        Select Case Ch
                            Case "r"c
                                Res.Append(ControlChars.Cr)
                            Case "n"c
                                Res.Append(ControlChars.Lf)
                            Case "\"c
                                Res.Append("\")
                            Case Else
                                Throw New Exception("Invalid escape character.")
                        End Select
                    End If

                    Res.Append(Ch)
                Next

                Return Res.ToString()
            End Function

            Public Shared ReadOnly Property CurruntFormatProvider As IFormatProvider
                Get
                    Return Nothing
                End Get
            End Property

            Public Shared Function FirstCapitalized(ByVal Str As String) As String
                Return Char.ToUpper(Str.Chars(0)) & Str.Substring(1).ToLower()
            End Function

            Public Shared Function EnumerableToString(Of T)(ByVal Enumerable As IEnumerable(Of T)) As String
                Dim Res = New StringBuilder("{"c)
                Dim Bl = True

                For Each I As T In Enumerable
                    If Bl Then
                        Bl = False
                    Else
                        Res.Append(", ")
                    End If
                    Res.Append(I)
                Next

                Return Res.Append("}"c).ToString()
            End Function

            Public Shared Function PadAlignString(Str As String, Ch As Char, Length As Integer, Alignment As TextAlignment) As String
                Verify.False(Length < Str.Length, "Length of Str is less than Length.")

                Dim N = Str.Length

                Dim Res = ""
                Select Case Alignment
                    Case TextAlignment.Left
                        Res = Str.PadRight(Length, Ch)
                    Case TextAlignment.Center
                        Res = Str.PadRight(Length - N \ 2, Ch).PadLeft(Length, Ch)
                    Case TextAlignment.Right
                        Res = Str.PadLeft(Length, Ch)
                    Case Else
                        Verify.FailArg(NameOf(Alignment), "Invalid Alignment.")
                End Select

                Return Res
            End Function

            Public Enum TextAlignment
                Right
                Center
                Left
            End Enum

        End Class

    End Class

End Namespace
