Imports System.Xml
Imports System.IO
Imports System.Xml.Schema

Module Mod_SLC_To_Xml

    Dim notFound As ArrayList
    Dim bMatch As Boolean
    Dim counter As Integer

    Public Enum SlcType
        Section
        Question
    End Enum

    Dim objFileType As SlcType

    Function SLC_to_xml(ByVal originalfile_path As String, ByVal translated_xliff_path As String, ByVal TT As TranslationType, ByVal lang As String) As Boolean
        Dim targetfilepath As String = ""
        notFound = New ArrayList
        Try
            If TT = TranslationType.Monolingual Then
                Dim sFileName As String = System.IO.Path.GetFileName(originalfile_path)

                'Monolingual
                targetfilepath = Replace(originalfile_path, "01-Input-B", "05-Output")
                targetfilepath = System.IO.Path.GetDirectoryName(targetfilepath) & "\Mono_" & lang & "\"

                If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(targetfilepath)) <> True Then
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetfilepath))
                End If

                targetfilepath = targetfilepath & sFileName

                File.Copy(originalfile_path, targetfilepath, True)

            Else
                If InStr(originalfile_path, "01-Input-B") <> 0 Then
                    'case 1 first language
                    targetfilepath = Replace(originalfile_path, "01-Input-B", "05-Output")

                ElseIf InStr(originalfile_path, "05-Output") <> 0 Then
                    'case 2 subsequent languages
                    targetfilepath = originalfile_path
                    originalfile_path = Replace(originalfile_path, "05-Output", "01-Input-B")

                Else
                    'case 3, project structure not used. Manual operations. Just add _out.
                    targetfilepath = Path.GetDirectoryName(originalfile_path) & "\" & Path.GetFileNameWithoutExtension(originalfile_path) & ".out" & Path.GetExtension(originalfile_path)

                End If

                If CheckFileOrFolderExists(targetfilepath, fType.file) <> True Then
                    Return False
                End If

                If CheckFileOrFolderExists(translated_xliff_path, fType.file) <> True Then
                    Return False
                End If
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Try
            Dim objXliff As New sXliff
            Try
                objXliff = ModHelper.load_xliff(translated_xliff_path)
            Catch ex As Exception
                If ModHelper.UnWrapXliffBack(translated_xliff_path) <> True Then
                    Throw New Exception("Error UnWrapping xliff back!")
                End If
                objXliff = ModHelper.cvload_xliff(Application.StartupPath & "\Temp_UnWrap.xliff")
            End Try

            Dim objXmlDefinition As New XmlDefinition
            objXmlDefinition.GetXmlDefinition(originalfile_path)

            If objXmlDefinition.DefinitionFile = "SLC_Question" Then
                objFileType = SlcType.Question
            Else
                objFileType = SlcType.Section
            End If

            Select Case TT
                Case TranslationType.Multilingual
                    'noting
                Case TranslationType.Monolingual
                    Select Case objFileType
                        Case SlcType.Question
                            MonoLingual_TranslationQuestion(targetfilepath, objXmlDefinition, objXliff, lang)
                        Case SlcType.Section
                            MonoLingual_TranslationSection(targetfilepath, objXmlDefinition, objXliff, lang)
                    End Select
            End Select

            'NO Translation found then show a msg box and log it as well.
            If notFound.Count > 0 Then
                Dim objMissingTransaltion As New MissedTranslations
                objMissingTransaltion.UpdateMsg(notFound, originalfile_path, lang)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return True
    End Function

