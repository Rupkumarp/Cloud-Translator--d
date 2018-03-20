Imports System.Xml
Imports System.IO
Imports System.Environment
Imports System.ComponentModel
Imports Microsoft.Office.Interop

Public Class Form_XliffToXlsConverter

    Private Sub btnBrowseInputFile_Click(sender As Object, e As EventArgs) Handles btnBrowseInputFile.Click

        If chkBulk.Checked Then
            Dim bDialog As New FolderBrowserDialog
            bDialog.Description = "Select folder"
            If bDialog.ShowDialog = DialogResult.OK Then
                txtInputFilePath.Text = bDialog.SelectedPath
            End If

        ElseIf rdXliffToXls.Checked = True Then
            Try
                ToolStripProgressBar1.Value = ToolStripProgressBar1.Minimum
                Dim OpenFileDialog1 As New OpenFileDialog
                OpenFileDialog1.Filter = "xliff  files (*.xliff)|*.xliff;"
                OpenFileDialog1.Title = "Select xliff file"
                If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                    txtInputFilePath.Text = OpenFileDialog1.FileName
                    Dim fileName As String = OpenFileDialog1.FileName
                    txtOutputFilePath.Text = fileName.Replace("xliff", "xlsx")
                End If
            Catch ex As Exception
                MsgBox(ex, MsgBoxStyle.Critical, "Xliff to Xls Conversion")
            End Try
        ElseIf rdXlsToXliff.Checked = True Then
            Try
                ToolStripProgressBar1.Value = ToolStripProgressBar1.Minimum
                Dim OpenFileDialog1 As New OpenFileDialog
                OpenFileDialog1.Filter = "xlsx  files (*.xlsx)|*.xlsx;"
                OpenFileDialog1.Title = "Select xls file"
                If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                    txtInputFilePath.Text = OpenFileDialog1.FileName
                    Dim fileName As String = OpenFileDialog1.FileName
                    txtOutputFilePath.Text = fileName.Replace("xlsx", "xliff")
                End If
            Catch ex As Exception
                MsgBox(ex, MsgBoxStyle.Critical, "Xlsx to Xliff Conversion")
            End Try
        End If
    End Sub

    Private Sub rdXliffToXls_CheckedChanged(sender As Object, e As EventArgs) Handles rdXliffToXls.CheckedChanged
        ClearFileSelection(0)
    End Sub

    Private Sub rdXlsToXliff_CheckedChanged(sender As Object, e As EventArgs) Handles rdXlsToXliff.CheckedChanged
        ClearFileSelection(1)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

