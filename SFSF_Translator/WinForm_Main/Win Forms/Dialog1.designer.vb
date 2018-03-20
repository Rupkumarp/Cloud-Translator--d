<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Dialog1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Dialog1))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.BtnBack = New System.Windows.Forms.Button()
        Me.BtnNext = New System.Windows.Forms.Button()
        Me.Lbl1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBox_ProjectName = New System.Windows.Forms.TextBox()
        Me.TextBox_ProjectPath = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox_Description = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Folder1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.ListBox1 = New System.Windows.Forms.CheckedListBox()
        Me.btnBrowseLang = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ChkMaxLength = New System.Windows.Forms.CheckBox()
        Me.ChkCleanTranslation = New System.Windows.Forms.CheckBox()
        Me.ChkRestrictCustomer = New System.Windows.Forms.CheckBox()
        Me.ChkInstance = New System.Windows.Forms.CheckBox()
        Me.lblLanguage = New System.Windows.Forms.Label()
        Me.txtSFSF_Instance = New System.Windows.Forms.TextBox()
        Me.txtCustomerName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ChkPretranslate = New System.Windows.Forms.CheckBox()
        Me.CmbCustomer = New System.Windows.Forms.ComboBox()
        Me.CmbInstance = New System.Windows.Forms.ComboBox()
        Me.GB2 = New System.Windows.Forms.GroupBox()
        Me.ChkCorrupt = New System.Windows.Forms.CheckBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.ChkUploadToDB = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.GB1 = New System.Windows.Forms.GroupBox()
        Me.ChkMaster = New System.Windows.Forms.CheckBox()
        Me.CmbProjectGroup = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.BW = New System.ComponentModel.BackgroundWorker()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GB2.SuspendLayout()
        Me.GB1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.BtnBack, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.BtnNext, 1, 0)
        Me.TableLayoutPanel1.Font = New System.Drawing.Font("BentonSans Regular", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(388, 471)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(338, 31)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.CausesValidation = False
        Me.OK_Button.Enabled = False
        Me.OK_Button.Font = New System.Drawing.Font("BentonSans Regular", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OK_Button.Location = New System.Drawing.Point(171, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(78, 25)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Finish"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Font = New System.Drawing.Font("BentonSans Regular", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cancel_Button.Location = New System.Drawing.Point(256, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(78, 25)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'BtnBack
        '
        Me.BtnBack.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.BtnBack.Enabled = False
        Me.BtnBack.Font = New System.Drawing.Font("BentonSans Regular", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnBack.Location = New System.Drawing.Point(3, 3)
        Me.BtnBack.Name = "BtnBack"
        Me.BtnBack.Size = New System.Drawing.Size(78, 25)
        Me.BtnBack.TabIndex = 0
        Me.BtnBack.Text = "< Back"
        '
        'BtnNext
        '
        Me.BtnNext.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.BtnNext.Enabled = False
        Me.BtnNext.Font = New System.Drawing.Font("BentonSans Regular", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnNext.Location = New System.Drawing.Point(87, 3)
        Me.BtnNext.Name = "BtnNext"
        Me.BtnNext.Size = New System.Drawing.Size(78, 25)
        Me.BtnNext.TabIndex = 0
        Me.BtnNext.Text = "Next >"
        '
        'Lbl1
        '
        Me.Lbl1.AutoSize = True
        Me.Lbl1.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lbl1.Location = New System.Drawing.Point(22, 149)
        Me.Lbl1.Name = "Lbl1"
        Me.Lbl1.Size = New System.Drawing.Size(64, 13)
        Me.Lbl1.TabIndex = 2
        Me.Lbl1.Text = "Languages:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(22, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Project Name:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(22, 68)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Project Path:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(22, 303)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(108, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Project Description:"
        '
        'TextBox_ProjectName
        '
        Me.TextBox_ProjectName.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_ProjectName.Location = New System.Drawing.Point(118, 38)
        Me.TextBox_ProjectName.Name = "TextBox_ProjectName"
        Me.TextBox_ProjectName.Size = New System.Drawing.Size(607, 21)
        Me.TextBox_ProjectName.TabIndex = 6
        '
        'TextBox_ProjectPath
        '
        Me.TextBox_ProjectPath.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_ProjectPath.Location = New System.Drawing.Point(118, 65)
        Me.TextBox_ProjectPath.Name = "TextBox_ProjectPath"
        Me.TextBox_ProjectPath.Size = New System.Drawing.Size(607, 21)
        Me.TextBox_ProjectPath.TabIndex = 7
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("BentonSans Regular", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(732, 65)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(26, 20)
        Me.Button1.TabIndex = 8
        Me.Button1.Text = "..."
        Me.ToolTip1.SetToolTip(Me.Button1, "Select New Project folder")
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox_Description
        '
        Me.TextBox_Description.AcceptsReturn = True
        Me.TextBox_Description.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_Description.Location = New System.Drawing.Point(22, 321)
        Me.TextBox_Description.Multiline = True
        Me.TextBox_Description.Name = "TextBox_Description"
        Me.TextBox_Description.Size = New System.Drawing.Size(703, 88)
        Me.TextBox_Description.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("BentonSans Bold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(13, 21)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(176, 19)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "New Project Settings"
        '
        'ListBox1
        '
        Me.ListBox1.CheckOnClick = True
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(22, 167)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(704, 132)
        Me.ListBox1.TabIndex = 11
        Me.ListBox1.ThreeDCheckBoxes = True
        '
        'btnBrowseLang
        '
        Me.btnBrowseLang.Font = New System.Drawing.Font("BentonSans Regular", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseLang.Location = New System.Drawing.Point(732, 165)
        Me.btnBrowseLang.Name = "btnBrowseLang"
        Me.btnBrowseLang.Size = New System.Drawing.Size(26, 20)
        Me.btnBrowseLang.TabIndex = 33
        Me.btnBrowseLang.Text = "..."
        Me.ToolTip1.SetToolTip(Me.btnBrowseLang, "Load Language Preset")
        Me.btnBrowseLang.UseVisualStyleBackColor = True
        '
        'ChkMaxLength
        '
        Me.ChkMaxLength.AutoSize = True
        Me.ChkMaxLength.Location = New System.Drawing.Point(21, 89)
        Me.ChkMaxLength.Name = "ChkMaxLength"
        Me.ChkMaxLength.Size = New System.Drawing.Size(117, 17)
        Me.ChkMaxLength.TabIndex = 45
        Me.ChkMaxLength.Text = "Check max length"
        Me.ToolTip1.SetToolTip(Me.ChkMaxLength, "Restricts char length defined in definition")
        Me.ChkMaxLength.UseVisualStyleBackColor = True
        '
        'ChkCleanTranslation
        '
        Me.ChkCleanTranslation.AutoSize = True
        Me.ChkCleanTranslation.Location = New System.Drawing.Point(21, 159)
        Me.ChkCleanTranslation.Name = "ChkCleanTranslation"
        Me.ChkCleanTranslation.Size = New System.Drawing.Size(399, 17)
        Me.ChkCleanTranslation.TabIndex = 34
        Me.ChkCleanTranslation.Text = "Clean Translation (Removes existing translation in multilingual input files)"
        Me.ChkCleanTranslation.UseVisualStyleBackColor = True
        '
        'ChkRestrictCustomer
        '
        Me.ChkRestrictCustomer.AutoSize = True
        Me.ChkRestrictCustomer.Enabled = False
        Me.ChkRestrictCustomer.Location = New System.Drawing.Point(65, 45)
        Me.ChkRestrictCustomer.Name = "ChkRestrictCustomer"
        Me.ChkRestrictCustomer.Size = New System.Drawing.Size(100, 17)
        Me.ChkRestrictCustomer.TabIndex = 40
        Me.ChkRestrictCustomer.Text = "Customer only"
        Me.ChkRestrictCustomer.UseVisualStyleBackColor = True
        '
        'ChkInstance
        '
        Me.ChkInstance.AutoSize = True
        Me.ChkInstance.Enabled = False
        Me.ChkInstance.Location = New System.Drawing.Point(65, 21)
        Me.ChkInstance.Name = "ChkInstance"
        Me.ChkInstance.Size = New System.Drawing.Size(93, 17)
        Me.ChkInstance.TabIndex = 41
        Me.ChkInstance.Text = "Instance only"
        Me.ChkInstance.UseVisualStyleBackColor = True
        '
        'lblLanguage
        '
        Me.lblLanguage.AutoSize = True
        Me.lblLanguage.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLanguage.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblLanguage.Location = New System.Drawing.Point(115, 149)
        Me.lblLanguage.Name = "lblLanguage"
        Me.lblLanguage.Size = New System.Drawing.Size(31, 13)
        Me.lblLanguage.TabIndex = 2
        Me.lblLanguage.Text = "Lang"
        '
        'txtSFSF_Instance
        '
        Me.txtSFSF_Instance.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSFSF_Instance.Location = New System.Drawing.Point(118, 119)
        Me.txtSFSF_Instance.Name = "txtSFSF_Instance"
        Me.txtSFSF_Instance.Size = New System.Drawing.Size(607, 21)
        Me.txtSFSF_Instance.TabIndex = 38
        '
        'txtCustomerName
        '
        Me.txtCustomerName.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCustomerName.Location = New System.Drawing.Point(118, 92)
        Me.txtCustomerName.Name = "txtCustomerName"
        Me.txtCustomerName.Size = New System.Drawing.Size(607, 21)
        Me.txtCustomerName.TabIndex = 37
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(22, 122)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 36
        Me.Label1.Text = "Instance:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(22, 95)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(60, 13)
        Me.Label6.TabIndex = 35
        Me.Label6.Text = "Customer:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ChkPretranslate)
        Me.GroupBox1.Controls.Add(Me.ChkInstance)
        Me.GroupBox1.Controls.Add(Me.ChkRestrictCustomer)
        Me.GroupBox1.Location = New System.Drawing.Point(21, 194)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(704, 75)
        Me.GroupBox1.TabIndex = 39
        Me.GroupBox1.TabStop = False
        '
        'ChkPretranslate
        '
        Me.ChkPretranslate.AutoSize = True
        Me.ChkPretranslate.Location = New System.Drawing.Point(25, 0)
        Me.ChkPretranslate.Name = "ChkPretranslate"
        Me.ChkPretranslate.Size = New System.Drawing.Size(255, 17)
        Me.ChkPretranslate.TabIndex = 41
        Me.ChkPretranslate.Text = "PreTranslate (Get translation from Cloud DB)"
        Me.ChkPretranslate.UseVisualStyleBackColor = True
        '
        'CmbCustomer
        '
        Me.CmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbCustomer.FormattingEnabled = True
        Me.CmbCustomer.Location = New System.Drawing.Point(118, 91)
        Me.CmbCustomer.Name = "CmbCustomer"
        Me.CmbCustomer.Size = New System.Drawing.Size(607, 21)
        Me.CmbCustomer.TabIndex = 41
        '
        'CmbInstance
        '
        Me.CmbInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbInstance.FormattingEnabled = True
        Me.CmbInstance.Location = New System.Drawing.Point(118, 118)
        Me.CmbInstance.Name = "CmbInstance"
        Me.CmbInstance.Size = New System.Drawing.Size(607, 21)
        Me.CmbInstance.TabIndex = 43
        '
        'GB2
        '
        Me.GB2.Controls.Add(Me.ChkCleanTranslation)
        Me.GB2.Controls.Add(Me.ChkMaxLength)
        Me.GB2.Controls.Add(Me.ChkCorrupt)
        Me.GB2.Controls.Add(Me.TextBox4)
        Me.GB2.Controls.Add(Me.Label8)
        Me.GB2.Controls.Add(Me.ChkUploadToDB)
        Me.GB2.Controls.Add(Me.Label7)
        Me.GB2.Controls.Add(Me.GroupBox1)
        Me.GB2.Location = New System.Drawing.Point(799, 43)
        Me.GB2.Name = "GB2"
        Me.GB2.Size = New System.Drawing.Size(767, 417)
        Me.GB2.TabIndex = 44
        Me.GB2.TabStop = False
        '
        'ChkCorrupt
        '
        Me.ChkCorrupt.AutoSize = True
        Me.ChkCorrupt.Location = New System.Drawing.Point(21, 54)
        Me.ChkCorrupt.Name = "ChkCorrupt"
        Me.ChkCorrupt.Size = New System.Drawing.Size(407, 17)
        Me.ChkCorrupt.TabIndex = 44
        Me.ChkCorrupt.Text = "Check Corrupted files (Identifies corrupt chars because of wrong encoding)"
        Me.ChkCorrupt.UseVisualStyleBackColor = True
        '
        'TextBox4
        '
        Me.TextBox4.AcceptsReturn = True
        Me.TextBox4.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox4.Location = New System.Drawing.Point(21, 318)
        Me.TextBox4.Multiline = True
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(703, 88)
        Me.TextBox4.TabIndex = 43
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(18, 287)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(148, 13)
        Me.Label8.TabIndex = 42
        Me.Label8.Text = "Key Words for DB exclusion"
        '
        'ChkUploadToDB
        '
        Me.ChkUploadToDB.AutoSize = True
        Me.ChkUploadToDB.Location = New System.Drawing.Point(21, 124)
        Me.ChkUploadToDB.Name = "ChkUploadToDB"
        Me.ChkUploadToDB.Size = New System.Drawing.Size(441, 17)
        Me.ChkUploadToDB.TabIndex = 41
        Me.ChkUploadToDB.Text = "Upload translation (when translation is back from translators, upload them to DB)" & _
    ""
        Me.ChkUploadToDB.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("BentonSans Bold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(22, 25)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(72, 19)
        Me.Label7.TabIndex = 40
        Me.Label7.Text = "Options"
        '
        'GB1
        '
        Me.GB1.Controls.Add(Me.ChkMaster)
        Me.GB1.Controls.Add(Me.CmbProjectGroup)
        Me.GB1.Controls.Add(Me.Label9)
        Me.GB1.Controls.Add(Me.Label2)
        Me.GB1.Controls.Add(Me.CmbInstance)
        Me.GB1.Controls.Add(Me.Lbl1)
        Me.GB1.Controls.Add(Me.CmbCustomer)
        Me.GB1.Controls.Add(Me.lblLanguage)
        Me.GB1.Controls.Add(Me.txtSFSF_Instance)
        Me.GB1.Controls.Add(Me.Label3)
        Me.GB1.Controls.Add(Me.Label1)
        Me.GB1.Controls.Add(Me.Label4)
        Me.GB1.Controls.Add(Me.Label6)
        Me.GB1.Controls.Add(Me.TextBox_ProjectName)
        Me.GB1.Controls.Add(Me.btnBrowseLang)
        Me.GB1.Controls.Add(Me.TextBox_ProjectPath)
        Me.GB1.Controls.Add(Me.ListBox1)
        Me.GB1.Controls.Add(Me.Button1)
        Me.GB1.Controls.Add(Me.TextBox_Description)
        Me.GB1.Controls.Add(Me.txtCustomerName)
        Me.GB1.Location = New System.Drawing.Point(1, 43)
        Me.GB1.Name = "GB1"
        Me.GB1.Size = New System.Drawing.Size(765, 417)
        Me.GB1.TabIndex = 45
        Me.GB1.TabStop = False
        '
        'ChkMaster
        '
        Me.ChkMaster.AutoSize = True
        Me.ChkMaster.Location = New System.Drawing.Point(589, 146)
        Me.ChkMaster.Name = "ChkMaster"
        Me.ChkMaster.Size = New System.Drawing.Size(136, 17)
        Me.ChkMaster.TabIndex = 46
        Me.ChkMaster.Text = "Set as Master Project"
        Me.ChkMaster.UseVisualStyleBackColor = True
        '
        'CmbProjectGroup
        '
        Me.CmbProjectGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbProjectGroup.FormattingEnabled = True
        Me.CmbProjectGroup.Location = New System.Drawing.Point(117, 11)
        Me.CmbProjectGroup.Name = "CmbProjectGroup"
        Me.CmbProjectGroup.Size = New System.Drawing.Size(608, 21)
        Me.CmbProjectGroup.TabIndex = 45
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(22, 14)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(80, 13)
        Me.Label9.TabIndex = 44
        Me.Label9.Text = "Project Group:"
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
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(766, 42)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 32
        Me.PictureBox1.TabStop = False
        '
        'Dialog1
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(768, 514)
        Me.Controls.Add(Me.GB1)
        Me.Controls.Add(Me.GB2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Font = New System.Drawing.Font("BentonSans Regular", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Dialog1"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GB2.ResumeLayout(False)
        Me.GB2.PerformLayout()
        Me.GB1.ResumeLayout(False)
        Me.GB1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Lbl1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextBox_ProjectName As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_ProjectPath As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox_Description As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Folder1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ListBox1 As System.Windows.Forms.CheckedListBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnBrowseLang As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents lblLanguage As System.Windows.Forms.Label
    Friend WithEvents ChkCleanTranslation As System.Windows.Forms.CheckBox
    Friend WithEvents txtSFSF_Instance As System.Windows.Forms.TextBox
    Friend WithEvents txtCustomerName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ChkInstance As System.Windows.Forms.CheckBox
    Friend WithEvents ChkRestrictCustomer As System.Windows.Forms.CheckBox
    Friend WithEvents CmbCustomer As System.Windows.Forms.ComboBox
    Friend WithEvents CmbInstance As System.Windows.Forms.ComboBox
    Friend WithEvents BtnBack As System.Windows.Forms.Button
    Friend WithEvents BtnNext As System.Windows.Forms.Button
    Friend WithEvents GB2 As System.Windows.Forms.GroupBox
    Friend WithEvents GB1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents BW As System.ComponentModel.BackgroundWorker
    Friend WithEvents ChkPretranslate As System.Windows.Forms.CheckBox
    Friend WithEvents ChkUploadToDB As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ChkCorrupt As System.Windows.Forms.CheckBox
    Friend WithEvents CmbProjectGroup As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ChkMaster As System.Windows.Forms.CheckBox
    Friend WithEvents ChkMaxLength As System.Windows.Forms.CheckBox

End Class
