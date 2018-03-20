Imports Microsoft.VisualBasic.FileIO
Imports System.IO
Imports System.Text
Imports System.Web.HttpUtility
Imports System.Globalization

Module Mod_LMS

    Dim Definitions() As String
    Dim CompareDT As DataTable
    Dim SourceDT As DataTable

#Region "Generate Xliff"
    'Public Function LMS_To_Xliff(ByVal LmsFile As String, ByVal xliffPath As String, ByVal lang As String, ByVal ComparingFile As String) As Integer
    '    Try
    '        fileCount = 0
    '        CompareDT = Get_LMS_toDatatable(ComparingFile)
    '        SourceDT = Get_LMS_toDatatable(LmsFile)

    '        Dim AllText As String = System.IO.File.ReadAllText(Application.StartupPath & "\Definition\LMS_Definition\LMS_definition.txt")

    '        Dim NewText As String() = Split(AllText, vbNewLine)

    '        For i As Integer = 1 To UBound(NewText)
    '            If NewText(i) = "######################################" Then
    '                For j As Integer = i + 1 To UBound(NewText)
    '                    If NewText(j) = "######################################" Then
    '                        GenerateXliff(NewText, i + 1, j - 1, xliffPath, lang)
    '                        i = j - 1
    '                        Exit For
    '                    End If
    '                Next
    '            End If
    '        Next

    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try
    '    Return fileCount
    'End Function
    Public Sub Setup_Datatables(ByVal LmsFile As String, ByVal ComparingLmsFile As String)
        Try
            If ComparingLmsFile <> "" Then
                CompareDT = Get_LMS_toDatatable(ComparingLmsFile)
                SourceDT = Get_LMS_toDatatable(LmsFile)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
    'Dim fileCount As Integer
    'Public Sub GenerateXliff(ByVal objText As String(), ByVal iStart As Integer, ByVal iEnd As Integer, ByVal xliff_Path As String, ByVal Targetlanguage As String)
    '    Try
    '        Dim sXliffFileName As String = objText(iStart)
    '        sXliffFileName = Mid(sXliffFileName, InStr(sXliffFileName, "(") + 1, Len(sXliffFileName))
    '        sXliffFileName = sXliffFileName.Substring(0, sXliffFileName.Length - 1) & "_" & Targetlanguage & ".xliff"

    '        If System.IO.File.Exists(xliff_Path & sXliffFileName) Then
    '            ShowMsgInMainForm(Now & Chr(9) & sXliffFileName & " File for translators in " & Targetlanguage & " exists." & vbCrLf)
    '            Exit Sub
    '        End If

    '        Dim arrList_DefContent As New ArrayList
    '        Dim tempArrlist As ArrayList

    '        For i As Integer = iStart + 1 To iEnd
    '            Dim SearchDef As String = objText(i)
    '            tempArrlist = New ArrayList
    '            tempArrlist.Add(GetDefRelatedContentToDT(SearchDef))
    '            If tempArrlist(0).Count > 1 Then
    '                arrList_DefContent.Add(tempArrlist(0))
    '            End If
    '        Next

    '        Dim xliffContent As New StringBuilder
    '        Dim myNum As Integer = 1

    '        Try
    '            For i As Integer = 0 To arrList_DefContent.Count - 1
    '                Dim dict_Def As New Dictionary(Of String, String)
    '                dict_Def = arrList_DefContent(i)
    '                For j As Integer = 0 To dict_Def.Count - 1
    '                    xliffContent.Append("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & Chr(34) & " resname=" & Chr(34) & dict_Def.Keys(j) & Chr(34) & ">" & vbCrLf)
    '                    xliffContent.Append("<source>" & wrap_html(dict_Def(dict_Def.Keys(j))) & "</source>" & vbCrLf)
    '                    If IsNumeric(dict_Def(dict_Def.Keys(j))) Then
    '                        xliffContent.Append("<target state=""needs-review-translation"">" & dict_Def(dict_Def.Keys(j)) & "</target>" & vbCrLf)
    '                    Else
    '                        xliffContent.Append("<target state=""needs-review-translation""></target>" & vbCrLf)
    '                    End If
    '                    xliffContent.Append("<note from=""Developer"" priority =""10"">LMS" & "</note>" & vbCrLf)
    '                    xliffContent.Append("</trans-unit>" & vbCrLf)
    '                    xliffContent.Append(vbCrLf)
    '                    myNum += 1
    '                Next
    '            Next
    '        Catch ex As Exception
    '            Throw New Exception(ex.Message)
    '        End Try

    '        If xliffContent.Length > 0 Then
    '            Using Writer As StreamWriter = New StreamWriter(xliff_Path & sXliffFileName, False, System.Text.Encoding.UTF8)
    '                Writer.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
    '                Writer.WriteLine("<xliff version=""1.2"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ")
    '                Writer.WriteLine("xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2- &#xD;&#xA;strict.xsd"">")
    '                Writer.WriteLine("<file original=""C:\Temp\en_fr_2.xliff"" source-language=""en-US"" target-language=" & Chr(34) & Replace(Targetlanguage, "_", "-") & Chr(34) & " datatype=""plaintext"" date=" & Chr(34) & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & Chr(34) & " tool-id=""SAP SFSF CONVERTER"" category=""MLT"">")
    '                Writer.WriteLine("<header>")
    '                Writer.WriteLine("<phase-group>")
    '                Writer.WriteLine("<phase phase-name=""Translation"" process-name=""999999"" company-name=""SAP"">")
    '                Writer.WriteLine("</phase>")
    '                Writer.WriteLine("</phase-group>")
    '                Writer.WriteLine("<tool tool-id=""SAP_SF_CONV""  tool-name=""SSC"">")
    '                Writer.WriteLine("</tool>")
    '                Writer.WriteLine("<note>TEST</note>")
    '                Writer.WriteLine("</header>")
    '                Writer.WriteLine("<body>")

    '                Writer.WriteLine(vbCrLf)

    '                Writer.WriteLine(xliffContent.ToString)

    '                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

    '                ShowMsgInMainForm(Now & Chr(9) & sXliffFileName & " - File in " & Targetlanguage & " generated." & vbCrLf)
    '                fileCount += 1
    '            End Using
    '        Else
    '            ShowMsgInMainForm(Now & Chr(9) & sXliffFileName & " - no data found for " & Targetlanguage & " langauge." & vbCrLf)
    '        End If

    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try


    'End Sub

    Public Function GenerateXliff(ByVal objText As String(), ByVal iStart As Integer, ByVal iEnd As Integer, ByVal xliff_Path As String, ByVal Targetlanguage As String, Optional ByVal bIsRestrictMaxLength As Boolean = False) As Integer
        Dim fileCount As Integer
        Try
            Dim sXliffFileName As String = objText(iStart)
            sXliffFileName = Mid(sXliffFileName, InStr(sXliffFileName, "(") + 1, Len(sXliffFileName))
            sXliffFileName = sXliffFileName.Substring(0, sXliffFileName.Length - 1) & "_" & Targetlanguage & ".xliff"

            Dim arrList_DefContent As New ArrayList
            Dim tempArrlist As ArrayList

            For i As Integer = iStart + 1 To iEnd
                Dim SearchDef As String = objText(i)
                tempArrlist = New ArrayList
                tempArrlist.Add(GetDefRelatedContentToDT(SearchDef))
                If tempArrlist(0).Count > 1 Then
                    arrList_DefContent.Add(tempArrlist(0))
                End If
            Next

            Dim xliffContent As New StringBuilder
            Dim myNum As Integer = 1

            Try
                For i As Integer = 0 To arrList_DefContent.Count - 1
                    Dim dict_Def As New Dictionary(Of String, String)
                    dict_Def = arrList_DefContent(i)
                    For j As Integer = 0 To dict_Def.Count - 1
                        xliffContent.Append("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & Chr(34) & " resname=" & Chr(34) & clean_xml(dict_Def.Keys(j)) & Chr(34) & ">" & vbCrLf)
                        xliffContent.Append("<source>" & wrap_html(dict_Def(dict_Def.Keys(j))) & "</source>" & vbCrLf)
                        If IsNumeric(dict_Def(dict_Def.Keys(j))) Then
                            xliffContent.Append("<target state=""needs-review-translation"">" & dict_Def(dict_Def.Keys(j)) & "</target>" & vbCrLf)
                        Else
                            xliffContent.Append("<target state=""needs-review-translation""></target>" & vbCrLf)
                        End If
                        xliffContent.Append("<note from=""Developer"" priority =""10"">LMS" & "</note>" & vbCrLf)
                        xliffContent.Append("</trans-unit>" & vbCrLf)
                        xliffContent.Append(vbCrLf)
                        myNum += 1
                    Next
                Next
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

            If xliffContent.Length > 0 Then
                Using Writer As StreamWriter = New StreamWriter(xliff_Path & sXliffFileName, False, System.Text.Encoding.UTF8)
                    Writer.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                    Writer.WriteLine("<xliff version=""1.2"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ")
                    Writer.WriteLine("xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2- &#xD;&#xA;strict.xsd"">")
                    Writer.WriteLine("<file original=""C:\Temp\en_fr_2.xliff"" source-language=""en-US"" target-language=" & Chr(34) & Replace(Targetlanguage, "_", "-") & Chr(34) & " datatype=""plaintext"" date=" & Chr(34) & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & Chr(34) & " tool-id=""SAP SFSF CONVERTER"" category=""MLT"">")
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

                    Writer.WriteLine(xliffContent.ToString)

                    Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

                    fileCount += 1
                End Using

            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return fileCount

    End Function

    Private Function GetDefRelatedContentToDT(ByVal objDef As String) As Dictionary(Of String, String)
        Try
            Dim DefContent As New Dictionary(Of String, String)
            Dim bFound As Boolean
            For i As Integer = 0 To SourceDT.Rows.Count - 1
                Dim str As String = SourceDT.Rows(i).Item(0).ToString.ToLower.Trim
                If str.Contains(LCase(objDef)) Then
                    bFound = False
                    For j As Integer = 0 To CompareDT.Rows.Count - 1
                        Dim strs As String = CompareDT.Rows(j).Item(0).ToString.ToLower.Trim
                        If strs = str Then
                            bFound = True
                            Exit For
                        End If
                    Next
                    If bFound <> True Then
                        Dim xString As String = SourceDT.Rows(i).Item(1)
                        For x As Integer = 2 To SourceDT.Columns.Count - 1
                            If IsDBNull(SourceDT.Rows(i).Item(x)) <> True Then
                                xString = xString & "=" & SourceDT.Rows(i).Item(x)
                            End If
                        Next
                        If SourceDT.Rows(i).Item(0).ToString.Trim.ToLower <> xString.ToString.Trim.ToLower And xString.ToString.Trim <> "\" Then
                            'Unescape Javascript text
                            'xString = System.Text.RegularExpressions.Regex.Unescape(xString)
                            'Update normal xml escapes
                            xString = xString.Replace("\n", vbCrLf)
                            xString = wrap_html(clean_xml(xString))
                            DefContent.Add(SourceDT.Rows(i).Item(0), xString)
                        End If
                    End If
                End If

            Next
            Return DefContent
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function
#End Region

    Private Function Get_LMS_toDatatable(ByVal sLMSFile As String) As DataTable

        Dim csvData As New DataTable()
        Try
            'First Get the Max last column used in csv file, this is used to set the column limit for datatable------------------------------------------
            Dim LastCol As Integer = 0
            Using csvReader As New TextFieldParser(sLMSFile)
                csvReader.SetDelimiters(New String() {"="})
                csvReader.HasFieldsEnclosedInQuotes = False
                While Not csvReader.EndOfData
                    Dim fieldData As String() = csvReader.ReadFields()
                    If LastCol < UBound(fieldData) Then
                        LastCol = UBound(fieldData)
                    End If
                End While
            End Using
            '---------------------------------------------------------------------------------------------------------------------------------------------

            'Now load the csv data to datatable
            Using csvReader As New TextFieldParser(sLMSFile)
                csvReader.SetDelimiters(New String() {"="})
                csvReader.HasFieldsEnclosedInQuotes = False

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

#Region "Generate LMS Back"
    Public Sub Xliff_To_LMS(ByVal translated_xliff_path As String, ByVal Lang As String)
        Dim sFileName As String = System.IO.Path.GetFileName(translated_xliff_path)

        'Monolingual
        targetfilepath = Replace(translated_xliff_path, "03-Backfromtranslation", "05-Output")
        targetfilepath = System.IO.Path.GetDirectoryName(targetfilepath) & "\Mono_" & Lang & "\"

        If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(targetfilepath)) <> True Then
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetfilepath))
        End If

        targetfilepath = targetfilepath & System.IO.Path.GetFileNameWithoutExtension(translated_xliff_path) & ".lms"

        Dim objXliff As New sXliff
        Try
            objXliff = ModHelper.load_xliff(translated_xliff_path)
        Catch ex As Exception
            If ModHelper.UnWrapXliffBack(translated_xliff_path) <> True Then
                Throw New Exception("Error UnWrapping xliff back!")
            End If
            objXliff = ModHelper.cvload_xliff(Application.StartupPath & "\Temp_UnWrap.xliff")
        End Try

        Try
            Using Writer As StreamWriter = New StreamWriter(targetfilepath, False, System.Text.ASCIIEncoding.ASCII)
                For i As Integer = 0 To objXliff.ID.Count - 1
                    Dim str As String = ""
                    If objXliff.Translation(i).ToString.Trim <> "" Then
                        str = unwrap_html(revert_xml(objXliff.Translation(i)))
                    End If

                    'str = System.Web.HttpUtility.JavaScriptStringEncode(str) ' Dont Use this

                    str = JsUnicodeEncode(str)
                    Writer.WriteLine(Encoding.ASCII.GetString(Encoding.UTF8.GetBytes(objXliff.Resname(i) & "=" & str)))
                Next
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
      

    End Sub

    Public Sub Xliff_To_LMS_New(ByVal translated_xliff_path As String, ByVal Lang As String) 'Change this if required to compare with source
        Dim sFileName As String = System.IO.Path.GetFileName(translated_xliff_path)

        'Monolingual
        targetfilepath = Replace(translated_xliff_path, "03-Backfromtranslation", "05-Output")
        targetfilepath = System.IO.Path.GetDirectoryName(targetfilepath) & "\Mono_" & Lang & "\"

        If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(targetfilepath)) <> True Then
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetfilepath))
        End If

        targetfilepath = targetfilepath & System.IO.Path.GetFileNameWithoutExtension(translated_xliff_path) & ".lms"

        Dim objXliff As New sXliff
        Try
            objXliff = ModHelper.load_xliff(translated_xliff_path)
        Catch ex As Exception
            If ModHelper.UnWrapXliffBack(translated_xliff_path) <> True Then
                Throw New Exception("Error UnWrapping xliff back!")
            End If
            objXliff = ModHelper.cvload_xliff(Application.StartupPath & "\Temp_UnWrap.xliff")
        End Try

        Try
            Using Writer As StreamWriter = New StreamWriter(targetfilepath, False, System.Text.ASCIIEncoding.ASCII)
                For i As Integer = 0 To objXliff.ID.Count - 1
                    Dim str As String = ""
                    If objXliff.Translation(i).ToString.Trim <> "" Then
                        str = unwrap_html(revert_xml(objXliff.Translation(i)))
                    End If

                    'str = System.Web.HttpUtility.JavaScriptStringEncode(str) ' Dont Use this

                    str = JsUnicodeEncode(str)
                    Writer.WriteLine(Encoding.ASCII.GetString(Encoding.UTF8.GetBytes(objXliff.Resname(i) & "=" & str)))
                Next
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


    End Sub

    Private Function JsUnicodeEncode(ByVal unEncodedString As String) As String
        Const noEncode As String = "[A-Za-z0-9 ]"    '' include other chars you don't want to encode
        Dim encoded As New System.Text.StringBuilder
        For Each c As Char In unEncodedString
            If c Like noEncode Then
                encoded.Append(c)
            Else
                encoded.AppendFormat("\u{0:x4}", AscW(c))
            End If
        Next
        Return encoded.ToString
    End Function
#End Region

End Module

'Public Class StringExtension
'    Private Shared Function StringFold(input As String, proc As Func(Of Char, String)) As String
'        Return String.Concat(input.[Select](proc).ToArray())
'    End Function

'    Private Shared Function FoldProc(input As Char) As String
'        'If (Asc(input) < &HFD AndAlso Asc(input) > &H1F) Then
'        If Asc(input) > 128 Then
'            Return String.Format("\u{0:x4}", CInt(AscW(input)))
'        End If


'        Return input.ToString()
'    End Function

'    Public Shared Function EscapeToAscii(input As String) As String
'        Return StringFold(input, AddressOf FoldProc)
'    End Function
'End Class

