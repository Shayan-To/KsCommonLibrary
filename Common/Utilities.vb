Imports System.Runtime.CompilerServices
Imports System.Text
Imports Media = System.Windows.Media
Imports Reflect = System.Reflection
Imports SIO = System.IO

Public NotInheritable Class Utilities

    Private Sub New()
        Throw New NotSupportedException()
    End Sub

    Public Class Text

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

#Region "Escape Support Logic"
        Private Shared ReadOnly EscapeDic As Dictionary(Of Char, Char) = InitEscapeDic()

        Private Shared Function InitEscapeDic() As Dictionary(Of Char, Char)
            Dim R = New Dictionary(Of Char, Char)()
            R.Add("0"c, Strings.ChrW(&H00))
            R.Add(""""c, Strings.ChrW(&H27))
            R.Add("'"c, Strings.ChrW(&H22))
            R.Add("?"c, Strings.ChrW(&H3F))
            R.Add("\"c, Strings.ChrW(&H5C))
            R.Add("a"c, Strings.ChrW(&H07))
            R.Add("b"c, Strings.ChrW(&H08))
            R.Add("f"c, Strings.ChrW(&H0C))
            R.Add("n"c, Strings.ChrW(&H0A))
            R.Add("r"c, Strings.ChrW(&H0D))
            R.Add("t"c, Strings.ChrW(&H09))
            R.Add("v"c, Strings.ChrW(&H0B))
            Return R
        End Function

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

    End Class

    Public Class Math

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

#Region "ModAdditions Logic"
        ''' <summary>
        ''' Calculates the non-negative reminder of the two numbers specified.
        ''' </summary>
        ''' <param name="A">The dividend</param>
        ''' <param name="B">The divisor</param>
        ''' <returns>
        ''' The reminder, R, of A divided by B. If it is a negative number, A + Abs(B) will be returned.
        ''' The result is always between 0 and B - 1.
        ''' </returns>
#If VBC_VER >= 11.0 Then
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function NonNegMod(ByVal A As Integer, ByVal B As Integer) As Integer
#Else
    Public Shared Function NonNegMod(ByVal A As Integer, ByVal B As Integer) As Integer
#End If
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
#If VBC_VER >= 11.0 Then
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function PosMod(ByVal A As Integer, ByVal B As Integer) As Integer
#Else
    Public Shared Function PosMod(ByVal A As Integer, ByVal B As Integer) As Integer
#End If
            A = A Mod B
            If A > 0 Then
                Return A
            End If
            If B > 0 Then
                Return A + B
            End If
            Return A - B
        End Function
#End Region

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

        Public Shared Function GetStaticRandom() As Random
            Static Random As Random = New Random()
            Return Random
        End Function

    End Class

    Public Class Debug

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        Public Shared Function CompactStackTrace(ByVal Count As Integer) As String
            Dim ST = New StackTrace(True)
            Dim F = ST.GetFrames()

            If Count >= F.Length Then
                Count = F.Length - 1
            End If

            Dim R = New StringBuilder()
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
            Dim R = New StringBuilder()

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
            Dim R = New StringBuilder()

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

    End Class

    Public Class Reflection

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        Public Shared Function GetAllAccessibleAssemblies() As IEnumerable(Of Reflect.Assembly)
            Return Reflect.Assembly.GetEntryAssembly().GetRecursiveReferencedAssemblies()
        End Function

        Public Shared Iterator Function GetAllMethods(Optional ByVal Assemblies As IEnumerable(Of Reflect.Assembly) = Nothing) As IEnumerable(Of Reflect.MethodInfo)
            If Assemblies Is Nothing Then
                Assemblies = GetAllAccessibleAssemblies()
            End If
            For Each Ass In Assemblies
                For Each Type As Type In Ass.GetTypes()
                    For Each Method As Reflect.MethodInfo In Type.GetMethods(Reflect.BindingFlags.Static Or Reflect.BindingFlags.Instance Or Reflect.BindingFlags.Public Or Reflect.BindingFlags.NonPublic)
                        Yield Method
                    Next
                Next
            Next
        End Function

        Public Shared Iterator Function GetAllTypes(Optional ByVal Assemblies As IEnumerable(Of Reflect.Assembly) = Nothing) As IEnumerable(Of Type)
            If Assemblies Is Nothing Then
                Assemblies = GetAllAccessibleAssemblies()
            End If
            For Each Ass In Assemblies
                For Each Type As Type In Ass.GetTypes()
                    Yield Type
                Next
            Next
        End Function

        Public Shared Iterator Function GetAllTypesDerivedFrom(ByVal Base As Type, Optional ByVal Assemblies As IEnumerable(Of Reflect.Assembly) = Nothing) As IEnumerable(Of Type)
            If Assemblies Is Nothing Then
                Assemblies = GetAllAccessibleAssemblies()
            End If
            For Each Type As Type In GetAllTypes(Assemblies)
                If Base.IsAssignableFrom(Type) Then
                    Yield Type
                End If
            Next
        End Function

        <Sample()>
        Public Shared Sub GetAllTypesDerivedFrom(ByVal Base As Cecil.TypeDefinition, Optional ByVal Assembly As Reflect.Assembly = Nothing)
            Dim Helper = CecilHelper.Instance

            If Assembly Is Nothing Then
                Assembly = Reflect.Assembly.GetEntryAssembly()
            End If

            For Each A In Helper.GetReferencedAssemblies(Helper.Convert(Assembly))
                For Each M In A.Modules
                    For Each T In M.Types
                        If Helper.IsBaseTypeOf(Base, T) Then
                            Console.WriteLine(T)
                        End If
                    Next
                Next
            Next
        End Sub

    End Class

    Public Class IO

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        Public Shared Function EnsureExists(ByVal Path As String) As Boolean
            If Not SIO.Directory.Exists(Path) Then
                SIO.Directory.CreateDirectory(Path)
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function DownloadURL(ByVal URL As String) As String
            Dim Request = Net.WebRequest.Create(URL)
            Using Response = Request.GetResponse(),
                  Reader = New SIO.StreamReader(Response.GetResponseStream())
                Dim Res = Reader.ReadToEnd()

                Return Res
            End Using
        End Function

        Public Shared Sub DownloadURLToFile(ByVal URL As String, ByVal Path As String)
            Dim Request = Net.WebRequest.Create(URL)
            Using Response = Request.GetResponse(),
                  WStream = Response.GetResponseStream(),
                  FStream = SIO.File.Open(Path, SIO.FileMode.CreateNew, SIO.FileAccess.Write, SIO.FileShare.Read)
                FStream.Write(WStream)
            End Using
        End Sub

        ''' <summary>
        ''' Replaces invalid file name characters with '_'.
        ''' </summary>
        ''' <param name="Name"></param>
        ''' <returns></returns>
        Public Shared Function CorrectFileName(ByVal Name As String) As String
            Dim Res = New StringBuilder(Name.Length)
            Dim Invalids = SIO.Path.GetInvalidFileNameChars()
            For Each Ch In Name
                If Invalids.Contains(Ch) Then
                    Res.Append("_"c)
                Else
                    Res.Append(Ch)
                End If
            Next

            Return Res.ToString().Trim()
        End Function

        Public Shared Function ReadAll(ByVal Reader As ReadCall, ByVal Buffer As Byte(), ByVal Offset As Integer, ByVal Length As Integer) As Integer
            Dim N = Length

            Do
                Dim T = Reader.Invoke(Buffer, Offset + Length - N, N)
                If T = 0 Then
                    Return Length - N
                End If
                N -= T
            Loop Until N = 0

            Return Length
        End Function

        Public Delegate Sub WriteCall(ByVal Buffer As Byte(), ByVal Offset As Integer, ByVal Length As Integer)
        Public Delegate Function ReadCall(ByVal Buffer As Byte(), ByVal Offset As Integer, ByVal MaxLength As Integer) As Integer

    End Class

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

        Public Shared Function ListToStirng(ByVal List As IEnumerable(Of String)) As String
            Dim Res = New StringBuilder("{")
            Dim Bl = True

            For Each Str As String In List
                If Bl Then
                    Bl = False
                Else
                    Res.Append(",")
                End If

                For Each Ch In Str
                    Select Case Ch
                        Case ControlChars.Cr
                            Res.Append("\r")
                        Case ControlChars.Lf
                            Res.Append("\n")
                        Case ","c, "\"c, "{"c, "}"c
                            Res.Append("\"c).Append(Ch)
                        Case Else
                            Res.Append(Ch)
                    End Select
                Next
            Next

            Return Res.Append("}").ToString()
        End Function

        Public Shared Function ListFromStirng(ByVal Str As String) As List(Of String)
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
                    Ch = Str.Chars(I)

                    Select Case Ch
                        Case "r"c
                            R.Append(ControlChars.Cr)
                        Case "n"c
                            R.Append(ControlChars.Lf)
                        Case ","c, "\"c, "{"c, "}"c
                            R.Append(Ch)
                        Case Else
                            Throw New Exception("Invalid escape character.")
                    End Select

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

        Private Shared Function EscapeChar(ByVal Ch As Char) As String
            Select Case Ch
                Case ControlChars.Cr
                    Return "\r"
                Case ControlChars.Lf
                    Return "\n"
                Case ","c, "\"c, "{"c, "}"c, ":"c
                    Return "\"c & Ch
                Case Else
                    Return Ch
            End Select
        End Function

        Private Shared Function UnescapeChar(ByVal Ch As Char) As Char
            Select Case Ch
                Case "r"c
                    Return ControlChars.Cr
                Case "n"c
                    Return ControlChars.Lf
                Case ","c, "\"c, "{"c, "}"c, ":"c
                    Return Ch
                Case Else
                    Throw New Exception("Invalid escape character.")
            End Select
        End Function

        Public Shared Function DicToStirng(ByVal Dic As IDictionary(Of String, String)) As String
            Dim Res = New StringBuilder("{")
            Dim Bl = True

            For Each KV In Dic
                If Bl Then
                    Bl = False
                Else
                    Res.Append(",")
                End If

                For Each Ch In KV.Key
                    Res.Append(EscapeChar(Ch))
                Next
                Res.Append(":"c)
                For Each Ch In KV.Value
                    Res.Append(EscapeChar(Ch))
                Next
            Next

            Return Res.Append("}").ToString()
        End Function

        Public Shared Function DicFromStirng(ByVal Str As String) As Dictionary(Of String, String)
            Dim Res = New Dictionary(Of String, String)()
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

                    R.Append(UnescapeChar(Str.Chars(I)))
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

    End Class

#Region "CombineHashCodes Logic"
    Public Shared Function CombineHashCodes(ByVal H1 As Integer,
                                            ByVal H2 As Integer) As Integer
        ' ToDo Return ((H1 << 5) + H1) Xor H2
        Return ((H1 << 5) Or (H1 >> 27)) Xor H2
    End Function

    Public Shared Function CombineHashCodes(ByVal H1 As Integer,
                                            ByVal H2 As Integer,
                                            ByVal H3 As Integer) As Integer
        Return CombineHashCodes(CombineHashCodes(H1, H2), H3)
    End Function

    Public Shared Function CombineHashCodes(ByVal H1 As Integer,
                                            ByVal H2 As Integer,
                                            ByVal H3 As Integer,
                                            ByVal H4 As Integer) As Integer
        Return CombineHashCodes(CombineHashCodes(H1, H2), CombineHashCodes(H3, H4))
    End Function

    Public Shared Function CombineHashCodes(ByVal H1 As Integer,
                                            ByVal H2 As Integer,
                                            ByVal H3 As Integer,
                                            ByVal H4 As Integer,
                                            ByVal H5 As Integer) As Integer
        Return CombineHashCodes(CombineHashCodes(H1, H2), CombineHashCodes(H3, H4, H5))
    End Function
#End Region

#Region "Empties Group"
    Public Shared EmptyObject As Object = New Object()

    Public NotInheritable Class Typed(Of T)

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        Public Shared ReadOnly EmptyArray As T() = New T(-1) {}

    End Class
#End Region

    Public Shared Iterator Function Range(ByVal Start As Integer, ByVal [End] As Integer, Optional ByVal [Step] As Integer = 1) As IEnumerable(Of Integer)
        Verify.TrueArg([Step] > 0, "Step")
        For Start = Start To [End] - 1 Step [Step]
            Yield Start
        Next
    End Function

    Public Shared Function Range(ByVal [End] As Integer) As IEnumerable(Of Integer)
        Return Range(0, [End])
    End Function

    Public Shared Sub DoNothing()

    End Sub

End Class
