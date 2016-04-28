Imports System.Windows.Markup
Imports Ks.Common.MVVM

Namespace Controls

    <ContentProperty("Content")>
    Public Class PopupHolder
        Inherits FrameworkElement ' UIElement is not used as base type throughout the whole libraries! Using FrameworkElement instead.

        Protected Overrides Function MeasureOverride(availableSize As Size) As Size
            Return Nothing
        End Function

        Protected Overrides Function ArrangeOverride(finalSize As Size) As Size
            Return Nothing
        End Function

        Protected Overridable Function ArrangePopup(ByVal Panel As PopupPanel, ByVal PanelSize As Size, ByVal PopupSize As Size) As Rect?
            Dim Target = Me.GetTarget()

            If Target Is Nothing Then
                Return Nothing
            End If

            Dim Transform = Target.TransformToVisual(Panel)
            Dim TargetSize = New Size(Target.ActualWidth, Target.ActualHeight)
            Dim BasePoint = Transform.Transform(New Point(TargetSize.Width * Me.TargetHorizontalAnchor, TargetSize.Height * Me.TargetVerticalAnchor))

            BasePoint -= New Vector(PopupSize.Width * Me.PopupHorizontalAnchor, PopupSize.Height * Me.PopupVerticalAnchor)

            Return New Rect(BasePoint, PopupSize)
        End Function

#Region "ShowPopup Command"
        Private _ShowPopupCommand As DelegateCommand = New DelegateCommand(AddressOf Me.ShowPopup)

        Public Sub ShowPopup()
            Me.IsPopupShown = True
        End Sub

        Public ReadOnly Property ShowPopupCommand As DelegateCommand
            Get
                Return Me._ShowPopupCommand
            End Get
        End Property
#End Region

#Region "HidePopup Command"
        Private _HidePopupCommand As DelegateCommand = New DelegateCommand(AddressOf Me.HidePopup)

        Public Sub HidePopup()
            Me.IsPopupShown = False
        End Sub

        Public ReadOnly Property HidePopupCommand As DelegateCommand
            Get
                Return Me._HidePopupCommand
            End Get
        End Property
#End Region

#Region "Content Property"
        Public Shared ReadOnly ContentProperty As DependencyProperty = DependencyProperty.Register("Content", GetType(UIElement), GetType(PopupHolder), New PropertyMetadata(Nothing, AddressOf Content_Changed))

        Private Shared Sub Content_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, PopupHolder)

            'Dim OldValue = DirectCast(E.OldValue, UIElement)
            Dim NewValue = DirectCast(E.NewValue, UIElement)
            Dim Popup = TryCast(NewValue, Popup)

            If Popup IsNot Nothing Then
                Self.Popup = Popup
            Else
                If Self.Popup IsNot Nothing Then
                    Self.Popup.Content = NewValue
                End If
            End If
        End Sub

        Public Property Content As UIElement
            Get
                Return DirectCast(Me.GetValue(ContentProperty), UIElement)
            End Get
            Set(ByVal value As UIElement)
                Me.SetValue(ContentProperty, value)
            End Set
        End Property
#End Region

#Region "Target Property"
        Public Shared ReadOnly TargetProperty As DependencyProperty = DependencyProperty.Register("Target", GetType(FrameworkElement), GetType(PopupHolder), New PropertyMetadata(Nothing, AddressOf Target_Changed))

        Private Shared Sub Target_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, PopupHolder)

            'Dim OldValue = DirectCast(E.OldValue, FrameworkElement)
            'Dim NewValue = DirectCast(E.NewValue, FrameworkElement)
        End Sub

        Public Property Target As FrameworkElement
            Get
                Return DirectCast(Me.GetValue(TargetProperty), FrameworkElement)
            End Get
            Set(ByVal value As FrameworkElement)
                Me.SetValue(TargetProperty, value)
            End Set
        End Property

        Private Function GetTarget() As FrameworkElement
            Return If(Me.Target, TryCast(Media.VisualTreeHelper.GetParent(Me), FrameworkElement))
        End Function
#End Region

#Region "Popup Property"
        Private Shared ReadOnly PopupPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("Popup", GetType(Popup), GetType(PopupHolder), New PropertyMetadata(Nothing, AddressOf Popup_Changed))

        Private Shared Sub Popup_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, PopupHolder)

            Dim OldValue = DirectCast(E.OldValue, Popup)
            Dim NewValue = DirectCast(E.NewValue, Popup)

            If OldValue IsNot Nothing Then
                OldValue.ArrangeCallBack = Nothing
            End If
            If NewValue IsNot Nothing Then
                NewValue.ArrangeCallBack = AddressOf Self.ArrangePopup
            End If
        End Sub

        Public Shared ReadOnly PopupProperty As DependencyProperty = PopupPropertyKey.DependencyProperty

        Public Property Popup As Popup
            Get
                Return DirectCast(Me.GetValue(PopupProperty), Popup)
            End Get
            Private Set(ByVal value As Popup)
                Me.SetValue(PopupPropertyKey, value)
            End Set
        End Property
