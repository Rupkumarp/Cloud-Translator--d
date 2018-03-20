Imports goGlobalandxliffToxliff
Imports System.Windows.Forms.ListViewItem
Imports System.IO

Public Class Form_Xliff_to_Xliff

    Public Enum FilePreference
        SameFileNameFirst
        OnlySameFileName
    End Enum

    Public xliff_Folder As String
    Dim fileCollection As New ArrayList

    Private Sub Form_Xliff_to_Xliff_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim lang As String = ProjectManagement.GetActiveProject.LangList ' get_last_projectlanguages()
        Dim langlist() As String = Split(lang, ",")
        CmbLang.Items.Add("[All]")

        Try
            For Each lang In Split(System.IO.File.ReadAllText(appData & DefinitionFiles.Lang_List), vbCrLf)
                If lang.Trim <> "" Then
                    CmbLang.Items.Add(Mid(lang, 1, InStr(lang, Chr(9)) - 1))
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
       

        OutputList.Items.Clear()
        CmbLang.SelectedIndex = 0
        PopulateFiles()
        UpdateStatus()
    End Sub

    Private Sub BtnReloadLang_Click(sender As Object, e As EventArgs) Handles BtnReloadLang.Click
        Reload()
    End Sub

    Private Sub Reload()

        If IsNothing(ReloadAllFiles) Then
            Exit Sub
        End If
        OutputList.Items.Clear()
        For i As Integer = 0 To ReloadAllFiles.Count - 1
            If System.IO.Path.GetExtension(ReloadAllFiles(i)).ToLower = ".xliff" Then
                If Not isXliffinList(ReloadAllFiles(i)) Then
                    If Not fileCollection.Contains(ReloadAllFiles(i)) Then
                        fileCollection.Add(ReloadAllFiles(i))
                    End If
                    Dim litem As ListViewItem = New ListViewItem
                    litem.Text = System.IO.Path.GetFileName(ReloadAllFiles(i))
                    Dim csitem As ListViewSubItem = New ListViewSubItem(litem, ReloadAllFiles(i).ToString)
                    litem.SubItems.Add(csitem)
                    OutputList.Items.Add(litem)
                End If
            End If
        Next
        CmbLang.SelectedIndex = 0
        UpdateStatus()
    End Sub

    Private Sub CmbLang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbLang.SelectedIndexChanged
        If CmbLang.SelectedIndex = 0 Then
            Reload()
        Else
            PopulateFilesbasedonLang()
        End If
        UpdateStatus()
    End Sub

    Dim list As List(Of String)

    Private Sub PopulateFiles()

        With OutputList
            .View = View.Details
            '.GridLines = True
            .FullRowSelect = True
            .HideSelection = False
            .MultiSelect = True
        End With

        'Test
        If Not System.IO.Directory.Exists(xliff_Folder) Then
            Exit Sub
        End If

        If Not BW.IsBusy Then
            BW.RunWorkerAsync()
        End If

    End Sub

    Private Sub BW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BW.DoWork

        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim stack As New Stack(Of String)

        ' Add the initial directory
        stack.Push(xliff_Folder)

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string
            Dim dir As String = stack.Pop
            Try
                ' Add all immediate file paths
                BW.ReportProgress(1, "Searching Directory:" & dir.ToString)
                'Application.DoEvents()
                result.AddRange(Directory.GetFiles(dir, "*.xliff"))

                ' Loop through all subdirectories and add them to the stack.
                Dim directoryName As String
                For Each directoryName In Directory.GetDirectories(dir)
                    stack.Push(directoryName)
                Next

            Catch ex As Exception
            End Try
        Loop

        result.Sort()

        list = result
    End Sub

    Private Function isXliffinList(ByVal sfile As String) As Boolean 'Checks if xliff files already there in listview
        Try
            For Each item As ListViewItem In Me.OutputList.Items
                If item.SubItems.Item(1).Text.ToLower = sfile.ToLower Then
                    Return True
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return False
    End Function

    Private Sub PopulateFilesbasedonLang()
        Dim lang As String = Mid(lang_to_langcode(CmbLang.Text), 1, 2) & "_" & Mid(lang_to_langcode(CmbLang.Text), 3, 2)
        OutputList.Items.Clear()
        For i As Integer = 0 To fileCollection.Count - 1
            If Not isXliffinList(fileCollection(i)) Then
                If Microsoft.VisualBasic.Right(System.IO.Path.GetFileNameWithoutExtension(fileCollection(i).ToString.ToLower), 5).ToLower = lang.ToLower Then
                    Dim litem As ListViewItem = New ListViewItem
                    litem.Text = System.IO.Path.GetFileName(fileCollection(i))
                    Dim csitem As ListViewSubItem = New ListViewSubItem(litem, fileCollection(i).ToString)
                    litem.SubItems.Add(csitem)
                    OutputList.Items.Add(litem)
                End If
            End If
        Next
    End Sub

    Private Sub BtnDeleteLang_Click(sender As Object, e As EventArgs) Handles BtnDeleteLang.Click

        'If OutputList.SelectedItems.Count = 0 Then
        '    If MsgBox("Do you want to remove all files from the list?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Xliff to Xliff") = MsgBoxResult.Yes Then
        '        OutputList.Items.Clear()
        '        Exit Sub
        '    End If
        'End If

        For i = 0 To OutputList.SelectedItems.Count - 1
            fileCollection.Remove(OutputList.SelectedItems(0).SubItems(1).Text)
            OutputList.Items.RemoveAt(OutputList.SelectedItems(0).Index)
        Next
        UpdateStatus()
    End Sub

    Private Sub BtnChangeFolder_Click(sender As Object, e As EventArgs) Handles BtnChangeFolder.Click
        Dim xFolderDialog As New FolderBrowserDialog
        xFolderDialog.SelectedPath = xliff_Folder

        If xFolderDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.Enabled = False
            xliff_Folder = xFolderDialog.SelectedPath
            CmbLang.SelectedIndex = 0
            PopulateFiles()
        End If

    End Sub

    Private Sub UpdateStatus()
        If OutputList.Items.Count = 0 Then
            StatusBar.Text = "Current Folder - " & xliff_Folder & " | Status: No files found please select some other folder or language"
        Else
            StatusBar.Text = "Current Folder - " & xliff_Folder & " | Status: " & OutputList.Items.Count & " files found"
        End If
    End Sub


    Private Sub MoveItemsInListView(ByRef lvwListView As ListView, ByVal blnMoveUp As Boolean)
        Try
            'Set the listview index to limit to depending on whether we are moving things up or down in the list
            Dim intLimittedIndex As Integer = (lvwListView.Items.Count - 1)
            If blnMoveUp Then intLimittedIndex = 0

            'Define a new collection of the listview indexes to move
            Dim colIndexesToMove As New List(Of Integer)()

            'Loop through each selected item in the listview (multiple select support)
            For Each lviSelectedItem As ListViewItem In lvwListView.SelectedItems
                'Add the item's index to the collection
                colIndexesToMove.Add(lviSelectedItem.Index)

                'If this item is at the limit we defined
                If lviSelectedItem.Index = intLimittedIndex Then
                    'Do not attempt to move item(s) as we are at the top or bottom of the list
                    Exit Try
                End If
            Next

            'If we are moving items down
            If Not blnMoveUp Then
                'Reverse the index list so that we move items from the bottom of the selection first
                colIndexesToMove.Reverse()
            End If

            'Loop through each index we want to move
            For Each intIndex As Integer In colIndexesToMove
                'Define a new listviewitem
                Dim lviNewItem As ListViewItem = CType(lvwListView.Items(intIndex).Clone(), ListViewItem)

                'Remove the currently selected item from the list
                lvwListView.Items(intIndex).Remove()

                'Insert the new item in it's new place
                If blnMoveUp Then
                    lvwListView.Items.Insert(intIndex - 1, lviNewItem)
                Else
                    lvwListView.Items.Insert(intIndex + 1, lviNewItem)
                End If

                'Set the new item to be selected
                lviNewItem.Selected = True
            Next
        Catch ex As Exception
            Trace.WriteLine("MoveItemsInListView() has thrown an exception: " & ex.Message)
        Finally
            'Set the focus on the listview
            lvwListView.Focus()
        End Try
    End Sub

    Private Sub BtnUP_Click(sender As Object, e As EventArgs) Handles BtnUP.Click
        MoveItemsInListView(OutputList, True)
    End Sub

    Private Sub BtnDown_Click(sender As Object, e As EventArgs) Handles BtnDown.Click
        MoveItemsInListView(OutputList, False)
    End Sub

    Private Sub Btn1Browse_Click(sender As Object, e As EventArgs) Handles Btn1Browse.Click
        Dim fDialog As Object
        If RBSingle.Checked Then
            fDialog = New OpenFileDialog
            fDialog.Filter = "xliff  files (*.xliff)|*.xliff;"
        Else
            fDialog = New FolderBrowserDialog
            fDialog.Description = "Select Xliff Input folder"
        End If

        If fDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            If RBSingle.Checked Then
                txtInputXlifffile.Text = fDialog.FileName
            Else
                txtInputXlifffile.Text = fDialog.Selectedpath
            End If
        End If
    End Sub

    Private Sub Btn2browse_Click(sender As Object, e As EventArgs) Handles Btn2browse.Click
        Dim fDialog As New FolderBrowserDialog
        fDialog.Description = "Select output path"
        If fDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtOutput.Text = fDialog.SelectedPath
        End If
    End Sub

    Private Sub BtnStartCopying_Click(sender As Object, e As EventArgs) Handles BtnStartCopying.Click
        'If CmbLang.SelectedIndex = 0 Then
        '    MsgBox("Please select language to copy xliffs!", MsgBoxStyle.Exclamation, "Cloud Translator!")
        '    Exit Sub
        'End If

        If txtInputXlifffile.Text = "" Then
            MsgBox("No input file selected!", MsgBoxStyle.Exclamation, "Cloud Translator!")
            Exit Sub
        Else
            If RBSingle.Checked Then
                If Not System.IO.File.Exists(txtInputXlifffile.Text) Then
                    MsgBox("Xliff input file cannot be accessed, Please check if the file exists in the folder!", MsgBoxStyle.Exclamation, "Cloud Translator!")
                    Exit Sub
                End If
            Else
                If Not System.IO.Directory.Exists(txtInputXlifffile.Text) Then
                    MsgBox("Input folder cannot be found, Please check the path!", MsgBoxStyle.Exclamation, "Cloud Translator!")
                    Exit Sub
                End If
            End If
        End If

        If ChkOutPut.Checked Then
            If txtOutput.Text = "" Then
                MsgBox("No output path mentioned!", MsgBoxStyle.Exclamation, "Cloud Translator!")
                Exit Sub
            ElseIf Not System.IO.Directory.Exists(txtOutput.Text) Then
                MsgBox("output path doesn't exitst!", MsgBoxStyle.Exclamation, "Cloud Translator!")
                Exit Sub
            End If
        End If

        Try
            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor

            If Not ChkOutPut.Checked Then
                txtOutput.Text = System.IO.Path.GetDirectoryName(txtInputXlifffile.Text)
            End If


            Form_MainNew.UpdateMsg(Now & Chr(9) & "Initiated Xliff to Xliff" & vbCrLf, Form_MainNew.RtbColor.Black)

            Dim countFiles As Integer = 0
            If RBSingle.Checked Then
                'PreProcessFileListFromLang(ComboBox1.SelectedIndex, txtInputXlifffile.Text)
                Pretranslate(txtInputXlifffile.Text)
                countFiles += 1
            Else
                For Each f In My.Computer.FileSystem.GetFiles(txtInputXlifffile.Text, FileIO.SearchOption.SearchTopLevelOnly)
                    If System.IO.Path.GetExtension(f).ToLower = ".xliff" Then
                        StatusBar.Text = "Processing: " & System.IO.Path.GetFileName(f)
                        ' PreProcessFileListFromLang(ComboBox1.SelectedIndex, f)
                        Pretranslate(f)
                        countFiles += 1
                    End If
                Next
            End If

            Form_MainNew.UpdateMsg(vbCrLf & Now & Chr(9) & "Process completed." & vbCrLf, Form_MainNew.RtbColor.Black)
            Form_MainNew.UpdateMsg(countFiles & " file(s) have been processed." & vbCrLf, Form_MainNew.RtbColor.Black)

        Catch ex As Exception
            Form_MainNew.UpdateMsg(vbCrLf & ex.Message & vbCrLf, Form_MainNew.RtbColor.Red)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error - Xliff to Xliff ")
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private WithEvents xx As xliff_To_xliff

    Private Sub updateUi(ByVal msg As String) Handles xx.UpdateMsg
        StatusBar.Text = msg
        Application.DoEvents()
    End Sub

    Dim Summary As String

    Private Sub Pretranslate(ByVal inputxliffifle As String)

        Try
            OutputList.SelectedItems.Clear()
            If Microsoft.VisualBasic.Right(txtOutput.Text, 1) = "\" Then
                txtOutput.Text = txtOutput.Text.Substring(0, Len(txtOutput.Text) - 1)
            End If

            Dim outPutfile As String = txtOutput.Text & "\" & System.IO.Path.GetFileName(inputxliffifle)
            Dim translatedXliffFile As String = ""
            'Dim lang As String = Mid(lang_to_langcode(CmbLang.Text), 1, 2) & "_" & Mid(lang_to_langcode(CmbLang.Text), 3, 2)

            For Each item As ListViewItem In Me.OutputList.Items
                item.Selected = True
                StatusBar.Text = "Status: Copying translation from " & System.IO.Path.GetFileNameWithoutExtension(translatedXliffFile)
                translatedXliffFile = item.SubItems.Item(1).Text
                xx = New xliff_To_xliff
                If xx.getLangFromxliff(inputxliffifle).ToLower = xx.getLangFromxliff(translatedXliffFile).ToLower Then
                    xx.CopyTranslation(inputxliffifle, translatedXliffFile, outPutfile) 'goGlobal translation
                    inputxliffifle = outPutfile
                    If xx.MatchCount = xx.TotalCount Then
                        Form_MainNew.UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileNameWithoutExtension(inputxliffifle) & " compared with " & System.IO.Path.GetFileNameWithoutExtension(translatedXliffFile) & " - " & "Total Match count " & xx.MatchCount & "\" & xx.TotalCount & vbCrLf, Form_MainNew.RtbColor.Black)
                        Exit For
                    End If
                    Form_MainNew.UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileNameWithoutExtension(inputxliffifle) & " compared with " & System.IO.Path.GetFileNameWithoutExtension(translatedXliffFile) & " - " & "Total Match count " & xx.MatchCount & "\" & xx.TotalCount & vbCrLf, Form_MainNew.RtbColor.Black)

                End If
            Next

            OutputList.SelectedItems.Clear()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub BW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BW.ProgressChanged
        StatusBar.Text = e.UserState.ToString
    End Sub

    Private Sub BW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BW.RunWorkerCompleted
        For i As Integer = 0 To list.Count - 1
            If System.IO.Path.GetExtension(list(i)).ToLower = ".xliff" Then
                If Not isXliffinList(list(i)) Then
                    fileCollection.Add(list(i))
                    Dim litem As ListViewItem = New ListViewItem
                    litem.Text = System.IO.Path.GetFileName(list(i))
                    Dim csitem As ListViewSubItem = New ListViewSubItem(litem, list(i))
                    litem.SubItems.Add(csitem)
                    OutputList.Items.Add(litem)
                End If
            End If
        Next

        For i As Integer = 0 To fileCollection.Count - 1
            ReloadAllFiles.Add(fileCollection(i))
        Next

        'ReloadAllFiles = fileCollection
        processSelection()
        UpdateStatus()
        Me.Enabled = True
    End Sub

    Private Sub OutputList_DoubleClick(sender As Object, e As EventArgs) Handles OutputList.DoubleClick, ListView2.DoubleClick
        If OutputList.Items.Count = 0 Then
            Exit Sub
        End If
        For i = 0 To OutputList.SelectedItems.Count - 1
            Dim str As ListViewItem.ListViewSubItem = OutputList.SelectedItems(i).SubItems.Item(1)
            System.Diagnostics.Process.Start(str.Text)
        Next
    End Sub

    Private Sub ChkOutPut_CheckedChanged(sender As Object, e As EventArgs) Handles ChkOutPut.CheckedChanged
        If ChkOutPut.Checked Then
            txtOutput.Enabled = True
            Btn2browse.Enabled = True
        Else
            txtOutput.Enabled = False
            Btn2browse.Enabled = False
            txtOutput.Text = ""
        End If
    End Sub

    Private Sub RBMultiple_CheckedChanged(sender As Object, e As EventArgs) Handles RBMultiple.CheckedChanged
        txtInputXlifffile.Text = ""
        txtOutput.Text = ""
    End Sub

    Private Sub RBSingle_CheckedChanged(sender As Object, e As EventArgs) Handles RBSingle.CheckedChanged
        txtInputXlifffile.Text = ""
        txtOutput.Text = ""
    End Sub


    Dim ReloadAllFiles As New ArrayList

    'The below function process the following enhancments
    '    1.       Option to automatically handle same file names first.
    'e.G.               Input file: 5.10_fr_FR.xliff 
    '                       Reference files (in this order): 
    'FOLDER1/(processed) 13.6_fr_FR.xliff
    'FOLDER1/(processed) 5.10_fr_FR.xliff
    '                                               FOLDER2/(processed) 5.10_fr_FR.xliff

    'In the current process, it would take first 13.6 & then 5.10 (folder1) and finally 5.10(folder2). We could manually take 5.10 first. However for an input folder, we won’ t be able to change the preferences for each file. The most likely preference is that a file is pretranslated first by previous versions of the same file.
    'What I propose if the option is checked (default), it would go through the list and check first if there is a file with the same file name (be careful with potentially “(processed)” and be careful not to take e.g. 15.10_fr_FR.xliff as a match –instr will not work). 
    'Take the first occurrence first, and then the second in the list,… (the priority should still remain. Folder1 might project a first version, that we want to give priority vs Folder2.). When all that is done, it would then take the other files (but check point 2 first).
    'Please do not change the priority in the list itself. It should stay the same after the processing is completed.

    '2.       Also you could put an option ‘take only same filenames’. In that case it would take only the 2 5.10_fr_FR and not 13.6 as reference. For single file process, it’s possible to easily remove from the reference list those files which are not required, but for folder input, it’s not possible.
    'Note that this option is an extension of option1. We can either have this option or option 1 or ‘as defined in the priority list’ (which is the current behavior).

    Private Sub PreProcessFileListFromLang(ByVal FP As FilePreference, ByVal sFile As String)
        Dim NewFileCollection As New ArrayList
        NewFileCollection = ReloadAllFiles
        fileCollection = Nothing
        fileCollection = New ArrayList

        Dim TempFileCollection As New ArrayList

        For i As Integer = 0 To NewFileCollection.Count - 1
            Dim TargetFile As String = System.IO.Path.GetFileNameWithoutExtension(NewFileCollection(i))
            Dim sFileName As String = System.IO.Path.GetFileNameWithoutExtension(sFile)
            If TargetFile.Contains(sFileName) Then
                TempFileCollection.Add(NewFileCollection(i))
            End If
        Next

        Select Case FP
            Case FilePreference.SameFileNameFirst
                For i As Integer = 0 To NewFileCollection.Count - 1
                    If Not TempFileCollection.Contains(NewFileCollection(i)) Then
                        TempFileCollection.Add(NewFileCollection(i))
                    End If
                Next
        End Select

        fileCollection = TempFileCollection

        OutputList.Items.Clear()
        For i As Integer = 0 To fileCollection.Count - 1
            Dim litem As ListViewItem = New ListViewItem
            litem.Text = System.IO.Path.GetFileName(fileCollection(i))
            Dim csitem As ListViewSubItem = New ListViewSubItem(litem, fileCollection(i))
            litem.SubItems.Add(csitem)
            OutputList.Items.Add(litem)
        Next

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        processSelection()
    End Sub

    Private Sub processSelection()
        If RBSingle.Checked Then
            If System.IO.File.Exists(txtInputXlifffile.Text) Then
                PreProcessFileListFromLang(ComboBox1.SelectedIndex, txtInputXlifffile.Text)
            End If
        Else
            If System.IO.Directory.Exists(txtInputXlifffile.Text) Then
                For Each f In My.Computer.FileSystem.GetFiles(txtInputXlifffile.Text, FileIO.SearchOption.SearchTopLevelOnly)
                    If System.IO.Path.GetExtension(f).ToLower = ".xliff" Then
                        PreProcessFileListFromLang(ComboBox1.SelectedIndex, f)
                    End If
                Next
            End If

        End If
    End Sub


End Class


'Public Function GetFilesRecursive(ByVal initial As String) As List(Of String)
'    ' This list stores the results.
'    Dim result As New List(Of String)

'    ' This stack stores the directories to process.
'    Dim stack As New Stack(Of String)

'    ' Add the initial directory
'    stack.Push(initial)

'    ' Continue processing for each stacked directory
'    Do While (stack.Count > 0)
'        ' Get top directory string
'        Dim dir As String = stack.Pop
'        Try
'            ' Add all immediate file paths
'            'StatusStrip1.Text = "Searching Directory:" & dir.ToString
'            'Application.DoEvents()
'            result.AddRange(Directory.GetFiles(dir, "*.xliff"))

'            ' Loop through all subdirectories and add them to the stack.
'            Dim directoryName As String
'            For Each directoryName In Directory.GetDirectories(dir)
'                stack.Push(directoryName)
'            Next

'        Catch ex As Exception
'        End Try
'    Loop

'    ' Return the list
'    Return result
'End Function