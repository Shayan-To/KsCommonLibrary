Imports Ks.Common.Controls

Namespace Common.MVVM

    Public MustInherit Class ViewModel
        Inherits BindableBase

        Public Sub New(ByVal KsApplication As KsApplication)
            Me._KsApplicationBase = KsApplication
            Me._Metadata = Me.GetMetadata()
        End Sub

        Public Sub New()
            Verify.True(KsApplication.IsInDesignMode, "Test constructor should not be called from runtime.")

            Me._Metadata = Me.GetMetadata()
            If Me._Metadata.KsApplicationType IsNot Nothing Then
                Me._KsApplicationBase = DirectCast(Me.Metadata.KsApplicationType.CreateInstance(), KsApplication)
            End If
        End Sub

        Private Function GetMetadata() As ViewModelMetadataAttribute
            Dim R = Me.GetType().GetCustomAttribute(Of ViewModelMetadataAttribute)(False)
            Verify.False(R Is Nothing, "Every view-model that could be instantiated must have a ViewModelMetadata attribute set to it.")
            Return R
        End Function

#Region "NavigatedTo Event"
        Public Event NavigatedTo As EventHandler(Of NavigationEventArgs)

        Protected Friend Overridable Sub OnNavigatedTo(ByVal E As NavigationEventArgs)
            RaiseEvent NavigatedTo(Me, E)
        End Sub
#End Region

#Region "NavigatedFrom Event"
        Public Event NavigatedFrom As EventHandler(Of NavigationEventArgs)

        Protected Friend Overridable Sub OnNavigatedFrom(ByVal E As NavigationEventArgs)
            RaiseEvent NavigatedFrom(Me, E)
        End Sub
#End Region

#Region "View Property"
        Private _View As Control = Nothing

        Public Property View As Control
            Get
                If Me._View Is Nothing Then
                    Me._View = DirectCast(Me.Metadata.ViewType.CreateInstance(), Control)
                    Me._View.DataContext = Me
                    Utils.SetViewModel(Me._View, Me)
                End If
                Return Me._View
            End Get
            Friend Set(ByVal Value As Control)
                Verify.True(Me._View Is Nothing)
                Me._View = Value
            End Set
        End Property
#End Region

#Region "KsApplicationBase Property"
        Private ReadOnly _KsApplicationBase As KsApplication

        Public ReadOnly Property KsApplicationBase As KsApplication
            Get
                Return Me._KsApplicationBase
            End Get
        End Property
#End Region

#Region "NavigationFrame Property"
        Private _NavigationFrame As NavigationFrame

        Public Property NavigationFrame As NavigationFrame
            Get
                Return Me._NavigationFrame
            End Get
            Friend Set(ByVal Value As NavigationFrame)
                Me.SetProperty(Me._NavigationFrame, Value)
            End Set
        End Property
#End Region

#Region "Metadata Property"
        Private ReadOnly _Metadata As ViewModelMetadataAttribute

        Public ReadOnly Property Metadata As ViewModelMetadataAttribute
            Get
                Return Me._Metadata
            End Get
        End Property
#End Region

#Region "Type Property"
        Public ReadOnly Property Type As Type
            Get
                Return Me.GetType()
            End Get
        End Property
#End Region

    End Class

End Namespace
