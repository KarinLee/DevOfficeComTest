﻿using OpenQA.Selenium;
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
    /// Static class for common functions of Graph site
    /// </summary>
    public static class GraphUtility
    {
        public static int DefaultWaitTime = int.Parse(GetConfigurationValue("DefaultWaitTime"));

        /// <summary>
        /// Verify if the toggle arrow is found on the page 
        /// </summary>
        /// <returns>Trye if yes, else no.</returns>
        public static bool IsToggleArrowDisplayed()
        {
            return GraphBrowser.FindElement(By.Id("toggleLeftPanelContainer")).Displayed;
        }

        /// <summary>
        /// Verify if the menu-content is found on the page
        /// </summary>
        /// <returns>Trye if yes, else no.</returns>
        public static bool IsMenuContentDisplayed()
        {
            return GraphBrowser.FindElement(By.CssSelector("#menu-content")).Displayed;
        }

        /// <summary>
        /// Execute the menu display toggle
        /// </summary>
        public static void ToggleMenu()
        {
            var element = GraphBrowser.FindElement(By.Id("toggleLeftPanelContainer"));
            GraphBrowser.Click(element);
            GraphBrowser.Wait(TimeSpan.FromSeconds(2));
        }

        /// <summary>
        /// Click the branding image on the page
        /// </summary>
        public static void ClickBranding()
        {
            var element = GraphBrowser.FindElement(By.CssSelector("#branding>a"));
            GraphBrowser.Click(element);
        }

        /// <summary>
        /// Get the document title in the current doc page
        /// </summary>
        /// <returns>The title of document</returns>
        public static string GetDocTitle()
        {
            string docTitle;
            var title = GraphBrowser.FindElementInFrame("docframe", By.TagName("h1"), out docTitle);
            return docTitle;
        }

        /// <summary>
        /// Get the banner image url of MS Graph site
        /// </summary>
        /// <returns>The url of the banner image</returns>
        public static string GetGraphBannerImageUrl()
        {
            var element = GraphBrowser.FindElement(By.Id("banner-image"));
            if (element == null)
            {
                element = GraphBrowser.FindElement(By.CssSelector("div#layout-featured>div>article>div>div>div>div"));
            }
            string styleString = element.GetAttribute("style");
            string[] styles = styleString.Split(';');

            string url = string.Empty;
            foreach (string style in styles)
            {
                if (style.Contains("background-image"))
                {
                    int startIndex = style.IndexOf("/");
                    //2 is the length of ")
                    url = style.Substring(startIndex).Substring(0, style.Substring(startIndex).Length - 2);
                    break;
                }
            }
            return url;
        }

        /// <summary>
        /// Find an link or a button role span according to the specific text and click it
        /// </summary>
        /// <param name="text">The text of the element</param>
        public static void Click(string text)
        {
            var element = GraphBrowser.FindElement(By.LinkText(text));
            //a link
            if (element != null && element.Displayed)
            {
                GraphBrowser.Click(element);
            }
            else
            {
                IReadOnlyList<IWebElement> elements = GraphBrowser.webDriver.FindElements(By.XPath("//*[@role='button']"));
                foreach (IWebElement elementToClick in elements)
                {
                    if (elementToClick.GetAttribute("innerHTML").Equals(text) && (elementToClick.Displayed))
                    {
                        GraphBrowser.Click(elementToClick);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Find a button according to the specific text and click it
        /// </summary>
        /// <param name="text">The text of the element</param>
        public static void ClickButton(string text)
        {
            IReadOnlyList<IWebElement> elements = GraphBrowser.webDriver.FindElements(By.TagName("button"));

            foreach (IWebElement elementToClick in elements)
            {
                if (elementToClick.GetAttribute("innerHTML").Trim().Contains(text) && elementToClick.Displayed)
                {
                    GraphBrowser.Click(elementToClick);
                    break;
                }
            }
        }

        /// <summary>
        /// Randomly find a TOC item, which has a sub content menu, at a specific layer.
        /// </summary>
        /// <param name="layerIndex">The layer index. Starts from 0.</param>
        /// <returns>A list including the names of path items</returns>
        public static List<string> FindTOCParentItems(int layerIndex)
        {
            List<string> tocPath = new List<string>();
            string xpath = @"//nav[@id='home-nav-blade']";

            for (int i = 0; i <= layerIndex; i++)
            {
                xpath += "/ul/li";
            }
            var elements = GraphBrowser.webDriver.FindElements(By.XPath(xpath + "/a[@data-target]"));
            int randomIndex = new Random().Next(elements.Count);
            var element = elements[randomIndex];
            string title = element.GetAttribute("innerHTML");
            if (element.GetAttribute("style").Contains("text-transform: uppercase"))
            {
                title = title.ToUpper();
            }

            var ancestorElements = element.FindElements(By.XPath("ancestor::li/a")); //parent relative to current element
            for (int j = 0; j < ancestorElements.Count - 1; j++)
            {
                string ancestorTitle = ancestorElements[j].GetAttribute("innerHTML");
                if (ancestorElements[j].GetAttribute("style").Contains("text-transform: uppercase"))
                {
                    ancestorTitle = ancestorTitle.ToUpper();
                }
                tocPath.Add(ancestorTitle);
            }

            tocPath.Add(title);
            return tocPath;
        }

        /// <summary>
        /// Verify whether a TOC item's related sub layer is shown 
        /// </summary>
        /// <param name="item">The TOC item</param>
        /// <returns>True if yes, else no.</returns>
        public static bool SubLayerDisplayed(string item)
        {
            string xpath = @"//nav[@id='home-nav-blade']";
            var element = GraphBrowser.FindElement(By.XPath(xpath));
            var menuItem = element.FindElement(By.LinkText(item));
            string subMenuId = menuItem.GetAttribute("data-target");
            if (subMenuId != null && subMenuId != string.Empty)
            {
                var subMenu = element.FindElement(By.XPath("//ul[@id='" + subMenuId.Replace("#", string.Empty) + "']"));
                return subMenu.Displayed;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get The layer count of TOC
        /// </summary>
        /// <returns>The layer count</returns>
        public static int GetTOCLayer()
        {
            string xpath = "//nav[@id='home-nav-blade']";
            var menuElement = GraphBrowser.FindElement(By.XPath(xpath));
            int layer = 0;
            try
            {
                do
                {
                    layer++;
                    xpath += "/ul/li";
                    var element = menuElement.FindElement(By.XPath(xpath + "/a"));
                } while (true);
            }
            catch (NoSuchElementException)
            {
            }
            return layer - 1;
        }

        /// <summary>
        /// Login on a sign-in page 
        /// </summary>
        /// <param name="userName">The userName to input</param>
        /// <param name="password">The password to input</param>
        public static void Login(string userName, string password)
        {
            var userIdElement = GraphBrowser.FindElement(By.XPath("//input[@id='cred_userid_inputtext']"));
            userIdElement.SendKeys(userName);
            var passwordElement = GraphBrowser.FindElement(By.XPath("//input[@id='cred_password_inputtext']"));
            passwordElement.SendKeys(password);
            var signInElement = GraphBrowser.FindElement(By.XPath("//span[@id='cred_sign_in_button']"));
            do
            {
                GraphBrowser.Wait(TimeSpan.FromSeconds(1));
            } while (!signInElement.Enabled);
            GraphBrowser.Click(signInElement);

        }

        /// <summary>
        /// Verify whether the logged in user is correct
        /// </summary>
        /// <param name="expectedUserName">The expected logged in user</param>
        /// <returns>True if yes, else no.</returns>
        public static bool IsLoggedIn(string expectedUserName = "")
        {
            var element = GraphBrowser.FindElement(By.XPath("//a[@ng-show='userInfo.isAuthenticated']"));
            if (element.Displayed && expectedUserName != "" && element.Text.Equals(expectedUserName))
            {
                return true;
            }
            else if (expectedUserName == "" && element.Displayed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Input a query string on Graph explorer page
        /// </summary>
        /// <param name="queryString">The query string to input</param>
        public static void InputExplorerQueryString(string queryString)
        {
            var inputElement = GraphBrowser.Driver.FindElement(By.XPath(@"//input[@id=""queryBar""]"));
            inputElement.Clear();
            inputElement.SendKeys(queryString);
        }

        /// <summary>
        /// Format a property to JSON format  and put it in Explorer request field
        /// </summary>
        /// <param name="properties">The properties to format</param>
        public static void InputExplorerJSONBody(Dictionary<string, string> properties)
        {
            var element = GraphBrowser.FindElement(By.CssSelector("div#jsonEditor>textarea"));
            //element.SendKeys("{\"" + property.Key + "\":\"" + property.Value + "\"}");
            element.SendKeys("{");
            int index = 0;
            foreach (KeyValuePair<string, string> property in properties)
            {
                index++;
                element.SendKeys("\"" + property.Key + "\":\"" + property.Value + "\"");
                if (index != properties.Count)
                {
                    element.SendKeys(",");
                }
            }
            element.SendKeys("}");
        }

        /// <summary>
        /// Get the response on Graph explorer page
        /// </summary>
        /// <returns>The composed response string</returns>
        public static string GetExplorerResponse()
        {
            var textElement = GraphBrowser.webDriver.FindElement(By.XPath("//div[@id='jsonViewer']/div/div[contains(@class,'ace_content')]/div[contains(@class,'ace_text-layer')]"));

            StringBuilder responseBuilder = new StringBuilder();

            IReadOnlyList<IWebElement> responseElements = textElement.FindElements(By.TagName("span"));
            for (int i = 0; i < responseElements.Count; i++)
            {
                responseBuilder.Append(responseElements[i].Text);
            }
            //Remove the braces
            int length = responseBuilder.Length;
            return responseBuilder.ToString().Substring(1, length - 2);
        }

        /// <summary>
        /// Parse a string of simple JSON format, which means no composed properties
        /// </summary>
        /// <param name="jsonString">The string to parse</param>
        /// <returns>A dictionary contains properties' keys and values</returns>
        public static Dictionary<string, string> ParseJsonFormatProperties(string jsonString)
        {
            string[] meProperties = jsonString.Split(',');

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string property in meProperties)
            {
                string propertyName = property.Split(':')[0].Replace("\"", "");
                //3 for the length of "":
                dictionary.Add(propertyName, property.Substring(propertyName.Length + 3).Replace("\"", ""));
            }
            return dictionary;
        }

        public static void SelectO365AppRegisstration()
        {
            var element = GraphBrowser.FindElement(By.XPath("//a[contains(@href,'dev.office.com/app-registration')]"));
            GraphBrowser.Click(element);
        }

        public static void SelectNewAppRegisstrationPortal()
        {
            var element = GraphBrowser.FindElement(By.XPath("//a[contains(@href,'apps.dev.microsoft.com')]"));
            GraphBrowser.Click(element);
        }

        /// <summary>
        /// Verify whether the page is at the expected app registration portal
        /// </summary>
        /// <param name="isNewPortal">The expected portal page type, true for
        /// new protal, false for O365 portal</param>
        /// <returns>True if yes, else no.</returns>
        public static bool IsAtApplicationRegistrationPortal(bool isNewPortal)
        {
            GraphBrowser.webDriver.SwitchTo().DefaultContent();
            string urlKeyWord = isNewPortal ? "apps.dev.microsoft.com" : "dev.office.com/app-registration";
            // get all window handles
            IList<string> handlers = GraphBrowser.webDriver.WindowHandles;
            foreach (var winHandler in handlers)
            {
                GraphBrowser.webDriver.SwitchTo().Window(winHandler);
                if (GraphBrowser.webDriver.Url.Contains(urlKeyWord))
                {
                    return true;
                }
                else
                {
                    GraphBrowser.webDriver.SwitchTo().DefaultContent();
                }
            }
            return false;
        }

        public static void SelectToSeeOverView()
        {
            var element = GraphBrowser.FindElement(By.XPath("//div/a[contains(@href,'/docs')]"));
            GraphBrowser.Click(element);
        }

        public static void SelectToTryAPI()
        {
            var element = GraphBrowser.FindElement(By.XPath("//div/a[contains(@href,'graphexplorer2.azurewebsites.net')]"));
            GraphBrowser.Click(element);
        }

        public static void SelectO365GettingStarted()
        {
            var element = GraphBrowser.FindElement(By.XPath("//button[contains(@onclick,'getting-started/office365apis')]"));
            GraphBrowser.Click(element);
        }

        /// <summary>
        /// Return a random selection of items at TOC's specific level
        /// </summary>
        /// <param name="index">The specific level index. Starts from 0</param>
        /// <param name="hasDoc">Indicates whether only returns the items each of which has the related documents</param>
        /// <returns>TOC Items' title-and-links(separated by ,). The title part contains the item's whole path in TOC</returns>
        public static string GetTOCItem(int index, bool hasDoc = false)
        {
            string xPath = "//nav[@id='home-nav-blade']";
            for (int i = 0; i <= index; i++)
            {
                xPath += "/ul/li";
            }
            IReadOnlyList<IWebElement> links = GraphBrowser.webDriver.FindElements(By.XPath(xPath + "/a"));
            string item = string.Empty;

            int randomIndex;
            do
            {
                randomIndex = new Random().Next(links.Count);

                string path = string.Empty;
                var ancestorElements = links[randomIndex].FindElements(By.XPath("ancestor::li/a")); //parent relative to current element
                for (int j = 0; j < ancestorElements.Count - 1; j++)
                {
                    string ancestorTitle = ancestorElements[j].GetAttribute("innerHTML");
                    if (ancestorElements[j].GetAttribute("style").Contains("text-transform: uppercase"))
                    {
                        ancestorTitle = ancestorTitle.ToUpper();
                    }
                    path += ancestorTitle + ">";
                }

                if (hasDoc)
                {
                    if (!links[randomIndex].GetAttribute("href").EndsWith("/"))
                    {
                        item = path + links[randomIndex].GetAttribute("innerHTML") + "," + links[randomIndex].GetAttribute("href");
                    }
                }
                else
                {
                    item = path + links[randomIndex].GetAttribute("innerHTML") + "," + links[randomIndex].GetAttribute("href");
                }
            } while (links[randomIndex].GetAttribute("href").EndsWith("/"));
            return item;
        }

        /// <summary>
        /// Verify if the document displayed matches TOC item's link
        /// </summary>
        /// <param name="tocLink">TOC item's link</param>
        /// <returns>True if yes, else no.</returns>
        public static bool ValidateDocument(string tocLink)
        {
            string elementSrc = GraphBrowser.FindElement(By.XPath("//iframe[@id='docframe']")).GetAttribute("src").Replace("zh-cn/", "").Replace("en-us/", "");

            if (tocLink.Replace("zh-cn/", "").Replace("en-us/", "").EndsWith(elementSrc.Replace(".htm", "").Replace("/GraphDocuments", "")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ClickLogin()
        {
            var element = GraphBrowser.FindElement(By.XPath("//a[@ng-click='login()']"));
            GraphBrowser.Click(element);
        }

        public static void ClickLogout()
        {
            var element = GraphBrowser.FindElement(By.XPath("//a[@ng-click='logout()']"));
            GraphBrowser.Click(element);
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
    }
}
