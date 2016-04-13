using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFramework;

namespace Tests
{
    /// <summary>
    /// Test class for pages under nav item Documentation
    /// </summary>
    [TestClass]
    public class DocumentationPageTest
    {
        #region Additional test attributes
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            Browser.Initialize();
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            Browser.Close();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            Browser.Goto(Utility.GetConfigurationValue("BaseAddress"));
        }

        #endregion

        /// <summary>
        /// Verify whether all links to Office add-in docs work
        /// </summary>
        [TestMethod]
        public void BVT_S18_TC01_CanGotoOfficeAddinDocPage()
        {
            Pages.Navigation.Select("Documentation", MenuItemOfDocumentation.AllDocumentation.ToString());
            foreach (ItemOfOfficeAddinDoc item in Enum.GetValues(typeof(ItemOfOfficeAddinDoc)))
            {
                Utility.SelectDocItem(item);
                Assert.IsTrue(Utility.IsAtOfficeAddinDocPage(item),
                    "Clicking {0} should refer to the related document page",
                    item.ToString());
                Browser.GoBack();
            }
        }

        /// <summary>
        /// Verify whether all links to Office add-in docs work
        /// </summary>
        [TestMethod]
        public void BVT_S18_TC02_CanGotoMSGraphDocPage()
        {
            Pages.Navigation.Select("Documentation", MenuItemOfDocumentation.AllDocumentation.ToString());
            foreach (ItemOfMSGraphDoc item in Enum.GetValues(typeof(ItemOfMSGraphDoc)))
            {
                Utility.SelectDocItem(item);
                Assert.IsTrue(Utility.IsAtMSGraphDocPage(item),
                    "Clicking {0} should refer to the related document page",
                    item.ToString());
                Browser.SwitchBack();
            }
        }

        /// <summary>
        /// Verify whether selecting an Office add-in's details can see the expected info
        /// on Office Add-in Availability Page
        /// </summary>
        [TestMethod]
        public void BVT_S18_TC03_CanFindOfficeAddInDetail()
        {
            Pages.Navigation.Select("Documentation", MenuItemOfDocumentation.OfficeAddinAvailability.ToString());
            string detailItem = string.Empty;
            Utility.SelectRandomOfficeAddInDetail(out detailItem);
            Assert.IsTrue(Utility.DetailExist(detailItem), "Details for {0} should appear", detailItem);
        }

        [TestMethod]
        public void Comps_S18_TC04_CheckOfficeAddInAvailabilityPageLinks()
        {
            Pages.Navigation.Select("Documentation", MenuItemOfDocumentation.OfficeAddinAvailability.ToString());
            Assert.IsTrue(Utility.CanSelectOfficeAddInRequirementSets(),"Office add-in requirement sets page should be available");

            Assert.IsTrue(Utility.CanSelectOfficeAddinsPlatformOverview(), "Office Add-ins platform overview page should be available");

            Assert.IsTrue(Utility.CanSelectJavaScriptAPIforOfficereference(), "JavaScript API for Office reference page should be available");
        }
    }
}
