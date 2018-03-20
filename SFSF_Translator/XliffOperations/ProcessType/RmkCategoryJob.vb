Imports System.ComponentModel
Imports System.IO
Imports CloudTranslator

Public Class RmkCategoryJob

    Implements iXliff

    Public Property cnt_newintegrated As Integer Implements iXliff.cnt_newintegrated

    Public Property cnt_newtrans As Integer Implements iXliff.cnt_newtrans

    Public Property curlang As String() Implements iXliff.curlang

    Public Property tr_type As TranslationType Implements iXliff.tr_type

    Public Property IP As InitiateProcess Implements iXliff.IP

    Private WithEvents CU As New Cls_CloudJob

    Private PTD As PreTranslateFileDetials

    Public Property ActiveProject As ProjectDetail Implements iXliff.ActiveProject

    Public Property CPS As CloudProjectsettings Implements iXliff.CPS

    Public Sub New()
        '  AddHandler Mod_RMK_jobCategory.UpdateMsg, AddressOf UpdateMsg
    End Sub

    Public Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As Form_MainNew.RtbColor)
        Dim str As New ArrayList
        str.Add(Msg)
        str.Add(MyColor)
        _bw.ReportProgress(4, str)
    End Sub

    Public Sub Extractxliff_file()
        If ExtractXliff() Then
            cnt_newtrans += 1
            IP.Pretranslate()
            'If all the enUS is pretranslated then Launch Reintegration of Pretranslated file
            If IP.ReIntegrateXliffAfterPretranslate() Then
                cnt_newtrans -= 1
            End If
            CreateOutFile()
        End If
    End Sub

    Public Sub CreateOutput_file()
        CreateOutFile()
    End Sub

    Public Sub Removetranslation()
        If ActiveProject.isCleanRequired Then
            'CleanTransaltion() 'Laurent to advice
        End If
    End Sub

    Private Sub InitialCurrentProjectSetting(ByRef _CPS As CloudProjectsettings)
        CPS = _CPS
    End Sub

    Public Sub StartProcessing(sFile As String, ByRef BW As BackgroundWorker) Implements iXliff.StartProcessing
        _sfile = sFile
        _bw = BW
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
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private _bw As BackgroundWorker

    Private Sub CT_UpdateMsg(Msg As String, RTBC As CleanTranslation.RtbColor) Handles CT.UpdateMsg
        UpdateMsg(Msg, RTBC)
    End Sub

    Private WithEvents LL As New Cls_GetLangList
    Private WithEvents CT As New CleanTranslation
    Private _sfile As String

    Public Sub CleanTransaltion() Implements iXliff.CleanTransaltion
        Try
            UpdateMsg(Now & Chr(9) & "Cleaning Transaltion Process started for  - " & Path.GetFileName(_sfile) & vbCrLf, Form_MainNew.RtbColor.Black)
            CT.RemoveTranslationXML(_sfile, curlang)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub CU_Progress(Max As Integer, val As Integer) Handles CU.Progress
        Dim str As New ArrayList
        str.Add(Max)
        str.Add(val)
        _bw.ReportProgress(3, str)
        'Form_Main.UpdateProgressBar(Max, val)
    End Sub

    Private Sub CU_UpdateMsg(Msg As String, RTBC As Cls_CloudJob.RtbColor) Handles CU.UpdateMsg
        UpdateMsg(Msg, RTBC)
    End Sub

    Private Sub CU_UpdateToolstripMsg(Msg As String) Handles CU.UpdateToolstripMsg
        _bw.ReportProgress(0, Msg)
    End Sub

    Public Sub CreateOutFile() Implements iXliff.CreateOutFile
        Try
            If File.Exists(CPS.OutFile) Then
                'we then use the existing file to add one language
                UpdateMsg(Now & Chr(9) & "Integrating language - " & CPS.CurrentLang & vbCrLf, Form_MainNew.RtbColor.Black)
                'Mod_RMK_jobCategory.Rmk_JobCategory_Xliff_to_Xml(Path.GetDirectoryName(Replace(_sfile, "01-Input-B", "05-Output")) & "\" & Path.GetFileName(_sfile), Path.GetDirectoryName(Replace(_sfile, "01-Input-B", "03-Backfromtranslation")) & "\" & Path.GetFileNameWithoutExtension(_sfile) & "_" & CurrentLang & ".xliff", CurrentLang, _bw, False)
                Mod_RMK_jobCategory.Rmk_JobCategory_Xliff_to_Xml(CPS.OutFile, CPS.Xliff_FileInBackFromtranslation, CPS.CurrentLang, _bw, False)
            Else
                'first language we integrate => we use the original file
                UpdateMsg(Now & Chr(9) & "Integrating language - " & CPS.CurrentLang & vbCrLf, Form_MainNew.RtbColor.Black)
                'File.Copy(_sfile, Path.GetDirectoryName(Replace(_sfile, "01-Input-B", "05-Output")) & "\" & Path.GetFileName(_sfile))
                File.Copy(_sfile, CPS.OutFile)
                'Mod_RMK_jobCategory.Rmk_JobCategory_Xliff_to_Xml(Path.GetDirectoryName(Replace(_sfile, "01-Input-B", "05-Output")) & "\" & Path.GetFileName(_sfile), Path.GetDirectoryName(Replace(_sfile, "01-Input-B", "03-Backfromtranslation")) & "\" & Path.GetFileNameWithoutExtension(_sfile) & "_" & CurrentLang & ".xliff", CurrentLang, _bw, True)
                Mod_RMK_jobCategory.Rmk_JobCategory_Xliff_to_Xml(CPS.OutFile, CPS.Xliff_FileInBackFromtranslation, CPS.CurrentLang, _bw, True)
            End If


            cnt_newintegrated = cnt_newintegrated + 1
            If System.IO.File.Exists(CPS.Xliff_ProcessedFileInBackFromtranslation) Then
                System.IO.File.Delete(CPS.Xliff_ProcessedFileInBackFromtranslation)
            End If

            File.Move(CPS.Xliff_FileInBackFromtranslation, CPS.Xliff_ProcessedFileInBackFromtranslation)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Function ExtractXliff() As Boolean Implements iXliff.ExtractXliff
        Try
            If Mod_RMK_jobCategory.Rmk_JobCategory_Xml_to_Xliff(_sfile, CPS.Xliff_FileInTobetransalted, CPS.CurrentLang, curlang, _bw) Then
                Return False
            End If
            UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & ".xliff - File in " & CPS.CurrentLang & " generated for translators." & vbCrLf, Form_MainNew.RtbColor.Black)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function

    Public Sub ImportExistingTranslationToDB() Implements iXliff.ImportExistingTranslationToDB
        UpdateMsg(Now & Chr(9) & "DB update not implemented for RMKcategory" & vbCrLf, Form_MainNew.RtbColor.Red)
    End Sub

    Public Sub ExtractXiffwithExistingTranslation() Implements iXliff.ExtractXiffwithExistingTranslation
        UpdateMsg(Now & Chr(9) & "ExtractXiffwithExistingTranslation not implemented for RMKcategory" & vbCrLf, Form_MainNew.RtbColor.Red)
    End Sub
End Class
