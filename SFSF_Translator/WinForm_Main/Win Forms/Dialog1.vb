Imports System.Windows.Forms
Imports System.Text
Imports System.IO

Public Class Dialog1

    Dim bNewCustomer As Boolean
    Dim bNewInstance As Boolean

    Dim OldProjectName As String = ""
    Dim OldProjectGroupName As String = ""

    Public Property ProjectGroupName As String

    Function IsValidFileNameOrPath(ByVal name As String) As Boolean
        ' Determines if the name is Nothing. 
        If name Is Nothing Then
            Return False
        End If

        ' Determines if there are bad characters in the name. 
        For Each badChar As Char In System.IO.Path.GetInvalidPathChars
            If InStr(name, badChar) > 0 Then
                Return False
            End If
        Next

        ' The name passes basic validation. 
        Return True
    End Function

    Public Event SetMaster()

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        'some checks
        If TextBox_ProjectName.Text = "" Then MsgBox("Project Name cannot be empty", MsgBoxStyle.Critical) : Exit Sub
        If Not IsValidFileNameOrPath(TextBox_ProjectPath.Text) Then
            MsgBox("Invalid Project Path!", MsgBoxStyle.Critical) : Exit Sub
        End If

        If (my_action = MyAction.Add_NewProject Or my_action = MyAction.Load_based_onMaserProject) And ProjectManagement.isProjectNameAvailable(TextBox_ProjectName.Text) Then MsgBox("Project already exists. Please use different project name", MsgBoxStyle.Critical) : Exit Sub
        If my_action = MyAction.Edit_Project And Not (System.IO.Directory.Exists(TextBox_ProjectPath.Text)) Then MsgBox("Invalid Project Path", MsgBoxStyle.Critical) : Exit Sub
        If ListBox1.CheckedItems.Count = 0 Then MsgBox("At least one language should be selected", MsgBoxStyle.Critical) : Exit Sub
        If CmbCustomer.SelectedIndex <= 0 Then MsgBox("Customer Not seleted", MsgBoxStyle.Critical) : Exit Sub
        If CmbInstance.SelectedIndex <= 0 Then MsgBox("Instance Not seleted", MsgBoxStyle.Critical) : Exit Sub

        'prepare lang list
        Dim mylanguage As String = ""
        For Each myitem In ListBox1.CheckedItems
            mylanguage = mylanguage & lang_to_langcode(myitem.ToString()) & ","
        Next

        'now add to project list\
        If Microsoft.VisualBasic.Right(TextBox_ProjectPath.Text, 1) <> "\" Then
            TextBox_ProjectPath.Text = TextBox_ProjectPath.Text & "\"
        End If

        sFolderPath = TextBox_ProjectPath.Text & TextBox_ProjectName.Text & "\"

        Dim bSetMaster As Boolean = False
        Dim PD As New ProjectDetail
        With PD
            .ProjectName = TextBox_ProjectName.Text
            .ProjectPath = sFolderPath
            .LangList = Mid(mylanguage, 1, Len(mylanguage) - 1)
            .ProjectDescription = Replace(TextBox_Description.Text, vbCrLf, "@CrLf@")
            .isCleanRequired = ChkCleanTranslation.CheckState
            .CustomerName = CmbCustomer.Text
            .InstaneName = CmbInstance.Text
            .isCorruptEnabled = ChkCorrupt.CheckState
            .isCurrentProject = True
            .isCustomerCheckRequired = ChkRestrictCustomer.CheckState
            .isPretranslateEnabled = ChkPretranslate.CheckState
            .isDBupdateRequired = ChkUploadToDB.CheckState
            .isInstanceCheckRequired = ChkInstance.CheckState
            .isMasterProject = ChkMaster.CheckState
            .isMaxLengthCheckRequired = ChkMaxLength.CheckState
            If ChkMaster.CheckState = CheckState.Checked Then
                If ProjectManagement.isMasterProjectInGroup(CmbProjectGroup.Text) Then
                    If MsgBox("Master Project already set in thies ProjectGroup!" & vbNewLine & "Are you sure you want to set this Project as Master?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Set Master") = MsgBoxResult.Yes Then
                        ProjectManagement.SetMasterProject(CmbProjectGroup.Text, TextBox_ProjectName.Text)
                    Else
                        .isMasterProject = False
                    End If
                End If
            End If

            If CmbProjectGroup.Text.Trim.Length = 0 Then
                CmbProjectGroup.Text = "Default"
            End If
            .ProjectGroupName = CmbProjectGroup.Text
        End With

        If my_action = MyAction.Add_NewProject Or my_action = MyAction.Load_based_onMaserProject Then
            ProjectManagement.AddNewProject(PD)
            ProjectManagement.SetActiveProject(TextBox_ProjectName.Text)
        ElseIf my_action = MyAction.Edit_Project Then

            ''''''Renaming Folder if changed''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim bRenameFolder As Boolean = False
            Dim ProjectLocation As String = TextBox_ProjectPath.Text
            If System.IO.Directory.Exists(TextBox_ProjectPath.Text & OldProjectName) Then
                If OldProjectName.ToLower.Trim <> TextBox_ProjectName.Text.ToLower.Trim Then
                    'Rename the folder
                    My.Computer.FileSystem.RenameDirectory(TextBox_ProjectPath.Text & OldProjectName, TextBox_ProjectName.Text)
                    bRenameFolder = True
                    ProjectManagement.RenameProjectName(OldProjectName, TextBox_ProjectName.Text)
                End If

            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If OldProjectGroupName.ToLower.Trim <> PD.ProjectGroupName.ToLower.Trim Then
                ProjectManagement.MoveProjectToAnotherGroup(PD.ProjectName, PD.ProjectGroupName)
            End If
            ProjectManagement.SetActiveProject(PD.ProjectName)
            ProjectManagement.UpdateProject(PD)

        End If

        XMLMethod.SaveProjectGroupList() 'Save xml

        'if new, create project folder structure
        If my_action = MyAction.Add_NewProject Or my_action = MyAction.Load_based_onMaserProject Then
            If Not (System.IO.Directory.Exists(sFolderPath)) Then System.IO.Directory.CreateDirectory(sFolderPath) : Form_MainNew.UpdateMsg("... Base project folder created..." & vbCrLf, Form_MainNew.RtbColor.Black)
            If Not (System.IO.Directory.Exists(sFolderPath & CloudProjectsettings.Folder_Input)) Then System.IO.Directory.CreateDirectory(sFolderPath & CloudProjectsettings.Folder_Input) : Form_MainNew.UpdateMsg("... \01-Input\ subfolder created..." & vbCrLf, Form_MainNew.RtbColor.Black)
            If Not (System.IO.Directory.Exists(sFolderPath & CloudProjectsettings.Folder_InputB)) Then System.IO.Directory.CreateDirectory(sFolderPath & CloudProjectsettings.Folder_InputB) : Form_MainNew.UpdateMsg("... \01-Input-B\ subfolder created..." & vbCrLf, Form_MainNew.RtbColor.Black)
            If Not (System.IO.Directory.Exists(sFolderPath & CloudProjectsettings.Folder_TobeTransalted)) Then System.IO.Directory.CreateDirectory(sFolderPath & CloudProjectsettings.Folder_TobeTransalted) : Form_MainNew.UpdateMsg("... \02-TobeTranslated\ subfolder created..." & vbCrLf, Form_MainNew.RtbColor.Black)
            If Not (System.IO.Directory.Exists(sFolderPath & CloudProjectsettings.Folder_BackFromTranslation)) Then System.IO.Directory.CreateDirectory(sFolderPath & CloudProjectsettings.Folder_BackFromTranslation) : Form_MainNew.UpdateMsg("... \03-Backfromtranslation\ subfolder created..." & vbCrLf, Form_MainNew.RtbColor.Black)
            If Not (System.IO.Directory.Exists(sFolderPath & CloudProjectsettings.Folder_TempReassmble)) Then System.IO.Directory.CreateDirectory(sFolderPath & CloudProjectsettings.Folder_TempReassmble) : Form_MainNew.UpdateMsg("... \04-tmpReassemble\ subfolder created..." & vbCrLf, Form_MainNew.RtbColor.Black)
            If Not (System.IO.Directory.Exists(sFolderPath & CloudProjectsettings.Folder_OutPut)) Then System.IO.Directory.CreateDirectory(sFolderPath & CloudProjectsettings.Folder_OutPut) : Form_MainNew.UpdateMsg("... \05-Output\ subfolder created..." & vbCrLf, Form_MainNew.RtbColor.Black)
            If Not (System.IO.Directory.Exists(sFolderPath & CloudProjectsettings.Folder_Compare)) Then System.IO.Directory.CreateDirectory(sFolderPath & CloudProjectsettings.Folder_Compare) : Form_MainNew.UpdateMsg("... \06-Compare\ subfolder created..." & vbCrLf, Form_MainNew.RtbColor.Black)
            If Not (System.IO.Directory.Exists(sFolderPath & CloudProjectsettings.Folder_Pretranslate)) Then System.IO.Directory.CreateDirectory(sFolderPath & CloudProjectsettings.Folder_Pretranslate) : Form_MainNew.UpdateMsg("... \07-Pretranslate\ subfolder created..." & vbCrLf, Form_MainNew.RtbColor.Black)
            If Not (System.IO.Directory.Exists(sFolderPath & CloudProjectsettings.Folder_ExistingTranslation)) Then System.IO.Directory.CreateDirectory(sFolderPath & CloudProjectsettings.Folder_ExistingTranslation) : Form_MainNew.UpdateMsg("... \09-ExistingTranslation\ subfolder created..." & vbCrLf, Form_MainNew.RtbColor.Black)

            'Competencies
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_Competencies)
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_CompetenciesStandardXml)
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_CompetenciesExtractedXliff)
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_CompetenciesInputMdfCsv)
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_CompetenciesOutputTranslatedMdfCsv)
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_CompetenciesStandardCsv)
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_CompetenciesStdCsvWithGuid)
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_CompetenciesMatchFiles)
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_CompetenciesOutputCSV)
            'Picklists
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_Picklists)
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_PicklistsStandard)
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_PicklistsExtractedXliff)
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_PicklistsInput)
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_PicklistsOutput)
            'Corrections
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_Corrections)
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_CorrectionsPtls)
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_CorrectionsStandard)
            'Lumira
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_Lumira)
            'Hybris
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_Hybris)
            'RMK
            CreateProjectFolders(sFolderPath, CloudProjectsettings.Folder_Rmk)

        End If

        UpdateExculsionList(sFolderPath)

        AddNewProjectGroupName()

        Form_MainNew.RefreshTreeview()

        Me.DialogResult = System.Windows.Forms.DialogResult.OK

        If bSetMaster Then
            RaiseEvent SetMaster()
        End If
        Me.Close()
    End Sub

    Private Sub UpdateExculsionList(ByVal ProjectLocation As String)
        Try

            ProjectLocation = sFolderPath

            Using writer As StreamWriter = New StreamWriter(ProjectLocation & "DbExclusion.config", False)
                writer.WriteLine(TextBox4.Text)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Private Sub GetExclusionList()
        Dim ExlFile As String = ""
        Try
            ExlFile = ProjectManagement.GetActiveProject.ProjectPath & "\DbExclusion.config"
            If Not System.IO.File.Exists(ExlFile) Then
                Exit Sub
            End If
            Using Reader As StreamReader = New StreamReader(ExlFile, True)
                TextBox4.Text = Reader.ReadToEnd
            End Using
        Catch ex As Exception
            'No exclusing file
        End Try

    End Sub

    Private Sub CreateProjectFolders(ByVal sFolderPath As String, ByVal sPath As String)
        If Not (System.IO.Directory.Exists(sFolderPath & sPath)) Then
            System.IO.Directory.CreateDirectory(sFolderPath & sPath)
            Form_MainNew.UpdateMsg("... " & sPath & " subfolder created..." & vbCrLf, Form_MainNew.RtbColor.Black)
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Dim sFolderPath As String

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If Folder1.ShowDialog = Windows.Forms.DialogResult.OK Then
            sFolderPath = Folder1.SelectedPath '& "\"
            TextBox_ProjectPath.Text = Folder1.SelectedPath ' & "\Project1\"
        End If

    End Sub

    Sub PopulateInstace(ByVal Customername As String)
        Try
            CmbInstance.Items.Clear()
            CmbInstance.Items.Add("[Add New Instance]")
            Dim Instancearray As New ArrayList

            If Not IsNothing(CI.CustomerInstance) Then
                For i As Integer = 0 To CI.CustomerInstance.Count - 1
                    If Not IsNothing(CI.CustomerInstance(i)) Then
                        If CI.CustomerInstance(i).ToString.ToLower = Customername.ToLower Then
                            If Not Instancearray.Contains(CI.InstanceName(i)) Then
                                Instancearray.Add(CI.InstanceName(i))
                            End If
                        End If
                    Else
                        CI.CustomerInstance.RemoveAt(i)
                        CI.InstanceName.RemoveAt(i)
                    End If

                Next

                For i As Integer = 0 To Instancearray.Count - 1
                    CmbInstance.Items.Add(Instancearray(i))
                Next
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Public Structure CustomerInstance
        Public CustomerInstance As ArrayList
        Public CustomerName As ArrayList
        Public InstanceName As ArrayList
    End Structure

    Dim CI As New CustomerInstance

    Private Sub setCustomerInstance()
        Try
            CmbCustomer.Items.Clear()
            CmbCustomer.Items.Add("[Add New Customer]")

            CmbInstance.Items.Clear()
            CmbInstance.Items.Add("[Add New Instance]")

            If Not IsNothing(CI.CustomerName) Then
                Dim CustomerList As New ArrayList
                For i As Integer = 0 To CI.CustomerName.Count - 1
                    If Not CustomerList.Contains(CI.CustomerName(i)) Then
                        CustomerList.Add(CI.CustomerName(i))
                    End If
                Next

                For i As Integer = 0 To CustomerList.Count - 1
                    CmbCustomer.Items.Add(CustomerList(i))
                Next
            End If
        Catch ex As Exception
            'old project
        End Try


    End Sub


    Private Sub Dialog1_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Me.Size = New Point(784, 548)


        SetProjectGroup()

        lblLanguage.Text = ""
        ListBox1.Items.Clear()


        If Not ProjectGroupName Is Nothing Then
            CmbProjectGroup.Text = ProjectGroupName
        End If

        If Not (System.IO.File.Exists(appData & DefinitionFiles.Lang_List)) Then MsgBox("File Language.txt doesn't exist. Critical error!", MsgBoxStyle.Critical)

        For Each lang In Split(System.IO.File.ReadAllText(appData & DefinitionFiles.Lang_List), vbLf)
            If lang.Trim <> "" Then
                ListBox1.Items.Add(Mid(lang, 1, InStr(lang, Chr(9)) - 1))
            End If
        Next

        Dim ActiveProject As ProjectDetail = Nothing

        Try
            ActiveProject = ProjectManagement.GetActiveProject
        Catch ex As Exception
            If ActiveProject Is Nothing Then
                sFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) '& "\Project1\"
            Else
                sFolderPath = ActiveProject.ProjectPath
            End If
        End Try


        If Microsoft.VisualBasic.Right(sFolderPath, 1) = "\" Then
            sFolderPath = sFolderPath.Substring(0, sFolderPath.Length - 1)
            sFolderPath = System.IO.Path.GetDirectoryName(sFolderPath)
        End If

        Select Case my_action
            Case MyAction.Add_NewProject
                Label5.Text = "New Project Settings"
                TextBox_ProjectName.Text = "Project1"
                TextBox_ProjectPath.Text = sFolderPath '& TextBox1.Text
                TextBox_ProjectPath.Enabled = True
                Button1.Enabled = True
            Case MyAction.Edit_Project
                Dim projectlist As ArrayList = ProjectManagement.GetProjectNameList ' get_project_list()
                If projectlist.Count = 0 Then
                    MsgBox("There are no active projects to select", MsgBoxStyle.Information, "Cloud translator")
                    Me.Close()
                    Exit Sub
                End If
                Label5.Text = "Edit Project Settings"
                TextBox_ProjectPath.Enabled = False
                Button1.Enabled = False

            Case MyAction.Load_based_onMaserProject
                Label5.Text = "New Project Settings from Master project"
                TextBox_ProjectName.Text = "Project1"
                ChkMaster.Checked = False
        End Select

        If Not BW.IsBusy Then
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            BW.RunWorkerAsync()
        End If

        CheckFinishEnable()
        Me.StartPosition = FormStartPosition.CenterParent
    End Sub

    Public Property MasterProjectName As String

    Private Sub SetProjectGroup()
        Try
            Dim sProjectGroupNames As ArrayList = ProjectManagement.GetProjectGroupNameList ' ProjectGroupAdditionalFunctions.GetProjectGroupList()
            CmbProjectGroup.Items.Clear()
            CmbProjectGroup.Items.Add("[Add Project Group]")
            For i As Integer = 0 To sProjectGroupNames.Count - 1
                CmbProjectGroup.Items.Add(sProjectGroupNames(i))
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub


    Private Sub AddNewProjectGroupName()
        Try
            Dim sProjectGroupNames As String() = System.IO.File.ReadAllLines(appData & DefinitionFiles.ProjectGroupName)

            For i As Integer = 0 To UBound(sProjectGroupNames)
                If String.Compare(CmbProjectGroup.Text, sProjectGroupNames(i), True) = 0 Then
                    Exit Sub 'No need to update this as it will be duplicated
                End If
            Next

            'Else Write the New ProjectGroupName to Defintion file
            Using Writer As StreamWriter = New StreamWriter(appData & DefinitionFiles.ProjectGroupName, False)
                For i As Integer = 0 To UBound(sProjectGroupNames)
                    Writer.WriteLine(sProjectGroupNames(i))
                Next
                Writer.WriteLine(CmbProjectGroup.Text)
            End Using

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub



    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox_ProjectName.TextChanged
        On Error Resume Next
        'If my_action = 1 And InStr(1, TextBox2.Text, Application.StartupPath) Then
        '    TextBox2.Text = Application.StartupPath & "\Projects\" & TextBox1.Text & "\"
        'End If

        'If my_action = 1 Then
        '    TextBox2.Text = sFolderPath & TextBox1.Text & "\"
        'End If
        CheckFinishEnable()
    End Sub

    Private Sub btnBrowseLang_Click(sender As Object, e As EventArgs) Handles btnBrowseLang.Click

        Try
            Dim FD As New OpenFileDialog
            FD.Filter = "(Lang Files)|*.Lang"
            Dim _LangFile As String = ""
            If FD.ShowDialog <> Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If

            _LangFile = FD.FileName

            Dim LangContent() As String = Split(System.IO.File.ReadAllText(_LangFile), vbNewLine)

            If UBound(LangContent) < 2 Then
                Throw New Exception("Not a valid file!")
            End If

            If Microsoft.VisualBasic.Left(LangContent(0), 1) <> "#" And Microsoft.VisualBasic.Left(LangContent(1), 0) <> "#" And Microsoft.VisualBasic.Left(LangContent(2), 1) <> "#" Then
                Throw New Exception("Not a valid file!")
            End If

            'Clear checked item
            lblLanguage.Text = ""
            For i = 0 To Me.ListBox1.Items.Count - 1
                ListBox1.SetItemChecked(i, False)
            Next i

            Dim strMsg As New StringBuilder
            strMsg.Append("Invalid language code!" & vbNewLine)
            Dim languageDisplayed As String = ""
            Dim bError As Boolean = False
            For i As Integer = 3 To UBound(LangContent)
                If LangContent(i).Trim <> "" Then
                    If LangContent(i).Length = 4 Then
                        Dim index As Integer = langcode_to_langline(LangContent(i))
                        If index = -1 Then
                            strMsg.Append("we have " & LangContent(i) & ", which is not known by cloud translator" & vbNewLine)
                            bError = True
                        Else
                            ListBox1.SetItemChecked(index, True)
                            languageDisplayed = languageDisplayed & LangContent(i) & ","
                        End If
                    Else
                        strMsg.Append("we have " & LangContent(i) & ", the language code should be of 4 letters" & vbNewLine)
                        bError = True
                    End If
                End If
            Next

            strMsg.Append("Please check the lang file.")

            If languageDisplayed = "" Then
                Throw New Exception("No languages found in the file!" & vbNewLine & "Please check the file.")
            End If
            languageDisplayed = languageDisplayed.Substring(0, languageDisplayed.Length - 1)

            lblLanguage.Text = ListBox1.CheckedIndices.Count & " languages selected"

            If bError Then
                MsgBox(strMsg.ToString, MsgBoxStyle.Critical, "Lang Error")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Lang Error")
        End Try
        CheckFinishEnable()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        lblLanguage.Text = ListBox1.CheckedIndices.Count & " languages selected"
        CheckFinishEnable()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Dim Instance As String = InputBox("Enter Instance", "Instance")
        If Instance.Trim = "" Then
            MsgBox("No Instance entered to Instance list!", MsgBoxStyle.Exclamation, "Empty Instance")
        End If
    End Sub

    Private Sub CmbCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbCustomer.SelectedIndexChanged
        If CmbCustomer.SelectedIndex = 0 Then
            Dim CustomerName As String = InputBox("Enter Customer name", "CustomerName")
            If CustomerName.Trim = "" Then
                CmbCustomer.SelectedIndex = -1
                MsgBox("No Customer name entered to Customer list!", MsgBoxStyle.Exclamation, "Customer name empty")
                Exit Sub
            End If
            Dim bFound As Boolean = False
            For i As Integer = 0 To CmbCustomer.Items.Count - 1
                If CustomerName.Trim.ToLower = CmbCustomer.Items(i).ToString.ToLower Then
                    MsgBox(CustomerName & " - Customer Name already available in the list!", MsgBoxStyle.Exclamation, "Duplicate Customer")
                    bFound = True
                    Exit For
                End If
            Next

            If Not bFound Then
                CmbCustomer.Items.Add(CustomerName)
                CmbCustomer.SelectedIndex = CmbCustomer.Items.Count - 1
            End If

        End If

        If CmbCustomer.SelectedIndex > 0 Then
            PopulateInstace(CmbCustomer.Text)
        End If

        CheckFinishEnable()

    End Sub

    Private Sub CmbInstance_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbInstance.SelectedIndexChanged

        If CmbInstance.SelectedIndex = 0 Then
            Dim Instance As String = InputBox("Enter Instance", "Instance")
            If Instance.Trim = "" Then
                CmbInstance.SelectedIndex = -1
                MsgBox("No Instance entered to Instance list!", MsgBoxStyle.Exclamation, "Empty Instance")
                Exit Sub
            End If
            Dim bFound As Boolean = False
            For i As Integer = 0 To CmbInstance.Items.Count - 1
                If Instance.Trim.ToLower = CmbInstance.Items(i).ToString.ToLower Then
                    MsgBox(Instance & " - Instance already available in the list!", MsgBoxStyle.Exclamation, "Duplicate Instance")
                    bFound = True
                    Exit For
                End If
            Next

            If Not bFound Then
                CmbInstance.Items.Add(Instance)
                CmbInstance.SelectedIndex = CmbInstance.Items.Count - 1
            End If
        End If
        CheckFinishEnable()
    End Sub

    Private Sub BtnNext_Click(sender As Object, e As EventArgs) Handles BtnNext.Click
        BtnNext.Enabled = False
        BtnBack.Enabled = True
        GB1.Visible = False
        GB2.Visible = True
        GB2.Location = GB1.Location
    End Sub

    Private Sub BtnBack_Click(sender As Object, e As EventArgs) Handles BtnBack.Click
        BtnBack.Enabled = False
        BtnNext.Enabled = True
        GB2.Visible = False
        GB1.Visible = True
    End Sub

    Private Sub CheckFinishEnable()
        If TextBox_ProjectName.Text.Trim <> String.Empty And _
            TextBox_ProjectPath.Text.Trim <> String.Empty And _
            CmbCustomer.SelectedIndex > 0 And _
            CmbInstance.SelectedIndex > 0 And _
            ListBox1.CheckedIndices.Count > 0 Then
            OK_Button.Enabled = True
            BtnNext.Enabled = True
        Else
            OK_Button.Enabled = False
            BtnNext.Enabled = False
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox_ProjectPath.TextChanged
        CheckFinishEnable()
    End Sub

    Private Sub BW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BW.DoWork
        CI.CustomerName = New ArrayList
        CI.InstanceName = New ArrayList
        CI.CustomerInstance = New ArrayList

        If CheckURL("http://10.66.9.51:8013/") Then
            Dim MyCD As New Cloud_TR.Service1
            Dim ds As New DataSet
            Try
                ds = MyCD.GetCustomerInstanceList
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                CI.InstanceName.Add(ds.Tables(0).Rows(i).Item(0))
                CI.CustomerName.Add(ds.Tables(0).Rows(i).Item(1))
                CI.CustomerInstance.Add(ds.Tables(0).Rows(i).Item(1))
            Next
        End If

        If my_action = MyAction.Edit_Project Or my_action = MyAction.Load_based_onMaserProject Then
            Dim CustName As String = Nothing
            Dim IntanceName As String = Nothing

            Try
                CustName = ProjectManagement.GetActiveProject.CustomerName ' GetCustomerName()
                IntanceName = ProjectManagement.GetActiveProject.InstaneName
            Catch ex As Exception
                If my_action = MyAction.Load_based_onMaserProject Then
                    CustName = ProjectManagement.GetProjectDetail(MasterProjectName).CustomerName
                    IntanceName = ProjectManagement.GetProjectDetail(MasterProjectName).InstaneName
                End If
            End Try

            If IntanceName Is Nothing Then
                IntanceName = ""
            End If

            'firs add custname if not got from service call

            Dim bCustFound As Boolean = False
            For i As Integer = 0 To CI.CustomerName.Count - 1
                If CI.CustomerName(i).ToString.ToLower.Trim = CustName.ToLower.Trim Then
                    bCustFound = True
                End If
            Next

            If Not bCustFound Then
                CI.CustomerName.Add(CustName)
            End If

            For i As Integer = 0 To CI.CustomerName.Count - 1
                If CI.CustomerName(i).ToString.ToLower.Trim = CustName.ToLower.Trim Then
                    Dim bFound As Boolean = False
                    For j As Integer = 0 To CI.InstanceName.Count - 1
                        If CI.InstanceName(j).ToString.ToLower.Trim = IntanceName.ToLower.Trim Then
                            bFound = True
                        End If
                    Next
                    If bFound <> True Then
                        CI.InstanceName.Add(IntanceName)
                        CI.CustomerInstance.Add(CI.CustomerName(i))
                    End If
                End If
            Next

        End If

    End Sub

    Private Sub BW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BW.RunWorkerCompleted
        If Not e.Error Is Nothing Then
            MsgBox(e.Error.Message.ToString, MsgBoxStyle.Critical, "Error")
            Me.Visible = True
        End If
        setCustomerInstance()
        Dim PD As New ProjectDetail
        If my_action = MyAction.Edit_Project Then
            GetExclusionList()
            PD = ProjectManagement.GetProjectDetail(ProjectManagement.GetActiveProject.ProjectName)
            AssingProjectDetails(PD, my_action)
        ElseIf my_action = MyAction.Load_based_onMaserProject Then
            GetExclusionList()
            PD = ProjectManagement.GetProjectDetail(MasterProjectName)
            PD.ProjectName = "Project1"
            PD.isMasterProject = False
            AssingProjectDetails(PD, my_action)
        End If

        Me.Cursor = Cursors.Default
        Me.Enabled = True
    End Sub


    Private Sub AssingProjectDetails(ByRef PD As ProjectDetail, ByRef action As MyAction)

        With PD
            OldProjectGroupName = .ProjectGroupName
            TextBox_ProjectName.Text = .ProjectName
            OldProjectName = .ProjectName
            TextBox_Description.Text = .ProjectDescription
            ChkCleanTranslation.Checked = .isCleanRequired
            ChkRestrictCustomer.Checked = .isCustomerCheckRequired
            ChkInstance.Checked = .isInstanceCheckRequired
            ChkPretranslate.Checked = .isPretranslateEnabled
            ChkUploadToDB.Checked = .isDBupdateRequired
            CmbProjectGroup.Text = .ProjectGroupName
            ChkCorrupt.Checked = .isCorruptEnabled
            ChkMaxLength.Checked = .isMaxLengthCheckRequired

            Dim ProjectLocation As String = .ProjectPath.Substring(0, Len(.ProjectPath) - 1)
            TextBox_ProjectPath.Text = System.IO.Path.GetDirectoryName(ProjectLocation)
            Dim Projectname As String = System.IO.Path.GetFileName(ProjectLocation)
            Dim CustName As String = .CustomerName
            CmbCustomer.Text = CustName
            If CmbCustomer.Text = "" Then
                CI.CustomerName.Add(CustName)
            End If
            Dim InstanceName As String = .InstaneName
            CmbInstance.Text = InstanceName
            setCustomerInstance()
            CmbCustomer.Text = CustName
            CmbInstance.Text = InstanceName
            For Each lang In Split(.LangList, ",")
                ListBox1.SetItemChecked(langcode_to_langline(lang), True)
            Next
            ChkMaster.Checked = .isMasterProject
            lblLanguage.Text = ListBox1.CheckedIndices.Count & " languages selected"
        End With

        If TextBox_ProjectPath.Text.Trim = "" Then
            MsgBox("No Project to select!", MsgBoxStyle.Information, "Cloud translator")
            Me.Close()
            Exit Sub
        End If

        CheckFinishEnable()

    End Sub

    Private Sub ChkPretranslate_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPretranslate.CheckedChanged
        If ChkPretranslate.Checked Then
            ChkRestrictCustomer.Enabled = True
            ChkInstance.Enabled = True
        Else
            ChkRestrictCustomer.Checked = False
            ChkInstance.Checked = False
            ChkRestrictCustomer.Enabled = False
            ChkInstance.Enabled = False
        End If
    End Sub

    Private Sub ChkInstance_CheckedChanged(sender As Object, e As EventArgs) Handles ChkInstance.CheckedChanged
        If ChkInstance.Checked Then
            ChkRestrictCustomer.Checked = True
        End If
    End Sub

    Private Sub CmbProjectGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbProjectGroup.SelectedIndexChanged
        If CmbProjectGroup.SelectedIndex = 0 Then
            Dim ProjectGroupName As String = InputBox("Enter Project group name", "ProjectGroup")
            If ProjectGroupName.Trim = "" Then
                CmbProjectGroup.SelectedIndex = -1
                MsgBox("No Project Group name entered!" & vbNewLine & "Default Group will be selected", MsgBoxStyle.Exclamation, "Project group name empty")
                CmbProjectGroup.Text = "Default"
                Exit Sub
            End If
            Dim bFound As Boolean = False
            For i As Integer = 0 To CmbProjectGroup.Items.Count - 1
                If ProjectGroupName.Trim.ToLower = CmbProjectGroup.Items(i).ToString.ToLower Then
                    MsgBox(ProjectGroupName & " - Project group name already available in the list!", MsgBoxStyle.Exclamation, "Duplicate ProjectGroup")
                    bFound = True
                    Exit For
                End If
            Next

            If Not bFound Then
                CmbProjectGroup.Items.Add(ProjectGroupName)
                CmbProjectGroup.SelectedIndex = CmbProjectGroup.Items.Count - 1
            End If

        End If

    End Sub

    Public Property ProjectGroupList As String()

End Class
