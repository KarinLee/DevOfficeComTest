using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net;
using TestFramework;

namespace MSGraphTest
{
    /// <summary>
    /// Site Test class for Microsoft Graph
    /// </summary>
    [TestClass]
    public class MSGraphSiteTest
    {
        #region Additional test attributes
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            GraphBrowser.Initialize();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            GraphBrowser.Close();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            GraphBrowser.Goto(GraphBrowser.BaseAddress);
        }
        #endregion

        /// <summary>
        /// Verify whether robots.txt specifies the site is accessible.
        /// </summary>
        [TestMethod]
        public void BVT_Graph_S06_TC01_CanAccessSiteRobots()
        {
            string prefix = GraphUtility.RemoveRedundantPartsfromExtractBaseAddress();
            string url = prefix + "/robots.txt";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream);
            string readResponse = readStream.ReadToEnd();
            bool disallowed = readResponse.Contains("Disallow:");
            bool allowed = readResponse.Contains("Allow:");
            if (GraphBrowser.BaseAddress.Contains("graph.microsoft.io"))
            {//Production site
                Assert.IsTrue(!disallowed && allowed, "The site should be allowed to access by search engines.");
            }
            else
            {
                Assert.IsTrue(disallowed, "The site should not be allowed to accessby search engines");
            }
        }

        /// <summary>
        /// Verify whether Graph explorer neutral URL redirects to language specific page
        /// </summary>
        [TestMethod]
        public void BVT_Graph_S06_TC02_CanGoToLanguageSpecificExplorerPage()
        {
            //Goto the site home page, get the current language
            string prefix = GraphUtility.RemoveRedundantPartsfromExtractBaseAddress();
            GraphBrowser.Goto(prefix);
            string homePageLanguage = GraphBrowser.Url.Replace(prefix, "");
            GraphBrowser.Goto(prefix+"/graph-explorer");
            string currentUrl = GraphBrowser.Url;
            if (prefix.StartsWith("http:"))
            {
                currentUrl = currentUrl.Replace("https","http");
            }
            string explorerLanguage = currentUrl.Replace(prefix, "").Replace("/graph-explorer", "");
            Assert.AreEqual(homePageLanguage,
                explorerLanguage,
                "Graph explorer neutral URL should redirect to language specific page");
        }
    }
}
