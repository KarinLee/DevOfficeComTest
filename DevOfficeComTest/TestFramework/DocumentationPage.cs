using OpenQA.Selenium;

namespace TestFramework
{
    public class DocumentationPage : BasePage
    {
        private OpenQA.Selenium.Remote.RemoteWebElement documentationTitle;
        public string DocumentationTitle
        {
            get { return documentationTitle.WrappedDriver.Title; }
        }

        public DocumentationPage()
        {
            documentationTitle = (OpenQA.Selenium.Remote.RemoteWebElement)Browser.Driver.FindElement(By.CssSelector("head>title"));
        }
    }
}