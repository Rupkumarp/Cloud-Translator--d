'Imports CloudTranslator
Imports System.Xml
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Xml.Serialization

Module Module1

    Sub Main()
        Try

            'RMK_Extraction()

            'ObjectId.GetParentID("C:\Users\C5195092\Desktop\ClouTest\Run01\01-Input\7.3.4.xml")

            'LangDefintion.GetLanguageList()
            'Impex
            'HybrisImpex.CreateXliff("", "", "de_DE", "C:\Users\C5195092\Desktop\SfTest\HybrisTest\02-TobeTranslated\hybriss.xliff")
            'HybrisImpex.xliff_To_Impex("", "", "es_ES", "C:\Users\C5195092\Desktop\SfTest\HybrisTest\03-Backfromtranslation\cms-content_ES.xliff")

            'Hybris Xml
            'HybrisXml.CreateXliff("", "C:\Users\C5195092\Desktop\SfTest\HybrisTest\02-TobeTranslated\public-sector-educational-grants-wizard_deDE.xliff", "es")
            ''HybrisXml.CreateHybrisXML("C:\Users\C5195092\Desktop\SfTest\HybrisTest\01-Input-B\public-sector-foebis-new.xml" _
            '                            , "C:\Users\C5195092\Desktop\SfTest\HybrisTest\03-Backfromtranslation\public-sector-educational-grants-wizard_esES.xliff" _
            '                            , TranslationType.Multilingual, "es")

            'Hybris Properties
            'HybrisProperties.CreateXliff("C:\Users\C5195092\Desktop\SfTest\HybrisTest\01-Input-B\appointments-locales_en.properties" _
            ', "C:\Users\C5195092\Desktop\SfTest\HybrisTest\01-Input-B\appointments-locales_fr.properties" _
            ', "de", "C:\Users\C5195092\Desktop\SfTest\HybrisTest\02-TobeTranslated\appointments-locales_de.xliff")
            'HybrisProperties.CreatePropertiesBack("C:\Users\C5195092\Desktop\SfTest\HybrisTest\01-Input-B\appointments-locales_en.properties" _
            '                                      , "C:\Users\C5195092\Desktop\SfTest\HybrisTest\01-Input-B\appointments-locales_en.properties" _
            '                                      , "es", "C:\Users\C5195092\Desktop\SfTest\HybrisTest\03-Backfromtranslation\appointments-locales_es.xliff")

            'RMK JObCategory
            'RMKJobCategory()

            'onboarding offboarding

            'Dim sFile As String = "C:\Users\C5195092\Desktop\ClouTest\OnboardingOffboarding\01-Input\OffboardingApprove_Localization.xml"
            'Dim xliff As String = "C:\Users\C5195092\Desktop\ClouTest\OnboardingOffboarding\01-Input-B\OffboardingApprove_Localization.xliff"
            'Extract(sFile, xliff)

            'Project Details load

            ' cross_form_functions.GetProjectGroupInfo()


            'Get Max length
            '

            'ClsMaxLength.LoadMaxLength("2.3.23.1")

            Dim str As String = File.ReadAllText("C:\Users\C5195092\Desktop\ogg\enUS_ruRU_S_000028-00001.xlf")
            Dim str1 As String = "C:\Users\C5195092\Desktop\ogg\test.xliff"

            Dim interfaces As xlf

            Dim serializer As New XmlSerializer(GetType(xlf))
            Using reader As TextReader = New StringReader(str)
                interfaces = serializer.Deserialize(reader)
            End Using

            Dim writer As StreamWriter = New StreamWriter(str1)
            Dim xwriter As XmlSerializer = New XmlSerializer(GetType(xlf))
            xwriter.Serialize(writer, interfaces)
            writer.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    '#Region "RMK Job Category"
    '    Sub RMKJobCategory()
    '        Try
    '            Dim sFile As String = "C:\Users\C5195092\Desktop\ClouTest\Rmk_JobCategory\05-Output\18.2.1.xml"
    '            Dim sXLiff As String = "C:\Users\C5195092\Desktop\ClouTest\Rmk_JobCategory\02-TobeTranslated\Best_Run_category_export with languagesxml.xliff"

    '            'Rmk_JobCategory_to_Xliff(sFile, sXLiff, "fr_FR", {"ru_RU", "fr_FR", "de_DE", "es_ES"})


    '            Dim sTransaltedXliff As String = "C:\Users\C5195092\Desktop\ClouTest\Rmk_JobCategory\03-Backfromtranslation\18.2.1_fr_FR.xliff"

    '            Dim sRmkOutFile As String = "C:\Users\C5195092\Desktop\ClouTest\Rmk_JobCategory\05-Output\18.2.1.xml"

    '            Rmk_JobCategory_to_Xml(sFile, sTransaltedXliff, "fr_FR", sRmkOutFile)

    '        Catch ex As Exception
    '            Throw New Exception(ex.Message)
    '        End Try
    '    End Sub

    '    Sub Rmk_JobCategory_to_Xml(ByVal sRmkXmlfile As String, ByVal sTransaltedXliff As String, ByVal TargetLang As String, ByVal sSaveRmkOutFile As String)

    '        Dim strMsg As New StringBuilder
    '        Dim xd As New Xml.XmlDocument
    '        Try
    '            'Load Translated xliff file.
    '            Dim objXliffData As sXliff = load_xliff(sTransaltedXliff)

    '            'Load Xml Input file
    '            Dim xmlcontent As String


    '            xmlcontent = dbletags_xml(System.IO.File.ReadAllText(sRmkXmlfile))
    '            xmlcontent = WebUtility.HtmlDecode(xmlcontent) ' remove the encoding for e.g. copyright sign

    '            xd.XmlResolver = Nothing
    '            Try
    '                xd.LoadXml(xmlcontent) 'NEED TO use try and catch the error. IF there is a xml error, an exception is raised here.
    '            Catch ex As Exception
    '                Throw New Exception("Error Loading xml file - " & System.IO.Path.GetFileName(sRmkXmlfile) & vbNewLine & ex.Message)
    '            End Try

    '            Dim xlist As XmlNodeList = xd.GetElementsByTagName("category")
    '            Dim objRmkJobCat As New RMK_JobCategory
    '            Dim enUS_Index As New ArrayList

    '            'Replace translation in Category ########################################################################################################################
    '            For i As Integer = 0 To xlist.Count - 1
    '                For j As Integer = 0 To xlist(i).Attributes.Count - 1
    '                    If xlist(i).Attributes(j).Name.ToLower = "name" Then
    '                        Dim Catname As String = Right(xlist(i).Attributes(j).Value.Trim, 5)
    '                        If Mid(Catname, 3, 1) = "_" Then
    '                            If Catname.ToLower = TargetLang.ToLower Then
    '                                ReadSingleNode(xlist(i), objXliffData, TargetLang, xlist, strMsg)
    '                            End If
    '                        Else
    '                            'will be enUS, as root element will not have any lang code, it belongs to enUS
    '                            enUS_Index.Add(i)
    '                        End If
    '                        Exit For
    '                    End If
    '                Next
    '            Next '###################################################################################################################################################

    '            'For 18.2, would it be possible to add a step to remove all enUS entries in the xml once translation is done? Consultants asked for that. 
    '            'If you don’t mind, make sure it’s easy to deactivate, as I have some doubts this is really required. 
    '            'I don 't think it would take you a lot of time and most probably it would take less than if I have to manually remove the enUS.
    '            RemoveEnUS(xlist, enUS_Index)


    '            xlist = xd.GetElementsByTagName("catgroupToCategories")
    '            Dim result As String = ""
    '            'Replace translation in Catgroup########################################################################################################################
    '            For i As Integer = 0 To xlist(0).ChildNodes.Count - 1
    '                For j As Integer = 0 To xlist(0).ChildNodes(i).Attributes.Count - 1
    '                    If xlist(0).ChildNodes(i).Attributes(j).Name.ToLower = "categoryname" Then
    '                        Dim Catname As String = Right(xlist(0).ChildNodes(i).Attributes(j).Value.Trim, 5)
    '                        If Mid(Catname, 3, 1) = "_" Then
    '                            If Catname.ToLower = TargetLang.ToLower Then
    '                                result = Find_and_ReplaceRMKContent(objXliffData, xlist(0).ChildNodes(i).Attributes(j).Value.Replace(TargetLang, ""), False, Nothing)
    '                                If result.Trim <> String.Empty Then
    '                                    xlist(0).ChildNodes(i).Attributes(j).Value = result
    '                                End If
    '                            End If
    '                        End If
    '                        Exit For
    '                    End If
    '                Next
    '            Next '###################################################################################################################################################

    '            xd.Save(sRmkXmlfile)

    '        Catch ex As Exception
    '            Throw New Exception(ex.Message)
    '        End Try
    '    End Sub

    '    Sub RemoveEnUS(ByRef xlist As XmlNodeList, ByVal enUS_Index As ArrayList)
    '        Try
    '            Dim x As Integer = enUS_Index.Count - 1
    '            Do Until x = -1
    '                Dim xNode As XmlNode = xlist(enUS_Index(x))
    '                xNode.ParentNode.RemoveChild(xNode)
    '                x -= 1
    '            Loop
    '        Catch ex As Exception
    '            Throw New Exception("Error @RemoveEnUS - " & ex.Message)
    '        End Try

    '    End Sub

    '    Sub ReadSingleNode(ByRef CategoryNode As XmlNode, ByRef objXLiff As sXliff, ByVal TargetLang As String, ByRef xlist As XmlNodeList, ByRef strmsg As StringBuilder)
    '        Try
    '            Dim CatName As String = ""
    '            Dim result As String
    '            For j As Integer = 0 To CategoryNode.Attributes.Count - 1
    '                result = ""
    '                Select Case CategoryNode.Attributes(j).Name.ToLower
    '                    Case "metakey"
    '                        result = Find_and_ReplaceRMKContent(objXLiff, CategoryNode.Attributes(j).Value, False, Nothing)
    '                        If result.Trim <> String.Empty Then
    '                            CategoryNode.Attributes(j).Value = result
    '                        End If
    '                    Case "metatitle"
    '                        result = Find_and_ReplaceRMKContent(objXLiff, CategoryNode.Attributes(j).Value, False, Nothing)
    '                        If result.Trim <> String.Empty Then
    '                            CategoryNode.Attributes(j).Value = result
    '                        End If
    '                    Case "metadesc"
    '                        result = Find_and_ReplaceRMKContent(objXLiff, CategoryNode.Attributes(j).Value, False, Nothing)
    '                        If result.Trim <> String.Empty Then
    '                            CategoryNode.Attributes(j).Value = result
    '                        End If
    '                    Case "headertext"
    '                        result = Find_and_ReplaceRMKContent(objXLiff, CategoryNode.Attributes(j).Value, True, Replace(CategoryNode.Attributes(0).Value, TargetLang, ""))
    '                        If result.Trim <> String.Empty Then
    '                            CategoryNode.Attributes(j).Value = result
    '                        End If
    '                    Case "keywordgroup"
    '                        result = Find_and_ReplaceRMKContent(objXLiff, CategoryNode.Attributes(j).Value, False, Nothing)
    '                        If result.Trim <> String.Empty Then
    '                            CategoryNode.Attributes(j).Value = result
    '                        End If
    '                    Case "categorylocale"
    '                        CategoryNode.Attributes(j).Value = TargetLang
    '                        'Case "id" 'As of now these are extra items which doesnot need translation, may be going forward it might require, keep it
    '                        'Case "display"
    '                        'Case "sortorder"
    '                        'Case "isDefault"
    '                        'Case "keywordgroupid"
    '                        'Case "locationgroupid"
    '                        'Case "locationgroup"
    '                        'Case "jobTitle"
    '                        'Case "SEOMarket"
    '                        'Case "stateAbbr"
    '                        'Case "city"
    '                        'Case "countryCode"
    '                        'Case "businessunit"
    '                        'Case "recruiterID"
    '                        'Case "segmentID"
    '                        'Case "buildTypeID"
    '                        'Case "brand"
    '                        'Case "skin"
    '                End Select
    '            Next

    '            For j As Integer = 0 To CategoryNode.Attributes.Count - 1 'This is in another step to get header text translation, if we use it in avove loop, it will replace category name and hence header will not match
    '                result = ""
    '                CatName = ""
    '                Select Case CategoryNode.Attributes(j).Name.ToLower
    '                    Case "name"
    '                        CatName = CategoryNode.Attributes(j).Value
    '                        result = Find_and_ReplaceRMKContent(objXLiff, CategoryNode.Attributes(j).Value.Replace(TargetLang, ""), False, Nothing) & " " & TargetLang
    '                        If result.Trim <> String.Empty Then
    '                            CategoryNode.Attributes(j).Value = result
    '                        End If
    '                End Select
    '            Next j

    '            Dim counter As Integer = 0
    '            For i As Integer = 0 To xlist.Count - 1
    '                ' Pass a test to ensure there are no 2 times the same job category name after translation (otherwise import will fail.). 
    '                'If you detect that, issue a warning with the name of the duplicate category name. We’ll have to fix manually.
    '                If xlist(i).Attributes(0).Value.ToLower.Trim = CatName.ToLower.Trim Then
    '                    counter += 1
    '                End If
    '            Next

    '            If counter >= 2 Then
    '                strmsg.AppendLine("RMK xml Category name - '" & CategoryNode.Value & "' repeated " & counter & " times")
    '            End If
    '        Catch ex As Exception
    '            Throw New Exception(ex.Message)
    '        End Try


    '    End Sub

    '    Function Find_and_ReplaceRMKContent(ByRef objxliff As sXliff, ByVal enUS As String, ByVal bHeader As Boolean, ByVal transId As String) As String
    '        Try
    '            Dim MyString As String
    '            If bHeader Then
    '                For i As Integer = 0 To objxliff.Source.Count - 1
    '                    MyString = Mid(objxliff.ID(i), InStr(objxliff.ID(i), "_") + 1, Len(objxliff.ID(i)))
    '                    If objxliff.Resname(i).ToString.ToLower = "headertext" And MyString.ToLower.Trim = transId.ToLower.Trim Then
    '                        Return objxliff.Translation(i)
    '                    End If
    '                Next
    '            Else
    '                Dim cleanUs As String = Replace(enUS, " ", String.Empty)
    '                If IsNothing(cleanUs) Then
    '                    cleanUs = ""
    '                End If
    '                For i As Integer = 0 To objxliff.Source.Count - 1
    '                    MyString = Replace(objxliff.Source(i), " ", String.Empty)
    '                    If cleanUs.ToLower = MyString.ToLower Or enUS.ToLower = objxliff.Source(i).ToString.ToLower Then
    '                        Return objxliff.Translation(i)
    '                    End If
    '                Next
    '            End If
    '        Catch ex As Exception
    '            Throw New Exception(ex.Message)
    '        End Try

    '        Return ""

    '    End Function



    '    Sub Rmk_JobCategory_to_Xliff(ByVal RmkFile As String, ByVal targetxliff_savePath As String, ByVal TargetLang As String, ByVal Langlist() As String)
    '        Dim xmlcontent As String
    '        Dim xd As New Xml.XmlDocument

    '        xmlcontent = dbletags_xml(System.IO.File.ReadAllText(RmkFile))
    '        xmlcontent = WebUtility.HtmlDecode(xmlcontent) ' remove the encoding for e.g. copyright sign

    '        xd.XmlResolver = Nothing
    '        Try
    '            xd.LoadXml(xmlcontent) 'NEED TO use try and catch the error. IF there is a xml error, an exception is raised here.
    '        Catch ex As Exception
    '            Throw New Exception("Error Loading xml file - " & System.IO.Path.GetFileName(RmkFile) & vbNewLine & ex.Message)
    '        End Try

    '        Dim objXliff As New sXliff
    '        objXliff.ID = New ArrayList
    '        objXliff.Note = New ArrayList
    '        objXliff.Resname = New ArrayList
    '        objXliff.Source = New ArrayList
    '        objXliff.TargetLang = TargetLang
    '        objXliff.Translation = New ArrayList

    '        Try
    '            Dim xlist As XmlNodeList = xd.GetElementsByTagName("category")

    '            Dim objRmkJobCat As RMK_JobCategory = Nothing

    '            Dim bFound As Boolean = False

    '            Dim enUSRootIndex As Integer = 0

    '            Dim Counter As Integer = 0
    '            For i As Integer = 0 To xlist.Count - 1
    '                Dim Catname As String = xlist(i).Attributes(0).Value
    '                If isEnUsOrTargetLangAvailableForCategoryName(Right(Catname.Trim, 5).ToLower) Then 'Get enUS Root Id.
    '                    objRmkJobCat = ExtractContent(xlist, TargetLang, i, Langlist)

    '                    With objRmkJobCat
    '                        UpdateXliffData(.CategoryName, .CategoryName, objXliff)
    '                        If Not .metatitle.ToString.Trim = String.Empty Then
    '                            UpdateXliffData(.metatitle, .CategoryName, objXliff)
    '                        End If
    '                        If Not .metakey.ToString.Trim = String.Empty Then
    '                            UpdateXliffData(.metakey, .CategoryName, objXliff)
    '                        End If
    '                        If .metadesc.ToString.Trim = String.Empty Then
    '                            UpdateXliffData(.metadesc, .CategoryName, objXliff)
    '                        End If
    '                        If Not .keywordgroup = String.Empty Then
    '                            UpdateXliffData(.keywordgroup, .CategoryName, objXliff)
    '                        End If
    '                    End With
    '                End If
    '            Next
    '        Catch ex As Exception
    '            Throw New Exception("Error @Rmk_JobCategory_to_Xliff " & ex.Message)
    '        End Try

    '        Try
    '            'Write to Xliff File
    '            CreateXliff(objXliff, targetxliff_savePath, TargetLang)
    '        Catch ex As Exception
    '            Throw New Exception("Error creating Rmk xliff file " & ex.Message)
    '        End Try

    '    End Sub

    '    Private Function CreateXliff(ByRef objXliff As sXliff, ByVal xliff_Path As String, ByVal Targetlanguage As String) As String

    '        Try
    '            Dim myNum As Integer = 0
    '            Using Writer As System.IO.StreamWriter = New System.IO.StreamWriter(xliff_Path, False, System.Text.Encoding.UTF8)
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


    '                For J As Integer = 0 To objXliff.ID.Count - 1
    '                    myNum += 1
    '                    Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & objXliff.ID(J) & Chr(34) & " resname=" & Chr(34) & "RMK_CategoryJob" & Chr(34) & ">")
    '                    'Writer.WriteLine("<trans-unit id=" & Chr(34) & clean_xml(RD.ID(J).ToString) & Chr(34) & " resname=" & Chr(34) & clean_xml(RD.resName(J).ToString) & Chr(34) & ">")
    '                    Writer.WriteLine("<source>" & wrap_html(clean_xml(objXliff.Source(J).ToString)) & "</source>")
    '                    If IsNumeric(objXliff.Source(J).ToString) Then
    '                        Writer.WriteLine("<target state=""needs-review-translation"">" & objXliff.Source(J) & "</target>")
    '                    Else
    '                        Writer.WriteLine("<target state=""needs-review-translation""></target>")
    '                    End If
    '                    Writer.WriteLine("<note from=""Developer"" priority =""10"">RMK</note>")
    '                    Writer.WriteLine("</trans-unit>")
    '                    Writer.WriteLine(vbCrLf)
    '                Next

    '                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

    '            End Using

    '            If myNum = 0 Then
    '                System.IO.File.Delete(xliff_Path)
    '                Return " - already has translation for " & Replace(Targetlanguage, "-", "_")
    '            End If

    '        Catch ex As Exception
    '            Throw New Exception(ex.Message)
    '        End Try

    '        Return ""

    '    End Function

    '    Private Sub UpdateXliffData(ByVal enUS As String, ByVal CategoryName As String, ByRef objxliff As sXliff)
    '        Try
    '            With objxliff
    '                If Not .Source.Contains(enUS) Then
    '                    .ID.Add(CategoryName)
    '                    .Source.Add(enUS)
    '                    .Note.Add("RMK-JobCategory")
    '                    .Resname.Add("RMK-JobCategory")
    '                End If
    '            End With
    '        Catch ex As Exception
    '            Throw New Exception(ex.Message)
    '        End Try
    '    End Sub

    '    Private Function ExtractContent(ByRef xlist As XmlNodeList, ByRef TargetLang As String, ByVal enUS_Index As Integer, ByRef Langlist() As String) As RMK_JobCategory
    '        Dim objRmkJobCat As New RMK_JobCategory

    '        Dim Catname As String = xlist(enUS_Index).Attributes(0).Value

    '        Dim bFound_targetLang As Boolean = False
    '        For i As Integer = 0 To xlist.Count - 1
    '            'Issue warning in the case a root (enUS) job category has no corresponding translated job categories at all. 
    '            'This is normal, some don’t need translations as they won’t be activated in the system.
    '            If xlist(i).Attributes(0).Value.ToLower.Trim = Catname.ToLower.Trim & " " & TargetLang.ToLower.Trim Then
    '                bFound_targetLang = True
    '                Exit For
    '            End If
    '        Next

    '        If Not bFound_targetLang Then
    '            Dim bFound_ProjectLang As Boolean = False
    '            For i As Integer = 0 To UBound(Langlist)
    '                'Also issue (non blocking) error if there are missing job categories with language code. 
    '                'E.g. you have ABC, ABC fr_FR, but in your project definition you have also de_DE, but you don’t find ABC de_DE.
    '                For j As Integer = 0 To xlist.Count - 1
    '                    For x As Integer = 0 To xlist(j).Attributes.Count - 1
    '                        If xlist(j).Attributes(x).Value.ToLower.Trim = Catname.ToLower.Trim & " " & Langlist(i).ToLower.Trim Then
    '                            bFound_ProjectLang = True
    '                            Exit For
    '                        End If
    '                    Next
    '                    If bFound_ProjectLang Then
    '                        Exit For
    '                    End If
    '                Next
    '                If bFound_ProjectLang Then
    '                    Exit For
    '                End If
    '            Next

    '            If bFound_ProjectLang Then
    '                'Raise another event
    '                MsgBox("Target Language '" & TargetLang & "' missing for Category Name - '" & Catname & "' but available for other language", MsgBoxStyle.Critical)
    '            Else
    '                'Raise event StrongWarningMsg( TargetLang & " language is not available for Category Name - " & Catname,"RED" )
    '                MsgBox("Target Language " & TargetLang & " language is not available for Category Name - " & Catname, MsgBoxStyle.Critical)
    '            End If

    '        End If

    '        With objRmkJobCat
    '            .CategoryName = xlist(enUS_Index).Attributes(0).Value
    '            For j As Integer = 0 To xlist(enUS_Index).Attributes.Count - 1
    '                Select Case xlist(enUS_Index).Attributes(j).Name.ToLower
    '                    Case "id"
    '                        .id = xlist(enUS_Index).Attributes(j).Value
    '                    Case "display"
    '                        .display = xlist(enUS_Index).Attributes(j).Value
    '                    Case "sortorder"
    '                        .sortorder = xlist(enUS_Index).Attributes(j).Value
    '                    Case "metakey"
    '                        .metakey = xlist(enUS_Index).Attributes(j).Value
    '                    Case "metatitle"
    '                        .metatitle = xlist(enUS_Index).Attributes(j).Value
    '                    Case "metadesc"
    '                        .metadesc = xlist(enUS_Index).Attributes(j).Value
    '                    Case "headertext"
    '                        .headertext = xlist(enUS_Index).Attributes(j).Value
    '                    Case "isDefault"
    '                        .isDefault = xlist(enUS_Index).Attributes(j).Value
    '                    Case "keywordgroupid"
    '                        .keywordgroupid = xlist(enUS_Index).Attributes(j).Value
    '                    Case "keywordgroup"
    '                        .keywordgroup = xlist(enUS_Index).Attributes(j).Value
    '                    Case "locationgroupid"
    '                        .locationgroupid = xlist(enUS_Index).Attributes(j).Value
    '                    Case "locationgroup"
    '                        .locationgroup = xlist(enUS_Index).Attributes(j).Value
    '                    Case "categorylocale"
    '                        .categorylocale = xlist(enUS_Index).Attributes(j).Value
    '                    Case "jobTitle"
    '                        .jobTitle = xlist(enUS_Index).Attributes(j).Value
    '                    Case "SEOMarket"
    '                        .SEOMarket = xlist(enUS_Index).Attributes(j).Value
    '                    Case "stateAbbr"
    '                        .stateAbbr = xlist(enUS_Index).Attributes(j).Value
    '                    Case "city"
    '                        .city = xlist(enUS_Index).Attributes(j).Value
    '                    Case "countryCode"
    '                        .countryCode = xlist(enUS_Index).Attributes(j).Value
    '                    Case "businessunit"
    '                        .businessunit = xlist(enUS_Index).Attributes(j).Value
    '                    Case "recruiterID"
    '                        .recruiterID = xlist(enUS_Index).Attributes(j).Value
    '                    Case "segmentID"
    '                        .segmentID = xlist(enUS_Index).Attributes(j).Value
    '                    Case "buildTypeID"
    '                        .buildTypeID = xlist(enUS_Index).Attributes(j).Value
    '                    Case "brand"
    '                        .brand = xlist(enUS_Index).Attributes(j).Value
    '                    Case "skin"
    '                        .skin = xlist(enUS_Index).Attributes(j).Value
    '                End Select
    '            Next
    '        End With

    '        Return objRmkJobCat
    '    End Function

    '    Private Function isEnUsOrTargetLangAvailableForCategoryName(ByVal CatName As String, Optional ByVal targetLang As String = "en_US") As Boolean

    '        Try
    '            LangDefintion.GetLanguageList()
    '            For i As Integer = 0 To LanguageDefination.LangFiveChars.Count - 1
    '                Dim lang As String = LanguageDefination.LangFiveChars(i).ToString.Replace("-", "_").ToLower.Trim
    '                lang = lang.Replace("(spain) ", "") 'exception with spainish lang
    '                If CatName.ToLower.Trim = lang Then
    '                    Return False
    '                End If
    '            Next

    '        Catch ex As Exception
    '            Throw New Exception("Error @GetEnusCatId " & ex.Message)
    '        End Try
    '        If targetLang = "en_US" Then
    '            Return True
    '        End If
    '        Return False
    '    End Function

    '    Public Class RMK_JobCategory

    '        'Public MyFields() As String
    '        'Public Sub New()
    '        '    Dim MyStr As String = "id,display,sortorder,metakey,metatitle,metadesc,headertext,isDefault,keywordgroupid,keywordgroup,locationgroupid,locationgroup,categorylocale,jobTitle,SEOMarket,stateAbbr,city,countryCode,businessunit,recruiterID,segmentID,buildTypeID,brand,skin,"
    '        '    MyFields = Split(MyStr, ",")
    '        'End Sub

    '        Public CategoryName As String
    '        Public id As String
    '        Public display As String
    '        Public sortorder As String
    '        Public metakey As String
    '        Public metatitle As String
    '        Public metadesc As String
    '        Public headertext As String
    '        Public isDefault As String
    '        Public keywordgroupid As String
    '        Public keywordgroup As String
    '        Public locationgroupid As String
    '        Public locationgroup As String
    '        Public categorylocale As String
    '        Public jobTitle As String
    '        Public SEOMarket As String
    '        Public stateAbbr As String
    '        Public city As String
    '        Public countryCode As String
    '        Public businessunit As String
    '        Public recruiterID As String
    '        Public segmentID As String
    '        Public buildTypeID As String
    '        Public brand As String
    '        Public skin As String
    '    End Class

    '#End Region

    '#Region "RMK"
    '    Sub RMK_Extraction()
    '        Dim xmlfile As String = "C:\Users\C5195092\Desktop\ClouTest\RMK\01-Input\300.2_index.xhtml"

    '        xmlfile = "C:\Users\C5195092\Desktop\ClouTest\RMK\RMK_RawData\Sample\johnsonandjohnson\index.xhtml"

    '        'xmlfile = "C:\Users\C5195092\Desktop\ClouTest\RMK\01-Input\300.2_index.xhtml"

    '        'xmlfile = "C:\Users\C5195092\Desktop\ClouTest\RMK\01-Input\test.xhtml"

    '        Dim xmlcontent As String
    '        Dim xd As New Xml.XmlDocument


    '        xmlcontent = dbletags_xml(System.IO.File.ReadAllText(xmlfile))
    '        xmlcontent = WebUtility.HtmlDecode(xmlcontent) ' remove the encoding for e.g. copyright sign

    '        xd.XmlResolver = Nothing
    '        xd.LoadXml(xmlcontent) 'NEED TO use try and catch the error. IF there is a xml error, an exception is raised here.

    '        Dim xlist As XmlNodeList = xd.GetElementsByTagName("h1")

    '        For i As Integer = 0 To k.Count - 1
    '            xlist = xd.GetElementsByTagName(k(i))
    '            Debug.Print(k(i))
    '        Next

    '        RecurseXML(xd.ChildNodes)

    '    End Sub


    '    Private Function CheckElementType(ByVal EleName As String) As Boolean
    '        Dim ExcludeElement As String = "ui:composition,ui:define,div,script"
    '        Dim str() As String = Split(ExcludeElement, ",")
    '        For i As Integer = 0 To UBound(str)
    '            If EleName.ToLower.Trim = str(i).ToLower.Trim Then
    '                Return True
    '            End If
    '        Next
    '        Return False
    '    End Function

    '    Dim s As New StringBuilder

    '    Dim k As New ArrayList
    '    Private Sub RecurseXML(nodes As XmlNodeList)

    '        For Each node As XmlNode In nodes

    '            If CheckElementType(node.Name) <> True Then
    '                If Not k.Contains(node.Name) Then
    '                    k.Add(node.Name)
    '                End If
    '            End If

    '            If (node.ChildNodes.Count > 0) Then
    '                For i As Integer = 0 To node.Attributes.Count - 1
    '                    If node.Attributes(i).Name.ToLower = "title" Or node.Attributes(i).Name.ToLower = "placeholder" Or node.Attributes(i).Name.ToLower = "value" Then
    '                        s.AppendLine(node.Attributes(i).InnerText)
    '                    End If
    '                Next
    '                RecurseXML(node.ChildNodes)
    '            Else

    '                If node.NodeType <> XmlNodeType.Comment Then
    '                    If Not IsNothing(node.Value) Then
    '                        If node.Value <> "×" And node.ParentNode.Name.ToLower <> "script" Then
    '                            s.AppendLine(node.Value())
    '                        End If
    '                    End If
    '                End If

    '            End If
    '        Next
    '    End Sub

    '    Private Function dbletags_xml(ByVal instring As String) As String
    '        instring = Replace(instring, "&amp;", "&amp;amp;")
    '        instring = Replace(instring, "&lt;", "&amp;lt;")
    '        instring = Replace(instring, "&gt;", "&amp;gt;")
    '        instring = Replace(instring, "&quot;", "&amp;quot;")
    '        dbletags_xml = Replace(instring, "&apos;", "&amp;apos;")
    '    End Function
    '#End Region

    '#Region "Onboarding offboarding"
    '    Sub Extract(ByVal sFile As String, ByVal sXliff As String)
    '        Dim xmlcontent As String
    '        Dim xd As New Xml.XmlDocument

    '        xmlcontent = (System.IO.File.ReadAllText(sFile))
    '        xmlcontent = WebUtility.HtmlDecode(xmlcontent) ' remove the encoding for e.g. copyright sign

    '        xd.XmlResolver = Nothing
    '        xd.LoadXml(xmlcontent) 'NEED TO use try and catch the error. IF there is a xml error, an exception is raised here.

    '        Dim xlist As XmlNodeList = xd.GetElementsByTagName("en-US")

    '        Dim objXliff As New sXliff
    '        objXliff.ID = New ArrayList
    '        objXliff.Note = New ArrayList
    '        objXliff.Resname = New ArrayList
    '        objXliff.Source = New ArrayList
    '        objXliff.TargetLang = ""
    '        objXliff.Translation = New ArrayList

    '        For i As Integer = 0 To xlist.Count - 1
    '            Dim MyParentNode As XmlNode = xlist(i).ParentNode
    '            For j As Integer = 0 To MyParentNode.ChildNodes.Count - 1
    '                If MyParentNode.ChildNodes(j).Name.ToLower = "wizard" Then
    '                    objXliff.Resname.Add(MyParentNode.ChildNodes(j).InnerText)
    '                ElseIf MyParentNode.ChildNodes(j).Name.ToLower = "label" Then
    '                    objXliff.ID.Add(MyParentNode.ChildNodes(j).InnerText)
    '                ElseIf MyParentNode.ChildNodes(j).Name.ToLower = "group" Then
    '                    objXliff.Note.Add(MyParentNode.ChildNodes(j).InnerText)
    '                ElseIf MyParentNode.ChildNodes(j).Name.ToLower = "en-us" Then
    '                    objXliff.Source.Add(MyParentNode.ChildNodes(j).InnerText)
    '                End If
    '            Next
    '        Next

    '        CreateXliffonboardingOffboarding(objXliff, sXliff, "fr-FR")

    '    End Sub

    '    Private Function CreateXliffonboardingOffboarding(ByRef objXliff As sXliff, ByVal xliff_Path As String, ByVal Targetlanguage As String) As String

    '        Try
    '            Dim myNum As Integer = 0
    '            Using Writer As System.IO.StreamWriter = New System.IO.StreamWriter(xliff_Path, False, System.Text.Encoding.UTF8)
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


    '                For J As Integer = 0 To objXliff.ID.Count - 1
    '                    If objXliff.Source(J).ToString.Trim <> String.Empty Then
    '                        myNum += 1
    '                        Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & objXliff.ID(J) & Chr(34) & " resname=" & Chr(34) & clean_xml(objXliff.Resname(J)) & Chr(34) & ">")
    '                        'Writer.WriteLine("<trans-unit id=" & Chr(34) & clean_xml(RD.ID(J).ToString) & Chr(34) & " resname=" & Chr(34) & clean_xml(RD.resName(J).ToString) & Chr(34) & ">")
    '                        Writer.WriteLine("<source>" & wrap_html(clean_xml(objXliff.Source(J).ToString)) & "</source>")
    '                        If IsNumeric(objXliff.Source(J).ToString) Then
    '                            Writer.WriteLine("<target state=""needs-review-translation"">" & objXliff.Source(J) & "</target>")
    '                        Else
    '                            Writer.WriteLine("<target state=""needs-review-translation""></target>")
    '                        End If
    '                        Writer.WriteLine("<note from=""Developer"" priority =""10"">Onboarding-Offboarding : " & clean_xml(objXliff.Note(J)) & "</note>")
    '                        Writer.WriteLine("</trans-unit>")
    '                        Writer.WriteLine(vbCrLf)
    '                    End If
    '                Next

    '                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

    '            End Using

    '            If myNum = 0 Then
    '                System.IO.File.Delete(xliff_Path)
    '                Return " - already has translation for " & Replace(Targetlanguage, "-", "_")
    '            End If

    '        Catch ex As Exception
    '            Throw New Exception(ex.Message)
    '        End Try

    '        Return ""

    '    End Function

    '#End Region


End Module
