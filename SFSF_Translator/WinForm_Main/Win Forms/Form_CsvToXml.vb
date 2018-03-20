Imports Microsoft.VisualBasic.FileIO
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form_CsvToXml

    Private Sub BtnBrowse_Click(sender As Object, e As EventArgs) Handles BtnBrowse.Click
        Dim OpenFileDialog1 As New OpenFileDialog
        OpenFileDialog1.Filter = ".csv  files (*.csv)|*.csv;"
        OpenFileDialog1.Title = "Select .csv file."
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox2.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub BtnGenerateXml_Click(sender As Object, e As EventArgs) Handles BtnGenerateXml.Click
        If TextBox2.Text.Trim = "" Then
            MsgBox("No csv file selected", MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        End If

        If Not System.IO.File.Exists(TextBox2.Text) Then
            MsgBox("File not found!" & vbCrLf & TextBox2.Text, MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        End If

        Try
            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor
            CsvToXML(TextBox2.Text, TextBox1.Text)
            MsgBox("Successfully creating xml Completed", MsgBoxStyle.Information, "Cloud translator")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Cloud translator")
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub CsvToXML(ByVal csvFile As String, ByVal Param As String)
        Try
            Dim csvData As New DataTable()

            csvData = GetDataTabletFromCSVFile(csvFile)
            Dim directoryName As String

            Dim filename As String = Path.GetFileNameWithoutExtension(csvFile)

            directoryName = Path.GetDirectoryName(csvFile) + "\CsvToXml"
            filename = directoryName + "\" + filename + ".xml"
            If Not Directory.Exists(directoryName) Then
                Directory.CreateDirectory(directoryName)
            End If

            Using Writer As StreamWriter = New StreamWriter(filename, False, System.Text.Encoding.UTF8)
                Writer.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                Writer.WriteLine("<!DOCTYPE competency-lib SYSTEM ""competency-lib.dtd"">")
                Writer.WriteLine("<competency-lib>")
                Writer.WriteLine("<competency-source>")
                Writer.WriteLine("<![CDATA[" & Param & "]]>")
                Writer.WriteLine("</competency-source>")
                Dim count1 As Integer = 0
                Dim count2 As Integer = 0
                Try
                    ToolStripProgressBar1.ToolTipText = "Status: Writing Competency"
                    ToolStripProgressBar1.Maximum = csvData.Rows.Count - 2
                    For i As Integer = 0 To csvData.Rows.Count - 2

                        Dim data1 = csvData.Rows(i)(0).ToString()
                        Dim test As String = csvData.Rows(i + 1)(0).ToString()
                        If (csvData.Rows(i)(0).ToString() = "COMPETENCY") Or (csvData.Rows(i)(0).ToString() = """COMPETENCY""") Then
                            Dim locale As String = clean_xml(csvData.Rows(i)(5).ToString())
                            Dim guid As String = clean_xml(csvData.Rows(i)(4).ToString())
                            Writer.WriteLine("<competency guid=""" & guid & """ locale=""" & locale & """>")
                            Writer.WriteLine("<competency-name>")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(2).ToString()) & "]]>")
                            Writer.WriteLine("</competency-name>")
                            Writer.WriteLine("<competency-desc>")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(3).ToString()) & "]]>")
                            Writer.WriteLine("</competency-desc>")
                            Writer.WriteLine("<category>")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(1).ToString()) & "]]>")
                            Writer.WriteLine("</category>")

                        ElseIf (csvData.Rows(i)(0).ToString() = "TEASER") Or (csvData.Rows(i)(0).ToString() = """TEASER""") Then
                            Dim mains As String = csvData.Rows(i)(6).ToString()
                            mains = clean_xml(mains)
                            Dim First As String = GetBetween(mains, "[-FIRST-]", "[-SECOND-]")
                            Dim Second As String = GetBetween(mains, "[-SECOND-]", "[-THIRD-]")
                            Dim third As String = GetBetween(mains, "[-THIRD-]", "")
                            Dim n As Integer = Int32.Parse(Regex.Replace(csvData.Rows(i)(5).ToString(), "\D", ""))
                            Writer.WriteLine("<subtopic>")
                            Writer.WriteLine("<subtopic-name>")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(3).ToString()) & "]]>")
                            Writer.WriteLine("</subtopic-name>")
                            Writer.WriteLine("<subtopic-type>")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(4).ToString()) & "]]>")
                            Writer.WriteLine("</subtopic-type>")
                            Writer.WriteLine("<subtopic-writing-asst>")
                            Writer.WriteLine("<writing-tone>" & n & "</writing-tone>")
                            Writer.WriteLine("<content person=""1"">")
                            Writer.WriteLine("<![CDATA[" & First & "]]>")
                            Writer.WriteLine("</content>")
                            Writer.WriteLine("<content person=""2"">")
                            Writer.WriteLine("<![CDATA[" & Second & " ]]>")
                            Writer.WriteLine("</content>")
                            Writer.WriteLine("<content person=""3"">")
                            Writer.WriteLine("<![CDATA[" & third & "]]>")
                            Writer.WriteLine("</content>")
                            Writer.WriteLine("</subtopic-writing-asst>")
                            Writer.WriteLine("<subtopic-advisor>")
                            Writer.WriteLine("<![CDATA[" & count1 & "]]>")
                            Writer.WriteLine("</subtopic-advisor>")
                            Writer.WriteLine("</subtopic>")
                            count1 = count1 + 1
                        End If
                        If (test = "COMPETENCY" Or test = """COMPETENCY""") Then
                            Writer.WriteLine("</competency>")
                        End If
                        ToolStripProgressBar1.Value = i
                    Next
                    Writer.WriteLine("</competency>")

                    ToolStripProgressBar1.ToolTipText = "Status: Writing Teasers"
                    ToolStripProgressBar1.Maximum = csvData.Rows.Count - 2
                    For i As Integer = 0 To csvData.Rows.Count - 2

                        Dim data1 = csvData.Rows(i)(0).ToString()

                        If (csvData.Rows(i)(0).ToString() = "TEASER") Or (csvData.Rows(i)(0).ToString() = """TEASER""") Then
                            Writer.WriteLine("<coaching-advisor>")
                            Writer.WriteLine("<coaching-advisor-name>")
                            Writer.WriteLine("<![CDATA[" & count2 & "]]>")
                            Writer.WriteLine("</coaching-advisor-name>")
                            Writer.WriteLine("<coaching-advisor-content>")
                            Writer.WriteLine("<advisor-text type=""1"">")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(8).ToString()) & "]]>")
                            Writer.WriteLine("</advisor-text>")
                            Writer.WriteLine("<advisor-text type=""0"">")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(9).ToString()) & "]]>")
                            Writer.WriteLine("</advisor-text>")
                            Writer.WriteLine("<advisor-text type=""0"">")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(11).ToString()) & "]]>")
                            Writer.WriteLine("</advisor-text>")
                            Writer.WriteLine("<advisor-text type=""0"">")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(13).ToString()) & "]]>")
                            Writer.WriteLine("</advisor-text>")
                            Writer.WriteLine("<advisor-text type=""0"">")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(15).ToString()) & "]]>")
                            Writer.WriteLine("</advisor-text>")
                            Writer.WriteLine("<advisor-text type=""0"">")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(17).ToString()) & "]]>")
                            Writer.WriteLine("</advisor-text>")
                            Writer.WriteLine("<advisor-text type=""0"">")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(19).ToString()) & "]]>")
                            Writer.WriteLine("</advisor-text>")
                            Writer.WriteLine("<advisor-text type=""0"">")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(21).ToString()) & "]]>")
                            Writer.WriteLine("</advisor-text>")
                            Writer.WriteLine("<advisor-text type=""0"">")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(23).ToString()) & "]]>")
                            Writer.WriteLine("</advisor-text>")
                            Writer.WriteLine("<advisor-text type=""0"">")
                            Writer.WriteLine("<![CDATA[" & clean_xml(csvData.Rows(i)(25).ToString()) & "]]>")
                            Writer.WriteLine("</advisor-text>")
                            Writer.WriteLine("</coaching-advisor-content>")
                            Writer.WriteLine("</coaching-advisor>")
                            count2 = count2 + 1
                        End If
                        ToolStripProgressBar1.Value = i
                    Next

                    Writer.WriteLine("</competency-lib>")
                Catch ex As Exception
                    Throw New Exception("Error creating xml" & vbNewLine & ex.Message)
                End Try

            End Using

            ToolStripStatusLabel1.Text = "File Saved: " & filename

            If MsgBox("Csv converted to Xml!" & vbCrLf & "Do you want to open the folder?", MsgBoxStyle.Information, "Cloud translator") = MsgBoxResult.Ok Then
                Diagnostics.Process.Start("explorer.exe", directoryName)
            End If

        Catch ex As Exception
            Throw New Exception("Error creating xml" & vbNewLine & ex.Message)
        End Try
    End Sub

    Private Function GetDataTabletFromCSVFile(sCsvFilePath As String) As DataTable

        ToolStripStatusLabel1.Text = "Reading csv file!"

        Dim csvData As New DataTable()
        Dim LastCol As Integer = 0
        Try
            Try
                'First Get the Max last column used in csv file, this is used to set the column limit for datatable------------------------------------------

                Using csvReader As New TextFieldParser(sCsvFilePath, System.Text.Encoding.UTF8)
                    csvReader.TextFieldType = FileIO.FieldType.Delimited
                    Try
                        csvReader.SetDelimiters(New String() {","})
                        csvReader.HasFieldsEnclosedInQuotes = True
                    Catch ex As Exception
                        Throw New Exception(ex.Message)
                    End Try

                    Try
                        Dim i As Integer = 0
                        While Not csvReader.EndOfData
                            Dim fieldData As String() = csvReader.ReadFields()
                            'Debug.Print(fieldData(0) & i)
                            i += 1
                            If LastCol < UBound(fieldData) Then
                                LastCol = UBound(fieldData)
                            End If
                        End While
                    Catch ex As Exception
                        Throw New Exception(ex.Message)
                    End Try

                End Using
                '---------------------------------------------------------------------------------------------------------------------------------------------
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try


            'Now load the csv data to datatable
            Using csvReader As New TextFieldParser(sCsvFilePath)
                csvReader.SetDelimiters(New String() {","})
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
            Throw New Exception("Error loading csv to datatable!" & vbNewLine & ex.Message)
            ToolStripStatusLabel1.Text = ex.Message
        End Try

        Return csvData

    End Function


    Private Function GetBetween(IStringStr As String, IBefore As String, IPast As String) As String
        Dim source As String = IStringStr
        Dim extract As String = ""
        Dim [end] As Integer
        Dim start As Integer = source.IndexOf(IBefore) + 1
        If IPast = "" Then
            [end] = source.LastIndexOf("[-THIRD-]")
        Else
            [end] = source.IndexOf(IPast)

        End If
        If start >= 0 AndAlso [end] > start Then
            extract = source.Substring(start, [end] - start)
        End If
        extract = Replace(extract, "[-FIRST-]", "")
        extract = Replace(extract, "-FIRST-]", "")
        extract = Replace(extract, "[-SECOND-]", "")
        extract = Replace(extract, "-SECOND-]", "")
        extract = Replace(extract, "-THIRD-]", "")
        extract = Replace(extract, "[-THIRD-]", "")

        Return extract
    End Function

 

End Class