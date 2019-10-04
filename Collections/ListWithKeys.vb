Namespace Common

    Public Class ListWithKeys(Of T)
        Inherits BaseList(Of T)

        Default Public Overrides Property Item(index As Integer) As T
            Get
                Return Me.List.Item(index)
            End Get
            Set(value As T)
                Dim Elem = Me.List.Item(index)
                Me.List.Item(index) = value

                For Each KInd In Me.Indexes
                    Dim Key = KInd.Key.KeySelector.Invoke(Elem)
                    KInd.Value.Item(Key).Remove(Elem)
                Next
                For Each KInd In Me.Indexes
                    Dim Key = KInd.Key.KeySelector.Invoke(value)
                    KInd.Value.Item(Key).Add(value)
                Next
            End Set
        End Property

        Public Overrides ReadOnly Property Count As Integer
            Get
                Return Me.List.Count
            End Get
        End Property

        Public Overrides Sub Insert(index As Integer, item As T)
            Me.List.Insert(index, item)
            For Each KInd In Me.Indexes
                Dim Key = KInd.Key.KeySelector.Invoke(item)
                KInd.Value.Item(Key).Add(item)
            Next
        End Sub

        Public Overrides Sub RemoveAt(index As Integer)
            Dim Elem = Me.List.Item(index)
            Me.List.RemoveAt(index)
            For Each KInd In Me.Indexes
                Dim Key = KInd.Key.KeySelector.Invoke(Elem)
                KInd.Value.Item(Key).Remove(Elem)
            Next
        End Sub

        Public Overrides Sub Clear()
            Me.List.Clear()
            For Each KInd In Me.Indexes
                KInd.Value.Clear()
            Next
        End Sub

        Protected Overrides Function IEnumerable_1_GetEnumerator() As IEnumerator(Of T)
            Return Me.GetEnumerator()
        End Function

        Public Function GetEnumerator() As List(Of T).Enumerator
            Return Me.List.GetEnumerator()
        End Function

        Private Function GetIndexList(ByVal Condition As Condition) As List(Of T)
            Dim Index = Me.Indexes.Item(Condition.Key)
            Dim Res As List(Of T) = Nothing
            Index.TryGetValue(Condition.KeyValue, Res)
            Return Res
        End Function

        Private Function GetIndexList(Of TK)(ByVal Condition As Condition(Of TK)) As List(Of T)
            Dim Index = Me.Indexes.Item(Condition.Key)
            Dim Res As List(Of T) = Nothing
            Index.TryGetValue(Condition.KeyValue, Res)
            Return Res
        End Function

        ''' <example>
        ''' List.GetAllWhere(Key1 = "Value1")
        ''' </example>
        Public Function GetAllWhere(Of TK)(ByVal Condition As Condition(Of TK)) As IEnumerable(Of T)
            Dim Res = Me.GetIndexList(Condition)
            If Res Is Nothing Then
                Return Enumerable.Empty(Of T)()
            End If
            Return Res.ToEnumerable()
        End Function

        ''' <example>
        ''' List.GetOneWhere(Key1 = "Value1")
        ''' </example>
        Public Function GetOneWhere(Of TK)(ByVal Condition As Condition(Of TK)) As T
            Dim Res = Me.GetIndexList(Condition)
            Verify.False(Res Is Nothing OrElse Res.Count = 0, "No value with such condition was found.")
            Return Res.Item(0)
        End Function

        ''' <example>
        ''' List.GetSingleWhere(Key1 = "Value1")
        ''' </example>
        Public Function GetSingleWhere(Of TK)(ByVal Condition As Condition(Of TK)) As T
            Dim Res = Me.GetIndexList(Condition)
            Verify.False(Res Is Nothing OrElse Res.Count = 0, "No value with such condition was found.")
            Verify.True(Res.Count = 1, "More than one value with such condition was found.")

            Return Res.Item(0)
        End Function

        ''' <example>
        ''' List.TryGetOneWhere(Key1 = "Value1")
        ''' </example>
        Public Function TryGetOneWhere(Of TK)(ByVal Condition As Condition(Of TK)) As (Success As Boolean, Value As T)
            Dim Res = Me.GetIndexList(Condition)
            If Res Is Nothing OrElse Res.Count = 0 Then
                Return (False, Nothing)
            End If
            Return (True, Res.Item(0))
        End Function

        ''' <example>
        ''' List.TryGetSingleWhere(Key1 = "Value1")
        ''' </example>
        Public Function TryGetSingleWhere(Of TK)(ByVal Condition As Condition(Of TK)) As (Success As Boolean, Value As T)
            Dim Res = Me.GetIndexList(Condition)
            If Res Is Nothing OrElse Res.Count <> 1 Then
                Return (False, Nothing)
            End If
            Return (True, Res.Item(0))
        End Function

        ''' <example>
        ''' List.GetAllWhere(Key1 = "Value1")
        ''' </example>
        Public Function GetAllWhere(ByVal Condition As Condition) As IEnumerable(Of T)
            Dim Res = Me.GetIndexList(Condition)
            If Res Is Nothing Then
                Return Enumerable.Empty(Of T)()
            End If
            Return Res.ToEnumerable()
        End Function

        ''' <example>
        ''' List.GetOneWhere(Key1 = "Value1")
        ''' </example>
        Public Function GetOneWhere(ByVal Condition As Condition) As T
            Dim Res = Me.GetIndexList(Condition)
            Verify.False(Res Is Nothing OrElse Res.Count = 0, "No value with such condition was found.")
            Return Res.Item(0)
        End Function

        ''' <example>
        ''' List.GetSingleWhere(Key1 = "Value1")
        ''' </example>
        Public Function GetSingleWhere(ByVal Condition As Condition) As T
            Dim Res = Me.GetIndexList(Condition)
            Verify.False(Res Is Nothing OrElse Res.Count = 0, "No value with such condition was found.")
            Verify.True(Res.Count = 1, "More than one value with such condition was found.")

            Return Res.Item(0)
        End Function

        ''' <example>
        ''' List.TryGetOneWhere(Key1 = "Value1")
        ''' </example>
        Public Function TryGetOneWhere(ByVal Condition As Condition) As (Success As Boolean, Value As T)
            Dim Res = Me.GetIndexList(Condition)
            If Res Is Nothing OrElse Res.Count = 0 Then
                Return (False, Nothing)
            End If
            Return (True, Res.Item(0))
        End Function

        ''' <example>
        ''' List.TryGetSingleWhere(Key1 = "Value1")
        ''' </example>
        Public Function TryGetSingleWhere(ByVal Condition As Condition) As (Success As Boolean, Value As T)
            Dim Res = Me.GetIndexList(Condition)
            If Res Is Nothing OrElse Res.Count <> 1 Then
                Return (False, Nothing)
            End If
            Return (True, Res.Item(0))
        End Function

        Public Function RegisterNewKey(Of TK)(ByVal KeySelector As Func(Of T, TK)) As Key(Of TK)
            Dim Key = New Key(Of TK)(Me, KeySelector)
            Dim Index = CreateInstanceDictionary.Create(Of Object, List(Of T))()
            For I = 0 To Me.List.Count - 1
                Dim Elem = Me.List.Item(I)
                Dim K = KeySelector.Invoke(Elem)
                Index.Item(K).Add(Elem)
            Next
            Me.Indexes.Add(Key, Index)
            Return Key
        End Function

        Private Sub DestroyKey(ByVal Key As Key)
            Me.Indexes.Remove(Key)
        End Sub

        Private ReadOnly List As List(Of T) = New List(Of T)()
        Private ReadOnly Indexes As Dictionary(Of Key, CreateInstanceDictionary(Of Object, List(Of T))) = New Dictionary(Of Key, CreateInstanceDictionary(Of Object, List(Of T)))()

        Public MustInherit Class Key

            Public Sub New(ByVal List As ListWithKeys(Of T), ByVal KeySelector As Func(Of T, Object))
                Me.List = List
                Me._KeySelector = KeySelector
            End Sub

            Public Shared Operator =(ByVal Key As Key, KeyValue As Object) As Condition
                Key.VerifyAlive()
                Return New Condition(Key, KeyValue)
            End Operator

            Public Shared Operator <>(ByVal Key As Key, KeyValue As Object) As Condition
                Key.VerifyAlive()
                Return New Condition(Key, KeyValue)
            End Operator

            Protected Sub VerifyAlive()
                Verify.False(Me.IsKeyDestroyed, "Cannot use a destroyed key.")
            End Sub

            Public Sub Destroy()
                Me._IsKeyDestroyed = True
                Me.List.DestroyKey(Me)
            End Sub

