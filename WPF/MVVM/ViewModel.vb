Imports Ks.Common.Controls

Namespace MVVM

    Public MustInherit Class ViewModel
        Inherits BindableBase

        Public Sub New(ByVal KsApplication As KsApplication)
            Me._KsApplicationBase = KsApplication
            Me._Metadata = Me.GetMetadata()
        End Sub

        Public Sub New()
            If Not KsApplication.IsInDesignMode Then
                Throw New InvalidOperationException("Should not call the test constructor from runtime.")
            End If

            Me._Metadata = Me.GetMetadata()
            If Me._Metadata.KsApplicationType IsNot Nothing Then
                Me._KsApplicationBase = DirectCast(Me.Metadata.KsApplicationType.GetConstructor(Utilities.Typed(Of Type).EmptyArray).Invoke(Utilities.Typed(Of Object).EmptyArray), KsApplication)
            End If
        End Sub

        Private Function GetMetadata() As ViewModelMetadataAttribute
            Dim R = Me.GetType().GetCustomAttribute(Of ViewModelMetadataAttribute)()
            If R Is Nothing Then
                Throw New InvalidOperationException("Every viewmodel that can be instantiated should have a ViewModelMetadata attribute set to it.")
            End If
            If Not (GetType(Window).IsAssignableFrom(R.ViewType) Or GetType(Page).IsAssignableFrom(R.ViewType)) Then
                Throw New ArgumentException("ViewType must be a Window or a Page.")
            End If
            Return R
        End Function

#Region "View Property"
        Private _View As Control = Nothing

        Public Property View As Control
            Get
                If Me._View Is Nothing Then
                    Me._View = DirectCast(Me.Metadata.ViewType.GetConstructor(Utilities.Typed(Of Type).EmptyArray).Invoke(Utilities.Typed(Of Object).EmptyArray), Control)
                    Me._View.DataContext = Me
                    Utils.SetViewModel(Me._View, Me)
                End If
                Return Me._View
            End Get
            Friend Set(ByVal Value As Control)
                If Me._View IsNot Nothing Then
                    Throw New InvalidOperationException()
                End If
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
