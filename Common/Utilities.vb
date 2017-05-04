Imports System.Runtime.CompilerServices
Imports System.Text
Imports Media = System.Windows.Media
Imports Reflect = System.Reflection
Imports SIO = System.IO

Namespace Common

    Public NotInheritable Class Utilities

        Private Sub New()
            Throw New NotSupportedException()
        End Sub

#Region "CombineHashCodes Logic"
        Public Shared Function CombineHashCodes(ByVal H1 As Integer,
                                                ByVal H2 As Integer) As Integer
            ' ToDo Return ((H1 << 5) + H1) Xor H2
            Return ((H1 << 5) Or (H1 >> 27)) Xor H2
        End Function

        Public Shared Function CombineHashCodes(ByVal H1 As Integer,
                                                ByVal H2 As Integer,
                                                ByVal H3 As Integer) As Integer
            Return CombineHashCodes(CombineHashCodes(H1, H2), H3)
        End Function

        Public Shared Function CombineHashCodes(ByVal H1 As Integer,
                                                ByVal H2 As Integer,
                                                ByVal H3 As Integer,
                                                ByVal H4 As Integer) As Integer
            Return CombineHashCodes(CombineHashCodes(H1, H2), CombineHashCodes(H3, H4))
        End Function

        Public Shared Function CombineHashCodes(ByVal H1 As Integer,
                                                ByVal H2 As Integer,
                                                ByVal H3 As Integer,
                                                ByVal H4 As Integer,
                                                ByVal H5 As Integer) As Integer
            Return CombineHashCodes(CombineHashCodes(H1, H2), CombineHashCodes(H3, H4, H5))
        End Function

        Public Shared Function CombineHashCodes(ByVal H1 As Integer,
                                                ByVal H2 As Integer,
                                                ByVal H3 As Integer,
                                                ByVal H4 As Integer,
                                                ByVal H5 As Integer,
                                                ByVal H6 As Integer) As Integer
            Return CombineHashCodes(CombineHashCodes(H1, H2, H3), CombineHashCodes(H4, H5, H6))
        End Function

        Public Shared Function CombineHashCodes(ByVal H1 As Integer,
                                                ByVal H2 As Integer,
                                                ByVal H3 As Integer,
                                                ByVal H4 As Integer,
                                                ByVal H5 As Integer,
                                                ByVal H6 As Integer,
                                                ByVal H7 As Integer) As Integer
            Return CombineHashCodes(CombineHashCodes(H1, H2, H3), CombineHashCodes(H4, H5, H6, H7))
        End Function

        Public Shared Function CombineHashCodes(ByVal H1 As Integer,
                                                ByVal H2 As Integer,
                                                ByVal H3 As Integer,
                                                ByVal H4 As Integer,
                                                ByVal H5 As Integer,
                                                ByVal H6 As Integer,
                                                ByVal H7 As Integer,
                                                ByVal H8 As Integer) As Integer
            Return CombineHashCodes(CombineHashCodes(H1, H2, H3, H4), CombineHashCodes(H5, H6, H7, H8))
        End Function
#End Region

#Region "Empties Group"
        Public Shared EmptyObject As Object = New Object()

        Public NotInheritable Class Typed(Of T)

            Private Sub New()
                Throw New NotSupportedException()
            End Sub

            Public Shared ReadOnly IdentityFunc As Func(Of T, T) = Function(X) X
            Public Shared ReadOnly EmptyArray As T() = New T(-1) {}

        End Class
#End Region

        Public Function HslToRgb(ByVal H As Double, ByVal S As Double, ByVal L As Double) As (R As Double, G As Double, B As Double)
            Throw New NotImplementedException()
        End Function

        Public Function HsbToRgb(ByVal H As Double, ByVal S As Double, ByVal B As Double) As (R As Double, G As Double, B As Double)
            Throw New NotImplementedException()
        End Function

        Public Shared Sub DoNothing()

        End Sub

    End Class

End Namespace
