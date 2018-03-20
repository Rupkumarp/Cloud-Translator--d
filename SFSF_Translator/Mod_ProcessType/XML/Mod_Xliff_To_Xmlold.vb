'Imports System.Xml
'Imports System.IO
'Imports System.Xml.Schema

'Module Mod_Xliff_To_Xml

'    Public Enum TranslationType
'        Monolingual
'        Multilingual
'    End Enum

'    Function xliff_to_xml(ByVal originalfile_path As String, ByVal translated_xliff_path As String, ByVal TT As TranslationType, ByVal lang As String) As Boolean
'        Dim targetfilepath As String = ""

'        Try
'            If TT = TranslationType.Monolingual Then
'                Dim sFileName As String = System.IO.Path.GetFileName(originalfile_path)

'                'Monolingual
'                targetfilepath = Replace(originalfile_path, "01-Input-B", "05-Output")
'                targetfilepath = System.IO.Path.GetDirectoryName(targetfilepath) & "\Mono_" & lang & "\"

'                If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(targetfilepath)) <> True Then
'                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetfilepath))
'                End If

'                targetfilepath = targetfilepath & sFileName

'                File.Copy(originalfile_path, targetfilepath, True)

'            Else
'                If InStr(originalfile_path, "01-Input-B") <> 0 Then
'                    'case 1 first language
'                    targetfilepath = Replace(originalfile_path, "01-Input-B", "05-Output")

'                ElseIf InStr(originalfile_path, "05-Output") <> 0 Then
'                    'case 2 subsequent languages
'                    targetfilepath = originalfile_path
'                    originalfile_path = Replace(originalfile_path, "05-Output", "01-Input-B")

'                Else
'                    'case 3, project structure not used. Manual operations. Just add _out.
'                    targetfilepath = Path.GetDirectoryName(originalfile_path) & "\" & Path.GetFileNameWithoutExtension(originalfile_path) & ".out" & Path.GetExtension(originalfile_path)

'                End If

'                If CheckFileOrFolderExists(targetfilepath, fType.file) <> True Then
'                    Return False
'                End If

'                If CheckFileOrFolderExists(translated_xliff_path, fType.file) <> True Then
'                    Return False
'                End If
'            End If
'        Catch ex As Exception
'            Throw New Exception(ex.Message)
'        End Try


'        Try
'            Dim objXliff As New sXliff
'            Try
'                objXliff = ModHelper.load_xliff(translated_xliff_path)
'            Catch ex As Exception
'                If ModHelper.UnWrapXliffBack(translated_xliff_path) <> True Then
'                    Throw New Exception("Error UnWrapping xliff back!")
'                End If
'                objXliff = ModHelper.cvload_xliff(Application.StartupPath & "\Temp_UnWrap.xliff")
'            End Try

'            Dim objXmlDefinition As New XmlDefinition
'            objXmlDefinition.GetXmlDefinition(originalfile_path)

'            Dim ElementName As String
'            For i As Integer = 0 To objXliff.Resname.Count - 1
'                ElementName = Mid(objXliff.ID(i), InStrRev(objXliff.ID(i), "_") + 1, Len(objXliff.ID(i)))
'                Dim iElementIndex As Integer = CheckElementisPresentInDefinition(ElementName, objXmlDefinition.Definitions)
'                If iElementIndex <> -1 Then
'                    If objXliff.Translation(i) <> "" Then
'                        Select Case TT
'                            Case TranslationType.Monolingual

'                                MonoLingual_Translation(targetfilepath, ElementName, objXliff.Source(i), objXliff.Translation(i), lang)

'                            Case TranslationType.Multilingual

'                                MultiLingual_Translation(targetfilepath, ElementName, objXliff.Source(i), objXliff.Translation(i), lang, objXmlDefinition.bLang(iElementIndex), objXmlDefinition.bXmlLang(iElementIndex), objXmlDefinition.bAddSuppLang)
'                        End Select
'                    End If
'                End If

'            Next
'        Catch ex As Exception
'            Throw New Exception(ex.Message)
'        End Try

'        Return True

'    End Function

'    Private Function CheckElementisPresentInDefinition(ByVal elementName As String, ByVal definition() As String) As Integer
'        For i As Integer = 1 To definition.Count - 1
'            Dim def As String = definition(i)
'            If Right(def, 1) <> ">" And Left(def, 1) <> "<" Then
'                def = "<" & def & ">"
'            End If
'            If "<" & LCase(elementName) & ">" = LCase(def) Then
'                Return i
'            End If
'        Next
'        Return -1
'    End Function

'    Sub MultiLingual_Translation(ByRef xmlFile As String, ByVal ElementName As String, ByVal Source As String, ByVal Translation As String, ByVal lang As String _
'                                  , ByVal bLanguage As Boolean, ByVal bXmlLang As Boolean, ByVal bSuppLang As Boolean)

