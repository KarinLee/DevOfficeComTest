using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework
{
    public class GraphBasePage
    {
        /// <summary>
        /// The constructor method
        /// </summary>
        public GraphBasePage()
        {
            PageFactory.InitElements(GraphBrowser.Driver, this);
        }
    }
}
