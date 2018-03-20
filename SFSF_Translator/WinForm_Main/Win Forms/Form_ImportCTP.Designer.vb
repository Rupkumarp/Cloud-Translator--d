<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ImportCTP
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ImportCTP))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.BtnImport = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BtnBrowseCTPfile = New System.Windows.Forms.Button()
        Me.TextBox_CTPfile = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CmbInstance = New System.Windows.Forms.ComboBox()
        Me.CmbCustomer = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBox_ProjectLocation = New System.Windows.Forms.TextBox()
        Me.BtnBrowseProjectLocation = New System.Windows.Forms.Button()
        Me.TextBox_ProjectName = New System.Windows.Forms.TextBox()
        Me.LbProjectGroup = New System.Windows.Forms.ListBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.BW = New System.ComponentModel.BackgroundWorker()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.BtnImport, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(531, 392)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'BtnImport
        '
        Me.BtnImport.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.BtnImport.Location = New System.Drawing.Point(3, 3)
        Me.BtnImport.Name = "BtnImport"
        Me.BtnImport.Size = New System.Drawing.Size(67, 23)
        Me.BtnImport.TabIndex = 0
        Me.BtnImport.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.BtnBrowseCTPfile)
        Me.GroupBox2.Controls.Add(Me.TextBox_CTPfile)
        Me.GroupBox2.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(184, 69)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(493, 67)
        Me.GroupBox2.TabIndex = 71
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Step1 : Input CTP file"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(15, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 13)
        Me.Label2.TabIndex = 53
        Me.Label2.Text = "CTP file"
        '
        'BtnBrowseCTPfile
        '
        Me.BtnBrowseCTPfile.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnBrowseCTPfile.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.BtnBrowseCTPfile.Location = New System.Drawing.Point(441, 31)
        Me.BtnBrowseCTPfile.Name = "BtnBrowseCTPfile"
        Me.BtnBrowseCTPfile.Size = New System.Drawing.Size(27, 22)
        Me.BtnBrowseCTPfile.TabIndex = 56
        Me.BtnBrowseCTPfile.Tag = "Select CTP file"
        Me.BtnBrowseCTPfile.Text = "..."
        Me.BtnBrowseCTPfile.UseVisualStyleBackColor = True
        '
        'TextBox_CTPfile
        '
        Me.TextBox_CTPfile.Enabled = False
        Me.TextBox_CTPfile.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.TextBox_CTPfile.Location = New System.Drawing.Point(18, 32)
        Me.TextBox_CTPfile.Name = "TextBox_CTPfile"
        Me.TextBox_CTPfile.Size = New System.Drawing.Size(414, 21)
        Me.TextBox_CTPfile.TabIndex = 47
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.CmbInstance)
        Me.GroupBox1.Controls.Add(Me.CmbCustomer)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.TextBox_ProjectLocation)
        Me.GroupBox1.Controls.Add(Me.BtnBrowseProjectLocation)
        Me.GroupBox1.Controls.Add(Me.TextBox_ProjectName)
        Me.GroupBox1.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(184, 142)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(493, 233)
        Me.GroupBox1.TabIndex = 70
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Step 2: Update Details"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(15, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 59
        Me.Label1.Text = "Project Name"
        '
        'CmbInstance
        '
        Me.CmbInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbInstance.FormattingEnabled = True
        Me.CmbInstance.Location = New System.Drawing.Point(15, 198)
        Me.CmbInstance.Name = "CmbInstance"
        Me.CmbInstance.Size = New System.Drawing.Size(418, 21)
        Me.CmbInstance.TabIndex = 64
        '
        'CmbCustomer
        '
        Me.CmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbCustomer.FormattingEnabled = True
        Me.CmbCustomer.Location = New System.Drawing.Point(15, 143)
        Me.CmbCustomer.Name = "CmbCustomer"
        Me.CmbCustomer.Size = New System.Drawing.Size(417, 21)
        Me.CmbCustomer.TabIndex = 63
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(15, 175)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 62
        Me.Label3.Text = "Instance:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.Label5.Location = New System.Drawing.Point(15, 69)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(89, 13)
        Me.Label5.TabIndex = 51
        Me.Label5.Text = "Project Location"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(15, 122)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 13)
        Me.Label4.TabIndex = 61
        Me.Label4.Text = "Customer:"
        '
        'TextBox_ProjectLocation
        '
        Me.TextBox_ProjectLocation.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.TextBox_ProjectLocation.Location = New System.Drawing.Point(15, 88)
        Me.TextBox_ProjectLocation.Name = "TextBox_ProjectLocation"
        Me.TextBox_ProjectLocation.Size = New System.Drawing.Size(417, 21)
        Me.TextBox_ProjectLocation.TabIndex = 48
        '
        'BtnBrowseProjectLocation
        '
        Me.BtnBrowseProjectLocation.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnBrowseProjectLocation.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.BtnBrowseProjectLocation.Location = New System.Drawing.Point(441, 87)
        Me.BtnBrowseProjectLocation.Name = "BtnBrowseProjectLocation"
        Me.BtnBrowseProjectLocation.Size = New System.Drawing.Size(27, 22)
        Me.BtnBrowseProjectLocation.TabIndex = 49
        Me.BtnBrowseProjectLocation.Tag = "Select Project loaction"
        Me.BtnBrowseProjectLocation.Text = "..."
        Me.BtnBrowseProjectLocation.UseVisualStyleBackColor = True
        '
        'TextBox_ProjectName
        '
        Me.TextBox_ProjectName.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.TextBox_ProjectName.Location = New System.Drawing.Point(15, 33)
        Me.TextBox_ProjectName.Name = "TextBox_ProjectName"
        Me.TextBox_ProjectName.Size = New System.Drawing.Size(417, 21)
        Me.TextBox_ProjectName.TabIndex = 58
        '
        'LbProjectGroup
        '
        Me.LbProjectGroup.FormattingEnabled = True
        Me.LbProjectGroup.Location = New System.Drawing.Point(11, 69)
        Me.LbProjectGroup.Name = "LbProjectGroup"
        Me.LbProjectGroup.Size = New System.Drawing.Size(161, 303)
        Me.LbProjectGroup.TabIndex = 69
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("BentonSans Bold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(24, 30)
        Me.Label6.Margin = New System.Windows.Forms.Padding(0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(129, 19)
        Me.Label6.TabIndex = 68
        Me.Label6.Text = "Import CTP file"
        '
        'BW
        '
        Me.BW.WorkerReportsProgress = True
        Me.BW.WorkerSupportsCancellation = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = CType(resources.GetObject("PictureBox1.InitialImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(1, 9)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(687, 42)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 67
        Me.PictureBox1.TabStop = False
        '
        'Form_ImportCTP
        '
        Me.AcceptButton = Me.BtnImport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(689, 433)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.LbProjectGroup)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ImportCTP"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents BtnImport As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents BtnBrowseCTPfile As System.Windows.Forms.Button
    Friend WithEvents TextBox_CTPfile As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CmbInstance As System.Windows.Forms.ComboBox
    Friend WithEvents CmbCustomer As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextBox_ProjectLocation As System.Windows.Forms.TextBox
    Friend WithEvents BtnBrowseProjectLocation As System.Windows.Forms.Button
    Friend WithEvents TextBox_ProjectName As System.Windows.Forms.TextBox
    Friend WithEvents LbProjectGroup As System.Windows.Forms.ListBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents BW As System.ComponentModel.BackgroundWorker
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox

End Class
