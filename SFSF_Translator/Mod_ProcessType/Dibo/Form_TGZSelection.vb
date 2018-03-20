Imports System.IO
Imports System.Text
Imports ICSharpCode.SharpZipLib.GZip
Imports ICSharpCode.SharpZipLib.Tar
Imports System.Threading
Imports System.Xml
Public Class Form_TGZSelection
    Public InputPath01 As String = ""
    Public iStatus As Integer
    Private Enum Status
        EXTRACT = 1
        MOVE = 2
    End Enum
    Private Sub Form_TGZSelection_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lbl_status.Text = ""
    End Sub

#Region "Control Events"
    Private Sub link_InputFolder_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles link_InputFolder.LinkClicked
        If iStatus = Status.EXTRACT Then
            Dim fd As OpenFileDialog = New OpenFileDialog()
            fd.Title = "Open File Dialog"
            fd.Filter = "Tar files (*.tgz*)|*.tgz*|Tar files (*.TGZ*)|*.TGZ*"
            'fd.FilterIndex = 2
            'fd.RestoreDirectory = True

            If fd.ShowDialog() = DialogResult.OK Then
                lbl_tgzFolderPath.Text = fd.FileName
            End If

        ElseIf iStatus = Status.MOVE Then
            If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
                lbl_tgzFolderPath.Text = FolderBrowserDialog1.SelectedPath
            End If
        End If
    End Sub
    Private Sub btn_thumbnails_Click(sender As Object, e As EventArgs) Handles btn_thumbnails.Click
        Try
            If iStatus = Status.EXTRACT Then
                If Path.GetExtension(lbl_tgzFolderPath.Text.Trim).ToUpper <> ".TGZ" Then
                    MessageBox.Show("Please Select proper file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                '' extract .tgz folder
                Dim ExtractPath As String = Path.GetDirectoryName(lbl_tgzFolderPath.Text.Trim) + "\" + Path.GetFileNameWithoutExtension(lbl_tgzFolderPath.Text.Trim)
                ExtractTGZ(lbl_tgzFolderPath.Text.Trim, ExtractPath)

                '' Delete existing files from [01-Input] folder
                lbl_status.Text = "move files to [01-Input] folder...."
                For Each _file As String In Directory.GetFiles(InputPath01 + "01-Input")
                    File.Delete(_file)
                Next
                Movefiles_01Input(ExtractPath)
                MessageBox.Show("Successfully extract [" + Path.GetFileNameWithoutExtension(lbl_tgzFolderPath.Text.Trim).ToString + "]" + vbCrLf + "necessery files move to [01-Input] folder...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()

            ElseIf iStatus = Status.MOVE Then
                Dim OutPutfiles As String() = Directory.GetFiles(InputPath01 + "05-Output")
                If OutPutfiles.Length = 0 Then
                    MessageBox.Show("There are no translated/output files in [05-Output] directory to move.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Close()
                Else
                    For Each Outfile In Directory.GetFiles(InputPath01 + "05-Output")
                        'File.Move(Outfile, lbl_tgzFolderPath.Text.Trim + "\" + Path.GetFileName(Outfile))
                        File.Delete(lbl_tgzFolderPath.Text.Trim + "\" + Path.GetFileName(Outfile))
                        File.Copy(Outfile, lbl_tgzFolderPath.Text.Trim + "\" + Path.GetFileName(Outfile))
                    Next
                    MessageBox.Show("All translated/output files from [05-Output] successfully move.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Close()
                End If

            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Sub Movefiles_01Input(ExtractPath As String)
        Try
            '' Create Xml
            Dim Writer As XmlTextWriter
            If Strings.Right(InputPath01, 1) <> "\" Then InputPath01 = InputPath01 & "\"
            Writer = New XmlTextWriter(InputPath01 & "FileList.xml", System.Text.Encoding.UTF8)
            Writer.WriteStartDocument(True)
            Writer.Formatting = Formatting.Indented
            Writer.WriteStartElement("FileCollection")
            Writer.WriteStartElement("DIBO")
            Writer.WriteStartElement("Path_tgz")
            Writer.WriteString(ExtractPath)
            Writer.WriteEndElement()
            Writer.WriteEndElement()


            '' Folder Count
            Dim numbers As New List(Of String)()
            For Each folder As String In Directory.GetDirectories(ExtractPath, "*", System.IO.SearchOption.AllDirectories)
                numbers.Add(folder)
            Next

            Dim FileCount As Integer = 0

            For Each number As String In numbers
                Dim filenames As String() = Directory.GetFiles(number)
                If filenames.Length > 0 Then

                    '' move files
                    For Each filename As String In filenames
                        If IO.Path.GetFileName(filename).IndexOf("RESOURCE", StringComparison.OrdinalIgnoreCase) > -1 Or
                                IO.Path.GetFileName(filename).IndexOf("DIMENSION_2", StringComparison.OrdinalIgnoreCase) > -1 Or
                                IO.Path.GetFileName(filename).IndexOf("DIMENSION_3", StringComparison.OrdinalIgnoreCase) > -1 Or
                                IO.Path.GetFileName(filename).IndexOf("DIMENSION_4", StringComparison.OrdinalIgnoreCase) > -1 Or
                                IO.Path.GetFileName(filename).IndexOf("DIMENSION_5", StringComparison.OrdinalIgnoreCase) > -1 Then

                            FileCount += 1
                            Dim TempFile As String = InputPath01 + "01-Input" + "\" + Path.GetFileName(filename) + "_" + FileCount.ToString + ""
                            Writer.WriteStartElement(Path.GetFileName(filename) + "_" + FileCount.ToString + "")
                            Writer.WriteStartElement("Temp_File")
                            Writer.WriteString(TempFile)
                            Writer.WriteEndElement()
                            Writer.WriteStartElement("Source_File")
                            Writer.WriteString(filename)
                            Writer.WriteEndElement()
                            Writer.WriteEndElement()
                            File.Copy(filename, TempFile)
                        End If
                    Next

                End If
            Next

            Writer.WriteEndElement()
            'Writer.WriteEndDocument()
            Writer.Close()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub
    Private Sub CreateXml_FileList(Writer As XmlTextWriter)
        If Not File.Exists(InputPath01) Then

            If Strings.Right(InputPath01, 1) <> "\" Then
                InputPath01 = InputPath01 & "\"
            End If

            Writer = New XmlTextWriter(InputPath01 & "FileList.xml", System.Text.Encoding.UTF8)
            Writer.WriteStartDocument(True)
            Writer.Formatting = Formatting.Indented
            Writer.WriteStartElement("FileCollection")

            'Writer.WriteStartElement("Type")
            'Writer.WriteString(analyze.Type)
            Writer.WriteEndElement()
        End If

    End Sub
    Public Sub ExtractTGZ(ByVal gzArchiveName As String, ByVal destFolder As String)

        Dim inStream As Stream = File.OpenRead(gzArchiveName)
        Dim gzipStream As Stream = New GZipInputStream(inStream)

        Dim tarArchive As TarArchive = tarArchive.CreateInputTarArchive(gzipStream)
        tarArchive.ExtractContents(destFolder)
        tarArchive.CloseArchive()

        gzipStream.Close()
        inStream.Close()
    End Sub
    Private Sub AddDirectoryFilesToTar(ByVal sourceDirectory As String, ByVal recurse As Boolean)
        If recurse Then
            Dim directories As String() = Directory.GetDirectories(sourceDirectory)
            For Each directory As String In directories
                AddDirectoryFilesToTar(directory, recurse)
            Next
        End If

        Dim filenames As String() = Directory.GetFiles(sourceDirectory)
        For Each filename As String In filenames
            Dim tarEntry As TarEntry = tarEntry.CreateEntryFromFile(filename)
            'TarArchive.WriteEntry(tarEntry, True)
        Next
    End Sub
#End Region
End Class