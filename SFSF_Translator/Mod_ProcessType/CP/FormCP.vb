Imports System.Xml
Imports System.IO

Public Class FormCP

    Public Enum Action
        Create
        Retrieve
    End Enum

    Public Enum CP_Child_SaveCreate
        Save
        Create
    End Enum

    Private CPSaveCreate As CP_Child_SaveCreate = CP_Child_SaveCreate.Create

    Public Property ActionType As Action
    Dim TotalRecord As Integer
    Dim DisplayedIndex As Integer
    Dim objRet As CPRetrieve
    Public Property CurrentDirectory As String

    Private bChildObjectSelected As Boolean = False

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        TotalRecord = 0
        DisplayedIndex = 0
        lblRecord.Text = "Record: 0/0"
    End Sub

    Private Sub FormCP_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        
        If Me.ActionType = Action.Create Then
            If bChildObjectSelected Then
                XmlFileName = cmbChildObjectType.Text
                SaveData(True)
                Exit Sub
            End If
            XmlFileName = TempXmlFile
            SaveData(False)
        End If
    End Sub

    Private Sub FormCP_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        objRet = New CPRetrieve(Me.ActionType)

        XmlSaveFolderPath = ProjectManagement.GetActiveProject.ProjectPath 'This path should be assigned before calling this form.

        If Microsoft.VisualBasic.Right(XmlSaveFolderPath, 1) <> "\" Then
            XmlSaveFolderPath = XmlSaveFolderPath & "\"
        End If
        XmlSaveFolderPath = XmlSaveFolderPath & "01-Input\"
        Select Case ActionType
            Case Action.Create
                lblCPType.Text = "CP Create"
                CmbLang.Visible = False
                CmbLang.Text = "en_US"
                Label7.Visible = False
                objRet.Retrieve(XmlSaveFolderPath)
                cmbParentObjectType.DataSource = Nothing
                cmbParentObjectType.DataSource = New BindingSource(Cp_Definition.CP, Nothing)
                BtnNew.Visible = True
            Case Action.Retrieve
                lblCPType.Text = "CP Retrieve"
                CPSaveCreate = CP_Child_SaveCreate.Save
                BtnCreateChild.Visible = False
                Label7.Visible = True
                objRet.Retrieve(XmlSaveFolderPath)
                cmbParentObjectType.DataSource = Nothing
                cmbParentObjectType.DataSource = New BindingSource(objRet.CP, Nothing)
                CmbLang.Visible = True
                CmbLang.Enabled = True
                BtnNew.Visible = False
        End Select

        cmbParentObjectType.DisplayMember = "key"
        cmbParentObjectType.ValueMember = "value"

        If cmbParentObjectType.Items.Count = 1 Then
            MsgBox("There is nothing to Display!", MsgBoxStyle.Information, "CP")
            Me.Close()
        End If

        ModifyButtonChild(CPSaveCreate)

    End Sub

    Dim objectIndex As Integer = 0

    Private Sub cmbObjectType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbParentObjectType.SelectedIndexChanged

        If ChkLangSeq.Checked Then
            ChkLangSeq.Checked = False
        End If
        BtnCreate.Visible = False
        bEdit = False

        CPSaveCreate = CP_Child_SaveCreate.Create
        ModifyButtonChild(CPSaveCreate)

        If cmbParentObjectType.SelectedIndex = 0 Then
            lblCPType.Text = "CP Form"
            BtnNew.Enabled = False
            BtnPrevious.Enabled = False
            CmbLang.Enabled = False
            BtnNext.Enabled = False
            BtnNavigateFirst.Enabled = False
            BtnNavigateLast.Enabled = False
            BtnSetting.Visible = False
            ChkLangSeq.Visible = False
            BtnCreateChild.Enabled = False
            ClearUI()
        Else
            lblCPType.Text = "CP Form - " & cmbParentObjectType.Text
            CmbLang.Enabled = True
            BtnNew.Enabled = True
            BtnPrevious.Enabled = True
            BtnNext.Enabled = True
            BtnNavigateFirst.Enabled = True
            BtnNavigateLast.Enabled = True
            BtnSetting.Visible = True
            BtnCreateChild.Enabled = True
        End If

        Panel1.Controls.Clear()

        If cmbParentObjectType.SelectedIndex <> 0 Then
            If Me.ActionType = Action.Create Then
                BtnSetting.Visible = False
                ChkLangSeq.Visible = False
                If Microsoft.VisualBasic.Left(cmbParentObjectType.Text, 2) = "@@" Then
                    BtnCreate.Visible = True
                End If
                objRet = New CPRetrieve(Me.ActionType)
                objRet.Retrieve(XmlSaveFolderPath)
                If bChildObjectSelected Then
                    ClearUI()
                End If
                SaveData(False)
                LoadControls()
                objectIndex = cmbParentObjectType.SelectedIndex
                If objRet.ParentObjectFileName.Count <> 0 Then
                    For i As Integer = 0 To objRet.ParentObjectFileName.Count - 1
                        Dim fname As String = Path.GetFileNameWithoutExtension(objRet.ParentObjectFileName(i).ToString)
                        If InStr("(" & cmbParentObjectType.Text & ")", "(" & fname & ")") > 0 Or InStr(cmbParentObjectType.Text & "-", fname & "-") > 0 Then
                            OpenMyFile(objRet.ParentObjectFileName(i))
                            Dim xInt As Integer = TranslationFileExits(objRet.ParentObjectFileName(i))
                            If xInt = 0 Then
                                MsgBox("File is already translated!", MsgBoxStyle.Exclamation, "Warning!")
                            ElseIf xInt = 1 Then
                                MsgBox("File might be sent to translation!", MsgBoxStyle.Exclamation, "Warning!")
                            End If
                        End If
                    Next
                End If

                If objRet.ChildObjectFileName.Count <> 0 Then
                    AddChildFileToCmb(objRet.ChildObjectFileName)
                End If

            Else
                Dim objSeq As New Cls_LangSequencer
                objSeq.CurrentDirectory = CurrentDirectory
                If objSeq.SequenceAvailable(cmbParentObjectType.Text) Then
                    ChkLangSeq.Visible = True
                Else
                    ChkLangSeq.Visible = False
                End If
                LoadControls()
                If objRet.ParentObjectFileName.Count <> 0 Then
                    'Replace(sFolderPath, "01-Input-B\", "05-Output\")
                    Dim file As String = objRet.ParentObjectFileName(cmbParentObjectType.SelectedIndex - 1)
                    file = Replace(file, "01-Input\", "05-Output\")
                    OpenMyFile(file)
                End If

                If objRet.ChildObjectFileName.Count <> 0 Then
                    AddChildFileToCmb(objRet.ChildObjectFileName)
                End If
            End If

        End If

        bChildObjectSelected = False

    End Sub

    Private Sub AddChildFileToCmb(ByVal arrChildList As ArrayList)
        cmbChildObjectType.Items.Clear()
        cmbChildObjectType.Enabled = False
        If arrChildList.Count = 0 Then
            Exit Sub
        End If

        cmbChildObjectType.Items.Add("[Select Child File]")
        For i As Integer = 0 To arrChildList.Count - 1
            If ObjectId.GetParentID(XmlFileName, True) = ObjectId.GetParentID(System.IO.Path.GetFileName(arrChildList(i)), True) Then
                cmbChildObjectType.Items.Add(System.IO.Path.GetFileName(arrChildList(i)))
            End If

        Next
        If cmbChildObjectType.Items.Count > 1 Then
            cmbChildObjectType.SelectedIndex = 0
            cmbChildObjectType.Enabled = True
        End If

    End Sub

    Private Function TranslationFileExits(ByVal sFile As String) As Integer
        Try
            Dim sLang As New ArrayList
            sLang.Add("en_US")
            If Me.ActionType = Action.Create Then
                sLang.Add("fr_FR")
                sLang.Add("ru_RU")
                sLang.Add("ja_JP")
                sLang.Add("de_DE")
                sLang.Add("es_ES")
                sLang.Add("ko_KR")
                sLang.Add("pt_BR")
                sLang.Add("it_IT")
            End If

            Dim sFname As String = Path.GetFileNameWithoutExtension(sFile)
            Dim sBackFromTranslation As String = Path.GetDirectoryName(sFile)
            sBackFromTranslation = Replace(sBackFromTranslation, "01-Input-B", "03-Backfromtranslation") & "\" & sFname

            Dim sTobeTranslated As String = Replace(sBackFromTranslation, "03-Backfromtranslation", "02-TobeTranslated")

            For i As Integer = 0 To sLang.Count - 1
                If IO.File.Exists(sBackFromTranslation & "_" & sLang(i) & ".xliff") Then
                    Return 0
                ElseIf IO.File.Exists(sTobeTranslated & "_" & sLang(i) & ".xliff") Then
                    Return 1
                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return 3
    End Function

    Private Sub OpenMyFile(ByVal xmlFile As String)
        CmbLang.Enabled = False
        ClearUI()
        Try
            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor
            ' ToolStripStatusLabel1.Text = "Loading xml please wait..."
            Application.DoEvents()

            'Get the CP xml defintions (elements tags for searching in xml)
            objCPxml = New CpXml(Nothing, "", Nothing, 0)
            Dim docType As String = objCPxml.GetDocType(xmlFile)

            If docType = "" Then
                Throw New Exception("Error getting doctype")
            End If

            bEdit = True
            objCplangList = New List(Of CpLangList)

            'Get Language List from XML - the lang list should be binded to cmblang control
            Dim langList As New Dictionary(Of String, Integer)
            langList = GetLangListFromXML(xmlFile)

            CmbLang.DataSource = Nothing
            CmbLang.Items.Clear()
            CmbLang.DataSource = New BindingSource(langList, Nothing)
            CmbLang.DisplayMember = "key"
            CmbLang.ValueMember = "value"
            CmbLang.SelectedIndex = 0

            CmbLang.Enabled = True

            objCPdata = objCplangList(0).xCpData
            DisplayedIndex = 0
            TotalRecord = objCPdata.Count
            UpdateRecord()
            ShowINui(0)
            ' ToolStripStatusLabel1.Text = "xml file loaded"
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub SaveData(ByVal bCreateChildObject As Boolean)
        If objCPdata Is Nothing Then
            Exit Sub
        End If

        If objCPdata.Count = 0 Then
            Exit Sub
        End If

        If bCreateChildObject Then
            If CPSaveCreate = CP_Child_SaveCreate.Create Then
                Dim objChildCp As New CP_Child(XmlSaveFolderPath, XmlFileName)
                Dim XmlChildCpFileName As String = objChildCp.GetChildName
                XmlFileName = XmlChildCpFileName
            End If
        End If

        Try
            Dim objXmlCP As CpXml
            objXmlCP = New CpXml(objCplangList, "en_US", Cp_Definition.CP_Text, objectIndex, XmlFileName)
            xmlData = objXmlCP.WriteTempCPxml(CmbLang, bCreateChildObject)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
        End Try

        Try
            If System.IO.Directory.Exists(XmlSaveFolderPath) <> True Then
                System.IO.Directory.CreateDirectory(XmlSaveFolderPath)
            End If
        Catch ex As Exception
            MsgBox("Could not create folder - You can create the folder manually and try again!" & vbNewLine & XmlSaveFolderPath & ex.Message)
            Exit Sub
        End Try

        System.IO.File.WriteAllText(XmlSaveFolderPath & XmlFileName, xmlData, System.Text.Encoding.UTF8)

        ClearUI()
        'MsgBox("Saved Successfully." & vbNewLine & XmlSaveFolderPath & "01-Input-B\" & XmlFileName, MsgBoxStyle.Information, "Status")
    End Sub

    Private objTblLayoutPanel As New TableLayoutPanel
    Public Property XmlSaveFolderPath As String
    Public Property XmlFileName As String

    Private Sub LoadControls()
        Dim objCP As CP_Element
        If Me.ActionType = Action.Create Then
            objCP = New CP_Element(cmbParentObjectType.SelectedIndex, Cp_Definition.CP_Text, cmbParentObjectType.Text, Cp_Definition.CP, CP_Element.Action.Create)
        Else
            objCP = New CP_Element(cmbParentObjectType.SelectedIndex, Cp_Definition.CP_Text, cmbParentObjectType.Text, Cp_Definition.CP, CP_Element.Action.Retrieve)
        End If
        AddHandler objCP.CopyControl, AddressOf CopyToClipboard
        AddHandler objCP.PasteControl, AddressOf PasteFromClipboard
        AddHandler objCP.Lst_Textbox_Controls, AddressOf SetTextBoxControls
        AddHandler objCP.Lst_Label_Controls, AddressOf SetLabelControls
        AddHandler objCP.SpaceNextItem, AddressOf AutoSwitchToNextItem
        objTblLayoutPanel = objCP.CreateCP
        objTblLayoutPanel.Location = New Point(17, 130)
        Panel1.Controls.Add(objTblLayoutPanel)
        XmlFileName = objCP.xmlFileName 'This is picked dynamically from cp object definition file.
        TempXmlFile = XmlFileName
    End Sub

    Dim TempXmlFile As String

    Public Sub CopyToClipboard(ByVal txtbxName As String)
        Dim txtbx As New Object
        Dim toolTip1 As New ToolTip
        txtbx = objTblLayoutPanel.Controls.Item(txtbxName)

        For i As Integer = 0 To lst_textbox_Controls.Count - 1
            eTxtBox = New Object
            eTxtBox = objTblLayoutPanel.Controls.Item(lst_textbox_Controls(i))
            eTxtBox.backcolor = Color.White
        Next

        Dim index() As String = Split(txtbxName, "_")

        If txtbx.Text <> "" Then
            Clipboard.SetText(txtbx.Text)
            txtbx.BackColor = Color.LightSkyBlue
            toolTip1.Show(objCplangList(0).xCpData(DisplayedIndex).Value(index(1)).ToString, txtbx, 3000)
        End If

    End Sub

    Public Sub AutoSwitchToNextItem(ByVal txtbxName As String, ByVal lblName As String) 'On key space down event change to next language
        Dim lbl As New Object
        lbl = objTblLayoutPanel.Controls.Item(lblName)

        Dim txtbxTemp As New Object
        txtbxTemp = objTblLayoutPanel.Controls.Item(txtbxName)
        If txtbxTemp.text = "" Then
            DisplayedIndex += 1
            If DisplayedIndex + 1 >= TotalRecord Then
                MsgBox("Copy sequence for object " & lbl.text & " completed!", MsgBoxStyle.Information, "CP Sequence")
                DisplayedIndex = TotalRecord - 1
                Exit Sub
            End If
            ShowINui(DisplayedIndex)
            AutoSwitchToNextItem(txtbxName, lblName)
            Exit Sub
        End If

        If CmbLang.SelectedIndex = CmbLang.Items.Count - 1 Then
            If DisplayedIndex + 1 >= TotalRecord Then
                MsgBox("Copy sequence for object " & lbl.text & " completed!", MsgBoxStyle.Information, "CP Sequence")
                DisplayedIndex = TotalRecord - 1
                Exit Sub
            Else
                DisplayedIndex += 1
                CmbLang.SelectedIndex = 0
                AutoSwitchToNextItem(txtbxName, lblName)
                Exit Sub
            End If
        End If

        CmbLang.SelectedIndex += 1

        Dim txtbx As New Object
        txtbx = objTblLayoutPanel.Controls.Item(txtbxName)

        For i As Integer = 0 To lst_textbox_Controls.Count - 1
            eTxtBox = New Object
            eTxtBox = objTblLayoutPanel.Controls.Item(lst_textbox_Controls(i))
            eTxtBox.backcolor = Color.White
        Next

        If txtbx.Text <> "" Then
            Clipboard.SetText(txtbx.Text)
            txtbx.BackColor = Color.LightSkyBlue
        End If

    End Sub

    Public Sub PasteFromClipboard(ByVal txtbxName As String)

        Dim txtbx As Object = objTblLayoutPanel.Controls.Item(txtbxName)
        If Clipboard.GetText <> "" Then
            txtbx.Text = Clipboard.GetText
        End If
    End Sub

#Region "Gets CP data"

    Dim lst_textbox_Controls As New ArrayList
    Public Sub SetTextBoxControls(ByVal txtbxControls As ArrayList)
        lst_textbox_Controls = txtbxControls
    End Sub

    Dim lst_Label_Controls As New ArrayList
    Public Sub SetLabelControls(ByVal lblConrols As ArrayList)
        lst_Label_Controls = lblConrols
    End Sub

    ' Dim eTxtBox As ExtendedTextBox
    Dim eTxtBox As Object
    Dim objCPdata As New List(Of CPData)

    Dim objCPLangDetail As CpLangList
    Dim objCplangList As New List(Of CpLangList)

    Dim xmlData As String


    Sub UpdateRecord()
        lblRecord.Text = "Record: " & DisplayedIndex + 1 & "/" & TotalRecord
    End Sub

    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles BtnNew.Click
        bEdit = False
        SaveCPinObj()
    End Sub

    Dim bEdit As Boolean = False

    Private Sub SaveCPinObj()

        Dim xdata As New CPData
        Dim k As Integer = 0

        'Check -  If all textboxes are empty then dont add
        Dim bEmptyTextbox As Boolean = True
        For i As Integer = 0 To lst_textbox_Controls.Count - 1
            eTxtBox = New ExtendedTextbox
            eTxtBox = objTblLayoutPanel.Controls.Item(lst_textbox_Controls(i))
            If eTxtBox.Text <> "" Then
                bEmptyTextbox = False
            End If
        Next

        If bEmptyTextbox Then
            Exit Sub
        End If

        For i As Integer = 0 To lst_textbox_Controls.Count - 1
            eTxtBox = New ExtendedTextbox
            eTxtBox = objTblLayoutPanel.Controls.Item(lst_textbox_Controls(i))

            If bEdit Then
                objCPdata(DisplayedIndex).LabelName.Item(k) = lst_Label_Controls(i)
                objCPdata(DisplayedIndex).isBold.Item(k) = eTxtBox.bBold
                objCPdata(DisplayedIndex).Value.Item(k) = eTxtBox.Text
                k += 1
            Else
                xdata.LabelName.Add(lst_Label_Controls(i))
                xdata.isBold.Add(eTxtBox.bBold)
                xdata.Value.Add(eTxtBox.Text)
                If eTxtBox.bBold <> True Then
                    eTxtBox.Text = ""
                End If
            End If
        Next

        If bEdit <> True Then
            objCPdata.Add(xdata)
            TotalRecord = objCPdata.Count
            DisplayedIndex = objCPdata.Count - 1
        End If

        'objCplangList is required for multi lang support
        If objCplangList.Count <> 0 Then
            For i As Integer = 0 To objCplangList.Count - 1
                If CmbLang.Text = objCplangList(i).Lang Then
                    objCplangList(i).xCpData = objCPdata
                    Exit For
                End If
            Next
        Else
            objCPLangDetail = New CpLangList
            objCPLangDetail.Lang = CmbLang.Text
            objCPLangDetail.xCpData = objCPdata
            objCplangList.Add(objCPLangDetail)
        End If
        UpdateRecord()

    End Sub

#End Region

#Region "Button up down events"
    'Private Sub BtnNew_MouseDown(sender As Object, e As MouseEventArgs) Handles BtnNew.MouseDown
    '    BtnNew.BackgroundImage = Nothing
    'End Sub

    'Private Sub BtnNew_MouseUp(sender As Object, e As MouseEventArgs) Handles BtnNew.MouseUp
    '    BtnNew.BackgroundImage = My.Resources._New
    'End Sub

    'Private Sub BtnNext_MouseDown(sender As Object, e As MouseEventArgs) Handles BtnNext.MouseDown
    '    BtnNext.BackgroundImage = Nothing
    'End Sub

    'Private Sub BtnNext_MouseUp(sender As Object, e As MouseEventArgs) Handles BtnNext.MouseUp
    '    BtnNext.BackgroundImage = My.Resources._Next
    'End Sub

    'Private Sub BtnPrevious_MouseDown(sender As Object, e As MouseEventArgs) Handles BtnPrevious.MouseDown
    '    BtnPrevious.BackgroundImage = Nothing
    'End Sub

    'Private Sub BtnPrevious_MouseUp(sender As Object, e As MouseEventArgs) Handles BtnPrevious.MouseUp
    '    BtnPrevious.BackgroundImage = My.Resources.Previous
    'End Sub

    Private Sub BtnSetting_MouseDown(sender As Object, e As MouseEventArgs) Handles BtnSetting.MouseDown
        BtnSetting.BackgroundImage = Nothing
    End Sub

    Private Sub BtnSetting_MouseUp(sender As Object, e As MouseEventArgs) Handles BtnSetting.MouseUp
        BtnSetting.BackgroundImage = My.Resources.rsz_sett
    End Sub
#End Region

    Private Sub ClearUI()
        objCPdata = New List(Of CPData)

        For i As Integer = 0 To lst_textbox_Controls.Count - 1
            eTxtBox = New Object
            eTxtBox = objTblLayoutPanel.Controls.Item(lst_textbox_Controls(i))
            eTxtBox.Text = ""
        Next
        TotalRecord = 0
        DisplayedIndex = 0
        lblRecord.Text = "Record: 0/0"
    End Sub
    Dim enUsToolTip As String
    Private Sub ShowINui(ByRef DisplayIndex As Integer)

        Dim k As Integer = 0
        Try
            For i As Integer = 0 To lst_textbox_Controls.Count - 1
                eTxtBox = New ExtendedTextBox
                eTxtBox = objTblLayoutPanel.Controls.Item(lst_textbox_Controls(i))
                eTxtBox.Text = revert_xml(objCPdata(DisplayIndex).Value.Item(k))
                enUsToolTip = objCplangList(0).xCpData(DisplayIndex).Value.Item(k)
                k += 1
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


    End Sub

    Dim objCPxml As CpXml
    Dim xmlFile As String = ""

    Private Sub CmbLang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbLang.SelectedIndexChanged
        BtnNext.Enabled = True
        BtnPrevious.Enabled = True
        Try
            If CmbLang.Enabled Then
                For i As Integer = 0 To objCplangList.Count - 1
                    If CmbLang.Text = objCplangList(i).Lang Then
                        objCPdata = objCplangList(i).xCpData
                        'DisplayedIndex = 0
                        TotalRecord = objCPdata.Count
                        ShowINui(DisplayedIndex)
                        UpdateRecord()
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Function GetLangListFromXML(ByVal xmlFile As String) As Dictionary(Of String, Integer)
        Dim langList As New Dictionary(Of String, Integer)
        'First Default lang is en_US, so add it as first iterm
        Dim counter As Integer = 1
        langList.Add("en_US", 0)

        Try
            'lst_Label_Controls has all the element name, loop and get lang list
            For k As Integer = 0 To lst_Label_Controls.Count - 1
                Dim xd As New Xml.XmlDocument
                xd.XmlResolver = Nothing
                xd.Load(xmlFile)

                Dim xNodeList As XmlNodeList
                xNodeList = xd.GetElementsByTagName((Replace(Replace(clean_element(lst_Label_Controls(k)), "<", ""), ">", "")))

                Dim MyAttributes As XmlAttributeCollection
                Dim str As String = ""

                For i As Integer = 0 To xNodeList.Count - 1

                    Dim blangFound As Boolean = False
                    If xNodeList(i).Attributes.Count > 0 Then
                        MyAttributes = xNodeList(i).Attributes
                        Dim att As XmlAttribute
                        For Each att In MyAttributes
                            If InStr(att.Name, "lang") > 0 Then
                                If langList.ContainsKey(att.Value) <> True Then
                                    langList.Add(att.Value, counter)
                                    counter += 1
                                End If
                            End If
                        Next

                    End If
                Next
            Next

            Dim LastIndexLang As Integer = 0
            If Me.ActionType = Action.Create Then
                LastIndexLang = 0
            Else
                LastIndexLang = langList.Count - 1
            End If

            For i As Integer = 0 To LastIndexLang
                objCPLangDetail = New CpLangList
                objCPLangDetail.Lang = langList.Keys(i)
                objCPLangDetail.xCpData = openxml(xmlFile, langList.Keys(i))
                objCplangList.Add(objCPLangDetail)
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return langList
    End Function
    Private Function openxml(ByVal xmlfile As String, ByVal lang As String) As List(Of CPData)

        Dim xData As CPData
        objCPdata = New List(Of CPData)
        Try
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(xmlfile)

            Dim xNodelist As XmlNodeList
            xNodelist = xd.GetElementsByTagName("MainTag")

            For i As Integer = 0 To xNodelist.Count - 1
                xData = New CPData
                If xNodelist(i).HasChildNodes Then
                    For j As Integer = 0 To xNodelist(i).ChildNodes.Count - 1
                        If xNodelist(i).ChildNodes(j).Name <> "SubTag" Then
                            xData.isBold.Add(True)
                            xData.LabelName.Add(Replace(xNodelist(i).ChildNodes(j).Name, "-", " "))
                            xData.Value.Add(revert_xml(xNodelist(i).ChildNodes(j).InnerText))
                        Else
                            Dim xSubNodelist As XmlNodeList = xNodelist(i).ChildNodes(j).ChildNodes
                            Dim bfound As Boolean = False
                            For k As Integer = 0 To xSubNodelist.Count - 1
                                If xSubNodelist(k).HasChildNodes Then
                                    Dim Myatt As XmlAttributeCollection = xSubNodelist(k).Attributes
                                    Dim att As XmlAttribute
                                    For Each att In Myatt
                                        If att.Name = "lang" Then
                                            If LCase(att.Value) = LCase(lang) Then
                                                bfound = True
                                                xData.isBold.Add(False)
                                                xData.LabelName.Add(xSubNodelist(k).Name)
                                                xData.Value.Add(revert_xml(xSubNodelist(k).InnerText))
                                                Exit For
                                            End If
                                        End If
                                    Next
                                End If
                            Next
                            If bfound <> True Then
                                xData.isBold.Add(False)
                                xData.LabelName.Add(Replace(xNodelist(i).ChildNodes(j).Name, "-", " "))
                                xData.Value.Add("")
                            End If
                        End If
                    Next
                End If
                objCPdata.Add(xData)
            Next i

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return objCPdata
    End Function

    Dim nTempVal As New ArrayList

#Region "Navigation Buttons"
    Private Sub BtnPrevious_Click(sender As Object, e As EventArgs) Handles BtnPrevious.Click

        Try
            If objCPdata Is Nothing Or objCPdata.Count = 0 Then
                Exit Sub
            End If

            If bEdit <> True Then
                ShowINui(DisplayedIndex)
                bEdit = True
                Exit Sub
            End If

            SaveCPinObj()

            BtnNext.Enabled = True
            ' BtnNext.BackgroundImage = Nothing

            If DisplayedIndex > 0 Then
                DisplayedIndex -= 1
            Else
                BtnPrevious.Enabled = False
                'BtnPrevious.BackgroundImage = Nothing
            End If

            ShowINui(DisplayedIndex)

            UpdateRecord()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
        End Try
    End Sub

    Private Sub BtnNext_Click(sender As Object, e As EventArgs) Handles BtnNext.Click

        Try
            If objCPdata Is Nothing Or objCPdata.Count = 0 Then
                Exit Sub
            End If

            If bEdit <> True Then
                ShowINui(DisplayedIndex)
                bEdit = True
                Exit Sub
            End If

            SaveCPinObj()

            BtnPrevious.Enabled = True
            ' BtnPrevious.BackgroundImage = My.Resources.Previous

            If DisplayedIndex + 1 < TotalRecord Then
                DisplayedIndex += 1
            Else
                BtnNext.Enabled = False
                'BtnNext.BackgroundImage = Nothing
            End If

            ShowINui(DisplayedIndex)

            UpdateRecord()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
        End Try
    End Sub

    Private Sub BtnNavigateLast_Click(sender As Object, e As EventArgs) Handles BtnNavigateLast.Click
        Try
            If objCPdata Is Nothing Or objCPdata.Count = 0 Then
                Exit Sub
            End If
            DisplayedIndex = objCPdata.Count - 1
            ShowINui(DisplayedIndex)
            UpdateRecord()
            BtnPrevious.Enabled = True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
        End Try
    End Sub

    Private Sub BtnNavigateFirst_Click(sender As Object, e As EventArgs) Handles BtnNavigateFirst.Click
        If objCPdata Is Nothing Or objCPdata.Count = 0 Then
            Exit Sub
        End If
        DisplayedIndex = 0
        ShowINui(DisplayedIndex)
        UpdateRecord()
        BtnNext.Enabled = True

    End Sub

#End Region

    Private Sub BtnCreate_Click(sender As Object, e As EventArgs) Handles BtnCreate.Click
        'Here is the idea: 
        'The user selects 13.8 e.g. in the C/P definition file we have something which allows to know that import is possible, for instance a double @
        '@@LMS - Item Types (13.8)
        'If there is a double @ for this particular type, we can import. We have then a button import (only for create, obviously). 
        'If we click we can select the input file.
        'It then checks based on the csv definition if the fields are well present (in the case of 13.8, Item Type ID & Description). If not -> error message.
        'If it works, then it needs to check if there are already entries in the current c/P xml. If yes, msgbox: “The xml file is already existing. Checking if new entries”.
        'Then it will look in the csv file, entry per entry and check if it already exists. If not, it’s added at the end of the xml file. Note that for some files, there are more columns than those required in the C/P definition. Just ignore those.
        'At the end of the process, we need to get a message: 23/24 entries added. (23 is the number added, 24 is the total nber of entries in the csv).

        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False

            Dim opnFdialog As New OpenFileDialog
            opnFdialog.Filter = "CSV file *.csv|*.csv"

            Dim objCPMore As New CP_More

            If opnFdialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                objCPMore.CheckCP(objCPdata, lst_Label_Controls, opnFdialog.FileName)
            Else
                Exit Sub
            End If

            TotalRecord = objCPdata.Count
            DisplayedIndex = objCPdata.Count - 1

            'objCplangList is required for multi lang support
            If objCplangList.Count <> 0 Then
                For i As Integer = 0 To objCplangList.Count - 1
                    If CmbLang.Text = objCplangList(i).Lang Then
                        objCplangList(i).xCpData = objCPdata
                        Exit For
                    End If
                Next
            Else
                objCPLangDetail = New CpLangList
                objCPLangDetail.Lang = CmbLang.Text
                objCPLangDetail.xCpData = objCPdata
                objCplangList.Add(objCPLangDetail)
            End If

            UpdateRecord()

            ShowINui(DisplayedIndex)

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "CPMore Error!")
        Finally
            Me.Cursor = Cursors.Default
            Me.Enabled = True
        End Try

    End Sub

    Private Sub ChkLangSeq_CheckedChanged(sender As Object, e As EventArgs) Handles ChkLangSeq.CheckedChanged
        Dim cmbindex As Integer = cmbParentObjectType.SelectedIndex
        If ChkLangSeq.Checked Then
            Dim objSequencer As New Cls_LangSequencer
            objSequencer.CurrentDirectory = CurrentDirectory
            objCplangList = objSequencer.GetSequence(cmbParentObjectType.Text, objCplangList)
            Dim langList As New Dictionary(Of String, Integer)

            For i As Integer = 0 To objCplangList.Count - 1
                langList.Add(objCplangList(i).Lang, i)
            Next

            CmbLang.DataSource = Nothing
            CmbLang.Items.Clear()
            CmbLang.DataSource = New BindingSource(langList, Nothing)
            CmbLang.DisplayMember = "key"
            CmbLang.ValueMember = "value"
            CmbLang.SelectedIndex = 0

            CmbLang.Enabled = True

            objCPdata = objCplangList(0).xCpData
            DisplayedIndex = 0
            TotalRecord = objCPdata.Count
            UpdateRecord()
            ShowINui(0)
        Else
            cmbParentObjectType.SelectedIndex = 0
            cmbParentObjectType.SelectedIndex = cmbindex
        End If
    End Sub

    Private Sub BtnSetting_Click(sender As Object, e As EventArgs) Handles BtnSetting.Click
        BtnSetting.BackgroundImage = My.Resources.rsz_sett
        Dim objLangSequencer As New Form_LangSequencer
        objLangSequencer.CurrentDirectory = CurrentDirectory
        objLangSequencer.CPobjectType = cmbParentObjectType.Text
        objLangSequencer.AssignLang(objCplangList)
        objLangSequencer.ShowDialog()
        objCplangList = objLangSequencer.CPLanglist

        Dim langList As New Dictionary(Of String, Integer)

        For i As Integer = 0 To objCplangList.Count - 1
            langList.Add(objCplangList(i).Lang, i)
        Next

        CmbLang.DataSource = Nothing
        CmbLang.Items.Clear()
        CmbLang.DataSource = New BindingSource(langList, Nothing)
        CmbLang.DisplayMember = "key"
        CmbLang.ValueMember = "value"
        CmbLang.SelectedIndex = 0

        CmbLang.Enabled = True

        objCPdata = objCplangList(0).xCpData
        DisplayedIndex = 0
        TotalRecord = objCPdata.Count
        UpdateRecord()
        ShowINui(0)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub BtnCreateChild_Click(sender As Object, e As EventArgs) Handles BtnCreateChild.Click

        If objCPdata Is Nothing Then
            Exit Sub
        End If

        If Me.ActionType = Action.Create Then

            If objCPdata.Count = 0 Then
                MsgBox("No data to create child object!" & vbCrLf & "Add some data and then Click Create child object", MsgBoxStyle.Information, "Warning!")
                Exit Sub
            End If

            If CPSaveCreate = CP_Child_SaveCreate.Save Then
                XmlFileName = cmbChildObjectType.Text
            End If

            SaveData(True)
            ClearUI()
            cmbParentObjectType.SelectedIndex = objectIndex
            Dim arr As New ArrayList
            arr.Add(XmlFileName)
            AddChildFileToCmb(arr)
            cmbChildObjectType.SelectedIndex = 1
        End If
        XmlFileName = TempXmlFile
    End Sub

    Private Sub cmbChildObjectType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbChildObjectType.SelectedIndexChanged
        CPSaveCreate = CP_Child_SaveCreate.Create
        bChildObjectSelected = False
        If cmbChildObjectType.SelectedIndex > 0 Then
            ClearUI()
            OpenMyFile(XmlSaveFolderPath & cmbChildObjectType.Text)
            bChildObjectSelected = True
            CPSaveCreate = CP_Child_SaveCreate.Save
            lblCPType.Text = "CP Form: Child File - " & cmbChildObjectType.Text
        End If
        ModifyButtonChild(CPSaveCreate)
    End Sub

    Private Sub ModifyButtonChild(ByVal CP As CP_Child_SaveCreate)
        Select Case CP
            Case CP_Child_SaveCreate.Create
                BtnCreateChild.Text = "Create New Child file"
            Case CP_Child_SaveCreate.Save
                BtnCreateChild.Text = "Save Child file"
        End Select
    End Sub

