Imports Microsoft.VisualBasic.FileIO
Imports System.IO
Imports System.ComponentModel

Public Module HybrisProperties

#Region "Create Xliff"

    Public Function CreateXliff(ByVal EnFileName As String, ByVal TargetFileName As String, ByVal Targetlanguage As String, ByVal xliff_savePath As String) As String
        Try
            xliff_savePath = xliff_savePath & "\" & System.IO.Path.GetFileNameWithoutExtension(EnFileName) & "_" & Targetlanguage & ".xliff"
            Dim fName As String = System.IO.Path.GetFileNameWithoutExtension(EnFileName)

            Dim enDT As DataTable = Get_Properties_toDatatable(EnFileName, False)
            Dim TargetDT As DataTable = Nothing

            Dim bTargetFile As Boolean = False

            If TargetFileName = "" Then
                TargetFileName = EnFileName
            End If

            TargetDT = Get_Properties_toDatatable(TargetFileName, False)
            MakeEqual_en_and_target_table(enDT, TargetDT)

            Dim AllText As String = System.IO.File.ReadAllText(appData & DefinitionFiles.HybrisProperties_List)
            Dim HybrisDefinition As String() = Split(AllText, vbNewLine)

            Dim myNum As Integer = 1
            'Now Write Xliff
            Using Writer As StreamWriter = New StreamWriter(xliff_savePath, False, System.Text.Encoding.UTF8)
                Writer.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                Writer.WriteLine("<xliff version=""1.2"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ")
                Writer.WriteLine("xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2- &#xD;&#xA;strict.xsd"">")
                Writer.WriteLine("<file original=""C:\Temp\en_fr_2.xliff"" source-language=" & Chr(34) & "en-US" & Chr(34) & " target-language=" & Chr(34) & GetShort_lang(Targetlanguage) & Chr(34) & " datatype=""plaintext"" date=" & Chr(34) & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & Chr(34) & " tool-id=""SAP SFSF CONVERTER"" category=""MLT"">")
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
                Dim transunit As String = ""
                Dim source As String = ""
                If enDT.Columns.Count > 2 Then
                    'Multi Row
                    Dim enDT_New As DataTable = CreateNewDatatables(enDT)
                    Dim targetDT_New As DataTable = CreateNewDatatables(TargetDT)
                    myNum = WriteDataToXliff(enDT_New, targetDT_New, Writer, HybrisDefinition, fName)
                Else
                    'Single Row
                    For i As Integer = 0 To enDT.Rows.Count - 1
                        If Not IsDBNull(enDT.Rows(i).Item(0)) Then
                            If Left(enDT.Rows(i).Item(0), 1) <> "#" And enDT.Rows(i).Item(0) <> "" Then
                                Dim InputfileDef As String
                                Dim iStart As Integer = InStrRev(enDT.Rows(i).Item(0), ".")
                                If iStart = 0 Then
                                    InputfileDef = enDT.Rows(i).Item(0)
                                Else
                                    InputfileDef = Mid(enDT.Rows(i).Item(0), InStrRev(enDT.Rows(i).Item(0), "."), Len(enDT.Rows(i).Item(0)))
                                End If
                                If FoundDef(HybrisDefinition, InputfileDef) Then
                                    Dim enUS As String = enDT.Rows(i).Item(0)
                                    Dim bFound As Boolean = False
                                    For j As Integer = 0 To TargetDT.Rows.Count - 1
                                        If enUS.ToString.ToLower.Trim = TargetDT.Rows(j).Item(0).ToString.ToLower.Trim _
                                            And enDT.Rows(i).Item(1).ToString.ToLower.Trim = TargetDT.Rows(j).Item(1).ToString.ToLower.Trim Then
                                            bFound = True
                                            Exit For
                                        End If
                                    Next
                                    If bFound And Not IsDBNull(enDT.Rows(i).Item(1)) Then
                                        If CanbeAccepted(enDT.Rows(i).Item(1)) Then
                                            Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & wrap_html(clean_xml(enUS.Trim)) & Chr(34) & " resname=" & Chr(34) & "Hybris" & Chr(34) & ">")
                                            Writer.WriteLine("<source>" & wrap_html(clean_xml(enDT.Rows(i).Item(0))) & "</source>")
                                            Writer.WriteLine("<target state=""needs-review-translation""></target>")
                                            Writer.WriteLine("<note from=""Developer"" priority =""10"">" & "Properties : " & fName & "</note>")
                                            Writer.WriteLine("</trans-unit>")
                                            Writer.WriteLine(vbCrLf)
                                            myNum += 1
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If

                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

            End Using

            If myNum = 1 Then
                Try
                    System.IO.File.Delete(xliff_savePath)
                    Return " Already has translation for " & Replace(Targetlanguage, "-", "_")
                Catch ex As Exception
                    'do nothing
                End Try
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return ""
    End Function

    'Multi row 
    Function WriteDataToXliff(ByRef enDT As DataTable, ByRef TargetDT As DataTable, ByRef Writer As StreamWriter, ByRef HybrisDefinition As String(), ByVal fName As String) As Integer
        Dim myNum As Integer = 1
        Try
            For i As Integer = 0 To enDT.Rows.Count - 1
                If Not IsDBNull(enDT.Rows(i).Item(0)) Then
                    If Left(enDT.Rows(i).Item(0), 1) <> "#" And enDT.Rows(i).Item(0).ToString.Trim <> "" Then
                        Dim hyDef As String = enDT.Rows(i).Item(0)
                        If InStrRev(enDT.Rows(i).Item(0), ".") <> 0 Then
                            hyDef = Mid(enDT.Rows(i).Item(0), InStrRev(enDT.Rows(i).Item(0), "."), Len(enDT.Rows(i).Item(0)))
                        End If
                        If FoundDef(HybrisDefinition, hyDef) Then
                            Dim IdColumn_enUS As String = enDT.Rows(i).Item(0) 'Match this id col

                            Dim enUS As String = ""
                            Dim targetUS As String = ""
                            Dim bFound As Boolean = False
                            For j As Integer = 0 To TargetDT.Rows.Count - 1
                                If IdColumn_enUS.ToLower = TargetDT.Rows(j).Item(0).ToString.ToLower Then ' here
                                    If IdColumn_enUS.ToString.ToLower.Trim = TargetDT.Rows(j).Item(0).ToString.ToLower.Trim Then
                                        If Microsoft.VisualBasic.Right(enDT.Rows(i).Item(1).ToString.Trim, 1) <> "\" Then
                                            'For single row
                                            If enDT.Rows(i).Item(1).ToString.ToLower.Trim = TargetDT.Rows(j).Item(1).ToString.ToLower.Trim Then
                                                enUS = enDT.Rows(i).Item(1).ToString
                                                bFound = True
                                                Exit For
                                            End If
                                        Else
                                            'For Multi row
                                            enUS = enDT.Rows(i).Item(1).ToString.Trim
                                            Dim mrow As Integer = 0
                                            For mrow = i + 1 To enDT.Rows.Count - 1
                                                If Microsoft.VisualBasic.Right(enDT.Rows(mrow).Item(0), 1) = "\" Then
                                                    If enUS = "" Then
                                                        enUS = enDT.Rows(mrow).Item(0)
                                                    Else
                                                        enUS = enUS & " " & enDT.Rows(mrow).Item(0).ToString.Trim
                                                    End If
                                                Else
                                                    enUS = enUS & " " & enDT.Rows(mrow).Item(0).ToString.Trim
                                                    Exit For
                                                End If
                                            Next

                                            i = mrow

                                            targetUS = TargetDT.Rows(j).Item(1).ToString.Trim

                                            For xRow As Integer = j + 1 To TargetDT.Rows.Count - 1
                                                If Microsoft.VisualBasic.Right(TargetDT.Rows(xRow).Item(0), 1) = "\" Then
                                                    If targetUS = "" Then
                                                        targetUS = TargetDT.Rows(xRow).Item(0)
                                                    Else
                                                        targetUS = targetUS & " " & TargetDT.Rows(xRow).Item(0).ToString.Trim
                                                    End If
                                                Else
                                                    targetUS = targetUS & " " & TargetDT.Rows(xRow).Item(0).ToString.Trim
                                                    Exit For
                                                End If
                                            Next

                                            If enUS = targetUS Then
                                                bFound = True
                                                Exit For
                                            End If

                                        End If
                                    End If
                                End If
                            Next

                            If bFound And CanbeAccepted(enUS) Then
                                Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & IdColumn_enUS.Trim & Chr(34) & " resname=" & Chr(34) & "Hybris" & Chr(34) & ">")
                                Writer.WriteLine("<source>" & wrap_html(clean_xml(enUS)) & "</source>")
                                Writer.WriteLine("<target state=""needs-review-translation""></target>")
                                Writer.WriteLine("<note from=""Developer"" priority =""10"">" & "Properties : " & fName & "</note>")
                                Writer.WriteLine("</trans-unit>")
                                Writer.WriteLine(vbCrLf)
                                myNum += 1
                            End If

                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return myNum
    End Function

    'Create table with standarad 2 columns, by concatenating from all columns > 1 in Column 1
    Function CreateNewDatatables(ByRef DT As DataTable) As DataTable
        Dim NewDT As New DataTable
        NewDT.Columns.Add("ID")
        NewDT.Columns.Add("enUS")

        Try
            'Take out multiple columns and concat with "="
            Dim dRow As DataRow
            For iRow As Integer = 0 To DT.Rows.Count - 1
                dRow = NewDT.NewRow
                Dim enUS As String = ""
                For iCol As Integer = 1 To DT.Columns.Count - 1
                    If Not IsDBNull(DT.Rows(iRow).Item(iCol)) Then
                        If enUS = "" Then
                            enUS = DT.Rows(iRow).Item(iCol)
                        Else
                            enUS = enUS & "=" & DT.Rows(iRow).Item(iCol)
                        End If
                    End If
                Next
                dRow("ID") = DT.Rows(iRow).Item(0)
                dRow("enUS") = enUS
                NewDT.Rows.Add(dRow)
            Next

            'Refine the New datatable
            For i As Integer = 0 To NewDT.Rows.Count - 1

                If Not IsDBNull(NewDT.Rows(i).Item(1)) Then
                    If Microsoft.VisualBasic.Right(NewDT.Rows(i).Item(1), 1) = "\" Then
                        For j As Integer = i + 1 To NewDT.Rows.Count - 1
                            If Not IsDBNull(NewDT.Rows(j).Item(0)) Then
                                If Microsoft.VisualBasic.Right(NewDT.Rows(j).Item(0), 1) <> "\" Then
                                    'check if Col2 is null
                                    If IsDBNull(NewDT.Rows(j).Item(1)) Then
                                        Exit For
                                    Else
                                        If NewDT.Rows(j).Item(1) = "" Then
                                            Exit For
                                        End If
                                        NewDT.Rows(j).Item(0) = NewDT.Rows(j).Item(0) & "=" & NewDT.Rows(j).Item(1)
                                        NewDT.Rows(j).Item(1) = ""
                                    End If
                                End If
                            End If
                            i = j
                        Next
                    End If
                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return NewDT

    End Function

    Function FoundDef(ByRef HybrisDefinition As String(), ByVal comparestring As String) As Boolean
        Try
            For x As Integer = 0 To HybrisDefinition.Count - 1
                If comparestring.ToLower = HybrisDefinition(x).ToLower Then
                    Return False
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function

    Function CanbeAccepted(ByVal enUS As String) As Boolean
        Try
            Select Case Microsoft.VisualBasic.Right(enUS, 4).ToLower
                Case ".jpg", ".ico", ".png"
                    Return False
                Case ".jpe"
                    Return False
                Case ""
                    Return False
            End Select
            If IsNumeric(enUS) Then
                Return False
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function

