using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework
{
    /// <summary>
    /// A page on MS Graph site 
    /// </summary>
    public class GraphPage : GraphBasePage
    {
        private OpenQA.Selenium.Remote.RemoteWebElement graphTitle;
        public string GraphTitle
        {
            get { return graphTitle.WrappedDriver.Title; }
        }

        /// <summary>
        /// The constructor method
        /// </summary>
        /// <param name="atGraphSite">Indicates whether it is during the testing of ms graph or dev.office.com</param>
        public GraphPage()
        {
            GraphBrowser.Wait(By.CssSelector("head>title"));
            graphTitle = (OpenQA.Selenium.Remote.RemoteWebElement)GraphBrowser.Driver.FindElement(By.CssSelector("head>title"));
        }

        public bool CanLoadImages(GraphPageImages image)
        {
            switch (image)
            {
                case (GraphPageImages.MainBanner):
                    var element = GraphBrowser.Driver.FindElement(By.Id("banner-image"));
                    string Url = element.GetAttribute("style");
                    Url = GraphBrowser.BaseAddress + Url.Substring(Url.IndexOf('/'), Url.LastIndexOf('"') - Url.IndexOf('/'));
                    return GraphUtility.ImageExist(Url);
                case (GraphPageImages.Others):
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

    public enum GraphPageImages
    {
        MainBanner,
        Others
    }
}