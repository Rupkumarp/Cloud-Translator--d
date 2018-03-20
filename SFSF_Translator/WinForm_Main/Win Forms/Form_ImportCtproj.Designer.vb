<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ImportCtproj
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ImportCtproj))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TextBox_Ctproj = New System.Windows.Forms.TextBox()
        Me.BtnBrowseCtproj = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox_ImportLocation = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox_ProjectGroupName = New System.Windows.Forms.TextBox()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(334, 274)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
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
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("BentonSans Bold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(20, 21)
        Me.Label6.Margin = New System.Windows.Forms.Padding(0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(137, 19)
        Me.Label6.TabIndex = 70
        Me.Label6.Text = "Import Ctroj file"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = CType(resources.GetObject("PictureBox1.InitialImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(-3, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(496, 42)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 69
        Me.PictureBox1.TabStop = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.Label5.Location = New System.Drawing.Point(24, 68)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(92, 13)
        Me.Label5.TabIndex = 73
        Me.Label5.Text = "Select Ctproj file"
        '
        'TextBox_Ctproj
        '
        Me.TextBox_Ctproj.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.TextBox_Ctproj.Location = New System.Drawing.Point(24, 87)
        Me.TextBox_Ctproj.Name = "TextBox_Ctproj"
        Me.TextBox_Ctproj.Size = New System.Drawing.Size(417, 21)
        Me.TextBox_Ctproj.TabIndex = 71
        '
        'BtnBrowseCtproj
        '
        Me.BtnBrowseCtproj.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnBrowseCtproj.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.BtnBrowseCtproj.Location = New System.Drawing.Point(450, 86)
        Me.BtnBrowseCtproj.Name = "BtnBrowseCtproj"
        Me.BtnBrowseCtproj.Size = New System.Drawing.Size(27, 22)
        Me.BtnBrowseCtproj.TabIndex = 72
        Me.BtnBrowseCtproj.Tag = "Select Project loaction"
        Me.BtnBrowseCtproj.Text = "..."
        Me.BtnBrowseCtproj.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.Label1.Location = New System.Drawing.Point(24, 135)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 13)
        Me.Label1.TabIndex = 76
        Me.Label1.Text = "Select Import Location"
        '
        'TextBox_ImportLocation
        '
        Me.TextBox_ImportLocation.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.TextBox_ImportLocation.Location = New System.Drawing.Point(24, 154)
        Me.TextBox_ImportLocation.Name = "TextBox_ImportLocation"
        Me.TextBox_ImportLocation.Size = New System.Drawing.Size(417, 21)
        Me.TextBox_ImportLocation.TabIndex = 74
        '
        'Button1
        '
        Me.Button1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button1.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.Button1.Location = New System.Drawing.Point(450, 153)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(27, 22)
        Me.Button1.TabIndex = 75
        Me.Button1.Tag = "Select Project loaction"
        Me.Button1.Text = "..."
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.Label2.Location = New System.Drawing.Point(24, 195)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(140, 13)
        Me.Label2.TabIndex = 78
        Me.Label2.Text = "Enter Project Group Name"
        '
        'TextBox_ProjectGroupName
        '
        Me.TextBox_ProjectGroupName.Font = New System.Drawing.Font("BentonSans Regular", 8.25!)
        Me.TextBox_ProjectGroupName.Location = New System.Drawing.Point(24, 214)
        Me.TextBox_ProjectGroupName.Name = "TextBox_ProjectGroupName"
        Me.TextBox_ProjectGroupName.Size = New System.Drawing.Size(417, 21)
        Me.TextBox_ProjectGroupName.TabIndex = 77
        '
        'Form_ImportCtproj
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(492, 315)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBox_ProjectGroupName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox_ImportLocation)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TextBox_Ctproj)
        Me.Controls.Add(Me.BtnBrowseCtproj)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ImportCtproj"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label6 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label5 As Label
    Friend WithEvents TextBox_Ctproj As TextBox
    Friend WithEvents BtnBrowseCtproj As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox_ImportLocation As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox_ProjectGroupName As TextBox
End Class
