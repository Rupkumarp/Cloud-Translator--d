Imports System.IO
Imports System.ComponentModel
Imports CloudTranslator

Public Class HybrisProperties_Cls

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

    Dim _sfile As String

    Private WithEvents CU As New Cls_CloudJob

    Public Sub StartProcessing(sFile As String, ByRef BW As BackgroundWorker) Implements iXliff.StartProcessing
        '_sfile = sFile
        '_BW = BW

        'If ActiveProject.bImportExistingtranslationsintoDB Then
        '    ImportExistingTranslationToDB()
        '    Exit Sub
        'End If

        'Dim Msg As String = ""
        'Try

        '    UpdateMsg(Now & Chr(9) & "File is of type Hybris Properties" & vbCrLf, Form_MainNew.RtbColor.Black)

        '    For f = 0 To UBound(curlang)

        '        CPS = New CloudProjectsettings(ActiveProject, sFile, curlang(f))

        '        If File.Exists(CPS.Xliff_FileInTobetransalted) Then
        '            UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(sFile) & " - File for translators in " & curlang(f) & " exists." & vbCrLf, Form_MainNew.RtbColor.Black)

        '            'check now if the translation is already back.
        '            If File.Exists(CPS.Xliff_FileInBackFromtranslation) Then
        '                If CPS._PD.isDBupdateRequired Then
        '                    CU.UpdateDB(CPS.Xliff_FileInBackFromtranslation, System.IO.Path.GetFileNameWithoutExtension(_sfile), , , CPS._PD.isMaxLengthCheckRequired)
        '                End If
        '                CreateOutFile()
        '            ElseIf File.Exists(CPS.Xliff_ProcessedFileInBackFromtranslation) Then
        '                UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(sFile) & " - File in " & curlang(f) & " already integrated." & vbCrLf, Form_MainNew.RtbColor.Black)
        '            Else
        '                UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(sFile) & " - File in " & curlang(f) & " not yet translated." & vbCrLf, Form_MainNew.RtbColor.Black)
        '            End If

        '        Else
        '            Dim TargetFile As String = Get_TargetFile_from_HybrisMappingFile(System.IO.Path.GetFileName(sFile), System.IO.Path.GetExtension(sFile), curlang(f))
        '            Msg = HybrisProperties.CreateXliff(sFile, TargetFile, curlang(f), CPS._PD.ProjectPath & CloudProjectsettings.Folder_TobeTransalted)
        '            If Msg = "" Then
        '                UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(sFile) & " - File in " & curlang(f) & " generated." & vbCrLf, Form_MainNew.RtbColor.Black)
        '                cnt_newtrans = cnt_newtrans + 1
        '            Else
        '                UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(sFile) & Msg & vbCrLf, Form_MainNew.RtbColor.Green)
        '                _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, Path.GetFileNameWithoutExtension(sFile), curlang(f) & " - Already has translation or no data found "})
        '            End If

        '        End If
        '    Next
        'Catch ex As Exception
        '    _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, Path.GetFileNameWithoutExtension(sFile), ex.Message})
        '    Throw New Exception(ex.Message)
        'End Try

        _BW = BW
        _sfile = sFile

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


    Public Sub CleanTransaltion() Implements iXliff.CleanTransaltion

    End Sub

    Public Function ExtractXliff() As Boolean Implements iXliff.ExtractXliff
        Try

            Dim TargetFile As String = String.Empty
            Try
                TargetFile = Get_TargetFile_from_HybrisMappingFile(System.IO.Path.GetFileName(_sfile), System.IO.Path.GetExtension(_sfile), CPS.CurrentLang)
            Catch ex As Exception
                TargetFile = String.Empty
            End Try

            Dim Msg As String = HybrisProperties.CreateXliff(_sfile, TargetFile, CPS.CurrentLang, CPS._PD.ProjectPath & CloudProjectsettings.Folder_TobeTransalted)
            If Msg <> "" Then
                UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(_sfile) & Msg & vbCrLf, Form_MainNew.RtbColor.Green)
                _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, Path.GetFileNameWithoutExtension(_sfile), CPS.CurrentLang & " - Already has translation or no data found "})
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


    'Public Sub CleanTransaltion() Implements iXliff.CleanTransaltion

    'End Sub

    Public Sub CreateOutFile() Implements iXliff.CreateOutFile

        UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & " - Translation is back!" & vbCrLf, Form_MainNew.RtbColor.Black)

        'first language we integrate => we use the original file

        'If ValidateHybrisMappingFile() <> True Then
        '    Throw New Exception(Now & Chr(9) & "#### ERROR Validating HybrisMapping.xml file #####" & vbCrLf)
        'End If
        Dim TargetFile As String = Get_TargetFile_from_HybrisMappingFile(System.IO.Path.GetFileName(_sfile), System.IO.Path.GetExtension(_sfile), CPS.CurrentLang)

        If HybrisProperties.CreatePropertiesBack(_sfile, TargetFile, CPS.CurrentLang, CPS.Xliff_FileInBackFromtranslation, _BW) Then
            UpdateMsg(Now & Chr(9) & "New output file created." & vbCrLf, Form_MainNew.RtbColor.Black)
            cnt_newintegrated = cnt_newintegrated + 1
            File.Move(CPS.Xliff_FileInBackFromtranslation, CPS.Xliff_ProcessedFileInBackFromtranslation)
        End If
    End Sub

    Public Sub ExtractXiffwithExistingTranslation() Implements iXliff.ExtractXiffwithExistingTranslation
        UpdateMsg(Now & Chr(9) & "ExtractXiffwithExistingTranslation not implemented for HybrisImpex" & vbCrLf, Form_MainNew.RtbColor.Red)
    End Sub

    'Public Function ExtractXliff() As Boolean Implements iXliff.ExtractXliff
    '    Return True
    'End Function



    'Public Sub ImportExistingTranslationToDB() Implements iXliff.ImportExistingTranslationToDB
    '    UpdateMsg(Now & Chr(9) & "DB update not implemented for HybrisProperties" & vbCrLf, Form_MainNew.RtbColor.Red)
    'End Sub

    'Private Sub CU_UpdateMsg(Msg As String, RTBC As Cls_CloudJob.RtbColor) Handles CU.UpdateMsg
    '    UpdateMsg(Msg, RTBC)
    'End Sub


    'Private Sub CU_UpdateToolstripMsg(Msg As String) Handles CU.UpdateToolstripMsg
    '    _BW.ReportProgress(0, Msg)
    'End Sub



End Class
