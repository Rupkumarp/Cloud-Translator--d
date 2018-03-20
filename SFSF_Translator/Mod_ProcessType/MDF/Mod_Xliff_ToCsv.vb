Imports System.Xml
Imports System.IO
Imports System.Xml.Schema
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.ComponentModel

Module Mod_Xliff_ToCsv

    'Function xliff_to_mdfsv(ByVal originalfile_path As String, ByVal translated_xliff_path As String, ByVal Lang As String) As Boolean
    '    Try
    '        Dim objParser As New CsvParser
    '        Dim Dt As DataTable
    '        Dt = objParser.GetDataTabletFromCSVFile(originalfile_path)

    '        Dim objXliff As New sXliff

    '        Try
    '            objXliff = ModHelper.load_xliff(translated_xliff_path)
    '        Catch ex As Exception
    '            If ModHelper.UnWrapXliffBack(translated_xliff_path) <> True Then
    '                Throw New Exception("Error UnWrapping xliff back!")
    '            End If
    '            objXliff = ModHelper.cvload_xliff(Application.StartupPath & "\Temp_UnWrap.xliff")
    '        End Try

    '        If objXliff.ID.Count = 0 Then
    '            ShowMsgInMainForm(vbCrLf & "0 translations found in " & System.IO.Path.GetFileName(translated_xliff_path) & vbCrLf)
    '            Throw New Exception("0 translations found in " & System.IO.Path.GetFileName(translated_xliff_path))
    '        End If

    '        Dim TargetColHeader As String
    '        Dim TargetColumnNumber As Integer
    '        Dim ExternalCodeColumn As Integer
    '        Dim enUSHeader As String
    '        Dim enUSColumnNumber As Integer

    '        Dim notFound As New ArrayList
    '        Dim bFound As Boolean

    '        For i As Integer = 0 To objXliff.ID.Count - 1
    '            TargetColHeader = Mid(objXliff.ID(i), InStrRev(objXliff.ID(i), "_") + 1, Len(objXliff.ID(i))) & "." & Lang
    '            TargetColumnNumber = Mod_CsvToXliff.GetColNumber(Dt, TargetColHeader)
    '            ExternalCodeColumn = Mod_CsvToXliff.GetColNumber(Dt, "externalCode")
    '            enUSHeader = Mid(objXliff.ID(i), InStrRev(objXliff.ID(i), "_") + 1, Len(objXliff.ID(i))) & "." & "en_US"
    '            enUSColumnNumber = Mod_CsvToXliff.GetColNumber(Dt, enUSHeader)
    '            bFound = False
    '            'Insert Translated content
    '            For j As Integer = 0 To Dt.Rows.Count - 1
    '                If IsDBNull(Dt.Rows(j).Item(TargetColumnNumber)) And IsDBNull(Dt.Rows(j).Item(enUSColumnNumber)) <> True Then
    '                    Dim clrDTSource As String = HTMLToText(LCase(Dt.Rows(j).Item(enUSColumnNumber)))
    '                    Dim clrXliffSource As String = HTMLToText(LCase(objXliff.Source(i)))

    '                    If clrDTSource <> "" Then
    '                        clrDTSource = System.Text.RegularExpressions.Regex.Replace(clrDTSource, "\\s+", "")
    '                        clrDTSource = clrDTSource.Replace(" ", String.Empty)
    '                        clrDTSource = Replace(Replace(clrDTSource, " ", ""), vbTab, "")
    '                        clrDTSource = Regex.Replace(clrDTSource, "(\r\n)?(^\s*$)+", "", RegexOptions.Multiline) 'Multiline will be removed
    '                        clrDTSource = Regex.Replace(clrDTSource, "[^\w\@-]", "") 'Unknown white space will be removed here
    '                    End If


    '                    If clrXliffSource <> "" Then
    '                        clrXliffSource = System.Text.RegularExpressions.Regex.Replace(clrXliffSource, "\\s+", "")
    '                        clrXliffSource = Replace(clrXliffSource, " ", String.Empty)
    '                        clrXliffSource = Regex.Replace(clrXliffSource, "(\r\n)?(^\s*$)+", "", RegexOptions.Multiline)
    '                        clrXliffSource = Regex.Replace(clrXliffSource, "[^\w\@-]", "")
    '                    End If

    '                    If (Dt.Rows(j).Item(ExternalCodeColumn) = objXliff.Resname(i)) Then
    '                        bFound = True
    '                        Dt.Rows(j).Item(TargetColumnNumber) = objXliff.Translation(i)
    '                        Exit For
    '                    ElseIf ((Dt.Rows(j).Item(1).ToString.ToLower.Trim = objXliff.Source(i).ToString.ToLower.Trim) Or clrDTSource = clrXliffSource) Then
    '                        Dt.Rows(j).Item(TargetColumnNumber) = objXliff.Translation(i)
    '                        bFound = True
    '                        Exit For
    '                    End If
    '                End If
    '            Next
    '            'Add the items which missed to update translation
    '            If bFound <> True Then
    '                notFound.Add(i + 1 & ". Source value -> " & objXliff.Source(i))
    '            End If
    '        Next


    '        'Build back csv
    '        'define target path

    '        Dim targetfilepath As String
    '        If InStr(originalfile_path, "01-Input-B") <> 0 Then
    '            'case 1 first language
    '            targetfilepath = Replace(originalfile_path, "01-Input-B", "05-Output")

    '        ElseIf InStr(originalfile_path, "05-Output") <> 0 Then
    '            'case 2 subsequent languages
    '            targetfilepath = originalfile_path

    '        Else
    '            'case 3, project structure not used. Manual operations. Just add _out.
    '            targetfilepath = Path.GetDirectoryName(originalfile_path) & "\" & Path.GetFileNameWithoutExtension(originalfile_path) & ".out" & Path.GetExtension(originalfile_path)

    '        End If

    '        Using writer As StreamWriter = New StreamWriter(targetfilepath, False, System.Text.Encoding.UTF8)
    '            WriteDataTable(Dt, writer)
    '        End Using

    '        'NO Translation found then show a msg box and log it as well.
    '        If notFound.Count > 0 Then
    '            Dim str As String = ""
    '            For i As Integer = 0 To notFound.Count - 1
    '                If str = "" Then
    '                    str = notFound(i) & vbCrLf
    '                Else
    '                    str = str & vbCrLf & notFound(i)
    '                End If
    '            Next
    '            ShowMsgInMainForm("**** Translation missing for - " & System.IO.Path.GetFileName(originalfile_path) & "****" & vbCrLf)
    '            ShowMsgInMainForm(str & vbCrLf)
    '            MsgBox("Translation not found for " & notFound.Count & " items" & vbNewLine & "Please check the log!", MsgBoxStyle.Critical, "Missing Translations")
    '        End If

    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try

    '    Return True
    'End Function

    Private _bw As BackgroundWorker

    Function xliff_to_mdfsv(ByVal originalfile_path As String, ByVal translated_xliff_path As String, ByVal Lang As String, ByVal bw As BackgroundWorker) As Boolean

        _bw = bw
        Try
            Dim objParser As New CsvParser
            Dim Dt As DataTable
            Dt = objParser.GetDataTabletFromCSVFile(originalfile_path)

            Dim objXliff As New sXliff

            Try
                objXliff = ModHelper.load_xliff(translated_xliff_path)
            Catch ex As Exception
                If ModHelper.UnWrapXliffBack(translated_xliff_path) <> True Then
                    Throw New Exception("Error UnWrapping xliff back!")
                End If
                objXliff = ModHelper.cvload_xliff(Application.StartupPath & "\Temp_UnWrap.xliff")
            End Try

            If objXliff.ID.Count = 0 Then
                UpdateMsg(Now & Chr(9) & "No translations found for " & System.IO.Path.GetFileName(translated_xliff_path) & vbCrLf, Form_MainNew.RtbColor.Red, _bw)
            End If

            Dim enUS_Column As New Dictionary(Of String, Integer)
            Dim Col_defaultvlaue As New Dictionary(Of String, Integer)
            Dim PositionColumn As Integer = 0

            'Get List of enUS, so we can loop it for exact match type -------------------------------------------------------------------------------------
            Dim i As Integer = 0
            'For Each column As DataColumn In Dt.Columns
            '    If InStr(column.ColumnName, ".") > 0 And InStr(column.ColumnName, "_") > 0 And InStr(column.ColumnName, "en_US") > 0 Then
            '        enUS_Column.Add(column.ColumnName, i)
            '    End If
            '    If LCase(column.ColumnName) = "externalcode" Then
            '        PositionColumn = i
            '    End If
            '    i += 1
            'Next
            For Each column As DataColumn In Dt.Columns
                If InStr(column.ColumnName, ".") > 0 Then
                    If InStr(column.ColumnName.ToLower, "en_us") > 0 And InStr(column.ColumnName, "_") > 0 Then
                        enUS_Column.Add(column.ColumnName, i)
                    ElseIf InStr(column.ColumnName.ToLower, "defaultvalue") > 0 Then
                        Col_defaultvlaue.Add(column.ColumnName, i)
                    End If
                End If
                If LCase(column.ColumnName) = "externalcode" Or LCase(column.ColumnName) = "code" Or LCase(column.ColumnName) = "id" Then
                    PositionColumn = i
                End If
                i += 1
            Next
            '------------------------------------------------------------------------------------------------------------------------------------------------------
            'For Each column As DataColumn In Dt.Columns
            '    ' If (InStr(column.ColumnName.ToLower, "en_us") > 0 And InStr(column.ColumnName, "_") > 0) Then 'revert this after
            '    If column.ColumnName.ToLower = "positiontitle" Then
            '        enUS_Column.Add(column.ColumnName, i)
            '    ElseIf InStr(column.ColumnName.ToLower, "defaultvalue") > 0 Then
            '        Col_defaultvlaue.Add(column.ColumnName, i)
            '    End If

            '    If LCase(column.ColumnName) = "externalcode" Or LCase(column.ColumnName) = "code" Or LCase(column.ColumnName) = "id" Then
            '        PositionColumn = i
            '    End If
            '    i += 1
            'Next
            i = 0

            If enUS_Column.Count = 0 Then
                Throw New Exception(Path.GetFileNameWithoutExtension(originalfile_path) & " - could not translate file as en_US column could not be found")
            End If

            Dt = Preprocess(Dt, enUS_Column, Col_defaultvlaue)

            'Creates XLiff per Language----------------------------------------------------------------------------------------------------------------------------------
            Dim filename As String = System.IO.Path.GetFileNameWithoutExtension(originalfile_path)

            Dim Colnames(Dt.Columns.Count) As String
            Dim x As Integer = 0
            For Each column As DataColumn In Dt.Columns
                Colnames(x) = column.ColumnName
                x += 1
            Next

            Dim TargetColumnNumber As Integer
            Dim notFound As New ArrayList
            Dim bFound As Boolean
            Dim counter As Integer = 1

            For J As Integer = 0 To enUS_Column.Count - 1
                Dim enUSColumnNumber As Integer = enUS_Column.Values(J)
                Dim HeaderName As String = Mid(enUS_Column.Keys(J), 1, InStrRev(enUS_Column.Keys(J), ".") - 1)
                TargetColumnNumber = GetColNumber(Dt, HeaderName & "." & Lang) 'Necessary to get Column other than en_US to check if it is already translated.
                ''TargetColumnNumber = GetColNumber(Dt, "externalName" & "." & Lang) 'Necessary to get Column other than en_US to check if it is already translated.
                For k As Integer = 1 To Dt.Rows.Count - 1
                    bFound = False
                    If IsDBNull(Dt.Rows(k).Item(TargetColumnNumber)) And IsDBNull(Dt.Rows(k).Item(enUSColumnNumber)) <> True Then
                        Dim clrDTSource As String = GetPlainText(LCase(Dt.Rows(k).Item(enUSColumnNumber)))

                        For z As Integer = 0 To objXliff.ID.Count - 1
                            Dim clrXliffSource As String = GetPlainText(LCase(objXliff.Source(z)))
                            If ((Dt.Rows(k).Item(enUSColumnNumber).ToString.ToLower.Trim = objXliff.Source(z).ToString.ToLower.Trim) Or (clrDTSource = clrXliffSource)) Then
                                Dt.Rows(k).Item(TargetColumnNumber) = objXliff.Translation(z)
                                bFound = True
                                Exit For
                            End If
                        Next
                        'Add the items which missed to update translation
                        If bFound <> True Then
                            counter += 1
                            notFound.Add(counter + 1 & ". Source value -> " & Dt.Rows(k).Item(enUSColumnNumber))
                        End If
                    End If
                Next
            Next

            'Build back csv
            'define target path

            Dim targetfilepath As String
            If InStr(originalfile_path, "01-Input-B") <> 0 Then
                'case 1 first language
                targetfilepath = Replace(originalfile_path, "01-Input-B", "05-Output")

            ElseIf InStr(originalfile_path, "05-Output") <> 0 Then
                'case 2 subsequent languages
                targetfilepath = originalfile_path

            Else
                'case 3, project structure not used. Manual operations. Just add _out.
                targetfilepath = Path.GetDirectoryName(originalfile_path) & "\" & Path.GetFileNameWithoutExtension(originalfile_path) & ".out" & Path.GetExtension(originalfile_path)

            End If

            Using writer As StreamWriter = New StreamWriter(targetfilepath, False, System.Text.Encoding.UTF8)
                WriteDataTable(Dt, writer)
            End Using

            'NO Translation found then show a msg box and log it as well.
            If notFound.Count > 0 Then
                Dim objMissingTransaltion As New MissedTranslations
                objMissingTransaltion.UpdateMsg(notFound, originalfile_path, Lang)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function

    Public Sub WriteDataTable(ByVal sourceTable As DataTable, ByVal writer As TextWriter)

        Dim headerValues As List(Of String) = New List(Of String)()

        'First write header
        For i As Integer = 0 To sourceTable.Columns.Count - 2
            headerValues.Add((sourceTable.Columns(i).ColumnName))
        Next
        writer.WriteLine(String.Join(",", headerValues.Where(Function(s) Not String.IsNullOrEmpty(s))))

        Dim rowValues As List(Of String)
        For j As Integer = 0 To sourceTable.Rows.Count - 1
            rowValues = New List(Of String)()
            For i As Integer = 0 To sourceTable.Columns.Count - 2
                rowValues.Add(QuoteValue(sourceTable.Rows(j).Item(i).ToString))
            Next
            'writer.WriteLine(String.Join(",", rowValues))
            writer.WriteLine(String.Join(",", rowValues.Where(Function(s) Not String.IsNullOrEmpty(s))))
        Next

        writer.Flush()

    End Sub

    Private Function QuoteValue(ByVal value As String) As String
        Return String.Concat("""", value.Replace("""", """"""), """")
    End Function

End Module
