Imports System
Imports System.Windows.Forms
Public Class MDF_Credentials
    Private Sub MDF_Credentials_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReadCredentials()
    End Sub

    Private Sub ReadCredentials()
        txt_baseurl.Text = My.Settings.BaseURL
        txt_compid.Text = My.Settings.CompID
        txt_uname.Text = My.Settings.Uname
        txt_pass.Text = My.Settings.Password
    End Sub

    Private Sub button1_Click(sender As Object, e As EventArgs) Handles button1.Click
        If txt_baseurl.Text.Trim = "" Then
            MessageBox.Show("Base Url can't be blank.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_baseurl.Focus()
            Exit Sub
        End If
        If txt_compid.Text.Trim = "" Then
            MessageBox.Show("Company Id can't be blank.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_compid.Focus()
            Exit Sub
        End If
        If txt_uname.Text.Trim = "" Then
            MessageBox.Show("User name can't be blank.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_uname.Focus()
            Exit Sub
        End If
        If txt_pass.Text.Trim = "" Then
            MessageBox.Show("Password can't be blank.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_pass.Focus()
            Exit Sub
        End If

        My.Settings.BaseURL = txt_baseurl.Text
        My.Settings.CompID = txt_compid.Text
        My.Settings.Uname = txt_uname.Text
        My.Settings.Password = txt_pass.Text
        My.Settings.Save()
        MessageBox.Show("User credentials saved successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Me.Close()
    End Sub
End Class