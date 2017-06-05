Imports Ks.Common.Controls

Namespace Common.MVVM

    Public MustInherit Class KsApplication
        Inherits BindableBase

        ' ToDo OnShuttingDown is only called when the ShutDown method is called, not when the application is shutting down.

        Public Sub New(ByVal Name As String, ByVal Application As Application)
            Me._Name = Name
            Me._Application = Application

            Me._Window = New WindowViewModel(Me)
            Me._EmptyNavigationFrame = New NavigationFrame({Me._Window})
            Me._Window.NavigationFrame = Me.EmptyNavigationFrame
            Me.CurrentNavigationFrame = Me.EmptyNavigationFrame

            Me.State = KsApplicationState.NotStarted
        End Sub

        Public Sub New()
            Verify.True(KsApplication.IsInDesignMode, "Test constructor should not be called from runtime.")

            Dim TestData = Me.OnTestConstruct()
            Me._Name = TestData.Name
            Me._Settings = TestData.Settings
        End Sub

        Protected Overridable Sub OnInitialize()
            Me._Settings = New AutoStoreDictionary(IO.Path.Combine(".", "Settings.dat"))

            Dim Dir = IO.Path.Combine(".", "Languages")
            If Not IO.Directory.Exists(Dir) Then
                IO.Directory.CreateDirectory(Dir)
            End If

            Dim Languages = New OneToOneDictionary(Of String, KsLanguage)(New Dictionary(Of String, KsLanguage)(StringComparer.InvariantCultureIgnoreCase), Function(L) L.Id)

            For Each F In IO.Directory.EnumerateFiles(Dir)
                If Not F.ToLowerInvariant().EndsWith(".csv") Then
                    Continue For
                End If
                Dim File = IO.File.Open(F, IO.FileMode.Open, IO.FileAccess.ReadWrite, IO.FileShare.Read)
                Languages.Add(New KsLanguage(File))
            Next

            If Languages.Count = 0 Then
                Dim F = IO.Path.Combine(Dir, "Default.csv")
                Dim File = IO.File.Open(F, IO.FileMode.Create, IO.FileAccess.ReadWrite, IO.FileShare.Read)
                Languages.Add(New KsLanguage(File))
            End If

            Me._Languages = Languages.AsReadOnly()

            Dim LangId As String = Nothing
            If Not Me.Settings.TryGetValue(NameOf(Me.Language), LangId) Then
                LangId = ""
            End If

            If Not Me.Languages.TryGetValue(LangId, Me._Language) Then
                Me._Language = Me.Languages.FirstOrDefault().Value
                Me.Settings.Item(NameOf(Me.Language)) = Me._Language?.Id
            End If
        End Sub

        Protected Overridable Function OnTestConstruct() As KsApplicationTestData
            Return New KsApplicationTestData() With {.Name = "", .Settings = New Dictionary(Of String, String)()}
        End Function

        Protected Overridable Sub OnStart()

        End Sub

        Protected Overridable Sub OnNavigated(ByVal OldFrame As NavigationFrame, ByVal NewFrame As NavigationFrame)

        End Sub

        Protected Overridable Sub OnShuttingDown()

        End Sub

        Protected Overridable Sub OnShutDown()
            TryCast(Me.Settings, IDisposable)?.Dispose()
            For Each L In Me.Languages.Values
                L.Dispose()
            Next
        End Sub

        <DebuggerHidden()>
        Public Sub Run()
            If Me.State <> KsApplicationState.NotStarted Then
                Throw New InvalidOperationException("Cannot run an already run KsApplication.")
            End If

            _Current = Me

            Me.State = KsApplicationState.Initializing
            Me.OnInitialize()

            Me.Application.ShutdownMode = ShutdownMode.OnMainWindowClose
            Me.Application.MainWindow = Nothing
            'Me.Application.StartupUri = Nothing

            Dim Window = DirectCast(Me.Window.View, Window)
            Window.Resources.MergedDictionaries.Add(Me.Application.Resources)

            Me.Application.Dispatcher.BeginInvoke(Sub()
                                                      Me.OnStart()
                                                      Me.State = KsApplicationState.Started
                                                  End Sub)

            Me.Application.Run(Window)

            Me.State = KsApplicationState.ShutDown
            Me.OnShutDown()
        End Sub

        Public Sub ShutDown(Optional ByVal ExitCode As Integer = 0)
            Verify.True(Me.State = KsApplicationState.Started, "The KsApplication has to be started to be able to be shut down.")

            If _Current Is Me Then
                _Current = Nothing
            End If

            Me.State = KsApplicationState.ShuttingDown
            Me.OnShuttingDown()

            Me.Application.Shutdown(ExitCode)
        End Sub

