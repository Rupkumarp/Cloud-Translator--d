'Imports System.IO
'Imports System.Environment
'Imports System.ComponentModel
'Imports System.Text
'Imports System.Text.RegularExpressions
'Imports Ionic.Zip

'Public Class Form_Main

'    Private WithEvents ObjDelProject As New DeleteProjects

'    Public Enum FileType
'        Mdfcsv
'        Xml_Cpxml
'        OtherCsv
'        Doc
'        Competency
'        QuestionLib
'        Picklist
'        MsgKey
'        LMS
'        SLC
'        HybrisImpex
'        HybrisXml
'        HybrisProperties
'        HybrisHtml
'        RmkXhtml
'        RmkCategoryJob
'        OnboardingOffboarding
'        UnKnown
'    End Enum

'    Public fileID() As String
'    Public fileTyp() As String
'    'Dim curpath As String

'    Private bNoProject As Boolean = False

'    'Open Current porject folder
'    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click


'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        If Not System.IO.Directory.Exists(ActiveProject.ProjectPath) Then
'            MsgBox("No Projects found!", MsgBoxStyle.Critical, "Cloud translator")
'            Exit Sub
'        End If
'        Diagnostics.Process.Start("explorer.exe", ActiveProject.ProjectPath)
'    End Sub

'    Private Sub Form_Main_Load(sender As Object, e As EventArgs) Handles Me.Load

'        Try
'            'Validate Defintion Files
'            Dim VD As New DefinitionFiles
'            VD.ValidateDefinitionFiles()

'            'Assigns Languages from defintion file to LanguageDefination As LL in ModOperations
'            LangDefintion.GetLanguageList()

'            'Check FileType.Txt exists
'            If Not File.Exists(appData & DefinitionFiles.FileType_List) Then
'                Throw New Exception(appData & DefinitionFiles.FileType_List & " not found!")
'            End If

'            Load_ProjectDetial()

'            'If is_there_project() Then update_statusbar()

'            RichTextBox1.AppendText("Welcome to SFSF translation Manager" & vbCrLf & "Brought to you by the PTLS team" & vbCrLf & "SAP - 2015" & vbCrLf)
'            If Not (load_filetype()) Then End
'        Catch ex As Exception
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
'            End
'        End Try

'    End Sub

'    Sub Load_ProjectDetailAgain()
'        LstProjectGroup = New List(Of ProjectGroup)
'        LstProjectGroup = XMLMethod.GetProjectGroupListFromXml
'    End Sub

'    Sub Load_ProjectDetial()

'        'Load Project list in treeview
'        'Tag = 1 is Root
'        'Tag = 2 is Project Group
'        'Tag = 3 is Project Name

'        TV.Nodes.Clear()

'        LstProjectGroup = New List(Of ProjectGroup)
'        LstProjectGroup = XMLMethod.GetProjectGroupListFromXml

'        If LstProjectGroup.Count = 0 Then
'            ProjectManagement.AddProjectGroupName("Default")
'            ProjectManagement.AddProjectGroupName("Test")
'            Load_ProjectDetial()
'            Exit Sub
'        End If

'        ActiveProject = ProjectManagement.GetActiveProject

'        If Not ActiveProject Is Nothing Then
'            If Microsoft.VisualBasic.Right(ActiveProject.ProjectPath, 1) <> "\" Then
'                ActiveProject.ProjectPath = ActiveProject.ProjectPath & "\"
'            End If
'        End If

'        Dim TT As TreeNodeTag

'        Try
'            Dim TVCounter As Integer = 0
'            If LstProjectGroup.Count = 0 Then
'                bNoProject = True
'                TV.Nodes.Add("No Projects to Load")
'                TV.ImageIndex = 10
'                TV.SelectedImageIndex = 10

'                TT = New TreeNodeTag
'                TT.TI = TreeNodeTag.TagIndex.Root
'                TT.isMaster = False
'                TT.ImageIndex = 10
'                TV.Nodes(0).Tag = TT
'                Exit Sub
'            End If

'            bNoProject = False
'            TT = New TreeNodeTag
'            TT.TI = TreeNodeTag.TagIndex.Root
'            TT.isMaster = False
'            TT.ImageIndex = 10
'            TV.Nodes.Add("(Total " & LstProjectGroup.Count & " Project)")
'            TV.Nodes(0).ImageIndex = TT.ImageIndex
'            TV.Nodes(0).SelectedImageIndex = TT.ImageIndex
'            TV.Nodes(0).BackColor = Color.NavajoWhite
'            TV.Nodes(0).Tag = TT
'            For i As Integer = 0 To (LstProjectGroup.Count - 1)
'                TT = New TreeNodeTag
'                TT.TI = TreeNodeTag.TagIndex.ProjectGroup
'                TT.isMaster = False
'                TT.ImageIndex = 0
'                TV.Nodes(0).Nodes.Add(LstProjectGroup(i).ProjectGroupName)
'                TV.Nodes(0).Nodes(TVCounter).ImageIndex = TT.ImageIndex
'                TV.Nodes(0).Nodes(TVCounter).SelectedImageIndex = TT.ImageIndex
'                TV.Nodes(0).Nodes(TVCounter).Tag = TT
'                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
'                    TT = New TreeNodeTag
'                    TT.TI = TreeNodeTag.TagIndex.ProjectName
'                    TT.isMaster = False
'                    TT.ImageIndex = 13
'                    If LstProjectGroup(i).ProjectDetail(j).isMasterProject Then
'                        TT.isMaster = True
'                        TT.ImageIndex = TreeNodeTag.MasterImageIndex
'                    End If

'                    Dim s As String = LstProjectGroup(i).ProjectDetail(j).ProjectName
'                    TV.Nodes(0).Nodes(TVCounter).Nodes.Add(s)
'                    TV.Nodes(0).Nodes(TVCounter).Nodes(j).ImageIndex = TT.ImageIndex
'                    TV.Nodes(0).Nodes(TVCounter).Nodes(j).SelectedImageIndex = TT.ImageIndex
'                    TV.Nodes(0).Nodes(TVCounter).Nodes(j).Tag = TT

'                    If LstProjectGroup(i).ProjectDetail(j).isCurrentProject Then
'                        TV.Nodes(0).Nodes(TVCounter).Nodes(j).ImageIndex = 12
'                        TV.Nodes(0).Nodes(TVCounter).Nodes(j).SelectedImageIndex = 12
'                        Dim TN As TreeNode = TV.Nodes(0).Nodes(TVCounter).Nodes(j)
'                        TV.SelectedNode = TN
'                    End If
'                Next
'                TVCounter += 1
'            Next i
'        Catch ex As Exception
'            Throw New Exception(ex.Message)
'        End Try
'    End Sub

'    Function load_filetype() As Boolean
'        Try
'            Dim tmp As String = File.ReadAllText(appData & DefinitionFiles.FileType_List)
'            Dim tmp_split() As String = Split(tmp, vbCrLf)
'            Dim cnt As Integer = UBound(tmp_split)
'            RichTextBox1.AppendText(Now & Chr(9) & cnt & " file types loaded" & vbCrLf)

'            ReDim fileID(cnt)
'            ReDim fileTyp(cnt)
'            Dim f As Integer = 0
'            Dim s() As String

'            For Each filetype In tmp_split
'                If filetype <> "" Then
'                    s = Split(filetype, "|")
'                    fileTyp(f) = s(0)
'                    fileID(f) = s(1)
'                    f = f + 1
'                End If
'            Next

'        Catch ex As Exception
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error Loading filetype")
'        End Try
'        Return True
'    End Function

'    Sub update_statusbar()
'        TS_ProjectName.Text = ActiveProject.ProjectName
'        TS_lang.Text = ActiveProject.LangList
'        Me.Refresh()
'    End Sub

'    Sub savemylog()
'        If Not (Directory.Exists(appData & "\log\")) Then Directory.CreateDirectory(appData & "\log\")
'        RichTextBox1.SaveFile(appData & "\log\log." & Format(Now, "yyyy.mm.dd.hh.mm.ss"))
'    End Sub

'    Dim cnt_newtrans As Integer = 0
'    Dim cnt_newintegrated As Integer = 0
'    Dim tr_type As TranslationType

'    Function GetFiletype(ByVal sFile As String) As FileType
'        'note: multiple same file types could be created for the same project.
'        'user would need to separate with a .
'        'E.G. 10.1 we need 2 version 10.1.1.xxx and 10.1.2.xxx
'        'the point for the comparison is required to ensure that we don't stop at 10.1, if e.g. we have 10.10

'        Dim sFileName As String = Path.GetFileNameWithoutExtension(sFile)

'        Dim fType As Short = 0

'        Dim FileNumber() As String = Split(sFileName, ".")
'        Dim FileNumberDefintion() As String
'        Dim bFound As Boolean = False

