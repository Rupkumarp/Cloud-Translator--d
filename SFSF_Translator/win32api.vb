Public Class win32api

    Private Declare Function FindWindow Lib "user32" Alias _
            "FindWindowA" (ByVal lpClassName As String, _
            ByVal lpWindowName As String) As Integer
    Private Declare Function GetWindowPlacement Lib _
            "user32" (ByVal hwnd As Integer, _
            ByRef lpwndpl As WINDOWPLACEMENT) As Integer
    Private Declare Function SetWindowPlacement Lib "user32" _
           (ByVal hwnd As Integer, ByRef lpwndpl As WINDOWPLACEMENT) As Integer

    Private Declare Function CloseWindow Lib "user32.dll" (ByVal hwnd As Int32) As Int32

    Private Declare Function DestroyWindow Lib "user32.dll" (ByVal hwnd As Int32) As Int32

    Private Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (ByVal hwnd As Int32, ByVal wMsg As Int32, ByVal wParam As Int32, ByVal lParam As Int32) As Int32

    Public Const WM_CLOSE = &H10

    Private Const SW_SHOWMINIMIZED As Short = 2
    Private Const SW_SHOWMAXIMIZED As Short = 3
    Private Const SW_SHOWNORMAL As Short = 1

    Private Structure POINTAPI
        Dim X As Integer
        Dim Y As Integer
    End Structure

    Private Structure RECT
        Dim Left_Renamed As Integer
        Dim Top_Renamed As Integer
        Dim Right_Renamed As Integer
        Dim Bottom_Renamed As Integer
    End Structure

    Private Structure WINDOWPLACEMENT
        Dim length As Integer
        Dim flags As Integer
        Dim showCmd As Integer
        Dim ptMinPosition As POINTAPI
        Dim ptMaxPosition As POINTAPI
        Dim rcNormalPosition As RECT
    End Structure


    Public Sub windowAction(ByVal classname As String, ByVal action As String)

        Dim app_hwnd As Integer
        Dim wp As WINDOWPLACEMENT
        app_hwnd = FindWindow(Nothing, classname)
        wp.length = Len(wp)
        GetWindowPlacement(app_hwnd, wp)

        Select Case action

            Case "Minimize"
                wp.showCmd = SW_SHOWMINIMIZED
            Case "Maximize"
                wp.showCmd = SW_SHOWMAXIMIZED
            Case "Restore"
                wp.showCmd = SW_SHOWNORMAL
            Case "close"
                wp.showCmd = SW_SHOWNORMAL
                SetWindowPlacement(app_hwnd, wp)
                SendMessage(app_hwnd, WM_CLOSE, 0, 0)
        End Select


    End Sub
End Class