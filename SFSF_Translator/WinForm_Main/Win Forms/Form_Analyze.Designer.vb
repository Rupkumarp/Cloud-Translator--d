<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Analyze
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_Analyze))
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.BtnBrowse = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtInputFile = New System.Windows.Forms.TextBox()
        Me.BtnAnalyze = New System.Windows.Forms.Button()
        Me.RBSingleFile = New System.Windows.Forms.RadioButton()
        Me.RbMultiFile = New System.Windows.Forms.RadioButton()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BW = New System.ComponentModel.BackgroundWorker()
        Me.ChkCustomer = New System.Windows.Forms.CheckBox()
        Me.ChkInstance = New System.Windows.Forms.CheckBox()
        Me.ChkFileId = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkTxtReport = New System.Windows.Forms.CheckBox()
        Me.ChkResName = New System.Windows.Forms.CheckBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.StatusStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 298)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(709, 22)
        Me.StatusStrip1.TabIndex = 0
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(524, 17)
        Me.ToolStripStatusLabel1.Text = "Status: Idle"
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(165, 16)
        '
        'BtnBrowse
        '
        Me.BtnBrowse.Location = New System.Drawing.Point(545, 30)
        Me.BtnBrowse.Name = "BtnBrowse"
        Me.BtnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.BtnBrowse.TabIndex = 1
        Me.BtnBrowse.Text = "Browse"
        Me.BtnBrowse.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Input Xliff File"
        '
        'TxtInputFile
        '
        Me.TxtInputFile.Location = New System.Drawing.Point(115, 31)
        Me.TxtInputFile.Name = "TxtInputFile"
        Me.TxtInputFile.Size = New System.Drawing.Size(412, 21)
        Me.TxtInputFile.TabIndex = 3
        '
        'BtnAnalyze
        '
        Me.BtnAnalyze.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAnalyze.Location = New System.Drawing.Point(522, 261)
        Me.BtnAnalyze.Name = "BtnAnalyze"
        Me.BtnAnalyze.Size = New System.Drawing.Size(75, 23)
        Me.BtnAnalyze.TabIndex = 1
        Me.BtnAnalyze.Text = "Analyze"
        Me.BtnAnalyze.UseVisualStyleBackColor = True
        '
        'RBSingleFile
        '
        Me.RBSingleFile.AutoSize = True
        Me.RBSingleFile.Checked = True
        Me.RBSingleFile.Location = New System.Drawing.Point(26, 20)
        Me.RBSingleFile.Name = "RBSingleFile"
        Me.RBSingleFile.Size = New System.Drawing.Size(76, 17)
        Me.RBSingleFile.TabIndex = 4
        Me.RBSingleFile.TabStop = True
        Me.RBSingleFile.Text = "Single File"
        Me.RBSingleFile.UseVisualStyleBackColor = True
        '
        'RbMultiFile
        '
        Me.RbMultiFile.AutoSize = True
        Me.RbMultiFile.Location = New System.Drawing.Point(134, 20)
        Me.RbMultiFile.Name = "RbMultiFile"
        Me.RbMultiFile.Size = New System.Drawing.Size(71, 17)
        Me.RbMultiFile.TabIndex = 4
        Me.RbMultiFile.Text = "Multi FIle"
        Me.RbMultiFile.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(615, 261)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(75, 23)
        Me.BtnCancel.TabIndex = 1
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BW
        '
        Me.BW.WorkerReportsProgress = True
        Me.BW.WorkerSupportsCancellation = True
        '
        'ChkCustomer
        '
        Me.ChkCustomer.AutoSize = True
        Me.ChkCustomer.Location = New System.Drawing.Point(26, 58)
        Me.ChkCustomer.Name = "ChkCustomer"
        Me.ChkCustomer.Size = New System.Drawing.Size(110, 17)
        Me.ChkCustomer.TabIndex = 5
        Me.ChkCustomer.Text = "Match Customer"
        Me.ChkCustomer.UseVisualStyleBackColor = True
        '
        'ChkInstance
        '
        Me.ChkInstance.AutoSize = True
        Me.ChkInstance.Location = New System.Drawing.Point(145, 58)
        Me.ChkInstance.Name = "ChkInstance"
        Me.ChkInstance.Size = New System.Drawing.Size(103, 17)
        Me.ChkInstance.TabIndex = 5
        Me.ChkInstance.Text = "Match Instance"
        Me.ChkInstance.UseVisualStyleBackColor = True
        '
        'ChkFileId
        '
        Me.ChkFileId.AutoSize = True
        Me.ChkFileId.Location = New System.Drawing.Point(257, 58)
        Me.ChkFileId.Name = "ChkFileId"
        Me.ChkFileId.Size = New System.Drawing.Size(91, 17)
        Me.ChkFileId.TabIndex = 5
        Me.ChkFileId.Text = "Match File ID"
        Me.ChkFileId.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RBSingleFile)
        Me.GroupBox1.Controls.Add(Me.chkTxtReport)
        Me.GroupBox1.Controls.Add(Me.ChkResName)
        Me.GroupBox1.Controls.Add(Me.ChkFileId)
        Me.GroupBox1.Controls.Add(Me.RbMultiFile)
        Me.GroupBox1.Controls.Add(Me.ChkInstance)
        Me.GroupBox1.Controls.Add(Me.ChkCustomer)
        Me.GroupBox1.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(27, 67)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(662, 93)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "STEP1"
        '
        'chkTxtReport
        '
        Me.chkTxtReport.AutoSize = True
        Me.chkTxtReport.Location = New System.Drawing.Point(480, 58)
        Me.chkTxtReport.Name = "chkTxtReport"
        Me.chkTxtReport.Size = New System.Drawing.Size(83, 17)
        Me.chkTxtReport.TabIndex = 5
        Me.chkTxtReport.Text = "Text Report"
        Me.chkTxtReport.UseVisualStyleBackColor = True
        '
        'ChkResName
        '
        Me.ChkResName.AutoSize = True
        Me.ChkResName.Location = New System.Drawing.Point(357, 58)
        Me.ChkResName.Name = "ChkResName"
        Me.ChkResName.Size = New System.Drawing.Size(107, 17)
        Me.ChkResName.TabIndex = 5
        Me.ChkResName.Text = "Match Resname"
        Me.ChkResName.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.BtnBrowse)
        Me.GroupBox2.Controls.Add(Me.TxtInputFile)
        Me.GroupBox2.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(27, 177)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(662, 78)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "STEP2"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("BentonSans Bold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(23, 24)
        Me.Label6.Margin = New System.Windows.Forms.Padding(0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(244, 19)
        Me.Label6.TabIndex = 36
        Me.Label6.Text = "Analyze: Creates Excel Report"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = CType(resources.GetObject("PictureBox1.InitialImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(709, 42)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 35
        Me.PictureBox1.TabStop = False
        '
        'Form_Analyze
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(709, 320)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnAnalyze)
        Me.Controls.Add(Me.StatusStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "Form_Analyze"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As ToolStripProgressBar
    Friend WithEvents BtnBrowse As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents TxtInputFile As TextBox
    Friend WithEvents BtnAnalyze As Button
    Friend WithEvents RBSingleFile As RadioButton
    Friend WithEvents RbMultiFile As RadioButton
    Friend WithEvents BtnCancel As Button
    Friend WithEvents BW As System.ComponentModel.BackgroundWorker
    Friend WithEvents ChkCustomer As CheckBox
    Friend WithEvents ChkInstance As CheckBox
    Friend WithEvents ChkFileId As CheckBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label6 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents ChkResName As CheckBox
    Friend WithEvents chkTxtReport As CheckBox
End Class
