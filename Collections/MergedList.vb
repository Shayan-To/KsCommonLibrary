Namespace Common

    Public Class MergedList(Of T)
        Inherits BaseReadOnlyList(Of T)

        Public Sub New(ByVal Lists As IEnumerable(Of IReadOnlyList(Of T)))
            Me.Lists = Lists.ToArray()
        End Sub

        Public Sub New(ParamArray ByVal Lists As IReadOnlyList(Of T)())
            Me.New(DirectCast(Lists, IEnumerable(Of IReadOnlyList(Of T))))
        End Sub

        Public Overrides ReadOnly Property Count As Integer
            Get
                Return Me.Lists.Sum(Function(L) L.Count)
            End Get
        End Property

        Default Public Overrides ReadOnly Property Item(Index As Integer) As T
            Get
                For Each L In Me.Lists
                    Dim Count = L.Count
                    If Index < Count Then
                        Return L.Item(Index)
                    End If
                    Index -= Count
                Next
                Throw New ArgumentOutOfRangeException(NameOf(Index))
            End Get
        End Property

        Public Overrides Sub CopyTo(array() As T, arrayIndex As Integer)
            For Each L In Me.Lists
                L.CopyTo(array, arrayIndex)
                arrayIndex += L.Count
            Next
        End Sub

        Public Overrides Function IndexOf(item As T) As Integer
            Dim I = 0
            For Each L In Me.Lists
                For Each It In L
                    If It.Equals(item) Then
                        Return I
                    End If
                    I += 1
                Next
            Next
            Return -1
        End Function

        Public Overrides Iterator Function GetEnumerator() As IEnumerator(Of T)
            For Each L In Me.Lists
                For Each I In L
                    Yield I
                Next
            Next
        End Function

        Private ReadOnly Lists As IReadOnlyList(Of T)()

    End Class

End Namespace