'        Try
'            Dim xd As New Xml.XmlDocument
'            xd.XmlResolver = Nothing
'            xd.Load(xmlFile)

'            Dim xNodeList As XmlNodeList

'            Try
'                If bSuppLang Then
'                    Dim Tempxd As New Xml.XmlDocument
'                    Tempxd.XmlResolver = Nothing
'                    Dim TempxNodeList As XmlNodeList
'                    Tempxd.Load(Application.StartupPath & "\Definition\xml_Definition\Supplang.txt")
'                    TempxNodeList = Tempxd.GetElementsByTagName("supported-languages")
'                    xNodeList = xd.GetElementsByTagName("supported-languages")
'                    xd.ImportNode(xNodeList(0), True)
'                    Dim newBook As XmlNode = xd.ImportNode(TempxNodeList(0), True)
'                    xNodeList(0).ParentNode.ReplaceChild(newBook, xNodeList(0))
'                End If
'            Catch ex As Exception
'                'Do nothing
'            End Try

'            xNodeList = xd.GetElementsByTagName(Replace(Replace(ElementName, "<", ""), ">", ""))

'            Dim MyAttributes As XmlAttributeCollection
'            Dim str As String = ""
'            Dim root As XmlNode = Nothing
'            'Dim xElement As XmlElement
'            Dim CData As XmlCDataSection

'            Dim bCdata As Boolean = False
'            Dim bFound As Boolean = False

'            'First compare source and innertext, if match then assign the node to Root - Gets the correct node here
'            Dim bSourceFound As Boolean = False
'            Dim cloneNode As XmlNode = Nothing
'            Dim XElement As XmlElement = Nothing
'            Dim vLang As String = "lang"

'            For i As Integer = 0 To xNodeList.Count - 1
'                bFound = False
'                'If source is lengthy string then concatenate,
'                'to concatenate, Clear all carriages and spaces from source and innertext from element to match 

'                Dim clrSource As String = GetPlainText(LCase(Source))
'                Dim clrNodeSource As String = GetPlainText(LCase(xNodeList(i).InnerText))

'                If bSourceFound <> True Then
'                    If LCase(Source) = LCase(xNodeList(i).InnerText) Or clrSource = clrNodeSource Then
'                        root = xNodeList(i)
'                        cloneNode = root.Clone
'                        bSourceFound = True
'                        'Based on the nodes parentnode, check the element inside the particular node.
'                        'Next check for lang, if lang found, translation is available, then exit and save, else put the translation back
'                        If bSourceFound Then
'                            For j As Integer = 0 To root.ParentNode.ChildNodes.Count - 1
'                                If root.ParentNode.ChildNodes(j).Name = ElementName Then
'                                    Dim subNode As XmlNode = root.ParentNode.ChildNodes(j)
'                                    If subNode.Attributes.Count > 0 Then
'                                        MyAttributes = subNode.Attributes
'                                        Dim att As XmlAttribute

'                                        If bLanguage = True And bXmlLang = False Then
'                                            vLang = "lang"
'                                        ElseIf bLanguage = False And bXmlLang = True Then
'                                            vLang = "xml:lang"
'                                        Else
'                                            vLang = "lang"
'                                        End If
'                                        For Each att In MyAttributes
'                                            If InStr(LCase(att.Name), vLang) > 0 Then
'                                                If InStr(LCase(att.Value), LCase(lang)) > 0 Then
'                                                    bFound = True
'                                                    If Trim(subNode.InnerText) = "" Then
'                                                        XElement = subNode
'                                                    End If
'                                                    Exit For
'                                                End If
'                                            End If
'                                        Next
'                                        If bFound Then
'                                            Exit For
'                                        End If
'                                    End If
'                                End If
'                            Next
'                        Else
'                            root = xNodeList(0)
'                        End If

'                        '__________________________________________________________________________________________________________________________________________________
'                        'Earlier working code by creating xmlelement. Now this is replaced by below code which uses xmlclone functionality. Commented on 19-01-2015
'                        'If bFound <> True Then
'                        '    xElement = xd.CreateElement(ElementName)
'                        '    xElement.SetAttribute("lang", lang)
'                        '    If InStr(LCase(root.InnerXml), "cdata") > 0 Then
'                        '        CData = xd.CreateCDataSection(Translation)
'                        '        xElement.AppendChild(CData)
'                        '    Else
'                        '        xElement.InnerText = Translation
'                        '    End If
'                        '    root.ParentNode.InsertAfter(xElement, root)
'                        '    str = ""
'                        '    root = Nothing
'                        'End If