#Region "Navigation Logic"
        Protected Overridable Function CanNavigateBackToEmpty() As Boolean
            Return False
        End Function

        Protected Overridable Function OnNavigateToEmpty() As (Frame As NavigationFrame, AddToStack As Boolean, ForceToStack As Boolean)
            Return Nothing
        End Function

        Public Function GetViewModel(Of TViewModel As ViewModel)() As TViewModel
            Return DirectCast(Me.GetViewModel(GetType(TViewModel)), TViewModel)
        End Function

        Public Function GetViewModel(ByVal ViewModelType As Type) As ViewModel
            Dim Metadata = ViewModelType.GetCustomAttribute(Of ViewModelMetadataAttribute)()
            Verify.False(Metadata Is Nothing, "No metadata available.")

            If Metadata.IsSingleInstance Then
                Dim T As ViewModel = Nothing
                If Me.SingleInstanceViewModels.TryGetValue(ViewModelType, T) Then
                    Return T
                End If
            End If

            Dim ApplicationType = Me.GetType()
            Dim Constructor As Reflection.ConstructorInfo = Nothing

            For Each C In ViewModelType.GetConstructors()
                Dim Ps = C.GetParameters()
                If Ps.Length = 1 AndAlso Ps(0).ParameterType.IsAssignableFrom(ApplicationType) Then
                    Constructor = C
                    Exit For
                End If
            Next

            Verify.False(Constructor Is Nothing, "No valid constructor available. Every view-model has to have a contructor getting one single argument, the KsApplication of that WPF application.")

            Dim R = DirectCast(Constructor.Invoke({Me}), ViewModel)

            If Metadata.IsSingleInstance Then
                Me.SingleInstanceViewModels.Item(ViewModelType) = R
            End If

            Return R
        End Function

        Friend Sub NavigateTo(ByVal Parent As NavigationViewModel, ByVal ViewModel As ViewModel, ByVal AddToStack As Boolean, ByVal ForceToStack As Boolean)
            Verify.False(Parent.NavigationFrame Is Nothing, "Cannot navigate. The navigation parent is not in the view.")

            Me.NavigateTo(Parent.NavigationFrame.AddViewModel(ViewModel), AddToStack, ForceToStack)
        End Sub

        Public Sub NavigateTo(ByVal Frame As NavigationFrame, ByVal AddToStack As Boolean, ByVal ForceToStack As Boolean)
            Dim Tip = Frame.Tip

            If Tip.NavigationFrame IsNot Nothing Then
                Me.NavigationFrames.Remove(Tip.NavigationFrame)
            End If

            If ForceToStack Or (AddToStack And Not Frame.IsOpenEnded) Then
                Me.NavigationFrames.Push(Frame)
            End If

            Me.DoNavigation(Frame, NavigationType.NewNavigation)
        End Sub

        Public Sub NavigateBack()
            ' See UpdateCanNavigateBack.
            If Not Me.CanNavigateBack Then
                Exit Sub
            End If

            If Me.NavigationFrames.PeekOrDefault() = Me.CurrentNavigationFrame Then
                Me.NavigationFrames.Pop()
            End If

            If Not Me.NavigationFrames.CanPop() Then
                Dim Data = Me.OnNavigateToEmpty()
                Me.NavigateTo(Data.Frame, Data.AddToStack, Data.ForceToStack)
                Exit Sub
            End If

            Me.DoNavigation(Me.NavigationFrames.Peek(), NavigationType.BackNavigation)
        End Sub

        Private Sub DoNavigation(ByVal NewFrame As NavigationFrame, ByVal NavigationType As NavigationType)
            Dim OldFrame = Me.CurrentNavigationFrame
            Me.CurrentNavigationFrame = NewFrame

            Me.UpdateCanNavigateBack()

            Dim I = 0
            For I = I To Math.Min(NewFrame.Count, OldFrame.Count) - 1
                If NewFrame.Item(I) IsNot OldFrame.Item(I) Then
                    Exit For
                End If
            Next

            Dim NavigationEventArgs = New NavigationEventArgs(NavigationType)

            For J = OldFrame.Count - 1 To I Step -1
                Dim VM = OldFrame.Item(J)

                VM.NavigationFrame = Nothing
                If VM.IsNavigation() Then
                    VM.SetView(Nothing)
                End If

                VM.OnNavigatedFrom(NavigationEventArgs)
            Next

            For J = I To NewFrame.Count
                Dim Parent = TryCast(NewFrame.Item(J - 1), NavigationViewModel)
                Dim Current = If(J <> NewFrame.Count, NewFrame.Item(J), Nothing)

                If Current IsNot Nothing Then
                    Current.NavigationFrame = NewFrame.SubFrame(J + 1)
                End If

                Parent?.SetView(Current)
                Current?.OnNavigatedTo(NavigationEventArgs)
            Next

            Me.OnNavigated(OldFrame, NewFrame)
        End Sub

        Private Sub UpdateCanNavigateBack()
            Dim Count = Me.NavigationFrames.Count

            ' If the top of stack is not the current frame, we can go back from the current frame to it.
            Count += If(Me.NavigationFrames.PeekOrDefault() = Me.CurrentNavigationFrame, 0, 1)

            Select Case Count
                Case 0
                    Me.CanNavigateBack = False
                Case 1
                    Me.CanNavigateBack = Me.CanNavigateBackToEmpty()
                Case Else
                    Me.CanNavigateBack = True
            End Select
        End Sub

