Imports System.IO

Public Class DeleteProjects

    Public Event UpdateMsg(ByVal Msg As String)

    Public Sub DeleteFilesIffound(ByVal Path As String, Optional ByVal bDeleteInputFiles As Boolean = True)
        Try

            If Directory.Exists(Path) Then

                'Delete all files from the Directory
                For Each filepath As String In Directory.GetFiles(Path)
                    Try
                        If System.IO.Path.GetFileName(filepath).ToLower <> "dbexclusion.config" Then
                            File.Delete(filepath)
                            RaiseEvent UpdateMsg(Now & Chr(9) & "Deleted - " & filepath.ToString & vbCrLf)
                        End If
                     

                    Catch ex As System.IO.PathTooLongException
                        'do nothing 
                    End Try

                Next
                'Delete all child Directories
                For Each dir As String In Directory.GetDirectories(Path)
                    If Not bDeleteInputFiles Then
                        If Microsoft.VisualBasic.Right(dir, 8).ToLower <> "01-input" Then
                            'Skip
                            DeleteFilesIffound(dir, bDeleteInputFiles)
                        End If
                    End If

                    If bDeleteInputFiles Then
                        DeleteFilesIffound(dir)
                    End If

                Next
                'Delete a Directory
                'Directory.Delete(Path, True)
            End If
          
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


    End Sub

    Public Sub DeleteDirectory(ByVal Path As String)
        Try
            If System.IO.Directory.Exists(Path) Then
                For Each Dir As String In Directory.GetDirectories(Path)
                    Try
                        Directory.Delete(Path, True)
                    Catch ex As System.IO.DirectoryNotFoundException
                        'do nothing
                    End Try
                Next
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

End Class
