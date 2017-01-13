Namespace Common.Controls

    Public Class PopupShelter
        Inherits Control

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(PopupShelter), New FrameworkPropertyMetadata(GetType(PopupShelter)))
        End Sub

#Region "IsShelterShown Property"
        Private Shared ReadOnly IsShelterShownPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("IsShelterShown", GetType(Boolean), GetType(PopupShelter), New PropertyMetadata(True))
        Public Shared ReadOnly IsShelterShownProperty As DependencyProperty = IsShelterShownPropertyKey.DependencyProperty

        Public Property IsShelterShown As Boolean
            Get
                Return DirectCast(Me.GetValue(IsShelterShownProperty), Boolean)
            End Get
            Friend Set(ByVal value As Boolean)
                Me.SetValue(IsShelterShownPropertyKey, value)
            End Set
        End Property
#End Region

    End Class

End Namespace
