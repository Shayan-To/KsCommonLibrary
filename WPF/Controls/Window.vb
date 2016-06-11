Imports Ks.Common.MVVM

Namespace Controls

    ' ToDo Add some context class that the main window can be get from it. (We kind of have it right now, the KsApplication class is our context class, we just have to make the main objects (like windows and pages and ...) have it. (And maybe it is not needed in this kind of design. Because a view is known only by its view model, and for a view by itself, there is no use.)

    <TemplatePart(Name:=Window.PopupsPanelName, Type:=GetType(PopupPanel))>
    <TemplatePart(Name:=Window.ContentPresenterName, Type:=GetType(ContentPresenter))>
    Public Class Window
        Inherits Windows.Window
        Implements INavigationView

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(Window), New FrameworkPropertyMetadata(GetType(Window)))
            ContentProperty.OverrideMetadata(GetType(Window), New FrameworkPropertyMetadata(Nothing, Nothing, Function(D, C) TryCast(C, UIElement)))
        End Sub

        Friend Sub AddPopup(ByVal Popup As Popup)
            Dim LayerIndex = Popup.Layer
            Dim NextLayerFirst As Popup = Nothing

            For Each KV In Me.ShelterLayers
                If KV.Key > LayerIndex Then
                    NextLayerFirst = KV.Value.Item(0)
                    Exit For
                End If
            Next

            Dim PanelChildren = Me.PopupsPanel.Children
            Dim LastIndex = -1

            If NextLayerFirst Is Nothing Then
                LastIndex = PanelChildren.Count
            Else
                LastIndex = PanelChildren.IndexOf(NextLayerFirst)
            End If

            Dim Layer = Me.ShelterLayers.Item(LayerIndex)
            Dim Shelter As PopupShelter = Nothing

            If Popup.HasShelter Then
                Shelter = New PopupShelter() With {.DataContext = Popup, .Style = Me.ShelterStyle}
                Me.PopupShelterDic.Add(Popup, Shelter)
                PanelChildren.Insert(LastIndex, Shelter)
                LastIndex += 1
            End If

            PanelChildren.Insert(LastIndex, Popup)

            Me.UpdateDims()
        End Sub

        Friend Sub RemovePopup(ByVal Popup As Popup, Optional ByVal LayerIndex As Integer = -1)
            If LayerIndex = -1 Then
                LayerIndex = Popup.Layer
            End If

            Dim Shelter = Me.PopupShelterDic.Item(Popup)
            Me.PopupShelterDic.Remove(Popup)

            Dim PanelChildren = Me.PopupsPanel.Children
            PanelChildren.Remove(Popup)
            PanelChildren.Remove(Shelter)

            Me.UpdateDims()
        End Sub

        Friend Sub RefreshPopup(ByVal Popup As Popup)
            Dim Shelter As PopupShelter = Nothing

            If Popup.HasShelter Then
                If Not Me.PopupShelterDic.ContainsKey(Popup) Then
                    Shelter = New PopupShelter() With {.DataContext = Popup}
                    Me.PopupShelterDic.Add(Popup, Shelter)

                    Dim PanelChildren = Me.PopupsPanel.Children
                    Dim PopupIndex = PanelChildren.IndexOf(Popup)
                    PanelChildren.Insert(PopupIndex, Shelter)
                End If
            Else
                If Me.PopupShelterDic.TryGetValue(Popup, Shelter) Then
                    Me.PopupsPanel.Children.Remove(Shelter)
                    Me.PopupShelterDic.Remove(Popup)
                End If
            End If

            Me.UpdateDims()
        End Sub

        Friend Sub UpdateDims()
            Dim DimmedSeen = False
            For Each I As Integer In Me.ShelterLayers.Keys.Reverse()
                Dim Layer = Me.ShelterLayers.Item(I)
                For J As Integer = Layer.Count - 1 To 0 Step -1
                    Dim Popup = Layer.Item(J)
                    If Popup.HasShelter Then
                        Dim Shelter = Me.PopupShelterDic.Item(Popup)
                        If DimmedSeen Then
                            Shelter.IsShelterShown = False
                        Else
                            Shelter.IsShelterShown = True
                            If Popup.DimShelter Then
                                DimmedSeen = True
                            End If
                        End If
                    End If
                Next
            Next
        End Sub

        Private Sub UpdateShelterStyle()
            Dim ShelterStyle = Me.ShelterStyle
            For Each KV In Me.PopupShelterDic
                KV.Value.Style = ShelterStyle
            Next
        End Sub

        Private Property INavigationView_Content As Page Implements INavigationView.Content
            Get
                Return TryCast(Me.Content, Page)
            End Get
            Set(ByVal Value As Page)
                Me.Content = Value
            End Set
        End Property

