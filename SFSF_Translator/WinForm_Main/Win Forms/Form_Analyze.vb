
Imports System.ComponentModel
Imports System.Text.RegularExpressions

Public Class Form_Analyze

    Dim objStats As List(Of AnalyzeStats)
    Dim objListStats As New List(Of AnalyzeStats)

    Private WithEvents objAnalyzeObject As AnalyzeObject

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objListStats = New List(Of AnalyzeStats)
    End Sub

    Private bInstance As Boolean, bCustomer As Boolean, bResName As Boolean, bFileId As Boolean, bTxtReport As Boolean, bErrMsg As Boolean
    Private errFile As String

    Sub GetReport(ByVal xliffFile As String)
        If Not objAnalyzeObject.CheckSapConnection() Then
            Throw New Exception("Cannot connect to DB!" & Environment.NewLine & "Please make sure you are connected to SAP CORPORATE connection.")
            BW.ReportProgress(2, "Status: Cannot connect to DB!")
            Exit Sub
        End If
        Dim PD As ProjectDetail = ProjectManagement.GetActiveProject
        Dim objStats As AnalyzeStats = objAnalyzeObject.GenerateReport(xliffFile, PD.CustomerName, PD.InstaneName, bCustomer, bInstance, bFileId, bResName)
        objListStats.Add(objStats)
    End Sub

    Private Sub BtnBrowse_Click(sender As Object, e As EventArgs) Handles BtnBrowse.Click
        If RBSingleFile.Checked Then
            Dim fDialog As New OpenFileDialog
            fDialog.Filter = "Xliff File *.xliff|*.xliff"
            If fDialog.ShowDialog = DialogResult.OK Then
                TxtInputFile.Text = fDialog.FileName
            End If
        Else
            Dim fDialog As New FolderBrowserDialog
            fDialog.Description = "Select xliff Folder"
            If fDialog.ShowDialog = DialogResult.OK Then
                TxtInputFile.Text = fDialog.SelectedPath
            End If
        End If
    End Sub

    Private Sub BtnAnalyze_Click(sender As Object, e As EventArgs) Handles BtnAnalyze.Click
        'validate input
        Try
            bErrMsg = False
            BtnCancel.Enabled = True
            If RBSingleFile.Checked Then
                If Not System.IO.File.Exists(TxtInputFile.Text) Then
                    Throw New Exception("File not found!" & Environment.NewLine & "Please check the input file name.")
                End If
                If Not System.IO.Path.GetExtension(TxtInputFile.Text).ToLower.Trim = ".xliff" Then
                    Throw New Exception("Please input xliff file only!")
                End If
            Else
                If Not System.IO.Directory.Exists(TxtInputFile.Text) Then
                    Throw New Exception("Directory not found!" & Environment.NewLine & "Please check the path.")
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            Exit Sub
        End Try
        objListStats = New List(Of AnalyzeStats)
        objAnalyzeObject = New AnalyzeObject
        objAnalyzeObject.CancelReportCreation = False
        If Not BW.IsBusy Then
            BW.RunWorkerAsync()
        End If
        EnableControls(False)
    End Sub

    Private Sub UpdateStatus(ByVal msg As String)
        ToolStripStatusLabel1.Text = msg
    End Sub

    Private Sub UpdateProgress(ByVal progress As Integer)
        ToolStripProgressBar1.Value = progress
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        If Not IsNothing(objAnalyzeObject) Then
            objAnalyzeObject.CancelReportCreation = True
        End If
        If BW.IsBusy Then
            BW.CancelAsync()
        End If
    End Sub

    Private Sub BW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BW.DoWork
        If RBSingleFile.Checked Then
            BW.ReportProgress(2, "Status: Analyzing File 1/1")
            GetReport(TxtInputFile.Text)
        Else
            Dim filecount As Integer = System.IO.Directory.GetFiles(TxtInputFile.Text, "*.xliff").Count
            Dim filecounter As Integer = 1
            For Each f In My.Computer.FileSystem.GetFiles(TxtInputFile.Text, FileIO.SearchOption.SearchTopLevelOnly)
                If BW.CancellationPending Then
                    e.Cancel = True
                    BW.ReportProgress(2, "Status: Report creation was cancelled by User!")
                    Exit For
                End If
                BW.ReportProgress(2, "Status: Analyzing File " & filecounter & "\" & filecount)
                GetReport(f)
                filecounter += 1
            Next
        End If
        If Not bTxtReport Then
            Dim objexcel As New ClsExcel
            BW.ReportProgress(2, "Status: Preparing Excel Report...")
            BW.ReportProgress(3)
            objexcel.CreateAnalyzeReport(objListStats, bTxtReport)
            BW.ReportProgress(2, "Status: Report Created.")
        Else
            BW.ReportProgress(2, "Status: Preparing Txt Report...")
            BW.ReportProgress(3)
            writeToTextFormat(objListStats, saveReportFilePath)
            BW.ReportProgress(2, "Status: Report Created.")
        End If

    End Sub

    Private Sub BW_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BW.ProgressChanged
        If e.ProgressPercentage = 1 Then
            UpdateProgress(e.UserState)
        ElseIf e.ProgressPercentage = 2 Then
            UpdateStatus(e.UserState)
        ElseIf e.ProgressPercentage = 3 Then
            BtnCancel.Enabled = False
        ElseIf e.ProgressPercentage = 4 Then
            BtnCancel.Enabled = True
        End If
    End Sub

    Private Sub BW_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BW.RunWorkerCompleted
        If Not e.Error Is Nothing Then
            UpdateStatus(e.Error.Message.ToString)
            MsgBox(e.Error.Message.ToString, MsgBoxStyle.Critical, "Error")
            Me.Visible = True
        End If

        If bErrMsg Then
            MsgBox("Errors found, please check the error log!" & Environment.NewLine & "Send to Developer for fixing it.", MsgBoxStyle.Critical, "Analyze Error")
            System.Diagnostics.Process.Start(errFile)
        End If

        EnableControls(True)
    End Sub

    Private Sub ChkCustomer_CheckedChanged(sender As Object, e As EventArgs) Handles ChkCustomer.CheckedChanged
        If ChkCustomer.Checked Then
            bCustomer = True
        Else
            bCustomer = False
        End If
    End Sub

    Private Sub ChkInstance_CheckedChanged(sender As Object, e As EventArgs) Handles ChkInstance.CheckedChanged
        If ChkInstance.Checked Then
            bInstance = True
        Else
            bInstance = False
        End If
    End Sub

    Private Sub ChkFileId_CheckedChanged(sender As Object, e As EventArgs) Handles ChkFileId.CheckedChanged
        If ChkFileId.Checked Then
            bFileId = True
        Else
            bFileId = False
        End If
    End Sub

    Private Sub ChkResName_CheckedChanged(sender As Object, e As EventArgs) Handles ChkResName.CheckedChanged
        If ChkResName.Checked Then
            bResName = True
        Else
            bResName = False
        End If
    End Sub

    Dim saveReportFilePath As String
    Private Sub chkTxtReport_CheckedChanged(sender As Object, e As EventArgs) Handles chkTxtReport.CheckedChanged
        If chkTxtReport.Checked Then
            Dim fileReportPath As New SaveFileDialog
            fileReportPath.Title = "Select Report text file"
            fileReportPath.Filter = "txt *.txt|*.txt"
            If fileReportPath.ShowDialog = DialogResult.OK Then
                saveReportFilePath = fileReportPath.FileName
            End If
            bTxtReport = True
        Else
            bTxtReport = False
        End If
    End Sub

    Private Sub objAnalyzeObject_Progress(progress As Integer) Handles objAnalyzeObject.Progress
        BW.ReportProgress(1, progress)
    End Sub

    Private Sub EnableControls(ByVal enable As Boolean)
        GroupBox1.Enabled = enable
        GroupBox2.Enabled = enable
        BtnAnalyze.Enabled = enable
    End Sub

    Sub writeToTextFormat(ByVal objAnalyzeStats As List(Of AnalyzeStats), path As String)
        Dim rowCount As Integer = objAnalyzeStats.Count - 1
        Dim str As String
        Using writer As IO.StreamWriter = New IO.StreamWriter(path)
            For k = 0 To rowCount
                For i = 0 To objAnalyzeStats(k).TT.Count - 1
                    str = objAnalyzeStats(k).objectType(i).ToString & "||" & objAnalyzeStats(k).TT(i).ToString
                    str = str & "||" & objAnalyzeStats(k).LocalSource(i).ToString & "||" & objAnalyzeStats(k).LocalTranslation(i).ToString
                    str = str & "||" & objAnalyzeStats(k).Lang(i).ToString & "||" & objAnalyzeStats(k).DBSource(i).ToString
                    str = str & "||" & objAnalyzeStats(k).DBTranslation(i).ToString
                    str = str & "||" & objAnalyzeStats(k).Customer(i).ToString & "||" & objAnalyzeStats(k).Instance(i).ToString
                    writer.WriteLine(str)
                Next
            Next
        End Using
    End Sub

    Private Sub objAnalyzeObject_MsgErrorThrow(errFilea As String) Handles objAnalyzeObject.MsgErrorThrow
        errFile = errFilea
        bErrMsg = True
    End Sub
End Class

''' <summary>
''' Add (if not already existing) an analyze only function in cloud translator. 
''' The idea is to create a list for each object, of entries which: 
''' a. Are not translated in the DB = new translation 
''' b. Are changed vs the DB = the translation for the exact same term in the same object The output should be a report importable in xls with all terms for (a) & (b), 
''' and some statistics per language and per object for (a) and (b)
''' </summary>
Public Class AnalyzeObject

    Private objType As ObjectType = New ObjectType
    Public CancelReportCreation As Boolean = False
    Public ErrMsg As String
    Public Function GenerateReport(xliff_file As String, CustName As String, OInstance As String, chkCustomer As Boolean, chkInstance As Boolean, chkFileId As Boolean, chkResName As Boolean) As AnalyzeStats
        Dim stats As New AnalyzeStats
        ErrMsg = ""
        Try
            Dim xliffData As New sXliff
            xliffData = load_xliff(xliff_file)
            Dim CT As Cloud_TR.CloudTr
            Dim objCT As Cloud_TR.Service1
            Dim CTR As Cloud_TR.CloudTr = Nothing
            Dim xliffCount As Integer = xliffData.Source.Count
            Dim counter As Integer = 0
            For i As Integer = 0 To xliffCount - 1
                If CancelReportCreation Then
                    Throw New Exception("Report creation was cancelled by User!")
                End If
                If xliffData.Translation(i).ToString.Trim <> String.Empty And Not IsNumeric(xliffData.Translation(i)) Then
                    CT = New Cloud_TR.CloudTr
                    With CT
                        If CustName = "TestCustomer" Then
                            .Customer = ProjectManagement.GetActiveProject.CustomerName ' GetCustomerName() ' CustomerName
                        Else
                            .Customer = CustName
                        End If
                        .CustomerSpecific = 0
                        .Datatype = "'" & System.IO.Path.GetFileNameWithoutExtension(xliff_file) & "'" & ""
                        If OInstance = "TestInstance" Then
                            .Instance = ProjectManagement.GetActiveProject.InstaneName ' GetInstance() 'Instance
                        Else
                            .Instance = OInstance
                        End If
                        .Maxlength = 0
                        If xliffData.Resname(i).ToString.Trim = String.Empty Then
                            .Resname = "Test"
                        Else
                            .Resname = xliffData.Resname(i)
                        End If
                        .Source = xliffData.Source(i)
                        .Target = xliffData.Translation(i)
                        .SourceLang = "enUS"
                        .TargetLang = xliffData.TargetLang.Replace("-", "")
                        .Maxlength = 0
                    End With

                    With stats
                        .Lang.Add(xliffData.TargetLang.Replace("-", ""))
                        .objectType.Add(System.IO.Path.GetFileNameWithoutExtension(xliff_file) & "")
                        .LocalSource.Add(RemoveNewLine(xliffData.Source(i)))
                        .LocalTranslation.Add(RemoveNewLine(xliffData.Translation(i)))
                    End With

                    Dim parentID As String = ObjectId.GetParentID(System.IO.Path.GetFileName(xliff_file), False) & ""
                    objCT = New Cloud_TR.Service1

                    Try
                        CTR = objCT.GetAnalyzedRecord(CT, chkFileId, chkResName, chkCustomer, chkInstance, parentID)
                        If Not IsNothing(CTR) Then
                            stats.DBSource.Add(RemoveNewLine(CTR.Source))
                            stats.DBTranslation.Add(RemoveNewLine(CTR.Target))
                            stats.Instance.Add(CTR.Instance)
                            stats.Customer.Add(CTR.Customer)
                            stats.TT.Add(getTranslationType(CTR.Source, xliffData.Source(i), CTR.Target, xliffData.Translation(i)))
                            counter += 1
                        Else
                            stats.DBSource.Add("")
                            stats.DBTranslation.Add("")
                            stats.Instance.Add("")
                            stats.Customer.Add("")
                            stats.TT.Add(AnalyzeStats.TranslationType.NewTranslation)
                        End If
                        RaiseEvent Progress(((i + 1) / xliffCount) * 100)
                    Catch ex As Exception
                        ErrMsg = ErrMsg & Environment.NewLine & ex.Message & Environment.NewLine & CT.Datatype & "---" & CT.Source
                    End Try

                End If
            Next

            If ErrMsg <> "" Then
                WriteErrMsg(ErrMsg)
            End If

            WriteReport(xliff_file, stats)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return stats
    End Function

    Dim ErrFile As String = Nothing
    Private Sub WriteErrMsg(ByVal err As String)
        ErrFile = ModHelper.appData & "\Analyze" & Now.Date.Day & "-" & Now.Date.Month & "-" & Now.Date.Year & ".txt"
        Using writer As IO.StreamWriter = New IO.StreamWriter(ErrFile, True, System.Text.Encoding.UTF8)
            writer.Write(err)
        End Using
        RaiseEvent MsgErrorThrow(ErrFile)
    End Sub

    Private Sub WriteReport(ByVal xliffFile As String, ByVal stats As AnalyzeStats)
        Dim str As String = ""
        Dim sReportFile As String = System.IO.Path.GetFileName(xliffFile)
        sReportFile = Replace(xliffFile, sReportFile, sReportFile & "_Report.txt")
        Using writer As IO.StreamWriter = New IO.StreamWriter(sReportFile, True, System.Text.Encoding.UTF8)
            For i As Integer = 0 To stats.LocalSource.Count - 1
                str = stats.objectType(i).ToString & "||" & stats.TT(i).ToString
                str = str & "||" & stats.LocalSource(i).ToString & "||" & stats.LocalTranslation(i).ToString
                str = str & "||" & stats.Lang(i).ToString & "||" & stats.DBSource(i).ToString & "||" & stats.DBTranslation(i).ToString
                str = str & "||" & stats.Customer(i).ToString & "||" & stats.Instance(i).ToString
                writer.WriteLine(str)
            Next
        End Using
    End Sub

    Public Event Progress(ByVal progress As Integer)

    Public Event MsgErrorThrow(ByVal errFilea As String)

    Public Function getTranslationType(ByVal DBSource As String, ByVal LocalSource As String, ByVal DBTranslation As String, ByVal LocalTranslation As String) As AnalyzeStats.TranslationType
        Dim objDBSource As String = RemoveNewLineAndSpace(DBSource)
        Dim objDBTranslation As String = RemoveNewLineAndSpace(DBTranslation)

        Dim objLocalSource As String = RemoveNewLineAndSpace(LocalSource)
        Dim objLocalTranslation As String = RemoveNewLineAndSpace(LocalTranslation)

        If (objDBSource.ToLower.Trim = objLocalSource.ToLower.Trim) And (objDBTranslation.ToLower.Trim = objLocalTranslation.ToLower.Trim) Then
            Return AnalyzeStats.TranslationType.ExactTranslation
        ElseIf (objDBSource.ToLower.Trim = objLocalSource.ToLower.Trim) And (objDBTranslation.ToLower.Trim <> objLocalTranslation.ToLower.Trim) Then
            Return AnalyzeStats.TranslationType.WrongTranslation
        ElseIf (objDBSource.ToLower.Trim <> objLocalSource.ToLower.Trim) And (objDBTranslation.ToLower.Trim = objLocalTranslation.ToLower.Trim) Then
            Return AnalyzeStats.TranslationType.WrongTranslation
        Else ' (DBSource <> LocalSource) And (DBTranslation <> LocalTranslation) Then
            Return AnalyzeStats.TranslationType.NewTranslation
        End If
    End Function

    Function RemoveNewLine(ByVal source As String) As String
        Return Regex.Replace(source, "\t|\n|\r", "")
    End Function

    Function RemoveNewLineAndSpace(ByVal source As String) As String
        source = Regex.Replace(source, "\s+", "")
        Return Regex.Replace(source, "\t|\n|\r", "")
    End Function


    Public Function CheckSapConnection() As Boolean
        If Not CheckURL("http://10.66.9.51:8013/") Then
            Dim x As Integer = 1
            Do Until x = 4
                'RaiseEvent UpdateMsg(Now & Chr(9) & "Attempting to connect - " & x & vbCrLf, Form_MainNew.RtbColor.Black)
                If CheckURL("http://10.66.9.51:8013/") Then
                    Return True
                End If
                System.Threading.Thread.Sleep(1000)
                x += 1
            Loop
            'RaiseEvent UpdateMsg(Now & Chr(9) & "No Connection to SAP corporate... " & Msg & vbCrLf, Form_MainNew.RtbColor.Black)
            Return False
        End If
        Return True
    End Function

End Class

Public Class AnalyzeStats
    Public Enum TranslationType
        NewTranslation
        WrongTranslation
        ExactTranslation
    End Enum
    Public TT As New List(Of TranslationType)
    Public LocalSource As New List(Of String)
    Public LocalTranslation As New List(Of String)
    Public DBSource As New List(Of String)
    Public DBTranslation As New List(Of String)
    Public Lang As New List(Of String)
    Public objectType As New List(Of String)
    Public Customer As New List(Of String)
    Public Instance As New List(Of String)
End Class






