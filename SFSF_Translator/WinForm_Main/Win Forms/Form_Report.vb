Public Class Form_Report

    Private Property NewFiles As Integer
    Private Property UnderTranslation As Integer
    Private Property BackFromTranslation As Integer
    Private Property Integrated As Integer

    Private DS As DataSet

    Private mcr As CloudReporting.MyCloudReport

    Private ActiveProject As ProjectDetail

    Private Sub Form_Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        mcr = CloudReporting.MyCloudReport.GetInstance

        For i As Integer = 0 To mcr.DateCollection.Count - 1
            CmbDate.Items.Add(mcr.DateCollection(i))
        Next

        Dim myXMLfile As String = ProjectManagement.GetActiveProject.ProjectPath & "ProjectStats.xml"
        DS = New DataSet()
        ' Create new FileStream with which to read the schema.
        Dim fsReadXml As New System.IO.FileStream(myXMLfile, System.IO.FileMode.Open)
        Try
            DS.ReadXml(fsReadXml)
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        Finally
            fsReadXml.Close()
        End Try

        ComboBox1.SelectedIndex = 0
        CmbDate.SelectedIndex = CmbDate.Items.Count - 1

        ActiveProject = ProjectManagement.GetActiveProject
        UpdateLang()

        'Time Line
        UpdateTimeLineChart()

    End Sub

    Private Sub UpdateTimeLineChart()
        Try
            Dim mNewFile As Integer = 0
            Dim mUnderTranslation As Integer = 0
            Dim mBackFromTranslation As Integer = 0
            Dim mIntegrated As Integer = 0
            For i As Integer = 0 To CmbDate.Items.Count - 1
                ChartTimeLine.Series.Add("Series" & i)
                mNewFile = GetFileCount(mcr.FStat, CloudReporting.FileStats.ProjectStatus.NewFile, CmbDate.Items(i))
                mUnderTranslation = GetFileCount(mcr.FStat, CloudReporting.FileStats.ProjectStatus.UnderTranslation, CmbDate.Items(i))
                mBackFromTranslation = GetFileCount(mcr.FStat, CloudReporting.FileStats.ProjectStatus.BackFromTranslation, CmbDate.Items(i))
                mIntegrated = GetFileCount(mcr.FStat, CloudReporting.FileStats.ProjectStatus.Integrated, CmbDate.Items(i))

                ChartTimeLine.Series("Series" & i).Points.AddXY("NewFiles", mNewFile)
                ChartTimeLine.Series("Series" & i).Points.AddXY("UnderTranslation", mUnderTranslation)
                ChartTimeLine.Series("Series" & i).Points.AddXY("BackFromTranslation", mBackFromTranslation)
                ChartTimeLine.Series("Series" & i).Points.AddXY("Integrated", mIntegrated)
                ChartTimeLine.Series("Series" & i).LegendText = CmbDate.Items(i).ToString

            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub UpdateLang()
        Dim Lang() As String = ActiveProject.LangList.Split(",")
        CmbLang.Items.Clear()
        CmbLang.Items.Add("[All]")
        For i As Integer = 0 To UBound(Lang)
            CmbLang.Items.Add(Lang(i).Insert(2, "_"))
        Next
        CmbLang.SelectedIndex = 0
    End Sub

    Public CS As CloudStats

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        NewFiles = GetFileCount(mcr.FStat, CloudReporting.FileStats.ProjectStatus.NewFile, CmbDate.Text)
        UnderTranslation = GetFileCount(mcr.FStat, CloudReporting.FileStats.ProjectStatus.UnderTranslation, CmbDate.Text)
        BackFromTranslation = GetFileCount(mcr.FStat, CloudReporting.FileStats.ProjectStatus.BackFromTranslation, CmbDate.Text)
        Integrated = GetFileCount(mcr.FStat, CloudReporting.FileStats.ProjectStatus.Integrated, CmbDate.Text)

        Chart1.Series("Postedit").Points.Clear()

        Chart1.Series("Postedit").Points.AddXY("NewFiles", Me.NewFiles)
        Chart1.Series("Postedit").Points.AddXY("UnderTranslation", Me.UnderTranslation)
        Chart1.Series("Postedit").Points.AddXY("BackFromTranslation", Me.BackFromTranslation)
        Chart1.Series("Postedit").Points.AddXY("Integrated", Me.Integrated)

        LoadDV()

    End Sub

    Private Sub LoadDV()
        DataGridView1.DataSource = Nothing
        DataGridView1.Rows.Clear()

        Dim dv As DataView = New DataView(DS.Tables("FileStats"))

        Dim Query As String = ""

        If ComboBox1.SelectedIndex > 0 Then
            Query = "FileStatus = '" & ComboBox1.Text & "' And mDate = '" & CmbDate.Text & "'"
        Else
            Query = "mDate = '" & CmbDate.Text & "'"
        End If

        If CmbLang.SelectedIndex > 0 Then
            Query = Query & " And Lang = '" & CmbLang.Text & "'"
        End If

        dv = New DataView(DS.Tables("FileStats"), Query, "FileStatus Desc", DataViewRowState.CurrentRows)

        DataGridView1.DataSource = dv

        DataGridView1.Columns(DataGridView1.Columns.Count - 1).Visible = False 'Last Column is Date, hide it

        UpdateLabels()

    End Sub

    Private Function GetFileCount(ByVal Projects As List(Of CloudReporting.FileStats), ByVal Stat As CloudReporting.FileStats.ProjectStatus, ByVal mDate As String) As Integer
        Dim FileCount As Integer = 0

        For j As Integer = 0 To Projects.Count - 1
            If Projects(j).FileStatus = Stat And Projects(j).mDate = mDate Then
                FileCount += 1
            End If
        Next
        Return FileCount
    End Function

    Private Sub CmbDate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbDate.SelectedIndexChanged
        Me.ComboBox1_SelectedIndexChanged(sender, e)
    End Sub

    Private Sub UpdateLabels()
        lblFileCount.Text = "Files found - " & DataGridView1.Rows.Count
        lblTransUnitCount.Text = "Transunit found - " & GetDVCount("TransUnitCount")
        lblWordCount.Text = "Word Count - " & GetDVCount("WordCount")
        If Integrated > 0 Then
            lblCompletion.Text = "Compeleted - " & Math.Round((Integrated / (NewFiles + UnderTranslation + BackFromTranslation + Integrated) * 100), 2) & "%"
        Else
            lblCompletion.Text = "Compeleted - 0%"
        End If
    End Sub

    Private Function GetDVCount(ByVal ColName As String) As Integer
        Dim total As Integer = 0
        For Each row As DataGridViewRow In DataGridView1.Rows
            total += row.Cells(ColName).Value
        Next
        Return total
    End Function

    Private Sub CmbLang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbLang.SelectedIndexChanged
        ComboBox1_SelectedIndexChanged(sender, e)
    End Sub
End Class