'        For f = 0 To UBound(fileID)
'            FileNumberDefintion = Split(fileID(f), ".")
'            If UBound(FileNumber) > UBound(FileNumberDefintion) Then
'                bFound = MapFileNumberWithFileTypeDefintion(FileNumberDefintion, FileNumber)
'            Else
'                bFound = MapFileNumberWithFileTypeDefintion(FileNumber, FileNumberDefintion)
'            End If
'            If bFound Then
'                fType = fileTyp(f)
'                Exit For
'            End If
'        Next

'        'fType=0 then check if the file belongs to Hybris file
'        If fType = 0 Then
'            Select Case Microsoft.VisualBasic.Left(sFileName, 3)
'                Case 200
'                    fType = 200
'                Case 201
'                    fType = 201
'                Case 202
'                    fType = 202
'                Case 203
'                    fType = 203
'                Case 300
'                    fType = 300
'            End Select
'        End If

'        'fType=0 then check if the file belongs to doc
'        If fType = 0 Then
'            If System.IO.Path.GetExtension(sFile).ToLower = ".doc" Or System.IO.Path.GetExtension(sFile).ToLower = ".docx" Then
'                fType = 5
'            End If
'        End If

'        Select Case fType
'            Case 1
'                Return FileType.Mdfcsv
'            Case 2, 3
'                Return FileType.Xml_Cpxml
'            Case 4
'                Return FileType.OtherCsv
'            Case 5
'                Return FileType.Doc
'            Case 6
'                Return FileType.Picklist
'            Case 7
'                Return FileType.QuestionLib
'            Case 8
'                Return FileType.Competency
'            Case 9
'                Return FileType.MsgKey
'            Case 10
'                Return FileType.LMS
'            Case 11
'                Return FileType.OnboardingOffboarding
'            Case 100, 101
'                Return FileType.SLC
'            Case 200
'                Return FileType.HybrisImpex
'            Case 201
'                Return FileType.HybrisProperties
'            Case 202
'                Return FileType.HybrisXml
'            Case 203
'                Return FileType.HybrisHtml
'            Case 300
'                Return FileType.RmkXhtml
'            Case 301
'                Return FileType.RmkCategoryJob
'            Case Else
'                Return FileType.UnKnown
'        End Select

'    End Function

'    Public Enum RtbColor
'        Red
'        Green
'        Black
'    End Enum

'    Private Sub Highlight(ByRef Rtb As RichTextBox, ByVal searchstring As String, ByVal mColor As RtbColor)

'        Select Case mColor
'            Case RtbColor.Black
'                Exit Sub
'        End Select

'        Try
'            Dim lastLine As Integer = Rtb.Lines.Count - 2

'            For i As Integer = lastLine To Rtb.Lines.Count - 2
'                Dim Text As String = RichTextBox1.Lines(i)
'                Rtb.Select(Rtb.GetFirstCharIndexFromLine(i), Text.Length)
'                Select Case mColor
'                    Case RtbColor.Red
'                        Rtb.SelectionColor = Color.Red
'                    Case RtbColor.Green
'                        Rtb.SelectionColor = Color.Green
'                End Select
'            Next
'        Catch
'        End Try
'        Rtb.[Select](RichTextBox1.Text.Length, 0)
'        Rtb.SelectionFont = New Font(Rtb.Font, FontStyle.Regular)
'    End Sub

'#Region "MenuBar Events"
'    'MenuBar -> NEW

'    Private WithEvents D1 As Dialog1
'    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
'        NewProject()
'    End Sub

'    Private Sub NewProject()
'        RichTextBox1.AppendText(Now & Chr(9) & "New project definition started." & vbCrLf)

'        D1 = New Dialog1
'        ReDim Preserve D1.ProjectGroupList(LstProjectGroup.Count - 1)
'        For i As Integer = 0 To LstProjectGroup.Count - 1
'            D1.ProjectGroupList(i) = LstProjectGroup(i).ProjectGroupName
'        Next i

'        Dim TT As TreeNodeTag = TV.SelectedNode.Tag
'        my_action = MyAction.Add_NewProject

'        Select Case TT.TI
'            Case TreeNodeTag.TagIndex.ProjectGroup
'                If TV.isMasterInProject(TV.SelectedNode) Then
'                    my_action = MyAction.Load_based_onMaserProject
'                    D1.MasterProjectName = TV.GetMasterProjectName(TV.SelectedNode)
'                End If
'                D1.ProjectGroupName = TV.SelectedNode.Text
'            Case TreeNodeTag.TagIndex.ProjectName
'                If TV.isMasterInProject(TV.SelectedNode.Parent) Then
'                    my_action = MyAction.Load_based_onMaserProject
'                    D1.MasterProjectName = TV.GetMasterProjectName(TV.SelectedNode.Parent)
'                End If
'                D1.ProjectGroupName = TV.SelectedNode.Parent.Text
'        End Select

'        If D1.ShowDialog = Windows.Forms.DialogResult.OK Then
'            update_statusbar()
'        End If

'    End Sub

'    'MenuBar -> EDIT
'    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
'        RichTextBox1.AppendText(Now & Chr(9) & "Project " & ActiveProject.ProjectName & " is being edited" & vbCrLf)
'        my_action = MyAction.Edit_Project
'        Dim FD As New Dialog1
'        FD.ShowDialog()
'        update_statusbar()
'    End Sub

'    'MenuBar -> OPEN
'    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
'        RichTextBox1.AppendText(Now & Chr(9) & "Change active project." & vbCrLf)
'        dia_projectlist.ShowDialog()
'        Load_ProjectDetial()
'        update_statusbar()
'    End Sub

'    'MenuBar -> Exit
'    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
'        savemylog()
'        End
'    End Sub

'    'MenuBar -> CP Form
'    Private Sub CreateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        RichTextBox1.AppendText(Now & Chr(9) & "Create xml C/P file initiated." & vbCrLf)
'        'here open the form for C/P creation
'        Dim objFormCP As New FormCP
'        objFormCP.ActionType = FormCP.Action.Create
'        objFormCP.ShowDialog()
'        objFormCP = Nothing
'    End Sub

'    'MenuBar -> CP Read
'    Private Sub RetrieveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RetrieveToolStripMenuItem.Click


'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        RichTextBox1.AppendText(Now & Chr(9) & "Reading translated xml C/P file for pasting content back initiated." & vbCrLf)
'        Dim objFormCP As New FormCP
'        objFormCP.ActionType = FormCP.Action.Retrieve
'        objFormCP.CurrentDirectory = ActiveProject.ProjectPath
'        objFormCP.ShowDialog()
'        objFormCP = Nothing
'        'here open the form for C/P reading
'    End Sub

'    'MenuBar -> Save Log
'    Private Sub SaveLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveLogToolStripMenuItem.Click
'        RichTextBox1.AppendText(Now & Chr(9) & "Log save initiated." & vbCrLf)
'        savemylog()
'    End Sub

'    'MenuBar -> Load Log
'    Private Sub LoadLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadLogToolStripMenuItem.Click
'        OpenFile.InitialDirectory = appData & "\log\"
'        OpenFile.FileName = ""
'        OpenFile.ShowDialog()

'        If File.Exists(OpenFile.FileName) Then RichTextBox1.Text = File.ReadAllText(OpenFile.FileName)
'        RichTextBox1.AppendText(Now & Chr(9) & OpenFile.FileName & " opened" & vbCrLf)
'    End Sub

'    'MenuBar -> Clear RichTextbox
'    Private Sub ClearLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearLogToolStripMenuItem.Click
'        RichTextBox1.Clear()
'        RichTextBox1.Text = Now & Chr(9) & "Log cleared" & vbCrLf
'    End Sub


'    Private Function CheckProjectSelected() As Boolean
'        If IsNothing(TV.SelectedNode) Then
'            MsgBox("Please Create\Select the project!" & vbNewLine & "Cannot perform this action on root folder.", MsgBoxStyle.Exclamation, "Cannot Run!")
'            Return False
'        End If
'        If Not TV.SelectedNode.Nodes.Count = 0 Then
'            MsgBox("Please Create\Select the project!" & vbNewLine & "Cannot perform this action on root folder.", MsgBoxStyle.Exclamation, "Cannot Run!")
'            Return False
'        End If
'        Return True
'    End Function

'    'MenuBar -> Auto Transaltion
'    Private Sub AutoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutoToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        Try
'            ToolStripStatusLabel1.Text = "busy"
'            Me.MenuStrip1.Enabled = False
'            Application.DoEvents()
'            notFoundDetails = ""
'            notFoundMsg = ""

'            cnt_newintegrated = 0
'            cnt_newtrans = 0

'            updateRtb(vbCrLf, RtbColor.Black)
'            RichTextBox1.AppendText(Now & Chr(9) & "Auto convert files initiated." & vbCrLf)

