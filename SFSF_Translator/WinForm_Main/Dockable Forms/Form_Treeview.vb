Public Class Form_Treeview


#Region "TreeView Events"
    Public Event Treeview_AfterSelect(ByRef Sender As Object, ByRef e As TreeViewEventArgs)
    Private Sub TV_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TV.AfterSelect
        RaiseEvent Treeview_AfterSelect(sender, e)
    End Sub

    Public Event Treeview_BeforeSelect(ByRef Sender As Object, ByRef e As TreeViewCancelEventArgs)
    Private Sub TV_BeforeSelect(sender As Object, e As TreeViewCancelEventArgs) Handles TV.BeforeSelect
        RaiseEvent Treeview_BeforeSelect(sender, e)
    End Sub

    Public Event Treeview_MyNodeSlected(ByRef NodeText As String)
    Private Sub TV_MyNodeSelected(NodeText As String) Handles TV.MyNodeSelected
        RaiseEvent Treeview_MyNodeSlected(NodeText)
    End Sub


    Private Sub TV_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TV.NodeMouseClick
        bCancelContextMenuStrip = False
        If e.Button = Windows.Forms.MouseButtons.Right Then
            TV.SelectedNode = e.Node
            bCancelContextMenuStrip = False
        End If
    End Sub

    Private bCancelContextMenuStrip As Boolean
    Private Sub TV_MouseUp(sender As Object, e As MouseEventArgs) Handles TV.MouseUp

        bCancelContextMenuStrip = False
        If e.Button = MouseButtons.Right Then
            ' Point where mouse is clicked
            Dim p As Point = New Point(e.X, e.Y)
            ' Go to the node that the user clicked
            Dim node As TreeNode = TV.GetNodeAt(p)

            If Not node Is Nothing Then
                Dim TT As TreeNodeTag = node.Tag

                Select Case TT.TI
                    Case TreeNodeTag.TagIndex.ProjectGroup
                        ContextMenu_ProjectGroup.Show(TV, New Point(e.X, e.Y))
                        Exit Sub

                    Case TreeNodeTag.TagIndex.Root
                        ContextMenu_Root.Show(TV, New Point(e.X, e.Y))
                        bCancelContextMenuStrip = True
                        Exit Sub

                    Case TreeNodeTag.TagIndex.ProjectName

                        ContextMenu_ProjectName.Show(TV, New Point(e.X, e.Y))
                        bCancelContextMenuStrip = True
                        Exit Sub
                End Select

            Else
                ContextMenu_Root.Close()
                ContextMenu_ProjectName.Close()
            End If
        End If

    End Sub


    Public Event Treeview_EditProjectDetail(ByRef sender As Object, ByRef e As EventArgs)
    Private Sub EditProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditProjectToolStripMenuItem.Click
        RaiseEvent Treeview_EditProjectDetail(sender, e)
    End Sub

    Public Event Treeview_DeleteFilesFromProject(ByRef sender As Object, ByRef e As EventArgs)
    Private Sub DeleteFilesOnlyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteFilesOnlyToolStripMenuItem.Click
        RaiseEvent Treeview_DeleteFilesFromProject(sender, e)
    End Sub

    Public Event Treeview_DeleteFilesAndProject(ByRef sender As Object, ByRef e As EventArgs)
    Private Sub DeleteFilesAndProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteFilesAndProjectToolStripMenuItem.Click
        RaiseEvent Treeview_DeleteFilesAndProject(sender, e)
    End Sub

    Public Event Treeview_ExportCTP(ByRef sender As Object, ByRef e As EventArgs)
    Private Sub ExportCTPFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportCTPFileToolStripMenuItem.Click
        RaiseEvent Treeview_ExportCTP(sender, e)
    End Sub

    Public Event Treeview_SetMasterProject(ByRef sender As Object, ByRef e As EventArgs)
    Private Sub Treeview_SetAsMasterProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetAsMasterProjectToolStripMenuItem.Click
        RaiseEvent Treeview_SetMasterProject(sender, e)
    End Sub

    Public Event Treeview_NewProjectGroup(ByRef sender As Object, ByRef e As EventArgs)
    Private Sub NewProjectGroupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewProjectGroupToolStripMenuItem.Click
        RaiseEvent Treeview_NewProjectGroup(sender, e)
    End Sub

    Public Event Treeview_ImportProjectGroup(ByRef sender As Object, ByRef e As EventArgs)
    Private Sub ImportProjectGroupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportProjectGroupToolStripMenuItem.Click
        RaiseEvent Treeview_ImportProjectGroup(sender, e)
    End Sub

    Public Event Treeview_Refresh(ByRef sender As Object, ByRef e As EventArgs)
    Private Sub RefreshToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RefreshToolStripMenuItem.Click
        RaiseEvent Treeview_Refresh(sender, e)
    End Sub
    Public Event Treeview_EditProjectGroup(ByRef sender As Object, ByRef e As EventArgs)
    Private Sub EditProjectGroupToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles EditProjectGroupToolStripMenuItem1.Click
        RaiseEvent Treeview_EditProjectGroup(sender, e)
    End Sub

    Public Event Treeview_NewProject(ByRef sender As Object, ByRef e As EventArgs)
    Private Sub NewProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewProjectToolStripMenuItem.Click
        RaiseEvent Treeview_NewProject(sender, e)
    End Sub

    Public Event Treeview_ImportCTP(ByRef sender As Object, ByRef e As EventArgs)
    Private Sub ExistingProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExistingProjectToolStripMenuItem.Click
        RaiseEvent Treeview_ImportCTP(sender, e)
    End Sub

    Public Event Treeview_DeleteProjectGroupOnly(ByRef sender As Object, ByRef e As EventArgs)
    Private Sub DelteProjectGroupOnlyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DelteProjectGroupOnlyToolStripMenuItem.Click
        RaiseEvent Treeview_DeleteProjectGroupOnly(sender, e)
    End Sub

    Public Event Treeview_DeleteProjectGroupAndAllProject(ByRef sender As Object, ByRef e As EventArgs)
    Private Sub DeleteProjectGroupAndProjectsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteProjectGroupAndProjectsToolStripMenuItem.Click
        RaiseEvent Treeview_DeleteProjectGroupAndAllProject(sender, e)
    End Sub

    Public Event Treeview_EditProjectGroupProperties(ByRef sender As Object, ByRef e As EventArgs)
    Private Sub EditProjectGroupPropertiesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditProjectGroupPropertiesToolStripMenuItem.Click
        RaiseEvent Treeview_EditProjectGroupProperties(sender, e)
    End Sub

    Public Event Treeview_ExportProject(ByRef sedner As Object, ByRef e As EventArgs)
    Private Sub ExportProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportProjectToolStripMenuItem.Click
        RaiseEvent Treeview_ExportProject(sender, e)
    End Sub

    Public Event Treeview_DeleteAllFilesExceptInputFiles(ByRef Sender As Object, ByRef e As EventArgs)
    Private Sub DeleteAllFilesExceptInputFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteAllFilesExceptInputFilesToolStripMenuItem.Click
        RaiseEvent Treeview_DeleteAllFilesExceptInputFiles(sender, e)
    End Sub

