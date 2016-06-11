'Imports System.ComponentModel
'Imports System.Security
'Imports System.Windows.Automation.Peers
'Imports System.Windows.Input
'Imports System.Windows.Markup
'Imports System.Windows.Media
'Imports System.Windows.Shell

'Namespace Controls

'    <ContentProperty("Content")>
'    Public Class WindowBase
'        Inherits Control

'        'Public Shared ReadOnly AllowsTransparencyProperty As DependencyProperty
'        'Public Shared ReadOnly IconProperty As DependencyProperty
'        'Public Shared ReadOnly IsActiveProperty As DependencyProperty
'        'Public Shared ReadOnly LeftProperty As DependencyProperty
'        'Public Shared ReadOnly ResizeModeProperty As DependencyProperty
'        'Public Shared ReadOnly ShowActivatedProperty As DependencyProperty
'        'Public Shared ReadOnly ShowInTaskbarProperty As DependencyProperty
'        'Public Shared ReadOnly SizeToContentProperty As DependencyProperty
'        'Public Shared ReadOnly TaskbarItemInfoProperty As DependencyProperty
'        'Public Shared ReadOnly TitleProperty As DependencyProperty
'        'Public Shared ReadOnly TopmostProperty As DependencyProperty
'        'Public Shared ReadOnly TopProperty As DependencyProperty
'        'Public Shared ReadOnly WindowStateProperty As DependencyProperty
'        'Public Shared ReadOnly WindowStyleProperty As DependencyProperty

'        <SecurityCritical>
'        Public Sub New()

'        End Sub

'        'public static readonly DependencyProperty TaskbarItemInfoProperty = DependencyProperty.Register(
'        '	"TaskbarItemInfo",
'        '	typeof(TaskbarItemInfo),
'        '	typeof(Window),
'        '	new PropertyMetadata(
'        '		null,
'        '		(d, e) => ((Window)d).OnTaskbarItemInfoChanged(e),
'        '		VerifyAccessCoercion));

'        'public static readonly DependencyProperty AllowsTransparencyProperty =
'        '		DependencyProperty.Register(
'        '				"AllowsTransparency",
'        '				typeof(bool),
'        '				typeof(Window),
'        '				new FrameworkPropertyMetadata(
'        '						BooleanBoxes.FalseBox,
'        '						new PropertyChangedCallback(OnAllowsTransparencyChanged),
'        '						new CoerceValueCallback(CoerceAllowsTransparency)));
'#Region "AllowsTransparency Property"
'        Public Shared ReadOnly AllowsTransparencyProperty As DependencyProperty = DependencyProperty.Register("AllowsTransparency", GetType(Boolean), GetType(WindowBase), New PropertyMetadata(False, AddressOf AllowsTransparency_Changed))

'        Private Shared Sub AllowsTransparency_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
'            Dim Self = DirectCast(D, WindowBase)

'            'Dim OldValue = DirectCast(E.OldValue, Boolean)
'            Dim NewValue = DirectCast(E.NewValue, Boolean)

'            Self.Window.AllowsTransparency = NewValue

'        End Sub

'        Public Property AllowsTransparency As Boolean
'            Get
'                Return DirectCast(Me.GetValue(AllowsTransparencyProperty), Boolean)
'            End Get
'            Set(ByVal value As Boolean)
'                Me.SetValue(AllowsTransparencyProperty, value)
'            End Set
'        End Property
'#End Region

'        'public static readonly DependencyProperty TitleProperty =
'        '		DependencyProperty.Register("Title", typeof(String), typeof(Window),
'        '				new FrameworkPropertyMetadata(String.Empty,
'        '						new PropertyChangedCallback(_OnTitleChanged)),
'        '				new ValidateValueCallback(_ValidateText));

'        'public static readonly DependencyProperty IconProperty =
'        '		DependencyProperty.Register(
'        '				"Icon",
'        '				typeof(ImageSource),
'        '				typeof(Window),
'        '				new FrameworkPropertyMetadata(
'        '						new PropertyChangedCallback(_OnIconChanged),
'        '						new CoerceValueCallback(VerifyAccessCoercion)));

'        'public static readonly DependencyProperty SizeToContentProperty =
'        '		DependencyProperty.Register("SizeToContent",
'        '				typeof(SizeToContent),
'        '				typeof(Window),
'        '				new FrameworkPropertyMetadata(
'        '						SizeToContent.Manual,
'        '						new PropertyChangedCallback(_OnSizeToContentChanged)),
'        '				new ValidateValueCallback(_ValidateSizeToContentCallback));

'        'public static readonly DependencyProperty ShowInTaskbarProperty =
'        '		DependencyProperty.Register("ShowInTaskbar",
'        '				typeof(bool),
'        '				typeof(Window),
'        '				new FrameworkPropertyMetadata(BooleanBoxes.TrueBox,
'        '						new PropertyChangedCallback(_OnShowInTaskbarChanged),
'        '						new CoerceValueCallback(VerifyAccessCoercion)));

