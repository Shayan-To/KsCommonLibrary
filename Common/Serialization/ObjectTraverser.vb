Namespace Common

    Public Class ObjectTraverser

    End Class

    Public Class ObjectProxy

        Public Sub Reset()
            Me.Container = Nothing
            Me.Type = Nothing
        End Sub

        Public Sub [Set](Of T)(ByVal Value As T)
            Verify.True(Me.Type Is Nothing And Me.Container Is Nothing, "Reset before setting.")
            Me.Type = GetType(T)

            Dim C As ObjectContainer(Of T) = Nothing
            If Me.Containers.TryGetValue(Me.Type, Me.Container) Then
                C = DirectCast(Me.Container, ObjectContainer(Of T))
            Else
                C = New ObjectContainer(Of T)()
                Me.Container = C
                Me.Containers.Add(Me.Type, Me.Container)
            End If

            C.Value = Value
        End Sub

        Public Function [Get](Of T)() As T
            Verify.True(Me.Type IsNot Nothing And Me.Container IsNot Nothing, "Set before getting.")
            Verify.True(Me.Type = GetType(T), "The type set is different.")

            Me.Type = GetType(T)
            Dim C = DirectCast(Me.Container, ObjectContainer(Of T))

            Return C.Value
        End Function

        Private Type As Type
        Private Container As Object
        Private ReadOnly Containers As Dictionary(Of Type, Object) = New Dictionary(Of Type, Object)()

        Private Class ObjectContainer(Of T)

#Region "Value Property"
            Private _Value As T

            Public Property Value As T
                Get
                    Return Me._Value
                End Get
                Set(ByVal Value As T)
                    Me._Value = Value
                End Set
            End Property
#End Region

        End Class

    End Class

End Namespace
