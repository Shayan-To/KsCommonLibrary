Imports System.Runtime.CompilerServices
Imports System.Text
Imports Media = System.Windows.Media
Imports Reflect = System.Reflection
Imports SIO = System.IO

Namespace Common

    Partial Class Utilities

        Public Class Debug

            Private Sub New()
                Throw New NotSupportedException()
            End Sub

            Public Shared Sub ShowMessageBox(text As String, Optional caption As String = "")
                Forms.MessageBox.Show(text, caption)
            End Sub

            Public Shared Function CompactStackTrace(ByVal Count As Integer) As String
                Dim ST = New StackTrace(True)
                Dim F = ST.GetFrames()

                If Count >= F.Length Then
                    Count = F.Length - 1
                End If

                Dim R = New StringBuilder()
                For I As Integer = 1 To Count
                    If I > 1 Then
                        R.Append(">"c)
                    End If
                    Dim M = F(I).GetMethod
                    R.Append(M.DeclaringType.Name) _
                     .Append("."c) _
                     .Append(M.Name) _
                     .Append(M.GetParameters().Length) _
                     .Append(":"c) _
                     .Append(F(I).GetFileLineNumber())
                Next

                Return R.ToString()
            End Function

            Public Shared Sub PrintCallInformation(ByVal ParamArray Args As Object())
                Dim R = New StringBuilder()

                Dim F = New StackFrame(1)
                Dim M = F.GetMethod()
                Dim P = M.GetParameters()
                F = New StackFrame(2, True)

                If Args.Length <> 0 And P.Length <> Args.Length Then
                    Throw New ArgumentException()
                End If

                R.Append(F.GetFileLineNumber) _
                 .Append(":") _
                 .Append(M.DeclaringType.Name) _
                 .Append("."c) _
                 .Append(M.Name) _
                 .Append("_"c) _
                 .Append(P.Length) _
                 .Append("("c)

                If Args.Length = 0 Then
                    If P.Length <> 0 Then
                        R.Append("...")
                    End If
                Else
                    Dim Bl = True
                    For I As Integer = 0 To Args.Length - 1
                        If Bl Then
                            Bl = False
                        Else
                            R.Append(", ")
                        End If
                        R.Append(P(I).Name).Append("="c).Append(Args(I))
                    Next
                End If

                R.AppendLine(")"c)

                Console.Write(R.ToString())
            End Sub

            Public Shared Function GetCallInformation(ByVal ParamArray Args As Object()) As String
                Dim R = New StringBuilder()

                Dim F = New StackFrame(1)
                Dim M = F.GetMethod()
                Dim P = M.GetParameters()
                F = New StackFrame(2, True)

                If Args.Length <> 0 And P.Length <> Args.Length Then
                    Throw New ArgumentException()
                End If

                R.Append(F.GetFileLineNumber) _
                 .Append(":") _
                 .Append(M.DeclaringType.Name) _
                 .Append("."c) _
                 .Append(M.Name) _
                 .Append(P.Length) _
                 .Append("("c)

                If Args.Length = 0 Then
                    If P.Length <> 0 Then
                        R.Append("...")
                    End If
                Else
                    Dim Bl = True
                    For Each A In Args
                        If Bl Then
                            Bl = False
                        Else
                            R.Append(", ")
                        End If
                        R.Append(A)
                    Next
                End If

                R.AppendLine(")"c)

                Return R.ToString()
            End Function

        End Class

    End Class

End Namespace
