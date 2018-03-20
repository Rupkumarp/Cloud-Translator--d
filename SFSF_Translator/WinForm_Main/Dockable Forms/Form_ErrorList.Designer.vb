<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ErrorList
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ErrorList))
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.ColDescription = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColFileName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColLineNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripComboBox1 = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ButtonError = New System.Windows.Forms.ToolStripButton()
        Me.ButtonWarning = New System.Windows.Forms.ToolStripButton()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColDescription, Me.ColFileName, Me.ColLineNumber})
        Me.DataGridView1.Location = New System.Drawing.Point(0, 26)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(874, 180)
        Me.DataGridView1.TabIndex = 0
        '
        'ColDescription
        '
        Me.ColDescription.FillWeight = 96.70051!
        Me.ColDescription.HeaderText = "Description"
        Me.ColDescription.Name = "ColDescription"
        Me.ColDescription.ReadOnly = True
        '
        'ColFileName
        '
        Me.ColFileName.FillWeight = 40.29949!
        Me.ColFileName.HeaderText = "Filename"
        Me.ColFileName.Name = "ColFileName"
        Me.ColFileName.ReadOnly = True
        '
        'ColLineNumber
        '
        Me.ColLineNumber.FillWeight = 20.0!
        Me.ColLineNumber.HeaderText = "LineNumber"
        Me.ColLineNumber.Name = "ColLineNumber"
        Me.ColLineNumber.ReadOnly = True
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripComboBox1, Me.ToolStripSeparator1, Me.ButtonError, Me.ButtonWarning})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ToolStrip1.Size = New System.Drawing.Size(874, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripComboBox1
        '
        Me.ToolStripComboBox1.AutoSize = False
        Me.ToolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripComboBox1.Name = "ToolStripComboBox1"
        Me.ToolStripComboBox1.Size = New System.Drawing.Size(200, 23)
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'ButtonError
        '
        Me.ButtonError.BackColor = System.Drawing.Color.LightYellow
        Me.ButtonError.Image = Global.CloudTranslator.My.Resources.Resources.Error__1_
        Me.ButtonError.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonError.Name = "ButtonError"
        Me.ButtonError.Size = New System.Drawing.Size(66, 22)
        Me.ButtonError.Text = "0 Errors"
        '
        'ButtonWarning
        '
        Me.ButtonWarning.BackColor = System.Drawing.Color.LightYellow
        Me.ButtonWarning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonWarning.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.ButtonWarning.Image = Global.CloudTranslator.My.Resources.Resources._error
        Me.ButtonWarning.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonWarning.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonWarning.Name = "ButtonWarning"
        Me.ButtonWarning.Size = New System.Drawing.Size(86, 22)
        Me.ButtonWarning.Text = "0 Warnings"
        '
        'Form_ErrorList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(874, 204)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form_ErrorList"
        Me.ShowIcon = False
        Me.Text = "Error List"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripComboBox1 As ToolStripComboBox
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ButtonWarning As ToolStripButton
    Friend WithEvents ButtonError As ToolStripButton
    Friend WithEvents ColDescription As DataGridViewTextBoxColumn
    Friend WithEvents ColFileName As DataGridViewTextBoxColumn
    Friend WithEvents ColLineNumber As DataGridViewTextBoxColumn
End Class
