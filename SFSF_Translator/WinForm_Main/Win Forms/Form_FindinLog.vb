Imports System.Windows.Forms

Public Class Form_FindinLog

    Public Event Find(ByVal findWhat As String, ByVal findOption As RichTextBoxFinds)

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txtFindWhat_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFindWhat.TextChanged
        If txtFindWhat.TextLength > 0 Then
            btnFind.Enabled = True
        Else
            btnFind.Enabled = False
        End If
    End Sub

    Private Sub btnFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFind.Click
        If txtFindWhat.TextLength > 0 Then
            Dim findOption As Integer = 0
            If chkMatchCase.Checked Then findOption = RichTextBoxFinds.MatchCase
            If chkWholeWord.Checked Then findOption = (findOption Or RichTextBoxFinds.WholeWord)
            If optUp.Checked Then findOption = (findOption Or RichTextBoxFinds.Reverse)
            RaiseEvent Find(txtFindWhat.Text, findOption)
        End If
    End Sub

End Class


