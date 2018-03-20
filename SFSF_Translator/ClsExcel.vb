Imports Excel = Microsoft.Office.Interop.Excel

Public Class ClsExcel

    Dim xlApp As New Excel.Application
    Dim xlWkb As Excel.Workbook
    Dim xlShtIN As Excel.Worksheet

    Public Sub CreateReport(ByVal targetFileName As String, ByVal objxliff As sXliff)
        Try

            Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            xlApp = DirectCast(CreateObject("Excel.Application"), Excel.Application)
            xlWkb = xlApp.Workbooks.Add
            xlShtIN = xlWkb.Worksheets(1)

            xlApp.Visible = False

            xlShtIN.Cells.NumberFormat = "@"

            xlShtIN.Cells(1, 1) = "Trans-unit ID"
            xlShtIN.Cells(1, 2) = "Resname"
            xlShtIN.Cells(1, 3) = "Source"
            xlShtIN.Cells(1, 4) = "Note"
            xlShtIN.Cells(1, 5) = "Target"

            Form_XliffToXlsConverter.ToolStripProgressBar1.Maximum = objxliff.ID.Count - 1
            For i = 0 To objxliff.ID.Count - 1
                Try
                    xlShtIN.Cells(i + 2, 1) = objxliff.ID(i).ToString()
                Catch ex As Exception
                    xlShtIN.Cells(i + 2, 1) = "Id not found"
                End Try

                Try
                    xlShtIN.Cells(i + 2, 2) = objxliff.Resname(i).ToString()
                Catch ex As Exception
                    xlShtIN.Cells(i + 2, 2) = "test" 'objxliff.Resname(i).ToString()
                End Try

                Try
                    xlShtIN.Cells(i + 2, 3) = objxliff.Source(i).ToString()
                Catch ex As Exception
                    xlShtIN.Cells(i + 2, 3) = ""
                End Try

                Try
                    xlShtIN.Cells(i + 2, 4) = objxliff.Note(i + 1).ToString()
                Catch ex As Exception
                    xlShtIN.Cells(i + 2, 4) = ""
                End Try

                Try
                    xlShtIN.Cells(i + 2, 5) = objxliff.Source(i).ToString()
                    If objxliff.Source(i).ToString().Trim.Length > 0 Then
                        xlShtIN.Cells(i + 2, 5) = objxliff.Translation(i).ToString()
                    End If

                Catch ex As Exception
                    xlShtIN.Cells(i + 2, 5) = ""
                End Try
                Form_XliffToXlsConverter.ToolStripProgressBar1.Value = i
            Next

            PerformFormatting()
            Try
                xlShtIN.SaveAs(targetFileName)
            Catch ex As Exception

            End Try
            releaseObject(xlShtIN)
            '~~> Close the File
            xlWkb.Close()
            releaseObject(xlWkb)
            '~~> Quit the Excel Application
            xlApp.Quit()
            releaseObject(xlApp)
        Catch ex As Exception
            xlApp = Nothing
            Throw New Exception(ex.Message)
        Finally
            xlApp = Nothing
        End Try

    End Sub

    Private Function EscapeNothing(ByVal str As String) As String
        If Not str Is Nothing Then
            Return str
        Else
            Return ""
        End If
    End Function

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    Public Function TranslateXlf(ByVal objXlf As xlf, ByVal translatedXlFile As String) As xlf
        Try
            Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            xlApp = DirectCast(CreateObject("Excel.Application"), Excel.Application)
            xlWkb = xlApp.Workbooks.Open(translatedXlFile)
            xlShtIN = xlWkb.Worksheets(1)

            Form_XLFtoExcel.ToolStripProgressBar1.Maximum = objXlf.file.Count - 1

            Dim lastRow As Integer = xlShtIN.Range("A66536").Rows.End(Excel.XlDirection.xlUp).Row

            For i = 0 To objXlf.file.Count - 1
                For j As Integer = 0 To objXlf.file(i).body.Count - 1
                    objXlf.file(i).body(j).target = GetTranslatedContent(xlShtIN, objXlf.file(i).body(j).source, lastRow)
                Next
                Form_XLFtoExcel.ToolStripProgressBar1.Value = i
            Next

            releaseObject(xlShtIN)
            xlWkb.Close()
            releaseObject(xlWkb)
            xlApp.Quit()
            releaseObject(xlApp)
        Catch ex As Exception
            xlWkb.Close()
            xlApp.Quit()
        End Try
        Return objXlf
    End Function

    Private Function GetTranslatedContent(ByVal xlSht As Excel.Worksheet, ByVal searchString As String, ByVal lastrow As Integer) As String

        Dim foundRange As Excel.Range = xlSht.Range("C1", "C" & lastrow).Find(What:=searchString, After:=xlSht.Range("C1"), LookIn:=
        -4123, LookAt:=1, SearchOrder:=1, SearchDirection:=
        1, MatchCase:=False, SearchFormat:=False)

        If Not foundRange Is Nothing Then
            Return foundRange.Offset(0, 2).Value
        End If

        For i As Integer = 2 To lastrow
            If String.Compare(xlShtIN.Range("C" & i).Value, searchString, True) Then
                Return xlShtIN.Range("E" & i).Value
            End If
        Next

        Return searchString
    End Function


    Public Sub CreateReportXlfToExcel(ByVal targetFileName As String, ByVal objXlf As xlf)
        Try
            Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            xlApp = DirectCast(CreateObject("Excel.Application"), Excel.Application)
            xlWkb = xlApp.Workbooks.Add
            xlShtIN = xlWkb.Worksheets(1)

            xlApp.Visible = False

            xlShtIN.Cells.NumberFormat = "@"

            xlShtIN.Cells(1, 1) = "Trans-unit ID"
            xlShtIN.Cells(1, 2) = "Resname"
            xlShtIN.Cells(1, 3) = "Source"
            xlShtIN.Cells(1, 4) = "Note"
            xlShtIN.Cells(1, 5) = "Target"

            Form_XLFtoExcel.ToolStripProgressBar1.Maximum = objXlf.file.Count - 1

            Dim content As New ArrayList
            Dim cellCounter As Integer = 2

            For i = 0 To objXlf.file.Count - 1
                For j As Integer = 0 To objXlf.file(i).body.Count - 1
                    If Not content.Contains(objXlf.file(i).body(j).source.ToString) Then
                        content.Add(objXlf.file(i).body(j).source.ToString)
                        xlShtIN.Cells(cellCounter, 1) = objXlf.file(i).body(j).id.ToString
                        xlShtIN.Cells(cellCounter, 2) = objXlf.file(i).body(j).resname.ToString
                        xlShtIN.Cells(cellCounter, 3) = objXlf.file(i).body(j).source.ToString
                        xlShtIN.Cells(cellCounter, 4) = objXlf.file(i).body(j).sizeunit.ToString
                        xlShtIN.Cells(cellCounter, 5) = objXlf.file(i).body(j).source.ToString
                        cellCounter += 1
                    End If
                Next
                Form_XLFtoExcel.ToolStripProgressBar1.Value = i
            Next

            PerformFormatting()
            Try
                xlShtIN.SaveAs(targetFileName)
            Catch ex As Exception

            End Try
            releaseObject(xlShtIN)
            '~~> Close the File
            xlWkb.Close()
            releaseObject(xlWkb)
            '~~> Quit the Excel Application
            xlApp.Quit()
            releaseObject(xlApp)
        Catch ex As Exception
            xlApp = Nothing
            Throw New Exception(ex.Message)
        Finally
            xlApp = Nothing
        End Try
    End Sub

    Private Sub PerformFormatting()

        xlShtIN.Cells.EntireColumn.AutoFit()

        With xlShtIN.Range("A1:E1").Interior
            .Pattern = 1
            .PatternColorIndex = -4105
            .Color = Color.Blue
            .TintAndShade = 0
            .PatternTintAndShade = 0
        End With

        Dim lastrow As Integer = xlShtIN.Range("A66532").End(Excel.XlDirection.xlUp).Row
        lastrow = 1000

        xlShtIN.Range("A1:E" & lastrow).Borders(5).LineStyle = -4142
        xlShtIN.Range("A1:E" & lastrow).Borders(6).LineStyle = -4142

        With xlShtIN.Range("A1:E" & lastrow).Borders(7)
            .LineStyle = 1
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = 2
        End With

        With xlShtIN.Range("A1:E" & lastrow).Borders(8)
            .LineStyle = 1
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = 2
        End With
        With xlShtIN.Range("A1:E" & lastrow).Borders(9)
            .LineStyle = 1
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = 2
        End With
        With xlShtIN.Range("A1:E" & lastrow).Borders(10)
            .LineStyle = 1
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = 2
        End With
        With xlShtIN.Range("A1:E" & lastrow).Borders(11)
            .LineStyle = 1
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = 2
        End With
        With xlShtIN.Range("A1:E" & lastrow).Borders(12)
            .LineStyle = 1
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = 2
        End With

        xlShtIN.Range("E1").Font.Color = Color.Yellow

        With xlShtIN.Range("A1:D" & xlShtIN.Range("D66536").End(Excel.XlDirection.xlUp).Row).Font
            .Color = Color.Yellow
            .TintAndShade = 0
        End With

    End Sub

