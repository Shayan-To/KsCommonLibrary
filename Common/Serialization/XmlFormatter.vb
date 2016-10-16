Public Class XmlFormatter
    Inherits StringFormatterBase

    Public Sub New()
        Me.Serializers.Add(Serializer(Of String).Create(NameOf([String]),
                                                        Function(F)
                                                            Return DirectCast(F.Formatter, XmlFormatter).OnGetString()
                                                        End Function,
                                                        Nothing,
                                                        Sub(F, O)
                                                            DirectCast(F.Formatter, XmlFormatter).OnSetString(O)
                                                        End Sub))
    End Sub

    Protected Overrides Sub OnGetStarted()

    End Sub

    Protected Overrides Sub OnGetEnterContext(Name As String)
        Name = If(Name, "Value")
        If Name IsNot Nothing Then
            Me.XmlReader.ReadStartElement(Name)
        End If
    End Sub

    Friend Function OnGetString() As String
        Return Me.XmlReader.ReadElementContentAsString()
    End Function

    Protected Overrides Sub OnGetExitContext(Name As String)
        Name = If(Name, "Value")
        If Name IsNot Nothing Then
            Me.XmlReader.ReadEndElement()
        End If
    End Sub

    Protected Overrides Sub OnGetFinished()

    End Sub

    Protected Overrides Sub OnSetStarted()

    End Sub

    Protected Overrides Sub OnSetEnterContext(Name As String)
        Name = If(Name, "Value")
        If Name IsNot Nothing Then
            Me.XmlWriter.WriteStartElement(Name)
        End If
    End Sub

    Friend Sub OnSetString(ByVal Str As String)
        Me.XmlWriter.WriteValue(Str)
    End Sub

    Protected Overrides Sub OnSetExitContext(Name As String)
        Name = If(Name, "Value")
        If Name IsNot Nothing Then
            Me.XmlWriter.WriteEndElement()
        End If
    End Sub

    Protected Overrides Sub OnSetFinished()

    End Sub

    Public Function GetXml(Of T)(ByVal Obj As T) As String
        Dim SB = New Text.StringBuilder()
        Me.XmlWriter = Xml.XmlWriter.Create(SB, New Xml.XmlWriterSettings() With {.Indent = True})
        Me.Set(Nothing, Obj)
        Me.XmlWriter.Dispose()
        Return SB.ToString()
    End Function

    Private XmlReader As Xml.XmlReader
    Private XmlWriter As Xml.XmlWriter

End Class
