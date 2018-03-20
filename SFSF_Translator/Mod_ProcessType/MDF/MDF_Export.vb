Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Threading
Imports System.Xml
Imports System.IO
Public Class MDF_Export
    Public Shared _GroupName As String = ""
    Private _BaseURL As String
    Private _CompID As String
    Private _Uname As String
    Private _Password As String
    Private obj_api As cls_API = New cls_API()
    Private Worker_MDFType As BackgroundWorker = New BackgroundWorker()
    Private Worker_MDFExport As BackgroundWorker = New BackgroundWorker()
    Private Enum ApiStatus
        JOBID = 1
        EXPORT = 2
    End Enum

    Private Sub MDF_Export_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'BG-Worker for populate MDF Types
        AddHandler Worker_MDFType.DoWork, AddressOf Worker_MDFType_DoWork
        AddHandler Worker_MDFType.RunWorkerCompleted, AddressOf Worker_MDFType_RunWorkerCompleted
        AddHandler Worker_MDFType.ProgressChanged, AddressOf Worker_MDFType_ProgressChanged
        Worker_MDFType.WorkerReportsProgress = True
        Worker_MDFType.WorkerSupportsCancellation = True

        'BG-Worker for populate MDF Export
        AddHandler Worker_MDFExport.DoWork, AddressOf Worker_MDFExport_DoWork
        AddHandler Worker_MDFExport.RunWorkerCompleted, AddressOf Worker_MDFExport_RunWorkerCompleted
        AddHandler Worker_MDFExport.ProgressChanged, AddressOf Worker_MDFExport_ProgressChanged
        Worker_MDFExport.WorkerReportsProgress = True
        Worker_MDFExport.WorkerSupportsCancellation = True

        obj_api.create_XMLFile()
        ReadCredentials()
        AddGroup_Combo()
        ControlStatus(False)
        CreateFile_MDFtypeList()

        ' Loading MDF Types
        lbl_populatingMDF.Show()
        If Not Worker_MDFType.IsBusy Then
            Worker_MDFType.RunWorkerAsync()
        End If
    End Sub

#Region "BG worker"
    Private Sub Worker_MDFType_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs)
        Try
            Dim sendingWorker As BackgroundWorker = CType(sender, BackgroundWorker)
            Dim str_session As String = obj_api.GetSession4MdfType(_BaseURL, _CompID, _Uname, _Password)
            Dim str_MDFJson As String = obj_api.GetMDFtypes_Raw(str_session)
            Dim List_MDF As List(Of MDFTypeList_items) = CType(Newtonsoft.Json.JsonConvert.DeserializeObject(str_MDFJson, GetType(List(Of MDFTypeList_items))), List(Of MDFTypeList_items))
            For Each item In List_MDF
                Worker_MDFType.ReportProgress(0, item.id.ToString())
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
    Private Sub Worker_MDFType_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)
        checkedListBox1.Items.Add(CStr(e.UserState))
    End Sub
    Private Sub Worker_MDFType_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
        Using file As System.IO.StreamWriter = New System.IO.StreamWriter(Application.StartupPath & "\MDFType.txt")
            For i As Integer = 0 To checkedListBox1.Items.Count - 1
                file.WriteLine(checkedListBox1.Items(i).ToString())
            Next
        End Using

        lbl_populatingMDF.Hide()
        ControlStatus(True)
        Worker_MDFType.CancelAsync()
    End Sub
    Private Sub Worker_MDFExport_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs)
        Try
            Dim sendingWorker As BackgroundWorker = CType(sender, BackgroundWorker)
            Worker_MDFExport.ReportProgress(ApiStatus.JOBID, "")
            Dim jobID As String = obj_api.GetJobId(_BaseURL, _CompID, _Uname, _Password, TypeIDs())
            Worker_MDFExport.ReportProgress(ApiStatus.JOBID, jobID)
            Thread.Sleep(2000)
            Worker_MDFExport.ReportProgress(ApiStatus.EXPORT, "")
            Dim str_MDFexport As String = obj_api.GetMdfInfo(jobID)
            Worker_MDFExport.ReportProgress(ApiStatus.EXPORT, str_MDFexport)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
    Private Sub Worker_MDFExport_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)
        Select Case e.ProgressPercentage
            Case 1
                updateStatus(CInt(ApiStatus.JOBID), e.UserState.ToString())
            Case 2
                updateStatus(CInt(ApiStatus.EXPORT), e.UserState.ToString())
        End Select
    End Sub
    Private Sub Worker_MDFExport_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
        lbl_jobStatus.Text = ""
        cmb_MDFGroup.Text = ""
        chekListUnTick()
        checkBox1.Text = "Check All"
        cmb_MDFGroup.Enabled = True
        checkBox1.Enabled = True
        linkLabel1.Enabled = True
        linkLabel2.Enabled = True
        btn_getjobid.Enabled = True
        checkedListBox1.Enabled = True
        Worker_MDFExport.CancelAsync()
    End Sub
