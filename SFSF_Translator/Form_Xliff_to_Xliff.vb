Imports goGlobalandxliffToxliff
Imports System.Windows.Forms.ListViewItem

Public Class Form_Xliff_to_Xliff

    Public xliff_Folder As String
    Dim fileCollection As New ArrayList

    Private Sub Form_Xliff_to_Xliff_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim lang As String = get_last_projectlanguages()
        Dim langlist() As String = Split(lang, ",")
        CmbLang.Items.Add("[Select]")
       
        For Each lang In Split(System.IO.File.ReadAllText(Application.StartupPath & "\FileType\languages.txt"), vbCrLf)
            CmbLang.Items.Add(Mid(lang, 1, InStr(lang, Chr(9)) - 1))
        Next

        OutputList.Items.Clear()
        CmbLang.SelectedIndex = 0
        PopulateFiles()
        UpdateStatus()

    End Sub

    Private Sub BtnReloadLang_Click(sender As Object, e As EventArgs) Handles BtnReloadLang.Click
        OutputList.Items.Clear()
        For i As Integer = 0 To fileCollection.Count - 1
            If System.IO.Path.GetExtension(fileCollection(i)).ToLower = ".xliff" Then
                If Not isXliffinList(fileCollection(i)) Then
                    fileCollection.Add(fileCollection(i))
                    Dim litem As ListViewItem = New ListViewItem
                    litem.Text = System.IO.Path.GetFileName(fileCollection(i))
                    Dim csitem As ListViewSubItem = New ListViewSubItem(litem, fileCollection(i).ToString)
                    litem.SubItems.Add(csitem)
                    OutputList.Items.Add(litem)
                End If
            End If
        Next
        CmbLang.SelectedIndex = 0
        UpdateStatus()
    End Sub

    Private Sub CmbLang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbLang.SelectedIndexChanged
        If CmbLang.SelectedIndex <> 0 Then
            PopulateFilesbasedonLang()
        End If
        UpdateStatus()
    End Sub

    Private Sub PopulateFiles()

        With OutputList
            .View = View.Details
            '.GridLines = True
            .FullRowSelect = True
            .HideSelection = False
            .MultiSelect = True

        End With

        'Comments

        For Each file In My.Computer.FileSystem.GetFiles(xliff_Folder, FileIO.SearchOption.SearchAllSubDirectories)
            If System.IO.Path.GetExtension(file).ToLower = ".xliff" Then
                If Not isXliffinList(file) Then
                    fileCollection.Add(file)
                    Dim litem As ListViewItem = New ListViewItem
                    litem.Text = System.IO.Path.GetFileName(file)
                    Dim csitem As ListViewSubItem = New ListViewSubItem(litem, file.ToString)
                    litem.SubItems.Add(csitem)
                    OutputList.Items.Add(litem)
                End If
            End If
        Next

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
            If Microsoft.VisualBasic.Right(System.IO.Path.GetFileNameWithoutExtension(fileCollection(i).ToString.ToLower), 5).ToLower = lang.ToLower Then
                Dim litem As ListViewItem = New ListViewItem
                litem.Text = System.IO.Path.GetFileName(fileCollection(i))
                Dim csitem As ListViewSubItem = New ListViewSubItem(litem, fileCollection(i).ToString)
                litem.SubItems.Add(csitem)
                OutputList.Items.Add(litem)
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
            OutputList.Items.RemoveAt(OutputList.SelectedItems(0).Index)
        Next
    End Sub

    Private Sub BtnChangeFolder_Click(sender As Object, e As EventArgs) Handles BtnChangeFolder.Click
        Dim xFolderDialog As New FolderBrowserDialog
        xFolderDialog.SelectedPath = xliff_Folder

        If xFolderDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            xliff_Folder = xFolderDialog.SelectedPath
            CmbLang.SelectedIndex = 0
            PopulateFiles()
            UpdateStatus()
        End If

    End Sub

    Private Sub UpdateStatus()
        If OutputList.Items.Count = 0 Then
            StatusBar.Text = "Current Folder - " & xliff_Folder & " | Status: No files found please select some other folder or language"
        Else
            StatusBar.Text = "Current Folder - " & xliff_Folder & " | Status: " & OutputList.Items.Count - 1 & " files found"
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
        Dim fDialog As New OpenFileDialog
        fDialog.Filter = "xliff  files (*.xliff)|*.xliff;"
        If fDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtInputXlifffile.Text = fDialog.FileName
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
        If CmbLang.SelectedIndex = 0 Then
            MsgBox("Please select language to copy xliffs!", MsgBoxStyle.Exclamation, "Cloud Translator!")
            Exit Sub
        End If

        If txtInputXlifffile.Text = "" Then
            MsgBox("No input file selected!", MsgBoxStyle.Exclamation, "Cloud Translator!")
            Exit Sub
        ElseIf Not System.IO.File.Exists(txtInputXlifffile.Text) Then
            MsgBox("Xliff input file cannot be accessed, Please check if the file exists in the folder!", MsgBoxStyle.Exclamation, "Cloud Translator!")
            Exit Sub
        End If

        If txtOutput.Text = "" Then
            MsgBox("No output path mentioned!", MsgBoxStyle.Exclamation, "Cloud Translator!")
            Exit Sub
        ElseIf Not System.IO.Directory.Exists(txtOutput.Text) Then
            MsgBox("output path doesn't exitst!", MsgBoxStyle.Exclamation, "Cloud Translator!")
            Exit Sub
        End If

        Try
            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor
            Pretranslate()
        Catch ex As Exception
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

    Private Sub Pretranslate()
    
        Try
            Dim inputxliffifle As String = txtInputXlifffile.Text
            If Microsoft.VisualBasic.Right(txtOutput.Text, 1) = "\" Then
                txtOutput.Text = txtOutput.Text.Substring(0, Len(txtOutput.Text) - 1)
            End If

            Dim outPutfile As String = txtOutput.Text & "\" & System.IO.Path.GetFileName(inputxliffifle)
            Dim translatedXliffFile As String = ""
            Dim lang As String = Mid(lang_to_langcode(CmbLang.Text), 1, 2) & "_" & Mid(lang_to_langcode(CmbLang.Text), 3, 2)

            For Each item As ListViewItem In Me.OutputList.Items
                item.Selected = True
                StatusBar.Text = "Status: Copying translation from " & System.IO.Path.GetFileNameWithoutExtension(translatedXliffFile)
                translatedXliffFile = item.SubItems.Item(1).Text
                xx = New xliff_To_xliff
                xx.CopyTranslation(inputxliffifle, translatedXliffFile, outPutfile)
                inputxliffifle = outPutfile
                StatusBar.Text = "Status: Total Match count " & xx.MatchCount & "\" & xx.TotalCount
                Application.DoEvents()
                If xx.MatchCount = xx.TotalCount Then
                    Exit For
                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

End Class