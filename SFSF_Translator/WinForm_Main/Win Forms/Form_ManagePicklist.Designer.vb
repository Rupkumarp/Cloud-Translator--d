<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManagePicklist
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ManagePicklist))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtMasterFile = New System.Windows.Forms.TextBox()
        Me.txtCustomFile = New System.Windows.Forms.TextBox()
        Me.BtnBrowseMasterFile = New System.Windows.Forms.Button()
        Me.BtnBrowseCutomFile = New System.Windows.Forms.Button()
        Me.BtnImportTranslation = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.lstLang = New System.Windows.Forms.ListBox()
        Me.BWorker = New System.ComponentModel.BackgroundWorker()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BtnWorkingDirectory = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.BtnOutFolder = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.Label1.Location = New System.Drawing.Point(22, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 14)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Master file"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.Label2.Location = New System.Drawing.Point(7, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 14)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Custom csv file"
        '
        'txtMasterFile
        '
        Me.txtMasterFile.Location = New System.Drawing.Point(120, 24)
        Me.txtMasterFile.Name = "txtMasterFile"
        Me.txtMasterFile.Size = New System.Drawing.Size(395, 21)
        Me.txtMasterFile.TabIndex = 1
        '
        'txtCustomFile
        '
        Me.txtCustomFile.Location = New System.Drawing.Point(120, 29)
        Me.txtCustomFile.Name = "txtCustomFile"
        Me.txtCustomFile.Size = New System.Drawing.Size(530, 21)
        Me.txtCustomFile.TabIndex = 1
        '
        'BtnBrowseMasterFile
        '
        Me.BtnBrowseMasterFile.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnBrowseMasterFile.Enabled = False
        Me.BtnBrowseMasterFile.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnBrowseMasterFile.Location = New System.Drawing.Point(523, 23)
        Me.BtnBrowseMasterFile.Name = "BtnBrowseMasterFile"
        Me.BtnBrowseMasterFile.Size = New System.Drawing.Size(167, 25)
        Me.BtnBrowseMasterFile.TabIndex = 2
        Me.BtnBrowseMasterFile.Text = "Generate xliff Transaltions"
        Me.BtnBrowseMasterFile.UseVisualStyleBackColor = True
        '
        'BtnBrowseCutomFile
        '
        Me.BtnBrowseCutomFile.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnBrowseCutomFile.Location = New System.Drawing.Point(657, 29)
        Me.BtnBrowseCutomFile.Name = "BtnBrowseCutomFile"
        Me.BtnBrowseCutomFile.Size = New System.Drawing.Size(33, 25)
        Me.BtnBrowseCutomFile.TabIndex = 3
        Me.BtnBrowseCutomFile.Text = ".."
        Me.BtnBrowseCutomFile.UseVisualStyleBackColor = True
        '
        'BtnImportTranslation
        '
        Me.BtnImportTranslation.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnImportTranslation.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnImportTranslation.Location = New System.Drawing.Point(549, 70)
        Me.BtnImportTranslation.Name = "BtnImportTranslation"
        Me.BtnImportTranslation.Size = New System.Drawing.Size(140, 25)
        Me.BtnImportTranslation.TabIndex = 4
        Me.BtnImportTranslation.Text = "Import Translation"
        Me.BtnImportTranslation.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnCancel.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(549, 109)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(87, 25)
        Me.BtnCancel.TabIndex = 5
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'lstLang
        '
        Me.lstLang.FormattingEnabled = True
        Me.lstLang.ItemHeight = 14
        Me.lstLang.Location = New System.Drawing.Point(120, 67)
        Me.lstLang.Name = "lstLang"
        Me.lstLang.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstLang.Size = New System.Drawing.Size(422, 144)
        Me.lstLang.TabIndex = 7
        '
        'BWorker
        '
        Me.BWorker.WorkerReportsProgress = True
        Me.BWorker.WorkerSupportsCancellation = True
        '
        'CheckBox1
        '
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.CheckBox1.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.CheckBox1.Location = New System.Drawing.Point(26, 60)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(331, 61)
        Me.CheckBox1.TabIndex = 9
        Me.CheckBox1.Text = "Run without Master file"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BtnWorkingDirectory)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.CheckBox1)
        Me.GroupBox1.Controls.Add(Me.txtMasterFile)
        Me.GroupBox1.Controls.Add(Me.BtnBrowseMasterFile)
        Me.GroupBox1.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 51)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(696, 125)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Step 1"
        '
        'BtnWorkingDirectory
        '
        Me.BtnWorkingDirectory.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnWorkingDirectory.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnWorkingDirectory.Location = New System.Drawing.Point(523, 78)
        Me.BtnWorkingDirectory.Name = "BtnWorkingDirectory"
        Me.BtnWorkingDirectory.Size = New System.Drawing.Size(167, 25)
        Me.BtnWorkingDirectory.TabIndex = 10
        Me.BtnWorkingDirectory.Text = "Open Working Directory"
        Me.BtnWorkingDirectory.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.Label3.Location = New System.Drawing.Point(7, 70)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(106, 141)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Select Languages from the list       (Multi seclection can be done)"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.BtnOutFolder)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.BtnBrowseCutomFile)
        Me.GroupBox2.Controls.Add(Me.txtCustomFile)
        Me.GroupBox2.Controls.Add(Me.BtnCancel)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.BtnImportTranslation)
        Me.GroupBox2.Controls.Add(Me.lstLang)
        Me.GroupBox2.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 182)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(696, 220)
        Me.GroupBox2.TabIndex = 11
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Step 2"
        '
        'BtnOutFolder
        '
        Me.BtnOutFolder.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnOutFolder.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnOutFolder.Location = New System.Drawing.Point(549, 186)
        Me.BtnOutFolder.Name = "BtnOutFolder"
        Me.BtnOutFolder.Size = New System.Drawing.Size(140, 25)
        Me.BtnOutFolder.TabIndex = 9
        Me.BtnOutFolder.Text = "Open Out Folder"
        Me.BtnOutFolder.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 407)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 16, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(709, 24)
        Me.StatusStrip1.TabIndex = 8
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(425, 19)
        Me.ToolStripStatusLabel1.Text = "Status:"
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.AutoSize = False
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(175, 18)
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = CType(resources.GetObject("PictureBox1.InitialImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, -1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(709, 45)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 46
        Me.PictureBox1.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("BentonSans Bold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(24, 20)
        Me.Label6.Margin = New System.Windows.Forms.Padding(0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(142, 21)
        Me.Label6.TabIndex = 47
        Me.Label6.Text = "Manage Picklist!"
        '
        'Form_ManagePicklist
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(709, 431)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form_ManagePicklist"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtMasterFile As System.Windows.Forms.TextBox
    Friend WithEvents txtCustomFile As System.Windows.Forms.TextBox
    Friend WithEvents BtnBrowseMasterFile As System.Windows.Forms.Button
    Friend WithEvents BtnBrowseCutomFile As System.Windows.Forms.Button
    Friend WithEvents BtnImportTranslation As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents lstLang As System.Windows.Forms.ListBox
    Friend WithEvents BWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents BtnWorkingDirectory As System.Windows.Forms.Button
    Friend WithEvents BtnOutFolder As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
End Class
