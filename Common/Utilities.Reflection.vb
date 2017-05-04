Imports System.Runtime.CompilerServices
Imports System.Text
Imports Media = System.Windows.Media
Imports Reflect = System.Reflection
Imports SIO = System.IO

Namespace Common

    Partial Class Utilities

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

    End Class

End Namespace
