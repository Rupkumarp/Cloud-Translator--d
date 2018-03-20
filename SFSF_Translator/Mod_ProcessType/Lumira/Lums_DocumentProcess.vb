Imports System.Xml
Imports Newtonsoft.Json.Linq
Imports System.IO

Module Lums_DocumentProcess



#Region "Extract Xliff"

    Dim arr As ArrayList
    Public Function LumiraDocumentExtractXliff(ByVal sDocumentXmlFile As String, ByVal sXliffLocation As String, ByVal Lang As String)
        Try
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing

            xd.Load(sDocumentXmlFile)

            Dim xNodeList As XmlNodeList
            xNodeList = xd.GetElementsByTagName("hi:jsonDef")

            arr = New ArrayList

            For i As Integer = 0 To xNodeList.Count - 1
                For j As Integer = 0 To xNodeList(i).ChildNodes.Count - 1
                    Dim CData As XmlCDataSection = xNodeList(i).ChildNodes(j)  '.ChildNodes(0).ChildNodes(0)

                    '  MyKeyPair(CData.InnerText, False)
                    Dim token As JToken = RemoveEmptyChildren(JToken.Parse(CData.InnerText), False)
                    xNodeList(i).ChildNodes(j).InnerText = token.ToString
                Next
            Next

            xNodeList = xd.GetElementsByTagName("hi:riv_definition")

            For i As Integer = 0 To xNodeList.Count - 1
                For j As Integer = 0 To xNodeList(i).Attributes.Count - 1
                    If xNodeList(i).Attributes(j).Name.ToLower = "title" Then

                        If Not arr.Contains(xNodeList(i).Attributes(j).InnerText) Then
                            arr.Add(wrap_html((clean_xml(xNodeList(i).Attributes(j).InnerText))))
                        End If
                    End If
                Next
            Next

            LumiraDocumentWriteXliff(sXliffLocation, Lang, "jsonDef", arr)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return ""
    End Function


    Private Function LumiraDocumentWriteXliff(ByVal xliff_Path As String, ByVal Targetlanguage As String, ByVal resName As String, ByVal enSource As ArrayList) As Boolean

        ' xliff_Path = System.IO.Path.GetFileNameWithoutExtension(xliff_Path) & "_" & Targetlanguage & ".xliff"

        Try

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

                For i As Integer = 1 To enSource.Count - 1
                    Dim myNum As Integer = myNum + 1
                    If enSource(i) <> "" Then
                        Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & (Replace(Replace(("Lumira"), "<", ""), ">", "") & Chr(34) & " resname=" & Chr(34) & clean_xml("name") & Chr(34) & ">"))
                        Writer.WriteLine("<source>" & (wrap_html(clean_xml(enSource(i)))) & "</source>")
                        Writer.WriteLine("<target state=""needs-review-translation""></target>")
                        Writer.WriteLine("<note from=""Developer"" priority =""10"">" & ("Lumira") & ": " & (Replace(Replace(resName, "<", ""), ">", "") & "</note>"))
                        Writer.WriteLine("</trans-unit>")
                        Writer.WriteLine(vbCrLf)
                        myNum = myNum + 1
                        Counter += 1
                    End If
                    myNum = myNum - 1
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

