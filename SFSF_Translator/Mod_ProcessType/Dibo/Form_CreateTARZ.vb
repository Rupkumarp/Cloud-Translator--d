Imports System.IO
Imports ICSharpCode.SharpZipLib.GZip
Imports ICSharpCode.SharpZipLib.Tar
Imports System.Xml
Public Class Form_CreateTARZ
    Public str_Projectpath As String = ""

#Region "Control Events"
    Private Sub Form_CreateTARZ_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim xmldoc As XmlDataDocument = New XmlDataDocument()
        Dim xmlnode As XmlNodeList
        Dim i As Integer = 0
        Dim str As String = Nothing
        Dim fs As FileStream = New FileStream(str_Projectpath + "FileList.xml", FileMode.Open, FileAccess.Read)
        xmldoc.Load(fs)
        xmlnode = xmldoc.GetElementsByTagName("DIBO")
        lbl_Dir2maketarz.Text = xmlnode(0).ChildNodes.Item(0).InnerText.Trim()
        lbl_Path4tarz.Text = Path.GetDirectoryName(lbl_Dir2maketarz.Text)
        txt_TarzName.Text = Path.GetFileNameWithoutExtension(lbl_Dir2maketarz.Text)
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If txt_TarzName.Text.Trim = "" Then
                MessageBox.Show("Please mentaion the file name to be create.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txt_TarzName.Focus()
                Exit Sub
            End If
            If lbl_Path4tarz.Text.Trim = "" Then
                MessageBox.Show("Please mentaion a path, where [.tar.gz] file will be create.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            If lbl_Dir2maketarz.Text.Trim = "" Then
                MessageBox.Show("Please mentaion a directory/folder to make [.tar.gz].", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            CreateTarGZ(lbl_Path4tarz.Text.Trim + "\" + txt_TarzName.Text.Trim + ".tar.gz", lbl_Dir2maketarz.Text.Trim)
            Directory.Delete(lbl_Dir2maketarz.Text.Trim, True)
            MessageBox.Show("Successfully created [" + txt_TarzName.Text.Trim + ".tar.gz].", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            lbl_Dir2maketarz.Text = FolderBrowserDialog1.SelectedPath
            lbl_Path4tarz.Text = Path.GetDirectoryName(FolderBrowserDialog1.SelectedPath)
            txt_TarzName.Text = Path.GetFileNameWithoutExtension(FolderBrowserDialog1.SelectedPath)
        End If
    End Sub
    Private Sub link_InputFolder_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles link_InputFolder.LinkClicked
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            lbl_Path4tarz.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub
#End Region

#Region "Methods"
    Private Sub CreateTarGZ(tgzFilename As String, sourceDirectory As String)
        Try
            Dim outStream As Stream = File.Create(tgzFilename)
            Dim gzoStream As Stream = New GZipOutputStream(outStream) 'New GZipOutputStream(outStream)
            Dim tarArchive__1 As TarArchive = TarArchive.CreateOutputTarArchive(gzoStream)

            ' Note that the RootPath is currently case sensitive and must be forward slashes e.g. "c:/temp"
            ' and must not end with a slash, otherwise cuts off first char of filename
            ' This is scheduled for fix in next release
            tarArchive__1.RootPath = sourceDirectory.Replace("\"c, "/"c)
            If tarArchive__1.RootPath.EndsWith("/") Then
                tarArchive__1.RootPath = tarArchive__1.RootPath.Remove(tarArchive__1.RootPath.Length - 1)
            End If

            AddDirectoryFilesToTar(tarArchive__1, sourceDirectory, True)

            tarArchive__1.CloseArchive()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
    Private Sub AddDirectoryFilesToTar(tarArchive As TarArchive, sourceDirectory As String, recurse As Boolean)

        Try
            ' Optionally, write an entry for the directory itself.
            ' Specify false for recursion here if we will add the directory's files individually.
            '
            Dim tarEntry__1 As TarEntry = TarEntry.CreateEntryFromFile(sourceDirectory)
            tarArchive.WriteEntry(tarEntry__1, False)

            ' Write each file to the tar.
            '
            Dim filenames As String() = Directory.GetFiles(sourceDirectory)
            For Each filename As String In filenames
                tarEntry__1 = TarEntry.CreateEntryFromFile(filename)
                tarArchive.WriteEntry(tarEntry__1, True)
            Next

            If recurse Then
                Dim directories As String() = Directory.GetDirectories(sourceDirectory)
                For Each directory__2 As String In directories
                    AddDirectoryFilesToTar(tarArchive, directory__2, recurse)
                Next
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
#End Region

End Class