using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using TestFramework;

namespace Tests
{
    /// <summary>
    /// Summary description for SkypePageTest
    /// </summary>
    [TestClass]
    public class SkypePageTest
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
            //Browser.Goto(Utility.GetConfigurationValue("BaseAddress"));
            Browser.SwitchBack();
        }
        #endregion

        /// <summary>
        /// Verify whether select one item on Skype page's own top nav bar can switch the correct page
        /// </summary>
        [TestMethod]
        public void BVT_S17_TC01_CanNavThroughSkypeMenu()
        {
            //int currentWidth = 0;
            //int currentHeight = 0;
            //Browser.GetWindowSize(out currentWidth, out currentHeight);
            //Browser.SetWindowSize(0, 0, true);
            Browser.GotoSkypePage();
            
            foreach (SkypeNavItem item in Enum.GetValues(typeof(SkypeNavItem)))
            {
                SkypePage page = new SkypePage();
                page.SelectTopNavItem(item);
                Assert.IsTrue(page.CanSwitchCorrectPage(item),
                    "Select {0} should navigate to the correct page",
                    EnumExtension.GetDescription(item));
            }
            ////Recover the window size
            //Browser.SetWindowSize(currentWidth, currentHeight);
        }

        /// <summary>
        /// Verify whether select any nav link on the left can refer to the correct doc part
        /// </summary>
        [TestMethod]
        public void BVT_S17_TC02_CanLeftNavWork()
        {
            //int currentWidth = 0;
            //int currentHeight = 0;
            //Browser.GetWindowSize(out currentWidth, out currentHeight); 
            //Browser.SetWindowSize(0, 0, true);
            Browser.GotoSkypePage();

            Array items = Enum.GetValues(typeof(SkypeNavItem));
            int randomIndex;
            SkypePage page = new SkypePage();
            //Overview and Marketplace don't have left nav items
            randomIndex = new Random().Next(1, items.Length - 1);
            page.SelectTopNavItem((SkypeNavItem)items.GetValue(randomIndex));

            randomIndex = new Random().Next(page.LeftNavItems.Count);
            string itemName;
            Assert.IsTrue(page.IsValidLeftNavItem(randomIndex, out itemName),
                "Click {0} should refer to the related doc part.",
                itemName);
            ////Recover the window size
            //Browser.SetWindowSize(currentWidth, currentHeight);
        }

        /// <summary>
        /// Verify whether there is a toggle arrow which work correctly when the window is small.
        /// </summary>
        [TestMethod]
        public void Comps_S17_TC03_CanToggleArrowWorkInSmallSkypePage()
        {
            Browser.GotoSkypePage();

            int currentWidth = 0;
            int currentHeight = 0;
            Browser.GetWindowSize(out currentWidth, out currentHeight);
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
                SkypePage.IsToggleMenuIconDisplayed(),
                "An IPad2 window size ({0} inches) can make menu icon appear.",
                deviceScreenSize);
            Assert.IsFalse(SkypePage.IsMobileMenuContentDisplayed(),
                "When the menu icon exists, menu should be hidden.");

            SkypePage.ToggleMobileMenu();
            Assert.IsTrue(SkypePage.IsMobileMenuContentDisplayed(),
                "When the menu icon exists and menu is hidden,clicking the menu icon should show menu.");

            SkypePage.ToggleMobileMenu();
            Assert.IsFalse(SkypePage.IsMobileMenuContentDisplayed(),
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
                SkypePage.IsToggleMenuIconDisplayed(),
                "An IPhone6 Plus window size ({0} inches) can make menu icon appear.",
                deviceScreenSize);

            //Recover the window size
            Browser.SetWindowSize(currentWidth, currentHeight);
        }

        /// <summary>
        /// Verify whether toggle mobile menu icon hides when the window is large.
        /// </summary>
        [TestMethod]
        public void Acceptance_S17_TC04_CanToggleArrowHideInLargeSkypePage()
        {
            Browser.GotoSkypePage();
            
            int currentWidth = 0;
            int currentHeight = 0;
            Browser.GetWindowSize(out currentWidth, out currentHeight);
            int actualWidth = 0;
            int actualHeight = 0;
            //Maxsize the window to see if it is possible to hide the arrow
            Browser.SetWindowSize(actualWidth, actualHeight, true);
            Browser.GetWindowSize(out actualWidth, out actualHeight);
            if (SkypePage.IsToggleMenuIconDisplayed())
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
                    SkypePage.IsToggleMenuIconDisplayed(),
                    "An large window size ({0} inches) can make menu icon hide.",
                    deviceScreenSize);
            }
            //Recover the window size
            Browser.SetWindowSize(currentWidth, currentHeight);
        }
    }
}
