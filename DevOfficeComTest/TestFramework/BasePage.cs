using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework
{
    public class BasePage
    {
        public BasePage()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }
    }
}
