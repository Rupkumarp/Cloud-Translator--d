<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_XLFtoExcel
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_XLFtoExcel))
        Me.lblOutputFilePath = New System.Windows.Forms.Label()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.WK_grid = New System.Windows.Forms.DataGridView()
        Me.btnBrowseOutputFile = New System.Windows.Forms.Button()
        Me.txtOutputFilePath = New System.Windows.Forms.TextBox()
        Me.rdXlsToXliff = New System.Windows.Forms.RadioButton()
        Me.rdXliffToXls = New System.Windows.Forms.RadioButton()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnBrowseInputFile = New System.Windows.Forms.Button()
        Me.txtInputFilePath = New System.Windows.Forms.TextBox()
        Me.lblInputFilePath = New System.Windows.Forms.Label()
        Me.btnProcess = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.WK_grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblOutputFilePath
        '
        Me.lblOutputFilePath.AutoSize = True
        Me.lblOutputFilePath.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutputFilePath.Location = New System.Drawing.Point(59, 243)
        Me.lblOutputFilePath.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOutputFilePath.Name = "lblOutputFilePath"
        Me.lblOutputFilePath.Size = New System.Drawing.Size(139, 20)
        Me.lblOutputFilePath.TabIndex = 60
        Me.lblOutputFilePath.Text = "Output File Path:"
        '
        'Column5
        '
        Me.Column5.HeaderText = "Target"
        Me.Column5.Name = "Column5"
        Me.Column5.Width = 150
        '
        'Column4
        '
        Me.Column4.HeaderText = "Source"
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 325
        '
        'Column7
        '
        Me.Column7.HeaderText = "Note"
        Me.Column7.Name = "Column7"
        '
        'Column3
        '
        Me.Column3.HeaderText = "Resname"
        Me.Column3.Name = "Column3"
        '
        'Column6
        '
        Me.Column6.HeaderText = "Trans-unit ID"
        Me.Column6.Name = "Column6"
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
        Me.WK_grid.Location = New System.Drawing.Point(980, 75)
        Me.WK_grid.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.WK_grid.Name = "WK_grid"
        Me.WK_grid.RowHeadersVisible = False
        Me.WK_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.WK_grid.Size = New System.Drawing.Size(0, 0)
        Me.WK_grid.TabIndex = 64
        Me.WK_grid.Visible = False
        '
        'btnBrowseOutputFile
        '
        Me.btnBrowseOutputFile.Font = New System.Drawing.Font("BentonSans Regular", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseOutputFile.Location = New System.Drawing.Point(932, 269)
        Me.btnBrowseOutputFile.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnBrowseOutputFile.Name = "btnBrowseOutputFile"
        Me.btnBrowseOutputFile.Size = New System.Drawing.Size(34, 31)
        Me.btnBrowseOutputFile.TabIndex = 62
        Me.btnBrowseOutputFile.Text = "..."
        Me.btnBrowseOutputFile.UseVisualStyleBackColor = True
        '
        'txtOutputFilePath
        '
        Me.txtOutputFilePath.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOutputFilePath.Location = New System.Drawing.Point(59, 269)
        Me.txtOutputFilePath.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtOutputFilePath.Name = "txtOutputFilePath"
        Me.txtOutputFilePath.Size = New System.Drawing.Size(865, 27)
        Me.txtOutputFilePath.TabIndex = 61
        '
        'rdXlsToXliff
        '
        Me.rdXlsToXliff.AutoSize = True
        Me.rdXlsToXliff.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.rdXlsToXliff.Location = New System.Drawing.Point(220, 92)
        Me.rdXlsToXliff.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.rdXlsToXliff.Name = "rdXlsToXliff"
        Me.rdXlsToXliff.Size = New System.Drawing.Size(100, 24)
        Me.rdXlsToXliff.TabIndex = 59
        Me.rdXlsToXliff.TabStop = True
        Me.rdXlsToXliff.Text = "Xls->XlF"
        Me.rdXlsToXliff.UseVisualStyleBackColor = True
        '
        'rdXliffToXls
        '
        Me.rdXliffToXls.AutoSize = True
        Me.rdXliffToXls.Checked = True
        Me.rdXliffToXls.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.rdXliffToXls.Location = New System.Drawing.Point(59, 92)
        Me.rdXliffToXls.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.rdXliffToXls.Name = "rdXliffToXls"
        Me.rdXliffToXls.Size = New System.Drawing.Size(102, 24)
        Me.rdXliffToXls.TabIndex = 58
        Me.rdXliffToXls.TabStop = True
        Me.rdXliffToXls.Text = "XlF-> Xls"
        Me.rdXliffToXls.UseVisualStyleBackColor = True
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.AutoSize = False
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(225, 25)
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 372)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(2, 0, 21, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(1016, 31)
        Me.StatusStrip1.TabIndex = 63
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
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("BentonSans Bold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(59, 28)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(374, 29)
        Me.Label5.TabIndex = 57
        Me.Label5.Text = "File conversion from xliff to xls"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(865, 306)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 57)
        Me.btnCancel.TabIndex = 52
        Me.btnCancel.Text = "&Cancel"
        '
        'btnBrowseInputFile
        '
        Me.btnBrowseInputFile.Font = New System.Drawing.Font("BentonSans Regular", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseInputFile.Location = New System.Drawing.Point(932, 188)
        Me.btnBrowseInputFile.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnBrowseInputFile.Name = "btnBrowseInputFile"
        Me.btnBrowseInputFile.Size = New System.Drawing.Size(34, 31)
        Me.btnBrowseInputFile.TabIndex = 55
        Me.btnBrowseInputFile.Text = "..."
        Me.btnBrowseInputFile.UseVisualStyleBackColor = True
        '
        'txtInputFilePath
        '
        Me.txtInputFilePath.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInputFilePath.Location = New System.Drawing.Point(59, 188)
        Me.txtInputFilePath.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtInputFilePath.Name = "txtInputFilePath"
        Me.txtInputFilePath.Size = New System.Drawing.Size(865, 27)
        Me.txtInputFilePath.TabIndex = 54
        '
        'lblInputFilePath
        '
        Me.lblInputFilePath.AutoSize = True
        Me.lblInputFilePath.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInputFilePath.Location = New System.Drawing.Point(59, 162)
        Me.lblInputFilePath.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblInputFilePath.Name = "lblInputFilePath"
        Me.lblInputFilePath.Size = New System.Drawing.Size(125, 20)
        Me.lblInputFilePath.TabIndex = 53
        Me.lblInputFilePath.Text = "Input File Path:"
        '
        'btnProcess
        '
        Me.btnProcess.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnProcess.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnProcess.Location = New System.Drawing.Point(755, 306)
        Me.btnProcess.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnProcess.Name = "btnProcess"
        Me.btnProcess.Size = New System.Drawing.Size(100, 57)
        Me.btnProcess.TabIndex = 50
        Me.btnProcess.Text = "&Process"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = CType(resources.GetObject("PictureBox1.InitialImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(-2, 2)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(1017, 65)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 56
        Me.PictureBox1.TabStop = False
        '
        'Form_XLFtoExcel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1016, 403)
        Me.Controls.Add(Me.lblOutputFilePath)
        Me.Controls.Add(Me.WK_grid)
        Me.Controls.Add(Me.btnBrowseOutputFile)
        Me.Controls.Add(Me.txtOutputFilePath)
        Me.Controls.Add(Me.rdXlsToXliff)
        Me.Controls.Add(Me.rdXliffToXls)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnBrowseInputFile)
        Me.Controls.Add(Me.txtInputFilePath)
        Me.Controls.Add(Me.lblInputFilePath)
        Me.Controls.Add(Me.btnProcess)
        Me.Controls.Add(Me.PictureBox1)
        Me.Name = "Form_XLFtoExcel"
        Me.Text = "Form_XLFtoExcel"
        CType(Me.WK_grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblOutputFilePath As Label
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column7 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column6 As DataGridViewTextBoxColumn
    Friend WithEvents WK_grid As DataGridView
    Friend WithEvents btnBrowseOutputFile As Button
    Friend WithEvents txtOutputFilePath As TextBox
    Friend WithEvents rdXlsToXliff As RadioButton
    Friend WithEvents rdXliffToXls As RadioButton
    Friend WithEvents ToolStripProgressBar1 As ToolStripProgressBar
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents Label5 As Label
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnBrowseInputFile As Button
    Friend WithEvents txtInputFilePath As TextBox
    Friend WithEvents lblInputFilePath As Label
    Friend WithEvents btnProcess As Button
    Friend WithEvents PictureBox1 As PictureBox
End Class