'            'check if all subfolders are existing
'            If Not (Directory.Exists(ActiveProject.ProjectPath & "01-Input")) Or
'                Not (Directory.Exists(ActiveProject.ProjectPath & "02-TobeTranslated")) Or
'                Not (Directory.Exists(ActiveProject.ProjectPath & "03-Backfromtranslation")) Or
'                Not (Directory.Exists(ActiveProject.ProjectPath & "04-tmpReassemble")) Or
'                Not (Directory.Exists(ActiveProject.ProjectPath & "05-Output")) Then
'                If MsgBox("Incorrect folder structure in the selected project or its been deleted!. Remvoing the Project", MsgBoxStyle.Critical, "Select another project") Then
'                    ProjectManagement.DeleteProjectDetail(ActiveProject.ProjectName)
'                    Load_ProjectDetial()
'                    Me.MenuStrip1.Enabled = True
'                    Exit Sub
'                End If
'            End If


'            Dim CoruptChars As New ArrayList
'            Dim CorruptFileName As New ArrayList
'            Dim obj As CorruptEncoding
'            Dim CorruptChar As StringBuilder

'            If ActiveProject.isCorruptEnabled Then
'                RichTextBox1.AppendText(Now & Chr(9) & "Corruption detection started." & vbCrLf)
'                For Each myFile In Directory.GetFiles(ActiveProject.ProjectPath & "01-Input")
'                    Dim format As String = System.IO.Path.GetExtension(myFile).ToLower
'                    If format = ".csv" Or format = ".xml" Or format = ".properties" Or format = ".impex" Then
'                        obj = New CorruptEncoding(myFile)
'                        CorruptChar = obj.SearchCorruptedChars
'                        If CorruptChar.ToString.Trim <> "" Then
'                            CoruptChars.Add(CorruptChar)
'                            CorruptFileName.Add(myFile)
'                            RichTextBox1.AppendText(Now & Chr(9) & "File Corrupted - " & System.IO.Path.GetFileName(myFile) & " - Number of lines affected " & Regex.Matches(CorruptChar.ToString(), Environment.NewLine).Count / 2 & vbCrLf)
'                            Highlight(RichTextBox1, "File Corrupted - " & System.IO.Path.GetFileName(myFile) & CorruptChar.Length, RtbColor.Red)
'                        End If
'                    End If
'                Next
'                If CoruptChars.Count > 0 Then
'                    Dim objCorruptForm As New Form_CorruptChars
'                    objCorruptForm.CorruptChars = CoruptChars
'                    objCorruptForm.CorruptFileName = CorruptFileName
'                    objCorruptForm.ShowDialog()
'                Else
'                    RichTextBox1.AppendText(Now & Chr(9) & "No corruption detected." & vbCrLf)
'                    Highlight(RichTextBox1, "", RtbColor.Green)
'                End If

'            End If

'            '1.0 first let's convert new files to xliff

'            'check if there are files which are present in 01 input and which are not yet in 02 tobetranslated
'            'detection of files is done based on filename
'            'if files are missing in 02, they are created.

'            BW = New BackgroundWorker
'            Me.BW.WorkerReportsProgress = True
'            If Not BW.IsBusy Then
'                BW.RunWorkerAsync()
'            End If

'        Catch ex As Exception
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
'        End Try

'    End Sub

'    'MenuBar -> Specific File to convert
'    Private Sub SpecifyFileToXliffToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SpecifyFileToXliffToolStripMenuItem.Click
'        Try
'            Dim objOpenFile As New OpenFileDialog
'            objOpenFile.Filter = "csv,xml,doc *.csv,*.xml|*.csv;*.xml;*.doc"

'            If objOpenFile.ShowDialog = Windows.Forms.DialogResult.OK Then
'                RichTextBox1.AppendText(Now & Chr(9) & "Conversion of specific file back from xliff initiated." & vbCrLf)
'                InitiateJob(objOpenFile.FileName, BW)

'                UpdateMsg(vbCrLf & Now & Chr(9) & "Process completed." & vbCrLf, RtbColor.Black)
'                UpdateMsg(cnt_newintegrated & " file(s) have been updated with new translations." & vbCrLf, RtbColor.Black)
'                UpdateMsg(cnt_newtrans & " newly added file(s) ready for translation." & vbCrLf, RtbColor.Black)
'            End If
'        Catch ex As Exception
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
'        End Try
'    End Sub

'    'MenuBar - > Xliff to Xliff
'    Private Sub XliffToXliffToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles XliffToXliffToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        Dim curpath As String = ActiveProject.ProjectPath
'        Dim objXX As New Form_Xliff_to_Xliff
'        objXX.xliff_Folder = curpath & "03-Backfromtranslation"
'        objXX.ShowDialog()
'    End Sub

'#End Region

'#Region "Grouping and Ungrouping files in 02-Translated"
'    'Menubar -> Grouping -> UngroupFiles
'    Private Sub UngroupFilesPerLangBackFromTRToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UngroupFilesPerLangBackFromTRToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        UpdateMsg(vbCrLf & Now & Chr(9) & "Ungrouping files initiated.", RtbColor.Black)
'        Try
'            Dim FolderList As String() = getAllFolders(ActiveProject.ProjectPath & "03-Backfromtranslation\")

'            For i As Integer = 0 To UBound(FolderList)
'                For Each myFile In Directory.GetFiles(FolderList(i))
'                    File.Move(myFile, ActiveProject.ProjectPath & "03-Backfromtranslation\" & Path.GetFileName(myFile))
'                Next
'                System.IO.Directory.Delete(FolderList(i))
'            Next
'            UpdateMsg(vbCrLf & Now & Chr(9) & "Ungrouping files completed", RtbColor.Black)
'        Catch ex As Exception
'            UpdateMsg(vbCrLf & Now & Chr(9) & ex.Message, RtbColor.Red)
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Cloud translator")
'        End Try

'    End Sub

'    'Get Folders and sub folder list to string array
'    Private Function getAllFolders(ByVal directory As String) As String()
'        'Create object
'        Dim fi As New IO.DirectoryInfo(directory)
'        'Array to store paths
'        Dim path() As String = {}
'        'Loop through subfolders
'        For Each subfolder As IO.DirectoryInfo In fi.GetDirectories()
'            'Add this folders name
'            Array.Resize(path, path.Length + 1)
'            path(path.Length - 1) = subfolder.FullName
'            'Recall function with each subdirectory
'            For Each s As String In getAllFolders(subfolder.FullName)
'                Array.Resize(path, path.Length + 1)
'                path(path.Length - 1) = s
'            Next
'        Next
'        Return path
'    End Function

'    'MenuBar -> Group File per language
'    Private Sub GroupFilesPerLangToTRToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GroupFilesPerLangToTRToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        Try
'            UpdateMsg(vbCrLf & Now & Chr(9) & "Grouping files initiated.", RtbColor.Black)

'            Dim curlang() As String = Split(ActiveProject.LangList, ",")

'            If curlang(0) = "" Then
'                MsgBox("Project folder not found!", MsgBoxStyle.Critical, "Cloud translator")
'                Exit Sub
'            End If

'            Dim tmp As String

'            For f = 0 To UBound(curlang)
'                If Not (Directory.Exists(ActiveProject.ProjectPath & "02-TobeTranslated\" & curlang(f) & "\")) Then Directory.CreateDirectory(ActiveProject.ProjectPath & "\02-TobeTranslated\" & curlang(f) & "\")
'            Next


'            For Each myFile In Directory.GetFiles(ActiveProject.ProjectPath & "02-TobeTranslated\")
'                For f = 0 To UBound(curlang)
'                    tmp = Mid(curlang(f), 1, 2) & "_" & Mid(curlang(f), 3, 2)
'                    If InStr(myFile, tmp) <> 0 Then File.Move(myFile, Replace(myFile, "\02-TobeTranslated\", "\02-TobeTranslated\" & curlang(f) & "\"))
'                Next
'            Next

'            UpdateMsg(vbCrLf & Now & Chr(9) & "Grouping files Completed.", RtbColor.Black)
'        Catch ex As Exception
'            UpdateMsg(vbCrLf & Now & Chr(9) & ex.Message, RtbColor.Red)
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Cloud translator")
'        End Try

'    End Sub

'#End Region

'#Region "Manage Picklists"
'    Private Sub ManageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        UpdateMsg(Now & Chr(9) & "Initiated Manage Picklist" & vbCrLf, RtbColor.Black)

'        Dim objManagePicklist As New Form_ManagePicklist
'        objManagePicklist.CurrentDirectory = ActiveProject.ProjectPath
'        objManagePicklist.ShowDialog()
'    End Sub
'#End Region

'#Region "Competency"
'    'Menubar -> Competencies -> Convert Csv to Xml
'    Private Sub ConvertCsvToXmlToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConvertCsvToXmlToolStripMenuItem.Click
'        Dim objCsvToXml As New Form_CsvToXml
'        objCsvToXml.ShowDialog()
'    End Sub

'    'Menubar -> Competencies -> ManageID
'    Private Sub ManageIDsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageIDsToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        Dim objCsvCompGuid As New Form_CsvCompetencyGuid
'        objCsvCompGuid.CurrentDirectory = ActiveProject.ProjectPath
'        objCsvCompGuid.ShowDialog()
'    End Sub

