Public Class CloudWebServiceNew
    Inherits Cloud_TR.Service1

    Protected Overrides Function GetWebRequest(ByVal uri As Uri) As System.Net.WebRequest
        Dim webRequest As System.Net.HttpWebRequest
        webRequest = CType(MyBase.GetWebRequest(uri), System.Net.HttpWebRequest)
        'Setting KeepAlive to false 
        webRequest.KeepAlive = False
        GetWebRequest = webRequest
    End Function

End Class

