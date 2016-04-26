using OpenQA.Selenium;
using System;

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

        /// <summary>
        /// Verify if the mobile menu-content is found on the page
        /// </summary>
        /// <returns>Trye if yes, else no.</returns>
        public static bool IsToggleMenuContentDisplayed()
        {
            Browser.Wait(TimeSpan.FromSeconds(2));
            return Browser.FindElement(By.CssSelector("div#menu-content")).Displayed;
        }

        /// <summary>
        /// Verify if the toggle menu icon is found on the page 
        /// </summary>
        /// <returns>Trye if yes, else no.</returns>
        public static bool IsToggleMenuIconDisplayed()
        {
            return Browser.FindElement(By.CssSelector("span#toggleLeftPanel")).Displayed;
        }

        /// <summary>
        /// Execute the menu display toggle
        /// </summary>
        public static void ToggleMobileMenu()
        {
            var element = Browser.FindElement(By.CssSelector("span#toggleLeftPanel"));
            Browser.Click(element);
            Browser.Wait(TimeSpan.FromSeconds(2));
        }

        /// <summary>
        /// Check whether a doc header exists
        /// </summary>
        /// <param name="docHeader">The expected doc header</param>
        /// <returns>True if yes, else no.</returns>
        public static bool HasDocHeader(string docHeader)
        {
            var element = Browser.FindElement(By.XPath("//table[@id='OfficeDocHeader']/tbody/tr/td/a[text()='" + docHeader + "']"));
            return element != null;
        }

        /// <summary>
        /// Check whether can edit Office add-in docs in github
        /// </summary>
        /// <returns>True if yes, else no.</returns>
        public static bool CanEditInGitHub()
        {
            var element = Browser.FindElement(By.XPath("//div[@id='GitHubInfo']/a[text()='Edit in GitHub']"));
            Browser.Click(element);
            Browser.SwitchToNewWindow();
            bool isPageCorrect = Browser.webDriver.Url.Contains("github.com/OfficeDev") && Browser.webDriver.Url.Contains("docs/overview/office-add-ins.md");
            Browser.SwitchBack();
            return isPageCorrect;
        }
    }
}