Imports System.Xml
Imports System.IO
Imports System.Text
Imports System.Net
Imports System.Xml.Resolvers
Module Mod_RMK_xhtml

    Function RMK_to_xliff(ByVal xhtml_path As String, ByVal xliff_SavePath As String, ByVal TargetLang As String) As String

        Try
            Dim RmkExtractor As New RMK_Extractor
            Dim RD As RMKDetails = RmkExtractor.Extract(xhtml_path)

            Dim xliffRmk As String = xliff_SavePath & System.IO.Path.GetFileNameWithoutExtension(xhtml_path) & "_" & TargetLang & ".xliff"

            Return CreateXliff(RD, xliffRmk, TargetLang)
        Catch ex As Exception
            Throw New Exception("Error @RMK_to_xliff" & vbNewLine & ex.Message)
        End Try

    End Function

    Sub xliff_to_RMK(ByVal xhtml_Path As String, ByVal sXliff As String, ByVal TargetLang As String)
        Try

            Dim targetfilepath As String

            Dim sFileName As String = System.IO.Path.GetFileName(xhtml_Path)

            'Monolingual
            targetfilepath = Replace(xhtml_Path, "01-Input-B", "05-Output")
            targetfilepath = System.IO.Path.GetDirectoryName(targetfilepath) & "\RmkMono_" & TargetLang & "\"

            If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(targetfilepath)) <> True Then
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetfilepath))
            End If

            targetfilepath = targetfilepath & sFileName

            File.Copy(xhtml_Path, targetfilepath, True)

            If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(targetfilepath)) <> True Then
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetfilepath))
            End If

            Dim xliffData As sXliff = load_xliff(sXliff)
            Dim RmkExtractor As New RMK_Extractor

            'RmkExtractor.ReIntegrate(targetfilepath, xliffData)

            RmkExtractor.ReIntegrateNew(targetfilepath, xliffData)

        Catch ex As Exception
            Throw New Exception("Error @xliff_to_RMK" & vbNewLine & ex.Message)
        End Try
    End Sub

    Private Function CreateXliff(ByRef RD As RMKDetails, ByVal xliff_Path As String, ByVal Targetlanguage As String) As String

        Try
            Dim myNum As Integer = 0
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


                For J As Integer = 0 To RD.enUS.Count - 1
                    myNum += 1
                    Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & Chr(34) & " resname=" & Chr(34) & "RMK_xHTML" & Chr(34) & ">")
                    'Writer.WriteLine("<trans-unit id=" & Chr(34) & clean_xml(RD.ID(J).ToString) & Chr(34) & " resname=" & Chr(34) & clean_xml(RD.resName(J).ToString) & Chr(34) & ">")
                    Writer.WriteLine("<source>" & wrap_html(clean_xml(RD.enUS(J).ToString)) & "</source>")
                    If IsNumeric(RD.enUS(J).ToString) Then
                        Writer.WriteLine("<target state=""needs-review-translation"">" & RD.enUS(J) & "</target>")
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

End Module

Public Class RMKDetails
    Public enUS As New ArrayList
    Public ID As New ArrayList
    Public resName As New ArrayList
End Class

Public Class RMK_Extractor

    Public Sub New()
        If Not (read_additional_extract(appData & DefinitionFiles.RMK_SpecialAtt)) Then Throw New Exception("Error reading RMK_Special_attributes - Continuing processing.")
        If Not (read_format_tags(appData & DefinitionFiles.RMK_Tags)) Then Throw New Exception("Error reading RMK_read_tags - Continuing processing.")
    End Sub

    Public RD As New RMKDetails

    Dim curID As Integer
    Dim cur_kept_tags As Integer
    Dim is_append As Boolean
    Dim mytext As String
    Dim depth_start_append As Integer
    Dim additional_extract_type1() As String
    Dim additional_extract_type2a() As String
    Dim additional_extract_type2b() As String
    Dim additional_extract_type3a() As String
    Dim additional_extract_type3b() As String
    Dim additional_extract_type3c() As String
    Dim formatting_tags() As String
    Dim kept_tags() As String
    Dim kept_tags_depth() As Integer
    Dim my_depth As Integer


