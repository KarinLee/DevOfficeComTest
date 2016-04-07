using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using TestFramework;

namespace Tests
{
    /// <summary>
    /// Test for Fabric Page
    /// </summary>
    [TestClass]
    public class FabricPageTest
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
        /// Verify whether select one item on Fabric page's own top nav bar can switch the correct page
        /// </summary>
        [TestMethod]
        public void BVT_S16_TC01_CanSwitchDocs()
        {
            Pages.Navigation.Select("Explore", MenuItemOfExplore.OfficeUIFabric.ToString());
            Browser.SetWindowSize(0, 0, true);
            foreach (FabricNavItem item in Enum.GetValues(typeof(FabricNavItem)))
            {
                FabricPage page = new FabricPage();
                page.SelectTopNavItem(item);
                Assert.IsTrue(page.CanSwitchCorrectPage(item),
                    "Select {0} should navigate to the correct page",
                    EnumExtension.GetDescription(item));
            }
        }

        /// <summary>
        /// Verify whether select any nav link on the left can refer to the correct doc part
        /// </summary>
        [TestMethod]
        public void BVT_S16_TC02_CanLeftNavWork()
        {
            Pages.Navigation.Select("Explore", MenuItemOfExplore.OfficeUIFabric.ToString());
            Browser.SetWindowSize(0, 0, true);
            Array items = Enum.GetValues(typeof(FabricNavItem));
            int randomIndex;
            FabricPage page = new FabricPage();
            //Overview and Blog don't have left nav items
            randomIndex = new Random().Next(1, items.Length - 1);
            page.SelectTopNavItem((FabricNavItem)items.GetValue(randomIndex));

            randomIndex = new Random().Next(page.LeftNavItems.Count);
            string itemName;
            Assert.IsTrue(page.IsValidLeftNavItem(randomIndex, out itemName),
                "Click {0} should refer to the related doc part.",
                itemName);
        }

        /// <summary>
        /// Verify whether there is a toggle arrow which work correctly when the window is small.
        /// </summary>
        [TestMethod]
        public void Comps_S16_TC03_CanToggleArrowWorkInSmallFabricPage()
        {
            int currentWidth = 0;
            int currentHeight = 0;
            Browser.GetWindowSize(out currentWidth, out currentHeight);
            Pages.Navigation.Select("Explore", MenuItemOfExplore.OfficeUIFabric.ToString());
            
            Size windowSize;
            //Set as the screen size of IPad2
            double deviceScreenSize = double.Parse(Utility.GetConfigurationValue("IPad2Size"));
            Browser.TransferPhysicalSizeToPixelSize(
                deviceScreenSize,
                new Size
                {
                    Width = int.Parse(Utility.GetConfigurationValue("IPad2ScreenResolutionWidth")),
                    Height = int.Parse(Utility.GetConfigurationValue("IPad2ScreenResolutionHeight"))
                },
                out windowSize);
            Browser.SetWindowSize(windowSize.Width, windowSize.Height);

            Assert.IsTrue(
                Utility.IsToggleMenuDisplayed(),
                "An IPad2 window size ({0} inches) can make menu icon appear.",
                deviceScreenSize);
            Assert.IsFalse(Utility.IsMobileMenuContentDisplayed(),
                "When the menu icon exists, menu should be hidden.");

            Utility.ToggleMobileMenu();
            Assert.IsTrue(Utility.IsMobileMenuContentDisplayed(),
                "When the menu icon exists and menu is hidden,clicking the menu icon should show menu.");

            Utility.ToggleMobileMenu();
            Assert.IsFalse(Utility.IsMobileMenuContentDisplayed(),
                "When the menu icon exists and menu is shown,clicking the icon should hide menu.");

            //Set as the screen size of IPhone6 plus
            deviceScreenSize = double.Parse(Utility.GetConfigurationValue("IPhone6PlusSize"));
            //Since mobile phone width<Height, invert the output values
            Browser.TransferPhysicalSizeToPixelSize(
               deviceScreenSize,
               new Size
               {
                   Width = int.Parse(Utility.GetConfigurationValue("IPhone6PlusScreenResolutionWidth")),
                   Height = int.Parse(Utility.GetConfigurationValue("IPhone6PlusScreenResolutionHeight"))
               },
               out windowSize);
            //Since mobile phone widh<height, invert height and width
            Browser.SetWindowSize(windowSize.Height, windowSize.Width);

            Assert.IsTrue(
                Utility.IsToggleMenuDisplayed(),
                "An IPhone6 Plus window size ({0} inches) can make menu icon appear.",
                deviceScreenSize);

            //Recover the window size
            Browser.SetWindowSize(currentWidth, currentHeight);
        }

        /// <summary>
        /// Verify whether toggle mobile menu icon hides when the window is large.
        /// </summary>
        [TestMethod]
        public void Acceptance_S16_TC04_CanToggleArrowHideInLargeFabricPage()
        {
            int currentWidth = 0;
            int currentHeight = 0;
            Browser.GetWindowSize(out currentWidth, out currentHeight);
            Pages.Navigation.Select("Explore", MenuItemOfExplore.OfficeUIFabric.ToString());
            
            int actualWidth = 0;
            int actualHeight = 0;
            //Maxsize the window to see if it is possible to hide the arrow
            Browser.SetWindowSize(actualWidth, actualHeight, true);
            Browser.GetWindowSize(out actualWidth, out actualHeight);
            if (Utility.IsToggleMenuDisplayed())
            {
                Browser.SetWindowSize(currentWidth, currentHeight);
                Assert.Inconclusive(
                    "A window size ({0}*{1}) is not big enough to hide menu icon",
                    actualWidth,
                    actualHeight);
            }
            else
            {
                //Set a common laptop size: 17.3 and a common screen resolution:1024*768
                double deviceScreenSize = 17.3;
                Size windowSize;
                Browser.TransferPhysicalSizeToPixelSize(
                    deviceScreenSize,
                    new Size
                    {
                        Width = 1024,
                        Height = 768
                    },
                    out windowSize);
                Browser.SetWindowSize(windowSize.Width, windowSize.Height);

                Assert.IsFalse(
                    Utility.IsToggleMenuDisplayed(),
                    "An large window size ({0} inches) can make menu icon hide.",
                    deviceScreenSize);
            }
            //Recover the window size
            Browser.SetWindowSize(currentWidth, currentHeight);
        }

    }
}
