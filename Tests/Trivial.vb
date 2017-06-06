Public Class Trivial

    <Fact()>
    Public Sub IsTrue()
        Assert.True(True)
    End Sub

    <Fact()>
    Public Sub IsNotFalse()
        Assert.False(False)
    End Sub

End Class
