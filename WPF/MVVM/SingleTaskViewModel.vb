Namespace Common.MVVM

    Public MustInherit Class SingleTaskViewModel
        Inherits ViewModel

        Public Sub New(ByVal KsApplication As KsApplication)
            MyBase.New(KsApplication)
        End Sub

        Public Sub New()
            MyBase.New()
        End Sub

        Protected Friend Overrides Sub OnNavigatedTo(E As NavigationEventArgs)
            If E.NavigationType = NavigationType.NewNavigation Then
                Me.IsWorkDone = False
            End If
        End Sub

        Protected Friend Overrides Sub OnNavigatedFrom(E As NavigationEventArgs)
            If E.NavigationType = NavigationType.NewNavigation Then
                Me.IsWorkDone = True
            End If
        End Sub

        Public Function WhenWorkDone() As Task
            If Me.WhenWorkDoneTaskSource Is Nothing Then
                Me.WhenWorkDoneTaskSource = New TaskCompletionSource(Of Void)()
                If Me.IsWorkDone Then
                    Me.WhenWorkDoneTaskSource.SetResult(Nothing)
                End If
            End If

            Return Me.WhenWorkDoneTaskSource.Task
        End Function

#Region "IsWorkDone Property"
        Private _IsWorkDone As Boolean

        Public Property IsWorkDone As Boolean
            Get
                Return Me._IsWorkDone
            End Get
            Protected Set(ByVal Value As Boolean)
                If Me.SetProperty(Me._IsWorkDone, Value) Then
                    If Value Then
                        Me.WhenWorkDoneTaskSource?.SetResult(Nothing)
                    Else
                        Me.WhenWorkDoneTaskSource = Nothing
                    End If
                End If
            End Set
        End Property
#End Region

        Private WhenWorkDoneTaskSource As TaskCompletionSource(Of Void)

    End Class

End Namespace
