using OpenQA.Selenium;
using System;

namespace TestFramework
{
    public class OpportunityPage :BasePage
    {
        public OpportunityPage()
        {
        }

        public bool CanLoadImage()
        {
            IWebElement element = Browser.Driver.FindElement(By.CssSelector("#carousel>div>div"));
            string Url = element.GetAttribute("style");
            Url = Browser.BaseAddress + Url.Substring(Url.IndexOf('/'), Url.LastIndexOf('"') - Url.IndexOf('/'));
            return Utility.ImageExist(Url);
        }
    }
}