Namespace Common.MVVM

    Public Class NavigationFrame
        Implements IEnumerable(Of ViewModel)

        Public Sub New(ByVal List As IEnumerable(Of ViewModel))
            Me.List = List.ToArray()
        End Sub

        Public Function GetEnumerator() As IEnumerator(Of ViewModel) Implements IEnumerable(Of ViewModel).GetEnumerator
            Return DirectCast(Me.List, IEnumerable(Of ViewModel)).GetEnumerator()
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function

        Public Function AddViewModel(ByVal ViewModel As ViewModel) As NavigationFrame
            Verify.True(Me.IsOpenEnded, "Cannot add a view-model to a non-open-ended frame.")
            Return New NavigationFrame(Me.List.Concat(ViewModel))
        End Function

        Public Function SubFrame(ByVal Length As Integer) As NavigationFrame
            Return New NavigationFrame(Me.List.Take(Length))
        End Function

        Public Function IndexOf(ByVal ViewModel As ViewModel) As Integer
            For I As Integer = 0 To Me.Count - 1
                If Me.Item(I) Is ViewModel Then
                    Return I
                End If
            Next
            Return -1
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            Dim Frame = TryCast(obj, NavigationFrame)
            If obj Is Nothing Then
                Return False
            End If
            Return Me = Frame
        End Function

        Public Shared Operator =(ByVal Left As NavigationFrame, ByVal Right As NavigationFrame) As Boolean
            If Left Is Nothing Then
                Return Right Is Nothing
            End If
            If Right Is Nothing Then
                Return False
            End If

            If Left.List.Length <> Right.List.Length Then
                Return False
            End If

            For I As Integer = 0 To Left.List.Length - 1
                If Left.List(I) IsNot Right.List(I) Then
                    Return False
                End If
            Next

            Return True
        End Operator

        Public Shared Operator <>(ByVal Left As NavigationFrame, ByVal Right As NavigationFrame) As Boolean
            Return Not Left = Right
        End Operator

#Region "IsOpenEnded Read-Only Property"
        Public ReadOnly Property IsOpenEnded As Boolean
            Get
                Return Me.Tip.IsNavigation()
            End Get
        End Property
#End Region

#Region "Tip Read-Only Property"
        Public ReadOnly Property Tip As ViewModel
            Get
                Return Me.Item(Me.Count - 1)
            End Get
        End Property
#End Region

#Region "Parents Read-Only Property"
        Public ReadOnly Property Parents(ByVal Index As Integer) As NavigationViewModel
            Get
                Return DirectCast(Me.Item(Me.Count - 2 - Index), NavigationViewModel)
            End Get
        End Property
#End Region

#Region "Item Read-Only Property"
        Public ReadOnly Property Item(Index As Integer) As ViewModel
            Get
                Return Me.List(Index)
            End Get
        End Property
#End Region

#Region "Count Read-Only Property"
        Public ReadOnly Property Count As Integer
            Get
                Return Me.List.Length
            End Get
        End Property
#End Region

        Private ReadOnly List As ViewModel()

    End Class

End Namespace
