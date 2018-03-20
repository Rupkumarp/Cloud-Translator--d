Imports System.Environment
Imports System.Text
Imports System.Xml
Imports System.Text.RegularExpressions
Imports System.IO

Public Class CpXml

    ' Dim _xCPdata As List(Of CPData)
    Dim _cplist As ArrayList
    Dim _cpIndex As Integer
    Dim _lang As String
    Dim _objCpData As List(Of CpLangList)
    Dim _ChildFileName As String

    Public Sub New(ByVal objCpData As List(Of CpLangList), ByVal lang As String, ByVal cpList As ArrayList, ByVal cpIndex As Integer, Optional ByVal ChildFileName As String = "0")
        _objCpData = objCpData
        _cpIndex = cpIndex
        _cplist = cpList
        _lang = lang
        _ChildFileName = ChildFileName
    End Sub

    Public Function WriteTempCPxml(ByVal cmblang As ComboBox, ByVal isChildFile As Boolean) As String

        '---------------Writing to string directly----------------------------------------------------------------------------------------
        'Dim sContent As New StringBuilder

        'Try
        '    Dim bFound As Boolean = False
        '    Dim iStart As Integer

        '    sContent.Append("<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf)
        '    Dim i As Integer = 0
        '    For i = 0 To _cplist.Count - 1
        '        iStart = InStr(_cplist(i), "|||")
        '        Dim MyNumber As String
        '        MyNumber = Mid(_cplist(i), iStart, Len(_cplist(i)))

        '        If MyNumber = "|||" & _cpIndex Then
        '            If Left(_cplist(i), 1) = "@" Then
        '                sContent.Append("<!DOCTYPE " & clean_xml(Mid(_cplist(i).substring(1), 1, iStart - 2)) & ">" & vbCrLf)
        '                sContent.Append("<" & clean_xml(Mid(_cplist(i).substring(1), 1, iStart - 2)) & ">" & vbCrLf)
        '                sContent.Append(vbNewLine)
        '                bFound = True
        '                Exit For
        '            End If
        '        End If
        '    Next

        '    If bFound <> True Then
        '        Throw New Exception("Error creating xml!" & vbNewLine & "Object type not found")
        '    End If

        '    For j As Integer = 0 To _xCPdata.Count - 1
        '        For k As Integer = 0 To _xCPdata(j).Value.Count - 1
        '            If _xCPdata(j).isBold.Item(k) Then
        '                sContent.Append("<" & clean_xml(_xCPdata(j).LabelName.Item(k)) & ">" & clean_xml(_xCPdata(j).Value.Item(k)) & "</" & clean_xml(_xCPdata(j).LabelName.Item(k)) & ">" & vbCrLf)
        '            Else
        '                sContent.Append("<" & clean_xml(_xCPdata(j).LabelName.Item(k)) & ">" & vbCrLf)
        '                sContent.Append("<Label lang = " & Chr(34) & "en_Us" & Chr(34) & ">" & clean_xml(_xCPdata(j).Value.Item(k)) & "/Label>" & vbCrLf)
        '                sContent.Append("</" & clean_xml(_xCPdata(j).LabelName.Item(k)) & ">" & vbCrLf)
        '                sContent.Append(vbNewLine)
        '            End If
        '        Next
        '    Next

        '    sContent.Append("<" & clean_xml(Mid(_cplist(i).substring(1), 1, iStart - 2)) & ">")
        'Catch ex As Exception
        '    Throw New Exception(ex.Message)
        'End Try

        'Return sContent
        '----------------------------------------------------------------------------------------------------------------------------------------------


        'Using xmlwriter ------------------------------------------------------------------------------------------------------------------------------
        Dim bFound As Boolean = False
        Dim iStart As Integer
        Dim docType As String = ""

        Dim i As Integer = 0
        For i = 0 To _cplist.Count - 1
            iStart = InStr(_cplist(i), "|||")
            Dim MyNumber As String
            MyNumber = Mid(_cplist(i), iStart, Len(_cplist(i)))

            If MyNumber = "|||" & _cpIndex Then
                If Left(_cplist(i), 1) = "@" Then
                    docType = ModHelper.clean_xml(Mid(_cplist(i).substring(1), 1, iStart - 2))
                    bFound = True
                    Exit For
                End If
            End If
        Next

        If docType = "" Then
            Throw New Exception("Error could not find doctype")
        End If

        Dim sXmlFileName As String = GetFileNameFromDocType(docType)

        Dim writer As XmlWriter = Nothing
        Dim strm As MemoryStream = New MemoryStream()

        Try
            Dim settings As XmlWriterSettings = New XmlWriterSettings()
            settings.Indent = True
            settings.IndentChars = (ControlChars.Tab)
            settings.OmitXmlDeclaration = False
            settings.Encoding = Encoding.UTF8

            writer = XmlWriter.Create(strm, settings)
            writer.WriteStartDocument()
            writer.WriteDocType(clean_element(docType), Nothing, Nothing, "")

            writer.WriteStartElement(clean_element(docType)) ' Root.

            writer.WriteStartElement("ParentFileName")
            writer.WriteString(sXmlFileName)
            writer.WriteEndElement()

            writer.WriteStartElement("isChildFile")
            writer.WriteString(isChildFile)
            writer.WriteEndElement()

            writer.WriteStartElement("ChildFileName")
            writer.WriteString(_ChildFileName)
            writer.WriteEndElement()

            For j As Integer = 0 To _objCpData.Count - 1
                For k As Integer = 0 To _objCpData(j).xCpData.Count - 1
                    writer.WriteStartElement("MainTag")
                    For x As Integer = 0 To _objCpData(j).xCpData(k).Value.Count - 1
                        If _objCpData(j).xCpData(k).isBold.Item(x) Then
                            'writer.WriteElementString(clean_element(_objCpData(j).xCpData(k).LabelName.Item(x)), ModHelper.clean_xml(_objCpData(j).xCpData(k).Value.Item(x)))

                            writer.WriteStartElement(clean_element(_objCpData(j).xCpData(k).LabelName.Item(x)))
                            writer.WriteAttributeString("lang", _objCpData(j).Lang)
                            writer.WriteString(ModHelper.clean_xml(_objCpData(j).xCpData(k).Value.Item(x)))
                            writer.WriteEndElement()
                        Else
                            'Earlier version with <label> tags
                            'writer.WriteStartElement("SubTag")
                            'writer.WriteStartElement(clean_element(_objCpData(j).xCpData(k).LabelName.Item(x)))
                            'writer.WriteStartElement("Label")
                            'writer.WriteAttributeString("lang", _objCpData(j).Lang)
                            'writer.WriteString(ModHelper.clean_xml(_objCpData(j).xCpData(k).Value.Item(x)))
                            'writer.WriteEndElement()
                            'writer.WriteEndElement()
                            'writer.WriteEndElement()

                            'without <label> tag
                            writer.WriteStartElement("SubTag")
                            writer.WriteStartElement(clean_element(_objCpData(j).xCpData(k).LabelName.Item(x)))
                            writer.WriteAttributeString("lang", _objCpData(j).Lang)
                            writer.WriteString(ModHelper.clean_xml(_objCpData(j).xCpData(k).Value.Item(x)))
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                        End If
                    Next
                    writer.WriteEndElement()
                Next
            Next

            ' End document.
            writer.WriteEndElement()
            writer.WriteEndDocument()
            writer.Flush()
        Finally
            If Not (writer Is Nothing) Then
                writer.Close()
            End If
        End Try

        Dim xmlContent As String
        strm.Position = 0
        Dim sreader As StreamReader = New StreamReader(strm)
        xmlContent = sreader.ReadToEnd

        Return xmlContent

    End Function

    Public Function GetDocType(ByVal xmlFile As String) As String
        Dim docType As String = ""
        Try
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(xmlFile)
            docType = xd.DocumentType.OuterXml
            docType = Replace(docType, "<!DOCTYPE ", "")
            docType = Replace(docType, "[]>", "")
        Catch ex As Exception
            Return "Could not get xml Doctype"
        End Try
        Return docType

    End Function

    Public Function isChildFile(ByVal xmlFile As String) As Boolean

        Try
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(xmlFile)
            Dim xNodeList As XmlNodeList = xd.GetElementsByTagName("isChildFile")
            If xNodeList.Count <> 0 Then
                Return CBool(xNodeList(0).InnerText)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return False

    End Function

    Public Function GetFileNameFromDocType(ByVal docType As String)
        Dim xmlFileName As String = ""
        If Left(docType, 1) = "@" Then
            xmlFileName = Mid(docType, InStr(docType, "("), Len(docType))
            xmlFileName = Replace(xmlFileName, "(", "")
            xmlFileName = Replace(xmlFileName, ")", "")
            If InStr(xmlFileName, "-") > 0 Then
                xmlFileName = Mid(xmlFileName, 1, InStr(xmlFileName, "-") - 1)
            End If
            xmlFileName = xmlFileName & ".xml"
        End If
        Return xmlFileName
    End Function

End Class