#Region "Functions"

    Sub ClearFileSelection(ByVal fileOption As Integer)
        Select Case fileOption
            Case 0
                ToolStripProgressBar1.Value = ToolStripProgressBar1.Minimum
                Label5.Text = "File conversion from xliff to xls"
                txtInputFilePath.Clear()
                txtOutputFilePath.Clear()
            Case 1
                ToolStripProgressBar1.Value = ToolStripProgressBar1.Minimum
                Label5.Text = "File conversion from xls to xliff"
                txtInputFilePath.Clear()
                txtOutputFilePath.Clear()
        End Select
    End Sub

    Sub CreateXliff(ByVal filepath As String, ByVal Targetlanguage As String)
        Try
            Dim filename As String = Path.GetFileNameWithoutExtension(filepath)

            If (System.IO.File.Exists(txtOutputFilePath.Text)) Then
                Dim buttons As MessageBoxButtons = MessageBoxButtons.OKCancel
                Dim result As DialogResult = MessageBox.Show("File already exists, Do you want overwrite?" + Environment.NewLine + txtOutputFilePath.Text, "File Exists", buttons, MessageBoxIcon.Information)
                If result = DialogResult.OK Then
                    File.Copy(filepath, txtOutputFilePath.Text, True)
                ElseIf result = DialogResult.Cancel Then
                    Exit Sub
                End If
            End If

            Using Writer As StreamWriter = New StreamWriter(txtOutputFilePath.Text, False, System.Text.Encoding.UTF8)
                Writer.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                Writer.WriteLine("<xliff version=""1.2"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" ")
                Writer.WriteLine("xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2- ")
                Writer.WriteLine("strict.xsd"">")
                Writer.WriteLine("<file original=""C:\Temp\en_fr_2.xliff"" source-language=""en-US"" target-language=" & Chr(34) & Targetlanguage & Chr(34) & " datatype=""plaintext"" date=" & Chr(34) & System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & Chr(34) & " tool-id=""PTLS SAP TRANSLATION MANAGER"" category=""MLT"">")
                ' Writer.WriteLine("tool-id=""PTLS SAP TRANSLATION MANAGER"" category=""MLT"">")
                Writer.WriteLine("<header>")
                Writer.WriteLine("<phase-group>")
                Writer.WriteLine("<phase phase-name=""Translation"" process-name=""999999"" company-name=""SAP"">")
                Writer.WriteLine("</phase>")
                Writer.WriteLine("</phase-group>")
                Writer.WriteLine("<tool tool-id=""SAP_STM""  tool-name=""STM"">")
                Writer.WriteLine("</tool>")
                Writer.WriteLine("<note>TEST</note>")
                Writer.WriteLine("</header>")
                Writer.WriteLine("<body>" & vbCrLf)

                xlWkb = XlApp.Workbooks.Open(filepath)
                xlWksht = xlWkb.Worksheets(1) 'Sheet1

                Dim emptyTarget As Boolean = False
                ToolStripProgressBar1.Maximum = xlWksht.Range("A66536").End(Microsoft.Office.Interop.Excel.XlDirection.xlUp).Row
                For i As Integer = 2 To xlWksht.Range("A66536").End(Microsoft.Office.Interop.Excel.XlDirection.xlUp).Row
                    Writer.WriteLine("<trans-unit id=" & Chr(34) & clean_xml(xlWksht.Cells(i, 1).value) & Chr(34) & " resname=" & Chr(34) & clean_xml(xlWksht.Cells(i, 2).value) & Chr(34) & ">")
                    Writer.WriteLine("<source>" & clean_xml(xlWksht.Cells(i, 3).value) & "</source>")
                    If xlWksht.Cells(i, 5).value.ToString = String.Empty And emptyTarget = False Then
                        Dim buttons As MessageBoxButtons = MessageBoxButtons.YesNo
                        emptyTarget = True
                        Dim result As DialogResult = MessageBox.Show("Some entries are untranslated – do you want to continue creating the xliff", "Untranslated entries", buttons, MessageBoxIcon.Information)
                        'If result = DialogResult.No Then
                        '    ToolStripProgressBar1.Value = ToolStripProgressBar1.Minimum
                        '    Exit For
                        'End If
                    End If
                    Writer.WriteLine("<target state=""needs-review-translation"">" & clean_xml(xlWksht.Cells(i, 5).value) & "</target>")
                    Writer.WriteLine("<note from=""Developer"" priority =""10"">" & clean_xml(xlWksht.Cells(i, 4).value) & "</note>")
                    Writer.WriteLine("</trans-unit>" & vbCrLf)
                    ToolStripProgressBar1.Value = i
                Next
                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")
                xlWkb.Close()
                '~~> Quit the Excel Application
                XlApp.Quit()

            End Using

        Catch ex As Exception
            Throw New Exception("Error creating xliff" & vbNewLine & ex.Message)
            XlApp = Nothing
        End Try

    End Sub

    Public Function clean_xml(ByVal instring As String) As String
        instring = Replace(instring, "&", "&amp;")
        instring = Replace(instring, "<", "&lt;")
        instring = Replace(instring, ">", "&gt;")
        instring = Replace(instring, Chr(34), "&quot;")
        clean_xml = Replace(instring, "'", "&apos;")
    End Function

    Public Function unwrap_html(ByVal instring As String) As String
        If InStr(instring, "<ph id") = 0 Then Return instring
        Dim ph_id As Integer = 200
        Do While InStr(instring, "<ph id") <> 0
            instring = Replace(instring, "<ph id=" & Chr(34) & ph_id & Chr(34) & ">", "")
            instring = Replace(instring, "</ph>", "")
            ph_id = ph_id - 1
            If ph_id = -1 Then
                Exit Do
            End If
        Loop
        Return instring

    End Function

    Private Function getFileDescription(ByVal filename As String) As String

        getFileDescription = "en-US"

        If (filename.Contains("ja_JP")) Then
            getFileDescription = "ja-JP"
        ElseIf (filename.Contains("fr_FR")) Then
            getFileDescription = "fr-FR"
        ElseIf (filename.Contains("es_ES")) Then
            getFileDescription = "es-ES"
        ElseIf (filename.Contains("zh_CN")) Then
            getFileDescription = "zh-CN"
        ElseIf (filename.Contains("ko_KR")) Then
            getFileDescription = "ko-KR"
        ElseIf (filename.Contains("de_DE")) Then
            getFileDescription = "de-DE"
        ElseIf (filename.Contains("ru_RU")) Then
            getFileDescription = "ru-RU"
        ElseIf (filename.Contains("pt_BR")) Then
            getFileDescription = "pt-BR"
        End If

    End Function

