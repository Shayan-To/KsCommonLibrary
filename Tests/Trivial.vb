Public Class Trivial

    <Fact()>
    Public Sub IsTrue()
        Assert.False(False)
    End Sub

    <Fact()>
    Public Sub IsNotFalse()
        Assert.False(False)
    End Sub

End Class
