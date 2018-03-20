Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.ComponentModel
Imports System.Environment

Public Module ModHelper

    Public Etabs As New List(Of ErrorTabs) 'Using for FormError docking form

    Public my_action As MyAction
    Public appData As String = GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\SFSF"
    Public LstProjectGroup As List(Of ProjectGroup)

    Public Enum MyAction
        Add_NewProject
        Edit_Project
        Load_based_onMaserProject
    End Enum

    Public Function lang_to_langcode(ByVal mylang As String) As String
        If Not (System.IO.File.Exists(appData & DefinitionFiles.Lang_List)) Then MsgBox("File Language.txt doesn't exist. Critical error!", MsgBoxStyle.Critical) : Return ""

        For Each lang In Split(System.IO.File.ReadAllText(appData & DefinitionFiles.Lang_List), vbLf)
            If Mid(lang, 1, InStr(lang, Chr(9)) - 1) = mylang Then Return Mid(lang, InStrRev(lang, Chr(9)) + 1)
        Next
        Return ""

    End Function

    Public Function langcode_to_langline(ByVal mylangcode As String) As Integer
        If Not (System.IO.File.Exists(appData & DefinitionFiles.Lang_List)) Then MsgBox("File Language.txt doesn't exist. Critical error!", MsgBoxStyle.Critical) : Return -1

        Dim f As Integer = 0
        For Each lang In Split(System.IO.File.ReadAllText(appData & DefinitionFiles.Lang_List), vbLf)
            If InStr(lang, mylangcode) <> 0 Then Return f
            f = f + 1
        Next

        Return -1

    End Function

    Public Function get_translation_type(ByVal inputfilename As String) As TranslationType
        Dim str() As String = Split(System.IO.File.ReadAllText(appData & DefinitionFiles.Xml_List), vbCrLf)
        For i As Integer = 0 To UBound(str)
            If InStr(str(i), System.IO.Path.GetFileNameWithoutExtension(inputfilename) & "-") > 0 Then
                Dim Dom() As String = Split(str(i), vbTab)
                If Dom(3) = 0 Then
                    Return TranslationType.Monolingual
                Else
                    Return TranslationType.Multilingual
                End If
                Exit For
            End If
        Next
        'let's take multilingual as a default but if we reached this level in the code, there is probably an issue.
        Return TranslationType.Multilingual

    End Function

    Public Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As Form_MainNew.RtbColor, ByRef _bw As BackgroundWorker)
        Dim str As New ArrayList
        str.Add(Msg)
        str.Add(MyColor)
        _bw.ReportProgress(4, str)
    End Sub

    Public Function CheckURL(ByVal URL As String) As Boolean
        Try
            Dim Response As Net.WebResponse = Nothing
            Dim WebReq As Net.HttpWebRequest = Net.HttpWebRequest.Create(URL)
            Response = WebReq.GetResponse
            Response.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Structure sXliff
        Public Resname As ArrayList
        Public TargetLang As String
        Public ID As ArrayList
        Public Source As ArrayList
        Public Translation As ArrayList
        Public Note As ArrayList
        Public MaxLength As ArrayList
    End Structure

    Public Function QuoteValue(ByVal value As String) As String
        Return String.Concat("""", value.Replace("""", """"""), """")
    End Function

    Public Function dbletags_xml(ByVal instring As String) As String
        instring = Replace(instring, "&amp;", "&amp;amp;")
        instring = Replace(instring, "&lt;", "&amp;lt;")
        instring = Replace(instring, "&gt;", "&amp;gt;")
        instring = Replace(instring, "&quot;", "&amp;quot;")
        dbletags_xml = Replace(instring, "&apos;", "&amp;apos;")
    End Function

    Public Function clean_element(ByVal instring As String) As String
        instring = Replace(instring, "@", "")
        instring = Replace(instring, "%", "")
        instring = Replace(instring, "&", "&amp;")
        instring = Replace(instring, "<", "&lt;")
        instring = Replace(instring, ">", "&gt;")
        instring = Regex.Replace(instring, "\\s+", " ")
        instring = Replace(instring, " ", "-")
        instring = Replace(instring, "(", "")
        instring = Replace(instring, ")", "")
        instring = Replace(instring, ",", "")
        instring = Replace(instring, ".", "")
        clean_element = Replace(instring, "'", "&apos;")
    End Function

    Public Function clean_element_exclude_Percent(ByVal instring As String) As String 'Writing xliff excluding header translation
        instring = Replace(instring, "@", "")
        'instring = Replace(instring, "%", "")
        instring = Replace(instring, "&", "&amp;")
        instring = Replace(instring, "<", "&lt;")
        instring = Replace(instring, ">", "&gt;")
        instring = Regex.Replace(instring, "\\s+", " ")
        instring = Replace(instring, " ", "-")
        instring = Replace(instring, "(", "")
        instring = Replace(instring, ")", "")
        instring = Replace(instring, ",", "")
        instring = Replace(instring, ".", "")
        clean_element_exclude_Percent = Replace(instring, "'", "&apos;")
    End Function

    Public Function clean_xml(ByVal instring As String) As String
        instring = Replace(instring, "&", "&amp;")
        instring = Replace(instring, "<", "&lt;")
        instring = Replace(instring, ">", "&gt;")
        instring = Replace(instring, Chr(34), "&quot;")
        clean_xml = Replace(instring, "'", "&apos;")
    End Function


    Public Function revert_xml(ByVal instring As String) As String
        instring = Replace(instring, "&amp;", "&")
        instring = Replace(instring, "&lt;", "<")
        instring = Replace(instring, "&gt;", ">")
        instring = Replace(instring, "&quot;", Chr(34))
        revert_xml = Replace(instring, "&apos;", "'")
    End Function

    Public Function RemoveEscapeChars(ByVal instring As String) As String
        instring = Replace(instring, "@", "")
        instring = Replace(instring, "%", "")
        instring = Replace(instring, "&amp;", "&")
        instring = Replace(instring, "(", "")
        instring = Replace(instring, ")", "")
        instring = Replace(instring, ",", "")
        instring = Replace(instring, ".", "")
        RemoveEscapeChars = Replace(instring, "&apos;", "")
    End Function

    Public Function GetPlainText(ByVal SourceText As String) As String
        Try
            Dim ClrText As String = SourceText
            If ClrText <> "" Then
                ClrText = HTMLToText(ClrText)
                If ClrText = "" Then
                    Return SourceText
                End If
                ClrText = RemoveEscapeChars(ClrText)
                ClrText = System.Text.RegularExpressions.Regex.Replace(ClrText, "\\s+", "")
                ClrText = ClrText.Replace(" ", String.Empty)
                ClrText = Replace(Replace(ClrText, " ", ""), vbTab, "")
                ClrText = System.Text.RegularExpressions.Regex.Replace(ClrText, "(\r\n)?(^\s*$)+", "", System.Text.RegularExpressions.RegexOptions.Multiline) 'Multiline will be removed
                ClrText = System.Text.RegularExpressions.Regex.Replace(ClrText, "[^\w\@-]", "") 'Unknown white space will be removed here
            End If

            If ClrText.Trim = String.Empty Then
                ClrText = SourceText
            End If

            Return ClrText
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function wrap_html(ByVal instring As String) As String
        'first check if there is any < and > in the string. if not, we get out of it.
        Dim str As String = instring
        If InStr(instring, "&lt;") = 0 Then Return instring
        If InStr(instring, "&gt;") = 0 Then Return instring

        Dim ph_id As Integer = 1
        'now let's check which tag there is
        Dim htmltags As String = "p;a;b;i;tt;sub;sup;big;small;hr;html;head;body;div;span;DOCTYPE;title;link;meta;style;h1;h2;h3;h4;h5;h6;strong;em;abbr;acronym;address;bdo;blockquote;cite;q;code;ins;del;dfn;kbd;pre;samp;var;br;base;img;area;map;object;param;ul;ol;li;dl;dt;dd;table;tr;td;th;tbody;thead;tfoot;col;colgroup;caption;script;noscript"
        For Each myhtmltag In htmltags.Split(";")
            If InStr(instring, "&lt;" & myhtmltag) <> 0 Then
                instring = add_ph_tag(instring, myhtmltag, ph_id)
                instring = add_ph_tag(instring, "/" & myhtmltag, ph_id)
            End If

        Next
        Return instring

    End Function
    Public Function add_ph_tag(ByVal instring As String, ByVal myhtmltag As String, ByRef ph_id As Integer) As String
        Dim start As Integer = InStr(instring, "&lt;" & myhtmltag)
        'note cannot do a simple replace, as there might multiple occurences

        Do While start <> 0
            instring = Mid(instring, 1, start - 1) & Replace(instring, "&lt;" & myhtmltag, "<ph id=" & Chr(34) & Trim(Str(ph_id)) & Chr(34) & ">&lt;" & myhtmltag, start, 1)
            instring = Mid(instring, 1, start - 1) & Replace(instring, "&gt;", "&gt;</ph>", start, 1)
            ph_id = ph_id + 1
            start = InStr(start + 13 + Len(Str(ph_id)), instring, "&lt;" & myhtmltag)
        Loop

        Return instring

    End Function

    Public Function unwrap_html(ByVal instring As String) As String
        If InStr(instring, "<ph id") = 0 Then Return instring
        Dim ph_id As Integer = 200
        Do While InStr(instring, "<ph id") <> 0
            instring = Replace(instring, "<ph id=" & Chr(34) & ph_id & Chr(34) & ">", "")
            instring = Replace(instring, "</ph>", "")
            ph_id = ph_id - 1
            If ph_id = -1 Then
                Exit Do
            End If
        Loop
        Return instring

    End Function

    Public Function MapFileNumberWithFileTypeDefintion(ByVal SmallNumber() As String, ByVal BigNumber() As String) As Boolean
        Dim fType As Integer = 0
        Dim bFound As Boolean
        Try
            For i As Integer = 0 To UBound(SmallNumber)
                bFound = False
                If SmallNumber(i) = BigNumber(i) Then
                    bFound = True
                Else
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error Mapping filename with Definition file!" & vbNewLine & "Please check fileName." & vbNewLine & ex.Message)
        End Try
        Return bFound
    End Function


    'Instead of this, use alternate function UnWrapXliffBack
    'Public Function load_xliff(ByVal filename As String) As sXliff 'Doesnt work if we have html tags, crashes when builtin function readinnerxml called.
    '    Dim MyXliff As New sXliff

    '    Try
    '        If System.IO.File.Exists(filename) <> True Then
    '            Throw New Exception(filename & " not found")
    '        End If
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try

    '    Dim reader As XmlTextReader = New XmlTextReader(filename)

    '    Dim row_cnt As Integer = 0

    '    MyXliff.Resname = New ArrayList
    '    MyXliff.Source = New ArrayList
    '    MyXliff.Translation = New ArrayList
    '    MyXliff.ID = New ArrayList

    '    Try
    '        Do While (reader.Read())

    '            'first we extract all data.
    '            If reader.NodeType = XmlNodeType.Element And reader.Name = "file" Then
    '                Do While reader.MoveToNextAttribute()
    '                    If reader.Name = "target-language" Then
    '                        MyXliff.TargetLang = GetLang(Replace(reader.Value, "-", "_"))
    '                        MyXliff.TargetLang = Mid(MyXliff.TargetLang, 1, 3) & UCase(Mid(MyXliff.TargetLang, 4, 2))
    '                    End If
    '                Loop
    '            End If

    '            If reader.NodeType = XmlNodeType.Element And reader.Name = "trans-unit" Then

    '                Do While reader.MoveToNextAttribute()
    '                    If reader.Name = "id" Then
    '                        MyXliff.ID.Add(reader.Value)
    '                    End If

    '                    If reader.Name = "resname" Then
    '                        MyXliff.Resname.Add(reader.Value)
    '                    End If
    '                Loop
    '            End If

    '            If reader.NodeType = XmlNodeType.Element And reader.Name = "source" Then
    '                reader.Read()
    '                MyXliff.Source.Add(reader.Value)
    '            End If

    '            If reader.NodeType = XmlNodeType.Element And reader.Name = "target" Then
    '                reader.Read()
    '                MyXliff.Translation.Add(reader.Value)
    '            End If

    '        Loop

    '        reader.Close()
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try
    '    Return MyXliff
    'End Function

    Public Function load_xliff(ByVal filename As String) As sXliff 'Doesnt work if we have html tags, crashes when builtin function readinnerxml called.
        Dim MyXliff As New sXliff

        Try
            If System.IO.File.Exists(filename) <> True Then
                Throw New Exception(filename & " not found")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Dim row_cnt As Integer = 0

        MyXliff.Resname = New ArrayList
        MyXliff.Source = New ArrayList
        MyXliff.Translation = New ArrayList
        MyXliff.ID = New ArrayList
        MyXliff.Note = New ArrayList
        MyXliff.MaxLength = New ArrayList

        Try
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            Try
                xd.Load(filename)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

            '"trans-unit"
            Dim xNodeList As XmlNodeList

            xNodeList = xd.GetElementsByTagName("file")

            Dim MyAttributes As XmlAttributeCollection


            For i As Integer = 0 To xNodeList.Count - 1
                If xNodeList(i).Attributes.Count > 0 Then
                    MyAttributes = xNodeList(i).Attributes
                    Dim att As XmlAttribute
                    For Each att In MyAttributes
                        If InStr(att.Name, "target-language") > 0 Then
                            MyXliff.TargetLang = (att.Value)
                            Exit For
                        End If
                    Next
                End If
            Next

            xNodeList = xd.GetElementsByTagName("trans-unit")
            For i As Integer = 0 To xNodeList.Count - 1
                If xNodeList(i).Attributes.Count > 0 Then
                    MyAttributes = xNodeList(i).Attributes
                    Dim att As XmlAttribute
                    For Each att In MyAttributes
                        If String.Compare(att.Name, "id", True) = 0 Then
                            MyXliff.ID.Add(att.Value)
                        ElseIf String.Compare(att.Name, "resname", True) = 0 Then
                            MyXliff.Resname.Add(att.Value)
                        ElseIf String.Compare(att.Name, "maxwidth", True) = 0 Then
                            MyXliff.MaxLength.Add(CInt(att.Value))
                        End If
                    Next
                End If
            Next

            xNodeList = xd.GetElementsByTagName("source")

            For i As Integer = 0 To xNodeList.Count - 1
                MyXliff.Source.Add(xNodeList(i).InnerText)
            Next

            xNodeList = xd.GetElementsByTagName("target")

            For i As Integer = 0 To xNodeList.Count - 1
                MyXliff.Translation.Add(xNodeList(i).InnerText)
            Next

            xNodeList = xd.GetElementsByTagName("note")

            For i As Integer = 0 To xNodeList.Count - 1
                MyXliff.Note.Add(xNodeList(i).InnerText)
            Next

            If MyXliff.Source.Count <> MyXliff.Translation.Count Then
                Dim str As String = "XML vaidation error, Source\Target count are not equal!" & vbNewLine
                str = str & "Source count - " & MyXliff.Source.Count & vbNewLine
                str = str & "Target count - " & MyXliff.Translation.Count & vbNewLine
                str = str & "Please check the xliff file - " & filename
                Throw New Exception(str)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message & Environment.NewLine & System.IO.Path.GetFileName(filename))
        End Try

        Return MyXliff

    End Function

    Public Function GetLong_lang(ByVal str As String) As String
        Select Case LCase(str)
            Case "en", "us", "en_us"
                Return "en_US"
            Case "fr", "fr_fr"
                Return "fr_FR"
            Case "de_de", "de"
                Return "de_DE"
            Case "es_es", "es"
                Return "es_ES"
            Case "ja_jp", "ja", "jp"
                Return "ja_JP"
            Case "ko_kr", "kr", "ko"
                Return "ko_KR"
            Case "zh_cn", "zh", "cn"
                Return "zh_CN"
            Case "ru_ru", "ru"
                Return "ru_RU"
            Case "pt_br", "pt", "br"
                Return "pt_BR"
            Case "it_it", "it"
                Return "it_IT"
            Case Else
                Return str
        End Select

    End Function

    Public Function GetShort_lang(ByVal str As String) As String
        Select Case LCase(str)
            Case "en", "us", "en_us"
                Return "en"
            Case "fr", "fr_fr"
                Return "fr"
            Case "de_de", "de"
                Return "de"
            Case "es_es", "es"
                Return "es"
            Case "es_co", "es"
                Return "es"
            Case "ja_jp", "ja", "jp"
                Return "jp"
            Case "ko_kr", "kr", "ko"
                Return "ko"
            Case "zh_cn", "zh", "cn"
                Return "zh"
            Case "zh_tw", "tw"
                Return "tw"
            Case "ru_ru", "ru"
                Return "ru"
            Case "pt_br", "pt", "br"
                Return "pt"
            Case "it_it", "it"
                Return "it"
            Case Else
                Return str
        End Select

    End Function

    Public Function UnWrapXliffBack(ByVal xliffFile As String) As Boolean
        Try
            If File.Exists(Application.StartupPath & "\Temp_UnWrap.xliff") Then
                File.Delete(Application.StartupPath & "\Temp_UnWrap.xliff")
            End If

            Dim str As String = ""
            Using Reader As StreamReader = New StreamReader(xliffFile, True)
                str = Reader.ReadToEnd
            End Using

            Dim content() As String
            content = Split(str, vbCrLf)

            For i As Integer = 0 To UBound(content)
                'If Left(content(i), 8) = "<source>" Then
                '    content(i) = revert_xml(unwrap_html((Mid(content(i), 9, InStr(content(i), "</source>") - 9))))
                'ElseIf Left(content(i), 14) = "<target state=" Then
                '    content(i) = revert_xml(unwrap_html((Mid(content(i), 42, InStr(content(i), "</target>") - 42))))
                'End If

                If Left(content(i), 8) = "<source>" Then
                    content(i) = revert_xml(unwrap_html(content(i)))
                ElseIf Left(content(i), 14) = "<target state=" Then
                    content(i) = revert_xml(unwrap_html(content(i)))
                End If
            Next

            Using writer As StreamWriter = New StreamWriter(Application.StartupPath & "\Temp_UnWrap.xliff", False, System.Text.Encoding.UTF8)
                For i As Integer = 0 To UBound(content)
                    writer.WriteLine(content(i))
                Next
            End Using

        Catch ex As Exception
            Throw New Exception("Error UnWrapping xliff" & vbNewLine & ex.Message)
        End Try
        Return True
    End Function

    'Load xliff to Structure Xliff
    Public Function cvload_xliff(ByVal filename As String) As sXliff
        Dim MyXliff As New sXliff
        MyXliff.Resname = New ArrayList
        MyXliff.Source = New ArrayList
        MyXliff.Translation = New ArrayList
        MyXliff.ID = New ArrayList
        Try
            Dim str As String = ""
            Using Reader As StreamReader = New StreamReader(filename, True)
                str = Reader.ReadToEnd
            End Using

            Dim content() As String
            content = Split(str, vbCrLf)

            For i As Integer = 0 To UBound(content)

                If Left(content(i), 15) = "<trans-unit id=" Then
                    MyXliff.ID.Add(Mid(content(i), 17, InStr(17, content(i), Chr(34)) - 17))

                    Dim istart As Integer = InStr(content(i), "resname") + 9
                    Dim iend As Integer = InStr(istart, content(i), ">")

                    MyXliff.Resname.Add(Mid(content(i), istart, iend - istart - 1))
                End If

                If Left(content(i), 8) = "<source>" Then
                    MyXliff.Source.Add(revert_xml(unwrap_html((Mid(content(i), 9, InStr(content(i), "</source>") - 9)))))
                ElseIf Left(content(i), 39) = "<target state=" & Chr(34) & "needs-review-translation" Then
                    MyXliff.Translation.Add(Mid(content(i), 42, InStr(content(i), "</target>") - 42))
                ElseIf Left(content(i), 25) = "<target state=" & Chr(34) & "translated" Then
                    MyXliff.Translation.Add(Mid(content(i), 28, InStr(content(i), "</target>") - 28))
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error loading cvXliff - " & filename & vbNewLine & ex.Message)
        End Try
        Return MyXliff
    End Function

    Public Function HTMLToText(ByVal HTMLCode As String) As String
        ' Remove new lines since they are not visible in HTML
        HTMLCode = HTMLCode.Replace("\n", " ")

        ' Remove tab spaces
        HTMLCode = HTMLCode.Replace("\t", " ")

        ' Remove multiple white spaces from HTML
        HTMLCode = Regex.Replace(HTMLCode, "\\s+", " ")

        ' Remove HEAD tag
        HTMLCode = Regex.Replace(HTMLCode, "<head.*?</head>", "" _
          , RegexOptions.IgnoreCase Or RegexOptions.Singleline)

        ' Remove any JavaScript
        HTMLCode = Regex.Replace(HTMLCode, "<script.*?</script>", "" _
          , RegexOptions.IgnoreCase Or RegexOptions.Singleline)

        ' Replace special characters like &, <, >, " etc.
        Dim sbHTML As StringBuilder = New StringBuilder(HTMLCode)
        ' Note: There are many more special characters, these are just
        ' most common. You can add new characters in this arrays if needed
        Dim OldWords() As String = {"&nbsp;", "&amp;", "&quot;", "&lt;",
           "&gt;", "&reg;", "&copy;", "&bull;", "&trade;"}
        Dim NewWords() As String = {" ", "&", """", "<", ">", "Â®", "Â©", "â€¢", "â„¢"}
        For i As Integer = 0 To OldWords.Length - 1
            sbHTML.Replace(OldWords(i), NewWords(i))
        Next i

        ' Check if there are line breaks (<br>) or paragraph (<p>)
        sbHTML.Replace("<br>", "\n<br>")
        sbHTML.Replace("<br ", "\n<br ")
        sbHTML.Replace("<p ", "\n<p ")

        ' Finally, remove all HTML tags and return plain text
        Return System.Text.RegularExpressions.Regex.Replace(
           sbHTML.ToString(), "<[^>]*>", "")
    End Function

    Public Sub ShowMsgInMainForm(ByVal Msg As String, ByVal MyColor As Form_MainNew.RtbColor)
        Form_MainNew.UpdateMsg(Msg, MyColor)
    End Sub

    'Missing translations
    Public notFoundDetails As New StringBuilder
    Public notFoundMsg As New StringBuilder

#Region "Hybris Mapping file location"

    Public Function ValidateHybrisMappingFile() As Boolean
        Try
            Dim HybrisFolder As String = ProjectManagement.GetActiveProject.ProjectPath & "HybrisRawData"
            Dim HybrisMappingFile As String = HybrisFolder & "\HybrisMapping.xml"

            If System.IO.File.Exists(HybrisMappingFile) <> True Then
                Throw New Exception("No HybrisMapping.xml file found!" & vbNewLine & "The operation cannot be performed...exiting...")
            End If

            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(HybrisMappingFile)

            '"trans-unit"
            Dim xNodeList As XmlNodeList
            xNodeList = xd.GetElementsByTagName("File_path")

            For i As Integer = 0 To xNodeList.Count - 1
                Dim TargetFolder As String = xNodeList(i).InnerText
                TargetFolder = Mid(TargetFolder, 1, InStr(Len(HybrisFolder) + 3, TargetFolder, "\"))

                If System.IO.Directory.Exists(TargetFolder) <> True Then
                    Throw New Exception(TargetFolder & vbNewLine & "No extracted zip/rar folder found in HybrisRawData!" & vbNewLine & "The operation cannot be performed...exiting...")
                End If
                Exit For
            Next

            xNodeList = xd.GetElementsByTagName("ArchiveType")

            If xNodeList(0).Attributes(0).InnerText = String.Empty Then
                Throw New Exception("TargetFolderName not found in Hybrisxml file!")
            End If


        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function

    Public Function GetArchiveType() As String
        Dim HybrisFolder As String = ProjectManagement.GetActiveProject.ProjectPath & "HybrisRawData"
        Dim HybrisMappingFile As String = HybrisFolder & "\HybrisMapping.xml"

        If System.IO.File.Exists(HybrisMappingFile) <> True Then
            Throw New Exception("No HybrisMapping.xml file found!" & vbNewLine & "The operation cannot be performed...exiting...")
        End If

        Dim xd As New Xml.XmlDocument
        xd.XmlResolver = Nothing
        xd.Load(HybrisMappingFile)

        '"trans-unit"
        Dim xNodeList As XmlNodeList
        xNodeList = xd.GetElementsByTagName("Type")

        Return xNodeList.Item(0).InnerText
    End Function

    Public Function Get_TargetFile_from_HybrisMappingFile(ByVal enUS_fileName As String, ByVal fileType As String, ByVal TargetLang As String) As String
        Dim TargetFolder As String = ""
        Dim TargetFile As String = ""
        TargetLang = GetShort_lang(TargetLang)
        Try
            Dim HybrisFolder As String = ProjectManagement.GetActiveProject.ProjectPath & "HybrisRawData"
            Dim HybrisMappingFile As String = HybrisFolder & "\HybrisMapping.xml"

            If Not System.IO.File.Exists(HybrisMappingFile) Then
                Return ""
            End If

            Dim FileId As String = Mid(enUS_fileName, 1, InStr(enUS_fileName, "_") - 1)

            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(HybrisMappingFile)

            '"trans-unit"
            Dim xNodeList As XmlNodeList
            xNodeList = xd.GetElementsByTagName(Replace(fileType, ".", ""))

            For i As Integer = 0 To xNodeList.Count - 1
                If xNodeList(i).ChildNodes(0).InnerText = FileId Then
                    TargetFolder = xNodeList(i).ChildNodes(2).InnerText
                    TargetFile = Replace(xNodeList(i).ChildNodes(1).InnerText, "_en", "_" & TargetLang)
                    Exit For
                End If
            Next

            'Now loop in Directory
            For Each f As String In Directory.GetFiles(TargetFolder)
                If System.IO.Path.GetFileName(f) = TargetFile Then
                    TargetFile = f
                    Exit For
                End If
            Next

            If System.IO.File.Exists(TargetFile) <> True Then
                TargetFile = ""
            End If

        Catch ex As Exception
            Throw New Exception("Error @GetHybrisMappingFile" & vbNewLine & ex.Message)
        End Try
        Return TargetFile
    End Function

    Public Function Get_TargetFolder_from_HybrisMappingFile(ByVal enUS_fileName As String, ByVal fileType As String, ByRef tName As String, ByVal TargetLang As String) As String
        Dim TargetFolder As String = ""

        TargetLang = GetShort_lang(TargetLang)
        Try
            Dim HybrisFolder As String = ProjectManagement.GetActiveProject.ProjectPath & "HybrisRawData"
            Dim HybrisMappingFile As String = HybrisFolder & "\HybrisMapping.xml"

            Dim FileId As String = Mid(enUS_fileName, 1, InStr(enUS_fileName, "_") - 1)

            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(HybrisMappingFile)

            '"trans-unit"
            Dim xNodeList As XmlNodeList
            xNodeList = xd.GetElementsByTagName(Replace(fileType, ".", ""))

            For i As Integer = 0 To xNodeList.Count - 1
                If xNodeList(i).ChildNodes(0).InnerText = FileId Then
                    TargetFolder = xNodeList(i).ChildNodes(2).InnerText
                    tName = Replace(xNodeList(i).ChildNodes(1).InnerText, "_en", "_" & TargetLang)
                    Exit For
                End If
            Next

        Catch ex As Exception
            Throw New Exception("Error @GetHybrisMappingFile" & vbNewLine & ex.Message)
        End Try
        Return TargetFolder
    End Function

    Public Function Get_ZipFolder() As String
        Dim TargetFolder As String = ""
        Try
            Dim HybrisFolder As String = ProjectManagement.GetActiveProject.ProjectPath & "HybrisRawData"
            Dim HybrisMappingFile As String = HybrisFolder & "\HybrisMapping.xml"

            If System.IO.File.Exists(HybrisMappingFile) <> True Then
                Throw New Exception("No HybrisMapping.xml file found!" & vbNewLine & "The operation cannot be performed...exiting...")
            End If

            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(HybrisMappingFile)

            '"trans-unit"
            Dim xNodeList As XmlNodeList
            xNodeList = xd.GetElementsByTagName("File_path")

            For i As Integer = 0 To xNodeList.Count - 1
                TargetFolder = xNodeList(i).InnerText
                TargetFolder = Mid(TargetFolder, 1, InStr(Len(HybrisFolder) + 3, TargetFolder, "\") - 1)

                If System.IO.Directory.Exists(TargetFolder) <> True Then
                    Throw New Exception(TargetFolder & vbNewLine & "No extracted zip/rar folder found in HybrisRawData!" & vbNewLine & "The operation cannot be performed...exiting...")
                End If

                Exit For
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return TargetFolder
    End Function

    Public Function PutMissingFilesBack(ByVal TargetLang As String) As Boolean
        Dim TargetFolder As String = ""
        Dim TargetFile As String = ""
        TargetLang = GetShort_lang(TargetLang)
        Try
            Dim HybrisFolder As String = ProjectManagement.GetActiveProject.ProjectPath & "HybrisRawData"
            Dim HybrisMappingFile As String = HybrisFolder & "\HybrisMapping.xml"

            Dim FileTypes As New ArrayList
            FileTypes.Add(".impex")
            FileTypes.Add(".properties")
            FileTypes.Add(".xml")
            FileTypes.Add(".html")

            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(HybrisMappingFile)

            For j As Integer = 0 To FileTypes.Count - 1
                '"trans-unit"
                Dim xNodeList As XmlNodeList
                xNodeList = xd.GetElementsByTagName(Replace(FileTypes(j), ".", ""))

                For i As Integer = 0 To xNodeList.Count - 1

                    TargetFolder = xNodeList(i).ChildNodes(2).InnerText
                    TargetFile = Replace(xNodeList(i).ChildNodes(1).InnerText, "_en", "_" & TargetLang)

                    Dim enFile As String = TargetFolder & "\" & xNodeList(i).ChildNodes(1).InnerText

                    If System.IO.File.Exists(TargetFolder & "\" & TargetFile) <> True Then
                        Try
                            File.Copy(enFile, TargetFolder & "\" & TargetFile, True)
                        Catch ex As Exception
                            Throw New Exception(ex.Message)
                        End Try
                    End If

                Next

            Next


        Catch ex As Exception
            Throw New Exception("Error @PutMissingFilesBack" & vbNewLine & ex.Message)
        End Try
        Return True
    End Function


#End Region

#Region "Language Defintion"
    ''' <summary>
    ''' example:
    ''' FullName = "French"
    ''' TwoChars = "fr"
    ''' FourChars = "frFR"
    ''' Fivechars = "fr-FR"
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum LangType
        FullName
        ' TwoChars
        FourChars
        FiveChars
    End Enum

    Structure LL
        Public LangFullName As ArrayList
        ' Public LangTwoChars As ArrayList
        Public LangFourChars As ArrayList
        Public LangFiveChars As ArrayList
    End Structure

    Public LanguageDefination As LL

    ''' <summary>
    ''' Finds the language code
    ''' </summary>
    ''' <param name="lType"></param>
    ''' <param name="searchLang"></param>
    ''' <returns>Language code</returns>
    ''' <remarks></remarks>
    Public Function GetLang(ByVal lType As LangType, ByVal searchLang As String) As String

        Dim LangL As String = ""
        Try
            For i As Integer = 0 To LanguageDefination.LangFullName.Count - 1
                If LanguageDefination.LangFullName(i).ToString.ToLower.Trim = searchLang.ToLower.Trim _
                    Or LanguageDefination.LangFourChars(i).ToString.ToLower.Trim = searchLang.ToLower.Trim _
                    Or LanguageDefination.LangFiveChars(i).ToString.ToLower.Trim = searchLang.ToLower.Trim Then
                    Select Case lType
                        Case LangType.FullName
                            LangL = LanguageDefination.LangFullName(i)
                        'Case LangType.TwoChars
                        '    LangL = LanguageDefination.LangTwoChars(i)
                        Case LangType.FourChars
                            LangL = LanguageDefination.LangFourChars(i)
                        Case LangType.FiveChars
                            LangL = LanguageDefination.LangFiveChars(i)
                    End Select
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error Number " & 1 & vbNewLine & ex.Message)
        End Try

        Return LangL
    End Function

#End Region

    ''' <summary>
    ''' For TreeView in Main Form
    ''' </summary>
    ''' <param name="Active"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetProjectImageIndex(ByVal Active As Boolean)
        Dim imageIndex As Integer
        If Active = True Then
            imageIndex = 12
        Else
            imageIndex = 13
        End If
        Return imageIndex
    End Function

End Module
