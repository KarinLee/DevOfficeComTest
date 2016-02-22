using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.OfficeAddInPage
{
    class CommonMoreResources : BasePage
    {
        private static string resourceKeyword;

        public void OutlookDevCenter()
        {
            resourceKeyword = "Outlook Dev Center";
            Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@onclick=\"window.open('https://dev.outlook.com/MailAppsGettingStarted/GetStarted');\"]")));
            
        }
        public void OfficeAddInTypes()
        {
            resourceKeyword = "Office Add-ins platform overview";
            Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@onclick=\"window.open('https://msdn.microsoft.com/en-us/library/office/jj220082.aspx#StartBuildingApps_TypesofApps')\"]")));
        }
        public void DownLoadStarterSample()
        {
            resourceKeyword = "OfficeDev/Add-in-TaskPane-Sample";
            Browser.Click(Browser.Driver.FindElement(By.Id("more-github")));
        }
        public void MoreCodeSamples(Product product)
        {
            resourceKeyword = "Code Samples";
            switch (product)
            {
                case Product.Excel:
                    Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@onclick=\"window.open('http://dev.office.com/code-samples#?filters=excel,office%20add-ins')\"]")));
                    break;
                case Product.Outlook:
                    Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@onclick=\"window.open('http://dev.office.com/code-samples#?filters=office%20add-ins,outlook')\"]")));
                    break;
                case Product.PowerPoint:
                    Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@onclick=\"window.open('http://dev.office.com/code-samples#?filters=office%20add-ins,powerpoint')\"]")));
                    break;
                case Product.Word:
                    Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@onclick=\"window.open('http://dev.office.com/code-samples#?filters=office%20add-ins,word')\"]")));
                    break;
                default:
                    break;
            }

        }
        public void ReadTheDocs()
        {
            resourceKeyword = "Office Add-ins platform overview";
            Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@onclick=\"window.open('https://msdn.microsoft.com/library/office/jj220082.aspx');\"]")));
        }
        public void DesignYourAddIn()
        {
            resourceKeyword = "Design guidelines for Office Add-ins";
            Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@onclick=\"window.open('https://msdn.microsoft.com/EN-US/library/office/mt484317.aspx');\"]")));
        }
        public void PublishYourAddIn()
        {
            resourceKeyword = "Publish your Office Add-in";
            Browser.Click(Browser.Driver.FindElement(By.XPath("//div[@onclick=\"window.open('https://msdn.microsoft.com/EN-US/library/office/fp123515.aspx');\"]")));
        }
        public bool IsShowingCorrectResourcePage(Product product)
        {
            bool canSwitchWindow = Browser.SwitchToNewWindow();
            bool isCorrectResourcePage = Browser.Title.Contains(resourceKeyword);
            if (resourceKeyword.Equals("Code Samples"))
            {
                isCorrectResourcePage = isCorrectResourcePage && Browser.Url.Contains("filters=" + product.ToString().ToLower() + ",office%20add-ins");
            }

            if (canSwitchWindow)
            {
                Browser.SwitchBack();
            }

            return canSwitchWindow;

        }
    }
}