'                        'The element in enUS has some attributes, they must be replicated in other languages. Edited on 19-01-2015
'                        '(example 5.11.xml)
'                        If bFound <> True Then
'                            Dim xatt As XmlAttribute
'                            xatt = xd.CreateAttribute(vLang)
'                            xatt.Value = lang

'                            If cloneNode.Attributes.Count > 0 Then
'                                Dim blangFoundhere As Boolean = False
'                                For k As Integer = 0 To cloneNode.Attributes.Count - 1
'                                    If LCase(cloneNode.Attributes(k).Name) = vLang Then
'                                        cloneNode.Attributes.SetNamedItem(xatt)
'                                        blangFoundhere = True
'                                    End If
'                                Next
'                                If blangFoundhere <> True Then
'                                    cloneNode.Attributes.InsertAfter(xatt, cloneNode.Attributes(0))
'                                End If
'                            Else
'                                cloneNode.Attributes.SetNamedItem(xatt)
'                            End If

'                            If InStr(LCase(root.InnerXml), "cdata") > 0 Then
'                                CData = xd.CreateCDataSection(Translation)
'                                cloneNode.ReplaceChild(CData, cloneNode.ChildNodes(0))
'                            Else
'                                cloneNode.InnerText = Translation
'                            End If

'                            root.ParentNode.InsertAfter(cloneNode, root)
'                            str = ""
'                            root = Nothing
'                        ElseIf Not XElement Is Nothing Then

'                            If InStr(LCase(root.InnerXml), "cdata") > 0 Then
'                                CData = XElement.ChildNodes(0)
'                                CData.InnerText = Translation
'                            Else
'                                XElement.InnerText = Translation
'                            End If
'                            root.ParentNode.InsertAfter(XElement, root)
'                            str = ""
'                            root = Nothing
'                        End If
'                        '_____________________________________________________________________________________________________________________________________________________

'                        xd.Save(xmlFile)
'                        bSourceFound = False
'                        'Exit For
'                    End If
'                End If
'            Next
'        Catch ex As Exception
'            Throw New Exception(ex.Message)
'        End Try

'    End Sub

'    Sub MonoLingual_Translation(ByRef xmlFile As String, ByVal ElementName As String, ByVal Source As String, ByVal Translation As String, ByVal lang As String)
'        Dim xd As New Xml.XmlDocument
'        xd.XmlResolver = Nothing
'        xd.Load(xmlFile)

'        Dim xNodeList As XmlNodeList
'        'Change locale in first node itself
'        xNodeList = xd.GetElementsByTagName("sf-form")
'        Try
'            xNodeList(0).Attributes(0).Value = lang
'        Catch ex As Exception
'            Throw New Exception("Locale not found in xml")
'        End Try

'        'Now assing node element
'        xNodeList = xd.GetElementsByTagName(Replace(Replace(ElementName, "<", ""), ">", ""))

'        Dim bSourceFound As Boolean = False
'        Dim bFound As Boolean = False

'        Dim root As XmlNode = Nothing

'        For i As Integer = 0 To xNodeList.Count - 1
'            bFound = False
'            'If source is lengthy string then concatenate,
'            'to concatenate, Clear all carriages and spaces from source and innertext from element to match 

'            Dim clrSource As String = GetPlainText(LCase(Source))
'            Dim clrNodeSource As String = GetPlainText(LCase(xNodeList(i).InnerText))

'            If bSourceFound <> True Then
'                If LCase(Source) = LCase(xNodeList(i).InnerText) Or clrSource = clrNodeSource Then
'                    If InStr(LCase(xNodeList(i).InnerXml), "cdata") > 0 Then
'                        Dim CData As XmlCDataSection = xNodeList(i).ChildNodes(0)
'                        CData.InnerText = Translation
'                    Else
'                        xNodeList(i).InnerText = Translation
'                    End If
'                    Exit For
'                End If
'            End If
'        Next

'        'xd.Save("C:\Users\C5195092\Desktop\SFSF\Input\xml\dump.xml") 'make dynamice file later
'        'xmlFile = "C:\Users\C5195092\Desktop\SFSF\Input\xml\dump.xml"


'        xd.Save(xmlFile)

'    End Sub

'#Region "Previous partial working code for reference"
'    'Sub InsertTranslation(ByRef xmlFile As String, ByVal ElementName As String, ByVal Source As String, ByVal Translation As String, ByVal lang As String)

'    '    Dim xd As New Xml.XmlDocument
'    '    xd.XmlResolver = Nothing
'    '    xd.Load(xmlFile)

'    '    Dim xNodeList As XmlNodeList
'    '    xNodeList = xd.GetElementsByTagName(Replace(Replace(ElementName, "<", ""), ">", ""))

