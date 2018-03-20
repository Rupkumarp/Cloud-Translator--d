Imports System.ComponentModel
Imports System.IO
Imports CloudTranslator

Public Class DiboAutomate
    Implements iXliff

    Public Property ActiveProject As ProjectDetail Implements iXliff.ActiveProject


    Public Property cnt_newintegrated As Integer Implements iXliff.cnt_newintegrated


    Public Property cnt_newtrans As Integer Implements iXliff.cnt_newtrans

    Public Property CPS As CloudProjectsettings Implements iXliff.CPS


    Public Property curlang As String() Implements iXliff.curlang


    Public Property IP As InitiateProcess Implements iXliff.IP


    Public Property tr_type As TranslationType Implements iXliff.tr_type
    Private PTD As PreTranslateFileDetials

    Private _BW As BackgroundWorker

    Public Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As Form_MainNew.RtbColor)
        Dim str As New ArrayList
        str.Add(Msg)
        str.Add(MyColor)
        _BW.ReportProgress(4, str)
    End Sub

    Private Sub InitialCurrentProjectSetting(ByRef _CPS As CloudProjectsettings)
        CPS = _CPS
    End Sub

    Public Sub CleanTransaltion() Implements iXliff.CleanTransaltion
        Throw New NotImplementedException()
    End Sub

    Dim Msg As String
    Public Sub CreateOutFile() Implements iXliff.CreateOutFile

        Try
            'Multilingual file
            If File.Exists(Path.GetDirectoryName(Replace(_sfile, "01-Input-B", "05-Output")) & "\" & Path.GetFileName(_sfile)) Then
                'we then use the existing file to add one language
                UpdateMsg(Now & Chr(9) & "Multilingual MsgKey. File has already been integrated. Adding language. pleast wait..." & vbCrLf, Form_MainNew.RtbColor.Black)
                Msg = Mod_Cube1.ToActualFile(CPS.InputFile, CPS.Xliff_FileInBackFromtranslation, CPS.OutFile, CPS.CurrentLang)
                If Msg = True Then
                    cnt_newintegrated = cnt_newintegrated + 1
                Else
                    UpdateMsg(Now & Chr(9) & Msg & vbCrLf, Form_MainNew.RtbColor.Black)
                End If

            Else
                'first language we integrate => we use the original file
                UpdateMsg(Now & Chr(9) & "Multilingual MsgKey file. Integrating first language. please wait..." & vbCrLf, Form_MainNew.RtbColor.Black)
                Msg = Mod_Cube1.ToActualFile(CPS.InputFile, CPS.Xliff_FileInBackFromtranslation, CPS.OutFile, CPS.CurrentLang)
                cnt_newintegrated = cnt_newintegrated + 1
            End If

            UpdateMsg(Now & Chr(9) & "Integration done for " & CPS.CurrentLang & vbCrLf, Form_MainNew.RtbColor.Black)

            File.Move(CPS.Xliff_FileInBackFromtranslation, CPS.Xliff_ProcessedFileInBackFromtranslation)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Public Sub ExtractXiffwithExistingTranslation() Implements iXliff.ExtractXiffwithExistingTranslation
        Throw New NotImplementedException()
    End Sub

    Public Sub ImportExistingTranslationToDB() Implements iXliff.ImportExistingTranslationToDB
        Throw New NotImplementedException()
    End Sub

    Private _sfile As String
    Public Sub StartProcessing(sFile As String, ByRef BW As BackgroundWorker) Implements iXliff.StartProcessing
        _BW = BW
        _sfile = sFile

        Try
            IP = New InitiateProcess(ActiveProject, BW, sFile, curlang)
            AddHandler IP.Extractxliff, AddressOf ExtractXliff
            AddHandler IP.CleanTransaltion, AddressOf CleanTransaltion
            AddHandler IP.CreateOutFile, AddressOf CreateOutFile
            AddHandler IP.InitializeCurrentProjectSetting, AddressOf InitialCurrentProjectSetting
            AddHandler IP.ImportExistingTranslationToDB, AddressOf ImportExistingTranslationToDB
            AddHandler IP.ExtractXliffWithExistingTranslation, AddressOf ExtractXiffwithExistingTranslation
            IP.StartProcessing()
        Catch ex As Exception
            BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, System.IO.Path.GetFileName(_sfile), ex.Message})
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Function ExtractXliff() As Boolean Implements iXliff.ExtractXliff

        Try
            Msg = Mod_Cube1.ToXliff(_sfile, CPS._PD.ProjectPath & CloudProjectsettings.Folder_TobeTransalted, CPS.CurrentLang)
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
End Class
