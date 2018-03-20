'RMK JOB CATEGORIES TRANSLATION

'File type: xml multilanguage, but you cannot generate the translated categories. They will be present in the input file.
'Prerequisite: the job categories to translate will be duplicated so that the ID is generated. The job category name will then be renamed to Xxxx (yyYY).

'EXTRACTION
'What fields need to be translated?
'Tags: name, metakey, metatitle, metadesc, keyword group
'Category locale of translated  must be adapted, it will always be en_US (note: Lynette changed the lang code, already. Normally, it would be not the case. Only the name would be adapted with langcode, not the locale. Overwrite with the locale anyway (even if it won’t change anything).
'The name of translated categories will be the same as the en_US one, but the langcode (5chars) will be added to the name, so that you can distinguish them.

'Extraction is quite straightforward. I would recommend to go through all of the categories and check if any has a language code in bracket at the end of the name. In that, case, you extract into xliff, in the given language, if it’s part of the current language project.Issue a warning in a log, if you detect a name with a language code which is not in the project. E.g. you have admin job fr_FR, but you see fr_FR is not a language of the current project. You can detect based on _ and you check if there are 2 chars before and 2 chars afterwards.

'Also issue (non blocking) error if there are missing job categories with language code. E.g. you have ABC, ABC fr_FR, but in your project definition you have also de_DE, but you don’t find ABC de_DE.

'Finally, issue warning in the case a root (enUS) job category has no corresponding translated job categories at all. This is normal, some don’t need translations as they won’t be activated in the system.

'Difference between this and previous warning is that in the previous, there is at least one corresponding category with language code.

'For translating the name itself, remove the language code before adding in the transunit. (e.g. « administration job fr_FR » -> you send only « administration job »).



'REINTEGRATION
'Search for the category name + langcode. Replace the content in the tags (overwrite them, including the category name) and modify the category locale.

'Pass a test to ensure there are no 2 times the same job category name after translation (otherwise import will fail.). If you detect that, issue a warning with the name of the duplicate category name. We’ll have to fix manually.

'Also, it’s required to update the catgrouptocategories.

'This contains the categorynames. As you’re sending the job category name without language code, be sure to search with language code, to make sure you replace the right ones.

'<catgroup catgroupmetaname="eng2" categoryname="Project Management Jobs de_DE" />
'<catgroup catgroupmetaname="hr1" categoryname="Recruiting Jobs de_DE" />
'<catgroup catgroupmetaname="ops6" categoryname="Plant Operations Jobs de_DE" />
'<catgroup catgroupmetaname="hr1" categoryname="Benefits Jobs de_DE" />

'Would appear:
'<catgroup catgroupmetaname="eng2" categoryname="Projektmanagement Arbeit" />


Imports System.Net
Imports System.Xml
Imports System.ComponentModel
Imports System.Text
Imports System.IO

Module Mod_RMK_jobCategory

    Private Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As Form_MainNew.RtbColor)
        Dim str As New ArrayList
        str.Add(Msg)
        str.Add(MyColor)
        _bw.ReportProgress(4, str)
    End Sub

    Private _bw As BackgroundWorker

    Class RMK_JobCategory

        'Public MyFields() As String
        'Public Sub New()
        '    Dim MyStr As String = "id,display,sortorder,metakey,metatitle,metadesc,headertext,isDefault,keywordgroupid,keywordgroup,locationgroupid,locationgroup,categorylocale,jobTitle,SEOMarket,stateAbbr,city,countryCode,businessunit,recruiterID,segmentID,buildTypeID,brand,skin,"
        '    MyFields = Split(MyStr, ",")
        'End Sub

        Public CategoryName As String
        Public id As String
        Public display As String
        Public sortorder As String
        Public metakey As String
        Public metatitle As String
        Public metadesc As String
        Public headertext As String
        Public isDefault As String
        Public keywordgroupid As String
        Public keywordgroup As String
        Public locationgroupid As String
        Public locationgroup As String
        Public categorylocale As String
        Public jobTitle As String
        Public SEOMarket As String
        Public stateAbbr As String
        Public city As String
        Public countryCode As String
        Public businessunit As String
        Public recruiterID As String
        Public segmentID As String
        Public buildTypeID As String
        Public brand As String
        Public skin As String
    End Class

