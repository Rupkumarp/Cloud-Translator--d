Imports System.IO
Imports System.ComponentModel
Imports CloudTranslator

Public Class Doc

    Implements iXliff

    Public Property ActiveProject As ProjectDetail Implements iXliff.ActiveProject

    Public Property cnt_newintegrated As Integer Implements iXliff.cnt_newintegrated

    Public Property cnt_newtrans As Integer Implements iXliff.cnt_newtrans

    Public Property CPS As CloudProjectsettings Implements iXliff.CPS

    Public Property curlang As String() Implements iXliff.curlang

    Public Property IP As InitiateProcess Implements iXliff.IP

    Public Property tr_type As TranslationType Implements iXliff.tr_type

    Public Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As Form_MainNew.RtbColor)
        Dim str As New ArrayList
        str.Add(Msg)
        str.Add(MyColor)
        _BW.ReportProgress(4, str)
    End Sub


    Private _BW As BackgroundWorker

    Public Sub StartProcessing(myFile As String, ByRef BW As BackgroundWorker) Implements iXliff.StartProcessing
        _BW = BW

        If ActiveProject.bImportExistingtranslationsintoDB Then
            ImportExistingTranslationToDB()
            Exit Sub
        End If

        Try
            For f = 0 To UBound(curlang)

                CPS = New CloudProjectsettings(ActiveProject, myFile, curlang(f))

                If File.Exists(CPS._PD.ProjectPath & CloudProjectsettings.Folder_TobeTransalted & Path.GetFileNameWithoutExtension(myFile) & "_" & curlang(f) & Path.GetExtension(myFile)) Then

                    If File.Exists(CPS._PD.ProjectPath & CloudProjectsettings.Folder_OutPut & Path.GetFileNameWithoutExtension(myFile) & "_" & curlang(f) & Path.GetExtension(myFile)) Then
                        'complete
                        UpdateMsg(Now & Chr(9) & " - File in " & curlang(f) & " already translated." & vbCrLf, Form_MainNew.RtbColor.Black)
                    Else
                        'translation back but not yet copied to 05 -> let's copy
                        If File.Exists(CPS._PD.ProjectPath & CloudProjectsettings.Folder_BackFromTranslation & Path.GetFileNameWithoutExtension(myFile) & "_" & curlang(f) & Path.GetExtension(myFile)) Then
                            File.Copy(CPS._PD.ProjectPath & CloudProjectsettings.Folder_BackFromTranslation & Path.GetFileNameWithoutExtension(myFile) & "_" & curlang(f) & Path.GetExtension(myFile), CPS._PD.ProjectPath & CloudProjectsettings.Folder_OutPut & Path.GetFileNameWithoutExtension(myFile) & "_" & curlang(f) & Path.GetExtension(myFile))
                            UpdateMsg(Now & Chr(9) & " - Translation is back in " & curlang(f) & "!" & vbCrLf, Form_MainNew.RtbColor.Black)
                            cnt_newintegrated = cnt_newintegrated + 1
                        Else
                            'translation not yet present
                            UpdateMsg(Now & Chr(9) & " - Translation not yet present in " & curlang(f) & "." & vbCrLf, Form_MainNew.RtbColor.Black)
                        End If
                    End If
                Else
                    'file not yet created for translation.
                    File.Copy(myFile, CPS._PD.ProjectPath & CloudProjectsettings.Folder_TobeTransalted & Path.GetFileNameWithoutExtension(myFile) & "_" & curlang(f) & Path.GetExtension(myFile))
                    UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(myFile) & " - File for translation created in " & curlang(f) & "." & vbCrLf, Form_MainNew.RtbColor.Black)
                    cnt_newtrans = cnt_newtrans + 1
                End If
            Next
        Catch ex As Exception
            _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, Path.GetFileNameWithoutExtension(myFile), ex.Message})
            Throw New Exception(ex.Message)
        End Try
    End Sub


    Public Sub CleanTransaltion() Implements iXliff.CleanTransaltion

    End Sub


    Public Sub CreateOutFile() Implements iXliff.CreateOutFile

    End Sub

    Public Function ExtractXliff() As Boolean Implements iXliff.ExtractXliff
        Return True
    End Function


    Public Sub ImportExistingTranslationToDB() Implements iXliff.ImportExistingTranslationToDB
        UpdateMsg(Now & Chr(9) & "DB update not implemented for DOC file" & vbCrLf, Form_MainNew.RtbColor.Red)
    End Sub

    Public Sub ExtractXiffwithExistingTranslation() Implements iXliff.ExtractXiffwithExistingTranslation
        UpdateMsg(Now & Chr(9) & "DB update not implemented for DOC file" & vbCrLf, Form_MainNew.RtbColor.Red)
    End Sub
End Class