#Region "Monolingual Translation for Question"

    Private Class Question
        Public transunit As String
        Public text As String
        Public answer As String
        Public explanation As String
        Public tText As String 'translation for text
        Public tAnswer As String 'translation for answer
        Public tExplanation As String 'transaltion for explanation
    End Class

    Sub MonoLingual_TranslationQuestion(ByRef xmlFile As String, ByVal objXmlDefinition As XmlDefinition, ByVal objXliff As sXliff, ByVal lang As String)
        lang = "ZH"
        Try
            Dim objQuestion As New List(Of Question)
            Dim MatchingTransunits As New ArrayList

            'Step1
            'Group matching transunit.
            MatchingTransunits = GetMatchingTransunitsQuestion(objXliff)

            'Step2
            'Using MatchingTranunits create Question object
            objQuestion = CreateQuestionObject(MatchingTransunits, objXliff)

            'Step3
            'Rebuild xml file

            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(xmlFile)
            For i As Integer = 0 To objQuestion.Count - 1

                Dim xNodeList As XmlNodeList

                xNodeList = xd.GetElementsByTagName("item")

                For j As Integer = 0 To xNodeList.Count - 1

                    Dim cloneNode As XmlNode = Nothing

                    If CheckTranslationExistforQUESTION(xNodeList, objQuestion(i)) = True Then
                        Exit For
                    ElseIf ValidateQuestionNode(xNodeList(j).ChildNodes, objXmlDefinition, objQuestion(i)) = True Then
                        cloneNode = xNodeList(j).Clone

                        'Push integration now 
                        If objQuestion(i).answer <> "" Then
                            For k As Integer = 0 To cloneNode.ChildNodes.Count - 1
                                If cloneNode.ChildNodes(k).Name.ToLower = "answer" Then
                                    cloneNode.ChildNodes(k).InnerText = objQuestion(i).tAnswer
                                    Exit For
                                End If
                            Next
                        End If

                        If objQuestion(i).text <> "" Then
                            For k As Integer = 0 To cloneNode.ChildNodes.Count - 1
                                If cloneNode.ChildNodes(k).Name.ToLower = "text" Then
                                    cloneNode.ChildNodes(k).InnerText = objQuestion(i).tText
                                    Exit For
                                End If
                            Next
                        End If

                        If objQuestion(i).explanation <> "" Then
                            For k As Integer = 0 To cloneNode.ChildNodes.Count - 1
                                If cloneNode.ChildNodes(k).Name.ToLower = "explanation" Then
                                    cloneNode.ChildNodes(k).InnerText = objQuestion(i).tExplanation
                                    Exit For
                                End If
                            Next
                        End If

                        For k As Integer = 0 To cloneNode.ChildNodes.Count - 1
                            If cloneNode.ChildNodes(k).Name.ToLower = "line" Then
                                Select Case cloneNode.ChildNodes(0).InnerText.ToLower()
                                    Case "q"
                                        cloneNode.ChildNodes(0).InnerText = "QT"
                                    Case "a"
                                        cloneNode.ChildNodes(0).InnerText = "AT"
                                End Select
                            ElseIf cloneNode.ChildNodes(k).Name.ToLower = "language_code" Then
                                cloneNode.ChildNodes(k).InnerText = lang
                            ElseIf cloneNode.ChildNodes(k).Name.ToLower = "translation_status" Then
                                cloneNode.ChildNodes(k).InnerText = "03"
                            End If
                        Next
                        xNodeList(j).ParentNode.InsertAfter(cloneNode, xNodeList(j))
                    End If
                Next j
            Next

            xd.Save(xmlFile)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Private Function CheckTranslationExistforQUESTION(ByRef xnodelist As XmlNodeList, ByVal objQuestion As Question) As Boolean
        Dim transunti() As String = Split(objQuestion.transunit, "_")
        Try
            For i As Integer = 0 To xnodelist.Count - 1
                'check line
                Dim bFound As Boolean = False
                For j As Integer = 0 To xnodelist(i).ChildNodes.Count - 1
                    'check line
                    If xnodelist(i).ChildNodes(j).Name.ToLower = "line" And (xnodelist(i).ChildNodes(j).InnerText.ToLower = "qt" Or xnodelist(i).ChildNodes(j).InnerText.ToLower = "at") Then
                        bFound = True
                    End If

                    If bFound Then
                        bFound = False
                        'check translation status
                        For k As Integer = 0 To xnodelist(i).ChildNodes.Count - 1
                            If xnodelist(i).ChildNodes(k).Name.ToLower = "translation_status" And xnodelist(i).ChildNodes(k).InnerText.ToLower <> "03" Then
                                bFound = True
                                Exit For
                            End If
                        Next
                    End If

                    If bFound Then
                        bFound = False
                        'Check ID
                        For l As Integer = 0 To xnodelist(i).ChildNodes.Count - 1
                            If xnodelist(i).ChildNodes(l).Name.ToLower = "id" And xnodelist(i).ChildNodes(l).InnerText.ToLower = transunti(2) Then
                                bFound = True
                                Exit For
                            End If
                        Next
                    End If

                    If bFound Then
                        For m As Integer = 0 To xnodelist(i).ChildNodes.Count - 1
                            If xnodelist(i).ChildNodes(m).Name.ToLower = "text" Then
                                xnodelist(i).ChildNodes(m).InnerText = objQuestion.tText
                                Exit For
                            End If
                        Next
                        For n As Integer = 0 To xnodelist(i).ChildNodes.Count - 1
                            If xnodelist(i).ChildNodes(n).Name.ToLower = "answer" Then
                                xnodelist(i).ChildNodes(n).InnerText = objQuestion.tAnswer
                                Exit For
                            End If
                        Next
                        For n As Integer = 0 To xnodelist(i).ChildNodes.Count - 1
                            If xnodelist(i).ChildNodes(n).Name.ToLower = "explanation" Then
                                xnodelist(i).ChildNodes(n).InnerText = objQuestion.tExplanation
                                Exit For
                            End If
                        Next
                        For n As Integer = 0 To xnodelist(i).ChildNodes.Count - 1
                            If xnodelist(i).ChildNodes(n).Name.ToLower = "translation_status" Then
                                xnodelist(i).ChildNodes(n).InnerText = "03"
                                Exit For
                            End If
                        Next
                        Return True
                    End If

                Next
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return False
    End Function

    Private Function GetMatchingTransunitsQuestion(ByRef objXliff As sXliff) As ArrayList
        Dim o As New ArrayList
        Try
            For i As Integer = 0 To objXliff.ID.Count - 1
                Dim transunit() As String = Split(objXliff.ID(i), "_")
                For j As Integer = i + 1 To objXliff.ID.Count - 1
                    Dim transunit2() As String = Split(objXliff.ID(j), "_")
                    If transunit(1).ToLower = "q" Or transunit(1).ToLower = "qt" Then
                        If (transunit(1) = transunit2(1)) And (transunit(2) = transunit2(2)) Then
                            o.Add(i & "-" & j)
                        End If
                    Else
                        If (transunit(1) = transunit2(1)) And (transunit(2) = transunit2(2)) And (transunit(3) = transunit2(3)) Then
                            o.Add(i & "-" & j)
                        End If
                    End If
                Next
            Next
        Catch ex As Exception
            Throw New Exception("Error @GetMatchingTransunitsQuestion" & vbNewLine & ex.Message)
        End Try
        Return o
    End Function

    Private Function CreateQuestionObject(ByRef MatchingTransunits As ArrayList, ByRef objXliff As sXliff) As List(Of Question)
        Dim o As New List(Of Question)
        Dim oQuestion As Question
        Try
            Dim counter As Integer = 0
            If MatchingTransunits.Count > 0 Then
                For i As Integer = 0 To MatchingTransunits.Count - 1
                    Dim numbers() As String = Split(MatchingTransunits(i), "-")
                    oQuestion = New Question
                    For j As Integer = 0 To UBound(numbers)
                        oQuestion.transunit = objXliff.ID(numbers(j))
                        If Mid(objXliff.ID(numbers(j)), InStrRev(objXliff.ID(numbers(j)), "_") + 1, Len(objXliff.ID(numbers(j)))) = "text" Then
                            oQuestion.text = objXliff.Source(numbers(j))
                            oQuestion.tText = objXliff.Translation(numbers(j))
                        ElseIf Mid(objXliff.ID(numbers(j)), InStrRev(objXliff.ID(numbers(j)), "_") + 1, Len(objXliff.ID(numbers(j)))) = "answer" Then
                            oQuestion.answer = objXliff.Source(numbers(j))
                            oQuestion.tAnswer = objXliff.Translation(numbers(j))
                        ElseIf Mid(objXliff.ID(numbers(j)), InStrRev(objXliff.ID(numbers(j)), "_") + 1, Len(objXliff.ID(numbers(j)))) = "explanation" Then
                            oQuestion.explanation = objXliff.Source(numbers(j))
                            oQuestion.tExplanation = objXliff.Translation(numbers(j))
                        End If
                    Next
                    o.Add(oQuestion)
                Next
            End If

            'Get new arraylist ungrouping MatchingTranunits
            Dim RealingnedMatchingTranunits As New ArrayList
            For i As Integer = 0 To MatchingTransunits.Count - 1
                Dim numbers() As String = Split(MatchingTransunits(i), "-")
                For j As Integer = 0 To UBound(numbers)
                    RealingnedMatchingTranunits.Add(CInt(numbers(j)))
                Next
            Next

            'add missing data to Question object

            Dim bfound As Boolean
            For i As Integer = 0 To objXliff.ID.Count - 1
                oQuestion = New Question
                bfound = False
                For j As Integer = 0 To RealingnedMatchingTranunits.Count - 1
                    If i = RealingnedMatchingTranunits(j) Then
                        bfound = True
                        Exit For
                    End If
                Next
                If bfound <> True Then
                    oQuestion.transunit = objXliff.ID(i)
                    If Mid(objXliff.ID(i), InStrRev(objXliff.ID(i), "_") + 1, Len(objXliff.ID(i))).ToLower = "text" Then
                        oQuestion.text = objXliff.Source(i)
                        oQuestion.tText = objXliff.Translation(i)
                    ElseIf Mid(objXliff.ID(i), InStrRev(objXliff.ID(i), "_") + 1, Len(objXliff.ID(i))).ToLower = "answer" Then
                        oQuestion.answer = objXliff.Source(i)
                        oQuestion.tAnswer = objXliff.Translation(i)
                    ElseIf Mid(objXliff.ID(i), InStrRev(objXliff.ID(i), "_") + 1, Len(objXliff.ID(i))).ToLower = "explanation" Then
                        oQuestion.explanation = objXliff.Source(i)
                        oQuestion.tExplanation = objXliff.Translation(i)
                    End If
                    o.Add(oQuestion)
                End If
            Next

        Catch ex As Exception
            Throw New Exception("Error @CreateQuestionObject" & vbNewLine & ex.Message)
        End Try
        Return o
    End Function

    Private Function ValidateQuestionNode(ByVal childNodes As XmlNodeList, ByRef objXmlDefinition As XmlDefinition, ByVal objQuestion As Question) As Boolean

        Dim elementName As String = ""

        Dim MyDict As New Dictionary(Of String, String)
        Dim transunit() As String = Split(objQuestion.transunit, "_")
        Try
            MyDict.Add("line", transunit(1))
            MyDict.Add("id", transunit(2))

            If transunit(1).ToLower = "a" Then
                MyDict.Add("answerid", transunit(3))
            End If

            If objQuestion.answer <> "" Then 'AnswerID
                For k As Integer = 0 To childNodes.Count - 1
                    If childNodes(k).Name.ToLower = "answer_id" Then
                        If childNodes(k).InnerText <> MyDict.Item("answerid") Then
                            Return False
                        End If
                    End If
                Next
            End If

            For k As Integer = 0 To childNodes.Count - 1 'ID
                If childNodes(k).Name.ToLower = "id" Then
                    If childNodes(k).InnerText <> MyDict.Item("id") Then
                        Return False
                    End If
                End If
            Next

            For x As Integer = 0 To MyDict.Count - 1
                elementName = MyDict.Keys(x)
                For i As Integer = 0 To childNodes.Count - 1
                    If elementName.ToLower = childNodes(i).Name.ToLower Then
                        If MyDict.Item(elementName.ToLower).ToLower <> childNodes(i).InnerText.ToLower Then
                            Return False
                        Else
                            Exit For
                        End If
                    End If
                Next
            Next

            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function


