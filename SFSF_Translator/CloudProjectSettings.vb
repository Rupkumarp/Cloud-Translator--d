Imports System.IO

Public Class CloudProjectsettings

    Public _ProjectPath As String
    Public _PD As ProjectDetail

    Public Sub New(ByVal PD As ProjectDetail)
        _ProjectPath = PD.ProjectPath
        _PD = PD
    End Sub

    Public Const Folder_Input As String = "01-Input\"
    Public Const Folder_InputB As String = "01-Input-B\"
    Public Const Folder_TobeTransalted As String = "02-TobeTranslated\"
    Public Const Folder_BackFromTranslation As String = "03-Backfromtranslation\"
    Public Const Folder_TempReassmble As String = "04-tmpReassemble\"
    Public Const Folder_OutPut As String = "05-Output\"
    Public Const Folder_Compare As String = "06-Compare\"
    Public Const Folder_Pretranslate As String = "07-Pretranslate\"

    Public Const Folder_Competencies As String = "Competencies\"
    Public Const Folder_CompetenciesStandardXml As String = "Competencies\01-Standard_xml\"
    Public Const Folder_CompetenciesExtractedXliff As String = "Competencies\02-Extracted_xliff\"
    Public Const Folder_CompetenciesInputMdfCsv As String = "Competencies\03-Input_mdf_csv\"
    Public Const Folder_CompetenciesOutputTranslatedMdfCsv As String = "Competencies\04-Output_translated_mdf_csv\"
    Public Const Folder_CompetenciesStandardCsv As String = "Competencies\05-Standard_csv\"
    Public Const Folder_CompetenciesStdCsvWithGuid As String = "Competencies\06-Std_csv_with_GUID\"
    Public Const Folder_CompetenciesMatchFiles As String = "Competencies\07-Match_files\"
    Public Const Folder_CompetenciesOutputCSV As String = "Competencies\08-Output_csv\"

    Public Const Folder_Picklists As String = "Picklists\"
    Public Const Folder_PicklistsStandard As String = "Picklists\01-Standard\"
    Public Const Folder_PicklistsExtractedXliff As String = "Picklists\02-Extracted_xliff\"
    Public Const Folder_PicklistsInput As String = "Picklists\03-Input_picklist\"
    Public Const Folder_PicklistsOutput As String = "Picklists\04-Output_picklist\"

    Public Const Folder_Corrections As String = "08-Corrections\"
    Public Const Folder_CorrectionsPtls As String = "08-Corrections\01-PTLS\"
    Public Const Folder_CorrectionsStandard As String = "08-Corrections\02-Standard\"

    Public Const Folder_ExistingTranslation As String = "09-ExistingTranslation\"

    Public Const Folder_Lumira As String = "LumiraExtracted"
    Public Const Folder_Hybris As String = "HybrisRawData"
    Public Const Folder_Rmk As String = "RMK"

    Public Sub New(ByVal PD As ProjectDetail, ByVal _InputFile As String, ByVal Lang As String)
        _PD = PD
        InputFile = _InputFile

        Dim FileName As String = System.IO.Path.GetFileNameWithoutExtension(InputFile) & "_" & Lang

        Xliff_FileInTobetransalted = _PD.ProjectPath & Folder_TobeTransalted & FileName & ".xliff"
        Xliff_FileInBackFromtranslation = _PD.ProjectPath & Folder_BackFromTranslation & FileName & ".xliff"
        Xliff_ProcessedFileInBackFromtranslation = _PD.ProjectPath & Folder_BackFromTranslation & "(processed)" & FileName & ".xliff"
        Xliff_PreTranslateFile = _PD.ProjectPath & Folder_Pretranslate & "Pre_" & FileName & ".xliff"
        Xliff_ProcessedPreTranslateFile = _PD.ProjectPath & Folder_Pretranslate & "(processed)Pre_" & FileName & ".xliff"
        Xliff_ExistingTranslationFile = _PD.ProjectPath & Folder_ExistingTranslation & FileName & ".xliff"
        OutFile = _PD.ProjectPath & Folder_OutPut & System.IO.Path.GetFileName(InputFile)
        CurrentLang = Lang
    End Sub

    Public Property InputFile As String
    Public Property Xliff_FileInTobetransalted As String
    Public Property Xliff_FileInBackFromtranslation As String
    Public Property Xliff_ProcessedFileInBackFromtranslation As String
    Public Property Xliff_PreTranslateFile As String
    Public Property Xliff_ProcessedPreTranslateFile As String
    Public Property Xliff_ExistingTranslationFile As String
    Public Property OutFile As String
    Public Property CurrentLang As String

End Class