Imports Microsoft.VisualBasic.FileIO
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.ComponentModel

Module Mod_PickList

    Public Enum PickListType
        ManagePicklist
        UnManagePicklist
    End Enum

#Region "Csv to XLiff"
    Public Function ToXliff(ByVal Inputfile As String, ByVal OutPutFolder As String, ByVal lang As String, ByVal objPickListyType As PickListType, Optional ByVal bIsMaxLengthRestricted As Boolean = False, Optional ByVal ExtractWithTranslationOnly As Boolean = False) As String
        Try
            '1. Get csv data to Datatable
            Dim DT As DataTable
            Dim objParser As New CsvParser
            DT = objParser.GetDataTabletFromCSVFile(Inputfile)

            '2. Generate xliff out
            Dim OutFile As String = OutPutFolder & "\" & Path.GetFileNameWithoutExtension(Inputfile) & "_" & lang & ".xliff"
            Dim TargetCol As Integer = 0
            For i As Integer = 0 To DT.Columns.Count - 1
                If Trim(LCase(DT.Columns(i).ColumnName)) = Trim(LCase(lang)) Then
                    TargetCol = i
                End If
            Next
            Return CreatePickListXliff(System.IO.Path.GetFileNameWithoutExtension(Inputfile), DT, OutFile, lang, TargetCol, objPickListyType, bIsMaxLengthRestricted, ExtractWithTranslationOnly)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function CreatePickListXliff(ByVal FileName As String, ByVal DT As DataTable, ByVal xliff_Path As String, ByVal TargetLanguage As String, ByVal TargetCol As Integer, ByVal objPickListyType As PickListType, ByVal bIsMaxLengthRestricted As Boolean, ByVal ExtractWithTranslationOnly As Boolean) As String

        Try
            TargetLanguage = Replace(TargetLanguage, "_", "-")
            Dim TR As String = ""
            Dim cnt As Integer = 0
            Using Writer As StreamWriter = New StreamWriter(xliff_Path, False, System.Text.Encoding.UTF8)
                Writer.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                Writer.WriteLine("<xliff version=""1.2"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ")
                Writer.WriteLine("xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2- &#xD;&#xA;strict.xsd"">")
                Writer.WriteLine("<file original=""C:\Temp\en_fr_2.xliff"" source-language=""en-US"" target-language=" & Chr(34) & TargetLanguage & Chr(34) & " datatype=""plaintext"" date=" & Chr(34) & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & Chr(34) & " tool-id=""SAP SFSF CONVERTER"" category=""MLT"">")
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
                Dim myNum As Integer = 0

                Dim MaxLengthTable As DataTable = ClsMaxLength.LoadMaxLength(FileName)

                For i As Integer = 0 To DT.Columns.Count - 1
                    If LCase(DT.Columns(i).ColumnName) = "en_us" Then
                        For J As Integer = 0 To DT.Rows.Count - 1

                            Dim HeaderName As String = Trim(DT.Rows(J).Item(0).ToString.ToLower)
                            If objPickListyType = PickListType.UnManagePicklist Then

                                If ExtractWithTranslationOnly Then
                                    ExtractWithTranslation(Writer, DT, J, i, TargetCol, myNum, cnt, HeaderName, bIsMaxLengthRestricted, MaxLengthTable)
                                Else
                                    ExtractWithoutTranslation(Writer, DT, J, i, TargetCol, myNum, cnt, HeaderName, bIsMaxLengthRestricted, MaxLengthTable)
                                End If

                            Else
                                If DT.Rows(J).Item(i).ToString <> "" Then
                                    cnt = cnt + 1
                                    myNum += 1
                                    Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(HeaderName) & "_Col" & i & "_Row" & J & Chr(34) & " resname=" & Chr(34) & HeaderName & Chr(34) & ">")
                                    Writer.WriteLine("<source>" & wrap_html(clean_xml(DT.Rows(J).Item(i).ToString)) & "</source>")

                                    If IsNumeric(DT.Rows(J).Item(i)) Then
                                        Writer.WriteLine("<target state=""needs-review-translation"">" & DT.Rows(J).Item(i) & "</target>")
                                    Else
                                        If DT.Rows(J).Item(TargetCol).ToString <> "" Then
                                            Writer.WriteLine("<target state=""needs-review-translation"">" & wrap_html(clean_xml(DT.Rows(J).Item(TargetCol))) & "</target>")
                                        Else
                                            Writer.WriteLine("<target state=""needs-review-translation""></target>")
                                        End If
                                    End If

                                    Writer.WriteLine("<note from=""Developer"" priority =""10"">" & "Picklists : " & clean_xml(DT.Rows(J).Item(1).ToString) & "</note>")
                                    Writer.WriteLine("</trans-unit>")
                                    Writer.WriteLine(vbCrLf)
                                End If
                            End If

                        Next
                    End If
                Next

                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

            End Using

            If cnt = 0 Then
                System.IO.File.Delete(xliff_Path)
                Return " Already has translation for " & Replace(TargetLanguage, "-=", "_")
            End If


        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return ""
    End Function

    Private Sub ExtractWithTranslation(ByRef Writer As StreamWriter, ByRef DT As DataTable, ByRef J As Integer, ByRef i As Integer, ByRef TargetCol As Integer, ByRef myNum As Integer, ByRef cnt As Integer, ByRef HeaderName As String, ByRef bIsMaxLengthRestricted As Boolean, ByRef MaxLengthTable As DataTable)
        If DT.Rows(J).Item(i).ToString <> String.Empty And DT.Rows(J).Item(TargetCol).ToString <> String.Empty Then
            cnt = cnt + 1
            myNum += 1

            Dim MaxLength As Integer = -1
            If bIsMaxLengthRestricted And MaxLengthTable.Rows.Count > 0 Then 'Support of length restriction
                Try
                    MaxLength = ClsMaxLength.GetMaxLength(MaxLengthTable, DT, i, J)
                    If MaxLength >= 0 Then
                        Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(HeaderName) & "_Col" & i & "_Row" & J & Chr(34) & " resname=" & Chr(34) & HeaderName & Chr(34) & " size-unit=" & Chr(34) & "char" & Chr(34) & " maxwidth=" & Chr(34) & MaxLength & Chr(34) & ">")
                    End If
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            End If
            If MaxLength = -1 Then
                Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(HeaderName) & "_Col" & i & "_Row" & J & Chr(34) & " resname=" & Chr(34) & HeaderName & Chr(34) & ">")
            End If

            Writer.WriteLine("<source>" & wrap_html(clean_xml(DT.Rows(J).Item(i).ToString)) & "</source>")

            If IsNumeric(DT.Rows(J).Item(i)) Then
                Writer.WriteLine("<target state=""needs-review-translation"">" & DT.Rows(J).Item(i) & "</target>")
            Else
                Writer.WriteLine("<target state=""needs-review-translation"">" & wrap_html(clean_xml(DT.Rows(J).Item(TargetCol).ToString)) & "</target>")
            End If

            Writer.WriteLine("<note from=""Developer"" priority =""10"">" & "Picklists : " & clean_xml(DT.Rows(J).Item(1).ToString) & "</note>")
            Writer.WriteLine("</trans-unit>")
            Writer.WriteLine(vbCrLf)
        End If
    End Sub

    Private Sub ExtractWithoutTranslation(ByRef Writer As StreamWriter, ByRef DT As DataTable, ByRef J As Integer, ByRef i As Integer, ByRef TargetCol As Integer, ByRef myNum As Integer, ByRef cnt As Integer, ByRef HeaderName As String, ByRef bIsMaxLengthRestricted As Boolean, ByRef MaxLengthTable As DataTable)
        If DT.Rows(J).Item(i).ToString <> String.Empty And DT.Rows(J).Item(TargetCol).ToString = String.Empty Then
            cnt = cnt + 1
            myNum += 1

            Dim MaxLength As Integer = -1
            If bIsMaxLengthRestricted And MaxLengthTable.Rows.Count > 0 Then 'Support of length restriction
                Try
                    MaxLength = ClsMaxLength.GetMaxLength(MaxLengthTable, DT, i, J)
                    If MaxLength >= 0 Then
                        Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(HeaderName) & "_Col" & i & "_Row" & J & Chr(34) & " resname=" & Chr(34) & HeaderName & Chr(34) & " size-unit=" & Chr(34) & "char" & Chr(34) & " maxwidth=" & Chr(34) & MaxLength & Chr(34) & ">")
                    End If
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            End If
            If MaxLength = -1 Then
                Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(HeaderName) & "_Col" & i & "_Row" & J & Chr(34) & " resname=" & Chr(34) & HeaderName & Chr(34) & ">")
            End If

            Writer.WriteLine("<source>" & wrap_html(clean_xml(DT.Rows(J).Item(i).ToString)) & "</source>")

            If IsNumeric(DT.Rows(J).Item(i)) Then
                Writer.WriteLine("<target state=""needs-review-translation"">" & DT.Rows(J).Item(i) & "</target>")
            Else
                Writer.WriteLine("<target state=""needs-review-translation""></target>")
            End If

            Writer.WriteLine("<note from=""Developer"" priority =""10"">" & "Picklists : " & clean_xml(DT.Rows(J).Item(1).ToString) & "</note>")
            Writer.WriteLine("</trans-unit>")
            Writer.WriteLine(vbCrLf)
        End If
    End Sub
