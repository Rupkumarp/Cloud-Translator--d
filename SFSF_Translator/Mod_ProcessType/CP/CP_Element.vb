Public Class CP_Element

    Dim _CpIndex As Integer
    Dim _CpList As New ArrayList
    Dim _CpDocType As String
    Dim _CpHeaders As New Dictionary(Of String, Integer)
    Dim _objActType As Action

    Public Enum Action
        Create
        Retrieve
    End Enum

    Public Sub New(ByVal CP_Index As Integer, ByVal CP_List As ArrayList, ByVal CpDoctype As String, ByVal CpHeaders As Dictionary(Of String, Integer), ByVal ActionType As Action)
        _CpIndex = CP_Index
        _CpList = CP_List
        _CpDocType = CpDoctype
        _CpHeaders = CpHeaders
        _objActType = ActionType
    End Sub


    Dim Tbl_Panel As New TableLayoutPanel
    Dim RowCount As Integer

    Public Event CopyControl(ByVal txtbxName As String)
    Public Event PasteControl(ByVal txtbxName As String)
    Public Event Lst_Textbox_Controls(ByVal txtControls As ArrayList)
    Public Event Lst_Label_Controls(ByVal lblControls As ArrayList)
    Public Event SpaceNextItem(ByVal txtbxName As String, ByVal lblName As String)

    Public xmlFileName As String

    Public Function CreateCP() As TableLayoutPanel

        Try
            Tbl_Panel.Controls.Clear()
            Tbl_Panel.ColumnStyles.Clear()
            Tbl_Panel.RowStyles.Clear()

            Tbl_Panel.Dock = DockStyle.Top
            Tbl_Panel.AutoSize = True

            Tbl_Panel.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
            Tbl_Panel.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
            Tbl_Panel.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
            'Tbl_Panel.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
            'Tbl_Panel.ColumnCount = 4
            Tbl_Panel.ColumnCount = 3

            AddHandler getTxtbxControlName, AddressOf getTxtbxControlList
            AddHandler getLblControlName, AddressOf getLblControlList

            Dim index As Integer = 0

            'For CP Retrieve change the cpindex required to get correct cp elements
            For i = 0 To _CpHeaders.Count - 1
                If LCase(clean_element(_CpHeaders.Keys(i))) = LCase(_CpDocType) Then
                    _CpIndex = i
                    Exit For
                End If
            Next

            For i = 0 To _CpList.Count - 1
                Dim x As Integer = InStr(_CpList(i), "|||")
                Dim MyNumber As String
                MyNumber = Mid(_CpList(i), x, Len(_CpList(i)))

                If MyNumber = "|||" & _CpIndex Then
                    If Left(_CpList(i), 1) = "@" Then
                        xmlFileName = Mid(_CpList(i).substring(1), 1, x - 2)
                        xmlFileName = Mid(xmlFileName, InStr(xmlFileName, "("), Len(xmlFileName))
                        xmlFileName = Replace(xmlFileName, "(", "")
                        xmlFileName = Replace(xmlFileName, ")", "")
                        If InStr(xmlFileName, "-") > 0 Then
                            xmlFileName = Mid(xmlFileName, 1, InStr(xmlFileName, "-") - 1)
                        End If
                        xmlFileName = xmlFileName & ".xml"
                        Exit For
                    End If
                End If
            Next

            For i As Integer = 0 To _CpList.Count - 1
                Dim iStart As Integer = InStr(_CpList(i), "|||")
                Dim MyNumber As String
                MyNumber = Mid(_CpList(i), iStart, Len(_CpList(i)))
                If MyNumber = "|||" & _CpIndex Then
                    If Left(_CpList(i), 1) <> "@" Then
                        Dim lbl As New Label
                        lbl = getLabel(iStart, _CpList(i))
                        'Dim etxtBx As New ExtendedTextbox
                        Dim etxtBx As New Object
                        etxtBx = getTextbox(iStart, _CpList(i))
                        AddRowsToTablelayoutPanel(index, 0, lbl, etxtBx)
                        index += 1
                    End If
                End If
            Next

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        RaiseEvent Lst_Textbox_Controls(lsttxtbxcontrol)
        RaiseEvent Lst_Label_Controls(lstlblControls)

        Return Tbl_Panel
    End Function

    Dim lstlblControls As New ArrayList

    Private Sub AddRowsToTablelayoutPanel(ByVal iStart As Integer, ByVal iEnd As Integer, ByVal lbl As Label, ByVal txtbx As Object)
        Try
            Dim _tooltip As New ToolTip

            Tbl_Panel.AutoScroll = True

            Tbl_Panel.RowStyles.Add(New RowStyle(SizeType.AutoSize))

            Dim B As New Button
            Dim B2 As New Button

            With lbl
                .Name = "lbl" & iStart
                .Font = New Font("BentonSans Book", 9)
            End With
            RaiseEvent getLblControlName(lbl.Text)
            Tbl_Panel.Controls.Add(lbl, 0, iStart)

            With txtbx
                .Name = "txtbx_" & iStart
                .Font = New Font("BentonSans Book", 9)
            End With
            RaiseEvent getTxtbxControlName(txtbx.Name)
            Tbl_Panel.Controls.Add(txtbx, 1, iStart)

            If _objActType = Action.Retrieve Then
                _tooltip.SetToolTip(B, "Copy to Clipboard")
                With B
                    .Name = "Btn_Copy_" & iStart
                    .Visible = True
                    .Text = ".."
                    .Size = New Point(20, 20)
                    .BackgroundImage = My.Resources.ClipboardCopy
                    If txtbx.bbold Then
                        .BackgroundImage = My.Resources.ClipboardCopydisabled
                        .Enabled = False
                    End If

                    .Dock = DockStyle.None
                    .FlatStyle = FlatStyle.Flat
                    .Cursor = Cursors.Hand
                    .FlatAppearance.BorderSize = 0
                    .FlatAppearance.MouseDownBackColor = Color.Transparent
                    .FlatAppearance.MouseOverBackColor = Color.Transparent
                    .BackgroundImageLayout = ImageLayout.Center
                    .TabIndex = 0
                End With

                AddHandler B.Click, AddressOf BtnCopy
                AddHandler B.MouseDown, AddressOf b2MouseDown
                AddHandler B.MouseUp, AddressOf bMouseup
                AddHandler B.KeyDown, AddressOf bKeyDown

                Tbl_Panel.Controls.Add(B, 2, iStart)
            End If

            If _objActType = Action.Create Then
                _tooltip.SetToolTip(B2, "Paste from Clipboard")
                With B2
                    .Name = "Btn_Paste_" & iStart
                    .Visible = True
                    .Text = ".."
                    .Size = New Point(20, 20)
                    .BackgroundImage = My.Resources.ClipboardPaste
                    .Dock = DockStyle.None
                    .FlatStyle = FlatStyle.Flat
                    .Cursor = Cursors.Hand
                    .FlatAppearance.BorderSize = 0
                    .FlatAppearance.MouseDownBackColor = Color.Transparent
                    .FlatAppearance.MouseOverBackColor = Color.Transparent
                    .BackgroundImageLayout = ImageLayout.Center
                    .TabIndex = 0
                End With
                AddHandler B2.Click, AddressOf BtnPaste
                AddHandler B2.MouseDown, AddressOf b2MouseDown
                AddHandler B2.MouseUp, AddressOf b2Mouseup

                Tbl_Panel.Controls.Add(B2, 2, iStart)
            End If

        Catch ex As Exception
            Throw New Exception("Error adding row in table layout panel" & vbNewLine & ex.Message)
        End Try

    End Sub

