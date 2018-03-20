<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Cleaning
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_Cleaning))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.lstCleaned = New System.Windows.Forms.ListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.LstTR = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.lstTRback = New System.Windows.Forms.ListBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.lstPre = New System.Windows.Forms.ListBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.lstOut = New System.Windows.Forms.ListBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Location = New System.Drawing.Point(12, 28)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(973, 430)
        Me.TabControl1.TabIndex = 6
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.lstCleaned)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(965, 404)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Cleaned files"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'lstCleaned
        '
        Me.lstCleaned.FormattingEnabled = True
        Me.lstCleaned.Location = New System.Drawing.Point(6, 6)
        Me.lstCleaned.Name = "lstCleaned"
        Me.lstCleaned.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstCleaned.Size = New System.Drawing.Size(952, 342)
        Me.lstCleaned.TabIndex = 19
        '
        'Label2
        '
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(5, 363)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(955, 36)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Attention! this tool should be used with care and only if you know what you are d" & _
    "oing.  Uncontrolled actions can lead to inconsistent status."
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.LstTR)
        Me.TabPage2.Controls.Add(Me.Label1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(965, 404)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Files for translation"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'LstTR
        '
        Me.LstTR.FormattingEnabled = True
        Me.LstTR.Location = New System.Drawing.Point(6, 6)
        Me.LstTR.Name = "LstTR"
        Me.LstTR.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.LstTR.Size = New System.Drawing.Size(952, 342)
        Me.LstTR.TabIndex = 19
        '
        'Label1
        '
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(4, 365)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(955, 36)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Attention! this tool should be used with care and only if you know what you are d" & _
    "oing.  Uncontrolled actions can lead to inconsistent status."
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.lstTRback)
        Me.TabPage3.Controls.Add(Me.Label3)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(965, 404)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Translated files"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'lstTRback
        '
        Me.lstTRback.FormattingEnabled = True
        Me.lstTRback.Location = New System.Drawing.Point(6, 6)
        Me.lstTRback.Name = "lstTRback"
        Me.lstTRback.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstTRback.Size = New System.Drawing.Size(952, 342)
        Me.lstTRback.TabIndex = 19
        '
        'Label3
        '
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(7, 368)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(955, 36)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "Attention! this tool should be used with care and only if you know what you are d" & _
    "oing.  Uncontrolled actions can lead to inconsistent status."
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.lstPre)
        Me.TabPage4.Controls.Add(Me.Label4)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(965, 404)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Pretranslate files"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'lstPre
        '
        Me.lstPre.FormattingEnabled = True
        Me.lstPre.Location = New System.Drawing.Point(6, 6)
        Me.lstPre.Name = "lstPre"
        Me.lstPre.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstPre.Size = New System.Drawing.Size(952, 342)
        Me.lstPre.TabIndex = 19
        '
        'Label4
        '
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(3, 364)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(955, 40)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = resources.GetString("Label4.Text")
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.lstOut)
        Me.TabPage5.Controls.Add(Me.Label5)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(965, 404)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Output files"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'lstOut
        '
        Me.lstOut.FormattingEnabled = True
        Me.lstOut.Location = New System.Drawing.Point(6, 6)
        Me.lstOut.Name = "lstOut"
        Me.lstOut.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstOut.Size = New System.Drawing.Size(952, 342)
        Me.lstOut.TabIndex = 18
        '
        'Label5
        '
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(3, 368)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(955, 36)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "Attention! this tool should be used with care and only if you know what you are d" & _
    "oing.  Uncontrolled actions can lead to inconsistent status."
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(748, 464)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(107, 27)
        Me.Button10.TabIndex = 15
        Me.Button10.Text = "Use again"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(871, 464)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(107, 27)
        Me.Button6.TabIndex = 16
        Me.Button6.Text = "Delete Selected"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.ItemHeight = 13
        Me.ComboBox2.Location = New System.Drawing.Point(567, 468)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(74, 21)
        Me.ComboBox2.TabIndex = 27
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(370, 464)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(88, 27)
        Me.Button5.TabIndex = 26
        Me.Button5.Text = "DeSelect All"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(464, 464)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(97, 27)
        Me.Button9.TabIndex = 25
        Me.Button9.Text = "Select All"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'Form_Cleaning
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(999, 515)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button9)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button10)
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Form_Cleaning"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Cleaning"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage5.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents lstCleaned As System.Windows.Forms.ListBox
    Friend WithEvents LstTR As System.Windows.Forms.ListBox
    Friend WithEvents lstTRback As System.Windows.Forms.ListBox
    Friend WithEvents lstPre As System.Windows.Forms.ListBox
    Friend WithEvents lstOut As System.Windows.Forms.ListBox

End Class
