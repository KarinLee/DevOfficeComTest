using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework
{
    public class FabricPage : BasePage
    {
        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'Carousel-staticContent')]")]
        private IWebElement fabricPageTitle;
        public string FabricPageTitle
        {
            get { return fabricPageTitle.Text; }
        }
    }
}