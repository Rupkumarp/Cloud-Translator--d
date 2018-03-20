Imports Microsoft.VisualBasic.FileIO
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.ComponentModel

Module Mod_QuestionLib

    'Simple format, 1 line per question
    'Column 2 & 4 to be translated. Column 3,5,7 to be translated if not empty.
    'Language code in column 11, but 1 csv file per language

#Region "Csv to XLiff"
    Public Function ToXliff(ByVal Inputfile As String, ByVal OutPutFolder As String, ByVal lang As String, Optional ByVal bIsRestrictMaxlength As Boolean = False) As String
        Try
            '1. Get csv data to Datatable
            Dim csvdata As DataTable = Get_Questionlib_CsvtoDatatable(Inputfile)

            '2. Generate xliff out
            Dim OutFile As String = OutPutFolder & "\" & Path.GetFileNameWithoutExtension(Inputfile) & "_" & lang & ".xliff"
            Return CreateQuestionLibXliff(System.IO.Path.GetFileNameWithoutExtension(Inputfile), csvdata, OutFile, lang, bIsRestrictMaxlength)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Private Function CreateQuestionLibXliff(ByVal FileName As String, ByVal DT As DataTable, ByVal xliff_SavePath As String, ByVal Targetlanguage As String, ByVal bIsRestrictMaxLength As Boolean) As String
        Try

            Targetlanguage = Replace(Targetlanguage, "_", "-")
            Dim SourceLang As String = DT.Rows(0).Item(10)
            SourceLang = Replace(SourceLang, "_", "-")
            If SourceLang = "" Then
                SourceLang = "en-US"
            End If
            Dim TR As String = ""
            Dim cnt As Integer = 0
            Using Writer As StreamWriter = New StreamWriter(xliff_SavePath, False, System.Text.Encoding.UTF8)
                Writer.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                Writer.WriteLine("<xliff version=""1.2"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ")
                Writer.WriteLine("xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2- &#xD;&#xA;strict.xsd"">")
                Writer.WriteLine("<file original=""C:\Temp\en_fr_2.xliff"" source-language=" & Chr(34) & SourceLang & Chr(34) & " target-language=" & Chr(34) & Targetlanguage & Chr(34) & " datatype=""plaintext"" date=" & Chr(34) & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & Chr(34) & " tool-id=""SAP SFSF CONVERTER"" category=""MLT"">")
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

                Dim MaxLengthTable As DataTable = ClsMaxLength.LoadMaxLength(FileName)

                Dim myNum As Integer = 1

                Dim ColNumber As New ArrayList
                ColNumber.Add(1)
                ColNumber.Add(2)
                ColNumber.Add(3)
                ColNumber.Add(4)
                ColNumber.Add(6)
                ColNumber.Add(8)

                For J As Integer = 0 To DT.Rows.Count - 1

                    Dim HeaderName As String = Trim(DT.Rows(J).Item(0).ToString)

                    For i As Integer = 0 To ColNumber.Count - 1
                        If Not IsDBNull(DT.Rows(J).Item(ColNumber(i))) Then
                            If DT.Rows(J).Item(ColNumber(i)).ToString <> "" Then
                                Dim MaxLength As Integer = -1
                                If bIsRestrictMaxLength And MaxLengthTable.Rows.Count > 0 Then 'Support of length restriction
                                    Try
                                        MaxLength = ClsMaxLength.GetMaxLength(MaxLengthTable, DT, i, J)
                                        If MaxLength >= 0 Then
                                            Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(HeaderName) & "_Col" & i & "_Row" & J & Chr(34) & " resname=" & Chr(34) & clean_xml(HeaderName) & Chr(34) & Chr(34) & " size-unit=" & Chr(34) & "char" & Chr(34) & " maxwidth=" & Chr(34) & MaxLength & Chr(34) & ">")
                                        End If
                                    Catch ex As Exception
                                        'For testing, later remove it
                                        MsgBox(ex.Message)
                                    End Try
                                End If
                                If MaxLength = -1 Then
                                    Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(HeaderName) & "_Col" & i & "_Row" & J & Chr(34) & " resname=" & Chr(34) & clean_xml(HeaderName) & Chr(34) & ">")
                                End If

                                ' Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(HeaderName) & "_Col" & ColNumber(i) & "_Row" & J & Chr(34) & " resname=" & Chr(34) & "Question_Library" & Chr(34) & ">")
                                Writer.WriteLine("<source>" & wrap_html(clean_xml(DT.Rows(J).Item(ColNumber(i)).ToString)) & "</source>")
                                Writer.WriteLine("<target state=""needs-review-translation"">" & TR & "</target>")
                                Writer.WriteLine("<note from=""Developer"" priority =""10"">" & "Question_Library : " & clean_xml(HeaderName) & "</note>")
                                Writer.WriteLine("</trans-unit>")
                                Writer.WriteLine(vbCrLf)
                                cnt = cnt + 1
                                myNum += 1
                            End If
                        End If
                    Next
                Next

                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

            End Using

            If cnt = 0 Then
                System.IO.File.Delete(xliff_SavePath)
                Return " Already has translation for " & Replace(Targetlanguage, "-", "_")
            End If


        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return ""
    End Function
