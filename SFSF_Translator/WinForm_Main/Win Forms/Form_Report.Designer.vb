<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Report
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim ChartArea2 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend2 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.CmbDate = New System.Windows.Forms.ComboBox()
        Me.lblTransUnitCount = New System.Windows.Forms.Label()
        Me.lblWordCount = New System.Windows.Forms.Label()
        Me.lblFileCount = New System.Windows.Forms.Label()
        Me.CmbLang = New System.Windows.Forms.ComboBox()
        Me.lblCompletion = New System.Windows.Forms.Label()
        Me.ChartTimeLine = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChartTimeLine, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Chart1
        '
        ChartArea1.AxisX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet
        ChartArea1.AxisX.LineWidth = 0
        ChartArea1.AxisX.MajorGrid.Enabled = False
        ChartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet
        ChartArea1.AxisX.MajorTickMark.Enabled = False
        ChartArea1.AxisY.LabelStyle.Enabled = False
        ChartArea1.AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet
        ChartArea1.AxisY.LineWidth = 0
        ChartArea1.AxisY.MajorGrid.Enabled = False
        ChartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet
        ChartArea1.AxisY.MajorTickMark.Enabled = False
        ChartArea1.BackColor = System.Drawing.Color.Transparent
        ChartArea1.CursorX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Months
        ChartArea1.CursorX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash
        ChartArea1.CursorY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash
        ChartArea1.Name = "ChartArea1"
        Me.Chart1.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.Chart1.Legends.Add(Legend1)
        Me.Chart1.Location = New System.Drawing.Point(23, 12)
        Me.Chart1.Name = "Chart1"
        Me.Chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SemiTransparent
        Series1.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.Top
        Series1.BackSecondaryColor = System.Drawing.Color.Gainsboro
        Series1.BorderColor = System.Drawing.Color.Transparent
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
        Series1.Color = System.Drawing.Color.DeepSkyBlue
        Series1.EmptyPointStyle.LabelForeColor = System.Drawing.Color.White
        Series1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Series1.IsValueShownAsLabel = True
        Series1.LabelBackColor = System.Drawing.Color.Transparent
        Series1.LabelBorderColor = System.Drawing.Color.White
        Series1.LabelBorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet
        Series1.Legend = "Legend1"
        Series1.MarkerBorderColor = System.Drawing.Color.Black
        Series1.MarkerColor = System.Drawing.Color.Black
        Series1.Name = "Postedit"
        Series1.SmartLabelStyle.CalloutBackColor = System.Drawing.Color.Black
        Series1.SmartLabelStyle.CalloutLineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet
        Series1.SmartLabelStyle.CalloutStyle = System.Windows.Forms.DataVisualization.Charting.LabelCalloutStyle.None
        Me.Chart1.Series.Add(Series1)
        Me.Chart1.Size = New System.Drawing.Size(464, 174)
        Me.Chart1.TabIndex = 11
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToOrderColumns = True
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridView1.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridView1.Location = New System.Drawing.Point(776, 12)
        Me.DataGridView1.Name = "DataGridView1"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(567, 482)
        Me.DataGridView1.TabIndex = 12
        '
        'ComboBox1
        '
        Me.ComboBox1.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"[ALL]", "NewFile", "UnderTranslation", "BackFromTranslation", "Integrated"})
        Me.ComboBox1.Location = New System.Drawing.Point(649, 94)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox1.TabIndex = 13
        '
        'CmbDate
        '
        Me.CmbDate.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbDate.FormattingEnabled = True
        Me.CmbDate.Location = New System.Drawing.Point(649, 12)
        Me.CmbDate.Name = "CmbDate"
        Me.CmbDate.Size = New System.Drawing.Size(121, 21)
        Me.CmbDate.TabIndex = 14
        '
        'lblTransUnitCount
        '
        Me.lblTransUnitCount.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransUnitCount.Location = New System.Drawing.Point(629, 183)
        Me.lblTransUnitCount.Name = "lblTransUnitCount"
        Me.lblTransUnitCount.Size = New System.Drawing.Size(141, 13)
        Me.lblTransUnitCount.TabIndex = 15
        Me.lblTransUnitCount.Text = "TransUnit count"
        Me.lblTransUnitCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblWordCount
        '
        Me.lblWordCount.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWordCount.Location = New System.Drawing.Point(629, 159)
        Me.lblWordCount.Name = "lblWordCount"
        Me.lblWordCount.Size = New System.Drawing.Size(141, 13)
        Me.lblWordCount.TabIndex = 16
        Me.lblWordCount.Text = "Total Word Count"
        Me.lblWordCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFileCount
        '
        Me.lblFileCount.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileCount.Location = New System.Drawing.Point(629, 135)
        Me.lblFileCount.Name = "lblFileCount"
        Me.lblFileCount.Size = New System.Drawing.Size(141, 13)
        Me.lblFileCount.TabIndex = 17
        Me.lblFileCount.Text = "Files found count"
        Me.lblFileCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CmbLang
        '
        Me.CmbLang.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbLang.FormattingEnabled = True
        Me.CmbLang.Location = New System.Drawing.Point(649, 53)
        Me.CmbLang.Name = "CmbLang"
        Me.CmbLang.Size = New System.Drawing.Size(121, 21)
        Me.CmbLang.TabIndex = 18
        '
        'lblCompletion
        '
        Me.lblCompletion.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompletion.Location = New System.Drawing.Point(629, 207)
        Me.lblCompletion.Name = "lblCompletion"
        Me.lblCompletion.Size = New System.Drawing.Size(141, 13)
        Me.lblCompletion.TabIndex = 19
        Me.lblCompletion.Text = "TransUnit count"
        Me.lblCompletion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ChartTimeLine
        '
        Me.ChartTimeLine.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
        ChartArea2.AxisX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet
        ChartArea2.AxisX.LineWidth = 0
        ChartArea2.AxisX.MajorGrid.Enabled = False
        ChartArea2.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet
        ChartArea2.AxisX.MajorTickMark.Enabled = False
        ChartArea2.AxisY.LabelStyle.Enabled = False
        ChartArea2.AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet
        ChartArea2.AxisY.LineWidth = 0
        ChartArea2.AxisY.MajorGrid.Enabled = False
        ChartArea2.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet
        ChartArea2.AxisY.MajorTickMark.Enabled = False
        ChartArea2.BackColor = System.Drawing.Color.Transparent
        ChartArea2.CursorX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Months
        ChartArea2.CursorX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash
        ChartArea2.CursorY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash
        ChartArea2.Name = "ChartArea1"
        Me.ChartTimeLine.ChartAreas.Add(ChartArea2)
        Legend2.Name = "Legend1"
        Me.ChartTimeLine.Legends.Add(Legend2)
        Me.ChartTimeLine.Location = New System.Drawing.Point(24, 233)
        Me.ChartTimeLine.Name = "ChartTimeLine"
        Me.ChartTimeLine.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SemiTransparent
        Me.ChartTimeLine.Size = New System.Drawing.Size(730, 261)
        Me.ChartTimeLine.TabIndex = 20
        Me.ChartTimeLine.Text = "Time Line"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(613, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 13)
        Me.Label1.TabIndex = 21
        Me.Label1.Text = "Date"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(589, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "Language"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("BentonSans Book", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(606, 98)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 13)
        Me.Label3.TabIndex = 23
        Me.Label3.Text = "Status"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("BentonSans Bold", 12.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.Location = New System.Drawing.Point(21, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(54, 19)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "Chart"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("BentonSans Bold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(21, 233)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(77, 19)
        Me.Label5.TabIndex = 25
        Me.Label5.Text = "Timeline"
        '
        'Form_Report
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1355, 503)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ChartTimeLine)
        Me.Controls.Add(Me.lblCompletion)
        Me.Controls.Add(Me.CmbLang)
        Me.Controls.Add(Me.lblFileCount)
        Me.Controls.Add(Me.lblWordCount)
        Me.Controls.Add(Me.lblTransUnitCount)
        Me.Controls.Add(Me.CmbDate)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Chart1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MinimizeBox = False
        Me.Name = "Form_Report"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChartTimeLine, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents CmbDate As System.Windows.Forms.ComboBox
    Friend WithEvents lblTransUnitCount As System.Windows.Forms.Label
    Friend WithEvents lblWordCount As System.Windows.Forms.Label
    Friend WithEvents lblFileCount As System.Windows.Forms.Label
    Friend WithEvents CmbLang As System.Windows.Forms.ComboBox
    Friend WithEvents lblCompletion As System.Windows.Forms.Label
    Friend WithEvents ChartTimeLine As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
