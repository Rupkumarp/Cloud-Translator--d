Imports System.IO
Imports System.ComponentModel
Imports CloudTranslator

Public Class LMS

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

    Public Sub StartProcessing(ByVal sFile As String, ByRef BW As BackgroundWorker) Implements iXliff.StartProcessing

        _BW = BW


        If ActiveProject.bImportExistingtranslationsintoDB Then
            ImportExistingTranslationToDB()
            Exit Sub
        End If

        For f = 0 To UBound(curlang)

            CPS = New CloudProjectsettings(ActiveProject, sFile, curlang(f))

            'Dim xliff_BackFromtranslation As String = Path.GetDirectoryName(Replace(sFile, "01-Input-B", "03-Backfromtranslation")) & "\" & Path.GetFileNameWithoutExtension(sFile) & "_" & curlang(f) & ".xliff"
            'Dim xliff_TobeTranslated As String = Path.GetDirectoryName(Folder_TobeTranslated) & "\" & Path.GetFileNameWithoutExtension(sFile) & "_" & curlang(f) & ".xliff"

            'PTD = New PreTranslateFileDetials(curlang(f), System.IO.Path.GetDirectoryName(Folder_Pretranslate), sFile, xliff_TobeTranslated, xliff_BackFromtranslation)

            Dim sCompareFile As String = ""

            Try
                sCompareFile = GetCompareFile(curlang(f)) 'Check if the lang comparer file available
            Catch ex As Exception
                BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, curlang(f), "Error while finding Comparer file - " & CPS._PD.ProjectPath & CloudProjectsettings.Folder_Compare})
                UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(sFile) & " - Error while finding Comparer file - " & CPS._PD.ProjectPath & CloudProjectsettings.Folder_Compare & " - " & ex.Message & vbCrLf, Form_MainNew.RtbColor.Red)
            End Try

            Setup_Datatables(sFile, sCompareFile)

            Dim AllText As String = System.IO.File.ReadAllText(appData & DefinitionFiles.Lms_List)

            Dim NewText As String() = Split(AllText, vbNewLine)

            For i As Integer = 1 To UBound(NewText)
                If NewText(i) = "######################################" Then
                    For j As Integer = i + 1 To UBound(NewText)
                        If NewText(j) = "######################################" Then
                            ProcessLMS(NewText, i + 1, j - 1, CPS._PD.ProjectPath & CloudProjectsettings.Folder_TobeTransalted, curlang(f), sCompareFile)
                            i = j - 1
                            Exit For
                        End If
                    Next
                End If
            Next

        Next

    End Sub

    Private PTD As PreTranslateFileDetials
    Private WithEvents CU As New Cls_CloudJob

    Private _sFile As String

    Dim xliff_BackFromtranslation As String
    Dim xliff_TobeTranslated As String

    Private Sub ProcessLMS(ByVal objText As String(), ByVal iStart As Integer, ByVal iEnd As Integer, ByVal xliff_Path As String, ByVal Targetlanguage As String, ByVal sCompareFile As String)

        Dim sFile As String = objText(iStart)
        sFile = Mid(sFile, InStr(sFile, "(") + 1, Len(sFile))
        sFile = sFile.Substring(0, sFile.Length - 1) & ".xliff"
        sFile = CPS._PD.ProjectPath & CloudProjectsettings.Folder_InputB & sFile

        Try
            _sFile = sFile

            xliff_BackFromtranslation = CPS.Xliff_FileInBackFromtranslation ' Path.GetDirectoryName(Replace(sFile, "01-Input-B", "03-Backfromtranslation")) & "\" & Path.GetFileNameWithoutExtension(sFile) & "_" & Targetlanguage & ".xliff"
            xliff_TobeTranslated = CPS.Xliff_FileInTobetransalted ' Path.GetDirectoryName(Folder_TobeTranslated) & "\" & Path.GetFileNameWithoutExtension(sFile) & "_" & Targetlanguage & ".xliff"

            UpdateMsg(Now & Chr(9) & "File is of type LMS" & vbCrLf, Form_MainNew.RtbColor.Black)

            If File.Exists(CPS.Xliff_FileInTobetransalted) Then
                UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(sFile) & " - File for translators in " & Targetlanguage & " exists." & vbCrLf, Form_MainNew.RtbColor.Black)

                'check now if the translation is already back.
                If File.Exists(CPS.Xliff_FileInBackFromtranslation) Then

                    'Update Cloud_TR
                    If ActiveProject.isDBupdateRequired Then
                        CU.UpdateDB(CPS.Xliff_FileInBackFromtranslation, System.IO.Path.GetFileNameWithoutExtension(sFile))
                    End If

                    ReIntegrateXliffAfterPretranslate()

                    CreateOutFile()

                ElseIf File.Exists(CPS.Xliff_ProcessedFileInBackFromtranslation) Then
                    UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(sFile) & " - File in " & Targetlanguage & " already integrated." & vbCrLf, Form_MainNew.RtbColor.Black)
                Else
                    UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(sFile) & " - File in " & Targetlanguage & " not yet translated." & vbCrLf, Form_MainNew.RtbColor.Black)
                End If

            Else

                If sCompareFile <> "" Then
                    Dim filecount As Integer = Mod_LMS.GenerateXliff(objText, iStart, iEnd, xliff_Path, Targetlanguage, ActiveProject.isMaxLengthCheckRequired)
                    If filecount >= 1 Then
                        UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(sFile) & ".xliff - File in " & Targetlanguage & " generated for translators." & vbCrLf, Form_MainNew.RtbColor.Black)
                        cnt_newtrans = cnt_newtrans + 1
                        Pretranslate()
                        If ReIntegrateXliffAfterPretranslate() Then
                            cnt_newtrans -= 1
                            CreateOutFile()
                        End If
                    Else
                        UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(sFile) & " - already has translation or no data found for " & Targetlanguage & vbCrLf, Form_MainNew.RtbColor.Green)
                        _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, Path.GetFileNameWithoutExtension(sFile), " - Already has translation or no data found " & Targetlanguage})
                    End If
                Else
                    UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(sFile) & " - No .lms Comparing file found for " & Targetlanguage & " in 06-Compare." & vbCrLf, Form_MainNew.RtbColor.Red)
                    _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, Path.GetFileNameWithoutExtension(sFile), " - No .lms Comparing file found for " & Targetlanguage & " in " & CPS._PD.ProjectPath & CloudProjectsettings.Folder_Compare})
                End If
            End If

        Catch ex As Exception
            _BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, System.IO.Path.GetFileName(sFile), ex.Message})
            Throw New Exception(ex.Message)
        End Try
    End Sub



    Private Function GetCompareFile(ByVal lang As String) As String
        Try
            For Each f In My.Computer.FileSystem.GetFiles(CPS._PD.ProjectPath & CloudProjectsettings.Folder_Compare, FileIO.SearchOption.SearchTopLevelOnly)
                Dim fExt As String = System.IO.Path.GetFileNameWithoutExtension(f)
                If Microsoft.VisualBasic.Right(fExt, 5).ToString.ToLower = lang.ToLower Then
                    Return f
                End If
            Next
            Return ""
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function


    Public Sub CleanTransaltion() Implements iXliff.CleanTransaltion

    End Sub

    Private Sub CU_Progress(Max As Integer, val As Integer) Handles CU.Progress
        Dim str As New ArrayList
        str.Add(Max)
        str.Add(val)
        _BW.ReportProgress(3, str)
        'Form_Main.UpdateProgressBar(Max, val)
    End Sub

    Private Sub CU_UpdateMsg(Msg As String, RTBC As Cls_CloudJob.RtbColor) Handles CU.UpdateMsg
        UpdateMsg(Msg, RTBC)
    End Sub


    Private Sub CU_UpdateToolstripMsg(Msg As String) Handles CU.UpdateToolstripMsg
        _BW.ReportProgress(0, Msg)
    End Sub


    Public Sub CreateOutFile() Implements iXliff.CreateOutFile
        Try
            UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sFile) & " - Translation is back!" & vbCrLf, Form_MainNew.RtbColor.Black)
            Xliff_To_LMS(Path.GetDirectoryName(Replace(_sFile, "01-Input-B", "03-Backfromtranslation")) & "\" & Path.GetFileNameWithoutExtension(_sFile) & "_" & CPS.CurrentLang & ".xliff", CPS.CurrentLang)

            cnt_newintegrated = cnt_newintegrated + 1
            File.Move(Path.GetDirectoryName(Replace(_sFile, "01-Input-B", "03-Backfromtranslation")) & "\" & Path.GetFileNameWithoutExtension(_sFile) & "_" & CPS.CurrentLang & ".xliff", Path.GetDirectoryName(Replace(_sFile, "01-Input-B", "03-Backfromtranslation")) & "\(processed)" & Path.GetFileNameWithoutExtension(_sFile) & "_" & CPS.CurrentLang & ".xliff")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Function ExtractXliff() As Boolean Implements iXliff.ExtractXliff
        Return True
    End Function

    Public Sub Pretranslate()
        Try
            If ActiveProject.isPretranslateEnabled Then
                Select Case PTD.isCheckPreTransltedFileAvailable
                    Case PreTranslateFileDetials.PreTranslateDetails.NotAvailable
                        CU.PreTranslate1(PTD, ActiveProject.isCustomerCheckRequired, ActiveProject.isInstanceCheckRequired)
                    Case PreTranslateFileDetials.PreTranslateDetails.Processed
                        UpdateMsg(Now & Chr(9) & Path.GetFileName(PTD.Pretranslatexliffpath) & " - Pretranslated File in " & CPS.CurrentLang & " already processed." & vbCrLf, Form_MainNew.RtbColor.Black)
                    Case Else
                        UpdateMsg(Now & Chr(9) & Path.GetFileName(PTD.Pretranslatexliffpath) & " - Pretranslated File in " & CPS.CurrentLang & " already exists." & vbCrLf, Form_MainNew.RtbColor.Black)
                End Select
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Function ReIntegrateXliffAfterPretranslate() As Boolean
        Try
            PTD = New PreTranslateFileDetials(CPS.CurrentLang, CPS._PD.ProjectPath & CloudProjectsettings.Folder_Pretranslate, _sFile, xliff_TobeTranslated, xliff_BackFromtranslation)
            Select Case PTD.isCheckPreTransltedFileAvailable
                Case PreTranslateFileDetials.PreTranslateDetails.Available
                    If System.IO.File.Exists(PTD.xliff_BackFromtransaltion) Then
                        UpdateMsg(Now & Chr(9) & "Reintegrating Pretranslate and Backfromtranslation file completed." & vbCrLf, Form_MainNew.RtbColor.Black)
                        CU.ReIntegrate(PTD)
                        Return True
                    End If
            End Select
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return False
    End Function



    Public Sub ImportExistingTranslationToDB() Implements iXliff.ImportExistingTranslationToDB
        UpdateMsg(Now & Chr(9) & "DB update not implemented for LMS file" & vbCrLf, Form_MainNew.RtbColor.Red)
    End Sub

    Public Sub ExtractXiffwithExistingTranslation() Implements iXliff.ExtractXiffwithExistingTranslation
        UpdateMsg(Now & Chr(9) & "ExtractXiffwithExistingTranslation not implemented for LMS file" & vbCrLf, Form_MainNew.RtbColor.Red)
    End Sub
End Class