End Class

Class CPRetrieve
    Dim _ActionType As Action
    Public Sub New(ByVal ActionType As Action)
        _ActionType = ActionType
    End Sub

    Private _ParentObjectfileName As New ArrayList

    Public ReadOnly Property ParentObjectFileName As ArrayList
        Get
            Return _ParentObjectfileName
        End Get
    End Property

    Private _ChildObjectFileName As New ArrayList

    Public ReadOnly Property ChildObjectFileName As ArrayList
        Get
            Return (_ChildObjectFileName)
        End Get
    End Property

    Dim objCPxml As CpXml

    Private CP_temp As New Dictionary(Of String, Integer)
    Private CP_Text_temp As New ArrayList

    Public CP As New Dictionary(Of String, Integer)
    Public CP_Text As New ArrayList

    Public Sub Retrieve(ByVal sFolderPath As String)
        Try
            'Load all definition to variable
            Cp_Definition.Process()
            CP_temp = Cp_Definition.CP
            CP_Text_temp = Cp_Definition.CP_Text

            'Initiliage Cp list
            CP.Add("[Select object]", 0)

            Dim myFolder As String = ""
            'Out folder will be searched
            Select Case _ActionType
                Case Action.Create
                    myFolder = sFolderPath
                Case Action.Retrieve
                    myFolder = Replace(sFolderPath, "01-Input-B\", "05-Output\")
            End Select

            If System.IO.Directory.Exists(myFolder) <> True Then
                Throw New Exception(myFolder & vbNewLine & "Please check the folder does not exist!")
            End If

            'Now check file present
            Try
                For Each file In My.Computer.FileSystem.GetFiles(myFolder)
                    'Get the CP xml defintions (elements tags for searching in xml)
                    If LCase(Path.GetExtension(file)) = ".xml" Then
                        objCPxml = New CpXml(Nothing, "", Nothing, 0)
                        Dim docType As String = objCPxml.GetDocType(file)
                        If docType <> String.Empty Then
                            If objCPxml.isChildFile(file) Then
                                _ChildObjectFileName.Add(file.ToString)
                            Else
                                If bCP_Found(docType, file.ToString) Then
                                    _ParentObjectfileName.Add(file.ToString)
                                End If
                            End If
                          
                        End If
                    End If
                Next
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
        End Try

    End Sub

    Private Function bCP_Found(ByVal docType As String, ByVal sFile As String) As Boolean
        Try

            For i As Integer = 0 To CP_temp.Count - 1
                If LCase(clean_element(CP_temp.Keys(i))) = LCase(docType) Then
                    Dim j As Integer = CP.Count
                    Try
                        CP.Add(docType, j)
                        Return True
                        Exit For
                    Catch ex As System.ArgumentException
                        'do nothing
                    Catch ex As Exception
                        Throw New Exception(ex.Message)
                    End Try
                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return False
    End Function

End Class 'Retrieve CP data if any found.


Public Class CPData
    Public Property LabelName As New ArrayList
    Public Property isBold As New ArrayList
    Public Property Value As New ArrayList
End Class

Public Class CpLangList
    Public Property Lang As String
    Public Property xCpData As List(Of CPData)
End Class

Public Class CP_Child

    Dim _XmlSaveFolderPath As String
    Dim _XmlFileName As String

    Public Sub New(ByVal XmlSaveFolderPath As String, ByVal XmlFileName As String)
        _XmlFileName = XmlFileName
        _XmlSaveFolderPath = XmlSaveFolderPath
    End Sub

    Public Function GetChildName() As String
        Dim sFileNumber As String = System.IO.Path.GetFileNameWithoutExtension(_XmlFileName)
        Dim PID As String = ""
        Dim sChildFile As String = ""
        Try
            For Each F In System.IO.Directory.GetFiles(_XmlSaveFolderPath, sFileNumber & "*.xml")
                PID = ObjectId.GetParentID(F, True)
                If PID = sFileNumber Then
                    Exit For
                End If
            Next

            If PID = String.Empty Then
                PID = sFileNumber
            End If

            For i As Integer = 1 To 100
                sChildFile = _XmlSaveFolderPath & PID & "." & i & ".xml"
                If Not System.IO.File.Exists(sChildFile) Then
                    sChildFile = PID & "." & i & ".xml"
                    Return sChildFile
                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return sChildFile
    End Function

    Public Function FindChildFile() As ArrayList
        Dim aChildFile As New ArrayList
        Dim sFileNumber As String = System.IO.Path.GetFileNameWithoutExtension(_XmlFileName)
        Try
            For Each F In System.IO.Directory.GetFiles(_XmlSaveFolderPath, sFileNumber & "*.xml")
                If isChildFile(F) Then
                    aChildFile.Add(F)
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error @FindChildFile" & vbNewLine & ex.Message)
        End Try
        Return aChildFile
    End Function

    Private Function isChildFile(ByVal xmlFile As String) As Boolean
        Try
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(xmlFile)
            Dim xNodeList As XmlNodeList = xd.GetElementsByTagName("isChildFile")
            If xNodeList.Count <> 0 Then
                Return CBool(xNodeList(0).InnerText)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return False
    End Function

End Class