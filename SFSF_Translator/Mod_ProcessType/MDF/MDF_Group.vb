Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Xml.Linq
Imports System.Xml
Public Class MDF_Group
    Private obj_api As cls_API = New cls_API()
    Private Sub MDF_Group_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

#Region "Control Events"
    Private Sub button1_Click(sender As Object, e As EventArgs) Handles button1.Click
        If radioButton1.Checked = True Then ADD()
        If radioButton2.Checked = True Then EDIT()
        If radioButton3.Checked = True Then DELETE()
    End Sub
    Private Sub radioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles radioButton1.CheckedChanged
        listGroupName.Enabled = False
        checkedListBox1.Enabled = True
        textBox1.Enabled = True
        textBox1.Text = String.Empty
        button1.Text = "Add New Group"
        chekListTickUntick(False)
    End Sub
    Private Sub radioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles radioButton2.CheckedChanged
        listGroupName.Enabled = True
        checkedListBox1.Enabled = True
        textBox1.Enabled = False
        button1.Text = "Edit Group"
        If listGroupName.Items.Count > 0 Then
            listGroupName.SelectedIndex = 0
            Read_GroupItems(listGroupName.SelectedItem.ToString())
            textBox1.Text = listGroupName.SelectedItem.ToString()
        End If
    End Sub
    Private Sub radioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles radioButton3.CheckedChanged
        listGroupName.Enabled = True
        checkedListBox1.Enabled = False
        textBox1.Enabled = False
        textBox1.Text = String.Empty
        button1.Text = "Delete Group"
        chekListTickUntick(False)
        If listGroupName.Items.Count > 0 Then
            listGroupName.SelectedIndex = 0
            Read_GroupItems(listGroupName.SelectedItem.ToString())
            textBox1.Text = listGroupName.SelectedItem.ToString()
        End If
    End Sub
    Private Sub listGroupName_SelectedValueChanged(sender As Object, e As EventArgs) Handles listGroupName.SelectedValueChanged
        Read_GroupItems(listGroupName.SelectedItem.ToString())
        If radioButton2.Checked OrElse radioButton3.Checked Then textBox1.Text = listGroupName.SelectedItem.ToString()
    End Sub
#End Region

