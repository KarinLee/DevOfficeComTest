using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.Office365Page
{
    public class CardMoreResources : BasePage
    {
        private static string resourceKeyword;

        public void OutlookDevCenter()
        {
            resourceKeyword = "Outlook Dev Center";
            Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@onclick=\"window.open('https://dev.outlook.com/RestGettingStarted/Overview');\"]")));
        }
        public void Training()
        {
            resourceKeyword = "Training";
            Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@onclick=\"window.open('http://dev.office.com/training');\"]")));
        }
        public void APIReferences()
        {
            resourceKeyword = "API catalog";
            Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@onclick=\"window.open('https://msdn.microsoft.com/en-us/office/office365/howto/rest-api-overview');\"]")));
        }
        public void CodeSamples()
        {
            resourceKeyword = "Code Samples";
            Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@onclick=\"window.open('http://dev.office.com/code-samples#?filters=office%20365%20app');\"]")));
        }
        public void AzureAppAndPermissions()
        {
            resourceKeyword = "Azure";
            Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@onclick=\"window.open('https://msdn.microsoft.com/en-us/office/office365/howto/add-common-consent-manually');\"]")));
        }
        public void AddToO365AppLauncher()
        {
            resourceKeyword = "app launcher";
            Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@id=\"next-step\"]/div/div[6]/table/tbody/tr/td[1]/p/a")));
        }
        public void SubmitToOfficeStore()
        {
            resourceKeyword = "Office Store";
            Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@id=\"next-step\"]/div/div[6]/table/tbody/tr/td[2]/p/a")));
        }
        public bool IsShowingCorrectResourcePage()
        {
            bool canSwitchWindow = Browser.SwitchToNewWindow();
            bool isCorrectResourcePage = Browser.Title.Contains(resourceKeyword);
            if (canSwitchWindow)
            {
                Browser.SwitchBack();
            }

            return isCorrectResourcePage;

        }
    }
}
