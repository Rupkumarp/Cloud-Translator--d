Imports System.IO
Imports System.Xml

''' <summary>
''' Removes translation from target lang from xml and csv files 
''' </summary>
''' <remarks></remarks>
Public Class CleanTranslation 'Error Number 100 - 120

    Private ErrNumber As Integer

    Public Enum RtbColor
        Red
        Green
        Black
    End Enum

    Public Event UpdateMsg(ByVal Msg As String, ByVal RTBC As RtbColor)

    Public Event Progress(ByVal Max As Integer, ByVal val As Integer)

    Private ActiveProject As ProjectDetail = ProjectManagement.GetActiveProject

#Region "Remove XML multi lang Translations"
    Public Function RemoveTranslationXML(ByVal sFileName As String, ByRef lang() As String, Optional ByVal bUpdateToDBbeforeCleaning As Boolean = False) As Boolean
        ErrNumber = 102
        Try
            Dim objXmlDefinition As New XmlDefinition
            objXmlDefinition.GetXmlDefinition(sFileName)

            RemoveTranslationFromXml(sFileName, objXmlDefinition, lang, bUpdateToDBbeforeCleaning)

        Catch ex As Exception
            Throw New Exception("Error Number - " & ErrNumber & vbNewLine & ex.Message)
        End Try
        Return True
        ErrNumber = 0
    End Function

    Private CT As New List(Of Cloud_TR.CloudTr)

    Private Sub RemoveTranslationFromXml(ByRef xmlFile As String, ByRef objXmlDefinition As XmlDefinition, ByVal lang() As String, ByVal bUpdateToDBbeforeCleaning As Boolean)

        Dim bLanguage As Boolean
        Dim bXmlLang As Boolean
        Dim bSuppLang As Boolean

        Dim xd As New Xml.XmlDocument

        xd.XmlResolver = Nothing
        xd.Load(xmlFile)

        Dim ResName As String = ""
        Dim enUS As String = ""
        If bUpdateToDBbeforeCleaning Then
            Dim objXmlDefiniton As New XmlDefinition
            objXmlDefiniton.GetXmlDefinition(xmlFile)
            ResName = objXmlDefinition.Position
        End If

        For x As Integer = 1 To objXmlDefinition.Definitions.Count - 1

            Dim ElementName As String = objXmlDefinition.Definitions(x)

            If ElementName.Trim <> "" Then

                Dim iElementIndex As Integer = CheckElementisPresentInDefinition(ElementName, objXmlDefinition.Definitions)
                bLanguage = objXmlDefinition.bLang(iElementIndex)
                bXmlLang = objXmlDefinition.bXmlLang(iElementIndex)
                bSuppLang = objXmlDefinition.bAddSuppLang

                Dim xNodeList As XmlNodeList

                xNodeList = xd.GetElementsByTagName(Replace(Replace(ElementName, "<", ""), ">", ""))

                Dim MyAttributes As XmlAttributeCollection
                Dim str As String = ""
                Dim root As XmlNode = Nothing
                'Dim xElement As XmlElement
                Dim bCdata As Boolean = False
                Dim bFound As Boolean = False
                ' Dim CData As XmlCDataSection
                'First compare source and innertext, if match then assign the node to Root - Gets the correct node here
                Dim bSourceFound As Boolean = False

                Dim XElement As XmlElement = Nothing
                Dim vLang As String = "lang"
                Dim i As Integer = 0

                Do Until xNodeList.Count - 1 < i 'As node will be increased when adding clone nodes, so get updated nodelist for every loop
                    bFound = False
                    root = xNodeList(i)
                    bSourceFound = True
                    'Based on the nodes parentnode, check the element inside the particular node.
                    'Next check for lang, if lang found, translation is available, then exit and save, else put the translation back
                    If bSourceFound Then
                        If root.Name = Replace(Replace(ElementName, "<", ""), ">", "") Then
                            Dim subNode As XmlNode = root
                            If subNode.Attributes.Count > 0 Then
                                MyAttributes = subNode.Attributes
                                Dim att As XmlAttribute

                                If bLanguage = True And bXmlLang = False Then
                                    vLang = "lang"
                                ElseIf bLanguage = False And bXmlLang = True Then
                                    vLang = "xml:lang"
                                Else
                                    vLang = "lang"
                                End If

                                For Each att In MyAttributes
                                    If InStr(LCase(att.Name), vLang) > 0 Then
                                        If GetLangForXml(lang, att.Value) Then
                                            bFound = True
                                            XElement = subNode
                                            If InStr(LCase(root.InnerXml), "cdata") > 0 Then
                                                If Check_enUS_Attribute_Available(root.ParentNode, Replace(Replace(ElementName, "<", ""), ">", "")) Then
                                                    XElement.ParentNode.RemoveChild(XElement)
                                                    i -= 1
                                                End If
                                            Else
                                                If Check_enUS_Attribute_Available(root.ParentNode, Replace(Replace(ElementName, "<", ""), ">", "")) Then
                                                    XElement.ParentNode.RemoveChild(XElement)
                                                    i -= 1
                                                End If
                                            End If
                                            ' root.ParentNode.InsertAfter(XElement, root)
                                            ''If bUpdateToDBbeforeCleaning Then
                                            ''    UpdateCTxml(enUS, XElement, att.Value, ResName, System.IO.Path.GetFileNameWithoutExtension(xmlFile))
                                            ''End If
                                        End If

                                    End If

                                Next
                            Else 'enUS node
                                enUS = root.InnerText
                            End If
                            bFound = False
                        End If
                    Else
                        root = xNodeList(0)
                    End If

                    bFound = False
                    i += 1
                Loop
                RaiseEvent UpdateMsg(Now & Chr(9) & "Removed translation for Xml Definition - " & objXmlDefinition.Definitions(x) & vbCrLf, RtbColor.Black)
            End If

        Next

        xd.Save(xmlFile)

        'If bUpdateToDBbeforeCleaning Then
        '    RaiseEvent UpdateMsg(Now & Chr(9) & "Importing existing translation to DB. Please wait..." & vbCrLf, RtbColor.Black)
        '    Dim counter As Integer = 0
        '    For i As Integer = 0 To CT.Count - 1
        '        objCT = New Cloud_TR.Service1
        '        If objCT.UpdateCloud(CT(i)) Then
        '            counter += 1
        '        End If
        '        RaiseEvent Progress(CT.Count, i)
        '    Next
        '    RaiseEvent UpdateMsg(Now & Chr(9) & "Updated DB with " & counter & "\" & CT.Count & vbCrLf, RtbColor.Black)
        'End If
        '

    End Sub

    Private WithEvents objCT As Cloud_TR.Service1


    ''' <summary>
    ''' Update to DB when Cleaning for xml only
    ''' </summary>
    ''' <param name="enUScontent"></param>
    ''' <param name="ele"></param>
    ''' <param name="Lang"></param>
    ''' <param name="resName"></param>
    ''' <param name="FileName"></param>
    ''' <remarks></remarks>
    Private Sub UpdateCTxml(ByVal enUScontent As String, ByVal ele As XmlElement, ByVal Lang As String, ByVal resName As String, ByVal FileName As String)

        Try
            Dim ctr As New Cloud_TR.CloudTr

            With ctr
                .Customer = ActiveProject.CustomerName
                .CustomerSpecific = 0
                .Datatype = FileName
                .Instance = ActiveProject.InstaneName
                .Maxlength = 0
                .Resname = "SunilsTest" & resName
                .Source = enUScontent
                .Target = ele.InnerText
                .SourceLang = "enUS"
                .TargetLang = Lang.Replace("_", "").Replace("-", "")
            End With

            CT.Add(ctr)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    'Cleaning occurs only if the enUS is present or when no attributes with en_US.
    Private Function Check_enUS_Attribute_Available(ByVal xNode As XmlNode, ByVal elementname As String) As Boolean
        If xNode.ChildNodes.Count = 0 Then
            Return True
        End If

        Dim MyAttributes As XmlAttributeCollection = Nothing
        For i As Integer = 0 To xNode.ChildNodes.Count - 1
            If xNode.ChildNodes(i).Name.ToLower = elementname.ToLower Then
                If xNode.ChildNodes(i).Attributes.Count = 0 Then 'This means its enUS, when no attribures ex:
                    Return True
                Else
                    Dim att As XmlAttribute
                    MyAttributes = xNode.ChildNodes(i).Attributes
                    For Each att In MyAttributes
                        If att.Value.ToLower = "en_us" Or att.Value = "enus" Then
                            Return True
                        End If
                    Next
                End If
            End If
        Next
        Return False
    End Function

    Private Function GetLangForXml(ByVal lang() As String, ByVal searchLang As String) As Boolean
        For i As Integer = 0 To UBound(lang)
            Dim sLang As String = searchLang.Replace("-", "_")
            Dim lLang As String = lang(i).Replace("-", "_")
            If InStr(sLang.ToLower, lLang.ToLower) Then
                Return True
            End If
        Next
        Return False
    End Function

#End Region

#Region "Remove MDF CSV Translations"
    '''  ''' <summary>
    ''' <para>Remove MDF CSV Translations.</para> 
    ''' <para>Get FileDetails by running Cls_GetLanglist.LangList </para>
    ''' </summary>
    ''' <param name="sFileName"></param>
    ''' <param name="lang"></param>
    ''' <param name="fDetails"></param>
    '''  ''' <param name="iStart">Row number</param>
    ''' <remarks>Get FileDetails by running Cls_GetLanglist.LangList</remarks>
    Public Sub RemoveTranslationMDF(ByVal sFileName As String, ByRef lang() As String, ByRef fDetails As FileDetails, Optional ByVal iStart As Integer = -1)
        ErrNumber = 103
        Try
            Dim P As New CsvParser
            Dim dt As DataTable = P.GetDataTabletFromCSVFile(sFileName)

            If iStart = -1 Then
                iStart = 1
            End If

            For i As Integer = 0 To UBound(lang)
                Dim LangColumns As ArrayList = getLangColumnFromMdf(fDetails, lang(i))
                For j As Integer = 0 To LangColumns.Count - 1
                    For m As Integer = iStart To dt.Rows.Count - 1
                        'UpdateCTcsv(dt.Rows(m).Item(LangColumns(j)),dt.Rows(m).Item(LangColumns(j)),lang(i),
                        dt.Rows(m).Item(LangColumns(j)) = ""
                    Next
                Next
                RaiseEvent UpdateMsg(Now & Chr(9) & "Removed translation for language - " & lang(i) & vbCrLf, RtbColor.Black)
            Next

            Using writer As StreamWriter = New StreamWriter(sFileName, False, System.Text.Encoding.UTF8)
                WriteDataTable(dt, writer)
            End Using

        Catch ex As Exception
            Throw New Exception("Error Number - " & ErrNumber & vbNewLine & ex.Message)
        End Try

        ErrNumber = 0
    End Sub

    Private Function getLangColumnFromMdf(ByRef fDetails As FileDetails, ByVal searchLang As String) As ArrayList
        Dim x As New ArrayList
        For i As Integer = 0 To fDetails.ColumnRepeatedLangIndex.Count - 1
            If fDetails.ColumnRepeatedLangIndex(i).ToString.ToLower = Replace(searchLang.ToLower, "_", "") Then
                x.Add(fDetails.ColumnNumberList(i))
            End If
        Next
        Return x
    End Function

    
#End Region

End Class
