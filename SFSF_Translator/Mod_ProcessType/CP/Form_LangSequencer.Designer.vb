<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_LangSequencer
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
        Me.LstLang = New System.Windows.Forms.ListBox()
        Me.BtnDown = New System.Windows.Forms.Button()
        Me.BtnUP = New System.Windows.Forms.Button()
        Me.BtnDeleteLang = New System.Windows.Forms.Button()
        Me.BtnReloadLang = New System.Windows.Forms.Button()
        Me.BtnDone = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'LstLang
        '
        Me.LstLang.FormattingEnabled = True
        Me.LstLang.Location = New System.Drawing.Point(12, 9)
        Me.LstLang.Name = "LstLang"
        Me.LstLang.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstLang.Size = New System.Drawing.Size(190, 251)
        Me.LstLang.TabIndex = 0
        '
        'BtnDown
        '
        Me.BtnDown.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnDown.Font = New System.Drawing.Font("Wingdings 3", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnDown.Location = New System.Drawing.Point(208, 143)
        Me.BtnDown.Name = "BtnDown"
        Me.BtnDown.Size = New System.Drawing.Size(38, 30)
        Me.BtnDown.TabIndex = 1
        Me.BtnDown.Text = "q"
        Me.ToolTip1.SetToolTip(Me.BtnDown, "Move Langauge Down")
        Me.BtnDown.UseVisualStyleBackColor = True
        '
        'BtnUP
        '
        Me.BtnUP.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnUP.Font = New System.Drawing.Font("Wingdings 3", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnUP.Location = New System.Drawing.Point(208, 107)
        Me.BtnUP.Name = "BtnUP"
        Me.BtnUP.Size = New System.Drawing.Size(38, 30)
        Me.BtnUP.TabIndex = 2
        Me.BtnUP.Text = "p"
        Me.ToolTip1.SetToolTip(Me.BtnUP, "Move Langue Up")
        Me.BtnUP.UseVisualStyleBackColor = True
        '
        'BtnDeleteLang
        '
        Me.BtnDeleteLang.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnDeleteLang.Font = New System.Drawing.Font("Wingdings 2", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnDeleteLang.ForeColor = System.Drawing.Color.Red
        Me.BtnDeleteLang.Location = New System.Drawing.Point(208, 45)
        Me.BtnDeleteLang.Name = "BtnDeleteLang"
        Me.BtnDeleteLang.Size = New System.Drawing.Size(38, 30)
        Me.BtnDeleteLang.TabIndex = 3
        Me.BtnDeleteLang.Text = "U"
        Me.ToolTip1.SetToolTip(Me.BtnDeleteLang, "Delete Language from list")
        Me.BtnDeleteLang.UseVisualStyleBackColor = True
        '
        'BtnReloadLang
        '
        Me.BtnReloadLang.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnReloadLang.Font = New System.Drawing.Font("Wingdings 3", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnReloadLang.Location = New System.Drawing.Point(208, 9)
        Me.BtnReloadLang.Name = "BtnReloadLang"
        Me.BtnReloadLang.Size = New System.Drawing.Size(38, 30)
        Me.BtnReloadLang.TabIndex = 4
        Me.BtnReloadLang.Text = "P"
        Me.BtnReloadLang.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.BtnReloadLang, "Reload Languages")
        Me.BtnReloadLang.UseVisualStyleBackColor = True
        '
        'BtnDone
        '
        Me.BtnDone.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnDone.Font = New System.Drawing.Font("Wingdings 2", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnDone.ForeColor = System.Drawing.Color.Green
        Me.BtnDone.Location = New System.Drawing.Point(208, 230)
        Me.BtnDone.Name = "BtnDone"
        Me.BtnDone.Size = New System.Drawing.Size(38, 30)
        Me.BtnDone.TabIndex = 3
        Me.BtnDone.Text = "P"
        Me.ToolTip1.SetToolTip(Me.BtnDone, "Ok and Done")
        Me.BtnDone.UseVisualStyleBackColor = True
        '
        'Form_LangSequencer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(256, 269)
        Me.Controls.Add(Me.BtnReloadLang)
        Me.Controls.Add(Me.BtnDone)
        Me.Controls.Add(Me.BtnDeleteLang)
        Me.Controls.Add(Me.BtnUP)
        Me.Controls.Add(Me.BtnDown)
        Me.Controls.Add(Me.LstLang)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_LangSequencer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Define Language Sequence"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LstLang As System.Windows.Forms.ListBox
    Friend WithEvents BtnDown As System.Windows.Forms.Button
    Friend WithEvents BtnUP As System.Windows.Forms.Button
    Friend WithEvents BtnDeleteLang As System.Windows.Forms.Button
    Friend WithEvents BtnReloadLang As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents BtnDone As System.Windows.Forms.Button
End Class
