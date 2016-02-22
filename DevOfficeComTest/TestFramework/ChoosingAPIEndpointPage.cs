using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework
{
    public class ChoosingAPIEndpointPage : BasePage
    {
        [FindsBy(How = How.Id, Using = "chooseYourAPIEndpoint")]
        private IWebElement pageTitle;
        public string EndpointPageTitle
        {
            get { return pageTitle.Text; }
        }
    }
}