Public MustInherit Class Xliff

    Public Property myXliff As iXliff
    Public cnt_newintegrated As Integer
    Public cnt_newtrans As Integer

    Public Sub Process(ByVal FileName As String, ByRef bw As System.ComponentModel.BackgroundWorker)
        Try
            myXliff.StartProcessing(FileName, bw)
            cnt_newintegrated = myXliff.cnt_newintegrated
            cnt_newtrans = myXliff.cnt_newtrans
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

End Class



