Imports System.Xml
Imports System.IO
Imports System.Xml.Schema
Imports System.Xml.XPath


Module Mod_Xml_ToXliff

    Public Enum fType
        file
        folder
    End Enum

    Dim Definitions() As String
    Dim enTags As ArrayList
    Dim TagName As ArrayList
    Dim trTags As ArrayList

    Public Function CheckFileOrFolderExists(ByVal sPath As String, ByVal sType As fType) As Boolean
        Try
            Select Case sType
                Case fType.file
                    If System.IO.File.Exists(sPath) <> True Then
                        Throw New Exception("Path not found!" & vbNewLine & sPath)
                    End If
                Case fType.folder
                    If System.IO.Directory.Exists(sPath) <> True Then
                        Throw New Exception("Path not found!" & vbNewLine & sPath)
                    End If
            End Select
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function

    Public Function Xml_ToXliff(ByVal XmlFile As String, ByVal xliff_Path As String, ByVal Targetlanguage As String, Optional ByVal ExtractWithTranslationOnly As Boolean = False) As Boolean

        Try
            If CheckFileOrFolderExists(XmlFile, fType.file) <> True Then
                Return False
            End If

            If CheckFileOrFolderExists(xliff_Path, fType.folder) <> True Then
                Return False
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        enTags = New ArrayList
        TagName = New ArrayList
        trTags = New ArrayList

        xliff_Path = xliff_Path & System.IO.Path.GetFileNameWithoutExtension(XmlFile) & "_" & Targetlanguage & ".xliff"

        Try
            Dim objXmlDefiniton As New XmlDefinition
            objXmlDefiniton.GetXmlDefinition(XmlFile)

            Definitions = objXmlDefiniton.Definitions

            Dim Counter As Integer = 0

            'Write xliff---------------------------------------------------------------------------------------------------------------------------------------
            Using Writer As StreamWriter = New StreamWriter(xliff_Path, False, System.Text.Encoding.UTF8)
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

                For i As Integer = 1 To UBound(Definitions) - 1
                    If Definitions(i) <> "" Then
                        Dim myNum As Integer = myNum + 1
                        Extract(XmlFile, Definitions(i), Targetlanguage, ExtractWithTranslationOnly)
                        For j As Integer = 0 To enTags.Count - 1
                            If enTags(j) <> "" Then
                                Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & (Replace(Replace((Definitions(i)), "<", ""), ">", "") & Chr(34) & " resname=" & Chr(34) & clean_xml(objXmlDefiniton.Position) & Chr(34) & ">"))
                                Writer.WriteLine("<source>" & (wrap_html(clean_xml(enTags(j)))) & "</source>")

                                If objXmlDefiniton.bCP And Left(Definitions(i), 1) = "%" Then 'This is used to avoid CP translatin headers, so when writing xliff, copy source to translate tag, % will identify this as header or not
                                    Writer.WriteLine("<target state=""needs-review-translation"">" & wrap_html(clean_xml(enTags(j))) & "</target>")
                                Else
                                    If ExtractWithTranslationOnly Then 'To Import existing translation to DB
                                        Writer.WriteLine("<target state=""needs-review-translation"">" & wrap_html(clean_xml(trTags(j))) & "</target>")
                                    Else
                                        Writer.WriteLine("<target state=""needs-review-translation""></target>")
                                    End If
                                End If

                                Writer.WriteLine("<note from=""Developer"" priority =""10"">" & (objXmlDefiniton.Position) & ": " & (Replace(Replace(Definitions(i), "<", ""), ">", "") & "</note>"))
                                Writer.WriteLine("</trans-unit>")
                                Writer.WriteLine(vbCrLf)
                                myNum = myNum + 1
                                Counter += 1
                            End If
                        Next
                        myNum = myNum - 1
                    End If
                Next

                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

            End Using

            If Counter = 0 Then
                Try
                    System.IO.File.Delete(xliff_Path)
                    Return False
                Catch ex As Exception
                    Return False
                End Try
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function

