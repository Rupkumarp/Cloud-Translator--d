Imports System.Xml
Imports System.Xml.Schema
Imports System.Text

Public Class ValidateXml
    Public Sub Validate(ByVal xmlFile As String)

        Dim myDocument As New XmlDocument
        myDocument.Load(xmlFile)
        myDocument.Schemas.Add("", "C:\someschema.xsd")
        Dim eventHandler As ValidationEventHandler = New ValidationEventHandler(AddressOf ValidationEventHandler)
        myDocument.Validate(eventHandler)
    End Sub

    Private Sub ValidationEventHandler(ByVal sender As Object, ByVal e As ValidationEventArgs)
        Select Case e.Severity
            Case XmlSeverityType.Error
                Debug.WriteLine("Error: {0}", e.Message)
            Case XmlSeverityType.Warning
                Debug.WriteLine("Warning {0}", e.Message)
        End Select
    End Sub

    Public Function LoadValidatedXDocument(xmlFilePath As String, xsdFilePath As String) As XDocument
        Dim doc As XDocument = XDocument.Load(xmlFilePath)
        Dim schemas As New XmlSchemaSet()
        schemas.Add(Nothing, xsdFilePath)
        Dim errorBuilder As New XmlValidationErrorBuilder()
        doc.Validate(schemas, New ValidationEventHandler(AddressOf errorBuilder.ValidationEventHandler))
        Dim errorsText As String = errorBuilder.GetErrors()
        If errorsText IsNot Nothing Then
            Throw New Exception(errorsText)
        End If
        Return doc
    End Function

End Class


Public Class XmlValidationErrorBuilder
    Private _errors As New List(Of ValidationEventArgs)()

    Public Sub ValidationEventHandler(ByVal sender As Object, ByVal args As ValidationEventArgs)
        If args.Severity = XmlSeverityType.Error Then
            _errors.Add(args)
        End If
    End Sub

    Public Function GetErrors() As String
        If _errors.Count <> 0 Then
            Dim builder As New StringBuilder()
            builder.Append("The following ")
            builder.Append(_errors.Count.ToString())
            builder.AppendLine(" error(s) were found while validating the XML document against the XSD:")
            For Each i As ValidationEventArgs In _errors
                builder.Append("* ")
                builder.AppendLine(i.Message)
            Next
            Return builder.ToString()
        Else
            Return Nothing
        End If
    End Function
End Class



