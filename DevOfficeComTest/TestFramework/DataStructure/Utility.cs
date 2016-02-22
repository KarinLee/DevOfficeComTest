using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework
{
    /// <summary>
    /// Static class for common functions
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Some typical search text
        /// </summary>
        public static readonly string[] TypicalSearchText = new string[] { "Office", "API", "SharePoint", "Add-in", "Graph", "ios", "OneDrive" };

        public static readonly int MinWidthToShowParam = 895;

        public static int DefaultWaitTime = int.Parse(GetConfigurationValue("DefaultWaitTime"));

        /// <summary>
        /// Verify if none of the filters is checked
        /// </summary>
        /// <param name="unclearedFilters">The name of the uncleared filters</param>
        /// <returns>True if yes, else no.</returns>
        public static bool areFiltersCleared(out List<string> unclearedFilters)
        {
            IReadOnlyList<IWebElement> elements = Browser.Driver.FindElements(By.XPath(@"//*[@ng-model=""selectedTypes""]"));
            unclearedFilters = new List<string>();
            foreach (IWebElement element in elements)
            {
                if (element.GetAttribute("type").Equals("checkbox") && element.GetAttribute("checked") != null && element.GetAttribute("checked").Equals("checked"))
                {
                    unclearedFilters.Add(element.GetAttribute("value"));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Verify whether the url contains the chosen filters
        /// </summary>
        /// <param name="filterNames">The chosen filters</param>
        /// <returns>True if yes, else no.</returns>
        public static bool AreFiltersInURL(List<string> filterNames, out List<string> unContainedFilters)
        {
            bool allContained = true;
            unContainedFilters = new List<string>();
            foreach (string filterName in filterNames)
            {
                if (!Browser.Url.Replace("%20", " ").ToLower().Contains(filterName.ToLower()))
                {
                    allContained = false;
                    unContainedFilters.Add(filterName);
                }
            }
            return allContained;
        }

        /// <summary>
        /// Get the count of filters
        /// </summary>
        public static int GetFilterCount()
        {
            return Browser.Driver.FindElements(By.XPath(@"//*[@ng-model=""selectedTypes""]")).Count;
        }

        /// <summary>
        /// Check whether a filter has an "ng-click" attribute with an updating seleted result function's name.
        /// </summary>
        /// <param name="index">The index of the filter type to select</param>
        /// <returns>True if yes, else no.</returns>
        public static bool isFilterWorkable(int index)
        {
            var element = Browser.Driver.FindElements(By.XPath(@"//*[@ng-model=""selectedTypes""]"))[index];
            return element.GetAttribute("ng-click").Contains("updateSelectedTypes(");
        }

        /// <summary>
        /// Set the displayed results' sort order
        /// </summary>
        /// <param name="sortType">Specifies which sort type to use</param>
        /// <param name="isDescendent">Specifies whether the results are sorted descendently. True means yes, False means no</param>
        public static void SetSortOrder(SortType sortType, bool isDescendent)
        {
            string typeString;
            if (sortType.Equals(SortType.ViewCount))
            {
                typeString = "orderByViews()";
            }
            else
            {
                typeString = "orderByDate()";
            }

            var sortElement = Browser.Driver.FindElement(By.XPath("//a[@ng-click='" + typeString + "']"));
            if (!sortElement.Selected)
            {
                sortElement.Click();
            }

            string orderString = isDescendent ? "sort_down" : "sort_up";
            if (!sortElement.FindElement(By.ClassName("sort-icon")).GetAttribute("src").Contains(orderString))
            {
                sortElement.Click();
            }
        }

        /// <summary>
        /// Clear the filters 
        /// </summary>
        public static void ExecuteClearFilters()
        {
            var element = Browser.FindElement(By.CssSelector(".clearfilters.filter-button"));
            Browser.Click(element);
        }

        /// <summary>
        /// Set a search text
        /// </summary>
        /// <param name="value">The value to set</param>
        public static void InputSearchString(string value)
        {
            var inputElement = Browser.Driver.FindElement(By.XPath(@"//input[@ng-model=""searchText""]"));
            inputElement.Clear();
            inputElement.SendKeys(value);
        }

        /// <summary>
        /// Returns the filtered traings
        /// </summary>
        /// <param name="searchString">The search text to use</param>
        /// <returns>The search result list. Each result contains the training title and description</returns>
        public static List<SearchedResult> GetFilterResults(string searchString = "")
        {
            InputSearchString(searchString);
            List<SearchedResult> resultList = new List<SearchedResult>();

            var nextElement = Browser.FindElement(By.ClassName("next-link"));
            bool shouldReadFirstPage = true;
            do
            {
                if (shouldReadFirstPage)
                {
                    shouldReadFirstPage = false;
                }
                else
                {
                    Browser.Click(nextElement);
                }
                var uList = Browser.FindElement(By.CssSelector(@"#OrderedResults+ul"));
                IReadOnlyList<IWebElement> listItems = uList.FindElements(By.XPath("li"));
                for (int i = 0; i < listItems.Count; i++)
                {
                    SearchedResult resultInfo = new SearchedResult();
                    resultInfo.Name = listItems[i].GetAttribute("aria-label");

                    var descriptionElement = listItems[i].FindElement(By.ClassName("description"));
                    resultInfo.Description = descriptionElement.Text;

                    resultInfo.ViewCount = Convert.ToInt64((listItems[i].FindElement(By.XPath("//span[contains(text(),' views')]")).GetAttribute("innerHTML").Split(' '))[0]);

                    // Add if() here to reduce the time cost searching for non-existent element of class date-updated
                    if (!Browser.Url.Contains("/training"))
                    {
                        IWebElement updatedDateElement;
                        try
                        {
                            updatedDateElement = listItems[i].FindElement(By.CssSelector(".date-updated"));
                        }
                        catch (NoSuchElementException)
                        {
                            updatedDateElement = null;
                        }
                        if (updatedDateElement != null)
                        {
                            resultInfo.UpdatedDate = DateTime.Parse(updatedDateElement.Text.Replace("Updated ", null));
                        }
                    }

                    resultInfo.DetailLink = listItems[i].FindElement(By.XPath("//a[@role='link']")).GetAttribute("href");

                    resultList.Add(resultInfo);
                }
            } while (nextElement.Displayed);

            return resultList;
        }

        /// <summary>
        /// Choose a filter type
        /// </summary>
        /// <param name="index">The index of the filter type to select</param>
        /// <returns>The selected filter name</returns>
        public static string SelectFilter(int index)
        {
            var element = Browser.Driver.FindElements(By.XPath(@"//*[@ng-model=""selectedTypes""]"))[index];
            Browser.Click(element);

            return element.Text;
        }

        /// <summary>
        /// Choose a filter type
        /// </summary>
        /// <param name="filterName">The filter name to select</param>
        public static void SelectFilter(string filterName)
        {
            IReadOnlyList<IWebElement> elements = Browser.Driver.FindElements(By.XPath(@"//*[@ng-model=""selectedTypes""]"));
            foreach (IWebElement element in elements)
            {
                if (element.Text.Equals(filterName) || element.GetAttribute("value").Equals(filterName))
                {
                    Browser.Click(element);
                    break;
                }
            }
        }

        /// <summary>
        /// Verify whether a link of a specific source type can be found on the page 
        /// </summary>
        /// <param name="sourcePart">A string that contains the part of source link</param>
        /// <returns>True if yes, else no</returns>
        public static bool CanFindSourceLink(string sourcePart)
        {
            var element = Browser.FindElement(By.XPath("//a[contains(@href,'" + sourcePart + "')]"));
            if (element != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get a property's value from App.config
        /// </summary>
        /// <param name="propertyName">The property's key</param>
        /// <returns>The property's value</returns>
        public static string GetConfigurationValue(string propertyName)
        {
            return ConfigurationManager.AppSettings[propertyName];
        }

        /// <summary>
        /// Verify whether a url refer to a valid image
        /// </summary>
        /// <param name="Url">The image url</param>
        /// <returns>True if yes, else no</returns>
        public static bool ImageExist(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Timeout = 15000;
            request.Method = "HEAD";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotModified);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Verify whether a url is redirected.
        /// </summary>
        /// <param name="Url">The url to be verified</param>
        /// <returns>True if yes, else no</returns>
        public static bool IsUrlRedirected(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Timeout = 15000;
            request.Method = "GET";
            request.AllowAutoRedirect = false;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return (response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Redirect);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Click the branding image on the page
        /// </summary>
        public static void ClickBranding()
        {
            var element = Browser.FindElement(By.CssSelector("#branding>a"));
            Browser.Click(element);
        }

        /// <summary>
        /// Get all the links in a NavItem's sub menu
        /// </summary>
        /// <param name="itemIndex">The nav item index in nav bar</param>
        public static string[] GetNavSubItems(int itemIndex)
        {
            var element = Browser.FindElement(By.CssSelector("#navbar-collapse-1 > ul > li:nth-child(" + ((int)itemIndex + 1) + ") > a"));
            try
            {
                var subMenu = element.FindElement(By.XPath("parent::li/div"));
                IReadOnlyList<IWebElement> subItems = subMenu.FindElements(By.TagName("a"));

                string[] items = new string[subItems.Count];
                for (int i = 0; i < subItems.Count; i++)
                {
                    if (subItems[i].Displayed)
                    {
                        items[i] = subItems[i].Text;
                    }
                    else
                    {
                        items = null;
                        break;
                    }
                }
                return items;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        /// <summary>
        /// Verify whether the branding of dev center exists on the current page
        /// </summary>
        /// <returns></returns>
        public static bool BrandingExists()
        {
            var element = Browser.FindElement(By.XPath("//div[@id='branding']/a/img[@alt='Office Dev Office Center logo']"));
            if (element != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Input search text into iwidget input field and search it
        /// </summary>
        /// <param name="searchText">The text to input</param>
        /// <returns>The names of the search results</returns>
        public static string[] SearchWidget(string searchText)
        {
            var element = Browser.FindElement(By.XPath("//fieldset/input[@id='q']"));
            element.SendKeys(searchText);
            var searchElement = Browser.FindElement(By.XPath("//fieldset/button[@type='submit']"));
            Browser.Click(searchElement);

            Browser.Wait(TimeSpan.FromSeconds(5));

            IReadOnlyList<IWebElement> results = Browser.webDriver.FindElements(By.CssSelector("div.col-xs-8.name.cp1"));
            string[] resultNames = new string[results.Count];
            for (int i = 0; i < results.Count; i++)
            {
                resultNames[i] = results[i].Text;
            }

            return resultNames;
        }

        public static void ClickIcon(string iconType)
        {
            switch (iconType)
            {
                //case "Facebook":
                //    var element = Browser.FindElement(By.CssSelector("#atstbx > a.at-custom-share-anchor.at-share-btn.at-svc-facebook > span > span > svg"));
                //    Browser.Click(element);
                //    break;
                case "RSS":
                    var element = Browser.FindElement(By.CssSelector("#header-social > a"));
                    Browser.Click(element);
                    break;
            }
        }

        public static SliderMenuItem GetLeftMenuItem(SliderMenuItem item)
        {
            if ((int)item == 0)
            {
                return (SliderMenuItem)(Enum.GetNames(item.GetType()).Count() - 1);
            }
            else
            {
                return (SliderMenuItem)(item - 1);
            }
        }

        public static SliderMenuItem GetRightMenuItem(SliderMenuItem item)
        {
            if ((int)item == Enum.GetNames(item.GetType()).Count() - 1)
            {
                return (SliderMenuItem)(0);
            }
            else
            {
                return (SliderMenuItem)(item + 1);
            }
        }
    }
}
