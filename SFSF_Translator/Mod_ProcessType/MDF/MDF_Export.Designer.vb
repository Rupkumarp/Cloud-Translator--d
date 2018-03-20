<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MDF_Export
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
        Me.panel1 = New System.Windows.Forms.Panel()
        Me.linkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.linkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.lbl_populatingMDF = New System.Windows.Forms.Label()
        Me.label10 = New System.Windows.Forms.Label()
        Me.checkBox1 = New System.Windows.Forms.CheckBox()
        Me.checkedListBox1 = New System.Windows.Forms.CheckedListBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.cmb_MDFGroup = New System.Windows.Forms.ComboBox()
        Me.lbl_jobStatus = New System.Windows.Forms.Label()
        Me.richTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.lbl_JobId = New System.Windows.Forms.Label()
        Me.label5 = New System.Windows.Forms.Label()
        Me.label9 = New System.Windows.Forms.Label()
        Me.btn_getjobid = New System.Windows.Forms.Button()
        Me.label8 = New System.Windows.Forms.Label()
        Me.label7 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.pictureBox2 = New System.Windows.Forms.PictureBox()
        Me.panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'panel1
        '
        Me.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panel1.Controls.Add(Me.linkLabel2)
        Me.panel1.Controls.Add(Me.linkLabel1)
        Me.panel1.Controls.Add(Me.lbl_populatingMDF)
        Me.panel1.Controls.Add(Me.label10)
        Me.panel1.Controls.Add(Me.checkBox1)
        Me.panel1.Controls.Add(Me.checkedListBox1)
        Me.panel1.Controls.Add(Me.label1)
        Me.panel1.Controls.Add(Me.cmb_MDFGroup)
        Me.panel1.Controls.Add(Me.lbl_jobStatus)
        Me.panel1.Controls.Add(Me.richTextBox1)
        Me.panel1.Controls.Add(Me.lbl_JobId)
        Me.panel1.Controls.Add(Me.label5)
        Me.panel1.Controls.Add(Me.label9)
        Me.panel1.Controls.Add(Me.btn_getjobid)
        Me.panel1.Controls.Add(Me.label8)
        Me.panel1.Controls.Add(Me.pictureBox2)
        Me.panel1.Controls.Add(Me.label7)
        Me.panel1.Location = New System.Drawing.Point(4, 23)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(1318, 712)
        Me.panel1.TabIndex = 11
        '
        'linkLabel2
        '
        Me.linkLabel2.AutoSize = True
        Me.linkLabel2.Font = New System.Drawing.Font("BentonSans Book", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.linkLabel2.Location = New System.Drawing.Point(1147, 85)
        Me.linkLabel2.Name = "linkLabel2"
        Me.linkLabel2.Size = New System.Drawing.Size(134, 20)
        Me.linkLabel2.TabIndex = 42
        Me.linkLabel2.TabStop = True
        Me.linkLabel2.Text = "MDF Group Info"
        '
        'linkLabel1
        '
        Me.linkLabel1.AutoSize = True
        Me.linkLabel1.Font = New System.Drawing.Font("BentonSans Book", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.linkLabel1.Location = New System.Drawing.Point(1142, 57)
        Me.linkLabel1.Name = "linkLabel1"
        Me.linkLabel1.Size = New System.Drawing.Size(139, 20)
        Me.linkLabel1.TabIndex = 41
        Me.linkLabel1.TabStop = True
        Me.linkLabel1.Text = "User Credentials"
        '
        'lbl_populatingMDF
        '
        Me.lbl_populatingMDF.Font = New System.Drawing.Font("BentonSans Regular", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_populatingMDF.ForeColor = System.Drawing.Color.Blue
        Me.lbl_populatingMDF.Location = New System.Drawing.Point(375, 197)
        Me.lbl_populatingMDF.Name = "lbl_populatingMDF"
        Me.lbl_populatingMDF.Size = New System.Drawing.Size(106, 22)
        Me.lbl_populatingMDF.TabIndex = 39
        Me.lbl_populatingMDF.Text = "populating...."
        Me.lbl_populatingMDF.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'label10
        '
        Me.label10.Font = New System.Drawing.Font("BentonSans Regular", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label10.Location = New System.Drawing.Point(127, 197)
        Me.label10.Name = "label10"
        Me.label10.Size = New System.Drawing.Size(92, 22)
        Me.label10.TabIndex = 38
        Me.label10.Text = "MDF type ID"
        Me.label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'checkBox1
        '
        Me.checkBox1.AutoSize = True
        Me.checkBox1.Location = New System.Drawing.Point(20, 222)
        Me.checkBox1.Name = "checkBox1"
        Me.checkBox1.Size = New System.Drawing.Size(88, 21)
        Me.checkBox1.TabIndex = 37
        Me.checkBox1.Text = "Check All"
        Me.checkBox1.UseVisualStyleBackColor = True
        '
        'checkedListBox1
        '
        Me.checkedListBox1.CheckOnClick = True
        Me.checkedListBox1.FormattingEnabled = True
        Me.checkedListBox1.Location = New System.Drawing.Point(130, 222)
        Me.checkedListBox1.Name = "checkedListBox1"
        Me.checkedListBox1.Size = New System.Drawing.Size(351, 480)
        Me.checkedListBox1.TabIndex = 36
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(17, 142)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(91, 17)
        Me.label1.TabIndex = 35
        Me.label1.Text = "Select Group"
        '
        'cmb_MDFGroup
        '
        Me.cmb_MDFGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmb_MDFGroup.FormattingEnabled = True
        Me.cmb_MDFGroup.Location = New System.Drawing.Point(130, 139)
        Me.cmb_MDFGroup.Name = "cmb_MDFGroup"
        Me.cmb_MDFGroup.Size = New System.Drawing.Size(351, 24)
        Me.cmb_MDFGroup.TabIndex = 31
        '
        'lbl_jobStatus
        '
        Me.lbl_jobStatus.Font = New System.Drawing.Font("BentonSans Regular", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_jobStatus.ForeColor = System.Drawing.Color.Blue
        Me.lbl_jobStatus.Location = New System.Drawing.Point(1037, 143)
        Me.lbl_jobStatus.Name = "lbl_jobStatus"
        Me.lbl_jobStatus.Size = New System.Drawing.Size(247, 22)
        Me.lbl_jobStatus.TabIndex = 28
        '
        'richTextBox1
        '
        Me.richTextBox1.Location = New System.Drawing.Point(510, 171)
        Me.richTextBox1.Name = "richTextBox1"
        Me.richTextBox1.ReadOnly = True
        Me.richTextBox1.Size = New System.Drawing.Size(771, 531)
        Me.richTextBox1.TabIndex = 19
        Me.richTextBox1.Text = ""
        '
        'lbl_JobId
        '
        Me.lbl_JobId.Font = New System.Drawing.Font("BentonSans Regular", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_JobId.Location = New System.Drawing.Point(704, 144)
        Me.lbl_JobId.Name = "lbl_JobId"
        Me.lbl_JobId.Size = New System.Drawing.Size(104, 22)
        Me.lbl_JobId.TabIndex = 18
        '
        'label5
        '
        Me.label5.Font = New System.Drawing.Font("BentonSans Regular", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label5.Location = New System.Drawing.Point(638, 144)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(65, 22)
        Me.label5.TabIndex = 17
        Me.label5.Text = "Job Log"
        '
        'label9
        '
        Me.label9.Font = New System.Drawing.Font("BentonSans Regular", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label9.Location = New System.Drawing.Point(15, 100)
        Me.label9.Name = "label9"
        Me.label9.Size = New System.Drawing.Size(483, 27)
        Me.label9.TabIndex = 16
        Me.label9.Text = "Automated export of multiple MDF object for various types"
        '
        'btn_getjobid
        '
        Me.btn_getjobid.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_getjobid.Location = New System.Drawing.Point(510, 135)
        Me.btn_getjobid.Name = "btn_getjobid"
        Me.btn_getjobid.Size = New System.Drawing.Size(87, 30)
        Me.btn_getjobid.TabIndex = 12
        Me.btn_getjobid.Text = "Run"
        Me.btn_getjobid.UseVisualStyleBackColor = True
        '
        'label8
        '
        Me.label8.Font = New System.Drawing.Font("BentonSans Regular", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label8.Location = New System.Drawing.Point(12, 63)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(584, 27)
        Me.label8.TabIndex = 15
        Me.label8.Text = "SF BizX: Export MDF objects scenario"
        '
        'label7
        '
        Me.label7.Font = New System.Drawing.Font("BentonSans Regular", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label7.Location = New System.Drawing.Point(12, 15)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(141, 33)
        Me.label7.TabIndex = 14
        Me.label7.Text = "Scenario"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackgroundImage = Global.CloudTranslator.My.Resources.Resources.sapband
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(1322, 16)
        Me.PictureBox1.TabIndex = 12
        Me.PictureBox1.TabStop = False
        '
        'pictureBox2
        '
        Me.pictureBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pictureBox2.Location = New System.Drawing.Point(18, 46)
        Me.pictureBox2.Name = "pictureBox2"
        Me.pictureBox2.Size = New System.Drawing.Size(1263, 2)
        Me.pictureBox2.TabIndex = 9
        Me.pictureBox2.TabStop = False
        '
        'MDF_Export
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1325, 737)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "MDF_Export"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "MDF Export"
        Me.panel1.ResumeLayout(False)
        Me.panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Private WithEvents panel1 As Panel
    Private WithEvents linkLabel2 As LinkLabel
    Private WithEvents linkLabel1 As LinkLabel
    Private WithEvents lbl_populatingMDF As Label
    Private WithEvents label10 As Label
    Private WithEvents checkBox1 As CheckBox
    Private WithEvents checkedListBox1 As CheckedListBox
    Private WithEvents label1 As Label
    Private WithEvents cmb_MDFGroup As ComboBox
    Private WithEvents lbl_jobStatus As Label
    Private WithEvents richTextBox1 As RichTextBox
    Private WithEvents lbl_JobId As Label
    Private WithEvents label5 As Label
    Private WithEvents label9 As Label
    Private WithEvents btn_getjobid As Button
    Private WithEvents label8 As Label
    Friend WithEvents pictureBox2 As PictureBox
    Private WithEvents label7 As Label
End Class
