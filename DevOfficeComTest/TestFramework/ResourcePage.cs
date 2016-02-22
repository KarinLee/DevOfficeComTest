using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework
{
    public class ResourcePage : BasePage
    {
        private IWebElement resourceName;
        private string resourceTitle;
        public string ResourceName
        {
            get { return resourceTitle; }
        }

        public bool CanLoadImage(ResourcePageImages image)
        {
            switch (image)
            {
                case (ResourcePageImages.Banner):
                    IWebElement element = Browser.Driver.FindElement(By.Id("banner-image"));
                    string Url = element.GetAttribute("style");
                    Url = Browser.BaseAddress + Url.Substring(Url.IndexOf('/'), Url.LastIndexOf('"') - Url.IndexOf('/'));
                    return Utility.ImageExist(Url);
                case (ResourcePageImages.Responsive):
                    var elements = Browser.Driver.FindElements(By.CssSelector("img.img-responsive"));
                    if (elements.Count != 0)
                    {
                        foreach (IWebElement item in elements)
                        {
                            Url = item.GetAttribute("src");
                            if (!Utility.ImageExist(Url))
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                case (ResourcePageImages.Background):
                    elements = Browser.Driver.FindElements(By.CssSelector("div.background-img"));
                    if (elements.Count != 0)
                    {
                        foreach (IWebElement item in elements)
                        {
                            Url = item.FindElement(By.TagName("img")).GetAttribute("src");
                            if (!Utility.ImageExist(Url))
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                default:
                    return false;
            }
        }

        public ResourcePage()
        {
            Browser.SetWaitTime(TimeSpan.FromSeconds(5));
            try
            {
                var bannerImage = Browser.Driver.FindElement(By.Id("banner-image"));
                if (bannerImage.FindElement(By.CssSelector("div")).GetAttribute("class") == "no-description-banner-contents")
                {
                    resourceName = bannerImage.FindElement(By.CssSelector("div>div>h2"));
                }
                else
                {
                    resourceName = bannerImage.FindElement(By.CssSelector("div>div>div>h2"));
                }
            }
            catch (NoSuchElementException)
            {
                var container = Browser.Driver.FindElement(By.CssSelector("div.zone.zone-content"));
                foreach (IWebElement article in container.FindElements(By.TagName("article")))
                {
                    if (article.GetAttribute("class") == "widget-HighlightsWidget widget-content widget-highlights-widget widget")
                    {
                        resourceName = article.FindElement(By.CssSelector("div>div>div>h1"));
                        break;
                    }

                    if (article.GetAttribute("class") == "widget-AppRegistrationWidget widget-content widget-app-registration-widget widget")
                    {
                        resourceName = article.FindElement(By.CssSelector("div#register-app.card>h1"));
                        break;
                    }
                }
            }

            resourceTitle = resourceName.Text;
            Browser.SetWaitTime(TimeSpan.FromSeconds(Utility.DefaultWaitTime));
        }
    }

    public enum ResourcePageImages
    {
        Banner,
        Responsive,
        Background
    }
}
