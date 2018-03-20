

Public Class CP_More 'CP definition with @@

    'Here is the idea: 
    'The user selects 13.8 e.g. in the C/P definition file we have something which allows to know that import is possible, for instance a double @
    '@@LMS - Item Types (13.8)
    'If there is a double @ for this particular type, we can import. We have then a button import (only for create, obviously). 
    'If we click we can select the input file.
    'It then checks based on the csv definition if the fields are well present (in the case of 13.8, Item Type ID & Description). If not -> error message.
    'If it works, then it needs to check if there are already entries in the current c/P xml. If yes, msgbox: “The xml file is already existing. Checking if new entries”.
    'Then it will look in the csv file, entry per entry and check if it already exists. If not, it’s added at the end of the xml file. Note that for some files, there are more columns than those required in the C/P definition. Just ignore those.
    'At the end of the process, we need to get a message: 23/24 entries added. (23 is the number added, 24 is the total nber of entries in the csv).

    Public Sub CheckCP(ByRef objCP As List(Of CPData), ByVal labellist As ArrayList, ByVal csvFile As String)
        Try

            If objCP Is Nothing Then
                objCP = New List(Of CPData)
            End If

            Dim xCPdata As CPData

            Dim objcsv As New CsvParser
            Dim DT As DataTable = objcsv.GetDataTabletFromCSVFile(csvFile)

            If DT.Columns(0).ColumnName.ToString.Trim.ToLower <> labellist(0).ToString.Trim.ToLower Then
                Throw New Exception("The headers does not match definition file!" & vbNewLine & "Please check you have selected the correct file!")
            End If

            For i As Integer = 0 To DT.Rows.Count - 1
                xCPdata = New CPData
                Dim cell1 As String = DT.Rows(i).Item(0)
                Dim cell2 As String = DT.Rows(i).Item(1)
                Dim bFound As Boolean = False

                For j As Integer = 0 To objCP.Count - 1
                    If (cell1.ToString.ToLower.Trim = objCP(j).Value(0).ToString.ToLower.Trim) And (cell2.ToString.ToLower.Trim = objCP(j).Value(1).ToString.ToLower.Trim) Then
                        bFound = True
                        Exit For
                    End If
                Next

                If bFound <> True Then
                    xCPdata.isBold.Add(True)
                    xCPdata.LabelName.Add(labellist(0))
                    xCPdata.Value.Add(cell1)
                    xCPdata.isBold.Add(False)
                    xCPdata.LabelName.Add(labellist(1))
                    xCPdata.Value.Add(cell2)
                    objCP.Add(xCPdata)
                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message & vbNewLine & "Error @CP_More:CheckCP")
        End Try
    End Sub

End Class
