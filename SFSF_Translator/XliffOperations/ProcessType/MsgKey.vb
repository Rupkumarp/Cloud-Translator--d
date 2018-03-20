Imports System.IO
Imports System.ComponentModel
Imports CloudTranslator

Public Class MsgKey

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

    Private WithEvents CU As New Cls_CloudJob

    Private PTD As PreTranslateFileDetials

    Private _BW As BackgroundWorker

    Public Sub StartProcessing(sFile As String, ByRef BW As BackgroundWorker) Implements iXliff.StartProcessing
        _BW = BW
        _sfile = sFile

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
            BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, System.IO.Path.GetFileName(_sfile), ex.Message})
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub CT_UpdateMsg(Msg As String, RTBC As CleanTranslation.RtbColor) Handles CT.UpdateMsg, LL.UpdateMsg
        UpdateMsg(Msg, RTBC)
    End Sub


    Private WithEvents LL As New Cls_GetLangList
    Private WithEvents CT As New CleanTranslation

    Private _sfile As String

    Public Sub CleanTransaltion() Implements iXliff.CleanTransaltion
        Try
            UpdateMsg(Now & Chr(9) & "Cleaning Transaltion Process started for  - " & Path.GetFileName(_sfile) & vbCrLf, Form_MainNew.RtbColor.Black)
            Dim _fDetails As New FileDetails
            _fDetails = LL.LangList(_sfile)
            CT.RemoveTranslationMDF(_sfile, curlang, _fDetails, 0)
        Catch ex As Exception
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
                UpdateMsg(Now & Chr(9) & Path.GetFileName(_sfile) & " - already has translation or No enUS source found for " & CPS.CurrentLang & "." & vbCrLf, Form_MainNew.RtbColor.Green)
                _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, System.IO.Path.GetFileName(_sfile), "Already has translation or No enUS source found for " & CPS.CurrentLang})
            End If
        Catch ex As Exception
            If ex.Message.ToLower.Contains("no definition file found") Then
                UpdateMsg(Now & Chr(9) & CPS.CurrentLang & " - " & ex.Message & vbCrLf, Form_MainNew.RtbColor.Red)
                _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, System.IO.Path.GetFileName(_sfile), "No definition file found"})
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

    Private Sub InitialCurrentProjectSetting(ByRef _CPS As CloudProjectsettings)
        CPS = _CPS
    End Sub

    Public Sub CreateOutFile() Implements iXliff.CreateOutFile
        Try
            'Multilingual file
            If File.Exists(Path.GetDirectoryName(Replace(_sfile, "01-Input-B", "05-Output")) & "\" & Path.GetFileName(_sfile)) Then
                'we then use the existing file to add one language
                UpdateMsg(Now & Chr(9) & "Multilingual MsgKey. File has already been integrated. Adding language. pleast wait..." & vbCrLf, Form_MainNew.RtbColor.Black)
                Msg = Mod_MsgKey.ToCsv(CPS.OutFile, CPS.Xliff_FileInBackFromtranslation, CPS.CurrentLang, _BW)
                If Msg = True Then
                    cnt_newintegrated = cnt_newintegrated + 1
                Else
                    UpdateMsg(Now & Chr(9) & Msg & vbCrLf, Form_MainNew.RtbColor.Black)
                End If

            Else
                'first language we integrate => we use the original file
                UpdateMsg(Now & Chr(9) & "Multilingual MsgKey file. Integrating first language. please wait..." & vbCrLf, Form_MainNew.RtbColor.Black)
                Mod_MsgKey.ToCsv(_sfile, CPS.Xliff_FileInBackFromtranslation, CPS.CurrentLang, _BW)
                cnt_newintegrated = cnt_newintegrated + 1
            End If

            UpdateMsg(Now & Chr(9) & "Integration done for " & CPS.CurrentLang & vbCrLf, Form_MainNew.RtbColor.Black)

            File.Move(CPS.Xliff_FileInBackFromtranslation, CPS.Xliff_ProcessedFileInBackFromtranslation)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Msg As String = ""
    Public Function ExtractXliff() As Boolean Implements iXliff.ExtractXliff
        Try
            Msg = Mod_MsgKey.ToXliff(_sfile, CPS._PD.ProjectPath & CloudProjectsettings.Folder_TobeTransalted, CPS.CurrentLang, CPS._PD.isMaxLengthCheckRequired)
            If Msg <> "" Then
                UpdateMsg(Now & Chr(9) & Msg & vbCrLf, Form_MainNew.RtbColor.Green)
                _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, System.IO.Path.GetFileName(_sfile), CPS.CurrentLang & " - Already has translation"})
                Return False
            Else
                UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & ".xliff - File in " & CPS.CurrentLang & " generated for translators." & vbCrLf, Form_MainNew.RtbColor.Black)
                Return True
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function


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


    Public Sub ImportExistingTranslationToDB() Implements iXliff.ImportExistingTranslationToDB
        Try

            If Not File.Exists(CPS.Xliff_ExistingTranslationFile) Then
                Msg = Mod_MsgKey.ToXliff(_sfile, CPS._PD.ProjectPath & CloudProjectsettings.Folder_ExistingTranslation, CPS.CurrentLang, CPS._PD.isMaxLengthCheckRequired, True)
                If Msg <> "" Then
                    UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(_sfile) & Msg & vbCrLf, Form_MainNew.RtbColor.Green)
                    _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, System.IO.Path.GetFileName(_sfile), Msg})
                Else
                    UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & ".xliff - File in " & CPS.CurrentLang & " Created in 09-ExitingTranslation folder." & vbCrLf, Form_MainNew.RtbColor.Black)
                End If
            Else
                UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & " - Xliff file in " & CPS.CurrentLang & " exists in 09-ExitingTranslation folder. Updating to DB" & vbCrLf, Form_MainNew.RtbColor.Black)
            End If

            If File.Exists(CPS.Xliff_ExistingTranslationFile) Then
                CU.UpdateDB(CPS.Xliff_ExistingTranslationFile, System.IO.Path.GetFileNameWithoutExtension(_sfile), , , CPS._PD.isMaxLengthCheckRequired)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Sub ExtractXiffwithExistingTranslation() Implements iXliff.ExtractXiffwithExistingTranslation
        Try
            If Not File.Exists(CPS.Xliff_ExistingTranslationFile) Then
                Msg = Mod_MsgKey.ToXliff(_sfile, CPS._PD.ProjectPath & CloudProjectsettings.Folder_ExistingTranslation, CPS.CurrentLang, CPS._PD.isMaxLengthCheckRequired, True)
                If Msg <> "" Then
                    UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(_sfile) & Msg & vbCrLf, Form_MainNew.RtbColor.Green)
                    _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, System.IO.Path.GetFileName(_sfile), Msg})
                Else
                    UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & ".xliff - File in " & CPS.CurrentLang & " Created in 09-ExitingTranslation folder." & vbCrLf, Form_MainNew.RtbColor.Black)
                End If
            Else
                UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & " - Xliff file in " & CPS.CurrentLang & " exists in 09-ExitingTranslation folder." & vbCrLf, Form_MainNew.RtbColor.Black)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub ExtractExistingTranslationfromInputFile()

    End Sub
End Class
