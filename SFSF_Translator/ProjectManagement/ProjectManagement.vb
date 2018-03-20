''' <summary>
''' This class is used for Project Addition,Modification and Deletion purpose.
''' It takes List of Projectgroup class object (xml serializable class) for performing the above task easily.
''' </summary>
Public Class ProjectManagement

    Public Shared Sub SaveAndReloadProject()
        XMLMethod.SaveProjectGroupList()
        LstProjectGroup = New List(Of ProjectGroup)
        LstProjectGroup = XMLMethod.GetProjectGroupListFromXml
    End Sub

    Public Shared Function GetActiveProject() As ProjectDetail
        Dim PD As ProjectDetail = Nothing
        If LstProjectGroup.Count = 0 Then
            Return PD
        End If
        Try
            Dim bFound As Boolean = False
            For i As Integer = 0 To LstProjectGroup.Count - 1
                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                    If LstProjectGroup(i).ProjectDetail(j).isCurrentProject Then
                        PD = LstProjectGroup(i).ProjectDetail(j)
                        bFound = True
                        Exit For
                    End If
                Next
                If bFound Then
                    Exit For
                End If
            Next

            If Not bFound Then
                Throw New Exception("Please select a Project")
            End If
           
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        SaveAndReloadProject()
        Return PD
    End Function

    Public Shared Function SetActiveProject(ByVal ProjectName As String) As ProjectDetail
        Dim PD As ProjectDetail = Nothing
        If LstProjectGroup.Count = 0 Then
            Return PD
        End If
        Try
            Dim bFound As Boolean = False
            For i As Integer = 0 To LstProjectGroup.Count - 1
                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                    LstProjectGroup(i).ProjectDetail(j).isCurrentProject = False
                    If CompareTwoStrings(LstProjectGroup(i).ProjectDetail(j).ProjectName, ProjectName) Then
                        LstProjectGroup(i).ProjectDetail(j).isCurrentProject = True
                    End If
                Next
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        SaveAndReloadProject()
        Return PD
    End Function

    Public Shared Function GetProjectDetail(ByVal ProjectName As String) As ProjectDetail
        Dim PD As ProjectDetail = Nothing
        If LstProjectGroup.Count = 0 Then
            Return PD
        End If
        Try
            Dim bFound As Boolean = False
            For i As Integer = 0 To LstProjectGroup.Count - 1
                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                    If CompareTwoStrings(ProjectName, LstProjectGroup(i).ProjectDetail(j).ProjectName) Then
                        PD = LstProjectGroup(i).ProjectDetail(j)
                        bFound = True
                        Exit For
                    End If
                Next
                If bFound Then
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        SaveAndReloadProject()
        Return PD
    End Function

    Public Shared Function GetProjectGroupDetail(ByVal ProjectGroupName As String) As ProjectGroup
        Dim PG As ProjectGroup = Nothing
        If LstProjectGroup.Count = 0 Then
            Return PG
        End If
        Try
            Dim bFound As Boolean = False
            For i As Integer = 0 To LstProjectGroup.Count - 1
                If CompareTwoStrings(LstProjectGroup(i).ProjectGroupName, ProjectGroupName) Then
                    PG = LstProjectGroup(i)
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return PG
    End Function

    Public Shared Function isProjectNameAvailable(ByVal ProjectName As String) As Boolean
        If LstProjectGroup.Count = 0 Then
            Return False
        End If
        Try
            For i As Integer = 0 To LstProjectGroup.Count - 1
                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                    If CompareTwoStrings(ProjectName, LstProjectGroup(i).ProjectDetail(j).ProjectName) Then
                        Return True
                    End If
                Next
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return False
    End Function

    Public Shared Sub MakeActiveProject(ByVal ProjectName As String)
        If LstProjectGroup.Count = 0 Then
            Exit Sub
        End If
        Try
            For i As Integer = 0 To LstProjectGroup.Count - 1
                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                    LstProjectGroup(i).ProjectDetail(j).isCurrentProject = False
                    If CompareTwoStrings(ProjectName, LstProjectGroup(i).ProjectDetail(j).ProjectName) Then
                        LstProjectGroup(i).ProjectDetail(j).isCurrentProject = True
                        'Exit For
                    End If
                Next
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        SaveAndReloadProject()
    End Sub

    Public Shared Sub RenameProjectName(ByVal OldProjectName As String, ByVal NewProjectName As String)
        If LstProjectGroup.Count = 0 Then
            Exit Sub
        End If
        Try
            For i As Integer = 0 To LstProjectGroup.Count - 1
                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                    If CompareTwoStrings(OldProjectName, LstProjectGroup(i).ProjectDetail(j).ProjectName) Then
                        LstProjectGroup(i).ProjectDetail(j).ProjectName = NewProjectName
                        SaveAndReloadProject()
                        Exit Sub
                    End If
                Next
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Sub RenameProjectGroupName(ByVal OldProjectGroupName As String, ByVal NewProjectGroupName As String)
        If LstProjectGroup.Count = 0 Then
            Exit Sub
        End If
        Try
            For i As Integer = 0 To LstProjectGroup.Count - 1
                If CompareTwoStrings(LstProjectGroup(i).ProjectGroupName, OldProjectGroupName) Then
                    LstProjectGroup(i).ProjectGroupName = NewProjectGroupName
                    For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                        LstProjectGroup(i).ProjectDetail(j).ProjectGroupName = NewProjectGroupName
                        SaveAndReloadProject()
                        Exit Sub
                    Next
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Sub MoveProjectToAnotherGroup(ByVal ProjectName As String, ByVal NewProjectGroup As String)
        If LstProjectGroup.Count = 0 Then
            Exit Sub
        End If
        Try
            Dim PD As ProjectDetail = Nothing
            Dim bFound As Boolean = False
            For i As Integer = 0 To LstProjectGroup.Count - 1
                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                    If CompareTwoStrings(ProjectName, LstProjectGroup(i).ProjectDetail(j).ProjectName) Then
                        PD = LstProjectGroup(i).ProjectDetail(j)
                        LstProjectGroup(i).ProjectDetail.Remove(PD)
                        PD.ProjectGroupName = NewProjectGroup
                        bFound = True
                        Exit For
                    End If
                Next
                If bFound Then
                    Exit For
                End If
            Next

            If PD Is Nothing Then
                Throw New Exception("Could not find Project Name")
            End If
            For i As Integer = 0 To LstProjectGroup.Count - 1
                If CompareTwoStrings(LstProjectGroup(i).ProjectGroupName, NewProjectGroup) Then
                    LstProjectGroup(i).ProjectDetail.Add(PD)
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        SaveAndReloadProject()
    End Sub

    Public Shared Sub DeleteProject(ByVal ProjectName As String)
        If LstProjectGroup.Count = 0 Then
            Exit Sub
        End If
        Try
            Dim PD As ProjectDetail = Nothing
            For i As Integer = 0 To LstProjectGroup.Count - 1
                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                    If CompareTwoStrings(ProjectName, LstProjectGroup(i).ProjectDetail(j).ProjectName) Then
                        PD = LstProjectGroup(i).ProjectDetail(j)
                        LstProjectGroup(i).ProjectDetail.Remove(PD)
                        SaveAndReloadProject()
                        Exit Sub
                    End If
                Next
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Deletes ProjectgroupName but moves Projects to Default group
    ''' </summary>
    ''' <param name="ProjectGroupName"></param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteProjectGroupWithoutPurge(ByVal ProjectGroupName As String)
        If LstProjectGroup.Count = 0 Then
            Exit Sub
        End If
        Try
            'First "Default Group" if not there create it
            If Not isProjectGroupNameAvailable("Default") Then
                AddProjectGroupName("Default")
            End If

            'Now Get the Default group index
            Dim DefaultGroupIndex As Integer = 0
            For i As Integer = 0 To LstProjectGroup.Count - 1
                If CompareTwoStrings(LstProjectGroup(i).ProjectGroupName, "Default") Then
                    DefaultGroupIndex = i
                    Exit For
                End If
            Next

            'Now Move all the projects from want to delete projectgroupname to Default
            For i As Integer = 0 To LstProjectGroup.Count - 1
                If CompareTwoStrings(LstProjectGroup(i).ProjectGroupName, ProjectGroupName) Then
                    LstProjectGroup(i).ProjectGroupName = ProjectGroupName
                    For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                        Dim PD As ProjectDetail = LstProjectGroup(i).ProjectDetail(j)
                        PD.ProjectGroupName = "Default"
                        PD.isMasterProject = False
                        LstProjectGroup(DefaultGroupIndex).ProjectDetail.Add(PD)
                    Next
                    Dim PG As ProjectGroup = LstProjectGroup(i)
                    LstProjectGroup.Remove(PG)
                    SaveAndReloadProject()
                    Exit Sub
                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Warning Deletes ProjectGroup and All Related Projects in it.
    ''' </summary>
    ''' <param name="ProjectGroupName"></param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteProjectGroupWithPurge(ByVal ProjectGroupName As String)
        If LstProjectGroup.Count = 0 Then
            Exit Sub
        End If
        Try
            'Deletes ProjectGroup
            For i As Integer = 0 To LstProjectGroup.Count - 1
                If CompareTwoStrings(LstProjectGroup(i).ProjectGroupName, ProjectGroupName) Then
                    Dim PG As ProjectGroup = LstProjectGroup(i)
                    LstProjectGroup.Remove(PG)
                    SaveAndReloadProject()
                    Exit Sub
                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Sub AddProjectGroupName(ByVal ProjectGroupName As String)

        Try
            If isProjectGroupNameAvailable(ProjectGroupName) Then
                Throw New Exception("Project GroupName already available!")
            End If
            Dim PG As New ProjectGroup
            PG.ProjectGroupName = ProjectGroupName
            LstProjectGroup.Add(PG)
            SaveAndReloadProject()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Public Shared Function isProjectGroupNameAvailable(ByVal ProjectGroupName As String) As Boolean
        Try
            For i As Integer = 0 To LstProjectGroup.Count - 1
                If CompareTwoStrings(LstProjectGroup(i).ProjectGroupName, ProjectGroupName) Then
                    Return True
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return False
    End Function

    Public Shared Sub SetMasterProject(ByVal ProjectGroupName As String, ByVal ProjectName As String)
        If LstProjectGroup.Count = 0 Then
            Exit Sub
        End If
        For i As Integer = 0 To LstProjectGroup.Count - 1
            If CompareTwoStrings(LstProjectGroup(i).ProjectGroupName, ProjectGroupName) Then
                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                    LstProjectGroup(i).ProjectDetail(j).isMasterProject = False
                    If CompareTwoStrings(LstProjectGroup(i).ProjectDetail(j).ProjectName, ProjectName) Then
                        LstProjectGroup(i).ProjectDetail(j).isMasterProject = True
                    End If
                Next
                SaveAndReloadProject()
                Exit For
            End If
        Next
    End Sub

    Public Shared Sub UnSetMasterProject(ByVal ProjectGroupName As String, ByVal ProjectName As String)
        If LstProjectGroup.Count = 0 Then
            Exit Sub
        End If
        For i As Integer = 0 To LstProjectGroup.Count - 1
            If CompareTwoStrings(LstProjectGroup(i).ProjectGroupName, ProjectGroupName) Then
                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                    If CompareTwoStrings(LstProjectGroup(i).ProjectDetail(j).ProjectName, ProjectName) Then
                        LstProjectGroup(i).ProjectDetail(j).isMasterProject = False
                    End If
                Next
                SaveAndReloadProject()
                Exit For
            End If
        Next
    End Sub

    Public Shared Function isMasterProject(ByVal ProjectName As String) As Boolean
        If LstProjectGroup.Count = 0 Then
            Return False
        End If
        For i As Integer = 0 To LstProjectGroup.Count - 1
            For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                If CompareTwoStrings(LstProjectGroup(i).ProjectDetail(j).ProjectName, ProjectName) Then
                    If LstProjectGroup(i).ProjectDetail(j).isMasterProject Then
                        Return True
                    End If
                End If
            Next
        Next
        Return False
    End Function

    Public Shared Function isMasterProjectInGroup(ByVal ProjectGroupName As String) As Boolean
        If LstProjectGroup.Count = 0 Then
            Return False
        End If
        For i As Integer = 0 To LstProjectGroup.Count - 1
            If CompareTwoStrings(ProjectGroupName, LstProjectGroup(i).ProjectGroupName) Then
                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                    If LstProjectGroup(i).ProjectDetail(j).isMasterProject Then
                        Return True
                    End If
                Next
                Exit For
            End If
        Next
        Return False
    End Function

    Public Shared Sub AddNewProject(ByVal PD As ProjectDetail)
        If Not isProjectGroupNameAvailable(PD.ProjectGroupName) Then
            AddProjectGroupName(PD.ProjectGroupName)
        End If
        If isProjectNameAvailable(PD.ProjectName) <> True Then
            For i As Integer = 0 To LstProjectGroup.Count - 1
                If CompareTwoStrings(LstProjectGroup(i).ProjectGroupName, PD.ProjectGroupName) Then
                    LstProjectGroup(i).ProjectDetail.Add(PD)
                    SaveAndReloadProject()
                    Exit Sub
                End If
            Next
        End If
    End Sub

    Public Shared Sub DeleteProjectDetail(ByVal ProjectName As String)
        If isProjectNameAvailable(ProjectName) Then
            For i As Integer = 0 To LstProjectGroup.Count - 1
                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                    If CompareTwoStrings(ProjectName, LstProjectGroup(i).ProjectDetail(j).ProjectName) Then
                        Dim PD As ProjectDetail = LstProjectGroup(i).ProjectDetail(j)
                        LstProjectGroup(i).ProjectDetail.Remove(PD)
                        SaveAndReloadProject()
                        Exit Sub
                    End If
                Next
            Next
        End If
    End Sub

    Public Shared Sub UpdateProject(ByVal PD As ProjectDetail)
        If isProjectNameAvailable(PD.ProjectName) Then
            For i As Integer = 0 To LstProjectGroup.Count - 1
                If CompareTwoStrings(LstProjectGroup(i).ProjectGroupName, PD.ProjectGroupName) Then
                    For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                        If CompareTwoStrings(LstProjectGroup(i).ProjectDetail(j).ProjectName, PD.ProjectName) Then
                            LstProjectGroup(i).ProjectDetail(j) = PD
                            SaveAndReloadProject()
                            Exit Sub
                        End If
                    Next
                End If
            Next
        End If
    End Sub

    Public Shared Function CompareTwoStrings(ByVal String1 As String, ByVal String2 As String) As Boolean
        If String.Compare(String1, String2, True) = 0 Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function GetProjectNameList(ByVal ProjectGroupName As String) As ArrayList
        Dim ProjectNameList As New ArrayList
        Try
            For i As Integer = 0 To LstProjectGroup.Count - 1
                If CompareTwoStrings(LstProjectGroup(i).ProjectGroupName, ProjectGroupName) Then
                    For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                        ProjectNameList.Add(LstProjectGroup(i).ProjectDetail(j).ProjectName)
                    Next
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return ProjectNameList
    End Function

    Public Shared Function GetProjectNameList() As ArrayList
        Dim ProjectNameList As New ArrayList
        Try
            For i As Integer = 0 To LstProjectGroup.Count - 1
                For j As Integer = 0 To LstProjectGroup(i).ProjectDetail.Count - 1
                    ProjectNameList.Add(LstProjectGroup(i).ProjectDetail(j).ProjectName)
                Next
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return ProjectNameList
    End Function

    Public Shared Function GetProjectGroupNameList() As ArrayList
        Dim ProjectGroupNameList As New ArrayList
        For i As Integer = 0 To LstProjectGroup.Count - 1
            ProjectGroupNameList.Add(LstProjectGroup(i).ProjectGroupName)
        Next
        Return ProjectGroupNameList
    End Function
End Class