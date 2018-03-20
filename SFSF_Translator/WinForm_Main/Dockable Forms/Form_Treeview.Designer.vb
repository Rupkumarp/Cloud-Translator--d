<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_Treeview
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_Treeview))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ContextMenu_Root = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewProjectGroupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportProjectGroupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditProjectGroupToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.RefreshToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenu_ProjectGroup = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExistingProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DelteProjectGroupOnlyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteProjectGroupAndProjectsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditProjectGroupPropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenu_ProjectName = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EditProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteFilesOnlyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteFilesAndProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportCTPFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetAsMasterProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenProjectFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TV = New CloudTranslator.TreeViewDraggableNodes()
        Me.DeleteAllFilesExceptInputFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenu_Root.SuspendLayout()
        Me.ContextMenu_ProjectGroup.SuspendLayout()
        Me.ContextMenu_ProjectName.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Config.png")
        Me.ImageList1.Images.SetKeyName(1, "Process3.ico")
        Me.ImageList1.Images.SetKeyName(2, "Criterion1.ico")
        Me.ImageList1.Images.SetKeyName(3, "Action.ico")
        Me.ImageList1.Images.SetKeyName(4, "DataElement2.ico")
        Me.ImageList1.Images.SetKeyName(5, "Criterioin2.ico")
        Me.ImageList1.Images.SetKeyName(6, "DataElementFormula.ico")
        Me.ImageList1.Images.SetKeyName(7, "DataElementExcelCell.ico")
        Me.ImageList1.Images.SetKeyName(8, "DataElementConcatenate.ico")
        Me.ImageList1.Images.SetKeyName(9, "DataElementUserInput.ico")
        Me.ImageList1.Images.SetKeyName(10, "DataElementBuiltInfunction.ico")
        Me.ImageList1.Images.SetKeyName(11, "DataElementConstant.ico")
        Me.ImageList1.Images.SetKeyName(12, "PActive.ico")
        Me.ImageList1.Images.SetKeyName(13, "PInactive.ico")
        Me.ImageList1.Images.SetKeyName(14, "AActive.ico")
        Me.ImageList1.Images.SetKeyName(15, "AInactive.ico")
        Me.ImageList1.Images.SetKeyName(16, "ConditionalActive.ico")
        Me.ImageList1.Images.SetKeyName(17, "ConditionalInactive.ico")
        Me.ImageList1.Images.SetKeyName(18, "MP4.ico")
        '
        'ContextMenu_Root
        '
        Me.ContextMenu_Root.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewProjectGroupToolStripMenuItem, Me.ImportProjectGroupToolStripMenuItem, Me.EditProjectGroupToolStripMenuItem1, Me.RefreshToolStripMenuItem})
        Me.ContextMenu_Root.Name = "ContextMenuTreeNode2"
        Me.ContextMenu_Root.Size = New System.Drawing.Size(232, 92)
        '
        'NewProjectGroupToolStripMenuItem
        '
        Me.NewProjectGroupToolStripMenuItem.Name = "NewProjectGroupToolStripMenuItem"
        Me.NewProjectGroupToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.NewProjectGroupToolStripMenuItem.Text = "New Project Group"
        '
        'ImportProjectGroupToolStripMenuItem
        '
        Me.ImportProjectGroupToolStripMenuItem.Image = Global.CloudTranslator.My.Resources.Resources.import1
        Me.ImportProjectGroupToolStripMenuItem.Name = "ImportProjectGroupToolStripMenuItem"
        Me.ImportProjectGroupToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.ImportProjectGroupToolStripMenuItem.Text = "Import Project Group (.ctproj)"
        '
        'EditProjectGroupToolStripMenuItem1
        '
        Me.EditProjectGroupToolStripMenuItem1.Image = Global.CloudTranslator.My.Resources.Resources.export_3
        Me.EditProjectGroupToolStripMenuItem1.Name = "EditProjectGroupToolStripMenuItem1"
        Me.EditProjectGroupToolStripMenuItem1.Size = New System.Drawing.Size(231, 22)
        Me.EditProjectGroupToolStripMenuItem1.Text = "Edit Project Group"
        '
        'RefreshToolStripMenuItem
        '
        Me.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem"
        Me.RefreshToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.RefreshToolStripMenuItem.Text = "Refresh list"
        '
        'ContextMenu_ProjectGroup
        '
        Me.ContextMenu_ProjectGroup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddToolStripMenuItem, Me.DeleteToolStripMenuItem, Me.EditProjectGroupPropertiesToolStripMenuItem, Me.ExportProjectToolStripMenuItem})
        Me.ContextMenu_ProjectGroup.Name = "ContextMenuTreeNode3"
        Me.ContextMenu_ProjectGroup.Size = New System.Drawing.Size(227, 92)
        '
        'AddToolStripMenuItem
        '
        Me.AddToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewProjectToolStripMenuItem, Me.ExistingProjectToolStripMenuItem})
        Me.AddToolStripMenuItem.Name = "AddToolStripMenuItem"
        Me.AddToolStripMenuItem.Size = New System.Drawing.Size(226, 22)
        Me.AddToolStripMenuItem.Text = "Add"
        '
        'NewProjectToolStripMenuItem
        '
        Me.NewProjectToolStripMenuItem.Name = "NewProjectToolStripMenuItem"
        Me.NewProjectToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.NewProjectToolStripMenuItem.Text = "New Project"
        '
        'ExistingProjectToolStripMenuItem
        '
        Me.ExistingProjectToolStripMenuItem.Image = CType(resources.GetObject("ExistingProjectToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ExistingProjectToolStripMenuItem.Name = "ExistingProjectToolStripMenuItem"
        Me.ExistingProjectToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.ExistingProjectToolStripMenuItem.Text = "Import Project (.ctp)"
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DelteProjectGroupOnlyToolStripMenuItem, Me.DeleteProjectGroupAndProjectsToolStripMenuItem})
        Me.DeleteToolStripMenuItem.Image = Global.CloudTranslator.My.Resources.Resources.Delete
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(226, 22)
        Me.DeleteToolStripMenuItem.Text = "Delete"
        '
        'DelteProjectGroupOnlyToolStripMenuItem
        '
        Me.DelteProjectGroupOnlyToolStripMenuItem.Name = "DelteProjectGroupOnlyToolStripMenuItem"
        Me.DelteProjectGroupOnlyToolStripMenuItem.Size = New System.Drawing.Size(251, 22)
        Me.DelteProjectGroupOnlyToolStripMenuItem.Text = "Delete Project Group only"
        '
        'DeleteProjectGroupAndProjectsToolStripMenuItem
        '
        Me.DeleteProjectGroupAndProjectsToolStripMenuItem.Name = "DeleteProjectGroupAndProjectsToolStripMenuItem"
        Me.DeleteProjectGroupAndProjectsToolStripMenuItem.Size = New System.Drawing.Size(251, 22)
        Me.DeleteProjectGroupAndProjectsToolStripMenuItem.Text = "Delete Project Group and Projects"
        '
        'EditProjectGroupPropertiesToolStripMenuItem
        '
        Me.EditProjectGroupPropertiesToolStripMenuItem.Image = Global.CloudTranslator.My.Resources.Resources.export_3
        Me.EditProjectGroupPropertiesToolStripMenuItem.Name = "EditProjectGroupPropertiesToolStripMenuItem"
        Me.EditProjectGroupPropertiesToolStripMenuItem.Size = New System.Drawing.Size(226, 22)
        Me.EditProjectGroupPropertiesToolStripMenuItem.Text = "Edit Project Group Properties"
        '
        'ExportProjectToolStripMenuItem
        '
        Me.ExportProjectToolStripMenuItem.Image = Global.CloudTranslator.My.Resources.Resources.export__1_
        Me.ExportProjectToolStripMenuItem.Name = "ExportProjectToolStripMenuItem"
        Me.ExportProjectToolStripMenuItem.Size = New System.Drawing.Size(226, 22)
        Me.ExportProjectToolStripMenuItem.Text = "Export Project (.ctproj)"
        '
        'ContextMenu_ProjectName
        '
        Me.ContextMenu_ProjectName.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditProjectToolStripMenuItem, Me.DeleteFilesToolStripMenuItem, Me.ExportCTPFileToolStripMenuItem, Me.SetAsMasterProjectToolStripMenuItem, Me.OpenProjectFolderToolStripMenuItem})
        Me.ContextMenu_ProjectName.Name = "ContextMenuStrip1"
        Me.ContextMenu_ProjectName.Size = New System.Drawing.Size(184, 136)
        '
        'EditProjectToolStripMenuItem
        '
        Me.EditProjectToolStripMenuItem.Image = CType(resources.GetObject("EditProjectToolStripMenuItem.Image"), System.Drawing.Image)
        Me.EditProjectToolStripMenuItem.Name = "EditProjectToolStripMenuItem"
        Me.EditProjectToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.EditProjectToolStripMenuItem.Text = "Edit Project"
        '
        'DeleteFilesToolStripMenuItem
        '
        Me.DeleteFilesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteFilesOnlyToolStripMenuItem, Me.DeleteFilesAndProjectToolStripMenuItem, Me.DeleteAllFilesExceptInputFilesToolStripMenuItem})
        Me.DeleteFilesToolStripMenuItem.Image = CType(resources.GetObject("DeleteFilesToolStripMenuItem.Image"), System.Drawing.Image)
        Me.DeleteFilesToolStripMenuItem.Name = "DeleteFilesToolStripMenuItem"
        Me.DeleteFilesToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.DeleteFilesToolStripMenuItem.Text = "Delete"
        '
        'DeleteFilesOnlyToolStripMenuItem
        '
        Me.DeleteFilesOnlyToolStripMenuItem.Name = "DeleteFilesOnlyToolStripMenuItem"
        Me.DeleteFilesOnlyToolStripMenuItem.Size = New System.Drawing.Size(240, 22)
        Me.DeleteFilesOnlyToolStripMenuItem.Text = "Delete Files only"
        '
        'DeleteFilesAndProjectToolStripMenuItem
        '
        Me.DeleteFilesAndProjectToolStripMenuItem.Name = "DeleteFilesAndProjectToolStripMenuItem"
        Me.DeleteFilesAndProjectToolStripMenuItem.Size = New System.Drawing.Size(240, 22)
        Me.DeleteFilesAndProjectToolStripMenuItem.Text = "Delete Files and Project"
        '
        'ExportCTPFileToolStripMenuItem
        '
        Me.ExportCTPFileToolStripMenuItem.Image = CType(resources.GetObject("ExportCTPFileToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ExportCTPFileToolStripMenuItem.Name = "ExportCTPFileToolStripMenuItem"
        Me.ExportCTPFileToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.ExportCTPFileToolStripMenuItem.Text = "Export CTP file"
        '
        'SetAsMasterProjectToolStripMenuItem
        '
        Me.SetAsMasterProjectToolStripMenuItem.Image = Global.CloudTranslator.My.Resources.Resources.MP4
        Me.SetAsMasterProjectToolStripMenuItem.Name = "SetAsMasterProjectToolStripMenuItem"
        Me.SetAsMasterProjectToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.SetAsMasterProjectToolStripMenuItem.Text = "Set as Master Project"
        '
        'OpenProjectFolderToolStripMenuItem
        '
        Me.OpenProjectFolderToolStripMenuItem.Image = Global.CloudTranslator.My.Resources.Resources.folder_Open
        Me.OpenProjectFolderToolStripMenuItem.Name = "OpenProjectFolderToolStripMenuItem"
        Me.OpenProjectFolderToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.OpenProjectFolderToolStripMenuItem.Text = "Open project folder"
        '
        'TV
        '
        Me.TV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TV.FullRowSelect = True
        Me.TV.HideSelection = False
        Me.TV.HotTracking = True
        Me.TV.ImageIndex = 0
        Me.TV.ImageList = Me.ImageList1
        Me.TV.Location = New System.Drawing.Point(0, 0)
        Me.TV.Name = "TV"
        Me.TV.SelectedImageIndex = 0
        Me.TV.ShowLines = False
        Me.TV.Size = New System.Drawing.Size(204, 585)
        Me.TV.TabIndex = 16
        '
        'DeleteAllFilesExceptInputFilesToolStripMenuItem
        '
        Me.DeleteAllFilesExceptInputFilesToolStripMenuItem.Name = "DeleteAllFilesExceptInputFilesToolStripMenuItem"
        Me.DeleteAllFilesExceptInputFilesToolStripMenuItem.Size = New System.Drawing.Size(240, 22)
        Me.DeleteAllFilesExceptInputFilesToolStripMenuItem.Text = "Delete all Files except Input files"
        '
        'Form_Treeview
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(204, 585)
        Me.Controls.Add(Me.TV)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form_Treeview"
        Me.Text = "Projects"
        Me.ContextMenu_Root.ResumeLayout(False)
        Me.ContextMenu_ProjectGroup.ResumeLayout(False)
        Me.ContextMenu_ProjectName.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TV As TreeViewDraggableNodes
    Friend WithEvents ContextMenu_Root As ContextMenuStrip
    Friend WithEvents NewProjectGroupToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImportProjectGroupToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditProjectGroupToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents RefreshToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenu_ProjectGroup As ContextMenuStrip
    Friend WithEvents AddToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewProjectToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExistingProjectToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DelteProjectGroupOnlyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteProjectGroupAndProjectsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditProjectGroupPropertiesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportProjectToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenu_ProjectName As ContextMenuStrip
    Friend WithEvents EditProjectToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteFilesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteFilesOnlyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteFilesAndProjectToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportCTPFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SetAsMasterProjectToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents OpenProjectFolderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteAllFilesExceptInputFilesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
