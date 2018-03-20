Imports System.IO
Imports System.Xml.Serialization

Public Class Form_XLFtoExcel

    Private Sub btnBrowseInputFile_Click(sender As Object, e As EventArgs) Handles btnBrowseInputFile.Click
        Dim opnDialog As OpenFileDialog = New OpenFileDialog
        opnDialog.Filter = "XLF file *.XLF|*.xlf"
        If opnDialog.ShowDialog = DialogResult.OK Then
            txtInputFilePath.Text = opnDialog.FileName
        End If

        If rdXliffToXls.Checked Then
            Dim fileName As String = opnDialog.FileName
            txtOutputFilePath.Text = fileName.Replace("xlf", "xlsx")
        End If

    End Sub

    Private Sub btnBrowseOutputFile_Click(sender As Object, e As EventArgs) Handles btnBrowseOutputFile.Click

        If rdXlsToXliff.Checked Then
            Dim fDialog As OpenFileDialog = New OpenFileDialog
            fDialog.Filter = "Excel file *.XLS,*.XLSX|*.xls;*.xlsx"
            If fDialog.ShowDialog = DialogResult.OK Then
                txtOutputFilePath.Text = fDialog.FileName
            End If
        Else
            Dim savDialog As SaveFileDialog = New SaveFileDialog
            savDialog.Filter = "Excel file *.XLS,*.XLSX|*.xls;*.xlsx"
            If savDialog.ShowDialog = DialogResult.OK Then
                txtOutputFilePath.Text = savDialog.FileName
            End If
        End If

    End Sub

    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click
        Try
            If rdXliffToXls.Checked Then
                CreateExcelFromXlf()
            Else
                Dim objXlf As xlf = GetXlfData(txtInputFilePath.Text)
                'Translate xlf file now
                CreateXlfBack(objXlf)
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub



    Function GetXlfData(ByVal xlfFile As String) As xlf
        Dim objxlf As xlf = Nothing
        Dim str As String = File.ReadAllText(xlfFile)
        Dim serializer As New XmlSerializer(GetType(xlf))
        Using reader As TextReader = New StringReader(str)
            objxlf = serializer.Deserialize(reader)
        End Using
        Return objxlf
    End Function

    Sub CreateExcelFromXlf()
        Dim objXlf As xlf = GetXlfData(txtInputFilePath.Text)
        Dim objExcel As ClsExcel = New ClsExcel()
        objExcel.CreateReportXlfToExcel(txtOutputFilePath.Text, objXlf)
    End Sub

    Sub CreateXlfBack(ByVal objXlf As xlf)

        Dim objExcel As ClsExcel = New ClsExcel()
        objXlf = objExcel.TranslateXlf(objXlf, txtOutputFilePath.Text)

        Dim writer As StreamWriter = New StreamWriter("C:\Users\C5195092\Desktop\ogg\out.xlf")
        Dim xwriter As XmlSerializer = New XmlSerializer(GetType(xlf))
        xwriter.Serialize(writer, objXlf)
        writer.Close()
    End Sub

    Private Sub rdXliffToXls_CheckedChanged(sender As Object, e As EventArgs) Handles rdXliffToXls.CheckedChanged
        lblOutputFilePath.Text = "Output File Path:"
    End Sub

    Private Sub rdXlsToXliff_CheckedChanged(sender As Object, e As EventArgs) Handles rdXlsToXliff.CheckedChanged
        lblOutputFilePath.Text = "Translated File Path:"
    End Sub
End Class