Imports Ionic.Zip
Imports System.ComponentModel

''' <summary>
''' Extract zip file to Lumira folder and copy Header.xml and Document.xml to Input Folder with 400,401 file type
''' </summary>
''' <remarks></remarks>
Public Class cls_Lumira

    Public Property _zFile As String
    Public Property _ActiveProject As ProjectDetail

    Public Sub New(ByVal zFile As String, ByVal ActiveProject As ProjectDetail)
        _zFile = zFile
        _ActiveProject = ActiveProject
    End Sub

    Public Event UpdateMsg(ByVal Msg As String)
    Public WithEvents BWlumira As New BackgroundWorker

    Public Sub ExtractZip()
        BWlumira.WorkerReportsProgress = True
        BWlumira.WorkerSupportsCancellation = True

        If Not BWlumira.IsBusy Then
            BWlumira.RunWorkerAsync()
        End If
    End Sub

    Private Sub BWlumira_DoWork(sender As Object, e As DoWorkEventArgs) Handles BWlumira.DoWork
        Try
            Using zip As ZipFile = ZipFile.Read(_zFile)
                Dim zipel As ZipEntry
                For Each zipel In zip
                    If Not System.IO.File.Exists(_ActiveProject.ProjectPath & "\LumiraExtracted\" & zipel.FileName.ToString) Then
                        zipel.Extract(_ActiveProject.ProjectPath & "\LumiraExtracted\", ExtractExistingFileAction.OverwriteSilently)
                        BWlumira.ReportProgress(1, "Extracting - " & zipel.FileName.ToString & vbNewLine)
                    End If
                Next
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub BWlumira_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BWlumira.ProgressChanged
        RaiseEvent UpdateMsg(e.UserState.ToString)
    End Sub

    Private Sub BWlumira_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BWlumira.RunWorkerCompleted
        If Not e.Error Is Nothing Then
            Throw New Exception(e.Error.Message.ToString)
        End If
    End Sub

    Public Sub CopyLumiraFilesToInputFolder()
        Try
            For Each f In System.IO.Directory.GetFiles(_ActiveProject.ProjectPath & CloudProjectsettings.Folder_Lumira)
                If System.IO.Path.GetFileName(f).ToLower = "document.xml" Then
                    My.Computer.FileSystem.CopyFile(f, _ActiveProject.ProjectPath & CloudProjectsettings.Folder_Input & "400_" & System.IO.Path.GetFileName(f), True)
                End If
                If System.IO.Path.GetFileName(f).ToLower = "header.xml" Then
                    My.Computer.FileSystem.CopyFile(f, _ActiveProject.ProjectPath & CloudProjectsettings.Folder_Input & "401_" & System.IO.Path.GetFileName(f), True)
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

End Class