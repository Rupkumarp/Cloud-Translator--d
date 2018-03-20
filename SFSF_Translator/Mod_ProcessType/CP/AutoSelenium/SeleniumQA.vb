Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.IE
Imports OpenQA.Selenium.Firefox


Namespace SeleniumQA
    ''' <summary>
    ''' Automate Webbrowser control using Selenium WebDriver for Successfactor webapplication
    ''' </summary>
    ''' <remarks></remarks>
    Public Class WebAutomation

        Public Enum BrowserType
            IE
            Chrome
            FireFox
        End Enum

        Private Driver As IWebDriver

        Public Sub New(Optional ByVal BT As BrowserType = BrowserType.IE)
            Select Case BT
                Case BrowserType.Chrome
                    Driver = New ChromeDriver
                Case BrowserType.FireFox
                    Driver = New FirefoxDriver
                Case BrowserType.IE
                    Driver = New InternetExplorerDriver
            End Select
        End Sub

        Public Sub Navigate(ByVal Url As String)
            Try
                Driver.Navigate.GoToUrl(Url)
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10))
            Catch ex As Exception
                Throw New Exception("Could not navigate to: " & Url & Environment.NewLine & ex.Message)
            End Try
        End Sub

        Public Sub ClickByID(ID As String, ErrMsg As String)
            Try
                Driver.FindElement(By.Id(ID)).Click()
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10))
            Catch e As Exception
                Throw New Exception(ErrMsg + Environment.NewLine + e.Message)
            End Try
        End Sub

        Public Sub ClickByXpathID(xPathID As String, ErrMsg As String)
            Try
                Driver.FindElement(By.XPath(xPathID)).Click()
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10))
            Catch e As Exception
                Throw New Exception(ErrMsg + Environment.NewLine + e.Message)
            End Try
        End Sub

        Public Sub ClickByCSSelector(CssID As String, ErrMsg As String)
            Try
                Driver.FindElement(By.CssSelector(CssID)).Click()
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10))
            Catch e As Exception
                Throw New Exception(ErrMsg + Environment.NewLine + e.Message)
            End Try
        End Sub

        Public Sub ClickByLinkText(LinkText As String, ErrMsg As String)
            Try
                Driver.FindElement(By.LinkText(LinkText)).Click()
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10))
            Catch e As Exception
                Throw New Exception(ErrMsg + Environment.NewLine + e.Message)
            End Try
        End Sub

        Public Sub SetTextByID(ID As String, MyKey As String, ErrMsg As String)
            Try
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10))
                Driver.FindElement(By.Id(ID)).SendKeys(MyKey)
            Catch e As Exception
                Throw New Exception(ErrMsg + Environment.NewLine + e.Message)
            End Try
        End Sub

        Public Sub SetTextByXpathID(xPathId As String, MyKey As String, ErrMsg As String)
            Try
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10))
                Driver.FindElement(By.XPath(xPathId)).SendKeys(MyKey)
            Catch e As Exception
                Throw New Exception(ErrMsg + Environment.NewLine + e.Message)
            End Try
        End Sub

    End Class

End Namespace


