Imports Microsoft.VisualBasic.FileIO
Imports System.IO
Imports System.ComponentModel
Imports System.Xml
Imports System.Net
Imports TidyManaged


Public Module HybrisImpex


#Region "Create Impex back"

    'Public Function xliff_To_Impex(ByVal EnFileName As String, ByVal TargetFileName As String, ByVal Targetlanguage As String, ByVal TranslatedxliffFile As String, ByRef Bw As BackgroundWorker) As Boolean
    '    Try
    '        'sample test files
    '        'EnFileName = "C:\Users\C5195092\Desktop\SfTest\HybrisTest\01-Input-B\cms-content_en.impex"

    '        If TargetFileName = "" Then
    '            TargetFileName = EnFileName
    '        End If

    '        ' TargetFileName = "C:\Users\C5195092\Desktop\SfTest\HybrisTest\01-Input-B\products_de.impex"

    '        Dim DT As DataTable = Get_Impex_toDatatable(TargetFileName, True)

    '        '2. Load Translated xliff data
    '        Dim objXliff As New sXliff
    '        Try
    '            objXliff = ModHelper.load_xliff(TranslatedxliffFile)
    '        Catch ex As Exception
    '            If ModHelper.UnWrapXliffBack(TranslatedxliffFile) <> True Then
    '                Throw New Exception("Error UnWrapping xliff back!")
    '            End If
    '            objXliff = ModHelper.cvload_xliff(Application.StartupPath & "\Temp_UnWrap.xliff")
    '        End Try

    '        Dim notFound As New ArrayList

    '        If objXliff.ID.Count = 0 Then
    '            UpdateMsg(Now & Chr(9) & "No translations found in " & System.IO.Path.GetFileName(TranslatedxliffFile) & vbCrLf, Form_MainNew.RtbColor.Red, Bw)
    '            'Throw New Exception("0 translations found in " & System.IO.Path.GetFileName(TranslatedxliffFile))
    '        End If

    '        '//To check if item has xml embedded
    '        DT = GetXmlTranslationIfPresent(DT, objXliff)

    '        '//Normal translation
    '        For x As Integer = 0 To objXliff.ID.Count - 1
    '            Dim clrXliffSource As String = GetPlainText(LCase(objXliff.Source(x)))
    '            Dim header As String = objXliff.ID(x)
    '            header = Mid(header, InStrRev(header, "_") + 1, Len(header))
    '            For j As Integer = 0 To DT.Rows.Count - 1
    '                If Not IsDBNull(DT.Rows(j).Item(header)) Then
    '                    Dim clrDTSource As String = GetPlainText(LCase(DT.Rows(j).Item(header)))
    '                    If (DT.Rows(j).Item((header)) = objXliff.Source(x)) Or (clrDTSource = clrXliffSource) Then
    '                        DT.Rows(j).Item((header)) = Chr(34) & Replace(objXliff.Translation(x), Chr(34), Chr(34) & Chr(34)) & Chr(34)
    '                        'Exit For
    '                    End If
    '                End If
    '            Next
    '        Next

    '        'Build back csv
    '        'define target path
    '        Dim targetfilepath As String
    '        Dim sFileName As String = System.IO.Path.GetFileName(EnFileName)

    '        'Monolingual
    '        targetfilepath = Replace(EnFileName, "01-Input-B", "05-Output")
    '        targetfilepath = System.IO.Path.GetDirectoryName(targetfilepath) & "\Mono_" & Targetlanguage & "\"

    '        If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(targetfilepath)) <> True Then
    '            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetfilepath))
    '        End If

    '        targetfilepath = targetfilepath & sFileName

    '        PutColonsInDatatable(DT)

    '        'Write csv file.
    '        Dim utf8WithoutBom As New System.Text.UTF8Encoding(False)
    '        Using writer As StreamWriter = New StreamWriter(targetfilepath, False, System.Text.Encoding.UTF8)
    '            WriteDataTableToImpex_Hybris(DT, writer, Targetlanguage)
    '        End Using

    '        'NO Translation found then show a msg box and log it as well.
    '        If notFound.Count > 0 Then
    '            Dim objMissingTransaltion As New MissedTranslations
    '            objMissingTransaltion.UpdateMsg(notFound, EnFileName, Targetlanguage)
    '        End If
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try
    '    Return True
    'End Function



    Public Function xliff_To_Impex(ByVal EnFileName As String, ByVal TargetFileName As String, ByVal Targetlanguage As String, ByVal TranslatedxliffFile As String, ByRef Bw As BackgroundWorker) As Boolean
        Try


            If TargetFileName = "" Then
                TargetFileName = EnFileName
            End If

            Dim DT As DataTable = Get_Impex_toDatatable(TargetFileName, True)

            '2. Load Translated xliff data
            Dim objXliff As New sXliff
            Try
                objXliff = ModHelper.load_xliff(TranslatedxliffFile)
            Catch ex As Exception
                If ModHelper.UnWrapXliffBack(TranslatedxliffFile) <> True Then
                    Throw New Exception("Error UnWrapping xliff back!")
                End If
                objXliff = ModHelper.cvload_xliff(Application.StartupPath & "\Temp_UnWrap.xliff")
            End Try


            'Dim strAsBytes() As Byte = New System.Text.UTF8Encoding().GetBytes(objXliff.Translation(24))
            'Dim ms As New System.IO.MemoryStream(strAsBytes)


            'Using doc As TidyManaged.Document = TidyManaged.Document.FromStream(ms)
            '    doc.InputCharacterEncoding = TidyManaged.EncodingType.Utf8
            '    doc.OutputCharacterEncoding = TidyManaged.EncodingType.Utf8
            '    doc.CleanAndRepair()
            '    Dim parsed As String = doc.Save()
            '    Debug.Print(parsed)
            'End Using



            Dim notFound As New ArrayList

            If objXliff.ID.Count = 0 Then
                UpdateMsg(Now & Chr(9) & "No translations found in " & System.IO.Path.GetFileName(TranslatedxliffFile) & vbCrLf, Form_MainNew.RtbColor.Red, Bw)
            End If

            '//To check if item has xml embedded
            DT = GetXmlTranslationIfPresent(DT, objXliff)

            Dim AllText As String = System.IO.File.ReadAllText(appData & DefinitionFiles.HybrisImpex_List)
            Dim HybrisDefinition As String() = Split(AllText, vbNewLine)

            Dim AllHybrisTags As ArrayList = GetHybrisTag(DT)

            For i As Integer = 1 To HybrisDefinition.Count - 1
                If Not AllHybrisTags.Contains(HybrisDefinition(i)) Then
                    AllHybrisTags.Add(HybrisDefinition(i))
                End If
            Next

            Dim En_Keynodes As New ArrayList
            Dim Target_Keynodes As New ArrayList

            For i As Integer = 0 To AllHybrisTags.Count - 1
                Dim objEn As HybrissNode = New HybrissNode(AllHybrisTags(i))
                objEn.getNode(DT)
                If objEn.MyKeyNode.row.Count <> 0 Then
                    En_Keynodes.Add(objEn.MyKeyNode)
                End If
            Next

            enCollection = New ArrayList

            'Taking "UPDATE Product" as base for loop
            For x As Integer = 0 To En_Keynodes.Count - 1
                If En_Keynodes(x).row.count = En_Keynodes(x).col.count Then
                    For iEn As Integer = 0 To En_Keynodes(x).row.count - 1

                        Dim enRow As Integer = En_Keynodes(x).row(iEn) + 1

                        For enStart As Integer = enRow To DT.Rows.Count - 1
                            Dim bFound As Boolean = False
                            Dim enCol As Integer = En_Keynodes(x).col(iEn)


                            'Do until string has [], example $contentCV[unique=true] 
                            If DT.Rows(enStart).Item(enCol).ToString.Contains("[") _
                                    And DT.Rows(enStart).Item(enCol).ToString.Contains("]") _
                                    And Microsoft.VisualBasic.Right(DT.Rows(enStart).Item(enCol).ToString.Trim, 1) = "]" Then
                                Exit For
                            End If

                            'If the below
                            If Microsoft.VisualBasic.Left(DT.Rows(enStart).Item(0).ToString.Trim, 13) = "INSERT_UPDATE" _
                                    Or Microsoft.VisualBasic.Left(DT.Rows(enStart).Item(0).ToString.Trim, 6) = "UPDATE" Then
                                Exit For
                            End If

                            If Not IsDBNull(DT.Rows(enStart).Item(enCol)) Then
                                Dim enUs As String = DT.Rows(enStart).Item(enCol)
                                ' Debug.Print(enUs & vbNewLine)
                                If enUs.Length > 0 Then 'New Scenario when xml file is embedded
                                    If Not enUs.Contains("</xh:html>") Then
                                        DT.Rows(enStart).Item(enCol) = GetHybrisTranslation(enUs, objXliff)
                                        Debug.Print(DT.Rows(enStart).Item(enCol))
                                    End If
                                End If
                            End If
                        Next

                    Next
                End If
            Next

            'Build back csv
            'define target path
            Dim targetfilepath As String
            Dim sFileName As String = System.IO.Path.GetFileName(EnFileName)

            'Monolingual
            targetfilepath = Replace(EnFileName, "01-Input-B", "05-Output")
            targetfilepath = System.IO.Path.GetDirectoryName(targetfilepath) & "\Mono_" & Targetlanguage & "\"

            If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(targetfilepath)) <> True Then
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetfilepath))
            End If

            targetfilepath = targetfilepath & sFileName

            PutColonsInDatatable(DT)

            For i As Integer = 0 To DT.Rows.Count - 1
                If Microsoft.VisualBasic.Left(DT.Rows(i).Item(0).ToString.ToLower.Trim, 8) = "$lang=en" Then
                    DT.Rows(i).Item(0) = "$lang=de"
                    Exit For
                End If
            Next

            'Write csv file.
            Dim utf8WithoutBom As New System.Text.UTF8Encoding(False)
            Using writer As StreamWriter = New StreamWriter(targetfilepath, False, System.Text.Encoding.UTF8)
                WriteDataTableToImpex_Hybris(DT, writer, Targetlanguage)
            End Using

            'NO Translation found then show a msg box and log it as well.
            If notFound.Count > 0 Then
                Dim objMissingTransaltion As New MissedTranslations
                objMissingTransaltion.UpdateMsg(notFound, EnFileName, Targetlanguage)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return True

    End Function


    Function GetHybrisTranslation(ByVal enUS As String, ByVal objXliff As sXliff) As String
        Dim sTransaltion As String = enUS
        For i As Integer = 0 To objXliff.Source.Count - 1
            Dim clrXliffSource As String = GetPlainText(LCase(objXliff.Source(i)))
            Dim clrDTSource As String = GetPlainText(enUS)
            If (clrXliffSource.Trim.ToLower = clrDTSource.Trim.ToLower) Or (objXliff.Source(i) = enUS) Then
                sTransaltion = objXliff.Translation(i)
                Exit For
            End If
        Next

        If sTransaltion = "" Then
            Return sTransaltion
        End If

        Return Chr(34) & sTransaltion.Replace(Chr(34), Chr(34) & Chr(34)) & Chr(34)

    End Function


    Function GetXmlTranslationIfPresent(ByVal DT As DataTable, ByVal objXliff As sXliff) As DataTable
        'This function is used to translate only xml content that is embedded in cell

        For iRow As Integer = 0 To DT.Rows.Count - 1
            For iCol As Integer = 0 To DT.Columns.Count - 1
                If Not IsDBNull(DT.Rows(iRow).Item(iCol).ToString) Then
                    If Microsoft.VisualBasic.Right(DT.Rows(iRow).Item(iCol).ToString, 10).ToLower = "</xh:html>" Then
                        DT.Rows(iRow).Item(iCol) = ProcessEmbeddedXmlenUsContent(DT.Rows(iRow).Item(iCol).ToString, objXliff)
                    End If
                End If
            Next
        Next

        Return DT
    End Function

    Function ProcessEmbeddedXmlenUsContent(ByVal enUs As String, ByRef ObjXliff As sXliff) As String
        'Embedded xml is passed as string and parsed with xmldocument
        'Based on the subtag from HybrisDefintion, it will clone the node and append to parent node
        'return the xmldocument innerxml

        Dim xd As New Xml.XmlDocument
        xd.PreserveWhitespace = True

        Try
            Dim enUScol As New ArrayList

            If Not System.IO.File.Exists(Application.StartupPath & "\Definition\HYBRIS_Definition\HybrisEmbeddedXmlTagList.txt") Then
                Throw New Exception("Error: Could not find HybrisEmbeddedXmlTagList.txt file")
            End If

            Dim AllText As String = System.IO.File.ReadAllText(Application.StartupPath & "\Definition\HYBRIS_Definition\HybrisEmbeddedXmlTagList.txt")
            Dim HybrisDefinition As String() = Split(AllText, vbNewLine)

            xd.XmlResolver = Nothing

            xd.LoadXml(enUs)

            For i As Integer = 4 To UBound(HybrisDefinition)
                Dim Expand As String() = Split(HybrisDefinition(i), "|")
                Dim MainTag As String = Expand(0)

                Dim xNodeList As XmlNodeList = xd.GetElementsByTagName(MainTag.ToLower)

                For x As Integer = 0 To xNodeList.Count - 1
                    Dim CN As XmlNode = xNodeList(x).CloneNode(True)
                    For a As Integer = 0 To CN.Attributes.Count - 1
                        If CN.Attributes(a).Name.ToLower = "xml:lang" Then
                            CN.Attributes(a).Value = "de"
                        End If
                    Next
                    NewRecNode(CN, Expand, enUScol, ObjXliff)
                    xNodeList(x).ParentNode.InsertAfter(CN, xNodeList(x))
                Next

            Next
        Catch ex As Exception
            Throw New Exception("Error @ProcessEmbeddedXmlenUsContent" & vbNewLine & ex.Message)
        End Try

        Return Chr(34) & xd.InnerXml.Replace(Chr(34), Chr(34) & Chr(34)) & Chr(34)

    End Function


    Sub NewRecNode(ByRef xNode As XmlNode, ByVal Expand As String(), ByRef enUScol As ArrayList, ByRef ObjXliff As sXliff)

        For i As Integer = 0 To xNode.ChildNodes.Count - 1
            If MatchSubTag(Expand, xNode.ChildNodes(i).Name) Then
                If enUScol.Contains(xNode.ChildNodes(i).InnerText.Trim) <> True And xNode.ChildNodes(i).InnerText <> String.Empty And IsNumeric(xNode.ChildNodes(i).InnerText) <> True Then
                    For x As Integer = 0 To ObjXliff.Source.Count - 1
                        If xNode.ChildNodes(i).InnerText.ToLower = ObjXliff.Source(x).ToString.ToLower Then
                            xNode.ChildNodes(i).InnerText = ObjXliff.Translation(x)
                        End If
                    Next
                End If
            End If

            If xNode.ChildNodes(i).HasChildNodes Then
                NewRecNode(xNode.ChildNodes(i), Expand, enUScol, ObjXliff)
            End If

        Next

    End Sub

    Private Sub WriteDataTableToImpex_Hybris(ByVal sourceTable As DataTable, ByVal writer As TextWriter, ByVal lang As String)
        Dim rowValues As List(Of String)
        lang = GetShort_lang(lang)

        Try

            For j As Integer = 0 To sourceTable.Rows.Count - 1
                rowValues = New List(Of String)()
                For i As Integer = 0 To sourceTable.Columns.Count - 1
                    'If sourceTable.Rows(j).Item(i).ToString.Trim <> "" Then
                    rowValues.Add((sourceTable.Rows(j).Item(i).ToString))
                    'End If
                Next
                If rowValues.Count = 0 Then
                    writer.WriteLine(String.Join("", rowValues))
                ElseIf (rowValues(0).ToString.Trim.Length > 1) And (Left(rowValues(0), 1) = "#" Or Left(rowValues(0), 1) = "$") Then
                    If rowValues(0).Trim.ToLower = "$lang=en" Then
                        rowValues(0) = "$lang=" & lang
                    End If
                    writer.WriteLine(String.Join("", rowValues))
                Else
                    Dim bEmpty As Boolean = True
                    For i As Integer = 0 To rowValues.Count - 1
                        If rowValues(i) <> "" Then
                            bEmpty = False
                            Exit For
                        End If
                    Next

                    writer.WriteLine(String.Join("", rowValues))

                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            writer.Flush()
        End Try

    End Sub

