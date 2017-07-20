Imports System.Reflection

Namespace Common

    <AttributeUsage(AttributeTargets.Method)>
    Public Class InteractiveRunnableAttribute
        Inherits Attribute

        Public Sub New(Optional ByVal ShouldBeRun As Boolean = False)
            Me._ShouldBeRun = ShouldBeRun
        End Sub

        <DebuggerHidden()>
        Public Shared Sub RunTestMethods(ByVal Methods As IEnumerable(Of MethodInfo), ByVal Optional JustTrue As Boolean = True)
            For Each MA In Methods.WithCustomAttribute(Of InteractiveRunnableAttribute)()
                Dim M = MA.Method
                Dim Attribute = MA.Attribute

                Dim FullMethodName = $"{M.DeclaringType.FullName}.{M.Name}"

                If Not M.IsStatic Then
                    ConsoleUtilities.WriteColored($"Skipping {FullMethodName}... Instance method.", ConsoleColor.Red)
                    Console.ReadKey(True)
                    Console.WriteLine()

                    Continue For
                End If

                If M.GetParameters().Length <> 0 Then
                    ConsoleUtilities.WriteColored($"Skipping {FullMethodName}... Accepts arguments.", ConsoleColor.Red)
                    Console.ReadKey(True)
                    Console.WriteLine()

                    Continue For
                End If

                If JustTrue.Implies(Attribute.ShouldBeRun) Then
                    If ConsoleUtilities.ReadYesNo($"Run {FullMethodName}? (Y/N)") Then
                        Do
                            M.Invoke(Nothing, Utilities.Typed(Of Object).EmptyArray)
                        Loop While ConsoleUtilities.ReadYesNo("Rerun? (Y/N)")
                    End If
                End If
            Next

            ConsoleUtilities.WriteColored("Done.", ConsoleColor.Green)
            Console.WriteLine()
        End Sub

        <Sample()>
        Private Shared Sub CecilRunTestMethods(Optional ByVal JustTrue As Boolean = True)
            Dim Helper = CecilHelper.Instance
            Dim AttributeType = Helper.Convert(GetType(InteractiveRunnableAttribute))
            For Each A In Helper.GetReferencedAssemblies(Helper.Convert(Reflection.Assembly.GetEntryAssembly()))
                For Each M In A.Modules
                    For Each T In M.Types
                        For Each Mth In T.Methods
                            For Each CA In Mth.CustomAttributes.Where(Function(Att) Helper.Equals(Att.AttributeType.Resolve(), AttributeType))
                                If Mth.Parameters.Count <> 0 Then
                                    ConsoleUtilities.WriteColored($"Skipping {T.FullName}.{M.Name}... Accepts arguments.", ConsoleColor.Red)
                                    Console.ReadKey(True)
                                    Console.WriteLine()

                                    Continue For
                                End If

                                Dim Att = New InteractiveRunnableAttribute(DirectCast(CA.ConstructorArguments.Item(0).Value, Boolean))
                                If Not JustTrue Or Att.ShouldBeRun Then
                                    If ConsoleUtilities.ReadYesNo($"Run {T.FullName}.{M.Name}? (Y/N)") Then
                                        Helper.Convert(Mth).Invoke(Nothing, Utilities.Typed(Of Object).EmptyArray)
                                    End If
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

    End Class

End Namespace
