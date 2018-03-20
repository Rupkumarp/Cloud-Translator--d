Public Interface ISubject
    Sub RegisterObserver(ByVal O As IObserver)
    Sub RemoverObserver(ByVal O As IObserver)
    Sub NotifyObserver()
End Interface