#Region "Methods"
    Private Sub LoadStatus()
        radioButton1.Checked = True
        listGroupName.Enabled = False
        button1.Text = "Add New Group"
    End Sub
    Private Sub chekListTickUntick(ByVal stat As Boolean)
        For i As Integer = 0 To checkedListBox1.Items.Count - 1
            checkedListBox1.SetItemChecked(i, stat)
        Next
    End Sub
    Private Sub ReadMDFtype()
        Dim lines = File.ReadAllLines(Application.StartupPath & "\MDFType.txt")
        For Each line In lines
            checkedListBox1.Items.Add(line)
        Next
    End Sub
    Private Sub ReadAllGroups()
        Try
            listGroupName.Items.Clear()
            Dim xmldoc As XmlDataDocument = New XmlDataDocument()
            Dim xmlnode As XmlNodeList
            Using fs As FileStream = New FileStream(Application.StartupPath & "\MDFGroup.xml", FileMode.Open, FileAccess.Read)
                xmldoc.Load(fs)
                xmlnode = xmldoc.GetElementsByTagName("Group")
                For grpcount As Integer = 0 To xmlnode.Count - 1
                    listGroupName.Items.Add(xmlnode(grpcount).ChildNodes.Item(0).InnerText.Trim())
                Next
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub Read_GroupItems(ByVal GroupName As String)
        Try
            chekListTickUntick(False)
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
    Private Function checkGroupname(ByVal GrpName As String) As Boolean
        Dim xmldoc As XmlDataDocument = New XmlDataDocument()
        Dim xmlnode As XmlNodeList
        Using fs As FileStream = New FileStream(Application.StartupPath & "\MDFGroup.xml", FileMode.Open, FileAccess.ReadWrite)
            xmldoc.Load(fs)
            xmlnode = xmldoc.GetElementsByTagName("Group")
            For grpcount As Integer = 0 To xmlnode.Count - 1
                If xmlnode(grpcount).ChildNodes.Item(0).InnerText.Trim().ToUpper() = GrpName.ToUpper() Then
                    xmldoc = Nothing
                    xmlnode = Nothing
                    Return False
                End If
            Next

            Return True
        End Using
    End Function
    Private Function createNode(ByVal MDFIds As String, ByVal _GroupName As String) As Boolean
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
    Private Sub deleteNode(ByVal MDFIds As String, ByVal _GroupName As String)
        Try
            Dim doc As XmlDocument = New XmlDocument()
            Using fileStream As FileStream = New FileStream(Application.StartupPath & "\MDFGroup.xml", FileMode.Open, FileAccess.ReadWrite)
                doc.Load(fileStream)
                Dim node As XmlNode = doc.SelectSingleNode("/MDFGroupList/Group[GroupName='" & _GroupName & "' and GroupIds='" & MDFIds & "']")
                node.ParentNode.RemoveChild(node)
                Dim newXML As String = doc.OuterXml
                fileStream.SetLength(0)
                doc.Save(fileStream)
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function ReadExistingGrpIds() As String
        Dim MDFIds As String = ""
        Dim xmldoc As XmlDataDocument = New XmlDataDocument()
        Dim xmlnode As XmlNodeList
        Using fs As FileStream = New FileStream(Application.StartupPath & "\MDFGroup.xml", FileMode.Open, FileAccess.ReadWrite)
            xmldoc.Load(fs)
            xmlnode = xmldoc.GetElementsByTagName("Group")
            For idcount As Integer = 0 To xmlnode.Count - 1
                If xmlnode(idcount).ChildNodes.Item(0).InnerText.Trim().ToUpper() = textBox1.Text.Trim().ToUpper() Then
                    Dim str_MDFids As String() = xmlnode(idcount).ChildNodes.Item(1).InnerText.Trim().Split(","c)
                    For countId As Integer = 0 To str_MDFids.Length - 1
                        MDFIds += str_MDFids(countId).ToString() & ","
                    Next
                End If
            Next
        End Using

        MDFIds = MDFIds.TrimEnd(","c)
        Return MDFIds
    End Function
    Private Function ReadCurrentGrpIds() As String
        Dim MDFtypeList As String = ""
        For i As Integer = 0 To checkedListBox1.Items.Count - 1
            Dim cc As String = checkedListBox1.GetItemCheckState(i).ToString()
            If checkedListBox1.GetItemCheckState(i).ToString().ToUpper() = "CHECKED" Then
                MDFtypeList += checkedListBox1.Items(i).ToString() & ","
            End If
        Next

        Return MDFtypeList.TrimEnd(","c)
    End Function
    Private Sub ADD()
        Try
            If checkedListBox1.CheckedItems.Count <= 0 Then
                MessageBox.Show("No MDF type is selected for create a new group.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If textBox1.Text.Trim() = "" Then
                MessageBox.Show("Please enter a group name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                textBox1.Focus()
                Return
            End If

            If Not checkGroupname(textBox1.Text) Then
                MessageBox.Show("Group Name already exist, please enter different name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                textBox1.Text = ""
                textBox1.Focus()
                Return
            Else
                obj_api.create_XMLFile()
                If createNode(ReadCurrentGrpIds(), textBox1.Text.Trim()) Then
                    MessageBox.Show("Group  [" & textBox1.Text.Trim() & "]  created successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    textBox1.Text = ""
                End If

                chekListTickUntick(False)
                ReadAllGroups()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub EDIT()
        Try
            If checkedListBox1.CheckedItems.Count <= 0 Then
                MessageBox.Show("Please select at least one MDF type.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If textBox1.Text.Trim() = "" Then
                MessageBox.Show("Please select one group.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim str_oldGrpIds As String = ReadExistingGrpIds()
            deleteNode(str_oldGrpIds, textBox1.Text.Trim())
            Dim _strMDF As String = ReadCurrentGrpIds()
            createNode(_strMDF, textBox1.Text.Trim())
            MessageBox.Show("Group  [" & textBox1.Text.Trim() & "]  edited successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            chekListTickUntick(False)
            textBox1.Text = ""
            ReadAllGroups()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub DELETE()
        Try
            If textBox1.Text.Trim() = "" Then
                MessageBox.Show("Please select a group.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim str_oldGrpIds As String = ReadExistingGrpIds()
            deleteNode(str_oldGrpIds, textBox1.Text.Trim())
            MessageBox.Show("MDF group [" & textBox1.Text.Trim() & "] deleted successfully.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            chekListTickUntick(False)
            textBox1.Text = ""
            ReadAllGroups()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
End Class