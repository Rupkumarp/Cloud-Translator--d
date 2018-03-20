Public Class Form_ErrorList
    Public Event Errlog_SelectionChanged(ByRef sender As Object, e As EventArgs)

    Private Sub ToolStripComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox1.SelectedIndexChanged
        RaiseEvent Errlog_SelectionChanged(sender, e)
    End Sub

    Public Event ErrorButton_Click(ByRef sender As Object, e As EventArgs)
    Private Sub ButtonError_Click(sender As Object, e As EventArgs) Handles ButtonError.Click
        RaiseEvent ErrorButton_Click(sender, e)
    End Sub

    Public Event WarningButton_Click(ByRef sender As Object, e As EventArgs)
    Private Sub ButtonWarning_Click(sender As Object, e As EventArgs) Handles ButtonWarning.Click
        RaiseEvent WarningButton_Click(sender, e)
    End Sub

    Public Event DV_SelectionChanged(ByRef sender As Object, e As EventArgs)
    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        RaiseEvent DV_SelectionChanged(sender, e)
    End Sub
End Class




