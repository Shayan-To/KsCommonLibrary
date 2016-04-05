Imports Ks.Common.Controls

Namespace MVVM

    Public MustInherit Class KsApplication
        Inherits BindableBase

        Public Sub New(ByVal Name As String, ByVal Application As Application)
            Me._Name = Name
            Me._Window = New WindowViewModel(Me)
            Me._Application = Application
            Me.State = KsApplicationState.NotStarted
        End Sub

        Public Sub New()
            If Not IsInDesignMode Then
                Throw New InvalidOperationException("Should not call the test constructor from runtime.")
            End If

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

            Dim Languages = New OneToOneDictionary(Of String, KsLanguage)(Function(L) L.Id)

            For Each F In IO.Directory.EnumerateFiles(Dir)
                If Not F.EndsWith(".csv") Then
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
            Me._Language = Me.Languages.Item(LangId)
        End Sub

        Protected Overridable Function OnTestConstruct() As KsApplicationTestData
            Return New KsApplicationTestData() With {.Name = "", .Settings = New Dictionary(Of String, String)()}
        End Function

        Protected Overridable Sub OnStart()

        End Sub

        Protected Overridable Sub OnNavigated(ByVal Parent As NavigationViewModel, ByVal ViewModel As ViewModel)

        End Sub

        Protected Overridable Sub OnShuttingDown()
            TryCast(Me.Settings, IDisposable)?.Dispose()
        End Sub

        Protected Overridable Sub OnShutDown()

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
            Window.Resources.MergedDictionaries.Add(Application.Resources)

            Me.Application.Dispatcher.BeginInvoke(Sub()
                                                      Me.OnStart()
                                                      Me.State = KsApplicationState.Started
                                                  End Sub)

            Me.Application.Run(Window)

            Me.State = KsApplicationState.ShutDown
        End Sub

        Public Sub ShutDown(Optional ByVal ExitCode As Integer = 0)
            If Me.State <> KsApplicationState.Started Then
                Throw New InvalidOperationException("The KsApplication has to be started to be able to be shut down.")
            End If

            If _Current Is Me Then
                _Current = Nothing
            End If

            Me.State = KsApplicationState.ShuttingDown
            Me.OnShuttingDown()

            Me.Application.Shutdown(ExitCode)
        End Sub

#Region "Navigation Logic"
        Protected Overridable Function CanNavigateToEmpty(ByVal ViewModel As ViewModel) As Boolean
            Return False
        End Function

        Protected Overridable Sub OnNavigateToEmpty()

        End Sub

        Public Function GetViewModel(Of TViewModel As ViewModel)() As TViewModel
            Return DirectCast(Me.GetViewModel(GetType(TViewModel)), TViewModel)
        End Function

        Public Function GetViewModel(ByVal ViewModelType As Type) As ViewModel
            Dim Metadata = ViewModelType.GetCustomAttribute(Of ViewModelMetadataAttribute)()
            If Metadata Is Nothing Then
                Throw New InvalidOperationException("No metadata available.")
            End If

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

            If Constructor Is Nothing Then
                Throw New InvalidOperationException("No valid constructor available. Every viewmodel has to have a contructor getting one single argument, the KsApplication of that WPF project.")
            End If

            Dim R = DirectCast(Constructor.Invoke({Me}), ViewModel)

            If Metadata.IsSingleInstance Then
                Me.SingleInstanceViewModels.Item(ViewModelType) = R
            End If

            Return R
        End Function

        Private Shared Sub SetViewOnContent(ByVal NavigationView As INavigationView, ByVal View As Page)
            Dim Prev = NavigationView.Content
            If Prev Is View Then
                Exit Sub
            End If

            If Prev IsNot Nothing Then
                Prev.ParentView = Nothing
            End If

            If View IsNot Nothing Then
                If View.ParentView IsNot Nothing Then
                    View.ParentView.Content = Nothing
                End If
                View.ParentView = NavigationView
            End If

            NavigationView.Content = View
        End Sub

        Private Function CreateFrame(Navigation As NavigationViewModel, Navigated As ViewModel) As NavigationFrame
            Dim Frame = New List(Of ViewModel)()

            Frame.Add(Navigated)

            Dim V As ViewModel = Navigation
            Do
                Frame.Add(V)
                If V.NavigationFrame.Count < 2 Then
                    Debug.Assert(V.NavigationFrame.Count = 1)
                    Debug.Assert(V Is Me.Window)
                    Exit Do
                End If
                V = V.NavigationFrame.Item(1)
            Loop

            Return New NavigationFrame(Frame)
        End Function

        Friend Sub NavigateTo(ByVal Navigation As NavigationViewModel, ByVal Navigated As ViewModel, ByVal AddToStack As Boolean, ByVal ForceToStack As Boolean)
            If Navigated.NavigationFrame IsNot Nothing Then
                Me.NavigationFrames.Remove(Navigated.NavigationFrame)
            End If

            Navigated.NavigationFrame = CreateFrame(Navigation, Navigated)

            If ForceToStack Or (AddToStack And Not GetType(NavigationViewModel).IsAssignableFrom(Navigated.GetType())) Then
                Me.NavigationFrames.Push(Navigated.NavigationFrame)
            End If

            Me.CurrentNavigationFrame = Navigated.NavigationFrame
            Me.DoNavigation()
            Me.UpdateCanNavigateBack()

            Me.OnNavigated(Navigation, Navigated)
        End Sub

