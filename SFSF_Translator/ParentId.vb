Imports System.IO

Public Class ObjectId

    '######################################################IMPORTANT NOTE#######################################################################################
    'NEED TO IMPLMENT THIS CLASS IN OTHER MODULES WHERE IT IS LOOKING FOR PARENTID, AS OF NOW THIS IS BEEN UTILIZED BY PRETRANSLATE FUNCTION ONLY.

    Public Shared Function GetParentID(ByVal sFile As String, ByVal bFileName As Boolean) As String
        Dim Id As String = ""
        load_filetype()
        If bFileName Then
            sFile = Path.GetFileNameWithoutExtension(sFile)
        End If

        Dim FileNumber() As String = Split(sFile, ".")
        Dim FileNumberDefintion() As String
        Dim bFound As Boolean = False

        Try
            For f = 0 To UBound(fileID)
                FileNumberDefintion = Split(fileID(f), ".")
                If UBound(FileNumber) > UBound(FileNumberDefintion) Then
                    bFound = MapFileNumberWithFileTypeDefintion(FileNumberDefintion, FileNumber)
                Else
                    bFound = MapFileNumberWithFileTypeDefintion(FileNumber, FileNumberDefintion)
                End If
                If bFound Then
                    Id = fileID(f)
                    Exit For
                End If
            Next
        Catch ex As Exception

        End Try
        Return Id
    End Function


    Private Shared fileID() As String
    Private Shared fileTyp() As String

    Private Shared Function load_filetype() As Boolean
        Try
            Dim tmp As String = File.ReadAllText(appData & DefinitionFiles.FileType_List)
            Dim tmp_split() As String = Split(tmp, vbCrLf)
            Dim cnt As Integer = UBound(tmp_split)

            ReDim fileID(cnt)
            ReDim fileTyp(cnt)
            Dim f As Integer = 0
            Dim s() As String

            For Each filetype In tmp_split
                If filetype <> "" Then
                    s = Split(filetype, "|")
                    fileTyp(f) = s(0)
                    fileID(f) = s(1)
                    f = f + 1
                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return True
    End Function

End Class