#End Region

#Region "Create Properties Back"

    Public Function CreatePropertiesBack(ByVal EnFileName As String, ByVal TargetFileName As String, ByVal Targetlanguage As String, ByVal TranslatedxliffFile As String, ByRef bw As BackgroundWorker) As Boolean
        Try
            If TargetFileName = "" Then
                TargetFileName = EnFileName
            End If

            Dim DT As DataTable = Get_Properties_toDatatable(TargetFileName, True)

            Dim enDT As DataTable = Get_Properties_toDatatable(EnFileName, True)

            MakeEqual_en_and_target_table(enDT, DT)

            If DT.Columns.Count > 2 Then 'IF more than 2 columns then it has multi row possiblity
                DT = CreateNewDatatables(DT)
                For i As Integer = 0 To DT.Rows.Count - 1
                    If Microsoft.VisualBasic.Right(DT.Rows(i).Item(1).ToString.Trim, 1) <> "\" Then
                        'For single row
                        'Do nothing
                    Else
                        'For Multi row
                        Dim mrow As Integer = 0
                        For mrow = i + 1 To DT.Rows.Count - 1
                            If Microsoft.VisualBasic.Right(DT.Rows(mrow).Item(0), 1) = "\" Then
                                DT.Rows(mrow).Item(0) = ""
                                i -= 1
                            Else
                                DT.Rows(mrow).Item(0) = ""
                                i -= 1
                                Exit For
                            End If
                        Next
                        i = mrow
                    End If
                Next
                'Check double row blanks and remove empty rows
                Dim x As Integer = DT.Rows.Count - 2
                Do While x > 0
                    If DT.Rows(x).Item(0) = "" And DT.Rows(x + 1).Item(0) = "" Then
                        DT.Rows(x).Delete()
                        x += 1
                    End If
                    x -= 1
                Loop
            End If


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

            Dim notFound As New ArrayList

            If objXliff.ID.Count = 0 Then
                UpdateMsg(Now & Chr(9) & "No translations found for " & System.IO.Path.GetFileName(TranslatedxliffFile) & vbCrLf, Form_MainNew.RtbColor.Red, bw)
                'Throw New Exception("0 translations found in " & System.IO.Path.GetFileName(TranslatedxliffFile))
            End If

            For x As Integer = 0 To objXliff.ID.Count - 1
                Dim header As String = objXliff.ID(x)
                header = Mid(header, InStr(header, "_") + 1, Len(header))
                For j As Integer = 0 To DT.Rows.Count - 1
                    If Not IsDBNull(DT.Rows(j).Item(0)) Then
                        If GetPlainText(DT.Rows(j).Item(0).ToString.ToLower.Trim) = GetPlainText(header.ToString.ToLower.Trim) Then
                            DT.Rows(j).Item(1) = unwrap_html(revert_xml(objXliff.Translation(x)))
                            Exit For
                        End If
                    End If
                Next
            Next

            'Build back csv
            'define target path
            Dim targetfilepath As String
            Dim sFileName As String = System.IO.Path.GetFileName(EnFileName)

            'Monolingual
            targetfilepath = Replace(EnFileName, "01-Input-B", "05-Output")
            targetfilepath = System.IO.Path.GetDirectoryName(targetfilepath) & "\Mono_" & Targetlanguage & "\"

            If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(targetfilepath)) <> True Then
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetfilepath))
            End If

            targetfilepath = targetfilepath & sFileName

            'Write csv file.
            Dim utf8WithoutBom As New System.Text.UTF8Encoding(False)
            Using writer As StreamWriter = New StreamWriter(targetfilepath, False, System.Text.Encoding.UTF8)
                WriteDataTableToProperties_Hybris(DT, writer, Targetlanguage)
            End Using

            'NO Translation found then show a msg box and log it as well.
            'If notFound.Count > 0 Then
            '    Dim objMissingTransaltion As New MissedTranslations
            '    objMissingTransaltion.UpdateMsg(notFound, EnFileName, Targetlanguage)
            'End If
        Catch ex As Exception
            Throw New Exception("Error @CreatePropertiesBack" & vbNewLine & ex.Message)
        End Try
        Return True
    End Function

    Private Sub WriteDataTableToProperties_Hybris(ByVal sourceTable As DataTable, ByVal writer As TextWriter, ByVal lang As String)
        Dim rowValues As List(Of String)

        lang = LCase(Right(lang, 2))
        Try

            For j As Integer = 0 To sourceTable.Rows.Count - 1
                rowValues = New List(Of String)()
                For i As Integer = 0 To sourceTable.Columns.Count - 1
                    'If sourceTable.Rows(j).Item(i).ToString.Trim <> "" Then
                    rowValues.Add((sourceTable.Rows(j).Item(i).ToString))
                    'End If
                Next
                If rowValues.Count = 0 Then
                    writer.WriteLine(String.Join("", rowValues))
                ElseIf (rowValues(0).ToString.Trim.Length >= 1) And (Left(rowValues(0), 1) = "#" Or rowValues(0) = "#" Or Left(rowValues(0), 1) = "$") Then
                    writer.WriteLine(String.Join("", rowValues))
                Else
                    Dim bEmpty As Boolean = True
                    For i As Integer = 0 To rowValues.Count - 1
                        If rowValues(i) <> "" Then
                            bEmpty = False
                            Exit For
                        End If
                    Next

                    If bEmpty Then
                        writer.WriteLine(String.Join("", rowValues))
                    Else
                        writer.WriteLine(String.Join("=", rowValues))
                    End If

                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            writer.Flush()
        End Try

    End Sub

