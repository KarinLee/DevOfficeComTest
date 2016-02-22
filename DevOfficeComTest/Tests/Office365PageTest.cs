using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestFramework;
using TestFramework.Office365Page;

namespace Tests
{
    [TestClass]
    public class Office365PageTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Browser.SetWaitTime(TimeSpan.FromSeconds(Utility.DefaultWaitTime));
        }

        // Design changed. No NavBar for now. So disable this case for the moment.
        /// <summary>
        /// Verify whether the navigation item style can be updated when it is chosen or rejected.
        /// </summary>
        //[TestMethod]
        //public void Can_Office365NavItem_Style_Updated_Accordingly()
        //{
        //    #region Make all the nav items selectable
        //    //Randomly choose a platform
        //    Platform[] platforms = Enum.GetValues(typeof(Platform)) as Platform[];
        //    Random random = new Random();
        //    Platform platform = platforms[random.Next(0, platforms.Length)];
        //    Pages.Office365Page.CardSetupPlatform.ChoosePlatform(platform);
            
        //    //Choose to sign in later
        //    Pages.Office365Page.CardRegisterApp.SigninLater();
        //    #endregion Make all the nav items selectable

        //    try
        //    {
        //        for (int i = 0; i < NavBar.NavItemCount; i++)
        //        {
        //            NavBar.SelectNavItem(i);
        //            NavBar.VerifyItemStyleCorrect(i);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        [TestMethod]
        public void Acceptance_S04_TC01_CanGoThroughO365API()
        {
            Pages.OfficeGettingStartedPage.Office365APIGetStarted();
            Assert.IsTrue(Pages.Office365Page.IsAtOffice365Page(), "Failed to open Office 365 APIs Getting started page.");
            Assert.IsTrue(Pages.Office365Page.OnlyDefaultCardsDisplayed(), "Cards in Office 365 page are not displayed correctly.");

            Pages.Office365Page.CardTryItOut.ChooseService(ServiceToTry.GetMessages);
            Pages.Office365Page.CardTryItOut.ClickTry();
            Assert.IsTrue(Pages.Office365Page.CardTryItOut.CanGetResponse(GetMessagesValue.Inbox), "Failed to get the response for the serivce to try.");

            Platform platform = Platform.Node;
            Pages.Office365Page.CardSetupPlatform.ChoosePlatform(platform);
            Assert.IsTrue(Pages.Office365Page.CardSetupPlatform.IsShowingPlatformSetup(platform), "Failed to choose platform {0}.", platform.ToString());
            Assert.IsFalse(Pages.Office365Page.IsCardDisplayed("setup-project"), "Card with id 'setup-project' in Office 365 page is not displayed correctly.");
            Assert.IsFalse(Pages.Office365Page.IsCardDisplayed("next-step"), "Card with id 'next-step' in Office 365 page is not displayed correctly.");

            Pages.Office365Page.CardRegisterApp.SigninLater();
            Assert.IsTrue(Pages.Office365Page.IsCardDisplayed("setup-project"), "Card with id 'setup-project' in Office 365 page is not displayed correctly.");
            Assert.IsTrue(Pages.Office365Page.IsCardDisplayed("next-step"), "Card with id 'next-step' in Office 365 page is not displayed correctly.");
            Assert.IsFalse(Pages.Office365Page.IsCardDisplayed("AllSet"), "Card with id 'AllSet' in Office 365 page is not displayed correctly.");
            Pages.Office365Page.CardDownloadCode.DownloadCode(); 
            Assert.IsTrue(Pages.Office365Page.CardDownloadCode.IsCodeDownloaded(), "Failed to download code.");
            Assert.IsTrue(Pages.Office365Page.IsCardDisplayed("AllSet"), "Card with id 'AllSet' in Office 365 page is not displayed correctly.");

            Pages.Office365Page.CardMoreResources.OutlookDevCenter();
            Assert.IsTrue(Pages.Office365Page.CardMoreResources.IsShowingCorrectResourcePage(), "Failed to open Outlook Dev Center page.");
            Pages.Office365Page.CardMoreResources.Training();
            Assert.IsTrue(Pages.Office365Page.CardMoreResources.IsShowingCorrectResourcePage(), "Failed to open Training page.");
            Pages.Office365Page.CardMoreResources.APIReferences();
            Assert.IsTrue(Pages.Office365Page.CardMoreResources.IsShowingCorrectResourcePage(), "Failed to open API References page.");
            Pages.Office365Page.CardMoreResources.CodeSamples();
            Assert.IsTrue(Pages.Office365Page.CardMoreResources.IsShowingCorrectResourcePage(), "Failed to open Code Samples page.");
            Pages.Office365Page.CardMoreResources.AzureAppAndPermissions();
            Assert.IsTrue(Pages.Office365Page.CardMoreResources.IsShowingCorrectResourcePage(), "Failed to open Azure app & Permissions page.");
            Pages.Office365Page.CardMoreResources.AddToO365AppLauncher();
            Assert.IsTrue(Pages.Office365Page.CardMoreResources.IsShowingCorrectResourcePage(), "Failed to open O365 App Launcher page.");
            Pages.Office365Page.CardMoreResources.SubmitToOfficeStore();
            Assert.IsTrue(Pages.Office365Page.CardMoreResources.IsShowingCorrectResourcePage(), "Failed to open Submit to Office Store page.");
        }

        [TestMethod]
        public void BVT_S04_TC02_ShowThreeCardsByDefault()
        {
            Pages.OfficeGettingStartedPage.Office365APIGetStarted();
            Assert.IsTrue(Pages.Office365Page.IsAtOffice365Page(), "Failed to open Office 365 APIs Getting started page.");

            // should show and only show first 3 cards
            Assert.IsTrue(Pages.Office365Page.OnlyDefaultCardsDisplayed(), "Cards in Office 365 page are not displayed correctly.");
        }

        [TestMethod]
        public void Acceptance_S05_TC01_CanTryO365API_GetUsers()
        {
            ServiceToTry service = ServiceToTry.GetUsers;
            Pages.Office365Page.CardTryItOut.ChooseService(service);
            int currentWidth = 0;
            int currentHeight = 0;
            Browser.GetWindowSize(out currentWidth, out currentHeight);
            if (currentWidth > Utility.MinWidthToShowParam)
            {
                foreach (GetUsersValue item in Enum.GetValues(typeof(GetUsersValue)))
                {
                    Pages.Office365Page.CardTryItOut.ChooseServiceValue(item);
                    Pages.Office365Page.CardTryItOut.ClickTry();
                    Assert.IsTrue(Pages.Office365Page.CardTryItOut.CanGetResponse(item), string.Format("The service {0} with parameter {1} is not work.", service.ToString(), item.ToString()));
                    Assert.IsTrue(Pages.Office365Page.CardTryItOut.UrlContainsParameterValue(), string.Format("The parameter {0} of service {1} is not contained in the url.", item.ToString(), service.ToString()));
                }
            }
            else
            {
                Pages.Office365Page.CardTryItOut.ClickTry();
                Assert.IsTrue(Pages.Office365Page.CardTryItOut.CanGetResponse(GetUsersValue.me), string.Format("The service {0} with parameter {1} is not work.", service.ToString(), GetUsersValue.me.ToString()));
                Assert.IsTrue(Pages.Office365Page.CardTryItOut.UrlContainsParameterValue(), string.Format("The parameter {0} of service {1} is not contained in the url.", GetUsersValue.me.ToString(), service.ToString()));
            }
        }

        [TestMethod]
        public void Comps_S05_TC02_CanTryO365API_GetGroups()
        {
            ServiceToTry service = ServiceToTry.GetGroups;
            Pages.Office365Page.CardTryItOut.ChooseService(service);
            int currentWidth = 0;
            int currentHeight = 0;
            Browser.GetWindowSize(out currentWidth, out currentHeight);
            if (currentWidth > Utility.MinWidthToShowParam)
            {
                foreach (GetGroupValue item in Enum.GetValues(typeof(GetGroupValue)))
                {
                    Pages.Office365Page.CardTryItOut.ChooseServiceValue(item);
                    Pages.Office365Page.CardTryItOut.ClickTry();
                    Assert.IsTrue(Pages.Office365Page.CardTryItOut.CanGetResponse(item), string.Format("The service {0} with parameter {1} is not work.", service.ToString(), item.ToString()));
                    Assert.IsTrue(Pages.Office365Page.CardTryItOut.UrlContainsParameterValue(), string.Format("The parameter {0} of service {1} is not contained in the url.", item.ToString(), service.ToString()));
                }
            }
            else
            {
                Pages.Office365Page.CardTryItOut.ClickTry();
                Assert.IsTrue(Pages.Office365Page.CardTryItOut.CanGetResponse(GetGroupValue.me_memberOf), string.Format("The service {0} with parameter {1} is not work.", service.ToString(), GetGroupValue.me_memberOf.ToString()));
                Assert.IsTrue(Pages.Office365Page.CardTryItOut.UrlContainsParameterValue(), string.Format("The parameter {0} of service {1} is not contained in the url.", GetGroupValue.me_memberOf.ToString(), service.ToString()));
            }
        }

        [TestMethod]
        public void Comps_S05_TC03_CanTryO365API_GetMessages()
        {
            ServiceToTry service = ServiceToTry.GetMessages;
            Pages.Office365Page.CardTryItOut.ChooseService(service);
            int currentWidth = 0;
            int currentHeight = 0;
            Browser.GetWindowSize(out currentWidth, out currentHeight);
            if (currentWidth > Utility.MinWidthToShowParam)
            {
                foreach (GetMessagesValue item in Enum.GetValues(typeof(GetMessagesValue)))
                {
                    Pages.Office365Page.CardTryItOut.ChooseServiceValue(item);
                    Pages.Office365Page.CardTryItOut.ClickTry();
                    Assert.IsTrue(Pages.Office365Page.CardTryItOut.CanGetResponse(item), string.Format("The service {0} with parameter {1} is not work.", service.ToString(), item.ToString()));
                    Assert.IsTrue(Pages.Office365Page.CardTryItOut.UrlContainsServiceName(), string.Format("The name of service {0} is not contained in the url.", service.ToString()));
                    Assert.IsTrue(Pages.Office365Page.CardTryItOut.UrlContainsParameterValue(), string.Format("The parameter {0} of service {1} is not contained in the url.", item.ToString(), service.ToString()));
                }
            }
            else
            {
                Pages.Office365Page.CardTryItOut.ClickTry();
                Assert.IsTrue(Pages.Office365Page.CardTryItOut.CanGetResponse(GetMessagesValue.Inbox), string.Format("The service {0} with parameter {1} is not work.", service.ToString(), GetMessagesValue.Inbox.ToString()));
                Assert.IsTrue(Pages.Office365Page.CardTryItOut.UrlContainsServiceName(), string.Format("The name of service {0} is not contained in the url.", service.ToString()));
                Assert.IsTrue(Pages.Office365Page.CardTryItOut.UrlContainsParameterValue(), string.Format("The parameter {0} of service {1} is not contained in the url.", GetMessagesValue.Inbox.ToString(), service.ToString()));
            }
        }

        [TestMethod]
        public void Comps_S05_TC04_CanTryO365API_GetFiles()
        {
            ServiceToTry service = ServiceToTry.GetFiles;
            Pages.Office365Page.CardTryItOut.ChooseService(service);
            int currentWidth = 0;
            int currentHeight = 0;
            Browser.GetWindowSize(out currentWidth, out currentHeight);
            if (currentWidth > Utility.MinWidthToShowParam)
            {
                foreach (GetFilesValue item in Enum.GetValues(typeof(GetFilesValue)))
                {
                    Pages.Office365Page.CardTryItOut.ChooseServiceValue(item);
                    Pages.Office365Page.CardTryItOut.ClickTry();
                    Assert.IsTrue(Pages.Office365Page.CardTryItOut.CanGetResponse(item), string.Format("The service {0} with parameter {1} is not work.", service.ToString(), item.ToString()));
                    Assert.IsTrue(Pages.Office365Page.CardTryItOut.UrlContainsParameterValue(), string.Format("The parameter {0} of service {1} is not contained in the url.", item.ToString(), service.ToString()));
                }
            }
            else
            {
                Pages.Office365Page.CardTryItOut.ClickTry();
                Assert.IsTrue(Pages.Office365Page.CardTryItOut.CanGetResponse(GetFilesValue.drive_root_children), string.Format("The service {0} with parameter {1} is not work.", service.ToString(), GetFilesValue.drive_root_children.ToString()));
                Assert.IsTrue(Pages.Office365Page.CardTryItOut.UrlContainsParameterValue(), string.Format("The parameter {0} of service {1} is not contained in the url.", GetFilesValue.drive_root_children.ToString(), service.ToString()));
            }
        }

        [TestMethod]
        public void Comps_S05_TC05_CanTryO365API_GetEvents()
        {
            ServiceToTry service = ServiceToTry.GetEvents;
            Pages.Office365Page.CardTryItOut.ChooseService(service);
            Pages.Office365Page.CardTryItOut.ClickTry();
            Assert.IsTrue(Pages.Office365Page.CardTryItOut.CanGetResponse(null));
            Assert.IsTrue(Pages.Office365Page.CardTryItOut.UrlContainsServiceName(), string.Format("The name of service {0} is not contained in the url.", service.ToString()));
        }

        [TestMethod]
        public void Comps_S05_TC06_CanTryO365API_GetContacts()
        {
            ServiceToTry service = ServiceToTry.GetContacts;
            Pages.Office365Page.CardTryItOut.ChooseService(service);
            Pages.Office365Page.CardTryItOut.ClickTry();
            Assert.IsTrue(Pages.Office365Page.CardTryItOut.CanGetResponse(null));
            Assert.IsTrue(Pages.Office365Page.CardTryItOut.UrlContainsServiceName(), string.Format("The name of service {0} is not contained in the url.", service.ToString()));
        }

        [TestMethod]
        public void Comps_S05_TC07_ParameterChangedBySwitchingService()
        {
            foreach (ServiceToTry service in Enum.GetValues(typeof(ServiceToTry)))
            {
                Pages.Office365Page.CardTryItOut.ChooseService(service);
                bool correctUrl = false;
                bool correctParameter = false;
                switch (service)
                {
                    case ServiceToTry.GetMessages:
                        correctParameter = Pages.Office365Page.CardTryItOut.ChooseServiceValue(GetMessagesValue.Inbox);
                        correctUrl = Pages.Office365Page.CardTryItOut.UrlContainsServiceName();
                        break;
                    case ServiceToTry.GetEvents:
                    case ServiceToTry.GetContacts:
                        correctParameter = !Pages.Office365Page.CardTryItOut.IsParameterTableDisplayed();
                        correctUrl = Pages.Office365Page.CardTryItOut.UrlContainsServiceName();
                        break;
                    case ServiceToTry.GetFiles:
                        correctParameter = Pages.Office365Page.CardTryItOut.ChooseServiceValue(GetFilesValue.drive_root_children);
                        correctUrl = Pages.Office365Page.CardTryItOut.UrlContainsParameterValue();
                        break;
                    case ServiceToTry.GetUsers:
                        correctParameter = Pages.Office365Page.CardTryItOut.ChooseServiceValue(GetUsersValue.me);
                        correctUrl = Pages.Office365Page.CardTryItOut.UrlContainsParameterValue();
                        break;
                    case ServiceToTry.GetGroups:
                        correctParameter = Pages.Office365Page.CardTryItOut.ChooseServiceValue(GetGroupValue.me_memberOf);
                        correctUrl = Pages.Office365Page.CardTryItOut.UrlContainsParameterValue();
                        break;
                    default:
                        break;
                }

                Assert.IsTrue(correctParameter, string.Format("The parameter of service {0} is not changed accordingly.", service.ToString()));
                Assert.IsTrue(correctUrl, string.Format("The url in the service {0} is not changed accordingly.", service.ToString()));
            }
        }

        [TestMethod]
        public void BVT_S06_TC01_CanChoosePlatform()
        {
            foreach (Platform item in Enum.GetValues(typeof(Platform)))
            {
                Pages.Office365Page.CardSetupPlatform.ChoosePlatform(item);
                Assert.IsTrue(Pages.Office365Page.CardSetupPlatform.IsShowingPlatformSetup(item), "Failed to choose platform {0}.", item.ToString());
            }
        }

        //[TestMethod]
        //public void Can_SignIn_OfficeDevAccount()
        //{
        //    Pages.Office365Page.CardRegisterApp.SigninAs("Tester@devexperience.onmicrosoft.com")
        //        .WithPassword("Password02@")
        //        .Signin();
        //    Assert.IsTrue(Pages.Office365Page.CardRegisterApp.IsSignedin("Tester@devexperience.onmicrosoft.com"), "Failed to sign in.");
        //}

        [TestMethod]
        public void Acceptance_S08_TC01_CanDownloadCode_Node()
        {
            Platform platform = Platform.Node;
            Pages.Office365Page.CardSetupPlatform.ChoosePlatform(platform);
            Assert.IsTrue(Pages.Office365Page.CardSetupPlatform.IsShowingPlatformSetup(platform), "Failed to choose platform {0}.", platform.ToString());

            Pages.Office365Page.CardRegisterApp.SigninLater();            

            Pages.Office365Page.CardDownloadCode.DownloadCode();
            Assert.IsTrue(Pages.Office365Page.CardDownloadCode.IsCodeDownloaded(), "Failed to download code.");
        }

        [TestMethod]
        public void BVT_S08_TC02_CanLoadOffice365PageImages()
        {
            Platform platform = Platform.PHP;
            Pages.Office365Page.CardSetupPlatform.ChoosePlatform(platform);
            foreach (Office365Images item in Enum.GetValues(typeof(Office365Images)))
            {
                Assert.IsTrue(Pages.Office365Page.CanLoadImages(item));
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Browser.Close();
        }
    }
}
