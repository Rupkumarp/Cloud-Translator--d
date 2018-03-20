Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.IO
Imports System.Configuration
Imports System.Data
Imports Microsoft.VisualBasic.FileIO
Imports System.Windows
Imports System.Exception
Imports System.Xml
Imports System.ComponentModel
Imports Microsoft.VisualBasic
Imports System.Text
Imports System.Dynamic
Imports System.Web.HttpUtility
Imports System.Text.RegularExpressions

Module Mod_Cube1
    Dim table_FileData = Nothing
    Dim iUniqueID As Integer = 0
    Dim Tagarray() As String = {"description", "title", "subtitle", "presenterName", "presenterRole", "text", "suggestedTitle", "suggestedSubTitle", "displayName", "name", "customizedTitle", "accountName"}
    Dim ExtnType() As String = {".ANI", ".BMP", ".CAL", ".FAX", ".GIF", ".IMG", ".JBG", ".JPE", ".JPEG", ".JPG", ".MAC", ".PBM", ".PCD", ".PCX", ".PCT",
                                ".PGM", ".PNG", ".PPM", ".PSD", ".RAS", ".TGA", ".TIFF", ".WMF", ".SVG"}
    ''' <summary>
    ''' -1 = Last Index
    ''' </summary>
    '''Dim DimensionStatus() As String = {"DIMENSION_3.-1", "DIMENSION_2.-1", "CUBE_2.2.4", "DIMENSION_4.-1"}
    Dim Filename As String = ""
    Dim obj_DimensionPopup As New FrmPopup_Dimension()
    Public tbl_DIMENSION As DataTable = Nothing

    Enum CUBEDATA
        PROP_ID = 0
        PROP_NAME = 1
        PROP_VAL = 2
    End Enum

