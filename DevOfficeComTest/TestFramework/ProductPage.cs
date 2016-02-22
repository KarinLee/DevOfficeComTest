using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework
{
    public class ProductPage : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = "div.col-xs-10.col-sm-8.col-md-6.col-lg-6.col-centered.product-header h1")]
        private IWebElement productName;
        public string ProductName
        {
            get { return productName.Text; }
        }
    }
}