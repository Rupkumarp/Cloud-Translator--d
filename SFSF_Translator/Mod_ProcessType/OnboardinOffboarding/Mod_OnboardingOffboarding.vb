Imports System.Net
Imports System.Xml

Module Mod_OnboardingOffboarding

#Region "Onboarding offboarding Extraction"
    Public Function To_Xliff(ByVal sFile As String, ByVal sXliff As String, ByVal TargetLang As String, Optional ByVal bExtractWithTranslationOnly As Boolean = False) As Boolean
        Try
            Dim xmlcontent As String
            Dim xd As New Xml.XmlDocument

            xmlcontent = (System.IO.File.ReadAllText(sFile, System.Text.Encoding.UTF8))
            ' xmlcontent = WebUtility.HtmlDecode(xmlcontent) ' remove the encoding for e.g. copyright sign

            xd.XmlResolver = Nothing

            Try
                xd.LoadXml(xmlcontent) 'NEED TO use try and catch the error. IF there is a xml error, an exception is raised here.
            Catch ex As Exception
                Throw New Exception("Error loading xml - " & System.IO.Path.GetFileName(sFile) & vbCrLf & ex.Message)
            End Try

            If bExtractWithTranslationOnly Then
                If Not ExtractWithTranslation(xd, sFile, TargetLang, bExtractWithTranslationOnly, sXliff) Then
                    Return False
                End If
            Else
                If Not ExtractWithoutTranslation(xd, sFile, TargetLang, bExtractWithTranslationOnly, sXliff) Then
                    Return False
                End If
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True

    End Function

    Private Function ExtractWithTranslation(ByRef xd As Xml.XmlDocument, ByRef sFile As String, ByVal TargetLang As String, ByVal bExtractWithTranslationOnly As String, ByVal sXliff As String) As Boolean
        Dim objXmlDefinition As New XmlDefinition
        objXmlDefinition.GetXmlDefinition(sFile)

        Dim objXliff As New sXliff
        Dim xlist As XmlNodeList

        For x As Integer = 0 To objXmlDefinition.Definitions.Count - 1
            Dim definitions As String = objXmlDefinition.Definitions(x).Replace("<", "").Replace(">", "")
            If definitions.Trim <> String.Empty Then
                xlist = xd.GetElementsByTagName(definitions)
                objXliff.ID = New ArrayList
                objXliff.Note = New ArrayList
                objXliff.Resname = New ArrayList
                objXliff.Source = New ArrayList
                objXliff.TargetLang = ""
                objXliff.Translation = New ArrayList

                For i As Integer = 0 To xlist.Count - 1
                    Dim MyParentNode As XmlNode = xlist(i).ParentNode
                    For j As Integer = 0 To MyParentNode.ChildNodes.Count - 1
                        If MyParentNode.ChildNodes(j).Name.ToLower = "wizard" Then
                            objXliff.Resname.Add(MyParentNode.ChildNodes(j).InnerText)
                        ElseIf MyParentNode.ChildNodes(j).Name.ToLower = "label" Then
                            objXliff.ID.Add(MyParentNode.ChildNodes(j).InnerText)
                        ElseIf MyParentNode.ChildNodes(j).Name.ToLower = "group" Then
                            objXliff.Note.Add(MyParentNode.ChildNodes(j).InnerText)
                        ElseIf MyParentNode.ChildNodes(j).Name.ToLower = "en-us" Then
                            ' objXliff.Source.Add(MyParentNode.ChildNodes(j).InnerText.ToString.Replace(" ", "&nbsp;"))

                            If InStr(LCase(MyParentNode.ChildNodes(j).InnerXml), "cdata") > 0 Then
                                Dim xCData As XmlCDataSection = MyParentNode.ChildNodes(j).ChildNodes(0)
                                xCData.InnerText = MyParentNode.ChildNodes(j).InnerText.ToString.Replace(" ", "&nbsp;")
                                objXliff.Source.Add(MyParentNode.ChildNodes(j).ChildNodes(0).InnerText)
                            Else
                                objXliff.Source.Add(MyParentNode.ChildNodes(j).InnerText)
                            End If
                        ElseIf MyParentNode.ChildNodes(j).Name.ToLower = "translations" Then
                            For L As Integer = 0 To MyParentNode.ChildNodes(j).ChildNodes.Count - 1
                                For k As Integer = 0 To MyParentNode.ChildNodes(j).ChildNodes(L).Attributes.Count - 1
                                    If MyParentNode.ChildNodes(j).ChildNodes(L).Attributes(k).Value.ToLower = "en-us" Then
                                        If InStr(LCase(MyParentNode.ChildNodes(j).ChildNodes(L).InnerXml), "cdata") > 0 Then
                                            Dim xCData As XmlCDataSection = MyParentNode.ChildNodes(j).ChildNodes(L).ChildNodes(0)
                                            xCData.InnerText = MyParentNode.ChildNodes(j).ChildNodes(L).InnerText.ToString.Replace(" ", "&nbsp;")
                                            objXliff.Source.Add(MyParentNode.ChildNodes(j).ChildNodes(L).ChildNodes(0).InnerText)
                                        Else
                                            If IsNothing(MyParentNode.ChildNodes(j).ChildNodes(L).ChildNodes(0)) Then
                                                objXliff.Source.Add("")
                                            Else
                                                objXliff.Source.Add(MyParentNode.ChildNodes(j).ChildNodes(L).ChildNodes(0).InnerText)
                                            End If

                                        End If
                                    End If
                                Next
                            Next


                        ElseIf MyParentNode.ChildNodes(j).Name.ToLower.Replace("-", "_") = TargetLang.ToLower Then
                            If bExtractWithTranslationOnly Then
                                If InStr(LCase(MyParentNode.ChildNodes(j).InnerXml), "cdata") > 0 Then
                                    Dim xCData As XmlCDataSection = MyParentNode.ChildNodes(j).ChildNodes(0)
                                    xCData.InnerText = MyParentNode.ChildNodes(j).InnerText.ToString.Replace(" ", "&nbsp;")
                                    objXliff.Translation.Add(MyParentNode.ChildNodes(j).ChildNodes(0).InnerText)
                                Else
                                    objXliff.Translation.Add(MyParentNode.ChildNodes(j).InnerText)
                                End If
                            End If
                        End If
                    Next
                Next
            End If
        Next

        If objXliff.Translation.Count = objXliff.Source.Count Then
            CreateXliffonboardingOffboarding(objXliff, sXliff, TargetLang, True)
        Else
            Return False
        End If

        Return True
    End Function

    Private Function ExtractWithoutTranslation(ByRef xd As Xml.XmlDocument, ByRef sFile As String, ByVal TargetLang As String, ByVal bExtractWithTranslationOnly As String, ByVal sXliff As String) As Boolean
        Dim objXmlDefinition As New XmlDefinition
        objXmlDefinition.GetXmlDefinition(sFile)

        Dim objXliff As New sXliff
        Dim xlist As XmlNodeList

        For x As Integer = 0 To objXmlDefinition.Definitions.Count - 1
            Dim definitions As String = objXmlDefinition.Definitions(x).Replace("<", "").Replace(">", "")
            If definitions.Trim <> String.Empty Then
                xlist = xd.GetElementsByTagName(definitions)
                objXliff.ID = New ArrayList
                objXliff.Note = New ArrayList
                objXliff.Resname = New ArrayList
                objXliff.Source = New ArrayList
                objXliff.TargetLang = ""
                objXliff.Translation = New ArrayList

                For i As Integer = 0 To xlist.Count - 1
                    Dim MyParentNode As XmlNode = xlist(i).ParentNode
                    For j As Integer = 0 To MyParentNode.ChildNodes.Count - 1
                        If MyParentNode.ChildNodes(j).Name.ToLower = "wizard" Then
                            objXliff.Resname.Add(MyParentNode.ChildNodes(j).InnerText)
                        ElseIf MyParentNode.ChildNodes(j).Name.ToLower = "label" Then
                            objXliff.ID.Add(MyParentNode.ChildNodes(j).InnerText)
                        ElseIf MyParentNode.ChildNodes(j).Name.ToLower = "group" Then
                            objXliff.Note.Add(MyParentNode.ChildNodes(j).InnerText)
                        ElseIf MyParentNode.ChildNodes(j).Name.ToLower = "en-us" Then
                            ' objXliff.Source.Add(MyParentNode.ChildNodes(j).InnerText.ToString.Replace(" ", "&nbsp;"))

                            If InStr(LCase(MyParentNode.ChildNodes(j).InnerXml), "cdata") > 0 Then
                                Dim xCData As XmlCDataSection = MyParentNode.ChildNodes(j).ChildNodes(0)
                                xCData.InnerText = MyParentNode.ChildNodes(j).InnerText.ToString.Replace(" ", "&nbsp;")
                                objXliff.Source.Add(MyParentNode.ChildNodes(j).ChildNodes(0).InnerText)
                            Else
                                objXliff.Source.Add(MyParentNode.ChildNodes(j).InnerText)
                            End If
                        ElseIf MyParentNode.ChildNodes(j).Name.ToLower = "translations" Then
                            For L As Integer = 0 To MyParentNode.ChildNodes(j).ChildNodes.Count - 1
                                For k As Integer = 0 To MyParentNode.ChildNodes(j).ChildNodes(L).Attributes.Count - 1
                                    If MyParentNode.ChildNodes(j).ChildNodes(L).Attributes(k).Value.ToLower = "en-us" Then
                                        If InStr(LCase(MyParentNode.ChildNodes(j).ChildNodes(L).InnerXml), "cdata") > 0 Then
                                            Dim xCData As XmlCDataSection = MyParentNode.ChildNodes(j).ChildNodes(L).ChildNodes(0)
                                            xCData.InnerText = MyParentNode.ChildNodes(j).ChildNodes(L).InnerText.ToString.Replace(" ", "&nbsp;")
                                            objXliff.Source.Add(MyParentNode.ChildNodes(j).ChildNodes(L).ChildNodes(0).InnerText)
                                        Else
                                            If IsNothing(MyParentNode.ChildNodes(j).ChildNodes(L).ChildNodes(0)) Then
                                                objXliff.Source.Add("")
                                            Else
                                                objXliff.Source.Add(MyParentNode.ChildNodes(j).ChildNodes(L).ChildNodes(0).InnerText)
                                            End If

                                        End If
                                    End If
                                Next
                            Next


                        End If
                    Next
                Next
            End If
        Next

        CreateXliffonboardingOffboarding(objXliff, sXliff, TargetLang, False)

        Return True

    End Function

    Private Function CreateXliffonboardingOffboarding(ByRef objXliff As sXliff, ByVal xliff_Path As String, ByVal Targetlanguage As String, ByVal ExtractWithTranslationOnly As Boolean) As String

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
                    If objXliff.Source(J).ToString.Trim <> String.Empty Then
                        myNum += 1
                        Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & objXliff.ID(J) & Chr(34) & " resname=" & Chr(34) & clean_xml(objXliff.Resname(J)) & Chr(34) & ">")
                        'Writer.WriteLine("<trans-unit id=" & Chr(34) & clean_xml(RD.ID(J).ToString) & Chr(34) & " resname=" & Chr(34) & clean_xml(RD.resName(J).ToString) & Chr(34) & ">")
                        Writer.WriteLine("<source>" & wrap_html(clean_xml(objXliff.Source(J).ToString)) & "</source>")
                        If IsNumeric(objXliff.Source(J).ToString) Then
                            Writer.WriteLine("<target state=""needs-review-translation"">" & objXliff.Source(J) & "</target>")
                        Else
                            If ExtractWithTranslationOnly Then
                                Writer.WriteLine("<target state=""needs-review-translation"">" & wrap_html(clean_xml(objXliff.Translation(J).ToString)) & "</target>")
                            Else
                                Writer.WriteLine("<target state=""needs-review-translation""></target>")
                            End If

                        End If
                        Writer.WriteLine("<note from=""Developer"" priority =""10"">Onboarding-Offboarding : " & clean_xml(objXliff.Note(J)) & "</note>")
                        Writer.WriteLine("</trans-unit>")
                        Writer.WriteLine(vbCrLf)
                    End If
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

