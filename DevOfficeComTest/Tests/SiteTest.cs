using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFramework;

namespace Tests
{
    /// <summary>
    /// Site Test class for dev.office.com
    /// </summary>
    [TestClass]
    public class SiteTest
    {
        #region Additional test attributes
        [ClassCleanup]
        public static void ClassCleanup()
        {
            Browser.Close();
        }
        #endregion

        /// <summary>
        /// Verify whether robots.txt specifies the site is accessible.
        /// </summary>
        [TestMethod]
        public void BVT_S15_TC01_CanAccessSiteRobots()
        {
            string url = Browser.BaseAddress + "/robots.txt";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream);
            string readResponse = readStream.ReadToEnd();
            bool disallowed = readResponse.Contains("Disallow:");
            bool allowed = readResponse.Contains("Allow:");
            if (Browser.BaseAddress.Contains("dev.office.com"))
            {//Production site
                Assert.IsTrue(!disallowed && allowed, "The site should be allowed to access by search engines.");
            }
            else
            {
                Assert.IsTrue(disallowed, "The site should not be allowed to accessby search engines");
            }
        }
    }
}