#Region "NavigateBack Command"
        Private _NavigateBackCommand As DelegateCommand = New DelegateCommand(AddressOf Me.NavigateBack)

        Public ReadOnly Property NavigateBackCommand As DelegateCommand
            Get
                Return Me._NavigateBackCommand
            End Get
        End Property
#End Region

#Region "CanNavigateBack Property"
        Private _CanNavigateBack As Boolean

        Public Property CanNavigateBack As Boolean
            Get
                Return Me._CanNavigateBack
            End Get
            Set(ByVal Value As Boolean)
                Me.SetProperty(Me._CanNavigateBack, Value)
            End Set
        End Property
#End Region

#Region "DefaultNavigationView Property"
        Public Overridable ReadOnly Property DefaultNavigationView As NavigationViewModel
            Get
                Return Me._Window
            End Get
        End Property
#End Region

#Region "CurrentNavigationFrame Property"
        Private _CurrentNavigationFrame As NavigationFrame

        Public Property CurrentNavigationFrame As NavigationFrame
            Get
                Return Me._CurrentNavigationFrame
            End Get
            Private Set(ByVal Value As NavigationFrame)
                Me.SetProperty(Me._CurrentNavigationFrame, Value)
            End Set
        End Property
#End Region

#Region "NavigationFrames Read-Only Property"
        Private ReadOnly _NavigationFrames As PushPopList(Of NavigationFrame) = New PushPopList(Of NavigationFrame)()

        Public ReadOnly Property NavigationFrames As PushPopList(Of NavigationFrame)
            Get
                Return Me._NavigationFrames
            End Get
        End Property
#End Region