#Region "RMK Job Category Extraction xliff"

    Public Sub WarningMessage_ExtraLanguageInJobcategory(ByVal Rmk_XmlFile As String, ByVal LangList() As String, ByRef bw As BackgroundWorker)
        _bw = bw
        'Issue a warning in a log, if you detect a name with a language code which is not in the project. 
        'E.g. you have admin job fr_FR, but you see fr_FR is not a language of the current project. 
        'You can detect based on _ and you check if there are 2 chars before and 2 chars afterwards.

        Dim xmlcontent As String
        Dim xd As New Xml.XmlDocument

        xmlcontent = dbletags_xml(System.IO.File.ReadAllText(Rmk_XmlFile))
        xmlcontent = WebUtility.HtmlDecode(xmlcontent) ' remove the encoding for e.g. copyright sign

        xd.XmlResolver = Nothing
        xd.LoadXml(xmlcontent) 'NEED TO use try and catch the error. IF there is a xml error, an exception is raised here.

        Dim xlist As XmlNodeList = xd.GetElementsByTagName("category")

        Dim bFound As Boolean = False
        For i As Integer = 0 To xlist.Count - 1
            Dim Catname As String = Right(xlist(i).Attributes(0).Value.Trim, 5)
            For j As Integer = 0 To UBound(LangList)
                If Mid(Catname, 3, 1) = "_" Then
                    If Catname.ToLower = LangList(j).ToLower Then
                        bFound = True
                        Exit For
                    End If
                Else
                    bFound = True 'will be enUS, as root element will not have any lang code, it belongs to enUS
                End If
            Next
            If Not bFound Then
                UpdateMsg(Now & Chr(9) & "Detected a Category-name '" & xlist(i).Attributes(0).Value & "', the language code is not in the project." & vbCrLf, Form_MainNew.RtbColor.Black)
            End If
            bFound = False
        Next
    End Sub

    Public Function Rmk_JobCategory_Xml_to_Xliff(ByVal RmkFile As String, ByVal targetxliff_savePath As String, ByVal TargetLang As String, ByVal Langlist() As String, ByRef bw As BackgroundWorker) As Boolean
        _bw = bw
        Dim xmlcontent As String
        Dim xd As New Xml.XmlDocument

        xmlcontent = dbletags_xml(System.IO.File.ReadAllText(RmkFile))
        xmlcontent = WebUtility.HtmlDecode(xmlcontent) ' remove the encoding for e.g. copyright sign

        xd.XmlResolver = Nothing
        Try
            xd.LoadXml(xmlcontent) 'NEED TO use try and catch the error. IF there is a xml error, an exception is raised here.
        Catch ex As Exception
            Throw New Exception("Error Loading xml file - " & System.IO.Path.GetFileName(RmkFile) & vbNewLine & ex.Message)
        End Try

        Dim objXliff As New sXliff
        objXliff.ID = New ArrayList
        objXliff.Note = New ArrayList
        objXliff.Resname = New ArrayList
        objXliff.Source = New ArrayList
        objXliff.TargetLang = TargetLang
        objXliff.Translation = New ArrayList

        Try
            Dim xlist As XmlNodeList = xd.GetElementsByTagName("category")

            Dim objRmkJobCat As RMK_JobCategory = Nothing

            Dim bFound As Boolean = False

            Dim enUSRootIndex As Integer = 0

            Dim Counter As Integer = 0
            For i As Integer = 0 To xlist.Count - 1
                Dim Catname As String = xlist(i).Attributes(0).Value
                If isEnUsOrTargetLangAvailableForCategoryName(Right(Catname.Trim, 5).ToLower) Then 'Get enUS Root Id.
                    objRmkJobCat = ExtractContent(xlist, TargetLang, i, Langlist)

                    With objRmkJobCat
                        UpdateXliffData(.CategoryName, .CategoryName, .id, objXliff, "name")
                        If Not .metatitle.ToString.Trim = String.Empty Then
                            UpdateXliffData(.metatitle, .CategoryName, .id, objXliff, "metatitle")
                        End If
                        If Not .metakey.ToString.Trim = String.Empty Then
                            UpdateXliffData(.metakey, .CategoryName, .id, objXliff, "metakey")
                        End If
                        If Not .metadesc.ToString.Trim = String.Empty Then
                            UpdateXliffData(.metadesc, .CategoryName, .id, objXliff, "metadesc")
                        End If
                        If Not .keywordgroup = String.Empty Then
                            UpdateXliffData(.keywordgroup, .CategoryName, .id, objXliff, "keywordgroup")
                        End If
                        If Not .headertext = String.Empty Then
                            UpdateXliffData(.headertext, .CategoryName, .id, objXliff, "headertext")
                        End If
                    End With
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error @Rmk_JobCategory_to_Xliff " & ex.Message)
        End Try

        Try
            'Write to Xliff File
            CreateXliff(objXliff, targetxliff_savePath, TargetLang)
        Catch ex As Exception
            Throw New Exception("Error creating Rmk xliff file " & ex.Message)
        End Try

        Return True
    End Function

    Private Function CreateXliff(ByRef objXliff As sXliff, ByVal xliff_Path As String, ByVal Targetlanguage As String) As String

        Try
            Dim myNum As Integer = 0
            Using Writer As System.IO.StreamWriter = New System.IO.StreamWriter(xliff_Path, False, System.Text.Encoding.UTF8)
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


                For J As Integer = 0 To objXliff.ID.Count - 1
                    myNum += 1
                    Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & objXliff.ID(J) & Chr(34) & " resname=" & Chr(34) & clean_xml(objXliff.Resname(J)) & Chr(34) & ">")
                    'Writer.WriteLine("<trans-unit id=" & Chr(34) & clean_xml(RD.ID(J).ToString) & Chr(34) & " resname=" & Chr(34) & clean_xml(RD.resName(J).ToString) & Chr(34) & ">")
                    Writer.WriteLine("<source>" & wrap_html(clean_xml(objXliff.Source(J).ToString)) & "</source>")
                    If IsNumeric(objXliff.Source(J).ToString) Then
                        Writer.WriteLine("<target state=""needs-review-translation"">" & objXliff.Source(J) & "</target>")
                    Else
                        Writer.WriteLine("<target state=""needs-review-translation""></target>")
                    End If
                    Writer.WriteLine("<note from=""Developer"" priority =""10"">RMK</note>")
                    Writer.WriteLine("</trans-unit>")
                    Writer.WriteLine(vbCrLf)
                Next

                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

            End Using

            If myNum = 0 Then
                System.IO.File.Delete(xliff_Path)
                Return " Already has translation for " & Replace(Targetlanguage, "-", "_")
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return ""

    End Function

    Private Sub UpdateXliffData(ByVal enUS As String, ByVal CategoryName As String, ByVal Id As String, ByRef objxliff As sXliff, ByVal resName As String)
        Try
            With objxliff
                If Not .Source.Contains(enUS) Then
                    .ID.Add(CategoryName)
                    .Source.Add(enUS)
                    .Note.Add("RMK-JobCategory")
                    .Resname.Add(resName)
                End If
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Function ExtractContent(ByRef xlist As XmlNodeList, ByRef TargetLang As String, ByVal enUS_Index As Integer, ByRef Langlist() As String) As RMK_JobCategory
        Dim objRmkJobCat As New RMK_JobCategory

        Dim Catname As String = xlist(enUS_Index).Attributes(0).Value

        Dim bFound_targetLang As Boolean = False
        For i As Integer = 0 To xlist.Count - 1
            'Issue warning in the case a root (enUS) job category has no corresponding translated job categories at all. 
            'This is normal, some don’t need translations as they won’t be activated in the system.
            If xlist(i).Attributes(0).Value.ToLower.Trim = Catname.ToLower.Trim & " " & TargetLang.ToLower.Trim Then
                bFound_targetLang = True
                Exit For
            End If
        Next

        If Not bFound_targetLang Then
            Dim bFound_ProjectLang As Boolean = False
            For i As Integer = 0 To UBound(Langlist)
                'Also issue (non blocking) error if there are missing job categories with language code. 
                'E.g. you have ABC, ABC fr_FR, but in your project definition you have also de_DE, but you don’t find ABC de_DE.
                For j As Integer = 0 To xlist.Count - 1
                    For x As Integer = 0 To xlist(j).Attributes.Count - 1
                        If xlist(j).Attributes(x).Value.ToLower.Trim = Catname.ToLower.Trim & " " & Langlist(i).ToLower.Trim Then
                            bFound_ProjectLang = True
                            Exit For
                        End If
                    Next
                    If bFound_ProjectLang Then
                        Exit For
                    End If
                Next
                If bFound_ProjectLang Then
                    Exit For
                End If
            Next

            If bFound_ProjectLang Then
                'Raise another event
                UpdateMsg(Now & Chr(9) & "Target Language '" & TargetLang & "' missing for Category Name - '" & Catname & "' but available for other language" & vbCrLf, Form_MainNew.RtbColor.Red)
            Else
                'Raise event StrongWarningMsg( TargetLang & " language is not available for Category Name - " & Catname,"RED" )
                UpdateMsg(Now & Chr(9) & "Target Language " & TargetLang & " language is not available for Category Name - " & Catname & vbCrLf, Form_MainNew.RtbColor.Red)
            End If

        End If

        With objRmkJobCat
            .CategoryName = xlist(enUS_Index).Attributes(0).Value
            For j As Integer = 0 To xlist(enUS_Index).Attributes.Count - 1
                Select Case xlist(enUS_Index).Attributes(j).Name.ToLower
                    Case "id"
                        .id = xlist(enUS_Index).Attributes(j).Value
                    Case "display"
                        .display = xlist(enUS_Index).Attributes(j).Value
                    Case "sortorder"
                        .sortorder = xlist(enUS_Index).Attributes(j).Value
                    Case "metakey"
                        .metakey = xlist(enUS_Index).Attributes(j).Value
                    Case "metatitle"
                        .metatitle = xlist(enUS_Index).Attributes(j).Value
                    Case "metadesc"
                        .metadesc = xlist(enUS_Index).Attributes(j).Value
                    Case "headertext"
                        .headertext = xlist(enUS_Index).Attributes(j).Value
                    Case "isDefault"
                        .isDefault = xlist(enUS_Index).Attributes(j).Value
                    Case "keywordgroupid"
                        .keywordgroupid = xlist(enUS_Index).Attributes(j).Value
                    Case "keywordgroup"
                        .keywordgroup = xlist(enUS_Index).Attributes(j).Value
                    Case "locationgroupid"
                        .locationgroupid = xlist(enUS_Index).Attributes(j).Value
                    Case "locationgroup"
                        .locationgroup = xlist(enUS_Index).Attributes(j).Value
                    Case "categorylocale"
                        .categorylocale = xlist(enUS_Index).Attributes(j).Value
                    Case "jobTitle"
                        .jobTitle = xlist(enUS_Index).Attributes(j).Value
                    Case "SEOMarket"
                        .SEOMarket = xlist(enUS_Index).Attributes(j).Value
                    Case "stateAbbr"
                        .stateAbbr = xlist(enUS_Index).Attributes(j).Value
                    Case "city"
                        .city = xlist(enUS_Index).Attributes(j).Value
                    Case "countryCode"
                        .countryCode = xlist(enUS_Index).Attributes(j).Value
                    Case "businessunit"
                        .businessunit = xlist(enUS_Index).Attributes(j).Value
                    Case "recruiterID"
                        .recruiterID = xlist(enUS_Index).Attributes(j).Value
                    Case "segmentID"
                        .segmentID = xlist(enUS_Index).Attributes(j).Value
                    Case "buildTypeID"
                        .buildTypeID = xlist(enUS_Index).Attributes(j).Value
                    Case "brand"
                        .brand = xlist(enUS_Index).Attributes(j).Value
                    Case "skin"
                        .skin = xlist(enUS_Index).Attributes(j).Value
                End Select
            Next
        End With

        Return objRmkJobCat
    End Function

    Private Function isEnUsOrTargetLangAvailableForCategoryName(ByVal CatName As String, Optional ByVal targetLang As String = "en_US") As Boolean

        Try
            LangDefintion.GetLanguageList()
            For i As Integer = 0 To LanguageDefination.LangFiveChars.Count - 1
                Dim lang As String = LanguageDefination.LangFiveChars(i).ToString.Replace("-", "_").ToLower.Trim
                lang = lang.Replace("(spain) ", "") 'exception with spainish lang
                If CatName.ToLower.Trim = lang Then
                    Return False
                End If
            Next

        Catch ex As Exception
            Throw New Exception("Error @GetEnusCatId " & ex.Message)
        End Try
        If targetLang = "en_US" Then
            Return True
        End If
        Return False
    End Function

