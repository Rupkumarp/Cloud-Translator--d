Imports Microsoft.VisualBasic.FileIO
Imports System.IO
Imports System.Text.RegularExpressions

Module Mod_Competency

#Region "Competency CSV to XLIFF"
    Public Function ToXliff(ByVal Inputfile As String, ByVal OutPutFolder As String, ByVal Lang As String) As String
        Try
            '1. Get csv data to Datatable
            Dim csvdata As DataTable = Get_Competency_CsvtoDatatable(Inputfile)

            '2. Generate xliff out
            Dim OutFile As String = OutPutFolder & Path.GetFileNameWithoutExtension(Inputfile) & "_" & Lang & ".xliff"

            Return CreateCompetencyXliff(csvdata, OutFile, Lang)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function


    Private Function CreateCompetencyXliff(ByVal DT As DataTable, ByVal xliff_Path As String, ByVal Targetlanguage As String) As String
        Try
            Targetlanguage = Replace(Targetlanguage, "_", "-")
            Dim TR As String = ""
            Dim cnt As Integer = 0
            Using Writer As StreamWriter = New StreamWriter(xliff_Path, False, System.Text.Encoding.UTF8)
                Writer.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                Writer.WriteLine("<xliff version=""1.2"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ")
                Writer.WriteLine("xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2- &#xD;&#xA;strict.xsd"">")
                Writer.WriteLine("<file original=""C:\Temp\en_fr_2.xliff"" source-language=""en-US"" target-language=" & Chr(34) & Targetlanguage & Chr(34) & " datatype=""plaintext"" date=" & Chr(34) & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & Chr(34) & " tool-id=""SAP SFSF CONVERTER"" category=""MLT"">")
                Writer.WriteLine("<header>")
                Writer.WriteLine("<phase-group>")
                Writer.WriteLine("<phase phase-name=""Translation"" process-name=""999999"" company-name=""SAP"">")
                Writer.WriteLine("</phase>")
                Writer.WriteLine("</phase-group>")
                Writer.WriteLine("<tool tool-id=""SAP_SF_CONV""  tool-name=""SSC"">")
                Writer.WriteLine("</tool>")
                Writer.WriteLine("<note>TEST</note>")
                Writer.WriteLine("</header>")
                Writer.WriteLine("<body>")

                Writer.WriteLine(vbCrLf)
                Dim myNum As Integer = 1
                For J As Integer = 0 To DT.Rows.Count - 1

                    Dim HeaderName As String = Trim(DT.Rows(J).Item(0).ToString.ToLower)

                    Select Case HeaderName
                        Case "competency", "behavior"
                            cnt = cnt + 1

                            Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(HeaderName) & "_Col1_" & "Row" & J & Chr(34) & " resname=" & Chr(34) & clean_xml(DT.Rows(J).Item(2).ToString) & Chr(34) & ">")
                            Writer.WriteLine("<source>" & wrap_html(clean_xml(DT.Rows(J).Item(1).ToString)) & "</source>")
                            Writer.WriteLine("<target state=""needs-review-translation"">" & TR & "</target>")
                            Writer.WriteLine("<note from=""Developer"" priority =""10"">" & "Competency library : " & clean_xml(DT.Rows(J).Item(2).ToString) & "</note>")
                            Writer.WriteLine("</trans-unit>")
                            Writer.WriteLine(vbCrLf)

                            myNum += 1

                            Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(HeaderName) & "_Col2_" & "Row" & J & Chr(34) & " resname=" & Chr(34) & clean_xml(DT.Rows(J).Item(2).ToString) & Chr(34) & ">")
                            Writer.WriteLine("<source>" & wrap_html(clean_xml(DT.Rows(J).Item(2).ToString)) & "</source>")
                            Writer.WriteLine("<target state=""needs-review-translation"">" & TR & "</target>")
                            Writer.WriteLine("<note from=""Developer"" priority =""10"">" & "Competency library : " & clean_xml(DT.Rows(J).Item(2).ToString) & "</note>")
                            Writer.WriteLine("</trans-unit>")
                            Writer.WriteLine(vbCrLf)

                            myNum += 1

                            Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(HeaderName) & "_Col3_" & "Row" & J & Chr(34) & " resname=" & Chr(34) & clean_xml(DT.Rows(J).Item(2).ToString) & Chr(34) & ">")
                            Writer.WriteLine("<source>" & wrap_html(clean_xml(DT.Rows(J).Item(3).ToString)) & "</source>")
                            Writer.WriteLine("<target state=""needs-review-translation"">" & TR & "</target>")
                            Writer.WriteLine("<note from=""Developer"" priority =""10"">" & "Competency library : " & clean_xml(DT.Rows(J).Item(2).ToString) & "</note>")
                            Writer.WriteLine("</trans-unit>")
                            Writer.WriteLine(vbCrLf)

                            myNum += 1

                        Case "teaser"
                            For i As Integer = 2 To DT.Columns.Count - 1
                                If DT.Rows(J).Item(i).ToString <> "" Then
                                    cnt = cnt + 1
                                    myNum += 1
                                    Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(HeaderName) & "_Col" & i & "_Row" & J & Chr(34) & " resname=" & Chr(34) & clean_xml(DT.Rows(J).Item(2).ToString) & Chr(34) & ">")
                                    Writer.WriteLine("<source>" & wrap_html(clean_xml(DT.Rows(J).Item(i).ToString)) & "</source>")
                                    Writer.WriteLine("<target state=""needs-review-translation"">" & TR & "</target>")
                                    Writer.WriteLine("<note from=""Developer"" priority =""10"">" & "Competency library : " & clean_xml(DT.Rows(J).Item(2).ToString) & "</note>")
                                    Writer.WriteLine("</trans-unit>")
                                    Writer.WriteLine(vbCrLf)
                                End If
                            Next
                    End Select

                Next

                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

            End Using

            If cnt = 0 Then
                System.IO.File.Delete(xliff_Path)
                Return " Already has translation for " & Replace(Targetlanguage, "-", "_")
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return ""
    End Function