#Region "NavigateBack Command"
        Private _NavigateBackCommand As DelegateCommand = New DelegateCommand(AddressOf Me.NavigateBack)

        Public Sub NavigateBack()
            If Me.NavigationFrames.Count <> 0 AndAlso Me.NavigationFrames.Peek() = CurrentNavigationFrame Then
                If Me.NavigationFrames.Count = 1 Then
                    If Me.CanNavigateToEmpty(Me.CurrentNavigationFrame.Item(0)) Then
                        Me.NavigationFrames.Pop()
                    End If
                Else
                    Me.NavigationFrames.Pop()
                End If
            End If

            If Me.NavigationFrames.Count = 0 Then
                Me.CurrentNavigationFrame = Nothing
                Me.OnNavigateToEmpty()
                Me.UpdateCanNavigateBack()
                Exit Sub
            End If

            Me.CurrentNavigationFrame = Me.NavigationFrames.Peek()
            Me.DoNavigation()
            Me.UpdateCanNavigateBack()

            Me.OnNavigated(DirectCast(Me.CurrentNavigationFrame.Item(1), NavigationViewModel),
                            Me.CurrentNavigationFrame.Item(0))
        End Sub

        Public ReadOnly Property NavigateBackCommand As DelegateCommand
            Get
                Return Me._NavigateBackCommand
            End Get
        End Property
#End Region

        Private Sub UpdateCanNavigateBack()
            If Me.NavigationFrames.Count > 1 Then
                Me.CanNavigateBack = True
                Exit Sub
            End If
            If Me.NavigationFrames.Count = 0 Then
                Me.CanNavigateBack = False
                Exit Sub
            End If
            Me.CanNavigateBack = Me.CanNavigateToEmpty(Me.CurrentNavigationFrame.Item(0))
        End Sub

        Private Sub DoNavigation()
            Dim Frame = Me.CurrentNavigationFrame
            Dim Cur = Frame.Item(Frame.Count - 1)
            For I As Integer = Frame.Count - 2 To 0 Step -1
                Dim Prev = DirectCast(Cur, NavigationViewModel)
                Cur = Frame.Item(I)
                SetViewOnContent(Prev.NavigationView, DirectCast(Cur.View, Page))
            Next

            Dim CurN = TryCast(Cur, NavigationViewModel)
            If CurN IsNot Nothing Then
                SetViewOnContent(CurN.NavigationView, Nothing)
            End If
        End Sub

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

#Region "DefaultNavigationViewModel Property"
        Public Overridable ReadOnly Property DefaultNavigationView As NavigationViewModel
            Get
                Return Me._Window
            End Get
        End Property
#End Region
#End Region

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

#Region "Name Property"
        Private ReadOnly _Name As String

        Public ReadOnly Property Name As String
            Get
                Return Me._Name
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
                Me._State = Value
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

        Private ReadOnly SingleInstanceViewModels As Dictionary(Of Type, ViewModel) = New Dictionary(Of Type, ViewModel)()
        Private ReadOnly NavigationFrames As PushPopList(Of NavigationFrame) = New PushPopList(Of NavigationFrame)()
        Private CurrentNavigationFrame As NavigationFrame = Nothing

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
