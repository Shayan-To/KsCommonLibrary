Imports System.Runtime.CompilerServices
Imports System.Text
Imports Media = System.Windows.Media
Imports Reflect = System.Reflection
Imports SIO = System.IO

Namespace Common

    Partial Class Utilities

        Public Class Collections

            Public Shared Iterator Function Concat(Of T)(ByVal Collections As IEnumerable(Of IEnumerable(Of T))) As IEnumerable(Of T)
                For Each L In Collections
                    For Each I In L
                        Yield I
                    Next
                Next
            End Function

            Public Shared Function Concat(Of T)(ParamArray ByVal Collections As IEnumerable(Of T)()) As IEnumerable(Of T)
                Return Concat(DirectCast(Collections, IEnumerable(Of IEnumerable(Of T))))
            End Function

            Public Shared Iterator Function Range(ByVal Start As Integer, ByVal Length As Integer, Optional ByVal [Step] As Integer = 1) As IEnumerable(Of Integer)
                Verify.TrueArg([Step] <> 0, "Step")
                For Start = Start To Start + Length - If(Length < 0, -1, 1) Step [Step]
                    Yield Start
                Next
            End Function

            Public Shared Function Range(ByVal Length As Integer) As IEnumerable(Of Integer)
                Return Range(0, Length)
            End Function

            Public Shared Iterator Function Repeat(Of T)(ByVal I1 As T, ByVal Count As Integer) As IEnumerable(Of T)
                For I As Integer = 1 To Count
                    Yield I1
                Next
            End Function

            Public Shared Iterator Function InfiniteEnumerable() As IEnumerable(Of Void)
                Do
                    Yield Nothing
                Loop
            End Function

            Public Shared Function Join(Of T, TKey)(ByVal Items1 As IEnumerable(Of T),
                                                    ByVal Items2 As IEnumerable(Of T),
                                                    ByVal KeySelector As Func(Of T, TKey),
                                                    ByVal JoinType As JoinDirection) As JoinElement(Of T, T, TKey)()
                Return Join(Items1, Items2, KeySelector, KeySelector, JoinType, EqualityComparer(Of TKey).Default)
            End Function

            Public Shared Function Join(Of T, TKey)(ByVal Items1 As IEnumerable(Of T),
                                                    ByVal Items2 As IEnumerable(Of T),
                                                    ByVal KeySelector As Func(Of T, TKey),
                                                    ByVal JoinType As JoinDirection,
                                                    ByVal Comparer As IEqualityComparer(Of TKey)) As JoinElement(Of T, T, TKey)()
                Return Join(Items1, Items2, KeySelector, KeySelector, JoinType, Comparer)
            End Function

            Public Shared Function Join(Of T1, T2, TKey)(ByVal Items1 As IEnumerable(Of T1),
                                                         ByVal Items2 As IEnumerable(Of T2),
                                                         ByVal KeySelector1 As Func(Of T1, TKey),
                                                         ByVal KeySelector2 As Func(Of T2, TKey),
                                                         ByVal JoinType As JoinDirection) As JoinElement(Of T1, T2, TKey)()
                Return Join(Items1, Items2, KeySelector1, KeySelector2, JoinType, EqualityComparer(Of TKey).Default)
            End Function

            Public Shared Function Join(Of T1, T2, TKey)(ByVal Items1 As IEnumerable(Of T1),
                                                         ByVal Items2 As IEnumerable(Of T2),
                                                         ByVal KeySelector1 As Func(Of T1, TKey),
                                                         ByVal KeySelector2 As Func(Of T2, TKey),
                                                         ByVal JoinType As JoinDirection,
                                                         ByVal Comparer As IEqualityComparer(Of TKey)) As JoinElement(Of T1, T2, TKey)()
                Dim Dic = New Dictionary(Of TKey, T2)(Comparer)
                For Each I In Items2
                    Dic.Add(KeySelector2.Invoke(I), I)
                Next

                Dim Res = New List(Of JoinElement(Of T1, T2, TKey))()

                For Each I1 In Items1
                    Dim Key = KeySelector1.Invoke(I1)
                    Dim I2 As T2 = Nothing
                    If Dic.TryGetValue(Key, I2) Then
                        Res.Add(New JoinElement(Of T1, T2, TKey)(Key, JoinDirection.Both, I1, I2))
                        Dic.Remove(Key)
                    ElseIf (JoinType And JoinDirection.Left) = JoinDirection.Left Then
                        Res.Add(New JoinElement(Of T1, T2, TKey)(Key, JoinDirection.Left, I1, Nothing))
                    End If
                Next

                If (JoinType And JoinDirection.Right) = JoinDirection.Right Then
                    For Each KI2 In Dic
                        Res.Add(New JoinElement(Of T1, T2, TKey)(KI2.Key, JoinDirection.Left, Nothing, KI2.Value))
                    Next
                End If

                Return Res.ToArray()
            End Function

#Region "InEnumerable Group"
            Public Shared Iterator Function InEnumerable(Of T)() As IEnumerable(Of T)
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T) As IEnumerable(Of T)
                Yield I1
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T, ByVal I2 As T) As IEnumerable(Of T)
                Yield I1
                Yield I2
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T, ByVal I2 As T, ByVal I3 As T) As IEnumerable(Of T)
                Yield I1
                Yield I2
                Yield I3
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T, ByVal I2 As T, ByVal I3 As T, ByVal I4 As T) As IEnumerable(Of T)
                Yield I1
                Yield I2
                Yield I3
                Yield I4
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T, ByVal I2 As T, ByVal I3 As T, ByVal I4 As T, ByVal I5 As T) As IEnumerable(Of T)
                Yield I1
                Yield I2
                Yield I3
                Yield I4
                Yield I5
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T, ByVal I2 As T, ByVal I3 As T, ByVal I4 As T, ByVal I5 As T, ByVal I6 As T) As IEnumerable(Of T)
                Yield I1
                Yield I2
                Yield I3
                Yield I4
                Yield I5
                Yield I6
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T, ByVal I2 As T, ByVal I3 As T, ByVal I4 As T, ByVal I5 As T, ByVal I6 As T, ByVal I7 As T) As IEnumerable(Of T)
                Yield I1
                Yield I2
                Yield I3
                Yield I4
                Yield I5
                Yield I6
                Yield I7
            End Function

            Public Shared Iterator Function InEnumerable(Of T)(ByVal I1 As T, ByVal I2 As T, ByVal I3 As T, ByVal I4 As T, ByVal I5 As T, ByVal I6 As T, ByVal I7 As T, ByVal I8 As T) As IEnumerable(Of T)
                Yield I1
                Yield I2
                Yield I3
                Yield I4
                Yield I5
                Yield I6
                Yield I7
                Yield I8
            End Function
#End Region

        End Class

    End Class

End Namespace
