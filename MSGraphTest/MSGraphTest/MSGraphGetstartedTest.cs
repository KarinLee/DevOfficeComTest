using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFramework;

namespace MSGraphTest
{
    /// <summary>
    /// Test Class for Microsoft Get started page
    /// </summary>
    [TestClass]
    public class MSGraphGetstartedTest
    {
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

        /// <summary>
        /// Verify whether "Office 365 Getting Started" on Get started page can navigate to devofficecom, getting started page.
        /// </summary>
        [TestMethod]
        public void Acceptance_Graph_S03_TC01_CanGoToOffice365GettingStartedPage()
        {
            GraphPages.Navigation.Select("Get started");
            GraphUtility.SelectO365GettingStarted();
            Assert.IsTrue(
                GraphBrowser.SwitchToWindow("Office Dev Center - Getting started with Office 365 REST APIs"),
                @"Clicking ""Office 365 Getting Started"" on Get started page can navigate to devofficecom, getting started page");
            GraphBrowser.SwitchBack();
        }
    }
}
