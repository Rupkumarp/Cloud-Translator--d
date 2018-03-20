<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_XliffToXlsConverter
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_XliffToXlsConverter))
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnBrowseInputFile = New System.Windows.Forms.Button()
        Me.txtInputFilePath = New System.Windows.Forms.TextBox()
        Me.lblInputFilePath = New System.Windows.Forms.Label()
        Me.btnProcess = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.rdXliffToXls = New System.Windows.Forms.RadioButton()
        Me.rdXlsToXliff = New System.Windows.Forms.RadioButton()
        Me.btnBrowseOutputFile = New System.Windows.Forms.Button()
        Me.txtOutputFilePath = New System.Windows.Forms.TextBox()
        Me.lblOutputFilePath = New System.Windows.Forms.Label()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.WK_grid = New System.Windows.Forms.DataGridView()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chkBulk = New System.Windows.Forms.CheckBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.WK_grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(576, 214)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(67, 23)
        Me.btnCancel.TabIndex = 36
        Me.btnCancel.Text = "&Cancel"
        '
        'btnBrowseInputFile
        '
        Me.btnBrowseInputFile.Font = New System.Drawing.Font("BentonSans Regular", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseInputFile.Location = New System.Drawing.Point(620, 124)
        Me.btnBrowseInputFile.Name = "btnBrowseInputFile"
        Me.btnBrowseInputFile.Size = New System.Drawing.Size(23, 20)
        Me.btnBrowseInputFile.TabIndex = 39
        Me.btnBrowseInputFile.Text = "..."
        Me.btnBrowseInputFile.UseVisualStyleBackColor = True
        '
        'txtInputFilePath
        '
        Me.txtInputFilePath.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInputFilePath.Location = New System.Drawing.Point(38, 124)
        Me.txtInputFilePath.Name = "txtInputFilePath"
        Me.txtInputFilePath.Size = New System.Drawing.Size(578, 21)
        Me.txtInputFilePath.TabIndex = 38
        '
        'lblInputFilePath
        '
        Me.lblInputFilePath.AutoSize = True
        Me.lblInputFilePath.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInputFilePath.Location = New System.Drawing.Point(38, 107)
        Me.lblInputFilePath.Name = "lblInputFilePath"
        Me.lblInputFilePath.Size = New System.Drawing.Size(83, 13)
        Me.lblInputFilePath.TabIndex = 37
        Me.lblInputFilePath.Text = "Input File Path:"
        '
        'btnProcess
        '
        Me.btnProcess.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnProcess.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnProcess.Location = New System.Drawing.Point(503, 214)
        Me.btnProcess.Name = "btnProcess"
        Me.btnProcess.Size = New System.Drawing.Size(67, 23)
        Me.btnProcess.TabIndex = 35
        Me.btnProcess.Text = "&Process"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = CType(resources.GetObject("PictureBox1.InitialImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(-3, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(678, 42)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 40
        Me.PictureBox1.TabStop = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("BentonSans Bold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(38, 20)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(254, 19)
        Me.Label5.TabIndex = 41
        Me.Label5.Text = "File conversion from xliff to xls"
        '
        'rdXliffToXls
        '
        Me.rdXliffToXls.AutoSize = True
        Me.rdXliffToXls.Checked = True
        Me.rdXliffToXls.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.rdXliffToXls.Location = New System.Drawing.Point(214, 62)
        Me.rdXliffToXls.Name = "rdXliffToXls"
        Me.rdXliffToXls.Size = New System.Drawing.Size(78, 17)
        Me.rdXliffToXls.TabIndex = 42
        Me.rdXliffToXls.TabStop = True
        Me.rdXliffToXls.Text = "Xliff -> Xls"
        Me.rdXliffToXls.UseVisualStyleBackColor = True
        '
        'rdXlsToXliff
        '
        Me.rdXlsToXliff.AutoSize = True
        Me.rdXlsToXliff.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.rdXlsToXliff.Location = New System.Drawing.Point(298, 62)
        Me.rdXlsToXliff.Name = "rdXlsToXliff"
        Me.rdXlsToXliff.Size = New System.Drawing.Size(72, 17)
        Me.rdXlsToXliff.TabIndex = 43
        Me.rdXlsToXliff.TabStop = True
        Me.rdXlsToXliff.Text = "Xls->Xliff"
        Me.rdXlsToXliff.UseVisualStyleBackColor = True
        '
        'btnBrowseOutputFile
        '
        Me.btnBrowseOutputFile.Font = New System.Drawing.Font("BentonSans Regular", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseOutputFile.Location = New System.Drawing.Point(620, 177)
        Me.btnBrowseOutputFile.Name = "btnBrowseOutputFile"
        Me.btnBrowseOutputFile.Size = New System.Drawing.Size(23, 20)
        Me.btnBrowseOutputFile.TabIndex = 46
        Me.btnBrowseOutputFile.Text = "..."
        Me.btnBrowseOutputFile.UseVisualStyleBackColor = True
        '
        'txtOutputFilePath
        '
        Me.txtOutputFilePath.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOutputFilePath.Location = New System.Drawing.Point(38, 177)
        Me.txtOutputFilePath.Name = "txtOutputFilePath"
        Me.txtOutputFilePath.Size = New System.Drawing.Size(578, 21)
        Me.txtOutputFilePath.TabIndex = 45
        '
        'lblOutputFilePath
        '
        Me.lblOutputFilePath.AutoSize = True
        Me.lblOutputFilePath.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutputFilePath.Location = New System.Drawing.Point(38, 160)
        Me.lblOutputFilePath.Name = "lblOutputFilePath"
        Me.lblOutputFilePath.Size = New System.Drawing.Size(92, 13)
        Me.lblOutputFilePath.TabIndex = 44
        Me.lblOutputFilePath.Text = "Output File Path:"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 254)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(673, 31)
        Me.StatusStrip1.TabIndex = 47
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(501, 26)
        Me.ToolStripStatusLabel1.Text = "Status:"
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.AutoSize = False
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(150, 25)
        '
        'WK_grid
        '
        Me.WK_grid.AllowUserToAddRows = False
        Me.WK_grid.AllowUserToResizeRows = False
        Me.WK_grid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WK_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.WK_grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column6, Me.Column3, Me.Column7, Me.Column4, Me.Column5})
        Me.WK_grid.Location = New System.Drawing.Point(652, 51)
        Me.WK_grid.Name = "WK_grid"
        Me.WK_grid.RowHeadersVisible = False
        Me.WK_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.WK_grid.Size = New System.Drawing.Size(10, 0)
        Me.WK_grid.TabIndex = 48
        Me.WK_grid.Visible = False
        '
        'Column6
        '
        Me.Column6.HeaderText = "Trans-unit ID"
        Me.Column6.Name = "Column6"
        '
        'Column3
        '
        Me.Column3.HeaderText = "Resname"
        Me.Column3.Name = "Column3"
        '
        'Column7
        '
        Me.Column7.HeaderText = "Note"
        Me.Column7.Name = "Column7"
        '
        'Column4
        '
        Me.Column4.HeaderText = "Source"
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 325
        '
        'Column5
        '
        Me.Column5.HeaderText = "Target"
        Me.Column5.Name = "Column5"
        Me.Column5.Width = 150
        '
        'chkBulk
        '
        Me.chkBulk.AutoSize = True
        Me.chkBulk.Location = New System.Drawing.Point(38, 62)
        Me.chkBulk.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.chkBulk.Name = "chkBulk"
        Me.chkBulk.Size = New System.Drawing.Size(115, 17)
        Me.chkBulk.TabIndex = 49
        Me.chkBulk.Text = "Check for Bulk File"
        Me.chkBulk.UseVisualStyleBackColor = True
        '
        'Form_XliffToXlsConverter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(673, 285)
        Me.Controls.Add(Me.chkBulk)
        Me.Controls.Add(Me.WK_grid)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.btnBrowseOutputFile)
        Me.Controls.Add(Me.txtOutputFilePath)
        Me.Controls.Add(Me.lblOutputFilePath)
        Me.Controls.Add(Me.rdXlsToXliff)
        Me.Controls.Add(Me.rdXliffToXls)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnBrowseInputFile)
        Me.Controls.Add(Me.txtInputFilePath)
        Me.Controls.Add(Me.lblInputFilePath)
        Me.Controls.Add(Me.btnProcess)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "Form_XliffToXlsConverter"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.WK_grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnBrowseInputFile As System.Windows.Forms.Button
    Friend WithEvents txtInputFilePath As System.Windows.Forms.TextBox
    Friend WithEvents lblInputFilePath As System.Windows.Forms.Label
    Friend WithEvents btnProcess As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents rdXliffToXls As System.Windows.Forms.RadioButton
    Friend WithEvents rdXlsToXliff As System.Windows.Forms.RadioButton
    Friend WithEvents btnBrowseOutputFile As System.Windows.Forms.Button
    Friend WithEvents txtOutputFilePath As System.Windows.Forms.TextBox
    Friend WithEvents lblOutputFilePath As System.Windows.Forms.Label
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents WK_grid As System.Windows.Forms.DataGridView
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chkBulk As CheckBox
End Class
