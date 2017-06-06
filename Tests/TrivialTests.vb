Public Class TrivialTests

    <Fact()>
    Public Sub IsTrue()
        Assert.True(True)
    End Sub

    <Fact()>
    Public Sub IsNotFalse()
        Assert.False(False)
    End Sub

    <Fact()>
    Public Sub DoNothing()
        Utilities.DoNothing()
    End Sub

End Class
