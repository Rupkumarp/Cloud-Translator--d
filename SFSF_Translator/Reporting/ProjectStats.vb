Imports System.Xml.Serialization
Imports System.Text
Imports System.IO

Namespace CloudReporting

    Public Class MyCloudReport

        Public Shared mMycloudReport As MyCloudReport 'Singleton implementation

        Public Shared Function GetInstance() As MyCloudReport
            GetMcr()
            If mMycloudReport Is Nothing Then
                mMycloudReport = New MyCloudReport
            End If
            Return mMycloudReport
        End Function

        Public Property DateCollection As New List(Of String)

        Private Shared Sub GetMcr()
            mMycloudReport = ReportingOperations.OpenReportStat(ProjectManagement.GetActiveProject.ProjectPath)
        End Sub

        Public Property FStat As New List(Of FileStats)

    End Class

    Public Class ReportingOperations

        Public Shared Function SaveReportStat(ByVal ProjectPath As String, ByRef MCR As MyCloudReport) As Boolean

            Dim FileName As String = ProjectPath & "ProjectStats.xml"

            'set up a blank namespace to eliminate unnecessary junk from the xml 
            Dim nsBlank As New XmlSerializerNamespaces
            nsBlank.Add("", "")

            'create an object for the xml settings to control how the xml is written and appears 
            Dim xSettings As New System.Xml.XmlWriterSettings
            With xSettings
                .Encoding = Encoding.UTF8
                .Indent = True
                .NewLineChars = Environment.NewLine
                .NewLineOnAttributes = False
                .ConformanceLevel = Xml.ConformanceLevel.Document
            End With

            Try

                'create the xmlwriter object that will write the file out 
                Dim xw As System.Xml.XmlWriter = Xml.XmlWriter.Create(FileName, xSettings)

                'create the xmlserializer that will serialize the object to XML 
                Dim writer As New XmlSerializer(MCR.GetType)

                'now write it out 
                writer.Serialize(xw, MCR, nsBlank)

                'be sure to close it or it will remain open 
                xw.Close()

                Return True

            Catch ex As Exception
                Throw New Exception(ex.Message & vbNewLine & ex.InnerException.Message)
            End Try

        End Function

        Public Shared Function OpenReportStat(ByVal ProjectPath As String) As MyCloudReport

            Dim MCR As New MyCloudReport

            Dim FileName As String = ProjectPath & "ProjectStats.xml"

            If Not System.IO.File.Exists(FileName) Then
                Return MCR
            End If

            Dim objStreamReader As StreamReader = Nothing
            Try
                'Deserialize text file to a new object.
                objStreamReader = New StreamReader(FileName)
                Dim reader As New XmlSerializer(MCR.GetType)
                MCR = reader.Deserialize(objStreamReader)

            Catch ex As Exception
                Return MCR
                'Throw New Exception(ex.InnerException.Message)
            Finally
                objStreamReader.Close()
            End Try
            Return MCR
        End Function
    End Class

    Public Class FileStats
        Public Enum ProjectStatus
            UnderTranslation
            BackFromTranslation
            Integrated
            NewFile
        End Enum
        Public Property FileName As String
        Public Property FileStatus As ProjectStatus
        Public Property Lang As String
        Public Property WordCount As Integer
        Public Property TransUnitCount As Integer
        Public Property mDate As String
    End Class

End Namespace