#End Region

    Public Event OpenProjectFolder()
    Private Sub OpenProjectFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenProjectFolderToolStripMenuItem.Click
        RaiseEvent OpenProjectFolder()
    End Sub

    Public Event SetMaster()
    Private Sub TV_SetMaster() Handles TV.SetMaster
        RaiseEvent SetMaster()
    End Sub

    Public Event UnSetMaster()
    Private Sub TV_UnSetMaster() Handles TV.UnSetMaster
        RaiseEvent UnSetMaster()
    End Sub
End Class

Public Class LoadTreeviewData
    Public Shared Sub Load_ProjectDetial(ByRef TV As TreeViewDraggableNodes)

        'Load Project list in treeview
        'Tag = 1 is Root
        'Tag = 2 is Project Group
        'Tag = 3 is Project Name

        TV.Nodes.Clear()

        LstProjectGroup = New List(Of ProjectGroup)
        LstProjectGroup = XMLMethod.GetProjectGroupListFromXml

        If LstProjectGroup.Count = 0 Then
            ProjectManagement.AddProjectGroupName("Default")
            ProjectManagement.AddProjectGroupName("Test")
            LoadTreeviewData.Load_ProjectDetial(TV)
            Exit Sub
        End If

        Dim TT As TreeNodeTag

        Try
            Dim TVCounter As Integer = 0
            If LstProjectGroup.Count = 0 Then
                TV.Nodes.Add("No Projects to Load")
                TV.ImageIndex = 10
                TV.SelectedImageIndex = 10
                TT = New TreeNodeTag
                TT.TI = TreeNodeTag.TagIndex.Root
                TT.isMaster = False
                TT.ImageIndex = 10
                TV.Nodes(0).Tag = TT
                Exit Sub
            End If

            TT = New TreeNodeTag
            TT.TI = TreeNodeTag.TagIndex.Root
            TT.isMaster = False
            TT.ImageIndex = 10

            TV.Nodes.Add("(Total " & LstProjectGroup.Count & " Project)")
            TV.Nodes(0).ImageIndex = TT.ImageIndex
            TV.Nodes(0).SelectedImageIndex = TT.ImageIndex
            TV.Nodes(0).BackColor = Color.NavajoWhite
            TV.Nodes(0).Tag = TT

            For i As Integer = 0 To (LstProjectGroup.Count - 1)
                TT = New TreeNodeTag
                TT.TI = TreeNodeTag.TagIndex.ProjectGroup
                TT.isMaster = False
                TT.ImageIndex = 0

                TV.Nodes(0).Nodes.Add(LstProjectGroup(i).ProjectGroupName)
                TV.Nodes(0).Nodes(TVCounter).ImageIndex = TT.ImageIndex
                TV.Nodes(0).Nodes(TVCounter).SelectedImageIndex = TT.ImageIndex
                TV.Nodes(0).Nodes(TVCounter).Tag = TT


                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                    TT = New TreeNodeTag
                    TT.TI = TreeNodeTag.TagIndex.ProjectName
                    TT.isMaster = False
                    TT.ImageIndex = 13
                    If LstProjectGroup(i).ProjectDetail(j).isMasterProject Then
                        TT.isMaster = True
                        TT.ImageIndex = TreeNodeTag.MasterImageIndex
                    End If

                    Dim s As String = LstProjectGroup(i).ProjectDetail(j).ProjectName

                    TV.Nodes(0).Nodes(TVCounter).Nodes.Add(s)
                    TV.Nodes(0).Nodes(TVCounter).Nodes(j).ImageIndex = TT.ImageIndex
                    TV.Nodes(0).Nodes(TVCounter).Nodes(j).SelectedImageIndex = TT.ImageIndex
                    TV.Nodes(0).Nodes(TVCounter).Nodes(j).Tag = TT

                    If LstProjectGroup(i).ProjectDetail(j).isCurrentProject Then
                        TV.Nodes(0).Nodes(TVCounter).Nodes(j).ImageIndex = 12
                        TV.Nodes(0).Nodes(TVCounter).Nodes(j).SelectedImageIndex = 12
                        Dim TN As TreeNode = TV.Nodes(0).Nodes(TVCounter).Nodes(j)
                        TV.SelectedNode = TN
                    End If
                Next
                TVCounter += 1
            Next i
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
End Class