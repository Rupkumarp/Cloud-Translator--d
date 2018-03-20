Imports System.IO

Public Class XmlDefinition

    Public xmlFileName As String
    Public Definitions() As String
    Public bAddSuppLang As Boolean
    Public bXmlLang() As Boolean
    Public bLang() As Boolean
    Public Position As String
    Public bCP As Boolean

    Public DefinitionFile As String

    Public Sub GetXmlDefinition(ByVal xmlFile As String)

        Try
            xmlFileName = xmlFile

            Dim CPxmlDefiniation As String = ""

            'Get Position based on file Name from XML_List.txt-------------------------------------------------------------------------------
            Dim ArrDefintion() As String = Nothing
            Dim str() As String = Split(System.IO.File.ReadAllText(appData & DefinitionFiles.Xml_List), vbLf)

            Dim bFound As Boolean = False
            For i As Integer = 0 To UBound(str)
                If InStr(LCase(str(i)), LCase(System.IO.Path.GetFileNameWithoutExtension(xmlFile)) & "-") > 0 Then
                    bFound = True
                    Dim Dom() As String = Split(str(i), vbTab)
                    Position = Dom(1)
                    DefinitionFile = Dom(0)
                    If DefinitionFile = "xml_08" Or DefinitionFile = "xml_09" Then
                        CPxmlDefiniation = GetCPFileNumber(xmlFileName, Dom(2))
                    End If
                    If Dom(4) = 0 Then
                        bAddSuppLang = False
                    Else
                        bAddSuppLang = True
                    End If
                    Exit For
                End If
            Next

            If bFound <> True Then
                Dim FileVtabNumber As String()
                Dim FileNumberDefintion As String()
                Dim FileNumber() As String = Split(System.IO.Path.GetFileNameWithoutExtension(xmlFile), ".")

                For f = 0 To UBound(str)
                    FileVtabNumber = Split(str(f), vbTab)
                    FileVtabNumber = Split(FileVtabNumber(2), "-")
                    For i As Integer = 0 To UBound(FileVtabNumber)
                        FileNumberDefintion = Split(FileVtabNumber(i), ".")
                        CPxmlDefiniation = FileVtabNumber(i)
                        If UBound(FileNumber) > UBound(FileNumberDefintion) Then
                            bFound = MapFileNumberWithFileTypeDefintion(FileNumberDefintion, FileNumber)
                        Else
                            bFound = MapFileNumberWithFileTypeDefintion(FileNumber, FileNumberDefintion)
                        End If
                        If bFound Then
                            Dim Dom() As String = Split(str(f), vbTab)
                            Position = Dom(1)
                            DefinitionFile = Dom(0)
                            If Dom(4) = 0 Then
                                bAddSuppLang = False
                            Else
                                bAddSuppLang = True
                            End If
                            Exit For
                        End If
                    Next

                    If bFound Then
                        Exit For
                    End If
                Next
            End If


            'Load Defination file-------------------------------------------------------------------------------------------------------------------------------
            'Dim Definitions() As String
            'This is used to avoid translating headers for CP, so when writing xliff, copy source to translate tag

            If DefinitionFile = "xml_08" Or DefinitionFile = "xml_09" Then
                'special case CP. We need to retrieve the tags
                bCP = True
                Cp_Definition.Process()
                'Cp_Definition.GetCp_Element(System.IO.Path.GetFileNameWithoutExtension(xmlFile))
                Cp_Definition.GetCp_Element(System.IO.Path.GetFileNameWithoutExtension(CPxmlDefiniation & ".xml"))
                ArrDefintion = Cp_Definition.CP_Element
                bAddSuppLang = False
                ReDim bLang(UBound(ArrDefintion))
                ReDim bXmlLang(UBound(ArrDefintion))
                For i As Integer = 0 To bLang.Count - 1
                    bLang(i) = True
                Next

            Else
                'for the others, we just search for the corresponding file.
                If File.Exists(appData & "\definition\xml_definition\" & DefinitionFile & ".txt") Then
                    Dim TempDefinitions() As String = Split(System.IO.File.ReadAllText(appData & "\definition\xml_definition\" & DefinitionFile & ".txt"), vbCrLf)
                    ReDim bLang(UBound(TempDefinitions))
                    ReDim bXmlLang(UBound(TempDefinitions))
                    Dim strDefinition As String = TempDefinitions(0) & vbCrLf
                    bLang(0) = True
                    For i As Integer = 1 To UBound(TempDefinitions)
                        Dim Temp() As String = Split(TempDefinitions(i), ",")
                        If strDefinition = "" Then
                            strDefinition = Temp(0) & vbCrLf
                        Else
                            strDefinition = strDefinition & Temp(0) & vbCrLf
                        End If
                        bLang(i) = True
                        Try
                            If Temp(1) = 1 Then
                                bLang(i) = False
                                bXmlLang(i) = True
                            End If
                        Catch ex As Exception
                            'do nothing
                        End Try

                    Next
                    ArrDefintion = Split(strDefinition, vbCrLf)

                End If
            End If

            If ArrDefintion Is Nothing Then
                Throw New Exception("No Definition file found! - " & xmlFile)
            End If

            Definitions = ArrDefintion

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Private Function GetCPFileNumber(ByVal xmlFile As String, ByVal str As String)

        Dim FileVtabNumber As String()
        Dim FileNumberDefintion As String()
        Dim FileNumber() As String = Split(System.IO.Path.GetFileNameWithoutExtension(xmlFile), ".")
        Dim CPxmlDefiniation As String = ""

        Dim bFound As Boolean = False

        FileVtabNumber = Split(str, "-")
        For i As Integer = 0 To UBound(FileVtabNumber)
            FileNumberDefintion = Split(FileVtabNumber(i), ".")
            CPxmlDefiniation = FileVtabNumber(i)
            If UBound(FileNumber) > UBound(FileNumberDefintion) Then
                bFound = MapFileNumberWithFileTypeDefintion(FileNumberDefintion, FileNumber)
            Else
                bFound = MapFileNumberWithFileTypeDefintion(FileNumber, FileNumberDefintion)
            End If

            If bFound Then
                Exit For
            End If
        Next

        Return CPxmlDefiniation

    End Function

End Class
