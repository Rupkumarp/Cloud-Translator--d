Imports System.Text
Imports System.IO

Module Mod_CsvToXliff

    Function mdfcsv_to_xliff(ByVal mdf_path As String, ByVal xliff_SavePath As String, ByVal languages As String, Optional ByVal bRestrictToMaxLength As Boolean = False, Optional ByVal ExtractWithTranslationonly As Boolean = False) As String

        'Get Position based on file Name from MDF_List.txt-----------------------------------------------------------------------------------------------------------------------
        Dim Position As String = ""
        Dim str() As String = Split(IO.File.ReadAllText(appData & DefinitionFiles.Mdf_List), vbCrLf)
        For i As Integer = 0 To UBound(str)
            If str(i) <> String.Empty Then
                Dim Dom() As String = Split(str(i), vbTab)
                Position = Dom(1)

                Dim FileNumberDefintion() As String = Split(Dom(2), ".")
                Dim FileNumber() As String = Split(System.IO.Path.GetFileNameWithoutExtension(mdf_path), ".")
                Dim bFound As Boolean

                If UBound(FileNumber) > UBound(FileNumberDefintion) Then
                    bFound = MapFileNumberWithFileTypeDefintion(FileNumberDefintion, FileNumber)
                Else
                    bFound = MapFileNumberWithFileTypeDefintion(FileNumber, FileNumberDefintion)
                End If

                If bFound Then
                    Exit For
                End If
            End If
        Next

        '------------------------------------------------------------------------------------------------------------------------------------------------------
        Dim msg As String
        Try
            Try
                Dim objParser As New CsvParser
                Dim dt As DataTable

                dt = objParser.GetDataTabletFromCSVFile(mdf_path)

                Dim enUS_List As New Dictionary(Of String, Integer)
                Dim Col_defaultvlaue As New Dictionary(Of String, Integer)
                Dim PositionColumn As Integer = 0

                'Get List of enUS, so we can loop it for exact match type -------------------------------------------------------------------------------------
                Dim i As Integer = 0
                For Each column As DataColumn In dt.Columns
                    If InStr(column.ColumnName, ".") > 0 Then
                        If (InStr(column.ColumnName.ToLower, "en_us") > 0 And InStr(column.ColumnName, "_") > 0) Then 'revert this after

                            enUS_List.Add(column.ColumnName, i)
                        ElseIf InStr(column.ColumnName.ToLower, "defaultvalue") > 0 Then
                            Col_defaultvlaue.Add(column.ColumnName, i)
                        End If
                    End If
                    If LCase(column.ColumnName) = "externalcode" Or LCase(column.ColumnName) = "code" Or LCase(column.ColumnName) = "id" Then
                        PositionColumn = i
                    End If
                    i += 1
                Next

                'For Each column As DataColumn In dt.Columns
                '    ' If (InStr(column.ColumnName.ToLower, "en_us") > 0 And InStr(column.ColumnName, "_") > 0) Then 'revert this after
                '    If column.ColumnName.ToLower = "positiontitle" Then
                '        enUS_List.Add(column.ColumnName, i)
                '    ElseIf InStr(column.ColumnName.ToLower, "defaultvalue") > 0 Then
                '        Col_defaultvlaue.Add(column.ColumnName, i)
                '    End If

                '    If LCase(column.ColumnName) = "externalcode" Or LCase(column.ColumnName) = "code" Or LCase(column.ColumnName) = "id" Then
                '        PositionColumn = i
                '    End If
                '    i += 1
                'Next
                '------------------------------------------------------------------------------------------------------------------------------------------------------
                i = 0

                If enUS_List.Count = 0 Then
                    Throw New Exception("Could not create " & languages & " xliff file as en_US column could not be found")
                End If

                dt = Preprocess(dt, enUS_List, Col_defaultvlaue)

                'Creates XLiff per Language----------------------------------------------------------------------------------------------------------------------------------
                Dim filename As String = System.IO.Path.GetFileNameWithoutExtension(mdf_path)

                Dim Colnames(dt.Columns.Count) As String
                Dim x As Integer = 0
                For Each column As DataColumn In dt.Columns
                    Colnames(x) = column.ColumnName
                    x += 1
                Next

                msg = CreateXliff(filename, PositionColumn, enUS_List, dt, languages, xliff_SavePath & filename & "_" & languages & ".xliff", Replace(languages, "_", "-"), Position, bRestrictToMaxLength, ExtractWithTranslationonly)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return msg

    End Function

    Private Function CreateXliff(ByVal filename As String, ByVal PositionID_FirstColumn As Integer, ByVal enUS_Column As Dictionary(Of String, Integer), ByRef DT As DataTable, ByVal ColumnLang As String, ByVal xliff_Path As String, ByVal Targetlanguage As String, ByVal Position As String, ByVal bRestrictToMaxLength As Boolean, ByVal ExtractWithTranslationonly As Boolean) As String

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

                Dim MaxLengthTable As DataTable = ClsMaxLength.LoadMaxLength(filename)

                If ExtractWithTranslationonly Then
                    ExtractWithTranslation(Writer, DT, MaxLengthTable, enUS_Column, PositionID_FirstColumn, ColumnLang, bRestrictToMaxLength, Position, myNum)
                Else
                    ExtractWithoutTranslation(Writer, DT, MaxLengthTable, enUS_Column, PositionID_FirstColumn, ColumnLang, bRestrictToMaxLength, Position, myNum)
                End If

                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

            End Using

            If Not ExtractWithTranslationonly Then
                If myNum = 0 Then
                    System.IO.File.Delete(xliff_Path)
                    Return " Already has translation for " & Replace(Targetlanguage, "-", "_")
                End If
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return ""

    End Function

    Private Sub ExtractWithTranslation(ByRef Writer As StreamWriter, ByRef DT As DataTable, ByRef MaxLengthTable As DataTable,
                                       ByRef enUS_Column As Dictionary(Of String, Integer), ByRef PositionID_FirstColumn As Integer,
                                       ByRef ColumnLang As String, ByRef bRestrictToMaxLength As Boolean, ByRef Position As String, ByRef myNum As Integer)

        Dim TargetCol As Integer
        Dim TR As String = ""

        For J As Integer = 0 To enUS_Column.Count - 1
            Dim col As Integer = enUS_Column.Values(J)
            Dim HeaderName As String = ""

            HeaderName = Mid(enUS_Column.Keys(J), 1, InStrRev(enUS_Column.Keys(J), ".") - 1)

            TargetCol = GetColNumber(DT, HeaderName & "." & ColumnLang) 'Necessary to get Column other than en_US to check if it is already translated.
            ' TargetCol = GetColNumber(DT, "externalName" & "." & ColumnLang) 'Comment this later
            For i As Integer = 1 To DT.Rows.Count - 1
                If DT.Rows(i).Item(col).ToString <> String.Empty And DT.Rows(i).Item(TargetCol).ToString <> String.Empty Then
                    myNum += 1
                    Dim MaxLength As Integer = -1
                    If bRestrictToMaxLength And MaxLengthTable.Rows.Count > 0 Then 'Support of length restriction
                        Try
                            MaxLength = ClsMaxLength.GetMaxLength(MaxLengthTable, DT, col, i)
                            If MaxLength >= 0 Then
                                Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(DT.Rows(i).Item(PositionID_FirstColumn).ToString) & "_" & HeaderName & Chr(34) & " resname=" & Chr(34) & clean_xml(DT.Rows(i).Item(PositionID_FirstColumn).ToString) & Chr(34) & " size-unit=" & Chr(34) & "char" & Chr(34) & " maxwidth=" & Chr(34) & MaxLength & Chr(34) & ">")
                            End If
                        Catch ex As Exception
                            MsgBox(ex.Message)
                        End Try
                    End If
                    If MaxLength = -1 Then
                        Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(DT.Rows(i).Item(PositionID_FirstColumn).ToString) & "_" & HeaderName & Chr(34) & " resname=" & Chr(34) & clean_xml(DT.Rows(i).Item(PositionID_FirstColumn).ToString) & Chr(34) & ">")
                    End If
                    Writer.WriteLine("<source>" & wrap_html(clean_xml(DT.Rows(i).Item(col).ToString)) & "</source>")
                    If IsNumeric(DT.Rows(i).Item(col)) Then
                        Writer.WriteLine("<target state=""needs-review-translation"">" & DT.Rows(i).Item(col) & "</target>")
                    Else
                        Writer.WriteLine("<target state=""needs-review-translation"">" & wrap_html(clean_xml(DT.Rows(i).Item(TargetCol).ToString)) & "</target>")
                    End If
                    Writer.WriteLine("<note from=""Developer"" priority =""10"">" & clean_xml(Position) & ": " & clean_xml(HeaderName) & "</note>")
                    Writer.WriteLine("</trans-unit>")
                    Writer.WriteLine(vbCrLf)
                End If
            Next
        Next
    End Sub

    Private Sub ExtractWithoutTranslation(ByRef Writer As StreamWriter, ByRef DT As DataTable, ByRef MaxLengthTable As DataTable,
                                          ByRef enUS_Column As Dictionary(Of String, Integer), ByRef PositionID_FirstColumn As Integer,
                                          ByRef ColumnLang As String, ByRef bRestrictToMaxLength As Boolean, ByRef Position As String, ByRef myNum As Integer)

        Dim TargetCol As Integer
        Dim TR As String = ""


        For J As Integer = 0 To enUS_Column.Count - 1
            Dim col As Integer = enUS_Column.Values(J)
            Dim HeaderName As String = ""

            'HeaderName = Mid(enUS_Column.Keys(J), 1, InStrRev(enUS_Column.Keys(J), ".") - 1)

            HeaderName = enUS_Column.Keys(J)

            TargetCol = GetColNumber(DT, HeaderName & "." & ColumnLang) 'Necessary to get Column other than en_US to check if it is already translated.
            TargetCol = GetColNumber(DT, "externalName" & "." & ColumnLang) 'Comment this later
            For i As Integer = 1 To DT.Rows.Count - 1
                If DT.Rows(i).Item(col).ToString <> "" And DT.Rows(i).Item(TargetCol).ToString = "" Then
                    myNum += 1
                    Dim MaxLength As Integer = -1
                    If bRestrictToMaxLength And MaxLengthTable.Rows.Count > 0 Then 'Support of length restriction
                        Try
                            MaxLength = ClsMaxLength.GetMaxLength(MaxLengthTable, DT, col, i)
                            If MaxLength >= 0 Then
                                Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(DT.Rows(i).Item(PositionID_FirstColumn).ToString) & "_" & HeaderName & Chr(34) & " resname=" & Chr(34) & clean_xml(DT.Rows(i).Item(PositionID_FirstColumn).ToString) & Chr(34) & " size-unit=" & Chr(34) & "char" & Chr(34) & " maxwidth=" & Chr(34) & MaxLength & Chr(34) & ">")
                            End If
                        Catch ex As Exception
                            MsgBox(ex.Message)
                        End Try
                    End If
                    If MaxLength = -1 Then
                        Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & "_" & clean_xml(DT.Rows(i).Item(PositionID_FirstColumn).ToString) & "_" & HeaderName & Chr(34) & " resname=" & Chr(34) & clean_xml(DT.Rows(i).Item(PositionID_FirstColumn).ToString) & Chr(34) & ">")
                    End If
                    Writer.WriteLine("<source>" & wrap_html(clean_xml(DT.Rows(i).Item(col).ToString)) & "</source>")
                    If IsNumeric(DT.Rows(i).Item(col)) Then
                        Writer.WriteLine("<target state=""needs-review-translation"">" & DT.Rows(i).Item(col) & "</target>")
                    Else
                        Writer.WriteLine("<target state=""needs-review-translation""></target>")
                    End If
                    Writer.WriteLine("<note from=""Developer"" priority =""10"">" & clean_xml(Position) & ": " & clean_xml(HeaderName) & "</note>")
                    Writer.WriteLine("</trans-unit>")
                    Writer.WriteLine(vbCrLf)
                End If
            Next
        Next
    End Sub

    Public Function GetColNumber(ByVal DT As DataTable, ByVal colName As String) As Integer

        Dim i As Integer = 0

        For j As Integer = 0 To DT.Columns.Count - 2
            If colName.Length <= 5 Then
                If Microsoft.VisualBasic.Right(LCase(DT.Columns(j).ColumnName), 5) = LCase(colName) Then
                    i = j
                    Exit For
                End If
            Else
                If (LCase(DT.Columns(j).ColumnName)) = LCase(colName) Then
                    i = j
                    Exit For
                End If
            End If
        Next

        Return i
    End Function

    'Preprocess csvdata file, if enUS col is emtpy, fill that with defualtvalue content
    Public Function Preprocess(ByRef Dt As DataTable, ByRef enUS As Dictionary(Of String, Integer), ByRef Col_defaultvlaue As Dictionary(Of String, Integer)) As DataTable
        Try
            For i As Integer = 0 To enUS.Count - 1
                For j As Integer = 1 To Dt.Rows.Count - 1
                    If Dt.Rows(j).Item(enUS.Item(enUS.Keys(i))).ToString = "" Then
                        If Dt.Rows(j).Item(Col_defaultvlaue.Item(Col_defaultvlaue.Keys(i))).ToString <> "" Then
                            Dt.Rows(j).Item(enUS.Item(enUS.Keys(i))) = Dt.Rows(j).Item(Col_defaultvlaue.Item(Col_defaultvlaue.Keys(i)))
                        End If
                    End If
                Next
            Next
        Catch ex As Exception
            Throw New Exception("Error @Preprocess" & vbNewLine & "Preprocess csvdata file, if enUS col is emtpy, fill that with defualtvalue content" & vbNewLine & ex.Message)
        End Try
        Return Dt
    End Function


    'NAR function to remove reference.
    Public Sub NAR(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

End Module
