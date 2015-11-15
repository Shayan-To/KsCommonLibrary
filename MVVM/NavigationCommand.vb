Imports System.Windows.Input

Namespace MVVM

    Public Class NavigationCommand
        Implements ICommand

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            Dim KsApplication = Me.GetKsApplication()
            KsApplication.NavigateTo(Me.GetParent(), KsApplication.GetViewModel(Me.ViewType), Me.AddToStack)
        End Sub

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return True
        End Function

        Private Function GetParent() As NavigationViewModel
            If Me._Parent IsNot Nothing Then
                Return Me._Parent
            End If
            If Me._ParentType IsNot Nothing Then
                Return DirectCast(Me.GetKsApplication().GetViewModel(Me._ParentType), NavigationViewModel)
            End If
            Return Me.GetKsApplication().Window
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

#Region "ViewType Property"
        Private _ViewType As Type

        Public Property ViewType As Type
            Get
                Return Me._ViewType
            End Get
            Set(ByVal Value As Type)
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
                If Me._Parent IsNot Nothing And Value IsNot Nothing Then
                    Throw New InvalidOperationException("Cannot set both Parent and ParentType.")
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
                If Me._ParentType IsNot Nothing And Value IsNot Nothing Then
                    Throw New InvalidOperationException("Cannot set both Parent and ParentType.")
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

        Private Function GetKsApplication() As KsApplication
            If Me._KsApplication Is Nothing Then
                Return Me._KsApplication
            End If
            Return KsApplication.Current
        End Function
#End Region

    End Class

End Namespace
