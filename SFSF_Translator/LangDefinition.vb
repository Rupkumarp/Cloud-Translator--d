Public NotInheritable Class LangDefintion
    Public Shared Sub GetLanguageList()
        If Not System.IO.File.Exists(appData & "\FileType\languages.txt") Then
            Throw New Exception("Language.txt definition file not found in " & vbNewLine & "'" & Application.StartupPath & "\Definition' folder." & vbNewLine & "Cannot Proceed!")
        End If
        Try
            LanguageDefination.LangFullName = New ArrayList
            LanguageDefination.LangFiveChars = New ArrayList
            'LanguageDefination.LangTwoChars = New ArrayList
            LanguageDefination.LangFourChars = New ArrayList
            For Each lang In Split(System.IO.File.ReadAllText(appData & "\FileType\languages.txt"), vbCrLf)
                If lang.Trim <> "" Then
                    Dim l As String() = Split(lang, Chr(9))
                    LanguageDefination.LangFullName.Add(l(0))
                    LanguageDefination.LangFiveChars.Add(l(1))
                    LanguageDefination.LangFourChars.Add(l(2))
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub
End Class