#Region "EmptyNavigationFrame Read-Only Property"
        Private ReadOnly _EmptyNavigationFrame As NavigationFrame

        Public ReadOnly Property EmptyNavigationFrame As NavigationFrame
            Get
                Return Me._EmptyNavigationFrame
            End Get
        End Property
#End Region

        Private ReadOnly SingleInstanceViewModels As Dictionary(Of Type, ViewModel) = New Dictionary(Of Type, ViewModel)()
#End Region

#Region "IsInDesignMode Shared Read-Only Property"
        Public Shared ReadOnly Property IsInDesignMode As Boolean
            Get
#If SimulateDesign Then
                Return True
#Else
                Static DepObj As DependencyObject = New DependencyObject()
                Return ComponentModel.DesignerProperties.GetIsInDesignMode(DepObj)
#End If
            End Get
        End Property
#End Region

#Region "Name Property"
        Private ReadOnly _Name As String

        Public ReadOnly Property Name As String
            Get
                Return Me._Name
            End Get
        End Property
#End Region

#Region "Version Property"
        Private ReadOnly _Version As String

        Public ReadOnly Property Version As String
            Get
                Return Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()
            End Get
        End Property
#End Region

#Region "Window Property"
        Private ReadOnly _Window As NavigationViewModel

        Public ReadOnly Property Window As NavigationViewModel
            Get
                Return Me._Window
            End Get
        End Property
#End Region

#Region "HomePageType Property"
        Private ReadOnly _HomePageType As Type

        Public ReadOnly Property HomePageType As Type
            Get
                Return Me._HomePageType
            End Get
        End Property
#End Region

#Region "State Property"
        Private _State As KsApplicationState

        Public Property State As KsApplicationState
            Get
                Return Me._State
            End Get
            Private Set(ByVal Value As KsApplicationState)
                Me.SetProperty(Me._State, Value)
            End Set
        End Property
#End Region

#Region "Languages Property"
        Private _Languages As IReadOnlyDictionary(Of String, KsLanguage)

        Public ReadOnly Property Languages As IReadOnlyDictionary(Of String, KsLanguage)
            Get
                Return Me._Languages
            End Get
        End Property
#End Region

#Region "Language Property"
        Private _Language As KsLanguage

        Public Property Language As KsLanguage
            Get
                Return Me._Language
            End Get
            Set(ByVal Value As KsLanguage)
                If Me.SetProperty(Me._Language, Value) Then
                    Me.Settings.Item(NameOf(Me.Language)) = Value.Id
                    ' ToDo A very bad workaround for updating the title of the window.
                    Me.NotifyPropertyChanged(NameOf(Me.Name))
                End If
            End Set
        End Property
#End Region

#Region "Settings Property"
        Private _Settings As IDictionary(Of String, String)

        Public ReadOnly Property Settings As IDictionary(Of String, String)
            Get
                Return Me._Settings
            End Get
        End Property
#End Region

#Region "Application Property"
        Private ReadOnly _Application As Application

        Public ReadOnly Property Application As Application
            Get
                Return Me._Application
            End Get
        End Property
#End Region

#Region "Current Shared Property"
        Private Shared _Current As KsApplication

        Public Shared ReadOnly Property Current As KsApplication
            Get
                Return _Current
            End Get
        End Property
#End Region

    End Class

    Public Enum KsApplicationState

        NotStarted
        Initializing
        Started
        ShuttingDown
        ShutDown

    End Enum

    Public Structure KsApplicationTestData

#Region "Name Property"
        Private _Name As String

        Public Property Name As String
            Get
                Return Me._Name
            End Get
            Set(ByVal Value As String)
                Me._Name = Value
            End Set
        End Property
#End Region

#Region "Settings Property"
        Private _Settings As IDictionary(Of String, String)

        Public Property Settings As IDictionary(Of String, String)
            Get
                Return Me._Settings
            End Get
            Set(ByVal Value As IDictionary(Of String, String))
                Me._Settings = Value
            End Set
        End Property
#End Region

    End Structure

End Namespace
