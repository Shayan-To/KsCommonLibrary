Imports Ks.Common.Controls

Namespace MVVM

    Public MustInherit Class ViewModel
        Inherits BindableBase

        Public Sub New(ByVal KsApplication As KsApplication)
            Me._KsApplication = KsApplication

            Me._Metadata = Me.GetType().GetCustomAttribute(Of ViewModelMetadataAttribute)()
            If Me.Metadata Is Nothing Then
                Throw New InvalidOperationException()
            End If

            Me._View = CreateView(Me.Metadata.ViewType)
            Me._View.DataContext = Me
        End Sub

        Private Shared Function CreateView(ByVal Type As Type) As Control
            If Not GetType(INavigationView).IsAssignableFrom(Type) Then
                Throw New ArgumentException("ViewType must be a Window or a Page.")
            End If

            Return DirectCast(Type.GetConstructor(Utilities.Typed(Of Type).EmptyArray).Invoke(Utilities.Typed(Of Object).EmptyArray), Control)
        End Function

#Region "View Property"
        Private ReadOnly _View As Control

        Public ReadOnly Property View As Control
            Get
                Return Me._View
            End Get
        End Property
#End Region

#Region "KsApplication Property"
        Private ReadOnly _KsApplication As KsApplication

        Public ReadOnly Property KsApplication As KsApplication
            Get
                Return Me._KsApplication
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

    End Class

End Namespace