#End Region

    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click

        Try
            If txtInputFilePath.Text = "" Then MsgBox("Please select a file for conversion", MsgBoxStyle.Critical) : Exit Sub
            If Not chkBulk.Checked Then
                If txtOutputFilePath.Text = "" Then MsgBox("Please select target filename ", MsgBoxStyle.Critical) : Exit Sub
            End If


            Dim targetLangauge As String = ""

            If rdXliffToXls.Checked = True Then
                Cursor.Current = Cursors.WaitCursor
                ToolStripStatusLabel1.Text = "Converting..."

                If chkBulk.Checked Then
                    Bulk_xliff_to_xls()
                Else
                    ProcessXliffToXls()
                End If

                ToolStripStatusLabel1.Text = "Idle"
                Cursor.Current = Cursors.Default
            ElseIf rdXlsToXliff.Checked = True Then
                Cursor.Current = Cursors.WaitCursor
                ToolStripStatusLabel1.Text = "Converting..."

                If chkBulk.Checked Then
                    Bulk_xls_to_xliff()
                Else
                    targetLangauge = getFileDescription(txtInputFilePath.Text.Split("\").Last)
                    CreateXliff(txtInputFilePath.Text, targetLangauge)
                End If

                ToolStripStatusLabel1.Text = "Idle"
                Cursor.Current = Cursors.Default
            End If

            MsgBox("Completed sucessfully", MsgBoxStyle.Information, "Cloud translator")

            ToolStripProgressBar1.Value = ToolStripProgressBar1.Minimum
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Cloud translator")
        End Try


    End Sub

    Private Sub Form_XliffToXlsConverter_Load(sender As Object, e As EventArgs) Handles Me.Load
        ToolStripStatusLabel1.Text = "Idle"
        ToolStripProgressBar1.Value = ToolStripProgressBar1.Minimum
    End Sub

    Private Sub btnBrowseOutputFile_Click(sender As Object, e As EventArgs) Handles btnBrowseOutputFile.Click
        Try
            Dim path As String = txtInputFilePath.Text.Split("\").Last()
            Dim OpenFileDialog1 As New FolderBrowserDialog
            OpenFileDialog1.RootFolder = Environment.SpecialFolder.Desktop
            OpenFileDialog1.SelectedPath = "C:\"
            OpenFileDialog1.Description = "Select Application Configeration Files Path"
            If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                If rdXlsToXliff.Checked = True Then
                    txtOutputFilePath.Text = OpenFileDialog1.SelectedPath & "\" & path.Replace("xls", "xliff")
                Else
                    txtOutputFilePath.Text = OpenFileDialog1.SelectedPath & "\" & path.Replace("xliff", "xls")

                End If
            End If
        Catch ex As Exception
            MsgBox(ex, MsgBoxStyle.Critical, "Xliff to Xls Conversion")
        End Try

    End Sub

    Sub ProcessXliffToXls()
        Dim objXliff As New sXliff
        objXliff = ModHelper.load_xliff(txtInputFilePath.Text)
        Dim objExcel As New ClsExcel()
        Try
            If objXliff.ID.Count > 0 Then
                objExcel.CreateReport(txtOutputFilePath.Text, objXliff)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Hybris")
        End Try
    End Sub


    Private Sub Bulk_xliff_to_xls()
        Dim myFolder As String = txtInputFilePath.Text ' "C:\TranslationRequest\19255 - 19256\02-TobeTranslated"

        If Not Directory.Exists(myFolder) Then
            Exit Sub
        End If

        For Each F In My.Computer.FileSystem.GetFiles(myFolder, FileIO.SearchOption.SearchTopLevelOnly)
            If (Not System.IO.Path.GetFileName(F).StartsWith("$") And (System.IO.Path.GetExtension(F).ToLower = ".xliff")) Then
                txtInputFilePath.Text = F
                Dim fileName As String = F
                txtOutputFilePath.Text = fileName.Replace("xliff", "xlsx")
                ProcessXliffToXls()
            End If
        Next

    End Sub

    Private Sub Bulk_xls_to_xliff()
        Dim myFolder As String = txtInputFilePath.Text ' "C:\TranslationRequest\19255 - 19256\03-Backfromtranslation"

        If Not Directory.Exists(myFolder) Then
            Exit Sub
        End If

        For Each F In My.Computer.FileSystem.GetFiles(myFolder, FileIO.SearchOption.SearchTopLevelOnly)
            If (Not System.IO.Path.GetFileName(F).StartsWith("$") And (System.IO.Path.GetExtension(F).ToLower = ".xlsx") Or System.IO.Path.GetExtension(F).ToLower = ".xls") Then
                txtInputFilePath.Text = F
                Dim fileName As String = F
                Dim targetLangauge As String = ""
                txtOutputFilePath.Text = fileName.Replace("xlsx", "xliff")
                targetLangauge = getFileDescription(txtInputFilePath.Text.Split("\").Last)
                CreateXliff(txtInputFilePath.Text, targetLangauge)
            End If
        Next
    End Sub




End Class