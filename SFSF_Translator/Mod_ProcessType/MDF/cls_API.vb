Imports System.Text
Imports System.Threading.Tasks
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.IO
Imports System.Threading
Imports System.Xml
Public Class cls_API

    Public MdfExport_response As String = ""
    Public MdfExport_status As Integer = 0
    Private API_SessionId As String = "https://csm-cluster.wdf.sap.corp/komvos/microservice/sfbizx/session"
    Private API_MDFTypes As String = "https://csm-cluster.wdf.sap.corp/komvos/microservice/sfbizx/session/<SessionID>/mdf/export/type"
    Private API_JobId As String = "https://csm-cluster.wdf.sap.corp/komvos/scenario/sfbizxMdfExport"
    Private API_MdfDesc As String = "https://csm-cluster.wdf.sap.corp/komvos/job/"
    Private Enum MdfStatus
        NONE = 0
        RUNNING = 1
        [END] = 2
        FAIL = 3
    End Enum

    Public Sub create_XMLFile()
        Try
            If Not File.Exists(Application.StartupPath & "\MDFGroup.xml") Then
                Dim xmlDoc As XmlDocument = New XmlDocument()
                Dim xmlDeclaration As XmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", Nothing)
                Dim rootNode As XmlElement = xmlDoc.CreateElement("MDFGroupList")
                xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement)
                xmlDoc.AppendChild(rootNode)
                xmlDoc.Save(Application.StartupPath & "\MDFGroup.xml")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GetJobId(ByVal BaseUrl As String, ByVal CompId As String, ByVal UserName As String, ByVal Pass As String, ByVal TypeId As String) As String
        Try
            Dim json_baseUrl As String = """" & BaseUrl & """"
            Dim json_CompId As String = """" & CompId & """"
            Dim json_UName As String = """" & UserName & """"
            Dim json_Pass As String = """" & Pass & """"
            Dim json_TypeId As String = """" & TypeId & """"
            Dim jsonContent As String = "{ ""baseUrl"": " & json_baseUrl & ", ""companyId"": " & json_CompId & ", ""username"": " & json_UName & ", ""password"": " & json_Pass & ", ""typeIds"":[" & json_TypeId & "],""isRaw"": false}"
            Dim request As HttpWebRequest = CType(WebRequest.Create(API_JobId), HttpWebRequest)
            request.Method = "POST"
            Dim encoding As System.Text.UTF8Encoding = New System.Text.UTF8Encoding()
            Dim byteArray As Byte() = encoding.GetBytes(jsonContent)
            request.ContentLength = byteArray.Length
            request.ContentType = "application/json"
            Using dataStream As Stream = request.GetRequestStream()
                dataStream.Write(byteArray, 0, byteArray.Length)
            End Using

            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
                responseString = responseString.Remove(0, 1)
                responseString = responseString.Remove(responseString.Length - 1, 1)
                Return responseString
            End Using
        Catch
            Throw
        End Try
    End Function
    Public Function GetMdfInfo(ByVal JobId As String) As String
        Try
            Dim _stat As Integer = SentMdfRequest(JobId)
            If _stat = CInt(MdfStatus.NONE) Then
                SentMdfRequest(JobId)
            End If

            If _stat = CInt(MdfStatus.RUNNING) Then
                Thread.Sleep(10000)
                GetMdfInfo(JobId)
            ElseIf _stat = CInt(MdfStatus.[END]) Then
                Return MdfExport_response
            ElseIf _stat = CInt(MdfStatus.FAIL) Then
                MdfExport_response = "Status..FAIL; Unable to get the response."
                Return MdfExport_response
            End If

            Return MdfExport_response
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function SentMdfRequest(ByVal JobId As String) As Integer
        Try
            Dim request1 = CType(WebRequest.Create(API_MdfDesc & JobId), HttpWebRequest)
            request1.Method = "GET"
            request1.ContentType = "text/plain"
            Thread.Sleep(10000)
            Dim response1 = CType(request1.GetResponse(), HttpWebResponse)
            MdfExport_response = New StreamReader(response1.GetResponseStream()).ReadToEnd()
            Dim stat As Integer = GetMdfStatus(MdfExport_response)
            Return stat
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function GetMdfStatus(ByVal str_Mdfjson As String) As Integer
        Dim _Status As Integer = 0
        If str_Mdfjson = "" Then
            MdfExport_status = CInt(MdfStatus.NONE)
        End If

        'Dim jObj As Dynamic = JObject.Parse(str_Mdfjson)
        Dim jObj = JObject.Parse(str_Mdfjson)
        Dim str_Status As String = jObj("status").ToString()
        Select Case str_Status.ToUpper()
            Case "RUN"
                _Status = CInt(MdfStatus.RUNNING)
            Case "END"
                _Status = CInt(MdfStatus.[END])
            Case "FAIL"
                _Status = CInt(MdfStatus.FAIL)
        End Select

        Return _Status
    End Function
    Public Function GetSession4MdfType(ByVal BaseUrl As String, ByVal CompId As String, ByVal UserName As String, ByVal Pass As String) As String
        Try
            Dim json_baseUrl As String = """" & BaseUrl & """"
            Dim json_company As String = """" & CompId & """"
            Dim json_UName As String = """" & UserName & """"
            Dim json_Pass As String = """" & Pass & """"
            Dim jsonContent As String = "{ ""baseUrl"": " & json_baseUrl & ", ""company"": " & json_company & ", ""username"": " & json_UName & ", ""password"": " & json_Pass & "}"
            Dim request As HttpWebRequest = CType(WebRequest.Create(API_SessionId), HttpWebRequest)
            request.Method = "POST"
            Dim encoding As System.Text.UTF8Encoding = New System.Text.UTF8Encoding()
            Dim byteArray As Byte() = encoding.GetBytes(jsonContent)
            request.ContentLength = byteArray.Length
            request.ContentType = "application/json"
            Using dataStream As Stream = request.GetRequestStream()
                dataStream.Write(byteArray, 0, byteArray.Length)
            End Using

            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
                responseString = responseString.Remove(0, 1)
                responseString = responseString.Remove(responseString.Length - 1, 1)
                Return responseString
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetMDFtypes_Raw(ByVal SessionId As String) As String
        Try
            Dim request1 = CType(WebRequest.Create(API_MDFTypes.Replace("<SessionID>", SessionId)), HttpWebRequest)
            request1.Method = "GET"
            request1.ContentType = "text/plain"
            Dim response1 = CType(request1.GetResponse(), HttpWebResponse)
            Dim MdfType_response As String = New StreamReader(response1.GetResponseStream()).ReadToEnd()
            Return MdfType_response
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetJobId_Microservice() As String
        Dim request = CType(WebRequest.Create("https://csm-cluster.wdf.sap.corp/komvos/microservice/sfbizx/session"), HttpWebRequest)
        Dim postData = "baseUrl=https://pmsalesdemo8.successfactors.com"
        postData += "&company=1702SteveShen"
        postData += "&username=sfadmin"
        postData += "&password=riesling"
        Dim data = Encoding.ASCII.GetBytes(postData)
        request.Method = "POST"
        request.ContentType = "application/x-www-form-urlencoded"
        request.ContentLength = data.Length
        Using stream = request.GetRequestStream()
            stream.Write(data, 0, data.Length)
        End Using

        Dim response = CType(request.GetResponse(), HttpWebResponse)
        Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
        responseString = responseString.Remove(0, 1)
        responseString = responseString.Remove(responseString.Length - 1, 1)
        Return responseString
    End Function
    Public Function GetMdfInfo(ByVal SessionID As String, ByVal TypeID As String) As String
        Dim request1 = CType(WebRequest.Create("https://csm-cluster.wdf.sap.corp/komvos/microservice/sfbizx/session/" & SessionID & "/mdf/export/type/" & TypeID), HttpWebRequest)
        request1.Method = "GET"
        request1.ContentType = "application/x-www-form-urlencoded"
        Dim response1 = CType(request1.GetResponse(), HttpWebResponse)
        Dim responseString1 = New StreamReader(response1.GetResponseStream()).ReadToEnd()
        Return responseString1
    End Function
End Class
