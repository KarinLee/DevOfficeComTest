﻿using System;
using OpenQA.Selenium;

namespace TestFramework.OfficeAddInPage
{
    public class OfficeAddInPage
    {
        ///// <summary>
        ///// The nav bar of the page
        ///// </summary>
        //public static NavBar OfficeAddInNavBar;
    
        public CardChooseProduct CardChooseProduct
        {
            get
            {
                return new CardChooseProduct();
            }
        }
        public CardExcel CardExcel
        {
            get
            {
                return new CardExcel();
            }
        }
        public CardOutlook CardOutlook
        {
            get
            {
                return new CardOutlook();
            }
        }
        public CardPowerPoint CardPowerPoint
        {
            get
            {
                return new CardPowerPoint();
            }
        }
        public CardWord CardWord
        {
            get
            {
                return new CardWord();
            }
        }

        public bool IsAtAddinPage()
        {
            return Browser.Title.Contains("Getting Started with Office Add-ins");
        }

        public bool OnlyDefaultCardsDisplayed()
        {
            var elements = Browser.Driver.FindElements(By.ClassName("card"));
            if (elements.Count > 0)
            {
                foreach (IWebElement item in elements)
                {
                    string itemId = item.GetAttribute("id");
                    if ((itemId == "intro" || itemId == "selectapp") && !item.Displayed)
                    {
                        return false;
                    }

                    if (itemId != "intro" && itemId != "selectapp" && item.Displayed)
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

        public bool CanLoadImages()
        {
            var elements = Browser.Driver.FindElements(By.CssSelector("img.img-responsive.imgGS"));
            foreach (IWebElement item in elements)
            {
                string Url = item.GetAttribute("src");
                if (!Utility.FileExist(Url))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsCardDisplayed(string CardId)
        {
            if (CardId.Contains("AllSet"))
            {
                var element = Browser.Driver.FindElement(By.Id("AllSetDeepBlue"));
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

        /// <summary>
        /// Choose development tool to build the add-in.
        /// </summary>
        /// <param name="selectVS">True if to select Visual Studio, else to choose Other tools</param>
        public void SelectBuildTool(bool selectVS = false)
        {
            if (selectVS)
            {
                var element = Browser.FindElement(By.CssSelector("button#button-visualstudio"));
                Browser.Click(element);
            }
            else
            {
                var element = Browser.FindElement(By.CssSelector("button#button-othertools"));
                Browser.Click(element);
            }
        }

        /// <summary>
        /// Check whether steps of building using other tools exist on the page.
        /// </summary>
        /// <returns>True if yes, elso no.</returns>
        public bool BuildOtherToolsStepsExist()
        {
            var element = Browser.FindElement(By.CssSelector("div#build-othertools"));
            return element.Displayed;
        }
    }
}
