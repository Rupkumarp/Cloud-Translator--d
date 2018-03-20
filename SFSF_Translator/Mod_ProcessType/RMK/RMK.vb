Imports System.Text
Imports System.IO
Imports Ionic.Zip
Imports System.Xml

Public Class RMK ' ErrNumber  1000 - 1020

    Private ErrNumber As Integer

    Public Enum RtbColor
        Red
        Green
        Black
    End Enum

    Public Event UpdateMsg(ByVal Msg As String, ByVal RTBC As RtbColor)

#Region "Zip extraction and Copy xhtml to Input folder"
    Private _Zipfile As String
    Private _RmkFolder As String
    Private _InputFolder As String

    Public Sub New(ByVal ZipFile As String, ByVal RmkFolder As String, ByVal InputFolder As String)
        _Zipfile = ZipFile
        _RmkFolder = RmkFolder
        _InputFolder = InputFolder
    End Sub

    Public Sub Extract()
        Try
            Dim counter As Integer = 0
            RaiseEvent UpdateMsg(Now & Chr(9) & "Extracting Zip file please wait..." & vbCrLf, Form_MainNew.RtbColor.Black)
            Using zip As ZipFile = ZipFile.Read(_Zipfile)
                Dim zipel As ZipEntry
                For Each zipel In zip
                    zipel.Extract(_RmkFolder, ExtractExistingFileAction.OverwriteSilently)
                Next
            End Using
            DirSearch(_RmkFolder)
            CreateRmkXmkfile()
        Catch ex As Exception
            Throw New Exception("Error Number - " & ErrNumber & vbNewLine & ex.Message)
        End Try
    End Sub

    Private FileCollection As New ArrayList

    Private Sub DirSearch(ByVal sDirectory As String)
        RaiseEvent UpdateMsg(Now & Chr(9) & "Searching xhtml file in Directory - " & sDirectory & vbCrLf, Form_MainNew.RtbColor.Black)
        Dim extension As String = ""
        For Each directoryName In Directory.GetDirectories(sDirectory)
            For Each f As String In Directory.GetFiles(directoryName)
                extension = Path.GetExtension(f)
                Select Case extension
                    Case ".xhtml"
                        If System.IO.Path.GetDirectoryName(f).ToLower.Contains("en_us") Or System.IO.Path.GetDirectoryName(f).ToLower.Contains("english") Then
                            FileCollection.Add(f)
                        End If
                End Select
            Next
            DirSearch(directoryName)
        Next
    End Sub

    Private Sub CopyToInputFolder(ByVal SourceFile As String, ByVal FileId As String)
        Try
            File.Copy(SourceFile, _InputFolder & FileId & "_" & System.IO.Path.GetFileName(SourceFile), True)
            RaiseEvent UpdateMsg(Now & Chr(9) & "Copied File - " & FileId & ".xhtml" & vbCrLf, Form_MainNew.RtbColor.Black)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub CreateRmkXmkfile()
        ErrNumber = 1002
        Try
            Dim tagName As String
            Dim fileName As String
            Dim filePath As String
            Dim TextLine As String
            Dim Id As Integer = 0

            Dim fileId As String = ""

            Using writer As New XmlTextWriter(_RmkFolder & "RmkMapping.xml", System.Text.Encoding.UTF8)
                writer.WriteStartDocument(True)
                writer.Formatting = Formatting.Indented
                writer.Indentation = 2
                writer.WriteStartElement("FileCollection")
                For i As Integer = 0 To FileCollection.Count - 1
                    TextLine = FileCollection(i)
                    fileName = TextLine.ToString().Split("\").Last
                    tagName = fileName.ToString().Split(".").Last
                    filePath = TextLine
                    Id += 1
                    fileId = "18.1." & Id
                    createNode(tagName, fileId, fileName, filePath, writer)
                    CopyToInputFolder(filePath, fileId)
                Next
                writer.WriteEndElement()
                writer.WriteEndDocument()
            End Using

            RaiseEvent UpdateMsg(Id & " xhtml files found for translation." & vbCrLf, Form_MainNew.RtbColor.Black)
        Catch ex As Exception
            Throw New Exception("Error Number - " & ErrNumber & vbNewLine & ex.Message)
        End Try
    End Sub

    Private Sub createNode(ByVal tagName As String, ByVal fileId As String, ByVal fileName As String, ByVal filePath As String, ByVal writer As XmlTextWriter)
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
    End Sub
#End Region

#Region "ReImport files from 05-Out to original location"
    Private _ProjectPath As String
    Private _OutFileFolder As String
    Private _RmkMappingFile As String
    Private _TargetLang As String
    Public Sub New(ByVal ProjectPath As String, ByVal OutFilesFolder As String, ByVal RmkMappingFile As String, ByVal TargetLang As String)
        _ProjectPath = ProjectPath
        _OutFileFolder = OutFilesFolder
        _RmkMappingFile = RmkMappingFile
        _RmkFolder = _ProjectPath & "RMK_RawData"
        _TargetLang = TargetLang
    End Sub

    Public Sub ReImport()
        Try
            If System.IO.Directory.Exists(_RmkFolder) <> True Then
                Throw New Exception("Could not find RMK_RawData folder!")
            End If

            If ValidateRMKMappingFile() <> True Then
                Throw New Exception("Error Validating RmkMapping.xml file!" & vbCrLf)
            End If

            For Each f In My.Computer.FileSystem.GetFiles(_OutFileFolder, FileIO.SearchOption.SearchTopLevelOnly)
                Dim sFileName As String = System.IO.Path.GetFileNameWithoutExtension(f)
                If Microsoft.VisualBasic.Left(sFileName, 5) = "18.1." Then  'its RMk
                    Dim sTargetFolder As String = GetTargetFolderRmk(sFileName, _TargetLang)
                    If System.IO.Directory.Exists(sTargetFolder) <> True Then
                        System.IO.Directory.CreateDirectory(sTargetFolder)
                        RaiseEvent UpdateMsg(Now & Chr(9) & "Folder created - " & sTargetFolder & vbCrLf, Form_MainNew.RtbColor.Black)
                    End If
                    Dim sTargetFile As String = GetTargetFileRmk(sFileName)
                    File.Copy(f, sTargetFolder & "\" & sTargetFile, True)
                    RaiseEvent UpdateMsg(Now & Chr(9) & "Reimported - " & sTargetFile & " to " & sTargetFolder & vbCrLf, Form_MainNew.RtbColor.Black)
                End If
            Next

            Dim zipFolder As String = GetMy_ZipFolder()
            Dim RmkOut As String = _ProjectPath & "Rmk_Out\"
            If Not System.IO.Directory.Exists(RmkOut) Then
                System.IO.Directory.CreateDirectory(RmkOut)
            End If
            Using zip2 As ZipFile = New ZipFile
                zip2.AddDirectory(zipFolder)
                RaiseEvent UpdateMsg(Now & Chr(9) & "Compressing folder " & System.IO.Path.GetFileName(zipFolder) & ".zip Please wait.." & vbCrLf, RtbColor.Black)
                zip2.Save(RmkOut & System.IO.Path.GetFileName(zipFolder) & ".zip")
                RaiseEvent UpdateMsg(Now & Chr(9) & "Created zip file " & System.IO.Path.GetFileName(zipFolder) & ".zip" & vbCrLf, RtbColor.Black)
                'Log.LogInfo(System.IO.Path.GetDirectoryName(_Path) & "ImagesforProcessing.zip")
            End Using

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Function ValidateRMKMappingFile() As Boolean
        Try
            If System.IO.File.Exists(_RmkMappingFile) <> True Then
                Throw New Exception("No RmkMapping.xml file found!" & vbNewLine & "The operation cannot be performed...exiting...")
            End If

            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(_RmkMappingFile)

            '"trans-unit"
            Dim xNodeList As XmlNodeList
            xNodeList = xd.GetElementsByTagName("File_path")

            For i As Integer = 0 To xNodeList.Count - 1
                Dim TargetFolder As String = xNodeList(i).InnerText
                TargetFolder = Mid(TargetFolder, 1, InStr(Len(_RmkFolder) + 3, TargetFolder, "\"))

                If System.IO.Directory.Exists(TargetFolder) <> True Then
                    Throw New Exception(TargetFolder & vbNewLine & "No extracted zip/rar folder found in RMK_RawData!" & vbNewLine & "The operation cannot be performed...exiting...")
                End If
                Exit For
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function

    Private Overloads Function MapLang(ByVal searchLang As String) As String
        Dim str As String = ""
        Try
            For i As Integer = 0 To LanguageDefination.LangFullName.Count - 1
                If searchLang.ToLower.Trim.Contains(LanguageDefination.LangFullName(i).ToString.ToLower.Trim) Then
                    str = LanguageDefination.LangFullName(i)
                    Exit For
                ElseIf searchLang.ToLower.Trim.Contains(LanguageDefination.LangFourChars(i).ToString.ToLower.Trim) Then
                    str = LanguageDefination.LangFourChars(i)
                    Exit For
                ElseIf searchLang.ToLower.Trim.Contains(LanguageDefination.LangFiveChars(i).ToString.ToLower.Trim) Then
                    str = LanguageDefination.LangFiveChars(i)
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return str
    End Function

    Private Enum LangType
        FullNamed
        Fourchar
        FiveChar
    End Enum

    Private Function MapLangType(ByVal searchLang As String) As LangType
        Dim lt As LangType
        Try
            For i As Integer = 0 To LanguageDefination.LangFullName.Count - 1
                If searchLang.ToLower.Trim.Contains(LanguageDefination.LangFullName(i).ToString.ToLower.Trim) Then
                    lt = LangType.FullNamed
                ElseIf searchLang.ToLower.Trim.Contains(LanguageDefination.LangFourChars(i).ToString.ToLower.Trim) Then
                    lt = LangType.Fourchar
                ElseIf searchLang.ToLower.Trim.Contains(LanguageDefination.LangFiveChars(i).ToString.ToLower.Trim) Then
                    lt = LangType.FiveChar
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return lt
    End Function

    Private Overloads Function MapLang(ByVal searchLang As String, ByVal LT As LangType) As String
        Dim str As String = ""
        Try
            For i As Integer = 0 To LanguageDefination.LangFullName.Count - 1
                If searchLang.ToLower.Trim.Contains(LanguageDefination.LangFullName(i).ToString.ToLower.Trim) Or _
                     searchLang.ToLower.Trim.Contains(LanguageDefination.LangFourChars(i).ToString.ToLower.Trim) Or _
                    searchLang.ToLower.Trim.Contains(LanguageDefination.LangFiveChars(i).ToString.ToLower.Trim) Then
                    Select Case LT
                        Case LangType.FullNamed
                            str = LanguageDefination.LangFullName(i)
                            Exit For
                        Case LangType.Fourchar
                            str = LanguageDefination.LangFourChars(i)
                            Exit For
                        Case LangType.FiveChar
                            str = LanguageDefination.LangFiveChars(i)
                            Exit For
                    End Select
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return str
    End Function

    Private Function GetTargetFolderRmk(ByVal sFileName As String, ByVal TargetLang As String) As String
        Dim xd As New Xml.XmlDocument
        xd.XmlResolver = Nothing
        xd.Load(_RmkMappingFile)

        '"trans-unit"
        Dim xNodeList As XmlNodeList
        xNodeList = xd.GetElementsByTagName("xhtml")
        Dim sTargetFolder As String = ""
        For i As Integer = 0 To xNodeList.Count - 1
            If (xNodeList(i).ChildNodes(0).InnerText & "_" & xNodeList(i).ChildNodes(1).InnerText).ToLower = (sFileName & ".xhtml").ToLower Then
                Dim sFileLang As String = MapLang(Replace(xNodeList(i).ChildNodes(2).InnerText, "_", "-"))
                Dim lt As LangType = MapLangType(sFileLang)
                Dim sTargetlang As String = MapLang(TargetLang.Replace("_", "-"), lt)
                sFileLang = sFileLang.Replace("-", "_")
                sTargetlang = sTargetlang.Replace("-", "_")
                sTargetFolder = (xNodeList(i).ChildNodes(2).InnerText.Replace(sFileLang, sTargetlang))
                Exit For
            End If
        Next

        If sTargetFolder = "" Then
            Throw New Exception("Error @GetTargetFolderRmk, Could not find TargetFolder for RMk" & vbNewLine & sFileName & TargetLang)
        Else
            Return sTargetFolder
        End If
    End Function

    Private Function GetTargetFileRmk(ByVal sFileName As String) As String
        Dim xd As New Xml.XmlDocument
        xd.XmlResolver = Nothing
        xd.Load(_RmkMappingFile)

        '"trans-unit"
        Dim xNodeList As XmlNodeList
        xNodeList = xd.GetElementsByTagName("xhtml")

        Dim sFile As String = ""

        For i As Integer = 0 To xNodeList.Count - 1
            If (xNodeList(i).ChildNodes(0).InnerText & "_" & xNodeList(i).ChildNodes(1).InnerText).ToLower = (sFileName & ".xhtml").ToLower Then
                sFile = xNodeList(i).ChildNodes(1).InnerText
                Exit For
            End If
        Next
        If sFile = "" Then
            Throw New Exception("Error @GetTargetFileRmk, Could not find TargetFilename for RMk" & vbNewLine & sFileName)
        Else
            Return sFile
        End If

    End Function

    Private Function GetMy_ZipFolder() As String
        Dim TargetFolder As String = ""
        Try
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(_RmkMappingFile)

            '"trans-unit"
            Dim xNodeList As XmlNodeList
            xNodeList = xd.GetElementsByTagName("File_path")

            For i As Integer = 0 To xNodeList.Count - 1
                TargetFolder = xNodeList(i).InnerText
                TargetFolder = Mid(TargetFolder, 1, InStr(Len(_RmkFolder) + 3, TargetFolder, "\") - 1)

                If System.IO.Directory.Exists(TargetFolder) <> True Then
                    Throw New Exception(TargetFolder & vbNewLine & "No extracted zip/rar folder found in HybrisRawData!" & vbNewLine & "The operation cannot be performed...exiting...")
                End If

                Exit For
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return TargetFolder
    End Function


#End Region

End Class
