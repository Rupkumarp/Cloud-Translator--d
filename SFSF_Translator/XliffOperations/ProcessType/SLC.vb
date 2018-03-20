Imports System.IO
Imports System.ComponentModel
Imports CloudTranslator

Public Class SLC

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

    Public Sub CreateXliff(sFile As String, ByRef BW As BackgroundWorker) Implements iXliff.StartProcessing
        _BW = BW
        _sFile = sFile

        Try
            'xml ( xml C/P)
            UpdateMsg(Now & Chr(9) & "File is of type xml." & vbCrLf, Form_MainNew.RtbColor.Black)

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

    Public Sub CleanTransaltion() Implements iXliff.CleanTransaltion
        Throw New Exception("SLC clean translation not implemented")
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


    Public Sub CreateOutFile() Implements iXliff.CreateOutFile
        If tr_type = TranslationType.Monolingual Then
            UpdateMsg(Now & Chr(9) & "Monolingual xml. Creating new file in " & CPS.CurrentLang & "." & vbCrLf, Form_MainNew.RtbColor.Black)
            Mod_SLC_To_Xml.SLC_to_xml(_sFile, CPS.Xliff_FileInBackFromtranslation, TranslationType.Monolingual, CPS.CurrentLang)
            'File.Delete(Path.GetDirectoryName(Replace(myFile, "01-Input-B", "05-Output")) & "\" & Path.GetFileName(myFile))
        Else
            If File.Exists(Path.GetDirectoryName(Replace(_sFile, "01-Input-B", "05-Output")) & "\" & Path.GetFileName(_sFile)) Then
                'we then use the existing file to add one language
                UpdateMsg(Now & Chr(9) & "Multilingual xml. File has already been integrated. Adding language. please wait..." & vbCrLf, Form_MainNew.RtbColor.Black)
                Mod_SLC_To_Xml.SLC_to_xml(CPS.OutFile, CPS.Xliff_FileInBackFromtranslation, TranslationType.Multilingual, CPS.CurrentLang)
            Else
                'first language we integrate => we use the original file
                UpdateMsg(Now & Chr(9) & "Multilingual xml. Integrating first language. please wait..." & vbCrLf, Form_MainNew.RtbColor.Black)
                File.Copy(_sFile, CPS.OutFile)
                Mod_SLC_To_Xml.SLC_to_xml(CPS.OutFile, CPS.Xliff_FileInBackFromtranslation, TranslationType.Multilingual, CPS.CurrentLang)
            End If
        End If

        UpdateMsg(Now & Chr(9) & "Integration done for " & CPS.CurrentLang & vbCrLf, Form_MainNew.RtbColor.Black)

        cnt_newintegrated = cnt_newintegrated + 1

        File.Move(CPS.Xliff_FileInBackFromtranslation, CPS.Xliff_ProcessedFileInBackFromtranslation)
    End Sub

    Public Function ExtractXliff() As Boolean Implements iXliff.ExtractXliff
        Try
            If Not Mod_SLC_To_Xliff.SLC_To_Xliff(_sFile, CPS._PD.ProjectPath & CloudProjectsettings.Folder_TobeTransalted, CPS.CurrentLang) Then
                Return False
            End If
            UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sFile) & ".xliff - File in " & CPS.CurrentLang & " generated for translators." & vbCrLf, Form_MainNew.RtbColor.Black)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function


    Public Sub ImportExistingTranslationToDB() Implements iXliff.ImportExistingTranslationToDB
        UpdateMsg(Now & Chr(9) & "Updating to DB not Implemented for SLC" & vbCrLf, Form_MainNew.RtbColor.Red)
    End Sub

    Public Sub ExtractXiffwithExistingTranslation() Implements iXliff.ExtractXiffwithExistingTranslation
        UpdateMsg(Now & Chr(9) & "ExtractXiffwithExistingTranslation not Implemented for SLC" & vbCrLf, Form_MainNew.RtbColor.Red)
    End Sub
End Class
