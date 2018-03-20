Public Class ReportData
    Implements ISubject

    Private m_observer As List(Of IObserver)
    Private m_NewFile As Integer
    Private m_ForTranslation As Integer
    Private m_BackFromtranslation As Integer
    Private m_IntegratedFile As Integer

    Public Sub New()
        m_observer = New List(Of IObserver)
    End Sub

    Public Sub NotifyObserver() Implements ISubject.NotifyObserver
        For Each observer As IObserver In m_observer
            observer.Update(m_NewFile, m_ForTranslation, m_BackFromtranslation, m_IntegratedFile)
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

    Private Sub SetData(ByVal NewFile As Integer, ByVal ForTranslation As Integer, ByVal BackFromtranslation As Integer, ByVal Integratedfile As Integer)
        Me.m_NewFile = NewFile
        Me.m_ForTranslation = ForTranslation
        Me.m_BackFromtranslation = BackFromtranslation
        Me.m_IntegratedFile = Integratedfile
    End Sub
End Class
