using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using System.Collections.Generic;
using System.Net;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace TestFramework
{
    public static class Browser
    {
        internal static IWebDriver webDriver;
        static string defaultTitle;
        static string defaultHandle;

        public static string BaseAddress
        {
            get { return Utility.GetConfigurationValue("BaseAddress"); }
            //get { return "http://officedevcenter-msprod-standby.azurewebsites.net"; }
            //get { return "http://officedevcentersite-orchard.azurewebsites.net"; }
            //get { return "http://localhost"; }
        }

        public static void Initialize(string postfix = "")
        {
            SetWaitTime(TimeSpan.FromSeconds(Utility.DefaultWaitTime));
            string address = BaseAddress;
            if (postfix != "")
            {
                address = BaseAddress + "/" + postfix;
            }
            webDriver.Navigate().GoToUrl(address);
            defaultTitle = Title;
        }

        public static void Goto(string url)
        {
            webDriver.Navigate().GoToUrl(url);
            defaultTitle = Title;
        }

        public static string Title
        {
            get { return webDriver.Title; }
        }

        public static string Url
        {
            get { return webDriver.Url; }
        }

        public static void Close()
        {
            webDriver.Quit();
        }

        public static SelectElement SelectElement(IWebElement webElement)
        {
            return new SelectElement(webElement);
        }

        public static ISearchContext Driver
        {
            get { return webDriver; }
        }

        public static void Wait(TimeSpan timeSpan)
        {
            // need to replace with Framework wait methods: implicit wait, explicit wait
            Thread.Sleep((int)timeSpan.TotalSeconds * 1000);
        }

        public static void Wait(By by)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.ElementExists(by));
        }

        public static void SetWaitTime(TimeSpan timeSpan)
        {
            webDriver.Manage().Timeouts().ImplicitlyWait(timeSpan);
            webDriver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(timeSpan.TotalSeconds * 2));
            webDriver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(timeSpan.TotalSeconds * 2));
        }

        /// <summary>
        /// Switch to a specific window
        /// </summary>
        /// <param name="title">The keyword of the window title</param>
        /// <returns>True if switching succeeds. If cannot find the window, returns false</returns>
        public static bool SwitchToWindow(string title)
        {
            webDriver.SwitchTo().DefaultContent();

            // get all window handles
            IList<string> handlers = webDriver.WindowHandles;
            foreach (var winHandler in handlers)
            {
                webDriver.SwitchTo().Window(winHandler);
                if (webDriver.Title.Contains(title))
                {
                    return true;
                }
                else
                {
                    webDriver.SwitchTo().DefaultContent();
                }
            }

            return false;
        }

        public static bool SwitchToNewWindow()
        {
            // get all window handles
            IList<string> handlers = webDriver.WindowHandles;
            string newWindowHandle = string.Empty;
            foreach (var winHandler in handlers)
            {
                if (!winHandler.Equals(webDriver.CurrentWindowHandle))
                {
                    newWindowHandle = winHandler;
                    break;
                }
            }

            if (!newWindowHandle.Equals(string.Empty))
            {
                webDriver.SwitchTo().Window(newWindowHandle);
                return true;
            }
            else
            {
                webDriver.SwitchTo().DefaultContent();
                return false;
            }
        }

        public static bool SwitchBack()
        {
            //bool canSwitchBack = SwitchToWindow(defaultTitle);
            //defaultTitle = Title;
            //return canSwitchBack;
            string currentHandle = webDriver.CurrentWindowHandle;
            if (!currentHandle.Equals(defaultHandle))
            {
                webDriver.Close();
                webDriver.SwitchTo().Window(defaultHandle);
                return true;
            }
            else
            {
                webDriver.SwitchTo().DefaultContent();
                return false;
            }
        }

        public static void GoBack()
        {
            if (!Title.Equals(defaultTitle))
            {
                webDriver.Navigate().Back();
            }
            else
            {
                webDriver.Navigate().Refresh();
            }
        }

        /// <summary>
        /// Get current window size
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void GetWindowSize(out int width, out int height)
        {
            width = webDriver.Manage().Window.Size.Width;
            height = webDriver.Manage().Window.Size.Height;
        }

        public static IWebElement FindElementInFrame(string frameIdOrName, By by, out string innerText)
        {
            IWebElement frame = FindFrame(frameIdOrName);
            if (frame != null)
            {
                webDriver.SwitchTo().Frame(frame);
                IWebElement element = webDriver.FindElement(by);
                innerText = element.Text;
                webDriver.SwitchTo().DefaultContent();
                return element;
            }
            else
            {
                throw new NoSuchFrameException(string.Format("Cannot find frame with name {0}", frameIdOrName));
            }
        }

        public static IWebElement FindElement(By by, bool isRootElement = true)
        {
            try
            {
                return webDriver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                IList<IWebElement> frames = webDriver.FindElements(By.TagName("iframe"));
                IWebElement element = null;
                if (frames.Count > 0)
                {
                    for (int i = 0; i < frames.Count; i++)
                    {
                        webDriver.SwitchTo().Frame(frames[i]);
                        element = FindElement(by, false);
                        if (element != null)
                        {
                            if (isRootElement)
                            {
                                webDriver.SwitchTo().DefaultContent();
                            }

                            return element;
                        }
                    }
                }

                if (element == null)
                {
                    webDriver.SwitchTo().ParentFrame();
                }

                return element;
            }
        }

        internal static void Click(IWebElement element)
        {
            (webDriver as IJavaScriptExecutor).ExecuteScript("arguments[0].click();", element);
        }

        public static void SaveScreenShot(string fileName)
        {
            ITakesScreenshot screenshot = (ITakesScreenshot)webDriver;
            fileName = string.Format("{0}\\{1}.png", Utility.GetConfigurationValue("ScreenShotSavePath"), fileName);
            Screenshot s = screenshot.GetScreenshot();
            s.SaveAsFile(fileName, System.Drawing.Imaging.ImageFormat.Png);

            //Screenshot ss = ((ITakesScreenshot)webDriver).GetScreenshot();
            //string screenshot = ss.AsBase64EncodedString;
            //byte[] screenshotAsByteArray = ss.AsByteArray;

            // Save the screenshot
            //ss.SaveAsFile(PathAndFileName, System.Drawing.Imaging.ImageFormat.Png);
        }

        /// <summary>
        /// Find an iframe element
        /// </summary>
        /// <param name="frameIdOrName">Id or name of the iframe</param>
        /// <returns>The found iframe</returns>
        private static IWebElement FindFrame(string frameIdOrName)
        {
            IList<IWebElement> frames = webDriver.FindElements(By.TagName("iframe"));
            IWebElement frame = null;
            if (frames.Count > 0)
            {
                for (int i = 0; i < frames.Count; i++)
                {
                    if (frames[i].GetAttribute("id") == frameIdOrName || frames[i].GetAttribute("name") == frameIdOrName)
                    {
                        frame = frames[i];
                        return frame;
                    }
                    else
                    {
                        webDriver.SwitchTo().Frame(frames[i]);
                        frame = FindFrame(frameIdOrName);
                        if (frame != null)
                        {
                            return frame;
                        }
                    }
                }
            }

            if (frame == null)
            {
                webDriver.SwitchTo().ParentFrame();
            }

            return frame;
        }

        static Browser()
        {
            switch (Utility.GetConfigurationValue("Browser"))
            {
                case ("Chrome"):
                    webDriver = new ChromeDriver(System.IO.Directory.GetCurrentDirectory() + @"/Drivers/");
                    break;
                case ("IE32"):
                    webDriver = new InternetExplorerDriver(System.IO.Directory.GetCurrentDirectory() + @"/Drivers/IE32/");
                    break;
                case ("IE64"):
                    webDriver = new InternetExplorerDriver(System.IO.Directory.GetCurrentDirectory() + @"/Drivers/IE64/");
                    break;
                case ("Firefox"):
                default:
                    webDriver = new FirefoxDriver();
                    break;
            }

            defaultHandle = webDriver.CurrentWindowHandle;
        }

        /// <summary>
        /// Adjust the window siae
        /// </summary>
        /// <param name="width">The new window width to set</param>
        /// <param name="height">The new window height to set</param>
        /// <param name="maxSize">whether maxsize the window and return the size</param>
        public static void SetWindowSize(int width, int height, bool maxSize = false)
        {
            if (maxSize)
            {
                webDriver.Manage().Window.Maximize();
            }
            else
            {
                System.Drawing.Size windowSize = new System.Drawing.Size();

                windowSize.Width = width;
                windowSize.Height = height;
                webDriver.Manage().Window.Size = windowSize;
            }
        }

        public static bool IsAtPage(string pageType)
        {
            SwitchToNewWindow();
            switch (pageType)
            {
                case "RSS":
                    if (webDriver.Url.Contains("feedburner.com/office/fmNx"))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
}