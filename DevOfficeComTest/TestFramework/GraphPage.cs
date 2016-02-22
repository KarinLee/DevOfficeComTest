using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework
{
    /// <summary>
    /// A page on MS Graph site 
    /// </summary>
    public class GraphPage : BasePage
    {
        private OpenQA.Selenium.Remote.RemoteWebElement graphTitle;
        public string GraphTitle
        {
            get { return graphTitle.WrappedDriver.Title; }
        }

        /// <summary>
        /// The constructor method
        /// </summary>
        public GraphPage()
        {
            Browser.Wait(By.CssSelector("head>title"));
            graphTitle = (OpenQA.Selenium.Remote.RemoteWebElement)Browser.Driver.FindElement(By.CssSelector("head>title"));
        }
    }
}