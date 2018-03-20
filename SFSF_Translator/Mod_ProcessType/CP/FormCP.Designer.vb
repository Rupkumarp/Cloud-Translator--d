<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormCP
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormCP))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbParentObjectType = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.CmbLang = New System.Windows.Forms.ComboBox()
        Me.lblRecord = New System.Windows.Forms.Label()
        Me.BtnNext = New System.Windows.Forms.Button()
        Me.BtnPrevious = New System.Windows.Forms.Button()
        Me.BtnNavigateLast = New System.Windows.Forms.Button()
        Me.BtnNavigateFirst = New System.Windows.Forms.Button()
        Me.BtnCreate = New System.Windows.Forms.Button()
        Me.BtnSetting = New System.Windows.Forms.Button()
        Me.BtnNew = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ChkLangSeq = New System.Windows.Forms.CheckBox()
        Me.BtnCreateChild = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblCPType = New System.Windows.Forms.Label()
        Me.cmbChildObjectType = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Location = New System.Drawing.Point(12, 49)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1046, 369)
        Me.Panel1.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("BentonSans Book", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(21, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(196, 14)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = """Copy/Paste"" Parent Object Type:"
        '
        'cmbParentObjectType
        '
        Me.cmbParentObjectType.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmbParentObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbParentObjectType.DropDownWidth = 400
        Me.cmbParentObjectType.Font = New System.Drawing.Font("BentonSans Book", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbParentObjectType.FormattingEnabled = True
        Me.cmbParentObjectType.Location = New System.Drawing.Point(220, 19)
        Me.cmbParentObjectType.Name = "cmbParentObjectType"
        Me.cmbParentObjectType.Size = New System.Drawing.Size(279, 22)
        Me.cmbParentObjectType.TabIndex = 20
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("BentonSans Book", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(844, 23)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(66, 14)
        Me.Label7.TabIndex = 17
        Me.Label7.Text = "Language:"
        '
        'CmbLang
        '
        Me.CmbLang.Cursor = System.Windows.Forms.Cursors.Hand
        Me.CmbLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbLang.Enabled = False
        Me.CmbLang.Font = New System.Drawing.Font("BentonSans Book", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbLang.FormattingEnabled = True
        Me.CmbLang.Items.AddRange(New Object() {"en_US", "fr_FR"})
        Me.CmbLang.Location = New System.Drawing.Point(916, 19)
        Me.CmbLang.Name = "CmbLang"
        Me.CmbLang.Size = New System.Drawing.Size(68, 22)
        Me.CmbLang.TabIndex = 21
        '
        'lblRecord
        '
        Me.lblRecord.AutoSize = True
        Me.lblRecord.Font = New System.Drawing.Font("BentonSans Book", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecord.Location = New System.Drawing.Point(743, 57)
        Me.lblRecord.Name = "lblRecord"
        Me.lblRecord.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblRecord.Size = New System.Drawing.Size(76, 14)
        Me.lblRecord.TabIndex = 15
        Me.lblRecord.Text = "Record: 0/0"
        Me.lblRecord.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'BtnNext
        '
        Me.BtnNext.AutoSize = True
        Me.BtnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnNext.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnNext.FlatAppearance.BorderSize = 0
        Me.BtnNext.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent
        Me.BtnNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.BtnNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnNext.Location = New System.Drawing.Point(913, 51)
        Me.BtnNext.Name = "BtnNext"
        Me.BtnNext.Size = New System.Drawing.Size(37, 27)
        Me.BtnNext.TabIndex = 24
        Me.BtnNext.Text = ">"
        Me.ToolTip1.SetToolTip(Me.BtnNext, "Next Item")
        Me.BtnNext.UseVisualStyleBackColor = True
        '
        'BtnPrevious
        '
        Me.BtnPrevious.AutoSize = True
        Me.BtnPrevious.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnPrevious.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnPrevious.FlatAppearance.BorderSize = 0
        Me.BtnPrevious.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent
        Me.BtnPrevious.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnPrevious.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.BtnPrevious.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnPrevious.Location = New System.Drawing.Point(873, 51)
        Me.BtnPrevious.Name = "BtnPrevious"
        Me.BtnPrevious.Size = New System.Drawing.Size(37, 27)
        Me.BtnPrevious.TabIndex = 23
        Me.BtnPrevious.Text = "<"
        Me.ToolTip1.SetToolTip(Me.BtnPrevious, "Previous Item")
        Me.BtnPrevious.UseVisualStyleBackColor = True
        '
        'BtnNavigateLast
        '
        Me.BtnNavigateLast.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnNavigateLast.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnNavigateLast.Location = New System.Drawing.Point(953, 51)
        Me.BtnNavigateLast.Name = "BtnNavigateLast"
        Me.BtnNavigateLast.Size = New System.Drawing.Size(37, 27)
        Me.BtnNavigateLast.TabIndex = 25
        Me.BtnNavigateLast.Text = ">>"
        Me.ToolTip1.SetToolTip(Me.BtnNavigateLast, "Move to Last Item")
        Me.BtnNavigateLast.UseVisualStyleBackColor = True
        '
        'BtnNavigateFirst
        '
        Me.BtnNavigateFirst.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnNavigateFirst.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnNavigateFirst.Location = New System.Drawing.Point(830, 51)
        Me.BtnNavigateFirst.Name = "BtnNavigateFirst"
        Me.BtnNavigateFirst.Size = New System.Drawing.Size(37, 27)
        Me.BtnNavigateFirst.TabIndex = 22
        Me.BtnNavigateFirst.Text = "<<"
        Me.ToolTip1.SetToolTip(Me.BtnNavigateFirst, "Move to First item")
        Me.BtnNavigateFirst.UseVisualStyleBackColor = True
        '
        'BtnCreate
        '
        Me.BtnCreate.Font = New System.Drawing.Font("BentonSans Book", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCreate.Location = New System.Drawing.Point(505, 19)
        Me.BtnCreate.Name = "BtnCreate"
        Me.BtnCreate.Size = New System.Drawing.Size(155, 23)
        Me.BtnCreate.TabIndex = 27
        Me.BtnCreate.Text = "Create CP from csv file"
        Me.ToolTip1.SetToolTip(Me.BtnCreate, "Create CP xml")
        Me.BtnCreate.UseVisualStyleBackColor = True
        Me.BtnCreate.Visible = False
        '
        'BtnSetting
        '
        Me.BtnSetting.AutoSize = True
        Me.BtnSetting.BackColor = System.Drawing.Color.Transparent
        Me.BtnSetting.BackgroundImage = Global.CloudTranslator.My.Resources.Resources.rsz_sett
        Me.BtnSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BtnSetting.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnSetting.FlatAppearance.BorderSize = 0
        Me.BtnSetting.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent
        Me.BtnSetting.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnSetting.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.BtnSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSetting.Location = New System.Drawing.Point(1010, 17)
        Me.BtnSetting.Name = "BtnSetting"
        Me.BtnSetting.Size = New System.Drawing.Size(28, 26)
        Me.BtnSetting.TabIndex = 28
        Me.ToolTip1.SetToolTip(Me.BtnSetting, "Language Sequencer")
        Me.BtnSetting.UseVisualStyleBackColor = False
        '
        'BtnNew
        '
        Me.BtnNew.AutoSize = True
        Me.BtnNew.BackColor = System.Drawing.Color.Transparent
        Me.BtnNew.BackgroundImage = Global.CloudTranslator.My.Resources.Resources._New
        Me.BtnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BtnNew.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnNew.FlatAppearance.BorderSize = 0
        Me.BtnNew.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent
        Me.BtnNew.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnNew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.BtnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnNew.Location = New System.Drawing.Point(1007, 51)
        Me.BtnNew.Name = "BtnNew"
        Me.BtnNew.Size = New System.Drawing.Size(34, 26)
        Me.BtnNew.TabIndex = 26
        Me.ToolTip1.SetToolTip(Me.BtnNew, "Add item")
        Me.BtnNew.UseVisualStyleBackColor = False
        '
        'ChkLangSeq
        '
        Me.ChkLangSeq.AutoSize = True
        Me.ChkLangSeq.Location = New System.Drawing.Point(991, 23)
        Me.ChkLangSeq.Name = "ChkLangSeq"
        Me.ChkLangSeq.Size = New System.Drawing.Size(15, 14)
        Me.ChkLangSeq.TabIndex = 37
        Me.ToolTip1.SetToolTip(Me.ChkLangSeq, "Load previous language sequence")
        Me.ChkLangSeq.UseVisualStyleBackColor = True
        '
        'BtnCreateChild
        '
        Me.BtnCreateChild.Enabled = False
        Me.BtnCreateChild.Font = New System.Drawing.Font("BentonSans Book", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCreateChild.Location = New System.Drawing.Point(505, 53)
        Me.BtnCreateChild.Name = "BtnCreateChild"
        Me.BtnCreateChild.Size = New System.Drawing.Size(155, 23)
        Me.BtnCreateChild.TabIndex = 38
        Me.BtnCreateChild.Text = "Create New Child file"
        Me.ToolTip1.SetToolTip(Me.BtnCreateChild, "Creates child object. for ex: 15.1 -> 15.1.1,15.1.2 etc")
        Me.BtnCreateChild.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = CType(resources.GetObject("PictureBox1.InitialImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(1069, 42)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 35
        Me.PictureBox1.TabStop = False
        '
        'lblCPType
        '
        Me.lblCPType.AutoSize = True
        Me.lblCPType.Font = New System.Drawing.Font("BentonSans Bold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCPType.Location = New System.Drawing.Point(34, 14)
        Me.lblCPType.Name = "lblCPType"
        Me.lblCPType.Size = New System.Drawing.Size(76, 19)
        Me.lblCPType.TabIndex = 36
        Me.lblCPType.Text = "CP Form"
        '
        'cmbChildObjectType
        '
        Me.cmbChildObjectType.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmbChildObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbChildObjectType.DropDownWidth = 200
        Me.cmbChildObjectType.Enabled = False
        Me.cmbChildObjectType.Font = New System.Drawing.Font("BentonSans Book", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbChildObjectType.FormattingEnabled = True
        Me.cmbChildObjectType.Location = New System.Drawing.Point(220, 53)
        Me.cmbChildObjectType.Name = "cmbChildObjectType"
        Me.cmbChildObjectType.Size = New System.Drawing.Size(279, 22)
        Me.cmbChildObjectType.TabIndex = 39
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("BentonSans Book", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(111, 57)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(106, 14)
        Me.Label2.TabIndex = 40
        Me.Label2.Text = "Child Object Type:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BtnCreate)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.cmbParentObjectType)
        Me.GroupBox1.Controls.Add(Me.cmbChildObjectType)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.BtnCreateChild)
        Me.GroupBox1.Controls.Add(Me.BtnPrevious)
        Me.GroupBox1.Controls.Add(Me.ChkLangSeq)
        Me.GroupBox1.Controls.Add(Me.lblRecord)
        Me.GroupBox1.Controls.Add(Me.CmbLang)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.BtnSetting)
        Me.GroupBox1.Controls.Add(Me.BtnNext)
        Me.GroupBox1.Controls.Add(Me.BtnNew)
        Me.GroupBox1.Controls.Add(Me.BtnNavigateFirst)
        Me.GroupBox1.Controls.Add(Me.BtnNavigateLast)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 424)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1046, 93)
        Me.GroupBox1.TabIndex = 41
        Me.GroupBox1.TabStop = False
        '
        'FormCP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1070, 529)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblCPType)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FormCP"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbParentObjectType As System.Windows.Forms.ComboBox
    Friend WithEvents BtnNew As System.Windows.Forms.Button
    Friend WithEvents BtnNext As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents CmbLang As System.Windows.Forms.ComboBox
    Friend WithEvents lblRecord As System.Windows.Forms.Label
    Friend WithEvents BtnPrevious As System.Windows.Forms.Button
    Friend WithEvents BtnNavigateLast As System.Windows.Forms.Button
    Friend WithEvents BtnNavigateFirst As System.Windows.Forms.Button
    Friend WithEvents BtnCreate As System.Windows.Forms.Button
    Friend WithEvents BtnSetting As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblCPType As System.Windows.Forms.Label
    Friend WithEvents ChkLangSeq As System.Windows.Forms.CheckBox
    Friend WithEvents BtnCreateChild As System.Windows.Forms.Button
    Friend WithEvents cmbChildObjectType As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
End Class
