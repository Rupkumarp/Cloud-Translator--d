Imports System.IO
Imports System.ComponentModel

Public Class Form_HanaPretranslate

    Public Sub UpdateToolStripStatus(ByVal Msg As String)
        ToolStripStatusLabel1.Text = Msg
    End Sub

    Public Sub UpdateProgress(ByVal Max As Integer, ByVal val As Integer)
        ToolStipUpdateTransId.Text = val & "\" & Max
        ToolStripProgressBar1.Maximum = Max
        ToolStripProgressBar1.Value = val
    End Sub

    Dim sProjectPath As String
    Dim CP As HanaProjectDetail

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            sProjectPath = ProjectManagement.GetActiveProject.ProjectPath ' get_last_projectpath()
            CP = New HanaProjectDetail(sProjectPath)

            LoadFilesFromTobeTranslatedFolder()
            LoadLangFromLastProject()
            LoadFileID()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            Exit Sub
        End Try

    End Sub

    Sub LoadFilesFromTobeTranslatedFolder()
        LB_FilestoPretranslate.Items.Clear()
        Dim sArr As New ArrayList

        For Each f In System.IO.Directory.GetFiles(CP.Path_TobeTranslated, "*.xliff")
            Dim sFileName As String = System.IO.Path.GetFileNameWithoutExtension(f)
            sFileName = sFileName.Substring(0, sFileName.Length - 6)
            If Not sArr.Contains(sFileName) Then
                sArr.Add(sFileName)
            End If
        Next

        For i As Integer = 0 To sArr.Count - 1
            LB_FilestoPretranslate.Items.Add(sArr(i))
        Next

        Label1.Text = "Files to pretranslate (" & LB_FilestoPretranslate.Items.Count & ")"
    End Sub

    Sub LoadLangFromLastProject()
        LB_Lang.Items.Clear()

        Dim str As String = ProjectManagement.GetActiveProject.LangList ' get_last_projectlanguages()
        For Each s In Split(str, ",")
            LB_Lang.Items.Add(s)
        Next
        Label3.Text = "Languages (" & LB_Lang.Items.Count & ")"
    End Sub

    Dim MyService As New Cloud_TR.Service1

    'Sub LoadFileID(ByVal id As String)
    '    Try
    '        LB_SourceID.Items.Clear()
    '        Dim ds As DataSet = MyService.GetPretranslateFileID(ObjectId.GetParentID(id, False))

    '        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
    '            LB_SourceID.Items.Add(ds.Tables(0).Rows(i).Item(0))
    '        Next
    '        Label2.Text = "Source IDs (" & LB_SourceID.Items.Count & ")"
    '    Catch ex As Exception
    '        MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
    '    End Try
    'End Sub

    Sub LoadFileID()
        Try

            Dim FD As Dictionary(Of String, String) = DefinitionFiles.GetFileDescription

            LB_SourceID.Items.Clear()
            Dim ds As DataSet = MyService.GetPretranslateFileID()

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim sDit As String = ""

                FD.TryGetValue(ds.Tables(0).Rows(i).Item(0), sDit)

                If sDit = String.Empty Then
                    sDit = "#NA"
                End If

                Dim sItemTemp As String
                sItemTemp = String.Format("{0}  ({1})", ds.Tables(0).Rows(i).Item(0), sDit)
                LB_SourceID.Items.Add(sItemTemp)
            Next
            Label2.Text = "Source IDs (" & LB_SourceID.Items.Count & ")"
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub BtnUP_Click(sender As Object, e As EventArgs) Handles BtnUP.Click
        If LB_SourceID.Items.Count > 0 Then
            Dim index As Integer = LB_SourceID.SelectedIndex - 1
            If index >= 0 Then
                LB_SourceID.Items.Insert(index, LB_SourceID.SelectedItem)
                LB_SourceID.Items.RemoveAt(LB_SourceID.SelectedIndex)
                LB_SourceID.SelectedIndex = index
            End If
        End If
    End Sub

    Private Sub BtnDown_Click(sender As Object, e As EventArgs) Handles BtnDown.Click

        If IsNothing(LB_SourceID.SelectedItem) = True Then
            Exit Sub
        End If

        If LB_SourceID.Items.Count > 0 Then
            Dim index As Integer = LB_SourceID.SelectedIndex + 1
            If index <= LB_SourceID.Items.Count - 1 Then
                LB_SourceID.Items.Insert(index + 1, LB_SourceID.SelectedItem)
                LB_SourceID.Items.RemoveAt(LB_SourceID.SelectedIndex)
                LB_SourceID.SelectedIndex = index
            End If
        End If
    End Sub

    'Private Sub LB_FilestoPretranslate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LB_FilestoPretranslate.SelectedIndexChanged
    '    If LB_FilestoPretranslate.Items.Count = 0 Then
    '        Exit Sub
    '    End If
    '    LoadFileID(LB_FilestoPretranslate.SelectedItem)
    'End Sub

    Private Sub BtnPretranslate_Click(sender As Object, e As EventArgs) Handles BtnPretranslate.Click

        ToolStripProgressBar1.Value = 0
        ToolStipUpdateTransId.Text = ""

        Try
            If LB_FilestoPretranslate.Items.Count = 0 Then
                Exit Sub
            End If

            If LB_FilestoPretranslate.SelectedIndex = -1 Then
                MsgBox("Please select the file to be transalted from the list!", MsgBoxStyle.Exclamation, "DB Pretranslate")
                Exit Sub
            End If

            If LB_SourceID.SelectedItems.Count = 0 Then
                MsgBox("Please select the Source file id from the list!", MsgBoxStyle.Exclamation, "DB Pretranslate")
                Exit Sub
            End If

            If LB_Lang.SelectedIndex = -1 Then
                MsgBox("Please select the Language from the list!", MsgBoxStyle.Exclamation, "DB Pretranslate")
                Exit Sub
            End If

            'Get FileID
            arrLb_SourceID = New ArrayList
            Dim sFileID As String = ""
            For Each fid In LB_SourceID.SelectedItems
                Dim s() As String = fid.ToString.Split("(")
                arrLb_SourceID.Add(s(0).Trim)
            Next

            Form_MainNew.UpdateMsg(Now & Chr(9) & "DB Pretranslate initiated" & vbCrLf, Form_MainNew.RtbColor.Black)

            arrLb_FilestoPretranslate = New ArrayList
            arrLb_lang = New ArrayList

            For Each f In LB_FilestoPretranslate.SelectedItems
                arrLb_FilestoPretranslate.Add(f)
            Next

            For Each l In LB_Lang.SelectedItems
                arrLb_lang.Add(l)
            Next

            Me.BtnPretranslate.Enabled = False
            Me.Cursor = Cursors.WaitCursor

            If Not BW.IsBusy Then
                BW.RunWorkerAsync()
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        Finally

        End Try
        UpdateToolStripStatus("Idle")
    End Sub


    Private arrLb_lang As ArrayList
    Private arrLb_FilestoPretranslate As ArrayList
    Private arrLb_SourceID As ArrayList

    Sub Pretranslate(ByVal sFile As String, ByVal sFileID As String, ByVal TargetLang As String, ByRef BW As BackgroundWorker)

        Dim Pretranslatexliffpath As String = CP.Path_Pretranslate & "Pre_" & System.IO.Path.GetFileNameWithoutExtension(sFile) & ".xliff"

        If RB_New.Checked Then
            If System.IO.File.Exists(Pretranslatexliffpath) Then
                System.IO.File.Delete(Pretranslatexliffpath)
            End If
        End If

        If RB_Skip.Checked Then
            If System.IO.File.Exists(Pretranslatexliffpath) Then
                BW.ReportProgress(2, {Now & Chr(9) & System.IO.Path.GetFileNameWithoutExtension(Pretranslatexliffpath) & " - File already translated, Pretranslation skipped" & vbCrLf, Form_MainNew.RtbColor.Green})
                Exit Sub
            End If
        End If

        Dim xliffData As New sXliff
        Dim MovedXliff As New sXliff
        Dim PretranslateCounter As Integer = 0
        Dim xliffCount As Integer

        Try
            xliffData = load_xliff(sFile)
            Dim CT As Cloud_TR.CloudTr
            Dim objCT As CloudWebServiceNew

            With MovedXliff
                .ID = New ArrayList
                .Note = New ArrayList
                .Resname = New ArrayList
                .Source = New ArrayList
                .Translation = New ArrayList
                .Note.Add(xliffData.Note(0))
            End With

            xliffCount = xliffData.Source.Count
            Dim counter As Integer = 0

            Dim i As Integer = xliffCount - 1

            Do Until i < 0
                BW.ReportProgress(1, "Pretranslating - " & System.IO.Path.GetFileName(sFile))
                If xliffData.Translation(i).ToString.Trim = String.Empty Then
                    CT = New Cloud_TR.CloudTr
                    With CT
                        .CustomerSpecific = 0
                        .Customer = ProjectManagement.GetActiveProject.CustomerName & "" ' GetCustomerName() & "" ' CustomerName
                        .Instance = ProjectManagement.GetActiveProject.InstaneName & "" ' GetInstance() & "" 'Instance
                        .Resname = xliffData.Resname(i) & ""
                        .Maxlength = 0 'To be checked with Laurent
                        .Source = revert_xml(xliffData.Source(i)) & ""
                        .Target = xliffData.Translation(i) & ""
                        .SourceLang = "enUS"
                        .TargetLang = xliffData.TargetLang.Replace("-", "") & ""
                        .Datatype = sFileID & ""
                    End With

                    If Not CheckSapConnection("Pretranslate exited") Then
                        Exit Sub
                    End If

                    objCT = New CloudWebServiceNew

                    Try
                        objCT.Timeout = 180000
                        Dim Myfile As String = System.IO.Path.GetFileNameWithoutExtension(sFile)
                        Myfile = Myfile.Substring(0, Myfile.Length - 6)
                        Dim parentID As String = ObjectId.GetParentID(System.IO.Path.GetFileName(Myfile), False) & ""
                        'CT.Target = objCT.PreTranslateFromCloud_New(CT, False, False, parentID)
                        CT.Target = objCT.GetTranslation(CT, False, False, parentID, True)
                    Catch ex As System.Net.WebException
                        If ex.Message.ToLower.Contains("the operation has timed out") Then
                            BW.ReportProgress(1, "Error - Pretranslate, Timeout - enUS searched -  '" & CT.Source & "'.")
                        ElseIf ex.Message.Contains("The underlying connection was closed: A connection that was expected to be kept alive was closed by the server.") Then
                            BW.ReportProgress(1, "Error - Pretranslate, Connection lost - enUS searched -  '" & CT.Source & "'.")
                        Else
                            Throw New Exception(ex.Message.ToString & " - " & ex.InnerException.ToString)
                        End If
                    Catch ex As System.IO.IOException
                        Throw New Exception(ex.Message.ToString & " - " & ex.InnerException.ToString)
                    Catch ex As Exception
                        If ex.Message.Contains("The underlying connection was closed: A connection that was expected to be kept alive was closed by the server.") Then
                            BW.ReportProgress(1, "Error - Pretranslate, Connection lost - enUS searched -  '" & CT.Source & "'.")
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
                    BW.ReportProgress(0, {xliffCount, counter})
                End If
                i -= 1
            Loop

        Catch ex As Exception
            Throw New Exception("Error @Cls_CloudJob.Pretranslate. " & ex.Message)
        Finally

            Dim MyxliffData As sXliff
            If RB_Append.Checked Then
                If System.IO.File.Exists(Pretranslatexliffpath) Then
                    MyxliffData = load_xliff(Pretranslatexliffpath)
                Else
                    MyxliffData = MovedXliff
                End If

                For i As Integer = 0 To MovedXliff.ID.Count - 1
                    With MyxliffData 'If found move to movedxliffdata
                        If Not .Source.Contains(MovedXliff.Source(i)) Then
                            .ID.Add(MovedXliff.ID(i))
                            .Note.Add(MovedXliff.Note(i + 1))
                            .Resname.Add(MovedXliff.Resname(i))
                            .Source.Add(MovedXliff.Source(i))
                            .TargetLang = xliffData.TargetLang
                            .Translation.Add(MovedXliff.Translation(i))
                        End If
                    End With
                Next
            Else
                MyxliffData = MovedXliff
            End If

            BW.ReportProgress(2, {Now & Chr(9) & System.IO.Path.GetFileName(sFile) & " - Pretranslate updated " & PretranslateCounter & "\" & xliffCount & " translations from DB." & vbCrLf, Form_MainNew.RtbColor.Black})

            'Updated MOved translation in Pretranslate folder
            If Not System.IO.Directory.Exists(CP.Path_Pretranslate) Then
                System.IO.Directory.CreateDirectory(CP.Path_Pretranslate)
            End If

            CreateXliffFile(MyxliffData, Pretranslatexliffpath, xliffData.TargetLang)
            BW.ReportProgress(1, System.IO.Path.GetFileName(Pretranslatexliffpath) & " Created in Pretranslate folder.")

            'Update xliff for transaltors
            'CreateXliffFile(xliffData, sFile, xliffData.TargetLang)
            'BW.ReportProgress(2, {Now & Chr(9) & System.IO.Path.GetFileName(sFile) & " Created for translators." & vbCrLf, Form_MainNew.RtbColor.Black})

        End Try

    End Sub

    Private Sub BW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BW.DoWork

        Dim Targetlang As String
        Dim sFileID As String = ""

        For i As Integer = 0 To arrLb_SourceID.Count - 1
            If sFileID = "" Then
                sFileID = arrLb_SourceID(i)
            Else
                sFileID = sFileID & "','" & arrLb_SourceID(i)
            End If
        Next
        sFileID = "'" & sFileID & "'"

        For x As Integer = 0 To arrLb_FilestoPretranslate.Count - 1
            Dim sFileName As String = CP.Path_TobeTranslated & arrLb_FilestoPretranslate(x)
            Dim sXliffFile As String
            For i As Integer = 0 To arrLb_lang.Count - 1
                Targetlang = arrLb_lang(i).ToString.Insert(2, "_")
                sXliffFile = sFileName & "_" & Targetlang & ".xliff"
                If System.IO.File.Exists(sXliffFile) Then
                    Pretranslate(sXliffFile, sFileID, Targetlang, BW)
                Else
                    BW.ReportProgress(0, "File missing")
                End If
            Next
        Next

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
                    Writer.WriteLine("<trans-unit id=" & Chr(34) & clean_xml(xliffData.ID(i)) & Chr(34) & " resname=" & Chr(34) & clean_xml(xliffData.Resname(i)) & Chr(34) & ">")
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

    Private Sub BW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BW.ProgressChanged
        If e.ProgressPercentage = 0 Then
            UpdateProgress(e.UserState(0), e.UserState(1))
        ElseIf e.ProgressPercentage = 1 Then
            UpdateToolStripStatus(e.UserState.ToString)
        ElseIf e.ProgressPercentage = 2 Then
            Form_MainNew.UpdateMsg(e.UserState(0), e.UserState(1))
        End If
    End Sub

    Private Sub BW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BW.RunWorkerCompleted
        If Not e.Error Is Nothing Then
            MsgBox(e.Error.Message.ToString, MsgBoxStyle.Critical, "Error")
            Me.Visible = True
            'Exit Sub
        End If
        Me.BtnPretranslate.Enabled = True
        Me.Cursor = Cursors.Default
    End Sub

    Private Function CheckSapConnection(ByVal Msg As String) As Boolean
        If Not CheckURL("http://10.66.9.51:8013/") Then
            Dim x As Integer = 1
            Do Until x = 4
                'RaiseEvent UpdateMsg(Now & Chr(9) & "Attempting to connect - " & x & vbCrLf, Form_MainNew.RtbColor.Black)
                If CheckURL("http://10.66.9.51:8013/") Then
                    Return True
                End If
                System.Threading.Thread.Sleep(1000)
                x += 1
            Loop
            'RaiseEvent UpdateMsg(Now & Chr(9) & "No Connection to SAP corporate... " & Msg & vbCrLf, Form_MainNew.RtbColor.Black)
            Return False
        End If
        Return True
    End Function



End Class

Public Class HanaProjectDetail

    Const Folder_Input As String = "01-Input\"
    Const Folder_InputB As String = "01-Input-B"
    Const Folder_TobeTransalted As String = "02-TobeTranslated\"
    Const Folder_BackFromTranslation As String = "03-Backfromtranslation\"
    Const Folder_OutPut As String = "05-Output\"
    Const Folder_Compare As String = "06-Compare\"
    Const Folder_Pretranslate As String = "07-Pretranslate\"
    Const Folder_TempReassmble As String = "04-tmpReassemble\"
    Const Folder_Competencies As String = "Competencies\"
    Const Folder_Picklists As String = "Picklists\"

    Public _ProjectPath As String

    Public Sub New(ByVal ProjectPath As String)
        _ProjectPath = ProjectPath
        Try
            CheckProjectSubFolderExistingOrNot_and_AssignPath()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Path_Input As String
    Public Path_InputB As String
    Public Path_Pretranslate As String
    Public Path_OutPut As String
    Public Path_TobeTranslated As String
    Public Path_BackFromTranslated As String
    Public Path_TempReassmble As String
    Public Path_Compare As String
    Public Path_Picklists As String
    Public Path_Competencies As String
    'Public CustomerName As String
    'Public Instance As String
    'Public isRestrictCustomer As Boolean
    'Public isInstance As Boolean
    'Public isCleanTranslation As Boolean

    Private Sub CheckProjectSubFolderExistingOrNot_and_AssignPath()

        Try
            If Not (Directory.Exists(_ProjectPath & Folder_Input)) Or _
               Not (Directory.Exists(_ProjectPath & Folder_TobeTransalted)) Or _
               Not (Directory.Exists(_ProjectPath & Folder_BackFromTranslation)) Or _
               Not (Directory.Exists(_ProjectPath & Folder_TempReassmble)) Or _
               Not (Directory.Exists(_ProjectPath & Folder_OutPut)) Then Throw New Exception("Incorrect folder structure in the selected project. Exiting now.") : Exit Sub

            Path_Input = _ProjectPath & Folder_Input
            Path_InputB = _ProjectPath & Folder_InputB
            Path_Pretranslate = _ProjectPath & Folder_Pretranslate
            Path_TobeTranslated = _ProjectPath & Folder_TobeTransalted
            Path_BackFromTranslated = _ProjectPath & Folder_BackFromTranslation
            Path_OutPut = _ProjectPath & Folder_OutPut
            Path_TempReassmble = _ProjectPath & Folder_TempReassmble
            Path_Compare = _ProjectPath & Folder_Compare
            Path_Competencies = _ProjectPath & Folder_Competencies
            Path_Picklists = _ProjectPath & Folder_Picklists
            'CustomerName = GetCustomerName()
            'Instance = GetInstance()
            'isCleanTranslation = isCleanTranslationEnabled()

        Catch ex As Exception
            Throw New Exception("Error Number @CheckProjectSubFolderExistingOrNot_and_AssignPath" & vbNewLine & ex.Message)
        End Try
    End Sub



End Class