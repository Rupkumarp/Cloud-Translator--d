

Public Class CompetencyLib

    Implements iCompLib

    Public Property Competencies_Path As String Implements iCompLib.Competencies_Path

    Public Property Extractedxliff_Path As String Implements iCompLib.Extractedxliff_Path

    Public Property Inputmdfcsv_Path As String Implements iCompLib.Inputmdfcsv_Path

    Public Property Matchfiles_Path As String Implements iCompLib.Matchfiles_Path

    Public Property Outputcsv_Path As String Implements iCompLib.Outputcsv_Path

    Public Property Outputtranslatedmdfcsv_Path As String Implements iCompLib.Outputtranslatedmdfcsv_Path

    Public Property Standardcsv_Path As String Implements iCompLib.Standardcsv_Path

    Public Property Standardxml_Path As String Implements iCompLib.Standardxml_Path

    Public Property StdcsvwithGUID_Path As String Implements iCompLib.StdcsvwithGUID_Path

    Public Property cnt_newintegrated As Integer Implements iCompLib.cnt_newintegrated

    Public Property cnt_newtrans As Integer Implements iCompLib.cnt_newtrans

    Public Sub StartProcessing() Implements iCompLib.StartProcessing

        Try
            'Step 1.
            ShowMsgInMainForm(Now & Chr(9) & "File is of type CompetencyLib" & vbCrLf, Form_MainNew.RtbColor.Black)

            cnt_newtrans = Mod_CompetencyLib.CreateBilingualxliff(Standardxml_Path, Extractedxliff_Path)

        Catch ex As Exception
            MsgBox("Step1. Operation could not be completed!", MsgBoxStyle.Critical, "Error!")
            ShowMsgInMainForm(ex.Message, Form_MainNew.RtbColor.Red)
            Form_MainNew.UpdateProjectErrorDetail(ProjectErrorDetail.ErrType.Errored, "Competencylib", ex.Message)
            Exit Sub
        End Try

        Try
            'Step 2.
            For Each f In My.Computer.FileSystem.GetFiles(Inputmdfcsv_Path)
                If System.IO.Path.GetExtension(f).ToLower = ".csv" Then
                    cnt_newintegrated = cnt_newintegrated + Mod_CompetencyLib.LibNameToOtherLang(f, Outputtranslatedmdfcsv_Path, Extractedxliff_Path)
                End If
            Next
        Catch ex As Exception
            MsgBox("Step2. Operation could not be completed!", MsgBoxStyle.Critical, "Error!")
            ShowMsgInMainForm(ex.Message, Form_MainNew.RtbColor.Red)
            Form_MainNew.UpdateProjectErrorDetail(ProjectErrorDetail.ErrType.Errored, "Competencylib", ex.Message)
        End Try

      

    End Sub

End Class