'#Region "Competency Lib - Convert xml to csv competencies lib"
'    Private Sub PushTranslationToMdfCsvToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PushTranslationToMdfCsvToolStripMenuItem.Click


'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If


'        cnt_newintegrated = 0
'        cnt_newtrans = 0

'        Dim objComplib As Complib
'        objComplib = New MallardCompLib

'        If Not System.IO.Directory.Exists(ActiveProject.ProjectPath & "Competencies") Then
'            MsgBox("Directory not found!" & vbCrLf & ActiveProject.ProjectPath & "Competencies", MsgBoxStyle.Critical, "Cloud translator")
'            Exit Sub
'        Else
'            objComplib.myXliff.Competencies_Path = ActiveProject.ProjectPath & "Competencies"
'        End If

'        If Not System.IO.Directory.Exists(objComplib.myXliff.Competencies_Path & "\02–Extracted_xliff\") Then
'            MsgBox("Directory not found!" & vbCrLf & objComplib.myXliff.Competencies_Path & "\02–Extracted_xliff\", MsgBoxStyle.Critical, "Cloud translator")
'            Exit Sub
'        Else
'            objComplib.myXliff.Extractedxliff_Path = objComplib.myXliff.Competencies_Path & "\02–Extracted_xliff\"
'        End If

'        If Not System.IO.Directory.Exists(objComplib.myXliff.Competencies_Path & "\03–Input_mdf_csv\") Then
'            MsgBox("Directory not found!" & vbCrLf & objComplib.myXliff.Competencies_Path & "\03–Input_mdf_csv\", MsgBoxStyle.Critical, "Cloud translator")
'            Exit Sub
'        Else
'            objComplib.myXliff.Inputmdfcsv_Path = objComplib.myXliff.Competencies_Path & "\03–Input_mdf_csv\"
'        End If

'        If Not System.IO.Directory.Exists(objComplib.myXliff.Competencies_Path & "\04–Output_translated_mdf_csv\") Then
'            MsgBox("Directory not found!" & vbCrLf & objComplib.myXliff.Competencies_Path & "\04–Output_translated_mdf_csv\", MsgBoxStyle.Critical, "Cloud translator")
'            Exit Sub
'        Else
'            objComplib.myXliff.Outputtranslatedmdfcsv_Path = objComplib.myXliff.Competencies_Path & "\04–Output_translated_mdf_csv\"
'        End If

'        objComplib.myXliff.Matchfiles_Path = objComplib.myXliff.Competencies_Path & "\07–Match_files\"
'        objComplib.myXliff.Outputcsv_Path = objComplib.myXliff.Competencies_Path & "\08–Output_csv\"
'        objComplib.myXliff.Standardcsv_Path = objComplib.myXliff.Competencies_Path & "\05–Standard_csv\"
'        objComplib.myXliff.Standardxml_Path = objComplib.myXliff.Competencies_Path & "\01-Standard_xml\"
'        objComplib.myXliff.StdcsvwithGUID_Path = objComplib.myXliff.Competencies_Path & "\06–Std_csv_with_GUID\"

'        Try
'            Me.Enabled = False
'            Me.Cursor = Cursors.WaitCursor

'            objComplib.Process()

'            cnt_newintegrated = cnt_newintegrated + objComplib.cnt_newintegrated
'            cnt_newtrans = cnt_newtrans + objComplib.cnt_newtrans

'            UpdateMsg(vbCrLf & Now & Chr(9) & "Process completed." & vbCrLf, RtbColor.Black)
'            UpdateMsg(cnt_newintegrated & " csv file have been updated." & vbCrLf, RtbColor.Black)
'            UpdateMsg(cnt_newtrans & " translated xliff's generated." & vbCrLf, RtbColor.Black)

'        Catch ex As Exception
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
'        Finally
'            Me.Enabled = True
'            Me.Cursor = Cursors.Default
'        End Try

'    End Sub

'#End Region
'#End Region

'#Region "Xliff Conversion"

'    Private Sub XliffToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles XliffToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        Form_XliffToXlsConverter.ToolStripProgressBar1.Value = Form_XliffToXlsConverter.ToolStripProgressBar1.Minimum
'        Form_XliffToXlsConverter.txtInputFilePath.Clear()
'        Form_XliffToXlsConverter.txtOutputFilePath.Clear()
'        Form_XliffToXlsConverter.rdXliffToXls.Checked = True
'        Form_XliffToXlsConverter.ShowDialog()
'    End Sub

'#End Region

'#Region "Corrections/Search & Replace"

'    Private Sub SearchReplaceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchReplaceToolStripMenuItem.Click
'        Form_SearchCorrect.btnSearch.Enabled = True
'        Form_SearchCorrect.cboLanguage.Text = ""
'        Form_SearchCorrect.txtSearchTerm.Clear()
'        Form_SearchCorrect.chkFullStringOnly.Checked = False
'        Form_SearchCorrect.grdSearchResult.DataSource = Nothing
'        Form_SearchCorrect.txtReplaceTerm.Clear()
'        Form_SearchCorrect.RichTextBox1.Clear()
'        Form_SearchCorrect.RichTextBox1.Text = ""
'        Form_SearchCorrect.txtFolderPath.Text = Application.StartupPath & "\tools\Corrections\01 - PTLS\"
'        Form_SearchCorrect.btnReplaceTerm.Enabled = False
'        Form_SearchCorrect.ToolStripStatusLabel1.Text = "Idle..."
'        Form_SearchCorrect.ShowDialog()
'    End Sub

'#End Region

'#Region "Hybris Extract/Import process"

'    Public Enum HybrisRMK
'        RMK
'        Hybris
'    End Enum

'    Private HRmk As HybrisRMK

'    Private Sub ImportRmkHybris()
'        Try
'            Dim zipFile As String = ""
'            Dim opnDialog As New OpenFileDialog

'            If HRmk = HybrisRMK.Hybris Then
'                opnDialog.Filter = "Hybris zip file '*.zip,*.rar|*.zip;*.rar"
'            Else
'                opnDialog.Filter = "RMK zip file '*.zip,*.rar|*.zip;*.rar"
'            End If

'            If opnDialog.ShowDialog <> Windows.Forms.DialogResult.OK Then
'                Exit Sub
'            End If

'            zipFile = opnDialog.FileName

'            'Validation Check -------------------------------------------------------------------------------------------------------------
'            If System.IO.Path.GetExtension(zipFile).ToLower = ".rar" Then
'                If System.IO.File.Exists(My.Settings.winRar) <> True Then
'                    If MessageBox.Show("Winrar could not be found!" & vbNewLine & "Please locate rar.exe or Click on help button to download the exe", "Cloud translator", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0, "http://rarlab.com/download.htm", "Cloud translator") = Windows.Forms.DialogResult.Yes Then
'                        Dim opDialog As New OpenFileDialog
'                        opDialog.Filter = "Winrar 'rar.exe|rar.exe"
'                        If opDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
'                            My.Settings.winRar = opDialog.FileName
'                            My.Settings.Save()
'                        Else
'                            Exit Sub
'                        End If
'                    End If
'                End If
'            End If

'            Dim ProjectFolder As String = ActiveProject.ProjectPath

'            If HRmk = HybrisRMK.Hybris Then
'                ProjectFolder = ProjectFolder & "HybrisRawData\"
'            Else
'                ProjectFolder = ProjectFolder & "RMK_RawData\"
'            End If

'            If Not System.IO.Directory.Exists(ProjectFolder) Then
'                System.IO.Directory.CreateDirectory(ProjectFolder)
'            End If

'            Dim myDir As DirectoryInfo = New DirectoryInfo(ProjectFolder)
'            Dim bExtract As Boolean = False

'            If (myDir.EnumerateFiles().Any()) Or myDir.EnumerateDirectories().Any Then
'                Dim Msg As String
'                If HRmk = HybrisRMK.Hybris Then
'                    Msg = "There are already some files in the Hybris folder!"
'                Else
'                    Msg = "There are already some files in the RMK folder!"
'                End If
'                If MsgBox(Msg & vbNewLine & "Do you want to delete them?", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation) = MsgBoxResult.Yes Then
'                    RichTextBox1.AppendText(vbNewLine & "Deleting files...")
'                    ObjDelProject.DeleteFilesIffound(ProjectFolder)
'                    ObjDelProject.DeleteDirectory(ProjectFolder)
'                    bExtract = True
'                Else
'                    bExtract = False
'                End If
'            Else
'                bExtract = True
'            End If
'            'Validation Check finished-------------------------------------------------------------------------------------------------------------

