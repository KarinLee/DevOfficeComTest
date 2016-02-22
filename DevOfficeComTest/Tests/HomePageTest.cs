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

        [TestMethod]
        public void Acceptance_S03_TC03_CanSlideToLeftMenuItem()
        {
            SliderMenuItem item = Pages.HomePage.GetCurrentMenuItem();
            Pages.HomePage.SlideToLeftMenuItem();
            SliderMenuItem nextItem = Utility.GetLeftMenuItem(item);
            Assert.IsTrue(Pages.HomePage.IsSliderMenuItemActive(nextItem), string.Format("The content of slider menu item {0} is not displayed correctly", nextItem.ToString()));
        }

        [TestMethod]
        public void Acceptance_S03_TC04_CanSlideToRightMenuItem()
        {
            SliderMenuItem item = Pages.HomePage.GetCurrentMenuItem();
            Pages.HomePage.SlideToRightMenuItem();
            SliderMenuItem nextItem = Utility.GetRightMenuItem(item);
            Assert.IsTrue(Pages.HomePage.IsSliderMenuItemActive(nextItem), string.Format("The content of slider menu item {0} is not displayed correctly", nextItem.ToString()));
        }

        [TestMethod]
        public void Comps_S03_TC05_CanChangeSliderMenuItem()
        {
            int width;
            int height;
            Browser.GetWindowSize(out width, out height);
            Browser.SetWindowSize(0, 0, true);
            if (Pages.HomePage.IsSliderMenuItemDisplayed())
            {
                foreach (SliderMenuItem item in Enum.GetValues(typeof(SliderMenuItem)))
                {
                    Pages.HomePage.ClickSliderMenu(item);
                    Assert.IsTrue(Pages.HomePage.IsSliderMenuItemActive(item), string.Format("The content of slider menu item {0} is not displayed correctly", item.ToString()));
                }
            }
            else
            {
                Assert.Inconclusive("This test case will be inconclusive if the Slider menu item is not displayed.");
            }

            Browser.SetWindowSize(width, height, false);
        }

        //[TestMethod]
        //public void Acceptance_S15_TC01_CanDisplayCorrectTradeMark()
        //{
        //    Assert.IsTrue(Pages.HomePage.CanDisplayCorrectTradeMark());
        //}

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Browser.Close();
        }
    }
}