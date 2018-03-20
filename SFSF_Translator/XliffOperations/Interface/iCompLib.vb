Public Interface iCompLib

    Property Competencies_Path As String
    Property Standardxml_Path As String
    Property Extractedxliff_Path As String
    Property Inputmdfcsv_Path As String
    Property Outputtranslatedmdfcsv_Path As String
    Property Standardcsv_Path As String
    Property StdcsvwithGUID_Path As String
    Property Matchfiles_Path As String
    Property Outputcsv_Path As String

    Property cnt_newintegrated As Integer
    Property cnt_newtrans As Integer

    Sub StartProcessing()
End Interface
