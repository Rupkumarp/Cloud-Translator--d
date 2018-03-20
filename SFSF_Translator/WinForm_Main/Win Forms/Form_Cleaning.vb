Imports System.IO

Imports System.Environment

Public Class Form_Cleaning

    Dim projectfolder As String
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        'For Each file In System.IO.Directory.GetFiles("C:\Users\i054796\Desktop\CloudTranslator\RUN01_Final\02-TobeTranslated")
        'Dim tmp As String = System.IO.File.ReadAllText(file)
        'If InStr(tmp, "The proficiency levels ") <> 0 Then MsgBox(file)
        'Next
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        projectfolder = ProjectManagement.GetActiveProject.ProjectPath & "\"
        Initialize_listboxes()

    End Sub
    Sub Initialize_listboxes()
        extract_unique_files(projectfolder & "01-Input-B\", lstCleaned)
        extract_unique_files(projectfolder & "02-TobeTranslated\", LstTR)
        extract_unique_files(projectfolder & "03-Backfromtranslation\", lstTRback)
        extract_unique_files(projectfolder & "05-Output\", lstOut)
        extract_unique_files(projectfolder & "07-Pretranslate\", lstPre)
    End Sub

    Sub extract_unique_files(ByVal subfolder As String, ByRef mylistbox As ListBox)
        mylistbox.Items.Clear()
        For Each myfile In System.IO.Directory.GetFiles(subfolder)
            mylistbox.Items.Add(System.IO.Path.GetFileName(myfile))
        Next

        For Each mydir In System.IO.Directory.GetDirectories(subfolder)
            For Each myfile In System.IO.Directory.GetFiles(mydir)
                mylistbox.Items.Add(Mid(mydir, InStrRev(mydir, "\") + 1) & "\" & System.IO.Path.GetFileName(myfile))
            Next
        Next

    End Sub
    Function deletefi(ByVal myfilepath As String) As Boolean
        If Not (System.IO.File.Exists(myfilepath)) Then Return False

        Try
            System.IO.File.Delete(myfilepath)
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function
    Function rename_processed(ByVal myfilepath As String) As Boolean
        If Not (System.IO.File.Exists(myfilepath)) Then Return False

        Try
            System.IO.File.Copy(myfilepath, System.IO.Path.GetDirectoryName(myfilepath) & "\" & Replace(System.IO.Path.GetFileName(myfilepath), "(processed)", ""))
            System.IO.File.Delete(myfilepath)
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function
    Sub update_combo_lang(ByVal tabID As Byte)
        Dim mySelection As String
        mySelection = ComboBox2.Text
        ComboBox2.Items.Clear() : ComboBox2.Text = ""
        Dim mylistbox As ListBox = LstTR
        Select Case tabID
            Case 1
                mylistbox = LstTR
            Case 2
                mylistbox = lstTRback
            Case 3
                mylistbox = lstPre
        End Select

        Dim tmp_lang As String
        Dim tmp_all_lang() As String
        ReDim tmp_all_lang(0)
        tmp_all_lang(0) = "All"

        Dim fnd As Boolean

        For Each myitem In mylistbox.Items
            tmp_lang = Strings.Right(System.IO.Path.GetFileNameWithoutExtension(myitem), 5)
            fnd = False
            For Each existing_lang In tmp_all_lang
                If existing_lang = tmp_lang Then fnd = True : Exit For
            Next
            If fnd = False Then
                ReDim Preserve tmp_all_lang(UBound(tmp_all_lang) + 1)
                tmp_all_lang(UBound(tmp_all_lang)) = tmp_lang
            End If
        Next

        fnd = False
        Dim fndindex As Byte
        For Each found_lang In tmp_all_lang
            ComboBox2.Items.Add(found_lang)
            If mySelection = found_lang Then fnd = True : fndindex = ComboBox2.Items.Count - 1
        Next

        If fnd = False Then
            mySelection = "All"
            ComboBox2.SelectedIndex = 0
        Else
            mySelection = ComboBox2.Items(fndindex).ToString
            ComboBox2.SelectedIndex = fndindex
        End If

        Button9.Text = "Select " & mySelection

    End Sub

    Sub display_commands(ByVal tabID As Byte)


        Select Case tabID
            Case 0 'cleaned
                Button10.Enabled = False
                ComboBox2.Visible = False
                Button9.Text = "Select All"

            Case 1 'tr
                Button10.Enabled = False
                ComboBox2.Visible = True
                update_combo_lang(tabID)

            Case 2 'tr back
                Button10.Enabled = True
                ComboBox2.Visible = True
                update_combo_lang(tabID)

            Case 3 'pre
                Button10.Enabled = True
                ComboBox2.Visible = True
                update_combo_lang(tabID)

            Case 4 'out
                Button10.Enabled = False
                ComboBox2.Visible = False

        End Select

    End Sub

    Private Sub TabControl1_Click(sender As Object, e As EventArgs) Handles TabControl1.Click
        display_commands(TabControl1.SelectedIndex)

    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Button9.Text = "Select " & ComboBox2.Text
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click

        Dim mylistbox As ListBox = LstTR
        Select Case TabControl1.SelectedIndex
            Case 0
                mylistbox = lstCleaned
            Case 1
                mylistbox = LstTR
            Case 2
                mylistbox = lstTRback
            Case 3
                mylistbox = lstPre
            Case 4
                mylistbox = lstOut
        End Select

        Dim cnt As Integer = 0
        Dim cur_lang As String = ""

        If Button9.Text = "Select All" Then

            For cnt = 0 To mylistbox.Items.Count - 1
                mylistbox.SetSelected(cnt, True)
            Next


        Else
            cur_lang = ComboBox2.Text
            For cnt = 0 To mylistbox.Items.Count - 1
                If InStr(1, mylistbox.Items(cnt).ToString, cur_lang) <> 0 Then mylistbox.SetSelected(cnt, True)
            Next

        End If

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim mylistbox As ListBox = lstTRback
        Dim mypath As String = ""

        Select Case TabControl1.SelectedIndex

            Case 2
                mylistbox = lstTRback
                mypath = projectfolder & "03-Backfromtranslation\"
            Case 3
                mylistbox = lstPre
                mypath = projectfolder & "07-Pretranslate\"
        End Select

        Dim cnt As Integer = 0
        Dim cnt_done As Integer = 0
        Dim cnt_tot As Integer = 0
        Dim err_itm As String = ""
        For cnt = 0 To mylistbox.Items.Count - 1
            If mylistbox.GetSelected(cnt) And InStr(mylistbox.Items(cnt).ToString, "(processed") <> 0 Then
                cnt_tot = cnt_tot + 1
                If rename_processed(mypath & mylistbox.Items(cnt).ToString) Then cnt_done = cnt_done + 1 Else err_itm = err_itm & mylistbox.Items(cnt).ToString & vbCrLf
            End If
        Next

        Select Case TabControl1.SelectedIndex
            Case 2
                extract_unique_files(projectfolder & "03-Backfromtranslation\", lstTRback)
            Case 3
                extract_unique_files(projectfolder & "07-Pretranslate\", lstPre)
        End Select

        If cnt_tot <> cnt_done Then
            MsgBox(cnt_done & "/" & cnt_tot & " files renamed." & vbCrLf & "Errors for files: " & vbCrLf & err_itm)
        Else
            MsgBox(cnt_done & " renamed files. No errors", MsgBoxStyle.Information)
        End If


    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim mylistbox As ListBox = lstTRback
        Dim mypath As String = ""

        Select Case TabControl1.SelectedIndex

            Case 2
                mylistbox = lstTRback
                mypath = projectfolder & "03-Backfromtranslation\"
            Case 3
                mylistbox = lstPre
                mypath = projectfolder & "07-Pretranslate\"

            Case 0
                mylistbox = lstCleaned
                mypath = projectfolder & "01-Input-B\"
            Case 1
                mylistbox = LstTR
                mypath = projectfolder & "02-TobeTranslated\"
            Case 4
                mylistbox = lstOut
                mypath = projectfolder & "05-Output\"

        End Select
        Dim cnt As Integer = 0
        Dim cnt_done As Integer = 0
        Dim cnt_tot As Integer = 0
        Dim err_itm As String = ""
        For cnt = 0 To mylistbox.Items.Count - 1
            If mylistbox.GetSelected(cnt) Then
                cnt_tot = cnt_tot + 1
                If deletefi(mypath & mylistbox.Items(cnt).ToString) Then cnt_done = cnt_done + 1 Else err_itm = err_itm & mylistbox.Items(cnt).ToString & vbCrLf
            End If
        Next

        Select Case TabControl1.SelectedIndex
            Case 2
                extract_unique_files(projectfolder & "03-Backfromtranslation\", lstTRback)
            Case 3
                extract_unique_files(projectfolder & "07-Pretranslate\", lstPre)
            Case 0
                extract_unique_files(projectfolder & "01-Input-B\", lstCleaned)
            Case 1
                extract_unique_files(projectfolder & "02-TobeTranslated\", LstTR)
            Case 4
                extract_unique_files(projectfolder & "05-Output\", lstOut)

        End Select

        If cnt_tot <> cnt_done Then
            MsgBox(cnt_done & "/" & cnt_tot & " files deleted." & vbCrLf & "Errors for files: " & vbCrLf & err_itm)
        Else
            MsgBox(cnt_done & " deleted files. No errors", MsgBoxStyle.Information)
        End If
    End Sub
End Class
