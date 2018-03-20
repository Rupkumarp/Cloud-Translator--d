
Imports System.IO
Imports System.Environment
Imports System.Xml

''' <summary>
''' This class is depreciated, it is used only when there is old Projects.txt file, This class will read that file and update them to newer version of xml settings
''' Do not use this class for new project setting
''' </summary>
Public Class cross_form_functions

    Private Function GetActiveProject(ByVal ProjectLine() As String) As String()
        Dim f As Integer = 0
        Dim tempProjectline() As String
        ReDim tempProjectline(f)
        For Each myline In ProjectLine
            If myline.Contains("|") Then
                Dim sDirectory() As String = Split(myline, "|")
                If System.IO.Directory.Exists(sDirectory(3)) Then
                    ReDim Preserve tempProjectline(f)
                    tempProjectline(f) = myline
                    f += 1
                End If
            Else
                ReDim Preserve tempProjectline(f)
                tempProjectline(f) = myline
                f += 1
            End If
        Next
        Return tempProjectline
    End Function

    'Read Project Detail functions
    Public Function GetProjectGroupInfo() As List(Of ProjectGroup)

        Dim PG As ProjectGroup
        Dim PD As ProjectDetail

        Try
            If Not (File.Exists(appData & "\projects.txt")) Then
                File.WriteAllText(appData & "\projects.txt", "###########################################" & vbCrLf & "#SF projects                              #" & vbCrLf & "###########################################")
                Return LstProjectGroup
            End If
            Dim mylist As String = ""
            Dim projectfile As String = File.ReadAllText(appData & "\projects.txt")
            Dim projectline() As String = Split(projectfile, vbCrLf)
            projectline = GetActiveProject(projectline)

            For Each myline In projectline
                If myline.ToString <> String.Empty Then
                    PD = New ProjectDetail
                    If Strings.Left(myline, 1) <> "#" Then
                        PD.ProjectGroupName = GetProjectGroupName(myline)
                        PD.ProjectName = get_projectname(myline)
                        PD.ProjectPath = get_last_projectpath(myline)
                        PD.ProjectDescription = get_last_projectdescription(myline)
                        PD.CustomerName = GetCustomerName(myline)
                        PD.isCleanRequired = isCleanTranslationEnabled(myline)
                        PD.isCorruptEnabled = isCheckCorruptionEnabled(myline)
                        PD.isCustomerCheckRequired = isRestrictCustomer(myline)
                        PD.isDBupdateRequired = isUploadtoDBenabled(myline)
                        PD.isInstanceCheckRequired = isInstanceEnabled(myline)
                        PD.isPretranslateEnabled = isPretranslateEnabled(myline)
                        PD.LangList = get_last_projectlanguages(myline)
                        PD.isCurrentProject = False
                        PD.isMasterProject = isMasterProject(myline)
                        PD.InstaneName = GetInstance(myline)
                        If Strings.Left(myline, 1) = "*" Then
                            PD.isCurrentProject = True
                        End If
                        If LstProjectGroup.Count = 0 Then
                            PG = New ProjectGroup
                            PG.ProjectDetail.Add(PD)
                            PG.ProjectGroupName = PD.ProjectGroupName

                            LstProjectGroup.Add(PG)
                        Else
                            Dim bFound As Boolean = False

                            For i As Integer = 0 To LstProjectGroup.Count - 1
                                If LstProjectGroup(i).ProjectGroupName = PD.ProjectGroupName Then
                                    LstProjectGroup(i).ProjectDetail.Add(PD)
                                    bFound = True
                                    Exit For
                                End If
                            Next
                            If Not bFound Then
                                PG = New ProjectGroup
                                PG.ProjectDetail.Add(PD)
                                PG.ProjectGroupName = PD.ProjectGroupName
                                LstProjectGroup.Add(PG)
                            End If
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return LstProjectGroup
    End Function

    Private Function get_projectname(ByVal InputLine As String) As String
        Return Mid(InputLine, 3, InStr(4, InputLine, "|") - 3)
    End Function

    Private Function get_last_projectdescription(ByVal InputLine As String) As String
        Dim tag() As String

        tag = Split(InputLine, "|")
        Return Replace(tag(4), "@CrLf@", vbCrLf)

        Return ""
    End Function

    Private Function isMasterProject(ByVal InputLine As String) As Boolean
        Dim tag() As String

        Try
            tag = Split(InputLine, "|")
            tag = Split(tag(14), ":")
            Return CBool(Replace(tag(1), "@CrLf@", vbCrLf))
        Catch ex As Exception
            Return False 'its old project
        End Try
        Return False
    End Function
    Private Function isCleanTranslationEnabled(ByVal InputLine As String) As Boolean
        Dim tag() As String

        Try
            tag = Split(InputLine, "|")
            tag = Split(tag(5), ":")
            Return CBool(Replace(tag(1), "@CrLf@", vbCrLf))
        Catch ex As Exception
            Return False 'its old project
        End Try
        Return False
    End Function
    Private Function GetCustomerName(ByVal InputLine As String) As String
        Dim tag() As String

        Try
            tag = Split(InputLine, "|")
            tag = Split(tag(6), ":")
            Return (Replace(tag(1), "@CrLf@", vbCrLf))
        Catch ex As Exception
            Return "" 'its old project
        End Try
        Return ""
    End Function

    Private Function GetInstance(ByVal InputLine As String) As String
        Dim tag() As String
        Try
            tag = Split(InputLine, "|")
            tag = Split(tag(7), ":")
            Return (Replace(tag(1), "@CrLf@", vbCrLf))

        Catch ex As Exception
            Return "" 'its old project
        End Try
        Return ""
    End Function
    Private Function isPretranslateEnabled(ByVal InputLine As String) As Boolean
        Dim tag() As String
        Try
            tag = Split(InputLine, "|")
            tag = Split(tag(10), ":")
            Return CBool(Replace(tag(1), "@CrLf@", vbCrLf))
        Catch ex As Exception
            Return False 'its old project
        End Try
        Return False
    End Function

    Private Function isUploadtoDBenabled(ByVal InputLine As String) As Boolean
        Dim tag() As String
        Try
            tag = Split(InputLine, "|")
            tag = Split(tag(11), ":")
            Return CBool(Replace(tag(1), "@CrLf@", vbCrLf))
        Catch ex As Exception
            Return False 'its old project
        End Try
        Return False
    End Function

    Private Function isCheckCorruptionEnabled(ByVal InputLine As String) As Boolean
        Dim tag() As String
        Try
            tag = Split(InputLine, "|")
            tag = Split(tag(12), ":")
            Return CBool(Replace(tag(1), "@CrLf@", vbCrLf))
        Catch ex As Exception
            Return False 'its old project
        End Try
        Return False
    End Function

    Private Function isInstanceEnabled(ByVal InputLine As String) As Boolean
        Dim tag() As String
        Try
            tag = Split(InputLine, "|")
            tag = Split(tag(9), ":")
            Return CBool(Replace(tag(1), "@CrLf@", vbCrLf))
        Catch ex As Exception
            Return False 'its old project
        End Try
        Return False
    End Function

    Private Function isRestrictCustomer(ByVal InputLine As String) As Boolean
        Dim tag() As String
        Try
            tag = Split(InputLine, "|")
            tag = Split(tag(8), ":")
            Return CBool(Replace(tag(1), "@CrLf@", vbCrLf))
        Catch ex As Exception
            Return False 'its old project
        End Try
        Return False
    End Function
    Private Function get_last_projectlanguages(ByVal InputLine As String) As String
        Dim tag() As String
        tag = Split(InputLine, "|")
        Return tag(2)
        Return ""
    End Function
    Private Function GetProjectGroupName(ByVal InputLine As String) As String
        Dim tag() As String
        Try
            tag = Split(InputLine, "|")
            tag = Split(tag(13), ":")
            Return (Replace(tag(1), "@CrLf@", vbCrLf))
        Catch ex As Exception
            Return "Default" 'its old project
        End Try
        Return String.Empty

    End Function
    Private Function get_last_projectpath(ByVal InputLine As String) As String
        Dim mytag() As String
        mytag = Split(InputLine, "|")
        Return mytag(3)
    End Function

End Class



