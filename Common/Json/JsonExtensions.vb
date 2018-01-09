'#Const RelaxedStrings = True

Imports System.Runtime.CompilerServices

Namespace Common

    Public Module JsonExtensions

        <Extension()>
        Public Sub WriteValue(ByVal Writer As JsonWriter, ByVal Obj As JsonObject, Optional ByVal MultiLine As Boolean? = Nothing)
            If TypeOf Obj Is JsonDictionaryObject Then
                Dim T = DirectCast(Obj, JsonDictionaryObject)

                Dim ML As Boolean
                If MultiLine.HasValue Then
                    ML = MultiLine.Value
                Else
                    ML = T.Count > 1
                    ML = ML OrElse T.Values.Any(Function(I) TypeOf I IsNot JsonValueObject)
                End If

                Using Writer.OpenDictionary(ML)
                    For Each I In T.OrderBy(Function(KV) KV.Key)
                        Writer.WriteKey(I.Key)
                        Writer.WriteValue(I.Value, MultiLine)
                    Next
                End Using

                Exit Sub
            End If

            If TypeOf Obj Is JsonListObject Then
                Dim T = DirectCast(Obj, JsonListObject)

                Dim ML As Boolean
                If MultiLine.HasValue Then
                    ML = MultiLine.Value
                Else
                    ML = T.Count > 10
                    ML = ML OrElse T.Any(Function(I) TypeOf I IsNot JsonValueObject)
                    ML = ML OrElse T.Cast(Of JsonValueObject).Sum(Function(I) I.Value.Length) > 150
                End If

                Using Writer.OpenList(ML)
                    For Each I In T
                        Writer.WriteValue(I, MultiLine)
                    Next
                End Using

                Exit Sub
            End If

            If TypeOf Obj Is JsonValueObject Then
                Dim T = DirectCast(Obj, JsonValueObject)
                Writer.WriteValue(T.Value, T.IsString)

                Exit Sub
            End If

            Verify.Fail("Invalid object type.")
        End Sub

        <Extension()>
        Public Function AsDictionary(ByVal Self As JsonObject) As JsonDictionaryObject
            Dim R = TryCast(Self, JsonDictionaryObject)
            Verify.False(R Is Nothing, "Item has to be a dictionary.")
            Return R
        End Function

        <Extension()>
        Public Function AsList(ByVal Self As JsonObject) As JsonListObject
            Dim R = TryCast(Self, JsonListObject)
            Verify.False(R Is Nothing, "Item has to be a list.")
            Return R
        End Function

        <Extension()>
        Private Function AsValue(ByVal Self As JsonObject, ByVal ErrorMessage As String) As JsonValueObject
            Dim R = TryCast(Self, JsonValueObject)
            Verify.False(R Is Nothing, ErrorMessage)
            Return R
        End Function

        <Extension()>
        Public Function GetString(ByVal Self As JsonObject) As String
            Verify.NonNull(Self)
            Dim V = Self.AsValue("A string value was expected, not a list or dictionary.")

#If Not RelaxedStrings Then
            Verify.True(V.IsString, "Value must be a string.")
#End If

            Return V.Value
        End Function

        <Extension()>
        Public Function GetBoolean(ByVal Self As JsonObject) As Boolean
            Verify.NonNull(Self)
            Dim V = Self.AsValue("A boolean value was expected, not a list or dictionary.")

#If Not RelaxedStrings Then
            Verify.False(V.IsString, "Value must be a boolean.")
#End If

            Verify.True(V.Value = [True] Or V.Value = [False], "Value must be a boolean.")
            Return V.Value = [True]
        End Function

        <Extension()>
        Public Function GetInteger(ByVal Self As JsonObject) As Integer
            Verify.NonNull(Self)
            Dim V = Self.AsValue("An integer value was expected, not a list or dictionary.")

#If Not RelaxedStrings Then
            Verify.False(V.IsString, "Value must be an integer.")
#End If

            Dim T = 0
            Verify.True(ParseInv.TryInteger(V.Value, T), "Value must be an integer.")
            Return T
        End Function

        <Extension()>
        Public Function GetDouble(ByVal Self As JsonObject) As Double
            Verify.NonNull(Self)
            Dim V = Self.AsValue("A number value was expected, not a list or dictionary.")

#If Not RelaxedStrings Then
            Verify.False(V.IsString, "Value must be a number.")
#End If

            Dim T = 0.0
            Verify.True(ParseInv.TryDouble(V.Value, T), "Value must be a number.")
            Return T
        End Function

        <Extension()>
        Public Function GetEnum(ByVal Self As JsonObject, ByVal Values As String()) As Integer
            Dim V = Self.GetString()
            Dim I = Array.IndexOf(Values, V)
            Verify.True(I <> -1, "Value must be from within the predefined values.")
            Return I
        End Function

        Private Const [True] = "true"
        Private Const [False] = "false"

    End Module

End Namespace
