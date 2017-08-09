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
            Dim FullNameSelector = Function(M As MethodInfo) $"{M.DeclaringType.FullName}.{M.Name}"
            Dim List = Methods.WithCustomAttribute(Of InteractiveRunnableAttribute)() _
                    .Where(Function(MA)
                               Dim M = MA.Method

                               If Not M.IsStatic Then
                                   ConsoleUtilities.WriteColored($"Skipping {FullNameSelector.Invoke(M)}... Instance method.", ConsoleColor.Red)
                                   Console.WriteLine()
                                   Return False
                               End If

                               If M.GetParameters().Length <> 0 Then
                                   ConsoleUtilities.WriteColored($"Skipping {FullNameSelector.Invoke(M)}... Accepts arguments.", ConsoleColor.Red)
                                   Console.WriteLine()
                                   Return False
                               End If

                               Return JustTrue.Implies(MA.Attribute.ShouldBeRun)
                           End Function) _
                    .Select(Function(MA) MA.Method)
            Dim ChoiceReader = New ConsoleListChoiceReader(Of MethodInfo)(List, FullNameSelector)

            Do
                Dim M = ChoiceReader.ReadChoice()
                If M Is Nothing Then
                    Exit Do
                End If
                M.Invoke(Nothing, Utilities.Typed(Of Object).EmptyArray)
            Loop

            Console.WriteLine()
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
