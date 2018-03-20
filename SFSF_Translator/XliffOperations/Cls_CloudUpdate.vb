Imports System.IO

Public Class PreTranslateFileDetials

    Public Enum RtbColor
        Red
        Green
        Black
    End Enum

    Public Lang As String
    Public Folder_Pretranslate As String
    Public InputFile As String
    Public xliff_BackFromtransaltion As String

    Public Event UpdateMsg(ByVal Msg As String, ByVal RTBC As RtbColor)

    Public Sub New(ByVal _Lang As String, ByVal _Folder_Pretranslate As String, ByVal _InputFile As String, ByVal _xliff_Tobetranslated As String, ByVal _xliff_backfromtranslation As String)
        Lang = _Lang
        Folder_Pretranslate = _Folder_Pretranslate
        InputFile = _InputFile
        Xliff_TobeTranslated = _xliff_Tobetranslated
        xliff_BackFromtransaltion = _xliff_backfromtranslation
        FileName = System.IO.Path.GetFileNameWithoutExtension(InputFile)
        Pretranslatexliffpath = Folder_Pretranslate & "Pre_" & FileName & "_" & Lang & ".xliff"
        ProcessedPretranslatefile = Folder_Pretranslate & "(Processed)Pre_" & FileName & "_" & Lang & ".xliff"
    End Sub

    Public FileName As String
    Public Pretranslatexliffpath As String
    Public ProcessedPretranslatefile As String
    Public Xliff_TobeTranslated As String


    Public Enum PreTranslateDetails
        NotAvailable
        Available
        Processed
    End Enum

    Public Function isCheckPreTransltedFileAvailable() As PreTranslateDetails
        Dim PTD As PreTranslateDetails
        Try
            If System.IO.File.Exists(ProcessedPretranslatefile) Then
                PTD = PreTranslateDetails.Processed
            ElseIf System.IO.File.Exists(Pretranslatexliffpath) Then
                PTD = PreTranslateDetails.Available
            Else
                PTD = PreTranslateDetails.NotAvailable
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return PTD
    End Function

End Class