#Region "WindowStartupPosition Property"
        Public Shared ReadOnly WindowStartupPositionProperty As DependencyProperty = DependencyProperty.Register("WindowStartupPosition", GetType(WindowStartupLocation), GetType(Window), New PropertyMetadata(WindowStartupLocation.Manual, AddressOf WindowStartupPosition_Changed, AddressOf WindowStartupPosition_Coerce))

        Private Shared Function WindowStartupPosition_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = DirectCast(D, Window)

            'Dim Value = DirectCast(BaseValue, WindowStartupLocation)

            Return BaseValue
        End Function

        Private Shared Sub WindowStartupPosition_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, Window)

            'Dim OldValue = DirectCast(E.OldValue, WindowStartupLocation)
            Dim NewValue = DirectCast(E.NewValue, WindowStartupLocation)

            Self.WindowStartupLocation = NewValue
        End Sub

        Public Property WindowStartupPosition As WindowStartupLocation
            Get
                Return DirectCast(Me.GetValue(WindowStartupPositionProperty), WindowStartupLocation)
            End Get
            Set(ByVal value As WindowStartupLocation)
                Me.SetValue(WindowStartupPositionProperty, value)
            End Set
        End Property
#End Region

#Region "ShelterStyle Property"
        Public Shared ReadOnly ShelterStyleProperty As DependencyProperty = DependencyProperty.Register("ShelterStyle", GetType(Style), GetType(Window), New PropertyMetadata(Nothing, AddressOf ShelterStyle_Changed))

        Private Shared Sub ShelterStyle_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, Window)

            'Dim OldValue = DirectCast(E.OldValue, Style)
            'Dim NewValue = DirectCast(E.NewValue, Style)

            Self.UpdateShelterStyle()
        End Sub

        Public Property ShelterStyle As Style
            Get
                Return DirectCast(Me.GetValue(ShelterStyleProperty), Style)
            End Get
            Set(ByVal value As Style)
                Me.SetValue(ShelterStyleProperty, value)
            End Set
        End Property
#End Region

#Region "TemplateParts Logic"
        Public Overrides Sub OnApplyTemplate()
            Me.PopupsPanel = TryCast(Me.Template.FindName(PopupsPanelName, Me), PopupPanel)
            Me.ContentPresenter = TryCast(Me.Template.FindName(ContentPresenterName, Me), ContentPresenter)

            MyBase.OnApplyTemplate()
        End Sub

#Region "PopupsPanel Part"
        Friend Const PopupsPanelName As String = "PART_PopupsPanel"
        Private _PopupsPanel As PopupPanel

        Friend Property PopupsPanel As PopupPanel
            Get
                Return Me._PopupsPanel
            End Get
            Private Set(ByVal Value As PopupPanel)
                Me._PopupsPanel = Value
            End Set
        End Property
#End Region

#Region "ContentPresenter Part"
        Friend Const ContentPresenterName As String = "PART_ContentPresenter"
        Private _ContentPresenter As ContentPresenter

        Private Property ContentPresenter As ContentPresenter
            Get
                Return Me._ContentPresenter
            End Get
            Set(ByVal Value As ContentPresenter)
                Me._ContentPresenter = Value
            End Set
        End Property
#End Region
#End Region

        Private ReadOnly ShelterLayers As CreateInstanceDictionary(Of Integer, List(Of Popup)) = CreateInstanceDictionary.Create(New SortedDictionary(Of Integer, List(Of Popup))())
        Private ReadOnly PopupShelterDic As Dictionary(Of Popup, PopupShelter) = New Dictionary(Of Popup, PopupShelter)()

    End Class

End Namespace