#End Region

#Region "Onboarding offboarding ReIntegration"
    Public Sub To_Xml(ByVal sInputFile As String, ByVal sTranslatedXliff As String, ByVal TargetLang As String)
        Try
            Dim xmlcontent As String
            Dim xd As New Xml.XmlDocument

            xmlcontent = (System.IO.File.ReadAllText(sInputFile))
            'xmlcontent = WebUtility.HtmlDecode(xmlcontent) ' remove the encoding for e.g. copyright sign

            xd.XmlResolver = Nothing

            Try
                'xd.PreserveWhitespace = True
                xd.LoadXml(xmlcontent) 'NEED TO use try and catch the error. IF there is a xml error, an exception is raised here.
            Catch ex As Exception
                Throw New Exception("Error loading xml - " & System.IO.Path.GetFileName(sInputFile) & vbCrLf & ex.Message)
            End Try

            Dim objXmlDefinition As New XmlDefinition
            objXmlDefinition.GetXmlDefinition(sInputFile)

            Dim objXliff As sXliff = load_xliff(sTranslatedXliff)

            Dim xNodeList As XmlNodeList = Nothing
            Dim root As XmlNode = Nothing
            Dim CData As XmlCDataSection = Nothing
            Dim bCdata As Boolean = False
            Dim bFound As Boolean = False
            Dim bSourceFound As Boolean = False
            Dim cloneNode As XmlNode = Nothing
            Dim MyAttributes As XmlAttributeCollection = Nothing

            Dim sWizard As String
            Dim sLabel As String
            Dim sgroup As String
            Dim i As Integer = 0

            For k As Integer = 0 To objXmlDefinition.Definitions.Count - 1

                Dim ElementName As String = objXmlDefinition.Definitions(k).Replace("<", "").Replace(">", "")
                xNodeList = xd.GetElementsByTagName(ElementName)

                If Not ElementName.Trim = String.Empty Then
                    Do Until xNodeList.Count - 1 < i
                        sWizard = ""
                        sLabel = ""
                        sgroup = ""
                        Dim xTargetElement As XmlElement = xd.CreateElement(TargetLang.Replace("_", "-"))

                        Dim MyParentNode As XmlNode = xNodeList(i).ParentNode
                        Dim enUs As String = ""
                        bCdata = False

                        For j As Integer = 0 To MyParentNode.ChildNodes.Count - 1
                            If MyParentNode.ChildNodes(j).Name.ToLower = "translations" Then
                                For L As Integer = 0 To MyParentNode.ChildNodes(j).ChildNodes.Count - 1
                                    For m As Integer = 0 To MyParentNode.ChildNodes(j).ChildNodes(L).Attributes.Count - 1
                                        If MyParentNode.ChildNodes(j).ChildNodes(L).Attributes(m).Value.ToLower = "en-us" Then
                                            If InStr(LCase(MyParentNode.ChildNodes(j).ChildNodes(L).InnerXml), "cdata") > 0 Then
                                                Dim xCData As XmlCDataSection = MyParentNode.ChildNodes(j).ChildNodes(L).ChildNodes(0)
                                                xCData.InnerText = MyParentNode.ChildNodes(j).ChildNodes(L).InnerText.ToString.Replace(" ", "&nbsp;")
                                                'Replace content
                                            Else
                                                If IsNothing(MyParentNode.ChildNodes(j).ChildNodes(L).ChildNodes(0)) Then
                                                    ' objXliff.Source.Add("")
                                                Else
                                                    'Replace content
                                                    ReplaceTranslatioNode(MyParentNode.ChildNodes(j), MyParentNode.ChildNodes(j).ChildNodes(L).ChildNodes(0).InnerText, objXliff, TargetLang)
                                                End If

                                            End If
                                        End If
                                    Next
                                Next
                            End If
                        Next

                        ''Old format - depreceatecd
                        'For j As Integer = 0 To MyParentNode.ChildNodes.Count - 1
                        '    If MyParentNode.ChildNodes(j).Name.ToLower = "wizard" Then
                        '        sWizard = MyParentNode.ChildNodes(j).InnerText
                        '    ElseIf MyParentNode.ChildNodes(j).Name.ToLower = "label" Then
                        '        sLabel = MyParentNode.ChildNodes(j).InnerText
                        '    ElseIf MyParentNode.ChildNodes(j).Name.ToLower = "group" Then
                        '        sgroup = MyParentNode.ChildNodes(j).InnerText
                        '    ElseIf MyParentNode.ChildNodes(j).Name.ToLower = ElementName.ToLower Then
                        '        If InStr(LCase(MyParentNode.ChildNodes(j).InnerXml), "cdata") > 0 Then
                        '            Dim xCData As XmlCDataSection = MyParentNode.ChildNodes(j).ChildNodes(0)
                        '            xCData.InnerText = MyParentNode.ChildNodes(j).InnerText.ToString.Replace(" ", "&nbsp;")
                        '            enUs = MyParentNode.ChildNodes(j).ChildNodes(0).InnerText
                        '            bCdata = True
                        '        Else
                        '            enUs = MyParentNode.ChildNodes(j).InnerText
                        '        End If

                        '    ElseIf MyParentNode.ChildNodes(j).Name.ToLower = TargetLang.Replace("_", "-").ToLower Then
                        '        '  MyParentNode.ChildNodes(j).ParentNode.RemoveChild(MyParentNode.ChildNodes(j))
                        '        xTargetElement = MyParentNode.ChildNodes(j)
                        '    End If
                        'Next

                        'If enUs.Trim <> String.Empty Then
                        '    Dim lbId As String = ""
                        '    Dim Mygroup As String = ""
                        '    For m As Integer = 0 To objXliff.Source.Count - 1
                        '        lbId = Mid(objXliff.ID(m), InStr(objXliff.ID(m), "_") + 1, Len(objXliff.ID(m)))
                        '        Mygroup = objXliff.Note(m + 1).ToString.Replace("Onboarding-Offboarding : ", "")
                        '        ' If sWizard.ToLower.Trim = objXliff.Resname(m).ToString.ToLower.Trim And sLabel.ToLower.Trim = lbId.ToLower.Trim And Mygroup.ToLower.Trim = sgroup.ToLower.Trim Then
                        '        If enUs.Trim.ToLower = objXliff.Source(m).trim.ToString.ToLower Or GetPlainText(enUs.ToLower) = GetPlainText(objXliff.Source(m).ToString.ToLower) Then

                        '            If bCdata Then
                        '                CData = xd.CreateCDataSection(objXliff.Translation(m))
                        '                xTargetElement.AppendChild(CData)
                        '            Else
                        '                xTargetElement.InnerText = objXliff.Translation(m)
                        '            End If

                        '            xNodeList(i).ParentNode.AppendChild(xTargetElement)
                        '            Exit For
                        '        End If
                        '    Next
                        'End If

                        i += 1

                        xNodeList = xd.GetElementsByTagName(Replace(Replace(ElementName, "<", ""), ">", ""))

                    Loop
                End If
            Next

            xd.Save(sInputFile)

            Beautify(xd, sInputFile)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Sub ReplaceTranslatioNode(ByRef childNode As XmlNode, ByVal enUs As String, objxliff As sXliff, ByVal targetLang As String)
        Dim bFound As Boolean = False
        For i As Integer = 0 To childNode.ChildNodes.Count - 1
            For j As Integer = 0 To childNode.ChildNodes(i).Attributes.Count - 1
                If childNode.ChildNodes(i).Attributes(j).Value.ToLower = targetLang.ToLower Or childNode.ChildNodes(i).Attributes(j).Value.ToLower = targetLang.Replace("_", "-").ToLower Then
                    bFound = True
                    childNode.ChildNodes(i).InnerText = getTranslationData(objxliff, enUs)
                    Exit For
                End If
            Next
        Next

        If Not bFound Then
            Throw New Exception("Onboarding file issue - <Translation locale> tag needs to be created." & vbNewLine & "Content - " & enUs) 'If element not found then create it ex <Translation locale ="de_de"> </Translation>
        End If
    End Sub

    Function getTranslationData(objXliff As sXliff, enUs As String) As String
        For m As Integer = 0 To objXliff.Source.Count - 1
            If enUs.Trim.ToLower = objXliff.Source(m).trim.ToString.ToLower Or GetPlainText(enUs.ToLower) = GetPlainText(objXliff.Source(m).ToString.ToLower) Then
                Return objXliff.Translation(m)
                Exit For
            End If
        Next
        Return String.Empty
    End Function

    Public Sub Beautify(doc As XmlDocument, ByVal sFile As String)
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

    Private Function Check_enUS_Attribute_Available(ByVal xNode As XmlNode, ByVal elementname As String) As Boolean
        If xNode.ChildNodes.Count = 0 Then
            Return True
        End If

        Dim MyAttributes As XmlAttributeCollection = Nothing

        If xNode.Name.ToLower = elementname.ToLower Then
            If xNode.Attributes.Count = 0 Then 'This means its enUS, when no attribures ex:
                Return True
            Else
                Dim att As XmlAttribute
                MyAttributes = xNode.Attributes
                For Each att In MyAttributes
                    If att.Value.ToLower = "en_us" Or att.Value = "enus" Then
                        Return True
                    End If
                Next
            End If
        End If

        Return False
    End Function

