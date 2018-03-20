Imports System.Windows.Forms

Public Class Form_DBCorrection

    Private EP As New ErrorProvider

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Form_DBCorrection_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        EnableDisableControls(False)
        LoadLangList()
    End Sub

    Private Sub LoadLangList()
        CmbLanglist.Items.Clear()
        CmbLanglist.Items.Add("[SELECT LANGUAGE]")
        If Not (System.IO.File.Exists(appData & DefinitionFiles.Lang_List)) Then MsgBox("File Language.txt doesn't exist. Critical error!", MsgBoxStyle.Critical, "Search & Correct")
        For Each lang In Split(System.IO.File.ReadAllText(appData & DefinitionFiles.Lang_List), vbCrLf)
            CmbLanglist.Items.Add(Mid(lang, 1, InStr(lang, Chr(9)) - 1))
        Next
        CmbLanglist.SelectedIndex = 0
    End Sub


    Dim CT() As Cloud_TR.CloudTr

    Dim SF As Cloud_TR.SearchingField = Cloud_TR.SearchingField.Target

    Private Sub BtnSearchDB_Click(sender As Object, e As EventArgs) Handles BtnSearchDB.Click
        'Validate
        If CmbLanglist.SelectedIndex = 0 Then
            MsgBox("Lanuage not selected!", MsgBoxStyle.Critical, "Error!")
            Set_EP(CmbLanglist, "Select Language")
            Exit Sub
        End If

        If Rtb_Search.Text.Trim = String.Empty Then
            MsgBox("Input text to search", MsgBoxStyle.Critical, "Error!")
            Set_EP(Rtb_Search, "Input Text")
            Exit Sub
        End If

        Dim Lang As String = lang_to_langcode(CmbLanglist.Text)

        Try
            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor
            ToolStripProgressBar1.Maximum = 100
            ToolStripProgressBar1.Value = 0

            Dim objCT As New CloudWebServiceNew
            CT = objCT.SearchCloud(Rtb_Search.Text, chkFullStringOnly.CheckState, Lang, SF)
            If CT.Length = 0 Then
                DV.Rows.Clear()
                ToolStripStatusLabel1.Text = "0 items found!"
                MsgBox("0 items found!", MsgBoxStyle.Information, "Result")
                EnableDisableControls(False)
                Exit Sub
            End If
            LoadDV()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub EnableDisableControls(ByVal Enable As Boolean)
        TableLayoutPanel1.Enabled = Enable
        GroupBox2.Enabled = Enable
    End Sub

    Private Sub Set_EP(ByRef cntl As Control, ByVal prompt As String)
        EP.SetError(cntl, "Select Language")
        EP.BlinkRate = 500
        EP.BlinkStyle = ErrorBlinkStyle.AlwaysBlink
        EP.SetIconAlignment(cntl, ErrorIconAlignment.TopLeft)
        EP.SetIconPadding(cntl, 1)
        cntl.Focus()
    End Sub

    Private Sub CmbLanglist_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbLanglist.SelectedIndexChanged
        If CmbLanglist.SelectedIndex <> 0 Then
            EP.Clear()
        End If
    End Sub

    Private Sub TextBox_Search_TextChanged(sender As Object, e As EventArgs) Handles Rtb_Search.TextChanged
        If Rtb_Search.Text.Trim.Length > 0 Then
            EP.Clear()
        End If
    End Sub

    Private Sub RB_SearchTarget_CheckedChanged(sender As Object, e As EventArgs) Handles RB_SearchTarget.CheckedChanged
        SF = Cloud_TR.SearchingField.Target
    End Sub

    Private Sub RB_SearchSource_CheckedChanged(sender As Object, e As EventArgs) Handles RB_SearchSource.CheckedChanged
        SF = Cloud_TR.SearchingField.Source
    End Sub

    Private Sub RB_Both_CheckedChanged(sender As Object, e As EventArgs) Handles RB_Both.CheckedChanged
        SF = Cloud_TR.SearchingField.Both
    End Sub

    Private Sub LoadDV()
        DV.Rows.Clear()
        ToolStripStatusLabel1.Text = CT.Length & " - Records found."
        For i As Integer = 0 To UBound(CT)
            With CT(i)
                DV.Rows.Add(.extID, True, .Source, .Target, .SourceLang, .TargetLang, .Customer, .Instance, .Datatype, .Resname, .Maxlength, .CustomerSpecific)
                DV.PerformLayout()
            End With
        Next
        EnableDisableControls(True)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Rtb_Search.Clear()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Rtb_Replace.Clear()
    End Sub

    Private Sub BtnSelectAll_Click(sender As Object, e As EventArgs) Handles BtnSelectAll.Click
        If DV.Rows.Count > 0 Then
            If DV.AreAllCellsSelected(True) Then
                DV.ClearSelection()
            Else
                DV.SelectAll()
            End If
        End If
    End Sub

    Private Sub BtnMarkAll_Click(sender As Object, e As EventArgs) Handles BtnMarkAll.Click
        For i As Integer = 0 To DV.Rows.Count - 1
            If DV.Rows(i).Selected Then
                If DV.Rows(i).Cells(1).Value = True Then
                    DV.Rows(i).Cells(1).Value = False
                Else
                    DV.Rows(i).Cells(1).Value = True
                End If
            End If
        Next
    End Sub

    Private Sub BtnReload_Click(sender As Object, e As EventArgs) Handles BtnReload.Click
        If Not CT Is Nothing Then
            LoadDV()
        End If

    End Sub

    Private Sub BtnReplace_Click(sender As Object, e As EventArgs) Handles BtnReplace.Click

        If Rtb_Replace.Text.Trim = String.Empty Then
            MsgBox("Input replace text!", MsgBoxStyle.Critical, "Error!")
            Set_EP(Rtb_Replace, "Input replace text!")
            Exit Sub
        End If

        For i As Integer = 0 To DV.Rows.Count - 1
            If DV.Rows(i).Cells(1).Value = True Then
                If RB_ReplaceSource.Checked Then
                    DV.Rows(i).Cells(2).Value = Rtb_Replace.Text
                ElseIf RB_ReplaceTarget.Checked Then
                    DV.Rows(i).Cells(3).Value = Rtb_Replace.Text
                End If
            End If
        Next

    End Sub

    Private Sub Rtb_Replace_TextChanged(sender As Object, e As EventArgs) Handles Rtb_Replace.TextChanged
        If Rtb_Replace.Text.Trim.Length > 0 Then
            EP.Clear()
        End If
    End Sub

    Private Sub BtnPasteSearch_Click(sender As Object, e As EventArgs) Handles BtnPasteSearch.Click
        Rtb_Search.Text = Clipboard.GetText
    End Sub

    Private Sub BtnPasteReplace_Click(sender As Object, e As EventArgs) Handles BtnPasteReplace.Click
        Rtb_Replace.Text = Clipboard.GetText
    End Sub

    Private Sub BtnUpdateDB_Click(sender As Object, e As EventArgs) Handles BtnUpdateDB.Click
        'Validation
        If DV.Rows.Count = 0 Then
            MsgBox("Grid is empty!" & vbNewLine & "Nothing to update.", MsgBoxStyle.Information, "Update Exiting.")
            Exit Sub
        End If

        'Prompt user
        If MsgBox("Are you sure you want to make corrections to DB.", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Update?") = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim objCT As CloudWebServiceNew
        Dim iProgress As Integer = 0
        Dim LastRow As Integer = DV.Rows.Count - 1
        Try

            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor
            ToolStripProgressBar1.Maximum = LastRow
            For i As Integer = 0 To LastRow
                If DV.Rows(i).Cells(1).Value = True Then
                    DV.Rows(i).Selected = True
                    objCT = New CloudWebServiceNew
                    iProgress += objCT.Update_DBCorrections(DV.Rows(i).Cells(0).Value, DV.Rows(i).Cells(2).Value, DV.Rows(i).Cells(3).Value)
                End If
                ToolStripProgressBar1.Value = i
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try

        ToolStripStatusLabel1.Text = "Updated: " & iProgress & " records"
    End Sub

End Class