#End Region

#Region "RMK Reintegration"
    Public Sub Rmk_JobCategory_Xliff_to_Xml(ByVal sRmkXmlfile As String, ByVal sTransaltedXliff As String, ByVal TargetLang As String, ByRef bw As BackgroundWorker, ByVal bRemoveEnus As Boolean)
        _bw = bw

        Dim strMsg As New StringBuilder
        Dim xd As New Xml.XmlDocument
        Try
            'Load Translated xliff file.
            Dim objXliffData As sXliff = load_xliff(sTransaltedXliff)

            'Load Xml Input file
            Dim xmlcontent As String

            xmlcontent = dbletags_xml(System.IO.File.ReadAllText(sRmkXmlfile))
            xmlcontent = WebUtility.HtmlDecode(xmlcontent) ' remove the encoding for e.g. copyright sign

            xd.XmlResolver = Nothing
            Try
                'xd.PreserveWhitespace = True
                xd.LoadXml(xmlcontent) 'NEED TO use try and catch the error. IF there is a xml error, an exception is raised here.
            Catch ex As Exception
                Throw New Exception("Error Loading xml file - " & System.IO.Path.GetFileName(sRmkXmlfile) & vbNewLine & ex.Message)
            End Try

            Dim xlist As XmlNodeList = xd.GetElementsByTagName("category")
            Dim objRmkJobCat As New RMK_JobCategory
            Dim enUS_Index As New ArrayList

            'Replace translation in Category ########################################################################################################################
            For i As Integer = 0 To xlist.Count - 1
                For j As Integer = 0 To xlist(i).Attributes.Count - 1
                    If xlist(i).Attributes(j).Name.ToLower = "name" Then
                        Dim Catname As String = Right(xlist(i).Attributes(j).Value.Trim, 5)
                        If Mid(Catname, 3, 1) = "_" Then
                            If Catname.ToLower = TargetLang.ToLower Then
                                ReadSingleNode(xlist(i), objXliffData, TargetLang, xlist, strMsg)
                            End If
                        Else
                            'will be enUS, as root element will not have any lang code, it belongs to enUS
                            enUS_Index.Add(i)
                        End If
                        Exit For
                    End If
                Next
            Next '###################################################################################################################################################

            'For 18.2, would it be possible to add a step to remove all enUS entries in the xml once translation is done? Consultants asked for that. 
            'If you don’t mind, make sure it’s easy to deactivate, as I have some doubts this is really required. 
            'I don 't think it would take you a lot of time and most probably it would take less than if I have to manually remove the enUS.
            If bRemoveEnus Then
                UpdateMsg(Now & Chr(9) & "Removing enUS nodes...." & vbCrLf, Form_MainNew.RtbColor.Black)
                RemoveEnUS(xlist, enUS_Index) 'Comment it if dont need to delete enUS content
            End If

            xlist = xd.GetElementsByTagName("catgroupToCategories")
            Dim result As String = ""
            'Replace translation in Catgroup########################################################################################################################
            For i As Integer = 0 To xlist(0).ChildNodes.Count - 1
                If Not IsNothing(xlist(0).ChildNodes(i).Attributes) Then
                    For j As Integer = 0 To xlist(0).ChildNodes(i).Attributes.Count - 1
                        If xlist(0).ChildNodes(i).Attributes(j).Name.ToLower = "categoryname" Then
                            Dim Catname As String = Right(xlist(0).ChildNodes(i).Attributes(j).Value.Trim, 5)
                            If Mid(Catname, 3, 1) = "_" Then
                                If Catname.ToLower = TargetLang.ToLower Then
                                    result = Find_and_ReplaceRMKContent(objXliffData, xlist(0).ChildNodes(i).Attributes(j).Value.Replace(TargetLang, ""), False, Nothing)
                                    If result.Trim <> String.Empty Then
                                        xlist(0).ChildNodes(i).Attributes(j).Value = result
                                    End If
                                End If
                            End If
                            Exit For
                        End If
                    Next
                End If

            Next '###################################################################################################################################################

            Beautify(xd, sRmkXmlfile)

            UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(sRmkXmlfile) & " out file created." & vbCrLf, Form_MainNew.RtbColor.Black)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub Beautify(doc As XmlDocument, ByVal sFile As String)
        Dim settings As New XmlWriterSettings()

        With settings
            .Indent = True
            .IndentChars = "    "
            .NewLineChars = vbCr & vbLf
            .NewLineHandling = NewLineHandling.Replace
        End With

        Using writer As XmlWriter = XmlWriter.Create(sFile, settings)
            doc.Save(writer)
        End Using

    End Sub

    Private Sub RemoveEnUS(ByRef xlist As XmlNodeList, ByVal enUS_Index As ArrayList)
        Try
            Dim x As Integer = enUS_Index.Count - 1
            Do Until x = -1
                Dim xNode As XmlNode = xlist(enUS_Index(x))
                xNode.ParentNode.RemoveChild(xNode)
                x -= 1
            Loop
        Catch ex As Exception
            Throw New Exception("Error @RemoveEnUS - " & ex.Message)
        End Try

    End Sub

    Private Sub ReadSingleNode(ByRef CategoryNode As XmlNode, ByRef objXLiff As sXliff, ByVal TargetLang As String, ByRef xlist As XmlNodeList, ByRef strmsg As StringBuilder)
        Try
            Dim CatName As String = ""
            Dim result As String
            For j As Integer = 0 To CategoryNode.Attributes.Count - 1
                result = ""
                Select Case CategoryNode.Attributes(j).Name.ToLower
                    Case "metakey"
                        result = Find_and_ReplaceRMKContent(objXLiff, CategoryNode.Attributes(j).Value, False, Nothing)
                        If result.Trim <> String.Empty Then
                            CategoryNode.Attributes(j).Value = result
                        End If
                    Case "metatitle"
                        result = Find_and_ReplaceRMKContent(objXLiff, CategoryNode.Attributes(j).Value, False, Nothing)
                        If result.Trim <> String.Empty Then
                            CategoryNode.Attributes(j).Value = result
                        End If
                    Case "metadesc"
                        result = Find_and_ReplaceRMKContent(objXLiff, CategoryNode.Attributes(j).Value, False, Nothing)
                        If result.Trim <> String.Empty Then
                            CategoryNode.Attributes(j).Value = result
                        End If
                    Case "headertext"
                        result = Find_and_ReplaceRMKContent(objXLiff, CategoryNode.Attributes(j).Value, True, Replace(CategoryNode.Attributes(0).Value, TargetLang, ""))
                        If result.Trim <> String.Empty Then
                            CategoryNode.Attributes(j).Value = result
                        End If
                    Case "keywordgroup"
                        result = Find_and_ReplaceRMKContent(objXLiff, CategoryNode.Attributes(j).Value, False, Nothing)
                        If result.Trim <> String.Empty Then
                            CategoryNode.Attributes(j).Value = result
                        End If
                    Case "categorylocale"
                        CategoryNode.Attributes(j).Value = TargetLang
                        'Case "id" 'As of now these are extra items which doesnot need translation, may be going forward it might require, keep it
                        'Case "display"
                        'Case "sortorder"
                        'Case "isDefault"
                        'Case "keywordgroupid"
                        'Case "locationgroupid"
                        'Case "locationgroup"
                        'Case "jobTitle"
                        'Case "SEOMarket"
                        'Case "stateAbbr"
                        'Case "city"
                        'Case "countryCode"
                        'Case "businessunit"
                        'Case "recruiterID"
                        'Case "segmentID"
                        'Case "buildTypeID"
                        'Case "brand"
                        'Case "skin"
                End Select
            Next

            For j As Integer = 0 To CategoryNode.Attributes.Count - 1 'This is in another step to get header text translation, if we use it in avove loop, it will replace category name and hence header will not match
                result = ""
                CatName = ""
                Select Case CategoryNode.Attributes(j).Name.ToLower
                    Case "name"
                        CatName = CategoryNode.Attributes(j).Value
                        result = Find_and_ReplaceRMKContent(objXLiff, CategoryNode.Attributes(j).Value.Replace(TargetLang, ""), False, Nothing) ' & " " & TargetLang
                        If result.Trim <> TargetLang Then
                            CategoryNode.Attributes(j).Value = result
                        End If
                End Select
            Next j

            Dim counter As Integer = 0
            For i As Integer = 0 To xlist.Count - 1
                ' Pass a test to ensure there are no 2 times the same job category name after translation (otherwise import will fail.). 
                'If you detect that, issue a warning with the name of the duplicate category name. We’ll have to fix manually.
                If xlist(i).Attributes(0).Value.ToLower.Trim = CatName.ToLower.Trim Then
                    counter += 1
                End If
            Next

            If counter >= 2 Then
                UpdateMsg(Now & Chr(9) & "RMK-Xml CategoryName - '" & CategoryNode.Value & "' repeated " & counter & " times" & vbCrLf, Form_MainNew.RtbColor.Red)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


    End Sub

    Private Function Find_and_ReplaceRMKContent(ByRef objxliff As sXliff, ByVal enUS As String, ByVal bHeader As Boolean, ByVal transId As String) As String

        Dim sTranslation As String = ""

        Try
            Dim MyString As String

            If bHeader Then
                For i As Integer = 0 To objxliff.Source.Count - 1
                    MyString = Mid(objxliff.ID(i), InStr(objxliff.ID(i), "_") + 1, Len(objxliff.ID(i)))
                    If objxliff.Resname(i).ToString.ToLower = "headertext" And MyString.ToLower.Trim = transId.ToLower.Trim Then
                        sTranslation = objxliff.Translation(i)
                        Exit For
                    End If
                Next
            Else
                Dim cleanUs As String = Replace(enUS, " ", String.Empty)
                If IsNothing(cleanUs) Then
                    cleanUs = ""
                End If
                For i As Integer = 0 To objxliff.Source.Count - 1
                    MyString = Replace(objxliff.Source(i), " ", String.Empty)
                    If cleanUs.ToLower = MyString.ToLower Or enUS.ToLower = objxliff.Source(i).ToString.ToLower Then
                        sTranslation = objxliff.Translation(i)
                        Exit For
                    End If
                Next
            End If

            If sTranslation.Trim = String.Empty Then
                For i As Integer = 0 To objxliff.Source.Count - 1
                    If GetPlainText(enUS.ToLower) = GetPlainText(objxliff.Source(i).ToString.ToLower) Then
                        Return objxliff.Translation(i)
                    End If
                Next
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return sTranslation

    End Function
#End Region

End Module
