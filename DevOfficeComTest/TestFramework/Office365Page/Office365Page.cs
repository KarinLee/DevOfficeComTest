
using System;
using OpenQA.Selenium;

namespace TestFramework.Office365Page
{
    public class Office365Page
    {
        public CardSetupPlatform CardSetupPlatform
        {
            get
            {
                return new CardSetupPlatform();
            }
        }

        public CardTryItOut CardTryItOut
        {
            get
            {
                return new CardTryItOut();
            }
        }

        public CardRegisterApp CardRegisterApp
        {
            get
            {
                return new CardRegisterApp(); 
            }
        }

        public CardDownloadCode CardDownloadCode
        {
            get
            {
                return new CardDownloadCode();
            }
        }

        public CardMoreResources CardMoreResources
        {
            get
            {
                return new CardMoreResources(); 
            }
        }

        public bool IsAtOffice365Page()
        {
            return Browser.Title.Contains("Getting started with Office 365 REST APIs");
        }

        public bool OnlyDefaultCardsDisplayed()
        {
            var elements = Browser.Driver.FindElements(By.ClassName("card"));
            if (elements.Count > 0)
            {
                foreach (IWebElement item in elements)
                {
                    string itemId = item.GetAttribute("id");
                    if ((itemId == "intro" || itemId == "try-it-out" || itemId == "setup") && !item.Displayed)
                    {
                        return false;
                    }

                    if (itemId != "intro" && itemId != "try-it-out" && itemId != "setup" && item.Displayed)
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsCardDisplayed(string CardId)
        {
            if (CardId.Contains("AllSet"))
            {
                var element = Browser.Driver.FindElement(By.Id("AllSet"));
                return element.Displayed;
            }
            else
            {
                var elements = Browser.Driver.FindElements(By.ClassName("card"));
                if (elements.Count > 0)
                {
                    foreach (IWebElement item in elements)
                    {
                        string itemId = item.GetAttribute("id");
                        if (itemId == CardId)
                        {
                            return item.Displayed;
                        }
                    }

                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool CanLoadImages(Office365Images image)
        {
            switch (image)
            {
                case (Office365Images.ServiceOrResource):
                    var elements = Browser.Driver.FindElements(By.CssSelector("img.img-responsive.imgGS"));
                    foreach (IWebElement item in elements)
                    {
                        string Url = item.GetAttribute("src");
                        if (!Utility.ImageExist(Url))
                        {
                            return false;
                        }
                    }

                    return true;
                case (Office365Images.Platform):
                    elements = Browser.Driver.FindElements(By.CssSelector("#pickPlatform > ul > li"));
                    foreach (IWebElement item in elements)
                    {
                        IWebElement subItem = item.FindElement(By.CssSelector("img"));
                        string Url = subItem.GetAttribute("src");
                        if (!Utility.ImageExist(Url))
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

    public enum Office365Images
    {
        ServiceOrResource,
        Platform
    }
}
