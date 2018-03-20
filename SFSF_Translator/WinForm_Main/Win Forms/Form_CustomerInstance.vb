Imports System.Windows.Forms

Public Class Form_CustomerInstance

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If CmbCustomer.SelectedIndex = -1 Or CmbCustomer.Text.Trim = String.Empty Then
            MsgBox("Enter Customer Name!", MsgBoxStyle.Critical, "Customer Detial")
            Exit Sub
        End If

        If CmbInstance.SelectedIndex = -1 Or CmbInstance.Text.Trim = String.Empty Then
            MsgBox("Enter Instance Name!", MsgBoxStyle.Critical, "Instance Detial")
            Exit Sub
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click

        If CmbCustomer.SelectedIndex = -1 Or CmbCustomer.Text.Trim = String.Empty Then
            MsgBox("You cannot Cancel!" & vbNewLine & "Enter Customer Name!", MsgBoxStyle.Critical, "Customer Detial")
            Exit Sub
        End If

        If CmbInstance.SelectedIndex = -1 Or CmbInstance.Text.Trim = String.Empty Then
            MsgBox("You cannot Cancel!" & vbNewLine & "Enter Instance Name!", MsgBoxStyle.Critical, "Instance Detial")
            Exit Sub
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CmbCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbCustomer.SelectedIndexChanged
        If CmbCustomer.SelectedIndex = 0 Then
            Dim CustomerName As String = InputBox("Enter Customer name", "CustomerName")
            If CustomerName.Trim = "" Then
                CmbCustomer.SelectedIndex = -1
                MsgBox("No Customer name entered to Customer list!", MsgBoxStyle.Exclamation, "Customer name empty")
                Exit Sub
            End If
            Dim bFound As Boolean = False
            For i As Integer = 0 To CmbCustomer.Items.Count - 1
                If CustomerName.Trim.ToLower = CmbCustomer.Items(i).ToString.ToLower Then
                    MsgBox(CustomerName & " - Customer Name already available in the list!", MsgBoxStyle.Exclamation, "Duplicate Customer")
                    bFound = True
                    Exit For
                End If
            Next

            If Not bFound Then
                CmbCustomer.Items.Add(CustomerName)
                CmbCustomer.SelectedIndex = CmbCustomer.Items.Count - 1
            End If

        End If

        If CmbCustomer.SelectedIndex > 0 Then
            PopulateInstace(CmbCustomer.Text)
        End If
    End Sub

    Private Sub CmbInstance_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbInstance.SelectedIndexChanged

        If CmbInstance.SelectedIndex = 0 Then
            Dim Instance As String = InputBox("Enter Instance", "Instance")
            If Instance.Trim = "" Then
                CmbInstance.SelectedIndex = -1
                MsgBox("No Instance entered to Instance list!", MsgBoxStyle.Exclamation, "Empty Instance")
                Exit Sub
            End If
            Dim bFound As Boolean = False
            For i As Integer = 0 To CmbInstance.Items.Count - 1
                If Instance.Trim.ToLower = CmbInstance.Items(i).ToString.ToLower Then
                    MsgBox(Instance & " - Instance already available in the list!", MsgBoxStyle.Exclamation, "Duplicate Instance")
                    bFound = True
                    Exit For
                End If
            Next

            If Not bFound Then
                CmbInstance.Items.Add(Instance)
                CmbInstance.SelectedIndex = CmbInstance.Items.Count - 1
            End If
        End If
    End Sub

    Dim CI As New Dialog1.CustomerInstance
    Sub PopulateInstace(ByVal Customername As String)
        Try
            CmbInstance.Items.Clear()
            CmbInstance.Items.Add("[Add New Instance]")
            Dim Instancearray As New ArrayList

            If Not IsNothing(CI.CustomerInstance) Then
                For i As Integer = 0 To CI.CustomerInstance.Count - 1
                    If Not IsNothing(CI.CustomerInstance(i)) Then
                        If CI.CustomerInstance(i).ToString.ToLower = Customername.ToLower Then
                            If Not Instancearray.Contains(CI.InstanceName(i)) Then
                                Instancearray.Add(CI.InstanceName(i))
                            End If
                        End If
                    Else
                        CI.CustomerInstance.RemoveAt(i)
                        CI.InstanceName.RemoveAt(i)
                    End If
                Next

                For i As Integer = 0 To Instancearray.Count - 1
                    CmbInstance.Items.Add(Instancearray(i))
                Next
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Private Sub BW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BW.DoWork
        CI.CustomerName = New ArrayList
        CI.InstanceName = New ArrayList
        CI.CustomerInstance = New ArrayList

        If CheckURL("http://10.66.9.51:8013/") Then
            Dim MyCD As New Cloud_TR.Service1
            Dim ds As New DataSet
            ds = MyCD.GetCustomerInstanceList
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                CI.InstanceName.Add(ds.Tables(0).Rows(i).Item(0))
                CI.CustomerName.Add(ds.Tables(0).Rows(i).Item(1))
                CI.CustomerInstance.Add(ds.Tables(0).Rows(i).Item(1))
            Next
        End If

        Dim CustName As String = ""
        Dim IntanceName As String = ""

        If IntanceName Is Nothing Then
            IntanceName = ""
        End If

        'firs add custname if not got from service call

        Dim bCustFound As Boolean = False
        For i As Integer = 0 To CI.CustomerName.Count - 1
            If CI.CustomerName(i).ToString.ToLower.Trim = CustName.ToLower.Trim Then
                bCustFound = True
            End If
        Next

        If Not bCustFound Then
            If CustName = String.Empty Then
                CI.CustomerName.Add(CustName)
            End If
        End If

        For i As Integer = 0 To CI.CustomerName.Count - 1
            If CI.CustomerName(i).ToString.ToLower.Trim = CustName.ToLower.Trim Then
                Dim bFound As Boolean = False
                For j As Integer = 0 To CI.InstanceName.Count - 1
                    If CI.InstanceName(j).ToString.ToLower.Trim = IntanceName.ToLower.Trim Then
                        bFound = True
                    End If
                Next
                If bFound <> True Then
                    CI.InstanceName.Add(IntanceName)
                    CI.CustomerInstance.Add(CI.CustomerName(i))
                End If
            End If
        Next

    End Sub

    Private Sub BW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BW.RunWorkerCompleted
        setCustomerInstance()
        Me.Cursor = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub setCustomerInstance()
        Try
            CmbCustomer.Items.Clear()
            CmbCustomer.Items.Add("[Add New Customer]")

            CmbInstance.Items.Clear()
            CmbInstance.Items.Add("[Add New Instance]")

            If Not IsNothing(CI.CustomerName) Then
                Dim CustomerList As New ArrayList
                For i As Integer = 0 To CI.CustomerName.Count - 1
                    If Not CustomerList.Contains(CI.CustomerName(i)) Then
                        CustomerList.Add(CI.CustomerName(i))
                    End If
                Next

                For i As Integer = 0 To CustomerList.Count - 1
                    CmbCustomer.Items.Add(CustomerList(i))
                Next
            End If
        Catch ex As Exception
            'old project
        End Try


    End Sub

    Private Sub Form_CustomerInstance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not BW.IsBusy Then
            BW.RunWorkerAsync()
            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor
        End If
    End Sub
End Class
