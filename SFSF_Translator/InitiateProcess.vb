Imports System.ComponentModel
Imports System.IO


''' <summary>
''' Extraction of xliff, Reintegration, Pretranslation, DB update.
''' </summary>
''' <remarks></remarks>
Public Class InitiateProcess

    Private _BW As BackgroundWorker

    Public Property cnt_newtrans As Integer

    Private Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As Form_MainNew.RtbColor)
        Dim str As New ArrayList
        str.Add(Msg)
        str.Add(MyColor)
        _BW.ReportProgress(4, str)
    End Sub

    Private _sFile As String

    Private CPS As CloudProjectsettings

    Private _ActiveProject As ProjectDetail

    Private _Curlang() As String

    Private WithEvents LL As New Cls_GetLangList

    Private WithEvents CT As New CleanTranslation

    Public Event InitializeCurrentProjectSetting(ByRef CPS As CloudProjectsettings)

    Public Event ImportExistingTranslationToDB()

    Public Event ExtractXliffWithExistingTranslation()

    Private Sub CT_UpdateMsg(Msg As String, RTBC As CleanTranslation.RtbColor) Handles CT.UpdateMsg, LL.UpdateMsg
        UpdateMsg(Msg, RTBC)
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

    Public Sub New(ByVal ActiveProject As ProjectDetail, ByRef BW As BackgroundWorker, ByVal sFile As String, ByVal curlang() As String)
        _BW = BW
        _sFile = sFile
        _ActiveProject = ActiveProject
        _Curlang = curlang
    End Sub

    Public Sub StartProcessing()

        Dim bCleaned As Boolean = False

        Try
            For f = 0 To UBound(_Curlang)

                CPS = New CloudProjectsettings(_ActiveProject, _sFile, _Curlang(f))

                RaiseEvent InitializeCurrentProjectSetting(CPS)

                If CPS._PD.bImportExistingtranslationsintoDB Then 'Extract xliff with translation to 09-ExitingFOlder and Update to DB
                    If Not System.IO.Directory.Exists(CPS._PD.ProjectPath & CloudProjectsettings.Folder_ExistingTranslation) Then
                        System.IO.Directory.CreateDirectory(CPS._PD.ProjectPath & CloudProjectsettings.Folder_ExistingTranslation)
                    End If
                    RaiseEvent ImportExistingTranslationToDB()
                ElseIf CPS._PD.bCreateXliffWithTranslation Then
                    If Not System.IO.Directory.Exists(CPS._PD.ProjectPath & CloudProjectsettings.Folder_ExistingTranslation) Then
                        System.IO.Directory.CreateDirectory(CPS._PD.ProjectPath & CloudProjectsettings.Folder_ExistingTranslation)
                    End If
                    RaiseEvent ExtractXliffWithExistingTranslation()
                Else 'Normal Cloud Process
                    If File.Exists(CPS.Xliff_FileInTobetransalted) Then
                        UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sFile) & " - File for translators in " & CPS.CurrentLang & " exists." & vbCrLf, Form_MainNew.RtbColor.Black)

                        'check now if the translation is already back.
                        If File.Exists(CPS.Xliff_FileInBackFromtranslation) Then

                            UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sFile) & " - Translation is back!" & vbCrLf, Form_MainNew.RtbColor.Black)

                            If CPS._PD.isDBupdateRequired Then
                                CU.UpdateDB(CPS.Xliff_FileInBackFromtranslation, System.IO.Path.GetFileNameWithoutExtension(_sFile), , , CPS._PD.isMaxLengthCheckRequired)
                            End If

                            ReIntegrateXliffAfterPretranslate()

                            RaiseEvent CreateOutFile()

                        ElseIf File.Exists(CPS.Xliff_ProcessedFileInBackFromtranslation) Then
                            UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sFile) & " - File in " & CPS.CurrentLang & " already integrated." & vbCrLf, Form_MainNew.RtbColor.Black)
                        Else
                            UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sFile) & " - File in " & CPS.CurrentLang & " not yet translated." & vbCrLf, Form_MainNew.RtbColor.Black)
                        End If

                    Else 'Extract xliff file.

                        If CPS._PD.isCleanRequired And bCleaned <> True Then
                            RaiseEvent CleanTransaltion()
                            bCleaned = True
                        End If

                        RaiseEvent Extractxliff()

                        If CPS._PD.isPretranslateEnabled Then
                            ReIntegrateXliffAfterPretranslate()
                        End If

                    End If
                End If
            Next
        Catch ex As Exception
            UpdateMsg(Now & Chr(9) & ex.Message & vbCrLf, Form_MainNew.RtbColor.Red)
            _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, System.IO.Path.GetFileName(_sFile), ex.Message})
        End Try
    End Sub

    Public Event Extractxliff()

    Public Event CreateOutFile()

    Public Event CleanTransaltion()

    Private PTD As PreTranslateFileDetials

    ''' <summary>
    ''' After Xliff file is created, Pretranslate from DB
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Pretranslate()
        Try
            PTD = New PreTranslateFileDetials(CPS.CurrentLang, CPS._PD.ProjectPath & CloudProjectsettings.Folder_Pretranslate, _sFile, CPS.Xliff_FileInTobetransalted, CPS.Xliff_FileInBackFromtranslation)
            If CPS._PD.isPretranslateEnabled Then
                Select Case PTD.isCheckPreTransltedFileAvailable
                    Case PreTranslateFileDetials.PreTranslateDetails.NotAvailable
                        CU.PreTranslate1(PTD, CPS._PD.isCustomerCheckRequired, CPS._PD.isInstanceCheckRequired, CPS._PD.isMaxLengthCheckRequired)
                    Case PreTranslateFileDetials.PreTranslateDetails.Processed
                        UpdateMsg(Now & Chr(9) & Path.GetFileName(PTD.Pretranslatexliffpath) & " - File in " & CPS.CurrentLang & " already exists." & vbCrLf, Form_MainNew.RtbColor.Black)
                    Case Else
                        UpdateMsg(Now & Chr(9) & Path.GetFileName(PTD.Pretranslatexliffpath) & " - Pretranslated File in " & CPS.CurrentLang & " already exists." & vbCrLf, Form_MainNew.RtbColor.Black)
                End Select
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub


    ''' <summary>
    ''' If Pretranslated file and backfromtranslation file is available then Combine both files.
    ''' </summary>
    ''' <remarks></remarks>
    Public Function ReIntegrateXliffAfterPretranslate() As Boolean
        Try
            PTD = New PreTranslateFileDetials(CPS.CurrentLang, CPS._PD.ProjectPath & CloudProjectsettings.Folder_Pretranslate, _sFile, CPS.Xliff_FileInTobetransalted, CPS.Xliff_FileInBackFromtranslation)
            Select Case PTD.isCheckPreTransltedFileAvailable
                Case PreTranslateFileDetials.PreTranslateDetails.Available
                    If System.IO.File.Exists(PTD.xliff_BackFromtransaltion) Then
                        UpdateMsg(Now & Chr(9) & "Reintegrating Pretranslate and Backfromtranslation file completed." & vbCrLf, Form_MainNew.RtbColor.Black)
                        CU.ReIntegrate(PTD, CPS._PD.isMaxLengthCheckRequired)
                        Return True
                    End If
            End Select
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return False
    End Function

End Class
