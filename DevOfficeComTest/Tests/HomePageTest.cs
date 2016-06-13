using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFramework;

namespace Tests
{
    [TestClass]
    public class HomePageTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Browser.Initialize();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Browser.Goto(Browser.BaseAddress);
        }

        [TestMethod]
        public void BVT_S03_TC01_CanGoToHomePage()
        {
            Assert.IsTrue(Pages.HomePage.IsAt());
        }

        [TestMethod]
        public void BVT_S03_TC02_CanLoadHomePageImages()
        {
            foreach (HomePageImages item in Enum.GetValues(typeof(HomePageImages)))
            {
                Assert.IsTrue(Pages.HomePage.CanLoadImages(item));
            }
        }

        /// <summary>
        /// Verify whether select a product on home page to get started can navigate to
        /// the correct page
        /// </summary>
		[TestMethod]
		public void BVT_S03_TC03_CanGetStartedWithProducts()
		{
            foreach (OfficeAppItem item in Enum.GetValues(typeof(OfficeAppItem)))
            {
                Pages.HomePage.SelectGetStartedProduct(item);
                Assert.IsTrue(Utility.IsAtAppDevPage(item),
                    "{0} page should be valid",
                    item.ToString());
            }
		}

        /// <summary>
        /// Verify whether Microsoft Graph page can be navigated to.
        /// </summary>
		[TestMethod]
		public void BVT_S03_TC04_CanGotoGraphPage()
		{
            Pages.HomePage.SelectMSGraph();
            Assert.IsTrue(Pages.Navigation.IsAtGraphPage("MicrosoftGraph"),
                "Microsoft Graph page should be opened.");
		}

        // Build section is removed from homepage. So comment out this case for now.
        /// <summary>
        /// Verify whether Build page can be navigated to.
        /// </summary>
        //[TestMethod]
        //public void Comps_S03_TC05_CanGotoBuildPage()
        //{
        //    string scheduledTime;
        //    Pages.HomePage.SelectBuild(out scheduledTime);
        //    Assert.IsTrue(Utility.IsAtBuildPage(string.Empty),
        //            "Build page should be opened");

        //    Pages.HomePage.SelectBuild(out scheduledTime,true);
        //    Assert.IsTrue(Utility.IsAtBuildPage(scheduledTime),
        //            "The build event time should be correct");
        //}

        /// <summary>
        /// Verify whether signup page can be navigated to.
        /// </summary>
        [TestMethod]
        public void BVT_S03_TC06_CanGotoSignup()
        {
            Pages.HomePage.SelectSignup();
            Assert.IsTrue(Utility.IsAtProfileCenterPage(),"Selecting to sign up should navigate to profile center page");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Browser.Close();
        }
    }
}