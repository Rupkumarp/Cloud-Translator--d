Imports Excel = Microsoft.Office.Interop.Excel
Imports System.IO

Module ModExcel

    Public XlApp As New Excel.Application
    Public xlWkb As Excel.Workbook
    Public xlWksht As Excel.Worksheet

    Public Function GetExcelSheetNames(ByVal FileName As String) As ArrayList
        Try

            Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            XlApp = DirectCast(CreateObject("Excel.Application"), Excel.Application)

            xlWkb = XlApp.Workbooks.Open(FileName)

            Dim wksht As Excel.Worksheet
            Dim SheetNames As New ArrayList
            SheetNames.Clear()

            For Each wksht In xlWkb.Worksheets
                SheetNames.Add(wksht.Name)
            Next

            xlWkb.Close(False)
            XlApp.Quit()
            xlWkb = Nothing
            XlApp = Nothing

            Return SheetNames
        Catch ex As Exception
            Throw New Exception("Error getting worksheet names" & vbNewLine & ex.Message)
        End Try
        
    End Function

    Public Sub InitiateExcelAction(ByVal FileName As String)
        Try
            Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            XlApp = DirectCast(CreateObject("Excel.Application"), Excel.Application)

            xlWkb = XlApp.Workbooks.Open(FileName)
        Catch ex As Exception
            XlApp.Quit()
            XlApp = Nothing
            Throw New Exception("Error InitiateExcelAction" & FileName & vbNewLine & vbNewLine & ex.Message)
        End Try
        
    End Sub


    'Scrapped
    Public Sub GenerateExcelReport()
        Try

            Dim myXMLfile As String = ProjectManagement.GetActiveProject.ProjectPath & "ProjectStats.xml"

            If Not System.IO.File.Exists(myXMLfile) Then
                Exit Sub
            End If

            Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            XlApp = DirectCast(CreateObject("Excel.Application"), Excel.Application)

            xlWkb = XlApp.Workbooks.Add
            xlWksht = xlWkb.Worksheets.Add


            Dim DS As DataSet = New DataSet()
            ' Create new FileStream with which to read the schema.
            Dim fsReadXml As New System.IO.FileStream(myXMLfile, System.IO.FileMode.Open)
            Try
                DS.ReadXml(fsReadXml)
            Catch ex As Exception
                MessageBox.Show(ex.ToString())
            Finally
                fsReadXml.Close()
            End Try


            Dim dc As System.Data.DataColumn
            Dim dr As System.Data.DataRow
            Dim colIndex As Integer = 0
            Dim rowIndex As Integer = 0

            'Export the Columns to excel file
            For Each dc In DS.Tables("FileStats").Columns
                colIndex = colIndex + 1
                xlWksht.Cells(1, colIndex) = dc.ColumnName
            Next

            'Export the rows to excel file
            For Each dr In DS.Tables("FileStats").Rows
                rowIndex = rowIndex + 1
                colIndex = 0
                For Each dc In DS.Tables("FileStats").Columns
                    colIndex = colIndex + 1
                    xlWksht.Cells(rowIndex + 1, colIndex) = dr(dc.ColumnName)
                Next
            Next

            XlApp.Visible = True


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub




End Module
