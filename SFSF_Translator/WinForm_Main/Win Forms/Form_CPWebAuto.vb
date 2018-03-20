Public Class Form_CPWebAuto

    Private Sub BtnStart_Click(sender As Object, e As EventArgs) Handles BtnStart.Click
        Try
            Me.BtnStart.Enabled = False
            Dim objSel As New SeleniumQA.WebAutomation(SeleniumQA.WebAutomation.BrowserType.Chrome)

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub BW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BW.DoWork
        Try

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub BW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BW.ProgressChanged
        Try

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub BW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BW.RunWorkerCompleted
        Try

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub Form_CPWebAuto_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class