'            If HRmk = HybrisRMK.Hybris Then
'                Dim objHybris As New Form_HybrisDialog
'                objHybris.zFile = zipFile
'                objHybris.bExtract = bExtract
'                objHybris.bUnzip = True
'                objHybris.ProjectFolder = ProjectFolder
'                'Extract Raw data to HybrisRawData folder
'                objHybris.ShowDialog()
'            Else
'                objRmk = New RMK(zipFile, ProjectFolder, Replace(ProjectFolder, "RMK_RawData", "01-Input"))
'                objRmk.Extract()
'            End If


'        Catch ex As Exception
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Cloud translator!")
'        End Try
'    End Sub

'    Private WithEvents objRmk As RMK
'    Private Sub ImportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        HRmk = HybrisRMK.Hybris
'        ImportRmkHybris()
'    End Sub

'    Private Sub ExtractRawFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExtractRawFilesToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        HRmk = HybrisRMK.RMK
'        ImportRmkHybris()
'    End Sub

'    Private Sub OpenHybrisFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenHybrisFolderToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        HRmk = HybrisRMK.Hybris
'        OpenRMKHybrisFolder()
'    End Sub

'    Private Sub OpenRmkFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenRMKFolderToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        HRmk = HybrisRMK.RMK
'        OpenRMKHybrisFolder()
'    End Sub


'    Private Sub OpenRMKHybrisFolder()
'        Dim ProjectFolder As String = ActiveProject.ProjectPath
'        If HRmk = HybrisRMK.Hybris Then
'            ProjectFolder = ProjectFolder & "HybrisRawData\"
'        Else
'            ProjectFolder = ProjectFolder & "RMK_RawData\"
'        End If

'        If Not System.IO.Directory.Exists(ProjectFolder) Then
'            MsgBox("No Projects found!", MsgBoxStyle.Critical, "Cloud translator")
'            Exit Sub
'        End If
'        Diagnostics.Process.Start("explorer.exe", ProjectFolder)
'    End Sub

'    'Copy files from out folder to zip file path
'    Private Sub ReImportOutFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReImportOutFilesToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        Dim ProjectFolder As String = ActiveProject.ProjectPath & "HybrisRawData\"
'        Dim MappingFile As String = ProjectFolder & "HybrisMapping.xml"

'        Try
'            If ValidateHybrisMappingFile() <> True Then
'                Throw New Exception("Error Validating HybrisMapping.xml file!" & vbCrLf)
'            End If

'            If GetArchiveType() = "rar" Then
'                If System.IO.File.Exists(My.Settings.winRar) <> True Then
'                    If MessageBox.Show("Winrar could not be found!" & vbNewLine & "Please locate rar.exe or Click on help button to download the exe", "Cloud translator", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0, "http://rarlab.com/download.htm", "Cloud translator") = Windows.Forms.DialogResult.Yes Then
'                        Dim opDialog As New OpenFileDialog
'                        opDialog.Filter = "Winrar 'rar.exe|rar.exe"
'                        If opDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
'                            My.Settings.winRar = opDialog.FileName
'                            My.Settings.Save()
'                        Else
'                            Exit Sub
'                        End If
'                    End If
'                End If
'            End If

'            Dim objHybris As New Form_HybrisDialog

'            objHybris.bExtract = False
'            objHybris.bUnzip = False
'            objHybris.ProjectFolder = ProjectFolder

'            'Extract Raw data to HybrisRawData folder
'            objHybris.ShowDialog()

'        Catch ex As Exception
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Cloud translator")
'        End Try

'    End Sub

'#End Region

'    Private Sub DefinitionSettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DefinitionSettingsToolStripMenuItem.Click
'        Dim FD As New Form_Definition
'        FD.ShowDialog()
'        load_filetype()
'    End Sub

'    Public Sub UpdateProgressBar(ByVal Max As Integer, ByVal val As Integer)
'        ToolStripProgressBar1.Maximum = Max
'        ToolStripProgressBar1.Value = val
'    End Sub

'    Private Sub objRmk_UpdateMsg(Msg As String, RTBC As RMK.RtbColor) Handles objRmk.UpdateMsg
'        UpdateMsg(Msg, RTBC)
'    End Sub

'    Private Sub UpdateToDBToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateToDBToolStripMenuItem.Click
'        ToolStripStatusLabel1.Text = "busy"
'        Dim FD As New Form_UpdateToDB
'        FD.ShowDialog()
'        ToolStripStatusLabel1.Text = "Idle"
'    End Sub

'    Public Sub UpdateToolstripStatus(ByVal Msg As String)
'        ToolStripStatusLabel1.Text = Msg
'    End Sub

'    Dim WithEvents BW As BackgroundWorker

'    Private ActiveProject As ProjectDetail

'    Sub InitiateJob(ByVal fileName As String, ByRef BW As BackgroundWorker)
'        Dim str As New ArrayList
'        Try
'            Dim FType As FileType = GetFiletype(fileName)

'            Dim objXliff As Xliff = Nothing

'            Select Case FType
'                Case FileType.Mdfcsv
'                    objXliff = New Mallard_MdfCsv
'                Case FileType.Xml_Cpxml
'                    objXliff = New MallardXmlCP
'                Case FileType.OtherCsv
'                    objXliff = New MallardOtherCsv
'                Case FileType.Doc
'                    objXliff = New MallardDoc
'                Case FileType.Competency
'                    objXliff = New MallardCompetency
'                Case FileType.Picklist
'                    objXliff = New MallardPicklist
'                Case FileType.QuestionLib
'                    objXliff = New MallardQuestionlib
'                Case FileType.MsgKey
'                    objXliff = New MallardMsgKey
'                Case FileType.LMS
'                    objXliff = New MallardLMS
'                Case FileType.SLC
'                    objXliff = New MallardSLC
'                Case FileType.HybrisHtml
'                    objXliff = New MallardHybrisHtml
'                Case FileType.HybrisImpex
'                    objXliff = New MallardHybrisImpex
'                Case FileType.HybrisProperties
'                    objXliff = New MallardHybrisProperties
'                Case FileType.HybrisXml
'                    objXliff = New MallardHybrisXml
'                Case FileType.RmkXhtml
'                    objXliff = New MallardRmkXhtml
'                Case FileType.RmkCategoryJob
'                    objXliff = New MallardRmkCategoryJob
'                Case FileType.OnboardingOffboarding
'                    objXliff = New MallardOnboardingOffboarding
'                Case FileType.UnKnown
'                    str.Add(Now & Chr(9) & "####" & Path.GetFileName(fileName) & "##### - Unknown file type..." & vbCrLf)
'                    str.Add(RtbColor.Red)
'                    BW.ReportProgress(4, str)
'                    Exit Sub
'            End Select

'            Dim curlang() As String = Split(ActiveProject.LangList, ",")

'            For f = 0 To UBound(curlang)
'                curlang(f) = Mid(curlang(f), 1, 2) & "_" & Mid(curlang(f), 3, 2)
'            Next

'            objXliff.myXliff.curlang = curlang
'            objXliff.myXliff.bPretranslate = ActiveProject.isPretranslateEnabled
'            objXliff.myXliff.bCleanTranslation = ActiveProject.isCleanRequired
'            objXliff.myXliff.bisInstance = ActiveProject.isInstanceCheckRequired
'            objXliff.myXliff.bisRestrictCustomer = ActiveProject.isCustomerCheckRequired
'            objXliff.myXliff.bisUploadToDB = ActiveProject.isDBupdateRequired
'            objXliff.myXliff.CustomerName = ActiveProject.CustomerName
'            objXliff.myXliff.Instance = ActiveProject.InstaneName
'            objXliff.myXliff.Folder_TobeTranslated = Replace(fileName, "01-Input-B", "02-TobeTranslated")
'            objXliff.myXliff.Folder_Backfromtranslation = Replace(fileName, "01-Input-B", "03-Backfromtranslation")
'            objXliff.myXliff.Folder_Output = Replace(fileName, "01-Input-B", "05-Output")
'            objXliff.myXliff.Folder_Compare = Replace(fileName, "01-Input-B", "06-Compare")
'            objXliff.myXliff.Folder_Pretranslate = Replace(fileName, "01-Input-B", "07-Pretranslate")

'            '*********************************************Process Starting point**************************************************************
'            str.Add(Now & Chr(9) & "*********************************************File Name - " & Path.GetFileName(fileName) & " Initiated*********************************************" & vbCrLf)
'            str.Add(RtbColor.Black)
'            BW.ReportProgress(4, str)
'            objXliff.Process(fileName, BW)
'            str = New ArrayList
'            str.Add(Now & Chr(9) & "*********************************************File Name - " & Path.GetFileName(fileName) & " Ended*********************************************" & vbCrLf)
'            str.Add(RtbColor.Black)
'            BW.ReportProgress(4, str)
'            cnt_newintegrated = cnt_newintegrated + objXliff.cnt_newintegrated
'            cnt_newtrans = cnt_newtrans + objXliff.cnt_newtrans

'        Catch ex As Exception
'            Throw New Exception(ex.Message)
'            '  UpdateMsg(Now & Chr(9) & "FileName - " & Path.GetFileName(fileName) & " - " & ex.Message & vbCrLf, RtbColor.Red)
'        End Try
'    End Sub

