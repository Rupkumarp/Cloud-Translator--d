
'To convert xml files into csv for competencies. 
'Here is how to achieve it:
'1.	Add a menu item “tools” in the main form. Add a menu action “Convert xml to csv competencies lib”
'2.	Add a folder “Tools” in the main application dir. Subfolder CompLib.
'3.	We’ll copy the standard xml libs into complib. The competency libs are xml based, but monolingual. They contain competencies and description. There are 2 versions of libs 1.0 & 2.0 (+2.1 which is close to 2.0).
'The business problem is that the comp libs are delivered in xml but we cannot import them in our system because of a special option which is activated; we need them in csv (mdf). The data is translated, so it’s better to translate the csv.

'Steps to translate.
'We have a basis mdfcsv which contains the strings to translate. There are 4 fields per competency to fill. Name, libname, category & description.

'Step1, Create bilingual xliff dictionnary.
'Open the competency libs 1.0 (xml) in English. Extract Competency-name, competency-desc and put this as the source for each entry of the xliff, keeping track of the guid. Everytime, check if the entry already exists to avoid duplicates.
'Open the same lib in another language and then add the target. 
'Like usual, keep one language pair per file. 

'Open then the competency libs 2.0 (xml) in English, and check if there is any new source text (in competency-name, desc or category; actually just the same process as above, which should already avoid duplicates). The GuiD might be different and is not critical, except for searching the translation. (not for the csv import later).

'Once you’re done, we should have 8 xliff with all the translations of name/desc/cat.
'Step2, For libname, please copy the enUS to all other languages. There are few entries in deDE & esES, just overwrite those with the enUS value.
'Step 3, just reimport the translations into the csv.
'Go through all entries in the csv, look into the first column (name.en_US), search in the xliff file for such a source and then copy the target in the right column. Redo it for cat & description.

Imports System.IO
Imports Microsoft.VisualBasic.FileIO
Imports System.Xml
Imports System.ComponentModel

Module Mod_CompetencyLib

    Public targetfilepath As String

    Dim TotalFileCount As Integer

