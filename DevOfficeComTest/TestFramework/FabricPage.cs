using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework
{
    public class FabricPage : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = "div#body-content div div div")]
        private IWebElement fabricPageTitle;
        public string FabricPageTitle
        {
            get { return fabricPageTitle.Text; }
        }
    }
}