#End Region

#Region "Create Xliff"
    Public Function CreateXliff(ByVal EnFileName As String, ByVal TargetFileName As String, ByVal Targetlanguage As String, ByVal xliff_savePath As String) As String

        Try
            'sample test files
            'EnFileName = "C:\Users\C5195092\Desktop\SfTest\HybrisTest\01-Input-B\cms-content_en.impex"
            ' TargetFileName = ""
            'TargetFileName = "C:\Users\C5195092\Desktop\SfTest\HybrisTest\01-Input-B\cms-content_de.impex"
            xliff_savePath = xliff_savePath & "\" & System.IO.Path.GetFileNameWithoutExtension(EnFileName) & "_" & Targetlanguage & ".xliff"
            Dim bTargetFile As Boolean = False

            Dim enDT As DataTable = Get_Impex_toDatatable(EnFileName, False)
            Dim TargetDT As DataTable = Nothing

            If TargetFileName = "" Then
                bTargetFile = True
            Else
                TargetDT = Get_Impex_toDatatable(TargetFileName, False)
            End If

            Dim AllText As String = System.IO.File.ReadAllText(appData & DefinitionFiles.HybrisImpex_List)
            Dim HybrisDefinition As String() = Split(AllText, vbNewLine)

            Dim AllHybrisTags As ArrayList = GetHybrisTag(enDT)

            For i As Integer = 1 To HybrisDefinition.Count - 1
                If Not AllHybrisTags.Contains(HybrisDefinition(i)) Then
                    AllHybrisTags.Add(HybrisDefinition(i))
                End If
            Next

            Dim En_Keynodes As New ArrayList
            Dim Target_Keynodes As New ArrayList

            For i As Integer = 0 To AllHybrisTags.Count - 1
                Dim objEn As HybrissNode = New HybrissNode(AllHybrisTags(i))
                objEn.getNode(enDT)
                If objEn.MyKeyNode.row.Count <> 0 Then
                    En_Keynodes.Add(objEn.MyKeyNode)
                End If

                If Not bTargetFile Then
                    Dim objTarget As HybrissNode = New HybrissNode(AllHybrisTags(i))
                    objTarget.getNode(TargetDT)
                    If objEn.MyKeyNode.row.Count <> 0 Then
                        Target_Keynodes.Add(objEn.MyKeyNode)
                    End If
                End If
            Next

            If Not bTargetFile Then
                If En_Keynodes.Count <> Target_Keynodes.Count Then
                    Throw New Exception("Error: en row/columns not matching with Target row/columns!")
                End If
            End If

            Dim myNum As Integer = 0
            Using Writer As StreamWriter = New StreamWriter(xliff_savePath, False, System.Text.Encoding.UTF8)
                Writer.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                Writer.WriteLine("<xliff version=""1.2"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ")
                Writer.WriteLine("xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2- &#xD;&#xA;strict.xsd"">")
                Writer.WriteLine("<file original=""C:\Temp\en_fr_2.xliff"" source-language=" & Chr(34) & "en-US" & Chr(34) & " target-language=" & Chr(34) & Replace(GetLong_lang(Targetlanguage), "_", "-") & Chr(34) & " datatype=""plaintext"" date=" & Chr(34) & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & Chr(34) & " tool-id=""SAP SFSF CONVERTER"" category=""MLT"">")
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

                enCollection = New ArrayList

                'Taking "UPDATE Product" as base for loop
                For x As Integer = 0 To En_Keynodes.Count - 1
                    If En_Keynodes(x).row.count = En_Keynodes(x).col.count Then
                        For iEn As Integer = 0 To En_Keynodes(x).row.count - 1

                            Dim enRow As Integer = En_Keynodes(x).row(iEn) + 1

                            For enStart As Integer = enRow To enDT.Rows.Count - 1
                                Dim bFound As Boolean = False
                                Dim enCol As Integer = En_Keynodes(x).col(iEn)
                                'If IsDBNull(enDT.Rows(enStart).Item(enCol)) Then
                                '    Exit For
                                'End If
                                'If Not CheckHashorNull(enDT.Rows(enStart).Item(0)) Then 'Exit if Column 0 has data
                                '    Exit For
                                'End If

                                'Do until string has [], example $contentCV[unique=true] 
                                If enDT.Rows(enStart).Item(enCol).ToString.Contains("[") _
                                    And enDT.Rows(enStart).Item(enCol).ToString.Contains("]") _
                                    And Microsoft.VisualBasic.Right(enDT.Rows(enStart).Item(enCol).ToString.Trim, 1) = "]" Then
                                    Exit For
                                End If

                                'If the below
                                If Microsoft.VisualBasic.Left(enDT.Rows(enStart).Item(0).ToString.Trim, 13) = "INSERT_UPDATE" _
                                    Or Microsoft.VisualBasic.Left(enDT.Rows(enStart).Item(0).ToString.Trim, 6) = "UPDATE" Then
                                    Exit For
                                End If

                                If Not IsDBNull(enDT.Rows(enStart).Item(enCol)) Then
                                    Dim enUs As String = enDT.Rows(enStart).Item(enCol)
                                    Debug.Print(enUs & vbNewLine)
                                    If enUs.Length > 10 Then 'New Scenario when xml file is embedded
                                        If Microsoft.VisualBasic.Right(enUs, 10).ToLower = "</xh:html>" Then
                                            WriteComplexNode(Writer, enUs, myNum, enCol, En_Keynodes, x)
                                        Else
                                            WriteNodeSimple(Writer, TargetDT, En_Keynodes, Target_Keynodes, enUs, enCol, myNum, bTargetFile, x, iEn)
                                        End If
                                    Else
                                        WriteNodeSimple(Writer, TargetDT, En_Keynodes, Target_Keynodes, enUs, enCol, myNum, bTargetFile, x, iEn)
                                    End If
                                End If
                            Next

                        Next
                    End If
                Next

                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

            End Using

            If myNum = 0 Then
                Try
                    System.IO.File.Delete(xliff_savePath)
                    Return " Already has translation for " & Replace(Targetlanguage, "-", "_")
                Catch ex As Exception
                    'do nothing
                End Try
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return ""
    End Function

    Function GetHybrisTag(ByRef DT As DataTable) As ArrayList
        Dim HybrisTag As New ArrayList
        For iCol As Integer = 0 To DT.Columns.Count - 1
            For iRow As Integer = 0 To DT.Rows.Count - 1
                If DT.Rows(iRow).Item(iCol).ToString.Trim.ToLower.Contains("[lang=$lang]") Or DT.Rows(iRow).Item(iCol).ToString.Trim.ToLower.Contains("[lang=en]") Then
                    If Not HybrisTag.Contains(DT.Rows(iRow).Item(iCol).ToString) Then
                        HybrisTag.Add(DT.Rows(iRow).Item(iCol).ToString)
                    End If
                End If
            Next
        Next
        Return HybrisTag
    End Function


    Dim enCollection As ArrayList

    Sub WriteComplexNode(ByRef Writer As StreamWriter, ByRef enUs As String, ByRef myNum As Integer, ByRef enCol As Integer, ByRef En_Keynodes As ArrayList, ByRef x As Integer)

        Dim enUcol As ArrayList = GetEmbeddedXmlenUsContent(enUs)
        Dim iCounter As Integer = enCollection.Count

        For i As Integer = 0 To enUcol.Count - 1
            If Not enCollection.Contains(enUcol(i)) Then
                enCollection.Add(enUcol(i))
            End If
        Next

        For i As Integer = iCounter To enCollection.Count - 1
            If CanbeAccepted(enCollection(i)) Then
                myNum += 1
                Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & En_Keynodes(x).key & "_" & enCol & Chr(34) & " resname=" & Chr(34) & "Hybris" & Chr(34) & ">")
                Writer.WriteLine("<source>" & wrap_html(clean_xml(enCollection(i))) & "</source>")
                Writer.WriteLine("<target state=""needs-review-translation""></target>")
                Writer.WriteLine("<note from=""Developer"" priority =""10"">" & "hybris : " & "Impex" & "</note>")
                Writer.WriteLine("</trans-unit>")
                Writer.WriteLine(vbCrLf)
            End If
        Next

    End Sub

    Function GetEmbeddedXmlenUsContent(ByRef enUs) As ArrayList

        Dim enUScol As New ArrayList

        If Not System.IO.File.Exists(Application.StartupPath & "\Definition\HYBRIS_Definition\HybrisEmbeddedXmlTagList.txt") Then
            Throw New Exception("Error: Could not find HybrisEmbeddedXmlTagList.txt file")
        End If

        Dim AllText As String = System.IO.File.ReadAllText(Application.StartupPath & "\Definition\HYBRIS_Definition\HybrisEmbeddedXmlTagList.txt")
        Dim HybrisDefinition As String() = Split(AllText, vbNewLine)


        Dim xd As New Xml.XmlDocument
        xd.XmlResolver = Nothing

        xd.LoadXml(enUs)


        For i As Integer = 4 To UBound(HybrisDefinition)
            Dim Expand As String() = Split(HybrisDefinition(i), "|")
            Dim MainTag As String = Expand(0)

            Dim xNodeList As XmlNodeList = xd.GetElementsByTagName(MainTag.ToLower)

            'Now Loop the Maintag to get sub tag, like alert,label,hint etc
            RecNode(xNodeList, Expand, enUScol)

        Next

        Return enUScol

    End Function

    Sub RecNode(ByRef xNodelist As XmlNodeList, ByVal Expand As String(), ByRef enUScol As ArrayList)
        For i As Integer = 0 To xNodelist.Count - 1

            If MatchSubTag(Expand, xNodelist(i).Name) Then
                If enUScol.Contains(xNodelist(i).InnerText.Trim) <> True And xNodelist(i).InnerText <> String.Empty And IsNumeric(xNodelist(i).InnerText) <> True Then
                    enUScol.Add(xNodelist(i).InnerText.Trim)
                End If
            End If
            If xNodelist(i).HasChildNodes Then
                RecNode(xNodelist(i).ChildNodes, Expand, enUScol)
            End If
        Next
    End Sub

    Function MatchSubTag(ByVal Expand As String(), ByVal TagName As String) As Boolean
        For i As Integer = 1 To UBound(Expand) - 1
            If Expand(i).ToLower = TagName.ToLower Then
                Return True
            End If
        Next
        Return False
    End Function


    Sub WriteNodeSimple(ByRef Writer As StreamWriter, ByRef TargetDT As DataTable, ByRef En_Keynodes As ArrayList, ByRef Target_Keynodes As ArrayList, ByRef enUS As String, ByRef enCol As Integer, ByRef myNum As Integer, ByVal bTargetFile As Boolean, ByRef x As Integer, ByRef iEn As Integer)
        Dim bFound As Boolean = False
        If Not bTargetFile Then
            If enUS.Trim <> "" Then
                Dim targetRow As Integer = Target_Keynodes(x).row(iEn) + 1

                For targetStart = targetRow To TargetDT.Rows.Count - 1
                    Dim targetCol As Integer = Target_Keynodes(x).col(iEn)
                    If IsDBNull(TargetDT.Rows(targetStart).Item(targetCol)) Then
                        Exit For
                    End If
                    If Not IsDBNull(TargetDT.Rows(targetStart).Item(targetCol)) Then
                        Dim target As String = TargetDT.Rows(targetStart).Item(targetCol)

                        If enUS.Trim.ToLower = target.Trim.ToLower Then
                            bFound = True
                            Exit For
                        End If
                    End If
                Next

            End If
        Else
            bFound = True
        End If

        If bFound And CanbeAccepted(enUS) Then
            myNum += 1
            Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & En_Keynodes(x).key & "_" & enCol & Chr(34) & " resname=" & Chr(34) & "Hybris" & Chr(34) & ">")
            Writer.WriteLine("<source>" & wrap_html(clean_xml(enUS)) & "</source>")
            Writer.WriteLine("<target state=""needs-review-translation""></target>")
            Writer.WriteLine("<note from=""Developer"" priority =""10"">" & "hybris : " & "Impex" & "</note>")
            Writer.WriteLine("</trans-unit>")
            Writer.WriteLine(vbCrLf)
        End If

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

    Private Function bKeyFound(ByRef Hybris() As String, ByVal Key As String) As Boolean
        For i As Integer = 1 To UBound(Hybris)
            If Key.Trim.ToLower = Hybris(i).Trim.ToLower Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Class HybrissNode
        Public _key As String
        Public Sub New(ByVal Key As String)
            _key = Key
        End Sub

        Public MyKeyNode As New KeyNodes

        Public Sub getNode(ByRef DT As DataTable)
            MyKeyNode.Key = _key
            Try
                'Taking UPDATE Product for looping start to end
                'For iCol As Integer = 0 To DT.Columns.Count - 1
                '    For iRow As Integer = 0 To DT.Rows.Count - 1
                '        If DT.Rows(iRow).Item(iCol).ToString.Trim.ToLower = _key.ToLower Then
                '            MyKeyNode.row.Add(iRow)
                '            MyKeyNode.col.Add(iCol)
                '        End If
                '    Next
                'Next


                For iRow As Integer = 0 To DT.Rows.Count - 1
                    If Microsoft.VisualBasic.Left(DT.Rows(iRow).Item(0).ToString.Trim.ToUpper, 13) = "INSERT_UPDATE" _
                        Or Microsoft.VisualBasic.Left(DT.Rows(iRow).Item(0).ToString.Trim.ToUpper, 6) = "UPDATE" _
                        Or Microsoft.VisualBasic.Left(DT.Rows(iRow).Item(0).ToString.Trim.ToUpper, 6) = "DELETE" Then
                        For eRow As Integer = iRow + 1 To DT.Rows.Count - 1
                            If Microsoft.VisualBasic.Left(DT.Rows(eRow).Item(0).ToString.Trim.ToUpper, 13) = "INSERT_UPDATE" _
                                Or Microsoft.VisualBasic.Left(DT.Rows(eRow).Item(0).ToString.Trim.ToUpper, 6) = "UPDATE" _
                                Or Microsoft.VisualBasic.Left(DT.Rows(eRow).Item(0).ToString.Trim.ToUpper, 6) = "DELETE" _
                                Or DT.Rows.Count - 1 = eRow Then

                                For iCol As Integer = 0 To DT.Columns.Count - 1
                                    If DT.Rows(iRow).Item(iCol).ToString.Trim.ToLower = _key.ToLower Then
                                        MyKeyNode.row.Add(iRow)
                                        MyKeyNode.col.Add(iCol)
                                    End If
                                Next

                                iRow = eRow - 1
                                Exit For
                            End If
                        Next
                    End If
                Next


            Catch ex As Exception
                Throw New Exception("Error @HybrissNode" & vbNewLine & ex.Message)
            End Try

        End Sub

        Public Class KeyNodes
            Public Key As String
            Public row As New ArrayList
            Public col As New ArrayList
        End Class

    End Class
