Imports System.IO
Imports System.ComponentModel
Imports CloudTranslator

Public Class QuestionLib

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


    Private _sFile As String

    Private PTD As PreTranslateFileDetials

    Private _BW As BackgroundWorker

    Private Sub InitialCurrentProjectSetting(ByRef _CPS As CloudProjectsettings)
        CPS = _CPS
    End Sub

    Public Sub StartProcessing(sFile As String, ByRef BW As BackgroundWorker) Implements iXliff.StartProcessing
        _BW = BW
        _sFile = sFile
        Dim Msg As String = ""
        Try
            IP = New InitiateProcess(ActiveProject, BW, sFile, curlang)
            AddHandler IP.Extractxliff, AddressOf Extractxliff_file
            AddHandler IP.CleanTransaltion, AddressOf Removetranslation
            AddHandler IP.CreateOutFile, AddressOf CreateOutput_file
            AddHandler IP.InitializeCurrentProjectSetting, AddressOf InitialCurrentProjectSetting
            AddHandler IP.ImportExistingTranslationToDB, AddressOf ImportExistingTranslationToDB
            AddHandler IP.ExtractXliffWithExistingTranslation, AddressOf ExtractXiffwithExistingTranslation
            IP.StartProcessing()
        Catch ex As Exception
            BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, System.IO.Path.GetFileName(_sFile), ex.Message})
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Sub Extractxliff_file()
        Try
            If ExtractXliff() Then
                cnt_newtrans += 1
                IP.Pretranslate()
                'If all the enUS is pretranslated then Launch Reintegration of Pretranslated file
                If IP.ReIntegrateXliffAfterPretranslate() Then
                    CreateOutFile()
                    cnt_newtrans -= 1
                End If
            Else
                UpdateMsg(Now & Chr(9) & Path.GetFileName(_sFile) & " - already has translation or No enUS source found for " & CPS.CurrentLang & "." & vbCrLf, Form_MainNew.RtbColor.Green)
                _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, System.IO.Path.GetFileName(_sFile), "Already has translation or No enUS source found for " & CPS.CurrentLang})
            End If
        Catch ex As Exception
            If ex.Message.ToLower.Contains("no definition file found") Then
                UpdateMsg(Now & Chr(9) & CPS.CurrentLang & " - " & ex.Message & vbCrLf, Form_MainNew.RtbColor.Red)
                _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, System.IO.Path.GetFileName(_sFile), "No definition file found"})
            Else
                Throw New Exception(ex.Message)
            End If
        End Try

    End Sub

    Public Sub CreateOutput_file()
        CreateOutFile()
    End Sub

    Public Sub Removetranslation()
        If tr_type = TranslationType.Multilingual Then
            If ActiveProject.isCleanRequired Then
                CleanTransaltion()
            End If
        End If

    End Sub

    Public Sub CleanTransaltion() Implements iXliff.CleanTransaltion
        Throw New Exception("Question lib Cleaning not implemented")
    End Sub

    Private WithEvents CU As New Cls_CloudJob
    Private Sub CU_Progress(Max As Integer, val As Integer) Handles CU.Progress
        Dim str As New ArrayList
        str.Add(Max)
        str.Add(val)
        _BW.ReportProgress(3, str)
        'Form_Main.UpdateProgressBar(Max, val)
    End Sub

    Private Sub CU_UpdateMsg(Msg As String, RTBC As Cls_CloudJob.RtbColor) Handles CU.UpdateMsg
        UpdateMsg(Msg, RTBC)
    End Sub

    Private Sub CU_UpdateToolstripMsg(Msg As String) Handles CU.UpdateToolstripMsg
        _BW.ReportProgress(0, Msg)
    End Sub


    Public Sub CreateOutFile() Implements iXliff.CreateOutFile
        Try
            UpdateMsg(Now & Chr(9) & "Outfile created." & vbCrLf, Form_MainNew.RtbColor.Black)
            If Mod_QuestionLib.ToCsv(_sFile, CPS.Xliff_FileInBackFromtranslation, CPS.CurrentLang, _BW) Then
                cnt_newintegrated = cnt_newintegrated + 1
                File.Move(CPS.Xliff_FileInBackFromtranslation, CPS.Xliff_ProcessedFileInBackFromtranslation)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Msg As String = ""
    Public Function ExtractXliff() As Boolean Implements iXliff.ExtractXliff
        Try
            Msg = Mod_QuestionLib.ToXliff(_sFile, CPS._PD.ProjectPath & CloudProjectsettings.Folder_TobeTransalted, CPS.CurrentLang)
            If Msg <> "" Then
                UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(_sFile) & Msg & vbCrLf, Form_MainNew.RtbColor.Green)
                _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, System.IO.Path.GetFileName(_sFile), CPS.CurrentLang & " - Already has translation"})
                Return False
            Else
                UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sFile) & ".xliff - File in " & CPS.CurrentLang & " generated for translators." & vbCrLf, Form_MainNew.RtbColor.Black)
                Return True
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function


    Public Sub ImportExistingTranslationToDB() Implements iXliff.ImportExistingTranslationToDB
        UpdateMsg(Now & Chr(9) & "No translation found for " & CPS.CurrentLang & "." & vbCrLf, Form_MainNew.RtbColor.Black)
    End Sub

    Public Sub ExtractXiffwithExistingTranslation() Implements iXliff.ExtractXiffwithExistingTranslation
        UpdateMsg(Now & Chr(9) & "ExtractXiffwithExistingTranslation is not implemented " & CPS.CurrentLang & "." & vbCrLf, Form_MainNew.RtbColor.Black)
    End Sub
End Class
