Imports System.Xml
Imports System.IO
Imports System.Xml.Schema

Module Mod_Xliff_To_Xml

    Dim notFound As ArrayList
    Dim bMatch As Boolean
    Dim counter As Integer

    Function xliff_to_xml(ByVal originalfile_path As String, ByVal translated_xliff_path As String, ByVal TT As TranslationType, ByVal lang As String) As Boolean
        Dim targetfilepath As String = ""
        notFound = New ArrayList
        Try
            If TT = TranslationType.Monolingual Then
                Dim sFileName As String = System.IO.Path.GetFileName(originalfile_path)

                'Monolingual
                targetfilepath = Replace(originalfile_path, "01-Input-B", "05-Output")
                targetfilepath = System.IO.Path.GetDirectoryName(targetfilepath) & "\Mono_" & lang & "\"

                If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(targetfilepath)) <> True Then
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetfilepath))
                End If

                targetfilepath = targetfilepath & sFileName

                File.Copy(originalfile_path, targetfilepath, True)

            Else
                If InStr(originalfile_path, "01-Input-B") <> 0 Then
                    'case 1 first language
                    targetfilepath = Replace(originalfile_path, "01-Input-B", "05-Output")

                ElseIf InStr(originalfile_path, "05-Output") <> 0 Then
                    'case 2 subsequent languages
                    targetfilepath = originalfile_path
                    originalfile_path = Replace(originalfile_path, "05-Output", "01-Input-B")

                Else
                    'case 3, project structure not used. Manual operations. Just add _out.
                    targetfilepath = Path.GetDirectoryName(originalfile_path) & "\" & Path.GetFileNameWithoutExtension(originalfile_path) & ".out" & Path.GetExtension(originalfile_path)

                End If

                If CheckFileOrFolderExists(targetfilepath, fType.file) <> True Then
                    Return False
                End If

                If CheckFileOrFolderExists(translated_xliff_path, fType.file) <> True Then
                    Return False
                End If
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Try
            Dim objXliff As New sXliff
            Try
                objXliff = ModHelper.load_xliff(translated_xliff_path)
            Catch ex As Exception
                If ModHelper.UnWrapXliffBack(translated_xliff_path) <> True Then
                    Throw New Exception("Error UnWrapping xliff back!")
                End If
                objXliff = ModHelper.cvload_xliff(Application.StartupPath & "\Temp_UnWrap.xliff")
            End Try

            Dim objXmlDefinition As New XmlDefinition
            objXmlDefinition.GetXmlDefinition(originalfile_path)

            ' Dim notFound As New ArrayList

            Select Case TT
                Case TranslationType.Monolingual
                    MonoLingual_Translation(targetfilepath, objXmlDefinition.Definitions, objXliff, lang)
                Case TranslationType.Multilingual
                    MultiLingual_Translation(targetfilepath, objXmlDefinition, objXliff, lang)
            End Select

            'NO Translation found then show a msg box and log it as well.
            If notFound.Count > 0 Then
                Dim objMissingTransaltion As New MissedTranslations
                objMissingTransaltion.UpdateMsg(notFound, originalfile_path, lang)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return True

    End Function


#Region "Monolingual Translation"
    Sub MonoLingual_Translation(ByVal xmlFile As String, ByVal ElementName() As String, ByVal xliffData As sXliff, ByVal lang As String)
        ' ElementName = clean_element(Replace(Replace(ElementName, "<", ""), ">", ""))

        Try
            Tags.Clear()
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(xmlFile)

            Dim xNodeList As XmlNodeList

            For j As Integer = 1 To ElementName.Count - 1

                If ElementName(j).ToString.Trim <> "" Then
                    xNodeList = xd.GetElementsByTagName(Replace(Replace(ElementName(j), "<", ""), ">", ""))

                    Dim MyAttributes As XmlAttributeCollection
                    Dim str As String = ""

                    For i As Integer = 0 To xNodeList.Count - 1

                        Dim blangFound As Boolean = False
                        If xNodeList(i).Attributes.Count > 0 Then
                            MyAttributes = xNodeList(i).Attributes
                            Dim att As XmlAttribute
                            For Each att In MyAttributes

                                'If att.Value.ToString <> "" Then 'AML Hungraian
                                '    str = GetTranslatedContent(xliffData, att.Value)
                                '    att.Value = str

                                'End If


                                If InStr(att.Name, "lang") > 0 Then
                                    blangFound = True
                                    If LCase(att.Value) = "en_us" Then
                                        str = GetTranslatedContent(xliffData, xNodeList(i).InnerText)
                                        If str <> "" Then
                                            bMatch = True
                                            If InStr(LCase(xNodeList(i).InnerXml), "cdata") > 0 Then
                                                Dim CData As XmlCDataSection = xNodeList(i).ChildNodes(0)
                                                CData.InnerText = str
                                            Else
                                                xNodeList(i).InnerText = str
                                            End If
                                        End If
                                        Exit For
                                    End If
                                End If
                            Next
                            If blangFound <> True Then
                                str = GetTranslatedContent(xliffData, xNodeList(i).InnerText)
                                If str <> "" Then
                                    bMatch = True
                                    If InStr(LCase(xNodeList(i).InnerXml), "cdata") > 0 Then
                                        Dim CData As XmlCDataSection = xNodeList(i).ChildNodes(0)
                                        CData.InnerText = str
                                    Else
                                        xNodeList(i).InnerText = str
                                    End If
                                End If

                            End If
                        Else
                            str = GetTranslatedContent(xliffData, xNodeList(i).InnerText)
                            If str <> "" Then
                                bMatch = True
                                If InStr(LCase(xNodeList(i).InnerXml), "cdata") > 0 Then
                                    Dim CData As XmlCDataSection = xNodeList(i).ChildNodes(0)
                                    CData.InnerText = str
                                Else
                                    xNodeList(i).InnerText = str
                                End If
                            End If
                        End If

                        str = ""
                        If bMatch <> True Then
                            counter += 1
                            notFound.Add(counter & ". Source value -> " & xNodeList(i).InnerText)
                        End If
                    Next

                End If
            Next

            xd.Save(xmlFile)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub
