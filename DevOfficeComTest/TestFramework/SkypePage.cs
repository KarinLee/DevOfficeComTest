using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;

namespace TestFramework
{
    public class SkypePage : BasePage
    {
        public IReadOnlyList<IWebElement> LeftNavItems
        {
            get
            {
                return Browser.webDriver.FindElements(By.CssSelector("ul.LeftNav-links > li> a"));
            }
        }

        /// <summary>
        /// Select an item on Skype page's own top nav bar.
        /// </summary>
        public void SelectTopNavItem(SkypeNavItem item)
        {
            int waitTime = Int32.Parse(Utility.GetConfigurationValue("WaitTime"));
            string itemToSelect = EnumExtension.GetDescription(item);
            Browser.Wait(By.XPath("//ul[@class='docs-NavItemsContainer']/li/a[text()='" + itemToSelect + "']"));
            var element = Browser.FindElement(By.XPath("//ul[@class='docs-NavItemsContainer']/li/a[text()='" + itemToSelect + "']"));
            Browser.Click(element);
            Browser.Wait(TimeSpan.FromSeconds(waitTime));
        }

        /// <summary>
        /// Check whether select a skype nav item can navigate to the correct page
        /// </summary>
        /// <param name="item">The item selected</param>
        /// <returns>True if yes, else no.</returns>
        public bool CanSwitchCorrectPage(SkypeNavItem item)
        {
            string itemSelected = EnumExtension.GetDescription(item);
            string end = string.Empty;
            switch (itemSelected)
            {
                case "Overview":
                    end = "Skype";
                    break;
                case "Explore":
                    end = "explore";
                    break;
                case "Getting Started":
                    end = "gettingStarted";
                    break;
                case "Skype APIs":
                    end = "skype-sdks";
                    break;
                case "Get Involved":
                    end = "getInvolved";
                    break;
                case "Marketplace":
                    end = "marketplace";
                    break;
            }
            return !end.Equals(string.Empty) && Browser.webDriver.Title.EndsWith(end);
        }

        /// <summary>
        /// Check whether a left nav item refers to a valid doc or doc part
        /// </summary>
        /// <param name="index">The index of the lfet nav item</param>
        /// <param name="itemText">The item text</param>
        /// <returns>True if yes, else no.</returns>
        public bool IsValidLeftNavItem(int index, out string itemText)
        {
            var element = Browser.FindElement(By.CssSelector("ul.LeftNav-links > li:nth-child(" + (int)(index + 1) + ") > a"));
            itemText = element.Text;
            string href = element.GetAttribute("href");
            return Utility.FileExist(href);
        }

        /// <summary>
        /// Verify if the toggle menu icon is found on the page 
        /// </summary>
        /// <returns>Trye if yes, else no.</returns>
        public static bool IsToggleMenuIconDisplayed()
        {
            return Browser.FindElement(By.CssSelector("div.docs-hamburgerButton.ms-Icon.ms-Icon--menu")).Displayed;
        }

        /// <summary>
        /// Verify if the mobile menu-content is found on the page
        /// </summary>
        /// <returns>Trye if yes, else no.</returns>
        public static bool IsMobileMenuContentDisplayed()
        {
            return Browser.FindElement(By.CssSelector("div.ms-Panel-contentInner")).Displayed;
        }

        /// <summary>
        /// Execute the mobile menu display toggle
        /// </summary>
        public static void ToggleMobileMenu()
        {
            var element = Browser.FindElement(By.CssSelector("div.docs-hamburgerButton"));
            var panelElement = Browser.FindElement(By.CssSelector("div.ms-Panel-contentInner"));
            if (element.Displayed && !panelElement.Displayed)
            {
                Browser.Click(element);
                Browser.Wait(TimeSpan.FromSeconds(2));
            }
            else
            {
                //Click at any position outside the menu to hide it
                Actions action = new Actions(Browser.webDriver);
                int offX = panelElement.Location.X + panelElement.Size.Width + 50;
                int offY = panelElement.Location.Y + panelElement.Size.Height / 2;
                action.MoveByOffset(offX, offY);
                action.Click().Build().Perform();
            }
        }
    }
}