#End Region

    Dim j As New ArrayList
    Dim l As Integer = 0

    Dim objXliff As sXliff
    Private Function RemoveEmptyChildren(ByRef token As JToken, ByVal bReintegrate As Boolean) As JToken

        l += 1

        If token.Type = JTokenType.Object Then

            Dim copy As JObject = New JObject()

            For Each prop As JProperty In token.Children(Of JProperty)()

                If Not prop.Value.HasValues Then
                    If prop.Name.ToLower = "text" Or prop.Name.ToLower = "title" Or prop.Name.ToLower = "name" Then

                        If bReintegrate Then
                            For j As Integer = 0 To objXliff.Source.Count - 1

                                If objXliff.Source(j).ToString.ToLower = wrap_html((clean_xml(prop.Value.ToString.ToLower))) Then

                                    Dim jv As New Newtonsoft.Json.Linq.JValue(unwrap_html(revert_xml(objXliff.Source(j) & "Sunil" & j)))
                                    prop.Value = jv
                                    Exit For
                                End If
                            Next
                        Else

                            'If prop.Value.ToString = "Heating Energy Consumption Terajoules by Year" Then
                            '    MsgBox("ok")
                            'End If

                            If Not IsNumeric(prop.Value.ToString) _
                                And Microsoft.VisualBasic.Right(prop.Value.ToString, 4) <> ".png" _
                                And Microsoft.VisualBasic.Right(prop.Value.ToString, 4) <> ".jpg" _
                                And Microsoft.VisualBasic.Right(prop.Value.ToString, 4) <> ".jpeg" Then
                                If Not arr.Contains(wrap_html((clean_xml(prop.Value)))) Then
                                    arr.Add(wrap_html((clean_xml(prop.Value))))
                                End If
                            End If

                        End If
                    End If
                End If

                Dim child As JToken = prop.Value
                If child.HasValues Then
                    child = RemoveEmptyChildren(child, bReintegrate)
                End If

                copy.Add(prop.Name, child)
            Next

            Return copy

        ElseIf token.Type = JTokenType.Array Then

            Dim copy As JArray = New JArray()

            For Each child As JToken In token.Children()

                If child.HasValues Then
                    child = RemoveEmptyChildren(child, bReintegrate)
                End If

                copy.Add(child)
            Next

            Return copy

        End If

        Return token

    End Function


#Region "ReIntegrate"

    Public Function LumiraDocumentReIntegrate(ByVal sXliff_File As String, ByVal sHeaderXmlFile As String, ByVal sOutFile As String, ByVal Lang As String) As String
        Try
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing

            xd.Load(sHeaderXmlFile)

            Dim xNodeList As XmlNodeList
            xNodeList = xd.GetElementsByTagName("hi:jsonDef")

            objXliff = load_xliff(sXliff_File)

            For i As Integer = 0 To xNodeList.Count - 1
                For j As Integer = 0 To xNodeList(i).ChildNodes.Count - 1
                    Dim CData As XmlCDataSection = xNodeList(i).ChildNodes(j)  '.ChildNodes(0).ChildNodes(0)

                    '  MyKeyPair(CData.InnerText, False)
                    Dim token As JToken = RemoveEmptyChildren(JToken.Parse(CData.InnerText), True)
                    xNodeList(i).ChildNodes(j).InnerText = token.ToString
                Next
            Next

            xNodeList = xd.GetElementsByTagName("hi:riv_definition")

            For i As Integer = 0 To xNodeList.Count - 1
                For j As Integer = 0 To xNodeList(i).Attributes.Count - 1
                    If xNodeList(i).Attributes(j).Name.ToLower = "title" Then

                        For x As Integer = 0 To objXliff.Source.Count - 1
                            If objXliff.Source(x).ToString.ToLower = wrap_html((clean_xml(xNodeList(i).Attributes(j).InnerText.ToLower))) Then
                                xNodeList(i).Attributes(j).InnerText = objXliff.Translation(x)
                                Exit For
                            End If
                        Next

                    End If
                Next
            Next


            xd.Save(sOutFile)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return ""
    End Function

#End Region




End Module

Public NotInheritable Class JsonHelper
    Private Sub New()
    End Sub
    Public Shared Function Deserialize(json As String) As Object
        Return ToObject(JToken.Parse(json))
    End Function

    Private Shared Function ToObject(token As JToken) As Object
        Select Case token.Type
            Case JTokenType.[Object]
                Return token.Children(Of JProperty)().ToDictionary(Function(prop) prop.Name, Function(prop) ToObject(prop.Value))

            Case JTokenType.Array
                Return token.[Select](AddressOf ToObject).ToList()
            Case Else

                Return DirectCast(token, JValue).Value
        End Select
    End Function
End Class