#Region "Step1"
    'Step 1
    Public Function CreateBilingualxliff(ByVal xmlFolder As String, ByVal ExtractedXliff_Path As String) As Integer
        TotalFileCount = 0
        Try
            Dim TotalCompLibFiles As New ArrayList '1.0
            Dim TotalPremiumFiles As New ArrayList '2.0

            For Each File In My.Computer.FileSystem.GetFiles(xmlFolder)
                If Path.GetExtension(File.ToString).ToLower() = ".xml" Then
                    If InStr(File.ToString.ToLower, "competency-lib") > 0 Then
                        TotalCompLibFiles.Add(File.ToString)
                    Else
                        TotalPremiumFiles.Add(File.ToString)
                    End If
                End If
            Next

            If TotalCompLibFiles.Count = 0 And TotalPremiumFiles.Count = 0 Then
                Throw New Exception("No xml files found in - " & xmlFolder)
            End If

            '
            Dim ComplibEnUS As String = ""
            Dim SfPremiumEnUs As String = ""
            Dim iRemoveComplibEnUs As Integer = -1
            Dim iRemovePremiumEnUS As Integer = -1
            Try
                For i As Integer = 0 To TotalCompLibFiles.Count - 1
                    If ChecklangExist(Right(Path.GetFileNameWithoutExtension(TotalCompLibFiles(i).ToString.ToLower), 2)) <> True Then
                        Throw New Exception(ComplibEnUS(i))
                    End If

                    If Right(Path.GetFileNameWithoutExtension(TotalCompLibFiles(i).ToString.ToLower), 2) = "en" Or Right(Path.GetFileNameWithoutExtension(TotalCompLibFiles(i).ToString.ToLower), 2) = "us" Then
                        ComplibEnUS = TotalCompLibFiles(i)
                        iRemoveComplibEnUs = i
                    End If
                Next

                For i As Integer = 0 To TotalPremiumFiles.Count - 1
                    If ChecklangExist(Right(Path.GetFileNameWithoutExtension(TotalPremiumFiles(i).ToString.ToLower), 2)) <> True Then
                        Throw New Exception(TotalPremiumFiles(i))
                    End If

                    If Right(Path.GetFileNameWithoutExtension(TotalPremiumFiles(i).ToString.ToLower), 2) = "en" Or Right(Path.GetFileNameWithoutExtension(TotalPremiumFiles(i).ToString.ToLower), 2) = "us" Then
                        SfPremiumEnUs = TotalPremiumFiles(i)
                        iRemovePremiumEnUS = i
                    End If
                Next

                'If iRemoveComplibEnUs = -1 Or iRemovePremiumEnUS = -1 Then
                '    Throw New Exception("Could not find xml with enUS!" & vbNewLine & "Please Rename file ex: competency-lib-SUCCESSFACTORS_enUS.xml")
                'End If

                If iRemoveComplibEnUs <> -1 Then
                    TotalCompLibFiles.RemoveAt(iRemoveComplibEnUs)
                End If

                If iRemovePremiumEnUS <> -1 Then
                    TotalPremiumFiles.RemoveAt(iRemovePremiumEnUS)
                End If


                Dim TargetLang As String
                Dim targetXliff As String

                'Complib xliffs
                For i As Integer = 0 To TotalCompLibFiles.Count - 1

                    TargetLang = Right(Path.GetFileNameWithoutExtension(TotalCompLibFiles(i).ToString.ToLower.Replace("_", "").Replace("-", "")), 4)
                    If Not ChecklangExist(TargetLang) Then
                        Throw New Exception("Language not found for " & TotalCompLibFiles(i).ToString)
                    End If

                    targetXliff = System.IO.Path.GetFileNameWithoutExtension(ComplibEnUS) & "_" & TargetLang & ".xliff"

                    If Not System.IO.File.Exists(ExtractedXliff_Path & targetXliff) Then
                        Competency_Xml_ToXliff(ComplibEnUS, TotalCompLibFiles(i), TargetLang, "Competency-lib 1.0", ExtractedXliff_Path)
                        ShowMsgInMainForm(Now & Chr(9) & System.IO.Path.GetFileNameWithoutExtension(TotalCompLibFiles(i)) & ".xliff" & " - Competency-lib 1.0 File for translators generated." & vbCrLf, Form_MainNew.RtbColor.Black)
                        TotalFileCount += 1
                    Else
                        ShowMsgInMainForm(Now & Chr(9) & System.IO.Path.GetFileNameWithoutExtension(TotalCompLibFiles(i)) & ".xliff" & " - Competency-lib 1.0 File for translators exists." & vbCrLf, Form_MainNew.RtbColor.Black)
                    End If
                Next

                'SF Premium xliffs
                For i As Integer = 0 To TotalPremiumFiles.Count - 1

                    TargetLang = Right(Path.GetFileNameWithoutExtension(TotalPremiumFiles(i).ToString.ToLower.Replace("_", "").Replace("-", "")), 4)
                    If Not ChecklangExist(TargetLang) Then
                        Throw New Exception("Language not found for " & TotalCompLibFiles(i).ToString)
                    End If

                    targetXliff = System.IO.Path.GetFileNameWithoutExtension(SfPremiumEnUs) & "_" & TargetLang & ".xliff"

                    If Not System.IO.File.Exists(ExtractedXliff_Path & targetXliff) Then
                        Competency_Xml_ToXliff(SfPremiumEnUs, TotalPremiumFiles(i), Right(Path.GetFileNameWithoutExtension(TotalPremiumFiles(i).ToString.ToLower), 2), "Competency-lib 2.0", ExtractedXliff_Path)
                        ShowMsgInMainForm(Now & Chr(9) & System.IO.Path.GetFileNameWithoutExtension(TotalPremiumFiles(i)) & ".xliff" & " - Competency-lib 2.0 File for translators generated." & vbCrLf, Form_MainNew.RtbColor.Black)
                        TotalFileCount += 1
                    Else
                        ShowMsgInMainForm(Now & Chr(9) & System.IO.Path.GetFileNameWithoutExtension(TotalPremiumFiles(i)) & ".xliff" & " - Competency-lib 2.0 File for translators exists." & vbCrLf, Form_MainNew.RtbColor.Black)
                    End If

                Next

            Catch ex As Exception
                Throw New Exception("Error did not find language" & vbNewLine & ex.Message)
            End Try

        Catch ex As Exception
            Throw New Exception("Error @CreateBilingualxliff" & vbNewLine & ex.Message)
        End Try
        Return TotalFileCount
    End Function

    Private Function Competency_Xml_ToXliff(ByVal XmlFile_enUS As String, ByVal XmlFile_Target As String, ByVal Targetlanguage As String, ByVal Position As String, ByVal XliffPath As String) As Boolean

        Dim SourceTags As New Dictionary(Of String, String)
        Dim TargetTags As New Dictionary(Of String, String)
        Dim TagName As New ArrayList

        Dim xliff_Path As String = XliffPath & System.IO.Path.GetFileNameWithoutExtension(XmlFile_enUS) & "_" & Targetlanguage & ".xliff"

        Try
            'Load Defination file-------------------------------------------------------------------------------------------------------------------------------
            Dim Definitions(2) As String
            Definitions(0) = "competency-name"
            Definitions(1) = "competency-desc"
            Definitions(2) = "category"

            'Write xliff---------------------------------------------------------------------------------------------------------------------------------------
            Using Writer As StreamWriter = New StreamWriter(xliff_Path, False, System.Text.Encoding.UTF8)
                Writer.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                Writer.WriteLine("<xliff version=""1.2"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ")
                Writer.WriteLine("xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2- &#xD;&#xA;strict.xsd"">")
                Writer.WriteLine("<file original=""C:\Temp\en_fr_2.xliff"" source-language=""en-US"" target-language=" & Chr(34) & GetShort_lang(Targetlanguage) & Chr(34) & " datatype=""plaintext"" date=" & Chr(34) & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & Chr(34) & " tool-id=""SAP SFSF CONVERTER"" category=""MLT"">")
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

                For i As Integer = 0 To Definitions.Count - 1
                    Dim myNum As Integer = myNum + 1
                    SourceTags = CompetencyExtract(XmlFile_enUS, Definitions(i), "en_us")
                    TargetTags = CompetencyExtract(XmlFile_Target, Definitions(i), Targetlanguage)
                    For j As Integer = 0 To SourceTags.Count - 1
                        Dim targetSource As String = wrap_html(clean_xml(getKeyValue(SourceTags.Values(j), TargetTags)))
                        If targetSource <> "" Then
                            Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & Definitions(i) & "_" & SourceTags.Values(j) & Chr(34) & " resname=" & Chr(34) & (Position) & Chr(34) & ">")
                            Writer.WriteLine("<source>" & wrap_html(clean_xml(SourceTags.Keys(j))) & "</source>")
                            Writer.WriteLine("<target state=""needs-review-translation"">" & targetSource & "</target>")
                            Writer.WriteLine("<note from=""Developer"" priority =""10"">" & (Position) & ": " & (Replace(Replace(Definitions(i), "<", ""), ">", "") & "</note>"))
                            Writer.WriteLine("</trans-unit>")
                            Writer.WriteLine(vbCrLf)
                            myNum = myNum + 1
                        End If
                    Next
                    myNum = myNum - 1
                Next

                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

            End Using

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function

    Function getKeyValue(ByVal searchString As String, ByVal myTag As Dictionary(Of String, String)) As String
        Dim str As String = ""
        Try
            For i As Integer = 0 To myTag.Count - 1
                If myTag.Values(i) = searchString Then
                    str = myTag.Keys(i)
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return str
    End Function


    Function CompetencyExtract(ByVal xmlFile As String, ByVal ElementName As String, ByVal lang As String) As Dictionary(Of String, String)
        ' ElementName = clean_element(Replace(Replace(ElementName, "<", ""), ">", ""))
        Dim Tag As New Dictionary(Of String, String)

        Try
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(xmlFile)

            Dim xNodeList As XmlNodeList
            xNodeList = xd.GetElementsByTagName(Replace(Replace(ElementName, "<", ""), ">", ""))

            Dim MyAttributes As XmlAttributeCollection
            Dim str As String = ""

            For i As Integer = 0 To xNodeList.Count - 1

                Dim blangFound As Boolean = False
                If xNodeList(i).Attributes.Count > 0 Then
                    MyAttributes = xNodeList(i).Attributes
                    Dim att As XmlAttribute
                    For Each att In MyAttributes
                        If InStr(att.Name, "lang") > 0 Then
                            blangFound = True
                            If LCase(att.Value) = "en_us" Then
                                str = xNodeList(i).InnerText
                                Exit For
                            End If
                        End If
                    Next
                    If str = "" And blangFound <> True Then
                        str = xNodeList(i).InnerText
                    End If
                Else
                    str = xNodeList(i).InnerText
                End If
                If str <> "" Then
                    'If ExtractedTextNeedsTranslation(str, ElementName, lang, xNodeList, xNodeList(i)) Then
                    'Tags.Add(str)

                    For k As Integer = 0 To xNodeList(i).ParentNode.Attributes.Count - 1
                        If xNodeList(i).ParentNode.Attributes(k).Name.ToLower = "guid" Then
                            Try
                                Tag.Add(str, xNodeList(i).ParentNode.Attributes(k).Value)
                            Catch ex As System.ArgumentException
                                'Do nothing as it takes out duplicates
                            Catch ex As Exception
                                Throw New Exception(ex.Message)
                            End Try
                            Exit For
                        End If
                    Next

                    str = ""
                    'End If
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return Tag

    End Function

    Private Function ExtractedTextNeedsTranslation(ByVal source As String, ByVal ElementName As String, ByVal lang As String, ByVal xNodelist As XmlNodeList, ByVal root As XmlNode) As Boolean
        Dim bNeedTransation As Boolean = False
        Try

            Dim MyAttributes As XmlAttributeCollection
            Dim bCdata As Boolean = False
            Dim bFound As Boolean = False

            'Based on the nodes parentnode, check the element inside the particular node.
            'Next check for lang, if lang found, translation is available, then exit and save, else put the translation back

            For j As Integer = 0 To root.ParentNode.ChildNodes.Count - 1
                If "<" & root.ParentNode.ChildNodes(j).Name & ">" = ElementName Then
                    Dim subNode As XmlNode = root.ParentNode.ChildNodes(j)
                    If subNode.Attributes.Count > 0 Then
                        MyAttributes = subNode.Attributes
                        Dim att As XmlAttribute
                        For Each att In MyAttributes
                            If InStr(LCase(att.Name), "lang") > 0 Then
                                If InStr(LCase(att.Value), LCase(lang)) > 0 Then
                                    bFound = True
                                    Exit For
                                End If
                            End If
                        Next
                        If bFound Then
                            Exit For
                        End If
                    End If
                End If
            Next

            If bFound <> True Then
                bNeedTransation = True
            End If

        Catch ex As Exception
            Throw New Exception("Error @ExtractedTextNeedsTranslation" & vbNewLine & ex.Message)
        End Try
        Return bNeedTransation
    End Function

    Private Function ChecklangExist(ByVal str As String) As Boolean
        Select Case LCase(str)
            Case "en", "us", "en_us", "enus"
                Return True
            Case "fr", "fr_fr", "frfr"
                Return True
            Case "de_de", "de", "dede"
                Return True
            Case "es_es", "es", "eses"
                Return True
            Case "ja_jp", "ja", "jp", "jajp"
                Return True
            Case "ko_kr", "kr", "ko", "kokr"
                Return True
            Case "zh_cn", "zh", "cn", "zhcn"
                Return True
            Case "ru_ru", "ru", "ruru"
                Return True
            Case "pt_br", "pt", "br", "ptbr"
                Return True
            Case "it_it", "it", "itit"
                Return True
            Case Else
                Return False
        End Select
    End Function
