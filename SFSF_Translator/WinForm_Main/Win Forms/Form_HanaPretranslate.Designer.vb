<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_HanaPretranslate
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_HanaPretranslate))
        Me.BtnDown = New System.Windows.Forms.Button()
        Me.RB_Skip = New System.Windows.Forms.RadioButton()
        Me.RB_Append = New System.Windows.Forms.RadioButton()
        Me.RB_New = New System.Windows.Forms.RadioButton()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStipUpdateTransId = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.BtnUP = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.BtnPretranslate = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LB_Lang = New System.Windows.Forms.ListBox()
        Me.LB_SourceID = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LB_FilestoPretranslate = New System.Windows.Forms.ListBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.BW = New System.ComponentModel.BackgroundWorker()
        Me.GroupBox1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnDown
        '
        Me.BtnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnDown.Font = New System.Drawing.Font("Wingdings 3", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnDown.Location = New System.Drawing.Point(502, 214)
        Me.BtnDown.Name = "BtnDown"
        Me.BtnDown.Size = New System.Drawing.Size(28, 36)
        Me.BtnDown.TabIndex = 27
        Me.BtnDown.Text = "q"
        Me.BtnDown.UseVisualStyleBackColor = True
        '
        'RB_Skip
        '
        Me.RB_Skip.AutoSize = True
        Me.RB_Skip.Font = New System.Drawing.Font("BentonSans Book", 8.25!)
        Me.RB_Skip.Location = New System.Drawing.Point(239, 20)
        Me.RB_Skip.Name = "RB_Skip"
        Me.RB_Skip.Size = New System.Drawing.Size(107, 17)
        Me.RB_Skip.TabIndex = 14
        Me.RB_Skip.Text = "Skip if file exists"
        Me.RB_Skip.UseVisualStyleBackColor = True
        '
        'RB_Append
        '
        Me.RB_Append.AutoSize = True
        Me.RB_Append.Font = New System.Drawing.Font("BentonSans Book", 8.25!)
        Me.RB_Append.Location = New System.Drawing.Point(114, 20)
        Me.RB_Append.Name = "RB_Append"
        Me.RB_Append.Size = New System.Drawing.Size(119, 17)
        Me.RB_Append.TabIndex = 13
        Me.RB_Append.Text = "Append to existing"
        Me.RB_Append.UseVisualStyleBackColor = True
        '
        'RB_New
        '
        Me.RB_New.AutoSize = True
        Me.RB_New.Checked = True
        Me.RB_New.Font = New System.Drawing.Font("BentonSans Book", 8.25!)
        Me.RB_New.Location = New System.Drawing.Point(10, 20)
        Me.RB_New.Name = "RB_New"
        Me.RB_New.Size = New System.Drawing.Size(98, 17)
        Me.RB_New.TabIndex = 12
        Me.RB_New.TabStop = True
        Me.RB_New.Text = "Create new file"
        Me.RB_New.UseVisualStyleBackColor = True
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(100, 16)
        '
        'ToolStipUpdateTransId
        '
        Me.ToolStipUpdateTransId.Font = New System.Drawing.Font("BentonSans Book", 8.25!)
        Me.ToolStipUpdateTransId.Name = "ToolStipUpdateTransId"
        Me.ToolStipUpdateTransId.Size = New System.Drawing.Size(27, 17)
        Me.ToolStipUpdateTransId.Text = "0/0"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.Font = New System.Drawing.Font("BentonSans Book", 8.25!)
        Me.ToolStripStatusLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(620, 17)
        Me.ToolStripStatusLabel1.Text = "Idle"
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'BtnUP
        '
        Me.BtnUP.Font = New System.Drawing.Font("Wingdings 3", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnUP.Location = New System.Drawing.Point(502, 77)
        Me.BtnUP.Name = "BtnUP"
        Me.BtnUP.Size = New System.Drawing.Size(28, 36)
        Me.BtnUP.TabIndex = 26
        Me.BtnUP.Text = "p"
        Me.BtnUP.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("BentonSans Book", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(12, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(186, 24)
        Me.Label4.TabIndex = 25
        Me.Label4.Text = "DB Pretranslate"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.RB_Skip)
        Me.GroupBox1.Controls.Add(Me.RB_Append)
        Me.GroupBox1.Controls.Add(Me.RB_New)
        Me.GroupBox1.Font = New System.Drawing.Font("BentonSans Book", 8.25!)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 257)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(668, 48)
        Me.GroupBox1.TabIndex = 24
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Pretranslate File options"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StatusStrip1.AutoSize = False
        Me.StatusStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripProgressBar1, Me.ToolStipUpdateTransId})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 331)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(804, 22)
        Me.StatusStrip1.TabIndex = 23
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'BtnPretranslate
        '
        Me.BtnPretranslate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnPretranslate.Font = New System.Drawing.Font("BentonSans Book", 8.25!)
        Me.BtnPretranslate.Location = New System.Drawing.Point(687, 268)
        Me.BtnPretranslate.Name = "BtnPretranslate"
        Me.BtnPretranslate.Size = New System.Drawing.Size(105, 37)
        Me.BtnPretranslate.TabIndex = 22
        Me.BtnPretranslate.Text = "PreTranslate"
        Me.BtnPretranslate.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("BentonSans Book", 8.25!)
        Me.Label3.Location = New System.Drawing.Point(544, 61)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 13)
        Me.Label3.TabIndex = 21
        Me.Label3.Text = "Languages (#)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("BentonSans Book", 8.25!)
        Me.Label2.Location = New System.Drawing.Point(276, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 13)
        Me.Label2.TabIndex = 20
        Me.Label2.Text = "Source IDs (#)"
        '
        'LB_Lang
        '
        Me.LB_Lang.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LB_Lang.Font = New System.Drawing.Font("BentonSans Book", 8.25!)
        Me.LB_Lang.FormattingEnabled = True
        Me.LB_Lang.Location = New System.Drawing.Point(547, 77)
        Me.LB_Lang.Name = "LB_Lang"
        Me.LB_Lang.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.LB_Lang.Size = New System.Drawing.Size(245, 173)
        Me.LB_Lang.TabIndex = 19
        '
        'LB_SourceID
        '
        Me.LB_SourceID.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LB_SourceID.Font = New System.Drawing.Font("BentonSans Book", 8.25!)
        Me.LB_SourceID.FormattingEnabled = True
        Me.LB_SourceID.HorizontalScrollbar = True
        Me.LB_SourceID.Location = New System.Drawing.Point(279, 77)
        Me.LB_SourceID.Name = "LB_SourceID"
        Me.LB_SourceID.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.LB_SourceID.Size = New System.Drawing.Size(222, 173)
        Me.LB_SourceID.TabIndex = 18
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("BentonSans Book", 8.25!)
        Me.Label1.Location = New System.Drawing.Point(13, 61)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(125, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Files to pretranslate (#)"
        '
        'LB_FilestoPretranslate
        '
        Me.LB_FilestoPretranslate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LB_FilestoPretranslate.Font = New System.Drawing.Font("BentonSans Book", 8.25!)
        Me.LB_FilestoPretranslate.FormattingEnabled = True
        Me.LB_FilestoPretranslate.Location = New System.Drawing.Point(13, 77)
        Me.LB_FilestoPretranslate.Name = "LB_FilestoPretranslate"
        Me.LB_FilestoPretranslate.Size = New System.Drawing.Size(245, 173)
        Me.LB_FilestoPretranslate.TabIndex = 16
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = CType(resources.GetObject("PictureBox1.InitialImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, -6)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(804, 42)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 33
        Me.PictureBox1.TabStop = False
        '
        'BW
        '
        Me.BW.WorkerReportsProgress = True
        Me.BW.WorkerSupportsCancellation = True
        '
        'Form_HanaPretranslate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(804, 352)
        Me.Controls.Add(Me.BtnDown)
        Me.Controls.Add(Me.BtnUP)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.BtnPretranslate)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LB_Lang)
        Me.Controls.Add(Me.LB_SourceID)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LB_FilestoPretranslate)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "Form_HanaPretranslate"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnDown As System.Windows.Forms.Button
    Friend WithEvents RB_Skip As System.Windows.Forms.RadioButton
    Friend WithEvents RB_Append As System.Windows.Forms.RadioButton
    Friend WithEvents RB_New As System.Windows.Forms.RadioButton
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents ToolStipUpdateTransId As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents BtnUP As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents BtnPretranslate As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LB_Lang As System.Windows.Forms.ListBox
    Friend WithEvents LB_SourceID As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LB_FilestoPretranslate As System.Windows.Forms.ListBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents BW As System.ComponentModel.BackgroundWorker
End Class
