Imports Ks.Common.Controls

Namespace MVVM

    <ViewModelMetadata(GetType(Window), IsSingleInstance:=True)>
    Public Class WindowViewModel
        Inherits NavigationViewModel

        Public Sub New(ByVal KsApplication As KsApplication)
            MyBase.New(KsApplication)
            Me.NavigationFrame = New NavigationFrame({Me})
        End Sub

        Public Sub New()
            MyBase.New()
        End Sub

    End Class

End Namespace