#End Region

#Region "Step2"
    'Step 2
    Dim fileIntegratedCount As Integer
    Public Function LibNameToOtherLang(ByVal FileName As String, ByVal targetfilepath As String, ByVal TranslatedXliffPath As String) As Integer
        fileIntegratedCount = 0
        Try
            '1. Get csv file to Datatable
            Dim DT As New DataTable
            Dim objParser As New CsvParser

            DT = objParser.GetDataTabletFromCSVFile(FileName)

            'Step3 ReImport Translation back
            If ReImportTranslationToCsv(DT, TranslatedXliffPath) <> True Then
                Throw New Exception("Error Re-importing Translation to Csv!")
            End If

            '2. Get Libnames colunms
            Dim LibName_enUS_Col As Byte = 0
            Dim LibNameCol As New ArrayList

            For i As Integer = 0 To DT.Columns.Count - 1
                If Trim(LCase(DT.Columns(i).ColumnName)) = "libname.en_us" Then
                    LibName_enUS_Col = i
                ElseIf InStr(Trim(LCase(DT.Columns(i).ColumnName)), "libname") And Trim(LCase(DT.Columns(i).ColumnName)) <> "libname.en_us" Then
                    LibNameCol.Add(i)
                End If
            Next

            '3. Copy enUS to Ohter libName columns
            For i As Integer = 0 To DT.Rows.Count - 1
                For j As Integer = 0 To LibNameCol.Count - 1
                    DT.Rows(i).Item(LibNameCol(j)) = DT.Rows(i).Item(LibName_enUS_Col)
                Next
            Next

            '4. Save the Copied File in IntermediateCsv Folder
            'targetfilepath = Application.StartupPath & "\Tools\TranslatedCsv\"

            targetfilepath = targetfilepath & "complib.csv"

            If System.IO.File.Exists(targetfilepath) Then
                Return fileIntegratedCount
                Exit Function
            End If

            DT.Columns.RemoveAt(DT.Columns.Count - 1)

            Dim lines = New List(Of String)()
            Dim columnNames As String() = DT.Columns.Cast(Of DataColumn)().[Select](Function(column) column.ColumnName).ToArray()

            Dim header = String.Join(",", columnNames)
            lines.Add(header)

            Dim valueLines = DT.AsEnumerable().Cast(Of DataRow)().[Select](Function(row) String.Join(",", row.ItemArray.[Select](Function(o) """" + o.ToString() + """").ToArray()))
            lines.AddRange(valueLines)
            File.WriteAllLines(targetfilepath, lines, System.Text.Encoding.UTF8)

        Catch ex As Exception
            Throw New Exception("Error @LibNameToOtherLang" & vbNewLine & ex.Message)
        End Try
        fileIntegratedCount = 1
        Return fileIntegratedCount
    End Function
#End Region

#Region "Step3"
    'Step 3
    Public Function ReImportTranslationToCsv(ByRef DT As DataTable, ByVal TranslatedXliffPath As String) As Boolean
        Try
            Dim LangList As New Dictionary(Of String, Integer)

            Dim TotalCompLibXliff As New ArrayList
            Dim TotalPremiumXliff As New ArrayList

            'Get List of files from xliff folder
            For Each File In My.Computer.FileSystem.GetFiles(TranslatedXliffPath)
                If Path.GetExtension(File.ToString) = ".xliff" Then
                    If InStr(File.ToString.ToLower, "competency-lib") > 0 Then
                        TotalCompLibXliff.Add(File.ToString)
                    Else
                        TotalPremiumXliff.Add(File.ToString)
                    End If
                    Try
                        LangList.Add(Right(Path.GetFileNameWithoutExtension(File.ToString), 2), 0)
                    Catch ex As System.ArgumentException
                        'Do nothing for duplicates
                    Catch ex As Exception
                        Throw New Exception(ex.Message)
                    End Try

                End If
            Next

            If TotalCompLibXliff.Count = 0 And TotalPremiumXliff.Count = 0 Then
                Throw New Exception("Error no xliff files found in: " & vbNewLine & Application.StartupPath & "\Tools\xliffs\")
            End If

            Dim Fields As New ArrayList
            Fields.Add("name")
            Fields.Add("category")
            Fields.Add("description")

            'Per lang perfrom translation in DT
            For i As Integer = 0 To LangList.Count - 1

                'Assign Files to object for lang and Load xliff
                Dim ComplibFile As String = ""
                Dim PremiumFile As String = ""
                For m As Integer = 0 To TotalCompLibXliff.Count - 1
                    If Right(Path.GetFileNameWithoutExtension(TotalCompLibXliff(m)), 2) = LangList.Keys(i) Then
                        ComplibFile = TotalCompLibXliff(m)
                        Exit For
                    End If
                Next
                For n As Integer = 0 To TotalPremiumXliff.Count - 1
                    If Right(Path.GetFileNameWithoutExtension(TotalPremiumXliff(n)), 2) = LangList.Keys(i) Then
                        PremiumFile = TotalPremiumXliff(n)
                        Exit For
                    End If
                Next

                Dim objComplib As sXliff = Nothing
                Dim objPremium As sXliff = Nothing

                If ComplibFile <> "" Then
                    objComplib = load_xliff(ComplibFile)
                End If

                If PremiumFile <> "" Then
                    objPremium = load_xliff(PremiumFile)
                End If

                Dim col_Target As Integer = -1
                Dim col_enUs As Integer = -1

                For j As Integer = 0 To Fields.Count - 1
                    col_enUs = GetCol_FromDT(DT, Fields(j), "en_us")
                    col_Target = GetCol_FromDT(DT, Fields(j), LangList.Keys(i))

                    If col_enUs = -1 Then
                        Throw New Exception("en_US column not found in csv file!" & vbNewLine & "Cannot proceed")
                    End If

                    If col_Target = -1 Then
                        Throw New Exception(LangList.Keys(i) & " column not found in csv file!" & vbNewLine & "Cannot proceed")
                    End If

                    For x As Integer = 0 To DT.Rows.Count - 1
                        If DT.Rows(x).Item(col_enUs).ToString <> "" And DT.Rows(x).Item(col_Target).ToString = "" Then
                            If ComplibFile <> "" Then
                                DT.Rows(x).Item(col_Target) = (GetTranslation(objComplib, DT.Rows(x).Item(col_enUs)))
                            End If
                            If DT.Rows(x).Item(col_Target).ToString = "" Then
                                If PremiumFile <> "" Then
                                    DT.Rows(x).Item(col_Target) = (GetTranslation(objPremium, DT.Rows(x).Item(col_enUs)))
                                End If
                            End If
                        End If
                    Next
                Next
            Next

        Catch ex As Exception
            Throw New Exception("Error @ReImportTranslationToCsv" & vbNewLine & ex.Message)
        End Try
        Return True
    End Function
#End Region

    Function GetTranslation(ByVal objxliff As sXliff, ByVal searchStr As String) As String
        Dim str As String = ""
        For i As Integer = 0 To objxliff.Source.Count - 1
            If GetPlainText(objxliff.Source(i).ToString.ToLower.Trim) = GetPlainText(searchStr.ToLower.Trim) Then
                str = revert_xml(objxliff.Translation(i))
                Exit For
            End If
        Next
        Return str
    End Function

    Function GetCol_FromDT(ByVal DT As DataTable, ByVal Field As String, ByVal lang As String) As Integer
        Dim _lang As String = GetLong_lang(lang)
        lang = GetShort_lang(lang)
        Dim col As Integer = -1
        For i As Integer = 0 To DT.Columns.Count - 1
            If DT.Columns(i).ColumnName.ToString.ToLower.Trim = Field & "." & lang.ToString.ToLower.Trim Or DT.Columns(i).ColumnName.ToString.ToLower.Trim = Field & "." & _lang.ToString.ToLower.Trim Then
                col = i
                Exit For
            End If
        Next
        Return col
    End Function

End Module


Public Class Tags
    Public Source As New ArrayList
    Public Guid As New ArrayList
End Class