#Region "IsKeyDestroyed Read-Only Property"
            Private _IsKeyDestroyed As Boolean

            Public ReadOnly Property IsKeyDestroyed As Boolean
                Get
                    Return Me._IsKeyDestroyed
                End Get
            End Property
#End Region

#Region "KeySelector Read-Only Property"
            Private ReadOnly _KeySelector As Func(Of T, Object)

            Public ReadOnly Property KeySelector As Func(Of T, Object)
                Get
                    Return Me._KeySelector
                End Get
            End Property
#End Region

            Protected ReadOnly List As ListWithKeys(Of T)

        End Class

        Public Class Key(Of TK)
            Inherits Key

            Public Sub New(ByVal List As ListWithKeys(Of T), ByVal KeySelector As Func(Of T, TK))
                MyBase.New(List, Function(V) KeySelector.Invoke(V))
                Me._KeySelector = KeySelector
            End Sub

            Public Overloads Shared Operator =(ByVal Key As Key(Of TK), KeyValue As TK) As Condition(Of TK)
                Key.VerifyAlive()
                Return New Condition(Of TK)(Key, KeyValue)
            End Operator

            Public Overloads Shared Operator <>(ByVal Key As Key(Of TK), KeyValue As TK) As Condition(Of TK)
                Key.VerifyAlive()
                Return New Condition(Of TK)(Key, KeyValue)
            End Operator