#Region "Mouse up down events"
    Private Sub b2MouseDown(sender As Object, e As EventArgs)
        sender.backgroundimage = Nothing
    End Sub

    Private Sub b2Mouseup(sender As Object, e As EventArgs)
        sender.backgroundimage = My.Resources.ClipboardPaste
    End Sub

    Private Sub bMouseup(sender As Object, e As EventArgs)
        sender.backgroundimage = My.Resources.ClipboardCopy
    End Sub

    Private Sub bKeydown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Space Then
            Dim txt As String = sender.name
            txt = Replace(txt, "Btn_Copy_", "txtbx_")
            Dim lbl As String = sender.name
            lbl = Replace(lbl, "Btn_Copy_", "lbl")
            RaiseEvent SpaceNextItem(txt, lbl)
        End If
    End Sub

#End Region

    Private Function getLabel(ByVal index As Integer, ByVal lblName As String) As Label
        Dim lbl As New Label
        Try
            If Left(lblName, 1) = "%" Then
                Dim myfont As New Font(lbl.Font.FontFamily, 9, FontStyle.Bold)
                lbl.Font = myfont
            Else
                Dim myfont As New Font(lbl.Font.FontFamily, 9, FontStyle.Regular)
                lbl.Font = myfont
            End If
            lbl.AutoSize = True
            lblName = Mid(lblName, 1, index - 1)
            lblName = Replace(lblName, "%", "")
            lbl.Text = Replace(lblName, "%", "")
            lbl.Dock = DockStyle.Fill
            lbl.TextAlign = ContentAlignment.MiddleRight

        Catch ex As Exception
            Throw New Exception("Error creating label" & vbNewLine & ex.Message)
        End Try

        Return lbl

    End Function

    Event getTxtbxControlName(ByVal tName As String)
    Event getLblControlName(ByVal lName As String)

    'Private Function getTextbox(ByVal index As Integer, ByVal txtName As String) As ExtendedTextbox

    '    Dim txtbx As New ExtendedTextbox

    '    Try
    '        If Left(txtName, 1) = "%" Then
    '            txtbx.bBold = True
    '        End If
    '        txtbx.Width = 762
    '        txtbx.Multiline = True
    '    Catch ex As Exception
    '        Throw New Exception("Error creating Textbox" & vbNewLine & ex.Message)
    '    End Try

    '    Return txtbx

    'End Function

    Private Function getTextbox(ByVal index As Integer, ByVal txtName As String) As Object
        Dim txtbx As Object
        If _objActType = Action.Create Then
            txtbx = New ExtendedTextbox1
        Else
            txtbx = New ExtendedTextBox
        End If

        Try
            If Left(txtName, 1) = "%" Then
                txtbx.bBold = True
            End If
            txtbx.Width = 762
            txtbx.Multiline = True
        Catch ex As Exception
            Throw New Exception("Error creating Textbox" & vbNewLine & ex.Message)
        End Try

        Return txtbx

    End Function

    Private Sub BtnCopy(sender As System.Object, e As System.EventArgs)
        Dim txt As String = sender.name
        txt = Replace(txt, "Btn_Copy_", "txtbx_")
        RaiseEvent CopyControl(txt)
    End Sub

    Private Sub BtnPaste(sender As System.Object, e As System.EventArgs)
        Dim txt As String = sender.name
        txt = Replace(txt, "Btn_Paste_", "txtbx_")
        RaiseEvent PasteControl(txt)
    End Sub

    Dim lsttxtbxcontrol As New ArrayList

    Private Sub getTxtbxControlList(ByVal tName As String)
        lsttxtbxcontrol.Add(tName)
    End Sub

    Private Sub getLblControlList(ByVal lName As String)
        lstlblControls.Add(lName)
    End Sub

End Class

Public Class ExtendedTextbox1
    Inherits TextBox
    Public Property bBold As Boolean
End Class

Public Class ExtendedTextBox
    Inherits TextBox
    Public Event OnEnterKeyPress()
    Public Property bBold As Boolean
    Protected Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs) 'Disable other keypad keys
        MyBase.OnKeyPress(e)

        If Asc(e.KeyChar) = 13 Then
            RaiseEvent OnEnterKeyPress()
        ElseIf Asc(e.KeyChar) = 3 Then
            If SelectedText <> "" Then
                Clipboard.SetText(Me.SelectedText)
            End If
        End If
        e.Handled = True

    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message) 'Disables context defualt menu
        Const WM_CONTEXTMENU As Integer = &H7B

        If m.Msg <> WM_CONTEXTMENU Then
            MyBase.WndProc(m)
        End If
    End Sub

    Protected Sub ExtendedLabel_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown 'Disable Delete key
        If e.KeyCode = Keys.Back Or e.KeyCode = Keys.Delete Then
            e.SuppressKeyPress = True
        End If
    End Sub
End Class



