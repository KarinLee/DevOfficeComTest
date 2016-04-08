using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace TestFramework
{
    public static class GraphBrowser
    {
        internal static IWebDriver webDriver;
        static string defaultTitle;
        static string defaultHandle;

        public static string BaseAddress
        {
            get
            {
                string address = GraphUtility.GetConfigurationValue("MSGraphBaseAddress");
                return address.EndsWith("/") ? address.Substring(0, address.Length - 1) : address;
            }
        }

        public static void Initialize()
        {
            SetWaitTime(TimeSpan.FromSeconds(GraphUtility.DefaultWaitTime));
            webDriver.Navigate().GoToUrl(BaseAddress);
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
                if (webDriver.Title.ToLower().Contains(title.ToLower()))
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

        public static void Refresh()
        {
            webDriver.Navigate().Refresh();
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
            fileName = string.Format("{0}\\{1}.png", GraphUtility.GetConfigurationValue("ScreenShotSavePath"), fileName);
            Screenshot s = screenshot.GetScreenshot();
            s.SaveAsFile(fileName, System.Drawing.Imaging.ImageFormat.Png);
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

        static GraphBrowser()
        {
            switch (GraphUtility.GetConfigurationValue("Browser").ToLower())
            {
                case ("chrome"):
                    webDriver = new ChromeDriver(System.IO.Directory.GetCurrentDirectory() + @"/Drivers/");
                    break;
                case ("ie32"):
                    webDriver = new InternetExplorerDriver(System.IO.Directory.GetCurrentDirectory() + @"/Drivers/IE32/");
                    break;
                case ("ie64"):
                    webDriver = new InternetExplorerDriver(System.IO.Directory.GetCurrentDirectory() + @"/Drivers/IE64/", new InternetExplorerOptions() {RequireWindowFocus = true });
                    break;
                case ("firefox"):
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

        /// <summary>
        /// Transfer the device screen size (in inches) to the pixel size on current screen(in pixels)
        /// </summary>
        /// <param name="deviceSize">The device Size. Commonly it is the diagonal length (in inches) of device screen</param>
        ///<param name="deviceResolution">Screen resolution of the device</param>
        ///<param name="windowSize">The size, in pixels, on the current screen.</param>
        /// <returns>The size on current screen(in pixels)</returns>
        public static void TransferPhysicalSizeToPixelSize(double deviceSize, Size deviceResolution, out Size windowSize)
        {
            Panel panel = new System.Windows.Forms.Panel();
            Graphics g = System.Drawing.Graphics.FromHwnd(panel.Handle);
            IntPtr hdc = g.GetHdc();

            //Get ppi
            int ppi = GetDeviceCaps(hdc, 88);
            g.ReleaseHdc(hdc);

            double ratio = (double)deviceResolution.Width / (double)deviceResolution.Height;
            //According to capulating formula, ppi=Math.Sqrt(1+Math.Pow(ratio,2))*height/deviceSize
            double dHeight = ppi * deviceSize / Math.Sqrt(1 + Math.Pow(ratio, 2));
            double dWidth = dHeight * ratio;
            windowSize = new Size();
            windowSize.Height = (int)dHeight;
            windowSize.Width = (int)dWidth;
        }

        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int Index);

        /// <summary>
        /// Wait for the returned response
        /// </summary>
        public static void WaitForExploreResponse()
        {
            GraphBrowser.Wait(By.CssSelector("#jsonViewer > div.ace_scroller > div > div.ace_layer.ace_text-layer > div.ace_line"));
            var element = GraphBrowser.FindElement(By.CssSelector("#jsonViewer > div.ace_scroller > div > div.ace_layer.ace_text-layer > div.ace_line"));
            int waitTime = Int32.Parse(GraphUtility.GetConfigurationValue("WaitTime"));
            int retryCount = Int32.Parse(GraphUtility.GetConfigurationValue("RetryCount"));
            int i = 0;
            do
            {
                GraphBrowser.Wait(TimeSpan.FromSeconds(waitTime));
                element = GraphBrowser.FindElement(By.CssSelector("#jsonViewer > div.ace_scroller > div > div.ace_layer.ace_text-layer > div.ace_line"));
                i++;
            } while (i < retryCount && element.Text.Equals(string.Empty));
        }

        public static bool SwitchBack()
        {
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
    }
}