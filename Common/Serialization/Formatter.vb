Namespace Common

    Public MustInherit Class Formatter

        ' ToDo Should there be a machanism for fallback? Two suggestions:
        ' 1. Put a way Get and Set of Serializer can tell that they cannot serialize the object.
        ' 2. Put back CanSerializeObject so that we can check it before calling Get or Set.
        ' Current design was chosen as it is simpler, and adds more restrictions (removes the unneccessary freedom (~Vocab)).

        ''' <summary>
        ''' This method has to be called at the end of every constructor of every derived class.
        ''' </summary>
        Protected Sub Initialize()
            Me.Serializers.LockCurrentElements()
        End Sub

        Protected MustOverride Sub OnGetStarted()
        Protected MustOverride Sub OnGetFinished()
        Protected MustOverride Sub OnGetEnterContext(ByVal Name As String)
        Protected MustOverride Sub OnGetExitContext(ByVal Name As String)

        Protected MustOverride Sub OnSetStarted()
        Protected MustOverride Sub OnSetFinished()
        Protected MustOverride Sub OnSetEnterContext(ByVal Name As String)
        Protected MustOverride Sub OnSetExitContext(ByVal Name As String)

        Private Function GetSerializer(Of T)(ByVal Obj As CNullable(Of T), ByVal IsGeneric As Boolean) As Serializer
            Dim Type = GetType(T)
            If Not IsGeneric And Obj.HasValue Then
                Type = Obj.Value.GetType()
            End If

            For I As Integer = Me.Serializers.Count - 1 To 0 Step -1
                Dim S = Me.Serializers.ItemAt(I)

                If Not S.CanSerializeType(Type) Then
                    Continue For
                End If

                ' We will also return the non-generic serializers for generic serialization. The users have to check whether the returned serializer is generic or not.

                Return S
            Next
            Return Nothing
        End Function

        Private Sub SetImpl(Of T)(ByVal Name As String, ByVal Obj As T, ByVal IsGeneric As Boolean)
            Dim WasRunning = Me.IsRunning
            Dim IsRefType = Not GetType(T).IsValueType And GetType(T) <> GetType(String)

            If Not WasRunning Then
                Me.OnSetStarted()

                Me.IsRunning = True
                Assert.True(Me.GetCache.Count = 0)
                Assert.True(Me.SetCache.Count = 0)
                Assert.True(Me.ObjectsCount = 0)
            End If

            Me.OnSetEnterContext(Name)

            Try
                Dim Id = Me.ObjectsCount
                If IsRefType Then
                    Dim Tmp As (Integer, Boolean) = Nothing
                    Dim WasInCache = Me.SetCache.TryGetValue(Obj, Tmp)

                    Dim IsDone = Tmp.Item2
                    If WasInCache Then
                        Id = Tmp.Item1
                    End If

                    ' We allow more than one serialization iterations on the same object. See the serializer for Object for a use case.

                    Me.Set(NameOf(Id), Id)

                    If IsDone Then
                        Exit Sub
                    Else
                        If Not WasInCache Then
                            Me.SetCache.Add(Obj, (Id, False))
                            Me.ObjectsCount += 1
                        End If
                    End If
                End If

                Dim S = Me.GetSerializer(Of T)(Obj, IsGeneric)
                If IsGeneric Then
                    Dim ST = TryCast(S, Serializer(Of T))
                    If ST IsNot Nothing Then
                        ST.SetT(Me.SetProxy, Obj)
                    Else
                        S.Set(Me.SetProxy, Obj)
                    End If
                Else
                    Me.Set(NameOf(Serializer), S)
                    S.Set(Me.SetProxy, Obj)
                End If

                If IsRefType Then
                    Assert.True(Me.SetCache.ContainsKey(Obj))
                    Me.SetCache.Item(Obj) = (Id, True)
                End If
            Finally
                Me.OnSetExitContext(Name)

                If Not WasRunning Then
                    Me.OnSetFinished()

                    Me.IsRunning = False
                    Me.SetCache.Clear()
                    Me.ObjectsCount = 0
                End If
            End Try
        End Sub

        Private Function GetImpl(Of T)(ByVal Name As String, ByVal GObj As CNullable(Of T), ByVal IsGeneric As Boolean) As T
            Dim WasRunning = Me.IsRunning
            Dim IsRefType = Not GetType(T).IsValueType

            ' We allow T to be a value type. See the serializer for SerializationArrayChunk for a use case.

            If Not WasRunning Then
                Me.OnGetStarted()

                Me.IsRunning = True
                Assert.True(Me.GetCache.Count = 0)
                Assert.True(Me.SetCache.Count = 0)
                Assert.True(Me.ObjectsCount = 0)
            End If

            Me.OnGetEnterContext(Name)

            Try
                Dim Id = 0
                If IsRefType Then
                    Id = Me.Get(Of Integer)(NameOf(Id))

                    Dim Obj As Object = Nothing
                    If Me.GetCache.TryGetValue(Id, Obj) AndAlso Obj IsNot Nothing Then
                        Return DirectCast(Obj, T)
                    End If

                    Me.GetCache.Add(Id, Nothing)
                End If

                Dim S As Serializer
                If IsGeneric Then
                    S = Me.GetSerializer(Of T)(Nothing, True)
                Else
                    S = Me.Get(Of Serializer)(NameOf(Serializer))
                End If

                Dim ST = TryCast(S, Serializer(Of T))
                Dim R As T = Nothing

                If IsGeneric And ST IsNot Nothing Then
                    If GObj.HasValue Then
                        ST.GetT(Me.GetProxy, GObj.Value)
                        R = GObj.Value
                    Else
                        R = ST.GetT(Me.GetProxy)
                    End If
                Else
                    If GObj.HasValue Then
                        S.Get(Me.GetProxy, GObj.Value)
                        R = GObj.Value
                    Else
                        R = DirectCast(S.Get(Me.GetProxy), T)
                    End If
                End If

                If IsRefType Then
                    Dim Obj = Me.GetCache.Item(Id)
                    If Obj IsNot Nothing Then
                        Verify.True(Obj Is DirectCast(R, Object), "Two different objects returned for the same id.")
                    Else
                        Me.GetCache.Item(Id) = R
                    End If
                End If

                Return R
            Finally
                Me.OnGetExitContext(Name)

                If Not WasRunning Then
                    Me.OnGetFinished()

                    Me.IsRunning = False
                    Me.GetCache.Clear()
                    Me.ObjectsCount = 0
                End If
            End Try
        End Function

        Protected Friend Sub [Set](Of T)(ByVal Name As String, ByVal Obj As T)
            Me.SetImpl(Name, Obj, True)
        End Sub

        Protected Friend Sub [Set](ByVal Name As String, ByVal Obj As Object)
            Me.SetImpl(Name, Obj, False)
        End Sub

        Protected Friend Function [Get](Of T)(ByVal Name As String) As T
            Return Me.GetImpl(Of T)(Name, Nothing, True)
        End Function

        Protected Friend Function [Get](ByVal Name As String) As Object
            Return Me.GetImpl(Of Object)(Name, Nothing, False)
        End Function

        Protected Friend Sub [Get](Of T)(ByVal Name As String, ByVal Obj As T)
            Me.GetImpl(Of T)(Name, Obj, True)
        End Sub

        Protected Friend Sub [Get](ByVal Name As String, ByVal Obj As Object)
            Me.GetImpl(Of Object)(Name, New CNullable(Of Object)(Obj), False)
        End Sub

#Region "Serializers Property"
        Private ReadOnly _Serializers As SerializerCollection = New SerializerCollection()

        Public ReadOnly Property Serializers As SerializerCollection
            Get
                Return Me._Serializers
            End Get
        End Property
#End Region

        Private ReadOnly SetProxy As FormatterSetProxy = New FormatterSetProxy(Me)
        Private ReadOnly GetProxy As FormatterGetProxy = New FormatterGetProxy(Me)

        Private ReadOnly SetCache As Dictionary(Of Object, (Integer, Boolean)) = New Dictionary(Of Object, (Integer, Boolean))(ReferenceEqualityComparer(Of Object).Instance)
        Private ReadOnly GetCache As Dictionary(Of Integer, Object) = New Dictionary(Of Integer, Object)()
        Private ObjectsCount As Integer
        Private IsRunning As Boolean

    End Class

End Namespace
