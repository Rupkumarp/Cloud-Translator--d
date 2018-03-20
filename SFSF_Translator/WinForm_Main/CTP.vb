Imports Ionic.Zip
Imports System.ComponentModel
Imports System.IO

Public Class CTP
    Private WithEvents BW As New BackgroundWorker
    Public Event UpdateMsg(ByVal Msg As String)
    Public Event UpdateForm(ByVal EnableForm As Boolean)

    Private Enum CTPtype
        Import_Add
        Import_Edit
        ExportCTP
        ExportCtproj
        ImportCtproj
    End Enum

    Private objCTP As CTPtype

    Private _NodeProjectName As String
    Private _CTPdestination As String
    Private _CTPfile As String
    Private _ProjectLocation As String
    Private _ProjectGroupName As String
    Private _CustomerName As String
    Private _Instance As String


    Private Sub BW_DoWork(sender As Object, e As DoWorkEventArgs) Handles BW.DoWork
        Try
            Select Case objCTP
                Case CTPtype.ExportCTP
                    ExportCTP(BW)
                Case CTPtype.Import_Add
                    ImportCTP_Add(BW)
                Case CTPtype.Import_Edit
                    ImportCTP_Edit(BW)
                Case CTPtype.ExportCtproj
                    ExportCtproj(BW)
            End Select
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub BW_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BW.ProgressChanged
        If e.ProgressPercentage = 0 Then
            RaiseEvent UpdateMsg(e.UserState.ToString)
        End If
    End Sub

    Private Sub BW_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BW.RunWorkerCompleted
        RaiseEvent UpdateForm(True)
        If Not e.Error Is Nothing Then
            RaiseEvent UpdateMsg(vbCrLf & e.Error.Message.ToString)
            MsgBox(e.Error.Message.ToString, MsgBoxStyle.Critical, "Error")
        End If
    End Sub


