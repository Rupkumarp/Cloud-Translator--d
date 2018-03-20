Imports System.Xml
Imports System.IO

Public Module HybrisXml

    Public Enum TranslationType
        Monolingual
        Multilingual
    End Enum

#Region "Create Xliff"

    Public Function CreateXliff(ByVal xmlFile As String, ByVal xliff_savePath As String, ByVal targetLang As String) As String

        xliff_savePath = xliff_savePath & "\" & System.IO.Path.GetFileNameWithoutExtension(xmlFile) & "_" & targetLang & ".xliff"
        ' xmlFile = "C:\Users\C5195092\Desktop\SfTest\HybrisTest\01-Input-B\public-sector-foebis-new.xml" 'Test
        Dim fName As String = System.IO.Path.GetFileName(xmlFile)
        Dim xd As New Xml.XmlDocument
        xd.XmlResolver = Nothing
        xd.Load(xmlFile)

        Dim objxml As New HybrisXml

        Dim node_Title As New ArrayList
        Dim node_Description As New ArrayList
        Dim node_Resource As New ArrayList

        Try
            Dim xNodeList As XmlNodeList
            '<title>
            xNodeList = xd.GetElementsByTagName("title")
            node_Title = GetNodeData(xNodeList, targetLang)
            '<description>
            xNodeList = xd.GetElementsByTagName("description")
            node_Description = GetNodeData(xNodeList, targetLang)
            '<resources>
            xNodeList = xd.GetElementsByTagName("resource")
            node_Resource = GetNodeData(xNodeList, targetLang)

            If node_Title Is Nothing And node_Description Is Nothing And node_Resource Is Nothing Then
                Throw New Exception("Nothing found for translation as <title>,<description>,<resource> not found...")
                Exit Function
            End If

            If node_Title.Count = 0 And node_Description.Count = 0 And node_Resource.Count = 0 Then
                Return ("Nothing found for translation!")
                Exit Function
            End If

            Dim myNum As Integer = 1
            Using Writer As StreamWriter = New StreamWriter(xliff_savePath, False, System.Text.Encoding.UTF8)
                Writer.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                Writer.WriteLine("<xliff version=""1.2"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ")
                Writer.WriteLine("xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2- &#xD;&#xA;strict.xsd"">")
                Writer.WriteLine("<file original=""C:\Temp\en_fr_2.xliff"" source-language=" & Chr(34) & "en-US" & Chr(34) & " target-language=" & Chr(34) & targetLang & Chr(34) & " datatype=""plaintext"" date=" & Chr(34) & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & Chr(34) & " tool-id=""SAP SFSF CONVERTER"" category=""MLT"">")
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

                If node_Title.Count > 0 Then
                    WriterToXliff(Writer, node_Title, myNum, fName)
                End If

                If node_Description.Count > 0 Then
                    WriterToXliff(Writer, node_Description, myNum, fName)
                End If

                If node_Resource.Count > 0 Then
                    WriterToXliff(Writer, node_Resource, myNum, fName)
                End If

                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

            End Using

            If myNum = 1 Then
                Try
                    System.IO.File.Delete(xliff_savePath)
                    Return " Already has translation for" & Replace(targetLang, "-", "_")
                Catch ex As Exception
                    'do nothing
                End Try
            End If


        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return ""
    End Function

    Private Sub WriterToXliff(ByRef Writer As StreamWriter, ByRef node As ArrayList, ByRef myNum As Integer, ByRef FName As String)
        For i As Integer = 0 To node.Count - 1
            If CanbeAccepted(node(i).value) Then
                Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & node(i).name & Chr(34) & " resname=" & Chr(34) & "Hybris" & Chr(34) & ">")
                Writer.WriteLine("<source>" & wrap_html(clean_xml(node(i).value)) & "</source>")
                Writer.WriteLine("<target state=""needs-review-translation""></target>")
                Writer.WriteLine("<note from=""Developer"" priority =""10"">" & "XML : " & FName & "</note>")
                Writer.WriteLine("</trans-unit>")
                Writer.WriteLine(vbCrLf)
                myNum += 1
            End If
          
        Next
    End Sub


    Function CanbeAccepted(ByVal enUS As String) As Boolean
        Try
            Select Case Microsoft.VisualBasic.Right(enUS, 4).ToLower
                Case ".jpg", ".ico", ".png"
                    Return False
                Case ".jpe"
                    Return False
                Case ""
                    Return False
            End Select
            If IsNumeric(enUS) Then
                Return False
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function

    Private Function GetNodeData(ByVal xNodelist As XmlNodeList, ByVal targetLang As String) As ArrayList

        targetLang = GetShort_lang(targetLang)

        Dim enNode As XmlNode = Nothing
        Dim targetNode As XmlNode = Nothing

        Dim objNodes As New ArrayList

        Dim oNode As MyNode

        Try
            For i As Integer = 0 To xNodelist.Count - 1 'Gives number of Lanugaes available
                For j As Integer = 0 To xNodelist(i).Attributes.Count - 1
                    If xNodelist(i).Attributes(j).Name.ToString.ToLower = "xml:lang" And xNodelist(i).Attributes(j).Value.ToString.ToLower = "en" Then
                        enNode = xNodelist(i)
                        Exit For
                    ElseIf xNodelist(i).Attributes(j).Name.ToString.ToLower = "xml:lang" And xNodelist(i).Attributes(j).Value.ToString.ToLower = targetLang Then
                        targetNode = xNodelist(i)
                        Exit For
                    End If
                Next j
            Next i

            If enNode Is Nothing Then
                Return objNodes
                Exit Function
            End If

            'Now Compare enNode and targetNode
            For i As Integer = 0 To enNode.ChildNodes.Count - 1
                Dim enName As String = ""
                Dim envalue As String = ""

                Dim targetName As String = ""
                Dim targetvalue As String = ""

                RecurseXML(enNode.ChildNodes(i), enName, envalue)

                If enNode.ChildNodes(i).Name.ToString.ToLower.Trim = targetNode.ChildNodes(i).Name.ToString.ToLower.Trim Then
                    RecurseXML(targetNode.ChildNodes(i), targetName, targetvalue)
                Else
                    For x As Integer = 0 To targetNode.ChildNodes.Count - 1
                        If enNode.ChildNodes(i).Name.ToString.ToLower.Trim = targetNode.ChildNodes(i).Name.ToString.ToLower.Trim Then
                            RecurseXML(targetNode.ChildNodes(i), targetName, targetvalue)
                        End If
                    Next
                End If

                If enName = targetName And envalue = targetvalue Then
                    oNode = New MyNode
                    oNode.Name = enName
                    oNode.Value = envalue
                    objNodes.Add(oNode)
                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return objNodes
    End Function

    Private Sub RecurseXML(nodes As XmlNode, ByRef Name As String, ByRef value As String)

        If nodes.HasChildNodes Then
            For Each node In nodes.ChildNodes
                If node.childnodes.count > 1 Then
                    RecurseXML(node.childnodes, "", "")
                Else
                    Name = node.ParentNode.Name
                    value = node.innertext
                    Exit Sub
                End If
            Next
        Else
            value = nodes.InnerText
            Name = nodes.ParentNode.Name
        End If

    End Sub

    Private Class MyNode
        Public Name As String
        Public Value As String
    End Class


    Private Class HybrisXml
        Public title As New ArrayList
        Public description As New ArrayList
        Public resource As New ArrayList
    End Class

