Imports Ks.Common.Controls

Namespace Common.MVVM

    <ViewModelMetadata(GetType(Window), IsSingleInstance:=True)>
    Public Class WindowViewModel
        Inherits NavigationViewModel

        Public Sub New(ByVal KsApplication As KsApplication)
            MyBase.New(KsApplication)
        End Sub

        Public Sub New()
            MyBase.New()
        End Sub

    End Class

End Namespace
