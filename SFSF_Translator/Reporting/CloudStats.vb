Imports System.IO
Imports System.Text
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions

Public Class CloudStats

    Public Sub GetReport(ByVal ActiveProject As ProjectDetail)

        Dim mcr As CloudReporting.MyCloudReport = CloudReporting.MyCloudReport.GetInstance

        Dim localDateTime As System.DateTime = Now.AddDays(0)

        Dim Mydate As String = Format(localDateTime.Date.AddDays(0).Date, "MM/dd/yyyy")

        If Not mcr.DateCollection.Contains(Mydate) Then
            mcr.DateCollection.Add(Mydate)
        End If

        Dim CPS As CloudProjectsettings

        Dim xliffdata As sXliff = Nothing

        For Each _sFile In My.Computer.FileSystem.GetFiles(ActiveProject.ProjectPath & CloudProjectsettings.Folder_Input, FileIO.SearchOption.SearchTopLevelOnly)

            Dim FS As CloudReporting.FileStats

            Dim _Curlang() As String = ActiveProject.LangList.Split(",")

            For f = 0 To UBound(_Curlang)

                FS = New CloudReporting.FileStats

                Dim curLang As String = _Curlang(f).Insert(2, "_")

                CPS = New CloudProjectsettings(ActiveProject, _sFile, curLang)

                FS.FileName = System.IO.Path.GetFileName(System.IO.Path.GetFileName(_sFile))

                FS.Lang = curLang

                FS.mDate = Mydate

                'Normal Cloud Process
                If File.Exists(CPS.Xliff_FileInTobetransalted) Then

                    FS.FileStatus = CloudReporting.FileStats.ProjectStatus.UnderTranslation
                    xliffdata = load_xliff(CPS.Xliff_FileInTobetransalted)

                    'check now if the translation is already back.
                    If File.Exists(CPS.Xliff_FileInBackFromtranslation) Then
                        FS.FileStatus = CloudReporting.FileStats.ProjectStatus.BackFromTranslation
                    ElseIf File.Exists(CPS.Xliff_ProcessedFileInBackFromtranslation) Then
                        FS.FileStatus = CloudReporting.FileStats.ProjectStatus.Integrated
                    Else
                        'Not integrated
                    End If

                    'GetWordCount and Transunit count

                    FS.WordCount = 0 'GetwordCount here
                    FS.TransUnitCount = 0

                    If Not xliffdata.ID Is Nothing Then
                        FS.TransUnitCount = xliffdata.ID.Count
                        For i As Integer = 0 To xliffdata.Source.Count - 1 'Wordcount
                            FS.WordCount += CountWords(xliffdata.Source(i))
                        Next
                    End If

                Else 'Extract xliff file.

                    FS.FileStatus = CloudReporting.FileStats.ProjectStatus.NewFile
                End If

                Dim Index As Integer = GetFileStatsIndex(mcr.FStat, FS)

                If Index = -1 Then
                    mcr.FStat.Add(FS)
                Else
                    mcr.FStat(Index) = FS
                End If

            Next

        Next

        CloudReporting.ReportingOperations.SaveReportStat(ActiveProject.ProjectPath, mcr)

    End Sub

    Private Function GetFileStatsIndex(ByRef Fstat As List(Of CloudReporting.FileStats), ByVal fs As CloudReporting.FileStats) As Integer
        Dim index As Integer = -1
        For i As Integer = 0 To Fstat.Count - 1
            If (Fstat(i).FileName = fs.FileName) And (Fstat(i).Lang = fs.Lang) And (Fstat(i).mDate = fs.mDate) Then
                index = i
                Exit For
            End If
        Next
        Return index
    End Function

    Private Function CountWords(ByVal value As String) As Integer
        ' Count matches.
        Dim collection As MatchCollection = Regex.Matches(value, "\S+")
        Return collection.Count
    End Function


End Class
