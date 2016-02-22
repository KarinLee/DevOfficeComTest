using OpenQA.Selenium;
using System;

namespace TestFramework
{
    public class OfficeGettingStartedPage : BasePage
    {
        public void Office365APIGetStarted()
        {
            var o365GetStarted = Browser.Driver.FindElement(By.CssSelector("#body-content>div:nth-child(2)>a"));
            Browser.Click(o365GetStarted);

            // When the card to choose platform is displayed, the click event can be considered as finished.
            Browser.Wait(By.Id("setup"));
        }

        public void OfficeAddInGetStarted()
        {
            var addinGetStarted = Browser.Driver.FindElement(By.CssSelector("#body-content>div:nth-child(3)>a"));
            Browser.Click(addinGetStarted);

            // When the card to choose product is displayed, the click event can be considered as finished.
            Browser.Wait(By.Id("selectapp"));
        }

        public bool CanLoadImage()
        {
            IWebElement element = Browser.Driver.FindElement(By.Id("banner-image"));
            string Url = element.GetAttribute("style");
            Url = Browser.BaseAddress + Url.Substring(Url.IndexOf('/'), Url.LastIndexOf('"') - Url.IndexOf('/'));
            return Utility.ImageExist(Url);
        }

        public OfficeGettingStartedPage()
        {
            if (!Browser.Url.EndsWith("/getting-started"))
            {
                Browser.Goto(Browser.BaseAddress + "/getting-started");
            }
        }
    }
}