#Region "KeySelector Read-Only Property"
            Private ReadOnly _KeySelector As Func(Of T, TK)

            Public Shadows ReadOnly Property KeySelector As Func(Of T, TK)
                Get
                    Return Me._KeySelector
                End Get
            End Property
#End Region

        End Class

        Public Structure Condition(Of TK)

            Public Sub New(ByVal Key As Key(Of TK), ByVal KeyValue As TK)
                Me._Key = Key
                Me._KeyValue = KeyValue
            End Sub

#Region "Key Read-Only Property"
            Private ReadOnly _Key As Key(Of TK)

            Public ReadOnly Property Key As Key(Of TK)
                Get
                    Return Me._Key
                End Get
            End Property
#End Region

#Region "KeyValue Read-Only Property"
            Private ReadOnly _KeyValue As TK

            Public ReadOnly Property KeyValue As TK
                Get
                    Return Me._KeyValue
                End Get
            End Property
#End Region

        End Structure

        Public Structure Condition

            Public Sub New(ByVal Key As Key, ByVal KeyValue As Object)
                Me._Key = Key
                Me._KeyValue = KeyValue
            End Sub

#Region "Key Read-Only Property"
            Private ReadOnly _Key As Key

            Public ReadOnly Property Key As Key
                Get
                    Return Me._Key
                End Get
            End Property
#End Region

#Region "KeyValue Read-Only Property"
            Private ReadOnly _KeyValue As Object

            Public ReadOnly Property KeyValue As Object
                Get
                    Return Me._KeyValue
                End Get
            End Property
#End Region

        End Structure

    End Class

End Namespace
