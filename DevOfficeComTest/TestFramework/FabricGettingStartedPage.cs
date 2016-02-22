using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework
{
    public class FabricGettingStartedPage : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = "div#body-content div div div div.docs-PagesBannerLogo")]
        private IWebElement fabricPageTitle;
        public string FabricPageTitle
        {
            get { return fabricPageTitle.Text; }
        }
    }
}