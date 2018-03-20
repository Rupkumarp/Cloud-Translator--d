''' <summary>
''' Support for length restriction
''' </summary>
''' <remarks></remarks>
''' 
Public NotInheritable Class ClsMaxLength

    'MaxLengthDT table field structrue
    Const ConstFileNameColumnNumber As Integer = 0
    Const ConstSeriesColumnNumber As Integer = 1
    Const ConstMaxlenghtColumnNumber As Integer = 2
    Const ConstFieldColumnNumber As Integer = 3
    Const ConstFieldContentRowNumber As Integer = 4

    ''' <summary>
    ''' Loads the Maxlength datatable from the Definition file, if file id not found, it will get Parent Id from the defintion file
    ''' </summary>
    ''' <param name="FileName">ex:2.3</param>
    ''' <returns>Maxlength datatalbe from Definition file</returns>
    ''' <remarks></remarks>
    Public Shared Function LoadMaxLength(ByVal FileName As String) As DataTable
        Dim MaxLengthFile As String = appData & DefinitionFiles.MaxLength_List
        Dim NewDT As New DataTable
        Try
            If Not System.IO.File.Exists(MaxLengthFile) Then
                Throw New Exception("File not found!" & vbNewLine & MaxLengthFile)
            End If

            Dim DT As DataTable = Form_Definition.GetDefintionToDatatable(MaxLengthFile)

            NewDT = DT.Clone

            For i As Integer = 0 To DT.Rows.Count - 1
                If FileName.ToLower = DT.Rows(i).Item(ConstFileNameColumnNumber).ToString.ToLower Then
                    NewDT.ImportRow(DT.Rows(i))
                End If
            Next

            If NewDT.Rows.Count = 0 Then
                NewDT = LoadfromParentId(FileName, DT)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return NewDT
    End Function

    Private Shared Function LoadfromParentId(ByVal FileName As String, ByRef DT As DataTable) As DataTable

        Dim PID As String
        Try
            PID = GetParentID(FileName, DT)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Dim NewDT As New DataTable

        Try
            NewDT = DT.Clone
            For i As Integer = 0 To DT.Rows.Count - 1
                If PID.ToLower = DT.Rows(i).Item(ConstFileNameColumnNumber).ToString.ToLower Then
                    NewDT.ImportRow(DT.Rows(i))
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error getting Maxlength Information @LoadfromParentId" & vbNewLine & ex.Message)
        End Try
        Return NewDT
    End Function

    ''' <summary>
    ''' Parent Id is got from comparing Column number 0 in MaxLength.cnf file
    ''' </summary>
    ''' <param name="sFile"></param>
    ''' <param name="DT"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function GetParentID(ByVal sFile As String, ByRef DT As DataTable) As String
        Dim Id As String = ""
        Dim FileNumber() As String = Split(sFile, ".")
        Dim FileNumberDefintion() As String
        Dim bFound As Boolean = False

        Try
            For f = 0 To DT.Rows.Count - 1

                FileNumberDefintion = Split(DT.Rows(f).Item(ConstFileNameColumnNumber), ".")
                If UBound(FileNumber) > UBound(FileNumberDefintion) Then
                    bFound = MapFileNumberWithFileTypeDefintion(FileNumberDefintion, FileNumber)
                Else
                    bFound = MapFileNumberWithFileTypeDefintion(FileNumber, FileNumberDefintion)
                End If
                If bFound Then
                    Id = DT.Rows(f).Item(ConstFileNameColumnNumber)
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error getting ParentId from MaxLength Table" & vbNewLine & ex.Message)
        End Try
        Return Id
    End Function


    ''' <summary>
    ''' Gets MaxLength from .cnf file, if not found -1 is returned
    ''' </summary>
    ''' <param name="MaxLengthDT">Datatable from Maxlength.cnf file</param>
    ''' <param name="SourceDT">Input csv file Datatable</param>
    ''' <param name="enUSColumnNumber">Integer</param>
    ''' <param name="enUSRowNumber">Integer</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMaxLength(ByVal MaxLengthDT As DataTable, ByVal SourceDT As DataTable, ByVal enUSColumnNumber As Integer, ByVal enUSRowNumber As Integer) As Integer
        Dim MaxLength As Integer = 0
        Try
            Dim enUSColName As String = SourceDT.Columns(enUSColumnNumber).ColumnName
            Dim enUSContent As String = SourceDT.Rows(enUSRowNumber).Item(enUSColumnNumber)
            For i As Integer = 0 To MaxLengthDT.Rows.Count - 1
                If enUSColName.ToLower.Contains(MaxLengthDT.Rows(i).Item(ConstSeriesColumnNumber).ToString.ToLower) Then
                    If Not IsDBNull(MaxLengthDT.Rows(i).Item(ConstFieldColumnNumber)) Then
                        '1 generic column containing the column header and then 1 series of ‘values’
                        Dim FieldColumnNumber As Integer = GetFieldColNumber(SourceDT, MaxLengthDT.Rows(i).Item(ConstFieldColumnNumber))
                        Dim FieldColumnContent As String = SourceDT.Rows(enUSRowNumber).Item(FieldColumnNumber)
                        MaxLength = GetLength(MaxLengthDT, FieldColumnContent)
                        Exit For
                    Else
                        '1 series column per field
                        MaxLength = CInt(MaxLengthDT.Rows(i).Item(ConstMaxlenghtColumnNumber))
                        Exit For
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error getting Max length @GetMaxLength" & vbNewLine & ex.Message)
        End Try
        Return MaxLength
    End Function

    'Get field number example: Field Name is "foField"
    Private Shared Function GetFieldColNumber(ByVal DT As DataTable, ByVal FieldName As String) As Integer
        Dim FieldColumnNumber As Integer = -1
        Try
            For i As Integer = 0 To DT.Columns.Count - 1
                If String.Compare(DT.Columns(i).ColumnName, FieldName, True) = 0 Then
                    FieldColumnNumber = i
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error Getting Field Column Number @GetFieldColNumber" & vbNewLine & ex.Message)
        End Try
        Return FieldColumnNumber
    End Function

    Private Shared Function GetLength(ByRef MaxLengthDT As DataTable, ByVal FieldName As String) As Integer
        Dim MaxLength As Integer = -1
        Try
            For i As Integer = 0 To MaxLengthDT.Rows.Count - 1
                If String.Compare(MaxLengthDT.Rows(i).Item(ConstFieldContentRowNumber), FieldName, True) = 0 Then
                    MaxLength = MaxLengthDT.Rows(i).Item(ConstMaxlenghtColumnNumber)
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error @GetLenth for Max length" & vbNewLine & ex.Message)
        End Try
        Return MaxLength
    End Function



End Class
