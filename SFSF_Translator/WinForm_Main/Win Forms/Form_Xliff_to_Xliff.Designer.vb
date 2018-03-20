<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Xliff_to_Xliff
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_Xliff_to_Xliff))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.BtnChangeFolder = New System.Windows.Forms.Button()
        Me.BtnUP = New System.Windows.Forms.Button()
        Me.BtnDown = New System.Windows.Forms.Button()
        Me.BtnReloadLang = New System.Windows.Forms.Button()
        Me.BtnDeleteLang = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusBar = New System.Windows.Forms.ToolStripStatusLabel()
        Me.CmbLang = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.BtnStartCopying = New System.Windows.Forms.Button()
        Me.OutputList = New System.Windows.Forms.ListView()
        Me.FileName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Path = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtInputXlifffile = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtOutput = New System.Windows.Forms.TextBox()
        Me.Btn1Browse = New System.Windows.Forms.Button()
        Me.Btn2browse = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.BW = New System.ComponentModel.BackgroundWorker()
        Me.ListView2 = New System.Windows.Forms.ListView()
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.ChkOutPut = New System.Windows.Forms.CheckBox()
        Me.RBMultiple = New System.Windows.Forms.RadioButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.RBSingle = New System.Windows.Forms.RadioButton()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = CType(resources.GetObject("PictureBox1.InitialImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(2, 2)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(834, 42)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 32
        Me.PictureBox1.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("BentonSans Bold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(23, 23)
        Me.Label6.Margin = New System.Windows.Forms.Padding(0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(102, 19)
        Me.Label6.TabIndex = 33
        Me.Label6.Text = "Xliff to Xliff"
        '
        'BtnChangeFolder
        '
        Me.BtnChangeFolder.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnChangeFolder.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnChangeFolder.Location = New System.Drawing.Point(15, 20)
        Me.BtnChangeFolder.Name = "BtnChangeFolder"
        Me.BtnChangeFolder.Size = New System.Drawing.Size(185, 22)
        Me.BtnChangeFolder.TabIndex = 36
        Me.BtnChangeFolder.Text = "Add Translated xliff files"
        Me.ToolTip1.SetToolTip(Me.BtnChangeFolder, "Populates xliff files from folder")
        Me.BtnChangeFolder.UseVisualStyleBackColor = True
        '
        'BtnUP
        '
        Me.BtnUP.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnUP.Font = New System.Drawing.Font("Wingdings 3", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnUP.Location = New System.Drawing.Point(779, 138)
        Me.BtnUP.Name = "BtnUP"
        Me.BtnUP.Size = New System.Drawing.Size(38, 30)
        Me.BtnUP.TabIndex = 39
        Me.BtnUP.Text = "p"
        Me.ToolTip1.SetToolTip(Me.BtnUP, "Move up")
        Me.BtnUP.UseVisualStyleBackColor = True
        '
        'BtnDown
        '
        Me.BtnDown.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnDown.Font = New System.Drawing.Font("Wingdings 3", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnDown.Location = New System.Drawing.Point(779, 194)
        Me.BtnDown.Name = "BtnDown"
        Me.BtnDown.Size = New System.Drawing.Size(38, 30)
        Me.BtnDown.TabIndex = 38
        Me.BtnDown.Text = "q"
        Me.ToolTip1.SetToolTip(Me.BtnDown, "Move down")
        Me.BtnDown.UseVisualStyleBackColor = True
        '
        'BtnReloadLang
        '
        Me.BtnReloadLang.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnReloadLang.Font = New System.Drawing.Font("Wingdings 3", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnReloadLang.Location = New System.Drawing.Point(779, 82)
        Me.BtnReloadLang.Name = "BtnReloadLang"
        Me.BtnReloadLang.Size = New System.Drawing.Size(38, 30)
        Me.BtnReloadLang.TabIndex = 41
        Me.BtnReloadLang.Text = "P"
        Me.BtnReloadLang.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.BtnReloadLang, "Refresh")
        Me.BtnReloadLang.UseVisualStyleBackColor = True
        '
        'BtnDeleteLang
        '
        Me.BtnDeleteLang.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnDeleteLang.Font = New System.Drawing.Font("Wingdings 2", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnDeleteLang.ForeColor = System.Drawing.Color.Red
        Me.BtnDeleteLang.Location = New System.Drawing.Point(779, 252)
        Me.BtnDeleteLang.Name = "BtnDeleteLang"
        Me.BtnDeleteLang.Size = New System.Drawing.Size(38, 30)
        Me.BtnDeleteLang.TabIndex = 40
        Me.BtnDeleteLang.Text = "U"
        Me.ToolTip1.SetToolTip(Me.BtnDeleteLang, "Delete")
        Me.BtnDeleteLang.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusBar})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 573)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(840, 22)
        Me.StatusStrip1.TabIndex = 42
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'StatusBar
        '
        Me.StatusBar.Font = New System.Drawing.Font("BentonSans Regular", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusBar.Name = "StatusBar"
        Me.StatusBar.Size = New System.Drawing.Size(48, 17)
        Me.StatusBar.Text = "Status:"
        '
        'CmbLang
        '
        Me.CmbLang.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbLang.FormattingEnabled = True
        Me.CmbLang.Location = New System.Drawing.Point(360, 20)
        Me.CmbLang.Name = "CmbLang"
        Me.CmbLang.Size = New System.Drawing.Size(135, 21)
        Me.CmbLang.TabIndex = 44
        Me.CmbLang.Text = "[ALL}"
        Me.ToolTip1.SetToolTip(Me.CmbLang, "Current project languages loaded")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(264, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 13)
        Me.Label2.TabIndex = 43
        Me.Label2.Text = "Select Language"
        '
        'BtnStartCopying
        '
        Me.BtnStartCopying.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnStartCopying.Font = New System.Drawing.Font("BentonSans Bold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStartCopying.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.BtnStartCopying.Location = New System.Drawing.Point(659, 538)
        Me.BtnStartCopying.Name = "BtnStartCopying"
        Me.BtnStartCopying.Size = New System.Drawing.Size(168, 30)
        Me.BtnStartCopying.TabIndex = 45
        Me.BtnStartCopying.Text = "Copy Translation!"
        Me.ToolTip1.SetToolTip(Me.BtnStartCopying, "Start Copying")
        Me.BtnStartCopying.UseVisualStyleBackColor = True
        '
        'OutputList
        '
        Me.OutputList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.FileName, Me.Path})
        Me.OutputList.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OutputList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.OutputList.HideSelection = False
        Me.OutputList.Location = New System.Drawing.Point(15, 50)
        Me.OutputList.Name = "OutputList"
        Me.OutputList.Size = New System.Drawing.Size(749, 256)
        Me.OutputList.TabIndex = 46
        Me.ToolTip1.SetToolTip(Me.OutputList, "double click to open xliff file")
        Me.OutputList.UseCompatibleStateImageBehavior = False
        Me.OutputList.View = System.Windows.Forms.View.Details
        '
        'FileName
        '
        Me.FileName.Text = "File Name"
        Me.FileName.Width = 180
        '
        'Path
        '
        Me.Path.Text = "File Path"
        Me.Path.Width = 800
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(25, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(74, 13)
        Me.Label3.TabIndex = 43
        Me.Label3.Text = "Xliff input file"
        '
        'txtInputXlifffile
        '
        Me.txtInputXlifffile.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInputXlifffile.Location = New System.Drawing.Point(113, 30)
        Me.txtInputXlifffile.Name = "txtInputXlifffile"
        Me.txtInputXlifffile.Size = New System.Drawing.Size(651, 21)
        Me.txtInputXlifffile.TabIndex = 47
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(22, 64)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(85, 13)
        Me.Label4.TabIndex = 43
        Me.Label4.Text = "Output location"
        '
        'txtOutput
        '
        Me.txtOutput.Enabled = False
        Me.txtOutput.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOutput.Location = New System.Drawing.Point(113, 61)
        Me.txtOutput.Name = "txtOutput"
        Me.txtOutput.Size = New System.Drawing.Size(651, 21)
        Me.txtOutput.TabIndex = 47
        '
        'Btn1Browse
        '
        Me.Btn1Browse.Location = New System.Drawing.Point(779, 29)
        Me.Btn1Browse.Name = "Btn1Browse"
        Me.Btn1Browse.Size = New System.Drawing.Size(38, 23)
        Me.Btn1Browse.TabIndex = 48
        Me.Btn1Browse.Text = "..."
        Me.Btn1Browse.UseVisualStyleBackColor = True
        '
        'Btn2browse
        '
        Me.Btn2browse.Enabled = False
        Me.Btn2browse.Location = New System.Drawing.Point(779, 61)
        Me.Btn2browse.Name = "Btn2browse"
        Me.Btn2browse.Size = New System.Drawing.Size(38, 23)
        Me.Btn2browse.TabIndex = 48
        Me.Btn2browse.Text = "..."
        Me.Btn2browse.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BtnChangeFolder)
        Me.GroupBox1.Controls.Add(Me.BtnDown)
        Me.GroupBox1.Controls.Add(Me.BtnUP)
        Me.GroupBox1.Controls.Add(Me.BtnDeleteLang)
        Me.GroupBox1.Controls.Add(Me.OutputList)
        Me.GroupBox1.Controls.Add(Me.BtnReloadLang)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.ComboBox1)
        Me.GroupBox1.Controls.Add(Me.CmbLang)
        Me.GroupBox1.Font = New System.Drawing.Font("BentonSans Bold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(3, 213)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(833, 319)
        Me.GroupBox1.TabIndex = 49
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Step 3 - Translated xliff files for copying translation"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(520, 24)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(83, 13)
        Me.Label7.TabIndex = 43
        Me.Label7.Text = "File Preference"
        '
        'ComboBox1
        '
        Me.ComboBox1.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"Same filename first", "Take only same filename"})
        Me.ComboBox1.Location = New System.Drawing.Point(609, 20)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(153, 21)
        Me.ComboBox1.TabIndex = 44
        Me.ComboBox1.Text = "Same filename first"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Btn2browse)
        Me.GroupBox2.Controls.Add(Me.txtInputXlifffile)
        Me.GroupBox2.Controls.Add(Me.Btn1Browse)
        Me.GroupBox2.Controls.Add(Me.txtOutput)
        Me.GroupBox2.Font = New System.Drawing.Font("BentonSans Bold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(3, 107)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(833, 100)
        Me.GroupBox2.TabIndex = 50
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Step 2 - Input xliff file that needs translation"
        '
        'BW
        '
        Me.BW.WorkerReportsProgress = True
        Me.BW.WorkerSupportsCancellation = True
        '
        'ListView2
        '
        Me.ListView2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2})
        Me.ListView2.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ListView2.HideSelection = False
        Me.ListView2.Location = New System.Drawing.Point(-855, 210)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(245, 256)
        Me.ListView2.TabIndex = 46
        Me.ListView2.UseCompatibleStateImageBehavior = False
        Me.ListView2.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "File Name"
        Me.ColumnHeader2.Width = 180
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.ChkOutPut)
        Me.GroupBox3.Controls.Add(Me.RBMultiple)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.RBSingle)
        Me.GroupBox3.Font = New System.Drawing.Font("BentonSans Bold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(3, 50)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(833, 51)
        Me.GroupBox3.TabIndex = 51
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Step 1 - Select Process Type"
        '
        'ChkOutPut
        '
        Me.ChkOutPut.AutoSize = True
        Me.ChkOutPut.Location = New System.Drawing.Point(795, 22)
        Me.ChkOutPut.Name = "ChkOutPut"
        Me.ChkOutPut.Size = New System.Drawing.Size(15, 14)
        Me.ChkOutPut.TabIndex = 44
        Me.ChkOutPut.UseVisualStyleBackColor = True
        '
        'RBMultiple
        '
        Me.RBMultiple.AutoSize = True
        Me.RBMultiple.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RBMultiple.Location = New System.Drawing.Point(113, 19)
        Me.RBMultiple.Name = "RBMultiple"
        Me.RBMultiple.Size = New System.Drawing.Size(94, 17)
        Me.RBMultiple.TabIndex = 1
        Me.RBMultiple.Text = "Multiple Files"
        Me.RBMultiple.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(573, 22)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(216, 13)
        Me.Label5.TabIndex = 43
        Me.Label5.Text = "Optional: Enable\Disable Output location"
        '
        'RBSingle
        '
        Me.RBSingle.AutoSize = True
        Me.RBSingle.Checked = True
        Me.RBSingle.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RBSingle.Location = New System.Drawing.Point(25, 19)
        Me.RBSingle.Name = "RBSingle"
        Me.RBSingle.Size = New System.Drawing.Size(77, 17)
        Me.RBSingle.TabIndex = 0
        Me.RBSingle.TabStop = True
        Me.RBSingle.Text = "Single File"
        Me.RBSingle.UseVisualStyleBackColor = True
        '
        'Form_Xliff_to_Xliff
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(840, 595)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.ListView2)
        Me.Controls.Add(Me.BtnStartCopying)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_Xliff_to_Xliff"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Xliff to Xliff"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents BtnChangeFolder As System.Windows.Forms.Button
    Friend WithEvents BtnUP As System.Windows.Forms.Button
    Friend WithEvents BtnDown As System.Windows.Forms.Button
    Friend WithEvents BtnReloadLang As System.Windows.Forms.Button
    Friend WithEvents BtnDeleteLang As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents StatusBar As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents CmbLang As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents BtnStartCopying As System.Windows.Forms.Button
    Friend WithEvents OutputList As System.Windows.Forms.ListView
    Friend WithEvents FileName As System.Windows.Forms.ColumnHeader
    Friend WithEvents Path As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtInputXlifffile As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtOutput As System.Windows.Forms.TextBox
    Friend WithEvents Btn1Browse As System.Windows.Forms.Button
    Friend WithEvents Btn2browse As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents BW As System.ComponentModel.BackgroundWorker
    Friend WithEvents ListView2 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents ChkOutPut As System.Windows.Forms.CheckBox
    Friend WithEvents RBMultiple As System.Windows.Forms.RadioButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents RBSingle As System.Windows.Forms.RadioButton
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
End Class
