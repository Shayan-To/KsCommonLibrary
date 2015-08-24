Imports System.Runtime.CompilerServices
Imports Media = System.Windows.Media

Public NotInheritable Class Utilities

    Private Shared ReadOnly EscapeDic As Dictionary(Of Char, Char)

    Shared Sub New()
        EscapeDic = New Dictionary(Of Char, Char)()
        EscapeDic.Add("0"c, Strings.ChrW(&H00))
        EscapeDic.Add(""""c, Strings.ChrW(&H27))
        EscapeDic.Add("'"c, Strings.ChrW(&H22))
        EscapeDic.Add("?"c, Strings.ChrW(&H3F))
        EscapeDic.Add("\"c, Strings.ChrW(&H5C))
        EscapeDic.Add("a"c, Strings.ChrW(&H07))
        EscapeDic.Add("b"c, Strings.ChrW(&H08))
        EscapeDic.Add("f"c, Strings.ChrW(&H0C))
        EscapeDic.Add("n"c, Strings.ChrW(&H0A))
        EscapeDic.Add("r"c, Strings.ChrW(&H0D))
        EscapeDic.Add("t"c, Strings.ChrW(&H09))
        EscapeDic.Add("v"c, Strings.ChrW(&H0B))
    End Sub

    Private Sub New()
        Throw New NotSupportedException()
    End Sub

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
        Dim Res As Text.StringBuilder,
            T1, T2 As Char

        Res = New Text.StringBuilder()

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

    ''' <summary>
    ''' Calculates the non-negative reminder of the two numbers specified.
    ''' </summary>
    ''' <param name="A">The dividend</param>
    ''' <param name="B">The divisor</param>
    ''' <returns>
    ''' The reminder, R, of A divided by B. If it is a negative number, A + Abs(B) will be returned.
    ''' The result is always between 0 and B - 1.
    ''' </returns>
#If VBC_VER >= 12.0 Then
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
#End If
    Public Shared Function NonNegMod(ByVal A As Integer, ByVal B As Integer) As Integer
        A = A Mod B
        If A >= 0 Then
            Return A
        End If
        If B > 0 Then
            Return A + B
        End If
        Return A - B
    End Function

    ''' <summary>
    ''' Calculates the positive reminder of the two numbers specified.
    ''' </summary>
    ''' <param name="A">The dividend</param>
    ''' <param name="B">The divisor</param>
    ''' <returns>
    ''' The reminder, R, of A divided by B. If it is not a positive number, A + Abs(B) will be returned.
    ''' The result is always between 1 and B.
    ''' </returns>
#If VBC_VER >= 12.0 Then
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
#End If
    Public Shared Function PosMod(ByVal A As Integer, ByVal B As Integer) As Integer
        A = A Mod B
        If A > 0 Then
            Return A
        End If
        If B > 0 Then
            Return A + B
        End If
        Return A - B
    End Function

    Public Shared Function GetStaticRandom() As Random
        Static Random As Random = New Random()
        Return Random
    End Function

    Public Shared Function CollectionToString(Of T)(ByVal Collection As IEnumerable(Of T)) As String
        Dim Res As Text.StringBuilder,
            Enumerator As IEnumerator(Of T)

        Res = New Text.StringBuilder("{")

        Enumerator = Collection.GetEnumerator()

        If Enumerator.MoveNext() Then
            Res.Append(Enumerator.Current)
        End If

        Do While Enumerator.MoveNext()
            Res.Append(", ").Append(Enumerator.Current)
        Loop

        Return Res.Append("}").ToString()
    End Function

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

    Public Shared ReadOnly Property IsInDesignMode As Boolean
        Get
            Return ComponentModel.DesignerProperties.GetIsInDesignMode(New DependencyObject())
        End Get
    End Property

    Public Shared ReadOnly Property CurruntFormatProvider As IFormatProvider
        Get
            Return Nothing
        End Get
    End Property

    <ConsoleTestMethodAttribute()>
    Friend Shared Sub TypeInterfaceFinder()
        Dim Type = GetType(Tuple(Of Integer))
        For Each T In Type.GetInterfaces()
            Console.WriteLine(T.FullName & ControlChars.Tab & T.Attributes.ToString())
        Next
    End Sub

    Public Shared Function EnumerableToString(Of T)(ByVal Enumerable As IEnumerable(Of T)) As String
        Dim Res = New Text.StringBuilder("{"c)
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

    Public Shared Function FloorDiv(ByVal A As Integer, ByVal B As Integer) As Integer
        If A >= 0 Or A Mod B = 0 Then
            Return A \ B
        End If
        Return A \ B - 1
    End Function

    Public Shared Function FloorDiv(ByVal A As Long, ByVal B As Long) As Long
        If A >= 0 Or A Mod B = 0 Then
            Return A \ B
        End If
        Return A \ B - 1
    End Function

    Public Shared Function FirstCapitalized(ByVal Str As String) As String
        Return Char.ToUpper(Str.Chars(0)) & Str.Substring(1).ToLower()
    End Function

    Public Shared Function CompactStackTrace(ByVal Count As Integer) As String
        Dim ST = New StackTrace(True)
        Dim F = ST.GetFrames()

        If Count >= F.Length Then
            Count = F.Length - 1
        End If

        Dim R = New Text.StringBuilder()
        For I As Integer = 1 To Count
            If I > 1 Then
                R.Append(">"c)
            End If
            Dim M = F(I).GetMethod
            R.Append(M.DeclaringType.Name) _
             .Append("."c) _
             .Append(M.Name) _
             .Append(M.GetParameters().Length) _
             .Append(":"c) _
             .Append(F(I).GetFileLineNumber())
        Next

        Return R.ToString()
    End Function

    Public Shared Sub PrintCallInformation(ByVal ParamArray Args As Object())
        Dim R = New Text.StringBuilder()

        Dim F = New StackFrame(1)
        Dim M = F.GetMethod()
        Dim P = M.GetParameters()
        F = New StackFrame(2, True)

        If Args.Length <> 0 And P.Length <> Args.Length Then
            Throw New ArgumentException()
        End If

        R.Append(F.GetFileLineNumber) _
         .Append(":") _
         .Append(M.DeclaringType.Name) _
         .Append("."c) _
         .Append(M.Name) _
         .Append(P.Length) _
         .Append("("c)

        If Args.Length = 0 Then
            If P.Length <> 0 Then
                R.Append("...")
            End If
        Else
            Dim Bl = True
            For Each A In Args
                If Bl Then
                    Bl = False
                Else
                    R.Append(", ")
                End If
                R.Append(A)
            Next
        End If

        R.AppendLine(")"c)

        Console.Write(R.ToString())
    End Sub

    Public Shared Function GetCallInformation(ByVal ParamArray Args As Object()) As String
        Dim R = New Text.StringBuilder()

        Dim F = New StackFrame(1)
        Dim M = F.GetMethod()
        Dim P = M.GetParameters()
        F = New StackFrame(2, True)

        If Args.Length <> 0 And P.Length <> Args.Length Then
            Throw New ArgumentException()
        End If

        R.Append(F.GetFileLineNumber) _
         .Append(":") _
         .Append(M.DeclaringType.Name) _
         .Append("."c) _
         .Append(M.Name) _
         .Append(P.Length) _
         .Append("("c)

        If Args.Length = 0 Then
            If P.Length <> 0 Then
                R.Append("...")
            End If
        Else
            Dim Bl = True
            For Each A In Args
                If Bl Then
                    Bl = False
                Else
                    R.Append(", ")
                End If
                R.Append(A)
            Next
        End If

        R.AppendLine(")"c)

        Return R.ToString()
    End Function

    Public Shared Function CombineHasCodes(ByVal H1 As Integer,
                                           ByVal H2 As Integer) As Integer
        Return ((H1 << 5) + H1) Xor H2
    End Function

    Public Shared Function CombineHasCodes(ByVal H1 As Integer,
                                           ByVal H2 As Integer,
                                           ByVal H3 As Integer) As Integer
        Return CombineHasCodes(CombineHasCodes(H1, H2), H3)
    End Function

    Public Shared Function CombineHasCodes(ByVal H1 As Integer,
                                           ByVal H2 As Integer,
                                           ByVal H3 As Integer,
                                           ByVal H4 As Integer) As Integer
        Return CombineHasCodes(CombineHasCodes(H1, H2), CombineHasCodes(H3, H4))
    End Function

    Public Shared Function CombineHasCodes(ByVal H1 As Integer,
                                           ByVal H2 As Integer,
                                           ByVal H3 As Integer,
                                           ByVal H4 As Integer,
                                           ByVal H5 As Integer) As Integer
        Return CombineHasCodes(CombineHasCodes(H1, H2), CombineHasCodes(H3, H4, H5))
    End Function

    Public Shared EmptyObject As Object = New Object()

    Public NotInheritable Class Typed(Of T)

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        Public Shared ReadOnly EmptyArray As T() = New T(-1) {}

    End Class

End Class
