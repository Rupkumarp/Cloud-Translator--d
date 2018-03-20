﻿Imports System.ComponentModel
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.IO
Imports CloudTranslator

''' <summary>
''' Xliff files will be created for excel input files.
''' Each worksheet is looped and contents are extracted.
''' The background of the cell should be colored.
''' </summary>
''' <remarks></remarks>
Public Class DigitalBoard

    Implements iXliff

    Public Property ActiveProject As ProjectDetail Implements iXliff.ActiveProject

    Public Sub CleanTransaltion() Implements iXliff.CleanTransaltion
        ' Should not implement, cannot clean in excel files
    End Sub

    Public Property cnt_newintegrated As Integer Implements iXliff.cnt_newintegrated

    Public Property cnt_newtrans As Integer Implements iXliff.cnt_newtrans

    Public Property CPS As CloudProjectsettings Implements iXliff.CPS

    Public Property curlang As String() Implements iXliff.curlang

    Public Property IP As InitiateProcess Implements iXliff.IP

    Public Property tr_type As TranslationType Implements iXliff.tr_type

    Private _sfile As String

    Private _bw As BackgroundWorker

    Private Sub InitialCurrentProjectSetting(ByRef _CPS As CloudProjectsettings)
        CPS = _CPS
    End Sub



    Public Sub StartProcessing(sFile As String, ByRef BW As System.ComponentModel.BackgroundWorker) Implements iXliff.StartProcessing
        _sfile = sFile
        tr_type = TranslationType.Monolingual
        _bw = BW

        UpdateMsg(Now & Chr(9) & "File Type - Digital Board" & vbCrLf, Form_MainNew.RtbColor.Black)

        Try
            'xml ( xml C/P)
            IP = New InitiateProcess(ActiveProject, BW, sFile, curlang)
            AddHandler IP.Extractxliff, AddressOf ExtractXliff
            AddHandler IP.CleanTransaltion, AddressOf CleanTransaltion
            AddHandler IP.CreateOutFile, AddressOf CreateOutFile
            AddHandler IP.InitializeCurrentProjectSetting, AddressOf InitialCurrentProjectSetting
            AddHandler IP.ImportExistingTranslationToDB, AddressOf ImportExistingTranslationToDB
            AddHandler IP.ExtractXliffWithExistingTranslation, AddressOf ExtractXiffwithExistingTranslation
            IP.StartProcessing()

        Catch ex As Exception
            BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, System.IO.Path.GetFileName(sFile), ex.Message})
            Throw New Exception(ex.Message)
        End Try
    End Sub


    Property _XliffFile As String
    Protected _XliffFolder As String
    Protected xlApp As Excel.Application
    Protected xlWkb As Excel.Workbook
    Protected xlShtIN As Excel.Worksheet
    Protected _Lang As ArrayList

    Protected _XliffFileName As String

    Protected XliffContent As ArrayList
    Protected xliffWkb As ArrayList
    Protected xliffSht As ArrayList


    Private WithEvents CU As New Cls_CloudJob
    Private Sub CU_Progress(Max As Integer, val As Integer) Handles CU.Progress
        Dim str As New ArrayList
        str.Add(Max)
        str.Add(val)
        _bw.ReportProgress(3, str)
        _bw.ReportProgress(0, val & "\" & Max)
    End Sub

    Public Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As Form_MainNew.RtbColor)
        _bw.ReportProgress(4, {Msg, MyColor})
    End Sub


    Public Function ExtractXliff() As Boolean Implements iXliff.ExtractXliff
        Try

            XliffContent = New ArrayList
            xliffWkb = New ArrayList
            xliffSht = New ArrayList

            _XliffFolder = ActiveProject.ProjectPath & CloudProjectsettings.Folder_TobeTransalted

            If Not System.IO.Directory.Exists(_XliffFolder) Then
                System.IO.Directory.CreateDirectory(_XliffFolder)
            End If

            Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            'xlApp = New Excel.Application(DirectCast(CreateObject("Excel.Application"), Excel.Application))

            xlApp = New Excel.Application

            xlApp.Visible = False

            'Get excel files in inputfolder

            If (System.IO.Path.GetExtension(_sfile).ToLower = ".xlsx" Or System.IO.Path.GetExtension(_sfile).ToLower = ".xls") And Not _sfile.ToString.Contains("$") Then
                CreateDigitalBoardXliff(_sfile)
                cnt_newtrans += 1
            End If

            WriteXliff()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            releaseObject(xlShtIN)
            releaseObject(xlWkb)
            xlApp.Quit()
            releaseObject(xlApp)
        End Try
        Return True
    End Function

    Private Sub CreateDigitalBoardXliff(ByVal xlFile As String)
        Try
            xlWkb = xlApp.Workbooks.Open(xlFile)

            Dim WorkSheet As Excel.Worksheet = Nothing

            For Each WorkSheet In xlWkb.Worksheets
                xlShtIN = WorkSheet
                CreateDigitalBoardXliff()
            Next

            xlWkb.Close()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub CreateDigitalBoardXliff()
        Try
            If xlShtIN Is Nothing Then
                Exit Sub
            End If

            Dim LastRow As Long = xlShtIN.UsedRange.Rows.Count - 1
            Dim LastColumn As Integer = xlShtIN.UsedRange.Columns(xlShtIN.UsedRange.Columns.Count).Column

            For j As Integer = 1 To LastColumn
                LastRow = xlShtIN.Cells(xlShtIN.Rows.Count, j).End(-4162).Row
                For i As Integer = 1 To LastRow
                    CU_Progress(LastRow, i)
                    If xlShtIN.Cells(i, j).interior.colorindex = ActiveProject.DigitalBoardColorIndex And Not IsNumeric(xlShtIN.Cells(i, j).text) Then '3=red
                        If Not XliffContent.Contains(xlShtIN.Cells(i, j).text) Then
                            XliffContent.Add(xlShtIN.Cells(i, j).text)
                            xliffWkb.Add(xlWkb.Name)
                            xliffSht.Add(xlShtIN.Name)
                        End If
                    End If
                Next
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            xlShtIN = Nothing
        End Try
    End Sub

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    Private Sub WriteXliff()

        Try
            For i As Integer = 0 To curlang.Count - 1
                ' _XliffFileName = _XliffFolder & "\" & System.IO.Path.GetFileName(ActiveProject.ProjectPath & CloudProjectsettings.Folder_TobeTransalted) & "_" & curlang(i) & ".xliff"

                _XliffFileName = ActiveProject.ProjectPath & CloudProjectsettings.Folder_TobeTransalted & Path.GetFileNameWithoutExtension(_sfile) & "_" & curlang(i) & ".xliff"

                Dim myNum As Integer = 0
                Using Writer As StreamWriter = New StreamWriter(_XliffFileName, False, System.Text.Encoding.UTF8)
                    Writer.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                    Writer.WriteLine("<xliff version=""1.2"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ")
                    Writer.WriteLine("xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2- &#xD;&#xA;strict.xsd"">")
                    Writer.WriteLine("<file original=""C:\Temp\en_fr_2.xliff"" source-language=" & Chr(34) & "en-US" & Chr(34) & " target-language=" & Chr(34) & Replace(GetLong_lang(curlang(i)), "_", "-") & Chr(34) & " datatype=""plaintext"" date=" & Chr(34) & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & Chr(34) & " tool-id=""SAP SFSF CONVERTER"" category=""MLT"">")
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

                    'Taking "UPDATE Product" as base for loop
                    For x As Integer = 0 To XliffContent.Count - 1
                        myNum += 1
                        Writer.WriteLine("<trans-unit id=" & Chr(34) & myNum.ToString().PadLeft(5, "0") & Chr(34) & " resname=" & Chr(34) & xliffWkb(x) & "_" & xliffSht(x) & Chr(34) & ">")
                        Writer.WriteLine("<source>" & wrap_html(clean_xml(XliffContent(x))) & "</source>")
                        Writer.WriteLine("<target state=""needs-review-translation""></target>")
                        Writer.WriteLine("<note from=""Developer"" priority =""10""> DigitalBoard </note>")
                        Writer.WriteLine("</trans-unit>")
                        Writer.WriteLine(vbCrLf)
                    Next

                    Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

                End Using
            Next

            UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & ".xliff - File in " & CPS.CurrentLang & " generated for translators." & vbCrLf, Form_MainNew.RtbColor.Black)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub


    Public Sub CreateOutFile() Implements iXliff.CreateOutFile
        Try
            Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            'xlApp = New Excel.Application(DirectCast(CreateObject("Excel.Application"), Excel.Application))
            xlApp = New Excel.Application

            If System.IO.Path.GetExtension(_sfile).ToLower = ".xlsx" Or System.IO.Path.GetExtension(_sfile).ToLower = ".xls" And Not _sfile.ToString.Contains("$") Then
                Integrate(_sfile)
            End If

            UpdateMsg(Now & Chr(9) & "Integration done for " & CPS.CurrentLang & vbCrLf, Form_MainNew.RtbColor.Black)

            cnt_newintegrated += 1


        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            releaseObject(xlShtIN)
            releaseObject(xlWkb)
            xlApp.Quit()
            releaseObject(xlApp)
        End Try
    End Sub


    Private Sub Integrate(ByVal xlFile As String)
        Try
            Integrate(xlFile, CPS.Xliff_FileInBackFromtranslation)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub Integrate(ByVal xlFile As String, ByVal Xliff As String)

        Try
            Dim objXliff As sXliff = load_xliff(Xliff)

            xlWkb = xlApp.Workbooks.Open(xlFile)

            Dim WorkSheet As Excel.Worksheet = Nothing

            Dim lastCount As Integer = objXliff.ID.Count
            xlApp.DisplayAlerts = False
            Dim searchString As String = ""
            For Each WorkSheet In xlWkb.Worksheets
                xlShtIN = WorkSheet
                'For k As Integer = 0 To objXliff.ID.Count - 1
                '    Debug.Print(k & " - " & objXliff.Translation(k))
                '    If objXliff.Translation(k) <> String.Empty Then
                '        CU_Progress(lastCount, k)
                '        searchString = objXliff.Source(k).ToString.Trim
                '        xlShtIN.Cells.Replace(What:=searchString, Replacement:=objXliff.Translation(k), LookAt:=1, SearchOrder:=1, MatchCase:=False, SearchFormat:=False, ReplaceFormat:=False)
                '    End If
                'Next

                ''This takes too long to process
                Dim LastRow As Long = xlShtIN.UsedRange.Rows.Count - 1
                Dim LastColumn As Integer = xlShtIN.UsedRange.Columns.Count

                For j As Integer = 1 To xlShtIN.UsedRange.Columns.Count
                    LastRow = xlShtIN.Cells(xlShtIN.Rows.Count, j).End(-4162).Row
                    If xlShtIN.Name.Trim.ToLower = "data" Then 'Because Data sheet has only 1 row i.e header to be translated
                        LastRow = 5
                    End If

                    For i As Integer = 1 To LastRow
                        CU_Progress(LastRow, i)
                        If xlShtIN.Cells(i, j).interior.colorindex = ActiveProject.DigitalBoardColorIndex Then
                            For k As Integer = 0 To objXliff.ID.Count - 1
                                If xlShtIN.Cells(i, j).text.ToString.ToLower.Trim = objXliff.Source(k).ToString.ToLower.Trim Then
                                    xlShtIN.Cells(i, j) = objXliff.Translation(k)
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                Next
            Next

            File.Move(CPS.Xliff_FileInBackFromtranslation, CPS.Xliff_ProcessedFileInBackFromtranslation)

            xlApp.DisplayAlerts = True

            If Not System.IO.Directory.Exists(ActiveProject.ProjectPath & CloudProjectsettings.Folder_OutPut & "\Mono_" & objXliff.TargetLang) Then
                System.IO.Directory.CreateDirectory(ActiveProject.ProjectPath & CloudProjectsettings.Folder_OutPut & "\Mono_" & objXliff.TargetLang)
            End If

            xlWkb.SaveAs(ActiveProject.ProjectPath & CloudProjectsettings.Folder_OutPut & "Mono_" & objXliff.TargetLang & "\" & System.IO.Path.GetFileNameWithoutExtension(xlWkb.Name) & ".xlsx")

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            xlWkb.Close()
        End Try
    End Sub

    Public Sub ImportExistingTranslationToDB() Implements iXliff.ImportExistingTranslationToDB
        Try
            If Not File.Exists(CPS.Xliff_ExistingTranslationFile) Then
                If Xml_ToXliff(_sfile, CPS._PD.ProjectPath & CloudProjectsettings.Folder_ExistingTranslation, CPS.CurrentLang, True) Then
                    UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & ".xliff - File in " & CPS.CurrentLang & " Created in 09-ExitingTranslation folder." & vbCrLf, Form_MainNew.RtbColor.Black)
                End If
            Else
                UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & " - Xliff file in " & CPS.CurrentLang & " exists in 09-ExitingTranslation folder. Updating to DB" & vbCrLf, Form_MainNew.RtbColor.Black)
            End If

            If File.Exists(CPS.Xliff_ExistingTranslationFile) Then
                CU.UpdateDB(CPS.Xliff_ExistingTranslationFile, System.IO.Path.GetFileNameWithoutExtension(_sfile), , , CPS._PD.isMaxLengthCheckRequired)
            Else
                UpdateMsg(Now & Chr(9) & "No translation found for " & CPS.CurrentLang & vbCrLf, Form_MainNew.RtbColor.Black)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub



    Public Sub ExtractXiffwithExistingTranslation() Implements iXliff.ExtractXiffwithExistingTranslation
        Try
            If Not File.Exists(CPS.Xliff_ExistingTranslationFile) Then
                If Xml_ToXliff(_sfile, CPS._PD.ProjectPath & CloudProjectsettings.Folder_ExistingTranslation, CPS.CurrentLang, True) Then
                    UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & ".xliff - File in " & CPS.CurrentLang & " Created in 09-ExitingTranslation folder." & vbCrLf, Form_MainNew.RtbColor.Black)
                End If
            Else
                UpdateMsg(Now & Chr(9) & Path.GetFileNameWithoutExtension(_sfile) & " - Xliff file in " & CPS.CurrentLang & " exists in 09-ExitingTranslation folder." & vbCrLf, Form_MainNew.RtbColor.Black)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub


End Class
