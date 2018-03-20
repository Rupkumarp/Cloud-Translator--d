Public Class ReportData

    Implements ISubject

    Private m_observer As List(Of IObserver)

    Private m_CS As CloudStats

    Public Sub New()
        m_observer = New List(Of IObserver)
    End Sub

    Public Sub NotifyObserver() Implements ISubject.NotifyObserver
        For Each observer As IObserver In m_observer
            observer.UpdateReport(m_CS)
        Next
    End Sub

    Public Sub RegisterObserver(O As IObserver) Implements ISubject.RegisterObserver
        m_observer.Add(O)
    End Sub

    Public Sub RemoverObserver(O As IObserver) Implements ISubject.RemoverObserver
        Dim i As Integer = m_observer.IndexOf(O)
        If i > 0 Then
            m_observer.RemoveAt(i)
        End If
    End Sub

    Private Sub DataChanged()
        NotifyObserver()
    End Sub

    Private Sub SetData(ByVal NewFile As Integer, ByVal ForTranslation As Integer, ByVal BackFromtranslation As Integer, ByVal Integratedfile As Integer, ByVal CS As CloudStats)
        Me.m_CS = CS
    End Sub

End Class
