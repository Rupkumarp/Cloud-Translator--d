Imports System.IO
Imports System.ComponentModel
Imports CloudTranslator

'Document.xml

Public Class LumiraHeader

    Implements iXliff

    Public Property ActiveProject As ProjectDetail Implements iXliff.ActiveProject

    Public Property cnt_newintegrated As Integer Implements iXliff.cnt_newintegrated

    Public Property cnt_newtrans As Integer Implements iXliff.cnt_newtrans

    Public Property CPS As CloudProjectsettings Implements iXliff.CPS

    Public Property curlang As String() Implements iXliff.curlang

    Public Property IP As InitiateProcess Implements iXliff.IP

    Public Property tr_type As TranslationType Implements iXliff.tr_type

    Private _BW As BackgroundWorker

    Private _sFile As String

    Public Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As Form_MainNew.RtbColor)
        Dim str As New ArrayList
        str.Add(Msg)
        str.Add(MyColor)
        _BW.ReportProgress(4, str)
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

    Private Msg As String = ""

    Public Function ExtractXliff() As Boolean Implements iXliff.ExtractXliff
        'Mod
        Try
            If Lums_HeaderProcess.LumiraHeaderExtractXliff(_sFile, CPS.Xliff_FileInTobetransalted, CPS.CurrentLang) <> "" Then
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

    Public Sub StartProcessing(sFile As String, ByRef BW As System.ComponentModel.BackgroundWorker) Implements iXliff.StartProcessing
        _BW = BW
        _sFile = sFile

        Try
            IP = New InitiateProcess(ActiveProject, BW, sFile, curlang)
            AddHandler IP.Extractxliff, AddressOf Extractxliff_file
            AddHandler IP.CreateOutFile, AddressOf CreateOutFile
            AddHandler IP.InitializeCurrentProjectSetting, AddressOf InitialCurrentProjectSetting
            AddHandler IP.ImportExistingTranslationToDB, AddressOf ImportExistingTranslationToDB
            AddHandler IP.ExtractXliffWithExistingTranslation, AddressOf ExtractXiffwithExistingTranslation
            IP.StartProcessing()
        Catch ex As Exception
            BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, System.IO.Path.GetFileName(_sFile), ex.Message})
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub InitialCurrentProjectSetting(ByRef _CPS As CloudProjectsettings)
        CPS = _CPS
    End Sub

    Public Sub ImportExistingTranslationToDB() Implements iXliff.ImportExistingTranslationToDB
        'not implemented
    End Sub

    Public Sub CleanTransaltion() Implements iXliff.CleanTransaltion
        'not implemented
    End Sub

    Public Sub CreateOutFile() Implements iXliff.CreateOutFile
        Try
            Lums_HeaderProcess.LumiraHeaderReintegrate(CPS.Xliff_FileInBackFromtranslation, _sFile, CPS.OutFile, CPS.CurrentLang)

            UpdateMsg(Now & Chr(9) & "Integration done for " & CPS.CurrentLang & vbCrLf, Form_MainNew.RtbColor.Black)

            cnt_newintegrated = cnt_newintegrated + 1

            If System.IO.File.Exists(CPS.Xliff_ProcessedFileInBackFromtranslation) Then
                System.IO.File.Delete(CPS.Xliff_ProcessedFileInBackFromtranslation)
            End If

            File.Move(CPS.Xliff_FileInBackFromtranslation, CPS.Xliff_ProcessedFileInBackFromtranslation)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Sub ExtractXiffwithExistingTranslation() Implements iXliff.ExtractXiffwithExistingTranslation
        'not implemented
    End Sub
End Class
