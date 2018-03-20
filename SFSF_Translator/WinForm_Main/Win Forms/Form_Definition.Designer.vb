<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Definition
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_Definition))
        Me.lblDefintion = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DV = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InsertRowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripComboBox()
        Me.OKToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TV = New CloudTranslator.NativeTreeView()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DV, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblDefintion
        '
        Me.lblDefintion.AutoSize = True
        Me.lblDefintion.BackColor = System.Drawing.Color.Transparent
        Me.lblDefintion.Font = New System.Drawing.Font("BentonSans Bold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDefintion.Location = New System.Drawing.Point(20, 22)
        Me.lblDefintion.Margin = New System.Windows.Forms.Padding(0)
        Me.lblDefintion.Name = "lblDefintion"
        Me.lblDefintion.Size = New System.Drawing.Size(221, 19)
        Me.lblDefintion.TabIndex = 49
        Me.lblDefintion.Text = "Manage Defintion Settings"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = CType(resources.GetObject("PictureBox1.InitialImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(-1, 2)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(1093, 42)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 48
        Me.PictureBox1.TabStop = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Font = New System.Drawing.Font("BentonSans Bold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnSave.Location = New System.Drawing.Point(947, 455)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(133, 28)
        Me.btnSave.TabIndex = 51
        Me.btnSave.Text = "Save Changes"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("BentonSans Bold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 455)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(295, 13)
        Me.Label1.TabIndex = 52
        Me.Label1.Text = "Note: Do not change the format of the definition file. "
        '
        'DV
        '
        Me.DV.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DV.BackgroundColor = System.Drawing.Color.White
        Me.DV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DV.ContextMenuStrip = Me.ContextMenuStrip1
        Me.DV.Location = New System.Drawing.Point(253, 50)
        Me.DV.Name = "DV"
        Me.DV.Size = New System.Drawing.Size(828, 394)
        Me.DV.TabIndex = 53
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.InsertRowToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(130, 70)
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Image = Global.CloudTranslator.My.Resources.Resources.ClipboardCopy
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.CopyToolStripMenuItem.Text = "Copy"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Image = Global.CloudTranslator.My.Resources.Resources.ClipboardPaste
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.PasteToolStripMenuItem.Text = "Paste"
        '
        'InsertRowToolStripMenuItem
        '
        Me.InsertRowToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem2, Me.OKToolStripMenuItem})
        Me.InsertRowToolStripMenuItem.Enabled = False
        Me.InsertRowToolStripMenuItem.Image = Global.CloudTranslator.My.Resources.Resources._Next
        Me.InsertRowToolStripMenuItem.Name = "InsertRowToolStripMenuItem"
        Me.InsertRowToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.InsertRowToolStripMenuItem.Text = "Insert Row"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.AutoSize = False
        Me.ToolStripMenuItem2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripMenuItem2.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ToolStripMenuItem2.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(80, 23)
        '
        'OKToolStripMenuItem
        '
        Me.OKToolStripMenuItem.Name = "OKToolStripMenuItem"
        Me.OKToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.OKToolStripMenuItem.Text = "Click here to Insert row"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Font = New System.Drawing.Font("BentonSans Bold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Button1.Location = New System.Drawing.Point(805, 455)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(136, 28)
        Me.Button1.TabIndex = 54
        Me.Button1.Text = "Load Default"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("BentonSans Bold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(12, 470)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(612, 13)
        Me.Label2.TabIndex = 56
        Me.Label2.Text = "If any wrong settings used, Use Load default to load initial definition file crea" & _
    "ted when installing the application."
        '
        'TV
        '
        Me.TV.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TV.HideSelection = False
        Me.TV.Location = New System.Drawing.Point(12, 50)
        Me.TV.Name = "TV"
        Me.TV.ShowLines = False
        Me.TV.Size = New System.Drawing.Size(215, 394)
        Me.TV.TabIndex = 55
        '
        'Form_Definition
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1093, 512)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TV)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.DV)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.lblDefintion)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "Form_Definition"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DV, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblDefintion As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DV As System.Windows.Forms.DataGridView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TV As CloudTranslator.NativeTreeView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InsertRowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents OKToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