'    '    Dim MyAttributes As XmlAttributeCollection
'    '    Dim str As String = ""
'    '    Dim root As XmlNode = Nothing
'    '    Dim xElement As XmlElement
'    '    Dim CData As XmlCDataSection

'    '    Dim bCdata As Boolean = False

'    '    Dim bFound As Boolean = False

'    '    'For i As Integer = 0 To xNodeList.Count - 1
'    '    '    bFound = False
'    '    '    'Clear all carriages and spaces from source and innertext from element to match 
'    '    '    Dim clrSource As String = System.Text.RegularExpressions.Regex.Replace(LCase(Source), "[ ]{2,}", "")
'    '    '    Dim clrNodeSource As String = System.Text.RegularExpressions.Regex.Replace(LCase(xNodeList(i).InnerText), "[ ]{2,}", "")
'    '    '    clrSource = System.Text.RegularExpressions.Regex.Replace(clrSource, "\s+", "")
'    '    '    clrNodeSource = System.Text.RegularExpressions.Regex.Replace(clrNodeSource, "\s+", "")

'    '    '    If LCase(Source) = LCase(xNodeList(i).InnerText) Or clrSource = clrNodeSource Then
'    '    '        root = xNodeList(i)
'    '    '        If xNodeList(i).Attributes.Count > 0 Then
'    '    '            MyAttributes = xNodeList(i).Attributes
'    '    '            Dim att As XmlAttribute
'    '    '            For Each att In MyAttributes
'    '    '                If InStr(att.Name, "lang") > 0 Then
'    '    '                    If InStr(LCase(att.Value), LCase(lang)) > 0 Then
'    '    '                        bFound = True
'    '    '                        Exit For
'    '    '                    End If
'    '    '                End If
'    '    '            Next
'    '    '        Else
'    '    '            Exit For 'Found
'    '    '        End If
'    '    '    End If
'    '    'Next


'    '    'First compare source and innertext, if match then assign the node to Root
'    '    Dim bSourceFound As Boolean = False

'    '    For i As Integer = 0 To xNodeList.Count - 1
'    '        bFound = False
'    '        'If source is lengthy string then concatenate,
'    '        'to concatenate, Clear all carriages and spaces from source and innertext from element to match 
'    '        Dim clrSource As String = System.Text.RegularExpressions.Regex.Replace(LCase(Source), "[ ]{2,}", "")
'    '        Dim clrNodeSource As String = System.Text.RegularExpressions.Regex.Replace(LCase(xNodeList(i).InnerText), "[ ]{2,}", "")
'    '        clrSource = System.Text.RegularExpressions.Regex.Replace(clrSource, "\s+", "")
'    '        clrNodeSource = System.Text.RegularExpressions.Regex.Replace(clrNodeSource, "\s+", "")

'    '        If bSourceFound <> True Then
'    '            If LCase(Source) = LCase(xNodeList(i).InnerText) Or clrSource = clrNodeSource Then
'    '                root = xNodeList(i)
'    '                bSourceFound = True
'    '                Exit For
'    '            End If
'    '        End If
'    '    Next

'    '    'Next check for lang, if lang then translation available already, then exit save else put the translation
'    '    If bSourceFound Then
'    '        For i As Integer = 0 To xNodeList.Count - 1
'    '            If xNodeList(i).Attributes.Count > 0 Then
'    '                MyAttributes = xNodeList(i).Attributes
'    '                Dim at As XmlAttribute
'    '                For Each at In MyAttributes
'    '                    If InStr(LCase(at.Name), "lang") > 0 Then
'    '                        If InStr(LCase(at.Value), LCase(lang)) > 0 Then
'    '                            bFound = True
'    '                            Exit For
'    '                        End If
'    '                    End If
'    '                Next
'    '            End If
'    '            If bFound Then
'    '                Exit For
'    '            End If
'    '        Next
'    '    Else
'    '        root = xNodeList(0)
'    '    End If





'    '    If bFound <> True Then
'    '        xElement = xd.CreateElement(ElementName)
'    '        xElement.SetAttribute("lang", lang)
'    '        If InStr(LCase(root.InnerXml), "cdata") > 0 Then
'    '            CData = xd.CreateCDataSection(Translation)
'    '            xElement.AppendChild(CData)
'    '        Else
'    '            xElement.InnerText = Translation
'    '        End If
'    '        root.ParentNode.InsertAfter(xElement, root)
'    '        str = ""
'    '        root = Nothing
'    '    End If

'    '    xd.Save("C:\Users\C5195092\Desktop\SFSF\Input\xml\dump.xml")
'    '    xmlFile = "C:\Users\C5195092\Desktop\SFSF\Input\xml\dump.xml"

'    'End Sub
'#End Region

'End Module