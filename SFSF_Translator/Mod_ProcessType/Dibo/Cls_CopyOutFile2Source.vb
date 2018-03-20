Imports System.IO
Imports System.Xml

Public Class Cls_CopyOutFile2Source

    Public str_ProjectPath As String = ""

    Public Sub CopyOutfiles2SourceFolder()

        Try

            Dim boolcopy As Boolean = False
            If Strings.Right(str_ProjectPath, 1) <> "\" Then str_ProjectPath = str_ProjectPath & "\"

            Dim OutPutfiles As String() = Directory.GetFiles(str_ProjectPath + "05-Output")
            If OutPutfiles.Length = 0 Then
                MessageBox.Show("There are no translated/output files in [05-Output] directory to move.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim xmldoc As XmlDataDocument = New XmlDataDocument()
                Dim xmlnode As XmlNodeList
                Dim i As Integer = 0
                Dim str As String = Nothing
                Dim fs As FileStream = New FileStream(str_ProjectPath + "FileList.xml", FileMode.Open, FileAccess.Read)
                xmldoc.Load(fs)

                For Each Outfile In Directory.GetFiles(str_ProjectPath + "05-Output")
                    xmlnode = xmldoc.GetElementsByTagName(Path.GetFileName(Outfile))

                    'If Not xmlnode(0) Is Nothing Then
                    Dim Path_Destinationfile As String = xmlnode(0).ChildNodes.Item(1).InnerText.Trim()

                        File.Delete(Path_Destinationfile)
                        File.Copy(Outfile, Path_Destinationfile)
                        boolcopy = True
                    'Else
                    '    MessageBox.Show("Destination directory is not found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    '    Exit For
                    'End If

                Next

                If boolcopy Then MessageBox.Show("All translated/output files from [05-Output], successfully moved to corresponding destination directory .", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("@ Method()-CopyOutfiles2SourceFolder  " + vbCrLf + ex.Message)
        End Try

    End Sub

End Class
