Imports System.Runtime.CompilerServices
Imports System.Text
Imports Media = System.Windows.Media
Imports Reflect = System.Reflection
Imports SIO = System.IO

Namespace Common

    Partial Class Utilities

        Public Class IO

            Private Sub New()
                Throw New NotSupportedException()
            End Sub

            Public Shared Function EnsureExists(ByVal Path As String) As Boolean
                If Not SIO.Directory.Exists(Path) Then
                    SIO.Directory.CreateDirectory(Path)
                    Return True
                Else
                    Return False
                End If
            End Function

            Public Shared Function DownloadURL(ByVal URL As String) As String
                Dim Request = Net.WebRequest.Create(URL)
                Using Response = Request.GetResponse(),
                      Reader = New SIO.StreamReader(Response.GetResponseStream())
                    Dim Res = Reader.ReadToEnd()

                    Return Res
                End Using
            End Function

            Public Shared Sub DownloadURLToFile(ByVal URL As String, ByVal Path As String)
                Dim Request = Net.WebRequest.Create(URL)
                Using Response = Request.GetResponse(),
                      WStream = Response.GetResponseStream(),
                      FStream = SIO.File.Open(Path, SIO.FileMode.CreateNew, SIO.FileAccess.Write, SIO.FileShare.Read)
                    FStream.Write(WStream)
                End Using
            End Sub

            ''' <summary>
            ''' Replaces invalid file name characters with '_'.
            ''' </summary>
            ''' <param name="Name"></param>
            ''' <returns></returns>
            Public Shared Function CorrectFileName(ByVal Name As String) As String
                Dim Res = New StringBuilder(Name.Length)
                Dim Invalids = SIO.Path.GetInvalidFileNameChars()
                For Each Ch In Name
                    If Invalids.Contains(Ch) Then
                        Res.Append("_"c)
                    Else
                        Res.Append(Ch)
                    End If
                Next

                Return Res.ToString().Trim()
            End Function

            Public Shared Function ReadAll(ByVal Reader As ReadCall, ByVal Buffer As Byte(), ByVal Offset As Integer, ByVal Length As Integer) As Integer
                Dim N = Length

                Do
                    Dim T = Reader.Invoke(Buffer, Offset + Length - N, N)
                    If T = 0 Then
                        Return Length - N
                    End If
                    N -= T
                Loop Until N = 0

                Return Length
            End Function

            Public Delegate Sub WriteCall(ByVal Buffer As Byte(), ByVal Offset As Integer, ByVal Length As Integer)
            Public Delegate Function ReadCall(ByVal Buffer As Byte(), ByVal Offset As Integer, ByVal MaxLength As Integer) As Integer

        End Class

    End Class

End Namespace
