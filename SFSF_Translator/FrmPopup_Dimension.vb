Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Public Class FrmPopup_Dimension
    Public DIMENSION_Content As String = ""
    Public FileName As String = ""
    Dim table_FileData As DataTable
    Dim table_Xliff As New DataTable


    Private Sub FrmPopup_Dimension_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = FileName
        Traverse_Json_DIMENSION(DIMENSION_Content)
        cmb_ColSelection.Focus()
    End Sub

#Region "Control Events"
    Private Sub btn_skip_Click(sender As Object, e As EventArgs) Handles btn_skip.Click
        Mod_Cube1.tbl_DIMENSION = Nothing
        Me.Close()
    End Sub
    Private Sub btn_ok_Click(sender As Object, e As EventArgs) Handles btn_ok.Click
        Try
            '' select Column, for translation
            If cmb_ColSelection.Text.Trim = "" Then
                MessageBox.Show("Please select one column for Translation.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            '' Create table for Xliff
            table_Xliff = New DataTable
            table_Xliff.Columns.Add("ID", GetType(String))
            table_Xliff.Columns.Add("PropName", GetType(String))
            table_Xliff.Columns.Add("PropVal", GetType(String))
            Dim iUniqueID As Integer = 0
            For Each row As DataRow In table_FileData.Rows
                table_Xliff.Rows.Add(iUniqueID.ToString(), "DUMMY_PROP_" + iUniqueID.ToString, row(Convert.ToInt16(cmb_ColSelection.Text) - 1).ToString())
                iUniqueID += 1
            Next
            Mod_Cube1.tbl_DIMENSION = table_Xliff
            Me.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        Mod_Cube1.tbl_DIMENSION = Nothing
        Me.Close()
    End Sub
    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
#End Region

#Region "Methods"
    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H2000000
            ' WS_EX_COMPOSITED
            Return cp
        End Get
    End Property
    Private Sub Traverse_Json_DIMENSION(ByVal str_jsonContent As String)
        Try

            Dim Jsondata As JToken = JObject.Parse(str_jsonContent)
            Dim str_jsonPart As String = Jsondata.SelectToken("content").ToString()

            str_jsonPart = str_jsonPart.Remove(0, 1)
            str_jsonPart = str_jsonPart.Remove(str_jsonPart.Length - 1)
            str_jsonPart = str_jsonPart.Replace(vbCr, "").Replace(vbLf, "")
            str_jsonPart = str_jsonPart.Replace("],  [", "^")
            'str_jsonPart = str_jsonPart.Replace("],", "^")
            str_jsonPart = str_jsonPart.Replace("[", "").Replace("]", "")

            Dim json_EachBlock() As String = str_jsonPart.Split("^")

            '' Datatable with column
            cmb_ColSelection.Items.Clear()
            cmb_ColSelection.Text = ""
            table_FileData = New DataTable
            For ic As Integer = 0 To json_EachBlock(0).Split(",").Length - 1
                table_FileData.Columns.Add()
                cmb_ColSelection.Items.Add(ic + 1)
            Next

            '' Loop for All file content (rows)
            For icnt As Integer = 0 To json_EachBlock.Count - 1
                Dim json_EachVal As String() = json_EachBlock(icnt).Split(""",")

                Dim dr As DataRow = table_FileData.NewRow()
                Dim rowcount As Integer = 0
                '' Loop for each rows
                For icntindv As Integer = 1 To json_EachVal.Count - 2
                    If json_EachVal(icntindv).ToString.Trim <> "," Then
                        Dim json_Lastval As String = LTrim(RTrim(json_EachVal(icntindv).Replace("""", "")))
                        dr(rowcount) = json_Lastval
                        rowcount += 1
                    End If
                Next
                table_FileData.Rows.Add(dr)
            Next

            '' Making Grid
            DataGridView1.DataSource = table_FileData
            Dim eachColLength As Integer = (DataGridView1.Width / DataGridView1.ColumnCount)
            For colIndex = 0 To DataGridView1.ColumnCount - 1
                DataGridView1.Columns(colIndex).Width = eachColLength
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub
    Private Sub addCheckBoxCell(newRow As DataGridViewRow, p As String)
        Dim checkbox As New DataGridViewCheckBoxCell()
        checkbox.IndeterminateValue = "0"
        checkbox.FalseValue = "0"
        checkbox.TrueValue = "1"
        'If Boolean.Parse(p) Then
        '    checkbox.Value = "1"
        'Else
        '    checkbox.Value = 0
        'End If
        newRow.Cells.Add(checkbox)
    End Sub
#End Region
End Class