<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_SearchCorrect
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_SearchCorrect))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkFullStringOnly = New System.Windows.Forms.CheckBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtSearchTerm = New System.Windows.Forms.TextBox()
        Me.btnBrowseFolderPath = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtFolderPath = New System.Windows.Forms.TextBox()
        Me.cboLanguage = New System.Windows.Forms.ComboBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.grdSearchResult = New System.Windows.Forms.DataGridView()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.btnPaste = New System.Windows.Forms.Button()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.btnReplaceTerm = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtReplaceTerm = New System.Windows.Forms.TextBox()
        Me.rdBoth = New System.Windows.Forms.RadioButton()
        Me.rdTarget = New System.Windows.Forms.RadioButton()
        Me.rdSource = New System.Windows.Forms.RadioButton()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.grdSearchResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(-1, -1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(1202, 104)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(1017, 19)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(142, 21)
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rdBoth)
        Me.GroupBox1.Controls.Add(Me.chkFullStringOnly)
        Me.GroupBox1.Controls.Add(Me.rdTarget)
        Me.GroupBox1.Controls.Add(Me.Button3)
        Me.GroupBox1.Controls.Add(Me.rdSource)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtSearchTerm)
        Me.GroupBox1.Controls.Add(Me.btnBrowseFolderPath)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtFolderPath)
        Me.GroupBox1.Controls.Add(Me.cboLanguage)
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 118)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1176, 107)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "1. Search"
        '
        'chkFullStringOnly
        '
        Me.chkFullStringOnly.AutoSize = True
        Me.chkFullStringOnly.Location = New System.Drawing.Point(1017, 56)
        Me.chkFullStringOnly.Name = "chkFullStringOnly"
        Me.chkFullStringOnly.Size = New System.Drawing.Size(102, 17)
        Me.chkFullStringOnly.TabIndex = 11
        Me.chkFullStringOnly.Text = "Full String Only"
        Me.chkFullStringOnly.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button3.Font = New System.Drawing.Font("BentonSans Regular", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.Image = CType(resources.GetObject("Button3.Image"), System.Drawing.Image)
        Me.Button3.Location = New System.Drawing.Point(940, 56)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(21, 20)
        Me.Button3.TabIndex = 10
        Me.Button3.Text = "..."
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(360, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Search term:"
        '
        'txtSearchTerm
        '
        Me.txtSearchTerm.Location = New System.Drawing.Point(448, 56)
        Me.txtSearchTerm.Name = "txtSearchTerm"
        Me.txtSearchTerm.Size = New System.Drawing.Size(486, 21)
        Me.txtSearchTerm.TabIndex = 8
        '
        'btnBrowseFolderPath
        '
        Me.btnBrowseFolderPath.Font = New System.Drawing.Font("BentonSans Regular", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseFolderPath.Location = New System.Drawing.Point(940, 20)
        Me.btnBrowseFolderPath.Name = "btnBrowseFolderPath"
        Me.btnBrowseFolderPath.Size = New System.Drawing.Size(21, 20)
        Me.btnBrowseFolderPath.TabIndex = 7
        Me.btnBrowseFolderPath.Text = "..."
        Me.btnBrowseFolderPath.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(70, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Language:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(87, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Folder:"
        '
        'txtFolderPath
        '
        Me.txtFolderPath.Location = New System.Drawing.Point(135, 20)
        Me.txtFolderPath.Name = "txtFolderPath"
        Me.txtFolderPath.Size = New System.Drawing.Size(799, 21)
        Me.txtFolderPath.TabIndex = 4
        '
        'cboLanguage
        '
        Me.cboLanguage.FormattingEnabled = True
        Me.cboLanguage.Location = New System.Drawing.Point(135, 56)
        Me.cboLanguage.Name = "cboLanguage"
        Me.cboLanguage.Size = New System.Drawing.Size(192, 21)
        Me.cboLanguage.TabIndex = 3
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.grdSearchResult)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 231)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1176, 293)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "2. Select"
        '
        'grdSearchResult
        '
        Me.grdSearchResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdSearchResult.Location = New System.Drawing.Point(19, 20)
        Me.grdSearchResult.Name = "grdSearchResult"
        Me.grdSearchResult.Size = New System.Drawing.Size(1140, 257)
        Me.grdSearchResult.TabIndex = 0
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 667)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1200, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(1000, 17)
        Me.ToolStripStatusLabel1.Text = "Idle..."
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.AutoSize = False
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(100, 16)
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnPaste)
        Me.GroupBox3.Controls.Add(Me.RichTextBox1)
        Me.GroupBox3.Controls.Add(Me.btnReplaceTerm)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.txtReplaceTerm)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 530)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(1176, 105)
        Me.GroupBox3.TabIndex = 6
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "3. Replace"
        '
        'btnPaste
        '
        Me.btnPaste.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnPaste.Font = New System.Drawing.Font("BentonSans Regular", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPaste.Image = CType(resources.GetObject("btnPaste.Image"), System.Drawing.Image)
        Me.btnPaste.Location = New System.Drawing.Point(598, 25)
        Me.btnPaste.Name = "btnPaste"
        Me.btnPaste.Size = New System.Drawing.Size(21, 20)
        Me.btnPaste.TabIndex = 16
        Me.btnPaste.Text = "..."
        Me.btnPaste.UseVisualStyleBackColor = True
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Location = New System.Drawing.Point(625, 21)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(533, 67)
        Me.RichTextBox1.TabIndex = 14
        Me.RichTextBox1.Text = ""
        '
        'btnReplaceTerm
        '
        Me.btnReplaceTerm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnReplaceTerm.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.btnReplaceTerm.Location = New System.Drawing.Point(450, 55)
        Me.btnReplaceTerm.Name = "btnReplaceTerm"
        Me.btnReplaceTerm.Size = New System.Drawing.Size(142, 21)
        Me.btnReplaceTerm.TabIndex = 13
        Me.btnReplaceTerm.Text = "&Replace"
        Me.btnReplaceTerm.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(18, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(78, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Replace term:"
        '
        'txtReplaceTerm
        '
        Me.txtReplaceTerm.Location = New System.Drawing.Point(106, 24)
        Me.txtReplaceTerm.Name = "txtReplaceTerm"
        Me.txtReplaceTerm.Size = New System.Drawing.Size(486, 21)
        Me.txtReplaceTerm.TabIndex = 11
        '
        'rdBoth
        '
        Me.rdBoth.AutoSize = True
        Me.rdBoth.Location = New System.Drawing.Point(881, 85)
        Me.rdBoth.Name = "rdBoth"
        Me.rdBoth.Size = New System.Drawing.Size(49, 17)
        Me.rdBoth.TabIndex = 17
        Me.rdBoth.TabStop = True
        Me.rdBoth.Text = "Both"
        Me.rdBoth.UseVisualStyleBackColor = True
        '
        'rdTarget
        '
        Me.rdTarget.AutoSize = True
        Me.rdTarget.Location = New System.Drawing.Point(788, 85)
        Me.rdTarget.Name = "rdTarget"
        Me.rdTarget.Size = New System.Drawing.Size(57, 17)
        Me.rdTarget.TabIndex = 16
        Me.rdTarget.TabStop = True
        Me.rdTarget.Text = "Target"
        Me.rdTarget.UseVisualStyleBackColor = True
        '
        'rdSource
        '
        Me.rdSource.AutoSize = True
        Me.rdSource.Location = New System.Drawing.Point(692, 85)
        Me.rdSource.Name = "rdSource"
        Me.rdSource.Size = New System.Drawing.Size(60, 17)
        Me.rdSource.TabIndex = 15
        Me.rdSource.TabStop = True
        Me.rdSource.Text = "Source"
        Me.rdSource.UseVisualStyleBackColor = True
        '
        'Form_SearchCorrect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1200, 689)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form_SearchCorrect"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.grdSearchResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtSearchTerm As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseFolderPath As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtFolderPath As System.Windows.Forms.TextBox
    Friend WithEvents cboLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents chkFullStringOnly As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents grdSearchResult As System.Windows.Forms.DataGridView
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents btnReplaceTerm As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtReplaceTerm As System.Windows.Forms.TextBox
    Friend WithEvents btnPaste As System.Windows.Forms.Button
    Friend WithEvents rdBoth As System.Windows.Forms.RadioButton
    Friend WithEvents rdTarget As System.Windows.Forms.RadioButton
    Friend WithEvents rdSource As System.Windows.Forms.RadioButton

End Class