#End Region

#Region "Monolingual Translation for Section"
    Sub MonoLingual_TranslationSection(ByRef xmlFile As String, ByVal objXmlDefinition As XmlDefinition, ByVal objXliff As sXliff, ByVal lang As String)
        lang = "ZH"
        Try
            Dim objSection As New List(Of Section)
            Dim MatchingTransunits As New ArrayList

            'Step1
            'Group matching transunit.
            MatchingTransunits = GetMatchingTransunitsSection(objXliff)

            'Step2
            'Using MatchingTranunits create Question object
            objSection = CreateSectionObject(MatchingTransunits, objXliff)

            'Step3
            'Rebuild xml file
            Dim xd As New Xml.XmlDocument
            xd.XmlResolver = Nothing
            xd.Load(xmlFile)

            For i As Integer = 0 To objSection.Count - 1
                Dim xNodeList As XmlNodeList

                xNodeList = xd.GetElementsByTagName("item")

                For j As Integer = 0 To xNodeList.Count - 1

                    Dim cloneNode As XmlNode = Nothing

                    If CheckTranslationExistforSECTION(xNodeList, objSection(i)) = True Then
                        Exit For
                    ElseIf ValidateSectionNode(xNodeList(j).ChildNodes, objXmlDefinition, objSection(i)) = True Then
                        cloneNode = xNodeList(j).Clone

                        'Push integration now 
                        If objSection(i).title <> "" Then
                            For k As Integer = 0 To cloneNode.ChildNodes.Count - 1
                                If cloneNode.ChildNodes(k).Name.ToLower = "title" Then
                                    cloneNode.ChildNodes(k).InnerText = objSection(i).tTitle
                                    Exit For
                                End If
                            Next
                        End If

                        If objSection(i).explanation <> "" Then
                            For k As Integer = 0 To cloneNode.ChildNodes.Count - 1
                                If cloneNode.ChildNodes(k).Name.ToLower = "explanation" Then
                                    cloneNode.ChildNodes(k).InnerText = objSection(i).tExplanation
                                    Exit For
                                End If
                            Next
                        End If

                        For k As Integer = 0 To cloneNode.ChildNodes.Count - 1
                            If cloneNode.ChildNodes(k).Name.ToLower = "line" Then
                                Select Case cloneNode.ChildNodes(0).InnerText.ToLower()
                                    Case "s"
                                        cloneNode.ChildNodes(0).InnerText = "ST"
                                    Case "a"
                                        cloneNode.ChildNodes(0).InnerText = "AT"
                                End Select
                            ElseIf cloneNode.ChildNodes(k).Name.ToLower = "language_code" Then
                                cloneNode.ChildNodes(k).InnerText = lang
                            ElseIf cloneNode.ChildNodes(k).Name.ToLower = "translation_status" Then
                                cloneNode.ChildNodes(k).InnerText = "03"
                            End If
                        Next
                        xNodeList(j).ParentNode.InsertAfter(cloneNode, xNodeList(j))
                    End If
                Next j
            Next

            xd.Save(xmlFile)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Function CheckTranslationExistforSECTION(ByRef xnodelist As XmlNodeList, ByVal objQuestion As Section) As Boolean
        Dim transunti() As String = Split(objQuestion.transunit, "_")
        Try
            For i As Integer = 0 To xnodelist.Count - 1
                'check line
                Dim bFound As Boolean = False
                For j As Integer = 0 To xnodelist(i).ChildNodes.Count - 1
                    'check line
                    If xnodelist(i).ChildNodes(j).Name.ToLower = "line" And xnodelist(i).ChildNodes(j).InnerText.ToLower = "st" Then
                        bFound = True
                    End If

                    If bFound Then
                        bFound = False
                        'check translation status
                        For k As Integer = 0 To xnodelist(i).ChildNodes.Count - 1
                            If xnodelist(i).ChildNodes(k).Name.ToLower = "translation_status" And xnodelist(i).ChildNodes(k).InnerText.ToLower <> "03" Then
                                bFound = True
                                Exit For
                            End If
                        Next
                    End If

                    If bFound Then
                        bFound = False
                        'Check ID
                        For l As Integer = 0 To xnodelist(i).ChildNodes.Count - 1
                            If xnodelist(i).ChildNodes(l).Name.ToLower = "id" And xnodelist(i).ChildNodes(l).InnerText.ToLower = transunti(2) Then
                                bFound = True
                                Exit For
                            End If
                        Next
                    End If

                    If bFound Then
                        For m As Integer = 0 To xnodelist(i).ChildNodes.Count - 1
                            If xnodelist(i).ChildNodes(m).Name.ToLower = "title" Then
                                xnodelist(i).ChildNodes(m).InnerText = objQuestion.tTitle
                                Exit For
                            End If
                        Next
                        For n As Integer = 0 To xnodelist(i).ChildNodes.Count - 1
                            If xnodelist(i).ChildNodes(n).Name.ToLower = "explanation" Then
                                xnodelist(i).ChildNodes(n).InnerText = objQuestion.tExplanation
                                Exit For
                            End If
                        Next
                        For n As Integer = 0 To xnodelist(i).ChildNodes.Count - 1
                            If xnodelist(i).ChildNodes(n).Name.ToLower = "translation_status" Then
                                xnodelist(i).ChildNodes(n).InnerText = "03"
                                Exit For
                            End If
                        Next
                        Return True
                    End If

                Next


            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return False
    End Function

    Private Function ValidateSectionNode(ByVal childNodes As XmlNodeList, ByRef objXmlDefinition As XmlDefinition, ByVal objQuestion As Section) As Boolean

        Dim elementName As String = ""

        Dim MyDict As New Dictionary(Of String, String)
        Dim transunit() As String = Split(objQuestion.transunit, "_")
        Try
            MyDict.Add("line", transunit(1))
            MyDict.Add("id", transunit(2))

            For k As Integer = 0 To childNodes.Count - 1 'ID
                If childNodes(k).Name.ToLower = "id" Then
                    If childNodes(k).InnerText <> MyDict.Item("id") Then
                        Return False
                    End If
                End If
            Next

            For x As Integer = 0 To MyDict.Count - 1
                elementName = MyDict.Keys(x)
                For i As Integer = 0 To childNodes.Count - 1
                    If elementName.ToLower = childNodes(i).Name.ToLower Then
                        If MyDict.Item(elementName.ToLower).ToLower <> childNodes(i).InnerText.ToLower Then
                            Return False
                        Else
                            Exit For
                        End If
                    End If
                Next
            Next

            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Private Function GetMatchingTransunitsSection(ByRef objXliff As sXliff) As ArrayList
        Dim o As New ArrayList
        Try
            For i As Integer = 0 To objXliff.ID.Count - 1
                Dim transunit() As String = Split(objXliff.ID(i), "_")
                For j As Integer = i + 1 To objXliff.ID.Count - 1
                    Dim transunit2() As String = Split(objXliff.ID(j), "_")
                    If (transunit(1) = transunit2(1)) And (transunit(2) = transunit2(2)) Then
                        o.Add(i & "-" & j)
                    End If
                Next
            Next
        Catch ex As Exception
            Throw New Exception("Error @GetMatchingTransunitsSection" & vbNewLine & ex.Message)
        End Try
        Return o
    End Function

    Private Function CreateSectionObject(ByRef MatchingTransunits As ArrayList, ByRef objXliff As sXliff) As List(Of Section)
        Dim o As New List(Of Section)
        Dim oQuestion As Section
        Try
            Dim counter As Integer = 0
            If MatchingTransunits.Count > 0 Then
                For i As Integer = 0 To MatchingTransunits.Count - 1
                    Dim numbers() As String = Split(MatchingTransunits(i), "-")
                    oQuestion = New Section
                    For j As Integer = 0 To UBound(numbers)
                        oQuestion.transunit = objXliff.ID(numbers(j))
                        If Mid(objXliff.ID(numbers(j)), InStrRev(objXliff.ID(numbers(j)), "_") + 1, Len(objXliff.ID(numbers(j)))).ToLower = "title" Then
                            oQuestion.title = objXliff.Source(numbers(j))
                            oQuestion.tTitle = objXliff.Translation(numbers(j))
                        ElseIf Mid(objXliff.ID(numbers(j)), InStrRev(objXliff.ID(numbers(j)), "_") + 1, Len(objXliff.ID(numbers(j)))).ToLower = "explanation" Then
                            oQuestion.explanation = objXliff.Source(numbers(j))
                            oQuestion.tExplanation = objXliff.Translation(numbers(j))
                        End If
                    Next
                    o.Add(oQuestion)
                Next
            End If

            'Get new arraylist ungrouping MatchingTranunits
            Dim RealingnedMatchingTranunits As New ArrayList
            For i As Integer = 0 To MatchingTransunits.Count - 1
                Dim numbers() As String = Split(MatchingTransunits(i), "-")
                For j As Integer = 0 To UBound(numbers)
                    RealingnedMatchingTranunits.Add(CInt(numbers(j)))
                Next
            Next

            'add missing data to Question object

            Dim bfound As Boolean
            For i As Integer = 0 To objXliff.ID.Count - 1
                oQuestion = New Section
                bfound = False
                For j As Integer = 0 To RealingnedMatchingTranunits.Count - 1
                    If i = RealingnedMatchingTranunits(j) Then
                        bfound = True
                        Exit For
                    End If
                Next
                If bfound <> True Then
                    oQuestion.transunit = objXliff.ID(i)
                    If Mid(objXliff.ID(i), InStrRev(objXliff.ID(i), "_") + 1, Len(objXliff.ID(i))).ToLower = "title" Then
                        oQuestion.title = objXliff.Source(i)
                        oQuestion.tTitle = objXliff.Translation(i)
                    ElseIf Mid(objXliff.ID(i), InStrRev(objXliff.ID(i), "_") + 1, Len(objXliff.ID(i))).ToLower = "explanation" Then
                        oQuestion.explanation = objXliff.Source(i)
                        oQuestion.tExplanation = objXliff.Translation(i)
                    End If
                    o.Add(oQuestion)
                End If
            Next

        Catch ex As Exception
            Throw New Exception("Error @CreateQuestionObject" & vbNewLine & ex.Message)
        End Try
        Return o
    End Function

    Private Class Section
        Public transunit As String
        Public title As String
        Public explanation As String
        Public tTitle As String
        Public tExplanation As String
    End Class

#End Region

End Module
