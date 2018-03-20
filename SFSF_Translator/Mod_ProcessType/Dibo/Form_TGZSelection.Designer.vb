<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_TGZSelection
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btn_thumbnails = New System.Windows.Forms.Button()
        Me.grp_folder = New System.Windows.Forms.GroupBox()
        Me.lbl_tgzFolderPath = New System.Windows.Forms.Label()
        Me.link_InputFolder = New System.Windows.Forms.LinkLabel()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.lbl_status = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.grp_folder.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btn_thumbnails
        '
        Me.btn_thumbnails.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_thumbnails.Location = New System.Drawing.Point(625, 72)
        Me.btn_thumbnails.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_thumbnails.Name = "btn_thumbnails"
        Me.btn_thumbnails.Size = New System.Drawing.Size(65, 26)
        Me.btn_thumbnails.TabIndex = 13
        Me.btn_thumbnails.Text = "Ok"
        Me.btn_thumbnails.UseVisualStyleBackColor = True
        '
        'grp_folder
        '
        Me.grp_folder.Controls.Add(Me.lbl_tgzFolderPath)
        Me.grp_folder.Controls.Add(Me.link_InputFolder)
        Me.grp_folder.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_folder.Location = New System.Drawing.Point(6, 18)
        Me.grp_folder.Margin = New System.Windows.Forms.Padding(4)
        Me.grp_folder.Name = "grp_folder"
        Me.grp_folder.Padding = New System.Windows.Forms.Padding(4)
        Me.grp_folder.Size = New System.Drawing.Size(700, 52)
        Me.grp_folder.TabIndex = 12
        Me.grp_folder.TabStop = False
        Me.grp_folder.Text = "Extract .tgz folder for DIBO"
        '
        'lbl_tgzFolderPath
        '
        Me.lbl_tgzFolderPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_tgzFolderPath.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_tgzFolderPath.Location = New System.Drawing.Point(112, 19)
        Me.lbl_tgzFolderPath.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_tgzFolderPath.Name = "lbl_tgzFolderPath"
        Me.lbl_tgzFolderPath.Size = New System.Drawing.Size(572, 24)
        Me.lbl_tgzFolderPath.TabIndex = 2
        Me.lbl_tgzFolderPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'link_InputFolder
        '
        Me.link_InputFolder.AutoSize = True
        Me.link_InputFolder.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.link_InputFolder.Location = New System.Drawing.Point(12, 22)
        Me.link_InputFolder.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.link_InputFolder.Name = "link_InputFolder"
        Me.link_InputFolder.Size = New System.Drawing.Size(91, 17)
        Me.link_InputFolder.TabIndex = 0
        Me.link_InputFolder.TabStop = True
        Me.link_InputFolder.Text = "Select Folder"
        '
        'lbl_status
        '
        Me.lbl_status.AutoSize = True
        Me.lbl_status.Location = New System.Drawing.Point(115, 78)
        Me.lbl_status.Name = "lbl_status"
        Me.lbl_status.Size = New System.Drawing.Size(114, 17)
        Me.lbl_status.TabIndex = 14
        Me.lbl_status.Text = "extracting files...."
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackgroundImage = Global.CloudTranslator.My.Resources.Resources.sapband
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(-1, -1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(714, 13)
        Me.PictureBox1.TabIndex = 15
        Me.PictureBox1.TabStop = False
        '
        'Form_TGZSelection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(710, 101)
        Me.Controls.Add(Me.btn_thumbnails)
        Me.Controls.Add(Me.grp_folder)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lbl_status)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_TGZSelection"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Extract TGZ"
        Me.grp_folder.ResumeLayout(False)
        Me.grp_folder.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btn_thumbnails As Button
    Friend WithEvents grp_folder As GroupBox
    Friend WithEvents lbl_tgzFolderPath As Label
    Friend WithEvents link_InputFolder As LinkLabel
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents lbl_status As Label
End Class
