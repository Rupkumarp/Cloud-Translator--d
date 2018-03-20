<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_CreateTARZ
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
        Me.grp_folder = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lbl_Dir2maketarz = New System.Windows.Forms.Label()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.txt_TarzName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbl_Path4tarz = New System.Windows.Forms.Label()
        Me.link_InputFolder = New System.Windows.Forms.LinkLabel()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.FolderBrowserDialog2 = New System.Windows.Forms.FolderBrowserDialog()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.grp_folder.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grp_folder
        '
        Me.grp_folder.Controls.Add(Me.Button1)
        Me.grp_folder.Controls.Add(Me.lbl_Dir2maketarz)
        Me.grp_folder.Controls.Add(Me.LinkLabel1)
        Me.grp_folder.Controls.Add(Me.txt_TarzName)
        Me.grp_folder.Controls.Add(Me.Label1)
        Me.grp_folder.Controls.Add(Me.lbl_Path4tarz)
        Me.grp_folder.Controls.Add(Me.link_InputFolder)
        Me.grp_folder.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_folder.Location = New System.Drawing.Point(3, 14)
        Me.grp_folder.Margin = New System.Windows.Forms.Padding(4)
        Me.grp_folder.Name = "grp_folder"
        Me.grp_folder.Padding = New System.Windows.Forms.Padding(4)
        Me.grp_folder.Size = New System.Drawing.Size(903, 119)
        Me.grp_folder.TabIndex = 15
        Me.grp_folder.TabStop = False
        Me.grp_folder.Text = "Create .tar.gz folder for DIBO import"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(784, 81)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(98, 23)
        Me.Button1.TabIndex = 14
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'lbl_Dir2maketarz
        '
        Me.lbl_Dir2maketarz.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_Dir2maketarz.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Dir2maketarz.Location = New System.Drawing.Point(179, 25)
        Me.lbl_Dir2maketarz.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_Dir2maketarz.Name = "lbl_Dir2maketarz"
        Me.lbl_Dir2maketarz.Size = New System.Drawing.Size(703, 24)
        Me.lbl_Dir2maketarz.TabIndex = 6
        Me.lbl_Dir2maketarz.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel1.Location = New System.Drawing.Point(19, 29)
        Me.LinkLabel1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(152, 17)
        Me.LinkLabel1.TabIndex = 5
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Directory to make tarz"
        '
        'txt_TarzName
        '
        Me.txt_TarzName.Location = New System.Drawing.Point(179, 84)
        Me.txt_TarzName.Name = "txt_TarzName"
        Me.txt_TarzName.Size = New System.Drawing.Size(585, 24)
        Me.txt_TarzName.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 87)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(138, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Name of the tarz file"
        '
        'lbl_Path4tarz
        '
        Me.lbl_Path4tarz.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_Path4tarz.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Path4tarz.Location = New System.Drawing.Point(179, 54)
        Me.lbl_Path4tarz.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_Path4tarz.Name = "lbl_Path4tarz"
        Me.lbl_Path4tarz.Size = New System.Drawing.Size(703, 24)
        Me.lbl_Path4tarz.TabIndex = 2
        Me.lbl_Path4tarz.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'link_InputFolder
        '
        Me.link_InputFolder.AutoSize = True
        Me.link_InputFolder.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.link_InputFolder.Location = New System.Drawing.Point(19, 58)
        Me.link_InputFolder.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.link_InputFolder.Name = "link_InputFolder"
        Me.link_InputFolder.Size = New System.Drawing.Size(127, 17)
        Me.link_InputFolder.TabIndex = 0
        Me.link_InputFolder.TabStop = True
        Me.link_InputFolder.Text = "Path to create tarz"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackgroundImage = Global.CloudTranslator.My.Resources.Resources.sapband
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(-1, -3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(913, 10)
        Me.PictureBox1.TabIndex = 14
        Me.PictureBox1.TabStop = False
        '
        'Form_CreateTARZ
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(910, 139)
        Me.Controls.Add(Me.grp_folder)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_CreateTARZ"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Create TARZ"
        Me.grp_folder.ResumeLayout(False)
        Me.grp_folder.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grp_folder As GroupBox
    Friend WithEvents Button1 As Button
    Friend WithEvents lbl_Dir2maketarz As Label
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents txt_TarzName As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lbl_Path4tarz As Label
    Friend WithEvents link_InputFolder As LinkLabel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents FolderBrowserDialog2 As FolderBrowserDialog
End Class