#Region "Extract"
    Function Extract(ByVal myFile As String) As RMKDetails
        Dim xmlcontent As String
        Dim xd As New Xml.XmlDocument
        curID = 1

        xmlcontent = dbletags_xml(System.IO.File.ReadAllText(myFile))
        xmlcontent = WebUtility.HtmlDecode(xmlcontent) ' remove the encoding for e.g. copyright sign

        Try
            xd.XmlResolver = Nothing
            xd.LoadXml(xmlcontent) 'NEED TO use try and catch the error. IF there is a xml error, an exception is raised here.

            'reinit variables
            is_append = False
            my_depth = 1
            depth_start_append = 0
            cur_kept_tags = 0
            ReDim kept_tags(0)
            ReDim kept_tags_depth(0)

            extract_subchild(xd.DocumentElement.ParentNode, myFile.ToString)

            'write last if required
            If mytext <> "" Then sendtotranslation(mytext, Format(curID, "00000") & "_TextContent", "RMK_xHTML", myFile.ToString)
            mytext = ""
        Catch ex As SystemException
            Throw New Exception(myFile.ToString & ": Exception - " & ex.Message & vbCrLf)
        End Try
        Return RD
    End Function

    Private Function dbletags_xml(ByVal instring As String) As String
        instring = Replace(instring, "&amp;", "&amp;amp;")
        instring = Replace(instring, "&lt;", "&amp;lt;")
        instring = Replace(instring, "&gt;", "&amp;gt;")
        instring = Replace(instring, "&quot;", "&amp;quot;")
        dbletags_xml = Replace(instring, "&apos;", "&amp;apos;")
    End Function

    Private Function read_format_tags(ByVal RMKfile As String) As Boolean
        ReDim formatting_tags(0)
        Try
            If Not System.IO.File.Exists(RMKfile) Then Return False
            Dim tmp() As String = Split(System.IO.File.ReadAllText(RMKfile), vbCrLf)

            For Each tmpline In tmp
                If Len(tmpline) <> 0 Then
                    If Mid(tmpline, 1, 1) <> "#" Then
                        ReDim Preserve formatting_tags(UBound(formatting_tags) + 1)
                        formatting_tags(UBound(formatting_tags) - 1) = tmpline
                    End If
                End If
            Next

        Catch ex As Exception
            Return False
        End Try
        ReDim Preserve formatting_tags(UBound(formatting_tags) - 1)
        Return True
    End Function

    Private Function read_additional_extract(ByVal RMKfile As String) As Boolean

        ReDim additional_extract_type1(0)
        ReDim additional_extract_type2a(0)
        ReDim additional_extract_type2b(0)
        ReDim additional_extract_type3a(0)
        ReDim additional_extract_type3b(0)
        ReDim additional_extract_type3c(0)
        'return true if ok
        If Not System.IO.File.Exists(RMKfile) Then Return False
        Dim tmp() As String = Split(System.IO.File.ReadAllText(RMKfile), vbCrLf)

        Try
            For Each tmpline In tmp
                If Len(tmpline) <> 0 Then
                    If Mid(tmpline, 1, 1) <> "#" Then
                        If Mid(tmpline, 1, 1) <> "%" Then Return False 'wrong file format

                        Dim tmp_entry() As String = Split(tmpline, "%")

                        Select Case Val(tmp_entry(1))
                            Case 1
                                ReDim Preserve additional_extract_type1(UBound(additional_extract_type1) + 1)
                                additional_extract_type1(UBound(additional_extract_type1) - 1) = tmp_entry(2)
                            Case 2
                                ReDim Preserve additional_extract_type2a(UBound(additional_extract_type2a) + 1)
                                additional_extract_type2a(UBound(additional_extract_type2a) - 1) = tmp_entry(2)

                                ReDim Preserve additional_extract_type2b(UBound(additional_extract_type2b) + 1)
                                additional_extract_type2b(UBound(additional_extract_type2b) - 1) = tmp_entry(3)

                            Case 3
                                ReDim Preserve additional_extract_type3a(UBound(additional_extract_type3a) + 1)
                                additional_extract_type3a(UBound(additional_extract_type3a) - 1) = tmp_entry(2)
                                ReDim Preserve additional_extract_type3b(UBound(additional_extract_type3b) + 1)
                                additional_extract_type3b(UBound(additional_extract_type3b) - 1) = tmp_entry(3)
                                ReDim Preserve additional_extract_type3c(UBound(additional_extract_type3c) + 1)
                                additional_extract_type3c(UBound(additional_extract_type3c) - 1) = tmp_entry(4)
                            Case Else

                                Return False 'invalid selection

                        End Select


                    End If
                End If
            Next

        Catch ex As Exception
            Return False
        End Try

        Return True

    End Function

    Private Function repeatchar(ByVal nbr As Integer, ByVal mychar As String) As String
        Dim tmp As String = ""
        For f = 1 To nbr
            tmp = tmp & mychar
        Next
        Return tmp

    End Function

    Private Function get_quotedcontent(ByVal myvalue As String) As String
        If InStr(myvalue, "'") = 0 Then Return ""
        Dim splitstring() As String = Split(myvalue, "'")
        Return splitstring(1)

    End Function

    Private Function has_textchild(ByVal subchildnode As Xml.XmlNode) As Boolean
        Try
            'return true if a child of the first lower level has a text. 
            'non recursive!
            For Each mychildnode In subchildnode.ChildNodes
                If mychildnode.name = "#text" Then Return True
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return False

    End Function

    Private Function has_notextchild(ByVal subchildnode As Xml.XmlNode) As Boolean
        Try
            'non recursive!
            For Each mychildnode In subchildnode.ChildNodes
                If mychildnode.name <> "#text" And is_format_tag(mychildnode.name) Then Return True

            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        'return true if a child of the first lower level has a non text. 

        Return False

    End Function

    Private Function is_format_tag(ByVal mytag As String) As Boolean

        Try
            Dim result As String = Array.Find(formatting_tags, Function(s) s.Equals(mytag))

            If result <> "" Then
                Return True
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


        Return False

    End Function

    Private Function is_format_tag_old(ByVal subchildnode As Xml.XmlNode) As Boolean
        Try
            If subchildnode.HasChildNodes Then
                Dim result As String = Array.Find(formatting_tags, Function(s) s.Equals(subchildnode.FirstChild.Name))
                If result <> "" Then Return True
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return False
    End Function

    Dim TagArr As New ArrayList

    Private Sub extract_subchild(ByVal subchildnode As Xml.XmlNode, ByVal curfilename As String)

        Dim PreserveSubChildNode As Xml.XmlNode = subchildnode

        Try
            If is_append Then
                If my_depth <= depth_start_append Then
                    'check if we can stop appending and thus write the text
                    sendtotranslation(mytext, Format(curID, "00000") & "_TextContent", "RMK_xHTML", curfilename)
                    is_append = False
                    mytext = ""
                    TagArr = New ArrayList
                    cur_kept_tags = 0

                End If
            End If

            If Not (is_append) And has_textchild(subchildnode) And has_notextchild(subchildnode) Then
                If my_depth >= depth_start_append Then
                    is_append = True
                    depth_start_append = my_depth
                End If
            End If


            'let's put in append mode if we have both text & non text children
            'If Not (is_append) And has_textchild(subchildnode) And has_notextchild(subchildnode) Then
            ' is_append = True
            ' depth_start_append = my_depth
            '    End If

            'add the text to variable or write it if not append. ***************************************
            For Each mychildnode In subchildnode.ChildNodes
                If is_format_tag(mychildnode.name) Then

                    If is_append And my_depth >= depth_start_append Then
                        kept_tags(cur_kept_tags) = mychildnode.name
                        kept_tags_depth(cur_kept_tags) = my_depth

                        If cur_kept_tags = UBound(kept_tags) Then
                            'increase the size of the array if required
                            ReDim Preserve kept_tags(cur_kept_tags + 1)
                            ReDim Preserve kept_tags_depth(cur_kept_tags + 1)
                        End If

                        cur_kept_tags = cur_kept_tags + 1
                        mytext = mytext & "<" & mychildnode.name & ">"
                        TagArr.Add(mychildnode)
                    End If

                End If


                If mychildnode.name = "#text" And Len(mychildnode.innertext) > 0 Then

                    'closing tags if same level
                    If is_append And cur_kept_tags > 0 Then
                        If my_depth = kept_tags_depth(cur_kept_tags - 1) Then
                            mytext = mytext & "</" & kept_tags(cur_kept_tags - 1) & ">"
                            cur_kept_tags = cur_kept_tags - 1
                        End If
                    End If

                    If Len(mychildnode.innertext) = 1 OrElse Strings.Left(mychildnode.innertext, 2) <> "#{" Then
                        'avoid testing left x,2 for a single char string. orelse just evaluates first part.
                        'strings.left ... do not take internal variables which start with #{  ===> 1 case whre the variable is not starting. We could put instr instead. Need to dble check.

                        If is_append Then
                            mytext = mytext & mychildnode.innertext
                            TagArr.Add(mychildnode)
                        Else
                            TagArr.Add(mychildnode)
                            sendtotranslation(mychildnode.innertext, Format(curID, "00000") & "_TextContent", "RMK_xHTML", curfilename)
                        End If

                    End If
                End If


                'Adding attributes - don't append in all cases *****************************************
                If Not (IsNothing(mychildnode.attributes)) Then
                    'check if any other thing than text should be translated
                    For Each mychildnodeattribute In mychildnode.attributes

                        'we'll have to put this very flexible in a config file. We see 3 different patterns
                        'Simple attribute value
                        'attribute value call param
                        'attribute value condition (similar to previous, but we also put constrain on childnodename, not only on childnodeattribute)

                        If UBound(additional_extract_type1) > 0 Then
                            For f = 0 To UBound(additional_extract_type1) - 1
                                If LCase(mychildnodeattribute.name) = additional_extract_type1(f) And Len(mychildnodeattribute.value) > 0 Then
                                    sendtotranslation(mychildnodeattribute.value, Format(curID, "00000") & "_" & additional_extract_type1(f), "RMK_xHTML", curfilename)
                                End If
                            Next
                        End If

                        If UBound(additional_extract_type2a) > 0 Then
                            For f = 0 To UBound(additional_extract_type2a) - 1
                                If LCase(mychildnodeattribute.name) = additional_extract_type2a(f) Then
                                    If InStr(mychildnodeattribute.value, additional_extract_type2b(f)) <> 0 Then
                                        sendtotranslation(get_quotedcontent(mychildnodeattribute.value), Format(curID, "00000") & "_" & additional_extract_type2a(f) & "_" & additional_extract_type2b(f), "RMK_xHTML", curfilename)
                                    End If
                                End If
                            Next
                        End If

                        If UBound(additional_extract_type3a) > 0 Then
                            For f = 0 To UBound(additional_extract_type3a) - 1
                                If LCase(mychildnode.name) = additional_extract_type3c(f) And LCase(mychildnodeattribute.name) = additional_extract_type3a(f) Then
                                    If InStr(mychildnodeattribute.value, additional_extract_type3b(f)) <> 0 Then
                                        sendtotranslation(get_quotedcontent(mychildnodeattribute.value), Format(curID, "00000") & "_" & additional_extract_type3a(f) & "_" & additional_extract_type3b(f) & "_" & additional_extract_type3c(f), "RMK_xHTML", curfilename)
                                    End If
                                End If
                            Next
                        End If
                    Next

                End If

                'check recursively
                If mychildnode.haschildnodes Then
                    my_depth = my_depth + 1
                    extract_subchild(mychildnode, curfilename)

                    'closing tags when we are done with all attributes
                    If is_append And cur_kept_tags > 0 Then
                        If my_depth = kept_tags_depth(cur_kept_tags - 1) Then
                            mytext = mytext & "</" & kept_tags(cur_kept_tags - 1) & ">"
                            cur_kept_tags = cur_kept_tags - 1
                        End If
                    End If

                    my_depth = my_depth - 1

                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Private Sub sendtotranslation(ByVal TU_source As String, ByVal TU_id As String, ByVal TU_resname As String, ByVal TU_note As String)
        curID = curID + 1
        'TextBox2.Text = TextBox2.Text & Chr(9) & TU_source & "|" & TU_id & "|" & TU_resname & "|" & TU_note & vbCrLf
        RD.enUS.Add(TU_source)
        RD.ID.Add(TU_id)
        RD.resName.Add(TU_resname)
    End Sub
