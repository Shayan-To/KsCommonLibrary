Namespace Common

    Public Class CecilHelper

        Public Function IsBaseTypeOf(ByVal Base As Cecil.TypeDefinition, Derived As Cecil.TypeDefinition) As Boolean
            If Base.IsInterface Then
                For Each I In Derived.Interfaces
                    Dim ID = I.Resolve()
                    If Me.Equals(Base, ID) Then
                        Return True
                    End If
                Next
            Else
                Do Until Derived Is Nothing
                    If Me.Equals(Base, Derived) Then
                        Return True
                    End If
                    Base = Derived.BaseType?.Resolve()
                Loop
            End If
            Return False
        End Function

        Public Overloads Function Equals(ByVal Type1 As Cecil.TypeDefinition, ByVal Type2 As Cecil.TypeDefinition) As Boolean
            Return Type1.FullName = Type2.FullName And Type1.Module.FullyQualifiedName = Type2.Module.FullyQualifiedName
        End Function

        Public Function Convert(ByVal Method As Cecil.MethodDefinition) As Reflection.MethodInfo
            Dim Type = Me.Convert(Method.DeclaringType)
            Return Type.GetMethod(Method.Name, Method.Parameters.Select(Function(P) Me.Convert(P.ParameterType.Resolve())).ToArray())
        End Function

        Public Function Convert(ByVal Type As Cecil.TypeDefinition) As Type
            Dim Assembly = Me.Convert(Type.Module.Assembly)
            Return Assembly.GetType(Type.FullName)
        End Function

        Public Function Convert(ByVal Assembly As Cecil.AssemblyDefinition) As Reflection.Assembly
            Return Reflection.Assembly.Load(New Reflection.AssemblyName(Assembly.FullName))
        End Function

        Public Function Convert(ByVal AssemblyName As Cecil.AssemblyNameReference) As Reflection.Assembly
            Return Reflection.Assembly.Load(New Reflection.AssemblyName(AssemblyName.FullName))
        End Function

        Public Function Convert(ByVal Type As Type) As Cecil.TypeDefinition
            Dim FName = Type.FullName
            For Each M In Me.Convert(Type.Assembly).Modules
                Dim T = M.GetType(FName)
                If T IsNot Nothing Then
                    Return T
                End If
            Next
            Throw New Exception()
        End Function

        Public Function Convert(ByVal Assembly As Reflection.Assembly) As Cecil.AssemblyDefinition
            Return Cecil.AssemblyDefinition.ReadAssembly(Assembly.Location)
        End Function

        Public Function GetReferencedAssemblyNames(ByVal Assembly As Cecil.AssemblyDefinition) As Cecil.AssemblyNameReference()
            Return Me.GetRawReferencedAssemblyNames(Assembly).ToArray()
        End Function

        Friend Function GetRawReferencedAssemblyNames(ByVal Assembly As Cecil.AssemblyDefinition) As IEnumerable(Of Cecil.AssemblyNameReference)
            Me.AssemblyNames.Clear()

            For Each M In Assembly.Modules
                For Each A In M.AssemblyReferences
                    Me.AssemblyNames.Add(A)
                Next
            Next

            Return Me.AssemblyNames
        End Function

        Public Function GetReferencedAssemblies(ByVal Assembly As Cecil.AssemblyDefinition) As Cecil.AssemblyDefinition()
            Return Me.GetRawReferencedAssemblies(Assembly).ToArray()
        End Function

        Friend Function GetRawReferencedAssemblies(ByVal Assembly As Cecil.AssemblyDefinition) As IEnumerable(Of Cecil.AssemblyDefinition)
            Me.Assemblies.Clear()

            For Each M In Assembly.Modules
                For Each A In M.AssemblyReferences
                    Me.Assemblies.Add(Me.Resolver.Resolve(A))
                Next
            Next

            Return Me.Assemblies
        End Function

        Public Function GetRecursiveReferencedAssemblies(ByVal Assembly As Cecil.AssemblyDefinition) As Cecil.AssemblyDefinition()
            Return Me.GetRawRecursiveReferencedAssemblies(Assembly).ToArray()
        End Function

        Friend Function GetRawRecursiveReferencedAssemblies(ByVal Assembly As Cecil.AssemblyDefinition) As IEnumerable(Of Cecil.AssemblyDefinition)
            Me.Assemblies.Clear()
            Me.CollectReferences(Assembly)
            Return Me.Assemblies
        End Function

        Private Sub CollectReferences(ByVal Assembly As Cecil.AssemblyDefinition)
            If Not Me.Assemblies.Add(Assembly) Then
                Exit Sub
            End If
            For Each M In Assembly.Modules
                For Each A In M.AssemblyReferences
                    Me.CollectReferences(Me.Resolver.Resolve(A))
                Next
            Next
        End Sub

#Region "Instance Property"
        Private Shared ReadOnly _Instance As CecilHelper = New CecilHelper()

        Public Shared ReadOnly Property Instance As CecilHelper
            Get
                Return _Instance
            End Get
        End Property
#End Region

        Private ReadOnly Resolver As Cecil.DefaultAssemblyResolver = New Cecil.DefaultAssemblyResolver()
        Private ReadOnly Assemblies As HashSet(Of Cecil.AssemblyDefinition) = New HashSet(Of Cecil.AssemblyDefinition)()
        Private ReadOnly AssemblyNames As HashSet(Of Cecil.AssemblyNameReference) = New HashSet(Of Cecil.AssemblyNameReference)()

    End Class

End Namespace