'    Private Sub BW_DoWork(sender As Object, e As DoWorkEventArgs) Handles BW.DoWork

'        Dim fileName As String = ""
'        If Not System.IO.Directory.Exists(ActiveProject.ProjectPath & "01-Input-B\") Then
'            System.IO.Directory.CreateDirectory(ActiveProject.ProjectPath & "01-Input-B\")
'        End If
'        Try
'            For Each myFile In Directory.GetFiles(ActiveProject.ProjectPath & "01-Input")
'                If Not Microsoft.VisualBasic.Left(System.IO.Path.GetFileNameWithoutExtension(myFile), 8).ToLower = "corrupt_" Then
'                    If Not System.IO.File.Exists(ActiveProject.ProjectPath & "01-Input-B\" & System.IO.Path.GetFileName(myFile)) Then
'                        File.Copy(myFile, ActiveProject.ProjectPath & "01-Input-B\" & System.IO.Path.GetFileName(myFile), False)
'                    End If
'                    fileName = ActiveProject.ProjectPath & "01-Input-B\" & System.IO.Path.GetFileName(myFile).ToString
'                    BW.ReportProgress(3, {100, 0})
'                    InitiateJob(fileName, BW)
'                Else
'                    BW.ReportProgress(4, {Now & Chr(9) & "File is Marked as corrupt - " & System.IO.Path.GetFileName(myFile) & vbCrLf, RtbColor.Black})
'                End If
'            Next
'        Catch ex As Exception
'            Dim str As New ArrayList
'            str.Add(Now & Chr(9) & "FileName - " & Path.GetFileName(fileName) & " - " & ex.Message & vbCrLf)
'            str.Add(RtbColor.Red)
'            BW.ReportProgress(4, str)
'            'UpdateMsg(Now & Chr(9) & "FileName - " & Path.GetFileName(fileName) & " - " & ex.Message & vbCrLf, RtbColor.Red)
'        End Try

'    End Sub

'    Private Sub updateRtb(ByVal Msg As String, ByVal mycolor As RtbColor)
'        RichTextBox1.AppendText(Msg)
'        RichTextBox1.SelectAll()
'        Dim BO As New Font("Arial", 8.5, FontStyle.Regular)
'        RichTextBox1.SelectionFont = BO
'        RichTextBox1.Select(RichTextBox1.TextLength - 1, 1)
'        Highlight(RichTextBox1, Msg.Trim, mycolor)
'        RichTextBox1.SelectionStart = RichTextBox1.TextLength
'        RichTextBox1.ScrollToCaret()
'    End Sub

'    Private Sub BW_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BW.ProgressChanged
'        If e.ProgressPercentage = 0 Then
'            ToolStripStatusLabel1.Text = e.UserState.ToString
'        ElseIf e.ProgressPercentage = 3 Then
'            ToolStripProgressBar1.Maximum = CInt(e.UserState(0))
'            ToolStripProgressBar1.Value = CInt(e.UserState(1))
'        ElseIf e.ProgressPercentage = 4 Or e.ProgressPercentage = 2 Or e.ProgressPercentage = 1 Then
'            RichTextBox1.AppendText(e.UserState(0))
'            RichTextBox1.SelectAll()
'            Dim BO As New Font("Arial", 8.5, FontStyle.Regular)
'            RichTextBox1.SelectionFont = BO
'            RichTextBox1.Select(RichTextBox1.TextLength - 1, 1)
'            Highlight(RichTextBox1, e.UserState(0).Trim, e.UserState(1))
'            RichTextBox1.SelectionStart = RichTextBox1.TextLength
'            RichTextBox1.ScrollToCaret()

'        End If
'    End Sub

'    Public Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As RtbColor)
'        RichTextBox1.AppendText(Msg)
'        RichTextBox1.SelectAll()
'        Dim BO As New Font("Arial", 8.5, FontStyle.Regular)
'        RichTextBox1.SelectionFont = BO
'        RichTextBox1.Select(RichTextBox1.TextLength - 1, 1)
'        Highlight(RichTextBox1, Msg.Trim, MyColor)
'        RichTextBox1.SelectionStart = RichTextBox1.TextLength
'        RichTextBox1.ScrollToCaret()
'    End Sub

'    Private Sub BW_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BW.RunWorkerCompleted

'        If Not e.Error Is Nothing Then
'            MsgBox(e.Error.Message.ToString, MsgBoxStyle.Critical, "")
'            Me.RichTextBox1.Text = vbCrLf & e.Error.Message.ToString
'            Exit Sub
'        End If

'        updateRtb(vbCrLf & Now & Chr(9) & "Process completed." & vbCrLf, RtbColor.Black)
'        updateRtb(cnt_newintegrated & " file(s) have been updated with new translations." & vbCrLf, RtbColor.Black)
'        updateRtb(cnt_newtrans & " newly added file(s) ready for translation." & vbCrLf, RtbColor.Black)

'        If notFoundDetails <> "" Then
'            updateRtb(notFoundDetails, RtbColor.Red)
'            updateRtb(vbCrLf & "There are missing translations, Please check the log for more information!" & vbCrLf, RtbColor.Red)
'        End If

'        ToolStripStatusLabel1.Text = "Idle"
'        Me.MenuStrip1.Enabled = True
'    End Sub

'    Private Sub ReimportOutFilesToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ReimportOutFilesToolStripMenuItem1.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        Try
'            Dim curlang() As String = Split(ActiveProject.LangList, ",")
'            Dim projectPath As String = ActiveProject.ProjectPath
'            Dim RmkMapFile As String = projectPath & "RMK_RawData\RmkMapping.xml"
'            For f = 0 To UBound(curlang)
'                curlang(f) = Mid(curlang(f), 1, 2) & "_" & Mid(curlang(f), 3, 2)

'                Dim OutFileFolder As String = projectPath & "05-Output\" & "RmkMono_" & curlang(f)
'                If System.IO.Directory.Exists(OutFileFolder) Then
'                    objRmk = New RMK(projectPath, OutFileFolder, RmkMapFile, curlang(f))
'                    objRmk.ReImport()
'                End If
'            Next
'        Catch ex As Exception
'            Throw New Exception(ex.Message)
'        End Try
'    End Sub

'    Private Sub PerForceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PerForceToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        Dim f As New Form_HanaPretranslate
'        f.ShowDialog()
'    End Sub

'    Private Sub CleaningToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CleaningToolStripMenuItem.Click

'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        Dim f As New Form_Cleaning
'        f.ShowDialog()
'    End Sub



'#Region "Project Management TreeNode events"
'    Private Sub TV_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TV.AfterSelect

'        If Not bNoProject Then
'            TS_ProjectName.Text = "Sample project name"
'            TS_lang.Text = "xx-XX, xx-XX, xx-XX, xx-XX, xx-XX, xx-XX"
'            Dim TT As TreeNodeTag = TV.SelectedNode.Tag

'            Select Case TT.TI
'                Case TreeNodeTag.TagIndex.ProjectGroup
'                    Exit Sub
'                Case TreeNodeTag.TagIndex.Root
'                    Exit Sub
'                Case TreeNodeTag.TagIndex.ProjectName
'                    If TT.isMaster Then
'                        TV.SelectedNode.ImageIndex = TreeNodeTag.MasterImageIndex
'                        TV.SelectedNode.SelectedImageIndex = TreeNodeTag.MasterImageIndex
'                    Else
'                        TV.SelectedNode.ImageIndex = GetProjectImageIndex(True)
'                        TV.SelectedNode.SelectedImageIndex = GetProjectImageIndex(True)
'                    End If
'                    TV.LabelEdit = True
'                    TV.SelectedNode.NodeFont = New Font(TV.Font, FontStyle.Bold)
'                    TV.LabelEdit = False
'            End Select
'            ProjectManagement.MakeActiveProject(e.Node.Text)
'            ActiveProject = ProjectManagement.GetActiveProject
'            If Microsoft.VisualBasic.Right(ActiveProject.ProjectPath, 1) <> "\" Then
'                ActiveProject.ProjectPath = ActiveProject.ProjectPath & "\"
'            End If
'            UpdateMsg(Now & Chr(9) & "Project Selected - " & e.Node.Text & vbCrLf, RtbColor.Black)
'            update_statusbar()
'        End If

'    End Sub

'    Private Sub TV_BeforeSelect(sender As Object, e As TreeViewCancelEventArgs) Handles TV.BeforeSelect

'        If Not bNoProject Then
'            If IsNothing(TV.SelectedNode) Then
'                Exit Sub
'            End If
'            For i As Integer = 0 To TV.Nodes(0).Nodes.Count - 1
'                For j As Integer = 0 To TV.Nodes(0).Nodes(i).Nodes.Count - 1
'                    Dim tt As TreeNodeTag = TV.Nodes(0).Nodes(i).Nodes(j).Tag