#Region "Using xmldocument with Getelementsbytagname - this gives the data in correct flow"

    Sub Extract(ByVal xmlFile As String, ByVal ElementName As String, ByVal lang As String, ByVal ExtractWithTranslationOnly As Boolean)
        ' ElementName = clean_element(Replace(Replace(ElementName, "<", ""), ">", ""))
        Try
            enTags.Clear()
            trTags.Clear()
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing

            '   Dim xhtmlstring As String = IO.File.ReadAllText(xmlFile)

            'xhtmlstring = XmlConvert.EncodeName(xhtmlstring)

            xd.Load(xmlFile)

            Dim xNodeList As XmlNodeList
            xNodeList = xd.GetElementsByTagName(Replace(Replace(ElementName, "<", ""), ">", ""))

            ' xNodeList = xd.GetElementsByTagName("h1")

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
                            Dim attLang As String = att.Value.Replace("-", "_")
                            If LCase(attLang).ToLower = "en_us" Then
                                str = xNodeList(i).InnerText
                                Exit For
                            End If
                        End If
                    Next

                    'For Each att In MyAttributes 'AML
                    '    If InStr(att.Name, "TextValue") > 0 Then
                    '        blangFound = True
                    '        str = att.Value
                    '        Exit For
                    '    End If
                    'Next

                    If str = "" And blangFound <> True Then
                        str = xNodeList(i).InnerText
                    End If
                Else
                    str = xNodeList(i).InnerText
                End If
                If str <> "" Then
                    If Right(ElementName, 1) = ">" And Left(ElementName, 1) = "<" Then
                        ElementName = ElementName
                    Else
                        ElementName = "<" & ElementName & ">"
                    End If

                    If ExtractWithTranslationOnly Then
                        Dim trContent As String = GetTranslation(str, ElementName, lang, xNodeList, xNodeList(i))
                        If trContent.Trim <> String.Empty Then
                            enTags.Add(str)
                            trTags.Add(trContent)
                        End If
                    Else
                        If ExtractedTextNeedsTranslation(str, ElementName, lang, xNodeList, xNodeList(i)) Then
                            enTags.Add(str)
                        End If
                    End If
                End If
                str = ""
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


    End Sub

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
                                Dim attLang As String = att.Value.Replace("-", "_")
                                If InStr(LCase(attLang).ToLower, LCase(lang)) > 0 Then
                                    If subNode.InnerText.Trim <> "" Then
                                        bFound = True
                                        Exit For
                                    End If
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

    ''' <summary>
    ''' This is required to get the translation out from Input files, we can then update DB Importing existing translation only
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="ElementName"></param>
    ''' <param name="lang"></param>
    ''' <param name="xNodelist"></param>
    ''' <param name="root"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTranslation(ByVal source As String, ByVal ElementName As String, ByVal lang As String, ByVal xNodelist As XmlNodeList, ByVal root As XmlNode) As String
        Dim bNeedTransation As Boolean = False
        Try

            Dim MyAttributes As XmlAttributeCollection
            Dim bCdata As Boolean = False

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
                                Dim attLang As String = att.Value.Replace("-", "_")
                                If InStr(LCase(attLang).ToLower, LCase(lang)) > 0 Then
                                    If subNode.InnerText.Trim <> "" Then
                                        Return subNode.InnerText
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
            Next

        Catch ex As Exception
            Throw New Exception("Error @ExtractedTextNeedsTranslation" & vbNewLine & ex.Message)
        End Try
        Return ""
    End Function

#End Region 'Final working code







#Region "Code for reference - Earlier partially working"
#Region "Using Dataset as reader - This works for some xml only"
    'Private Function GetTagList(ByVal xmlFile As String) 'Error reading xml - "a column named <fm-sect-intro> already belongs to this datatable"
    '    'Using dataset readxml cell by cell
    '    Dim settingsw As New XmlReaderSettings()
    '    settingsw.DtdProcessing = DtdProcessing.Ignore
    '    settingsw.ValidationType = ValidationType.DTD

    '    Dim data As String = Nothing
    '    Dim validatingReader As XmlReader = XmlReader.Create(File.Open(xmlFile, FileMode.Open), settingsw, data)

    '    Dim ds As New DataSet()
    '    ds.ReadXml(validatingReader)

    '    Dim def As String = ""
    '    Dim str As String = ""

    '    For i As Integer = 1 To UBound(Definitions)
    '        def = Replace(Replace(Definitions(i), ">", ""), "<", "")
    '        For j As Integer = 0 To ds.Tables.Count - 1
    '            For k As Integer = 0 To ds.Tables(j).Columns.Count - 1
    '                If LCase(def) = LCase(ds.Tables(j).Columns(k).ColumnName) Or LCase(def & "_text") = LCase(ds.Tables(j).Columns(k).ColumnName) Then
    '                    Dim langColNumber As Integer = GetlangColNumber(ds.Tables(j), "lang")
    '                    For x As Integer = 0 To ds.Tables(j).Rows.Count - 1
    '                        If LCase(ds.Tables(j).Columns(langColNumber).ColumnName) = "lang" Then
    '                            If IsDBNull(ds.Tables(j).Rows(x).Item(langColNumber)) Then
    '                                Tags.Add(ds.Tables(j).Rows(x).Item(k))
    '                                TagName.Add(def)
    '                            End If
    '                        Else
    '                            Tags.Add(ds.Tables(j).Rows(x).Item(k))
    '                            TagName.Add(def)
    '                        End If

    '                    Next
    '                End If

    '            Next
    '        Next
    '    Next
    'End Function

    'Private Function GetlangColNumber(ByVal DT As DataTable, ByVal colname As String) As Integer
    '    For i As Integer = 0 To DT.Columns.Count - 1
    '        If LCase(DT.Columns(i).ColumnName) = colname Then
    '            Return i
    '        End If
    '    Next
    'End Function
#End Region

#Region "Using XMLREADeR"
    ''This doesnt work if the element has cdata, to read cdata use readxml.readinnerxml, it moves out of the element for cdata, so if the element has attributes it will be skipped.
    'Private Function GetXMltext(ByVal xmlFile As String, ByVal ElementName As String) As ArrayList
    '    Dim Tags As New ArrayList

    '    Dim xmlnode As String
    '    Using s As StreamReader = New StreamReader(xmlFile)
    '        xmlnode = s.ReadToEnd
    '    End Using

    '    Dim settings As XmlReaderSettings = New XmlReaderSettings()
    '    settings.DtdProcessing = DtdProcessing.Ignore
    '    settings.ValidationType = ValidationType.None
    '    AddHandler settings.ValidationEventHandler, AddressOf ValidationCallBack

    '    Dim str As String = ""
    '    Dim tmpXML As XmlReader
    '    Dim readXML As XmlReader = XmlReader.Create(New StringReader(xmlnode), settings)
    '    While readXML.Read()
    '        Select Case readXML.NodeType
    '            Case XmlNodeType.Element
    '                'If ElementName = "<field-label>" Then
    '                '    MsgBox("")
    '                'End If

    '                'If "<" & readXML.Name & ">" = "<field-label>" Then
    '                '    MsgBox("")
    '                'End If
    '                If "<" & readXML.Name & ">" = ElementName Then
    '                    Dim bLang As Boolean = False
    '                    Dim bFound As Boolean = False
    '                    tmpXML = readXML
    '                    If "<" & readXML.Name & ">" = ElementName And readXML.HasAttributes = False Then
    '                        str = GetWithoutCdata(readXML.ReadInnerXml)
    '                    Else
    '                        'str = readXML.ReadInnerXml
    '                        Do While readXML.MoveToNextAttribute()
    '                            If LCase(readXML.Name) = "lang" Then
    '                                bLang = True
    '                                If LCase(readXML.Value) = "en_us" Then
    '                                    bFound = True
    '                                    str = GetWithoutCdata(readXML.Value)
    '                                    Exit Do
    '                                End If
    '                            End If
    '                        Loop
    '                    End If

    '                    If bLang = True And bFound <> True Then
    '                        str = GetWithoutCdata(tmpXML.ReadInnerXml)
    '                    End If
    '                    Tags.Add(str)
    '                End If
    '                Exit Select
    '            Case XmlNodeType.Text
    '                Exit Select
    '            Case XmlNodeType.EndElement
    '                Exit Select
    '        End Select
    '    End While

    '    Return Tags
    'End Function
#End Region

    Private Function GetWithoutCdata(ByVal str As String) As String
        Try
            If InStr(LCase(str), "cdata") Then
                Dim iStart As Integer = InStr(str, "A[") + 2
                Dim iEnd As Integer = InStr(str, "]")
                str = Mid(str, iStart, iEnd - iStart)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return str
    End Function

    ' Display any validation errors.
    Private Sub ValidationCallBack(sender As Object, e As ValidationEventArgs)
        Throw New Exception(e.Message)
    End Sub

#Region "Using xmldocument recursiverly" 'this works but need to do more testing
    Sub xmldoc(ByVal xmlFile As String, ByVal ElementName As String)
        Try
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(xmlFile)
            childs(xd, ElementName)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Dim sl As New Collections.Specialized.StringCollection


    Sub childs(ByVal xn As Xml.XmlNode, ByVal ElementName As String)
        Dim str As String = ""
        Dim bfound As Boolean = False
        Dim bLang As Boolean = False

        Try
            If xn.HasChildNodes = True Then
                For Each ch As Xml.XmlNode In xn.ChildNodes
                    childs(ch, ElementName)
                Next
            End If

            For i As Integer = 1 To UBound(Definitions) - 1
                If "<" & xn.Name & ">" = Definitions(i) Then
                    'If Definitions(i) = "<label>" Then
                    '    MsgBox("")
                    'End If
                    If xn.Attributes.Count > 0 Then
                        For j As Integer = 0 To xn.Attributes.Count - 1
                            If InStr(LCase(xn.Attributes(j).Name), "lang") > 0 Then
                                bLang = True
                                If LCase(xn.Attributes(j).Value) = "en_us" Then
                                    str = xn.InnerText
                                    bfound = True
                                    Exit For
                                End If
                            End If
                        Next
                        If bfound <> True And bLang <> True Then
                            str = xn.InnerText
                        End If
                    Else
                        str = xn.InnerText
                    End If

                    enTags.Add(str)
                    TagName.Add(Definitions(i))
                    bfound = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

#End Region
#End Region

End Module