#Region "Cube to Xliff"
    Public Function ToXliff(ByVal InputFilePath As String, ByVal OutPutFolder As String, ByVal Lang As String) As String

        'Dim Filename As String = ""
        Try
            Dim Lastindex_path As Integer = InputFilePath.LastIndexOf("\")
            Dim FilePath As String = InputFilePath.Substring(0, Lastindex_path)
            Filename = InputFilePath.Replace(FilePath + "\", "")

            '1. Make content as JSON format
            table_FileData = Nothing
            Dim fileContent_jsonformat As String = MakeFileContent_Json(InputFilePath)

            Dim OutFile As String = OutPutFolder & Path.GetFileNameWithoutExtension(Filename) & "_" & Lang & ".xliff"
            If Not File.Exists(OutFile) Then

                '' Check for DIMENSION Files
                If Filename.StartsWith("DIMENSION") Then
                    obj_DimensionPopup.FileName = Filename
                    obj_DimensionPopup.DIMENSION_Content = fileContent_jsonformat
                    obj_DimensionPopup.ShowDialog()

                    If tbl_DIMENSION IsNot Nothing Then
                        If tbl_DIMENSION.Rows.Count > 0 Then
                            'Dim OutFile As String = OutPutFolder & Path.GetFileNameWithoutExtension(Filename) & "_" & Lang & ".xliff"
                            CreateXliff(tbl_DIMENSION, OutFile, Lang)
                        End If
                    End If
                Else
                    '2. Extract JSON Content
                    Dim arrlist_EnUS As DataTable = GetJsonPropertyValue(fileContent_jsonformat)
                    '3. Create xliff
                    If arrlist_EnUS.Rows.Count > 0 Then
                        'Dim OutFile As String = OutPutFolder & Path.GetFileNameWithoutExtension(Filename) & "_" & Lang & ".xliff"
                        CreateXliff(arrlist_EnUS, OutFile, Lang)
                    End If
                End If

            End If
            ''2. Extract JSON Content
            'Dim arrlist_EnUS As DataTable = GetJsonPropertyValue(fileContent_jsonformat)
            ''Dim arrlist_EnUS As ArrayList = ListPeople(DirectCast(token.SelectToken("content.dimensions"), JArray))

            ''3. Create xliff
            'If arrlist_EnUS.Rows.Count > 0 Then
            '    Dim OutFile As String = OutPutFolder & Path.GetFileNameWithoutExtension(Filename) & "_" & Lang & ".xliff"

            '    CreateXliff(arrlist_EnUS, OutFile, Lang)
            'End If

        Catch ex As Exception
            'Dim xx As String = Filename
            Throw New Exception(ex.Message)
        End Try

    End Function
    Private Function MakeFileContent_Json(fullFilePath As String) As String
        Try
            Dim FileContent_Raw As String = IO.File.ReadAllText(fullFilePath)
            Dim output As String = FileContent_Raw.Substring(FileContent_Raw.IndexOf("{"))
            Dim FileContent_Json As String = output.Substring(0, output.LastIndexOf("}") + 1)

            Return FileContent_Json
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Function MakeFileContent_JsonNew(FileContent_Raw As String) As String
        Try
            Dim output As String = FileContent_Raw.Substring(FileContent_Raw.IndexOf("{"))
            Dim FileContent_Json As String = output.Substring(0, output.LastIndexOf("}") + 1)

            Return FileContent_Json
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function GetJsonPropertyValue(json As String) As DataTable

        Try
            '''' Content
            Dim Jsondata As JToken = JObject.Parse(json)
            Dim str_jsonContent As String = Jsondata.SelectToken("content").ToString()

            table_FileData = New DataTable
            table_FileData.Columns.Add("ID", GetType(String))
            table_FileData.Columns.Add("PropName", GetType(String))
            table_FileData.Columns.Add("PropVal", GetType(String))

            If (str_jsonContent.StartsWith("{") AndAlso str_jsonContent.EndsWith("}")) Then
                iUniqueID = 0
                Traverse_Json(str_jsonContent)
                Return table_FileData

                '''' [ Dimension Files are handle by another popup window. ]
                'ElseIf (str_jsonContent.StartsWith("[") AndAlso str_jsonContent.EndsWith("]")) Then
                '    iUniqueID = 0
                '    Traverse_Json_NoKey(str_jsonContent)
                '    Return table_FileData

            Else
                Console.WriteLine("Need to check if the file content is valid or not for translation")
                Return Nothing
            End If

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Sub CreateXliff(ByVal arrlist_EnUS As DataTable, ByVal xliff_Path As String, ByVal Targetlanguage As String)
        Using Writer As StreamWriter = New StreamWriter(xliff_Path, False, System.Text.Encoding.UTF8)
            Writer.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
            Writer.WriteLine("<xliff version=""1.2"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ")
            Writer.WriteLine("xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2- &#xD;&#xA;strict.xsd"">")
            Writer.WriteLine("<file original=""C:\Temp\en_fr_2.xliff"" source-language=""en-US"" target-language=" & Chr(34) & Targetlanguage & Chr(34) & " datatype=""plaintext"" date=" & Chr(34) & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & Chr(34) & " tool-id=""SAP SFSF CONVERTER"" category=""MLT"">")
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
            Dim myNum As Integer = 1

            For J As Integer = 0 To arrlist_EnUS.Rows.Count - 1

                Dim valCubedata_EnUS As String = arrlist_EnUS(J).ToString()

                Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(arrlist_EnUS.Rows(J)(CUBEDATA.PROP_ID).ToString) & Chr(34) & " resname=" & Chr(34) & clean_xml(arrlist_EnUS.Rows(J)(CUBEDATA.PROP_NAME).ToString) & Chr(34) & ">")
                Writer.WriteLine("<source>" & wrap_html(clean_xml(arrlist_EnUS.Rows(J)(CUBEDATA.PROP_VAL).ToString)) & "</source>")
                Writer.WriteLine("<target state=""needs-review-translation""></target>")
                Writer.WriteLine("<note from=""Developer"" priority =""10"">" & "DIBO : " & "TEST" & "</note>")
                Writer.WriteLine("</trans-unit>")
                Writer.WriteLine(vbCrLf)
                myNum += 1
            Next

            Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

        End Using
    End Sub

    Private Sub Traverse_Json(ByVal str_jsonContent As String)

        If str_jsonContent = "" Then Exit Sub
        If Not chk_jsonobject(str_jsonContent) Then Exit Sub

        Try
            Dim jObj = JObject.Parse(str_jsonContent)
            For icnt As Integer = 0 To jObj.Properties.Count - 1

                'If iUniqueID = 24 Then
                '    Dim xx As String = ""
                'End If

                Dim Prop_Name As String = jObj.Properties(icnt).Name.ToString
                Dim Prop_Val As String = jObj.Properties(icnt).Value.ToString.Replace("'", "''")

                If jObj.Properties(icnt).Value.Type = JTokenType.Array Then '' If inner Property
                    Dim obj = jObj.Properties(icnt).Value
                    For icnt_inner As Integer = 0 To obj.Count - 1
                        Dim innerProp As String = obj(icnt_inner).ToString

                        If (innerProp.StartsWith("{") AndAlso innerProp.EndsWith("}")) Then Traverse_Json(innerProp)

                        'End If

                    Next

                ElseIf Prop_Val.StartsWith("{") And Prop_Val.EndsWith("}") Then '' If inner Property
                    Traverse_Json(Prop_Val)
                Else
                    For Each value As String In Tagarray
                        If value = "content" Then
                            'Extract content value
                        Else
                            If (Prop_Name.ToLower = value.ToLower) And Prop_Val.Trim <> "" Then

                                '' ignore image type & other file type
                                Dim bool_image As Boolean = False
                                For Each extn As String In ExtnType
                                    If Prop_Val.Trim.ToUpper.EndsWith(extn) Then
                                        bool_image = True
                                        Exit For
                                    End If
                                Next
                                If bool_image = False Then
                                    If Not DataAlreadyExist(iUniqueID, Prop_Name, Prop_Val) Then
                                        iUniqueID += 1
                                        Dim str_SingleLine As String = Prop_Val.Replace(vbCr, String.Empty).Replace(vbLf, "\n")
                                        table_FileData.Rows.Add(iUniqueID.ToString(), Prop_Name, str_SingleLine)
                                        Exit For
                                    End If
                                End If

                            End If
                        End If

                    Next
                End If

            Next

        Catch jex As JsonReaderException
            ''Exception in parsing json
            'Console.WriteLine(jex.Message)
            Throw New Exception(jex.Message)

        Catch ex As Exception
            'Console.WriteLine(ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Private Function chk_jsonobject(ByVal str_jsonContent As String) As Boolean
        Try
            Dim jObj = JObject.Parse(str_jsonContent)
            Return True
        Catch jex As JsonReaderException
            ''Exception in parsing json
            'Console.WriteLine(jex.Message)
            Return False
        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Function DataAlreadyExist(ByVal iUniqueID As Integer, ByVal Prop_Name As String, ByVal Prop_Val As String) As Boolean

        If iUniqueID >= 1 Then

            If (table_FileData.Select("PropName = '" + Prop_Name + "' ").Length > 0) AndAlso (table_FileData.Select("PropVal = '" + Prop_Val + "' ").Length > 0) Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function



    ''Private Sub Traverse_Json_NoKey(ByVal str_jsonContent As String)
    ''    Try
    ''        str_jsonContent = str_jsonContent.Remove(0, 1)
    ''        str_jsonContent = str_jsonContent.Remove(str_jsonContent.Length - 1)
    ''        str_jsonContent = str_jsonContent.Replace(vbCr, "").Replace(vbLf, "")
    ''        str_jsonContent = str_jsonContent.Replace("],", "^")
    ''        str_jsonContent = str_jsonContent.Replace("[", "").Replace("]", "")

    ''        Dim json_EachBlock() As String = str_jsonContent.Split("^")

    ''        '' Loop for All file content (rows)
    ''        For icnt As Integer = 0 To json_EachBlock.Count - 1

    ''            Dim json_EachVal As String() = json_EachBlock(icnt).Split(",")
    ''            '' Get All Value position based on filename
    ''            Dim val_position As String = GetPosition_Value()

    ''            '' Loop for each rows
    ''            For icntindv As Integer = 1 To json_EachVal.Count

    ''                Dim ok_position As Boolean = False
    ''                '' Get Indivisual Value position based on filename
    ''                Dim indv_val_position As String() = val_position.Split(".")
    ''                For poscount As Integer = 0 To indv_val_position.Count - 1
    ''                    If icntindv = Convert.ToInt16(indv_val_position(poscount)) Then
    ''                        ok_position = True
    ''                        Exit For
    ''                    ElseIf json_EachVal.Length = icntindv And val_position.Contains("-1") Then
    ''                        ok_position = True
    ''                        Exit For
    ''                    End If
    ''                Next

    ''                If ok_position Then
    ''                    'Dim json_Lastval As String = LTrim(RTrim(json_EachVal(json_EachVal.Length - 1).Replace("""", "")))
    ''                    Dim json_Lastval As String = LTrim(RTrim(json_EachVal(icntindv - 1).Replace("""", "")))

    ''                    If json_Lastval <> "" And json_Lastval <> "#" And json_Lastval.ToUpper <> "NULL" Then
    ''                        Dim is_Currency As Boolean = isNumericVal(json_Lastval, System.Globalization.NumberStyles.Currency)
    ''                        Dim is_Float As Boolean = isNumericVal(json_Lastval, System.Globalization.NumberStyles.Float)
    ''                        'Dim is_HexNum As Boolean = isNumericVal(json_Lastval, System.Globalization.NumberStyles.HexNumber)
    ''                        Dim is_Integer As Boolean = isNumericVal(json_Lastval, System.Globalization.NumberStyles.Integer)
    ''                        Dim is_Number As Boolean = isNumericVal(json_Lastval, System.Globalization.NumberStyles.Number)

    ''                        If is_Currency = False And is_Float = False And is_Integer = False And is_Number = False Then
    ''                            iUniqueID += 1
    ''                            table_FileData.Rows.Add(iUniqueID.ToString(), "DUMMY_PROP_" + iUniqueID.ToString, json_Lastval)
    ''                        End If
    ''                    End If
    ''                End If

    ''            Next


    ''        Next

    ''    Catch ex As Exception
    ''        Throw New Exception(ex.Message)
    ''    End Try

    ''End Sub
    ''' <summary>
    ''' Based on the FileName(initial),return position against which pick the value
    ''' [By default last value(-1)]
    ''' </summary>
    ''' <returns></returns>
    ''Private Function GetPosition_Value() As String

    ''    Try
    ''        Dim val_pos As String = ""
    ''        For Each FileValuePair As String In DimensionStatus
    ''            Dim FileValuePair_Indv() As String = FileValuePair.Split(".")
    ''            Dim FileInitial As String = FileValuePair_Indv(0)
    ''            If (Filename.Contains(FileInitial)) Then val_pos = FileValuePair.Replace(FileInitial + ".", "")
    ''        Next

    ''        If val_pos = "" Then
    ''            Return "-1"
    ''        Else
    ''            Return val_pos
    ''        End If

    ''    Catch ex As Exception
    ''        Throw New Exception(ex.Message)
    ''    End Try

    ''End Function
    Public Function isNumericVal(val As String, NumberStyle As System.Globalization.NumberStyles) As Boolean
        Dim result As [Double]
        Return [Double].TryParse(val, NumberStyle, System.Globalization.CultureInfo.CurrentCulture, result)
    End Function

    Public Sub Traverse(name As String, j As JToken)
        For Each token As JToken In j.AsJEnumerable()
            If token.Type = JTokenType.[Object] Then
                For Each pair As Object In TryCast(token, JObject)
                    Dim name_ As String = pair.Key
                    Dim child As JToken = pair.Value
                    Traverse(name, child)
                Next
            ElseIf token.Type = JTokenType.Array Then
                'an array property found 
                For Each child As Object In token.Children()
                    Traverse(DirectCast(j, JProperty).Name, child)
                Next
            ElseIf token.Type = JTokenType.[Property] Then
                Dim [property] = TryCast(token, JProperty)
                'current level property
                Traverse(name, DirectCast(token, JContainer))
            Else
                'current level property name & value
                Dim nm = ""
                Dim t = ""
                If TypeOf j Is JProperty Then
                    nm = DirectCast(j, JProperty).Name
                    t = Convert.ToString(DirectCast(j, JProperty).Value)
                End If
                t = Convert.ToString(token)
            End If
        Next
    End Sub
#End Region

#Region "XLIFF to CUBE"
    Public Function ToActualFile(ByVal OriginalFile As String, ByVal TranslatedxliffFile As String, ByVal Targetfilepath As String, ByVal Lang As String) As Boolean
        Dim sFileName As String = ""
        Dim Convertedfilepath As String = ""
        Try
            sFileName = System.IO.Path.GetFileName(OriginalFile)

            'Monolingual
            'targetfilepath = Replace(OriginalFile, "01-Input-B", "05-Output")
            'targetfilepath = System.IO.Path.GetDirectoryName(targetfilepath) & "\Mono_" & Lang & "\"

            'If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(targetfilepath)) <> True Then
            '    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetfilepath))
            'End If

            Convertedfilepath = Targetfilepath
            File.Copy(OriginalFile, Convertedfilepath, True)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Try

            '1. Get Actual File Content 
            Dim FileContent_Raw As String = IO.File.ReadAllText(Convertedfilepath)

            '2. Load Translated xliff data          
            Dim objXliff As New sXliff

            Try
                'objXliff = load_xliff(TranslatedxliffFile & "\" & "(translated) " & sFileName & ".xliff")
                objXliff = load_xliff(TranslatedxliffFile)
            Catch ex As Exception
                If UnWrapXliffBack(TranslatedxliffFile) <> True Then
                    Throw New Exception("Error UnWrapping xliff back!")
                End If
                objXliff = cvload_xliff("C:" & "\Temp_UnWrap.xliff")
            End Try

            If objXliff.ID.Count = 0 Then
                'UpdateMsg(Now & Chr(9) & "No translations found for " & System.IO.Path.GetFileName(TranslatedxliffFile) & vbCrLf, Form_MainNew.RtbColor.Red)
                Throw New Exception("0 translations found in " & System.IO.Path.GetFileName(TranslatedxliffFile))
            End If

            '' Compare & Marge
            Compare_Xliff_Actual(FileContent_Raw, objXliff, Convertedfilepath)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function
    Private Function Compare_Xliff_Actual(ByVal FileContent_Raw As String, ByVal objXliff As Object, ByVal Convertedfilepath As String) As Boolean

        Try
            FileContent_Raw.Replace("\n", String.Empty)
            '''' Fill Conversion details (Xliff) in DataTable
            Dim table_XliffData = New DataTable
            table_XliffData.Columns.Add("ID", GetType(String))
            table_XliffData.Columns.Add("SourceName", GetType(String))
            table_XliffData.Columns.Add("SourceVal", GetType(String))
            table_XliffData.Columns.Add("Translation", GetType(String))

            For icnt_ID As Integer = 0 To objXliff.ID.Count - 1
                table_XliffData.Rows.Add()
                table_XliffData.Rows(icnt_ID)("ID") = objXliff.ID(icnt_ID).ToString
            Next

            For icnt_Resname As Integer = 0 To objXliff.Resname.Count - 1
                table_XliffData.Rows(icnt_Resname)("SourceName") = objXliff.Resname(icnt_Resname).ToString
            Next

            For icnt_Source As Integer = 0 To objXliff.Source.Count - 1
                table_XliffData.Rows(icnt_Source)("SourceVal") = objXliff.Source(icnt_Source).ToString
            Next

            For icnt_Translation As Integer = 0 To objXliff.Translation.Count - 1
                table_XliffData.Rows(icnt_Translation)("Translation") = objXliff.Translation(icnt_Translation).ToString
            Next

            ''Replace Coverted values
            'FileContent_Raw.Replace("Employee Satisfaction", "")
            Dim FileContent As String = FileContent_Raw
            For icnt_translate As Integer = 0 To table_XliffData.Rows.Count - 1

                Dim strProp_SearchBy As String = ""
                Dim strProp_ReplaceBy As String = ""
                Dim strProp_SearchVal As String = table_XliffData.Rows(icnt_translate)("SourceVal").ToString
                Dim strProp_ReplaceVal As String = table_XliffData.Rows(icnt_translate)("Translation").ToString

                'If strProp_SearchVal.Contains("Employee satisfaction") Then
                '    MsgBox("hi")
                'End If

                '' cheking for HTML tag (\...issue) [need to add sunil's code-- getalltext() method]
                Dim tagRegex As New Regex("<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>")
                If tagRegex.IsMatch(strProp_SearchVal) Then
                    strProp_SearchVal = strProp_SearchVal.Replace("""", "\""")
                    strProp_ReplaceVal = strProp_ReplaceVal.Replace("""", "\""")
                End If
                'If strProp_SearchVal.StartsWith("<p><span") Or strProp_SearchVal.StartsWith("<p style") Or strProp_SearchVal.StartsWith("<p class") Then
                '    strProp_SearchVal = strProp_SearchVal.Replace("""", "\""")
                '    strProp_ReplaceVal = strProp_ReplaceVal.Replace("""", "\""")
                'End If


                If table_XliffData.Rows(icnt_translate)("SourceName").ToString.ToUpper.StartsWith("DUMMY_PROP") Then
                    strProp_SearchBy = """" + strProp_SearchVal + """"
                    'strProp_ReplaceBy = """" + table_XliffData.Rows(icnt_translate)("Translation").ToString + """"
                    strProp_ReplaceBy = """" + strProp_ReplaceVal + """"
                Else
                    strProp_SearchBy = """" + table_XliffData.Rows(icnt_translate)("SourceName").ToString + """" & ":" & """" + strProp_SearchVal + """"
                    'strProp_ReplaceBy = """" + table_XliffData.Rows(icnt_translate)("SourceName").ToString + """" & ":" & """" + table_XliffData.Rows(icnt_translate)("Translation").ToString.Replace("""", "\""") + """"
                    strProp_ReplaceBy = """" + table_XliffData.Rows(icnt_translate)("SourceName").ToString + """" & ":" & """" + strProp_ReplaceVal + """"
                End If

                'FileContent.Replace(Environment.NewLine, "")
                'Dim count As Integer = Regex.Matches(FileContent, strProp_SearchBy).Count
                'If count > 1 Then
                '    For cnt As Integer = 0 To count - 1
                '        If (FileContent.Contains(strProp_SearchBy)) Then
                '            FileContent = FileContent.Replace(strProp_SearchBy, strProp_ReplaceBy)
                '        End If
                '    Next
                'Else
                If (FileContent.Contains(strProp_SearchBy)) Then
                        FileContent = FileContent.Replace(strProp_SearchBy, strProp_ReplaceBy)
                    End If
                'End If
            Next

            Dim str_margecontent As String = MakeFileContent_JsonNew(FileContent)
            Dim u8 As Encoding = Encoding.UTF8
            Dim iu8_BC As Integer = u8.GetByteCount(str_margecontent)

            ''Replace Byte
            Dim charLocation As Integer = FileContent.IndexOf("{", StringComparison.Ordinal)
            Dim BytePart_old As String = FileContent.Substring(0, charLocation)

            Dim BytePart_split As String() = BytePart_old.Split(" ")


            ''...........Remove Brace part
            'Dim firstChar As String = str_margecontent.Substring(0, 1)
            'Dim result As Integer = String.Compare(firstChar, "{")
            'If result = 0 Then str_margecontent = str_margecontent.Remove(0, 1)
            'str_margecontent = str_margecontent.Remove(str_margecontent.Length - 1)
            ''...............................

            ''Dim iBCascii As Integer = asc.GetByteCount(s)
            ''Dim asc As Encoding = Encoding.ASCII

            BytePart_split(8) = iu8_BC
            Dim BytePart_new As String = ""
            For Each element In BytePart_split
                BytePart_new += element + " "
            Next
            BytePart_new = BytePart_new.Remove(BytePart_new.Length - 1)
            FileContent = FileContent.Replace(BytePart_old, BytePart_new)


            ' File.WriteAllText(Convertedfilepath, FileContent)

            Dim utf8WithoutBom As New System.Text.UTF8Encoding(False)

            Using sink As New StreamWriter(Convertedfilepath, False, utf8WithoutBom)
                sink.WriteLine(FileContent)
            End Using



            ''File.WriteAllText("C:\Users\C5260534\Desktop\SFSF\Project1\ByteCount\Byte.txt", str_margecontent)

            Return True

        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        End Try
    End Function

    'Private Function CompareSource(ByRef DT As DataTable, ByVal rowIndex As Integer, ByVal colIndex As Integer, ByVal clrDTSource As String, ByVal objxliff As sxliff) As Boolean
    '    For i As Integer = 0 To objxliff.ID.Count - 1
    '        Dim clrXliffSource As String = GetPlainText(LCase(objxliff.source(i)))
    '        clrDTSource = GetPlainText(clrDTSource)
    '        If Not IsDBNull(DT.Rows(rowIndex).Item(colIndex)) Then
    '            If (clrDTSource = clrXliffSource) Or (Trim(LCase(objxliff.source(i))) = Trim(LCase(DT.Rows(rowIndex).Item(colIndex)))) Then
    '                DT.Rows(rowIndex).Item(colIndex) = objxliff.translation(i)
    '                Return True
    '            End If
    '        End If
    '    Next

    '    Return False

    'End Function

    'Private Sub WriteDataTableToCSV_Competency(ByVal sourceTable As DataTable, ByVal writer As TextWriter, ByVal lang As String)
    '    Try
    '        Dim rowValues As List(Of String)
    '        For j As Integer = 0 To sourceTable.Rows.Count - 1
    '            rowValues = New List(Of String)()
    '            For i As Integer = 0 To sourceTable.Columns.Count - 1
    '                If LangFound_Competency(sourceTable.Rows(j).Item(i).ToString) Then
    '                    rowValues.Add(QuoteValue(lang))
    '                Else
    '                    rowValues.Add(QuoteValue(sourceTable.Rows(j).Item(i).ToString))
    '                End If

    '            Next
    '            writer.WriteLine(String.Join(",", rowValues))
    '        Next

    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    Finally
    '        writer.Flush()
    '    End Try

    'End Sub

    'Private Function LangFound_Competency(ByVal str As String) As Boolean
    '    Select Case LCase(str)
    '        Case "en_us", "fr_fr", "de_de", "es_es", "ja_jp", "ko_kr", "zh_cn", "ru_ru", "pt_br", "it_it"
    '            Return True
    '        Case Else
    '            Return False
    '    End Select
    'End Function

    'Public Function load_xliff(ByVal filename As String) As sXliff 'Doesnt work if we have html tags, crashes when builtin function readinnerxml called.
    '    Dim MyXliff As New sXliff

    '    Try
    '        If System.IO.File.Exists(filename) <> True Then
    '            Throw New Exception(filename & " not found")
    '        End If
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try

    '    Dim row_cnt As Integer = 0

    '    MyXliff.Resname = New ArrayList
    '    MyXliff.Source = New ArrayList
    '    MyXliff.Translation = New ArrayList
    '    MyXliff.ID = New ArrayList
    '    MyXliff.Note = New ArrayList
    '    MyXliff.MaxLength = New ArrayList

    '    Try
    '        Dim xd As New Xml.XmlDocument
    '        xd.XmlResolver = Nothing
    '        Try
    '            xd.Load(filename)
    '        Catch ex As Exception
    '            Throw New Exception(ex.Message)
    '        End Try

    '        '"trans-unit"
    '        Dim xNodeList As XmlNodeList

    '        xNodeList = xd.GetElementsByTagName("file")

    '        Dim MyAttributes As XmlAttributeCollection


    '        For i As Integer = 0 To xNodeList.Count - 1
    '            If xNodeList(i).Attributes.Count > 0 Then
    '                MyAttributes = xNodeList(i).Attributes
    '                Dim att As XmlAttribute
    '                For Each att In MyAttributes
    '                    If InStr(att.Name, "target-language") > 0 Then
    '                        MyXliff.TargetLang = (att.Value)
    '                        Exit For
    '                    End If
    '                Next
    '            End If
    '        Next

    '        xNodeList = xd.GetElementsByTagName("trans-unit")
    '        For i As Integer = 0 To xNodeList.Count - 1
    '            If xNodeList(i).Attributes.Count > 0 Then
    '                MyAttributes = xNodeList(i).Attributes
    '                Dim att As XmlAttribute
    '                For Each att In MyAttributes
    '                    If String.Compare(att.Name, "id", True) = 0 Then
    '                        MyXliff.ID.Add(att.Value)
    '                    ElseIf String.Compare(att.Name, "resname", True) = 0 Then
    '                        MyXliff.Resname.Add(att.Value)
    '                    ElseIf String.Compare(att.Name, "maxwidth", True) = 0 Then
    '                        MyXliff.MaxLength.Add(CInt(att.Value))
    '                    End If
    '                Next
    '            End If
    '        Next

    '        xNodeList = xd.GetElementsByTagName("source")

    '        For i As Integer = 0 To xNodeList.Count - 1
    '            MyXliff.Source.Add(xNodeList(i).InnerText)
    '        Next

    '        xNodeList = xd.GetElementsByTagName("target")

    '        For i As Integer = 0 To xNodeList.Count - 1
    '            MyXliff.Translation.Add(xNodeList(i).InnerText)
    '        Next

    '        xNodeList = xd.GetElementsByTagName("note")

    '        For i As Integer = 0 To xNodeList.Count - 1
    '            MyXliff.Note.Add(xNodeList(i).InnerText)
    '        Next

    '        If MyXliff.Source.Count <> MyXliff.Translation.Count Then
    '            Dim str As String = "XML vaidation error, Source\Target count are not equal!" & vbNewLine
    '            str = str & "Source count - " & MyXliff.Source.Count & vbNewLine
    '            str = str & "Target count - " & MyXliff.Translation.Count & vbNewLine
    '            str = str & "Please check the xliff file - " & filename
    '            Throw New Exception(str)
    '        End If

    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try

    '    Return MyXliff

    'End Function

    'Public Function UnWrapXliffBack(ByVal xliffFile As String) As Boolean
    '    Try
    '        If File.Exists(Application.StartupPath & "\Temp_UnWrap.xliff") Then
    '            File.Delete(Application.StartupPath & "\Temp_UnWrap.xliff")
    '        End If

    '        Dim str As String = ""
    '        Using Reader As StreamReader = New StreamReader(xliffFile, True)
    '            str = Reader.ReadToEnd
    '        End Using

    '        Dim content() As String
    '        content = Split(str, vbCrLf)

    '        For i As Integer = 0 To UBound(content)
    '            If Left(content(i), 8) = "<source>" Then
    '                content(i) = revert_xml(unwrap_html((Mid(content(i), 9, InStr(content(i), "</source>") - 9))))
    '            ElseIf Left(content(i), 14) = "<target state=" Then
    '                content(i) = revert_xml(unwrap_html((Mid(content(i), 42, InStr(content(i), "</target>") - 42))))
    '            End If

    '            If Left(content(i), 8) = "<source>" Then
    '                content(i) = revert_xml(unwrap_html(content(i)))
    '            ElseIf Left(content(i), 14) = "<target state=" Then
    '                content(i) = revert_xml(unwrap_html(content(i)))
    '            End If
    '        Next

    '        Using writer As StreamWriter = New StreamWriter(Application.StartupPath & "\Temp_UnWrap.xliff", False, System.Text.Encoding.UTF8)
    '            For i As Integer = 0 To UBound(content)
    '                writer.WriteLine(content(i))
    '            Next
    '        End Using

    '    Catch ex As Exception
    '        Throw New Exception("Error UnWrapping xliff" & vbNewLine & ex.Message)
    '    End Try
    '    Return True
    'End Function

    'Public Function cvload_xliff(ByVal filename As String) As sXliff
    '    Dim MyXliff As New sXliff
    '    MyXliff.Resname = New ArrayList
    '    MyXliff.Source = New ArrayList
    '    MyXliff.Translation = New ArrayList
    '    MyXliff.ID = New ArrayList
    '    Try
    '        Dim str As String = ""
    '        Using Reader As StreamReader = New StreamReader(filename, True)
    '            str = Reader.ReadToEnd
    '        End Using

    '        Dim content() As String
    '        content = Split(str, vbCrLf)

    '        For i As Integer = 0 To UBound(content)

    '            If Left(content(i), 15) = "<trans-unit id=" Then
    '                MyXliff.ID.Add(Mid(content(i), 17, InStr(17, content(i), Chr(34)) - 17))

    '                Dim istart As Integer = InStr(content(i), "resname") + 9
    '                Dim iend As Integer = InStr(istart, content(i), ">")

    '                MyXliff.Resname.Add(Mid(content(i), istart, iend - istart - 1))
    '            End If

    '            If Left(content(i), 8) = "<source>" Then
    '                ''''MyXliff.source.Add(revert_xml(unwrap_html((Mid(content(i), 9, InStr(content(i), "</source>") - 9)))))
    '            ElseIf Left(content(i), 39) = "<target state=" & Chr(34) & "needs-review-translation" Then
    '                MyXliff.Translation.Add(Mid(content(i), 42, InStr(content(i), "</target>") - 42))
    '            ElseIf Left(content(i), 25) = "<target state=" & Chr(34) & "translated" Then
    '                MyXliff.Translation.Add(Mid(content(i), 28, InStr(content(i), "</target>") - 28))
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Throw New Exception("Error loading cvXliff - " & filename & vbNewLine & ex.Message)
    '    End Try
    '    Return MyXliff
    'End Function

    'Public Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As Form_MainNew.RtbColor, ByRef _bw As BackgroundWorker)
    '    Dim str As New ArrayList
    '    str.Add(Msg)
    '    str.Add(MyColor)
    '    _bw.ReportProgress(4, str)
    'End Sub

    'Public Structure sXliff
    '    Public Resname As ArrayList
    '    Public TargetLang As String
    '    Public ID As ArrayList
    '    Public Source As ArrayList
    '    Public Translation As ArrayList
    '    Public Note As ArrayList
    '    Public MaxLength As ArrayList
    'End Structure

#End Region

End Module


