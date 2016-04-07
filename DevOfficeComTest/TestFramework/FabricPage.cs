using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;

namespace TestFramework
{
    public class FabricPage : BasePage
    {
        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'Carousel-staticContent')]")]
        private IWebElement fabricPageTitle;

        public IReadOnlyList<IWebElement> LeftNavItems
        {
            get
            {
                return Browser.webDriver.FindElements(By.CssSelector("ul.LeftNav-links > li> a"));
            }
        }

        public string FabricPageTitle
        {
            get { return fabricPageTitle.Text; }
        }

        /// <summary>
        /// Select an item on Fabric page's own top nav bar.
        /// </summary>
        public void SelectTopNavItem(FabricNavItem item)
        {
            int waitTime = Int32.Parse(Utility.GetConfigurationValue("WaitTime"));
            string itemToSelect = EnumExtension.GetDescription(item);
            Browser.Wait(By.XPath("//ul[@class='docs-Nav']/li/a[text()='" + itemToSelect + "']"));
            var element = Browser.FindElement(By.XPath("//ul[@class='docs-Nav']/li/a[text()='" + itemToSelect + "']"));
            Browser.Click(element);
            Browser.Wait(TimeSpan.FromSeconds(waitTime));
        }

        /// <summary>
        /// Check whether select a fabric nav item can navigate to the correct page
        /// </summary>
        /// <param name="item">The item selected</param>
        /// <returns>True if yes, else no.</returns>
        public bool CanSwitchCorrectPage(FabricNavItem item)
        {
            string itemSelected = EnumExtension.GetDescription(item);
            if (itemSelected.Equals("Overview"))
            {
                return Browser.webDriver.Title.EndsWith("Fabric Home Page");
            }
            else if (itemSelected.Equals("Get Started"))
            {
                return Browser.webDriver.Title.EndsWith("Getting Started");
            }
            else
            {
                return Browser.webDriver.Title.EndsWith(itemSelected);
            }
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
    }
}