'                    If tt.TI = TreeNodeTag.TagIndex.ProjectGroup Or tt.TI = TreeNodeTag.TagIndex.Root Then
'                        Exit Sub
'                    End If

'                    TV.Nodes(0).Nodes(i).Nodes(j).ImageIndex = GetProjectImageIndex(False)
'                    TV.Nodes(0).Nodes(i).Nodes(j).SelectedImageIndex = GetProjectImageIndex(False)

'                    If tt.TI = TreeNodeTag.TagIndex.ProjectName Then
'                        If tt.isMaster Then
'                            TV.Nodes(0).Nodes(i).Nodes(j).ImageIndex = TreeNodeTag.MasterImageIndex
'                            TV.Nodes(0).Nodes(i).Nodes(j).SelectedImageIndex = TreeNodeTag.MasterImageIndex
'                        End If
'                    End If

'                    TV.Nodes(0).Nodes(i).Nodes(j).NodeFont = New Font(TV.Font, FontStyle.Regular)
'                Next
'            Next

'        End If
'    End Sub
'    Private Sub TV_MyNodeSelected(NodeText As String) Handles TV.MyNodeSelected
'        Dim bFound As Boolean = False
'        For i As Integer = 0 To TV.Nodes(0).Nodes.Count - 1
'            For j As Integer = 0 To TV.Nodes(0).Nodes(i).Nodes.Count - 1
'                TV.Nodes(0).Nodes(i).Nodes(j).ImageIndex = GetProjectImageIndex(False)
'                TV.Nodes(0).Nodes(i).Nodes(j).SelectedImageIndex = GetProjectImageIndex(False)

'                If TV.Nodes(0).Nodes(i).Nodes(j).Text.ToLower = NodeText.ToLower Then

'                    Dim TT As TreeNodeTag = TV.Nodes(0).Nodes(i).Nodes(j).Tag
'                    Dim TN As TreeNode = TV.Nodes(0).Nodes(i).Nodes(j)
'                    TV.SelectedNode = TN
'                    If TT.isMaster Then
'                        TV.Nodes(0).Nodes(i).Nodes(j).ImageIndex = TreeNodeTag.MasterImageIndex
'                        TV.Nodes(0).Nodes(i).Nodes(j).SelectedImageIndex = TreeNodeTag.MasterImageIndex
'                    Else
'                        TV.Nodes(0).Nodes(i).Nodes(j).ImageIndex = GetProjectImageIndex(True)
'                        TV.Nodes(0).Nodes(i).Nodes(j).SelectedImageIndex = GetProjectImageIndex(True)
'                    End If

'                    bFound = True
'                    UpdateMsg(Now & Chr(9) & "Project Moved - " & NodeText & vbCrLf, RtbColor.Black)

'                    ' cross_form_functions.edit_project(NodeText, TV.Nodes(0).Nodes(i).Text, TT.isMaster)

'                    ProjectManagement.MoveProjectToAnotherGroup(NodeText, TV.Nodes(0).Nodes(i).Text)

'                    Load_ProjectDetailAgain()
'                    Exit For
'                End If
'            Next
'            If bFound Then
'                Exit For
'            End If
'        Next
'    End Sub


'    Private Sub CsvToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CsvToolStripMenuItem.Click
'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If
'    End Sub

'    Private Sub TV_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TV.NodeMouseClick
'        bCancelContextMenuStrip = False
'        If e.Button = Windows.Forms.MouseButtons.Right Then
'            TV.SelectedNode = e.Node
'            bCancelContextMenuStrip = False
'        End If
'    End Sub
'    Private bCancelContextMenuStrip As Boolean = False
'    Private Sub TV_MouseUp(sender As Object, e As MouseEventArgs) Handles TV.MouseUp
'        bCancelContextMenuStrip = False
'        If e.Button = MouseButtons.Right Then
'            ' Point where mouse is clicked
'            Dim p As Point = New Point(e.X, e.Y)
'            ' Go to the node that the user clicked
'            Dim node As TreeNode = TV.GetNodeAt(p)

'            If Not node Is Nothing Then
'                Dim TT As TreeNodeTag = node.Tag

'                Select Case TT.TI
'                    Case TreeNodeTag.TagIndex.ProjectGroup
'                        ContextMenu_ProjectGroup.Show(TV, New Point(e.X, e.Y))
'                        Exit Sub

'                    Case TreeNodeTag.TagIndex.Root
'                        ContextMenu_Root.Show(TV, New Point(e.X, e.Y))
'                        bCancelContextMenuStrip = True
'                        Exit Sub

'                    Case TreeNodeTag.TagIndex.ProjectName

'                        ContextMenu_ProjectName.Show(TV, New Point(e.X, e.Y))
'                        bCancelContextMenuStrip = True
'                        Exit Sub
'                End Select

'            Else
'                ContextMenu_Root.Close()
'                ContextMenu_ProjectName.Close()
'            End If
'        End If
'    End Sub

'    Private Sub DeleteProjectToolStripMenuItem_Click(sender As Object, e As EventArgs)
'        MsgBox("Not yet implemented", MsgBoxStyle.Exclamation, "Pending")
'    End Sub

'    Private WithEvents objCTP As CTP

'    Private Sub ExportCTPFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportCTPFileToolStripMenuItem.Click

'        MsgBox("Please select the folder where you want to save the CTP file", MsgBoxStyle.Information, "Cloud TR")
'        Dim FD As New FolderBrowserDialog
'        FD.Description = "Select folder!"
'        If FD.ShowDialog = Windows.Forms.DialogResult.Cancel Then
'            Exit Sub
'        End If

'        Me.Enabled = False
'        Me.Cursor = Cursors.WaitCursor
'        Try
'            objCTP = New CTP
'            objCTP.ExportCTP(TV.SelectedNode.Text, FD.SelectedPath)
'        Catch ex As Exception
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
'        End Try

'    End Sub

'    Private Sub objCTP_UpdateForm(EnableForm As Boolean) Handles objCTP.UpdateForm
'        Load_ProjectDetial()
'        update_statusbar()
'        Me.Enabled = True
'        Me.Cursor = Cursors.Default
'        NAR(objCTP)
'    End Sub

'    Private Sub objCTP_UpdateMsg(Msg As String) Handles objCTP.UpdateMsg
'        UpdateMsg(Msg, RtbColor.Black)
'    End Sub

'    Private Sub ImportCTPProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportCTPProjectToolStripMenuItem.Click
'        Dim f As New Form_ImportCTP
'        f.ShowDialog()
'    End Sub

'    Private Sub EditProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditProjectToolStripMenuItem.Click
'        If Not CheckProjectSelected() Then
'            Exit Sub
'        End If

'        RichTextBox1.AppendText(Now & Chr(9) & "Project " & ActiveProject.ProjectName & " is being edited" & vbCrLf)
'        my_action = MyAction.Edit_Project
'        Dim FD As New Dialog1
'        FD.ShowDialog()
'        update_statusbar()
'    End Sub

'    Private Sub ExpandAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExpandAllToolStripMenuItem.Click
'        TV.SelectedNode = Nothing
'        If TV.SelectedNode Is Nothing Then
'            TV.ExpandAll()
'        Else
'            TV.SelectedNode.ExpandAll()
'        End If
'    End Sub

'    Private Sub CollapseAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CollapseAllToolStripMenuItem.Click
'        TV.SelectedNode = Nothing
'        If TV.SelectedNode Is Nothing Then
'            TV.CollapseAll()
'        Else
'            If TV.SelectedNode.Index = 0 Then
'                TV.CollapseAll()
'            Else
'                TV.SelectedNode.Collapse()
'            End If
'        End If
'    End Sub

'    Private Sub NewProjectGroupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewProjectGroupToolStripMenuItem.Click
'        Dim f As New Form_ProjectGroup
'        If f.ShowDialog = Windows.Forms.DialogResult.OK Then
'            Load_ProjectDetial()
'        End If
'    End Sub

'    Private Sub SetAsMasterProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetAsMasterProjectToolStripMenuItem.Click
'        If TV.isMasterAlready(TV.SelectedNode.Parent, TV.SelectedNode.Text) Then
'            Exit Sub
'        End If
'        If TV.isMasterInProject(TV.SelectedNode.Parent) Then
'            If MsgBox("This Projectgroup already has a master!" & vbNewLine & "Are you sure you want to set the selected one as Master?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "?") = MsgBoxResult.No Then
'                Exit Sub
'            End If
'        End If
'        SetMasterProject()
'    End Sub


'    Public Sub SetMasterProject()

'        Dim TT As TreeNodeTag
'        For i As Integer = 0 To TV.SelectedNode.Parent.Nodes.Count - 1
'            TT = TV.SelectedNode.Parent.Nodes(i).Tag
'            TT.isMaster = False
'            TV.SelectedNode.Parent.Nodes(i).Tag = TT
'            TV.SelectedNode.Parent.Nodes(i).SelectedImageIndex = GetProjectImageIndex(False)
'            TV.SelectedNode.Parent.Nodes(i).ImageIndex = GetProjectImageIndex(False)
'            'cross_form_functions.edit_project(TV.SelectedNode.Parent.Nodes(i).Text, TV.SelectedNode.Parent.Text, TT.isMaster)
'            ProjectManagement.SetMasterProject(TV.SelectedNode.Parent.Text, TV.SelectedNode.Parent.Nodes(i).Text)
'        Next

