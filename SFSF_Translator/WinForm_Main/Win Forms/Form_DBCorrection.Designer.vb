<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_DBCorrection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_DBCorrection))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.BtnUpdateDB = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RB_SearchTarget = New System.Windows.Forms.RadioButton()
        Me.RB_SearchSource = New System.Windows.Forms.RadioButton()
        Me.RB_Both = New System.Windows.Forms.RadioButton()
        Me.CmbLanglist = New System.Windows.Forms.ComboBox()
        Me.BtnSearchDB = New System.Windows.Forms.Button()
        Me.BtnPasteSearch = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Rtb_Search = New System.Windows.Forms.RichTextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.chkFullStringOnly = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.BtnSelectAll = New System.Windows.Forms.Button()
        Me.BtnMarkAll = New System.Windows.Forms.Button()
        Me.Rtb_Replace = New System.Windows.Forms.RichTextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.BtnReload = New System.Windows.Forms.Button()
        Me.BtnReplace = New System.Windows.Forms.Button()
        Me.RB_ReplaceTarget = New System.Windows.Forms.RadioButton()
        Me.RB_ReplaceSource = New System.Windows.Forms.RadioButton()
        Me.BtnPasteReplace = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DV = New System.Windows.Forms.DataGridView()
        Me.ColExtID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColCB = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.ColSource = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColTarget = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColSourceLang = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColTargetLang = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColCustomer = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColInstance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColDataType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColResName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColMaxLength = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColCustomerSpecific = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DV, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.BtnUpdateDB, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(1137, 517)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(170, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'BtnUpdateDB
        '
        Me.BtnUpdateDB.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.BtnUpdateDB.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnUpdateDB.Location = New System.Drawing.Point(3, 3)
        Me.BtnUpdateDB.Name = "BtnUpdateDB"
        Me.BtnUpdateDB.Size = New System.Drawing.Size(79, 23)
        Me.BtnUpdateDB.TabIndex = 0
        Me.BtnUpdateDB.Text = "Update DB"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cancel_Button.Location = New System.Drawing.Point(88, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(79, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(103, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Input text to search"
        '
        'RB_SearchTarget
        '
        Me.RB_SearchTarget.AutoSize = True
        Me.RB_SearchTarget.Checked = True
        Me.RB_SearchTarget.Location = New System.Drawing.Point(954, 56)
        Me.RB_SearchTarget.Name = "RB_SearchTarget"
        Me.RB_SearchTarget.Size = New System.Drawing.Size(57, 17)
        Me.RB_SearchTarget.TabIndex = 3
        Me.RB_SearchTarget.TabStop = True
        Me.RB_SearchTarget.Text = "Target"
        Me.RB_SearchTarget.UseVisualStyleBackColor = True
        '
        'RB_SearchSource
        '
        Me.RB_SearchSource.AutoSize = True
        Me.RB_SearchSource.Location = New System.Drawing.Point(1014, 56)
        Me.RB_SearchSource.Name = "RB_SearchSource"
        Me.RB_SearchSource.Size = New System.Drawing.Size(60, 17)
        Me.RB_SearchSource.TabIndex = 4
        Me.RB_SearchSource.Text = "Source"
        Me.RB_SearchSource.UseVisualStyleBackColor = True
        '
        'RB_Both
        '
        Me.RB_Both.AutoSize = True
        Me.RB_Both.Location = New System.Drawing.Point(1077, 56)
        Me.RB_Both.Name = "RB_Both"
        Me.RB_Both.Size = New System.Drawing.Size(49, 17)
        Me.RB_Both.TabIndex = 5
        Me.RB_Both.Text = "Both"
        Me.RB_Both.UseVisualStyleBackColor = True
        '
        'CmbLanglist
        '
        Me.CmbLanglist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbLanglist.FormattingEnabled = True
        Me.CmbLanglist.Items.AddRange(New Object() {"[Select Langauge]"})
        Me.CmbLanglist.Location = New System.Drawing.Point(17, 22)
        Me.CmbLanglist.Name = "CmbLanglist"
        Me.CmbLanglist.Size = New System.Drawing.Size(160, 21)
        Me.CmbLanglist.TabIndex = 6
        '
        'BtnSearchDB
        '
        Me.BtnSearchDB.Location = New System.Drawing.Point(1103, 112)
        Me.BtnSearchDB.Name = "BtnSearchDB"
        Me.BtnSearchDB.Size = New System.Drawing.Size(152, 23)
        Me.BtnSearchDB.TabIndex = 7
        Me.BtnSearchDB.Text = "Search DB"
        Me.ToolTip1.SetToolTip(Me.BtnSearchDB, "Searches database in server and fills grid")
        Me.BtnSearchDB.UseVisualStyleBackColor = True
        '
        'BtnPasteSearch
        '
        Me.BtnPasteSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnPasteSearch.Font = New System.Drawing.Font("BentonSans Regular", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnPasteSearch.Image = CType(resources.GetObject("BtnPasteSearch.Image"), System.Drawing.Image)
        Me.BtnPasteSearch.Location = New System.Drawing.Point(1259, 84)
        Me.BtnPasteSearch.Name = "BtnPasteSearch"
        Me.BtnPasteSearch.Size = New System.Drawing.Size(21, 20)
        Me.BtnPasteSearch.TabIndex = 11
        Me.BtnPasteSearch.Text = "..."
        Me.BtnPasteSearch.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Rtb_Search)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.BtnSearchDB)
        Me.GroupBox1.Controls.Add(Me.BtnPasteSearch)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.RB_SearchTarget)
        Me.GroupBox1.Controls.Add(Me.RB_SearchSource)
        Me.GroupBox1.Controls.Add(Me.CmbLanglist)
        Me.GroupBox1.Controls.Add(Me.RB_Both)
        Me.GroupBox1.Controls.Add(Me.chkFullStringOnly)
        Me.GroupBox1.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(20, 62)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1287, 144)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Step1: Search DB"
        '
        'Rtb_Search
        '
        Me.Rtb_Search.Location = New System.Drawing.Point(17, 72)
        Me.Rtb_Search.Name = "Rtb_Search"
        Me.Rtb_Search.Size = New System.Drawing.Size(1238, 32)
        Me.Rtb_Search.TabIndex = 14
        Me.Rtb_Search.Text = ""
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(945, 112)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(152, 23)
        Me.Button1.TabIndex = 13
        Me.Button1.Text = "Clear Text"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'chkFullStringOnly
        '
        Me.chkFullStringOnly.AutoSize = True
        Me.chkFullStringOnly.Location = New System.Drawing.Point(1161, 56)
        Me.chkFullStringOnly.Name = "chkFullStringOnly"
        Me.chkFullStringOnly.Size = New System.Drawing.Size(100, 17)
        Me.chkFullStringOnly.TabIndex = 12
        Me.chkFullStringOnly.Text = "Full String Only"
        Me.ToolTip1.SetToolTip(Me.chkFullStringOnly, "wildcard search if not checked, else exact search")
        Me.chkFullStringOnly.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("BentonSans Book", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(16, 22)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(136, 24)
        Me.Label4.TabIndex = 34
        Me.Label4.Text = "DB Corrections"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = CType(resources.GetObject("PictureBox1.InitialImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(-3, -2)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(1331, 58)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 35
        Me.PictureBox1.TabStop = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.BtnSelectAll)
        Me.GroupBox2.Controls.Add(Me.BtnMarkAll)
        Me.GroupBox2.Controls.Add(Me.Rtb_Replace)
        Me.GroupBox2.Controls.Add(Me.Button2)
        Me.GroupBox2.Controls.Add(Me.BtnReload)
        Me.GroupBox2.Controls.Add(Me.BtnReplace)
        Me.GroupBox2.Controls.Add(Me.RB_ReplaceTarget)
        Me.GroupBox2.Controls.Add(Me.RB_ReplaceSource)
        Me.GroupBox2.Controls.Add(Me.BtnPasteReplace)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.DV)
        Me.GroupBox2.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(20, 208)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1287, 304)
        Me.GroupBox2.TabIndex = 36
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Step2: Select"
        '
        'BtnSelectAll
        '
        Me.BtnSelectAll.Location = New System.Drawing.Point(78, 21)
        Me.BtnSelectAll.Name = "BtnSelectAll"
        Me.BtnSelectAll.Size = New System.Drawing.Size(67, 23)
        Me.BtnSelectAll.TabIndex = 44
        Me.BtnSelectAll.Text = "Select All"
        Me.ToolTip1.SetToolTip(Me.BtnSelectAll, "Select\UnSelect all rows")
        Me.BtnSelectAll.UseVisualStyleBackColor = True
        '
        'BtnMarkAll
        '
        Me.BtnMarkAll.Font = New System.Drawing.Font("Wingdings 2", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnMarkAll.Location = New System.Drawing.Point(17, 19)
        Me.BtnMarkAll.Name = "BtnMarkAll"
        Me.BtnMarkAll.Size = New System.Drawing.Size(55, 25)
        Me.BtnMarkAll.TabIndex = 43
        Me.BtnMarkAll.Text = "RQ"
        Me.ToolTip1.SetToolTip(Me.BtnMarkAll, "Mark Unmark Selected rows")
        Me.BtnMarkAll.UseVisualStyleBackColor = True
        '
        'Rtb_Replace
        '
        Me.Rtb_Replace.Location = New System.Drawing.Point(17, 232)
        Me.Rtb_Replace.Name = "Rtb_Replace"
        Me.Rtb_Replace.Size = New System.Drawing.Size(1238, 32)
        Me.Rtb_Replace.TabIndex = 42
        Me.Rtb_Replace.Text = ""
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(945, 270)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(152, 23)
        Me.Button2.TabIndex = 41
        Me.Button2.Text = "Clear Text"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'BtnReload
        '
        Me.BtnReload.Font = New System.Drawing.Font("Wingdings 3", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnReload.Location = New System.Drawing.Point(1255, 47)
        Me.BtnReload.Name = "BtnReload"
        Me.BtnReload.Size = New System.Drawing.Size(32, 41)
        Me.BtnReload.TabIndex = 40
        Me.BtnReload.Text = "Q"
        Me.ToolTip1.SetToolTip(Me.BtnReload, "Reload the grid")
        Me.BtnReload.UseVisualStyleBackColor = True
        '
        'BtnReplace
        '
        Me.BtnReplace.Location = New System.Drawing.Point(1103, 270)
        Me.BtnReplace.Name = "BtnReplace"
        Me.BtnReplace.Size = New System.Drawing.Size(152, 23)
        Me.BtnReplace.TabIndex = 39
        Me.BtnReplace.Text = "Replace"
        Me.ToolTip1.SetToolTip(Me.BtnReplace, "Replaces text in grid based on replace selection")
        Me.BtnReplace.UseVisualStyleBackColor = True
        '
        'RB_ReplaceTarget
        '
        Me.RB_ReplaceTarget.AutoSize = True
        Me.RB_ReplaceTarget.Checked = True
        Me.RB_ReplaceTarget.Location = New System.Drawing.Point(1134, 216)
        Me.RB_ReplaceTarget.Name = "RB_ReplaceTarget"
        Me.RB_ReplaceTarget.Size = New System.Drawing.Size(57, 17)
        Me.RB_ReplaceTarget.TabIndex = 37
        Me.RB_ReplaceTarget.TabStop = True
        Me.RB_ReplaceTarget.Text = "Target"
        Me.RB_ReplaceTarget.UseVisualStyleBackColor = True
        '
        'RB_ReplaceSource
        '
        Me.RB_ReplaceSource.AutoSize = True
        Me.RB_ReplaceSource.Location = New System.Drawing.Point(1194, 216)
        Me.RB_ReplaceSource.Name = "RB_ReplaceSource"
        Me.RB_ReplaceSource.Size = New System.Drawing.Size(60, 17)
        Me.RB_ReplaceSource.TabIndex = 38
        Me.RB_ReplaceSource.Text = "Source"
        Me.RB_ReplaceSource.UseVisualStyleBackColor = True
        '
        'BtnPasteReplace
        '
        Me.BtnPasteReplace.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnPasteReplace.Font = New System.Drawing.Font("BentonSans Regular", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnPasteReplace.Image = CType(resources.GetObject("BtnPasteReplace.Image"), System.Drawing.Image)
        Me.BtnPasteReplace.Location = New System.Drawing.Point(1259, 244)
        Me.BtnPasteReplace.Name = "BtnPasteReplace"
        Me.BtnPasteReplace.Size = New System.Drawing.Size(21, 20)
        Me.BtnPasteReplace.TabIndex = 14
        Me.BtnPasteReplace.Text = "..."
        Me.BtnPasteReplace.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(17, 216)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Replace Term"
        '
        'DV
        '
        Me.DV.AllowUserToAddRows = False
        Me.DV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DV.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColExtID, Me.ColCB, Me.ColSource, Me.ColTarget, Me.ColSourceLang, Me.ColTargetLang, Me.ColCustomer, Me.ColInstance, Me.ColDataType, Me.ColResName, Me.ColMaxLength, Me.ColCustomerSpecific})
        Me.DV.Location = New System.Drawing.Point(17, 47)
        Me.DV.Name = "DV"
        Me.DV.Size = New System.Drawing.Size(1238, 158)
        Me.DV.TabIndex = 0
        '
        'ColExtID
        '
        Me.ColExtID.HeaderText = "ExtID"
        Me.ColExtID.Name = "ColExtID"
        Me.ColExtID.ReadOnly = True
        Me.ColExtID.Visible = False
        '
        'ColCB
        '
        Me.ColCB.HeaderText = "Set"
        Me.ColCB.Name = "ColCB"
        Me.ColCB.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ColCB.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.ColCB.Width = 30
        '
        'ColSource
        '
        Me.ColSource.HeaderText = "Source"
        Me.ColSource.Name = "ColSource"
        Me.ColSource.Width = 400
        '
        'ColTarget
        '
        Me.ColTarget.HeaderText = "Target"
        Me.ColTarget.Name = "ColTarget"
        Me.ColTarget.Width = 400
        '
        'ColSourceLang
        '
        Me.ColSourceLang.HeaderText = "SoruceLang"
        Me.ColSourceLang.Name = "ColSourceLang"
        Me.ColSourceLang.ReadOnly = True
        '
        'ColTargetLang
        '
        Me.ColTargetLang.HeaderText = "TargetLang"
        Me.ColTargetLang.Name = "ColTargetLang"
        Me.ColTargetLang.ReadOnly = True
        '
        'ColCustomer
        '
        Me.ColCustomer.HeaderText = "Customer"
        Me.ColCustomer.Name = "ColCustomer"
        Me.ColCustomer.ReadOnly = True
        '
        'ColInstance
        '
        Me.ColInstance.HeaderText = "Instance"
        Me.ColInstance.Name = "ColInstance"
        Me.ColInstance.ReadOnly = True
        '
        'ColDataType
        '
        Me.ColDataType.HeaderText = "DateType"
        Me.ColDataType.Name = "ColDataType"
        Me.ColDataType.ReadOnly = True
        '
        'ColResName
        '
        Me.ColResName.HeaderText = "ResName"
        Me.ColResName.Name = "ColResName"
        Me.ColResName.ReadOnly = True
        '
        'ColMaxLength
        '
        Me.ColMaxLength.HeaderText = "MaxLength"
        Me.ColMaxLength.Name = "ColMaxLength"
        Me.ColMaxLength.ReadOnly = True
        '
        'ColCustomerSpecific
        '
        Me.ColCustomerSpecific.HeaderText = "CustomerSpecific"
        Me.ColCustomerSpecific.Name = "ColCustomerSpecific"
        Me.ColCustomerSpecific.ReadOnly = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 550)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1329, 22)
        Me.StatusStrip1.TabIndex = 37
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(1200, 17)
        Me.ToolStripStatusLabel1.Text = "Status:"
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(100, 16)
        '
        'Form_DBCorrection
        '
        Me.AcceptButton = Me.BtnUpdateDB
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(1329, 572)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_DBCorrection"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.DV, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents BtnUpdateDB As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As Label
    Friend WithEvents RB_SearchTarget As RadioButton
    Friend WithEvents RB_SearchSource As RadioButton
    Friend WithEvents RB_Both As RadioButton
    Friend WithEvents CmbLanglist As ComboBox
    Friend WithEvents BtnSearchDB As Button
    Friend WithEvents BtnPasteSearch As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents chkFullStringOnly As CheckBox
    Friend WithEvents Label4 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents DV As DataGridView
    Friend WithEvents BtnReplace As Button
    Friend WithEvents RB_ReplaceTarget As RadioButton
    Friend WithEvents RB_ReplaceSource As RadioButton
    Friend WithEvents BtnPasteReplace As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents BtnReload As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Rtb_Search As RichTextBox
    Friend WithEvents Rtb_Replace As RichTextBox
    Friend WithEvents BtnSelectAll As Button
    Friend WithEvents BtnMarkAll As Button
    Friend WithEvents ColExtID As DataGridViewTextBoxColumn
    Friend WithEvents ColCB As DataGridViewCheckBoxColumn
    Friend WithEvents ColSource As DataGridViewTextBoxColumn
    Friend WithEvents ColTarget As DataGridViewTextBoxColumn
    Friend WithEvents ColSourceLang As DataGridViewTextBoxColumn
    Friend WithEvents ColTargetLang As DataGridViewTextBoxColumn
    Friend WithEvents ColCustomer As DataGridViewTextBoxColumn
    Friend WithEvents ColInstance As DataGridViewTextBoxColumn
    Friend WithEvents ColDataType As DataGridViewTextBoxColumn
    Friend WithEvents ColResName As DataGridViewTextBoxColumn
    Friend WithEvents ColMaxLength As DataGridViewTextBoxColumn
    Friend WithEvents ColCustomerSpecific As DataGridViewTextBoxColumn
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents ToolStripProgressBar1 As ToolStripProgressBar
End Class