'        'private static readonly DependencyPropertyKey IsActivePropertyKey
'        '	= DependencyProperty.RegisterReadOnly("IsActive", typeof(bool), typeof(Window),
'        '								  new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

'        'public static readonly DependencyProperty WindowStyleProperty =
'        '		DependencyProperty.Register("WindowStyle", typeof(WindowStyle), typeof(Window),
'        '				new FrameworkPropertyMetadata(
'        '						WindowStyle.SingleBorderWindow,
'        '						new PropertyChangedCallback(_OnWindowStyleChanged),
'        '						new CoerceValueCallback(CoerceWindowStyle)),
'        '				new ValidateValueCallback(_ValidateWindowStyleCallback));

'        'public static readonly DependencyProperty WindowStateProperty =
'        '		DependencyProperty.Register("WindowState", typeof(WindowState), typeof(Window),
'        '				new FrameworkPropertyMetadata(
'        '						WindowState.Normal,
'        '						FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
'        '						new PropertyChangedCallback(_OnWindowStateChanged),
'        '						new CoerceValueCallback(VerifyAccessCoercion)),
'        '				new ValidateValueCallback(_ValidateWindowStateCallback));

'        'public static readonly DependencyProperty ResizeModeProperty =
'        '		DependencyProperty.Register("ResizeMode", typeof(ResizeMode), typeof(Window),
'        '				new FrameworkPropertyMetadata(ResizeMode.CanResize,
'        '						FrameworkPropertyMetadataOptions.AffectsMeasure,
'        '						new PropertyChangedCallback(_OnResizeModeChanged),
'        '						new CoerceValueCallback(VerifyAccessCoercion)),
'        '				new ValidateValueCallback(_ValidateResizeModeCallback));

'        'public static readonly DependencyProperty TopmostProperty =
'        '		DependencyProperty.Register("Topmost",
'        '				typeof(bool),
'        '				typeof(Window),
'        '				new FrameworkPropertyMetadata(BooleanBoxes.FalseBox,
'        '						new PropertyChangedCallback(_OnTopmostChanged),
'        '						new CoerceValueCallback(VerifyAccessCoercion)));

'        'public static readonly DependencyProperty ShowActivatedProperty =
'        '		DependencyProperty.Register("ShowActivated",
'        '				typeof(bool),
'        '				typeof(Window),
'        '				new FrameworkPropertyMetadata(BooleanBoxes.TrueBox,
'        '						null,
'        '						new CoerceValueCallback(VerifyAccessCoercion)));

'        'internal static readonly DependencyProperty IWindowServiceProperty
'        '	= DependencyProperty.RegisterAttached("IWindowService", typeof(IWindowService), typeof(Window),
'        '								  new FrameworkPropertyMetadata((IWindowService)null,
'        '								  FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior));


'        'Public Property AllowsTransparency As Boolean
'        '<DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> <TypeConverter(GetType(DialogResultConverter))>
'        'Public Property DialogResult As Boolean?
'        'Public Property Icon As ImageSource
'        'Public ReadOnly Property IsActive As Boolean
'        '<TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")>
'        'Public Property Left As Double
'        'Public ReadOnly Property OwnedWindows As WindowCollection
'        '<DefaultValue(DirectCast(Nothing, Object))>
'        'Public Property Owner As Window
'        'Public Property ResizeMode As ResizeMode
'        'Public ReadOnly Property RestoreBounds As Rect
'        'Public Property ShowActivated As Boolean
'        'Public Property ShowInTaskbar As Boolean
'        'Public Property SizeToContent As SizeToContent
'        'Public Property TaskbarItemInfo As TaskbarItemInfo
'        '<Localizability(LocalizationCategory.Title)>
'        'Public Property Title As String
'        '<TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")>
'        'Public Property Top As Double
'        'Public Property Topmost As Boolean
'        '<DefaultValue(WindowStartupLocation.Manual)>
'        'Public Property WindowStartupLocation As WindowStartupLocation
'        'Public Property WindowState As WindowState
'        'Public Property WindowStyle As WindowStyle
'        'Protected Overrides ReadOnly Property LogicalChildren As IEnumerator

'        Public Event Activated As EventHandler
'        Public Event Closed As EventHandler
'        Public Event Closing As CancelEventHandler
'        Public Event ContentRendered As EventHandler
'        Public Event Deactivated As EventHandler
'        Public Event LocationChanged As EventHandler
'        Public Event SourceInitialized As EventHandler
'        Public Event StateChanged As EventHandler

'        <SecurityCritical>
'        Public Sub Close()

'        End Sub
'        <SecurityCritical>
'        Public Sub DragMove()

'        End Sub

'        Public Sub Hide()

'        End Sub
'        Public Sub Show()

'        End Sub
'        Protected Overridable Sub OnActivated(e As EventArgs)

'        End Sub
'        Protected Overridable Sub OnClosed(e As EventArgs)

'        End Sub
'        Protected Overridable Sub OnClosing(e As CancelEventArgs)

'        End Sub
'        Protected Overridable Sub OnContentChanged(oldContent As Object, newContent As Object)

'        End Sub
'        Protected Overridable Sub OnContentRendered(e As EventArgs)

'        End Sub
'        Protected Overridable Sub OnDeactivated(e As EventArgs)

'        End Sub
'        Protected Overridable Sub OnLocationChanged(e As EventArgs)

'        End Sub
'        Protected Overrides Sub OnManipulationBoundaryFeedback(e As ManipulationBoundaryFeedbackEventArgs)

'        End Sub
'        Protected Overridable Sub OnSourceInitialized(e As EventArgs)

'        End Sub
'        Protected Overridable Sub OnStateChanged(e As EventArgs)

'        End Sub
'        Protected NotOverridable Overrides Sub OnVisualParentChanged(oldParent As DependencyObject)

'        End Sub

'        Public Shared Function GetWindow(dependencyObject As DependencyObject) As Window

'        End Function
'        <SecurityCritical>
'        Public Function Activate() As Boolean

'        End Function
'        <SecurityCritical>
'        Public Function ShowDialog() As Boolean?

'        End Function
'        Protected Overrides Function ArrangeOverride(arrangeBounds As Size) As Size

'        End Function
'        Protected Overrides Function MeasureOverride(availableSize As Size) As Size

'        End Function
'        Protected Overrides Function OnCreateAutomationPeer() As AutomationPeer

'        End Function

'        Private ReadOnly Window As Windows.Window = New Window()

'    End Class

'End Namespace
