Imports System.Windows.Forms

Public Class Dialog3

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Dialog3_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ListBox1.Items.Clear()
        If Not (System.IO.File.Exists(appData & DefinitionFiles.Lang_List)) Then MsgBox("File Language.txt doesn't exist. Critical error!", MsgBoxStyle.Critical)

        For Each lang In Split(System.IO.File.ReadAllText(appData & DefinitionFiles.Lang_List), vbCrLf)
            ListBox1.Items.Add(Mid(lang, 1, InStr(lang, Chr(9)) - 1))
        Next

        ComboBox1.Items.Clear()
        'insert here the code to load all file formats.


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFile1.InitialDirectory = ProjectManagement.GetActiveProject.ProjectPath & "01-Input-B\"
        OpenFile1.FileName = ""
        OpenFile1.Filter = "Csv file|*.csv|xml file|*.xml"
        OpenFile1.ShowDialog()
        If System.IO.File.Exists(OpenFile1.FileName) Then TextBox2.Text = OpenFile1.FileName Else MsgBox("Invalid file - exiting", MsgBoxStyle.Critical)

    End Sub
End Class
