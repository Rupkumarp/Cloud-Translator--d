<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_FindinLog
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.optDown = New System.Windows.Forms.RadioButton()
        Me.optUp = New System.Windows.Forms.RadioButton()
        Me.chkMatchCase = New System.Windows.Forms.CheckBox()
        Me.chkWholeWord = New System.Windows.Forms.CheckBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnFind = New System.Windows.Forms.Button()
        Me.txtFindWhat = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.optDown)
        Me.GroupBox1.Controls.Add(Me.optUp)
        Me.GroupBox1.Location = New System.Drawing.Point(158, 37)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(103, 40)
        Me.GroupBox1.TabIndex = 11
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Direction"
        '
        'optDown
        '
        Me.optDown.AutoSize = True
        Me.optDown.Checked = True
        Me.optDown.Location = New System.Drawing.Point(46, 17)
        Me.optDown.Name = "optDown"
        Me.optDown.Size = New System.Drawing.Size(53, 17)
        Me.optDown.TabIndex = 1
        Me.optDown.TabStop = True
        Me.optDown.Text = "Down"
        Me.optDown.UseVisualStyleBackColor = True
        '
        'optUp
        '
        Me.optUp.AutoSize = True
        Me.optUp.Location = New System.Drawing.Point(7, 17)
        Me.optUp.Name = "optUp"
        Me.optUp.Size = New System.Drawing.Size(39, 17)
        Me.optUp.TabIndex = 0
        Me.optUp.Text = "Up"
        Me.optUp.UseVisualStyleBackColor = True
        '
        'chkMatchCase
        '
        Me.chkMatchCase.AutoSize = True
        Me.chkMatchCase.Location = New System.Drawing.Point(15, 62)
        Me.chkMatchCase.Name = "chkMatchCase"
        Me.chkMatchCase.Size = New System.Drawing.Size(82, 17)
        Me.chkMatchCase.TabIndex = 10
        Me.chkMatchCase.Text = "Match case"
        Me.chkMatchCase.UseVisualStyleBackColor = True
        '
        'chkWholeWord
        '
        Me.chkWholeWord.AutoSize = True
        Me.chkWholeWord.Location = New System.Drawing.Point(15, 39)
        Me.chkWholeWord.Name = "chkWholeWord"
        Me.chkWholeWord.Size = New System.Drawing.Size(135, 17)
        Me.chkWholeWord.TabIndex = 9
        Me.chkWholeWord.Text = "Match whole word only"
        Me.chkWholeWord.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(267, 35)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(71, 23)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnFind
        '
        Me.btnFind.Enabled = False
        Me.btnFind.Location = New System.Drawing.Point(267, 6)
        Me.btnFind.Name = "btnFind"
        Me.btnFind.Size = New System.Drawing.Size(71, 23)
        Me.btnFind.TabIndex = 7
        Me.btnFind.Text = "Find next"
        Me.btnFind.UseVisualStyleBackColor = True
        '
        'txtFindWhat
        '
        Me.txtFindWhat.Location = New System.Drawing.Point(68, 6)
        Me.txtFindWhat.Name = "txtFindWhat"
        Me.txtFindWhat.Size = New System.Drawing.Size(193, 20)
        Me.txtFindWhat.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Find what:"
        '
        'Form_FindinLog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(343, 86)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.chkMatchCase)
        Me.Controls.Add(Me.chkWholeWord)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnFind)
        Me.Controls.Add(Me.txtFindWhat)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_FindinLog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Find"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents optDown As RadioButton
    Friend WithEvents optUp As RadioButton
    Friend WithEvents chkMatchCase As CheckBox
    Friend WithEvents chkWholeWord As CheckBox
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnFind As Button
    Friend WithEvents txtFindWhat As TextBox
    Friend WithEvents Label1 As Label
End Class
