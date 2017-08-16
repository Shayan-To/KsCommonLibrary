Namespace Common

    Public Class ConsoleProgressDrawer

        ' ToDo Add support for unavailable total.

        Public Sub New(Optional ByVal SpeedSupported As Boolean = True)
            If SpeedSupported Then
                Me.StopWatch = New Stopwatch()
            End If
            Me.ShowSpeed = SpeedSupported
        End Sub

        Private Sub Draw(ByVal TextBefore As String, ByVal TextAfter As String)
            If Me.IsSingleLine Then
                Console.Write(ControlChars.Cr)
            End If

            Console.Write(TextBefore)

            Console.Write("["c)
            If Total = -1 Then
                Console.Write("...")
            Else
                Dim Width = Console.WindowWidth - 1
                Dim ProgWidth = Math.Min(Width - TextBefore.Length - TextAfter.Length - 2, 100)
                Dim ProgFull = CInt((ProgWidth / Me.Total) * Me.Amount)
                Dim ProgEmp = ProgWidth - ProgFull

                Console.Write(New String("#"c, ProgFull))
                Console.Write(New String(" "c, ProgEmp))
            End If
            Console.Write("]"c)

            Console.Write(TextAfter)
        End Sub

        Private Function GetString(ByVal Value As Double) As String
            If Double.IsNaN(Value) Then
                Return "   ???  "
            End If

            If Not Me.AddMultiplier Then
                Return Value.ToString("F2").PadLeft(6) + "  "
            End If

            Dim Rep = Utilities.Representation.GetPrefixedRepresentation(Value, Utilities.Representation.BinaryPrefixes)

            Return String.Format("{0,6:F2} {1,1}", Rep.Value, Rep.Prefix)
        End Function

        Private Sub Draw()
            If Not Me.IsDrawing Then
                Exit Sub
            End If

            Dim T = New Text.StringBuilder(Me.Text)

            If Me.ShowAmount Or Me.ShowTotal Then
                If T.Length <> 0 Then
                    T.Append(" ")
                End If
                T.Append("(")
                If Me.ShowAmount Then
                    T.Append(Me.GetString(Me.Amount))
                    If Me.ShowTotal Then
                        T.Append(" / ").Append(Me.GetString(Me.Total))
                    End If
                Else
                    T.Append("Total ").Append(Me.GetString(Me.Total))
                End If
                T.Append(")")
            End If

            If T.Length <> 0 And Me.ShowProgressBar Then
                T.Append(" ")
            End If

            Dim Bef = T.ToString()

            T.Clear()

            If Bef.Length <> 0 Then
                T.Append(" ")
            End If

            If Me.ShowPercentage Then
                Dim Per = Me.Amount / Me.Total * 100
                Dim Tmp = Per.ToString("F2").PadLeft(5)
                If Tmp.Length > 5 Then
                    Tmp = Tmp.Substring(0, 5)
                End If
                T.Append(Tmp).Append("%")
            End If

            If Me.ShowSpeed Then
                If T.Length <> 0 Then
                    T.Append(" ")
                End If
                T.AppendFormat("at ") _
                 .Append(Me.GetString(Me.Amount / Me.StopWatch.Elapsed.TotalSeconds)) _
                 .Append("/s")
            End If

            Dim Aft = T.ToString()

            Me.Draw(Bef, Aft)
        End Sub

        Public Sub ReportProgress(ByVal Amount As Long)
            Me.Amount = Amount
        End Sub

        Public Sub Reset()
            Me.Amount = 0
            Me.StopWatch?.Reset()
        End Sub

        Public Sub StartDrawing()
            Me.IsDrawing = True
        End Sub

        Public Sub StopDrawing()
            Me.IsDrawing = False
        End Sub

#Region "Text Property"
        Private _Text As String

        Public Property Text As String
            Get
                Return Me._Text
            End Get
            Set(ByVal Value As String)
                Me._Text = Value
                Me.Draw()
            End Set
        End Property
#End Region

#Region "ShowSpeed Property"
        Private _ShowSpeed As Boolean = True

        Public Property ShowSpeed As Boolean
            Get
                Return Me._ShowSpeed
            End Get
            Set(ByVal Value As Boolean)
                Verify.False(Value And Me.StopWatch Is Nothing, "Speed is not supported.")
                Me._ShowSpeed = Value
                Me.Draw()
            End Set
        End Property
#End Region

#Region "ShowAmount Property"
        Private _ShowAmount As Boolean = True

        Public Property ShowAmount As Boolean
            Get
                Return Me._ShowAmount
            End Get
            Set(ByVal Value As Boolean)
                Me._ShowAmount = Value
                Me.Draw()
            End Set
        End Property
#End Region

#Region "ShowTotal Property"
        Private _ShowTotal As Boolean = True

        Public Property ShowTotal As Boolean
            Get
                Return Me._ShowTotal
            End Get
            Set(ByVal Value As Boolean)
                Me._ShowTotal = Value
                Me.Draw()
            End Set
        End Property
#End Region

#Region "ShowPercentage Property"
        Private _ShowPercentage As Boolean = True

        Public Property ShowPercentage As Boolean
            Get
                Return Me._ShowPercentage
            End Get
            Set(ByVal Value As Boolean)
                Me._ShowPercentage = Value
                Me.Draw()
            End Set
        End Property
#End Region

#Region "ShowProgressBar Property"
        Private _ShowProgressBar As Boolean = True

        Public Property ShowProgressBar As Boolean
            Get
                Return Me._ShowProgressBar
            End Get
            Set(ByVal Value As Boolean)
                If Not Me.IsSingleLine Then
                    Value = False
                End If
                Me._ShowProgressBar = Value
                Me.Draw()
            End Set
        End Property
#End Region

#Region "AddMultiplier Property"
        Private _AddMultiplier As Boolean = True

        Public Property AddMultiplier As Boolean
            Get
                Return Me._AddMultiplier
            End Get
            Set(ByVal Value As Boolean)
                Me._AddMultiplier = Value
                Me.Draw()
            End Set
        End Property
#End Region

#Region "Amount Property"
        Private _Amount As Long

        Public Property Amount As Long
            Get
                Return Me._Amount
            End Get
            Private Set(ByVal Value As Long)
                Me._Amount = Value
                If Not Me.StopWatch.IsRunning Then
                    Me.StopWatch.Start()
                End If
                Me.Draw()
            End Set
        End Property
#End Region

#Region "Total Property"
        Private _Total As Long = 100

        Public Property Total As Long
            Get
                Return Me._Total
            End Get
            Set(ByVal Value As Long)
                Me._Total = Value
                Me.Draw()
            End Set
        End Property
#End Region

#Region "IsSingleLine Property"
        Private _IsSingleLine As Boolean = True

        Public Property IsSingleLine As Boolean
            Get
                Return Me._IsSingleLine
            End Get
            Set(ByVal Value As Boolean)
                Me._IsSingleLine = Value
                If Not Value Then
                    Me.ShowProgressBar = False
                Else
                    Me.Draw()
                End If
            End Set
        End Property
#End Region

#Region "IsDrawing Property"
        Private _IsDrawing As Boolean = False

        Public Property IsDrawing As Boolean
            Get
                Return Me._IsDrawing
            End Get
            Set(ByVal Value As Boolean)
                If Me._IsDrawing <> Value Then
                    Me._IsDrawing = Value

                    If Value Then
                        Me.Draw()
                    Else
                        If Me.IsSingleLine Then
                            Console.WriteLine()
                        End If
                    End If
                End If
            End Set
        End Property
#End Region

        Private ReadOnly StopWatch As Stopwatch

    End Class

End Namespace
