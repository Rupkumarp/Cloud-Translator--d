Public Class Complib

    Public Property myXliff As iCompLib

    Public cnt_newintegrated As Integer
    Public cnt_newtrans As Integer

    Public Sub Process()
        Try
            myXliff.StartProcessing()
            cnt_newintegrated = myXliff.cnt_newintegrated
            cnt_newtrans = myXliff.cnt_newtrans
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub
End Class