#End Region

#Region "xliff to csv"

    Function ToCsv(ByVal originalfile_path As String, ByVal translated_xliff_path As String, ByVal Lang As String, ByVal objPickListType As PickListType, ByRef Bw As BackgroundWorker) As Boolean
        Try
            Dim targetfilepath As String = ""
            '1. Get csv data to Datatable
            If objPickListType = PickListType.ManagePicklist Then
                If System.IO.File.Exists(Replace(System.IO.Path.GetDirectoryName(translated_xliff_path), "02-Extracted_xliff", "04-Output_picklist\") & Path.GetFileNameWithoutExtension(originalfile_path) & ".out.csv") Then
                    originalfile_path = Replace(System.IO.Path.GetDirectoryName(translated_xliff_path), "02-Extracted_xliff", "04-Output_picklist\") & Path.GetFileNameWithoutExtension(originalfile_path) & ".out.csv"
                    targetfilepath = originalfile_path
                End If
            End If

            Dim objParser As New CsvParser
            Dim Dt As DataTable
            Dt = objParser.GetDataTabletFromCSVFile(originalfile_path)

            '2. Load Translated xliff data

            Dim objXliff As New sXliff
            Try
                objXliff = ModHelper.load_xliff(translated_xliff_path)
            Catch ex As Exception
                If ModHelper.UnWrapXliffBack(translated_xliff_path) <> True Then
                    Throw New Exception("Error UnWrapping xliff back!")
                End If
                objXliff = ModHelper.cvload_xliff(Application.StartupPath & "\Temp_UnWrap.xliff")
            End Try

            If objXliff.ID.Count = 0 Then
                UpdateMsg(Now & Chr(9) & "No translations found for " & System.IO.Path.GetFileName(translated_xliff_path) & vbCrLf, Form_MainNew.RtbColor.Red, Bw)
                'Throw New Exception("0 translations found in " & System.IO.Path.GetFileName(translated_xliff_path))
            End If

            Dim TargetColHeader As String
            Dim TargetColumnNumber As Integer
            Dim ColEnUs As Integer

            TargetColHeader = Lang
            TargetColumnNumber = Mod_CsvToXliff.GetColNumber(Dt, TargetColHeader)
            ColEnUs = Mod_CsvToXliff.GetColNumber(Dt, "en_us")

            Dim notFound As New ArrayList
            Dim bFound As Boolean

            For j As Integer = 0 To Dt.Rows.Count - 1
                bFound = False
                If IsDBNull(Dt.Rows(j).Item(TargetColumnNumber)) Then
                    Debug.Print(j)
                    For i As Integer = 0 To objXliff.ID.Count - 1 'Insert Translated content
                        Dim clrDTSource As String = GetPlainText(LCase(Dt.Rows(j).Item(ColEnUs)))
                        Dim clrXliffSource As String = GetPlainText(LCase(objXliff.Source(i)))

                        If Dt.Rows(j).Item(ColEnUs) = objXliff.Source(i) Or clrDTSource = clrXliffSource Then 'And Trim(LCase(Dt.Rows(j).Item(0))) = Trim(LCase(objXliff.Resname(i))) Then
                            Dt.Rows(j).Item(TargetColumnNumber) = objXliff.Translation(i)
                            bFound = True
                            Exit For
                        End If
                    Next
                    'Add the items which missed to update translation
                    If bFound <> True Then
                        notFound.Add(j + 1 & ". Source value -> " & Dt.Rows(j).Item(ColEnUs))
                    End If
                End If
            Next

            'Build back csv
            'define target path

            If objPickListType = PickListType.UnManagePicklist Then
                If InStr(originalfile_path, "01-Input-B") <> 0 Then
                    'case 1 first language
                    targetfilepath = Replace(originalfile_path, "01-Input-B", "05-Output")

                ElseIf InStr(originalfile_path, "05-Output") <> 0 Then
                    'case 2 subsequent languages
                    targetfilepath = originalfile_path

                Else
                    'case 3, project structure not used. Manual operations. Just add _out.
                    targetfilepath = Path.GetDirectoryName(originalfile_path) & "\" & Path.GetFileNameWithoutExtension(originalfile_path) & ".out" & Path.GetExtension(originalfile_path)

                End If
            Else
                'picklist
                If System.IO.Directory.Exists(Replace(System.IO.Path.GetDirectoryName(translated_xliff_path), "02-Extracted_xliff", "04-Output_picklist\")) <> True Then
                    System.IO.Directory.CreateDirectory(Replace(System.IO.Path.GetDirectoryName(translated_xliff_path), "02-Extracted_xliff", "04-Output_picklist\"))
                End If
                If System.IO.File.Exists(targetfilepath) <> True Then
                    targetfilepath = Replace(System.IO.Path.GetDirectoryName(translated_xliff_path), "02-Extracted_xliff", "04-Output_picklist\") & Path.GetFileNameWithoutExtension(originalfile_path) & ".out" & Path.GetExtension(originalfile_path)
                End If
            End If

            Using writer As StreamWriter = New StreamWriter(targetfilepath, False, System.Text.Encoding.UTF8)
                WriteDataTable(Dt, writer)
            End Using

            'NO Translation found then show a msg box and log it as well.
            If notFound.Count > 0 Then
                Dim objMissingTransaltion As New MissedTranslations
                objMissingTransaltion.UpdateMsg(notFound, originalfile_path, Lang)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return True
    End Function
#End Region


End Module
