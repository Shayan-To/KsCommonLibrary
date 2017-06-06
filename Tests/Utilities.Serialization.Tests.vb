Partial Class Utilities_Tests

    Public Class Serialization

        <[Property]>
        Public Sub ListToString_ListFromString_Consistency(ByVal List As String())
            If List.Contains(Nothing) Then
                Exit Sub
            End If
            Dim Serialized = Utilities.Serialization.ListToString(List)
            Dim Deserialized = Utilities.Serialization.ListFromString(Serialized)
            Assert.Equal(List, Deserialized)
        End Sub

        <Fact>
        Public Sub ListFromString()
            Assert.Equal(New String() {}, Utilities.Serialization.ListFromString("{}"))
            Assert.Equal(New String() {"AA", "BB"}, Utilities.Serialization.ListFromString("{AA,BB}"))
            Assert.Equal(New String() {"{,}", "{" & ControlChars.Cr & "}" & ControlChars.Lf},
                         Utilities.Serialization.ListFromString("{\{\,\},\{\r\}\n}"))
        End Sub

        <Fact>
        Public Sub ListToString()
            Assert.Equal("{}", Utilities.Serialization.ListToString({}))
            Assert.Equal("{AA,BB}", Utilities.Serialization.ListToString({"AA", "BB"}))
            Assert.Equal("{\{\,\},\{\r\}\n}",
                         Utilities.Serialization.ListToString({"{,}", "{" & ControlChars.Cr & "}" & ControlChars.Lf}))
        End Sub

        <[Property]>
        Public Sub ListToStringMultiline_ListFromStringMultiline_Consistency(ByVal List As String())
            If List.Contains(Nothing) Then
                Exit Sub
            End If
            Dim Serialized = Utilities.Serialization.ListToStringMultiline(List)
            Dim Deserialized = Utilities.Serialization.ListFromStringMultiline(Serialized)
            Assert.Equal(List, Deserialized)
        End Sub

    End Class

End Class
