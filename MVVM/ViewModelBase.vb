Namespace MVVM

    Public Class ViewModelBase
        Inherits BindableBase

        ''' <remarks>
        ''' Remember to call your GenerateDesignTimeData subroutine in your design-time constructor.
        ''' </remarks>
        Public Sub New(Optional ByVal IsDesignTime As Boolean = True)
            If IsDesignTime Then
                '#If VBC_VER >= 11.0 Then
                If Not Utilities.IsInDesignMode Then
                    Throw New NotSupportedException("You cannot call this constructor outside the designer.")
                End If
                '#End If
            End If
        End Sub

    End Class

End Namespace