#End Region

#Region "Methods"
    Private Sub ControlStatus(ByVal stat As Boolean)
        cmb_MDFGroup.Enabled = stat
        checkBox1.Enabled = stat
        btn_getjobid.Enabled = stat
        linkLabel1.Enabled = stat
        linkLabel2.Enabled = stat
    End Sub
    Private Sub CreateFile_MDFtypeList()
        Try
            If Not File.Exists(Application.StartupPath & "\MDFType.txt") Then
                File.Create(Application.StartupPath & "\MDFType.txt")
            Else
                File.WriteAllText(Application.StartupPath & "\MDFType.txt", String.Empty)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function TypeIDs() As String
        Dim selectedTypeIds As String = ""
        For Each item In checkedListBox1.CheckedItems
            selectedTypeIds += item & ","
        Next

        Return selectedTypeIds.TrimEnd(","c)
    End Function
    Private Sub ReadCredentials()
        _BaseURL = My.Settings.BaseURL
        _CompID = My.Settings.CompID
        _Uname = My.Settings.Uname
        _Password = My.Settings.Password
    End Sub
    Private Sub updateStatus(ByVal ApiType As Integer, ByVal message As String)
        If ApiType = CInt(ApiStatus.JOBID) Then
            If message = "" Then
                lbl_jobStatus.Text = "generating JobID....."
            Else
                lbl_jobStatus.Text = "exporting MDF objects....."
                lbl_JobId.Text = "(id= " & message & ")"
            End If
        ElseIf ApiType = CInt(ApiStatus.EXPORT) Then
            If message = "" Then
                lbl_jobStatus.Text = "exporting MDF objects....."
            Else
                lbl_jobStatus.Text = ""
                richTextBox1.Text = message
            End If
        End If
    End Sub
    Private Sub AddGroup_Combo()
        Try
            cmb_MDFGroup.Items.Clear()
            Dim xmldoc As XmlDataDocument = New XmlDataDocument()
            Dim xmlnode As XmlNodeList
            Using fs As FileStream = New FileStream(Application.StartupPath & "\MDFGroup.xml", FileMode.Open, FileAccess.Read)
                xmldoc.Load(fs)
                xmlnode = xmldoc.GetElementsByTagName("Group")
                For grpcount As Integer = 0 To xmlnode.Count - 1
                    cmb_MDFGroup.Items.Add(xmlnode(grpcount).ChildNodes.Item(0).InnerText.Trim())
                Next
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub Read_GroupItems(ByVal GroupName As String)
        Try
            Dim xmldoc As XmlDataDocument = New XmlDataDocument()
            Dim xmlnode As XmlNodeList
            Using fs As FileStream = New FileStream(Application.StartupPath & "\MDFGroup.xml", FileMode.Open, FileAccess.Read)
                xmldoc.Load(fs)
                xmlnode = xmldoc.GetElementsByTagName("Group")
                For idcount As Integer = 0 To xmlnode.Count - 1
                    If xmlnode(idcount).ChildNodes.Item(0).InnerText.Trim().ToUpper() = GroupName.ToUpper() Then
                        Dim str_MDFids As String() = xmlnode(idcount).ChildNodes.Item(1).InnerText.Trim().Split(","c)
                        For countId As Integer = 0 To str_MDFids.Length - 1
                            For i As Integer = 0 To checkedListBox1.Items.Count - 1
                                If checkedListBox1.Items(i).ToString() = str_MDFids(countId).ToString() Then
                                    checkedListBox1.SetItemChecked(i, True)
                                    Exit For
                                End If
                            Next
                        Next
                    End If
                Next
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function createNode(ByVal MDFIds As String) As Boolean
        Try
            Dim doc As XDocument = XDocument.Load(Application.StartupPath & "\MDFGroup.xml")
            Dim school As XElement = doc.Element("MDFGroupList")
            school.Add(New XElement("Group", New XElement("GroupName", _GroupName), New XElement("GroupIds", MDFIds)))
            doc.Save(Application.StartupPath & "\MDFGroup.xml")
            Return True
        Catch ex As Exception
            Return False
            Throw ex
        End Try
    End Function
    Public Sub create_XMLFile()
        Try
            If Not File.Exists(Application.StartupPath & "\MDFGroup.xml") Then
                Dim xmlDoc As XmlDocument = New XmlDocument()
                Dim xmlDeclaration As XmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", Nothing)
                Dim rootNode As XmlElement = xmlDoc.CreateElement("MDFGroupList")
                xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement)
                xmlDoc.AppendChild(rootNode)
                xmlDoc.Save(Application.StartupPath & "\MDFGroup.xml")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub chekListUnTick()
        For i As Integer = 0 To checkedListBox1.Items.Count - 1
            checkedListBox1.SetItemChecked(i, False)
        Next
    End Sub
    Private Sub chekListTick()
        For i As Integer = 0 To checkedListBox1.Items.Count - 1
            checkedListBox1.SetItemChecked(i, True)
        Next
    End Sub
