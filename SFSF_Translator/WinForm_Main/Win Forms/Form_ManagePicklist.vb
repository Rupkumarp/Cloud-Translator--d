Imports System.Data.OleDb

'New tool: “Manage picklists”
'This opens a window with 2 files selectors (textbox + button “…”): 
'-	the master file (as a standard in xlsx format!!! but same as the file you have already seen) which contains all standard translations in all standard languages
'-	the custom file with only the activated languages, but which contains also additional non standard rows . it’s a csv file (format you already handled)

'2 buttons: “import translation” and “cancel”. Cancel just closes the form.
'(note: when opening the file, there should be some logging in the richtextbox of the main form)

'Import translation purpose:
'-	simply copy the translations from the master file into the right column of the custom csv file.

'How to:
'You can implement as you want, but I would do the following, based on the existing processes:
'-	I would create an big xliff with all translations in the different languages 1:1. 
'-	Then I would go through the custom translation 1:1. For those with the same picklistID and same enUS, I would just add the translation in the corresponding column, if not already existing (do not overwrite!)
'-	Going through xliff is not mandatory, but is interesting for other reasons and for further applications.
'-	In the log, do not forget to put the number of imported entries. (e.g. frFR: 123 translation imported)
'-	Eventually to speed up the process, you might also display a combox with the languages which are available in the custom file. In that case, only those languages which are selected are processed. 

'Only 1 thing to note: the master file is delivered by product management as an xlsx. It’s a 1:1 import of csv. I have attached the recommendation on how to convert back into csv. I would assume, it’s better to manually convert to csv. Processing through xlsx would be much slower.



