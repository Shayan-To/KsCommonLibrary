Imports System.Runtime.CompilerServices

Namespace Common

    Public Module JsonDynamicExtensions

        <Extension()>
        Public Sub WriteValue(ByVal Writer As JsonWriter, ByVal Obj As JsonDynamicBase, Optional ByVal MultiLine As Boolean? = Nothing)
            If TypeOf Obj Is JsonDynamicDictionary Then
                Dim T = DirectCast(Obj, JsonDynamicDictionary)

                Dim ML As Boolean
                If MultiLine.HasValue Then
                    ML = MultiLine.Value
                Else
                    ML = T.Count > 1
                    ML = ML OrElse T.Values.Any(Function(I) TypeOf I IsNot JsonDynamicValue)
                End If

                Using Writer.OpenDictionary(ML)
                    For Each I In T.OrderBy(Function(KV) KV.Key)
                        Writer.WriteKey(I.Key)
                        Writer.WriteValue(I.Value, MultiLine)
                    Next
                End Using

                Exit Sub
            End If

            If TypeOf Obj Is JsonDynamicList Then
                Dim T = DirectCast(Obj, JsonDynamicList)

                Dim ML As Boolean
                If MultiLine.HasValue Then
                    ML = MultiLine.Value
                Else
                    ML = T.Count > 10
                    ML = ML OrElse T.Any(Function(I) TypeOf I IsNot JsonDynamicValue)
                    ML = ML OrElse T.Cast(Of JsonDynamicValue).Sum(Function(I) I.Value.Length) > 150
                End If

                Using Writer.OpenList(ML)
                    For Each I In T
                        Writer.WriteValue(I, MultiLine)
                    Next
                End Using

                Exit Sub
            End If

            If TypeOf Obj Is JsonDynamicValue Then
                Dim T = DirectCast(Obj, JsonDynamicValue)
                Writer.WriteValue(T.Value, T.IsString)

                Exit Sub
            End If

            Verify.Fail("Invalid object type.")
        End Sub

        <Extension()>
        Public Function ToDynamic(ByVal Self As JsonObject) As JsonDynamicBase
            If TypeOf Self Is JsonDictionaryObject Then
                Dim Res = New JsonDynamicDictionary()
                For Each T In DirectCast(Self, JsonDictionaryObject)
                    Res.Add(T.Key, T.Value.ToDynamic())
                Next
                Return Res
            End If

            If TypeOf Self Is JsonListObject Then
                Dim Res = New JsonDynamicList()
                For Each T In DirectCast(Self, JsonListObject)
                    Res.Add(T.ToDynamic())
                Next
                Return Res
            End If

            If TypeOf Self Is JsonValueObject Then
                Dim T = DirectCast(Self, JsonValueObject)
                Return New JsonDynamicValue(T.Value, T.IsString)
            End If

            Verify.Fail("Invalid object type.")
            Return Nothing
        End Function

        <Extension()>
        Public Function ToConstant(ByVal Self As JsonDynamicBase) As JsonObject
            If TypeOf Self Is JsonDynamicDictionary Then
                Dim Res = New List(Of KeyValuePair(Of String, JsonObject))()
                For Each T In DirectCast(Self, JsonDynamicDictionary)
                    Res.Add(New KeyValuePair(Of String, JsonObject)(T.Key, T.Value.ToConstant()))
                Next
                Return New JsonDictionaryObject(Res)
            End If

            If TypeOf Self Is JsonDynamicList Then
                Dim Res = New List(Of JsonObject)()
                For Each T In DirectCast(Self, JsonDynamicList)
                    Res.Add(T.ToConstant())
                Next
                Return New JsonListObject(Res)
            End If

            If TypeOf Self Is JsonDynamicValue Then
                Dim T = DirectCast(Self, JsonDynamicValue)
                Return New JsonValueObject(T.Value, T.IsString)
            End If

            Verify.Fail("Invalid object type.")
            Return Nothing
        End Function

        <Extension()>
        Public Function AsList(ByVal Self As JsonDynamicBase) As JsonDynamicList
            Verify.NonNull(Self)
            Return DirectCast(Self, JsonDynamicList)
        End Function

        <Extension()>
        Public Function AsValue(ByVal Self As JsonDynamicBase) As JsonDynamicValue
            Verify.NonNull(Self)
            Return DirectCast(Self, JsonDynamicValue)
        End Function

        <Extension()>
        Public Function AsDictionary(ByVal Self As JsonDynamicBase) As JsonDynamicDictionary
            Verify.NonNull(Self)
            Return DirectCast(Self, JsonDynamicDictionary)
        End Function

        <Extension()>
        Public Function GetString(ByVal Self As JsonDynamicValue) As String
            Return Self.Value
        End Function

        <Extension()>
        Public Function GetBoolean(ByVal Self As JsonDynamicValue) As Boolean
            Verify.True(Self.Value = JsonDynamicValue.True Or Self.Value = JsonDynamicValue.False, "Value must be a boolean.")
            Return Self.Value = JsonDynamicValue.True
        End Function

        <Extension()>
        Public Function GetInteger(ByVal Self As JsonDynamicValue) As Integer
            Dim T = 0
            Verify.True(ParseInv.TryInteger(Self.Value, T), "Value must be an integer.")
            Return T
        End Function

        <Extension()>
        Public Function GetDouble(ByVal Self As JsonDynamicValue) As Double
            Dim T = 0.0
            Verify.True(ParseInv.TryDouble(Self.Value, T), "Value must be a number.")
            Return T
        End Function

    End Module

End Namespace