#Region "Export CTP"
    Public Sub ExportCTP(ByVal NodeProjectName As String, ByVal CTPdestination As String)
        BW.WorkerReportsProgress = True
        BW.WorkerSupportsCancellation = True
        objCTP = CTPtype.ExportCTP
        _NodeProjectName = NodeProjectName
        _CTPdestination = CTPdestination
        Try
            If Not BW.IsBusy Then
                BW.RunWorkerAsync()
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Private Sub ExportCTP(ByRef BW As BackgroundWorker)
        'Zip CTP file
        Try
            Dim ProjectFolder As String = ProjectManagement.GetActiveProject.ProjectPath
            Dim ProjectName As String = ProjectManagement.GetActiveProject.ProjectName

            'Writing project setting file
            Dim PD As ProjectDetail = ProjectManagement.GetProjectDetail(_NodeProjectName)

            XMLMethod.SaveProjectCTP(ProjectFolder, PD)

            BW.ReportProgress(0, Now & Chr(9) & "Compressing archive, please wait..." & vbCrLf)
            Using zip2 As ZipFile = New ZipFile
                zip2.AddDirectory(ProjectFolder)
                zip2.Save(_CTPdestination & "\" & ProjectName & ".ctp")
            End Using
            BW.ReportProgress(0, Now & Chr(9) & ProjectName & ".ctp" & " - CTP files have been exported to '" & _CTPdestination & "'" & vbCrLf)

        Catch ex As Exception
            Throw New Exception("Error @ExportCTP" & vbNewLine & ex.Message)
        End Try
    End Sub
#End Region

#Region "Exprot Ctproj"
    Public Sub ExportCtproj(ByVal ProjectGroupName As String, ByVal CTPdestination As String)
        BW.WorkerReportsProgress = True
        BW.WorkerSupportsCancellation = True
        objCTP = CTPtype.ExportCtproj
        _ProjectGroupName = ProjectGroupName
        _CTPdestination = CTPdestination
        Try
            If Not BW.IsBusy Then
                BW.RunWorkerAsync()
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Private Sub ExportCtproj(ByRef BW As BackgroundWorker)

        Try
            Dim PG As ProjectGroup = ProjectManagement.GetProjectGroupDetail(_ProjectGroupName)
            XMLMethod.SaveProjectCtproj(_CTPdestination, PG)

            Dim ProjectList As ArrayList = ProjectManagement.GetProjectNameList(_ProjectGroupName)
            BW.ReportProgress(0, Now & Chr(9) & "Compressing archive, please wait..." & vbCrLf)

            Using zip2 As ZipFile = New ZipFile
                zip2.AddFile(_CTPdestination & "\Projects.ini", "")
                For i As Integer = 0 To ProjectList.Count - 1
                    Dim PD As ProjectDetail = ProjectManagement.GetProjectDetail(ProjectList(i))
                    If System.IO.Directory.Exists(PD.ProjectPath) Then
                        Dim ParentDirectory As String = PD.ProjectPath
                        If Microsoft.VisualBasic.Right(ParentDirectory, 1) = "\" Then
                            ParentDirectory = ParentDirectory.Substring(0, Len(ParentDirectory) - 1)
                        End If
                        zip2.AddDirectory(PD.ProjectPath, System.IO.Path.GetFileName(ParentDirectory))
                    Else
                        BW.ReportProgress(0, Now & Chr(9) & "Missing Project - " & PD.ProjectPath & vbCrLf)
                    End If
                Next i
                zip2.Save(_CTPdestination & "\" & _ProjectGroupName & ".ctproj")
            End Using

            If System.IO.File.Exists(_CTPdestination & "\Projects.ini") Then
                System.IO.File.Delete(_CTPdestination & "\Projects.ini")
            End If

            BW.ReportProgress(0, Now & Chr(9) & _ProjectGroupName & ".ctproj" & " - Ctproj file have been exported to '" & _CTPdestination & "'" & vbCrLf)

        Catch ex As Exception
            Throw New Exception("Error @ExportCtproj" & vbNewLine & ex.Message)
        End Try

    End Sub

#End Region

    Private Sub DeleteFilesIffound(ByVal Path As String, ByRef BW As BackgroundWorker)
        Try
            If Directory.Exists(Path) Then
                'Delete all files from the Directory
                For Each filepath As String In Directory.GetFiles(Path)
                    Try
                        File.Delete(filepath)
                        BW.ReportProgress(0, vbNewLine & "Deleted - " & filepath.ToString)

                    Catch ex As System.IO.PathTooLongException
                        'do nothing
                    End Try

                Next
                'Delete all child Directories
                For Each dir As String In Directory.GetDirectories(Path)
                    DeleteFilesIffound(dir, BW)
                Next
                'Delete a Directory
                'Directory.Delete(Path, True)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub


#Region "ImportCTP_Edit"
    Public Sub ImportCTP_Edit(ByVal ProjectGroupName As String, ByVal ProjectName As String, ByVal ProjectLocation As String, ByVal CTPFile As String) 'Edit Mode Only
        BW.WorkerReportsProgress = True
        BW.WorkerSupportsCancellation = True
        _NodeProjectName = ProjectName
        _CTPfile = CTPFile
        _ProjectGroupName = ProjectGroupName
        _ProjectLocation = ProjectLocation
        objCTP = CTPtype.Import_Edit
        Try
            If Not BW.IsBusy Then
                BW.RunWorkerAsync()
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Private Sub ImportCTP_Edit(ByRef BW As BackgroundWorker) 'Edit Mode only
        Try
            Dim ProjectFolder As String = ProjectManagement.GetProjectDetail(_NodeProjectName).ProjectPath
            Dim ProjectName As String = _NodeProjectName

            Dim ProjectLine As String = ""

            'Delete Existing Project
            Try
                BW.ReportProgress(0, Now & Chr(9) & "Deleting files, please wait..." & vbCrLf)
                DeleteFilesIffound(ProjectFolder, BW)
                BW.ReportProgress(0, Now & Chr(9) & "All files have been deleted." & vbCrLf)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

            'Unzip and read Project.txt file
            Using zip As ZipFile = ZipFile.Read(_CTPfile)
                Dim zipel As ZipEntry
                For Each zipel In zip
                    zipel.Extract(ProjectFolder, ExtractExistingFileAction.OverwriteSilently)
                    BW.ReportProgress(0, Now & Chr(9) & "Imported - " & ProjectFolder & zipel.FileName.ToString & vbCrLf)
                Next
            End Using

            Dim SettingFile As String = ProjectFolder & "projects.ini"

            If Not System.IO.File.Exists(SettingFile) Then
                Throw New Exception("Error Could not find Project.ini file in " & _CTPfile)
            End If

            Dim Pd As ProjectDetail = XMLMethod.GetProjectDetailFromCTP(SettingFile)

            'Means we are editing child node
            ' cross_form_functions.edit_project(_NodeProjectName, Pd.LangList, Pd.ProjectDescription, Pd.isCleanRequired, Pd.CustomerName, Pd.InstaneName, Pd.isCustomerCheckRequired, Pd.isInstanceCheckRequired, Pd.isPretranslateEnabled, Pd.isDBupdateRequired, Pd.isCorruptEnabled, PD_Default.ProjectGroupName)
            '   cross_form_functions.edit_project(Pd)
            ProjectManagement.UpdateProject(Pd)
        Catch ex As Exception
            Throw New Exception("Error @ImportCTP" & vbNewLine & ex.Message)
        End Try
    End Sub
#End Region

#Region "ImportCTP_Add"

    Private _PDNew As ProjectDetail
    Public Sub ImportCTP_Add(ByVal PD As ProjectDetail, ByVal CTPfile As String) 'Add Mode Only
        BW.WorkerReportsProgress = True
        BW.WorkerSupportsCancellation = True
        _CTPfile = CTPfile
        objCTP = CTPtype.Import_Add
        _PDNew = PD
        Try
            If Not BW.IsBusy Then
                BW.RunWorkerAsync()
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub ImportCTP_Add(ByRef BW As BackgroundWorker) 'Edit Mode only
        Try
            'Unzip and read Project.txt file
            Using zip As ZipFile = ZipFile.Read(_CTPfile)
                Dim zipel As ZipEntry
                For Each zipel In zip
                    zipel.Extract(_PDNew.ProjectPath, ExtractExistingFileAction.OverwriteSilently)
                    BW.ReportProgress(0, Now & Chr(9) & "Imported - " & _ProjectLocation & zipel.FileName.ToString & vbCrLf)
                Next
            End Using

            Dim SettingFile As String = _PDNew.ProjectPath & "\projects.ini"

            If Not System.IO.File.Exists(SettingFile) Then
                Throw New Exception("Error Could not find Project.txt file in " & _CTPfile)
            End If

            ProjectManagement.AddNewProject(_PDNew)
            BW.ReportProgress(0, "Import was successful" & vbCrLf)

        Catch ex As Exception
            Throw New Exception("Error @ImportCTP" & vbNewLine & ex.Message)
        End Try
    End Sub

#End Region

End Class