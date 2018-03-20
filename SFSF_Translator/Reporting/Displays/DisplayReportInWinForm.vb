Public Class DisplayReportInWinForm
    Implements IDisplay, IObserver

    Dim m_CS As CloudStats
    Dim m_RD As ReportData

    Public Sub New(ByVal RD As ReportData)
        m_RD = RD
        RD.RegisterObserver(Me)
    End Sub

    Public Sub Display() Implements IDisplay.Display
        Dim f As New Form_Report
        f.CS = m_CS
        f.Show()
    End Sub

    Public Sub UpdateReport(cs As CloudStats) Implements IObserver.UpdateReport
        Me.m_CS = cs
        Display()
    End Sub
End Class
