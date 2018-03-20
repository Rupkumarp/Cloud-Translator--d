Imports System.Windows.Forms
Imports Ionic.Zip

Public Class Form_ImportCTP

    Public ProjectGroupName As String

    Private Sub Form_ImportCTP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            LoadDetails()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

    End Sub

    Sub LoadDetails()
        If LstProjectGroup.Count = 0 Then
            Throw New Exception("No Projects to load")
            Exit Sub
        End If

        For i As Integer = 0 To (LstProjectGroup.Count - 1)
            LbProjectGroup.Items.Add(LstProjectGroup(i).ProjectGroupName)
        Next

        If ProjectGroupName <> String.Empty Then
            For i As Integer = 0 To LbProjectGroup.Items.Count - 1
                If LbProjectGroup.Items(i).ToString.ToLower = ProjectGroupName.ToLower Then
                    LbProjectGroup.SelectedIndex = i
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub BtnBrowseCTPfile_Click(sender As Object, e As EventArgs) Handles BtnBrowseCTPfile.Click
        Dim OD As New OpenFileDialog
        OD.Filter = "Cloud translator project *.cpt|*.ctp"
        If OD.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox_CTPfile.Text = OD.FileName
            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor
            ValidateCTPfile(TextBox_CTPfile.Text)
        End If
    End Sub

    Private Sub BtnBrowseProjectLocation_Click(sender As Object, e As EventArgs) Handles BtnBrowseProjectLocation.Click
        Dim FD As New FolderBrowserDialog
        FD.Description = "Select project location"
        If FD.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox_ProjectLocation.Text = FD.SelectedPath & "\"
        End If
    End Sub

    Private WithEvents objCTP As CTP

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnImport.Click

        If TextBox_CTPfile.Text = String.Empty Then
            MsgBox("CTP file not selected!", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        If TextBox_ProjectLocation.Text = String.Empty Then
            MsgBox("Project Location not set!", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        If Not System.IO.File.Exists(TextBox_CTPfile.Text) Then
            MsgBox("Cannot find the CTP file in the directory!", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        If Not System.IO.Directory.Exists(TextBox_ProjectLocation.Text) Then
            MsgBox("Invalid project location! Please select another path.", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        If CmbCustomer.SelectedIndex = -1 Or CmbCustomer.Text.Trim = String.Empty Then
            MsgBox("Enter Customer Name!", MsgBoxStyle.Critical, "Customer Detial")
            Exit Sub
        End If

        If CmbInstance.SelectedIndex = -1 Or CmbInstance.Text.Trim = String.Empty Then
            MsgBox("Enter Instance Name!", MsgBoxStyle.Critical, "Instance Detial")
            Exit Sub
        End If


        Dim ProjectName As String = TextBox_ProjectName.Text

        Try
            If ProjectManagement.isProjectNameAvailable(TextBox_ProjectName.Text) Then
                If MsgBox("Project Name already exists." & vbNewLine & "Do you want to create this project with numbered item?", MsgBoxStyle.Critical + MsgBoxStyle.YesNo, "Duplicate Project Name") = MsgBoxResult.Yes Then
                    Dim counter As Integer = 1
                    Do Until ProjectManagement.isProjectNameAvailable(ProjectName & "_" & counter) = False
                        counter += 1
                    Loop
                    ProjectName = ProjectName & "_" & counter
                Else
                    Exit Sub
                End If
            End If

            Dim ProjectLocation As String = TextBox_ProjectLocation.Text & ProjectName

            If Not System.IO.Directory.Exists(ProjectLocation) Then
                System.IO.Directory.CreateDirectory(ProjectLocation)
            End If

            Me.Enabled = False
            With Pd
                .ProjectName = ProjectName
                .ProjectGroupName = LbProjectGroup.Text
                .ProjectPath = ProjectLocation
                .isMasterProject = False
                .CustomerName = CmbCustomer.Text
                .InstaneName = CmbInstance.Text
            End With

            objCTP = New CTP
            objCTP.ImportCTP_Add(Pd, TextBox_CTPfile.Text)

            ProjectManagement.SetActiveProject(ProjectName)

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            Exit Sub
        End Try
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub objCTP_UpdateForm(EnableForm As Boolean) Handles objCTP.UpdateForm
        Form_MainNew.Enabled = True
        Me.Enabled = True
        Form_MainNew.RefreshTreeview()
        Form_MainNew.update_statusbar()
        Me.Close()
    End Sub

    Private Sub objCTP_UpdateMsg(Msg As String) Handles objCTP.UpdateMsg
        Form_MainNew.UpdateMsg(Msg, Form_MainNew.RtbColor.Black)
    End Sub

    Dim Pd As ProjectDetail

    Private Sub ValidateCTPfile(ByVal CtpFile As String)

        Try
            Using zip As ZipFile = ZipFile.Read(CtpFile)
                Dim zipel As ZipEntry
                For Each zipel In zip
                    If zipel.FileName.ToLower = "projects.ini" Then
                        zipel.Extract(Application.StartupPath, ExtractExistingFileAction.OverwriteSilently)
                        Exit For
                    End If
                Next
            End Using


            Dim SettingFile As String = Application.StartupPath & "\projects.ini"

            If Not System.IO.File.Exists(SettingFile) Then
                Throw New Exception("Error Could not find Project.ini file in " & CtpFile)
            End If

            Pd = XMLMethod.GetProjectDetailFromCTP(SettingFile)

            TextBox_ProjectName.Text = Pd.ProjectName

            If Not BW.IsBusy Then
                BW.RunWorkerAsync()
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


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

    Dim CI As New Dialog1.CustomerInstance


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
    End Sub

    Private Sub BW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BW.DoWork
        CI.CustomerName = New ArrayList
        CI.InstanceName = New ArrayList
        CI.CustomerInstance = New ArrayList

        If CheckURL("http://10.66.9.51:8013/") Then
            Dim MyCD As New Cloud_TR.Service1
            Dim ds As New DataSet
            ds = MyCD.GetCustomerInstanceList
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                CI.InstanceName.Add(ds.Tables(0).Rows(i).Item(0))
                CI.CustomerName.Add(ds.Tables(0).Rows(i).Item(1))
                CI.CustomerInstance.Add(ds.Tables(0).Rows(i).Item(1))
            Next
        End If

        Dim CustName As String = Pd.CustomerName
        Dim IntanceName As String = Pd.InstaneName

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

    End Sub

    Private Sub BW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BW.RunWorkerCompleted
        setCustomerInstance()
        CmbCustomer.Text = Pd.CustomerName
        CmbInstance.Text = Pd.InstaneName
        Me.Cursor = Cursors.Default
        Me.Enabled = True
    End Sub

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

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub LbProjectGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LbProjectGroup.SelectedIndexChanged
        If LbProjectGroup.Items.Count = 0 Then
            Exit Sub
        End If

        Label6.Text = "Import CTP file in " & LbProjectGroup.Text

    End Sub
End Class