#End Region

#Region "Create XML"

    Public Sub CreateHybrisXML(ByVal originalfile_path As String, ByVal translated_xliff_path As String, ByVal TT As TranslationType, ByVal lang As String)
        Dim targetfilepath As String = ""
        Dim notFound As New ArrayList
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
                    Exit Sub
                End If

                If CheckFileOrFolderExists(translated_xliff_path, fType.file) <> True Then
                    Exit Sub
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

            ' Dim notFound As New ArrayList

            Select Case TT
                Case TranslationType.Monolingual
                    'MonoLingual_Translation(targetfilepath, objXmlDefinition.Definitions, objXliff, lang)
                Case TranslationType.Multilingual
                    MultiLingual_HybrisTranslation(targetfilepath, objXliff, lang)
            End Select

            'NO Translation found then show a msg box and log it as well.
            If notFound.Count > 0 Then
                Dim objMissingTransaltion As New MissedTranslations
                objMissingTransaltion.UpdateMsg(notFound, originalfile_path, lang)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub MultiLingual_HybrisTranslation(ByRef targetFilePath As String, ByRef objXliff As sXliff, ByRef lang As String)

        Try
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(targetFilePath)

            Dim xNodeList As XmlNodeList

            For i As Integer = 0 To objXliff.ID.Count - 1
                Dim elementName As String = objXliff.ID(i)
                elementName = Mid(elementName, InStr(elementName, "_") + 1, Len(elementName))

                '<title> or '<description>
                If elementName = "title" Or elementName = "description" Then
                    xNodeList = xd.GetElementsByTagName(elementName)
                    For x As Integer = 0 To xNodeList.Count - 1
                        For iAtt As Integer = 0 To xNodeList(x).Attributes.Count - 1
                            If xNodeList(x).Attributes(iAtt).Name = "xml:lang" And xNodeList(x).Attributes(iAtt).Value = lang Then
                                'And xNodeList(x).InnerText = objXliff.Source(i) Then
                                For y As Integer = 0 To xNodeList(x).ChildNodes.Count - 1
                                    If GetPlainText(xNodeList(x).ChildNodes(y).InnerText.ToLower.Trim) = GetPlainText(objXliff.Source(i).ToString.ToLower.Trim) Then
                                        xNodeList(x).ChildNodes(y).InnerText = objXliff.Translation(i)
                                        Exit For
                                    End If
                                Next
                            End If
                        Next
                    Next
                Else
                    '<resources>
                    xNodeList = xd.GetElementsByTagName(elementName)

                    For x As Integer = 0 To xNodeList.Count - 1
                        If xNodeList(x).InnerText <> "" Then
                            For iAtt As Integer = 0 To xNodeList(x).ParentNode.Attributes.Count - 1
                                If xNodeList(x).ParentNode.Attributes(iAtt).Name = "xml:lang" _
                                    And xNodeList(x).ParentNode.Attributes(iAtt).Value = lang Then

                                    'And xNodeList(x).InnerText.ToLower.Trim = objXliff.Source(i).ToString.ToLower.Trim Then
                                    For y As Integer = 0 To xNodeList(x).ChildNodes.Count - 1
                                        If xNodeList(x).ChildNodes(y).InnerText.ToLower.Trim = objXliff.Source(i).ToString.ToLower.Trim Then
                                            xNodeList(x).ChildNodes(y).InnerText = objXliff.Translation(i)
                                            Exit For
                                        End If
                                    Next

                                End If
                            Next
                        End If
                    Next
                End If
            Next

            xd.Save(targetFilePath)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

#End Region

End Module
