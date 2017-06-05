Imports System.Windows.Input

Namespace Common.MVVM

    Public Class NavigationCommand
        Inherits DependencyObject
        Implements ICommand

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

#Region "AddToStack Property"
        Public Shared ReadOnly AddToStackProperty As DependencyProperty = DependencyProperty.Register("AddToStack", GetType(Boolean), GetType(NavigationCommand), New PropertyMetadata(True))

        Public Property AddToStack As Boolean
            Get
                Return DirectCast(Me.GetValue(AddToStackProperty), Boolean)
            End Get
            Set(ByVal value As Boolean)
                Me.SetValue(AddToStackProperty, value)
            End Set
        End Property
#End Region

#Region "ForceToStack Property"
        Public Shared ReadOnly ForceToStackProperty As DependencyProperty = DependencyProperty.Register("ForceToStack", GetType(Boolean), GetType(NavigationCommand), New PropertyMetadata(False))

        Public Property ForceToStack As Boolean
            Get
                Return DirectCast(Me.GetValue(ForceToStackProperty), Boolean)
            End Get
            Set(ByVal value As Boolean)
                Me.SetValue(ForceToStackProperty, value)
            End Set
        End Property
#End Region

#Region "ViewType Property"
        Public Shared ReadOnly ViewTypeProperty As DependencyProperty = DependencyProperty.Register("ViewType", GetType(Type), GetType(NavigationCommand), New PropertyMetadata(Nothing, Nothing, AddressOf ViewType_Coerce))

        Private Shared Function ViewType_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            Dim Value = TryCast(BaseValue, Type)

            If Value Is Nothing OrElse Not GetType(ViewModel).IsAssignableFrom(Value) Then
                Return Nothing
            End If

            Return BaseValue
        End Function

        Public Property ViewType As Type
            Get
                Return DirectCast(Me.GetValue(ViewTypeProperty), Type)
            End Get
            Set(ByVal value As Type)
                Me.SetValue(ViewTypeProperty, value)
            End Set
        End Property
#End Region

#Region "ParentType Property"
        Public Shared ReadOnly ParentTypeProperty As DependencyProperty = DependencyProperty.Register("ParentType", GetType(Type), GetType(NavigationCommand), New PropertyMetadata(Nothing, Nothing, AddressOf ParentType_Coerce))

        Private Shared Function ParentType_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            Dim Self = DirectCast(D, NavigationCommand)

            If Self.Parent IsNot Nothing Then
                Return Nothing
            End If

            Dim Value = TryCast(BaseValue, Type)
            If Value Is Nothing OrElse Not GetType(NavigationViewModel).IsAssignableFrom(Value) Then
                Return Nothing
            End If

            Return BaseValue
        End Function

        Public Property ParentType As Type
            Get
                Return DirectCast(Me.GetValue(ParentTypeProperty), Type)
            End Get
            Set(ByVal value As Type)
                Me.SetValue(ParentTypeProperty, value)
            End Set
        End Property
#End Region

#Region "Parent Property"
        Public Shared ReadOnly ParentProperty As DependencyProperty = DependencyProperty.Register("Parent", GetType(NavigationViewModel), GetType(NavigationCommand), New PropertyMetadata(Nothing, Nothing, AddressOf Parent_Coerce))

        Private Shared Function Parent_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            Dim Self = DirectCast(D, NavigationCommand)

            If Self.ParentType IsNot Nothing Then
                Return Nothing
            End If

            Return BaseValue
        End Function

        Public Property Parent As NavigationViewModel
            Get
                Return DirectCast(Me.GetValue(ParentProperty), NavigationViewModel)
            End Get
            Set(ByVal value As NavigationViewModel)
                Me.SetValue(ParentProperty, value)
            End Set
        End Property
#End Region

#Region "KsApplication Property"
        Public Shared ReadOnly KsApplicationProperty As DependencyProperty = DependencyProperty.Register("KsApplication", GetType(KsApplication), GetType(NavigationCommand), New PropertyMetadata(Nothing))

        Public Property KsApplication As KsApplication
            Get
                Return DirectCast(Me.GetValue(KsApplicationProperty), KsApplication)
            End Get
            Set(ByVal value As KsApplication)
                Me.SetValue(KsApplicationProperty, value)
            End Set
        End Property
#End Region

    End Class

End Namespace