#End Region

#Region "IsPopupShown Property"
        Public Shared ReadOnly IsPopupShownProperty As DependencyProperty = DependencyProperty.Register("IsPopupShown", GetType(Boolean), GetType(PopupHolder), New PropertyMetadata(False, AddressOf IsPopupShown_Changed))

        Private Shared Sub IsPopupShown_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, PopupHolder)

            'Dim OldValue = DirectCast(E.OldValue, Boolean)
            Dim NewValue = DirectCast(E.NewValue, Boolean)

            Dim Popup = Self.Popup
            If Popup Is Nothing And NewValue Then
                Popup = New Popup() With {.Content = Self.Content}
                Self.Popup = Popup
            End If

            If Popup IsNot Nothing Then
                'If NewValue Then
                '    Self.OnBeforeShowPopup()
                'End If
                Popup.IsShown = NewValue
            End If
        End Sub

        Public Property IsPopupShown As Boolean
            Get
                Return DirectCast(Me.GetValue(IsPopupShownProperty), Boolean)
            End Get
            Set(ByVal value As Boolean)
                Me.SetValue(IsPopupShownProperty, value)
            End Set
        End Property
#End Region

#Region "TargetHorizontalAnchor Property"
        Public Shared ReadOnly TargetHorizontalAnchorProperty As DependencyProperty = DependencyProperty.Register("TargetHorizontalAnchor", GetType(Double), GetType(PopupHolder), New PropertyMetadata(0.5, AddressOf TargetHorizontalAnchor_Changed))

        Private Shared Sub TargetHorizontalAnchor_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, PopupHolder)

            Dim OldValue = DirectCast(E.OldValue, Double)
            Dim NewValue = DirectCast(E.NewValue, Double)
        End Sub

        Public Property TargetHorizontalAnchor As Double
            Get
                Return DirectCast(Me.GetValue(TargetHorizontalAnchorProperty), Double)
            End Get
            Set(ByVal value As Double)
                Me.SetValue(TargetHorizontalAnchorProperty, value)
            End Set
        End Property
#End Region

#Region "TargetVerticalAnchor Property"
        Public Shared ReadOnly TargetVerticalAnchorProperty As DependencyProperty = DependencyProperty.Register("TargetVerticalAnchor", GetType(Double), GetType(PopupHolder), New PropertyMetadata(0.5, AddressOf TargetVerticalAnchor_Changed))

        Private Shared Sub TargetVerticalAnchor_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, PopupHolder)

            Dim OldValue = DirectCast(E.OldValue, Double)
            Dim NewValue = DirectCast(E.NewValue, Double)
        End Sub

        Public Property TargetVerticalAnchor As Double
            Get
                Return DirectCast(Me.GetValue(TargetVerticalAnchorProperty), Double)
            End Get
            Set(ByVal value As Double)
                Me.SetValue(TargetVerticalAnchorProperty, value)
            End Set
        End Property
#End Region

#Region "PopupHorizontalAnchor Property"
        Public Shared ReadOnly PopupHorizontalAnchorProperty As DependencyProperty = DependencyProperty.Register("PopupHorizontalAnchor", GetType(Double), GetType(PopupHolder), New PropertyMetadata(0.5, AddressOf PopupHorizontalAnchor_Changed))

        Private Shared Sub PopupHorizontalAnchor_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, PopupHolder)

            Dim OldValue = DirectCast(E.OldValue, Double)
            Dim NewValue = DirectCast(E.NewValue, Double)
        End Sub

        Public Property PopupHorizontalAnchor As Double
            Get
                Return DirectCast(Me.GetValue(PopupHorizontalAnchorProperty), Double)
            End Get
            Set(ByVal value As Double)
                Me.SetValue(PopupHorizontalAnchorProperty, value)
            End Set
        End Property
#End Region

#Region "PopupVerticalAnchor Property"
        Public Shared ReadOnly PopupVerticalAnchorProperty As DependencyProperty = DependencyProperty.Register("PopupVerticalAnchor", GetType(Double), GetType(PopupHolder), New PropertyMetadata(0.5, AddressOf PopupVerticalAnchor_Changed))

        Private Shared Sub PopupVerticalAnchor_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, PopupHolder)

            Dim OldValue = DirectCast(E.OldValue, Double)
            Dim NewValue = DirectCast(E.NewValue, Double)
        End Sub

        Public Property PopupVerticalAnchor As Double
            Get
                Return DirectCast(Me.GetValue(PopupVerticalAnchorProperty), Double)
            End Get
            Set(ByVal value As Double)
                Me.SetValue(PopupVerticalAnchorProperty, value)
            End Set
        End Property
#End Region

    End Class

End Namespace