#End Region

#Region "Helper"
    Public Function Get_Impex_toDatatable(ByVal sCsvFilePath As String, ByVal bWithBlankLines As Boolean) As DataTable

        Dim csvData As New DataTable()
        Try
            'First Get the Max last column used in csv file, this is used to set the column limit for datatable------------------------------------------
            Dim LastCol As Integer = 0
            Using csvReader As New TextFieldParser(sCsvFilePath, System.Text.Encoding.UTF8)
                csvReader.SetDelimiters(New String() {";"})
                csvReader.HasFieldsEnclosedInQuotes = True
                While Not csvReader.EndOfData
                    Dim fieldData As String() = csvReader.ReadFields
                    If LastCol < UBound(fieldData) Then
                        LastCol = UBound(fieldData)
                    End If
                End While
            End Using
            '---------------------------------------------------------------------------------------------------------------------------------------------

            'Now load the csv data to datatable
            Using csvReader As New TextFieldParser(sCsvFilePath, System.Text.Encoding.UTF8)
                csvReader.SetDelimiters(New String() {";"})
                csvReader.HasFieldsEnclosedInQuotes = True
                csvReader.TrimWhiteSpace = False


                For i As Integer = 0 To LastCol
                    csvData.Columns.Add(i, GetType(String))
                Next

                While Not csvReader.EndOfData
                    Dim fieldData As String() = csvReader.ReadFields()
                    'Making empty value as null
                    For i As Integer = 0 To fieldData.Length - 2
                        If fieldData(i) = "" Then
                            fieldData(i) = Nothing
                        End If
                    Next
                    csvData.Rows.Add(fieldData)
                End While
            End Using


            Dim bBlank As New ArrayList

            If bWithBlankLines Then

                'PutColonsInDatatable(csvData)

                'now place blank lines
                Dim AllText As New ArrayList

                Using csvReader As New TextFieldParser(sCsvFilePath)
                    csvReader.SetDelimiters(New String() {";"})
                    csvReader.HasFieldsEnclosedInQuotes = True
                    csvReader.TrimWhiteSpace = False
                    While Not csvReader.EndOfData
                        Dim fieldData As String = csvReader.ReadLine
                        AllText.Add(fieldData)
                    End While
                End Using

                For i As Integer = 1 To AllText.Count - 1
                    If AllText(i) = "" Then
                        Dim s() As String = Split(AllText(i + 1), ";")
                        Dim bFound As Boolean = False
                        For j As Integer = 0 To bBlank.Count - 1
                            If bBlank(j).ToString.Trim = s(0).ToString.Trim Then
                                bFound = True
                            End If
                        Next
                        If Not bFound Then
                            bBlank.Add(s(0))
                        End If
                    End If
                Next

                For i As Integer = 0 To bBlank.Count - 1
                    If bBlank(i) <> "" Then
                        getMyDt(csvData, bBlank(i))
                    End If
                Next

            End If


        Catch ex As Exception
            Throw New Exception("Error loading Impex file to datatable!" & vbNewLine & ex.Message)
        End Try

        Return csvData

    End Function


    Private Sub getMyDt(ByRef csvData As DataTable, ByVal bBlank As String)
        For z As Integer = 0 To csvData.Rows.Count - 1
            If csvData.Rows(z).Item(0).ToString.Trim.ToLower = bBlank.ToString.Trim.ToLower _
                Or csvData.Rows(z).Item(0).ToString.Trim.ToLower = bBlank.ToString.Trim.ToLower & ";" Then
                Dim d As DataRow = csvData.NewRow
                csvData.Rows.InsertAt(d, z)
                z += 1
            End If
        Next
    End Sub

    Private Sub PutColonsInDatatable(ByRef csvData As DataTable)
        Try
            'For i As Integer = 0 To csvData.Rows.Count - 1
            '    If CheckHashorNull(csvData.Rows(i).Item(0)) <> True Then
            '        For j As Integer = i + 1 To csvData.Rows.Count - 1
            '            If Not IsDBNull(csvData.Rows(j).Item(0)) Or j = csvData.Rows.Count - 1 Then

            '                Dim lastCol As Integer = 0
            '                'Get Exact column number
            '                For u As Integer = 0 To csvData.Columns.Count - 1
            '                    If Not IsDBNull(csvData.Rows(i).Item(u)) Then
            '                        lastCol += 1
            '                    End If
            '                Next

            '                For y As Integer = 0 To lastCol - 2
            '                    If IsDBNull(csvData.Rows(i).Item(y)) Then
            '                        Exit For
            '                    End If
            '                    Dim lastRow As Integer
            '                    If j = csvData.Rows.Count - 1 Then
            '                        lastRow = j
            '                    Else
            '                        lastRow = j - 2
            '                    End If
            '                    For x As Integer = i To lastRow
            '                        If IsDBNull(csvData.Rows(x).Item(y)) Then
            '                            csvData.Rows(x).Item(y) = ";"
            '                        Else
            '                            If csvData.Rows(x).Item(y) = "#" Then
            '                                csvData.Rows(x).Item(y) = "#;"
            '                            Else
            '                                csvData.Rows(x).Item(y) = csvData.Rows(x).Item(y) & ";"
            '                            End If
            '                        End If
            '                    Next
            '                Next
            '                i = j - 1
            '                Debug.Print(i)
            '                Exit For
            '            End If
            '        Next
            '    End If
            'Next



            For iRow As Integer = 0 To csvData.Rows.Count - 1
                If Microsoft.VisualBasic.Left(csvData.Rows(iRow).Item(0).ToString.Trim.ToUpper, 13) = "INSERT_UPDATE" _
                    Or Microsoft.VisualBasic.Left(csvData.Rows(iRow).Item(0).ToString.Trim.ToUpper, 6) = "UPDATE" _
                    Or Microsoft.VisualBasic.Left(csvData.Rows(iRow).Item(0).ToString.Trim.ToUpper, 6) = "DELETE" Then
                    For eRow As Integer = iRow + 1 To csvData.Rows.Count - 1
                        If Microsoft.VisualBasic.Left(csvData.Rows(eRow).Item(0).ToString.Trim.ToUpper, 13) = "INSERT_UPDATE" _
                            Or Microsoft.VisualBasic.Left(csvData.Rows(eRow).Item(0).ToString.Trim.ToUpper, 6) = "UPDATE" _
                            Or Microsoft.VisualBasic.Left(csvData.Rows(eRow).Item(0).ToString.Trim.ToUpper, 6) = "DELETE" _
                            Or csvData.Rows.Count - 1 = eRow Then

                            Dim lastCol As Integer = 0
                            'Get Exact column number
                            For u As Integer = 0 To csvData.Columns.Count - 1
                                If Not IsDBNull(csvData.Rows(iRow).Item(u)) Then
                                    lastCol += 1
                                End If
                            Next

                            Dim lRow As Integer = eRow
                            If eRow <> csvData.Rows.Count - 1 Then
                                lRow = eRow - 1
                            End If

                            For xRow As Integer = iRow To lRow
                                If Microsoft.VisualBasic.Left(csvData.Rows(xRow).Item(0).ToString, 1) <> "#" And CheckRowIsBlank(csvData.Rows(xRow)) = False Then
                                    For xCol As Integer = 0 To lastCol - 2
                                        If IsDBNull(csvData.Rows(xRow).Item(xCol)) Then
                                            csvData.Rows(xRow).Item(xCol) = ";"
                                        Else
                                            If csvData.Rows(xRow).Item(xCol) = "#" Then
                                                csvData.Rows(xRow).Item(xCol) = "#;"
                                            Else
                                                csvData.Rows(xRow).Item(xCol) = csvData.Rows(xRow).Item(xCol) & ";"
                                            End If
                                        End If
                                    Next
                                End If
                            Next
                            iRow = eRow - 1
                            Exit For
                        End If
                    Next
                End If
            Next

        Catch ex As Exception
            Throw New Exception("Error @PutColonsInDatatable" & vbNewLine & ex.Message)
        End Try

    End Sub

    Private Function CheckRowIsBlank(ByVal ro As DataRow) As Boolean
        Dim bEmpty As Boolean = True
        For i As Integer = 0 To ro.ItemArray.Count - 1
            If ro.Item(i).ToString <> String.Empty Then
                bEmpty = False
                Exit For
            End If
        Next
        Return bEmpty
    End Function


    Private Function CheckHashorNull(ByVal str As Object) As Boolean
        If IsDBNull(str) Then 'Check # is there
            Return True
        Else
            If str = "#" Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
#End Region


End Module
