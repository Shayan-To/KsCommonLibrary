Namespace Common

    Public MustInherit Class BinaryFormatterBase
        Inherits Formatter

        Public Sub New()
            Me.Serializers.Add(Serializer(Of Boolean).Create(
                            $"Binary_{NameOf([Boolean])}",
                            Function(F)
                                F.Get(Nothing, New SerializationArrayChunk(Of Byte)(TempArray, 0, 1))
                                Return BitConverter.ToBoolean(TempArray, 0)
                            End Function,
                            Nothing,
                            Sub(F, O)
                                F.Set(Nothing, New SerializationArrayChunk(Of Byte)(BitConverter.GetBytes(O)))
                            End Sub))

            Me.Serializers.Add(Serializer(Of Int16).Create(
                            $"Binary_{NameOf(Int16)}",
                            Function(F)
                                F.Get(Nothing, New SerializationArrayChunk(Of Byte)(TempArray, 0, 2))
                                Return BitConverter.ToInt16(TempArray, 0)
                            End Function,
                            Nothing,
                            Sub(F, O)
                                F.Set(Nothing, New SerializationArrayChunk(Of Byte)(BitConverter.GetBytes(O)))
                            End Sub))
            Me.Serializers.Add(Serializer(Of Int32).Create(
                            $"Binary_{NameOf(Int32)}",
                            Function(F)
                                F.Get(Nothing, New SerializationArrayChunk(Of Byte)(TempArray, 0, 4))
                                Return BitConverter.ToInt32(TempArray, 0)
                            End Function,
                            Nothing,
                            Sub(F, O)
                                F.Set(Nothing, New SerializationArrayChunk(Of Byte)(BitConverter.GetBytes(O)))
                            End Sub))
            Me.Serializers.Add(Serializer(Of Int64).Create(
                            $"Binary_{NameOf(Int64)}",
                            Function(F)
                                F.Get(Nothing, New SerializationArrayChunk(Of Byte)(TempArray, 0, 8))
                                Return BitConverter.ToInt64(TempArray, 0)
                            End Function,
                            Nothing,
                            Sub(F, O)
                                F.Set(Nothing, New SerializationArrayChunk(Of Byte)(BitConverter.GetBytes(O)))
                            End Sub))
            Me.Serializers.Add(Serializer(Of UInt16).Create(
                            $"Binary_{NameOf(UInt16)}",
                            Function(F)
                                F.Get(Nothing, New SerializationArrayChunk(Of Byte)(TempArray, 0, 2))
                                Return BitConverter.ToUInt16(TempArray, 0)
                            End Function,
                            Nothing,
                            Sub(F, O)
                                F.Set(Nothing, New SerializationArrayChunk(Of Byte)(BitConverter.GetBytes(O)))
                            End Sub))
            Me.Serializers.Add(Serializer(Of UInt32).Create(
                            $"Binary_{NameOf(UInt32)}",
                            Function(F)
                                F.Get(Nothing, New SerializationArrayChunk(Of Byte)(TempArray, 0, 4))
                                Return BitConverter.ToUInt32(TempArray, 0)
                            End Function,
                            Nothing,
                            Sub(F, O)
                                F.Set(Nothing, New SerializationArrayChunk(Of Byte)(BitConverter.GetBytes(O)))
                            End Sub))
            Me.Serializers.Add(Serializer(Of UInt64).Create(
                            $"Binary_{NameOf(UInt64)}",
                            Function(F)
                                F.Get(Nothing, New SerializationArrayChunk(Of Byte)(TempArray, 0, 8))
                                Return BitConverter.ToUInt64(TempArray, 0)
                            End Function,
                            Nothing,
                            Sub(F, O)
                                F.Set(Nothing, New SerializationArrayChunk(Of Byte)(BitConverter.GetBytes(O)))
                            End Sub))

            Me.Serializers.Add(Serializer(Of Single).Create(
                            $"Binary_{NameOf([Single])}",
                            Function(F)
                                F.Get(Nothing, New SerializationArrayChunk(Of Byte)(TempArray, 0, 4))
                                Return BitConverter.ToSingle(TempArray, 0)
                            End Function,
                            Nothing,
                            Sub(F, O)
                                F.Set(Nothing, New SerializationArrayChunk(Of Byte)(BitConverter.GetBytes(O)))
                            End Sub))
            Me.Serializers.Add(Serializer(Of Double).Create(
                            $"Binary_{NameOf([Double])}",
                            Function(F)
                                F.Get(Nothing, New SerializationArrayChunk(Of Byte)(TempArray, 0, 8))
                                Return BitConverter.ToDouble(TempArray, 0)
                            End Function,
                            Nothing,
                            Sub(F, O)
                                F.Set(Nothing, New SerializationArrayChunk(Of Byte)(BitConverter.GetBytes(O)))
                            End Sub))

            Me.Serializers.Add(Serializer(Of Char).Create(
                            $"Binary_{NameOf([Char])}",
                            Function(F)
                                F.Get(Nothing, New SerializationArrayChunk(Of Byte)(TempArray, 0, 2))
                                Return BitConverter.ToChar(TempArray, 0)
                            End Function,
                            Nothing,
                            Sub(F, O)
                                F.Set(Nothing, New SerializationArrayChunk(Of Byte)(BitConverter.GetBytes(O)))
                            End Sub))
            Me.Serializers.Add(Serializer(Of String).Create(
                            $"Binary_{NameOf([String])}",
                            Function(F) Text.Encoding.UTF8.GetString(F.Get(Of Byte())(Nothing)),
                            Nothing,
                            Sub(F, O) F.Set(Nothing, Text.Encoding.UTF8.GetBytes(O))))

            Me.Initialize()
        End Sub

        Private ReadOnly TempArray As Byte() = New Byte(8 - 1) {}

    End Class

End Namespace
