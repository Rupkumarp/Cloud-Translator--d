Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows
Imports System.Windows.Forms

Public Class Form_Log

    'Public Property VerticalScrollCurrentValue As Integer
    'Public Property VerticalScrollMaxValue As Integer
    'Public Property RunVerticalScroll As Boolean

    'Private Sub RichTextBox1_VScroll(sender As Object, e As EventArgs) Handles RichTextBox1.VScroll
    '    If (ScrollBarInfo.IsAtBottom(Me.RichTextBox1) = True) Or (ScrollBarInfo.ReachedBottom(Me.RichTextBox1) = True) Then
    '        RunVerticalScroll = True
    '        Me.RichTextBox1.HideSelection = False
    '    Else
    '        RunVerticalScroll = False
    '        Me.RichTextBox1.HideSelection = True
    '    End If
    'End Sub

End Class

Public Class ScrollBarInfo

    <System.Runtime.InteropServices.DllImport("user32")> _
    Private Shared Function GetScrollInfo(hwnd As IntPtr, nBar As Integer, ByRef scrollInfo As SCROLLINFO) As Integer
    End Function

    Private Shared scrollInf As New SCROLLINFO()

    Private Structure SCROLLINFO
        Public cbSize As Integer
        Public fMask As Integer
        Public min As Integer
        Public max As Integer
        Public nPage As Integer
        Public nPos As Integer
        Public nTrackPos As Integer
    End Structure

    Private Shared Sub Get_ScrollInfo(control As Control)
        scrollInf = New SCROLLINFO()
        scrollInf.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(scrollInf)
        scrollInf.fMask = &H10 Or &H1 Or &H2
        GetScrollInfo(control.Handle, 1, scrollInf)
    End Sub

    Public Shared Function ReachedBottom(control As Control) As Boolean
        Get_ScrollInfo(control)
        Return scrollInf.max = scrollInf.nTrackPos + scrollInf.nPage
    End Function

    Public Shared Function ReachedTop(control As Control) As Boolean
        Get_ScrollInfo(control)
        Return scrollInf.nTrackPos < 0
    End Function

    Public Shared Function IsAtBottom(control As Control) As Boolean
        Get_ScrollInfo(control)
        Return scrollInf.max = (scrollInf.nTrackPos + scrollInf.nPage) - 1
    End Function

    Public Shared Function IsAtTop(control As Control) As Boolean
        Get_ScrollInfo(control)
        Return scrollInf.nTrackPos = 0
    End Function

    Public Shared Function Get_MaxValueForVerticalBar(control As Control) As Integer
        Get_ScrollInfo(control)
        Return scrollInf.max
    End Function

    Public Shared Function Get_CurrentPostionVerticalBar(control As Control) As Integer
        Get_ScrollInfo(control)
        Return scrollInf.nTrackPos + scrollInf.nPage
    End Function

End Class



