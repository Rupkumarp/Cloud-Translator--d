Imports Microsoft.VisualBasic.FileIO
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Text
Imports System.Windows.Forms.DataGridView

Public Class CsvParser

    Public Function GetDataTabletFromCSVFile(csv_file_path As String) As DataTable
        Dim csvData As New DataTable()
        Try
            Using csvReader As New TextFieldParser(csv_file_path)
                csvReader.SetDelimiters(New String() {","})
                csvReader.HasFieldsEnclosedInQuotes = True
                'read column names
                Dim colFields As String() = csvReader.ReadFields()
                For Each column As String In colFields
                    Dim datecolumn As New DataColumn(column)
                    datecolumn.AllowDBNull = True
                    csvData.Columns.Add(datecolumn)
                Next
                Dim datecolumn2 As New DataColumn("LastCol")
                csvData.Columns.Add(datecolumn2)
                While Not csvReader.EndOfData
                    Dim fieldData As String() = csvReader.ReadFields()
                    'Making empty value as null
                    For i As Integer = 0 To fieldData.Length - 1
                        If fieldData(i) = "" Then
                            fieldData(i) = Nothing
                        End If
                    Next
                    csvData.Rows.Add(fieldData)
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Return csvData
    End Function

  
End Class
