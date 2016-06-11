Imports System.Reflection

<AttributeUsage(AttributeTargets.Method)>
Public Class ConsoleTestMethodAttribute
    Inherits Attribute

    Public Sub New(Optional ByVal ShouldBeRun As Boolean = False)
        Me._ShouldBeRun = ShouldBeRun
    End Sub

    <DebuggerHidden()>
    Public Shared Sub RunTestMethods(ByVal Methods As IEnumerable(Of MethodInfo), ByVal Optional JustTrue As Boolean = True)
        ConsoleUtilities.Initialize()

        For Each MA In Methods.WithCustomAttribute(Of ConsoleTestMethodAttribute)()
            Dim M = MA.Item1
            Dim Attribute = MA.Item2

            Dim FullMethodName = String.Concat(M.DeclaringType.FullName, ".", M.Name)

            If (M.Attributes And MethodAttributes.Static) = MethodAttributes.Static Then
                'Console.WriteLine(String.Concat(T.FullName, ".", M.Name))
                'Console.WriteLine(HMA)
                'Console.WriteLine(HMA.ShouldBeRun)
                'Console.WriteLine(M.GetParameters().Length)

                If Attribute.CheckVisibility And
                   ((M.Attributes And MethodAttributes.Public) = MethodAttributes.Public Or
                    (M.Attributes And MethodAttributes.Family) = MethodAttributes.Family Or
                    (M.Attributes And MethodAttributes.FamORAssem) = MethodAttributes.FamORAssem) Then
                    Console.BackgroundColor = ConsoleColor.Yellow
                    Console.Write(String.Concat("Warning, ", FullMethodName, " is visible."))
                    Console.BackgroundColor = ConsoleColor.White
                    Console.WriteLine()
                End If

                If M.GetParameters().Length = 0 Then
                    ' JustTrue => Attribute.ShouldBeRun   ===   ~JustTrue | Attribute.ShouldBeRun
                    If Not JustTrue Or Attribute.ShouldBeRun Then
                        Console.BackgroundColor = ConsoleColor.Green
                        Console.Write(String.Concat("Run ", FullMethodName, "? (Y/N)"))
                        Console.BackgroundColor = ConsoleColor.White

                        Dim K As ConsoleKey
                        Do
                            K = Console.ReadKey(True).Key
                        Loop Until K = ConsoleKey.Y Or K = ConsoleKey.N

                        Console.WriteLine(If(K = ConsoleKey.Y, " Y", " N"))

                        If K = ConsoleKey.Y Then
                            Do
                                M.Invoke(Nothing, Utilities.Typed(Of Object).EmptyArray)

                                Console.BackgroundColor = ConsoleColor.Green
                                Console.Write(String.Concat("Rerun? (Y/N)"))
                                Console.BackgroundColor = ConsoleColor.White

                                Do
                                    K = Console.ReadKey(True).Key
                                Loop Until K = ConsoleKey.Y Or K = ConsoleKey.N

                                Console.WriteLine(If(K = ConsoleKey.Y, " Y", " N"))
                            Loop While K = ConsoleKey.Y
                        End If
                    End If
                Else
                    Console.BackgroundColor = ConsoleColor.Red
                    Console.Write(String.Concat("Skipping ", FullMethodName, "... Accepts arguments."))
                    Console.BackgroundColor = ConsoleColor.White
                    Console.ReadKey(True)
                    Console.WriteLine()
                End If
            Else ' If M is Instance
                Console.BackgroundColor = ConsoleColor.Red
                Console.Write(String.Concat("Skipping ", FullMethodName, "... Instance method."))
                Console.BackgroundColor = ConsoleColor.White
                Console.ReadKey(True)
                Console.WriteLine()
            End If
        Next
        Console.BackgroundColor = ConsoleColor.Green
        Console.Write("Done.")
        Console.BackgroundColor = ConsoleColor.White
        Console.WriteLine()
    End Sub

    <Sample()>
    Private Shared Sub CecilRunTestMethods(Optional ByVal JustTrue As Boolean = True)
        Dim Helper = CecilHelper.Instance
        Dim AttributeType = Helper.Convert(GetType(ConsoleTestMethodAttribute))
        For Each A In Helper.GetReferencedAssemblies(Helper.Convert(Reflection.Assembly.GetEntryAssembly()))
            For Each M In A.Modules
                For Each T In M.Types
                    For Each Mth In T.Methods
                        For Each CA In Mth.CustomAttributes.Where(Function(Att) Helper.Equals(Att.AttributeType.Resolve(), AttributeType))
                            If Mth.Parameters.Count = 0 Then
                                Dim Att = New ConsoleTestMethodAttribute(DirectCast(CA.ConstructorArguments.Item(0).Value, Boolean))
                                If Not JustTrue Or Att.ShouldBeRun Then
                                    Console.BackgroundColor = ConsoleColor.Green
                                    Console.Write(String.Concat("Run ", T.FullName, ".", M.Name, "? (Y/N)"))
                                    Console.BackgroundColor = ConsoleColor.White

                                    Dim K As ConsoleKey
                                    Do
                                        K = Console.ReadKey(True).Key
                                    Loop Until K = ConsoleKey.Y Or K = ConsoleKey.N

                                    Console.WriteLine(If(K = ConsoleKey.Y, " Y", " N"))

                                    If K = ConsoleKey.Y Then
                                        Helper.Convert(Mth).Invoke(Nothing, Utilities.Typed(Of Object).EmptyArray)
                                    End If
                                End If
                            Else
                                Console.BackgroundColor = ConsoleColor.Red
                                Console.Write(String.Concat("Skipping ", T.FullName, ".", M.Name, "... Accepts arguments."))
                                Console.BackgroundColor = ConsoleColor.White
                                Console.ReadKey(True)
                                Console.WriteLine()
                            End If
                        Next
                    Next
                Next
            Next
        Next
    End Sub

    <DebuggerHidden()>
    Public Shared Sub RunTestMethods(ByVal Optional JustTrue As Boolean = True)
        RunTestMethods({Assembly.GetEntryAssembly()}, JustTrue)
    End Sub

    <DebuggerHidden()>
    Public Shared Sub RunTestMethods(ByVal Assemblies As IEnumerable(Of Assembly), ByVal Optional JustTrue As Boolean = True)
        RunTestMethods(Utilities.Reflection.GetAllMethods(Assemblies), JustTrue)
    End Sub

#Region "ShouldBeRun Property"
    Private ReadOnly _ShouldBeRun As Boolean

    Public ReadOnly Property ShouldBeRun As Boolean
        Get
            Return Me._ShouldBeRun
        End Get
    End Property
#End Region

#Region "CheckVisibility Property"
    Private _CheckVisibility As Boolean = True

    Public Property CheckVisibility As Boolean
        Get
            Return Me._CheckVisibility
        End Get
        Set(ByVal Value As Boolean)
            Me._CheckVisibility = Value
        End Set
    End Property
#End Region

End Class
