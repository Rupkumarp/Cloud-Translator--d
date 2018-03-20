Imports System.IO
Imports System.Environment
Imports System.ComponentModel
Imports System.Text
Imports System.Text.RegularExpressions
Imports Ionic.Zip
Imports WeifenLuo.WinFormsUI



Public Class Form_MainNew

    Private WithEvents ObjDelProject As New DeleteProjects
    Private WithEvents FTV As New Form_Treeview
    Private FLog As Form_Log
    Private WithEvents FErrLog As New Form_ErrorList

    Private fileID() As String
    Private fileTyp() As String
    Private bNoProject As Boolean = False
    Private _isDigitalBoard As Boolean = False
    Private _isDiBoAutomate As Boolean = False
    Private _isCxc As Boolean = False
    Private _isDBRProperties As Boolean = False
    Public Enum FileType
        Mdfcsv
        Xml_Cpxml
        OtherCsv
        Doc
        Competency
        QuestionLib
        Picklist
        MsgKey
        LMS
        SLC
        HybrisImpex
        HybrisXml
        HybrisProperties
        HybrisHtml
        RmkXhtml
        RmkCategoryJob
        OnboardingOffboarding
        LumiraDocument
        LumiraHeader
        DigitalBoard
        DBRproperties
        Cxc
        DiBOAutomate
        UnKnown
    End Enum

    Private Sub Form_MainNew_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.DockLeftPortion = FTV.DockPanel.DockLeftPortion
        My.Settings.Save()
    End Sub

    Sub ImpexSearch() 'IS for Hybris, this will check all files and extract contents in onel file


        'Impex search

        ' Dim ImpexFolder As String = "C:\Users\C5195092\Desktop\HybrisBankingStore\Test3\HybrisRawData\ext-publicsector"

        'check drive is local or remote

        'Dim di As New System.IO.DirectoryInfo("Z:\test")

        'Dim xDr As Boolean = IsLocal(di)

        Dim ImpexFolder As String = "C:\banking-translations-full\bin\custom"

        Dim str As New StringBuilder

        Dim k As Integer = 0

        For Each f In My.Computer.FileSystem.GetFiles(ImpexFolder, FileIO.SearchOption.SearchAllSubDirectories)

            If System.IO.Path.GetExtension(f).ToLower = ".impex" Or System.IO.Path.GetExtension(f).ToLower = ".html" _
           Or System.IO.Path.GetExtension(f).ToLower = ".properties" Or System.IO.Path.GetExtension(f).ToLower = ".tag" _
           Or System.IO.Path.GetExtension(f).ToLower = ".jsp" Or System.IO.Path.GetExtension(f).ToLower = ".js" Then
                k += 1
                Try
                    Dim reader As StreamReader = My.Computer.FileSystem.OpenTextFileReader(f.ToString)
                    str.Append(k & "________________________________________________________________________________________________________" & vbCrLf)
                    str.Append(f.ToString & vbCrLf & vbCrLf)
                    str.Append(reader.ReadToEnd.ToString & vbCrLf)
                    str.Append("________________________________________________________________________________________________________" & vbCrLf)
                    str.Append(vbNewLine)
                    str.Append(vbNewLine)
                    reader.Close()
                Catch ex As System.IO.PathTooLongException

                Catch ex As Exception
                    Throw New Exception(ex.Message)
                End Try
            End If

        Next


        Using Writer As StreamWriter = New StreamWriter("C:\Users\C5195092\Desktop\HybrisBankingStore\HybrisFourthSet1\01-Input\t.txt", False, System.Text.Encoding.UTF8)
            Writer.Write(str.ToString)
        End Using




        'For Each f In My.Computer.FileSystem.GetFiles(ImpexFolder, FileIO.SearchOption.SearchAllSubDirectories)
        '    If System.IO.Path.GetExtension(f).ToLower = ".xml" Then

        '        Dim reader As StreamReader = My.Computer.FileSystem.OpenTextFileReader(f.ToString)
        '        Dim a As String

        '        Do
        '            a = reader.ReadLine
        '            If Not a Is Nothing Then
        '                If a.ToLower.Contains("savings account") Then
        '                    Debug.Print(a & " -- - " & f.ToString)
        '                End If
        '            End If

        '        Loop Until a Is Nothing

        '        reader.Close()


        '    End If

        'Next


    End Sub

    Public Function IsLocal(dir As DirectoryInfo) As Boolean
        For Each d As DriveInfo In DriveInfo.GetDrives()
            If String.Compare(dir.Root.FullName, d.Name, StringComparison.OrdinalIgnoreCase) = 0 Then
                '[drweb86] Fix for different case.
                Return (d.DriveType <> DriveType.Network)
            End If
        Next
        Throw New DriveNotFoundException()
    End Function

    Private Sub Form_MainNew_Load(sender As Object, e As EventArgs) Handles Me.Load

        'ImpexSearch()

        FLog = New Form_Log

        FTV.AutoHidePortion = 200
        FTV.CloseButtonVisible = False
        FLog.CloseButtonVisible = False
        FLog.Show(DockPanel1)
        FTV.Show(DockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft)
        ' FTV.VisibleState = WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide

        FErrLog.CloseButtonVisible = False
        FErrLog.Show(DockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.DockBottom)
        'FErrLog.VisibleState = WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide

        FTV.DockState = Docking.DockState.DockLeft
        FErrLog.DockState = Docking.DockState.DockBottom

        FTV.DockPanel.DockLeftPortion = My.Settings.DockLeftPortion


        Try
            'Validate Defintion Files
            Dim VD As New DefinitionFiles
            VD.ValidateDefinitionFiles()

            'Assigns Languages from defintion file to LanguageDefination As LL in ModOperations
            LangDefintion.GetLanguageList()

            'Check FileType.Txt exists
            If Not File.Exists(appData & DefinitionFiles.FileType_List) Then
                Throw New Exception(appData & DefinitionFiles.FileType_List & " not found!")
            End If

            LoadTreeviewData.Load_ProjectDetial(FTV.TV)

            FLog.RichTextBox1.AppendText("Welcome to SFSF translation Manager" & vbCrLf & "Brought to you by the PTLS team" & vbCrLf & "SAP - 2015" & vbCrLf)

            If Not (load_filetype()) Then End

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
            End
        End Try


        'Report Initialization
        Try
            CloudReporting.MyCloudReport.GetInstance()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

    End Sub

    Private Function load_filetype() As Boolean
        Try
            Dim tmp As String = File.ReadAllText(appData & DefinitionFiles.FileType_List)
            Dim tmp_split() As String = Split(tmp, vbLf)
            Dim cnt As Integer = UBound(tmp_split)
            FLog.RichTextBox1.AppendText(Now & Chr(9) & cnt & " file types loaded" & vbCrLf)

            ReDim fileID(cnt)
            ReDim fileTyp(cnt)
            Dim f As Integer = 0
            Dim s() As String

            For Each filetype In tmp_split
                If filetype <> "" Then
                    s = Split(filetype, "|")
                    fileTyp(f) = s(0)
                    fileID(f) = s(1)
                    f = f + 1
                End If
            Next

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error Loading filetype")
        End Try
        Return True
    End Function


    Sub update_statusbar()
        TS_ProjectName.Text = ActiveProject.ProjectName
        TS_lang.Text = ActiveProject.LangList
    End Sub

    Sub savemylog()
        If Not (Directory.Exists(appData & "\log\")) Then Directory.CreateDirectory(appData & "\log\")
        FLog.RichTextBox1.SaveFile(appData & "\log\log." & Format(Now, "yyyy.mm.dd.hh.mm.ss"))
    End Sub

    Dim cnt_newtrans As Integer = 0
    Dim cnt_newintegrated As Integer = 0
    Dim tr_type As TranslationType

    Function GetFiletype(ByVal sFile As String) As FileType
        'note: multiple same file types could be created for the same project.
        'user would need to separate with a .
        'E.G. 10.1 we need 2 version 10.1.1.xxx and 10.1.2.xxx
        'the point for the comparison is required to ensure that we don't stop at 10.1, if e.g. we have 10.10

        If _isCxc Then
            If System.IO.Path.GetExtension(sFile) = ".xls" Or System.IO.Path.GetExtension(sFile) = ".xlsx" Or System.IO.Path.GetExtension(sFile) = ".xlsb" Then
                Return FileType.Cxc
            End If
        End If

        If _isDigitalBoard Then
            If System.IO.Path.GetExtension(sFile) = ".xls" Or System.IO.Path.GetExtension(sFile) = ".xlsx" Or System.IO.Path.GetExtension(sFile) = ".xlsb" Then
                Return FileType.DigitalBoard
            End If
        ElseIf _isDBRProperties Then
            Return FileType.DBRproperties
        End If

        If _isDiBoAutomate Then
            Return FileType.DiBOAutomate
        End If

        Dim sFileName As String = Path.GetFileNameWithoutExtension(sFile)

        Dim fType As Short = 0

        Dim FileNumber() As String = Split(sFileName, ".")
        Dim FileNumberDefintion() As String
        Dim bFound As Boolean = False

        If System.IO.Path.GetExtension(sFile) = ".docx" Or System.IO.Path.GetExtension(sFile) = ".doc" Then
            Return FileType.Doc
        End If

        For f = 0 To UBound(fileID)
            FileNumberDefintion = Split(fileID(f), ".")
            If UBound(FileNumber) > UBound(FileNumberDefintion) Then
                bFound = MapFileNumberWithFileTypeDefintion(FileNumberDefintion, FileNumber)
            Else
                bFound = MapFileNumberWithFileTypeDefintion(FileNumber, FileNumberDefintion)
            End If
            If bFound Then
                fType = fileTyp(f)
                Exit For
            End If
        Next

        'fType=0 then check if the file belongs to Hybris file or Lumira file
        If fType = 0 Then
            Select Case Microsoft.VisualBasic.Left(sFileName, 3)
                Case 200
                    fType = 200
                Case 201
                    fType = 201
                Case 202
                    fType = 202
                Case 203
                    fType = 203
                Case 300
                    fType = 300
                Case 400
                    fType = 400
                Case 401
                    fType = 401
            End Select
        End If

        'fType=0 then check if the file belongs to doc
        If fType = 0 Then
            If System.IO.Path.GetExtension(sFile).ToLower = ".doc" Or System.IO.Path.GetExtension(sFile).ToLower = ".docx" Then
                fType = 5
            End If
        End If

        Select Case fType
            Case 1
                Return FileType.Mdfcsv
            Case 2, 3
                Return FileType.Xml_Cpxml
            Case 4
                Return FileType.OtherCsv
            Case 5
                Return FileType.Doc
            Case 6
                Return FileType.Picklist
            Case 7
                Return FileType.QuestionLib
            Case 8
                Return FileType.Competency
            Case 9
                Return FileType.MsgKey
            Case 10
                Return FileType.LMS
            Case 11
                Return FileType.OnboardingOffboarding
            Case 100, 101
                Return FileType.SLC
            Case 200
                Return FileType.HybrisImpex
            Case 201
                Return FileType.HybrisProperties
            Case 202
                Return FileType.HybrisXml
            Case 203
                Return FileType.HybrisHtml
            Case 300
                Return FileType.RmkXhtml
            Case 301
                Return FileType.RmkCategoryJob
            Case 400
                Return FileType.LumiraDocument
            Case 401
                Return FileType.LumiraHeader
            Case Else
                Return FileType.UnKnown
        End Select

    End Function

    Public Enum RtbColor
        Red
        Green
        Black
    End Enum

    Private Sub Highlight(ByRef Rtb As RichTextBox, ByVal searchstring As String, ByVal mColor As RtbColor)

        Select Case mColor
            Case RtbColor.Black
                Exit Sub
        End Select

        Try
            Dim LineCounter As Integer = searchstring.Split(vbCrLf).Length
            Dim lastLine As Integer = (Rtb.Lines.Count - 1) - LineCounter

            For i As Integer = lastLine To Rtb.Lines.Count - 2
                Dim Text As String = FLog.RichTextBox1.Lines(i)
                Rtb.Select(Rtb.GetFirstCharIndexFromLine(i), Text.Length)
                Select Case mColor
                    Case RtbColor.Red
                        Rtb.SelectionColor = Color.Red
                    Case RtbColor.Green
                        Rtb.SelectionColor = Color.Green
                End Select
            Next

        Catch
        End Try
        Rtb.[Select](FLog.RichTextBox1.Text.Length, 0)
        Rtb.SelectionFont = New Font(Rtb.Font, FontStyle.Regular)
    End Sub

#Region "MenuBar Events"
    'MenuBar -> NEW

    Private WithEvents D1 As Dialog1
    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
        NewProject()
    End Sub

    Private Sub NewProject()
        FLog.RichTextBox1.AppendText(Now & Chr(9) & "New project definition started." & vbCrLf)

        D1 = New Dialog1
        ReDim Preserve D1.ProjectGroupList(LstProjectGroup.Count - 1)
        For i As Integer = 0 To LstProjectGroup.Count - 1
            D1.ProjectGroupList(i) = LstProjectGroup(i).ProjectGroupName
        Next i

        Dim TT As TreeNodeTag = FTV.TV.SelectedNode.Tag
        my_action = MyAction.Add_NewProject

        Select Case TT.TI
            Case TreeNodeTag.TagIndex.ProjectGroup
                If FTV.TV.isMasterInProject(FTV.TV.SelectedNode) Then
                    my_action = MyAction.Load_based_onMaserProject
                    D1.MasterProjectName = FTV.TV.GetMasterProjectName(FTV.TV.SelectedNode)
                End If
                D1.ProjectGroupName = FTV.TV.SelectedNode.Text
            Case TreeNodeTag.TagIndex.ProjectName
                If FTV.TV.isMasterInProject(FTV.TV.SelectedNode.Parent) Then
                    my_action = MyAction.Load_based_onMaserProject
                    D1.MasterProjectName = FTV.TV.GetMasterProjectName(FTV.TV.SelectedNode.Parent)
                End If
                D1.ProjectGroupName = FTV.TV.SelectedNode.Parent.Text
        End Select

        If D1.ShowDialog = Windows.Forms.DialogResult.OK Then
            update_statusbar()
        End If

    End Sub

    'MenuBar -> EDIT
    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        FLog.RichTextBox1.AppendText(Now & Chr(9) & "Project " & ActiveProject.ProjectName & " is being edited" & vbCrLf)
        my_action = MyAction.Edit_Project
        Dim FD As New Dialog1
        FD.ShowDialog()
        update_statusbar()
    End Sub

    'MenuBar -> OPEN
    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        FLog.RichTextBox1.AppendText(Now & Chr(9) & "Change active project." & vbCrLf)
        dia_projectlist.ShowDialog()
        LoadTreeviewData.Load_ProjectDetial(FTV.TV)
        update_statusbar()
    End Sub

    'MenuBar -> Exit
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        savemylog()
        End
    End Sub

    'MenuBar -> CP Form
    Private Sub CreateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        FLog.RichTextBox1.AppendText(Now & Chr(9) & "Create xml C/P file initiated." & vbCrLf)
        'here open the form for C/P creation
        Dim objFormCP As New FormCP
        objFormCP.ActionType = FormCP.Action.Create
        objFormCP.ShowDialog()
        objFormCP = Nothing
    End Sub

    'MenuBar -> CP Read
    Private Sub RetrieveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RetrieveToolStripMenuItem.Click


        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        FLog.RichTextBox1.AppendText(Now & Chr(9) & "Reading translated xml C/P file for pasting content back initiated." & vbCrLf)
        Dim objFormCP As New FormCP
        objFormCP.ActionType = FormCP.Action.Retrieve
        objFormCP.CurrentDirectory = ActiveProject.ProjectPath
        objFormCP.ShowDialog()
        objFormCP = Nothing
        'here open the form for C/P reading
    End Sub

    'MenuBar -> Save Log
    Private Sub SaveLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveLogToolStripMenuItem.Click
        FLog.RichTextBox1.AppendText(Now & Chr(9) & "Log save initiated." & vbCrLf)
        savemylog()
    End Sub

    'MenuBar -> Load Log
    Private Sub LoadLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadLogToolStripMenuItem.Click
        OpenFile.InitialDirectory = appData & "\log\"
        OpenFile.FileName = ""
        OpenFile.ShowDialog()

        If File.Exists(OpenFile.FileName) Then FLog.RichTextBox1.Text = File.ReadAllText(OpenFile.FileName)
        FLog.RichTextBox1.AppendText(Now & Chr(9) & OpenFile.FileName & " opened" & vbCrLf)
    End Sub

    'MenuBar -> Clear RichTextbox
    Private Sub ClearLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearLogToolStripMenuItem.Click
        FLog.RichTextBox1.Clear()
        FLog.RichTextBox1.Text = Now & Chr(9) & "Log cleared" & vbCrLf
    End Sub


    Private Function CheckProjectSelected() As Boolean

        If IsNothing(FTV.TV.SelectedNode) Then
            MsgBox("Please Create\Select the project!" & vbNewLine & "Cannot perform this action on root folder.", MsgBoxStyle.Exclamation, "Cannot Run!")
            Return False
        End If

        Dim tt As TreeNodeTag = FTV.TV.SelectedNode.Tag

        If Not tt.TI = TreeNodeTag.TagIndex.ProjectName Then
            MsgBox("Please select the Project from list!", MsgBoxStyle.Exclamation, "Cannot Run!")
            Return False
        End If
        Return True

    End Function

    'MenuBar -> Auto Transaltion
    Private Sub AutoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutoToolStripMenuItem.Click
        ActiveProject.bImportExistingtranslationsintoDB = False
        ActiveProject.bCreateXliffWithTranslation = False
        Execute()
    End Sub

    Private Sub Execute(Optional ByVal isDigitalBoard As Boolean = False, Optional isDBRproperties As Boolean = False)
        _isDigitalBoard = isDigitalBoard
        _isDBRProperties = isDBRproperties
        ActiveProject.bImportExistingtranslationsintoDB = False
        AutoCall()
    End Sub

    Private Sub AutoCall()

        Try
            'Assign\Create Log tab for Active Project--------------------------------------------------------------------------------------------------------------------------
            If FLog.Text = "Log" Then
                FLog.Text = ActiveProject.ProjectGroupName & "|" & ActiveProject.ProjectName
            Else
                CreateNewLogForm(ActiveProject.ProjectGroupName & "|" & ActiveProject.ProjectName, String.Empty)
            End If '---------------------------------------------------------------------------------------------------------------------------------------------------------

            Try
                ReCompileErrorStatus(FLog.Text)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Error Log Crash!")
            End Try

            ToolStripStatusLabel3.Text = "busy"
            Me.MenuStrip1.Enabled = False
            Me.FTV.TV.Enabled = False
            Application.DoEvents()
            notFoundDetails.Clear()
            notFoundMsg.Clear()

            cnt_newintegrated = 0
            cnt_newtrans = 0

            updateRtb(vbCrLf, RtbColor.Black)

            'check if all subfolders are existing
            If Not (Directory.Exists(ActiveProject.ProjectPath & "01-Input")) Or
                Not (Directory.Exists(ActiveProject.ProjectPath & "02-TobeTranslated")) Or
                Not (Directory.Exists(ActiveProject.ProjectPath & "03-Backfromtranslation")) Or
                Not (Directory.Exists(ActiveProject.ProjectPath & "04-tmpReassemble")) Or
                Not (Directory.Exists(ActiveProject.ProjectPath & "05-Output")) Then
                If MsgBox("Incorrect folder structure in the selected project or its been deleted!. Remvoing the Project", MsgBoxStyle.Critical, "Select another project") Then
                    ProjectManagement.DeleteProjectDetail(ActiveProject.ProjectName)
                    LoadTreeviewData.Load_ProjectDetial(FTV.TV)
                    Me.MenuStrip1.Enabled = True
                    Exit Sub
                End If
            End If

            Dim counter As Integer = My.Computer.FileSystem.GetFiles(ActiveProject.ProjectPath & "01-Input").Count

            If counter = 0 Then
                MsgBox("No input files available in the directory!" & vbNewLine & ActiveProject.ProjectPath & "01-Input", MsgBoxStyle.Information, "Exiting.")
                FLog.RichTextBox1.AppendText(Now & Chr(9) & "No input files available in the directory!" & vbNewLine & ActiveProject.ProjectPath & "01-Input" & vbCrLf)
                Me.MenuStrip1.Enabled = True
                Me.FTV.TV.Enabled = True
                Exit Sub
            End If

            Dim CoruptChars As New ArrayList
            Dim CorruptFileName As New ArrayList
            Dim obj As CorruptEncoding
            Dim CorruptChar As StringBuilder

            FLog.RichTextBox1.AppendText(Now & Chr(9) & "Auto convert files initiated." & vbCrLf)
            If ActiveProject.isCorruptEnabled Then
                FLog.RichTextBox1.AppendText(Now & Chr(9) & "Corruption detection started." & vbCrLf)
                For Each myFile In Directory.GetFiles(ActiveProject.ProjectPath & "01-Input")
                    Dim format As String = System.IO.Path.GetExtension(myFile).ToLower
                    If format = ".csv" Or format = ".xml" Or format = ".properties" Or format = ".impex" Then
                        obj = New CorruptEncoding(myFile)
                        CorruptChar = obj.SearchCorruptedChars
                        If CorruptChar.ToString.Trim <> "" Then
                            CoruptChars.Add(CorruptChar)
                            CorruptFileName.Add(myFile)
                            FLog.RichTextBox1.AppendText(Now & Chr(9) & "Corrupt File - " & System.IO.Path.GetFileName(myFile) & " - Number of lines affected " & Regex.Matches(CorruptChar.ToString(), Environment.NewLine).Count / 2 & vbCrLf)
                            UpdateProjectErrorDetail(ProjectErrorDetail.ErrType.Warninged, System.IO.Path.GetFileName(myFile), "Corrupt File")
                            Highlight(FLog.RichTextBox1, "Corrupt File - " & System.IO.Path.GetFileName(myFile) & CorruptChar.Length, RtbColor.Red)
                        End If
                    End If
                Next
                If CoruptChars.Count > 0 Then
                    Dim objCorruptForm As New Form_CorruptChars
                    objCorruptForm.CorruptChars = CoruptChars
                    objCorruptForm.CorruptFileName = CorruptFileName
                    objCorruptForm.ShowDialog()
                Else
                    FLog.RichTextBox1.AppendText(Now & Chr(9) & "No corruption detected." & vbCrLf)
                    Highlight(FLog.RichTextBox1, "", RtbColor.Green)
                End If

            End If

            '1.0 first let's convert new files to xliff

            'check if there are files which are present in 01 input and which are not yet in 02 tobetranslated
            'detection of files is done based on filename
            'if files are missing in 02, they are created.

            BW = New BackgroundWorker
            Me.BW.WorkerReportsProgress = True
            If Not BW.IsBusy Then
                BW.RunWorkerAsync()
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
        End Try
    End Sub

    'MenuBar -> Specific File to convert
    Private Sub SpecifyFileToXliffToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SpecifyFileToXliffToolStripMenuItem.Click
        Try
            Dim objOpenFile As New OpenFileDialog
            objOpenFile.Filter = "csv,xml,doc *.csv,*.xml|*.csv;*.xml;*.doc"

            If objOpenFile.ShowDialog = Windows.Forms.DialogResult.OK Then
                FLog.RichTextBox1.AppendText(Now & Chr(9) & "Conversion of specific file back from xliff initiated." & vbCrLf)
                InitiateJob(objOpenFile.FileName, BW)

                UpdateMsg(vbCrLf & Now & Chr(9) & "Process completed." & vbCrLf, RtbColor.Black)
                UpdateMsg(cnt_newintegrated & " file(s) have been updated with new translations." & vbCrLf, RtbColor.Black)
                UpdateMsg(cnt_newtrans & " newly added file(s) ready for translation." & vbCrLf, RtbColor.Black)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
        End Try
    End Sub

    'MenuBar - > Xliff to Xliff
    Private Sub XliffToXliffToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles XliffToXliffToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        Dim curpath As String = ActiveProject.ProjectPath
        Dim objXX As New Form_Xliff_to_Xliff
        objXX.xliff_Folder = curpath & "03-Backfromtranslation"
        objXX.ShowDialog()
    End Sub

#End Region

#Region "Grouping and Ungrouping files in 02-Translated"
    'Menubar -> Grouping -> UngroupFiles
    Private Sub UngroupFilesPerLangBackFromTRToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UngroupFilesPerLangBackFromTRToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        UpdateMsg(vbCrLf & Now & Chr(9) & "Ungrouping files initiated.", RtbColor.Black)
        Try
            Dim FolderList As String() = getAllFolders(ActiveProject.ProjectPath & "03-Backfromtranslation\")

            For i As Integer = 0 To UBound(FolderList)
                For Each myFile In Directory.GetFiles(FolderList(i))
                    File.Move(myFile, ActiveProject.ProjectPath & "03-Backfromtranslation\" & Path.GetFileName(myFile))
                Next
                System.IO.Directory.Delete(FolderList(i))
            Next
            UpdateMsg(vbCrLf & Now & Chr(9) & "Ungrouping files completed", RtbColor.Black)
        Catch ex As Exception
            UpdateMsg(vbCrLf & Now & Chr(9) & ex.Message, RtbColor.Red)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Cloud translator")
        End Try

    End Sub

    'Get Folders and sub folder list to string array
    Private Function getAllFolders(ByVal directory As String) As String()
        'Create object
        Dim fi As New IO.DirectoryInfo(directory)
        'Array to store paths
        Dim path() As String = {}
        'Loop through subfolders
        For Each subfolder As IO.DirectoryInfo In fi.GetDirectories()
            'Add this folders name
            Array.Resize(path, path.Length + 1)
            path(path.Length - 1) = subfolder.FullName
            'Recall function with each subdirectory
            For Each s As String In getAllFolders(subfolder.FullName)
                Array.Resize(path, path.Length + 1)
                path(path.Length - 1) = s
            Next
        Next
        Return path
    End Function

    'MenuBar -> Group File per language
    Private Sub GroupFilesPerLangToTRToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GroupFilesPerLangToTRToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        Try
            UpdateMsg(vbCrLf & Now & Chr(9) & "Grouping files initiated.", RtbColor.Black)

            Dim curlang() As String = Split(ActiveProject.LangList, ",")

            If curlang(0) = "" Then
                MsgBox("Project folder not found!", MsgBoxStyle.Critical, "Cloud translator")
                Exit Sub
            End If

            Dim tmp As String

            For f = 0 To UBound(curlang)
                If Not (Directory.Exists(ActiveProject.ProjectPath & "02-TobeTranslated\" & curlang(f) & "\")) Then Directory.CreateDirectory(ActiveProject.ProjectPath & "\02-TobeTranslated\" & curlang(f) & "\")
            Next


            For Each myFile In Directory.GetFiles(ActiveProject.ProjectPath & "02-TobeTranslated\")
                For f = 0 To UBound(curlang)
                    tmp = Mid(curlang(f), 1, 2) & "_" & Mid(curlang(f), 3, 2)
                    If InStr(myFile, tmp) <> 0 Then File.Move(myFile, Replace(myFile, "\02-TobeTranslated\", "\02-TobeTranslated\" & curlang(f) & "\"))
                Next
            Next

            UpdateMsg(vbCrLf & Now & Chr(9) & "Grouping files Completed.", RtbColor.Black)
        Catch ex As Exception
            UpdateMsg(vbCrLf & Now & Chr(9) & ex.Message, RtbColor.Red)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Cloud translator")
        End Try

    End Sub

#End Region

#Region "Manage Picklists"
    Private Sub ManageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        UpdateMsg(Now & Chr(9) & "Initiated Manage Picklist" & vbCrLf, RtbColor.Black)

        Dim objManagePicklist As New Form_ManagePicklist
        objManagePicklist.CurrentDirectory = ActiveProject.ProjectPath
        objManagePicklist.ShowDialog()
    End Sub
#End Region

#Region "Competency"
    'Menubar -> Competencies -> Convert Csv to Xml
    Private Sub ConvertCsvToXmlToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConvertCsvToXmlToolStripMenuItem.Click
        Dim objCsvToXml As New Form_CsvToXml
        objCsvToXml.ShowDialog()
    End Sub

    'Menubar -> Competencies -> ManageID
    Private Sub ManageIDsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageIDsToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        Dim objCsvCompGuid As New Form_CsvCompetencyGuid
        objCsvCompGuid.CurrentDirectory = ActiveProject.ProjectPath
        objCsvCompGuid.ShowDialog()
    End Sub

#Region "Competency Lib - Convert xml to csv competencies lib"
    Private Sub PushTranslationToMdfCsvToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PushTranslationToMdfCsvToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        cnt_newintegrated = 0
        cnt_newtrans = 0

        Dim objComplib As Complib
        objComplib = New MallardCompLib

        If Not System.IO.Directory.Exists(ActiveProject.ProjectPath & "Competencies") Then
            MsgBox("Directory not found!" & vbCrLf & ActiveProject.ProjectPath & "Competencies", MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        Else
            objComplib.myXliff.Competencies_Path = ActiveProject.ProjectPath & "Competencies"
        End If

        If Not System.IO.Directory.Exists(objComplib.myXliff.Competencies_Path & "\02-Extracted_xliff\") Then
            MsgBox("Directory not found!" & vbCrLf & objComplib.myXliff.Competencies_Path & "\02–Extracted_xliff\", MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        Else
            objComplib.myXliff.Extractedxliff_Path = objComplib.myXliff.Competencies_Path & "\02-Extracted_xliff\"
        End If

        If Not System.IO.Directory.Exists(objComplib.myXliff.Competencies_Path & "\03-Input_mdf_csv\") Then
            MsgBox("Directory not found!" & vbCrLf & objComplib.myXliff.Competencies_Path & "\03-Input_mdf_csv\", MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        Else
            objComplib.myXliff.Inputmdfcsv_Path = objComplib.myXliff.Competencies_Path & "\03-Input_mdf_csv\"
        End If

        If Not System.IO.Directory.Exists(objComplib.myXliff.Competencies_Path & "\04-Output_translated_mdf_csv\") Then
            MsgBox("Directory not found!" & vbCrLf & objComplib.myXliff.Competencies_Path & "\04-Output_translated_mdf_csv\", MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        Else
            objComplib.myXliff.Outputtranslatedmdfcsv_Path = objComplib.myXliff.Competencies_Path & "\04-Output_translated_mdf_csv\"
        End If

        objComplib.myXliff.Matchfiles_Path = objComplib.myXliff.Competencies_Path & "\07-Match_files\"
        objComplib.myXliff.Outputcsv_Path = objComplib.myXliff.Competencies_Path & "\08-Output_csv\"
        objComplib.myXliff.Standardcsv_Path = objComplib.myXliff.Competencies_Path & "\05-Standard_csv\"
        objComplib.myXliff.Standardxml_Path = objComplib.myXliff.Competencies_Path & "\01-Standard_xml\"
        objComplib.myXliff.StdcsvwithGUID_Path = objComplib.myXliff.Competencies_Path & "\06-Std_csv_with_GUID\"

        Try
            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor

            objComplib.Process()

            cnt_newintegrated = cnt_newintegrated + objComplib.cnt_newintegrated
            cnt_newtrans = cnt_newtrans + objComplib.cnt_newtrans

            UpdateMsg(vbCrLf & Now & Chr(9) & "Process completed." & vbCrLf, RtbColor.Black)
            UpdateMsg(cnt_newintegrated & " csv file have been updated." & vbCrLf, RtbColor.Black)
            UpdateMsg(cnt_newtrans & " translated xliff's generated." & vbCrLf, RtbColor.Black)

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
            UpdateProjectErrorDetail(ProjectErrorDetail.ErrType.Errored, "Competency Lib", ex.Message)
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
            UdpateErrorLog()
        End Try

    End Sub

#End Region
#End Region



#Region "Corrections/Search & Replace"

    Private Sub SearchReplaceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchReplaceToolStripMenuItem.Click
        Form_SearchCorrect.btnSearch.Enabled = True
        Form_SearchCorrect.cboLanguage.Text = ""
        Form_SearchCorrect.txtSearchTerm.Clear()
        Form_SearchCorrect.chkFullStringOnly.Checked = False
        Form_SearchCorrect.grdSearchResult.DataSource = Nothing
        Form_SearchCorrect.txtReplaceTerm.Clear()
        Form_SearchCorrect.RichTextBox1.Clear()
        Form_SearchCorrect.RichTextBox1.Text = ""
        Form_SearchCorrect.txtFolderPath.Text = Application.StartupPath & "\tools\Corrections\01 - PTLS\"
        Form_SearchCorrect.btnReplaceTerm.Enabled = False
        Form_SearchCorrect.ToolStripStatusLabel1.Text = "Idle..."
        Form_SearchCorrect.ShowDialog()
    End Sub

#End Region

#Region "Hybris Extract/Import process"

    Public Enum HybrisRMK
        RMK
        Hybris
    End Enum

    Private HRmk As HybrisRMK

    Private Sub ImportRmkHybris()
        Try
            Dim zipFile As String = ""
            Dim opnDialog As New OpenFileDialog

            If HRmk = HybrisRMK.Hybris Then
                opnDialog.Filter = "Hybris zip file '*.zip,*.rar|*.zip;*.rar"
            Else
                opnDialog.Filter = "RMK zip file '*.zip,*.rar|*.zip;*.rar"
            End If

            If opnDialog.ShowDialog <> Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If

            zipFile = opnDialog.FileName

            'Validation Check -------------------------------------------------------------------------------------------------------------
            If System.IO.Path.GetExtension(zipFile).ToLower = ".rar" Then
                If System.IO.File.Exists(My.Settings.winRar) <> True Then
                    If MessageBox.Show("Winrar could not be found!" & vbNewLine & "Please locate rar.exe or Click on help button to download the exe", "Cloud translator", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0, "http://rarlab.com/download.htm", "Cloud translator") = Windows.Forms.DialogResult.Yes Then
                        Dim opDialog As New OpenFileDialog
                        opDialog.Filter = "Winrar 'rar.exe|rar.exe"
                        If opDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                            My.Settings.winRar = opDialog.FileName
                            My.Settings.Save()
                        Else
                            Exit Sub
                        End If
                    End If
                End If
            End If

            Dim ProjectFolder As String = ActiveProject.ProjectPath

            If HRmk = HybrisRMK.Hybris Then
                ProjectFolder = ProjectFolder & "HybrisRawData\"
            Else
                ProjectFolder = ProjectFolder & "RMK_RawData\"
            End If

            If Not System.IO.Directory.Exists(ProjectFolder) Then
                System.IO.Directory.CreateDirectory(ProjectFolder)
            End If

            Dim myDir As DirectoryInfo = New DirectoryInfo(ProjectFolder)
            Dim bExtract As Boolean = False

            If (myDir.EnumerateFiles().Any()) Or myDir.EnumerateDirectories().Any Then
                Dim Msg As String
                If HRmk = HybrisRMK.Hybris Then
                    Msg = "There are already some files in the Hybris folder!"
                Else
                    Msg = "There are already some files in the RMK folder!"
                End If
                If MsgBox(Msg & vbNewLine & "Do you want to delete them?", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation) = MsgBoxResult.Yes Then
                    FLog.RichTextBox1.AppendText(vbNewLine & "Deleting files...")
                    ObjDelProject.DeleteFilesIffound(ProjectFolder)
                    ObjDelProject.DeleteDirectory(ProjectFolder)
                    bExtract = True
                Else
                    bExtract = False
                End If
            Else
                bExtract = True
            End If
            'Validation Check finished-------------------------------------------------------------------------------------------------------------

            If HRmk = HybrisRMK.Hybris Then
                Dim objHybris As New Form_HybrisDialog
                objHybris.zFile = zipFile
                objHybris.bExtract = bExtract
                objHybris.bUnzip = True
                objHybris.ProjectFolder = ProjectFolder
                'Extract Raw data to HybrisRawData folder
                objHybris.ShowDialog()
            Else
                objRmk = New RMK(zipFile, ProjectFolder, Replace(ProjectFolder, "RMK_RawData", "01-Input"))
                objRmk.Extract()
            End If


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Cloud translator!")
        End Try
    End Sub

    Private WithEvents objRmk As RMK
    Private Sub ImportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        HRmk = HybrisRMK.Hybris
        ImportRmkHybris()
    End Sub

    Private Sub ExtractRawFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExtractRawFilesToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        HRmk = HybrisRMK.RMK
        ImportRmkHybris()
    End Sub

    Private Sub OpenHybrisFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenHybrisFolderToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        HRmk = HybrisRMK.Hybris
        OpenRMKHybrisFolder()
    End Sub

    Private Sub OpenRmkFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenRMKFolderToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        HRmk = HybrisRMK.RMK
        OpenRMKHybrisFolder()
    End Sub


    Private Sub OpenRMKHybrisFolder()
        Dim ProjectFolder As String = ActiveProject.ProjectPath
        If HRmk = HybrisRMK.Hybris Then
            ProjectFolder = ProjectFolder & "HybrisRawData\"
        Else
            ProjectFolder = ProjectFolder & "RMK_RawData\"
        End If

        If Not System.IO.Directory.Exists(ProjectFolder) Then
            MsgBox("No Projects found!", MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        End If
        Diagnostics.Process.Start("explorer.exe", ProjectFolder)
    End Sub

    'Copy files from out folder to zip file path
    Private Sub ReImportOutFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReImportOutFilesToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        Dim ProjectFolder As String = ActiveProject.ProjectPath & "HybrisRawData\"
        Dim MappingFile As String = ProjectFolder & "HybrisMapping.xml"

        Try
            If ValidateHybrisMappingFile() <> True Then
                Throw New Exception("Error Validating HybrisMapping.xml file!" & vbCrLf)
            End If

            If GetArchiveType() = "rar" Then
                If System.IO.File.Exists(My.Settings.winRar) <> True Then
                    If MessageBox.Show("Winrar could not be found!" & vbNewLine & "Please locate rar.exe or Click on help button to download the exe", "Cloud translator", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0, "http://rarlab.com/download.htm", "Cloud translator") = Windows.Forms.DialogResult.Yes Then
                        Dim opDialog As New OpenFileDialog
                        opDialog.Filter = "Winrar 'rar.exe|rar.exe"
                        If opDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                            My.Settings.winRar = opDialog.FileName
                            My.Settings.Save()
                        Else
                            Exit Sub
                        End If
                    End If
                End If
            End If

            Dim objHybris As New Form_HybrisDialog

            objHybris.bExtract = False
            objHybris.bUnzip = False
            objHybris.ProjectFolder = ProjectFolder

            'Extract Raw data to HybrisRawData folder
            objHybris.ShowDialog()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Cloud translator")
        End Try

    End Sub

#End Region

    Private Sub DefinitionSettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DefinitionSettingsToolStripMenuItem.Click
        Dim FD As New Form_Definition
        FD.ShowDialog()
        load_filetype()
    End Sub

    Public Sub UpdateProgressBar(ByVal Max As Integer, ByVal val As Integer)
        ToolStripProgressBar1.Maximum = Max
        ToolStripProgressBar1.Value = val
    End Sub

    Private Sub objRmk_UpdateMsg(Msg As String, RTBC As RMK.RtbColor) Handles objRmk.UpdateMsg
        UpdateMsg(Msg, RTBC)
    End Sub

    Public Sub UpdateToolstripStatus(ByVal Msg As String)
        ToolStripStatusLabel3.Text = Msg
    End Sub

    Dim WithEvents BW As BackgroundWorker

    Private ActiveProject As ProjectDetail

    Sub InitiateJob(ByVal fileName As String, ByRef BW As BackgroundWorker)
        Dim str As New ArrayList
        Try
            Dim FType As FileType = GetFiletype(fileName)

            Dim objXliff As Xliff = Nothing

            Select Case FType
                Case FileType.Mdfcsv
                    objXliff = New Mallard_MdfCsv
                Case FileType.Xml_Cpxml
                    objXliff = New MallardXmlCP
                Case FileType.OtherCsv
                    objXliff = New MallardOtherCsv
                Case FileType.Doc
                    objXliff = New MallardDoc
                Case FileType.Competency
                    objXliff = New MallardCompetency
                Case FileType.Picklist
                    objXliff = New MallardPicklist
                Case FileType.QuestionLib
                    objXliff = New MallardQuestionlib
                Case FileType.MsgKey
                    objXliff = New MallardMsgKey
                Case FileType.LMS
                    objXliff = New MallardLMS
                Case FileType.SLC
                    objXliff = New MallardSLC
                Case FileType.HybrisHtml
                    objXliff = New MallardHybrisHtml
                Case FileType.HybrisImpex
                    objXliff = New MallardHybrisImpex
                Case FileType.HybrisProperties
                    objXliff = New MallardHybrisProperties
                Case FileType.HybrisXml
                    objXliff = New MallardHybrisXml
                Case FileType.RmkXhtml
                    objXliff = New MallardRmkXhtml
                Case FileType.RmkCategoryJob
                    objXliff = New MallardRmkCategoryJob
                Case FileType.OnboardingOffboarding
                    objXliff = New MallardOnboardingOffboarding
                Case FileType.LumiraHeader
                    objXliff = New MallardLumiraHeader
                Case FileType.LumiraDocument
                    objXliff = New MallardLumiraDocument
                Case FileType.DigitalBoard
                    objXliff = New MallardDigitalBoard
                Case FileType.DBRproperties
                    objXliff = New MallardHybrisProperties
                Case FileType.Cxc
                    objXliff = New Mallard_CXC
                Case FileType.DiBOAutomate
                    objXliff = New Mallard_DiboAutomate
                Case FileType.UnKnown
                    str.Add(Now & Chr(9) & "####" & Path.GetFileName(fileName) & "##### - Unknown file type..." & vbCrLf)
                    BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, System.IO.Path.GetFileName(fileName), "Unknown file type"})
                    str.Add(RtbColor.Red)
                    BW.ReportProgress(4, str)
                    Exit Sub
            End Select


            Dim curlang() As String = Split(ActiveProject.LangList, ",")

            For f = 0 To UBound(curlang)
                curlang(f) = Mid(curlang(f), 1, 2) & "_" & Mid(curlang(f), 3, 2)
            Next

            objXliff.myXliff.curlang = curlang
            objXliff.myXliff.ActiveProject = ActiveProject

            '*********************************************Process Starting point**************************************************************
            str.Add(Now & Chr(9) & "*********************************************File Name - " & Path.GetFileName(fileName) & " Initiated*********************************************" & vbCrLf)
            str.Add(RtbColor.Black)
            BW.ReportProgress(4, str)
            objXliff.Process(fileName, BW)
            str = New ArrayList
            str.Add(Now & Chr(9) & "*********************************************File Name - " & Path.GetFileName(fileName) & " Ended*********************************************" & vbCrLf)
            str.Add(RtbColor.Black)
            BW.ReportProgress(4, str)
            cnt_newintegrated = cnt_newintegrated + objXliff.cnt_newintegrated
            cnt_newtrans = cnt_newtrans + objXliff.cnt_newtrans

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub BW_DoWork(sender As Object, e As DoWorkEventArgs) Handles BW.DoWork

        BW.ReportProgress(6, {100, 0}) 'Initialize Progress bar value to 0 

        Dim fileName As String = ""
        If Not System.IO.Directory.Exists(ActiveProject.ProjectPath & CloudProjectsettings.Folder_InputB) Then
            System.IO.Directory.CreateDirectory(ActiveProject.ProjectPath & CloudProjectsettings.Folder_InputB)
        End If
        Try
            Dim FileCount As Integer = My.Computer.FileSystem.GetFiles(ActiveProject.ProjectPath & CloudProjectsettings.Folder_Input).Count
            Dim FileCounter As Integer = 0
            For Each myFile In Directory.GetFiles(ActiveProject.ProjectPath & CloudProjectsettings.Folder_Input)

                If Not System.IO.Path.GetFileName(myFile).StartsWith("~$") Then
                    If Not Microsoft.VisualBasic.Left(System.IO.Path.GetFileNameWithoutExtension(myFile), 8).ToLower = "corrupt_" Then
                        If Not System.IO.File.Exists(ActiveProject.ProjectPath & CloudProjectsettings.Folder_InputB & System.IO.Path.GetFileName(myFile)) Then
                            File.Copy(myFile, ActiveProject.ProjectPath & CloudProjectsettings.Folder_InputB & System.IO.Path.GetFileName(myFile), False)
                        End If
                        fileName = ActiveProject.ProjectPath & CloudProjectsettings.Folder_InputB & System.IO.Path.GetFileName(myFile).ToString
                        BW.ReportProgress(3, {100, 0})
                        Try
                            InitiateJob(fileName, BW)
                        Catch ex As Exception
                            Dim str As New ArrayList
                            str.Add(Now & Chr(9) & "FileName - " & Path.GetFileName(fileName) & " - " & ex.Message & vbCrLf)
                            str.Add(RtbColor.Red)
                            BW.ReportProgress(4, str)
                            'Assign Error to Error log
                            BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, System.IO.Path.GetFileName(fileName), ex.Message})
                        End Try
                    Else
                        BW.ReportProgress(4, {Now & Chr(9) & "File is Marked as corrupt - " & System.IO.Path.GetFileName(myFile) & vbCrLf, RtbColor.Green})
                        BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Warninged, System.IO.Path.GetFileName(myFile), "File is Marked as corrupt"})
                    End If
                End If


                FileCounter += 1
                BW.ReportProgress(6, {FileCount, FileCounter}) 'OVerall Progress
            Next
        Catch ex As Exception
            Dim str As New ArrayList
            str.Add(Now & Chr(9) & "FileName - " & Path.GetFileName(fileName) & " - " & ex.Message & vbCrLf)
            str.Add(RtbColor.Red)
            BW.ReportProgress(4, str)

            'Assign Error to Error log
            BW.ReportProgress(5, {ProjectErrorDetail.ErrType.Errored, System.IO.Path.GetFileName(fileName), ex.Message})
        End Try

    End Sub

    Private Sub updateRtb(ByVal Msg As String, ByVal mycolor As RtbColor)
        UpdateMsg(Msg, mycolor)
    End Sub

    Private Sub BW_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BW.ProgressChanged
        If e.ProgressPercentage = 0 Then
            ToolStripStatusLabel1.Text = e.UserState.ToString
        ElseIf e.ProgressPercentage = 3 Then
            ToolStripProgressBar1.Maximum = CInt(e.UserState(0))
            ToolStripProgressBar1.Value = CInt(e.UserState(1))
        ElseIf e.ProgressPercentage = 4 Or e.ProgressPercentage = 2 Or e.ProgressPercentage = 1 Then
            UpdateMsg(e.UserState(0), e.UserState(1))
        ElseIf e.ProgressPercentage = 5 Then
            UpdateProjectErrorDetail(e.UserState(0), e.UserState(1), e.UserState(2))
        ElseIf e.ProgressPercentage = 6 Then
            ToolStripProgressBar2.Maximum = e.UserState(0)
            ToolStripProgressBar2.Value = e.UserState(1)
        End If
    End Sub

    Public Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As RtbColor)
        FLog.RichTextBox1.AppendText(Msg)
        Highlight(FLog.RichTextBox1, Msg.Trim, MyColor)

        If Not bPauseScroll Then
            Exit Sub
        End If
        FLog.RichTextBox1.SelectionStart = FLog.RichTextBox1.Text.Length
        FLog.RichTextBox1.ScrollToCaret()


    End Sub

    Private Sub BW_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BW.RunWorkerCompleted

        If Not e.Error Is Nothing Then
            MsgBox(e.Error.Message.ToString, MsgBoxStyle.Critical, "")
            Me.FLog.RichTextBox1.Text = vbCrLf & e.Error.Message.ToString
            Exit Sub
        End If

        updateRtb(vbCrLf & Now & Chr(9) & "Process completed." & vbCrLf, RtbColor.Black)
        updateRtb(cnt_newintegrated & " file(s) have been updated with new translations." & vbCrLf, RtbColor.Black)
        updateRtb(cnt_newtrans & " newly added file(s) ready for translation." & vbCrLf, RtbColor.Black)

        If notFoundDetails.ToString <> "" Then
            updateRtb(notFoundDetails.ToString, RtbColor.Red)
            updateRtb(vbCrLf & "There are missing translations, Please check the log for more information!" & vbCrLf, RtbColor.Red)
            UpdateProjectErrorDetail(ProjectErrorDetail.ErrType.Warninged, "", "There are missing translations, Please check the log for more information!")
        End If

        'Update Errortab
        UdpateErrorLog()

        _isDBRProperties = False
        _isCxc = False
        _isDigitalBoard = False

        ToolStripStatusLabel3.Text = "Idle"
        Me.MenuStrip1.Enabled = True
        Me.FTV.TV.Enabled = True
    End Sub

    Private Sub ReimportOutFilesToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ReimportOutFilesToolStripMenuItem1.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        Try
            Dim curlang() As String = Split(ActiveProject.LangList, ",")
            Dim projectPath As String = ActiveProject.ProjectPath
            Dim RmkMapFile As String = projectPath & "RMK_RawData\RmkMapping.xml"
            For f = 0 To UBound(curlang)
                curlang(f) = Mid(curlang(f), 1, 2) & "_" & Mid(curlang(f), 3, 2)

                Dim OutFileFolder As String = projectPath & CloudProjectsettings.Folder_OutPut & "RmkMono_" & curlang(f)
                If System.IO.Directory.Exists(OutFileFolder) Then
                    objRmk = New RMK(projectPath, OutFileFolder, RmkMapFile, curlang(f))
                    objRmk.ReImport()
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub PerForceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PerForceToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        Dim f As New Form_HanaPretranslate
        f.ShowDialog()
    End Sub

    Private Sub CleaningToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CleaningToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        Dim f As New Form_Cleaning
        f.ShowDialog()
    End Sub

#Region "Project Management"



    Private Sub FTV_Treeview_AfterSelect(ByRef Sender As Object, ByRef e As TreeViewEventArgs) Handles FTV.Treeview_AfterSelect
        If Not bNoProject Then
            TS_ProjectName.Text = "Sample project name"
            TS_lang.Text = "xx-XX, xx-XX, xx-XX, xx-XX, xx-XX, xx-XX"
            Dim TT As TreeNodeTag = FTV.TV.SelectedNode.Tag

            Select Case TT.TI
                Case TreeNodeTag.TagIndex.ProjectGroup
                    Exit Sub
                Case TreeNodeTag.TagIndex.Root
                    Exit Sub
                Case TreeNodeTag.TagIndex.ProjectName
                    If TT.isMaster Then
                        FTV.TV.SelectedNode.ImageIndex = TreeNodeTag.MasterImageIndex
                        FTV.TV.SelectedNode.SelectedImageIndex = TreeNodeTag.MasterImageIndex
                    Else
                        FTV.TV.SelectedNode.ImageIndex = GetProjectImageIndex(True)
                        FTV.TV.SelectedNode.SelectedImageIndex = GetProjectImageIndex(True)
                    End If
                    FTV.TV.LabelEdit = True
                    FTV.TV.SelectedNode.NodeFont = New Font(FTV.TV.Font, FontStyle.Bold)
                    FTV.TV.LabelEdit = False
            End Select
            ProjectManagement.MakeActiveProject(e.Node.Text)
            ActiveProject = ProjectManagement.GetActiveProject
            If Microsoft.VisualBasic.Right(ActiveProject.ProjectPath, 1) <> "\" Then
                ActiveProject.ProjectPath = ActiveProject.ProjectPath & "\"
            End If
            UpdateMsg(Now & Chr(9) & "Project Selected - " & e.Node.Text & vbCrLf, RtbColor.Black)
            update_statusbar()
        End If
    End Sub

    Private Sub FTV_Treeview_BeforeSelect(ByRef Sender As Object, ByRef e As TreeViewCancelEventArgs) Handles FTV.Treeview_BeforeSelect
        If Not bNoProject Then
            If IsNothing(FTV.TV.SelectedNode) Then
                Exit Sub
            End If
            For i As Integer = 0 To FTV.TV.Nodes(0).Nodes.Count - 1
                For j As Integer = 0 To FTV.TV.Nodes(0).Nodes(i).Nodes.Count - 1
                    Dim tt As TreeNodeTag = FTV.TV.Nodes(0).Nodes(i).Nodes(j).Tag

                    If tt.TI = TreeNodeTag.TagIndex.ProjectGroup Or tt.TI = TreeNodeTag.TagIndex.Root Then
                        Exit Sub
                    End If

                    FTV.TV.Nodes(0).Nodes(i).Nodes(j).ImageIndex = GetProjectImageIndex(False)
                    FTV.TV.Nodes(0).Nodes(i).Nodes(j).SelectedImageIndex = GetProjectImageIndex(False)

                    If tt.TI = TreeNodeTag.TagIndex.ProjectName Then
                        If tt.isMaster Then
                            FTV.TV.Nodes(0).Nodes(i).Nodes(j).ImageIndex = TreeNodeTag.MasterImageIndex
                            FTV.TV.Nodes(0).Nodes(i).Nodes(j).SelectedImageIndex = TreeNodeTag.MasterImageIndex
                        End If
                    End If

                    FTV.TV.Nodes(0).Nodes(i).Nodes(j).NodeFont = New Font(FTV.TV.Font, FontStyle.Regular)
                Next
            Next

        End If
    End Sub

    Private Sub FTV_Treeview_MyNodeSlected(ByRef NodeText As String) Handles FTV.Treeview_MyNodeSlected
        Dim bFound As Boolean = False
        For i As Integer = 0 To FTV.TV.Nodes(0).Nodes.Count - 1
            For j As Integer = 0 To FTV.TV.Nodes(0).Nodes(i).Nodes.Count - 1
                FTV.TV.Nodes(0).Nodes(i).Nodes(j).ImageIndex = GetProjectImageIndex(False)
                FTV.TV.Nodes(0).Nodes(i).Nodes(j).SelectedImageIndex = GetProjectImageIndex(False)

                If FTV.TV.Nodes(0).Nodes(i).Nodes(j).Text.ToLower = NodeText.ToLower Then

                    Dim TT As TreeNodeTag = FTV.TV.Nodes(0).Nodes(i).Nodes(j).Tag
                    Dim TN As TreeNode = FTV.TV.Nodes(0).Nodes(i).Nodes(j)
                    FTV.TV.SelectedNode = TN
                    If TT.isMaster Then
                        FTV.TV.Nodes(0).Nodes(i).Nodes(j).ImageIndex = TreeNodeTag.MasterImageIndex
                        FTV.TV.Nodes(0).Nodes(i).Nodes(j).SelectedImageIndex = TreeNodeTag.MasterImageIndex
                    Else
                        FTV.TV.Nodes(0).Nodes(i).Nodes(j).ImageIndex = GetProjectImageIndex(True)
                        FTV.TV.Nodes(0).Nodes(i).Nodes(j).SelectedImageIndex = GetProjectImageIndex(True)
                    End If

                    bFound = True
                    UpdateMsg(Now & Chr(9) & "Project Moved - " & NodeText & vbCrLf, RtbColor.Black)

                    ProjectManagement.MoveProjectToAnotherGroup(NodeText, FTV.TV.Nodes(0).Nodes(i).Text)

                    Load_ProjectDetailAgain()
                    Exit For
                End If
            Next
            If bFound Then
                Exit For
            End If
        Next
    End Sub

    Private Sub FTV_Treeview_DeleteFilesAndProject(ByRef sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_DeleteFilesAndProject
        If MsgBox("This will delete the Project!", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Warning Deleting Project") = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim ProjectFolder As String = ProjectManagement.GetActiveProject.ProjectPath '  GetProjectLocation(TV.SelectedNode.Text)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            FLog.RichTextBox1.AppendText(Now & Chr(9) & "Deleting files, please wait..." & vbCrLf)
            Highlight(FLog.RichTextBox1, "", RtbColor.Black)
            ObjDelProject.DeleteFilesIffound(ProjectFolder)
            ObjDelProject.DeleteDirectory(ProjectFolder)
            FLog.RichTextBox1.AppendText(Now & Chr(9) & "All files have been deleted." & vbCrLf)
            ProjectManagement.DeleteProject(FTV.TV.SelectedNode.Text)
            LoadTreeviewData.Load_ProjectDetial(FTV.TV)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        Finally
            Me.Cursor = Cursors.Default
            Me.Enabled = True
        End Try
    End Sub

    Private Sub FTV_Treeview_DeleteFilesFromProject(ByRef sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_DeleteFilesFromProject
        If MsgBox("This will delete all the Input files,xliff files and Out files!" & vbNewLine & "Are you sure you want to delete those files?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Delete files") = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim ProjectFolder As String = ActiveProject.ProjectPath
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            FLog.RichTextBox1.AppendText(Now & Chr(9) & "Deleting files, please wait..." & vbCrLf)
            Highlight(FLog.RichTextBox1, "", RtbColor.Black)
            ObjDelProject.DeleteFilesIffound(ProjectFolder)
            ' DeleteDirectory(ProjectFolder)
            FLog.RichTextBox1.AppendText(Now & Chr(9) & "All files have been deleted." & vbCrLf)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        Finally
            Me.Cursor = Cursors.Default
            Me.Enabled = True
        End Try
    End Sub

    Private Sub FTV_Treeview_EditProjectDetail(ByRef sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_EditProjectDetail
        FLog.RichTextBox1.AppendText(Now & Chr(9) & "Project " & ActiveProject.ProjectName & " is being edited" & vbCrLf)
        my_action = MyAction.Edit_Project
        Dim FD As New Dialog1
        FD.ShowDialog()
        update_statusbar()
    End Sub

    Private WithEvents objCTP As CTP
    Private Sub FTVTreeview_ExportCTP(ByRef sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_ExportCTP

        MsgBox("Please select the folder where you want to save the CTP file", MsgBoxStyle.Information, "Cloud TR")
        Dim FD As New FolderBrowserDialog
        FD.Description = "Select folder!"
        If FD.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        Me.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Try
            objCTP = New CTP
            objCTP.ExportCTP(FTV.TV.SelectedNode.Text, FD.SelectedPath)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub FTV_Treeview_SetMasterProject(ByRef sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_SetMasterProject
        If FTV.TV.isMasterAlready(FTV.TV.SelectedNode.Parent, FTV.TV.SelectedNode.Text) Then
            Exit Sub
        End If
        If FTV.TV.isMasterInProject(FTV.TV.SelectedNode.Parent) Then
            If MsgBox("This Projectgroup already has a master!" & vbNewLine & "Are you sure you want to set the selected one as Master?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "?") = MsgBoxResult.No Then
                Exit Sub
            End If
        End If
        SetMasterProject()
    End Sub

    Private Sub FTV_SetMaster() Handles FTV.SetMaster
        SetMasterProject()
    End Sub

    Private Sub FTV_UnsetMaster() Handles FTV.UnSetMaster
        UnsetMasterProject()
    End Sub


    Public Sub SetMasterProject()

        Dim TT As TreeNodeTag
        For i As Integer = 0 To FTV.TV.SelectedNode.Parent.Nodes.Count - 1
            TT = FTV.TV.SelectedNode.Parent.Nodes(i).Tag
            TT.isMaster = False
            FTV.TV.SelectedNode.Parent.Nodes(i).Tag = TT
            FTV.TV.SelectedNode.Parent.Nodes(i).SelectedImageIndex = GetProjectImageIndex(False)
            FTV.TV.SelectedNode.Parent.Nodes(i).ImageIndex = GetProjectImageIndex(False)
            'cross_form_functions.edit_project(TV.SelectedNode.Parent.Nodes(i).Text, TV.SelectedNode.Parent.Text, TT.isMaster)
            ProjectManagement.SetMasterProject(FTV.TV.SelectedNode.Parent.Text, FTV.TV.SelectedNode.Parent.Nodes(i).Text)
        Next

        TT = FTV.TV.SelectedNode.Tag
        TT.isMaster = True
        FTV.TV.SelectedNode.SelectedImageIndex = TreeNodeTag.MasterImageIndex
        FTV.TV.SelectedNode.ImageIndex = TreeNodeTag.MasterImageIndex
        FTV.TV.SelectedNode.Tag = TT
        ProjectManagement.SetMasterProject(FTV.TV.SelectedNode.Parent.Text, FTV.TV.SelectedNode.Text)
        ' cross_form_functions.edit_project(TV.SelectedNode.Text, TV.SelectedNode.Parent.Text, TT.isMaster)
        Load_ProjectDetailAgain()
    End Sub

    Public Sub UnsetMasterProject()
        ProjectManagement.UnSetMasterProject(FTV.TV.SelectedNode.Parent.Text, FTV.TV.SelectedNode.Text)
        Load_ProjectDetailAgain()
        LoadTreeviewData.Load_ProjectDetial(FTV.TV)
    End Sub


    Private Sub FTV_Treeview_NewProjectGroup(ByRef sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_NewProjectGroup
        Dim f As New Form_ProjectGroup
        If f.ShowDialog = Windows.Forms.DialogResult.OK Then
            LoadTreeviewData.Load_ProjectDetial(FTV.TV)
        End If
    End Sub

    Private Sub FTV_Treeview_EditProjectGroup(ByRef sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_EditProjectGroup
        Dim f As New Form_EditProjectGroup
        If f.ShowDialog = Windows.Forms.DialogResult.OK Then
            LoadTreeviewData.Load_ProjectDetial(FTV.TV)
        End If
    End Sub

    Private Sub FTV_Treeview_ImportProjectGroup(ByRef sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_ImportProjectGroup
        Dim f As New Form_ImportCtproj
        If f.ShowDialog = DialogResult.OK Then
            LoadTreeviewData.Load_ProjectDetial(FTV.TV)
        End If
    End Sub

    Private Sub FTV_Treeview_Refresh(ByRef sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_Refresh
        LoadTreeviewData.Load_ProjectDetial(FTV.TV)
    End Sub


    Public Sub RefreshTreeview() Handles RefeshToolStripMenuItem.Click
        LoadTreeviewData.Load_ProjectDetial(FTV.TV)
    End Sub

    Private Sub FTV_Treeview_NewProject(ByRef sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_NewProject
        NewProject()
    End Sub

    Private Sub FTV_Treeview_EditProjectGroupProperties(ByRef sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_EditProjectGroupProperties
        Dim f As New Form_EditProjectGroup
        f.ProjectGroupName = FTV.TV.SelectedNode.Text
        If f.ShowDialog = Windows.Forms.DialogResult.OK Then
            LoadTreeviewData.Load_ProjectDetial(FTV.TV)
        End If
    End Sub

    Private Sub FTV_Treeview_DeleteProjectGroupAndAllProject(ByRef sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_DeleteProjectGroupAndAllProject

        If ProjectManagement.CompareTwoStrings(FTV.TV.SelectedNode.Text, "Default") Then
            MsgBox("Default Group cannot be Deleted!", MsgBoxStyle.Critical, "Permission Denied")
            Exit Sub
        End If

        If MsgBox("This will delete the Project in this group!" & vbNewLine & "Are you sure you want to Continue?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Warning Deleting Project") = MsgBoxResult.No Then
            Exit Sub
        End If

        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            FLog.RichTextBox1.AppendText(Now & Chr(9) & "Deleting Project Group - " & FTV.TV.SelectedNode.Text & ", please wait..." & vbCrLf)
            Highlight(FLog.RichTextBox1, "", RtbColor.Black)


            Dim ProjectList As ArrayList = ProjectManagement.GetProjectNameList(FTV.TV.SelectedNode.Text)

            For i As Integer = 0 To ProjectList.Count - 1
                Dim ProjectFolder As String = ProjectManagement.GetProjectDetail(ProjectList(i)).ProjectPath
                ObjDelProject.DeleteFilesIffound(ProjectFolder)
                ObjDelProject.DeleteDirectory(ProjectFolder)
            Next

            FLog.RichTextBox1.AppendText(Now & Chr(9) & "All files have been deleted." & vbCrLf)
            ProjectManagement.DeleteProjectGroupWithPurge(FTV.TV.SelectedNode.Text)
            LoadTreeviewData.Load_ProjectDetial(FTV.TV)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        Finally
            Me.Cursor = Cursors.Default
            Me.Enabled = True
        End Try
    End Sub

    Private Sub FTV_Treeview_DeleteProjectGroupOnly(ByRef sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_DeleteProjectGroupOnly
        Try
            Dim ProjectGroupName As String = FTV.TV.SelectedNode.Text

            If ProjectManagement.CompareTwoStrings(ProjectGroupName, "Default") Then
                MsgBox("Default Group cannot be Deleted!", MsgBoxStyle.Critical, "Permission Denied")
                Exit Sub
            End If

            If MsgBox("This action will delete the Project-GroupName from the list." & vbNewLine & "But the Project will be moved to 'Default' group." & vbNewLine & "Are you sure you want to continue?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Delete ProjectGroupName") = MsgBoxResult.No Then
                Exit Sub
            End If

            ProjectManagement.DeleteProjectGroupWithoutPurge(ProjectGroupName)
            LoadTreeviewData.Load_ProjectDetial(FTV.TV)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub FTV_Treeview_ExportProject(ByRef sedner As Object, ByRef e As EventArgs) Handles FTV.Treeview_ExportProject
        MsgBox("Please select the folder where you want to save the .ctproj file", MsgBoxStyle.Information, "Cloud TR")
        Dim FD As New FolderBrowserDialog
        FD.Description = "Select folder!"
        If FD.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        Me.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Try
            objCTP = New CTP
            objCTP.ExportCtproj(FTV.TV.SelectedNode.Text, FD.SelectedPath)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub FTV_Treeview_ImportCTP(ByRef sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_ImportCTP
        Try
            Dim f As New Form_ImportCTP
            f.ProjectGroupName = FTV.TV.SelectedNode.Text
            If f.ShowDialog = Windows.Forms.DialogResult.OK Then
                LoadTreeviewData.Load_ProjectDetial(FTV.TV)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

    End Sub

    Private Sub objCTP_UpdateForm(EnableForm As Boolean) Handles objCTP.UpdateForm
        LoadTreeviewData.Load_ProjectDetial(FTV.TV)
        update_statusbar()
        Me.Enabled = True
        Me.Cursor = Cursors.Default
        NAR(objCTP)
    End Sub

    Private Sub objCTP_UpdateMsg(Msg As String) Handles objCTP.UpdateMsg
        UpdateMsg(Msg, RtbColor.Black)
    End Sub

#End Region

    Sub Load_ProjectDetailAgain()
        LstProjectGroup = New List(Of ProjectGroup)
        LstProjectGroup = XMLMethod.GetProjectGroupListFromXml
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        OpenProjectFolder()
    End Sub

    Private Sub OpenProjectFolder()
        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        If Not System.IO.Directory.Exists(ActiveProject.ProjectPath) Then
            MsgBox("No Projects found!", MsgBoxStyle.Critical, "Cloud translator")
            Exit Sub
        End If
        Diagnostics.Process.Start("explorer.exe", ActiveProject.ProjectPath)
    End Sub

    Private Sub FTV_OpenProjectFolder() Handles FTV.OpenProjectFolder
        OpenProjectFolder()
    End Sub

    ''' <summary>
    ''' Removes all Tabbed Log sheets and Create New tab with "Log" as title
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DeleteLogsAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteLogsAllToolStripMenuItem.Click
        If MsgBox("This will remove all the Log sheets!" & vbNewLine & "Are you sure you want to remove?", MsgBoxStyle.Critical + vbYesNo, "Remove Log tabs") = MsgBoxResult.No Then
            Exit Sub
        End If
        Dim bFound As Boolean = False
        For Each ChildForm As Form In Me.MdiChildren
            If ChildForm.Text = "Log" Then
                FLog = ChildForm
                bFound = True
            Else
                ChildForm.Close()
            End If
        Next
        ClearFErrorlog()
        If Not bFound Then
            CreateNewLogForm("Log", "Log Cleared")
        End If
    End Sub

    ''' <summary>
    ''' Creates New tabbed log window
    ''' </summary>
    ''' <param name="LogTitle"></param>
    ''' <param name="RichText"></param>
    Public Sub CreateNewLogForm(ByVal LogTitle As String, ByVal RichText As String)

        Dim bFound As Boolean = False
        For Each ChildForm As Form In Me.MdiChildren
            If ChildForm.Text = LogTitle Then
                FLog = ChildForm
                FLog.Activate()
                bFound = True
                Exit For
            End If
        Next

        If Not bFound Then
            FLog = New Form_Log
            FLog.Text = LogTitle
            FLog.CloseButtonVisible = False
            FLog.Show(DockPanel1)
            FLog.RichTextBox1.Text = RichText
        End If

    End Sub

#Region "Error List Details"

    Dim PED As New ProjectErrorDetail

    ''' <summary>
    ''' The function sets the PED ProjectErrorDetail Object.
    ''' This object will store errors and warnings in list(of string) format.
    ''' </summary>
    ''' <param name="EType"></param>
    ''' <param name="FileName"></param>
    ''' <param name="Msg"></param>
    Public Sub UpdateProjectErrorDetail(ByVal EType As ProjectErrorDetail.ErrType, ByVal FileName As String, ByVal Msg As String)

        Dim bFound As Boolean = False
        For i As Integer = 0 To PED._ErrTab.Count - 1
            If PED._EType(i) = EType And PED._FileName(i) = FileName And PED._Msg(i) = Msg Then
                bFound = True
            End If
        Next i

        Dim LineCounter As Integer = Msg.Split(vbCrLf).Length
        Dim lastLine As Integer = (FLog.RichTextBox1.Lines.Count - 1) - LineCounter

        If Not bFound Then
            PED._EType.Add(EType)
            PED._FileName.Add(FileName)
            PED._Msg.Add(Msg)
            PED._ErrTab.Add(FLog.Text)
            PED._LineNumber.Add(lastLine)
        End If

    End Sub

    ''' <summary>
    ''' Required when Auto is reRun after making corrections for the errored file or item.
    ''' This will remove the error listed earlier from the Log tab name refernce.
    ''' </summary>
    ''' <param name="TabName"></param>
    Private Sub ReCompileErrorStatus(ByVal TabName As String)

        'First Remove ProjectErrorDetails 
        Dim iCounter As Integer = PED._ErrTab.Count - 1
        Do Until iCounter = -1
            If PED._ErrTab(iCounter) = TabName Then
                PED._ErrTab.RemoveAt(iCounter)
                PED._EType.RemoveAt(iCounter)
                PED._FileName.RemoveAt(iCounter)
                PED._LineNumber.RemoveAt(iCounter)
                PED._Msg.RemoveAt(iCounter)
            End If
            iCounter -= 1
        Loop


        'Second Remove from Error Tabs collection
        For i As Integer = 0 To ModHelper.Etabs.Count - 1
            If ModHelper.Etabs(i)._ErrTab = TabName Then
                ModHelper.Etabs.RemoveAt(i)
                Exit For
            End If
        Next
    End Sub

    ''' <summary>
    ''' ModHelper - Etabs with list(of Errortabs) is been used to store each Log Tab information.
    ''' Here it sets the Form_Errlog's Combobox with Log Tab name
    ''' </summary>
    Private Sub UdpateErrorLog()
        Dim eTab As New ErrorTabs
        eTab._ErrTab = FLog.Text
        eTab._PED = PED

        If ModHelper.Etabs.Count = 0 Then
            ModHelper.Etabs.Add(eTab)
        Else
            Dim bFound As Boolean = False
            For i As Integer = 0 To ModHelper.Etabs.Count - 1
                If ModHelper.Etabs(i)._ErrTab = eTab._ErrTab Then
                    ModHelper.Etabs.RemoveAt(i)
                    ModHelper.Etabs.Add(eTab)
                    bFound = True
                End If
            Next
            If Not bFound Then
                ModHelper.Etabs.Add(eTab)
            End If
        End If

        'Update Datagrid in Ferrlog
        FErrLog.ToolStripComboBox1.Items.Clear()
        For i As Integer = 0 To ModHelper.Etabs.Count - 1
            FErrLog.ToolStripComboBox1.Items.Add(ModHelper.Etabs(i)._ErrTab)
            FErrLog.ToolStripComboBox1.Text = FLog.Text
        Next

    End Sub


    Private Sub FErrLog_Errlog_SelectionChanged(ByRef sender As Object, e As EventArgs) Handles FErrLog.Errlog_SelectionChanged
        FErrLog.DataGridView1.Rows.Clear()
        Dim WarningCount As Integer = 0
        Dim ErrorCount As Integer = 0
        For i As Integer = 0 To ModHelper.Etabs.Count - 1
            If ModHelper.Etabs(i)._ErrTab = FErrLog.ToolStripComboBox1.Text Then
                'Update Error and Warning Count
                For j As Integer = 0 To ModHelper.Etabs(i)._PED._EType.Count - 1
                    If ModHelper.Etabs(i)._PED._EType(j) = ProjectErrorDetail.ErrType.Errored And ModHelper.Etabs(i)._PED._ErrTab(j) = FErrLog.ToolStripComboBox1.Text Then
                        ErrorCount += 1
                    ElseIf ModHelper.Etabs(i)._PED._EType(j) = ProjectErrorDetail.ErrType.Warninged And ModHelper.Etabs(i)._PED._ErrTab(j) = FErrLog.ToolStripComboBox1.Text Then
                        WarningCount += 1
                    End If
                Next
            End If
        Next
        FErrLog.ButtonWarning.Text = WarningCount & " Warnings"
        FErrLog.ButtonError.Text = ErrorCount & " Errors"
        ErrorList()
    End Sub

    Private Sub FErrLog_ErrorButton_Click(ByRef sender As Object, e As EventArgs) Handles FErrLog.ErrorButton_Click
        ErrorList()
    End Sub

    Private Sub ErrorList()
        FErrLog.DataGridView1.Rows.Clear()
        For i As Integer = 0 To ModHelper.Etabs.Count - 1
            If ModHelper.Etabs(i)._ErrTab = FErrLog.ToolStripComboBox1.Text Then
                'Update Error and Warning Count
                For j As Integer = 0 To ModHelper.Etabs(i)._PED._EType.Count - 1
                    If ModHelper.Etabs(i)._PED._EType(j) = ProjectErrorDetail.ErrType.Errored And ModHelper.Etabs(i)._PED._ErrTab(j) = FErrLog.ToolStripComboBox1.Text Then
                        FErrLog.DataGridView1.Rows.Add(ModHelper.Etabs(i)._PED._Msg(j), ModHelper.Etabs(i)._PED._FileName(j), ModHelper.Etabs(i)._PED._LineNumber(j))
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub FErrLog_WarningButton_Click(ByRef sender As Object, e As EventArgs) Handles FErrLog.WarningButton_Click
        FErrLog.DataGridView1.Rows.Clear()
        For i As Integer = 0 To ModHelper.Etabs.Count - 1
            If ModHelper.Etabs(i)._ErrTab = FErrLog.ToolStripComboBox1.Text Then
                'Update Error and Warning Count
                For j As Integer = 0 To ModHelper.Etabs(i)._PED._EType.Count - 1
                    If ModHelper.Etabs(i)._PED._EType(j) = ProjectErrorDetail.ErrType.Warninged And ModHelper.Etabs(i)._PED._ErrTab(j) = FErrLog.ToolStripComboBox1.Text Then
                        FErrLog.DataGridView1.Rows.Add(ModHelper.Etabs(i)._PED._Msg(j), ModHelper.Etabs(i)._PED._FileName(j), ModHelper.Etabs(i)._PED._LineNumber(j))
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub FErrLog_DV_SelectionChanged(ByRef sender As Object, e As EventArgs) Handles FErrLog.DV_SelectionChanged
        If FErrLog.DataGridView1.SelectedRows.Count = 0 Then
            Exit Sub
        End If
        Dim r As DataGridViewRow = FErrLog.DataGridView1.SelectedRows(0)
        SelectErrorWarningLine(r.Cells(2).Value)

    End Sub

    Private Sub SelectErrorWarningLine(ByVal LineNumber As Long)
        For Each ChildForm As Form In Me.MdiChildren
            If ChildForm.Text = FErrLog.ToolStripComboBox1.Text Then
                FLog = ChildForm
                FLog.Activate()
                Exit For
            End If
        Next

        Dim Start As Integer = FLog.RichTextBox1.GetFirstCharIndexFromLine(LineNumber)
        Dim length As Integer = 0
        Try
            length = FLog.RichTextBox1.Lines(LineNumber).Length
        Catch ex As Exception
            MsgBox("Cannot find the Error\Warning message!" & vbNewLine & "The log might have been cleared.", MsgBoxStyle.Information, "Nothing to Display")
            Exit Sub
        End Try
        FLog.RichTextBox1.SelectionStart = Start
        FLog.RichTextBox1.SelectionLength = length
        FLog.RichTextBox1.Select(Start, length)
        If Not bPauseScroll Then
            FLog.RichTextBox1.ScrollToCaret()
        End If
        FErrLog.DataGridView1.Focus()
    End Sub

    Private Sub ClearFErrorlog()
        FErrLog.ToolStripComboBox1.Items.Clear()
        FErrLog.DataGridView1.Rows.Clear()
        FErrLog.ButtonError.Text = "0 Errors"
        FErrLog.ButtonWarning.Text = "0 Warnings"
        PED = New ProjectErrorDetail
        Etabs = New List(Of ErrorTabs)
    End Sub

    Private Sub FindLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FindLogToolStripMenuItem.Click
        Form_FindinLog.ShowDialog()
    End Sub

    Private Sub DBToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DBToolStripMenuItem.Click
        Dim f As New Form_DBCorrection
        f.ShowDialog()
    End Sub

#End Region

    Private Sub ExpandAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExpandAllToolStripMenuItem.Click
        FTV.TV.SelectedNode = Nothing
        If FTV.TV.SelectedNode Is Nothing Then
            FTV.TV.ExpandAll()
        Else
            FTV.TV.SelectedNode.ExpandAll()
        End If
    End Sub

    Private Sub CollapseAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CollapseAllToolStripMenuItem.Click
        FTV.TV.SelectedNode = Nothing
        If FTV.TV.SelectedNode Is Nothing Then
            FTV.TV.CollapseAll()
        Else
            If FTV.TV.SelectedNode.Index = 0 Then
                FTV.TV.CollapseAll()
            Else
                FTV.TV.SelectedNode.Collapse()
            End If
        End If
    End Sub

    Private bPauseScroll As Boolean = True

    Private Sub BtnScroller_Click(sender As Object, e As EventArgs) Handles BtnScroller.Click
        If bPauseScroll Then
            bPauseScroll = False
            FLog.RichTextBox1.HideSelection = True
            Me.BtnScroller.Text = "Enable Vertical Scroll"
        Else
            bPauseScroll = True
            FLog.RichTextBox1.HideSelection = False
            Me.BtnScroller.Text = "Disable Vertical Scroll"
        End If
        FLog.RichTextBox1.SelectionStart = FLog.RichTextBox1.Text.Length
        FLog.RichTextBox1.ScrollToCaret()
    End Sub


    Private Sub FTV_Treeview_DeleteAllFilesExceptInputFiles(ByRef Sender As Object, ByRef e As EventArgs) Handles FTV.Treeview_DeleteAllFilesExceptInputFiles
        If MsgBox("This will delete all files except the Input files from the Project!", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Warning Deleting files!") = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim ProjectFolder As String = ActiveProject.ProjectPath
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            FLog.RichTextBox1.AppendText(Now & Chr(9) & "Deleting files, please wait..." & vbCrLf)
            Highlight(FLog.RichTextBox1, "", RtbColor.Black)
            ObjDelProject.DeleteFilesIffound(ProjectFolder, False)
            ' DeleteDirectory(ProjectFolder)
            FLog.RichTextBox1.AppendText(Now & Chr(9) & "All files have been deleted." & vbCrLf)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        Finally
            Me.Cursor = Cursors.Default
            Me.Enabled = True
        End Try

    End Sub

    Private Sub ImportFromBackfromTranslationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportFromBackfromTranslationToolStripMenuItem.Click
        ToolStripStatusLabel3.Text = "busy"
        Dim FD As New Form_UpdateToDB
        FD.ShowDialog()
        ToolStripStatusLabel3.Text = "Idle"
    End Sub

    Private Sub ImportExistingTranslationToDBToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportExistingTranslationToDBToolStripMenuItem.Click
        ActiveProject.bImportExistingtranslationsintoDB = True
        ActiveProject.bCreateXliffWithTranslation = False
        AutoCall()
    End Sub

#Region "Reporting"
    Private Sub ReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportToolStripMenuItem.Click

        Dim counter As Integer = My.Computer.FileSystem.GetFiles(ActiveProject.ProjectPath & "01-Input").Count

        If counter = 0 Then
            MsgBox("No input files available in the directory!" & vbNewLine & ActiveProject.ProjectPath & "01-Input", MsgBoxStyle.Information, "Exiting.")
            FLog.RichTextBox1.AppendText(Now & Chr(9) & "No input files available in the directory!" & vbNewLine & ActiveProject.ProjectPath & "01-Input" & vbCrLf)
            Me.MenuStrip1.Enabled = True
            Me.FTV.TV.Enabled = True
            Exit Sub
        End If

        Dim Report As New CloudStats
        Report.GetReport(ProjectManagement.GetActiveProject)

        Dim RD As New ReportData
        Dim DisplayInform As New DisplayReportInWinForm(RD)

        DisplayInform.UpdateReport(Report)

    End Sub
#End Region


    'Select Lumira file, extract the contents and Load them to Input folder
#Region "Lumira Extraction"
    Private WithEvents objLumira As cls_Lumira

    Private Sub SelectLumsFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectLumsFileToolStripMenuItem.Click

        Dim objFolderDialog As New OpenFileDialog
        objFolderDialog.Title = "Select .lums file"
        objFolderDialog.Filter = "Lums file |*.lums"

        Dim sZipFile As String = ""

        If objFolderDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            sZipFile = objFolderDialog.FileName
        Else
            Exit Sub
        End If

        Try
            objLumira = New cls_Lumira(sZipFile, ActiveProject)
            objLumira.ExtractZip()
            objLumira.CopyLumiraFilesToInputFolder()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
        End Try
    End Sub

    Private Sub objLumira_UpdateMsg(Msg As String) Handles objLumira.UpdateMsg
        UpdateMsg(Msg, RtbColor.Black)
    End Sub

#End Region


#Region "DigitalBoard"

    Private Sub DigitalBoradToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DigitalBoradToolStripMenuItem.Click
        Try

            If Dialog_DigitalBoardvb.ShowDialog = Windows.Forms.DialogResult.OK Then
                ActiveProject.DigitalBoardColorIndex = Dialog_DigitalBoardvb.txtColorIndex.Text
                Execute(True, False)
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
        Finally
        End Try
    End Sub
#End Region

    Private Sub CXCToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CXCToolStripMenuItem1.Click
        Try
            _isCxc = True
            AutoCall()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
        Finally
        End Try
    End Sub

#Region "DBRProperties"
    Private Sub DBRPropertiesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DBRPropertiesToolStripMenuItem.Click
        Try
            Execute(False, True)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "DBRproperties Error!")
        End Try
    End Sub

#End Region

#Region "Xliff Conversion"

    Private Sub XliffToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles XliffToolStripMenuItem.Click

        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        Form_XliffToXlsConverter.ToolStripProgressBar1.Value = Form_XliffToXlsConverter.ToolStripProgressBar1.Minimum
        Form_XliffToXlsConverter.txtInputFilePath.Clear()
        Form_XliffToXlsConverter.txtOutputFilePath.Clear()
        Form_XliffToXlsConverter.rdXliffToXls.Checked = True
        Form_XliffToXlsConverter.ShowDialog()
    End Sub

#End Region

#Region "XlF conversion"
    Private Sub XLFToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles XLFToolStripMenuItem.Click
        If Not CheckProjectSelected() Then
            Exit Sub
        End If

        Form_XLFtoExcel.ToolStripProgressBar1.Value = Form_XLFtoExcel.ToolStripProgressBar1.Minimum
        Form_XLFtoExcel.txtInputFilePath.Clear()
        Form_XLFtoExcel.txtOutputFilePath.Clear()
        Form_XLFtoExcel.rdXliffToXls.Checked = True
        Form_XLFtoExcel.ShowDialog()
    End Sub

    Private Sub ExtractXliffWithTranslationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExtractXliffWithTranslationsToolStripMenuItem.Click
        ActiveProject.bImportExistingtranslationsintoDB = False
        ActiveProject.bCreateXliffWithTranslation = True
        AutoCall()
    End Sub

    Private Sub AnalyzeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AnalyzeToolStripMenuItem.Click
        Dim f As New Form_Analyze
        f.ShowDialog()
    End Sub

    Private Sub ExtractTGZToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExtractTGZToolStripMenuItem.Click
        Dim obj_extractTGZ As Form_TGZSelection = New Form_TGZSelection()
        obj_extractTGZ.InputPath01 = ActiveProject.ProjectPath
        obj_extractTGZ.iStatus = 1
        obj_extractTGZ.ShowDialog()
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        Try
            _isDiBoAutomate = True
            AutoCall()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
        Finally
        End Try
    End Sub

    Private Sub MoveTranslatedFilesToActualFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MoveTranslatedFilesToActualFolderToolStripMenuItem.Click
        Dim obj_CopyOut2Source As Cls_CopyOutFile2Source = New Cls_CopyOutFile2Source()
        obj_CopyOut2Source.str_ProjectPath = ActiveProject.ProjectPath
        obj_CopyOut2Source.CopyOutfiles2SourceFolder()
    End Sub

    Private Sub CreateTarzFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateTarzFileToolStripMenuItem.Click
        Dim obj_createTARZ As Form_CreateTARZ = New Form_CreateTARZ()
        obj_createTARZ.str_Projectpath = ActiveProject.ProjectPath
        obj_createTARZ.ShowDialog()
    End Sub

    Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click
        Dim obj_MDF As New MDF_Export()
        'obj_MDF.InputFolderPath = ActiveProject.ProjectPath
        obj_MDF.ShowDialog()
    End Sub
#End Region
End Class

''' <summary>
''' Used only for Analyze
''' </summary>
Public Class ObjectType

    Public Enum FileType
        Mdfcsv
        Xml_Cpxml
        OtherCsv
        Doc
        Competency
        QuestionLib
        Picklist
        MsgKey
        LMS
        SLC
        HybrisImpex
        HybrisXml
        HybrisProperties
        HybrisHtml
        RmkXhtml
        RmkCategoryJob
        OnboardingOffboarding
        LumiraDocument
        LumiraHeader
        DigitalBoard
        DBRproperties
        Cxc
        UnKnown
    End Enum

    Public Shared fileID() As String
    Public Shared fileTyp() As String

    Public Sub New()
        Try
            load_filetype()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Function load_filetype() As Boolean
        Try
            Dim tmp As String = File.ReadAllText(appData & DefinitionFiles.FileType_List)
            Dim tmp_split() As String = Split(tmp, vbCrLf)
            Dim cnt As Integer = UBound(tmp_split)

            ReDim fileID(cnt)
            ReDim fileTyp(cnt)
            Dim f As Integer = 0
            Dim s() As String

            For Each filetype In tmp_split
                If filetype <> "" Then
                    s = Split(filetype, "|")
                    fileTyp(f) = s(0)
                    fileID(f) = s(1)
                    f = f + 1
                End If
            Next

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error Loading filetype")
        End Try
        Return True
    End Function


    Private Shared Function GetFiletype(ByVal sFile As String) As FileType
        'note: multiple same file types could be created for the same project.
        'user would need to separate with a .
        'E.G. 10.1 we need 2 version 10.1.1.xxx and 10.1.2.xxx
        'the point for the comparison is required to ensure that we don't stop at 10.1, if e.g. we have 10.10

        'If _isCxc Then
        '    If System.IO.Path.GetExtension(sFile) = ".xls" Or System.IO.Path.GetExtension(sFile) = ".xlsx" Or System.IO.Path.GetExtension(sFile) = ".xlsb" Then
        '        Return FileType.Cxc
        '    End If
        'End If

        'If _isDigitalBoard Then
        '    If System.IO.Path.GetExtension(sFile) = ".xls" Or System.IO.Path.GetExtension(sFile) = ".xlsx" Or System.IO.Path.GetExtension(sFile) = ".xlsb" Then
        '        Return FileType.DigitalBoard
        '    End If
        'ElseIf _isDBRProperties Then
        '    Return FileType.DBRproperties
        'End If

        Dim sFileName As String = Path.GetFileNameWithoutExtension(sFile)

        Dim fType As Short = 0

        Dim FileNumber() As String = Split(sFileName, ".")
        Dim FileNumberDefintion() As String
        Dim bFound As Boolean = False

        If System.IO.Path.GetExtension(sFile) = ".docx" Or System.IO.Path.GetExtension(sFile) = ".doc" Then
            Return FileType.Doc
        End If

        For f = 0 To UBound(fileID)
            FileNumberDefintion = Split(fileID(f), ".")
            If UBound(FileNumber) > UBound(FileNumberDefintion) Then
                bFound = MapFileNumberWithFileTypeDefintion(FileNumberDefintion, FileNumber)
            Else
                bFound = MapFileNumberWithFileTypeDefintion(FileNumber, FileNumberDefintion)
            End If
            If bFound Then
                fType = fileTyp(f)
                Exit For
            End If
        Next

        'fType=0 then check if the file belongs to Hybris file or Lumira file
        If fType = 0 Then
            Select Case Microsoft.VisualBasic.Left(sFileName, 3)
                Case 200
                    fType = 200
                Case 201
                    fType = 201
                Case 202
                    fType = 202
                Case 203
                    fType = 203
                Case 300
                    fType = 300
                Case 400
                    fType = 400
                Case 401
                    fType = 401
            End Select
        End If

        'fType=0 then check if the file belongs to doc
        If fType = 0 Then
            If System.IO.Path.GetExtension(sFile).ToLower = ".doc" Or System.IO.Path.GetExtension(sFile).ToLower = ".docx" Then
                fType = 5
            End If
        End If

        Select Case fType
            Case 1
                Return FileType.Mdfcsv
            Case 2, 3
                Return FileType.Xml_Cpxml
            Case 4
                Return FileType.OtherCsv
            Case 5
                Return FileType.Doc
            Case 6
                Return FileType.Picklist
            Case 7
                Return FileType.QuestionLib
            Case 8
                Return FileType.Competency
            Case 9
                Return FileType.MsgKey
            Case 10
                Return FileType.LMS
            Case 11
                Return FileType.OnboardingOffboarding
            Case 100, 101
                Return FileType.SLC
            Case 200
                Return FileType.HybrisImpex
            Case 201
                Return FileType.HybrisProperties
            Case 202
                Return FileType.HybrisXml
            Case 203
                Return FileType.HybrisHtml
            Case 300
                Return FileType.RmkXhtml
            Case 301
                Return FileType.RmkCategoryJob
            Case 400
                Return FileType.LumiraDocument
            Case 401
                Return FileType.LumiraHeader
            Case Else
                Return FileType.UnKnown
        End Select

    End Function


    Public Shared Function GetXliffType(ByVal fileName As String) As Xliff
        Dim str As New ArrayList
        Try
            Dim FType As FileType = GetFiletype(fileName)

            Dim objXliff As Xliff = Nothing

            Select Case FType
                Case FileType.Mdfcsv
                    objXliff = New Mallard_MdfCsv
                Case FileType.Xml_Cpxml
                    objXliff = New MallardXmlCP
                Case FileType.OtherCsv
                    objXliff = New MallardOtherCsv
                Case FileType.Doc
                    objXliff = New MallardDoc
                Case FileType.Competency
                    objXliff = New MallardCompetency
                Case FileType.Picklist
                    objXliff = New MallardPicklist
                Case FileType.QuestionLib
                    objXliff = New MallardQuestionlib
                Case FileType.MsgKey
                    objXliff = New MallardMsgKey
                Case FileType.LMS
                    objXliff = New MallardLMS
                Case FileType.SLC
                    objXliff = New MallardSLC
                Case FileType.HybrisHtml
                    objXliff = New MallardHybrisHtml
                Case FileType.HybrisImpex
                    objXliff = New MallardHybrisImpex
                Case FileType.HybrisProperties
                    objXliff = New MallardHybrisProperties
                Case FileType.HybrisXml
                    objXliff = New MallardHybrisXml
                Case FileType.RmkXhtml
                    objXliff = New MallardRmkXhtml
                Case FileType.RmkCategoryJob
                    objXliff = New MallardRmkCategoryJob
                Case FileType.OnboardingOffboarding
                    objXliff = New MallardOnboardingOffboarding
                Case FileType.LumiraHeader
                    objXliff = New MallardLumiraHeader
                Case FileType.LumiraDocument
                    objXliff = New MallardLumiraDocument
                Case FileType.DigitalBoard
                    objXliff = New MallardDigitalBoard
                Case FileType.DBRproperties
                    objXliff = New MallardHybrisProperties
                Case FileType.Cxc
                    objXliff = New Mallard_CXC
                Case FileType.UnKnown
                    objXliff = Nothing
            End Select

            Return objXliff

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function


End Class