#End Region

    Private Function Get_Properties_toDatatable(ByVal sCsvFilePath As String, ByVal bWithBlankLines As Boolean) As DataTable

        Dim csvData As New DataTable()
        Try
            'First Get the Max last column used in csv file, this is used to set the column limit for datatable------------------------------------------
            Dim LastCol As Integer = 0
            Using csvReader As New TextFieldParser(sCsvFilePath, System.Text.Encoding.UTF8)
                csvReader.SetDelimiters(New String() {"="})
                csvReader.HasFieldsEnclosedInQuotes = False
                csvReader.TrimWhiteSpace = False
                While Not csvReader.EndOfData
                    Dim fieldData As String() = csvReader.ReadFields
                    If LastCol < UBound(fieldData) Then
                        LastCol = UBound(fieldData)
                    End If
                End While
            End Using
            '---------------------------------------------------------------------------------------------------------------------------------------------

            'Now load the csv data to datatable
            Using csvReader As New TextFieldParser(sCsvFilePath, System.Text.Encoding.UTF8)
                csvReader.SetDelimiters(New String() {"="})
                csvReader.HasFieldsEnclosedInQuotes = False
                csvReader.TrimWhiteSpace = False
                For i As Integer = 0 To LastCol
                    csvData.Columns.Add(i, GetType(String))
                Next

                While Not csvReader.EndOfData
                    Dim fData As String = csvReader.ReadLine
                    Dim fieldData As String() = Split(fData, "=")
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

    Private Sub MakeEqual_en_and_target_table(ByRef enDT As DataTable, ByRef targetDT As DataTable)
        'Add rows to target table 
        'If enDT.Rows.Count > targetDT.Rows.Count Then
        '    Dim d As DataRow
        '    For i As Integer = targetDT.Rows.Count - 1 To enDT.Rows.Count - 2
        '        d = targetDT.NewRow
        '        targetDT.Rows.Add(d)
        '    Next
        'End If
        'Match content from en to target
        For i As Integer = 0 To enDT.Rows.Count - 1
            If Not IsDBNull(enDT.Rows(i).Item(0)) Then
                If Left(enDT.Rows(i).Item(0), 1) <> "#" Then
                    Dim enUS As String = enDT.Rows(i).Item(0)
                    Dim bFound As Boolean = False
                    For j As Integer = 0 To targetDT.Rows.Count - 1
                        If enUS.ToString.ToLower.Trim = targetDT.Rows(j).Item(0).ToString.ToLower.Trim Then
                            bFound = True
                            Exit For
                        End If
                    Next
                    If Not bFound Then
                        Dim d As DataRow
                        d = targetDT.NewRow
                        d.Item(0) = enDT.Rows(i).Item(0)
                        d.Item(1) = enDT.Rows(i).Item(1)
                        targetDT.Rows.Add(d)
                        'targetDT.Rows(i).Item(0) = enDT.Rows(i).Item(0)
                        'targetDT.Rows(i).Item(1) = enDT.Rows(i).Item(1)
                    End If
                End If
            End If
        Next

    End Sub

End Module
