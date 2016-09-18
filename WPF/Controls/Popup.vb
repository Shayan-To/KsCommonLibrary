Imports Ks.Common.MVVM

Namespace Controls

    Public Class Popup
        Inherits ContentElement

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(Popup), New FrameworkPropertyMetadata(GetType(Popup)))
            HorizontalAlignmentProperty.OverrideMetadata(GetType(Popup), New FrameworkPropertyMetadata(HorizontalAlignment.Center))
            VerticalAlignmentProperty.OverrideMetadata(GetType(Popup), New FrameworkPropertyMetadata(VerticalAlignment.Center))
            'WidthProperty.OverrideMetadata(GetType(Popup), New FrameworkPropertyMetadata(300.0))
            'HeightProperty.OverrideMetadata(GetType(Popup), New FrameworkPropertyMetadata(300.0))
        End Sub

#Region "BeforeShow Event"
        Public Shared ReadOnly BeforeShowEvent As RoutedEvent = EventManager.RegisterRoutedEvent("BeforeShow", RoutingStrategy.Direct, GetType(RoutedEventHandler), GetType(Popup))

        Protected Overridable Sub OnBeforeShow()
            Dim E = New RoutedEventArgs(BeforeShowEvent)
            Me.RaiseEvent(E)
        End Sub

        Public Custom Event BeforeShow As RoutedEventHandler
            AddHandler(ByVal value As RoutedEventHandler)
                Me.AddHandler(BeforeShowEvent, value)
            End AddHandler
            RemoveHandler(ByVal value As RoutedEventHandler)
                Me.RemoveHandler(BeforeShowEvent, value)
            End RemoveHandler
            RaiseEvent()
            End RaiseEvent
        End Event
#End Region

#Region "Shown Event"
        Public Shared ReadOnly ShownEvent As RoutedEvent = EventManager.RegisterRoutedEvent("Shown", RoutingStrategy.Direct, GetType(RoutedEventHandler), GetType(Popup))

        Protected Overridable Sub OnShown()
            Dim E = New RoutedEventArgs(ShownEvent)
            Me.RaiseEvent(E)
        End Sub

        Public Custom Event Shown As RoutedEventHandler
            AddHandler(ByVal value As RoutedEventHandler)
                Me.AddHandler(ShownEvent, value)
            End AddHandler
            RemoveHandler(ByVal value As RoutedEventHandler)
                Me.RemoveHandler(ShownEvent, value)
            End RemoveHandler
            RaiseEvent()
            End RaiseEvent
        End Event
#End Region

#Region "Hid Event"
        Public Shared ReadOnly HidEvent As RoutedEvent = EventManager.RegisterRoutedEvent("Hid", RoutingStrategy.Direct, GetType(RoutedEventHandler), GetType(Popup))

        Protected Overridable Sub OnHid()
            Dim E = New RoutedEventArgs(HidEvent)
            Me.RaiseEvent(E)
        End Sub

        Public Custom Event Hid As RoutedEventHandler
            AddHandler(ByVal value As RoutedEventHandler)
                Me.AddHandler(HidEvent, value)
            End AddHandler
            RemoveHandler(ByVal value As RoutedEventHandler)
                Me.RemoveHandler(HidEvent, value)
            End RemoveHandler
            RaiseEvent()
            End RaiseEvent
        End Event
#End Region

#Region "Show Command"
        Private _ShowCommand As DelegateCommand = New DelegateCommand(AddressOf Me.Show)

        Public Sub Show()
            Me.IsShown = True
        End Sub

        Public ReadOnly Property ShowCommand As DelegateCommand
            Get
                Return Me._ShowCommand
            End Get
        End Property
#End Region

#Region "Hide Command"
        Private _HideCommand As DelegateCommand = New DelegateCommand(AddressOf Me.Hide)

        Public Sub Hide()
            Me.IsShown = False
        End Sub

        Public ReadOnly Property HideCommand As DelegateCommand
            Get
                Return Me._HideCommand
            End Get
        End Property
#End Region

#Region "IsShown Property"
        Public Shared ReadOnly IsShownProperty As DependencyProperty = DependencyProperty.Register("IsShown", GetType(Boolean), GetType(Popup), New PropertyMetadata(False, AddressOf IsShown_Changed))

        Private Shared Sub IsShown_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, Popup)

            If DirectCast(E.NewValue, Boolean) Then
                Self.OnBeforeShow()
                Self.GetWindow()?.AddPopup(Self)
                Self.OnShown()
            Else
                Self.GetWindow()?.RemovePopup(Self)
                Self.OnHid()
            End If
        End Sub

        Public Property IsShown As Boolean
            Get
                Return DirectCast(Me.GetValue(IsShownProperty), Boolean)
            End Get
            Set(ByVal value As Boolean)
                Me.SetValue(IsShownProperty, value)
            End Set
        End Property
