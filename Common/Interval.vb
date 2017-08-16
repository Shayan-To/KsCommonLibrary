Namespace Common

    Public Structure Interval

        Public Sub New(ByVal Start As Double, ByVal IsStartInclusive As Boolean,
                       ByVal [End] As Double, ByVal IsEndInclusive As Boolean)
            Me._Start = Start
            Me._End = [End]
            Me._IsStartInclusive = IsStartInclusive
            Me._IsEndInclusive = IsEndInclusive
        End Sub

        Public Function Compare(ByVal Value As Double) As Integer
            If Value < Me.Start Then
                Return -1
            End If
            If Me.End < Value Then
                Return 1
            End If
            If Me.Start = Value And Not Me.IsStartInclusive Then
                Return -1
            End If
            If Value = Me.End And Not Me.IsEndInclusive Then
                Return 1
            End If
            Return 0
        End Function

#Region "Start Read-Only Property"
        Private ReadOnly _Start As Double

        Public ReadOnly Property Start As Double
            Get
                Return Me._Start
            End Get
        End Property
#End Region

#Region "End Read-Only Property"
        Private ReadOnly _End As Double

        Public ReadOnly Property [End] As Double
            Get
                Return Me._End
            End Get
        End Property
#End Region

#Region "IsStartInclusive Read-Only Property"
        Private ReadOnly _IsStartInclusive As Boolean

        Public ReadOnly Property IsStartInclusive As Boolean
            Get
                Return Me._IsStartInclusive
            End Get
        End Property
#End Region

#Region "IsEndInclusive Read-Only Property"
        Private ReadOnly _IsEndInclusive As Boolean

        Public ReadOnly Property IsEndInclusive As Boolean
            Get
                Return Me._IsEndInclusive
            End Get
        End Property
#End Region

    End Structure

End Namespace
