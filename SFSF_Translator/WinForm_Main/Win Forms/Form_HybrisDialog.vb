Imports System.Text
Imports System.IO
Imports Ionic.Zip
Imports System.Xml

Public Class Form_HybrisDialog

    Public bExtract As Boolean
    Public bUnzip As Boolean
    Public ProjectFolder As String
    Public zFile As String
    Public sArchiveType As String 'zip or rar

    Private Sub Form_HybrisDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If bUnzip Then
                If System.IO.Path.GetExtension(zFile) = ".zip" Then
                    sArchiveType = "zip"
                    If bExtract Then
                        Me.Text = "Extracting data to project folder. Please wait......."

                        If BW_Zip.IsBusy <> True Then
                            BW_Zip.RunWorkerAsync()
                        End If

                    Else
                        If BW_Xml.IsBusy <> True Then
                            BW_Xml.RunWorkerAsync()
                        End If
                    End If
                ElseIf System.IO.Path.GetExtension(zFile) = ".rar" Then
                    sArchiveType = "rar"
                    If bExtract Then
                        RarExtract()
                    Else
                        If BW_Xml.IsBusy <> True Then
                            BW_Xml.RunWorkerAsync()
                        End If
                    End If
                End If
            Else
                Me.Text = "Compressing data. Please wait......."
                sArchiveType = GetArchiveType()
                If sArchiveType = "zip" Then
                    If BW_Zip.IsBusy <> True Then
                        BW_Zip.RunWorkerAsync()
                    End If
                Else
                    RarExtract()
                End If
              
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

