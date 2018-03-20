Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports CloudTranslator


<TestClass()> Public Class UnitTest1

    ''' <summary>
    ''' Test Analye Function
    ''' </summary>
    <TestMethod()> Public Sub TestMethod1()

        Dim objf As New AnalyzeObject
        Dim localsource As String = "This section is for evaluating the accomplishments of the Core Competencies.
[[BREAK]][[BREAK]]
Ratings must be entered at the end of year assessment, while comments are optional."
        Dim localtransation As String = "Hiredepartment"
        Dim dbsource As String = " HireDepartment"
        Dim dbtranslation As String = "Hire department"
        Dim obj As AnalyzeStats.TranslationType = objf.getTranslationType(dbsource, localsource, dbtranslation, localtransation)
        '//Analyze()
    End Sub

    Sub Analyze()
        'Dim xliffFIle As String = "C:\Users\C5195092\Desktop\issue1506\1506Picklist\09-ExistingTranslation\2.5_de_DE.xliff"
        'Dim objstats As AnalyzeStats = AnalyzeObject.GenerateReport(xliffFIle, "", "")
        'Dim objListStats As New List(Of AnalyzeStats)
        'objListStats.Add(objstats)
        'Dim objexcel As New ClsExcel
        'objexcel.CreateAnalyzeReport(objListStats)
    End Sub

    Sub CloudTRTest()
        Dim objCloud As New Cloud_TR.CloudTr
        With objCloud
            .Customer = "NGD"
            .CustomerSpecific = 1
            .Datatype = "2.5"
            .Instance = "NG1502V1"
            .Resname = "yesnomaybe"
            .Source = "1. Yes"
            .Target = ""
        End With

        Dim objC As New CloudWebServiceNew
        Dim CT() As Cloud_TR.CloudTr
        ' CT = objC.SearchCloud("", True, Cloud_TR.SearchingField.Both)

    End Sub

End Class