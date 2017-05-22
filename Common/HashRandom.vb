Namespace Common

    Public Class HashRandom

        Public Sub New(ByVal Hasher As Security.Cryptography.HashAlgorithm)
            Me.New(Hasher, DateTime.Now.Ticks)
        End Sub

        Public Sub New(ByVal Hasher As Security.Cryptography.HashAlgorithm, ByVal Seed As Long)
            Me.New(Hasher, BitConverter.GetBytes(Seed))
        End Sub

        Public Sub New(ByVal Hasher As Security.Cryptography.HashAlgorithm, ByVal Seed As Byte())
            Me.Buffer = New Byte(Hasher.OutputBlockSize - 1) {}
            Hasher.ComputeHash(Seed, Me.Buffer)
            Me.Hasher = Hasher
            Me.Index = 0
        End Sub

        Protected Sub New(ByVal Hasher As Security.Cryptography.HashAlgorithm, ByVal Buffer As Byte(), ByVal Index As Integer)
            Me.Hasher = Hasher
            Me.Buffer = Buffer.ToArray()
            Me.Index = Index
        End Sub

        Private Sub RenewBuffer()
            Dim Tmp = Me.Buffer.ToArray()
            Me.Hasher.ComputeHash(Tmp, Me.Buffer)
            Me.Index = 0
        End Sub

        Public Function GetRandomInteger() As Integer
            Me.GetRandomBytes(Me.Tmp, 0, 4)
            Return BitConverter.ToInt32(Me.Tmp, 0)
        End Function

        Public Function GetRandomUInteger() As UInteger
            Me.GetRandomBytes(Me.Tmp, 0, 4)
            Return BitConverter.ToUInt32(Me.Tmp, 0)
        End Function

        Public Function GetRandomLong() As Long
            Me.GetRandomBytes(Me.Tmp, 0, 8)
            Return BitConverter.ToInt64(Me.Tmp, 0)
        End Function

        Public Function GetRandomULong() As ULong
            Me.GetRandomBytes(Me.Tmp, 0, 8)
            Return BitConverter.ToUInt64(Me.Tmp, 0)
        End Function

        Public Function GetRandomShort() As Short
            Me.GetRandomBytes(Me.Tmp, 0, 2)
            Return BitConverter.ToInt16(Me.Tmp, 0)
        End Function

        Public Function GetRandomUShort() As UShort
            Me.GetRandomBytes(Me.Tmp, 0, 2)
            Return BitConverter.ToUInt16(Me.Tmp, 0)
        End Function

        Public Function GetRandomByte() As Byte
            Me.GetRandomBytes(Me.Tmp, 0, 1)
            Return Me.Tmp(0)
        End Function

        Public Function GetRandomSByte() As SByte
            Me.GetRandomBytes(Me.Tmp, 0, 1)
            Me.Tmp(1) = 0
            Return CSByte(BitConverter.ToUInt16(Me.Tmp, 0))
        End Function

        Public Function GetRandomBytes(ByVal Count As Integer) As Byte()
            Dim Res = New Byte(Count - 1) {}
            Me.GetRandomBytes(Res, 0, Count)
            Return Res
        End Function

        Public Overridable Sub GetRandomBytes(ByVal Array As Byte(), ByVal Index As Integer, ByVal Length As Integer)
            Verify.TrueArg(Length >= 0, NameOf(Length), "Length cannot be negative.")
            Do Until Length = 0
                If Me.Index = Me.Buffer.Length Then
                    Me.RenewBuffer()
                End If
                Dim Len = Math.Min(Me.Buffer.Length - Me.Index, Length)
                System.Array.Copy(Me.Buffer, Me.Index, Array, Index, Len)
                Length -= Len
                Me.Index += Len
            Loop
        End Sub

        Public Overridable Function Export() As (Buffer As Byte(), Index As Integer)
            Return (Me.Buffer.ToArray(), Me.Index)
        End Function

        Public Overridable Sub Import(ByVal Data As (Buffer As Byte(), Index As Integer))
            Verify.True(Data.Buffer.Length = Me.Buffer.Length)
            Array.Copy(Data.Buffer, Me.Buffer, Me.Buffer.Length)
            Me.Index = Data.Index
        End Sub

        Private ReadOnly Tmp As Byte() = New Byte(7) {}
        Private ReadOnly Hasher As Security.Cryptography.HashAlgorithm
        Private ReadOnly Buffer As Byte()
        Private Index As Integer

    End Class

End Namespace
