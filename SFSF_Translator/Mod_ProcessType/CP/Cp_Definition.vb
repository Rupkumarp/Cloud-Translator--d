Imports System.Text.RegularExpressions

Public Module Cp_Definition

    Public CP As New Dictionary(Of String, Integer)
    Public CP_Text As New ArrayList

    Public CP_Element() As String 'Required for xml->xliff

    Public Enum Action
        Create
        Retrieve
    End Enum

    Public Sub Process()
        Try
            CP.Clear()
            CP_Text.Clear()

            Dim AllText As String = System.IO.File.ReadAllText(appData & DefinitionFiles.CP_List)

            Dim NewText As String() = Split(AllText, vbNewLine)
            Dim Dictionary_index As Integer = 0

            CP.Add("[Select Object]", Dictionary_index)
            Dictionary_index += 1


            For i As Integer = 1 To UBound(NewText)
                If NewText(i) = "######################################" Then
                    For j As Integer = i + 1 To UBound(NewText)
                        If NewText(j) = "######################################" Then
                            SetCP(NewText, i, j, Dictionary_index)
                            i = j - 1
                            Exit For
                        End If
                    Next
                    Dictionary_index += 1
                End If
            Next


        Catch ex As Exception
            Throw New Exception("Error @CP_Definition" & vbNewLine & ex.Message)
        End Try
    End Sub

    Private Sub SetCP(ByVal NewText As String(), ByVal iStart As Integer, ByVal iEnd As Integer, ByVal index As Integer)
        Try
            CP.Add(NewText(iStart + 1), index)
            For i As Integer = iStart + 1 To iEnd - 1
                CP_Text.Add(NewText(i) & "|||" & index)
            Next
        Catch ex As Exception
            Throw New Exception("Error @SetCp" & vbNewLine & ex.Message)
        End Try
    End Sub

    Public Sub GetCp_Element(ByVal FileName As String) 'Required for xml->xliff

        Dim index As Integer = 0

        For i As Integer = 0 To CP.Count - 1
            If InStr(LCase(CP.Keys(i)), LCase(FileName)) > 0 Then
                index = i
                Exit For
            End If
        Next

        Dim cpElement As String = ""
        For i As Integer = 0 To CP_Text.Count - 1
            Dim iStart As Integer = InStr(CP_Text(i), "|||")
            Dim MyNumber As String
            MyNumber = Mid(CP_Text(i), iStart, Len(CP_Text(i)))

            If MyNumber = "|||" & index Then
                If cpElement = "" Then
                    cpElement = clean_element_exclude_Percent(Mid(CP_Text(i), 1, iStart - 1)) & vbCrLf
                Else
                    cpElement = cpElement & clean_element_exclude_Percent(Mid(CP_Text(i), 1, iStart - 1)) & vbCrLf
                End If

            End If
        Next

        If cpElement.ToString.Trim = String.Empty Then
            Throw New Exception(FileName & " - no definition file found")
        End If

        CP_Element = Split(cpElement, vbCrLf)
    End Sub

End Module
