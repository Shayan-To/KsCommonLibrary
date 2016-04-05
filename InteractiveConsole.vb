Imports System.Reflection

Public MustInherit Class InteractiveConsole

    Public Sub Run()
        'AddHandler Console.CancelKeyPress, AddressOf Me.Console_CancelKeyPress
        Console.TreatControlCAsInput = True
        Me.OnLoad()
        Do
            Me.OnKeyPressed(Console.ReadKey(True))
        Loop
    End Sub

    'Private Sub Console_CancelKeyPress(ByVal S As Object, ByVal E As ConsoleCancelEventArgs)
    '    E.Cancel = True
    '    If E.SpecialKey = ConsoleSpecialKey.ControlC Then
    '        Me.OnCombinationalKeyPressed(New ConsoleKeyInfo(New Char(), ConsoleKey.C, False, False, True))
    '    ElseIf E.SpecialKey = ConsoleSpecialKey.ControlBreak Then
    '        Me.OnCombinationalKeyPressed(New ConsoleKeyInfo(New Char(), ConsoleKey.Pause, False, False, True))
    '    Else
    '        Me.OnCombinationalKeyPressed(New ConsoleKeyInfo(New Char(), 0, False, False, True))
    '    End If
    'End Sub

    Protected Overridable Sub OnLoad()

    End Sub

    Protected Overridable Sub OnKeyPressed(ByVal KeyInfo As ConsoleKeyInfo)
        If KeyInfo.Modifiers <> 0 Then
            Me.OnCombinationalKeyPressed(KeyInfo)
            Exit Sub
        End If
        Select Case KeyInfo.Key
            Case ConsoleKey.Tab
                Me.OnTabKeyPressed()
            Case ConsoleKey.Enter
                Me.OnEnterKeyPressed()
            Case Else
                If KeyInfo.KeyChar <> Nothing Then
                    Me.OnCharacterKeyPressed(KeyInfo)
                Else
                    Me.OnOtherKeyPressed(KeyInfo)
                End If
        End Select
    End Sub

    Protected Overridable Sub OnEnterKeyPressed()

    End Sub

    Protected Overridable Sub OnTabKeyPressed()

    End Sub

    Protected Overridable Sub OnCombinationalKeyPressed(ByVal KeyInfo As ConsoleKeyInfo)

    End Sub

    Protected Overridable Sub OnCharacterKeyPressed(ByVal KeyInfo As ConsoleKeyInfo)

    End Sub

    Protected Overridable Sub OnOtherKeyPressed(ByVal KeyInfo As ConsoleKeyInfo)

    End Sub

End Class

Public Class Scripter
    Inherits InteractiveConsole

    Protected Overrides Sub OnLoad()
        Assembly.GetEntryAssembly()
    End Sub

    Private Sub AddAssemblyContainers(ByVal AssemblyInfo As Assembly)
        If Not Me.Assemblies.Add(AssemblyInfo) Then
            Exit Sub
        End If

        For Each T In AssemblyInfo.GetTypes()
            Me.AddTypeContainers(New ComparableCollection(Of String)(T.FullName.Split("."c, "+"c)), T)
        Next

        For Each A In AssemblyInfo.GetReferencedAssemblies()
            Me.AddAssemblyContainers(Assembly.Load(A))
        Next
    End Sub

    Private Function AddTypeContainers(ByVal Path As ComparableCollection(Of String),
                                       ByVal TypeInfo As Type) As Container
        Dim C As Container = Nothing
        If Me.Containers.TryGetValue(Path, C) Then
            Debug.Assert(C.TypeInfo Is Nothing Or TypeInfo Is Nothing)
            If C.TypeInfo Is Nothing Then
                C.TypeInfo = TypeInfo
            End If
            Return C
        End If

        Dim PathC = Path.Clone()
        C = Me.Containers.Item(PathC)

        C.Path = PathC
        C.TypeInfo = TypeInfo

        Path.RemoveAt(Path.Count - 1)
        C.Parent = Me.AddTypeContainers(Path, Nothing)
        C.Parent.Children.Add(C.Name, C)

        If Me.Names.ContainsKey(C.Name) Then
            Me.Names.Item(C.Name) = Container.Ambigues
        Else
            Me.Names.Item(C.Name) = C
        End If

        Return C
    End Function

    Protected Overrides Sub OnEnterKeyPressed()

    End Sub

    Protected Overrides Sub OnTabKeyPressed()

    End Sub

    Protected Overrides Sub OnCharacterKeyPressed(ByVal KeyInfo As System.ConsoleKeyInfo)

    End Sub

    Private ReadOnly Objects As Dictionary(Of String, Object) = New Dictionary(Of String, Object)()
    Private ReadOnly Input As Text.StringBuilder = New Text.StringBuilder()
    Private ReadOnly Names As SortedDictionary(Of String, Container) = New SortedDictionary(Of String, Container)()
    Private ReadOnly Containers As CreateInstanceDictionary(Of ComparableCollection(Of String), Container) = CreateInstanceDictionary.Create(New SortedDictionary(Of ComparableCollection(Of String), Container)())
    Private ReadOnly Assemblies As HashSet(Of Assembly) = New HashSet(Of Assembly)()

    Public Class Container

        Public ReadOnly Property Name As String
            Get
                Return Me.Path.Item(Me.Path.Count - 1)
            End Get
        End Property

        Public Property Path As ComparableCollection(Of String)
        Public Property TypeInfo As Type
        Public Property Children As SortedDictionary(Of String, Container) = New SortedDictionary(Of String, Container)()
        Public Property Parent As Container

        Public Shared ReadOnly Ambigues As Container = New Container()

    End Class

End Class