#End Region

#Region "Methods"


    Private Sub linkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkLabel2.LinkClicked
        chekListUnTick()
        cmb_MDFGroup.SelectedIndex = -1
        Dim obj_form As MDF_Group = New MDF_Group()
        obj_form.ShowDialog()
    End Sub

    Private Sub linkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkLabel1.LinkClicked
        Dim obj_Credentials As MDF_Credentials = New MDF_Credentials()
        obj_Credentials.ShowDialog()
    End Sub

    Private Sub btn_getjobid_Click(sender As Object, e As EventArgs) Handles btn_getjobid.Click
        If checkedListBox1.CheckedItems.Count <= 0 Then
            MessageBox.Show("Please select atleast one MDF type.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        Else
            richTextBox1.Text = ""
            cmb_MDFGroup.Enabled = False
            checkBox1.Enabled = False
            linkLabel1.Enabled = False
            linkLabel2.Enabled = False
            btn_getjobid.Enabled = False
            checkedListBox1.Enabled = False
            If Not Worker_MDFExport.IsBusy Then
                Worker_MDFExport.RunWorkerAsync()
            End If
        End If
    End Sub

    Private Sub checkBox1_CheckedChanged(sender As Object, e As EventArgs) Handles checkBox1.CheckedChanged
        If checkedListBox1.Items.Count > 0 Then
            If checkBox1.Checked Then
                chekListTick()
                checkBox1.Text = "Uncheck All"
            Else
                chekListUnTick()
                checkBox1.Text = "Check All"
            End If
        End If
    End Sub

    Private Sub cmb_MDFGroup_Enter(sender As Object, e As EventArgs) Handles cmb_MDFGroup.Enter
        cmb_MDFGroup.SelectedIndex = -1
        AddGroup_Combo()
    End Sub

    Private Sub cmb_MDFGroup_SelectedValueChanged(sender As Object, e As EventArgs) Handles cmb_MDFGroup.SelectedValueChanged
        chekListUnTick()
        Read_GroupItems(cmb_MDFGroup.Text)
    End Sub

    Private Sub MDF_Export_Enter(sender As Object, e As EventArgs) Handles MyBase.Enter
        AddGroup_Combo()
    End Sub
#End Region

    Public Class MDFTypeList_items
        Public Property id As String
        Public Property name As String
    End Class

End Class