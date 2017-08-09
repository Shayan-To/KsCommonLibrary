Namespace Common

    Public Class EnumerableCacher(Of T)
        Inherits BaseReadOnlyList(Of T)

        Public Sub New(ByVal Enumerable As IEnumerable(Of T))
            Me.Enumerator = Enumerable.GetEnumerator()
        End Sub

        Public Overrides ReadOnly Property Count As Integer
            Get
                Me.TryGetValue(Integer.MaxValue, Nothing)
                Return Me.List.Count
            End Get
        End Property

        Default Public Overrides ReadOnly Property Item(Index As Integer) As T
            Get
                Dim Res As T = Nothing
                Verify.TrueArg(Me.TryGetValue(Index, Res), NameOf(Index), "Index out of range.")
                Return Res
            End Get
        End Property

        Public Overrides Iterator Function GetEnumerator() As IEnumerator(Of T)
            Dim I = 0
            Dim T As T = Nothing
            Do While Me.TryGetValue(I, T)
                Yield T
                I += 1
            Loop
        End Function

        Public Function TryGetValue(ByVal Index As Integer, ByRef Value As T) As Boolean
            For I = Me.List.Count To Index
                If Not Me.Enumerator.MoveNext() Then
                    Return False
                End If
                Me.List.Add(Me.Enumerator.Current)
            Next

            Value = Me.List.Item(Index)
            Return True
        End Function

        Private ReadOnly Enumerator As IEnumerator(Of T)
        Private ReadOnly List As List(Of T) = New List(Of T)()

    End Class

End Namespace
