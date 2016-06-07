using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFramework;

namespace Tests
{
    /// <summary>
    /// Test class for build videos page
    /// </summary>
    [TestClass]
    public class BuildVideoPageTest
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
            Browser.GoBack();
        }
        
        #endregion

        /// <summary>
        /// Check videos' "Get Started" links
        /// </summary>
        [TestMethod]
        public void BVT_S19_TC01_CheckGetStartedLinks()
        {
            Pages.Navigation.Select("Resources", "BuildVideos");
            string videoTitle;
            Utility.CheckBuildVideosPageGetStartedLinks(out videoTitle);
            if (!videoTitle.Equals(string.Empty))
            {
                Assert.Fail("{0}'s Get Started link does nor refer to the correct page",
                    videoTitle);
            }
        }

        /// <summary>
        /// Check build-video' Share icon and shared link
        /// </summary>
        [TestMethod]
        public void BVT_S19_TC02_CheckShareOnTwitter()
        {
            Pages.Navigation.Select("Resources", "BuildVideos");
            string videoTitle;
            string sharedLink;
            int randomIndex = new Random().Next(Utility.GetBuildVideoCount());
            Utility.CheckBuildVideosPageShareOnTwitter(randomIndex,out videoTitle, out sharedLink);
            if (!videoTitle.Equals(string.Empty))
            {
                Assert.Fail(@"{0}'s Twitter-shared link: ""{1}"" is incorrect",
                    videoTitle,
                    sharedLink);
            }
        }
    }
}
