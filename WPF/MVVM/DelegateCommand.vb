Imports System.Windows.Input

Namespace Common.MVVM

    Public Class DelegateCommand
        Implements ICommand

        Public Sub New(ByVal ExecuteFunc As Action)
            Me._ExecuteFunc = Sub(O) ExecuteFunc.Invoke()
            Me._CanExecuteFunc = Nothing
        End Sub

        Public Sub New(ByVal ExecuteFunc As Action, ByVal CanExecuteFunc As Func(Of Boolean))
            Me._ExecuteFunc = Sub(O) ExecuteFunc.Invoke()
            Me._CanExecuteFunc = Function(O) CanExecuteFunc.Invoke()
        End Sub

        Public Sub New(ByVal ExecuteFunc As Action(Of Object))
            Me._ExecuteFunc = ExecuteFunc
            Me._CanExecuteFunc = Nothing
        End Sub

        Public Sub New(ByVal ExecuteFunc As Action(Of Object), ByVal CanExecuteFunc As Func(Of Object, Boolean))
            Me._ExecuteFunc = ExecuteFunc
            Me._CanExecuteFunc = CanExecuteFunc
        End Sub

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            Me._ExecuteFunc.Invoke(parameter)
        End Sub

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return If(Me._CanExecuteFunc Is Nothing, True, Me._CanExecuteFunc.Invoke(parameter))
        End Function

        Private ReadOnly _ExecuteFunc As Action(Of Object)
        Private ReadOnly _CanExecuteFunc As Func(Of Object, Boolean)

    End Class

End Namespace
