Imports System.Windows.Forms
Imports Ionic.Zip

Public Class Form_ImportCtproj

    Private Sub BtnBrowseProjectLocation_Click(sender As Object, e As EventArgs) Handles BtnBrowseCtproj.Click
        Dim OD As New OpenFileDialog
        OD.Filter = "Cloud translator project *.ctproj|*.ctproj"
        If OD.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox_Ctproj.Text = OD.FileName
            ValidateCTPfile(TextBox_Ctproj.Text)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim FD As New FolderBrowserDialog
        FD.Description = "Select project location"
        If FD.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox_ImportLocation.Text = FD.SelectedPath & "\"
        End If
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If TextBox_Ctproj.Text = String.Empty Then
            MsgBox("Ctproj file not selected!", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        If Not System.IO.File.Exists(TextBox_Ctproj.Text) Then
            MsgBox("Ctproj file not found!", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        If TextBox_ImportLocation.Text = String.Empty Then
            MsgBox("Import Location not set!", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        If Not System.IO.Directory.Exists(TextBox_ImportLocation.Text) Then
            MsgBox("Import Location not found!", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        Dim ProjectGroupName As String = TextBox_ProjectGroupName.Text

        Try
            'First Check if the project can be extracted in the Import Location. We cannot replace the same Project as it might be referenced in other Project group
            If isExtractionCannotbeDone() Then
                MsgBox("The Import location already has the Project with same name!" & vbNewLine & "Please Select some other location.", MsgBoxStyle.Critical, "Cannot Import")
                Exit Sub
            End If

            'If same ProjectGroupName exists then append number
            If ProjectManagement.isProjectGroupNameAvailable(PG.ProjectGroupName) Then
                If MsgBox("Project Group Name already exists." & vbNewLine & "Tool will append number.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Duplicate Project Name") Then
                    Dim counter As Integer = 1
                    Do Until ProjectManagement.isProjectGroupNameAvailable(ProjectGroupName & "_" & counter) = False
                        counter += 1
                    Loop
                    ProjectGroupName = ProjectGroupName & "_" & counter
                End If
            End If

            'Extract the Ctproj file to Import Location
            Using zip As ZipFile = ZipFile.Read(TextBox_Ctproj.Text)
                Dim zipel As ZipEntry
                For Each zipel In zip
                    If InStr(zipel.FileName, "Projects.ini") = 0 Then 'Do not extract Projects.ini file
                        zipel.Extract(TextBox_ImportLocation.Text, ExtractExistingFileAction.OverwriteSilently)
                    End If
                Next
            End Using

            'Now Update the list of Projecct object, Rename ProjectName if it exists.
            PG.ProjectGroupName = ProjectGroupName
            For i As Integer = 0 To PG.ProjectDetail.Count - 1
                PG.ProjectDetail(i).ProjectGroupName = ProjectGroupName
                'Check ProjectName, if Duplicate then add number enum 
                Dim ProjectName As String = PG.ProjectDetail(i).ProjectName
                Dim OriginalProjectName As String = PG.ProjectDetail(i).ProjectName
                If ProjectManagement.isProjectNameAvailable(ProjectName) Then
                    Dim counter As Integer = 1
                    Do Until ProjectManagement.isProjectNameAvailable(ProjectName & "_" & counter) = False
                        counter += 1
                    Loop
                    ProjectName = ProjectName & "_" & counter
                End If
                PG.ProjectDetail(i).ProjectPath = TextBox_ImportLocation.Text & ProjectName
                PG.ProjectDetail(i).ProjectName = ProjectName
                If CheckCustomerInstance(PG.ProjectDetail(i).CustomerName) Or CheckCustomerInstance(PG.ProjectDetail(i).InstaneName) Then
                    Dim f As New Form_CustomerInstance
                    f.Text = "Missing Customer and Instance"
                    f.LblTitle.Text = "Project Name - " & PG.ProjectDetail(i).ProjectName
                    If f.ShowDialog = DialogResult.OK Then
                        PG.ProjectDetail(i).CustomerName = f.CmbCustomer.Text
                        PG.ProjectDetail(i).InstaneName = f.CmbInstance.Text
                    End If
                End If
                If Not ProjectManagement.CompareTwoStrings(OriginalProjectName, ProjectName) Then
                    My.Computer.FileSystem.RenameDirectory(TextBox_ImportLocation.Text & OriginalProjectName, ProjectName)
                End If
            Next

            LstProjectGroup.Add(PG)

            ProjectManagement.SaveAndReloadProject()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            Exit Sub
        End Try

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Function isExtractionCannotbeDone() As Boolean

        For i As Integer = 0 To PG.ProjectDetail.Count - 1
            Using zip As ZipFile = ZipFile.Read(TextBox_Ctproj.Text)
                Dim zipel As ZipEntry
                For Each zipel In zip
                    If InStr(zipel.FileName, "/") Then
                        Dim str() As String = Split(zipel.FileName, "/")
                        If str(0).ToLower = PG.ProjectDetail(i).ProjectName.ToLower Then
                            If System.IO.Directory.Exists(TextBox_ImportLocation.Text & str(0)) Then
                                Return True
                            End If
                            Exit For
                        End If
                    End If
                Next
            End Using
        Next

        Return False
    End Function

    Private Function isFolder(ByVal sPath As String) As Boolean
        Dim attr As FileAttribute = System.IO.File.GetAttributes(sPath)
        If attr.HasFlag(FileAttribute.Directory) Then
            Return True
        End If
        Return False
    End Function

    Private Function CheckCustomerInstance(ByVal CI As Object) As Boolean
        If IsNothing(CI) Then
            Return True
        ElseIf CI.ToString.Trim = String.Empty Then
            Return True
        End If
        Return False
    End Function

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Dim PG As ProjectGroup

    Private Sub ValidateCTPfile(ByVal CtprojFile As String)
        Try
            If System.IO.File.Exists(Application.StartupPath & "\projects.ini") Then
                System.IO.File.Delete(Application.StartupPath & "\projects.ini")
            End If

            Using zip As ZipFile = ZipFile.Read(CtprojFile)
                Dim zipel As ZipEntry
                For Each zipel In zip
                    If zipel.FileName.ToLower = "projects.ini" Then
                        zipel.Extract(Application.StartupPath, ExtractExistingFileAction.OverwriteSilently)
                        Exit For
                    End If
                Next
            End Using

            Dim SettingFile As String = Application.StartupPath & "\projects.ini"

            If Not System.IO.File.Exists(SettingFile) Then
                Throw New Exception("Error Could not find Project.ini file in " & CtprojFile)
            End If

            PG = XMLMethod.GetProjectGroupDetailFromCtproj(SettingFile)

            TextBox_ProjectGroupName.Text = PG.ProjectGroupName

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub


End Class
