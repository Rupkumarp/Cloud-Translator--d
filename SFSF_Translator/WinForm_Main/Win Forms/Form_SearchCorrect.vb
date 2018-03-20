Imports System
Imports System.Globalization
Imports System.Threading
Imports System.ComponentModel
Imports System.IO
Imports Microsoft.Office.Interop
Imports System.Collections.Specialized
Imports Microsoft.Office
Imports System.Xml
Imports System.Text.RegularExpressions
Imports System.Xml.Serialization
Imports System.Text
Imports System.Diagnostics

Public Class Form_SearchCorrect

#Region "Form Load"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CboLanguage_Load()
        If Not System.IO.Directory.Exists(Application.StartupPath & "\tools\Corrections\01 - PTLS\") Then
            System.IO.Directory.CreateDirectory(Application.StartupPath & "\tools\Corrections\01 - PTLS\")
        End If
        txtFolderPath.Text = Application.StartupPath & "\tools\Corrections\01 - PTLS\"
        btnReplaceTerm.Enabled = False
    End Sub

#End Region

#Region "Button_Click Events"

    Private Sub btnBrowseFolderPath_Click(sender As Object, e As EventArgs) Handles btnBrowseFolderPath.Click
        Dim dialog = New FolderBrowserDialog()
        dialog.SelectedPath = Application.StartupPath
        If DialogResult.OK = dialog.ShowDialog() Then
            txtFolderPath.Text = dialog.SelectedPath
        End If
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim sPath As String = txtFolderPath.Text
        If String.IsNullOrEmpty(cboLanguage.SelectedItem) Then
            MsgBox("Please select the Language", MsgBoxStyle.Critical, "Search & Correct")
            cboLanguage.Focus()
        ElseIf txtSearchTerm.Text Is String.Empty Or txtSearchTerm.Text = "" Then
            grdSearchResult.DataSource = Nothing
            MsgBox("Please enter the string for searching", MsgBoxStyle.Critical, "Search & Correct")
            txtSearchTerm.Focus()
        Else
            btnSearch.Enabled = False
            ' btnReplaceTerm.Enabled = True
            Cursor.Current = Cursors.WaitCursor
            grdSearchResult.DataSource = Nothing
            txtReplaceTerm.Clear()
            txtReplaceTerm.Focus()
            RichTextBox1.Clear()
            DirSearch(sPath)
            Cursor.Current = Cursors.Default
            btnSearch.Enabled = True
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        txtSearchTerm.Text = Clipboard.GetText
    End Sub


    Private Sub btnPaste_Click(sender As Object, e As EventArgs) Handles btnPaste.Click
        txtReplaceTerm.Text = Clipboard.GetText
    End Sub

    Private Sub btnReplaceTerm_Click(sender As Object, e As EventArgs) Handles btnReplaceTerm.Click
        Cursor.Current = Cursors.WaitCursor
        If Not System.IO.Directory.Exists(Application.StartupPath & "\tools\Corrections\03 - Backup\") Then
            System.IO.Directory.CreateDirectory(Application.StartupPath & "\tools\Corrections\03 - Backup\")
        End If
        Dim DestPath As String = Application.StartupPath & "\tools\Corrections\03 - Backup\"
        If txtReplaceTerm.Text Is String.Empty Or txtReplaceTerm.Text = "" Then
            MsgBox("Please enter the string for Replace Term", MsgBoxStyle.Critical, "Search & Correct")
            txtReplaceTerm.Focus()
            Return
        End If
        Dim TotalRows As Integer = grdSearchResult.Rows.Count
        Dim SelectedRows As Integer = grdSearchResult.SelectedRows.Count
        If (SelectedRows > 0) Then
            Dim selectedItems As DataGridViewSelectedRowCollection = grdSearchResult.SelectedRows
            For Each selectedItem As DataGridViewRow In selectedItems
                Try
                    RichTextBox1.AppendText(DateTime.Now.ToString("ddMMyyyy.hhmmss") & "      " & "Replace function started." & Environment.NewLine)
                    RichTextBox1.AppendText(DateTime.Now.ToString("ddMMyyyy.hhmmss") & "      " & "Replace " & txtSearchTerm.Text & " by " & txtReplaceTerm.Text & Environment.NewLine)
                    Dim file = New FileInfo(grdSearchResult.Rows(selectedItem.Index).Cells(5).Value)
                    Dim logFile As String = String.Format(DestPath & grdSearchResult.Rows(selectedItem.Index).Cells(0).Value & "." & DateTime.Now.ToString("ddMMyyyy") & "_" & file.Name.Substring(file.Name.LastIndexOf("_") - 2))
                    If Not System.IO.File.Exists(logFile) Then
                        My.Computer.FileSystem.CopyFile(file.ToString(), logFile, True)
                    End If
                    'find & Replace-------------------------------------------------------------------------------------------------
                    Dim extension As String = Path.GetExtension(file.FullName)
                    If extension = ".docx" Or extension = ".doc" Then
                        closeWinword(file.FullName)
                        Dim objWordApp As Word.Application
                        objWordApp = CreateObject("Word.Application")
                        Dim objDoc As Word.Document = objWordApp.Documents.Open(file.FullName, )
                        objDoc.Content.Find.ClearFormatting()
                        Try
                            If chkFullStringOnly.Checked = True Then
                                If objDoc.Content.Find.Execute(txtSearchTerm.Text, True, True, , , , , , , txtReplaceTerm.Text, Word.WdReplace.wdReplaceAll, Word.WdFindWrap.wdFindContinue) = True Then
                                    Dim pageNo As Int32 = objDoc.Range.Information(Word.WdInformation.wdActiveEndPageNumber)
                                End If
                            Else
                                If objDoc.Content.Find.Execute(txtSearchTerm.Text, , True, , , , True, , , txtReplaceTerm.Text, Word.WdReplace.wdReplaceAll, Word.WdFindWrap.wdFindContinue) = True Then
                                    Dim pageNo As Int32 = objDoc.Range.Information(Word.WdInformation.wdActiveEndPageNumber)
                                End If
                            End If
                            objDoc.Close()
                            objWordApp.Quit()
                            objDoc = Nothing
                            objWordApp = Nothing
                        Catch ex As Exception
                            objDoc.Close()
                            objWordApp.Quit()
                            objDoc = Nothing
                            objWordApp = Nothing
                        End Try
                    ElseIf extension = ".xliff" Then
                        Dim strTextFileInfo() As String
                        Dim strTextToSearch As String = String.Empty
                        Dim buf = My.Computer.FileSystem.ReadAllText(file.FullName)
                        strTextFileInfo = Split(buf, vbNewLine)

                        For i As Integer = 0 To strTextFileInfo.Count - 1
                            If (strTextFileInfo(i).ToLower.StartsWith("<target state") = True) Then
                                Dim splittedValue() As String = strTextFileInfo(i).Split(New Char() {">"c}, 2)
                                Dim splitValue As String = splittedValue(1).ToString

                                If splitValue = String.Empty Then
                                    i += 1
                                    splitValue = splitValue + strTextFileInfo(i)
                                    While splitValue.EndsWith("</target>") = False
                                        i += 1
                                        splitValue = splitValue + strTextFileInfo(i)
                                    End While
                                End If

                                splitValue = splitValue.Substring(0, splitValue.Length - 9)
                                If chkFullStringOnly.Checked = True Then
                                    If clean_xml(splitValue.ToString().ToLower.Trim) = clean_xml(txtSearchTerm.Text.ToLower.Trim) = True Then
                                        strTextFileInfo(i) = strTextFileInfo(i).Replace(Mid(strTextFileInfo(i), InStr(strTextFileInfo(i), ">") + 1, InStr(strTextFileInfo(i), "</") - InStr(strTextFileInfo(i), ">") - 1), clean_xml(txtReplaceTerm.Text))
                                        IO.File.WriteAllLines(file.FullName, strTextFileInfo)
                                    End If
                                Else
                                    If clean_xml(splitValue.ToString().ToLower.Trim) = clean_xml(txtSearchTerm.Text.ToLower.Trim) = True Then
                                        strTextFileInfo(i) = strTextFileInfo(i).Replace(Mid(strTextFileInfo(i), InStr(strTextFileInfo(i), ">") + 1, InStr(strTextFileInfo(i), "</") - InStr(strTextFileInfo(i), ">") - 1), clean_xml(txtReplaceTerm.Text))
                                        IO.File.WriteAllLines(file.FullName, strTextFileInfo)
                                    ElseIf clean_xml(splitValue.ToString().ToLower).Contains(clean_xml(txtSearchTerm.Text.ToLower)) = True Then
                                        strTextFileInfo(i) = Regex.Replace(strTextFileInfo(i), "\b" & clean_xml(txtSearchTerm.Text.ToLower) & "\b", clean_xml(txtReplaceTerm.Text), RegexOptions.IgnoreCase)
                                        IO.File.WriteAllLines(file.FullName, strTextFileInfo)
                                    End If
                                End If
                            End If
                        Next
                    ElseIf extension = ".txt" Or extension = ".xml" Or extension = ".csv" Or extension = ".lms" Then
                        Dim objReader As New System.IO.StreamReader(file.FullName)
                        Dim strTextFileInfo() As String = Nothing
                        Dim arrCounter As Integer = 0
                        Dim strTextToSearch As String = String.Empty

                        Do While objReader.Peek <> -1
                            ReDim Preserve strTextFileInfo(arrCounter)
                            strTextFileInfo(arrCounter) = objReader.ReadLine
                            arrCounter = arrCounter + 1
                        Loop
                        objReader.Close()
                        For i As Integer = 0 To arrCounter - 1
                            If (strTextFileInfo(i).ToLower.Contains(txtSearchTerm.Text.ToLower) = True) Then
                                strTextFileInfo(i) = strTextFileInfo(i).ToLower.Replace(txtSearchTerm.Text.ToLower, txtReplaceTerm.Text)
                                IO.File.WriteAllLines(file.FullName, strTextFileInfo)
                            End If
                        Next
                        '-------------------------------------------------------------------------------------------------------------------
                    End If
                    grdSearchResult.Rows.RemoveAt(grdSearchResult.SelectedRows.Item(0).Index)
                    RichTextBox1.AppendText(DateTime.Now.ToString("ddMMyyyy.hhmmss") & "      " & "File " & file.Name & " updated." & Environment.NewLine)
                    If Not System.IO.File.Exists(Application.StartupPath & "\tools\Corrections\Log.txt") Then
                        System.IO.File.Exists(Application.StartupPath & "\tools\Corrections\Log.txt")
                    End If
                    My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\tools\Corrections\Log.txt", RichTextBox1.Text.Replace(Chr(10), Environment.NewLine), True)
                Catch ex As Exception

                End Try
            Next
            ToolStripStatusLabel1.Text = "Task Completed – " & SelectedRows & "/" & TotalRows & " files updated."
        Else
            MsgBox("Please select files for Correction", MsgBoxStyle.Information, "Search & Correct")
        End If
        Cursor.Current = Cursors.Default
    End Sub

#End Region

#Region "grdSearchResult_CellMouseEnter"

    Private Sub grdSearchResult_CellMouseEnter(sender As Object, e As DataGridViewCellEventArgs) Handles grdSearchResult.CellMouseEnter
        If e.RowIndex > -1 And e.ColumnIndex = 0 Then
            If grdSearchResult.Rows(e.RowIndex).Cells(e.ColumnIndex).Value IsNot Nothing Then
                grdSearchResult.Rows(e.RowIndex).Cells(0).ToolTipText = getFileDescription(grdSearchResult.Rows(e.RowIndex).Cells(0).Value)
            End If
        End If
    End Sub

#End Region

#Region "grdSearchResult_CellDoubleClick"

    Private Sub grdSearchResult_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles grdSearchResult.CellDoubleClick
        If e.RowIndex > -1 And e.ColumnIndex = 4 Then
            Dim value As Object = grdSearchResult.Rows(e.RowIndex).Cells(5).Value
            Dim extension As String = Path.GetExtension(value)
            If (value Is Nothing) Then
                MsgBox("Cannot get file name from grid", MsgBoxStyle.Information)
                Return
            Else
                If extension = ".xliff" Then

                    Dim strTextFileInfo() As String
                    Dim strTextToSearch As String = String.Empty
                    Dim buf = My.Computer.FileSystem.ReadAllText(value.ToString())
                    strTextFileInfo = Split(buf, vbNewLine)

                    Dim Loc As Integer = grdSearchResult.Rows(e.RowIndex).Cells(3).Value
                    Loc = Loc - 2
                    While (strTextFileInfo(Loc).ToLower.StartsWith("<trans-unit") = False)
                        Loc = Loc - 1
                    End While

                    For i As Integer = Loc To strTextFileInfo.Count - 1
                        If (strTextFileInfo(i).ToLower.EndsWith("/trans-unit>") = True) Then
                            Form_TransUnit.RichTextBox1.AppendText(strTextFileInfo(i) & Environment.NewLine)
                            Form_TransUnit.Show()
                            Return
                        Else
                            Form_TransUnit.RichTextBox1.AppendText(strTextFileInfo(i) & Environment.NewLine)
                        End If
                    Next

                ElseIf extension = ".docx" Or extension = ".doc" Then
                    Dim objWordApp As New Word.Application
                    objWordApp.Visible = True
                    Dim objDoc As Word.Document = objWordApp.Documents.Open(value)
                    objDoc = objWordApp.ActiveDocument
                    objDoc.Content.Find.Execute(FindText:=txtSearchTerm.Text.ToString(), Forward:=True)
                    If chkFullStringOnly.Checked = True Then
                        objDoc.Content.Find.HitHighlight(FindText:=txtSearchTerm.Text.ToString(), MatchCase:=True, MatchWholeWord:=True, HighlightColor:=2552550)
                    Else
                        objDoc.Content.Find.HitHighlight(FindText:=txtSearchTerm.Text.ToString(), HighlightColor:=2552550)
                    End If
                Else
                    Diagnostics.Process.Start(value)
                End If
            End If
        End If
    End Sub

#End Region

#Region "Methods"

    Public Sub CboLanguage_Load()
        cboLanguage.Items.Clear()
        If Not (System.IO.File.Exists(Application.StartupPath & "\FileType\Languages.txt")) Then MsgBox("File Language.txt doesn't exist. Critical error!", MsgBoxStyle.Critical, "Search & Correct")
        For Each lang In Split(System.IO.File.ReadAllText(Application.StartupPath & "\FileType\Languages.txt"), vbCrLf)
            cboLanguage.Items.Add(Mid(lang, 1, InStr(lang, Chr(9)) - 1))
        Next
    End Sub

    Private Sub DirSearch(ByVal sPath As String)
        btnReplaceTerm.Enabled = True
        Dim table As New DataTable
        Dim extension As String
        Dim findText As String = txtSearchTerm.Text
        Dim count As Int32 = 0
        Dim FC_IASN, FC_IASN1 As Integer
        Dim stack As New Stack(Of String)
        Dim directoryName As String

        table.Columns.Add("File ID ", GetType(String))
        table.Columns.Add("Subfolder", GetType(String))
        table.Columns.Add("Full String", GetType(String))
        table.Columns.Add("Loc", GetType(String))
        table.Columns.Add("Search Term", GetType(String))
        table.Columns.Add("FilePath", GetType(String))
        Try
            Dim pattern As String = ""
            If cboLanguage.SelectedItem = "Arabic" Then
                pattern = "*ar_AR.*"
            ElseIf cboLanguage.SelectedItem = "Chinese (simplified)" Then
                pattern = "*zh_CN.*"
            ElseIf cboLanguage.SelectedItem = "Chinese (traditional)" Then
                pattern = "*zh_TW.*"
            ElseIf cboLanguage.SelectedItem = "Croatian" Then
                pattern = "*hr_HR.*"
            ElseIf cboLanguage.SelectedItem = "Czech" Then
                pattern = "*cs_CZ.*"
            ElseIf cboLanguage.SelectedItem = "Dutch" Then
                pattern = "*nl_NL.*"
            ElseIf cboLanguage.SelectedItem = "French (Quebec)" Then
                pattern = "*fr_CA.*"
            ElseIf cboLanguage.SelectedItem = "French (France)" Then
                pattern = "*fr_FR.*"
            ElseIf cboLanguage.SelectedItem = "German" Then
                pattern = "*de_DE.*"
            ElseIf cboLanguage.SelectedItem = "Greek" Then
                pattern = "*el_GR.*"
            ElseIf cboLanguage.SelectedItem = "Hebrew" Then
                pattern = "*he_IL.*"
            ElseIf cboLanguage.SelectedItem = "Hungarian" Then
                pattern = "*hu_HU.*"
            ElseIf cboLanguage.SelectedItem = "Indonesian" Then
                pattern = "*id_ID.*"
            ElseIf cboLanguage.SelectedItem = "Italian" Then
                pattern = "*it_IT.*"
            ElseIf cboLanguage.SelectedItem = "Japanese" Then
                pattern = "*ja_JP.*"
            ElseIf cboLanguage.SelectedItem = "Korean" Then
                pattern = "*ko_KR.*"
            ElseIf cboLanguage.SelectedItem = "Polish" Then
                pattern = "*pl_PL.*"
            ElseIf cboLanguage.SelectedItem = "Portuguese (Brazil)" Then
                pattern = "*pt_BR.*"
            ElseIf cboLanguage.SelectedItem = "Portuguese (Portugal)" Then
                pattern = "*pt_PT.*"
            ElseIf cboLanguage.SelectedItem = "Romanian" Then
                pattern = "*ro_RO.*"
            ElseIf cboLanguage.SelectedItem = "Russian" Then
                pattern = "*ru_RU.*"
            ElseIf cboLanguage.SelectedItem = "Serbian" Then
                pattern = "*sr_SP.*"
            ElseIf cboLanguage.SelectedItem = "Slovenian" Then
                pattern = "*sl_si.*"
            ElseIf cboLanguage.SelectedItem = "Spanish" Then
                pattern = "*es_ES.*"
            ElseIf cboLanguage.SelectedItem = "Spanish (LA)" Then
                pattern = "*es_CO.*"
            ElseIf cboLanguage.SelectedItem = "Turkish" Then
                pattern = "*tr_TR.*"
            Else
                pattern = "*.*"
            End If

            For Each directoryName In Directory.GetDirectories(sPath)
                stack.Push(directoryName)
            Next

            FC_IASN1 = Directory.GetFiles(sPath, pattern).Length
            If FC_IASN1 > 0 Then
                For Each f As String In Directory.GetFiles(sPath, pattern)
                    Dim id As String = String.Empty
                    Dim subFolder As String = String.Empty
                    Dim fullString As String = String.Empty
                    count = count + 1
                    ToolStripProgressBar1.Text = count
                    ToolStripProgressBar1.Value = count
                    ToolStripStatusLabel1.Text = Split(Path.GetFileName(f), "\").Last
                    extension = Path.GetExtension(f)

                    If (extension = ".docx" Or extension = ".doc") And chkFullStringOnly.Checked = False Then
                        closeWinword(f)
                        Dim objWordApp As Word.Application
                        objWordApp = CreateObject("Word.Application")
                        Dim objDoc As Word.Document = objWordApp.Documents.Open(f, )
                        objDoc.Content.Find.ClearFormatting()
                        Try
                            If objDoc.Content.Find.Execute(findText, , , , , , True) = True Then
                                Dim pageNo As Int32 = objDoc.Range.Information(Word.WdInformation.wdActiveEndPageNumber)
                                id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                subFolder = "\"
                                fullString = "No"
                                table.Rows.Add(id, subFolder, fullString, pageNo, txtSearchTerm.Text, f)
                            End If
                            objDoc.Close()
                            objWordApp.Quit()
                            objDoc = Nothing
                            objWordApp = Nothing
                        Catch ex As Exception
                            objDoc.Close()
                            objWordApp.Quit()
                            objDoc = Nothing
                            objWordApp = Nothing
                        End Try

                    ElseIf (extension = ".docx" Or extension = ".doc") And chkFullStringOnly.Checked = True Then
                        closeWinword(f)
                        Dim objWordApp As Word.Application
                        objWordApp = CreateObject("Word.Application")
                        Dim objDoc As Word.Document = objWordApp.Documents.Open(f, )
                        objDoc.Content.Find.ClearFormatting()
                        Try
                            If objDoc.Content.Find.Execute(findText, MatchCase:=True, MatchWholeWord:=True) = True Then
                                Dim pageNo As Int32 = objDoc.Range.Information(Word.WdInformation.wdActiveEndPageNumber)
                                id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                subFolder = "\"
                                fullString = "Yes"
                                table.Rows.Add(id, subFolder, fullString, pageNo, txtSearchTerm.Text, f)
                            End If
                            objDoc.Close()
                            objWordApp.Quit()
                            objDoc = Nothing
                            objWordApp = Nothing
                        Catch ex As Exception
                            objDoc.Close()
                            objWordApp.Quit()
                            objDoc = Nothing
                            objWordApp = Nothing
                        End Try

                    ElseIf extension = ".xliff" And chkFullStringOnly.Checked = True Then
                        Dim strTextFileInfo() As String
                        Dim strTextToSearch As String = String.Empty
                        Dim buf = My.Computer.FileSystem.ReadAllText(f)
                        strTextFileInfo = Split(buf, vbNewLine)
                        If rdTarget.Checked = True Then
                            For i As Integer = 0 To strTextFileInfo.Count - 1
                                If (strTextFileInfo(i).ToLower.StartsWith("<target state") = True) Then
                                    Dim splittedValue() As String = strTextFileInfo(i).Split(New Char() {">"c}, 2)
                                    Dim splitValue As String = splittedValue(1).ToString

                                    If splitValue = String.Empty Then
                                        i += 1
                                        splitValue = splitValue + strTextFileInfo(i)
                                        While splitValue.EndsWith("</target>") = False
                                            i += 1
                                            splitValue = splitValue + strTextFileInfo(i)
                                        End While
                                    End If

                                    splitValue = splitValue.Substring(0, splitValue.Length - 9)
                                    If _
                                        clean_xml(splitValue.ToString().ToLower.Trim) =
                                        clean_xml(txtSearchTerm.Text.ToLower.Trim) = True Then
                                        id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                        subFolder = "\"
                                        fullString = "Yes"
                                        table.Rows.Add(id, subFolder, fullString, i + 1, txtSearchTerm.Text, f)
                                    End If
                                End If
                            Next

                        ElseIf rdSource.Checked = True Then

                            For i As Integer = 0 To strTextFileInfo.Count - 1
                                If (strTextFileInfo(i).ToLower.StartsWith("<source>") = True) Then
                                    Dim splittedValue() As String = strTextFileInfo(i).Split(New Char() {">"c}, 2)
                                    Dim splitValue As String = splittedValue(1).ToString

                                    If splitValue = String.Empty Then
                                        i += 1
                                        splitValue = splitValue + strTextFileInfo(i)
                                        While splitValue.EndsWith("</source>") = False
                                            i += 1
                                            splitValue = splitValue + strTextFileInfo(i)
                                        End While
                                    End If

                                    splitValue = splitValue.Substring(0, splitValue.Length - 9)
                                    If _
                                        clean_xml(splitValue.ToString().ToLower.Trim) =
                                        clean_xml(txtSearchTerm.Text.ToLower.Trim) = True Then
                                        id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                        subFolder = "\"
                                        fullString = "Yes"
                                        table.Rows.Add(id, subFolder, fullString, i + 1, txtSearchTerm.Text, f)
                                    End If
                                End If
                            Next

                        ElseIf rdBoth.Checked = True Then

                            For i As Integer = 0 To strTextFileInfo.Count - 1
                                If (strTextFileInfo(i).ToLower.StartsWith("<source>") = True) Then
                                    Dim splittedValue() As String = strTextFileInfo(i).Split(New Char() {">"c}, 2)
                                    Dim splitValue As String = splittedValue(1).ToString

                                    If splitValue = String.Empty Then
                                        i += 1
                                        splitValue = splitValue + strTextFileInfo(i)
                                        While splitValue.EndsWith("</source>") = False
                                            i += 1
                                            splitValue = splitValue + strTextFileInfo(i)
                                        End While
                                    End If

                                    splitValue = splitValue.Substring(0, splitValue.Length - 9)
                                    If _
                                        clean_xml(splitValue.ToString().ToLower.Trim) =
                                        clean_xml(txtSearchTerm.Text.ToLower.Trim) = True Then
                                        id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                        subFolder = "\"
                                        fullString = "Yes"
                                        table.Rows.Add(id, subFolder, fullString, i + 1, txtSearchTerm.Text, f)
                                    End If
                                ElseIf (strTextFileInfo(i).ToLower.StartsWith("<target state") = True) Then
                                    Dim splittedValue() As String = strTextFileInfo(i).Split(New Char() {">"c}, 2)
                                    Dim splitValue As String = splittedValue(1).ToString

                                    If splitValue = String.Empty Then
                                        i += 1
                                        splitValue = splitValue + strTextFileInfo(i)
                                        While splitValue.EndsWith("</target>") = False
                                            i += 1
                                            splitValue = splitValue + strTextFileInfo(i)
                                        End While
                                    End If

                                    splitValue = splitValue.Substring(0, splitValue.Length - 9)
                                    If _
                                        clean_xml(splitValue.ToString().ToLower.Trim) =
                                        clean_xml(txtSearchTerm.Text.ToLower.Trim) = True Then
                                        id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                        subFolder = "\"
                                        fullString = "Yes"
                                        table.Rows.Add(id, subFolder, fullString, i + 1, txtSearchTerm.Text, f)
                                    End If
                                End If
                            Next
                        End If

                    ElseIf extension = ".xliff" And chkFullStringOnly.Checked = False Then
                        Dim strTextFileInfo() As String
                        Dim strTextToSearch As String = String.Empty
                        Dim buf = My.Computer.FileSystem.ReadAllText(f)
                        strTextFileInfo = Split(buf, vbNewLine)

                        For i As Integer = 0 To strTextFileInfo.Count - 1
                            If (strTextFileInfo(i).ToLower.StartsWith("<target state") = True) Then
                                Dim splittedValue() As String = strTextFileInfo(i).Split(New Char() {">"c}, 2)
                                Dim splitValue As String = splittedValue(1).ToString

                                If splitValue = String.Empty Then
                                    i += 1
                                    splitValue = splitValue + strTextFileInfo(i)
                                    While splitValue.EndsWith("</target>") = False
                                        i += 1
                                        splitValue = splitValue + strTextFileInfo(i)
                                    End While
                                End If

                                splitValue = splitValue.Substring(0, splitValue.Length - 9)
                                If clean_xml(splitValue.ToString().ToLower).Contains(clean_xml(txtSearchTerm.Text.ToLower)) = True Then
                                    fullString = "No"
                                    If clean_xml(splitValue.ToString().ToLower.Trim) = clean_xml(txtSearchTerm.Text.ToLower.Trim) Then
                                        fullString = "Yes"
                                    End If
                                    id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                    subFolder = "\"
                                    table.Rows.Add(id, subFolder, fullString, i + 1, txtSearchTerm.Text, f)
                                End If
                            End If
                        Next

                    ElseIf (extension = ".txt" Or extension = ".xml" Or extension = ".csv" Or extension = ".lms ") And chkFullStringOnly.Checked = False Then
                        Dim objReader As New System.IO.StreamReader(f)
                        Dim strTextFileInfo() As String = Nothing
                        Dim arrCounter As Integer = 0
                        Dim strTextToSearch As String = String.Empty

                        Do While objReader.Peek <> -1
                            ReDim Preserve strTextFileInfo(arrCounter)
                            strTextFileInfo(arrCounter) = objReader.ReadLine
                            arrCounter = arrCounter + 1
                        Loop
                        objReader.Close()
                        For i As Integer = 0 To arrCounter - 1
                            If (strTextFileInfo(i).ToLower.Contains(txtSearchTerm.Text.ToLower) = True) Then
                                id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                subFolder = "\"
                                fullString = "No"
                                table.Rows.Add(id, subFolder, fullString, i + 1, txtSearchTerm.Text, f)
                            End If
                        Next

                    ElseIf (extension = ".txt" Or extension = ".xml" Or extension = ".csv" Or extension = ".lms ") And chkFullStringOnly.Checked = True Then
                        Dim objReader As New System.IO.StreamReader(f)
                        Dim strTextFileInfo() As String = Nothing
                        Dim arrCounter As Integer = 0
                        Dim strTextToSearch As String = String.Empty

                        Do While objReader.Peek <> -1
                            ReDim Preserve strTextFileInfo(arrCounter)
                            strTextFileInfo(arrCounter) = objReader.ReadLine
                            arrCounter = arrCounter + 1
                        Loop
                        objReader.Close()
                        For i As Integer = 0 To arrCounter - 1
                            If (strTextFileInfo(i).ToLower = txtSearchTerm.Text.ToLower) = True Then
                                id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                subFolder = "\"
                                fullString = "Yes"
                                table.Rows.Add(id, subFolder, fullString, i + 1, txtSearchTerm.Text, f)
                            End If
                        Next

                    End If
                Next
            End If

            If stack.Count > 0 Then
                Dim dir As String = stack.Pop
                FC_IASN = Directory.GetFiles(dir, pattern).Length
                If FC_IASN > 0 Then
                    For Each f As String In Directory.GetFiles(dir, pattern)
                        Dim id As String = String.Empty
                        Dim subFolder As String = String.Empty
                        Dim fullString As String = String.Empty
                        count = count + 1
                        ToolStripProgressBar1.Text = count
                        ToolStripProgressBar1.Value = count
                        ToolStripStatusLabel1.Text = Split(Path.GetFileName(f), "\").Last
                        extension = Path.GetExtension(f)
                        If (extension = ".docx" Or extension = ".doc") And chkFullStringOnly.Checked = False Then
                            closeWinword(f)
                            Dim objWordApp As Word.Application
                            objWordApp = CreateObject("Word.Application")
                            Dim objDoc As Word.Document = objWordApp.Documents.Open(f, )
                            objDoc.Content.Find.ClearFormatting()
                            Try
                                If objDoc.Content.Find.Execute(findText, , , , , , True) = True Then
                                    Dim pageNo As Int32 = objDoc.Range.Information(Word.WdInformation.wdActiveEndPageNumber)
                                    id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                    subFolder = "\"
                                    fullString = "No"
                                    table.Rows.Add(id, subFolder, fullString, pageNo, txtSearchTerm.Text, f)
                                End If
                                objDoc.Close()
                                objWordApp.Quit()
                                objDoc = Nothing
                                objWordApp = Nothing
                            Catch ex As Exception
                                objDoc.Close()
                                objWordApp.Quit()
                                objDoc = Nothing
                                objWordApp = Nothing
                            End Try

                        ElseIf (extension = ".docx" Or extension = ".doc") And chkFullStringOnly.Checked = True Then
                            closeWinword(f)
                            Dim objWordApp As Word.Application
                            objWordApp = CreateObject("Word.Application")
                            Dim objDoc As Word.Document = objWordApp.Documents.Open(f, )
                            objDoc.Content.Find.ClearFormatting()
                            Try
                                If objDoc.Content.Find.Execute(findText, MatchCase:=True, MatchWholeWord:=True) = True Then
                                    Dim pageNo As Int32 = objDoc.Range.Information(Word.WdInformation.wdActiveEndPageNumber)
                                    id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                    subFolder = "\"
                                    fullString = "Yes"
                                    table.Rows.Add(id, subFolder, fullString, pageNo, txtSearchTerm.Text, f)
                                End If
                                objDoc.Close()
                                objWordApp.Quit()
                                objDoc = Nothing
                                objWordApp = Nothing
                            Catch ex As Exception
                                objDoc.Close()
                                objWordApp.Quit()
                                objDoc = Nothing
                                objWordApp = Nothing
                            End Try

                        ElseIf extension = ".xliff" And chkFullStringOnly.Checked = True Then
                            Dim strTextFileInfo() As String
                            Dim strTextToSearch As String = String.Empty
                            Dim buf = My.Computer.FileSystem.ReadAllText(f)
                            strTextFileInfo = Split(buf, vbNewLine)

                            For i As Integer = 0 To strTextFileInfo.Count - 1
                                If (strTextFileInfo(i).ToLower.StartsWith("<target state") = True) Then
                                    Dim splittedValue() As String = strTextFileInfo(i).Split(New Char() {">"c}, 2)
                                    Dim splitValue As String = splittedValue(1).ToString

                                    If splitValue = String.Empty Then
                                        i += 1
                                        splitValue = splitValue + strTextFileInfo(i)
                                        While splitValue.EndsWith("</target>") = False
                                            i += 1
                                            splitValue = splitValue + strTextFileInfo(i)
                                        End While
                                    End If

                                    splitValue = splitValue.Substring(0, splitValue.Length - 9)
                                    If clean_xml(splitValue.ToString().ToLower.Trim) = clean_xml(txtSearchTerm.Text.ToLower.Trim) = True Then
                                        id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                        subFolder = "\"
                                        fullString = "Yes"
                                        table.Rows.Add(id, subFolder, fullString, i + 1, txtSearchTerm.Text, f)
                                    End If
                                End If
                            Next
                        ElseIf extension = ".xliff" And chkFullStringOnly.Checked = False Then
                            Dim strTextFileInfo() As String
                            Dim strTextToSearch As String = String.Empty
                            Dim buf = My.Computer.FileSystem.ReadAllText(f)
                            strTextFileInfo = Split(buf, vbNewLine)

                            For i As Integer = 0 To strTextFileInfo.Count - 1
                                If (strTextFileInfo(i).ToLower.StartsWith("<target state") = True) Then
                                    Dim splittedValue() As String = strTextFileInfo(i).Split(New Char() {">"c}, 2)
                                    Dim splitValue As String = splittedValue(1).ToString

                                    If splitValue = String.Empty Then
                                        i += 1
                                        splitValue = splitValue + strTextFileInfo(i)
                                        While splitValue.EndsWith("</target>") = False
                                            i += 1
                                            splitValue = splitValue + strTextFileInfo(i)
                                        End While
                                    End If

                                    splitValue = splitValue.Substring(0, splitValue.Length - 9)
                                    If clean_xml(splitValue.ToString().ToLower).Contains(clean_xml(txtSearchTerm.Text.ToLower)) = True Then
                                        fullString = "No"
                                        If clean_xml(splitValue.ToString().ToLower.Trim) = clean_xml(txtSearchTerm.Text.ToLower.Trim) Then
                                            fullString = "Yes"
                                        End If
                                        id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                        subFolder = "\"
                                        table.Rows.Add(id, subFolder, fullString, i + 1, txtSearchTerm.Text, f)
                                    End If
                                End If
                            Next

                        ElseIf (extension = ".txt" Or extension = ".xml" Or extension = ".csv" Or extension = ".lms ") And chkFullStringOnly.Checked = False Then
                            Dim objReader As New System.IO.StreamReader(f)
                            Dim strTextFileInfo() As String = Nothing
                            Dim arrCounter As Integer = 0
                            Dim strTextToSearch As String = String.Empty

                            Do While objReader.Peek <> -1
                                ReDim Preserve strTextFileInfo(arrCounter)
                                strTextFileInfo(arrCounter) = objReader.ReadLine
                                arrCounter = arrCounter + 1
                            Loop
                            objReader.Close()
                            For i As Integer = 0 To arrCounter - 1
                                If (strTextFileInfo(i).ToLower.Contains(txtSearchTerm.Text.ToLower) = True) Then
                                    id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                    subFolder = "\"
                                    fullString = "No"
                                    table.Rows.Add(id, subFolder, fullString, i + 1, txtSearchTerm.Text, f)
                                End If
                            Next

                        ElseIf (extension = ".txt" Or extension = ".xml" Or extension = ".csv" Or extension = ".lms ") And chkFullStringOnly.Checked = True Then
                            Dim objReader As New System.IO.StreamReader(f)
                            Dim strTextFileInfo() As String = Nothing
                            Dim arrCounter As Integer = 0
                            Dim strTextToSearch As String = String.Empty

                            Do While objReader.Peek <> -1
                                ReDim Preserve strTextFileInfo(arrCounter)
                                strTextFileInfo(arrCounter) = objReader.ReadLine
                                arrCounter = arrCounter + 1
                            Loop
                            objReader.Close()
                            For i As Integer = 0 To arrCounter - 1
                                If (strTextFileInfo(i).ToLower = txtSearchTerm.Text.ToLower) = True Then
                                    id = Split(System.IO.Path.GetFileNameWithoutExtension(f), "_").First
                                    subFolder = "\"
                                    fullString = "Yes"
                                    table.Rows.Add(id, subFolder, fullString, i + 1, txtSearchTerm.Text, f)
                                End If
                            Next

                        End If
                    Next
                End If
            End If

            If (FC_IASN = 0 And FC_IASN1 = 0) Then
                MsgBox("No " & cboLanguage.SelectedItem.ToString() & " files in the specified directory", MsgBoxStyle.Information, "Search & Correct")
                Return
            End If

        Catch excpt As System.Exception
            Debug.WriteLine(excpt.Message)
        End Try
        ToolStripProgressBar1.Value = 100
        ToolStripProgressBar1.Value = 0
        ToolStripStatusLabel1.Text = Nothing

        If (table.Rows.Count = 0) Then
            btnReplaceTerm.Enabled = False
            txtSearchTerm.Focus()
            MsgBox("No search results found", MsgBoxStyle.Information, "Search & Correct")
            Return
        ElseIf (table.Rows.Count > 200) Then
            MsgBox("Too many matches", MsgBoxStyle.Information, "Search & Correct")
        End If

        grdSearchResult.DataSource = table
        grdSearchResult.AllowUserToAddRows = False
        grdSearchResult.Columns(0).ReadOnly = True
        grdSearchResult.Columns(0).Width = 100
        grdSearchResult.Columns(1).Width = 280
        grdSearchResult.Columns(2).Width = 100
        grdSearchResult.Columns(3).Width = 100
        grdSearchResult.Columns(4).Width = 500
        grdSearchResult.Columns(5).Width = 0
        grdSearchResult.Columns(5).Visible = False

    End Sub

    Private Function getFileDescription(file_ID As String) As String

        Dim file_Description As String = String.Empty
        If file_ID Like "1.1" Or file_ID Like "1.1.*" Then
            file_Description = "mdf picklist"
        ElseIf file_ID Like "2.1" Or file_ID Like "2.1.*" Then
            file_Description = "Employee File"
        ElseIf file_ID Like "2.2" Or file_ID Like "2.2.*" Then
            file_Description = "Employee File country specific"
        ElseIf file_ID Like "2.3" Or file_ID Like "2.3.*" Then
            file_Description = "Foundations Objects"
        ElseIf file_ID Like "2.4" Or file_ID Like "2.4.*" Then
            file_Description = "Foundations Objects country specific"
        ElseIf file_ID Like "2.5" Or file_ID Like "2.5.*" Then
            file_Description = "Picklists"
        ElseIf file_ID Like "3.1" Or file_ID Like "3.1.*" Then
            file_Description = "Position Names"
        ElseIf file_ID Like "4.1" Or file_ID Like "4.1.*" Then
            file_Description = "Time Type Profile"
        ElseIf file_ID Like "4.2" Or file_ID Like "4.2.*" Then
            file_Description = "Time Type"
        ElseIf file_ID Like "4.3" Or file_ID Like "4.3.*" Then
            file_Description = "Time Account Type"
        ElseIf file_ID Like "5.1" Or file_ID Like "5.1.*" Then
            file_Description = "Route Maps"
        ElseIf file_ID Like "5.2" Or file_ID Like "5.2.*" Then
            file_Description = "Requisition Template"
        ElseIf file_ID Like "5.3" Or file_ID Like "5.3.*" Then
            file_Description = "Candidate profile Template"
        ElseIf file_ID Like "5.4" Or file_ID Like "5.4.*" Then
            file_Description = "Application template"
        ElseIf file_ID Like "5.5" Or file_ID Like "5.5.*" Then
            file_Description = "Recruiting roles"
        ElseIf file_ID Like "5.6" Or file_ID Like "5.6.*" Then
            file_Description = "Application Status #1"
        ElseIf file_ID Like "5.7" Or file_ID Like "5.7.*" Then
            file_Description = "Application Status #2"
        ElseIf file_ID Like "5.8" Or file_ID Like "5.8.*" Then
            file_Description = "Application Status #3"
        ElseIf file_ID Like "5.9" Or file_ID Like "5.9.*" Then
            file_Description = "Application Status #4"
        ElseIf file_ID Like "5.10" Or file_ID Like "5.10.*" Then
            file_Description = "Rating Scale"
        ElseIf file_ID Like "5.11" Or file_ID Like "5.11.*" Then
            file_Description = "Event Application template"
        ElseIf file_ID Like "5.12" Or file_ID Like "5.12.*" Then
            file_Description = "Internal recruiting Site"
        ElseIf file_ID Like "5.13" Or file_ID Like "5.13.*" Then
            file_Description = "Offer Detail template"
        ElseIf file_ID Like "5.14" Or file_ID Like "5.14.*" Then
            file_Description = "Default Career Site"
        ElseIf file_ID Like "5.15" Or file_ID Like "5.15.*" Then
            file_Description = "Offer templates"
        ElseIf file_ID Like "5.16" Or file_ID Like "5.16.*" Then
            file_Description = "Question library"
        ElseIf file_ID Like "5.17" Or file_ID Like "5.17.*" Then
            file_Description = "Email templates – recruiting"
        ElseIf file_ID Like "5.18" Or file_ID Like "5.18.*" Then
            file_Description = "Email Notification templates"
        ElseIf file_ID Like "6.1" Or file_ID Like "6.1.*" Then
            file_Description = "Development Goals"
        ElseIf file_ID Like "6.2" Or file_ID Like "6.2.*" Then
            file_Description = "Learning Activity"
        ElseIf file_ID Like "6.3" Or file_ID Like "6.3.*" Then
            file_Description = "Learning Activity with catalog"
        ElseIf file_ID Like "6.4" Or file_ID Like "6.4.*" Then
            file_Description = "Disciplinary Incident History"
        ElseIf file_ID Like "6.5" Or file_ID Like "6.5.*" Then
            file_Description = "Career Worksheet"
        ElseIf file_ID Like "7.1" Or file_ID Like "7.1.*" Then
            file_Description = "Succession Org Chart"
        ElseIf file_ID Like "7.2" Or file_ID Like "7.2.*" Then
            file_Description = "Talent Pools"
        ElseIf file_ID Like "7.3" Or file_ID Like "7.3.*" Then
            file_Description = "Matrix"
        ElseIf file_ID Like "7.4" Or file_ID Like "7.4.*" Then
            file_Description = "Talent Search"
        ElseIf file_ID Like "7.5" Or file_ID Like "7.5.*" Then
            file_Description = "Tier 3 & Text Replacement"
        ElseIf file_ID Like "8.1.1" Or file_ID Like "8.1.1.*" Then
            file_Description = "JPB - job template"
        ElseIf file_ID Like "8.1.2" Or file_ID Like "8.1.2.*" Then
            file_Description = "JPB - job template sections"
        ElseIf file_ID Like "8.1.3" Or file_ID Like "8.1.3.*" Then
            file_Description = "JPB - Family"
        ElseIf file_ID Like "8.2.1" Or file_ID Like "8.2.1.*" Then
            file_Description = "JPB - Job Profile - Short description"
        ElseIf file_ID Like "8.2.2" Or file_ID Like "8.2.2.*" Then
            file_Description = "JPB - Job Profile - Job Responsibility"
        ElseIf file_ID Like "8.2.3" Or file_ID Like "8.2.3.*" Then
            file_Description = "JPB - Job Profile - Degrees"
        ElseIf file_ID Like "8.2.4" Or file_ID Like "8.2.4.*" Then
            file_Description = "JPB - Job Profile - Long description"
        ElseIf file_ID Like "8.3" Or file_ID Like "8.3.*" Then
            file_Description = "Job Skills"
        ElseIf file_ID Like "8.4" Or file_ID Like "8.4.*" Then
            file_Description = "Job Competencies - standard"
        ElseIf file_ID Like "8.5" Or file_ID Like "8.5.*" Then
            file_Description = "Job Competencies - mdf csv"
        ElseIf file_ID Like "9.1A" Or file_ID Like "9.1A.*" Then
            file_Description = "Compensation - Column Designer"
        ElseIf file_ID Like "9.1B" Or file_ID Like "9.1B.*" Then
            file_Description = "Compensation - Budget"
        ElseIf file_ID Like "9.1C" Or file_ID Like "9.1C.*" Then
            file_Description = "Compensation - Budget Rule"
        ElseIf file_ID Like "9.1D" Or file_ID Like "9.1D.*" Then
            file_Description = "Compensation - Instructional text"
        ElseIf file_ID Like "10.1" Or file_ID Like "10.1.*" Then
            file_Description = "Form - Goal Plan"
        ElseIf file_ID Like "10.2" Or file_ID Like "10.2.*" Then
            file_Description = "Form - Goal Plan"
        ElseIf file_ID Like "10.3" Or file_ID Like "10.3.*" Then
            file_Description = "Form - Goal Plan"
        ElseIf file_ID Like "10.4" Or file_ID Like "10.4.*" Then
            file_Description = "Form - Performance review"
        ElseIf file_ID Like "10.5" Or file_ID Like "10.5.*" Then
            file_Description = "Form - Multirater"
        ElseIf file_ID Like "10.6" Or file_ID Like "10.6.*" Then
            file_Description = "Form - Development Plan"
        ElseIf file_ID Like "10.7" Or file_ID Like "10.7.*" Then
            file_Description = "Form - Disciplinary Incident Report"
        ElseIf file_ID Like "10.8" Or file_ID Like "10.8.*" Then
            file_Description = "Form - Role readiness Assessment"
        ElseIf file_ID Like "10.9" Or file_ID Like "10.9.*" Then
            file_Description = "Form - Talent Review"
        ElseIf file_ID Like "10.12" Or file_ID Like "10.12.*" Then
            file_Description = "PM Form Names"
        ElseIf file_ID Like "11.1.1" Or file_ID Like "11.1.1.*" Then
            file_Description = "Employee Profile Standard"
        ElseIf file_ID Like "11.1.2" Or file_ID Like "11.1.2.*" Then
            file_Description = "Employee Profile User Info"
        ElseIf file_ID Like "12.1" Or file_ID Like "12.1.*" Then
            file_Description = "Formlabel keys for PM forms translation"
        ElseIf file_ID Like "13.1" Or file_ID Like "13.1.*" Then
            file_Description = "Rating Criteria"
        ElseIf file_ID Like "13.2" Or file_ID Like "13.2.*" Then
            file_Description = "Learning items"
        ElseIf file_ID Like "13.3" Or file_ID Like "13.3.*" Then
            file_Description = "Curricula"
        ElseIf file_ID Like "13.4" Or file_ID Like "13.4.*" Then
            file_Description = "Questionnaire Survey"
        ElseIf file_ID Like "13.5" Or file_ID Like "13.5.*" Then
            file_Description = "Deployment Location"
        ElseIf file_ID Like "13.6" Or file_ID Like "13.6.*" Then
            file_Description = "Completion status"
        ElseIf file_ID Like "13.7" Or file_ID Like "13.7.*" Then
            file_Description = "Delivery methods"
        ElseIf file_ID Like "13.8" Or file_ID Like "13.8.*" Then
            file_Description = "Item Types"
        ElseIf file_ID Like "13.9" Or file_ID Like "13.9.*" Then
            file_Description = "Subject Areas"
        ElseIf file_ID Like "13.10" Or file_ID Like "13.10.*" Then
            file_Description = "Rating scales"
        ElseIf file_ID Like "13.11" Or file_ID Like "13.11.*" Then
            file_Description = "Report group"
        ElseIf file_ID Like "13.12" Or file_ID Like "13.12.*" Then
            file_Description = "Programs"
        ElseIf file_ID Like "14.1" Or file_ID Like "14.1.*" Then
            file_Description = "LMS"
        ElseIf file_ID Like "15.1" Or file_ID Like "15.1.*" Then
            file_Description = "Dashboard names"
        End If
        Return file_Description

    End Function

    Public Function clean_xml(ByVal instring As String) As String
        instring = Replace(instring, "&", "&amp;")
        instring = Replace(instring, "<", "&lt;")
        instring = Replace(instring, ">", "&gt;")
        instring = Replace(instring, Chr(34), "&quot;")
        clean_xml = Replace(instring, "'", "&apos;")
    End Function

    Public Sub closeWinword(ByVal filename As String)

        Try
            Dim aProcWrd() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcessesByName("WINWORD")
            For Each oProc As System.Diagnostics.Process In aProcWrd
                If oProc.MainWindowTitle.Contains(Path.GetFileName(filename)) Then
                    oProc.Kill()
                End If
            Next

            If (Path.GetFileName(filename).StartsWith("~$.")) Then
                File.Delete(filename)
            End If
        Catch ex As Exception

        End Try

    End Sub

#End Region

End Class

