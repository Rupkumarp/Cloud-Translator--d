Imports System.Xml.Serialization
Imports System.Text
Imports System.IO
Imports System.Xml

Public Class XMLMethod

    Public Shared Function SaveProjectGroupList() As Boolean

        Dim FileName As String = appData & "\projects.xml"

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
            Dim writer As New XmlSerializer(LstProjectGroup.GetType)

            'now write it out 
            writer.Serialize(xw, LstProjectGroup, nsBlank)

            'be sure to close it or it will remain open 
            xw.Close()

            Return True

        Catch ex As Exception
            Throw New Exception(ex.Message & vbNewLine & ex.InnerException.Message)
        End Try

    End Function


    Public Shared Function GetProjectGroupListFromXml() As List(Of ProjectGroup)

        Dim FileName As String = appData & "\projects.xml"
        Try
            If Not System.IO.File.Exists(FileName) Then
                If System.IO.File.Exists(appData & "\projects.txt") Then
                    Dim objCF As New cross_form_functions
                    objCF.GetProjectGroupInfo()
                    SaveProjectGroupList()
                End If
            End If
        Catch ex As Exception
            Throw New Exception("Error upgrading to xml Project!")
            Exit Function
        End Try

        Dim LstProjectGroup As New List(Of ProjectGroup)
        Try
            If System.IO.File.Exists(FileName) Then
                'Deserialize text file to a new object.
                Dim objStreamReader As New StreamReader(FileName)
                Dim reader As New XmlSerializer(LstProjectGroup.GetType)
                LstProjectGroup = reader.Deserialize(objStreamReader)
                objStreamReader.Close()
            End If

        Catch ex As Exception
            Throw New Exception(ex.InnerException.Message)
        End Try
        LstProjectGroup.Sort(Function(x, y) x.ProjectGroupName.CompareTo(y.ProjectGroupName))
        Return LstProjectGroup
    End Function

    Public Shared Function SaveProjectCTP(ByVal CtpLocation As String, ByVal PD As ProjectDetail)
        Dim FileName As String = CtpLocation & "projects.ini"

        If System.IO.File.Exists(FileName) Then
            File.Delete(FileName)
        End If

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
            Dim writer As New XmlSerializer(PD.GetType)

            'now write it out 
            writer.Serialize(xw, PD, nsBlank)

            'be sure to close it or it will remain open 
            xw.Close()

            Return True

        Catch ex As Exception
            Throw New Exception(ex.Message & vbNewLine & ex.InnerException.Message)

        End Try
    End Function

    Public Shared Function GetProjectDetailFromCTP(ByVal iniFile As String) As ProjectDetail

        Dim PD As New ProjectDetail
        Try
            'Deserialize text file to a new object.
            Dim objStreamReader As New StreamReader(iniFile)
            Dim reader As New XmlSerializer(PD.GetType)
            PD = reader.Deserialize(objStreamReader)
            objStreamReader.Close()

        Catch ex As Exception
            Throw New Exception(ex.InnerException.Message)
        End Try
        Return PD
    End Function


    Public Shared Function SaveProjectCtproj(ByVal CtprojLocation As String, ByVal PG As ProjectGroup) As Boolean

        CtprojLocation = CtprojLocation & "\projects.ini"

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
            Dim xw As System.Xml.XmlWriter = Xml.XmlWriter.Create(CtprojLocation, xSettings)

            'create the xmlserializer that will serialize the object to XML 
            Dim writer As New XmlSerializer(PG.GetType)

            'now write it out 
            writer.Serialize(xw, PG, nsBlank)

            'be sure to close it or it will remain open 
            xw.Close()

            Return True

        Catch ex As Exception
            Throw New Exception(ex.Message & vbNewLine & ex.InnerException.Message)
        End Try

    End Function

    Public Shared Function GetProjectGroupDetailFromCtproj(ByVal iniFile As String) As ProjectGroup

        Dim PG As New ProjectGroup
        Try
            'Deserialize text file to a new object.
            Dim objStreamReader As New StreamReader(iniFile)
            Dim reader As New XmlSerializer(PG.GetType)
            PG = reader.Deserialize(objStreamReader)
            objStreamReader.Close()

        Catch ex As Exception
            Throw New Exception(ex.InnerException.Message)
        End Try
        Return PG
    End Function


End Class


