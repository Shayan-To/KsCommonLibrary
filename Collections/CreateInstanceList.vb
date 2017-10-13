Namespace Common

    Public NotInheritable Class CreateInstanceList

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

        Public Shared Function Create(Of T)(ByVal List As IList(Of T), ByVal Creator As Func(Of Integer, T)) As CreateInstanceList(Of T)
            Return New CreateInstanceList(Of T)(List, Creator)
        End Function

        Public Shared Function Create(Of T)(ByVal Creator As Func(Of Integer, T)) As CreateInstanceList(Of T)
            Return New CreateInstanceList(Of T)(Creator)
        End Function

        Public Shared Function Create(Of T As New)(ByVal List As IList(Of T)) As CreateInstanceList(Of T)
            Return New CreateInstanceList(Of T)(List, Function(I) New T())
        End Function

        Public Shared Function Create(Of T As New)() As CreateInstanceList(Of T)
            Return New CreateInstanceList(Of T)(Function(I) New T())
        End Function

    End Class

    Public Class CreateInstanceList(Of T)
        Inherits BaseList(Of T)

        Public Sub New(ByVal List As IList(Of T), ByVal Creator As Func(Of Integer, T))
            Me.List = List
            Me.Creator = Creator
        End Sub

        Public Sub New(ByVal Creator As Func(Of Integer, T))
            Me.New(New List(Of T)(), Creator)
        End Sub

        Public Overrides Sub Clear()
            Me.List.Clear()
        End Sub

        Public Overrides ReadOnly Property Count As Integer
            Get
                Return Me.List.Count
            End Get
        End Property

        Public Overrides Sub Insert(ByVal Index As Integer, ByVal Item As T)
            Me.List.Insert(Index, Item)
        End Sub

        Public Overloads Sub Insert(ByVal Index As Integer)
            Me.Insert(Index, Me.Creator.Invoke(Index))
        End Sub

        Default Public Overrides Property Item(ByVal Index As Integer) As T
            Get
                If Index = Me.List.Count Then
                    Dim V = Me.Creator.Invoke(Index)
                    Me.List.Add(V)
                    Return V
                End If
                Return Me.List.Item(Index)
            End Get
            Set(ByVal Value As T)
                If Index = Me.List.Count Then
                    Me.List.Add(Value)
                Else
                    Me.List.Item(Index) = Value
                End If
            End Set
        End Property

        Public Overrides Sub RemoveAt(ByVal Index As Integer)
            Me.List.RemoveAt(Index)
        End Sub

        Public Function GetEnumerator() As IEnumerator(Of T)
            Return Me.List.GetEnumerator()
        End Function

        Protected Overrides Function IEnumerable_1_GetEnumerator() As IEnumerator(Of T)
            Return Me.GetEnumerator()
        End Function

        Private ReadOnly Creator As Func(Of Integer, T)
        Private ReadOnly List As IList(Of T)

    End Class

End Namespace
