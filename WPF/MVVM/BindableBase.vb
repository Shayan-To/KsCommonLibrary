Imports System.ComponentModel

Namespace Common.MVVM

    Public Class BindableBase
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

#If VBC_VER >= 11.0 Then
        Protected Function SetProperty(Of T)(ByRef Source As T, ByVal Value As T, <Runtime.CompilerServices.CallerMemberName()> Optional ByVal PropertyName As String = Nothing) As Boolean
#Else
        Protected Function SetProperty(Of T)(ByRef Source As T, ByVal Value As T, ByVal PropertyName As String) As Boolean
#End If
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

#If VBC_VER >= 11.0 Then
        Protected Sub NotifyPropertyChanged(<Runtime.CompilerServices.CallerMemberName()> Optional ByVal PropertyName As String = Nothing)
#Else
        Protected Sub NotifyPropertyChanged(ByVal PropertyName As String)
#End If
            Me.OnPropertyChanged(New PropertyChangedEventArgs(PropertyName))
        End Sub

    End Class

End Namespace
