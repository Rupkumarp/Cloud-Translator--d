
Public Class ProjectDetail
    Public ProjectGroupName As String
    Public ProjectName As String
    Public ProjectPath As String
    Public LangList As String
    Public CustomerName As String
    Public InstaneName As String
    Public isCorruptEnabled As String
    Public isCleanRequired As Boolean
    Public bImportExistingtranslationsintoDB As Boolean
    Public bCreateXliffWithTranslation As Boolean
    Public isDBupdateRequired As Boolean
    Public isPretranslateEnabled As Boolean
    Public isCustomerCheckRequired As Boolean
    Public isInstanceCheckRequired As Boolean
    Public KeyWordExclsion As String
    Public ProjectDescription As String
    Public isCurrentProject As Boolean
    Public isMasterProject As Boolean
    Public isMaxLengthCheckRequired As Boolean
    Public DigitalBoardColorIndex As Integer
End Class

Public Class ProjectErrorDetail
    Public Enum ErrType
        Errored
        Warninged
    End Enum
    Public Property _EType As New List(Of ErrType)
    Public Property _FileName As New List(Of String)
    Public Property _Msg As New List(Of String)
    Public Property _ErrTab As New List(Of String)
    Public Property _LineNumber As New List(Of Long)
End Class

Public Class ErrorTabs
    Public Property _ErrTab As String
    Public Property _PED As ProjectErrorDetail
End Class