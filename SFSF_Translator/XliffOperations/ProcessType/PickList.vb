Imports System.IO
Imports System.ComponentModel
Imports CloudTranslator

Public Class PickList

    Implements iXliff

    Private Property ActiveProject As ProjectDetail Implements iXliff.ActiveProject

    Public Property cnt_newintegrated As Integer Implements iXliff.cnt_newintegrated

    Public Property cnt_newtrans As Integer Implements iXliff.cnt_newtrans

    Private Property CPS As CloudProjectsettings Implements iXliff.CPS

    Private Property curlang As String() Implements iXliff.curlang

    Private Property IP As InitiateProcess Implements iXliff.IP

    Private Property tr_type As TranslationType Implements iXliff.tr_type

    Public Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As Form_MainNew.RtbColor)
        Dim str As New ArrayList
        str.Add(Msg)
        str.Add(MyColor)
        _BW.ReportProgress(4, str)
    End Sub

    Private _sFile As String

    Private PTD As PreTranslateFileDetials

    Private _BW As BackgroundWorker

    Public Sub Extractxliff_file()
        If ExtractXliff() Then
            cnt_newtrans += 1
            IP.Pretranslate()
            'If all the enUS is pretranslated then Launch Reintegration of Pretranslated file
            If IP.ReIntegrateXliffAfterPretranslate() Then
                CreateOutFile()
                cnt_newtrans -= 1
            End If

        End If
    End Sub

    Public Sub CreateOutput_file()
        CreateOutFile()
    End Sub

    Public Sub Removetranslation()
        If ActiveProject.isCleanRequired Then
            CleanTransaltion() 'Laurent to advice
        End If
    End Sub

    Private Sub InitialCurrentProjectSetting(ByRef _CPS As CloudProjectsettings)
        CPS = _CPS
    End Sub

    Public Sub StartProcessing(sFile As String, ByRef BW As BackgroundWorker) Implements iXliff.StartProcessing
        _BW = BW
        _sFile = sFile
        Dim bCleaned As Boolean = False
        UpdateMsg(Now & Chr(9) & "File Type - Picklist" & vbCrLf, Form_MainNew.RtbColor.Black)

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


    Private Sub LL_UpdateMsg(Msg As String, RTBC As Cls_GetLangList.RtbColor) Handles LL.UpdateMsg
        UpdateMsg(Msg, RTBC)
    End Sub

    Private Sub CT_UpdateMsg(Msg As String, RTBC As CleanTranslation.RtbColor) Handles CT.UpdateMsg
        UpdateMsg(Msg, RTBC)
    End Sub


    Private WithEvents LL As New Cls_GetLangList
    Private WithEvents CT As New CleanTranslation

    Private Sub CleanTransaltion() Implements iXliff.CleanTransaltion
        Try
            UpdateMsg(Now & Chr(9) & "Cleaning Transaltion Process started for  - " & Path.GetFileName(_sFile) & vbCrLf, Form_MainNew.RtbColor.Black)
            Dim _fDetails As New FileDetails
            _fDetails = LL.LangList(_sFile)
            CT.RemoveTranslationMDF(_sFile, curlang, _fDetails, 0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private WithEvents CU As New Cls_CloudJob
    Private Sub CU_Progress(Max As Integer, val As Integer) Handles CU.Progress
        Dim str As New ArrayList
        str.Add(Max)
        str.Add(val)
        _BW.ReportProgress(3, str)
        _BW.ReportProgress(0, val & "\" & Max)
    End Sub

    Private Sub CU_UpdateMsg(Msg As String, RTBC As Cls_CloudJob.RtbColor) Handles CU.UpdateMsg
        UpdateMsg(Msg, RTBC)
    End Sub


    Private Sub CU_UpdateToolstripMsg(Msg As String) Handles CU.UpdateToolstripMsg
        _BW.ReportProgress(0, Msg)
    End Sub


    Private Sub CreateOutFile() Implements iXliff.CreateOutFile
        Try
            If File.Exists(CPS.OutFile) Then
                'we then use the existing file to add one language
                UpdateMsg(Now & Chr(9) & "Multilingual Piclist. File has already been integrated. Adding language. pleast wait..." & vbCrLf, Form_MainNew.RtbColor.Black)
                If Mod_PickList.ToCsv(CPS.OutFile, CPS.Xliff_FileInBackFromtranslation, CPS.CurrentLang, PickListType.UnManagePicklist, _BW) Then
                    cnt_newintegrated = cnt_newintegrated + 1
                    File.Move(CPS.Xliff_FileInBackFromtranslation, CPS.Xliff_ProcessedFileInBackFromtranslation)
                Else
                    UpdateMsg(Now & Chr(9) & Msg & vbCrLf, Form_MainNew.RtbColor.Black)
                End If

            Else
                'first language we integrate => we use the original file
                UpdateMsg(Now & Chr(9) & "Multilingual Picklist file. Integrating first language. please wait..." & vbCrLf, Form_MainNew.RtbColor.Black)
                If Mod_PickList.ToCsv(_sFile, CPS.Xliff_FileInBackFromtranslation, CPS.CurrentLang, PickListType.UnManagePicklist, _BW) Then
                    cnt_newintegrated += 1
                    File.Move(CPS.Xliff_FileInBackFromtranslation, CPS.Xliff_ProcessedFileInBackFromtranslation)
                Else
                    UpdateMsg(Now & Chr(9) & Msg & vbCrLf, Form_MainNew.RtbColor.Black)
                End If
            End If

            UpdateMsg(Now & Chr(9) & "Integration done for " & CPS.CurrentLang & vbCrLf, Form_MainNew.RtbColor.Black)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


    End Sub

    Private Msg As String = ""
    Private Function ExtractXliff() As Boolean Implements iXliff.ExtractXliff
        Try
            Msg = Mod_PickList.ToXliff(_sFile, CPS._PD.ProjectPath & CloudProjectsettings.Folder_TobeTransalted, CPS.CurrentLang, PickListType.UnManagePicklist, CPS._PD.isMaxLengthCheckRequired)
            If Msg <> "" Then
                UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(_sFile) & Msg & vbCrLf, Form_MainNew.RtbColor.Green)
                _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, System.IO.Path.GetFileName(_sFile), Msg})
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
        Try
            If Not File.Exists(CPS.Xliff_ExistingTranslationFile) Then
                Msg = Mod_PickList.ToXliff(_sFile, CPS._PD.ProjectPath & CloudProjectsettings.Folder_ExistingTranslation, CPS.CurrentLang, PickListType.UnManagePicklist, CPS._PD.isMaxLengthCheckRequired, True)
                If Msg <> "" Then
                    UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(_sFile) & Msg & vbCrLf, Form_MainNew.RtbColor.Green)
                    _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, System.IO.Path.GetFileName(_sFile), Msg})
                Else
                    UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sFile) & ".xliff - File in " & CPS.CurrentLang & " Created in 09-ExitingTranslation folder." & vbCrLf, Form_MainNew.RtbColor.Black)
                End If
            Else
                UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sFile) & " - Xliff file in " & CPS.CurrentLang & " exists in 09-ExitingTranslation folder. Updating to DB" & vbCrLf, Form_MainNew.RtbColor.Black)
            End If

            If File.Exists(CPS.Xliff_ExistingTranslationFile) Then
                CU.UpdateDB(CPS.Xliff_ExistingTranslationFile, System.IO.Path.GetFileNameWithoutExtension(_sFile), , , CPS._PD.isMaxLengthCheckRequired)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Sub ExtractXiffwithExistingTranslation() Implements iXliff.ExtractXiffwithExistingTranslation
        Try
            If Not File.Exists(CPS.Xliff_ExistingTranslationFile) Then
                Msg = Mod_PickList.ToXliff(_sFile, CPS._PD.ProjectPath & CloudProjectsettings.Folder_ExistingTranslation, CPS.CurrentLang, PickListType.UnManagePicklist, CPS._PD.isMaxLengthCheckRequired, True)
                If Msg <> "" Then
                    UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(_sFile) & Msg & vbCrLf, Form_MainNew.RtbColor.Green)
                    _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, System.IO.Path.GetFileName(_sFile), Msg})
                Else
                    UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sFile) & ".xliff - File in " & CPS.CurrentLang & " Created in 09-ExitingTranslation folder." & vbCrLf, Form_MainNew.RtbColor.Black)
                End If
            Else
                UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sFile) & " - Xliff file in " & CPS.CurrentLang & " exists in 09-ExitingTranslation folder." & vbCrLf, Form_MainNew.RtbColor.Black)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

End Class