#End Region

#Region "Multilingual Translation"
    Sub MultiLingual_Translation(ByRef xmlFile As String, ByVal objXmlDefinition As XmlDefinition, ByVal objXliff As sXliff, ByVal lang As String)

        Dim bLanguage As Boolean
        Dim bXmlLang As Boolean
        Dim bSuppLang As Boolean

        Dim IsHypon As Boolean = False

        For x As Integer = 1 To objXmlDefinition.Definitions.Count - 1

            Dim ElementName As String = objXmlDefinition.Definitions(x)

            If ElementName.Trim <> "" Then

                Dim iElementIndex As Integer = CheckElementisPresentInDefinition(ElementName, objXmlDefinition.Definitions)
                bLanguage = objXmlDefinition.bLang(iElementIndex)
                bXmlLang = objXmlDefinition.bXmlLang(iElementIndex)
                bSuppLang = objXmlDefinition.bAddSuppLang

                Try
                    Dim xd As New Xml.XmlDocument
                    xd.XmlResolver = Nothing
                    xd.Load(xmlFile)

                    Dim xNodeList As XmlNodeList

                    Try
                        If bSuppLang Then
                            Dim Tempxd As New Xml.XmlDocument
                            Tempxd.XmlResolver = Nothing
                            Dim TempxNodeList As XmlNodeList
                            Tempxd.Load(appData & DefinitionFiles.SupLang_List)
                            TempxNodeList = Tempxd.GetElementsByTagName("supported-languages")
                            xNodeList = xd.GetElementsByTagName("supported-languages")
                            xd.ImportNode(xNodeList(0), True)
                            Dim newBook As XmlNode = xd.ImportNode(TempxNodeList(0), True)
                            xNodeList(0).ParentNode.ReplaceChild(newBook, xNodeList(0))
                        End If
                    Catch ex As Exception
                        'Do nothing
                    End Try

                    xNodeList = xd.GetElementsByTagName(Replace(Replace(ElementName, "<", ""), ">", ""))

                    Dim MyAttributes As XmlAttributeCollection
                    Dim str As String = ""
                    Dim root As XmlNode = Nothing
                    'Dim xElement As XmlElement
                    Dim CData As XmlCDataSection

                    Dim bCdata As Boolean = False
                    Dim bFound As Boolean = False

                    'First compare source and innertext, if match then assign the node to Root - Gets the correct node here
                    Dim bSourceFound As Boolean = False
                    Dim cloneNode As XmlNode = Nothing
                    Dim XElement As XmlElement = Nothing
                    Dim vLang As String = "lang"
                    Dim i As Integer = 0

                    Do Until xNodeList.Count - 1 < i 'As node will be increased when adding clone nodes, so get updated nodelist for every loop

                        bFound = False
                        'If source is lengthy string then concatenate,
                        'to concatenate, Clear all carriages and spaces from source and innertext from element to match 
                        If Check_enUS_Attribute_Available(xNodeList(i), Replace(Replace(ElementName, "<", ""), ">", "")) Then
                            If ExtractedTextNeedsTranslation(xNodeList(i).InnerText, ElementName, lang, xNodeList, xNodeList(i)) Then
                                If xNodeList(i).InnerText.Trim <> "" Then
                                    Dim clrNodeSource As String = GetPlainText(LCase(xNodeList(i).InnerText))
                                    root = xNodeList(i)
                                    For m As Integer = 0 To objXliff.Source.Count - 1

                                        Dim clrSource As String = GetPlainText(LCase(objXliff.Source(m)))

                                        If bSourceFound <> True Then
                                            If (LCase(objXliff.Source(m)) = LCase(xNodeList(i).InnerText) Or clrSource = clrNodeSource) And objXliff.Translation(m).ToString.Trim <> String.Empty Then
                                                cloneNode = root.Clone
                                                bSourceFound = True
                                                'Based on the nodes parentnode, check the element inside the particular node.
                                                'Next check for lang, if lang found, translation is available, then exit and save, else put the translation back
                                                If bSourceFound Then
                                                    For j As Integer = 0 To root.ParentNode.ChildNodes.Count - 1
                                                        If root.ParentNode.ChildNodes(j).Name = Replace(Replace(ElementName, "<", ""), ">", "") Then
                                                            Dim subNode As XmlNode = root.ParentNode.ChildNodes(j)
                                                            If subNode.Attributes.Count > 0 Then
                                                                MyAttributes = subNode.Attributes
                                                                Dim att As XmlAttribute

                                                                'commented this, Earlier working code ----------------------------------------------------------
                                                                'If bLanguage = True And bXmlLang = False Then
                                                                '    vLang = "lang"
                                                                'ElseIf bLanguage = False And bXmlLang = True Then
                                                                '    vLang = "xml:lang"
                                                                'Else
                                                                '    vLang = "lang"
                                                                'End If

                                                                vLang = "lang"
                                                                IsHypon = False ' TO check lang value. For example : "zh_CN" or "zh-CN", then adapt accordingly.
                                                                For Each att In MyAttributes
                                                                    If InStr(LCase(att.Name), vLang) > 0 Then
                                                                        vLang = att.Name  'New logic to above implemented for above commented code ----------------------------------------------------------
                                                                        Dim attLang As String = att.Value.Replace("-", "_")
                                                                        If att.Value.Contains("-") Then
                                                                            IsHypon = True
                                                                        End If
                                                                        If InStr(LCase(attLang), LCase(lang)) > 0 Then
                                                                            bFound = True
                                                                            If Trim(subNode.InnerText) = "" Then
                                                                                XElement = subNode
                                                                            End If
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
                                                Else
                                                    root = xNodeList(0)
                                                End If

                                                '__________________________________________________________________________________________________________________________________________________
                                                'Earlier working code by creating xmlelement. Now this is replaced by below code which uses xmlclone functionality. Commented on 19-01-2015
                                                'If bFound <> True Then
                                                '    xElement = xd.CreateElement(ElementName)
                                                '    xElement.SetAttribute("lang", lang)
                                                '    If InStr(LCase(root.InnerXml), "cdata") > 0 Then
                                                '        CData = xd.CreateCDataSection(Translation)
                                                '        xElement.AppendChild(CData)
                                                '    Else
                                                '        xElement.InnerText = Translation
                                                '    End If
                                                '    root.ParentNode.InsertAfter(xElement, root)
                                                '    str = ""
                                                '    root = Nothing
                                                'End If

                                                'The element in enUS has some attributes, they must be replicated in other languages. Edited on 19-01-2015
                                                '(example 5.11.xml)
                                                If bFound <> True Then
                                                    Dim xatt As XmlAttribute
                                                    xatt = xd.CreateAttribute(vLang)
                                                    If IsHypon Then
                                                        lang = lang.Replace("_", "-")
                                                    End If
                                                    xatt.Value = lang

                                                    If cloneNode.Attributes.Count > 0 Then
                                                        Dim blangFoundhere As Boolean = False
                                                        For k As Integer = 0 To cloneNode.Attributes.Count - 1
                                                            If LCase(cloneNode.Attributes(k).Name) = vLang Then
                                                                cloneNode.Attributes.SetNamedItem(xatt)
                                                                blangFoundhere = True
                                                            End If
                                                        Next
                                                        If blangFoundhere <> True Then
                                                            cloneNode.Attributes.InsertAfter(xatt, cloneNode.Attributes(0))
                                                        End If
                                                    Else
                                                        cloneNode.Attributes.SetNamedItem(xatt)
                                                    End If

                                                    If InStr(LCase(root.InnerXml), "cdata") > 0 Then
                                                        CData = xd.CreateCDataSection(objXliff.Translation(m))
                                                        cloneNode.ReplaceChild(CData, cloneNode.ChildNodes(0))
                                                    Else
                                                        cloneNode.InnerText = objXliff.Translation(m)
                                                    End If

                                                    root.ParentNode.InsertAfter(cloneNode, root)
                                                    str = ""
                                                    root = Nothing
                                                ElseIf Not XElement Is Nothing Then

                                                    If InStr(LCase(root.InnerXml), "cdata") > 0 Then
                                                        CData = XElement.ChildNodes(0)
                                                        CData.InnerText = objXliff.Translation(m)
                                                    Else
                                                        XElement.InnerText = objXliff.Translation(m)
                                                    End If
                                                    root.ParentNode.InsertAfter(XElement, root)
                                                    str = ""
                                                    root = Nothing
                                                End If
                                            End If
                                        End If
                                        If bSourceFound Then
                                            Exit For
                                        End If
                                    Next
                                End If
                                '_____________________________________________________________________________________________________________________________________________________
                                If bSourceFound <> True And xNodeList(i).InnerText.Trim <> "" Then
                                    'If ExtractedTextNeedsTranslation(xNodeList(i).InnerText, ElementName, lang, xNodeList, root) Then
                                    counter += 1
                                    notFound.Add(counter & ". Source value -> " & xNodeList(i).InnerText)
                                    'End If
                                End If
                            End If

                        End If

                        bSourceFound = False

                        i += 1
                        xNodeList = xd.GetElementsByTagName(Replace(Replace(ElementName, "<", ""), ">", ""))
                    Loop

                    xd.Save(xmlFile)
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                End Try
            End If
        Next

    End Sub


