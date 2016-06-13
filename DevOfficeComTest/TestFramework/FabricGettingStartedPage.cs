using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework
{
    public class FabricGettingStartedPage : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = "a#docs-PagesBannerLogo")]
        private IWebElement fabricPageTitle;
        public string FabricPageTitle
        {
            get {
                Browser.Wait(By.CssSelector("a#docs-PagesBannerLogo"));
                return fabricPageTitle.Text; }
        }
    }
}