#Region "Rar extraction\import"
    Public Sub RarExtract()
        Dim p As New Process
        RichTextBox1.Text = ""
        RichTextBox1.AppendText(vbNewLine & "Please wait...")
        RichTextBox1.Enabled = False
        WriteToBatfile()
        p = New Process
        p.StartInfo.FileName = ProjectFolder & "\Extract.bat"
        p.StartInfo.UseShellExecute = False
        p.StartInfo.RedirectStandardOutput = True
        p.StartInfo.RedirectStandardError = True
        p.StartInfo.CreateNoWindow = True
        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        p.EnableRaisingEvents = True
        AddHandler p.OutputDataReceived, AddressOf p_OutputDataReceived
        AddHandler p.Exited, AddressOf P_Exited
        p.Start()
        p.BeginOutputReadLine()
    End Sub

    Public Sub p_OutputDataReceived(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        UpdateTextBox(e.Data)
    End Sub

    Private Sub P_Exited(ByVal sender As Object, ByVal e As System.EventArgs)

        If Me.RichTextBox1.InvokeRequired Then
            Dim d As New CloseMe(AddressOf CloseMeNow)
            Me.Invoke(d)
        Else
            'do nothing
        End If

    End Sub

    Delegate Sub SetTextCallback(ByVal text As String)

    Delegate Sub CloseMe()
    Private Sub CloseMeNow()
        If BW_Xml.IsBusy <> True Then
            BW_Xml.RunWorkerAsync()
        End If
    End Sub

    Private Sub UpdateTextBox(ByVal text As String)
        Try
            If Me.RichTextBox1.InvokeRequired Then
                Dim d As New SetTextCallback(AddressOf UpdateTextBox)
                Me.Invoke(d, New Object() {text})
            Else
                If RichTextBox1.Text = Nothing Then RichTextBox1.Text = text Else RichTextBox1.AppendText(vbNewLine & text)
                RichTextBox1.ScrollToCaret()
                If InStrRev(text, "%") > 0 Then
                    Me.Text = "Extracting data to project folder. Please wait......." & Mid(text, InStrRev(text, "%") - 3, 4) & " done"
                End If
            End If

        Catch ex As System.ObjectDisposedException
            'do nothing
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub WriteToBatfile()

        Try
            If Not System.IO.Directory.Exists(ProjectFolder) Then
                System.IO.Directory.CreateDirectory(ProjectFolder)
            End If

            If System.IO.File.Exists(ProjectFolder & "\Extract.bat") Then
                System.IO.File.Delete(ProjectFolder & "\Extract.bat")
            End If

            Using SR As New IO.StreamWriter(ProjectFolder & "\Extract.bat")
                SR.WriteLine("cd\")
                SR.WriteLine("cd C:\Program Files\WinRAR\")
                'If sArchiveType = "rar" Then
                If Not bExtract Then
                    'Copy Out files to original folder
                    DirSearch(Replace(ProjectFolder, "HybrisRawData", "05-Output"))
                    For f As Integer = 0 To fileCollection.Count - 1
                        Dim fName As String = System.IO.Path.GetFileNameWithoutExtension(fileCollection(f))
                        If Microsoft.VisualBasic.Left(fName, 3) = 200 _
                            Or Microsoft.VisualBasic.Left(fName, 3) = 201 _
                            Or Microsoft.VisualBasic.Left(fName, 3) = 202 _
                            Or Microsoft.VisualBasic.Left(fName, 3) = 203 Then
                            Dim fType As String = System.IO.Path.GetExtension(fileCollection(f))
                            Dim tName As String = Replace(fName, "_en", "_ja")
                            Dim targetFolder As String = Get_TargetFolder_from_HybrisMappingFile(fName, fType, tName, "ja")

                            Try
                                File.Copy(fileCollection(f), targetFolder & "\" & tName, True)
                                BW_Zip.ReportProgress(1, targetFolder & "\" & tName & " copied...")
                            Catch ex As Exception
                                Throw New Exception(ex.Message)
                            End Try
                        End If
                    Next

                    Dim zipFolder As String = Get_ZipFolder()
                    SR.WriteLine("Rar.exe a -ep1 -r " & Chr(34) & ProjectFolder & System.IO.Path.GetFileName(zipFolder) & ".rar" & Chr(34) & " " & Chr(34) & zipFolder & Chr(34))
                Else
                    SR.WriteLine("Rar.exe x -o+ " & Chr(34) & zFile & Chr(34) & " " & ProjectFolder)
                End If


            End Using

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Cloud Translator")
        End Try

    End Sub
#End Region
    
#Region "Zip extraction\import"
    Private Sub BW_Zip_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BW_Zip.DoWork
        Try
            If bUnzip Then
                Using zip As ZipFile = ZipFile.Read(zFile)
                    Dim zipel As ZipEntry
                    For Each zipel In zip
                        zipel.Extract(ProjectFolder, ExtractExistingFileAction.OverwriteSilently)
                        BW_Zip.ReportProgress(1, ProjectFolder & zipel.FileName.ToString)
                    Next
                End Using
            Else

                'Copy Out files to original folder
                DirSearch(Replace(ProjectFolder, "HybrisRawData", "05-Output"))
                For f As Integer = 0 To fileCollection.Count - 1
                    Dim fName As String = System.IO.Path.GetFileNameWithoutExtension(fileCollection(f))
                    If Microsoft.VisualBasic.Left(fName, 3) = 200 _
                        Or Microsoft.VisualBasic.Left(fName, 3) = 201 _
                        Or Microsoft.VisualBasic.Left(fName, 3) = 202 _
                        Or Microsoft.VisualBasic.Left(fName, 3) = 203 Then
                        Dim fType As String = System.IO.Path.GetExtension(fileCollection(f))
                        Dim tName As String = Replace(fName, "_en", "_de")
                        Dim targetFolder As String = Get_TargetFolder_from_HybrisMappingFile(fName, fType, tName, "de")
                        ' targetFolder = "C:\Users\C5195092\Desktop\HybrisBankingStore\HybrisThirdSet1\05-Output"
                        Try
                            File.Copy(fileCollection(f), targetFolder & "\" & tName, True)
                            BW_Zip.ReportProgress(1, targetFolder & "\" & tName & " copied...")
                        Catch ex As Exception
                            Throw New Exception(ex.Message)
                        End Try
                    End If
                Next

                '  PutMissingFilesBack("ja")

                Dim zipFolder As String = Get_ZipFolder()
                Using zip2 As ZipFile = New ZipFile
                    If System.IO.Directory.Exists(zipFolder) <> True Then
                        System.IO.Directory.CreateDirectory(zipFolder)
                    End If
                    zip2.AddDirectory(zipFolder)
                    zip2.Save(ProjectFolder & System.IO.Path.GetFileName(zipFolder) & ".zip")
                    BW_Zip.ReportProgress(1, ProjectFolder & " Compressing folder.. Please wait..")
                    'Log.LogInfo(System.IO.Path.GetDirectoryName(_Path) & "ImagesforProcessing.zip")
                End Using
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Private Sub BW_Zip_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BW_Zip.ProgressChanged
        RichTextBox1.AppendText(CStr(e.UserState) & vbCrLf)
    End Sub

    Private Sub BW_Zip_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BW_Zip.RunWorkerCompleted
        If Not e.Error Is Nothing Then
            MsgBox(e.Error.Message.ToString, MsgBoxStyle.Critical, "")
            Me.RichTextBox1.Text = vbCrLf & e.Error.Message.ToString
            Exit Sub
        End If
        If bExtract Then
            If BW_Xml.IsBusy <> True Then
                BW_Xml.RunWorkerAsync()
            End If
        Else
            MsgBox("Copied files to zip folder!", MsgBoxStyle.Information, "Cloud translator")
            Me.Close()
        End If
    End Sub
#End Region

#Region "Creating HybrisMapping xml and Moveing files to 01-Input-B folder"

    Dim fileCollection As New ArrayList

    Private Enum LingualType
        Mono
        Multi
    End Enum

    Private Sub BW_Xml_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BW_Xml.DoWork
        Try
            '// This Function reads Hybrisconfig.txt and searches the directory accordingly.//////////////////////////////////////////////////////////////////////////////////////////////////////////
            Dim AllText As String = System.IO.File.ReadAllText(Application.StartupPath & "\Definition\HYBRIS_Definition\HybrisConfig.txt")
            Dim HybrisDefinition As String() = Split(AllText, vbNewLine)

            For i As Integer = 5 To UBound(HybrisDefinition)
                Dim aDir As String() = Split(HybrisDefinition(i), "|")
                Dim fileType As String = ""
                Dim similarFile As String = ""
                If aDir(0).Contains("*") Then
                    Dim oDir As String = Replace(aDir(0), "Searchdir:", "")
                    Dim ParentDirectory As String() = Split(oDir, "*")
                    ParentDirectory(0) = ProjectFolder & ParentDirectory(0)
                    fileType = System.IO.Path.GetExtension(aDir(1))
                    similarFile = System.IO.Path.GetFileNameWithoutExtension(aDir(1))
                    If aDir(2) = "Monolingual" Then
                        DirSearch(ParentDirectory(0), ParentDirectory(1), similarFile, fileType, False, LingualType.Mono)
                    Else
                        DirSearch(ParentDirectory(0), ParentDirectory(1), similarFile, fileType, False, LingualType.Multi)
                    End If

                Else
                    aDir(0) = ProjectFolder & Replace(aDir(0), "Searchdir:", "")
                    fileType = System.IO.Path.GetExtension(aDir(1))
                    similarFile = System.IO.Path.GetFileNameWithoutExtension(aDir(1))
                    If aDir(2) = "Monolingual" Then
                        DirSearch(aDir(0), "", similarFile, fileType, True, LingualType.Mono)
                    Else
                        DirSearch(aDir(0), "", similarFile, fileType, True, LingualType.Multi)
                    End If

                End If
            Next '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            '//If want to search all Directory for files, comment above code and uncommed below function
            'DirSearch(ProjectFolder) 'Reads all files

            Dim tagName As String
            Dim fileName As String
            Dim filePath As String
            Dim TextLine As String
            Dim Id_Properties As Integer = 0
            Dim Id_Impex As Integer = 0
            Dim Id_Xml As Integer = 0
            Dim Id_Html As Integer = 0
            Dim fileId As String = ""

            If fileCollection.Count > 1 Then
                Dim writer As New XmlTextWriter(ProjectFolder & "HybrisMapping.xml", System.Text.Encoding.UTF8)
                writer.WriteStartDocument(True)
                writer.Formatting = Formatting.Indented
                writer.Indentation = 2

                writer.WriteStartElement("ArchiveType")
                writer.WriteAttributeString("TargetFolderName", System.IO.Path.GetFileNameWithoutExtension(zFile))
                writer.WriteStartElement("Type")
                writer.WriteString(sArchiveType)
                writer.WriteEndElement()

                writer.WriteStartElement("FileCollection")


                For i As Integer = 0 To fileCollection.Count - 1
                    TextLine = fileCollection(i)
                    fileName = TextLine.ToString().Split("\").Last
                    tagName = fileName.ToString().Split(".").Last
                    filePath = TextLine
                    If tagName.ToString().ToLower.Trim() = "impex" Then
                        Id_Impex += 1
                        fileId = "200." & Id_Impex
                        createNode(tagName, fileId, fileName, filePath, writer)
                    ElseIf tagName.ToString().ToLower.Trim() = "properties" Then
                        Id_Properties += 1
                        fileId = "201." & Id_Properties
                        createNode(tagName, fileId, fileName, filePath, writer)
                    ElseIf tagName.ToString().ToLower.Trim() = "xml" Then
                        Id_Xml += 1
                        fileId = "202." & Id_Xml
                        createNode(tagName, fileId, fileName, filePath, writer)
                    ElseIf tagName.ToString().ToLower.Trim() = "html" Then
                        Id_Html += 1
                        fileId = "203." & Id_Html
                        createNode(tagName, fileId, fileName, filePath, writer)
                    End If
                    CopyToInputFolder(filePath, fileId)
                    BW_Xml.ReportProgress(1, "Copied to 01-Input-B - " & fileName)
                Next
                writer.WriteEndElement()
                writer.WriteEndDocument()
                writer.Close()
                BW_Xml.ReportProgress(1, "Copied files to 01-Input-B folder!" & vbNewLine & "Run Convert -> Auto to generate xliff for translation.")
            Else
                BW_Xml.ReportProgress(1, "No files found!" & vbNewLine & "Please check the source file. The file structure  doesn't match with HybrisConfig file")
                Throw New Exception("No files found!" & vbNewLine & "Please check the source file. The file structure doesn't match with HybrisConfig file")
            End If
        Catch ex As Exception
            Throw New Exception("Error creating HybrisXml file" & vbNewLine & ex.Message)
        End Try
    End Sub

    Private Sub BW_Xml_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BW_Xml.ProgressChanged
        Me.RichTextBox1.AppendText(vbCrLf & CStr(e.UserState))
    End Sub

    Private Sub BW_Xml_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BW_Xml.RunWorkerCompleted
        If Not e.Error Is Nothing Then
            MsgBox(e.Error.Message.ToString, MsgBoxStyle.Critical, "Cloud translator")
            Me.RichTextBox1.Text = vbCrLf & e.Error.Message.ToString
            Exit Sub
        End If
        If bExtract Then
            MsgBox("Copied files to 01-Input-B folder!" & vbNewLine & "Run Convert -> Auto to generate xliff for translation.", MsgBoxStyle.Information, "Cloud translator")
        Else
            MsgBox("Copied files to zip folder!", MsgBoxStyle.Information, "Cloud translator")
        End If

    End Sub

    Private Sub DirSearch(ByVal sDirectory As String)
        Try
            Dim extension As String = ""
            For Each directoryName In Directory.GetDirectories(sDirectory)
                For Each f As String In Directory.GetFiles(directoryName)
                    extension = Path.GetExtension(f)
                    Select Case extension
                        Case ".properties", ".impex", ".html"
                            'If (Microsoft.VisualBasic.Right(System.IO.Path.GetFileNameWithoutExtension(f), 3).ToLower <> "_en") Then
                            '    AddFileToCollection(f)
                            'End If

                            AddFileToCollection(f)
                        Case ".xml"
                            AddFileToCollection(f)
                    End Select
                    'If (Microsoft.VisualBasic.Right(System.IO.Path.GetFileNameWithoutExtension(f), 2).ToLower = "en") _
                    '    And (extension = ".properties" Or extension = ".impex" Or extension = ".xml" Or extension = ".html") Then
                    '    fileCollection.Add(f)
                    'End If
                Next
                DirSearch(directoryName)
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
      

    End Sub

   

    Private Sub createNode(ByVal tagName As String, ByVal fileId As String, ByVal fileName As String, ByVal filePath As String, ByVal writer As XmlTextWriter)
        Try
            writer.WriteStartElement(tagName)
            writer.WriteStartElement("File_Id")
            writer.WriteString(fileId)
            writer.WriteEndElement()
            writer.WriteStartElement("File_name")
            writer.WriteString(fileName)
            writer.WriteEndElement()
            writer.WriteStartElement("File_path")
            writer.WriteString(System.IO.Path.GetDirectoryName(filePath))
            writer.WriteEndElement()
            writer.WriteEndElement()
        Catch ex As System.IO.PathTooLongException
            'Do nothing
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
      
    End Sub

    Private Sub CopyToInputFolder(ByVal SourceFile As String, ByVal FileId As String)
        Try

            File.Copy(SourceFile, Replace(ProjectFolder, "HybrisRawData", "01-Input-B") & FileId & "_" & System.IO.Path.GetFileName(SourceFile), True)
        Catch ex As System.IO.PathTooLongException
            'Do nothing
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

#End Region


    Private Sub DirSearch(ByVal ParentDirectory As String, ByVal child As String, ByVal similarFile As String, ByVal sFileType As String, ByVal bFileSearch As Boolean, ByVal lingual As LingualType)
        Dim extension As String

        Try
            If Not bFileSearch Then
                FileSearchInDirectory(ParentDirectory, child, similarFile, sFileType, bFileSearch, lingual)
                If System.IO.Directory.Exists(ParentDirectory) Then
                    For Each directoryName In Directory.GetDirectories(ParentDirectory)
                        If System.IO.Directory.Exists(directoryName & child) Then
                            For Each f As String In Directory.GetFiles(directoryName & child) 'Append child here
                                extension = Path.GetExtension(f)
                                If extension.ToLower = sFileType.ToLower Then

                                    If System.IO.Path.GetFileNameWithoutExtension(f).ToLower.Contains(similarFile.ToLower) Or similarFile = "*" Then
                                        Select Case lingual
                                            Case LingualType.Mono
                                                If Microsoft.VisualBasic.Right(System.IO.Path.GetFileNameWithoutExtension(f), 3).ToLower = "_en" Then
                                                    AddFileToCollection(f)
                                                End If
                                            Case LingualType.Multi
                                                If Microsoft.VisualBasic.Mid(f, Microsoft.VisualBasic.Len(f) - 8, 1) = "_" Then
                                                    If Microsoft.VisualBasic.Right(System.IO.Path.GetFileNameWithoutExtension(f), 3).ToLower = "_en" Then
                                                        AddFileToCollection(f)
                                                    End If
                                                Else
                                                    AddFileToCollection(f)
                                                End If

                                        End Select
                                    End If

                                End If
                            Next
                        End If
                        DirSearch(directoryName, child, similarFile, sFileType, bFileSearch, lingual)
                    Next
                End If
            Else
                FileSearchInDirectory(ParentDirectory, child, similarFile, sFileType, bFileSearch, lingual)
            End If
        Catch excpt As System.Exception
            Throw New Exception(excpt.Message)
        End Try

    End Sub


    Private Sub FileSearchInDirectory(ByVal ParentDirectory As String, ByVal child As String, ByVal similarFile As String, ByVal sFileType As String, ByVal bFileSearch As Boolean, ByVal lingual As LingualType)
        Dim extension As String
        If System.IO.Directory.Exists(ParentDirectory) Then
            For Each f As String In Directory.GetFiles(ParentDirectory)
                extension = Path.GetExtension(f)
                If extension.ToLower = sFileType.ToLower Then

                    If System.IO.Path.GetFileNameWithoutExtension(f).ToLower.Contains(similarFile.ToLower) Or similarFile = "*" Then
                        Select Case lingual
                            Case LingualType.Mono
                                If Microsoft.VisualBasic.Right(System.IO.Path.GetFileNameWithoutExtension(f), 3).ToLower = "_en" Then
                                    AddFileToCollection(f)
                                End If
                            Case LingualType.Multi
                                If Microsoft.VisualBasic.Mid(f, Microsoft.VisualBasic.Len(f) - 8, 1) = "_" Then
                                    If Microsoft.VisualBasic.Right(System.IO.Path.GetFileNameWithoutExtension(f), 3).ToLower = "_en" Then
                                        AddFileToCollection(f)
                                    End If
                                Else
                                    AddFileToCollection(f)
                                End If
                        End Select
                    End If

                End If
            Next
        End If
    End Sub

    Private Sub AddFileToCollection(ByVal sFile As String)
        If Not fileCollection.Contains(sFile) Then
            fileCollection.Add(sFile)
        End If
    End Sub

End Class

