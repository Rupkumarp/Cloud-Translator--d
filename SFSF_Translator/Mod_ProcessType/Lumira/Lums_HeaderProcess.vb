Imports System.Xml
Imports System.IO

Module Lums_HeaderProcess

#Region "Extraction"

    Public Function LumiraHeaderExtractXliff(ByVal sHeaderXmlFile As String, ByVal sXliff_Location As String, ByVal Lang As String)
        Dim Msg As String = ""

        Try
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing

            xd.Load(sHeaderXmlFile)

            Dim xNodeList As XmlNodeList
            xNodeList = xd.GetElementsByTagName("hi:property")


            Dim arr As New ArrayList
            For i As Integer = 0 To xNodeList.Count - 1
                Debug.Print(xNodeList(i).OuterXml)
                If xNodeList(i).Attributes(0).Name.ToLower = "name" And xNodeList(i).Attributes(0).Value.ToLower = "name" Then
                    If Microsoft.VisualBasic.Right(xNodeList(i).InnerText, 4).ToLower <> ".png" Then
                        arr.Add(xNodeList(i).InnerText)
                    End If
                End If

            Next

            LumsXml_ToXliff(sXliff_Location, Lang, "Header", arr)


        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return Msg
    End Function

#End Region

#Region "ReIntegrate"

    Public Function LumiraHeaderReintegrate(ByVal sXliff_File As String, ByVal sHeaderXmlFile As String, ByVal sOutFile As String, ByVal Lang As String)
        Try
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing

            xd.Load(sHeaderXmlFile)

            Dim xNodeList As XmlNodeList
            xNodeList = xd.GetElementsByTagName("hi:property")

            Dim objXliff As sXliff = load_xliff(sXliff_File)

            xNodeList = xd.GetElementsByTagName("hi:property")

            For i As Integer = 0 To xNodeList.Count - 1
                Debug.Print(xNodeList(i).OuterXml)
                If xNodeList(i).Attributes(0).Name.ToLower = "name" And xNodeList(i).Attributes(0).Value.ToLower = "name" Then
                    If Microsoft.VisualBasic.Right(xNodeList(i).InnerText, 4).ToLower <> ".png" Then
                        For j As Integer = 0 To objXliff.Source.Count - 1
                            If objXliff.Source(j).ToString.ToLower = xNodeList(i).InnerText.ToLower Then
                                xNodeList(i).InnerText = objXliff.Translation(j)
                            End If
                        Next
                    End If
                End If

            Next

            xd.Save(sOutFile)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return ""
    End Function

#End Region



    Private Function LumsXml_ToXliff(ByVal xliff_Path As String, ByVal Targetlanguage As String, ByVal resName As String, ByVal enSource As ArrayList) As Boolean

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

End Module