#End Region

#Region "XLIFF to CSV"
    Public Sub ToCsv(ByVal OriginalFile As String, ByVal TranslatedxliffFile As String, ByVal Lang As String, ByRef Bw As System.ComponentModel.BackgroundWorker)
        Dim targetfilepath As String
        Try
            Dim sFileName As String = System.IO.Path.GetFileName(OriginalFile)

            'Monolingual

            targetfilepath = Replace(OriginalFile, "01-Input-B", "05-Output")
            targetfilepath = System.IO.Path.GetDirectoryName(targetfilepath) & "\Mono_" & Lang & "\"

            If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(targetfilepath)) <> True Then
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetfilepath))
            End If

            targetfilepath = targetfilepath & sFileName

            File.Copy(OriginalFile, targetfilepath, True)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Try
            '1. Get csv data to Datatable
            Dim DT As DataTable = Get_Competency_CsvtoDatatable(OriginalFile)

            '2. Load Translated xliff data          

            Dim objXliff As New sXliff

            Try
                objXliff = ModHelper.load_xliff(TranslatedxliffFile)
            Catch ex As Exception
                If ModHelper.UnWrapXliffBack(TranslatedxliffFile) <> True Then
                    Throw New Exception("Error UnWrapping xliff back!")
                End If
                objXliff = ModHelper.cvload_xliff(Application.StartupPath & "\Temp_UnWrap.xliff")
            End Try

            If objXliff.ID.Count = 0 Then
                UpdateMsg(Now & Chr(9) & "No translations found for " & System.IO.Path.GetFileName(TranslatedxliffFile) & vbCrLf, Form_MainNew.RtbColor.Red, Bw)
                'Throw New Exception("0 translations found in " & System.IO.Path.GetFileName(TranslatedxliffFile))
            End If

            '3. Collate both data
            Dim notFound As New ArrayList
            Dim bFound As Boolean
            Dim counter As Integer

            For i As Integer = 0 To DT.Rows.Count - 1
                bFound = False
                Dim HeaderName As String = Trim(DT.Rows(i).Item(0).ToString.ToLower)
                Dim clrDTSource As String
                Select Case HeaderName
                    Case "competency", "behavior"

                        'Column 1
                        clrDTSource = HTMLToText(LCase(DT.Rows(i).Item(1).ToString))
                        bFound = CompareSource(DT, i, 1, clrDTSource, objXliff)

                        If bFound <> True Then
                            'Add the items which missed to update translation
                            If bFound <> True Then
                                counter += 1
                                notFound.Add(counter & ". Source value -> " & DT.Rows(i).Item(2).ToString)
                            End If
                        End If

                        'Column 2
                        clrDTSource = HTMLToText(LCase(DT.Rows(i).Item(2).ToString))
                        bFound = CompareSource(DT, i, 2, clrDTSource, objXliff)

                        If bFound <> True Then
                            'Add the items which missed to update translation
                            If bFound <> True Then
                                counter += 1
                                notFound.Add(counter & ". Source value -> " & DT.Rows(i).Item(2).ToString)
                            End If
                        End If

                        'Column 3
                        clrDTSource = HTMLToText(LCase(DT.Rows(i).Item(3).ToString))
                        bFound = CompareSource(DT, i, 3, clrDTSource, objXliff)

                        If bFound <> True Then
                            'Add the items which missed to update translation
                            If bFound <> True Then
                                counter += 1
                                notFound.Add(counter & ". Source value -> " & DT.Rows(i).Item(3).ToString)
                            End If
                        End If

                    Case "teaser"
                        For j As Integer = 2 To DT.Columns.Count - 1
                            If DT.Rows(i).Item(j).ToString <> "" Then
                                clrDTSource = clean_xml(DT.Rows(i).Item(j).ToString)
                                bFound = CompareSource(DT, i, j, clrDTSource, objXliff)
                                If bFound <> True Then
                                    'Add the items which missed to update translation
                                    If bFound <> True Then
                                        counter += 1
                                        notFound.Add(counter & ". Source value -> " & DT.Rows(i).Item(j).ToString)
                                    End If
                                End If
                            End If
                        Next
                End Select
            Next

            'Build back csv
            'define target path

            'Dim targetfilepath As String
            'If InStr(OriginalFile, "01-Input-B") <> 0 Then
            '    'case 1 first language
            '    targetfilepath = Replace(OriginalFile, "01-Input-B", "05-Output")

            'ElseIf InStr(OriginalFile, "05-Output") <> 0 Then
            '    'case 2 subsequent languages
            '    targetfilepath = OriginalFile

            'Else
            '    'case 3, project structure not used. Manual operations. Just add _out.
            '    targetfilepath = Path.GetDirectoryName(OriginalFile) & "\" & Path.GetFileNameWithoutExtension(OriginalFile) & ".out" & Path.GetExtension(OriginalFile)

            'End If

            Using writer As StreamWriter = New StreamWriter(targetfilepath, False, System.Text.Encoding.UTF8)
                WriteDataTableToCSV_Competency(DT, writer, Lang)
            End Using


            'NO Translation found then show a msg box and log it as well.
            If notFound.Count > 0 Then
                Dim objMissingTransaltion As New MissedTranslations
                objMissingTransaltion.UpdateMsg(notFound, OriginalFile, Lang)
            End If


        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Function CompareSource(ByRef DT As DataTable, ByVal rowIndex As Integer, ByVal colIndex As Integer, ByVal clrDTSource As String, ByVal objxliff As sXliff) As Boolean
        For i As Integer = 0 To objxliff.ID.Count - 1
            Dim clrXliffSource As String = GetPlainText(LCase(objxliff.Source(i)))
            clrDTSource = GetPlainText(clrDTSource)
            If Not IsDBNull(DT.Rows(rowIndex).Item(colIndex)) Then
                If (clrDTSource = clrXliffSource) Or (Trim(LCase(objxliff.Source(i))) = Trim(LCase(DT.Rows(rowIndex).Item(colIndex)))) Then
                    DT.Rows(rowIndex).Item(colIndex) = objxliff.Translation(i)
                    Return True
                End If
            End If
        Next

        Return False

    End Function

    Private Sub WriteDataTableToCSV_Competency(ByVal sourceTable As DataTable, ByVal writer As TextWriter, ByVal lang As String)
        Try
            Dim rowValues As List(Of String)
            For j As Integer = 0 To sourceTable.Rows.Count - 1
                rowValues = New List(Of String)()
                For i As Integer = 0 To sourceTable.Columns.Count - 1
                    If LangFound_Competency(sourceTable.Rows(j).Item(i).ToString) Then
                        rowValues.Add(QuoteValue(lang))
                    Else
                        rowValues.Add(QuoteValue(sourceTable.Rows(j).Item(i).ToString))
                    End If

                Next
                writer.WriteLine(String.Join(",", rowValues))
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            writer.Flush()
        End Try

    End Sub

    Private Function LangFound_Competency(ByVal str As String) As Boolean
        Select Case LCase(str)
            Case "en_us", "fr_fr", "de_de", "es_es", "ja_jp", "ko_kr", "zh_cn", "ru_ru", "pt_br", "it_it"
                Return True
            Case Else
                Return False
        End Select
    End Function

#End Region

    'Helper Function
    Private Function Get_Competency_CsvtoDatatable(ByVal sCsvFilePath As String) As DataTable

        Dim csvData As New DataTable()
        Try
            'First Get the Max last column used in csv file, this is used to set the column limit for datatable------------------------------------------
            Dim LastCol As Integer = 0
            Using csvReader As New TextFieldParser(sCsvFilePath)
                csvReader.SetDelimiters(New String() {","})
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
        End Try

        Return csvData

    End Function

End Module