#End Region

#Region "HasShelter Property"
        Public Shared ReadOnly HasShelterProperty As DependencyProperty = DependencyProperty.Register("HasShelter", GetType(Boolean), GetType(Popup), New PropertyMetadata(True, AddressOf HasShelter_Changed))

        Private Shared Sub HasShelter_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, Popup)

            'Dim OldValue = DirectCast(E.OldValue, Boolean)
            'Dim NewValue = DirectCast(E.NewValue, Boolean)

            If Self.IsShown Then
                Self.GetWindow()?.RefreshPopup(Self)
            End If
        End Sub

        Public Property HasShelter As Boolean
            Get
                Return DirectCast(Me.GetValue(HasShelterProperty), Boolean)
            End Get
            Set(ByVal value As Boolean)
                Me.SetValue(HasShelterProperty, value)
            End Set
        End Property
#End Region

#Region "DimShelter Property"
        Public Shared ReadOnly DimShelterProperty As DependencyProperty = DependencyProperty.Register("DimShelter", GetType(Boolean), GetType(Popup), New PropertyMetadata(True, AddressOf DimShelter_Changed))

        Private Shared Sub DimShelter_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, Popup)

            'Dim OldValue = DirectCast(E.OldValue, Boolean)
            'Dim NewValue = DirectCast(E.NewValue, Boolean)

            If Self.IsShown Then
                Self.GetWindow()?.UpdateDims()
            End If
        End Sub

        Public Property DimShelter As Boolean
            Get
                Return DirectCast(Me.GetValue(DimShelterProperty), Boolean)
            End Get
            Set(ByVal value As Boolean)
                Me.SetValue(DimShelterProperty, value)
            End Set
        End Property
#End Region

#Region "Layer Property"
        Public Shared ReadOnly LayerProperty As DependencyProperty = DependencyProperty.Register("Layer", GetType(Integer), GetType(Popup), New PropertyMetadata(0, AddressOf Layer_Changed))

        Private Shared Sub Layer_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, Popup)

            If Self.IsShown Then
                Dim OldValue = DirectCast(E.OldValue, Integer)
                'Dim NewValue = DirectCast(E.NewValue, Integer)

                Dim Window = Self.GetWindow()

                Window?.RemovePopup(Self, OldValue)
                Window?.AddPopup(Self)
            End If
        End Sub

        Public Property Layer As Integer
            Get
                Return DirectCast(Me.GetValue(LayerProperty), Integer)
            End Get
            Set(ByVal value As Integer)
                Me.SetValue(LayerProperty, value)
            End Set
        End Property
#End Region

#Region "IsShelterSensitive Property"
        Public Shared ReadOnly IsShelterSensitiveProperty As DependencyProperty = DependencyProperty.Register("IsShelterSensitive", GetType(Boolean), GetType(PopupShelter), New PropertyMetadata(True, AddressOf IsShelterSensitive_Changed))

        Private Shared Sub IsShelterSensitive_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, PopupShelter)

            Dim OldValue = DirectCast(E.OldValue, Boolean)
            Dim NewValue = DirectCast(E.NewValue, Boolean)
        End Sub

        Public Property IsShelterSensitive As Boolean
            Get
                Return DirectCast(Me.GetValue(IsShelterSensitiveProperty), Boolean)
            End Get
            Set(ByVal value As Boolean)
                Me.SetValue(IsShelterSensitiveProperty, value)
            End Set
        End Property
#End Region

#Region "Window Property"
        Public Shared ReadOnly WindowProperty As DependencyProperty = DependencyProperty.Register("Window", GetType(Window), GetType(Popup), New PropertyMetadata(Nothing, AddressOf Window_Changed))

        Private Shared Sub Window_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, Popup)

            Dim OldValue = GetWindow(DirectCast(E.OldValue, Window))
            Dim NewValue = GetWindow(DirectCast(E.NewValue, Window))

            If Self.IsShown Then
                OldValue?.RemovePopup(Self)
                NewValue?.AddPopup(Self)
            End If
        End Sub

        Public Property Window As Window
            Get
                Return DirectCast(Me.GetValue(WindowProperty), Window)
            End Get
            Set(ByVal value As Window)
                Me.SetValue(WindowProperty, value)
            End Set
        End Property

        Private Shared Function GetWindow(ByVal Window As Window) As Window
            If Window IsNot Nothing Then
                Return Window
            End If
            Return TryCast(KsApplication.Current.Window.View, Window)
        End Function

        Private Function GetWindow() As Window
            Return GetWindow(Me.Window)
        End Function
#End Region

        Friend ArrangeCallBack As Func(Of PopupPanel, Size, Size, Rect?)

    End Class

End Namespace