#End Region

End Module



#Region "Sample File"
'<?xml version="1.0" encoding="utf-8"?>
'<LocalizationDataExchange xmlns : xsd = "http://www.w3.org/2001/XMLSchema" xmlns:xsi = "http://www.w3.org/2001/XMLSchema-instance" >
'  <Item>
'      <Group>Copy_of_NextGen_NES_ContactDetails</Group>
'      <Wizard>Crossboarding - Internal Employee</Wizard>
'      <Label>Label_3.Title</Label>
'      <Translations>
'          <Translation Locale="en-US">Contact Information</Translation>
'          <Translation Locale="de-DE"/>
'          <Translation Locale="en-GB"/>
'          <Translation Locale="fr-FR"/>
'          <Translation Locale="ja-JP"/>
'          <Translation Locale="ko-KR"/>
'          <Translation Locale="ru-RU"/>
'          <Translation Locale="zh-CN"/>
'          <Translation Locale="es-ES"/>
'          <Translation Locale="pt-BR"/>
'      </Translations>
'  </Item>
'<Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> Label_7.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Please enter your contact details below:</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> TextBox_11.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Home Phone</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> TextBox_13.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Mobile</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> TextBox_15.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Personal Email</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> Label_11.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Please enter your Home/Permanent And Current/Postal Address below:</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> Label_15.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Home / Permanent Address:</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> TextBox_17.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Address Line 1</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> TextBox_19.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Address Line 2</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> TextBox_21.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > City</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> TextBox_25.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Postcode</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> Label_29.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Current / Postal Address:</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> DropDownList_20.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Same As Home Address</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> Label_30.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Please enter your Current/Postal Address below:</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> TextBox_32.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Address Line 1</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> TextBox_36.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Address Line 2</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> TextBox_34.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > City</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> TextBox_40.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Postcode</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> DropDownList_20.EmptyItemText</Label>
'    <Translations>
'<Translation Locale = "en-US" > -- SELECT --</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> BizxPickList_33.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > State</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> BizxPickList_33.EmptyItemText</Label>
'    <Translations>
'<Translation Locale = "en-US" > -- SELECT --</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> DropDownList_31.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > Country</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> DropDownList_31.EmptyItemText</Label>
'    <Translations>
'<Translation Locale = "en-US" > --SELECTED - -</Translation>
'<Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> DropDownList_33.Title</Label>
'    <Translations>
'<Translation Locale = "en-US" > State</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'  <Item>
'<Group> Copy_of_NextGen_NES_ContactDetails</Group>
'    <Wizard> Crossboarding - Internal Employee</Wizard>
'    <Label> DropDownList_33.EmptyItemText</Label>
'<Translations>
'<Translation Locale = "en-US" > -- SELECT --</Translation>
'      <Translation Locale = "de-DE" />
'      <Translation Locale="en-GB"/>
'<Translation Locale = "fr-FR" />
'      <Translation Locale="ja-JP"/>
'<Translation Locale = "ko-KR" />
'      <Translation Locale="ru-RU"/>
'<Translation Locale = "zh-CN" />
'      <Translation Locale="es-ES"/>
'<Translation Locale = "pt-BR" />
'    </Translations>
'  </Item>
'</LocalizationDataExchange>
#End Region
