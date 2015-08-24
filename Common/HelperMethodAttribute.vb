<AttributeUsage(AttributeTargets.Method)>
Public Class ConsoleTestMethodAttribute
    Inherits Attribute

    Public Sub New(Optional ByVal ShouldBeRun As Boolean = False)
        Me._ShouldBeRun = ShouldBeRun
    End Sub

#Region "ShouldBeRun Property"
    Private ReadOnly _ShouldBeRun As Boolean

    Public ReadOnly Property ShouldBeRun As Boolean
        Get
            Return Me._ShouldBeRun
        End Get
    End Property
#End Region

End Class
