Imports System.ComponentModel

Namespace Common.MVVM

    Public Class BindableBase
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Function SetProperty(Of T)(ByRef Source As T, ByVal Value As T, <Runtime.CompilerServices.CallerMemberName()> Optional ByVal PropertyName As String = Nothing) As Boolean
            If Not Object.Equals(Source, Value) Then
                Source = Value
                Me.NotifyPropertyChanged(PropertyName)
                Return True
            End If

            Return False
        End Function

        Protected Overridable Sub OnPropertyChanged(ByVal E As PropertyChangedEventArgs)
            RaiseEvent PropertyChanged(Me, E)
        End Sub

        Protected Sub NotifyPropertyChanged(<Runtime.CompilerServices.CallerMemberName()> Optional ByVal PropertyName As String = Nothing)
            Me.OnPropertyChanged(New PropertyChangedEventArgs(PropertyName))
        End Sub

    End Class

End Namespace
