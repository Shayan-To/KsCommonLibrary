Imports System.Runtime.CompilerServices
Imports System.Text
Imports Media = System.Windows.Media
Imports Reflect = System.Reflection
Imports SIO = System.IO

Namespace Common

    Public NotInheritable Class Utilities

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

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
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
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
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
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
#End Region

            Public Shared Function Power(ByVal A As Integer, ByVal B As Integer) As Integer
                Dim R = 1
                Do Until B = 0
                    If (B And 1) = 1 Then
                        R *= A
                    End If
                    A *= A
                    B >>= 1
                Loop
                Return R
            End Function

            Public Shared Function PowerL(ByVal A As Long, ByVal B As Long) As Long
                Dim R = 1L
                Do Until B = 0
                    If (B And 1) = 1 Then
                        R *= A
                    End If
                    A *= A
                    B >>= 1
                Loop
                Return R
            End Function

            Public Shared Function SquareRoot(ByVal A As Integer) As Integer
                Dim ARev = 0
                Dim T = A
                Do Until T = 0
                    ARev = (ARev << 2) Or (T And 3)
                    T >>= 2
                Loop

                Dim Reminder = 0
                Dim Root = 0
                Do Until A = 0
                    Reminder = (Reminder << 2) Or (ARev And 3)

                    ARev >>= 2
                    A >>= 2

                    Root <<= 1
                    Dim Root2 = (Root << 1) Or 1

                    If Reminder >= Root2 Then
                        Root = Root Or 1
                        Reminder -= Root2
                    End If
                Loop

                Return Root
            End Function

            Public Shared Function SquareRootL(ByVal A As Long) As Long
                Dim ARev = 0L
                Dim T = A
                Do Until T = 0
                    ARev = (ARev << 2) Or (T And 3)
                    T >>= 2
                Loop

                Dim Reminder = 0L
                Dim Root = 0L
                Do Until A = 0
                    Reminder = (Reminder << 2) Or (ARev And 3)

                    ARev >>= 2
                    A >>= 2

                    Root <<= 1
                    Dim Root2 = (Root << 1) Or 1

                    If Reminder >= Root2 Then
                        Root = Root Or 1
                        Reminder -= Root2
                    End If
                Loop

                Return Root
            End Function

            Public Shared Function LeastPowerOfTwoOnMin(ByVal Min As Integer) As Integer
                If Min < 1 Then
                    Return 1
                End If

                ' If Min is a power of two, we should return Min, otherwise, Min * 2
                Dim T = (Min - 1) And Min
                If T = 0 Then
                    Return Min
                End If
                Min = T

                Do
                    T = (Min - 1) And Min
                    If T = 0 Then
                        Return Min << 1
                    End If
                    Min = T
                Loop
            End Function

            Public Shared Function LeastPowerOfTwoOnMin(ByVal Min As Long) As Long
                If Min < 1 Then
                    Return 1
                End If

                ' If Min is a power of two, we should return Min, otherwise, Min * 2
                Dim T = (Min - 1) And Min
                If T = 0 Then
                    Return Min
                End If
                Min = T

                Do
                    T = (Min - 1) And Min
                    If T = 0 Then
                        Return Min << 1
                    End If
                    Min = T
                Loop
            End Function

            Public Shared Function FloorDiv(ByVal A As Integer, ByVal B As Integer) As Integer
                ' ToDo What if B is negative?
                If A >= 0 Or A Mod B = 0 Then
                    Return A \ B
                End If
                Return A \ B - 1
            End Function

            Public Shared Function FloorDiv(ByVal A As Long, ByVal B As Long) As Long
                ' ToDo What if B is negative?
                If A >= 0 Or A Mod B = 0 Then
                    Return A \ B
                End If
                Return A \ B - 1
            End Function

            Public Shared Function GreatestCommonDivisor(ByVal A As Integer, ByVal B As Integer) As Integer
                If B < 0 Then
                    B = -B
                End If
                If A < 0 Then
                    A = -A
                End If
                Do Until B = 0
                    Dim C = A Mod B
                    A = B
                    B = C
                Loop
                Return A
            End Function

            Public Shared Function GreatestCommonDivisor(ByVal A As Long, ByVal B As Long) As Long
                If B < 0 Then
                    B = -B
                End If
                If A < 0 Then
                    A = -A
                End If
                Do Until B = 0
                    Dim C = A Mod B
                    A = B
                    B = C
                Loop
                Return A
            End Function

            Public Shared Function LeastCommonMultiple(ByVal A As Integer, ByVal B As Integer) As Integer
                Return (A \ GreatestCommonDivisor(A, B)) * B
            End Function

            Public Shared Function LeastCommonMultiple(ByVal A As Long, ByVal B As Long) As Long
                Return (A \ GreatestCommonDivisor(A, B)) * B
            End Function

            Public Shared Function IsOfIntegralType(O As Object) As Boolean
                Return TypeOf O Is Byte Or TypeOf O Is UShort Or TypeOf O Is UInteger Or TypeOf O Is ULong Or
                       TypeOf O Is SByte Or TypeOf O Is Short Or TypeOf O Is Integer Or TypeOf O Is Long Or
                       TypeOf O Is Single Or TypeOf O Is Double
            End Function

        End Class

        Public Class Debug

            Private Sub New()
                Throw New NotSupportedException()
            End Sub

            Public Shared Sub ShowMessageBox(text As String, Optional caption As String = "")
                Forms.MessageBox.Show(text, caption)
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
                 .Append("_"c) _
                 .Append(P.Length) _
                 .Append("("c)

                If Args.Length = 0 Then
                    If P.Length <> 0 Then
                        R.Append("...")
                    End If
                Else
                    Dim Bl = True
                    For I As Integer = 0 To Args.Length - 1
                        If Bl Then
                            Bl = False
                        Else
                            R.Append(", ")
                        End If
                        R.Append(P(I).Name).Append("="c).Append(Args(I))
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

        Public Class Collections

            Public Shared Iterator Function Concat(Of T)(ByVal Collections As IEnumerable(Of IEnumerable(Of T))) As IEnumerable(Of T)
                For Each L In Collections
                    For Each I In L
                        Yield I
                    Next
                Next
            End Function

            Public Shared Function Concat(Of T)(ParamArray ByVal Collections As IEnumerable(Of T)()) As IEnumerable(Of T)
                Return Concat(DirectCast(Collections, IEnumerable(Of IEnumerable(Of T))))
            End Function

            Public Shared Iterator Function Range(ByVal Start As Integer, ByVal [End] As Integer, Optional ByVal [Step] As Integer = 1) As IEnumerable(Of Integer)
                Verify.TrueArg([Step] > 0, "Step")
                For Start = Start To [End] - 1 Step [Step]
                    Yield Start
                Next
            End Function

            Public Shared Function Range(ByVal [End] As Integer) As IEnumerable(Of Integer)
                Return Range(0, [End])
            End Function

            Public Shared Iterator Function Repeat(Of T)(ByVal I1 As T, ByVal Count As Integer) As IEnumerable(Of T)
                For I As Integer = 1 To Count
                    Yield I1
                Next
            End Function

