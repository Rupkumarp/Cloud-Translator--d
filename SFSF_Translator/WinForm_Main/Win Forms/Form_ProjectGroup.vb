Imports System.Windows.Forms
Imports System.IO

Public Class Form_ProjectGroup

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If Not TextBox1.Text.Trim.Length > 0 Then
            MsgBox("Enter Project Group Name", MsgBoxStyle.Exclamation, "Project Group Name")
            Exit Sub
        End If

        Dim ProjectgroupNamelist As ArrayList = ProjectManagement.GetProjectGroupNameList

        For i As Integer = 0 To ProjectgroupNamelist.Count - 1
            If ProjectgroupNamelist(i).ToLower = TextBox1.Text.ToLower Then
                MsgBox("The Project Group is already available!", MsgBoxStyle.Information, "Cannot Add the project grou name")
                Exit Sub
            End If
        Next

        Try
            ProjectManagement.AddProjectGroupName(TextBox1.Text)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class

'Public Class ProjectGroupAdditionalFunctions
'    Public Shared Sub UpdateProjectGroup(ByVal ProjectList() As String)
'        Try
'            Using writer As StreamWriter = New StreamWriter(appData & DefinitionFiles.ProjectGroupName, False)
'                For i As Integer = 0 To UBound(ProjectList)
'                    If ProjectList(i).Trim.Length > 0 Then
'                        writer.WriteLine(ProjectList(i))
'                    End If
'                Next
'            End Using
'        Catch ex As Exception
'            Throw New Exception("Error Updatint Project Group Name" & vbNewLine & ex.Message)
'        End Try
'    End Sub

'    Public Shared Sub UpdateProjectGroup(ByVal OldProjectGroupName As String, ByVal NewProjectGroupName As String)
'        Try
'            Dim ProjectList As String() = GetProjectGroupList()
'            Using writer As StreamWriter = New StreamWriter(appData & DefinitionFiles.ProjectGroupName, False)
'                For i As Integer = 0 To UBound(ProjectList)
'                    If ProjectList(i).ToLower.Trim = OldProjectGroupName.ToLower.Trim Then
'                        ProjectList(i) = NewProjectGroupName
'                    End If
'                    If ProjectList(i).Trim.Length > 0 Then
'                        writer.WriteLine(ProjectList(i))
'                    End If
'                Next
'            End Using
'        Catch ex As Exception
'            Throw New Exception("Error Updatint Project Group Name" & vbNewLine & ex.Message)
'        End Try
'    End Sub

'    Public Shared Sub DeleteProjectGroup(ByVal DeleteProjectGroupName As String)
'        Try
'            Dim ProjectList As String() = GetProjectGroupList()
'            Using writer As StreamWriter = New StreamWriter(appData & DefinitionFiles.ProjectGroupName, False)
'                For i As Integer = 0 To UBound(Projectlist)
'                    If ProjectList(i).ToLower.Trim <> DeleteProjectGroupName.ToLower.Trim Then
'                        If ProjectList(i).Trim.Length > 0 Then
'                            writer.WriteLine(ProjectList(i))
'                        End If
'                    End If
'                Next
'            End Using
'        Catch ex As Exception
'            Throw New Exception("Error Updatint Project Group Name" & vbNewLine & ex.Message)
'        End Try
'    End Sub


'    Public Shared Function GetProjectGroupList() As String()
'        Dim sProjectGroupNames As String() = System.IO.File.ReadAllLines(appData & DefinitionFiles.ProjectGroupName)
'        Return sProjectGroupNames
'    End Function

'    Public Shared Function GetProjectGroupList(ByVal ProjectList() As String) As String()

'        Dim bFound As Boolean

'        Dim sProjectGroupNames As String() = System.IO.File.ReadAllLines(appData & DefinitionFiles.ProjectGroupName)
'        Dim NewProjectList As String() = sProjectGroupNames
'        Dim counter As Integer = UBound(sProjectGroupNames) + 1
'        For i As Integer = 0 To UBound(ProjectList)
'            bFound = False
'            For j As Integer = 0 To UBound(sProjectGroupNames)
'                If Not IsNothing(ProjectList(i)) And Not IsNothing(sProjectGroupNames(j)) Then
'                    If ProjectList(i).ToLower.Trim = sProjectGroupNames(j).ToLower.Trim Then
'                        bFound = True
'                        Exit For
'                    End If
'                End If

'            Next j
'            If Not bFound Then
'                ReDim Preserve NewProjectList(counter)
'                NewProjectList(counter) = ProjectList(i)
'                counter += 1
'            End If
'        Next i

'        Return NewProjectList

'    End Function

'End Class
