Public Class Form_LangSequencer

    Dim _objCplangList As New List(Of CpLangList)

    Public Sub AssignLang(ByVal objCpLangList As List(Of CpLangList))
        _objCplangList = objCpLangList
    End Sub

    Public Property CurrentDirectory As String
    Public Property CPobjectType As String

    Private Sub Form_LangSequencer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            LoadLangList()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Dim LangList As New Dictionary(Of String, Integer)

    Private Sub LoadLangList()
        Try
            LangList = New Dictionary(Of String, Integer)
            LstLang.DataSource = Nothing
            LstLang.Items.Clear()
            For i As Integer = 1 To _objCplangList.Count - 1
                LangList.Add(_objCplangList(i).Lang, i)
                LstLang.Items.Add(_objCplangList(i).Lang)
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub BtnDeleteLang_Click(sender As Object, e As EventArgs) Handles BtnDeleteLang.Click
        For i = 0 To LstLang.SelectedItems.Count - 1
            LangList.Remove(LstLang.SelectedItems(i))
        Next
        LstLang.DataSource = Nothing
        LstLang.Items.Clear()
        For i As Integer = 0 To LangList.Count - 1
            LstLang.Items.Add(LangList.Keys(i))
        Next
    End Sub

    Private Sub BtnReloadLang_Click(sender As Object, e As EventArgs) Handles BtnReloadLang.Click
        LoadLangList()
    End Sub

    Private Sub BtnUP_Click(sender As Object, e As EventArgs) Handles BtnUP.Click
        If LstLang.Items.Count > 0 Then
            Dim index As Integer = LstLang.SelectedIndex - 1
            If index >= 0 Then
                LstLang.Items.Insert(index, LstLang.SelectedItem)
                LstLang.Items.RemoveAt(LstLang.SelectedIndex)
                LstLang.SelectedIndex = index
            End If
        End If
    End Sub

    Private Sub BtnDown_Click(sender As Object, e As EventArgs) Handles BtnDown.Click

        If IsNothing(LstLang.SelectedItem) = True Then
            Exit Sub
        End If

        If LstLang.Items.Count > 0 Then
            Dim index As Integer = LstLang.SelectedIndex + 1
            If index <= LstLang.Items.Count - 1 Then
                LstLang.Items.Insert(index + 1, LstLang.SelectedItem)
                LstLang.Items.RemoveAt(LstLang.SelectedIndex)
                LstLang.SelectedIndex = index
            End If
        End If

    End Sub

    Private Sub BtnDone_Click(sender As Object, e As EventArgs) Handles BtnDone.Click
        Dim objCplangList As New List(Of CpLangList)
        If _objCplangList.Count = 0 Then
            Me.Close()
            Exit Sub
        End If
        objCplangList.Add(_objCplangList.Item(0))
        For i As Integer = 0 To LstLang.Items.Count - 1
            objCplangList.Add(_objCplangList.Item(LangList.Item(LstLang.Items(i))))
        Next
        CPLanglist = objCplangList

        If System.IO.Directory.Exists(CurrentDirectory) Then
            SaveLangSequence(objCplangList)
        End If

        Me.Close()
    End Sub

    Public CPLanglist As New List(Of CpLangList)

    Private Sub Form_LangSequencer_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If CPLanglist Is Nothing Then
            CPLanglist = _objCplangList
        End If

        If CPLanglist.Count = 0 Then
            CPLanglist = _objCplangList
        End If
    End Sub

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

    End Sub

End Class