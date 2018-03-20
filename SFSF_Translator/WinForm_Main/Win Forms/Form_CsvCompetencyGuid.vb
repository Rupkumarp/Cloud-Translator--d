'. It adds GUID to csv comp files

Public Class Form_CsvCompetencyGuid

    Dim stored_guid() As String
    Dim stored_competency() As String

    Public Property CurrentDirectory As String

    Private Sub BtnAddGuid_Click(sender As Object, e As EventArgs) Handles BtnAddGuid.Click

        '<Validation>-------------------------------------------------------------------------------------------------------------------------------
        If TxtCsvFile.Text.Trim = "" Then
            MsgBox("No csv file selected!", MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        ElseIf Not System.IO.File.Exists(TxtCsvFile.Text) Then
            MsgBox("File not found!" & vbCrLf & TxtCsvFile.Text, MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        End If

        If TextBox1.Text.Trim.Length <> 5 And Not TextBox1.Text.Contains("_") Then
            MsgBox("Please check the lang code!" & vbNewLine & "Format: " & Chr(34) & "en_US or fr_FR" & Chr(34), MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        End If

        If (TextBox2.Text.Trim = "" Or TextBox5.Text.Trim = "") Then
            MsgBox("Please enter values for Str ID comp and Str ID behave!" & vbNewLine & "Format: " & Chr(34) & "en_US or fr_FR" & Chr(34), MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        ElseIf Not IsNumeric(TextBox2.Text.Trim) Or Not IsNumeric(TextBox5.Text.Trim) Then
            MsgBox("Str ID comp and Str ID behave should be numbers, Please check!" & vbNewLine & "Format: " & Chr(34) & "en_US or fr_FR" & Chr(34), MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        End If
        '</Validation>-------------------------------------------------------------------------------------------------------------------------------

        Try
            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor

            ToolStripStatusLabel1.Text = "Status: Initiated adding Guid."

            Dim mode_work As Byte = 0 '0 - new file; 1 will be with a matching table
            Dim myfi As String = System.IO.File.ReadAllText(TxtCsvFile.Text, System.Text.Encoding.UTF8)

            Dim outputfile As String = CurrentDirectory & "\08–Output_csv\" & System.IO.Path.GetFileNameWithoutExtension(TxtCsvFile.Text) & "_withGUID" & System.IO.Path.GetExtension(TxtCsvFile.Text)
            If System.IO.File.Exists(outputfile) Then
                If MsgBox("output file exists - do you want to overwrite?", vbYesNo) = MsgBoxResult.Yes Then
                    System.IO.File.Delete(outputfile)
                Else
                    MsgBox("exiting", MsgBoxStyle.Critical) : Exit Sub
                End If
            End If

            Dim dquote As String
            If Strings.Left(myfi, 12) = Chr(34) & "COMPETENCY" & Chr(34) Then
                dquote = Chr(34)
            ElseIf Strings.Left(myfi, 10) = "COMPETENCY" Then
                dquote = ""
            Else
                MsgBox("Error in file format - Should start with a Competency." & vbCrLf & "Exiting now", MsgBoxStyle.Critical)
                Exit Sub
            End If

            ReDim stored_guid(0)
            ReDim stored_competency(0)

            Dim tofind As String = ",," & dquote & TextBox1.Text & dquote
            Dim myguid As Integer = Val(TextBox2.Text) - 1
            Dim myguidbeh As Integer = Val(TextBox5.Text) - 1
            Dim line_split() As String = Split(myfi, vbCrLf)
            'Dim segsplit() As String
            If UBound(line_split) = 0 Then line_split = System.Text.RegularExpressions.Regex.Split(myfi, "\r")
            If UBound(line_split) = 0 Then line_split = System.Text.RegularExpressions.Regex.Split(myfi, "\n")
            Dim f As Integer

            ToolStripProgressBar1.Maximum = UBound(line_split)

            For f = 0 To UBound(line_split)
                If line_split(f) = "" Then Exit For
                Select Case Strings.Left(line_split(f), InStr(line_split(f), ",") - 1)
                    Case dquote & "COMPETENCY" & dquote
                        If mode_work = 0 Then myguid = myguid + 1
                        line_split(f) = Replace(line_split(f), tofind, "," & dquote & myguid & dquote & "," & dquote & TextBox1.Text & dquote, 1, 1)
                        'segsplit = Split(line_split(f), ",")
                        'add_seg(segsplit(2), dquote & myguid & dquote)
                    Case dquote & "BEHAVIOR" & dquote
                        myguidbeh = myguidbeh + 1
                        'segsplit = Split(line_split(f), ",")
                        'myguid = get_guid(segsplit(2))
                        line_split(f) = Replace(line_split(f), tofind, "," & dquote & myguidbeh & dquote & "," & dquote & TextBox1.Text & dquote, 1, 1)
                    Case Else
                        'MsgBox(Strings.Left(line_split(f), InStr(line_split(f), ",") - 1))
                End Select
                ToolStripProgressBar1.Value = f
            Next

            System.IO.File.WriteAllText(outputfile, Join(line_split, vbCrLf), System.Text.Encoding.UTF8)

            ToolStripStatusLabel1.Text = "File saved: " & outputfile

            MsgBox("Adding Guid Completed!", MsgBoxStyle.Information, "Cloud translator")

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Cloud translator")
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try


    End Sub
    Private Sub add_seg(ByVal new_seg As String, ByVal myguid As String)
        ReDim Preserve stored_guid(UBound(stored_guid) + 1)
        stored_guid(UBound(stored_guid)) = Str(myguid)
        ReDim Preserve stored_competency(UBound(stored_guid) + 1)
        stored_competency(UBound(stored_guid)) = new_seg
    End Sub

    Private Function get_guid(ByVal myseg As String) As String
        Try
            For f = 0 To UBound(stored_competency)
                If stored_competency(f) = myseg Then Return stored_guid(f)
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If System.IO.Directory.Exists(CurrentDirectory & "\05–Standard_csv\") Then
            OpenFileDialog1.InitialDirectory = CurrentDirectory & "\05–Standard_csv\"
        End If
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TxtCsvFile.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CurrentDirectory = CurrentDirectory & "Competencies"
        TxtCsvFile.Text = CurrentDirectory & "\05-Standard_csv\SF_premium_competency_library2.1_enUS.csv"
        TxtMatchfile.Text = CurrentDirectory & "\07-Match_files\match_fr.txt"

        If Not (System.IO.Directory.Exists(CurrentDirectory)) Or
           Not (System.IO.Directory.Exists(CurrentDirectory & "\05-Standard_csv\")) Or
           Not (System.IO.Directory.Exists(CurrentDirectory & "\07-Match_files\")) Then MsgBox("Incorrect Competency folder structure in the selected project. Exiting now.", MsgBoxStyle.Critical, "Cloud translator") : Me.Close()

        ReDim Preserve stored_guid(0)
        ReDim Preserve stored_competency(0)
    End Sub


    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles BtnReplaceGuid.Click
        '<Validation>-------------------------------------------------------------------------------------------------------------------------------
        If TxtCsvFile.Text.Trim = "" Then
            MsgBox("No csv file selected!", MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        ElseIf System.IO.File.Exists(TxtCsvFile.Text) Then
            MsgBox("File not found!" & vbCrLf & TxtCsvFile.Text, MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        End If

        If TxtMatchfile.Text.Trim = "" Then
            MsgBox("No csv file selected!", MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        ElseIf System.IO.File.Exists(TxtMatchfile.Text) Then
            MsgBox("File not found!" & vbCrLf & TxtMatchfile.Text, MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        End If
        '</Validation>-------------------------------------------------------------------------------------------------------------------------------

        Try
            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor

            'needs much more robustness here - code written in 3min !!!!
            Dim myfi As String = System.IO.File.ReadAllText(TxtCsvFile.Text)
            Dim tmp_matches As String = System.IO.File.ReadAllText(TxtMatchfile.Text)
            Dim tmp_matches_l() As String = Split(tmp_matches, vbCrLf)
            Dim dquote As String
            Dim findnbr As String
            Dim repnbr As String

            If Strings.Left(myfi, 12) = Chr(34) & "COMPETENCY" & Chr(34) Then
                dquote = Chr(34)
            ElseIf Strings.Left(myfi, 10) = "COMPETENCY" Then
                dquote = ""
            Else
                MsgBox("Error in file format - Should start with a Competency." & vbCrLf & "Exiting now", MsgBoxStyle.Critical)
                Exit Sub
            End If

            ToolStripProgressBar1.Maximum = UBound(tmp_matches_l)

            For f = 0 To UBound(tmp_matches_l)
                findnbr = Mid(tmp_matches_l(f), 1, InStr(tmp_matches_l(f), "|") - 1)
                repnbr = Mid(tmp_matches_l(f), InStr(tmp_matches_l(f), "|") + 1)
                myfi = Replace(myfi, dquote & findnbr & dquote & "," & dquote & TextBox1.Text & dquote, dquote & repnbr & dquote & "," & dquote & TextBox1.Text & dquote)
                ToolStripProgressBar1.Value = f
            Next

            Dim outputfile As String = CurrentDirectory & "\08–Output_csv\" & System.IO.Path.GetFileNameWithoutExtension(TxtCsvFile.Text) & "_repGUID" & System.IO.Path.GetExtension(TxtCsvFile.Text)

            System.IO.File.WriteAllText(outputfile, myfi, System.Text.Encoding.UTF8)

            ToolStripStatusLabel1.Text = "File saved: " & outputfile

            MsgBox("Replacing Guid Completed!", MsgBoxStyle.Information, "Cloud translator")

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Cloud translator")
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If System.IO.Directory.Exists(CurrentDirectory & "\07–Match_files\") Then
            OpenFileDialog1.InitialDirectory = CurrentDirectory & "\07–Match_files\"
        End If
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TxtMatchfile.Text = OpenFileDialog1.FileName
        End If
    End Sub

End Class