Imports System.Windows.Forms

Public Class Form_EditProjectGroup

    Public Property ProjectGroupName As String

    Private Sub Form_EditProjectGroup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            LoadProjectGroups()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub


    Dim ProjectgroupNamelist As ArrayList

    Sub LoadProjectGroups()

        ProjectgroupNamelist = ProjectManagement.GetProjectGroupNameList

        If ProjectgroupNamelist.Count = 0 Then
            Throw New Exception("No Projects to load")
            Exit Sub
        End If

        For i As Integer = 0 To (ProjectgroupNamelist.Count - 1)
            LbProjectGroup.Items.Add(ProjectgroupNamelist(i))
        Next

        If ProjectGroupName <> String.Empty Then
            For i As Integer = 0 To LbProjectGroup.Items.Count - 1
                If LbProjectGroup.Items(i).ToString.ToLower = ProjectGroupName.ToLower Then
                    LbProjectGroup.SelectedIndex = i
                    Exit For
                End If
            Next
        End If

    End Sub

    Sub LoadProjects(ByVal ProjectGroupName As String)
        CmbProjectList.Items.Clear()
        Dim PName As ArrayList = ProjectManagement.GetProjectNameList(ProjectGroupName) ' GetProjectNameList(ProjectGroupName, LstProjectGroup)
        If PName.Count = 0 Then
            Exit Sub
        End If

        Dim iMaster As Integer = -1
        For i As Integer = 0 To PName.Count - 1
            CmbProjectList.Items.Add(PName(i))
            If ProjectManagement.isMasterProject(PName(i)) Then
                iMaster = i
            End If
        Next

        If iMaster >= 0 Then
            CmbProjectList.SelectedIndex = iMaster
        End If

    End Sub

    Private Sub LbProjectGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LbProjectGroup.SelectedIndexChanged
        LoadProjects(LbProjectGroup.Text)
        TextBox1.Text = LbProjectGroup.Text
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Try
            If LbProjectGroup.SelectedIndex = -1 Then
                Exit Sub
            End If

            If TextBox1.Text.Trim = String.Empty Then
                Throw New Exception("Enter Project group name!")
            End If

            For i As Integer = 0 To LbProjectGroup.Items.Count - 1
                If LbProjectGroup.SelectedIndex <> i Then
                    If TextBox1.Text.ToLower = LbProjectGroup.Items(i).ToString.ToLower Then
                        Throw New Exception("The Project Group is already available in the list!" & vbNewLine & "Please give some other name.")
                    End If
                End If
            Next

            updateLstProjectGroup(LbProjectGroup.Text, TextBox1.Text, CmbProjectList.Text, True)

            ProjectManagement.RenameProjectGroupName(LbProjectGroup.Text, TextBox1.Text)

            ProjectManagement.SaveAndReloadProject()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            Exit Sub
        End Try

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CmbProjectList_DropDown(sender As Object, e As EventArgs) Handles CmbProjectList.DropDown
        If CmbProjectList.Items.Count = 0 Then
            MsgBox("No Projects to load", MsgBoxStyle.Exclamation, "Empty Group")
        End If
    End Sub

    Private Sub updateLstProjectGroup(ByVal OldProjectGroupName As String, ByVal NewProjectGroupName As String, ByVal ProjectName As String, ByVal isMaster As Boolean)
        Dim PD As ProjectDetail
        For i As Integer = 0 To LstProjectGroup.Count - 1
            For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                PD = LstProjectGroup(i).ProjectDetail(j)
                With PD
                    .isCurrentProject = False
                    If .ProjectGroupName = OldProjectGroupName Then
                        .isMasterProject = False
                        .ProjectGroupName = NewProjectGroupName
                        If .ProjectName.ToLower = ProjectName.ToLower Then
                            .isMasterProject = True
                            .isCurrentProject = True
                        End If
                    End If
                End With
            Next
        Next
    End Sub


End Class