'        TT = TV.SelectedNode.Tag
'        TT.isMaster = True
'        TV.SelectedNode.SelectedImageIndex = TreeNodeTag.MasterImageIndex
'        TV.SelectedNode.ImageIndex = TreeNodeTag.MasterImageIndex
'        TV.SelectedNode.Tag = TT
'        ProjectManagement.SetMasterProject(TV.SelectedNode.Parent.Text, TV.SelectedNode.Text)
'        ' cross_form_functions.edit_project(TV.SelectedNode.Text, TV.SelectedNode.Parent.Text, TT.isMaster)
'        Load_ProjectDetailAgain()
'    End Sub

'    Private Sub TV_SetMaster() Handles TV.SetMaster
'        SetMasterProject()
'    End Sub


'    Private Sub ExistingProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExistingProjectToolStripMenuItem.Click
'        Try
'            Dim f As New Form_ImportCTP
'            f.ProjectGroupName = TV.SelectedNode.Text
'            If f.ShowDialog = Windows.Forms.DialogResult.OK Then
'                Load_ProjectDetial()
'            End If
'        Catch ex As Exception
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
'        End Try


'    End Sub

'    Private Sub DeleteFilesOnlyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteFilesOnlyToolStripMenuItem.Click

'        If MsgBox("This will delete all the Input files,xliff files and Out files!" & vbNewLine & "Are you sure you want to delete those files?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Delete files") = MsgBoxResult.No Then
'            Exit Sub
'        End If

'        Dim ProjectFolder As String = ActiveProject.ProjectPath
'        Try
'            Me.Cursor = Cursors.WaitCursor
'            Me.Enabled = False
'            RichTextBox1.AppendText(Now & Chr(9) & "Deleting files, please wait..." & vbCrLf)
'            Highlight(RichTextBox1, "", RtbColor.Black)
'            ObjDelProject.DeleteFilesIffound(ProjectFolder)
'            ' DeleteDirectory(ProjectFolder)
'            RichTextBox1.AppendText(Now & Chr(9) & "All files have been deleted." & vbCrLf)
'        Catch ex As Exception
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
'        Finally
'            Me.Cursor = Cursors.Default
'            Me.Enabled = True
'        End Try
'    End Sub

'    Private Sub NewProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewProjectToolStripMenuItem.Click
'        NewProject()
'    End Sub

'    Private Sub D1_SetMaster() Handles D1.SetMaster
'        SetMasterProject()
'    End Sub

'    Private Sub EditProjectGroupPropertiesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditProjectGroupPropertiesToolStripMenuItem.Click
'        Dim f As New Form_EditProjectGroup
'        f.ProjectGroupName = TV.SelectedNode.Text
'        If f.ShowDialog = Windows.Forms.DialogResult.OK Then
'            Load_ProjectDetial()
'        End If
'    End Sub

'    Private Sub EditProjectGroupToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles EditProjectGroupToolStripMenuItem1.Click
'        Dim f As New Form_EditProjectGroup
'        If f.ShowDialog = Windows.Forms.DialogResult.OK Then
'            Load_ProjectDetial()
'        End If
'    End Sub

'    Private Sub DelteProjectGroupOnlyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DelteProjectGroupOnlyToolStripMenuItem.Click
'        Try
'            Dim ProjectGroupName As String = TV.SelectedNode.Text

'            If ProjectManagement.CompareTwoStrings(ProjectGroupName, "Default") Then
'                MsgBox("Default Group cannot be Deleted!", MsgBoxStyle.Critical, "Permission Denied")
'                Exit Sub
'            End If

'            If MsgBox("This action will delete the Project-GroupName from the list." & vbNewLine & "But the Project will be moved to 'Default' group." & vbNewLine & "Are you sure you want to continue?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Delete ProjectGroupName") = MsgBoxResult.No Then
'                Exit Sub
'            End If

'            ProjectManagement.DeleteProjectGroupWithoutPurge(ProjectGroupName)
'            Load_ProjectDetial()
'        Catch ex As Exception
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
'        End Try

'    End Sub

'    Private Sub DeleteFilesAndProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteFilesAndProjectToolStripMenuItem.Click
'        If MsgBox("This will delete the Project!", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Warning Deleting Project") = MsgBoxResult.No Then
'            Exit Sub
'        End If

'        Dim ProjectFolder As String = ProjectManagement.GetActiveProject.ProjectPath '  GetProjectLocation(TV.SelectedNode.Text)
'        Try
'            Me.Cursor = Cursors.WaitCursor
'            Me.Enabled = False
'            RichTextBox1.AppendText(Now & Chr(9) & "Deleting files, please wait..." & vbCrLf)
'            Highlight(RichTextBox1, "", RtbColor.Black)
'            ObjDelProject.DeleteFilesIffound(ProjectFolder)
'            ObjDelProject.DeleteDirectory(ProjectFolder)
'            RichTextBox1.AppendText(Now & Chr(9) & "All files have been deleted." & vbCrLf)
'            ProjectManagement.DeleteProject(TV.SelectedNode.Text)
'            Load_ProjectDetial()
'        Catch ex As Exception
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
'        Finally
'            Me.Cursor = Cursors.Default
'            Me.Enabled = True
'        End Try
'    End Sub

'    Private Sub DeleteProjectGroupAndProjectsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteProjectGroupAndProjectsToolStripMenuItem.Click

'        If ProjectManagement.CompareTwoStrings(TV.SelectedNode.Text, "Default") Then
'            MsgBox("Default Group cannot be Deleted!", MsgBoxStyle.Critical, "Permission Denied")
'            Exit Sub
'        End If

'        If MsgBox("This will delete the Project in this group!" & vbNewLine & "Are you sure you want to Continue?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Warning Deleting Project") = MsgBoxResult.No Then
'            Exit Sub
'        End If

'        Try
'            Me.Cursor = Cursors.WaitCursor
'            Me.Enabled = False
'            RichTextBox1.AppendText(Now & Chr(9) & "Deleting Project Group - " & TV.SelectedNode.Text & ", please wait..." & vbCrLf)
'            Highlight(RichTextBox1, "", RtbColor.Black)


'            Dim ProjectList As ArrayList = ProjectManagement.GetProjectNameList(TV.SelectedNode.Text)

'            For i As Integer = 0 To ProjectList.Count - 1
'                Dim ProjectFolder As String = ProjectManagement.GetProjectDetail(ProjectList(i)).ProjectPath
'                ObjDelProject.DeleteFilesIffound(ProjectFolder)
'                ObjDelProject.DeleteDirectory(ProjectFolder)
'            Next

'            RichTextBox1.AppendText(Now & Chr(9) & "All files have been deleted." & vbCrLf)
'            ProjectManagement.DeleteProjectGroupWithPurge(TV.SelectedNode.Text)
'            Load_ProjectDetial()
'        Catch ex As Exception
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
'        Finally
'            Me.Cursor = Cursors.Default
'            Me.Enabled = True
'        End Try

'    End Sub

'    Private Sub RefreshToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RefreshToolStripMenuItem.Click
'        Load_ProjectDetial()
'    End Sub

'    Private Sub RefeshToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RefeshToolStripMenuItem.Click
'        Load_ProjectDetial()
'        RichTextBox1.Clear()
'        RichTextBox1.Text = Now & Chr(9) & "Log cleared" & vbCrLf
'    End Sub

'    Private Sub ExportProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportProjectToolStripMenuItem.Click
'        MsgBox("Please select the folder where you want to save the .ctproj file", MsgBoxStyle.Information, "Cloud TR")
'        Dim FD As New FolderBrowserDialog
'        FD.Description = "Select folder!"
'        If FD.ShowDialog = Windows.Forms.DialogResult.Cancel Then
'            Exit Sub
'        End If

'        Me.Enabled = False
'        Me.Cursor = Cursors.WaitCursor
'        Try
'            objCTP = New CTP
'            objCTP.ExportCtproj(TV.SelectedNode.Text, FD.SelectedPath)
'        Catch ex As Exception
'            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
'        End Try
'    End Sub

'    Private WithEvents objCtproj As CTP
'    Private Sub EditProjectGroupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditProjectGroupToolStripMenuItem.Click
'        Dim f As New Form_ImportCtproj
'        f.ShowDialog()
'        Load_ProjectDetial()
'    End Sub

'    Private Sub ObjDelProject_UpdateMsg(Msg As String) Handles ObjDelProject.UpdateMsg
'        UpdateMsg(Msg, RtbColor.Black)
'    End Sub

'#End Region


'End Class


