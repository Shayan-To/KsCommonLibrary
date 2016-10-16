Public MustInherit Class StringFormatterBase
    Inherits Formatter

    Public Sub New()
        Me.Serializers.Add(Serializer(Of Boolean).Create(
                        $"Binary_{NameOf([Boolean])}",
                        Function(F) ParseInv.Boolean(F.Get(Of String)(Nothing)),
                        Nothing,
                        Sub(F, O) F.Set(Nothing, O.ToStringInv())))

        Me.Serializers.Add(Serializer(Of SByte).Create(
                        $"Binary_{NameOf([SByte])}",
                        Function(F) ParseInv.SByte(F.Get(Of String)(Nothing)),
                        Nothing,
                        Sub(F, O) F.Set(Nothing, O.ToStringInv())))
        Me.Serializers.Add(Serializer(Of Int16).Create(
                        $"Binary_{NameOf(Int16)}",
                        Function(F) ParseInv.Short(F.Get(Of String)(Nothing)),
                        Nothing,
                        Sub(F, O) F.Set(Nothing, O.ToStringInv())))
        Me.Serializers.Add(Serializer(Of Int32).Create(
                        $"Binary_{NameOf(Int32)}",
                        Function(F) ParseInv.Integer(F.Get(Of String)(Nothing)),
                        Nothing,
                        Sub(F, O) F.Set(Nothing, O.ToStringInv())))
        Me.Serializers.Add(Serializer(Of Int64).Create(
                        $"Binary_{NameOf(Int64)}",
                        Function(F) ParseInv.Long(F.Get(Of String)(Nothing)),
                        Nothing,
                        Sub(F, O) F.Set(Nothing, O.ToStringInv())))
        Me.Serializers.Add(Serializer(Of Byte).Create(
                        $"Binary_{NameOf([Byte])}",
                        Function(F) ParseInv.Byte(F.Get(Of String)(Nothing)),
                        Nothing,
                        Sub(F, O) F.Set(Nothing, O.ToStringInv())))
        Me.Serializers.Add(Serializer(Of UInt16).Create(
                        $"Binary_{NameOf(UInt16)}",
                        Function(F) ParseInv.UShort(F.Get(Of String)(Nothing)),
                        Nothing,
                        Sub(F, O) F.Set(Nothing, O.ToStringInv())))
        Me.Serializers.Add(Serializer(Of UInt32).Create(
                        $"Binary_{NameOf(UInt32)}",
                        Function(F) ParseInv.UInteger(F.Get(Of String)(Nothing)),
                        Nothing,
                        Sub(F, O) F.Set(Nothing, O.ToStringInv())))
        Me.Serializers.Add(Serializer(Of UInt64).Create(
                        $"Binary_{NameOf(UInt64)}",
                        Function(F) ParseInv.ULong(F.Get(Of String)(Nothing)),
                        Nothing,
                        Sub(F, O) F.Set(Nothing, O.ToStringInv())))

        Me.Serializers.Add(Serializer(Of Single).Create(
                        $"Binary_{NameOf([Single])}",
                        Function(F) ParseInv.Single(F.Get(Of String)(Nothing)),
                        Nothing,
                        Sub(F, O) F.Set(Nothing, O.ToStringInv())))
        Me.Serializers.Add(Serializer(Of Double).Create(
                        $"Binary_{NameOf([Double])}",
                        Function(F) ParseInv.Double(F.Get(Of String)(Nothing)),
                        Nothing,
                        Sub(F, O) F.Set(Nothing, O.ToStringInv())))

        Me.Serializers.Add(Serializer(Of Char).Create(
                        $"Binary_{NameOf([Char])}",
                        Function(F) F.Get(Of String)(Nothing).Chars(0),
                        Nothing,
                        Sub(F, O) F.Set(Of String)(Nothing, O)))

        Me.Initialize()
    End Sub

End Class
