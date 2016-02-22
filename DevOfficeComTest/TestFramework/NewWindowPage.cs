using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework
{
    public class NewWindowPage : BasePage
    {
        private OpenQA.Selenium.Remote.RemoteWebElement title;

        public bool IsAt(string pageTitle)
        {
            return title.WrappedDriver.Title.Contains(pageTitle);
        }

        public NewWindowPage()
        {
            Browser.Wait(By.CssSelector("head>title"));
            title = (OpenQA.Selenium.Remote.RemoteWebElement)Browser.Driver.FindElement(By.CssSelector("head>title"));
        }
    }
}