#End Region

#Region "ReIntegrate with Laurent's extract algorithm"

    Private Function Replace_quotedcontent(ByVal myvalue As String, ByRef objXliff As sXliff) As String
        Dim MyTranslation As String

        If InStr(myvalue, "'") <> 0 Then
            Dim splitstring() As String = Split(myvalue, "'")
            Dim sTranslation As String = Get_transaltionFromxliff(objXliff, splitstring(1))
            MyTranslation = myvalue
            MyTranslation = myvalue.Replace(splitstring(1), sTranslation)
        Else
            MyTranslation = Get_transaltionFromxliff(objXliff, myvalue)
        End If


        Return MyTranslation

    End Function

    Private Function Get_transaltionFromxliff(ByRef objXliff As sXliff, ByVal enUS As String) As String
        Dim sTranslation As String = ""
        Dim sSource As String = GetPlainText(enUS)
        For i As Integer = 0 To objXliff.Source.Count - 1
            Dim objSource As String = GetPlainText(objXliff.Source(i))
            If objSource.ToLower = sSource.ToLower Or objSource.Replace(" ", "").Trim.ToLower = sSource.Replace(" ", "").Trim.ToLower Then
                sTranslation = objXliff.Translation(i)
                Exit For
            End If
        Next
        Return sTranslation
    End Function

    Sub ReIntegrateNew(ByVal sFile As String, ByRef xliffdata As sXliff)
        Dim xmlcontent As String
        Dim xd As New Xml.XmlDocument
        curID = 1

        xmlcontent = dbletags_xml(System.IO.File.ReadAllText(sFile))
        xmlcontent = WebUtility.HtmlDecode(xmlcontent) ' remove the encoding for e.g. copyright sign

        Try
            xd.XmlResolver = Nothing
            xd.PreserveWhitespace = True
            xd.LoadXml(xmlcontent) 'NEED TO use try and catch the error. IF there is a xml error, an exception is raised here.

            'reinit variables
            is_append = False
            my_depth = 1
            depth_start_append = 0
            cur_kept_tags = 0
            ReDim kept_tags(0)
            ReDim kept_tags_depth(0)

            Rintegrate_subchild(xd.DocumentElement.ParentNode, sFile.ToString, xliffdata)

            'xd.Save(sFile)

            Beautify(xd, sFile)

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

    Private Sub Rintegrate_subchild(ByRef subchildnode As Xml.XmlNode, ByVal curfilename As String, ByRef objXliff As sXliff)

        Try
            If is_append Then
                If my_depth <= depth_start_append Then
                    'check if we can stop appending and thus write the text
                    is_append = False
                    mytext = ""
                    cur_kept_tags = 0
                End If
            End If

            If Not (is_append) And has_textchild(subchildnode) And has_notextchild(subchildnode) Then
                If my_depth >= depth_start_append Then
                    is_append = True
                    depth_start_append = my_depth
                End If
            End If


            'let's put in append mode if we have both text & non text children
            'If Not (is_append) And has_textchild(subchildnode) And has_notextchild(subchildnode) Then
            ' is_append = True
            ' depth_start_append = my_depth
            '    End If

            'add the text to variable or write it if not append. ***************************************

            Dim kCounterRemoveIndex As New ArrayList 'Is used to remove the Innertexted <b> or </br> nodes

            For k As Integer = 0 To subchildnode.ChildNodes.Count - 1

                If is_format_tag(subchildnode.ChildNodes(k).Name) Then

                    If is_append And my_depth >= depth_start_append Then
                        kept_tags(cur_kept_tags) = subchildnode.ChildNodes(k).Name
                        kept_tags_depth(cur_kept_tags) = my_depth

                        If cur_kept_tags = UBound(kept_tags) Then
                            'increase the size of the array if required
                            ReDim Preserve kept_tags(cur_kept_tags + 1)
                            ReDim Preserve kept_tags_depth(cur_kept_tags + 1)
                        End If

                        cur_kept_tags = cur_kept_tags + 1
                        mytext = mytext & "<" & subchildnode.ChildNodes(k).Name & ">"
                        kCounterRemoveIndex.Add(k)
                    End If

                End If


                If subchildnode.ChildNodes(k).Name = "#text" And Len(subchildnode.ChildNodes(k).InnerText) > 0 Then

                    'closing tags if same level
                    If is_append And cur_kept_tags > 0 Then
                        If my_depth = kept_tags_depth(cur_kept_tags - 1) Then
                            mytext = mytext & "</" & kept_tags(cur_kept_tags - 1) & ">"
                            cur_kept_tags = cur_kept_tags - 1
                            kCounterRemoveIndex.Add(k)
                        End If
                    End If

                    If Len(subchildnode.ChildNodes(k).InnerText) = 1 OrElse Strings.Left(subchildnode.ChildNodes(k).InnerText, 2) <> "#{" Then
                        'avoid testing left x,2 for a single char string. orelse just evaluates first part.
                        'strings.left ... do not take internal variables which start with #{  ===> 1 case whre the variable is not starting. We could put instr instead. Need to dble check.

                        If is_append Then
                            mytext = mytext & subchildnode.ChildNodes(k).InnerText
                            If IsNothing(mytext) <> True Then
                                Dim sTranslation As String = GetTranslatedContent(objXliff, mytext).Replace("<br></br>", "").Replace("<b></b>", "")
                                If sTranslation <> String.Empty Then
                                    For t As Integer = 0 To kCounterRemoveIndex.Count - 1
                                        subchildnode.ChildNodes(kCounterRemoveIndex(t)).InnerText = String.Empty
                                    Next
                                    subchildnode.ChildNodes(k).InnerText = sTranslation
                                    kCounterRemoveIndex = New ArrayList
                                Else
                                    kCounterRemoveIndex.Add(k) 'it might have <b>
                                End If

                            End If
                        Else
                            subchildnode.ChildNodes(k).InnerText = Get_transaltionFromxliff(objXliff, subchildnode.ChildNodes(k).InnerText)
                        End If

                    End If
                End If


                'Adding attributes - don't append in all cases *****************************************
                If Not (IsNothing(subchildnode.ChildNodes(k).Attributes)) Then
                    'check if any other thing than text should be translated
                    For Each mychildnodeattribute In subchildnode.ChildNodes(k).Attributes

                        'we'll have to put this very flexible in a config file. We see 3 different patterns
                        'Simple attribute value
                        'attribute value call param
                        'attribute value condition (similar to previous, but we also put constrain on childnodename, not only on childnodeattribute)

                        If UBound(additional_extract_type1) > 0 Then
                            For f = 0 To UBound(additional_extract_type1) - 1
                                If LCase(mychildnodeattribute.name) = additional_extract_type1(f) And Len(mychildnodeattribute.value) > 0 Then
                                    ' sendtotranslation(mychildnodeattribute.value, Format(curID, "00000") & "_" & additional_extract_type1(f), "RMK_xHTML", curfilename)
                                    Dim sTransltion As String = Replace_quotedcontent(mychildnodeattribute.value, objXliff)
                                    If sTransltion <> String.Empty Then
                                        mychildnodeattribute.value = sTransltion
                                    End If

                                End If
                            Next
                        End If

                        If UBound(additional_extract_type2a) > 0 Then
                            For f = 0 To UBound(additional_extract_type2a) - 1
                                If LCase(mychildnodeattribute.name) = additional_extract_type2a(f) Then
                                    If InStr(mychildnodeattribute.value, additional_extract_type2b(f)) <> 0 Then
                                        ' sendtotranslation(get_quotedcontent(mychildnodeattribute.value), Format(curID, "00000") & "_" & additional_extract_type2a(f) & "_" & additional_extract_type2b(f), "RMK_xHTML", curfilename)
                                        Dim sTransltion As String = Replace_quotedcontent(mychildnodeattribute.value, objXliff)
                                        If sTransltion <> String.Empty Then
                                            mychildnodeattribute.value = sTransltion
                                        End If
                                    End If
                                End If
                            Next
                        End If

                        If UBound(additional_extract_type3a) > 0 Then
                            For f = 0 To UBound(additional_extract_type3a) - 1
                                If LCase(subchildnode.ChildNodes(k).Name) = additional_extract_type3c(f) And LCase(mychildnodeattribute.name) = additional_extract_type3a(f) Then
                                    If InStr(mychildnodeattribute.value, additional_extract_type3b(f)) <> 0 Then
                                        ' sendtotranslation(get_quotedcontent(mychildnodeattribute.value), Format(curID, "00000") & "_" & additional_extract_type3a(f) & "_" & additional_extract_type3b(f) & "_" & additional_extract_type3c(f), "RMK_xHTML", curfilename)
                                        Dim sTransltion As String = Replace_quotedcontent(mychildnodeattribute.value, objXliff)
                                        If sTransltion <> String.Empty Then
                                            mychildnodeattribute.value = sTransltion
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    Next

                End If

                'check recursively
                If subchildnode.ChildNodes(k).HasChildNodes Then
                    my_depth = my_depth + 1
                    Rintegrate_subchild(subchildnode.ChildNodes(k), curfilename, objXliff)
                    'closing tags when we are done with all attributes
                    If is_append And cur_kept_tags > 0 Then
                        If my_depth = kept_tags_depth(cur_kept_tags - 1) Then
                            mytext = mytext & "</" & kept_tags(cur_kept_tags - 1) & ">"
                            If IsNothing(mytext) <> True Then
                                Dim sTranslation As String = GetTranslatedContent(objXliff, mytext).Replace("<br></br>", "").Replace("<b></b>", "")
                                If sTranslation <> String.Empty Then
                                    For t As Integer = 0 To kCounterRemoveIndex.Count - 1
                                        subchildnode.ChildNodes(kCounterRemoveIndex(t)).InnerText = String.Empty
                                    Next
                                    subchildnode.ChildNodes(k).InnerText = sTranslation
                                    kCounterRemoveIndex = New ArrayList
                                Else
                                    kCounterRemoveIndex.Add(k) 'it might have <b>
                                End If
                            End If
                            cur_kept_tags = cur_kept_tags - 1
                        End If
                    End If

                    my_depth = my_depth - 1

                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