#Region "InEnumerable Logic"
            Public Shared Iterator Function InEnumerable(Of T)() As IEnumerable(Of T)
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T) As IEnumerable(Of T)
                Yield I1
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T, ByVal I2 As T) As IEnumerable(Of T)
                Yield I1
                Yield I2
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T, ByVal I2 As T, ByVal I3 As T) As IEnumerable(Of T)
                Yield I1
                Yield I2
                Yield I3
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T, ByVal I2 As T, ByVal I3 As T, ByVal I4 As T) As IEnumerable(Of T)
                Yield I1
                Yield I2
                Yield I3
                Yield I4
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T, ByVal I2 As T, ByVal I3 As T, ByVal I4 As T, ByVal I5 As T) As IEnumerable(Of T)
                Yield I1
                Yield I2
                Yield I3
                Yield I4
                Yield I5
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T, ByVal I2 As T, ByVal I3 As T, ByVal I4 As T, ByVal I5 As T, ByVal I6 As T) As IEnumerable(Of T)
                Yield I1
                Yield I2
                Yield I3
                Yield I4
                Yield I5
                Yield I6
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T, ByVal I2 As T, ByVal I3 As T, ByVal I4 As T, ByVal I5 As T, ByVal I6 As T, ByVal I7 As T) As IEnumerable(Of T)
                Yield I1
                Yield I2
                Yield I3
                Yield I4
                Yield I5
                Yield I6
                Yield I7
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T, ByVal I2 As T, ByVal I3 As T, ByVal I4 As T, ByVal I5 As T, ByVal I6 As T, ByVal I7 As T, ByVal I8 As T) As IEnumerable(Of T)
                Yield I1
                Yield I2
                Yield I3
                Yield I4
                Yield I5
                Yield I6
                Yield I7
                Yield I8
            End Function
