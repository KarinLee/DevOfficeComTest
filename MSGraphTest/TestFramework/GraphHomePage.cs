using System;
using System.Collections.Generic;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework
{
    public class GraphHomePage : GraphBasePage
    {
        public GraphHomePage()
        { }
        private static string PageTitle = "Microsoft Graph - Home";

        [FindsBy(How = How.XPath, Using = "//ul[@class='nav navbar-nav']/li/a[contains(@href,'graphexplorer2.azurewebsites.net')]")]
        private IWebElement exploreLink;

        public bool CanLoadImages(GraphHomePageImages image)
        {
            switch (image)
            {
                case (GraphHomePageImages.MainBanner):
                    var element = GraphBrowser.Driver.FindElement(By.CssSelector("#layout-featured > div > article > div > div > div > div"));
                    string Url = element.GetAttribute("style");
                    Url = GraphBrowser.BaseAddress + Url.Substring(Url.IndexOf('/'), Url.LastIndexOf('"') - Url.IndexOf('/'));
                    return GraphUtility.ImageExist(Url);
                case (GraphHomePageImages.WebIllustration):
                    element = GraphBrowser.Driver.FindElement(By.CssSelector("#layout-featured > div > article > div > div > div > div:nth-child(2) > div:nth-child(4)"));
                    Url = element.GetAttribute("style");
                    Url = GraphBrowser.BaseAddress + Url.Substring(Url.IndexOf('/'), Url.LastIndexOf('"') - Url.IndexOf('/'));
                    return GraphUtility.ImageExist(Url);
                case (GraphHomePageImages.Others):
                    var elements = GraphBrowser.Driver.FindElements(By.CssSelector("img"));
                    foreach (IWebElement item in elements)
                    {
                        Url = item.GetAttribute("src");
                        if (!GraphUtility.ImageExist(Url))
                        {
                            return false;
                        }
                    }

                    return true;
                default:
                    return false;
            }
        }
    }

    public enum GraphHomePageImages
    {
        MainBanner,
        WebIllustration,
        Others
    }
}