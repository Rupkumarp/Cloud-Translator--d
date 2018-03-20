<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MDF_Group
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
        Me.checkedListBox1 = New System.Windows.Forms.CheckedListBox()
        Me.radioButton1 = New System.Windows.Forms.RadioButton()
        Me.radioButton2 = New System.Windows.Forms.RadioButton()
        Me.radioButton3 = New System.Windows.Forms.RadioButton()
        Me.button1 = New System.Windows.Forms.Button()
        Me.label1 = New System.Windows.Forms.Label()
        Me.listGroupName = New System.Windows.Forms.ListBox()
        Me.textBox1 = New System.Windows.Forms.TextBox()
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.panel1 = New System.Windows.Forms.Panel()
        Me.statusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.toolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.label2 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.groupBox1.SuspendLayout()
        Me.panel1.SuspendLayout()
        Me.statusStrip1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'checkedListBox1
        '
        Me.checkedListBox1.CheckOnClick = True
        Me.checkedListBox1.FormattingEnabled = True
        Me.checkedListBox1.Location = New System.Drawing.Point(16, 58)
        Me.checkedListBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.checkedListBox1.Name = "checkedListBox1"
        Me.checkedListBox1.Size = New System.Drawing.Size(353, 497)
        Me.checkedListBox1.TabIndex = 18
        '
        'radioButton1
        '
        Me.radioButton1.AutoSize = True
        Me.radioButton1.Checked = True
        Me.radioButton1.Location = New System.Drawing.Point(23, 8)
        Me.radioButton1.Margin = New System.Windows.Forms.Padding(4)
        Me.radioButton1.Name = "radioButton1"
        Me.radioButton1.Size = New System.Drawing.Size(71, 21)
        Me.radioButton1.TabIndex = 0
        Me.radioButton1.TabStop = True
        Me.radioButton1.Text = "Create"
        Me.radioButton1.UseVisualStyleBackColor = True
        '
        'radioButton2
        '
        Me.radioButton2.AutoSize = True
        Me.radioButton2.Location = New System.Drawing.Point(116, 8)
        Me.radioButton2.Margin = New System.Windows.Forms.Padding(4)
        Me.radioButton2.Name = "radioButton2"
        Me.radioButton2.Size = New System.Drawing.Size(75, 21)
        Me.radioButton2.TabIndex = 1
        Me.radioButton2.TabStop = True
        Me.radioButton2.Text = "Update"
        Me.radioButton2.UseVisualStyleBackColor = True
        '
        'radioButton3
        '
        Me.radioButton3.AutoSize = True
        Me.radioButton3.Location = New System.Drawing.Point(218, 8)
        Me.radioButton3.Margin = New System.Windows.Forms.Padding(4)
        Me.radioButton3.Name = "radioButton3"
        Me.radioButton3.Size = New System.Drawing.Size(70, 21)
        Me.radioButton3.TabIndex = 2
        Me.radioButton3.TabStop = True
        Me.radioButton3.Text = "Delete"
        Me.radioButton3.UseVisualStyleBackColor = True
        '
        'button1
        '
        Me.button1.Location = New System.Drawing.Point(169, 448)
        Me.button1.Margin = New System.Windows.Forms.Padding(4)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(155, 28)
        Me.button1.TabIndex = 6
        Me.button1.Text = "Update"
        Me.button1.UseVisualStyleBackColor = True
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(17, 116)
        Me.label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(89, 17)
        Me.label1.TabIndex = 3
        Me.label1.Text = "Group Name"
        '
        'listGroupName
        '
        Me.listGroupName.FormattingEnabled = True
        Me.listGroupName.ItemHeight = 16
        Me.listGroupName.Location = New System.Drawing.Point(21, 165)
        Me.listGroupName.Margin = New System.Windows.Forms.Padding(4)
        Me.listGroupName.Name = "listGroupName"
        Me.listGroupName.Size = New System.Drawing.Size(301, 276)
        Me.listGroupName.TabIndex = 5
        '
        'textBox1
        '
        Me.textBox1.Location = New System.Drawing.Point(21, 135)
        Me.textBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New System.Drawing.Size(301, 22)
        Me.textBox1.TabIndex = 4
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.panel1)
        Me.groupBox1.Controls.Add(Me.button1)
        Me.groupBox1.Controls.Add(Me.label1)
        Me.groupBox1.Controls.Add(Me.listGroupName)
        Me.groupBox1.Controls.Add(Me.textBox1)
        Me.groupBox1.Location = New System.Drawing.Point(401, 58)
        Me.groupBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Padding = New System.Windows.Forms.Padding(4)
        Me.groupBox1.Size = New System.Drawing.Size(349, 496)
        Me.groupBox1.TabIndex = 19
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "MDF Group"
        '
        'panel1
        '
        Me.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panel1.Controls.Add(Me.radioButton1)
        Me.panel1.Controls.Add(Me.radioButton2)
        Me.panel1.Controls.Add(Me.radioButton3)
        Me.panel1.Location = New System.Drawing.Point(20, 33)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(302, 40)
        Me.panel1.TabIndex = 13
        '
        'statusStrip1
        '
        Me.statusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.statusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.toolStripStatusLabel1})
        Me.statusStrip1.Location = New System.Drawing.Point(0, 586)
        Me.statusStrip1.Name = "statusStrip1"
        Me.statusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 19, 0)
        Me.statusStrip1.Size = New System.Drawing.Size(770, 25)
        Me.statusStrip1.TabIndex = 20
        Me.statusStrip1.Text = "statusStrip1"
        '
        'toolStripStatusLabel1
        '
        Me.toolStripStatusLabel1.Name = "toolStripStatusLabel1"
        Me.toolStripStatusLabel1.Size = New System.Drawing.Size(52, 20)
        Me.toolStripStatusLabel1.Text = "Status:"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(17, 37)
        Me.label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(100, 17)
        Me.label2.TabIndex = 21
        Me.label2.Text = "MDF object list"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackgroundImage = Global.CloudTranslator.My.Resources.Resources.sapband
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(770, 16)
        Me.PictureBox1.TabIndex = 22
        Me.PictureBox1.TabStop = False
        '
        'MDF_Group
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(770, 611)
        Me.Controls.Add(Me.checkedListBox1)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.statusStrip1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.label2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "MDF_Group"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "MDF Group"
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.panel1.ResumeLayout(False)
        Me.panel1.PerformLayout()
        Me.statusStrip1.ResumeLayout(False)
        Me.statusStrip1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents checkedListBox1 As CheckedListBox
    Private WithEvents radioButton1 As RadioButton
    Private WithEvents radioButton2 As RadioButton
    Private WithEvents radioButton3 As RadioButton
    Private WithEvents button1 As Button
    Private WithEvents label1 As Label
    Private WithEvents listGroupName As ListBox
    Private WithEvents textBox1 As TextBox
    Private WithEvents groupBox1 As GroupBox
    Private WithEvents panel1 As Panel
    Private WithEvents statusStrip1 As StatusStrip
    Private WithEvents toolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents PictureBox1 As PictureBox
    Private WithEvents label2 As Label
End Class
