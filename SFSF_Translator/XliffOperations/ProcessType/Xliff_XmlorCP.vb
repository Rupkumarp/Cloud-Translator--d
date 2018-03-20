Imports System.IO
Imports System.ComponentModel
Imports System.Xml
Imports CloudTranslator

Public Class Xliff_XmlorCP

    Implements iXliff

    Public Property ActiveProject As ProjectDetail Implements iXliff.ActiveProject

    Public Property cnt_newintegrated As Integer Implements iXliff.cnt_newintegrated

    Public Property cnt_newtrans As Integer Implements iXliff.cnt_newtrans

    Public Property CPS As CloudProjectsettings Implements iXliff.CPS

    Public Property curlang As String() Implements iXliff.curlang

    Public Property IP As InitiateProcess Implements iXliff.IP

    Public Property tr_type1 As TranslationType Implements iXliff.tr_type


    Private PTD As PreTranslateFileDetials

    Public Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As Form_MainNew.RtbColor)
        _bw.ReportProgress(4, {Msg, MyColor})
    End Sub

    Private tr_type As TranslationType

    Private Sub InitialCurrentProjectSetting(ByRef _CPS As CloudProjectsettings)
        CPS = _CPS
    End Sub

    Private WithEvents CU As New Cls_CloudJob
    Private Sub CU_Progress(Max As Integer, val As Integer) Handles CU.Progress
        Dim str As New ArrayList
        str.Add(Max)
        str.Add(val)
        _bw.ReportProgress(3, str)
        _bw.ReportProgress(0, val & "\" & Max)
    End Sub

    Private Sub CU_UpdateMsg(Msg As String, RTBC As Cls_CloudJob.RtbColor) Handles CU.UpdateMsg
        UpdateMsg(Msg, RTBC)
    End Sub


    Private Sub CU_UpdateToolstripMsg(Msg As String) Handles CU.UpdateToolstripMsg
        _bw.ReportProgress(0, Msg)
    End Sub

    Public Sub StartProcessing(sFile As String, ByRef BW As BackgroundWorker) Implements iXliff.StartProcessing
        _sfile = sFile
        tr_type = get_translation_type(sFile)
        _bw = BW

        Try
            'xml ( xml C/P)
            IP = New InitiateProcess(ActiveProject, BW, sFile, curlang)
            AddHandler IP.Extractxliff, AddressOf Extractxliff_file
            AddHandler IP.CleanTransaltion, AddressOf Removetranslation
            AddHandler IP.CreateOutFile, AddressOf CreateOutput_file
            AddHandler IP.InitializeCurrentProjectSetting, AddressOf InitialCurrentProjectSetting
            AddHandler IP.ImportExistingTranslationToDB, AddressOf ImportExistingTranslationToDB
            AddHandler IP.ExtractXliffWithExistingTranslation, AddressOf ExtractXiffwithExistingTranslation
            IP.StartProcessing()

        Catch ex As Exception
            BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, System.IO.Path.GetFileName(sFile), ex.Message})
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
                _bw.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, System.IO.Path.GetFileName(_sfile), "Already has translation or No enUS source found for " & CPS.CurrentLang})
            End If
        Catch ex As Exception
            If ex.Message.ToLower.Contains("no definition file found") Then
                UpdateMsg(Now & Chr(9) & CPS.CurrentLang & " - " & ex.Message & vbCrLf, Form_MainNew.RtbColor.Red)
                _bw.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, System.IO.Path.GetFileName(_sfile), "No definition file found"})
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

    Private _bw As BackgroundWorker

    Private Sub CT_Progress(Max As Integer, val As Integer) Handles CT.Progress
        Dim str As New ArrayList
        str.Add(Max)
        str.Add(val)
        _bw.ReportProgress(3, str)
        _bw.ReportProgress(0, val & "\" & Max)
    End Sub

    Private Sub CT_UpdateMsg(Msg As String, RTBC As CleanTranslation.RtbColor) Handles CT.UpdateMsg
        UpdateMsg(Msg, RTBC)
    End Sub

    Private WithEvents LL As New Cls_GetLangList
    Private WithEvents CT As New CleanTranslation
    Private _sfile As String

    Public Sub CleanTransaltion() Implements iXliff.CleanTransaltion
        Try
            UpdateMsg(Now & Chr(9) & "Cleaning Transaltion Process started for  - " & Path.GetFileName(_sfile) & vbCrLf, Form_MainNew.RtbColor.Black)
            CT.RemoveTranslationXML(_sfile, curlang, CPS._PD.bImportExistingtranslationsintoDB)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Function ExtractXliff() As Boolean Implements iXliff.ExtractXliff
        Try
            If Not Xml_ToXliff(_sfile, CPS._PD.ProjectPath & CloudProjectsettings.Folder_TobeTransalted, CPS.CurrentLang) Then
                Return False
            End If
            UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & ".xliff - File in " & CPS.CurrentLang & " generated for translators." & vbCrLf, Form_MainNew.RtbColor.Black)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Create Xml Out file
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CreateOutFile() Implements iXliff.CreateOutFile
        Try
            If tr_type = TranslationType.Monolingual Then
                UpdateMsg(Now & Chr(9) & "Monolingual xml. Creating new file in " & CPS.CurrentLang & "." & vbCrLf, Form_MainNew.RtbColor.Black)
                xliff_to_xml(_sfile, CPS.Xliff_FileInBackFromtranslation, TranslationType.Monolingual, CPS.CurrentLang)
                'File.Delete(Path.GetDirectoryName(Replace(myFile, "01-Input-B", "05-Output")) & "\" & Path.GetFileName(myFile))
            Else
                If File.Exists(Path.GetDirectoryName(Replace(_sfile, "01-Input-B", "05-Output")) & "\" & Path.GetFileName(_sfile)) Then
                    'we then use the existing file to add one language
                    UpdateMsg(Now & Chr(9) & "Multilingual xml. File has already been integrated. Adding language. pleast wait..." & vbCrLf, Form_MainNew.RtbColor.Black)
                    xliff_to_xml(CPS.OutFile, CPS.Xliff_FileInBackFromtranslation, TranslationType.Multilingual, CPS.CurrentLang)
                Else
                    'first language we integrate => we use the original file
                    UpdateMsg(Now & Chr(9) & "Multilingual xml. Integrating first language. please wait..." & vbCrLf, Form_MainNew.RtbColor.Black)
                    File.Copy(_sfile, CPS.OutFile)
                    xliff_to_xml(CPS.OutFile, CPS.Xliff_FileInBackFromtranslation, TranslationType.Multilingual, CPS.CurrentLang)
                End If
            End If

            UpdateMsg(Now & Chr(9) & "Integration done for " & CPS.CurrentLang & vbCrLf, Form_MainNew.RtbColor.Black)

            cnt_newintegrated += 1
            If System.IO.File.Exists(CPS.Xliff_ProcessedFileInBackFromtranslation) Then
                System.IO.File.Delete(CPS.Xliff_ProcessedFileInBackFromtranslation)
            End If
            File.Move(CPS.Xliff_FileInBackFromtranslation, CPS.Xliff_ProcessedFileInBackFromtranslation)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Public Sub ImportExistingTranslationToDB() Implements iXliff.ImportExistingTranslationToDB
        Try
            ExtractExistingTranslationfromInputFile()

            If File.Exists(CPS.Xliff_ExistingTranslationFile) Then
                CU.UpdateDB(CPS.Xliff_ExistingTranslationFile, System.IO.Path.GetFileNameWithoutExtension(_sfile), , , CPS._PD.isMaxLengthCheckRequired)
            Else
                UpdateMsg(Now & Chr(9) & "No translation found for " & CPS.CurrentLang & vbCrLf, Form_MainNew.RtbColor.Black)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Sub ExtractXiffwithExistingTranslation() Implements iXliff.ExtractXiffwithExistingTranslation
        Try
            ExtractExistingTranslationfromInputFile()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
    Private Sub ExtractExistingTranslationfromInputFile()
        If Not File.Exists(CPS.Xliff_ExistingTranslationFile) Then
            If Xml_ToXliff(_sfile, CPS._PD.ProjectPath & CloudProjectsettings.Folder_ExistingTranslation, CPS.CurrentLang, True) Then
                UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & ".xliff - File in " & CPS.CurrentLang & " Created in 09-ExitingTranslation folder." & vbCrLf, Form_MainNew.RtbColor.Black)
            End If
        Else
            UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & " - Xliff file in " & CPS.CurrentLang & " exists in 09-ExitingTranslation folder. Updating to DB" & vbCrLf, Form_MainNew.RtbColor.Black)
        End If

    End Sub
End Class

