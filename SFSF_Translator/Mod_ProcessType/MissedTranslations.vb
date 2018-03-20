Public Class MissedTranslations

    Public Sub UpdateMsg(ByVal notfound As ArrayList, ByVal FileName As String, ByVal Lang As String)

        Try
            If notfound.Count > 0 Then
                notFoundDetails.AppendLine(vbCrLf & "****" & notfound.Count & " - Translation missing for - " & Lang & " - " & System.IO.Path.GetFileName(FileName) & "****")
                notFoundDetails.AppendLine("******************************************************************************************************************************************************************************************************************************************************************************************")
                For i As Integer = 0 To notfound.Count - 1
                    If notfound(i).ToString.Length > 200 Then
                        notFoundDetails.AppendLine(notfound(i).ToString.Substring(0, 197))
                    Else
                        notFoundDetails.AppendLine(notfound(i))
                    End If
                Next
                notFoundDetails.AppendLine("******************************************************************************************************************************************************************************************************************************************************************************************")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
       
    End Sub

End Class
