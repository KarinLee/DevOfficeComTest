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
    public class NavigationTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Browser.Initialize();
        }

        [TestMethod]
        public void BVT_S02_TC01_CanGoToGettingStartedPage()
        {
            Pages.Navigation.Select("Getting Started");
            Assert.IsTrue(Pages.Navigation.IsAtOfficeGettingStartedPage("Getting Started"));
        }

        [TestMethod]
        public void BVT_S02_TC02_CanGoToCodeSamplesPage()
        {
            Browser.SetWaitTime(TimeSpan.FromSeconds(30));
            Pages.Navigation.Select("Code Samples");
            Assert.IsTrue(Pages.Navigation.IsAtCodeSamplesPage("Code Samples"));
            Browser.SetWaitTime(TimeSpan.FromSeconds(Utility.DefaultWaitTime));
        }

        [TestMethod]
        public void BVT_S02_TC03_CanGoToExploreSubPage()
        {
            foreach (MenuItemOfExplore item in Enum.GetValues(typeof(MenuItemOfExplore)))
            {
                Pages.Navigation.Select("Explore", item.ToString());
                //Browser.Wait(TimeSpan.FromSeconds(1));
                Assert.IsTrue(Pages.Navigation.IsAtExplorePage(item), string.Format("The menu item {0} is not opened currectly.", item.ToString()));
            }
        }

        [TestMethod]
        public void BVT_S02_TC04_CanGoToResourceSubPage()
        {
            foreach (MenuItemOfResource item in Enum.GetValues(typeof(MenuItemOfResource)))
            {
                switch (item)
                {
                    case (MenuItemOfResource.Training):
                        Browser.SetWaitTime(TimeSpan.FromSeconds(30));
                        Pages.Navigation.Select("Resources", item.ToString());
                        //Browser.Wait(TimeSpan.FromSeconds(20));
                        Assert.IsTrue(Pages.Navigation.IsAtResourcePage(item), string.Format("The menu item {0} is not opened currectly.", item.ToString()));
                        Browser.SetWaitTime(TimeSpan.FromSeconds(Utility.DefaultWaitTime));
                        break;
                    default:
                        Pages.Navigation.Select("Resources", item.ToString());
                        //Browser.Wait(TimeSpan.FromSeconds(10));
                        Assert.IsTrue(Pages.Navigation.IsAtResourcePage(item), string.Format("The menu item {0} is not opened currectly.", item.ToString()));
                        break;
                }
            }
        }

        [TestMethod]
        public void BVT_S02_TC05_CanGoToDocumentationSubPage()
        {
            foreach (MenuItemOfDocumentation item in Enum.GetValues(typeof(MenuItemOfDocumentation)))
            {
                Pages.Navigation.Select("Documentation", item.ToString());
                Assert.IsTrue(Pages.Navigation.IsAtDocumentationPage(item), string.Format("The menu item {0} is not opened currectly.", item.ToString()));
            }
        }

        [TestMethod]
        public void Acceptance_S02_TC06_CanRedirectPage()
        {
            string url = Browser.BaseAddress + "/codesamples";
            //Browser.Goto(url);
            //bool isRedirected = (Browser.Url.Equals(Browser.BaseAddress + "/code-samples"));
            bool isRedirected = Utility.IsUrlRedirected(url);
            Assert.IsTrue(isRedirected, string.Format("The custom alias {0} is not redirected.", url));

            url = Browser.BaseAddress + "/gettingstarted";
            isRedirected = Utility.IsUrlRedirected(url);
            Assert.IsTrue(isRedirected, string.Format("The custom alias {0} is not redirected.", url));

            url = Browser.BaseAddress + "/sample-code";
            isRedirected = Utility.IsUrlRedirected(url);
            Assert.IsTrue(isRedirected, string.Format("The custom alias {0} is not redirected.", url));

            url = Browser.BaseAddress + "/snack-videos!"; 
            isRedirected = Utility.IsUrlRedirected(url);
            Assert.IsTrue(isRedirected, string.Format("The custom alias {0} is not redirected.", url));

            url = Browser.BaseAddress + "/opportuni";
            isRedirected = Utility.IsUrlRedirected(url);
            Assert.IsTrue(isRedirected, string.Format("The custom alias {0} is not redirected.", url));
        }

        [TestMethod]
        public void BVT_S02_TC07_CanLoadGettingStartedPageImage()
        {
            Pages.Navigation.Select("Getting Started");
            Assert.IsTrue(Pages.OfficeGettingStartedPage.CanLoadImage(), "Cannot load image in getting started page.");
        }

        [TestMethod]
        public void BVT_S02_TC08_CanLoadCodeSamplePageImages()
        {
            Browser.SetWaitTime(TimeSpan.FromSeconds(30));
            Pages.Navigation.Select("Code Samples");
            CodeSamplesPage page = new CodeSamplesPage();
            foreach (CodeSamplePageImages item in Enum.GetValues(typeof(CodeSamplePageImages)))
            {
                Assert.IsTrue(page.CanLoadImages(item));
            }

            Browser.SetWaitTime(TimeSpan.FromSeconds(Utility.DefaultWaitTime));
        }

        [TestMethod]
        public void BVT_S02_TC09_CanLoadResourcePageImages()
        {
            foreach (MenuItemOfResource item in Enum.GetValues(typeof(MenuItemOfResource)))
            {
                ResourcePage page;
                switch (item)
                {
                    case (MenuItemOfResource.AppRegistrationTool):
                    case (MenuItemOfResource.APISandbox):
                    case (MenuItemOfResource.MiniLabs):
                        break;
                    case (MenuItemOfResource.Training):
                        Browser.SetWaitTime(TimeSpan.FromSeconds(30));
                        Pages.Navigation.Select("Resources", item.ToString());
                        //Browser.Wait(TimeSpan.FromSeconds(20));
                        page = new ResourcePage();
                        foreach (ResourcePageImages image in Enum.GetValues(typeof(ResourcePageImages)))
                        {
                            Assert.IsTrue(page.CanLoadImage(image), string.Format("The images in resource page {0} is not loaded currectly.", item.ToString()));
                        }
                        Browser.SetWaitTime(TimeSpan.FromSeconds(Utility.DefaultWaitTime));
                        break;
                    default:
                        Pages.Navigation.Select("Resources", item.ToString());
                        //Browser.Wait(TimeSpan.FromSeconds(20));
                        page = new ResourcePage();
                        foreach (ResourcePageImages image in Enum.GetValues(typeof(ResourcePageImages)))
                        {
                            Assert.IsTrue(page.CanLoadImage(image), string.Format("The images in resource page {0} is not loaded currectly.", item.ToString()));
                        }
                        break;
                }
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Browser.Close();
        }
    }
}