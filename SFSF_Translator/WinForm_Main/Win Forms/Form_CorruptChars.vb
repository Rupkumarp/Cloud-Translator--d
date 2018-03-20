Public Class Form_CorruptChars

    Public CorruptChars As ArrayList
    Public CorruptFileName As ArrayList

    Private MarkedCorrupt() As Boolean

    Private Sub Form_CorruptChars_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReDim MarkedCorrupt(CorruptFileName.Count - 1)
        populateData()
    End Sub

    Private index As Integer
    Private bNextChange As Boolean

    Private Sub BtnNext_Click(sender As Object, e As EventArgs) Handles BtnNext.Click
        Try
            bNextChange = True
            If CorruptChars Is Nothing Then
                Throw New Exception("No Data to Proceed")
            End If

            If CorruptChars.Count = 0 Then
                Throw New Exception("No Data to Proceed")
            End If

            BtnPrevious.Enabled = True

            If index + 1 < CorruptChars.Count Then
                index = index + 1
            Else
                BtnNext.Enabled = False
            End If

            populateData()

            bNextChange = False
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
       
    End Sub

    Private Sub BtnPrevious_Click(sender As Object, e As EventArgs) Handles BtnPrevious.Click
        Try
            bNextChange = True
            BtnNext.Enabled = True
          
            If CorruptChars Is Nothing Then
                Throw New Exception("No Data to Proceed")
            End If

            If CorruptChars.Count = 0 Then
                Throw New Exception("No Data to Proceed")
            End If

            If index > 0 Then
                index = index - 1
            Else
                BtnPrevious.Enabled = False
            End If

            populateData()

            bNextChange = False

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub populateData()
        RichTextBox1.Text = String.Empty
        RichTextBox1.Text = CorruptChars(index).ToString
        Me.Label5.Text = "Corrupt Chars - " & System.IO.Path.GetFileName(CorruptFileName(index))
        Me.Text = index + 1 & "\" & CorruptFileName.Count
        ToolStripStatusLabel1.Text = "Status:"
        If Microsoft.VisualBasic.Left(System.IO.Path.GetFileName(CorruptFileName(index)), 8).ToLower = "corrupt_" Then
            MarkedCorrupt(index) = True
        End If
        bAutoCheck = True
        Me.ChkCorrupt.Checked = MarkedCorrupt(index)
        bAutoCheck = False
        If Me.ChkCorrupt.Checked Then
            ToolStripStatusLabel1.Text = "Status: This file is marked as Corrupt"
        End If
    End Sub

    Private bAutoCheck As Boolean = False
    Private Sub ChkCorrupt_CheckedChanged(sender As Object, e As EventArgs) Handles ChkCorrupt.CheckedChanged

        If bAutoCheck Then
            Exit Sub
        End If

        Try
            Dim sInputFileName As String = System.IO.Path.GetFileName(CorruptFileName(index))
            Dim sRenameFileName As String = ""

            If Microsoft.VisualBasic.Left(sInputFileName, 8).ToLower = "corrupt_" Then
                sRenameFileName = System.IO.Path.GetFileName(CorruptFileName(index)).ToLower.Replace("corrupt_", "")
            Else
                sRenameFileName = "Corrupt_" & System.IO.Path.GetFileName(CorruptFileName(index))
            End If

            If ChkCorrupt.Checked Then
                If Microsoft.VisualBasic.Left(sInputFileName, 8).ToLower = "corrupt_" Then
                    Exit Sub
                End If
                My.Computer.FileSystem.RenameFile(CorruptFileName(index), sRenameFileName)
                ToolStripStatusLabel1.Text = "Status: This file is marked as Corrupt"
                If System.IO.File.Exists(System.IO.Path.GetDirectoryName(CorruptFileName(index)) & "\" & sRenameFileName) Then
                    CorruptFileName(index) = System.IO.Path.GetDirectoryName(CorruptFileName(index)) & "\" & sRenameFileName
                End If
                MarkedCorrupt(index) = True
            Else
                My.Computer.FileSystem.RenameFile(CorruptFileName(index), sRenameFileName)
                ToolStripStatusLabel1.Text = "Status: Renamed to original file"
                If System.IO.File.Exists(System.IO.Path.GetDirectoryName(CorruptFileName(index)) & "\" & sRenameFileName) Then
                    CorruptFileName(index) = System.IO.Path.GetDirectoryName(CorruptFileName(index)) & "\" & sRenameFileName
                End If

                MarkedCorrupt(index) = False
            End If

            Me.Label5.Text = "Corrupt Chars - " & sRenameFileName

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Diagnostics.Process.Start("explorer.exe", CorruptFileName(index))
    End Sub
End Class