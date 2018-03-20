Imports System.Xml

''' <summary>
''' Will have Langlist in 4,5 and fullname characters, ColumnNumberlist for MDF csv and For Xml only Langlist
''' </summary>
''' <remarks></remarks>
Public Class FileDetails
    Public LangFullName As New ArrayList
    Public LangFiveChars As New ArrayList
    Public LangFourChars As New ArrayList
    Public ColumnNumberList As New ArrayList
    Public ColumnNameList As New ArrayList
    Public ColumnRepeatedLangIndex As New ArrayList
End Class


''' <summary>
''' Gets list of available language from input files xml,csv dynamically
''' </summary>
''' <remarks></remarks>
Public Class Cls_GetLangList 'Errornumber 120 - 140

    Private _sfileName As String
    Private ErrNumber As Integer

    Public Enum RtbColor
        Red
        Green
        Black
    End Enum

    Private _Fdetails As New FileDetails

    Public Event UpdateMsg(ByVal Msg As String, ByVal RTBC As RtbColor)

    Public Function LangList(ByVal sFileName As String) As FileDetails
        _sfileName = sFileName
        RaiseEvent UpdateMsg(Now & Chr(9) & "Loading Language list from File - " & System.IO.Path.GetFileName(_sfileName) & vbCrLf, RtbColor.Black)
        Try
            Select Case System.IO.Path.GetExtension(sFileName).ToLower
                Case ".xml"
                    getLangListFromXMl()
                Case ".csv"
                    getLangListFromCsv()
            End Select
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        RaiseEvent UpdateMsg(Now & Chr(9) & "Total Language found - " & _Fdetails.LangFullName.Count & vbCrLf, RtbColor.Black)
        Return _Fdetails
    End Function


    Private Sub getLangListFromXMl()
        Try
            Dim xx As New XmlDefinition
            xx.GetXmlDefinition(_sfileName)

            If xx.Definitions.Count = 0 Then

                Exit Sub
            End If

            For i As Integer = 0 To xx.Definitions.Count - 1
                If xx.Definitions(i).ToString <> "" Then
                    getformxml(xx.Definitions(i))
                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub getformxml(ByVal ElementName As String)
        Dim lang As String = ""
        Try
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing

            xd.Load(_sfileName)

            Dim xNodeList As XmlNodeList
            xNodeList = xd.GetElementsByTagName(Replace(Replace(ElementName, "<", ""), ">", ""))

            Dim MyAttributes As XmlAttributeCollection
            Dim str As String = ""

            For i As Integer = 0 To xNodeList.Count - 1
                If xNodeList(i).Attributes.Count > 0 Then
                    MyAttributes = xNodeList(i).Attributes
                    Dim att As XmlAttribute
                    For Each att In MyAttributes
                        If InStr(att.Name, "lang") > 0 Then
                            Dim col As String = Microsoft.VisualBasic.Right(att.Value.ToString.Replace("_", "-").ToLower, 5)
                            If GetLang(col) Then
                                _Fdetails.ColumnNumberList.Add(i)
                            End If
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub getLangListFromCsv()
        Try
            Dim P As New CsvParser
            Dim DT As DataTable = P.GetDataTabletFromCSVFile(_sfileName)
            Dim lang As String = ""
            For i As Integer = 0 To DT.Columns.Count - 1
                Dim col As String = Microsoft.VisualBasic.Right(DT.Columns(i).ColumnName.ToString.Replace("_", "-").ToLower, 5)
                If GetLang(col) Then
                    _Fdetails.ColumnNumberList.Add(i)
                    _Fdetails.ColumnRepeatedLangIndex.Add(Replace(col, "-", ""))
                    _Fdetails.ColumnNameList.Add(DT.Columns(i).ColumnName)
                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub


    Private Function GetLang(ByVal searchLang As String) As Boolean
        Dim bFound As Boolean = False
        Try
            For i As Integer = 0 To LanguageDefination.LangFullName.Count - 1
                If LanguageDefination.LangFullName(i).ToString.ToLower.Trim.Contains(searchLang.ToLower.Trim) _
                    Or LanguageDefination.LangFourChars(i).ToString.ToLower.Trim.Contains(searchLang.ToLower.Trim) _
                    Or LanguageDefination.LangFiveChars(i).ToString.ToLower.Trim.Contains(searchLang.ToLower.Trim) Then
                    bFound = True
                    If Not _Fdetails.LangFiveChars.Contains(LanguageDefination.LangFiveChars(i)) Then
                        _Fdetails.LangFiveChars.Add(LanguageDefination.LangFiveChars(i))
                    End If

                    If Not _Fdetails.LangFourChars.Contains(LanguageDefination.LangFourChars(i)) Then
                        _Fdetails.LangFourChars.Add(LanguageDefination.LangFourChars(i))
                    End If

                    If Not _Fdetails.LangFullName.Contains(LanguageDefination.LangFullName(i)) Then
                        _Fdetails.LangFullName.Add(LanguageDefination.LangFullName(i))
                    End If

                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return bFound
    End Function

End Class
