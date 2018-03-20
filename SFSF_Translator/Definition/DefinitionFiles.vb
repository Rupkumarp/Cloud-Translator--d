Imports System.IO
Imports System.Environment
Imports System.Reflection

''' <summary>
''' 'Checks if the Defintion files are available in User app data folder, if not copies default files from tool to appdata folder.
''' 'This Class is required for Dynamic updating of Defintion files.
''' </summary>
''' <remarks></remarks>

Public Class DefinitionFiles

    Public Enum DefintionType
        CP_List
        FileType_List
        Mdf_List
        Xml_List
        HybrisConfig_List
        HybrisProperties_List
        HybrisImpex_List
        Lms_List
        SupLang_List
        Language_List
        Length_List
    End Enum

    Public Const CP_List As String = "\Definition\CP_Definition\CP_definition.txt"
    Public Const FileType_List As String = "\FileType\Filetypes.txt"
    Public Const FileDescription_list As String = "\FileType\FileDescriptions.txt"
    Public Const Mdf_List As String = "\Definition\Mdf_Definition\Mdf_List.txt"
    Public Const Xml_List As String = "\Definition\XML_Definition\Xml_List.txt"
    Public Const Lms_List As String = "\Definition\LMS_Definition\LMS_definition.txt"
    Public Const HybrisConfig_List As String = "\Definition\HYBRIS_Definition\HybrisConfig.txt"
    Public Const HybrisProperties_List As String = "\Definition\HYBRIS_Definition\HybrisProperties.txt"
    Public Const HybrisImpex_List As String = "\Definition\HYBRIS_Definition\Impex_Hybris.txt"
    Public Const SupLang_List As String = "\Definition\XML_Definition\SuppLang.txt"
    Public Const Lang_List As String = "\FileType\languages.txt"
    Public Const RMK_Tags As String = "\Definition\RMK_Definition\RMK_KeepTAGS.txt"
    Public Const RMK_SpecialAtt As String = "\Definition\RMK_Definition\RMK_Special_attributes.txt"
    Public Const ProjectGroupName As String = "\FileType\ProjectgroupName.txt"
    Public Const MaxLength_List As String = "\Definition\Length_Definition\MaxLength.cnf"

    Public Shared Function GetFileDescription() As Dictionary(Of String, String)
        Dim FD As New Dictionary(Of String, String)
        Try
            Dim str() As String = Split(IO.File.ReadAllText(appData & FileDescription_list), vbLf)
            For i As Integer = 0 To UBound(str)
                If str(i).Trim <> String.Empty Then
                    Dim s() As String = str(i).Split(vbTab)
                    If Not FD.ContainsKey(s(0)) Then
                        FD.Add(s(0), s(1))
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return FD
    End Function

    Public Shared Function EnumConstants(Of T)(ByVal type As Type) As List(Of T) 'enumerate all the const values in a class 
        Dim values As New List(Of T)()

        Dim fieldInfos As FieldInfo() = type.GetFields(BindingFlags.[Public] Or BindingFlags.[Static])

        For Each fieldInfo As FieldInfo In fieldInfos
            If fieldInfo.IsLiteral AndAlso Not fieldInfo.IsInitOnly Then
                Dim value As Object = fieldInfo.GetValue(Nothing)
                If value.[GetType]() Is GetType(T) Then
                    values.Add(DirectCast(value, T))
                End If
            End If
        Next

        Return values
    End Function

    Public Function ValidateDefinitionFiles() As Boolean

        Dim DefintionFileList = EnumConstants(Of String)(GetType(DefinitionFiles))

        Try
            For Each Line In DefintionFileList
                Try

                    Dim dd1 As DateTime = File.GetLastWriteTime(Application.StartupPath & Line)
                    Dim dd2 As DateTime = File.GetLastWriteTime(appData & Line)

                    If dd1 > dd2 Then
                        CopyFilesToFolder(Application.StartupPath & Line, appData & Line)
                    End If

                    If Not System.IO.File.Exists(appData & Line) Then
                        CopyFilesToFolder(Application.StartupPath & Line, appData & Line)
                    End If
                    If Line.ToLower.Trim = Xml_List.ToLower.Trim Then
                        ValidateXML_List(False)
                    End If
                Catch ex As Exception
                    Throw New Exception("Could not Copy " & System.IO.Path.GetFileName(Line))
                End Try
            Next
        Catch ex As Exception
            Throw New Exception("Error @ValidateDefinitionFiles" & vbNewLine & ex.Message)
        End Try

        Return True

    End Function

    Private Shared Sub CopyFilesToFolder(ByVal SourceFiles As String, ByVal Destination As String)
        Try

            If Not System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(Destination)) Then
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Destination))
            End If

            If System.IO.File.Exists(Destination) Then
                System.IO.File.SetAttributes(Destination, IO.FileAttributes.Normal)
            End If
            System.IO.File.Copy(SourceFiles, Destination, True)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub


    Private Shared Sub ValidateXML_List(ByVal bReplace As Boolean) 'As Xml has multiple Definition file types, this function will dydnamically set the type of Defintion file to be set.
        Try
            Dim str() As String = Split(IO.File.ReadAllText(appData & Xml_List), vbCrLf)
            For i As Integer = 0 To UBound(str)
                Dim Dom() As String = Split(str(i), vbTab)

                Dim dd1 As DateTime = File.GetLastWriteTime(Application.StartupPath & "\Definition\xml_Definition\" & Dom(0) & ".txt")
                Dim dd2 As DateTime = File.GetLastWriteTime(appData & "\Definition\xml_Definition\" & Dom(0) & ".txt")

                If dd1 > dd2 Then
                    CopyFilesToFolder(Application.StartupPath & "\Definition\xml_Definition\" & Dom(0) & ".txt", appData & "\Definition\xml_Definition\" & Dom(0) & ".txt")
                End If

                If System.IO.File.Exists(Application.StartupPath & "\Definition\xml_Definition\" & Dom(0) & ".txt") Then
                    If Not System.IO.File.Exists(appData & "\Definition\xml_Definition\" & Dom(0) & ".txt") Then
                        CopyFilesToFolder(Application.StartupPath & "\Definition\xml_Definition\" & Dom(0) & ".txt", appData & "\Definition\xml_Definition\" & Dom(0) & ".txt")
                    ElseIf bReplace = True Then
                        CopyFilesToFolder(Application.StartupPath & "\Definition\xml_Definition\" & Dom(0) & ".txt", appData & "\Definition\xml_Definition\" & Dom(0) & ".txt")
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error @ValidateXML_List" & vbNewLine & ex.Message)
        End Try

    End Sub


    Public Shared Function ReplaceDefinitionFilefromResource(ByVal sFileName As String) As Boolean
      
        Dim sFile As String = ""
        Try
            Select Case System.IO.Path.GetFileNameWithoutExtension(sFileName)
                Case System.IO.Path.GetFileNameWithoutExtension(CP_List)
                    sFile = CP_List
                Case System.IO.Path.GetFileNameWithoutExtension(FileType_List)
                    sFile = FileType_List
                Case System.IO.Path.GetFileNameWithoutExtension(HybrisConfig_List)
                    sFile = HybrisConfig_List
                Case System.IO.Path.GetFileNameWithoutExtension(HybrisImpex_List)
                    sFile = HybrisImpex_List
                Case System.IO.Path.GetFileNameWithoutExtension(HybrisProperties_List)
                    sFile = HybrisProperties_List
                Case System.IO.Path.GetFileNameWithoutExtension(Lms_List)
                    sFile = Lms_List
                Case System.IO.Path.GetFileNameWithoutExtension(Mdf_List)
                    sFile = Mdf_List
                Case System.IO.Path.GetFileNameWithoutExtension(Xml_List)
                    sFile = Xml_List
                Case System.IO.Path.GetFileNameWithoutExtension(SupLang_List)
                    sFile = SupLang_List
                Case System.IO.Path.GetFileNameWithoutExtension(Lang_List)
                    sFile = Lang_List
            End Select

            If sFile <> "" Then
                CopyFilesToFolder(Application.StartupPath & sFile, appData & sFile)
                Return True
            End If


            Dim str() As String = Split(IO.File.ReadAllText(appData & DefinitionFiles.Xml_List), vbCrLf)
            For i As Integer = 0 To UBound(str)
                Dim Dom() As String = Split(str(i), vbTab)
                If Dom(0) = System.IO.Path.GetFileNameWithoutExtension(sFileName) Then
                    CopyFilesToFolder(Application.StartupPath & "\Definition\XML_Definition\" & Dom(0) & ".txt", appData & "\Definition\XML_Definition\" & Dom(0) & ".txt")
                    Exit For
                End If
            Next

        Catch ex As Exception
            Throw New Exception("Error @ReplaceDefinitionFilefromResource" & vbNewLine & ex.Message)
        End Try

        Return True

    End Function


End Class
