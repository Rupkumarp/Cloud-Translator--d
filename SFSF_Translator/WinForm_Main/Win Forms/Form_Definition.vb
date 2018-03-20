Imports System.IO
Imports Microsoft.VisualBasic.FileIO
Imports System.Environment

Public Class Form_Definition

    Private _sFileName As String = ""

    Public Property DefinitionType As DefinitionFiles.DefintionType = DefinitionFiles.DefintionType.FileType_List
    Dim DefintionFileList = DefinitionFiles.EnumConstants(Of String)(GetType(DefinitionFiles))

    Private Sub Form_Definition_Load(sender As Object, e As EventArgs) Handles Me.Load
        ToolStripMenuItem2.SelectedIndex = 0
        Dim counter As Integer = 0

        DefintionFileList.sort()

        For Each Line In DefintionFileList
            Try
                TV.Nodes.Add(System.IO.Path.GetFileNameWithoutExtension(Line))
                If Line.ToLower.Trim = DefinitionFiles.Xml_List.ToLower.Trim Then
                    Dim str() As String = Split(IO.File.ReadAllText(appData & DefinitionFiles.Xml_List), vbCrLf)
                    For i As Integer = 0 To UBound(str)
                        If str(i) <> "" Then
                            Dim Dom() As String = Split(str(i), vbTab)
                            TV.Nodes(counter).Nodes.Add(Dom(0) & "-" & Dom(1))
                        End If
                    Next
                End If
            Catch ex As Exception
                Throw New Exception("Could not Copy " & System.IO.Path.GetFileName(Line))
            End Try
            counter += 1
        Next
    End Sub

    Dim bSource As New BindingSource

    Private Sub LoadDefintion(ByVal sFileName As String)
        _sFileName = sFileName
        Dim dt As DataTable
        dt = GetDefintionToDatatable(sFileName)
        bSource.DataSource = dt
        DV.DataSource = Nothing
        DV.DataSource = bSource
        If DV.Columns.Count > 3 Then
            DV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Else
            DV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        End If
    End Sub

    Public Function GetDefintionToDatatable(ByVal sDefinitionFile As String) As DataTable
        Dim csvData As New DataTable()

        If Not System.IO.File.Exists(sDefinitionFile) Then
            Return csvData
        End If

        Try
            'First Get the Max last column used in csv file, this is used to set the column limit for datatable------------------------------------------
            Dim LastCol As Integer = 0
            Using csvReader As New TextFieldParser(sDefinitionFile)
                csvReader.SetDelimiters(New String() {vbTab})
                csvReader.HasFieldsEnclosedInQuotes = True
                While Not csvReader.EndOfData
                    Dim fieldData As String() = csvReader.ReadFields()
                    If LastCol < UBound(fieldData) Then
                        LastCol = UBound(fieldData)
                    End If
                End While
            End Using
            '---------------------------------------------------------------------------------------------------------------------------------------------

            'Now load the csv data to datatable
            Using csvReader As New TextFieldParser(sDefinitionFile)
                csvReader.SetDelimiters(New String() {vbTab})
                csvReader.HasFieldsEnclosedInQuotes = True

                For i As Integer = 0 To LastCol
                    csvData.Columns.Add(i, GetType(String))
                Next

                While Not csvReader.EndOfData
                    Dim fieldData As String() = csvReader.ReadFields()
                    'Making empty value as null
                    For i As Integer = 0 To fieldData.Length - 2
                        If fieldData(i) = "" Then
                            fieldData(i) = Nothing
                        End If
                    Next
                    csvData.Rows.Add(fieldData)
                End While
            End Using
        Catch ex As Exception
            Throw New Exception("Error loading Defintion file to datatable!" & vbNewLine & ex.Message)
        End Try

        Return csvData

    End Function


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If _sFileName.Trim = "" Then
            Exit Sub
        End If
        _sFileName = Replace(_sFileName, appData, Application.StartupPath)

        LoadDefintion(_sFileName)
        _sFileName = Replace(_sFileName, Application.StartupPath, appData)
        DV.CurrentCell = Nothing
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If _sFileName Is Nothing Or _sFileName.Trim = "" Then
            Exit Sub
        End If

        If MsgBox("This will replace the existing file settings!" & vbNewLine & "Are you sure you want to replace the file?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Definition File") = MsgBoxResult.No Then
            Exit Sub
        End If

        Try
            saveDefinition()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Defintion Files")
        End Try

    End Sub

    Sub saveDefinition()

        Try
            Using Writer As StreamWriter = New StreamWriter(_sFileName, False)
                Dim str As String = ""
                For i As Integer = 0 To DV.Rows.Count - 1
                    For j As Integer = 0 To DV.Columns.Count - 1
                        If Not IsDBNull(DV.Rows(i).Cells(j).Value) Then
                            If DV.Rows(i).Cells(j).Value <> "" Then
                                If str = "" Then
                                    str = DV.Rows(i).Cells(j).Value
                                Else
                                    str = str & vbTab & DV.Rows(i).Cells(j).Value
                                End If
                            End If
                        End If
                    Next
                    If str <> "" Then
                        Writer.WriteLine(str)
                    End If
                    str = ""
                Next
            End Using
        Catch ex As ArgumentNullException
            Throw New Exception("Save filename is invalied!")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
       
    End Sub

    Private Sub TV_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TV.AfterSelect
        Dim NodeText As String = e.Node.Text
        For Each Line In DefintionFileList
            Try
                If System.IO.Path.GetFileNameWithoutExtension(Line) = NodeText Then
                    _sFileName = appData & Line
                    Exit For
                End If
                If Line.ToLower.Trim = DefinitionFiles.Xml_List.ToLower.Trim Then
                    Dim str() As String = Split(IO.File.ReadAllText(appData & DefinitionFiles.Xml_List), vbCrLf)
                    For i As Integer = 0 To UBound(str)
                        If str(i).Trim <> "" Then
                            Dim Dom() As String = Split(str(i), vbTab)
                            If Dom(0) & "-" & Dom(1) = NodeText Then
                                _sFileName = appData & "\Definition\XML_Definition\" & Dom(0) & ".txt"
                                Exit For
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                Throw New Exception("Could not Copy " & System.IO.Path.GetFileName(Line))
            End Try

        Next
        If Not System.IO.File.Exists(_sFileName) Then
            DV.DataSource = Nothing
            _sFileName = ""
            MsgBox("No Definition file found for the item selected!", MsgBoxStyle.Exclamation, "Definition file")
            Exit Sub
        End If
        LoadDefintion(_sFileName)
        Me.DV.CurrentCell = Nothing
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        If IsNothing(DV.DataSource) Then
            e.Cancel = True
        End If
        If (DV.SelectedCells.Count = 0) Then
            e.Cancel = True
        End If
    End Sub

  
    Private Sub DV_MouseDown(sender As Object, e As MouseEventArgs) Handles DV.MouseDown

        If IsNothing(DV.DataSource) Then
            Exit Sub
        End If
        If e.Button = Windows.Forms.MouseButtons.Right Then

            Dim ht As DataGridView.HitTestInfo
            ht = Me.DV.HitTest(e.X, e.Y)
            If ht.Type = DataGridViewHitTestType.Cell Then
                DV.Rows(ht.RowIndex).Cells(ht.ColumnIndex).Selected = True
                DV.CurrentCell = DV.Rows(ht.RowIndex).Cells(ht.ColumnIndex)
            ElseIf ht.Type = DataGridViewHitTestType.RowHeader Then
                ' DV.CurrentRow = DV.Rows(0)
                ' DV.Rows(ht.RowIndex).Selected = True
            ElseIf ht.Type = DataGridViewHitTestType.ColumnHeader Then
                'do nothing
            End If

            'Try
            '    DV.CurrentCell = DV.Rows(DV.HitTest(e.Location.X, e.Location.Y).RowIndex).Cells(DV.HitTest(e.Location.X, e.Location.Y).ColumnIndex)
            'Catch ex As System.ArgumentOutOfRangeException
            'Catch ex As Exception
            '    Throw New Exception(ex.Message)
            'End Try

            'DV.Rows(DV.HitTest(e.Location.X, e.Location.Y).RowIndex).Cells(DV.HitTest(e.Location.X, e.Location.Y).ColumnIndex).Selected = True
        End If
    End Sub


    'Private Sub dgvReturnSeries_CellMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DV.CellMouseClick
    '    'if the datagridview was right clicked, set the currentcell to the right-clicked cell and then show a context menu with options for filling the right-clicked cell
    '    If (e.Button = Windows.Forms.MouseButtons.Right) Then
    '        Try
    '            'highlight the right-clicked cell, make it the dgv's current cell
    '            DV.CurrentRow.Selected = False
    '            DV.Rows(e.RowIndex).Selected = True
    '            DV.CurrentCell = DV.Rows(e.RowIndex).Cells(e.ColumnIndex)
    '            DV.ContextMenuStrip = ContextMenuStrip1
    '        Catch ex As Exception
    '            MsgBox(ex.Message)
    '        End Try

    '    End If
    'End Sub
   
    'Private Sub cutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CutToolStripMenuItem.Click
    '    ' Copy to clipboard
    '    CopyToClipboard()
    '    ' Clear selected cells
    '    For Each dgvCell As DataGridViewCell In DV.SelectedCells
    '        dgvCell.Value = String.Empty
    '    Next
    'End Sub
    Private Sub copyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        CopyToClipboard()
    End Sub
    Private Sub pasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click
        ' Perform paste Operation
        PasteClipboardValue()
    End Sub
    Private Sub CopyToClipboard()
        ' Copy to clipboard
        Dim dataObj As DataObject = DV.GetClipboardContent()
        If (Not IsNothing(dataObj)) Then
            Clipboard.SetDataObject(dataObj)
        End If
    End Sub
    Private Sub PasteClipboardValue()
        ' Show Error if no cell is selected
        If (DV.SelectedCells.Count = 0) Then
            MessageBox.Show("Please select a cell", "Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get the starting Cell
        Dim startCell As DataGridViewCell = GetStartCell(DV)
        ' Get the clipboard value in a dictionary
        Dim cbValue As New Dictionary(Of Integer, Dictionary(Of Integer, String))
        cbValue = ClipBoardValues(Clipboard.GetText())

        Dim iRowIndex As Integer = startCell.RowIndex
        For Each rowKey As Integer In cbValue.Keys
            Dim iColIndex As Integer = startCell.ColumnIndex
            For Each cellKey As Integer In cbValue(rowKey).Keys
                ' Check if the index is within the limit
                If (iColIndex <= DV.Columns.Count - 1 And _
                iRowIndex <= DV.Rows.Count - 1) Then
                    Dim cell As DataGridViewCell = DV(iColIndex, iRowIndex)
                    cell.Value = cbValue(rowKey)(cellKey)
                End If
                iColIndex += 1
            Next
            iRowIndex += 1
        Next
    End Sub
    Private Function GetStartCell(dgView As DataGridView) As DataGridViewCell
        ' get the smallest row,column index
        If (dgView.SelectedCells.Count = 0) Then
            Return Nothing
        End If
        Dim rowIndex As Integer = dgView.Rows.Count - 1
        Dim colIndex As Integer = dgView.Columns.Count - 1

        For Each dgvCell As DataGridViewCell In dgView.SelectedCells
            If (dgvCell.RowIndex < rowIndex) Then
                rowIndex = dgvCell.RowIndex
            End If
            If (dgvCell.ColumnIndex < colIndex) Then
                colIndex = dgvCell.ColumnIndex
            End If
        Next
        Return dgView(colIndex, rowIndex)
    End Function
    Private Function ClipBoardValues(clipboardValue As String) As Dictionary(Of Integer, Dictionary(Of Integer, String))
        Dim copyValues As Dictionary(Of Integer, Dictionary(Of Integer, String)) = _
        New Dictionary(Of Integer, Dictionary(Of Integer, String))()

        Dim lines As String() = clipboardValue.Split(vbCrLf)

        For i As Integer = 0 To lines.Length - 1
            copyValues(i) = New Dictionary(Of Integer, String)()
            Dim lineContent As String() = lines(i).Split(vbTab)

            ' if an empty cell value copied, then set the dictionary with an empty string
            ' else Set value to dictionary
            If (lineContent.Length = 0) Then
                copyValues(i)(0) = String.Empty
            Else
                For j As Integer = 0 To lineContent.Length - 1
                    copyValues(i)(j) = lineContent(j)
                Next
            End If
        Next
        Return copyValues
    End Function


    Private Sub OKToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OKToolStripMenuItem.Click
        ' Dim dsNewRow As DataRow
        DV.DataSource.addnew()

        DV.DataSource.endedit()
    End Sub

End Class


Public Class NativeTreeView : Inherits TreeView

    Private Declare Unicode Function SetWindowTheme Lib "uxtheme.dll" (hWnd As IntPtr, pszSubAppName As String, pszSubIdList As String) As Integer

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        SetWindowTheme(Me.Handle, "Explorer", Nothing)
    End Sub

End Class