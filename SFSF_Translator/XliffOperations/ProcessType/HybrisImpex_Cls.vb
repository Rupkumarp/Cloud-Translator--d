Imports System.IO
Imports System.ComponentModel
Imports CloudTranslator

Public Class HybrisImpex_Cls

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

    Public Sub StartProcessing(sFile As String, ByRef BW As BackgroundWorker) Implements iXliff.StartProcessing
        _BW = BW
        _sFile = sFile

        Dim Msg As String = ""
        Try

            UpdateMsg(Now & Chr(9) & "File is of type Hybris Impex" & vbCrLf, Form_MainNew.RtbColor.Black)

            IP = New InitiateProcess(ActiveProject, BW, sFile, curlang)
            AddHandler IP.Extractxliff, AddressOf Extractxliff_file
            AddHandler IP.CleanTransaltion, AddressOf Removetranslation
            AddHandler IP.CreateOutFile, AddressOf CreateOutput_file
            AddHandler IP.InitializeCurrentProjectSetting, AddressOf InitialCurrentProjectSetting
            AddHandler IP.ImportExistingTranslationToDB, AddressOf ImportExistingTranslationToDB
            AddHandler IP.ExtractXliffWithExistingTranslation, AddressOf ExtractXiffwithExistingTranslation
            IP.StartProcessing()
        Catch ex As Exception
            _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, Path.GetFileNameWithoutExtension(sFile), ex.Message})
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

    Private Sub InitialCurrentProjectSetting(ByRef _CPS As CloudProjectsettings)
        CPS = _CPS
    End Sub


    Public Sub CleanTransaltion() Implements iXliff.CleanTransaltion

    End Sub

    Public Sub CreateOutFile() Implements iXliff.CreateOutFile
        Try
            UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sFile) & " - Translation is back!" & vbCrLf, Form_MainNew.RtbColor.Black)

            'first language we integrate => we use the original file
            UpdateMsg(Now & Chr(9) & "New output file created." & vbCrLf, Form_MainNew.RtbColor.Black)
            Dim TargetFile As String = Get_TargetFile_from_HybrisMappingFile(System.IO.Path.GetFileName(_sFile), System.IO.Path.GetExtension(_sFile), CPS.CurrentLang)
            If HybrisImpex.xliff_To_Impex(_sFile, TargetFile, CPS.CurrentLang, CPS.Xliff_FileInBackFromtranslation, _BW) Then
                cnt_newintegrated = cnt_newintegrated + 1
                If System.IO.File.Exists(CPS.Xliff_ProcessedFileInBackFromtranslation) Then
                    System.IO.File.Delete(CPS.Xliff_ProcessedFileInBackFromtranslation)
                End If
                File.Move(CPS.Xliff_FileInBackFromtranslation, CPS.Xliff_ProcessedFileInBackFromtranslation)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Function ExtractXliff() As Boolean Implements iXliff.ExtractXliff
        Try
            Dim TargetFile As String = Get_TargetFile_from_HybrisMappingFile(System.IO.Path.GetFileName(_sFile), System.IO.Path.GetExtension(_sFile), CPS.CurrentLang)
            Dim Msg As String = HybrisImpex.CreateXliff(_sFile, TargetFile, CPS.CurrentLang, CPS._PD.ProjectPath & CloudProjectsettings.Folder_TobeTransalted)
            If Msg <> "" Then
                UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(_sFile) & Msg & vbCrLf, Form_MainNew.RtbColor.Green)
                _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, Path.GetFileNameWithoutExtension(_sFile), CPS.CurrentLang & " - Already has translation or no data found "})
                Return False
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function



    Public Sub ImportExistingTranslationToDB() Implements iXliff.ImportExistingTranslationToDB
        UpdateMsg(Now & Chr(9) & "DB update not implemented for HybrisImpex" & vbCrLf, Form_MainNew.RtbColor.Red)
    End Sub

    Public Sub ExtractXiffwithExistingTranslation() Implements iXliff.ExtractXiffwithExistingTranslation
        UpdateMsg(Now & Chr(9) & "ExtractXiffwithExistingTranslation not implemented for HybrisImpex" & vbCrLf, Form_MainNew.RtbColor.Red)
    End Sub
End Class