#End Region

#Region "Xliff to Csv"

    Public Function ToCsv(ByVal OriginalFile As String, ByVal TranslatedxliffFile As String, ByVal Lang As String, ByRef bw As BackgroundWorker) As Boolean
        Try
            '1. Get csv data to Datatable
            Dim DT As DataTable = Get_Questionlib_CsvtoDatatable(OriginalFile)

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

            '3. Collate both data           
            Dim ExternalCodeColumn As Integer = 0

            Dim notFound As New ArrayList
            Dim bFound As Boolean

            If objXliff.ID.Count = 0 Then
                UpdateMsg(Now & Chr(9) & "No translations found for " & System.IO.Path.GetFileName(TranslatedxliffFile) & vbCrLf, Form_MainNew.RtbColor.Red, bw)
                ' Throw New Exception("0 translations found in " & System.IO.Path.GetFileName(TranslatedxliffFile))
            End If

            Dim ColNumbers As New ArrayList
            Dim counter As Integer
            ColNumbers.Add(1)
            ColNumbers.Add(2)
            ColNumbers.Add(3)
            ColNumbers.Add(4)
            ColNumbers.Add(6)
            ColNumbers.Add(8)

            For J As Integer = 0 To DT.Rows.Count - 1

                Dim HeaderName As String = Trim(DT.Rows(J).Item(0).ToString)

                For i As Integer = 0 To ColNumbers.Count - 1
                    bFound = False
                    If Not IsDBNull(DT.Rows(J).Item(ColNumbers(i))) Then
                        If DT.Rows(J).Item(ColNumbers(i)) <> "" Then
                            If DT.Rows(J).Item(ColNumbers(i)).ToString.Trim <> "" Then
                                Dim clrDTSource As String = GetPlainText(LCase(DT.Rows(J).Item(ColNumbers(i)).ToString))
                                For x As Integer = 0 To objXliff.ID.Count - 1
                                    Dim clrXliffSource As String = GetPlainText(LCase(objXliff.Source(x)))
                                    If (DT.Rows(J).Item(ColNumbers(i)) = objXliff.Source(x)) Or (clrDTSource = clrXliffSource) Then
                                        DT.Rows(J).Item(ColNumbers(i)) = objXliff.Translation(x)
                                        bFound = True
                                        For k As Integer = 0 To DT.Columns.Count - 1
                                            If IsDBNull(DT.Rows(J).Item(k)) <> True Then
                                                If LangFound_QuestionLib(DT.Rows(J).Item(k)) Then
                                                    DT.Rows(J).Item(k) = Lang
                                                    Exit For
                                                End If
                                            End If
                                        Next
                                        Exit For
                                    End If
                                Next
                                'Add the items which missed to update translation
                                If bFound <> True Then
                                    counter += 1
                                    notFound.Add(counter & ". Source value -> " & DT.Rows(J).Item(ColNumbers(i)))
                                End If
                            End If
                        End If
                    End If
                Next
            Next

            'Build back csv
            'define target path
            Dim targetfilepath As String
            Dim sFileName As String = System.IO.Path.GetFileName(OriginalFile)

            'Monolingual
            targetfilepath = Replace(OriginalFile, "01-Input-B", "05-Output")
            targetfilepath = System.IO.Path.GetDirectoryName(targetfilepath) & "\Mono_" & Lang & "\"

            If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(targetfilepath)) <> True Then
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetfilepath))
            End If

            targetfilepath = targetfilepath & sFileName

            'Write csv file.
            Using writer As StreamWriter = New StreamWriter(targetfilepath, False, System.Text.Encoding.UTF8)
                WriteDataTableToCSV_QuestionLib(DT, writer, Lang)
            End Using


            'NO Translation found then show a msg box and log it as well.
            If notFound.Count > 0 Then
                Dim objMissingTransaltion As New MissedTranslations
                objMissingTransaltion.UpdateMsg(notFound, OriginalFile, Lang)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return True
    End Function

    Private Sub WriteDataTableToCSV_QuestionLib(ByVal sourceTable As DataTable, ByVal writer As TextWriter, ByVal lang As String)
        Try
            Dim rowValues As List(Of String)
            For j As Integer = 0 To sourceTable.Rows.Count - 1
                rowValues = New List(Of String)()
                For i As Integer = 0 To sourceTable.Columns.Count - 1
                    'If LangFound_QuestionLib(sourceTable.Rows(j).Item(i).ToString) Then
                    '    ' rowValues.Add(QuoteValue(lang))
                    'Else
                    rowValues.Add(QuoteValue(sourceTable.Rows(j).Item(i).ToString))
                    ' End If
                Next
                writer.WriteLine(String.Join(",", rowValues))
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            writer.Flush()
        End Try

    End Sub

#End Region

    Private Function LangFound_QuestionLib(ByVal str As String) As Boolean
        Select Case LCase(str)
            Case "en_us", "fr_fr", "de_de", "es_es", "ja_jp", "ko_kr", "zh_cn", "ru_ru", "pt_br", "it_it"
                Return True
            Case Else
                Return False
        End Select
    End Function

    Private Function Get_Questionlib_CsvtoDatatable(ByVal sCsvFilePath As String) As DataTable

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
