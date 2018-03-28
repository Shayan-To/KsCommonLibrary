Namespace Common

    Public Class Timer

        Public Sub New(ByVal Callback As Action)
            Me.Callback = Callback
        End Sub

        Public Async Sub Start()
            Verify.False(Me.IsRunning, "Cannot start an already started timer.")
            Me._IsRunning = True

            If Me.RunAtBeginning Then
                Me.Callback.Invoke()
            End If

            Do
                Await Task.Delay(Me.Interval)
                If Not Me.IsRunning Then
                    Exit Do
                End If
                Me.Callback.Invoke()
            Loop
        End Sub

        Public Sub [Stop]()
            Me._IsRunning = False
        End Sub

#Region "Interval Property"
        Private _Interval As TimeSpan

        Public Property Interval As TimeSpan
            Get
                Return Me._Interval
            End Get
            Set(ByVal Value As TimeSpan)
                Me._Interval = Value
            End Set
        End Property
#End Region

#Region "IsRunning Property"
        Private _IsRunning As Boolean

        Public Property IsRunning As Boolean
            Get
                Return Me._IsRunning
            End Get
            Set(ByVal Value As Boolean)
                If Value <> Me._IsRunning Then
                    If Value Then
                        Me.Start()
                    Else
                        Me.Stop()
                    End If
                End If
            End Set
        End Property
#End Region

#Region "RunAtBeginning Property"
        Private _RunAtBeginning As Boolean

        Public Property RunAtBeginning As Boolean
            Get
                Return Me._RunAtBeginning
            End Get
            Set(ByVal Value As Boolean)
                Me._RunAtBeginning = Value
            End Set
        End Property
#End Region

        Private ReadOnly Callback As Action

    End Class

End Namespace
