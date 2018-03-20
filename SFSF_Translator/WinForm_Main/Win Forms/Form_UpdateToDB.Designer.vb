<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_UpdateToDB
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_UpdateToDB))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.RBMultiple = New System.Windows.Forms.RadioButton()
        Me.RBSingle = New System.Windows.Forms.RadioButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.CmbInstance = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CmbCustomer = New System.Windows.Forms.ComboBox()
        Me.txtInputXlifffile = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Btn1Browse = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BW = New System.ComponentModel.BackgroundWorker()
        Me.BtnImport = New System.Windows.Forms.Button()
        Me.Bw2 = New System.ComponentModel.BackgroundWorker()
        Me.Button1 = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = CType(resources.GetObject("PictureBox1.InitialImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(3, 2)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(768, 42)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 33
        Me.PictureBox1.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("BentonSans Bold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(51, 23)
        Me.Label6.Margin = New System.Windows.Forms.Padding(0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(245, 21)
        Me.Label6.TabIndex = 34
        Me.Label6.Text = "Update Xliff translation to DB"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.RBMultiple)
        Me.GroupBox3.Controls.Add(Me.RBSingle)
        Me.GroupBox3.Font = New System.Drawing.Font("BentonSans Bold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(4, 50)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(767, 51)
        Me.GroupBox3.TabIndex = 52
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Step 1 - Select Process Type"
        '
        'RBMultiple
        '
        Me.RBMultiple.AutoSize = True
        Me.RBMultiple.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RBMultiple.Location = New System.Drawing.Point(113, 19)
        Me.RBMultiple.Name = "RBMultiple"
        Me.RBMultiple.Size = New System.Drawing.Size(91, 18)
        Me.RBMultiple.TabIndex = 1
        Me.RBMultiple.Text = "Select Folder"
        Me.RBMultiple.UseVisualStyleBackColor = True
        '
        'RBSingle
        '
        Me.RBSingle.AutoSize = True
        Me.RBSingle.Checked = True
        Me.RBSingle.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RBSingle.Location = New System.Drawing.Point(25, 19)
        Me.RBSingle.Name = "RBSingle"
        Me.RBSingle.Size = New System.Drawing.Size(78, 18)
        Me.RBSingle.TabIndex = 0
        Me.RBSingle.TabStop = True
        Me.RBSingle.Text = "Select File"
        Me.RBSingle.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.TextBox4)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.CmbInstance)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.CmbCustomer)
        Me.GroupBox2.Controls.Add(Me.txtInputXlifffile)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.Btn1Browse)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Font = New System.Drawing.Font("BentonSans Bold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(4, 107)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(767, 236)
        Me.GroupBox2.TabIndex = 53
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Step - 2"
        '
        'TextBox4
        '
        Me.TextBox4.AcceptsReturn = True
        Me.TextBox4.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox4.Location = New System.Drawing.Point(21, 131)
        Me.TextBox4.Multiline = True
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(703, 88)
        Me.TextBox4.TabIndex = 43
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(21, 117)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(148, 14)
        Me.Label8.TabIndex = 42
        Me.Label8.Text = "Key Words for DB exclusion"
        '
        'CmbInstance
        '
        Me.CmbInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbInstance.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbInstance.FormattingEnabled = True
        Me.CmbInstance.Location = New System.Drawing.Point(113, 82)
        Me.CmbInstance.Name = "CmbInstance"
        Me.CmbInstance.Size = New System.Drawing.Size(607, 22)
        Me.CmbInstance.TabIndex = 57
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(25, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 14)
        Me.Label3.TabIndex = 43
        Me.Label3.Text = "Xliff input"
        '
        'CmbCustomer
        '
        Me.CmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbCustomer.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbCustomer.FormattingEnabled = True
        Me.CmbCustomer.Location = New System.Drawing.Point(113, 51)
        Me.CmbCustomer.Name = "CmbCustomer"
        Me.CmbCustomer.Size = New System.Drawing.Size(607, 22)
        Me.CmbCustomer.TabIndex = 56
        '
        'txtInputXlifffile
        '
        Me.txtInputXlifffile.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInputXlifffile.Location = New System.Drawing.Point(113, 21)
        Me.txtInputXlifffile.Name = "txtInputXlifffile"
        Me.txtInputXlifffile.Size = New System.Drawing.Size(607, 21)
        Me.txtInputXlifffile.TabIndex = 47
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(21, 86)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 14)
        Me.Label1.TabIndex = 55
        Me.Label1.Text = "Instance:"
        '
        'Btn1Browse
        '
        Me.Btn1Browse.Location = New System.Drawing.Point(726, 19)
        Me.Btn1Browse.Name = "Btn1Browse"
        Me.Btn1Browse.Size = New System.Drawing.Size(31, 23)
        Me.Btn1Browse.TabIndex = 48
        Me.Btn1Browse.Text = "..."
        Me.Btn1Browse.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(21, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 14)
        Me.Label2.TabIndex = 54
        Me.Label2.Text = "Customer:"
        '
        'BW
        '
        Me.BW.WorkerReportsProgress = True
        Me.BW.WorkerSupportsCancellation = True
        '
        'BtnImport
        '
        Me.BtnImport.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnImport.Font = New System.Drawing.Font("BentonSans Bold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnImport.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.BtnImport.Location = New System.Drawing.Point(540, 349)
        Me.BtnImport.Name = "BtnImport"
        Me.BtnImport.Size = New System.Drawing.Size(108, 30)
        Me.BtnImport.TabIndex = 54
        Me.BtnImport.Text = "Import!"
        Me.BtnImport.UseVisualStyleBackColor = True
        '
        'Bw2
        '
        Me.Bw2.WorkerReportsProgress = True
        Me.Bw2.WorkerSupportsCancellation = True
        '
        'Button1
        '
        Me.Button1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button1.Font = New System.Drawing.Font("BentonSans Bold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Button1.Location = New System.Drawing.Point(654, 349)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(108, 30)
        Me.Button1.TabIndex = 55
        Me.Button1.Text = "Cancel"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Form_UpdateToDB
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(774, 389)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.BtnImport)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "Form_UpdateToDB"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents RBMultiple As System.Windows.Forms.RadioButton
    Friend WithEvents RBSingle As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtInputXlifffile As System.Windows.Forms.TextBox
    Friend WithEvents Btn1Browse As System.Windows.Forms.Button
    Friend WithEvents CmbInstance As System.Windows.Forms.ComboBox
    Friend WithEvents CmbCustomer As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents BW As System.ComponentModel.BackgroundWorker
    Friend WithEvents BtnImport As System.Windows.Forms.Button
    Friend WithEvents Bw2 As System.ComponentModel.BackgroundWorker
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