Public Class Form_ManagePicklist

    Public Delegate Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As Form_MainNew.RtbColor)
    Dim Progress As UpdateMsg
    Public Property CurrentDirectory As String
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        
        Progress = AddressOf Form_MainNew.UpdateMsg

        CheckBox1.Text = "Run without Master file" & vbCrLf & "Note: Transalted xliff files should be copied to working directory!"

    End Sub

    Private Sub BtnBrowseMasterFile_Click(sender As Object, e As EventArgs) Handles BtnBrowseMasterFile.Click
        Dim opnFDialog As New OpenFileDialog
        opnFDialog.Filter = "Master File *.xlsx,*.csv|*.xlsx;*.csv;"

        If System.IO.Directory.Exists(CurrentDirectory & CloudProjectsettings.Folder_PicklistsStandard) Then
            opnFDialog.InitialDirectory = CurrentDirectory & CloudProjectsettings.Folder_PicklistsStandard
        End If

        If opnFDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtMasterFile.Text = opnFDialog.FileName
        Else
            Exit Sub
        End If

        Try
            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor

            Dim dt As New DataTable()
            Dim langdict As New Dictionary(Of String, Integer)

            If System.IO.Directory.Exists(Application.StartupPath & "\Tools\Picklist\Xliff\") Then
                For Each f In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\Tools\Picklist\Xliff\", FileIO.SearchOption.SearchTopLevelOnly)
                    System.IO.File.Delete(f)
                Next
                System.IO.Directory.Delete(Application.StartupPath & "\Tools\Picklist\Xliff\")
            End If
            System.IO.Directory.CreateDirectory(Application.StartupPath & "\Tools\Picklist\Xliff\")

            System.Threading.Thread.Sleep(2000)

            If System.IO.Path.GetExtension(txtMasterFile.Text) <> ".csv" Then
                Dim conStr As String = ""
                Select Case System.IO.Path.GetExtension(txtMasterFile.Text)
                    Case ".xls"
                        'Excel 97-03
                        conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}; Extended Properties='Excel 8.0;HDR={1}'"
                        Exit Select
                    Case ".xlsx"
                        'Excel 07
                        conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties='Excel 8.0;HDR={1}'"
                        Exit Select
                End Select
                conStr = String.Format(conStr, txtMasterFile.Text, True)

                Dim connExcel As New OleDbConnection(conStr)
                Dim cmdExcel As New OleDbCommand()
                Dim oda As New OleDbDataAdapter()
                cmdExcel.Connection = connExcel

                'Get the name of First Sheet
                connExcel.Open()
                Dim dtExcelSchema As DataTable
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
                Dim SheetName As String = dtExcelSchema.Rows(0)("TABLE_NAME").ToString()
                connExcel.Close()

                'Read Data from First Sheet
                connExcel.Open()
                cmdExcel.CommandText = "SELECT * From [" & SheetName & "]"
                oda.SelectCommand = cmdExcel
                oda.Fill(dt)
                connExcel.Close()
            Else
                Dim objParser As New CsvParser
                dt = objParser.GetDataTabletFromCSVFile(txtMasterFile.Text)
            End If
            

            Dim counter As Integer = 0
            For i As Integer = 0 To DT.Columns.Count - 1
                Try
                    Dim lang As String = CsvLang(DT.Columns(i).ColumnName)
                    If lang <> "" And lang <> "en_US" Then
                        langdict.Add(lang, i)
                        Mod_PickList.CreatePickListXliff("", dt, CurrentDirectory & CloudProjectsettings.Folder_PicklistsExtractedXliff & System.IO.Path.GetFileNameWithoutExtension(txtMasterFile.Text) & "_" & lang & ".xliff", lang, i, PickListType.ManagePicklist, False, False)
                        Progress(Now & Chr(9) & System.IO.Path.GetFileNameWithoutExtension(txtMasterFile.Text) & "_" & lang & ".xliff - Picklist file Created" & vbCrLf, Form_MainNew.RtbColor.Black)
                        counter += 1
                        Application.DoEvents()
                    End If

                Catch ex As System.ArgumentException
                    'do nothing
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                End Try
            Next

            Progress(Now & Chr(9) & counter & " - Plist file created" & vbCrLf, Form_MainNew.RtbColor.Black)

            If counter > 0 Then
                If MsgBox(counter & " - Picklist file's have been created." & vbNewLine & "Do you want to open the folder?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "?") = MsgBoxResult.Yes Then
                    Diagnostics.Process.Start("explorer.exe", CurrentDirectory & CloudProjectsettings.Folder_PicklistsExtractedXliff)
                End If
            End If

        Catch ex As Exception
            ToolStripStatusLabel1.Text = "The process terminated unexpectedly"
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Errro!")
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Dim DT_CustomFile As DataTable
    Private Sub BtnBrowseCutomFile_Click(sender As Object, e As EventArgs) Handles BtnBrowseCutomFile.Click

        Dim opnFDialog As New OpenFileDialog
        opnFDialog.Filter = "Custom csv file *.csv|*.csv"

        If System.IO.Directory.Exists(CurrentDirectory & CloudProjectsettings.Folder_PicklistsInput) Then
            opnFDialog.InitialDirectory = CurrentDirectory & CloudProjectsettings.Folder_PicklistsInput
        End If

        If opnFDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtCustomFile.Text = opnFDialog.FileName
        Else
            Exit Sub
        End If

        Try 'Loading combobox with the languages which are available in the custom file.
            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor

            Dim objParser As New CsvParser
            DT_CustomFile = objParser.GetDataTabletFromCSVFile(txtCustomFile.Text)

            Dim langdict As New Dictionary(Of String, Integer)

            For i As Integer = 0 To DT_CustomFile.Columns.Count - 1
                Try
                    Dim lang As String = CsvLang(DT_CustomFile.Columns(i).ColumnName)
                    If lang <> "" Then
                        langdict.Add(lang, i)
                    End If

                Catch ex As System.ArgumentException
                    'do nothing
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                End Try
            Next

            Try
                lstLang.DataSource = Nothing
                lstLang.Items.Clear()
                lstLang.DataSource = New BindingSource(langdict, Nothing)
                lstLang.DisplayMember = "key"
                lstLang.ValueMember = "value"
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

            MsgBox("Select the languages that you want to translate!", MsgBoxStyle.Exclamation, "Select language")

        Catch ex As Exception
            ToolStripStatusLabel1.Text = "The process terminated unexpectedly"
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        Try
            Me.BtnCancel.Enabled = False

            If BWorker.IsBusy And BWorker.WorkerSupportsCancellation Then
                ToolStripStatusLabel1.Text = "Status: Please wait...cancelling"
                BWorker.CancelAsync()
            End If

            While BWorker.IsBusy
                Application.DoEvents()
            End While

            Me.BtnImportTranslation.Enabled = True
            Me.Cursor = Cursors.Default
            ToolStripStatusLabel1.Text = "Cancelled by User!"
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error Cancelling")
        Finally
            Me.BtnCancel.Enabled = True
        End Try
    End Sub

    Private Sub BtnImportTranslation_Click(sender As Object, e As EventArgs) Handles BtnImportTranslation.Click
        Try
            Me.BtnImportTranslation.Enabled = False

            If CheckBox1.Checked <> True Then
                If txtMasterFile.Text.ToString.Trim = "" Then
                    Throw New Exception("Master file not selected")
                ElseIf System.IO.File.Exists(txtMasterFile.Text) <> True Then
                    Throw New Exception("Master xlsx file not found:" & vbNewLine & txtMasterFile.Text)
                End If
            End If

            If txtCustomFile.Text.ToString.Trim = "" Then
                Throw New Exception("Custom csv file not selected")
            ElseIf System.IO.File.Exists(txtCustomFile.Text) <> True Then
                Throw New Exception("Custom csv file not found:" & vbNewLine & txtCustomFile.Text)
            End If

            If lstLang.Items.Count = 0 Then
                MsgBox("Language list is empty!", MsgBoxStyle.Information, "Manage Picklist!")
                Exit Sub
            End If

            'Translate
            Dim counter As Integer = 0

            ToolStripProgressBar1.Maximum = lstLang.SelectedItems.Count


            Dim langlist As New ArrayList

            For i As Integer = 0 To lstLang.SelectedItems.Count - 1
                Dim langl() As String = (Split(Replace(Replace(lstLang.SelectedItems(i).ToString, "[", ""), "]", ""), ","))
                langlist.Add(langl)
            Next

            If BWorker.IsBusy <> True Then
                BWorker.RunWorkerAsync(langlist)
            End If

            'For i As Integer = 0 To lstLang.SelectedItems.Count - 1
            '    Dim langl() As String = (Split(Replace(Replace(lstLang.SelectedItems(i).ToString, "[", ""), "]", ""), ","))
            '    Mod_PickList.ToCsv(txtCustomFile.Text, GetTranslatedFilePath(langl(0)), langl(0), PickListType.ManagePicklist)
            '    Progress(Now & Chr(9) & "Picklist csv file generated in " & lstLang.Text & vbCrLf)
            '    ToolStripProgressBar1.Value = i
            '    Application.DoEvents()
            'Next

        Catch ex As Exception
            ToolStripStatusLabel1.Text = "The process terminated unexpectedly"
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
            Me.BtnImportTranslation.Enabled = True
        Finally

        End Try
    End Sub

    Private Function GetTranslatedFilePath(ByVal Lang As String) As String
        Try
            For Each f In My.Computer.FileSystem.GetFiles(CurrentDirectory & CloudProjectsettings.Folder_PicklistsExtractedXliff, FileIO.SearchOption.SearchTopLevelOnly)
                If Lang.Trim.ToLower = Microsoft.VisualBasic.Right(System.IO.Path.GetFileNameWithoutExtension(f).ToString.Trim.ToLower, 5) Then
                    Return f
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Throw New Exception("No xliff files found for the selected language! " & vbCrLf & Application.StartupPath & "\Tools\Picklist\Xliff\")
    End Function

    Private Function CsvLang(ByVal str As String) As String
        Select Case Microsoft.VisualBasic.Right(LCase(str), 5)
            Case "bg_bg"
                Return "bg_BG"
            Case "bs_bs"
                Return "bs_BS"
            Case "bs_id"
                Return "bs_ID"
            Case "cs_cz"
                Return "cs_CZ"
            Case "cy_gb"
                Return "cy_GB"
            Case "da_dk"
                Return "ds_DK"
            Case "de_ch"
                Return "de_CH"
            Case "es_mx"
                Return "es_MX"
            Case "fi_fi"
                Return "fi_FI"
            Case "fr_ca"
                Return "fr_CA"
            Case "el_gr"
                Return "el_GR"
            Case "en_gb"
                Return "en_GB"
            Case "hi_in"
                Return "hi_IN"
            Case "hr_hr"
                Return "hr_HR"
            Case "hu_hu"
                Return "hu_HU"
            Case "iw_il"
                Return "iw_IL"
            Case "nb_no"
                Return "nb_NO"
            Case "nl_nl"
                Return "nl_NL"
            Case "pl_pl"
                Return "pl_PL"
            Case "pt_pt"
                Return "pt_PT"
            Case "ro_ro"
                Return "ro_RO"
            Case "sk_sk"
                Return "sk_SK"
            Case "sl_si"
                Return "sl_SI"
            Case "sr_rs"
                Return "sr_RS"
            Case "sv_se"
                Return "sv_SE"
            Case "th_th"
                Return "th_TH"
            Case "tr_tr"
                Return "tr_TR"
            Case "vi_vn"
                Return "vi_VN"
            Case "zh_tw"
                Return "zh_TW"
            Case "uk_ua"
                Return "uk_UA"
            Case "de_de", "de"
                Return "de_DE"
            Case "en", "us", "en_us"
                Return ""
            Case "es_es", "es"
                Return "es_ES"
            Case "fr", "fr_fr"
                Return "fr_FR"
            Case "it_it", "it"
                Return "it_IT"
            Case "ja_jp", "ja", "jp"
                Return "ja_JP"
            Case "ko_kr", "kr", "ko"
                Return "ko_KR"
            Case "pt_br", "pt", "br"
                Return "pt_BR"
            Case "ru_ru", "ru"
                Return "ru_RU"
            Case "zh_cn", "zh", "cn"
                Return "zh_CN"
            Case Else
                Return ""
        End Select
    End Function

    Dim txtCustom As String

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            BtnBrowseMasterFile.Enabled = False
        Else
            BtnBrowseMasterFile.Enabled = True
        End If
    End Sub

    Private Sub BtnWorkingDirectory_Click(sender As Object, e As EventArgs) Handles BtnWorkingDirectory.Click
        If Not System.IO.Directory.Exists(CurrentDirectory & CloudProjectsettings.Folder_Picklists) Then
            MsgBox("Directory not found!" & vbCrLf & CurrentDirectory & CloudProjectsettings.Folder_Picklists, MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        End If
        Diagnostics.Process.Start("explorer.exe", CurrentDirectory & CloudProjectsettings.Folder_Picklists)
    End Sub

    Dim bwLang As String
    Private Sub BWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BWorker.DoWork
        'Try
        Dim ddl = CType(e.Argument, ArrayList)
        For i As Integer = 0 To ddl.Count - 1
            bwLang = ddl(i)(0).ToString
            BWorker.ReportProgress(-1)

            Dim sTrFile As String = ""
            Try
                sTrFile = GetTranslatedFilePath(bwLang)
            Catch ex As Exception
                BWorker.ReportProgress(-999, ex.Message)
                e.Cancel = True
                Exit For
            End Try

            e.Result = Mod_PickList.ToCsv(txtCustom, sTrFile, bwLang, PickListType.ManagePicklist, BWorker)
            BWorker.ReportProgress(i + 1)
            If BWorker.CancellationPending Then
                e.Cancel = True
                Exit For
            End If
        Next
        ' Catch ex As Exception
        'Throw New Exception(ex.Message)
        ' End Try

    End Sub

    Private Sub BWorker_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BWorker.ProgressChanged

        If e.ProgressPercentage = -999 Then
            MsgBox(e.UserState, MsgBoxStyle.Critical, "Picklist Failed")
        ElseIf e.ProgressPercentage = -1 Then
            ToolStripStatusLabel1.Text = "Status: Processing - " & bwLang & ". Please wait.... "
            Me.Cursor = Cursors.WaitCursor
        Else
            ToolStripProgressBar1.Value = e.ProgressPercentage
            Progress(Now & Chr(9) & "Picklist csv file generated in " & bwLang & vbCrLf, Form_MainNew.RtbColor.Black)
        End If
    End Sub

    Private Sub BWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BWorker.RunWorkerCompleted
        If e.[Error] IsNot Nothing Then
            MessageBox.Show(e.[Error].Message, "Plist FAILED")
        End If

        If e.Cancelled Then
            Me.BtnImportTranslation.Enabled = True
            Me.Cursor = Cursors.Default
            Me.ToolStripStatusLabel1.Text = "Status: Cancelled by user.... "
        Else
            Me.BtnImportTranslation.Enabled = True
            Me.Cursor = Cursors.Default
            Me.ToolStripStatusLabel1.Text = "Status: Completed successfully "
        End If
    End Sub

    Private Sub txtCustomFile_TextChanged(sender As Object, e As EventArgs) Handles txtCustomFile.TextChanged
        txtCustom = txtCustomFile.Text
    End Sub

    Private Sub Form_ManagePicklist_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If BWorker.IsBusy Then
            MsgBox("Please wait while it finishes the process...", MsgBoxStyle.Exclamation, "Wait!")
            e.Cancel = True
        End If
    End Sub

    Private Sub BtnOutFolder_Click(sender As Object, e As EventArgs) Handles BtnOutFolder.Click
        If Not System.IO.Directory.Exists(CurrentDirectory & CloudProjectsettings.Folder_PicklistsOutput) Then
            MsgBox("Directory not found!" & vbCrLf & CurrentDirectory & CloudProjectsettings.Folder_PicklistsOutput, MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        End If
        Diagnostics.Process.Start("explorer.exe", CurrentDirectory & CloudProjectsettings.Folder_PicklistsOutput)
    End Sub

    Private Sub Form_ManagePicklist_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If Not (System.IO.Directory.Exists(CurrentDirectory & CloudProjectsettings.Folder_Picklists)) Or
             Not (System.IO.Directory.Exists(CurrentDirectory & CloudProjectsettings.Folder_PicklistsOutput)) Or
             Not (System.IO.Directory.Exists(CurrentDirectory & CloudProjectsettings.Folder_PicklistsStandard)) Or
             Not (System.IO.Directory.Exists(CurrentDirectory & CloudProjectsettings.Folder_PicklistsExtractedXliff)) Or
             Not (System.IO.Directory.Exists(CurrentDirectory & CloudProjectsettings.Folder_PicklistsInput)) Then MsgBox("Incorrect Picklist folder structure in the selected project. Exiting now.", MsgBoxStyle.Critical, "Cloud translator") : Me.Close()

    End Sub
End Class