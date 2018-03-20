Imports System.IO

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Interface iXliff
    Sub StartProcessing(ByVal sFile As String, ByRef BW As System.ComponentModel.BackgroundWorker)
    Function ExtractXliff() As Boolean
    Sub ExtractXiffwithExistingTranslation()
    Sub CreateOutFile()
    Sub CleanTransaltion()
    Sub ImportExistingTranslationToDB()

    Property curlang As String()
    Property cnt_newintegrated As Integer
    Property cnt_newtrans As Integer
    Property tr_type As TranslationType

    Delegate Sub UpdateMsg(ByVal Msg As String, ByVal MyColor As Form_MainNew.RtbColor)
    Property CPS As CloudProjectsettings
    Property ActiveProject As ProjectDetail
    Property IP As InitiateProcess


End Interface




