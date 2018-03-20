Public Class Cls_LangSequencer

    Dim LangList As New Dictionary(Of String, Integer)
    Public CPLanglist As New List(Of CpLangList)
    Public Property CurrentDirectory As String
    Public Property CPobjectType As String

    Public Function SequenceAvailable(ByVal CPobjectType As String) As Boolean
        Dim bFound As Boolean = False
        Try
            Dim CpLangSequence As String = ""
            If System.IO.File.Exists(CurrentDirectory & "CpLangSequence.ini") Then
                CpLangSequence = System.IO.File.ReadAllText(CurrentDirectory & "CpLangSequence.ini")
                For Each line In Split(CpLangSequence, vbCrLf)
                    If line.Trim.ToLower = "@" & CPobjectType.Trim.ToLower Then
                        bFound = True
                    End If
                Next
            End If
        Catch ex As Exception
            Throw New Exception("Error reading CpLangSequence.ini" & vbCrLf & ex.Message)
        End Try
        Return bFound
    End Function

    Public Function GetSequence(ByVal CPobjectType As String, ByVal Langs As List(Of CpLangList)) As List(Of CpLangList)
        Dim langArr As New ArrayList
        Dim bFound As Boolean = False

        Dim objCpList As New List(Of CpLangList)

        Try
            Dim CpLangSequence As String = ""
            If System.IO.File.Exists(CurrentDirectory & "CpLangSequence.ini") Then
                CpLangSequence = System.IO.File.ReadAllText(CurrentDirectory & "CpLangSequence.ini")
                For Each line In Split(CpLangSequence, vbCrLf)
                    If Microsoft.VisualBasic.Left(line, 1) = "@" And line.Trim.ToLower = "@" & CPobjectType.Trim.ToLower Then
                        bFound = True
                    ElseIf Microsoft.VisualBasic.Left(line, 1) = "@" Then
                        bFound = False
                    End If
                    If bFound Then
                        langArr.Add(line)
                    End If
                Next
            End If

            For i As Integer = 1 To langArr.Count - 1
                For j As Integer = 0 To Langs.Count - 1
                    If Langs(j).Lang = langArr(i) Then
                        objCpList.Add(Langs(j))
                    End If
                Next
            Next


        Catch ex As Exception
            Throw New Exception("Error reading CpLangSequence.ini" & vbCrLf & ex.Message)
        End Try
        Return objCpList
    End Function

    Private Sub SaveLangSequence(ByVal Langs As List(Of CpLangList))

        Try
            Dim LangList As String = ""
            Dim CpLangSequence As String = ""
            Dim langArr As New ArrayList

            If System.IO.File.Exists(CurrentDirectory & "CpLangSequence.ini") Then
                CpLangSequence = System.IO.File.ReadAllText(CurrentDirectory & "CpLangSequence.ini")
                Dim bFound As Boolean = False
                For Each line In Split(CpLangSequence, vbCrLf)
                    If Microsoft.VisualBasic.Left(line, 1) = "@" And line.Trim.ToLower = "@" & CPobjectType.Trim.ToLower Then
                        bFound = True
                    ElseIf Microsoft.VisualBasic.Left(line, 1) = "@" Then
                        bFound = False
                    End If
                    If bFound <> True Then
                        langArr.Add(line)
                    End If
                Next
            End If

            langArr.Add("@" & CPobjectType)

            For i As Integer = 0 To Langs.Count - 1
                langArr.Add(Langs(i).Lang)
            Next

            Using Writer As IO.StreamWriter = New IO.StreamWriter(CurrentDirectory & "CpLangSequence.ini", False)
                For i As Integer = 0 To langArr.Count - 1
                    Writer.WriteLine(langArr(i))
                Next
            End Using
        Catch ex As Exception
            Throw New Exception("Error saving CpLanguageSequence.txt" & vbCrLf & ex.Message)
        End Try

        CPLanglist = Langs

    End Sub

End Class