#End Region

        End Class

        Public Class Time

            Public Shared Function GetFriendlyRepresentation(ByVal Time As TimeSpan, ByVal MaxError As TimeSpan) As String
                Dim Units = {VTuple.Create("ms", TimeSpan.FromMilliseconds(1)),
                         VTuple.Create("s", TimeSpan.FromSeconds(1)),
                         VTuple.Create("min", TimeSpan.FromMinutes(1)),
                         VTuple.Create("h", TimeSpan.FromHours(1)),
                         VTuple.Create("d", TimeSpan.FromDays(1))}
                ' We want to be able to show values using a max error value, so we have to remove the unnecessary units.
                ' So we will keep the units that are greater than or equal to MaxError.
                ' But these are not enough, as they may not show the value with needed precision if no unit is equal to MaxError.
                ' We will use in that case A'shari numbers. See below.

                ' We want the units that are greater than or equal to Error.
                ' And if the last unit is less that Error, we have no choice but to use it alone. So Func is true for the last unit.
                Dim Start = Algorithm.BinarySearchIn(Function(X) Units(X).Item2 >= MaxError, 0, Units.Length - 1)
                ' The Units with false should not be used.
                ' And from the remaining ones, we will use at most Count of them.
                Dim Count = 2

                Dim Res = New StringBuilder()
                Dim Ticks = Time.Ticks
                ' We will start from the greatest unit, and pick at most Count of them with non-zero Value.
                ' ToDo If ticks is negative, the greatest unit will always be non-zero, even if not necessary. (-1 days 23 hours, instead of -1 hours)
                Dim I = 0
                For I = Units.Length - 1 To Start + 1 Step -1
                    Dim Unit = Units(I)
                    Dim UnitTicks = Unit.Item2.Ticks
                    Dim Value = Math.FloorDiv(Ticks, UnitTicks)
                    Ticks -= Value * UnitTicks

                    If Value <> 0 Then
                        If Count = 1 Then
                            Exit For
                        End If
                        If Res.Length <> 0 Then
                            Res.Append(" ")
                        End If
                        Res.Append(Value).Append(Unit.Item1.ToLowerInvariant())
                        Count -= 1
                    End If
                Next

                ' A block for limiting the scope of variables.
                Do
                    Dim Unit = Units(I)
                    Dim UnitTicks = Unit.Item2.Ticks

                    ' Just like the units, we have to check whether (0.01 U) is a good unit or not. (Division will move us upwards in the list.)
                    ' We will find units less than or equal to MaxError (the bad ones + equals), and choose the first of them (plus the good ones of course).
                    ' 
                    ' And we cannot optimize it by assuming we can do any number of digits when not at Start.
                    ' A unit is something about 20-100 times smaller than the previous one, so maybe we are forced to use only one digit.
                    Dim Prec = 100
                    Do While UnitTicks \ Prec <= MaxError.Ticks
                        Prec \= 10
                        If Prec = 0 Then
                            ' We will end up here only if the unit equals the error.
                            Exit Do
                        End If
                    Loop
                    Prec = If(Prec = 0, 1, Prec * 10)
                    If Prec > 100 Then
                        Prec = 100
                    End If

                    ' We assume (Unit / Prec) to be another unit, but print the result divided by Prec.
                    Dim Value = System.Math.Round(Ticks / (UnitTicks \ Prec))
                    If Value <> 0 Or Res.Length = 0 Then
                        If Res.Length <> 0 Then
                            Res.Append(" ")
                        End If
                        Res.Append(Value / Prec).Append(Unit.Item1.ToLowerInvariant())
                    End If

                    Exit Do
                Loop

                Return Res.ToString()
            End Function

            Public Shared Function GetTimeStamp(ByVal Compact As Boolean) As String
                Static Builder As StringBuilder = New StringBuilder()

                Dim Now = DateTime.Now
                Dim IC = Globalization.CultureInfo.InvariantCulture

                If Compact Then
                    Builder.Clear() _
                      .Append(Now.Year.ToString(IC).PadLeft(4, "0"c)) _
                      .Append(Now.Month.ToString(IC).PadLeft(2, "0"c)) _
                      .Append(Now.Day.ToString(IC).PadLeft(2, "0"c)) _
                      .Append(Now.Hour.ToString(IC).PadLeft(2, "0"c)) _
                      .Append(Now.Minute.ToString(IC).PadLeft(2, "0"c)) _
                      .Append(Now.Second.ToString(IC).PadLeft(2, "0"c)) _
                      .Append(Now.Millisecond.ToString(IC).PadLeft(3, "0"c))
                Else
                    Builder.Clear() _
                      .Append(Now.Year.ToString(IC).PadLeft(4, "0"c)) _
                      .Append("-"c) _
                      .Append(Now.Month.ToString(IC).PadLeft(2, "0"c)) _
                      .Append("-"c) _
                      .Append(Now.Day.ToString(IC).PadLeft(2, "0"c)) _
                      .Append(" "c) _
                      .Append(Now.Hour.ToString(IC).PadLeft(2, "0"c)) _
                      .Append(":"c) _
                      .Append(Now.Minute.ToString(IC).PadLeft(2, "0"c)) _
                      .Append(":"c) _
                      .Append(Now.Second.ToString(IC).PadLeft(2, "0"c)) _
                      .Append("."c) _
                      .Append(Now.Millisecond.ToString(IC).PadLeft(3, "0"c))
                End If

                Dim Stamp = Builder.ToString()
                Return Stamp
            End Function

            ''' <returns>(Hour, IsPm)</returns>
            Public Shared Function Hour24To12(ByVal Hour As Integer) As VTuple(Of Integer, Boolean)
                Dim IsPm = False

                If Hour >= 12 Then
                    Hour -= 12
                    IsPm = True
                End If
                If Hour = 0 Then
                    Hour = 12
                End If

                Return VTuple.Create(Hour, IsPm)
            End Function

            Public Shared Function Hour12To24(ByVal Hour As Integer, ByVal IsPm As Boolean) As Integer
                If Hour = 12 Then
                    Hour = 0
                End If
                If IsPm Then
                    Hour += 12
                End If

                Return Hour
            End Function

        End Class

        Public Class Algorithm

            ''' <param name="Func">N must exist that N >= 0, and Func(I) = True if and only if I >= N.</param>
            ''' <param name="MaxX">Func(MaxIndex) must equal True.</param>
            ''' <returns>Index of first true.</returns>
            Public Shared Function BinarySearch(ByVal Func As Func(Of Integer, Boolean), Optional ByVal MaxX As Integer = -1) As Integer
                If MaxX = -1 Then
                    MaxX = 8
                    Do Until Func.Invoke(MaxX)
                        MaxX <<= 1
                    Loop
                Else
                    Verify.True(Func.Invoke(MaxX))
                    MaxX = Math.LeastPowerOfTwoOnMin(MaxX)
                End If

                Dim X = -1
                Do While MaxX > 1
                    MaxX >>= 1
                    If Not Func.Invoke(X + MaxX) Then
                        X += MaxX
                    End If
                Loop

                Return X + 1
            End Function

            ''' <param name="Func">N must exist that N >= 0, and Func(I) = True if and only if I >= N.</param>
            ''' <param name="End">Func(End) must equal True.</param>
            ''' <returns>Index of first true.</returns>
            Public Shared Function BinarySearch(ByVal Func As Func(Of Integer, Boolean), ByVal Foreward As Boolean, ByVal Start As Integer, Optional ByVal [End] As Integer? = Nothing) As Integer
                If Foreward Then
                    Dim R = BinarySearch(Function(I) Func(I + Start), If([End] - Start, -1))
                    Return R + Start
                Else
                    Dim R = BinarySearch(Function(I) Func(Start - I), If(Start - [End], -1))
                    Return Start - R
                End If
            End Function

            ''' <param name="Func">N must exist that N >= 0, and Func(I) = True if and only if I >= N.</param>
            ''' <param name="End">Func(End) must equal True. Func will never be called on End.</param>
            ''' <returns>Index of first true.</returns>
            Public Shared Function BinarySearchIn(ByVal Func As Func(Of Integer, Boolean), ByVal Start As Integer, ByVal [End] As Integer) As Integer
                If Start <= [End] Then
                    Dim R = BinarySearch(Function(I) If(I + Start >= [End], True, Func(I + Start)), [End] - Start)
                    Return R + Start
                Else
                    Dim R = BinarySearch(Function(I) If(Start - I <= [End], True, Func(Start - I)), Start - [End])
                    Return Start - R
                End If
            End Function

            ''' <param name="Func">N must exist that N >= 0, and Func(I) = True if and only if I >= N.</param>
            ''' <param name="MaxX">Func(MaxIndex) must equal True.</param>
            ''' <returns>Some X that Func(X) = True and |X - N| &lt; MaxError (N is from doc of Func).</returns>
            Public Shared Function BinarySearch(ByVal Func As Func(Of Double, Boolean), ByVal MaxError As Double, Optional ByVal MaxX As Double = Double.NaN) As Double
                Verify.True(MaxError > 0)
                If Double.IsNaN(MaxX) Then
                    MaxX = 8 * MaxError
                    Do Until Func.Invoke(MaxX)
                        MaxX *= 2
                    Loop
                Else
                    Verify.True(Func.Invoke(MaxX))
                End If

                Dim X = 0.0
                Do While MaxX > MaxError
                    MaxX /= 2
                    If Not Func.Invoke(X + MaxX) Then
                        X += MaxX
                    End If
                Loop

                Return X + MaxError
            End Function

            ''' <summary>
            ''' Returns a list of (I, J) where List1[I] = List2[J] and the list is [one of] the longest possible list[s].
            ''' </summary>
            Public Shared Function GetLongestCommonSubsequence(Of T)(ByVal List1 As IReadOnlyList(Of T), ByVal List2 As IReadOnlyList(Of T)) As IReadOnlyList(Of VTuple(Of Integer, Integer))
                Return GetLongestCommonSubsequence(List1, List2, EqualityComparer(Of T).Default)
            End Function

            ''' <summary>
            ''' Returns a list of (I, J) where List1[I] = List2[J] and the list is [one of] the longest possible list[s].
            ''' </summary>
            Public Shared Function GetLongestCommonSubsequence(Of T)(ByVal List1 As IReadOnlyList(Of T), ByVal List2 As IReadOnlyList(Of T), ByVal Comparer As IEqualityComparer(Of T)) As IReadOnlyList(Of VTuple(Of Integer, Integer))
                ' We use dynamic programming.

                ' The length of longest common subsequence of List1[0..m] and List2[0..n] is max of:
                ' - If List1[m] = List2[n] Then: 1 + The length of longest common subsequence of List1[0..(m - 1)] and List2[0..(n - 1)].
                ' - The length of longest common subsequence of List1[0..m] and List2[0..(n - 1)].
                ' - The length of longest common subsequence of List1[0..(m - 1)] and List2[0..n].

                ' We do it from the other end so that we can have the result without reversing it.

                Dim M = List1.Count
                Dim N = List2.Count

                ' The tuple is (Length, Mode). See below.
                Dim Dyn = New VTuple(Of Integer, Integer)(M - 1, N - 1) {}

                ' Mode:
                ' 1 -> Did equal?
                ' 2 -> First index has +1?
                ' 4 -> Second index has +1?

                For I = M - 1 To 0 Step -1
                    For J = N - 1 To 0 Step -1
                        Dim Length = 0
                        Dim Mode = 0

                        If Comparer.Equals(List1.Item(I), List2.Item(J)) Then
                            Length = 1
                            Mode = 1
                            If I <> M - 1 And J <> N - 1 Then
                                Length += Dyn(I + 1, J + 1).Item1
                                Mode += 2 + 4
                            End If
                        End If

                        If I <> M - 1 Then
                            Dim L = Dyn(I + 1, J).Item1
                            If L > Length Then
                                Length = L
                                Mode = 2
                            End If
                        End If

                        If J <> N - 1 Then
                            Dim L = Dyn(I, J + 1).Item1
                            If L > Length Then
                                Length = L
                                Mode = 4
                            End If
                        End If

                        Dyn(I, J) = VTuple.Create(Length, Mode)
                    Next
                Next

                Dim Res = New List(Of VTuple(Of Integer, Integer))()
                Do ' Block for variable scopes
                    Dim I = 0
                    Dim J = 0
                    Dim Cur As VTuple(Of Integer, Integer)
                    Do
                        Cur = Dyn(I, J)
                        If (Cur.Item2 And 1) = 1 Then
                            Res.Add(VTuple.Create(I, J))
                        End If
                        If (Cur.Item2 And 2) = 2 Then
                            I += 1
                        End If
                        If (Cur.Item2 And 4) = 4 Then
                            J += 1
                        End If

                        Assert.True(((Cur.Item2 And (2 + 4)) = 2 + 4).Implies((Cur.Item2 And 1) = 1))
                    Loop Until (Cur.Item2 And (2 + 4)) = 0

                    Exit Do
                Loop

                Return Res.AsReadOnly()
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

        Public Function HslToRgb(ByVal H As Integer, ByVal S As Integer, ByVal L As Integer) As VTuple(Of Byte, Byte, Byte)
            Verify.True(0 <= H And H < 360)
            Verify.True(0 <= S And S < 100)
            Verify.True(0 <= L And L < 100)

            Dim HPart = H \ 60

            Throw New NotImplementedException()
        End Function

        Public Function HsbToRgb(ByVal H As Integer, ByVal S As Integer, ByVal B As Integer) As VTuple(Of Byte, Byte, Byte)
            Verify.True(0 <= H And H < 360)
            Verify.True(0 <= S And S < 100)
            Verify.True(0 <= B And B < 100)

            Dim HPart = H \ 60

            Throw New NotImplementedException()
        End Function

        Public Shared Sub DoNothing()

        End Sub

    End Class

End Namespace