#Region "Analyze Function"

    Public Event UpdateProgress(ByVal percent As Integer)

    Public Sub CreateAnalyzeReport(ByVal objAnalyzeStats As List(Of AnalyzeStats), writeToText As Boolean)
        Try
            Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            xlApp = DirectCast(CreateObject("Excel.Application"), Excel.Application)
            xlWkb = xlApp.Workbooks.Add
            xlShtIN = xlWkb.Worksheets(1)

            xlApp.Visible = False

            xlShtIN.Cells.NumberFormat = "@"

            xlShtIN.Cells(1, 1) = "Object Type"
            xlShtIN.Cells(1, 2) = "TranslationType"
            xlShtIN.Cells(1, 3) = "Local Source"
            xlShtIN.Cells(1, 4) = "Local Translation"
            xlShtIN.Cells(1, 5) = "Lang"
            xlShtIN.Cells(1, 6) = "DB Source"
            xlShtIN.Cells(1, 7) = "DB Translation"

            xlShtIN.Columns("C:G").EntireColumn.AutoFit

            Dim cellCounter As Integer = 2

            Dim rowCount As Integer = objAnalyzeStats.Count - 1

            For k = 0 To rowCount
                For i = 0 To objAnalyzeStats(k).TT.Count - 1
                    xlShtIN.Cells(cellCounter, 1) = objAnalyzeStats(k).objectType(i).ToString
                    xlShtIN.Cells(cellCounter, 2) = objAnalyzeStats(k).TT(i).ToString
                    xlShtIN.Cells(cellCounter, 3) = objAnalyzeStats(k).LocalSource(i).ToString
                    xlShtIN.Cells(cellCounter, 4) = objAnalyzeStats(k).LocalTranslation(i).ToString
                    xlShtIN.Cells(cellCounter, 5) = objAnalyzeStats(k).Lang(i).ToString
                    xlShtIN.Cells(cellCounter, 6) = objAnalyzeStats(k).DBSource(i).ToString
                    xlShtIN.Cells(cellCounter, 7) = objAnalyzeStats(k).DBTranslation(i).ToString
                    cellCounter += 1
                Next
                RaiseEvent UpdateProgress((k + 1 / rowCount) * 100)
            Next

            PerformFormattingAnalzye()

            CreatePivotTable(xlShtIN)
            xlApp.Visible = True
            Try
                ' xlShtIN.SaveAs(targetFileName)
            Catch ex As Exception

            End Try
            releaseObject(xlShtIN)
            '~~> Close the File
            ' xlWkb.Close()
            releaseObject(xlWkb)
            '~~> Quit the Excel Application
            ' xlApp.Quit()
            releaseObject(xlApp)
        Catch ex As Exception
            xlApp = Nothing
            Throw New Exception(ex.Message)
        Finally
            'xlApp = Nothing
        End Try
    End Sub

    Private Sub CreatePivotTable(ByVal targetSheet As Excel.Worksheet)

        Dim ptName As String = "MyPivotTable"

        Dim lastrow As Integer = targetSheet.Range("A66532").End(Excel.XlDirection.xlUp).Row

        'We'll assume the passed table name exists in the ActiveWorkbook
        targetSheet.PivotTableWizard(Excel.XlPivotTableSourceType.xlDatabase,
        "Sheet1!R1C1:R" & lastrow & "C7", targetSheet.Range("J2"))
        targetSheet.Select()

        Dim pt As Excel.PivotTable = targetSheet.PivotTables(1)

        'To be professional or merely resuable, the name could be passed as parameter
        With pt.PivotFields("Object Type")
            .Orientation = Excel.XlPivotFieldOrientation.xlRowField
            .Position = 1
        End With
        With pt.PivotFields("TranslationType")
            .Orientation = Excel.XlPivotFieldOrientation.xlColumnField
            .Position = 1
        End With
        With pt.PivotFields("Lang")
            .Orientation = Excel.XlPivotFieldOrientation.xlRowField
            .Position = 2
        End With

        pt.AddDataField(pt.PivotFields("Local Translation"), "Analyze Report",
        Excel.XlConsolidationFunction.xlCount)

        targetSheet.Columns("J:J").EntireColumn.AutoFit

        'pt.AddDataField(pt.PivotFields("Order Total"), "Total for Date",
        'Excel.XlConsolidationFunction.xlSum)

        '--OR--
        'AddPivotFields(pt, "Order Total", "Order Count", _
        '   Excel.XlConsolidationFunction.xlCount)
        'AddPivotFields(pt, "Order Total", "Total For Date", _
        '   Excel.XlConsolidationFunction.xlSum)

    End Sub

    Private Sub PerformFormattingAnalzye()


        With xlShtIN.Range("A1:G1").Interior
            .Pattern = 1
            .PatternColorIndex = -4105
            .Color = Color.Blue
            .TintAndShade = 0
            .PatternTintAndShade = 0
        End With

        Dim lastrow As Integer = xlShtIN.Range("A66532").End(Excel.XlDirection.xlUp).Row
        ' lastrow = 1000

        xlShtIN.Range("A1:G" & lastrow).Borders(5).LineStyle = -4142
        xlShtIN.Range("A1:G" & lastrow).Borders(6).LineStyle = -4142

        With xlShtIN.Range("A1:G" & lastrow).Borders(7)
            .LineStyle = 1
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = 2
        End With

        With xlShtIN.Range("A1:G" & lastrow).Borders(8)
            .LineStyle = 1
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = 2
        End With
        With xlShtIN.Range("A1:G" & lastrow).Borders(9)
            .LineStyle = 1
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = 2
        End With
        With xlShtIN.Range("A1:G" & lastrow).Borders(10)
            .LineStyle = 1
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = 2
        End With
        With xlShtIN.Range("A1:G" & lastrow).Borders(11)
            .LineStyle = 1
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = 2
        End With
        With xlShtIN.Range("A1:G" & lastrow).Borders(12)
            .LineStyle = 1
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = 2
        End With

        With xlShtIN.Range("A1:G" & xlShtIN.Range("D66536").End(Excel.XlDirection.xlUp).Row).Font
            .Color = Color.Black
            .TintAndShade = 0
        End With

        xlShtIN.Range("A1:G1").Font.Color = Color.Yellow

        xlShtIN.Columns("A:B").EntireColumn.AutoFit

        With xlShtIN.Columns("C:D")
            .HorizontalAlignment = 1
            .VerticalAlignment = -4107
            .WrapText = True
            .Orientation = 0
            .AddIndent = False
            .IndentLevel = 0
            .ShrinkToFit = False
            .ReadingOrder = -5002
            .MergeCells = False
        End With

        With xlShtIN.Columns("F:G")
            .HorizontalAlignment = 1
            .VerticalAlignment = -4107
            .WrapText = True
            .Orientation = 0
            .AddIndent = False
            .IndentLevel = 0
            .ShrinkToFit = False
            .ReadingOrder = -5002
            .MergeCells = False
        End With

    End Sub

#End Region

End Class


Public Interface ReportAnalyze
    Sub PerformFormattingAnalzye()
End Interface