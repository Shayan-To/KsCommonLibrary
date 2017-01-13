Namespace Common

    Public Structure Lazy(Of TRes)

        Public Sub New(ByVal Func As Func(Of TRes))
            Me._Func = Func
        End Sub

        Public Sub Reset()
            'Console.WriteLine("Resetting...")
            Me.ValueCalculated = False
        End Sub

#Region "Func Property"
        Private ReadOnly _Func As Func(Of TRes)

        Public ReadOnly Property Func As Func(Of TRes)
            Get
                Return Me._Func
            End Get
        End Property
#End Region

#Region "Value Property"
        Private _Value As TRes
        Private ValueCalculated As Boolean

        Public ReadOnly Property Value As TRes
            Get
                If Not Me.ValueCalculated Then
                    Me._Value = Me._Func.Invoke()
                    Me.ValueCalculated = True
                End If

                Return Me._Value
            End Get
        End Property
#End Region

    End Structure

End Namespace
