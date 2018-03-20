Imports System.Windows.Forms

Public Class dia_projectlist

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If ListBox1.Items.Count = 0 Then Me.Close()
        If ListBox1.SelectedItems.Count = 0 Then MsgBox("One project needs to be selected", MsgBoxStyle.Exclamation) : Exit Sub

        ProjectManagement.SetActiveProject(ListBox1.Text)

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dia_projectlist_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListBox1.Items.Clear()
        Dim projectlist As ArrayList = ProjectManagement.GetProjectNameList
        If projectlist.Count = 0 Then
            MsgBox("There are no active projects to select", MsgBoxStyle.Information, "Cloud translator")
            Me.Close()
            Exit Sub
        End If

        Dim curproj As String = ProjectManagement.GetActiveProject.ProjectPath

        For i As Integer = 0 To projectlist.Count - 1
            ListBox1.Items.Add(projectlist(i))
            If curproj = projectlist(i) Then ListBox1.SetSelected(ListBox1.Items.Count - 1, True)
        Next

    End Sub


End Class
