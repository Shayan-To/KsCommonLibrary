Namespace Common

    Partial Class Formatter

        Public Sub New()
            Me.Serializers.Add(GenericSerializer.Create(NameOf(Array),
                                                        GetType(ArraySerializer(Of )),
                                                        Function(T)
                                                            If Not T.IsArray OrElse T.GetArrayRank() <> 1 Then
                                                                Return Nothing
                                                            End If
                                                            Return {T.GetElementType()}
                                                        End Function))
            Me.Serializers.Add(GenericSerializer.Create(NameOf(List(Of Object)),
                                                        GetType(ListSerializer(Of )),
                                                        Function(T)
                                                            Dim IT As Type = Nothing
                                                            For Each I In T.GetInterfaces()
                                                                If I.IsGenericType AndAlso I.GetGenericTypeDefinition() = GetType(IEnumerable(Of )) Then
                                                                    IT = I
                                                                    Exit For
                                                                End If
                                                            Next
                                                            Return IT?.GetGenericArguments()
                                                        End Function))
            Me.Serializers.Add(GenericSerializer.Create(NameOf(Dictionary(Of Object, Object)),
                                                        GetType(DictionarySerializer(Of ,)),
                                                        Function(T)
                                                            Dim IT As Type = Nothing
                                                            For Each I In T.GetInterfaces()
                                                                If I.IsGenericType AndAlso I.GetGenericTypeDefinition() = GetType(IEnumerable(Of )) Then
                                                                    IT = I
                                                                    Exit For
                                                                End If
                                                            Next
                                                            If IT Is Nothing Then
                                                                Return Nothing
                                                            End If
                                                            T = IT.GetGenericArguments().Single()
                                                            If Not T.IsGenericType OrElse T.GetGenericTypeDefinition() <> GetType(KeyValuePair(Of ,)) Then
                                                                Return Nothing
                                                            End If
                                                            Return T.GetGenericArguments()
                                                        End Function))
            Me.Serializers.Add(GenericSerializer.Create(NameOf(KeyValuePair(Of Object, Object)),
                                                        GetType(KeyValuePairSerializer(Of ,)),
                                                        Function(T)
                                                            If Not T.IsGenericType OrElse T.GetGenericTypeDefinition() <> GetType(KeyValuePair(Of ,)) Then
                                                                Return Nothing
                                                            End If
                                                            Return T.GetGenericArguments()
                                                        End Function))

            Me.Serializers.Add(Serializer(Of Type).Create(NameOf(Type),
                                                          Function(F)
                                                              Dim FullName As String
                                                              FullName = F.Get(Of String)(NameOf(FullName))
                                                              Return Type.GetType(FullName) ' ToDo This method wants the assembly-qualified name. See how it's done using the full name.
                                                          End Function,
                                                          Nothing,
                                                          Sub(F, O)
                                                              F.Set(NameOf(O.FullName), O.FullName)
                                                          End Sub))

            Me.Serializers.Add(Serializer(Of Serializer).Create(NameOf(Serializer),
                                                          Function(F)
                                                              Dim Id As String
                                                              Id = F.Get(Of String)(NameOf(Id))
                                                              Return F.Formatter.Serializers.Item(Id)
                                                          End Function,
                                                          Nothing,
                                                          Sub(F, O)
                                                              F.Set(NameOf(O.Id), O.Id)
                                                          End Sub))

            Me.Serializers.Add(Serializer(Of Object).Create(NameOf([Object]),
                                                            Function(F)
                                                                Dim IsObject As Boolean
                                                                IsObject = F.Get(Of Boolean)(NameOf(IsObject))
                                                                If IsObject Then
                                                                    Return New Object()
                                                                End If
                                                                Return F.Get(Nothing)
                                                            End Function,
                                                            Nothing,
                                                            Sub(F, O)
                                                                Dim IsObject = O.GetType() = GetType(Object)
                                                                F.Set(NameOf(IsObject), IsObject)

                                                                If Not IsObject Then
                                                                    F.Set(Nothing, O)
                                                                End If
                                                            End Sub))

            Me.Initialize()
        End Sub

    End Class

End Namespace
