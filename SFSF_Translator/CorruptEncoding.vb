Imports System.Text

Public Class CorruptEncoding

    Private _sFile As String

    Public Sub New(ByVal sFile As String)
        _sFile = sFile
    End Sub

    Public Function SearchCorruptedChars() As StringBuilder
        Dim CorruptChar As New StringBuilder
        Try

            Dim objReader As New System.IO.StreamReader(_sFile)
            Dim strTextFileInfo() As String = Nothing
            Dim arrCounter As Integer = 0
            Dim strTextToSearch As String = String.Empty

            Do While objReader.Peek <> -1
                ReDim Preserve strTextFileInfo(arrCounter)
                strTextFileInfo(arrCounter) = objReader.ReadLine
                arrCounter = arrCounter + 1
            Loop
            objReader.Close()

            Dim iStart As Integer

            Dim Mystr As String = ""
            For i As Integer = 0 To arrCounter - 1
                iStart = 0
                If (strTextFileInfo(i).ToLower.Contains("??") = True) Or (strTextFileInfo(i).ToLower.Contains("�") = True) Then
                    If strTextFileInfo(i).ToLower.Contains("??") Then
                        iStart = InStr(strTextFileInfo(i), "??")
                        If strTextFileInfo(i).ToString.Length < 100 Then
                            Mystr = Mid(strTextFileInfo(i), iStart, strTextFileInfo(i).ToString.Length)
                        Else
                            Mystr = Mid(strTextFileInfo(i), iStart - 15, 100)
                        End If
                        CorruptChar.AppendLine("Line Number " & i + 1 & vbTab & Mystr & Environment.NewLine)
                    ElseIf strTextFileInfo(i).ToLower.Contains("�") Then
                        iStart = InStr(strTextFileInfo(i), "�")
                        If strTextFileInfo(i).ToString.Length < 100 Then
                            Mystr = Mid(strTextFileInfo(i), iStart, strTextFileInfo(i).ToString.Length)
                        Else
                            Mystr = Mid(strTextFileInfo(i), iStart - 15, 100)
                        End If
                        CorruptChar.AppendLine("Line Number " & i + 1 & vbTab & Mystr & Environment.NewLine)
                    ElseIf strTextFileInfo(i).ToLower.Contains("Ð½") Then
                        iStart = InStr(strTextFileInfo(i), "Ð½")
                        If strTextFileInfo(i).ToString.Length < 100 Then
                            Mystr = Mid(strTextFileInfo(i), iStart, strTextFileInfo(i).ToString.Length)
                        Else
                            Mystr = Mid(strTextFileInfo(i), iStart - 15, 100)
                        End If
                        CorruptChar.AppendLine("Line Number " & i + 1 & vbTab & Mystr & Environment.NewLine)
                    End If
                    'CorruptChar.AppendLine("Line Number " & i + 1 & vbTab & strTextFileInfo(i) & Environment.NewLine)
                End If
            Next

        Catch ex As Exception
            Throw New Exception("Error @SearchCorruptedChars" & vbNewLine & ex.Message)
        End Try
        Return CorruptChar
    End Function


End Class