#End Region

#Region "ReIntegrate Innerxml version, this doesnot work with Laurent' algorithm, but does 90% reintegration"

    Private _xliffdata As sXliff

    Sub ReIntegrate(ByVal myFile As String, ByRef xliffData As sXliff)

        _xliffdata = xliffData

        Dim xmlcontent As String
        Dim xd As New Xml.XmlDocument
        curID = 1

        xmlcontent = dbletags_xml(System.IO.File.ReadAllText(myFile))

        xmlcontent = WebUtility.HtmlDecode(xmlcontent) ' remove the encoding for e.g. copyright sign

        Try
            xd.XmlResolver = Nothing
            xd.LoadXml(xmlcontent) 'NEED TO use try and catch the error. IF there is a xml error, an exception is raised here.

            'reinit variables
            is_append = False
            my_depth = 1
            depth_start_append = 0
            cur_kept_tags = 0
            ReDim kept_tags(0)
            ReDim kept_tags_depth(0)

            ReIntegrate_subchild(xd.DocumentElement.ParentNode, myFile.ToString)

            xd.Save(myFile)

            'write last if required
            If mytext <> "" Then sendtotranslation(mytext, Format(curID, "00000") & "_TextContent", "RMK_xHTML", myFile.ToString)
            mytext = ""
        Catch ex As SystemException
            Throw New Exception(myFile.ToString & ": Exception - " & ex.Message & vbCrLf)
        End Try

    End Sub

    Private Sub ReIntegrate_subchild(ByVal subchildnode As Xml.XmlNode, ByVal curfilename As String)

        Try
            For Each mychildnode In subchildnode.ChildNodes

                Dim str As String = mychildnode.innerxml.ToString.Replace(" xmlns=" & Chr(34) & "http://www.w3.org/1999/xhtml" & Chr(34), "")
                Debug.Print(str & vbCrLf)

                str = (ReplaceUnvalidstring(str))
                Dim xdata As String = ""
                For i As Integer = 0 To _xliffdata.Source.Count - 1
                    If str.Trim = "" Then
                        Exit For
                    End If
                    xdata = ReplaceUnvalidstring(_xliffdata.Source(i).ToString.Replace("&", "&amp;"))
                    If str.ToLower.Replace(" ", "") = xdata.ToString.ToLower.Replace(" ", "") Then
                        Try
                            mychildnode.innerxml = _xliffdata.Translation(i).Replace("&", "&amp;")
                        Catch ex As Exception
                            Throw New Exception(ex.Message)
                        End Try
                    End If
                Next

                If Not (IsNothing(mychildnode.attributes)) Then
                    For Each MyChildAttribute In mychildnode.attributes
                        str = (ReplaceUnvalidstring(MyChildAttribute.value))
                        For i As Integer = 0 To _xliffdata.Source.Count - 1
                            If str.Trim = "" Then
                                Exit For
                            End If
                            xdata = ReplaceUnvalidstring(_xliffdata.Source(i).ToString.Replace("&", "&amp;"))
                            If str.ToLower.Replace(" ", "") = xdata.ToString.ToLower.Replace(" ", "") Then
                                Try
                                    MyChildAttribute.value = _xliffdata.Translation(i).Replace("&", "&amp;")
                                Catch ex As Exception
                                    Throw New Exception(ex.Message)
                                End Try
                            End If
                        Next
                    Next
                End If

                'check recursively
                If mychildnode.haschildnodes Then
                    ReIntegrate_subchild(mychildnode, curfilename)
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


    End Sub


    Private Function ReplaceUnvalidstring(ByVal htmlstring As String) As String
        If htmlstring.Trim = String.Empty Then
            Return htmlstring
        End If

        htmlstring = htmlstring.Replace("<br>", String.Empty)
        htmlstring = htmlstring.Replace("<br/>", String.Empty)
        htmlstring = htmlstring.Replace("<br />", String.Empty)
        htmlstring = htmlstring.Replace("</br>", String.Empty)
        htmlstring = htmlstring.Replace("<span>", String.Empty)
        htmlstring = htmlstring.Replace("</span>", String.Empty)
        htmlstring = htmlstring.Replace("<span />", String.Empty)
        htmlstring = htmlstring.Replace("<span/>", String.Empty)
        htmlstring = htmlstring.Replace("<Br>", String.Empty)
        htmlstring = htmlstring.Replace("<BR>", String.Empty)

        Return htmlstring
    End Function


    'Private Function CheckStartElementHasSubNodes(ByVal str As String) As Boolean


    '    Dim regex As New System.Text.RegularExpressions.Regex("\<(.*?)\>")
    '    Dim result As System.Text.RegularExpressions.MatchCollection = regex.Matches(str)

    '    If result.Count = 0 Then
    '        Return False
    '    End If

    '    For i As Integer = 0 To result.Count

    '    Next

    'End Function


#End Region

End Class
