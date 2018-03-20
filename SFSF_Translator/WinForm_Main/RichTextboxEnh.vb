Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Public Class RichTextBoxEnh

    Private WithEvents findDialog As Form_FindinLog
    Private foundIndex As Integer
    Private foundWord As String

    Private Sub findDialog_Find(ByVal findWhat As String, ByVal findOption As RichTextBoxFinds) Handles findDialog.Find
        Dim findIndex As Integer = 0
        If findWhat.Equals(foundWord) Then findIndex = foundIndex
        If findOption And RichTextBoxFinds.Reverse = RichTextBoxFinds.Reverse Then
            findIndex = Me.Find(findWhat, 0, findIndex, findOption)
        Else
            findIndex = Me.Find(findWhat, findIndex, findOption)
        End If
        If findIndex > 0 Then
            foundWord = findWhat
            If findOption And RichTextBoxFinds.Reverse = RichTextBoxFinds.Reverse Then
                foundIndex = findIndex
            Else
                foundIndex = findIndex + findWhat.Length
            End If
        End If
    End Sub

    Private Sub RichTextBoxEnh_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.Modifiers = Keys.Control And e.KeyCode = Keys.F Then
            If findDialog Is Nothing Then findDialog = New Form_FindinLog()
            Me.HideSelection = False
            findDialog.ShowDialog()
        End If
    End Sub

End Class