Imports System.IO
Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.XPath

Module Mod_SLC_To_Xliff

    Public Enum FileType
        Section
        Question
    End Enum

    Dim objFileType As FileType

    Dim Definitions() As String

    Dim TagName As ArrayList

    Dim objTags As ArrayList

    Public Function SLC_To_Xliff(ByVal XmlFile As String, ByVal xliff_Path As String, ByVal Targetlanguage As String) As Boolean

        objTags = New ArrayList

        Dim Tags As New ArrayList
        TagName = New ArrayList

        xliff_Path = xliff_Path & System.IO.Path.GetFileNameWithoutExtension(XmlFile) & "_" & Targetlanguage & ".xliff"

        Try

            Dim objXmlDefiniton As New XmlDefinition
            objXmlDefiniton.GetXmlDefinition(XmlFile)

            Definitions = objXmlDefiniton.Definitions

            If objXmlDefiniton.DefinitionFile = "SLC_Question" Then
                objFileType = FileType.Question
            Else
                objFileType = FileType.Section
            End If

            For i As Integer = 1 To UBound(Definitions) - 1
                Tags = SLC_Extract(XmlFile, Definitions(i), Targetlanguage)
                Dim MyDict As New SLC_Data
                MyDict.Def = Definitions(i)
                MyDict.Tag = Tags
                objTags.Add(MyDict)
            Next

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

                Select Case objFileType
                    Case FileType.Question
                        Counter = WriteQuestion(Writer)
                    Case FileType.Section
                        Counter = WriteSection(Writer)
                End Select

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


    Function WriteSection(ByRef Writer As StreamWriter) As Integer

        Dim ID As Integer = 0
        Dim LINE As Integer = 0
        Dim TRANSLATIONSTATUS As Integer = 0
        Dim TITLE As Integer = 0
        Dim EXPLANATION As Integer = 0

        For i As Integer = 0 To objTags.Count - 1
            If objTags(i).def.ToString.ToLower = "<line>" Then
                LINE = i
            ElseIf objTags(i).def.ToString.ToLower = "<id>" Then
                ID = i
            ElseIf objTags(i).def.ToString.ToLower = "<translation_status>" Then
                TRANSLATIONSTATUS = i
            ElseIf objTags(i).def.ToString.ToLower = "<title>" Then
                TITLE = i
            ElseIf objTags(i).def.ToString.ToLower = "<explanation>" Then
                EXPLANATION = i
            End If
        Next

        Try
            Dim myNum As Integer = 0
            Dim transUnit As String = ""

            '<Title>
            For j As Integer = 0 To objTags(0).tag.count - 1

                If objTags(TITLE).Tag(j) <> "" And objTags(TRANSLATIONSTATUS).tag(j) <> "03" Then
                    myNum += 1
                    transUnit = myNum & "_" & objTags(LINE).tag(j) & "_" & objTags(ID).tag(j) & "_Title"

                    Writer.WriteLine("<trans-unit id=" & Chr(34) & transUnit & Chr(34) & " resname=" & Chr(34) & "SLC" & Chr(34) & ">")
                    Writer.WriteLine("<source>" & (wrap_html(clean_xml(objTags(TITLE).Tag(j)))) & "</source>")
                    Writer.WriteLine("<target state=""needs-review-translation""></target>")
                    Writer.WriteLine("<note from=""Developer"" priority =""10"">Questionnaire Section: Title </note>")
                    Writer.WriteLine("</trans-unit>")
                    Writer.WriteLine(vbCrLf)
                End If
            Next

            '<EXPLANATION>
            For j As Integer = 0 To objTags(0).tag.count - 1

                If objTags(EXPLANATION).Tag(j) <> "" And objTags(TRANSLATIONSTATUS).tag(j) <> "03" Then
                    myNum += 1
                    transUnit = myNum & "_" & objTags(LINE).tag(j) & "_" & objTags(ID).tag(j) & "_Explanation"

                    Writer.WriteLine("<trans-unit id=" & Chr(34) & transUnit & Chr(34) & " resname=" & Chr(34) & "SLC" & Chr(34) & ">")
                    Writer.WriteLine("<source>" & (wrap_html(clean_xml(objTags(EXPLANATION).Tag(j)))) & "</source>")
                    Writer.WriteLine("<target state=""needs-review-translation""></target>")
                    Writer.WriteLine("<note from=""Developer"" priority =""10"">Questionnaire Section: Explanation </note>")
                    Writer.WriteLine("</trans-unit>")
                    Writer.WriteLine(vbCrLf)
                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return 1
    End Function

    Function WriteQuestion(ByRef Writer As StreamWriter) As Integer

        Dim ID As Integer = 0
        Dim ANSWERID As Integer = 0
        Dim LINE As Integer = 0
        Dim ANSWER As Integer = 0
        Dim TRANSLATIONSTATUS As Integer = 0
        Dim TEXT As Integer = 0
        Dim EXPLANATION As Integer = 0

        For i As Integer = 0 To objTags.Count - 1
            If objTags(i).def.ToString.ToLower = "<line>" Then
                LINE = i
            ElseIf objTags(i).def.ToString.ToLower = "<id>" Then
                ID = i
            ElseIf objTags(i).def.ToString.ToLower = "<answer>" Then
                ANSWER = i
            ElseIf objTags(i).def.ToString.ToLower = "<answer_id>" Then
                ANSWERID = i
            ElseIf objTags(i).def.ToString.ToLower = "<translation_status>" Then
                TRANSLATIONSTATUS = i
            ElseIf objTags(i).def.ToString.ToLower = "<text>" Then
                TEXT = i
            ElseIf objTags(i).def.ToString.ToLower = "<explanation>" Then
                EXPLANATION = i
            End If
        Next

        Try
            Dim myNum As Integer = 0
            Dim transUnit As String = ""

            '<Text>
            For j As Integer = 0 To objTags(0).tag.count - 1

                If objTags(TEXT).Tag(j) <> "" And objTags(TRANSLATIONSTATUS).tag(j) <> "03" Then
                    myNum += 1
                    transUnit = myNum & "_" & objTags(LINE).tag(j) & "_" & objTags(ID).tag(j) & "_"
                    If objTags(0).tag(j).ToString.Substring(0).ToUpper = "A" Then
                        transUnit = transUnit & objTags(ANSWERID).tag(j) & "_text"
                    Else
                        transUnit = transUnit & "text"
                    End If

                    Writer.WriteLine("<trans-unit id=" & Chr(34) & transUnit & Chr(34) & " resname=" & Chr(34) & "SLC" & Chr(34) & ">")
                    Writer.WriteLine("<source>" & (wrap_html(clean_xml(objTags(TEXT).Tag(j)))) & "</source>")
                    Writer.WriteLine("<target state=""needs-review-translation""></target>")
                    If objTags(0).tag(j).ToString.Substring(0).ToUpper = "A" Then
                        Writer.WriteLine("<note from=""Developer"" priority =""10"">Questionnaire Answer: answer </note>")
                    Else
                        Writer.WriteLine("<note from=""Developer"" priority =""10"">Questionnaire Question: text </note>")
                    End If
                    Writer.WriteLine("</trans-unit>")
                    Writer.WriteLine(vbCrLf)
                End If
            Next

            '<explanation>
            For j As Integer = 0 To objTags(0).tag.count - 1

                If objTags(EXPLANATION).Tag(j) <> "" And objTags(TRANSLATIONSTATUS).tag(j) <> "03" Then
                    myNum += 1
                    transUnit = myNum & "_" & objTags(LINE).tag(j) & "_" & objTags(ID).tag(j) & "_"
                    If objTags(0).tag(j).ToString.Substring(0).ToUpper = "A" Then
                        transUnit = transUnit & objTags(ANSWERID).tag(j) & "_explanation"
                    Else
                        transUnit = transUnit & "explanation"
                    End If

                    Writer.WriteLine("<trans-unit id=" & Chr(34) & transUnit & Chr(34) & " resname=" & Chr(34) & "SLC" & Chr(34) & ">")
                    Writer.WriteLine("<source>" & (wrap_html(clean_xml(objTags(EXPLANATION).Tag(j)))) & "</source>")
                    Writer.WriteLine("<target state=""needs-review-translation""></target>")
                    If objTags(0).tag(j).ToString.Substring(0).ToUpper = "A" Then
                        Writer.WriteLine("<note from=""Developer"" priority =""10"">Questionnaire Answer: answer </note>")
                    Else
                        Writer.WriteLine("<note from=""Developer"" priority =""10"">Questionnaire Question: text </note>")
                    End If
                    Writer.WriteLine("</trans-unit>")
                    Writer.WriteLine(vbCrLf)
                End If
            Next

            '<answer>
            For j As Integer = 0 To objTags(0).tag.count - 1

                If objTags(ANSWER).Tag(j) <> "" And objTags(TRANSLATIONSTATUS).tag(j) <> "03" Then
                    myNum += 1
                    transUnit = myNum & "_" & objTags(LINE).tag(j) & "_" & objTags(ID).tag(j) & "_"
                    If objTags(0).tag(j).ToString.Substring(0).ToUpper = "A" Then
                        transUnit = transUnit & objTags(ANSWERID).tag(j) & "_Answer"
                    Else
                        transUnit = transUnit & "answer"
                    End If

                    Writer.WriteLine("<trans-unit id=" & Chr(34) & transUnit & Chr(34) & " resname=" & Chr(34) & "SLC" & Chr(34) & ">")
                    Writer.WriteLine("<source>" & (wrap_html(clean_xml(objTags(ANSWER).Tag(j)))) & "</source>")
                    Writer.WriteLine("<target state=""needs-review-translation""></target>")
                    If objTags(0).tag(j).ToString.Substring(0).ToUpper = "A" Then
                        Writer.WriteLine("<note from=""Developer"" priority =""10"">Questionnaire Answer: answer </note>")
                    Else
                        Writer.WriteLine("<note from=""Developer"" priority =""10"">Questionnaire Question: text </note>")
                    End If
                    Writer.WriteLine("</trans-unit>")
                    Writer.WriteLine(vbCrLf)
                End If

            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return 1

    End Function

    Function SLC_Extract(ByVal xmlFile As String, ByVal ElementName As String, ByVal lang As String) As ArrayList
        ' ElementName = clean_element(Replace(Replace(ElementName, "<", ""), ">", ""))
        Dim Tags As New ArrayList
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
                Tags.Add(str)
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return Tags
    End Function

End Module

Public Class SLC_Data
    Public Def As String
    Public Tag As New ArrayList
End Class