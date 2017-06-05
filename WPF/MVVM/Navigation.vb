Imports System.Windows.Input

Namespace Common.MVVM

    Public Class Navigation
        Inherits Markup.MarkupExtension
        Implements ICommand

        Public Sub New()

        End Sub

        Public Sub New(ByVal ViewType As Type)
            Me.ViewType = ViewType
        End Sub

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            Dim KsApplication = Me.GetKsApplication()
            KsApplication.NavigateTo(Me.GetParent(), KsApplication.GetViewModel(Me.ViewType), Me.AddToStack, Me.ForceToStack)
        End Sub

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return True
        End Function

        Private Function GetParent() As NavigationViewModel
            Dim T = Me.Parent
            If T IsNot Nothing Then
                Return T
            End If
            Dim T2 = Me.ParentType
            If T2 IsNot Nothing Then
                Return DirectCast(Me.GetKsApplication().GetViewModel(T2), NavigationViewModel)
            End If
            Return Me.GetKsApplication().DefaultNavigationView
        End Function

        Private Function GetKsApplication() As KsApplication
            Return If(Me.KsApplication, KsApplication.Current)
        End Function

        Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
            Return Me
        End Function

#Region "AddToStack Property"
        Private _AddToStack As Boolean = True

        Public Property AddToStack As Boolean
            Get
                Return Me._AddToStack
            End Get
            Set(ByVal Value As Boolean)
                Me._AddToStack = Value
            End Set
        End Property
#End Region

#Region "ForceToStack Property"
        Private _ForceToStack As Boolean = False

        Public Property ForceToStack As Boolean
            Get
                Return Me._ForceToStack
            End Get
            Set(ByVal Value As Boolean)
                Me._ForceToStack = Value
            End Set
        End Property
#End Region

#Region "ViewType Property"
        Private _ViewType As Type

        Public Property ViewType As Type
            Get
                Return Me._ViewType
            End Get
            Set(ByVal Value As Type)
                If Value Is Nothing OrElse Not GetType(ViewModel).IsAssignableFrom(Value) Then
                    Value = Nothing
                End If

                Me._ViewType = Value
            End Set
        End Property
#End Region

#Region "ParentType Property"
        Private _ParentType As Type

        Public Property ParentType As Type
            Get
                Return Me._ParentType
            End Get
            Set(ByVal Value As Type)
                If Me.Parent IsNot Nothing Then
                    Value = Nothing
                End If

                If Value Is Nothing OrElse Not GetType(NavigationViewModel).IsAssignableFrom(Value) Then
                    Value = Nothing
                End If

                Me._ParentType = Value
            End Set
        End Property
#End Region

#Region "Parent Property"
        Private _Parent As NavigationViewModel

        Public Property Parent As NavigationViewModel
            Get
                Return Me._Parent
            End Get
            Set(ByVal Value As NavigationViewModel)
                If Me.ParentType IsNot Nothing Then
                    Value = Nothing
                End If

                Me._Parent = Value
            End Set
        End Property
#End Region

#Region "KsApplication Property"
        Private _KsApplication As KsApplication

        Public Property KsApplication As KsApplication
            Get
                Return Me._KsApplication
            End Get
            Set(ByVal Value As KsApplication)
                Me._KsApplication = Value
            End Set
        End Property
#End Region

    End Class

End Namespace
