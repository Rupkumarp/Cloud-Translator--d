Imports System.IO

Public Class Form_UpdateToDB

    Private CustomerName As String

    Private InstanceName As String

    Private singleFile As String


    Private Enum ProcessType
        SingleFile
        MultipleFile
    End Enum

    Private _processType As ProcessType

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        counter = 0
        totalfiles = 0
    End Sub

    Private Sub RBSingle_CheckedChanged(sender As Object, e As EventArgs) Handles RBSingle.CheckedChanged
        txtInputXlifffile.Text = ""
        _processType = ProcessType.SingleFile
    End Sub

    Private Sub RBMultiple_CheckedChanged(sender As Object, e As EventArgs) Handles RBMultiple.CheckedChanged
        txtInputXlifffile.Text = ""
        _processType = ProcessType.MultipleFile
    End Sub

    Private Sub Btn1Browse_Click(sender As Object, e As EventArgs) Handles Btn1Browse.Click
        Dim fDialog As Object
        If RBSingle.Checked Then
            fDialog = New OpenFileDialog
            fDialog.Filter = "Translated xliff  files (*.xliff)|*.xliff;"
        Else
            fDialog = New FolderBrowserDialog
            fDialog.Description = "Select Transalted Xliff Input folder"
        End If

        If fDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            If RBSingle.Checked Then
                txtInputXlifffile.Text = fDialog.FileName
            Else
                txtInputXlifffile.Text = fDialog.Selectedpath
            End If
        End If
    End Sub

    Public Structure CustomerInstance
        Public CustomerName As ArrayList
        Public Instance As ArrayList
    End Structure

    Dim CI As New CustomerInstance

    Private Sub BW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BW.DoWork
        CI.CustomerName = New ArrayList
        CI.Instance = New ArrayList

        If CheckURL("http://10.66.9.51:8013/") Then
            Dim MyCD As New Cloud_TR.Service1
            Dim ds As New DataSet
            ds = MyCD.GetCustomerInstanceList

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                CI.Instance.Add(ds.Tables(0).Rows(i).Item(0))
                CI.CustomerName.Add(ds.Tables(0).Rows(i).Item(1))
            Next
        End If
    End Sub

    Private Sub BW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BW.RunWorkerCompleted
        GetExclusionList()
        If Not e.Error Is Nothing Then
            MsgBox(e.Error.Message.ToString, MsgBoxStyle.Critical, "Cloud translator")
            Me.Visible = True
            'Exit Sub
        End If
        Dim CustName As String = ProjectManagement.GetActiveProject.CustomerName ' GetCustomerName()
        CmbCustomer.Text = CustName
        If CmbCustomer.Text = "" Then
            CI.CustomerName.Add(CustName)
        End If
        Dim InstanceName As String = ProjectManagement.GetActiveProject.InstaneName ' GetInstance()
        CmbInstance.Text = InstanceName
        If CmbInstance.Text = "" Then
            CI.Instance.Add(InstanceName)
        End If
        setCustomerInstance()
        CmbCustomer.Text = CustName
        CmbInstance.Text = InstanceName

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

    Private Sub GetExclusionList()
        Dim ExlFile As String = ProjectManagement.GetActiveProject.ProjectPath & "DbExclusion.config"
        If Not System.IO.File.Exists(ExlFile) Then
            Exit Sub
        End If
        Using Reader As StreamReader = New StreamReader(ExlFile, True)
            TextBox4.Text = Reader.ReadToEnd
        End Using
    End Sub


    Private Sub Form_UpdateToDB_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        If Not BW.IsBusy Then
            BW.RunWorkerAsync()
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
    End Sub

    Sub PopulateInstace(ByVal Customername As String)
        Try
            CmbInstance.Items.Clear()
            CmbInstance.Items.Add("[Add New Instance]")
            Dim Instancearray As New ArrayList

            If Not IsNothing(CI.CustomerName) Then
                For i As Integer = 0 To CI.CustomerName.Count - 1
                    If Not IsNothing(CI.CustomerName(i)) Then
                        If CI.CustomerName(i).ToString.ToLower = Customername.ToLower Then
                            If Not Instancearray.Contains(CI.Instance(i)) Then
                                If Not IsNothing(CI.Instance(i)) Then
                                    Instancearray.Add(CI.Instance(i))
                                End If
                            End If
                        End If
                    Else
                        CI.CustomerName.RemoveAt(i)
                        CI.Instance.RemoveAt(i)
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

    Private WithEvents CU As New Cls_CloudJob

    Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles BtnImport.Click
        'Validate Input files
        If txtInputXlifffile.Text.Trim = "" Then
            MsgBox("Xliff input is empty! Please select file\folder.", MsgBoxStyle.Critical, "Cloud Translator")
            Exit Sub
        End If

        If RBMultiple.Checked Then
            If Not System.IO.Directory.Exists(txtInputXlifffile.Text) Then
                MsgBox("Folder doesnot exists!", MsgBoxStyle.Critical, "Cloud Translator")
                Exit Sub
            End If
        End If

        If RBSingle.Checked Then
            If Not System.IO.File.Exists(txtInputXlifffile.Text) Then
                MsgBox("File doesnot exists!", MsgBoxStyle.Critical, "Cloud Translator")
                Exit Sub
            End If
        End If

        If CmbCustomer.SelectedIndex <= 0 Or CmbInstance.SelectedIndex <= 0 Then
            MsgBox("File Input valid Customer\Instance", MsgBoxStyle.Critical, "Cloud Translator")
            Exit Sub
        End If

        Try
            UpdateExculsionList()
        Catch ex As Exception
            MsgBox("Error updating Exclusion list!" & vbNewLine & ex.Message, MsgBoxStyle.Critical, "Cloud Translator")
        End Try

        Try
            EnableDisableControls(False)

            CustomerName = CmbCustomer.Text
            InstanceName = CmbInstance.Text
            singleFile = txtInputXlifffile.Text

            If Not Bw2.IsBusy Then
                Bw2.RunWorkerAsync()
            End If

        Catch ex As Exception
            Me.Visible = True
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

    End Sub

    Sub EnableDisableControls(ByVal b As Boolean)
        Me.BtnImport.Enabled = b
        Me.GroupBox2.Enabled = b
        Me.GroupBox3.Enabled = b
    End Sub

    Dim counter As Integer = 0
    Dim totalfiles As Integer = 0
    Private Sub Bw2_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles Bw2.DoWork

        If _processType = ProcessType.MultipleFile Then
            totalfiles = System.IO.Directory.GetFiles(txtInputXlifffile.Text, "*.xliff").Count
            For Each f In My.Computer.FileSystem.GetFiles(txtInputXlifffile.Text, FileIO.SearchOption.SearchTopLevelOnly)
                If Bw2.CancellationPending = True Then
                    Exit For
                End If
                If System.IO.Path.GetExtension(f).ToLower = ".xliff" Then
                    counter += 1
                    Bw2.ReportProgress(2)
                    Try
                        UpdateToDB(f)
                    Catch ex As Exception
                        Dim str As String = ex.Message & vbCrLf
                        Bw2.ReportProgress(3, str)
                    End Try
                End If
            Next
        ElseIf _processType = ProcessType.SingleFile Then
            totalfiles = 1
            counter += 1
            Bw2.ReportProgress(2)
            UpdateToDB(singleFile)
        End If


    End Sub

    Private Sub Bw2_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles Bw2.ProgressChanged
        If e.ProgressPercentage = 0 Then
            Form_MainNew.UpdateProgressBar(e.UserState(0), CInt(e.UserState(1)))
        ElseIf e.ProgressPercentage = 1 Then
            Form_MainNew.UpdateMsg(e.UserState(0), e.UserState(1))
        ElseIf e.ProgressPercentage = 3 Then
            Form_MainNew.UpdateMsg(e.UserState.ToString, Form_MainNew.RtbColor.Red)
        Else
            Form_MainNew.UpdateToolstripStatus("Importing translated xliff " & counter & "\" & totalfiles)
        End If
    End Sub

    Private Sub Bw2_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles Bw2.RunWorkerCompleted
        EnableDisableControls(True)
        If counter >= 1 Then
            MsgBox(counter & " files Uploaded Successfully!", MsgBoxStyle.Information, "Cloud Translator")
        Else
            MsgBox("No files found!", MsgBoxStyle.Information, "Cloud Translator")
        End If
        Me.Button1.Enabled = True
        Me.Button1.Text = "Cancel"
    End Sub

    Private Sub UpdateToDB(ByVal sFile As String)
        Try
            Dim FileNumber As String = System.IO.Path.GetFileNameWithoutExtension(sFile)
            FileNumber = FileNumber.Substring(0, Len(FileNumber) - 6)
            CU.UpdateDB(sFile, FileNumber, CustomerName, InstanceName)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub CU_Progress(Max As Integer, val As Integer) Handles CU.Progress
        Dim str As New ArrayList
        str.Add(Max)
        str.Add(val)
        Bw2.ReportProgress(0, str)
    End Sub

    Private Sub CU_UpdateMsg(Msg As String, RTBC As Cls_CloudJob.RtbColor) Handles CU.UpdateMsg
        Dim str As New ArrayList
        str.Add(Msg)
        str.Add(RTBC)
        Bw2.ReportProgress(1, str)
    End Sub

    Private Sub UpdateExculsionList()
        Try
            Dim sProject As String = ProjectManagement.GetActiveProject.ProjectPath ' get_last_projectpath()
            Using writer As StreamWriter = New StreamWriter(sProject & "\DbExclusion.config", False)
                writer.WriteLine(TextBox4.Text)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

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

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Bw2.IsBusy Then
            Me.Button1.Enabled = False
            Me.Button1.Text = "please wait"
            Bw2.CancelAsync()
        End If
    End Sub
End Class