#End Region

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
                    If att.Value.ToLower = "en_us" Or att.Value = "enus" Or att.Value.ToLower = "en-us" Then
                        Return True
                    End If
                Next
            End If
        End If

        Return False
    End Function

    Public Function CheckElementisPresentInDefinition(ByVal elementName As String, ByVal definition() As String) As Integer

        Dim MyElement As String = elementName

        If Microsoft.VisualBasic.Right(MyElement, 1) <> ">" And Microsoft.VisualBasic.Left(MyElement, 1) <> "<" Then
            MyElement = "<" & MyElement & ">"
        End If


        For i As Integer = 1 To definition.Count - 1
            Dim def As String = definition(i)
            If Microsoft.VisualBasic.Right(def, 1) <> ">" And Microsoft.VisualBasic.Left(def, 1) <> "<" Then
                def = "<" & def & ">"
            End If
            If LCase(MyElement) = LCase(def) Then
                Return i
            End If
        Next
        Return -1
    End Function

    Dim Tags As New ArrayList

    Function GetTranslatedContent(ByVal xliffdata As sXliff, ByVal source As String) As String
        Try
            Dim NewSource As String = GetPlainText(LCase(source))
            For i As Integer = 0 To xliffdata.ID.Count - 1
                Dim xliffsource As String = GetPlainText(xliffdata.Source(i)).ToLower
                If (NewSource = xliffsource) Or xliffdata.Source(i).ToString.Trim.ToLower = source.Trim.ToLower Then
                    Return xliffdata.Translation(i)
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return ""
    End Function

    Private Function ExtractedTextNeedsTranslation(ByVal source As String, ByVal ElementName As String, ByVal lang As String, ByVal xNodelist As XmlNodeList, ByVal root As XmlNode) As Boolean

        Dim bNeedTransation As Boolean = False
        Try

            Dim MyAttributes As XmlAttributeCollection
            Dim bCdata As Boolean = False
            Dim bFound As Boolean = False

            Dim MyElement As String = ElementName

            If Microsoft.VisualBasic.Right(ElementName, 1) <> ">" And Microsoft.VisualBasic.Left(ElementName, 1) <> "<" Then
                MyElement = "<" & ElementName & ">"
            End If

            'Based on the nodes parentnode, check the element inside the particular node.
            'Next check for lang, if lang found, translation is available, then exit and save, else put the translation back

            For j As Integer = 0 To root.ParentNode.ChildNodes.Count - 1
                If "<" & root.ParentNode.ChildNodes(j).Name & ">" = MyElement Then
                    Dim subNode As XmlNode = root.ParentNode.ChildNodes(j)
                    If subNode.Attributes.Count > 0 Then
                        MyAttributes = subNode.Attributes
                        Dim att As XmlAttribute
                        For Each att In MyAttributes

                            If InStr(LCase(att.Name), "lang") > 0 Then
                                Dim attLang As String = att.Value.Replace("-", "_")
                                Debug.Print(attLang)
                                If (LCase(attLang).ToString.Contains(LCase(lang)) Or LCase(attLang.Replace("_", "-")).ToString.Contains(LCase(lang))) Then 'Or (LCase(att.Value).ToString.Contains("en_us") = True) Then
                                    bFound = True
                                    Exit For
                                End If
                            End If
                        Next
                        If bFound Then
                            If subNode.InnerText = String.Empty Then
                                bFound = False 'Target node available but it is empty
                            End If
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
End Module