Public Class Cls_CloudJob

    Public Enum RtbColor
        Red
        Green
        Black
    End Enum

    Public Event UpdateMsg(ByVal Msg As String, ByVal RTBC As RtbColor)

    Public Event Progress(ByVal Max As Integer, ByVal val As Integer)

    Public Event UpdateToolstripMsg(ByVal Msg As String)



    ''' <summary>
    ''' Updating dbo.Cloud_TR table with enUS and translation for target lang
    ''' </summary>
    ''' <param name="xliff_file">Translated Xliff file</param>
    ''' <param name="_sFileId">Input Numbered file</param>
    ''' <remarks></remarks>
    ''' 
    Public Sub UpdateDB(ByVal xliff_file As String, ByVal _sFileId As String, Optional ByVal CustName As String = "TestCustomer", Optional ByVal OInstance As String = "TestInstance", Optional ByVal bRestrictMaxLenth As Boolean = False, Optional ByVal bRestrictToMaxLength As Boolean = False) ' Implements iXliff.UpdateDB

        Dim ExclusionList As ArrayList = GetExclusionList()

        Try
            RaiseEvent UpdateMsg(Now & Chr(9) & "Updating Database with " & Path.GetFileName(xliff_file) & " Please wait...." & vbCrLf, Form_MainNew.RtbColor.Black)
            Dim xliffData As New sXliff
            xliffData = load_xliff(xliff_file)
            Dim CT As Cloud_TR.CloudTr
            Dim objCT As Cloud_TR.Service1

            Dim xliffCount As Integer = xliffData.Source.Count
            Dim counter As Integer = 0
            For i As Integer = 0 To xliffCount - 1
                If xliffData.Translation(i).ToString.Trim <> String.Empty And Not IsNumeric(xliffData.Translation(i)) Then
                    CT = New Cloud_TR.CloudTr
                    With CT
                        If CustName = "TestCustomer" Then
                            .Customer = ProjectManagement.GetActiveProject.CustomerName ' GetCustomerName() ' CustomerName
                        Else
                            .Customer = CustName
                        End If

                        If OInstance = "TestInstance" Then
                            .Instance = ProjectManagement.GetActiveProject.InstaneName ' GetInstance() 'Instance
                        Else
                            .Instance = OInstance
                        End If
                        If xliffData.Resname(i).ToString.Trim = String.Empty Then
                            .Resname = "Test"
                        Else
                            .Resname = xliffData.Resname(i)
                        End If

                        .Maxlength = 0
                        If bRestrictMaxLenth Then
                            Try
                                .Maxlength = xliffData.Resname(i) & ""
                            Catch ex As Exception
                                .Maxlength = 0
                            End Try
                        End If
                        .Source = xliffData.Source(i)
                        .Target = xliffData.Translation(i)
                        .SourceLang = "enUS"
                        .TargetLang = xliffData.TargetLang.Replace("-", "")
                        If ExclusionList.Contains(.Source) Or ExclusionList.Contains(.Target) Then
                            .CustomerSpecific = 1
                        Else
                            .CustomerSpecific = 0
                        End If
                        .Datatype = _sFileId
                    End With

                    If Not CheckSapConnection(" Cannot update DB") Then
                        Exit Sub
                    End If
                    objCT = New Cloud_TR.Service1
                    If objCT.UpdateCloud(CT) Then
                        counter += 1
                    End If
                    RaiseEvent Progress(xliffCount, i + 1)
                End If
            Next
            RaiseEvent UpdateMsg(Now & Chr(9) & "Updated " & counter & " translations in DB." & vbCrLf, Form_MainNew.RtbColor.Black)
        Catch ex As Exception
            Throw New Exception("Error @Cls_CloudJob.UpdateDB" & vbNewLine & ex.Message)
        End Try

    End Sub


    ''' <summary>
    ''' <para>Pretranslate will save two xliff files</para>
    ''' <para>1. Moved xliff trnslation, the pretranslated xliff file</para>
    ''' <para>2. xliff file that needs to be sent for transaltion</para>
    ''' </summary>
    ''' <param name="PFD">PreTranslateFileDetials</param>
    ''' <param name="bRestrictCustomer"></param>
    ''' <param name="bInstance"></param>
    ''' <remarks></remarks>
    Public Sub PreTranslate1(ByVal PFD As PreTranslateFileDetials, ByVal bRestrictCustomer As Boolean, ByVal bInstance As Boolean, Optional ByVal bRestricttoMaxLength As Boolean = False)

        Dim ExclusionList As ArrayList = GetExclusionList()
        Dim xliffData As New sXliff
        Dim MovedXliff As New sXliff
        Dim PretranslateCounter As Integer = 0
        Try
            RaiseEvent UpdateMsg(Now & Chr(9) & "Pretranslating " & Path.GetFileName(PFD.Xliff_TobeTranslated) & " Please wait...." & vbCrLf, Form_MainNew.RtbColor.Black)

            xliffData = load_xliff(PFD.Xliff_TobeTranslated)
            Dim CT As Cloud_TR.CloudTr
            Dim objCT As CloudWebServiceNew

            With MovedXliff
                .ID = New ArrayList
                .Note = New ArrayList
                .Resname = New ArrayList
                .Source = New ArrayList
                .Translation = New ArrayList
                .MaxLength = New ArrayList
                .Note.Add(xliffData.Note(0))
            End With

            Dim xliffCount As Integer = xliffData.Source.Count
            Dim counter As Integer = 0


            Dim i As Integer = xliffCount - 1

            Do Until i < 0
                If xliffData.Translation(i).ToString.Trim = String.Empty Then
                    CT = New Cloud_TR.CloudTr
                    With CT
                        If ExclusionList.Contains(.Source) Or ExclusionList.Contains(.Target) Then
                            .CustomerSpecific = 1
                        Else
                            .CustomerSpecific = 0
                        End If
                        .Customer = ProjectManagement.GetActiveProject.CustomerName & "" ' GetCustomerName() & "" ' CustomerName
                        .Instance = ProjectManagement.GetActiveProject.InstaneName & "" ' GetInstance() & "" 'Instance
                        If xliffData.Resname(i).ToString.Trim = String.Empty Then
                            .Resname = "Test"
                        Else
                            .Resname = xliffData.Resname(i)
                        End If
                        .Maxlength = 0 'No limitations
                        If bRestricttoMaxLength Then
                            .Maxlength = xliffData.MaxLength(i)
                        End If
                        .Source = (xliffData.Source(i)) & "" ' revert_xml(xliffData.Source(i)) & ""
                        .Target = xliffData.Translation(i) & ""
                        .SourceLang = "enUS"
                        .TargetLang = xliffData.TargetLang.Replace("-", "") & ""
                        .Datatype = System.IO.Path.GetFileNameWithoutExtension(PFD.InputFile) & ""
                    End With

                    If Not CheckSapConnection("Pretranslate exited") Then
                        Exit Sub
                    End If

                    objCT = New CloudWebServiceNew

                    Try
                        objCT.Timeout = 880000
                        Dim parentID As String = ObjectId.GetParentID(System.IO.Path.GetFileName(PFD.InputFile), True) & ""
                        CT.Target = objCT.PreTranslateFromCloud(CT, bRestrictCustomer, bInstance, parentID)
                    Catch ex As System.Net.WebException
                        If ex.Message.ToLower.Contains("the operation has timed out") Then
                            RaiseEvent UpdateMsg(Now & Chr(9) & "Error - Pretranslate, Timeout - enUS searched -  '" & CT.Source & "'." & vbCrLf, Form_MainNew.RtbColor.Red)
                        ElseIf ex.Message.Contains("The underlying connection was closed: A connection that was expected to be kept alive was closed by the server.") Then
                            RaiseEvent UpdateMsg(Now & Chr(9) & "Error - Pretranslate, Connection lost - enUS searched -  '" & CT.Source & "'." & vbCrLf, Form_MainNew.RtbColor.Red)
                        Else
                            Throw New Exception(ex.Message.ToString & " - " & ex.InnerException.ToString)
                        End If
                    Catch ex As System.IO.IOException
                        Throw New Exception(ex.Message.ToString & " - " & ex.InnerException.ToString)
                    Catch ex As Exception
                        If ex.Message.Contains("The underlying connection was closed: A connection that was expected to be kept alive was closed by the server.") Then
                            RaiseEvent UpdateMsg(Now & Chr(9) & "Error - Pretranslate, Connection lost - enUS searched -  '" & CT.Source & "'." & vbCrLf, Form_MainNew.RtbColor.Red)
                        Else
                            Throw New Exception(ex.Message.ToString & " - " & ex.InnerException.ToString)
                        End If
                    Finally
                        objCT.Dispose()
                    End Try

                    If CT.Target.Trim <> String.Empty Then
                        With MovedXliff 'If found move to movedxliffdata
                            .ID.Add(xliffData.ID(i))
                            .Note.Add(xliffData.Note(i + 1))
                            .Resname.Add(xliffData.Resname(i))
                            .Source.Add(xliffData.Source(i))
                            .TargetLang = xliffData.TargetLang
                            .Translation.Add(CT.Target)
                            If bRestricttoMaxLength Then
                                .MaxLength.Add(xliffData.MaxLength(i))
                            End If
                        End With

                        With xliffData 'If Found remove from xliffdata
                            .ID.RemoveAt(i)
                            .Note.RemoveAt(i + 1)
                            .Resname.RemoveAt(i)
                            .Source.RemoveAt(i)
                            .Translation.RemoveAt(i)
                        End With
                        PretranslateCounter += 1
                    End If
                    counter += 1
                    RaiseEvent Progress(xliffCount, counter)
                    RaiseEvent UpdateToolstripMsg(counter & "\" & xliffCount)
               
                End If
                i -= 1
            Loop

        Catch ex As Exception
            Throw New Exception("Error @Cls_CloudJob.Pretranslate. " & ex.Message)
        Finally
            RaiseEvent UpdateMsg(Now & Chr(9) & "Pretranslate updated " & PretranslateCounter & " translations from DB." & vbCrLf, Form_MainNew.RtbColor.Black)

            'Updated MOved translation in Pretranslate folder
            If Not System.IO.Directory.Exists(PFD.Folder_Pretranslate) Then
                System.IO.Directory.CreateDirectory(PFD.Folder_Pretranslate)
            End If
            Dim Pretranslatexliffpath As String = PFD.Pretranslatexliffpath
            CreateXliffFile(MovedXliff, Pretranslatexliffpath, xliffData.TargetLang)
            RaiseEvent UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(Pretranslatexliffpath) & " Created in Pretranslate folder." & vbCrLf, Form_MainNew.RtbColor.Black)

            'Update xliff for transaltors
            If xliffData.ID.Count > 0 Then
                CreateXliffFile(xliffData, PFD.Xliff_TobeTranslated, xliffData.TargetLang)
            Else
                System.IO.File.Delete(PFD.Xliff_TobeTranslated)
                'RaiseEvent UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(PFD.Xliff_TobeTranslated) & " Created." & vbCrLf, Form_MainNew.RtbColor.Black)
            End If

            'Update Xliff in BackFromTranslation if all enUs is pretranslated
            If xliffData.Source.Count = 0 Then
                CreateXliffFile(xliffData, PFD.xliff_BackFromtransaltion, xliffData.TargetLang)
                'RaiseEvent UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(PFD.xliff_BackFromtransaltion) & " Created." & vbCrLf, Form_MainNew.RtbColor.Black)
            End If
        End Try

    End Sub

    Private Function CheckSapConnection(ByVal Msg As String) As Boolean
        If Not CheckURL("http://10.66.9.51:8013/") Then
            Dim x As Integer = 1
            Do Until x = 4
                RaiseEvent UpdateMsg(Now & Chr(9) & "Attempting to connect - " & x & vbCrLf, Form_MainNew.RtbColor.Black)
                If CheckURL("http://10.66.9.51:8013/") Then
                    Return True
                End If
                System.Threading.Thread.Sleep(1000)
                x += 1
            Loop
            RaiseEvent UpdateMsg(Now & Chr(9) & "No Connection to SAP corporate... " & Msg & vbCrLf, Form_MainNew.RtbColor.Black)
            Return False
        End If
        Return True
    End Function

    Public Sub ReIntegrate(ByVal PFD As PreTranslateFileDetials, Optional ByVal bRestrictToMaxLength As Boolean = False)
        Try
            Dim xliffData As New sXliff
            xliffData = load_xliff(PFD.xliff_BackFromtransaltion)

            If xliffData.Source.Count >= xliffData.Note.Count Then 'If note tag is less then use this
                Dim counter As Integer = xliffData.Source.Count - xliffData.Note.Count
                counter += 1
                For i As Integer = 1 To counter
                    xliffData.Note.Add("Dummy tag added")
                Next
            End If

            Dim PreTranslateXliffdata As sXliff
            PreTranslateXliffdata = load_xliff(PFD.Pretranslatexliffpath)

            Dim bCopy As Boolean
            Dim iCounter As Integer = 0

            For i As Integer = 0 To PreTranslateXliffdata.Source.Count - 1
                bCopy = True
                For j As Integer = 0 To xliffData.Source.Count - 1
                    If String.Compare(xliffData.Source(j), PreTranslateXliffdata.Source(i), True) = 0 Then
                        bCopy = False
                        If bRestrictToMaxLength Then 'To support max length restriction when checked in settings
                            If PreTranslateXliffdata.MaxLength.Count > 0 Then
                                If Not xliffData.MaxLength(j) = PreTranslateXliffdata.MaxLength(i) Then
                                    bCopy = True
                                End If
                            End If
                        End If

                        Exit For
                    End If
                Next
                If bCopy Then
                    With xliffData
                        .ID.Add(PreTranslateXliffdata.ID(i))
                        .Note.Add(PreTranslateXliffdata.Note(i + 1))
                        .Resname.Add(PreTranslateXliffdata.Resname(i))
                        .Source.Add(PreTranslateXliffdata.Source(i))
                        .Translation.Add(PreTranslateXliffdata.Translation(i))
                        If PreTranslateXliffdata.MaxLength.Count > 0 Then
                            .MaxLength.Add(PreTranslateXliffdata.MaxLength(i))
                        End If
                    End With
                End If
            Next

            'For i As Integer = 0 To PreTranslateXliffdata.Source.Count - 1
            '    bCopy = True
            '    With xliffData
            '        If Not .Source.Contains(PreTranslateXliffdata.Source(i)) Then
            '            If bCopy Then
            '                .ID.Add(PreTranslateXliffdata.ID(i))
            '                .Note.Add(PreTranslateXliffdata.Note(i + 1))
            '                .Resname.Add(PreTranslateXliffdata.Resname(i))
            '                .Source.Add(PreTranslateXliffdata.Source(i))
            '                .Translation.Add(PreTranslateXliffdata.Translation(i))            '              
            '            End If
            '        End If        
            '    End With
            'Next

            'Update xliff for transaltors
            CreateXliffFile(xliffData, PFD.xliff_BackFromtransaltion, xliffData.TargetLang)
            RaiseEvent UpdateMsg(Now & Chr(9) & System.IO.Path.GetFileName(PFD.xliff_BackFromtransaltion) & " Reintegrated." & vbCrLf, Form_MainNew.RtbColor.Black)

            File.Move(PFD.Pretranslatexliffpath, PFD.Folder_Pretranslate & "\(processed)" & Path.GetFileNameWithoutExtension(PFD.Pretranslatexliffpath) & ".xliff")

        Catch ex As Exception
            Throw New Exception("Error @Cls_CloudJob.ReIntegrate" & ex.Message)
        End Try
    End Sub


    Private Sub CreateXliffFile(ByVal xliffData As sXliff, ByVal xliff_Path As String, ByVal TargetLanguage As String)

        Try
            TargetLanguage = Replace(TargetLanguage, "_", "-")
            Dim TR As String = ""
            Dim cnt As Integer = 0
            Using Writer As StreamWriter = New StreamWriter(xliff_Path, False, System.Text.Encoding.UTF8)
                Writer.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                Writer.WriteLine("<xliff version=""1.2"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ")
                Writer.WriteLine("xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2- &#xD;&#xA;strict.xsd"">")
                Writer.WriteLine("<file original=""C:\Temp\en_fr_2.xliff"" source-language=""en-US"" target-language=" & Chr(34) & TargetLanguage & Chr(34) & " datatype=""plaintext"" date=" & Chr(34) & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & Chr(34) & " tool-id=""SAP SFSF CONVERTER"" category=""MLT"">")
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
                Dim myNum As Integer = 0

                For i As Integer = 0 To xliffData.Source.Count - 1
                    cnt = cnt + 1
                    myNum += 1
                    If xliffData.MaxLength.Count > 0 Then
                        Writer.WriteLine("<trans-unit id=" & Chr(34) & clean_xml(xliffData.ID(i)) & Chr(34) & " resname=" & Chr(34) & clean_xml(xliffData.Resname(i)) & Chr(34) & " size-unit=" & Chr(34) & "char" & Chr(34) & " maxwidth=" & Chr(34) & xliffData.MaxLength(i) & Chr(34) & ">")
                    Else
                        Writer.WriteLine("<trans-unit id=" & Chr(34) & clean_xml(xliffData.ID(i)) & Chr(34) & " resname=" & Chr(34) & clean_xml(xliffData.Resname(i)) & Chr(34) & ">")
                    End If
                    Writer.WriteLine("<source>" & wrap_html(clean_xml(xliffData.Source(i))) & "</source>")
                    Writer.WriteLine("<target state=""needs-review-translation"">" & wrap_html(clean_xml(xliffData.Translation(i))) & "</target>")
                    Writer.WriteLine("<note from=""Developer"" priority =""10"">" & clean_xml(xliffData.Note(i + 1)) & "</note>")
                    Writer.WriteLine("</trans-unit>")
                    Writer.WriteLine(vbCrLf)
                Next

                Writer.WriteLine("</body>" & vbNewLine & "</file>" & vbNewLine & "</xliff>")

            End Using

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub


    Private Function GetExclusionList() As ArrayList
        Dim ExclusionList As New ArrayList
        Try
            Dim ExlFile As String = ProjectManagement.GetActiveProject.ProjectPath & "DbExclusion.config"
            If Not System.IO.File.Exists(ExlFile) Then
                Return ExclusionList
            End If
            Dim str As String = ""
            Using reader As StreamReader = New StreamReader(ExlFile, True)
                str = reader.ReadLine
                If str.Trim <> "" Then
                    ExclusionList.Add(str)
                End If
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return ExclusionList
    End Function


End Class


