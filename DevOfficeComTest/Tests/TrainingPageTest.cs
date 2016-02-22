using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFramework;

namespace Tests
{
    /// <summary>
    /// Test class for Resources-->Training page 
    /// </summary>
    [TestClass]
    public class TrainingPageTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Browser.Initialize();
            Browser.SetWaitTime(TimeSpan.FromSeconds(30));
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Browser.Close();
        }

        /// <summary>
        /// Verify whether the filters in Training page can navigate to correct results
        /// </summary>
        [TestMethod]
        public void Acceptance_S14_TC01_Can_Filter_Trainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            int filterCount = Utility.GetFilterCount();

            for (int i = 0; i < filterCount; i++)
            {
                string filterName = Utility.SelectFilter(i);

                List<SearchedResult> resultList = Utility.GetFilterResults();
                Assert.AreNotEqual<int>(0,
                    resultList.Count,
                    "If select the filter {0}, there should be at least one training displayed",
                    filterName);
            }
        }

        /// <summary>
        /// Verify the search function in Resources->Training
        /// </summary>
        [TestMethod]
        public void Acceptance_S14_TC02_Can_Search_CorrectTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            int randomIndex = new Random().Next(Utility.TypicalSearchText.Length);
            string searchString = Utility.TypicalSearchText[randomIndex];

            List<SearchedResult> resultList = Utility.GetFilterResults(searchString);
            foreach (SearchedResult resultInfo in resultList)
            {
                bool isNameMatched = resultInfo.Name.ToLower().Contains(searchString.ToLower());
                bool isDescriptionMatched = resultInfo.Description.ToLower().Contains(searchString.ToLower());
                Assert.IsTrue(isNameMatched || isDescriptionMatched,
                    "The training:\n {0}:{1}\n should contain the search text: {2}",
                    resultInfo.Name,
                    resultInfo.Description,
                    searchString);
            }
        }

        /// <summary>
        /// Verify whether the trainings can be sorted by view count correctly
        /// </summary>
        [TestMethod]
        public void Acceptance_S14_TC03_Can_Sort_Trainings_By_ViewCount()
        {
            Pages.Navigation.Select("Resources", "Training");
            int filterCount = Utility.GetFilterCount();

            //Randomly choose two filters to check
            int randomIndex;
            int usedIndex = filterCount;
            for (int i = 0; i < 2; i++)
            {
                do
                {
                    randomIndex = new Random().Next(filterCount);
                } while (randomIndex == usedIndex);
                string filterName = Utility.SelectFilter(randomIndex);

                // Set the sort order as descendent
                Utility.SetSortOrder(SortType.ViewCount, true);
                List<SearchedResult> resultList = Utility.GetFilterResults();
                for (int j = 0; j < resultList.Count - 1; j++)
                {
                    Assert.IsTrue(resultList[j].ViewCount >= resultList[j + 1].ViewCount,
                        @"When {0} filter is chosen and the sort order is descendent, the view count of ""{1}"" should be larger than or equal to its sibling ""{2}""'s",
                        filterName,
                        resultList[j].Name,
                        resultList[j + 1].Name);
                }

                // Set the sort order as ascendent
                Utility.SetSortOrder(SortType.ViewCount, false);
                resultList = Utility.GetFilterResults();
                for (int j = 0; j < resultList.Count - 1; j++)
                {
                    Assert.IsTrue(resultList[j].ViewCount <= resultList[j + 1].ViewCount,
                        @"When {0} filter is chosen and the sort order is ascendent, the view count of ""{1}"" should be smaller than or equal to its sibling ""{2}""'s",
                        filterName,
                        resultList[j].Name,
                        resultList[j + 1].Name);
                }
                usedIndex = randomIndex;
            }
        }

        /// <summary>
        /// Verify whether the URL can be updated accordingly when a filter is chosen
        /// </summary>
        [TestMethod]
        public void BVT_S14_TC04_Can_URL_Updated_By_TrainingFilter()
        {
            Pages.Navigation.Select("Resources", "Training");
            int filterCount = Utility.GetFilterCount();
            string filterName;

            //Generate the index of filters to select
            int randomIndex = new Random().Next(filterCount);

            filterName = Utility.SelectFilter(randomIndex);

            List<string> unContainedFilters;
            Assert.IsTrue(Utility.AreFiltersInURL(new List<string> { filterName }, out unContainedFilters),
                "The filter {0} should be contained in URL!",
                filterName);
        }

        /// <summary>
        /// Verify if choosing the filter Introduction to Office 365 Development can get any correct training. 
        /// </summary>
        [TestMethod]
        public void Comps_S14_TC05_CanFindOffice365DevelopmentTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            Utility.SelectFilter("Introduction to Office 365 Development");
            bool hasFounded = false;

            List<SearchedResult> resultList = Utility.GetFilterResults();
            foreach (SearchedResult resultInfo in resultList)
            {
                bool isNameMatched = resultInfo.Name.ToLower().Contains("office 365 development");
                bool isDescriptionMatched = resultInfo.Description.ToLower().Contains("office 365 development");
                if (isNameMatched || isDescriptionMatched)
                {
                    hasFounded = true;
                    break;
                }
            }
            Assert.IsTrue(hasFounded, "There should be at least one training which meets the filter Introduction to Office 365 Development");
        }

        /// <summary>
        /// Verify if choosing the filter Transform SharePoint Customizations to SharePoint Add-in Model can get any correct training. 
        /// </summary>
        [TestMethod]
        public void Comps_S14_TC06_CanFindSharePointAddinModelTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            Utility.SelectFilter("Transform SharePoint Customizations to SharePoint Add-in Model");
            bool hasFounded = false;

            List<SearchedResult> resultList = Utility.GetFilterResults();
            foreach (SearchedResult resultInfo in resultList)
            {
                bool isNameMatched = resultInfo.Name.ToLower().Contains("sharepoint add-in model");
                bool isDescriptionMatched = resultInfo.Description.ToLower().Contains("sharepoint add-in model");
                if (isNameMatched || isDescriptionMatched)
                {
                    hasFounded = true;
                    break;
                }
            }
            Assert.IsTrue(hasFounded, "There should be at least one training which meets the filter Transform SharePoint Customizations to SharePoint Add-in Model");
        }

        /// <summary>
        /// Verify if choosing the filter Deep Dive into the Office 365 Add-in Model can get any correct training. 
        /// </summary>
        [TestMethod]
        public void Comps_S14_TC07_CanFindOffice365AddinModelTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            Utility.SelectFilter("Deep Dive into the Office 365 Add-in Model");
            bool hasFounded = false;

            List<SearchedResult> resultList = Utility.GetFilterResults();
            foreach (SearchedResult resultInfo in resultList)
            {
                bool isNameMatched = resultInfo.Name.ToLower().Contains("office add-in");
                bool isDescriptionMatched = resultInfo.Description.ToLower().Contains("office add-in");
                if (isNameMatched || isDescriptionMatched)
                {
                    hasFounded = true;
                    break;
                }
            }
            Assert.IsTrue(hasFounded, "There should be at least one training which meets the filter Deep Dive into the Office 365 Add-in Model");
        }

        /// <summary>
        /// Verify if choosing the filter Deep Dive Integrate Office 365 APIs in Your Web Apps can get any correct training. 
        /// </summary>
        [TestMethod]
        public void Comps_S14_TC08_CanFindIntegrateOffice365APIsTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            Utility.SelectFilter("Deep Dive Integrate Office 365 APIs in Your Web Apps");
            bool hasFounded = false;

            List<SearchedResult> resultList = Utility.GetFilterResults();
            foreach (SearchedResult resultInfo in resultList)
            {
                bool isNameMatched = resultInfo.Name.ToLower().Contains("office 365 apis");
                bool isDescriptionMatched = resultInfo.Description.ToLower().Contains("office 365 apis");
                if (isNameMatched || isDescriptionMatched)
                {
                    hasFounded = true;
                    break;
                }
            }
            Assert.IsTrue(hasFounded, "There should be at least one training which meets the filter Deep Dive Integrate Office 365 APIs in Your Web Apps");
        }

        /// <summary>
        /// Verify if choosing the filter Deep Dive Building Blocks and Services of SharePoint can get any correct training. 
        /// </summary>
        [TestMethod]
        public void Comps_S14_TC09_CanFindBuildingBlocksandServicesofSharePointTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            Utility.SelectFilter("Deep Dive Building Blocks and Services of SharePoint");
            bool hasFounded = false;

            List<SearchedResult> resultList = Utility.GetFilterResults();
            foreach (SearchedResult resultInfo in resultList)
            {
                bool isNameMatched = resultInfo.Name.ToLower().Contains("sharepoint");
                bool isDescriptionMatched = resultInfo.Description.ToLower().Contains("sharepoint");
                if (isNameMatched || isDescriptionMatched)
                {
                    hasFounded = true;
                    break;
                }
            }
            Assert.IsTrue(hasFounded, "There should be at least one training which meets the filter Deep Dive Building Blocks and Services of SharePoint");
        }

        /// <summary>
        /// Verify if choosing the filter Property Manager Hero Demo Deep Dive can get any correct training. 
        /// </summary>
        [TestMethod]
        public void Comps_S14_TC10_CanFindPropertyManagerHeroDemoDeepDiveTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            Utility.SelectFilter("Property Manager Hero Demo Deep Dive");
            bool hasFounded = false;

            List<SearchedResult> resultList = Utility.GetFilterResults();
            foreach (SearchedResult resultInfo in resultList)
            {
                bool isNameMatched = resultInfo.Name.ToLower().Contains("property management");
                bool isDescriptionMatched = resultInfo.Description.ToLower().Contains("property management");
                if (isNameMatched || isDescriptionMatched)
                {
                    hasFounded = true;
                    break;
                }
            }
            Assert.IsTrue(hasFounded, "There should be at least one training which meets the filter Property Manager Hero Demo Deep Dive");
        }

        /// <summary>
        /// Verify if choosing the filter Shipping Your Office App to the Office Store can get any correct training. 
        /// </summary>
        [TestMethod]
        public void Comps_S14_TC11_CanFindShippingYourOfficeApptotheOfficeStoreTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            Utility.SelectFilter("Shipping Your Office App to the Office Store");
            bool hasFounded = false;

            List<SearchedResult> resultList = Utility.GetFilterResults();
            foreach (SearchedResult resultInfo in resultList)
            {
                bool isNameMatched = resultInfo.Name.ToLower().Contains("app store");
                bool isDescriptionMatched = resultInfo.Description.ToLower().Contains("app store");
                if (isNameMatched || isDescriptionMatched)
                {
                    hasFounded = true;
                    break;
                }
            }
            Assert.IsTrue(hasFounded, "There should be at least one training which meets the filter Shipping Your Office App to the Office Store");
        }

        /// <summary>
        /// Verify if choosing the filter Independent Courses can get any correct training. 
        /// </summary>
        [TestMethod]
        public void Comps_S14_TC12_CanFindIndependentCoursesTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            Utility.SelectFilter("Independent Courses");

            List<SearchedResult> resultList = Utility.GetFilterResults();

            Assert.IsTrue(resultList.Count > 0, "There should be at least one training which meets the filter Independent Courses");
        }

        /// <summary>
        /// Verify if choosing the filter Deep Dive into Office 365 Development on non-Microsoft Stack can get any correct training. 
        /// </summary>
        [TestMethod]
        public void Comps_S14_TC13_CanFindIndependentCoursesTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            Utility.SelectFilter("Deep Dive into Office 365 Development on non-Microsoft Stack");

            List<SearchedResult> resultList = Utility.GetFilterResults();

            Assert.IsTrue(resultList.Count > 0, "There should be at least one training which meets the filter Deep Dive into Office 365 Development on non-Microsoft Stack");
        }

        /// <summary>
        /// Verify if choosing the filter Deep Dive: Integrate Office 365 APIs in Your Mobile Device Apps can get any correct training. 
        /// </summary>
        [TestMethod]
        public void Comps_S14_TC14_CanFindIntegrateOffice365APIsinMobileDeviceAppsTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            Utility.SelectFilter("Deep Dive: Integrate Office 365 APIs in Your Mobile Device Apps");
            bool hasFounded = false;

            List<SearchedResult> resultList = Utility.GetFilterResults();
            foreach (SearchedResult resultInfo in resultList)
            {
                bool isNameMatched = resultInfo.Name.ToLower().Contains("mobile device");
                bool isDescriptionMatched = resultInfo.Description.ToLower().Contains("mobile device");
                if (isNameMatched || isDescriptionMatched)
                {
                    hasFounded = true;
                    break;
                }
            }
            Assert.IsTrue(hasFounded, "There should be at least one training which meets the filter Deep Dive: Integrate Office 365 APIs in Your Mobile Device Apps");
        }

        /// <summary>
        /// Verify if choosing the filter Ignite 2015 can get any correct training. 
        /// </summary>
        [TestMethod]
        public void Comps_S14_TC15_CanFindIgniteTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            Utility.SelectFilter("Ignite 2015");
            bool hasFounded = false;

            List<SearchedResult> resultList = Utility.GetFilterResults();
            foreach (SearchedResult resultInfo in resultList)
            {

                if (resultInfo.DetailLink.Contains("myignite.microsoft.com"))
                {
                    hasFounded = true;
                    break;
                }
            }
            Assert.IsTrue(hasFounded, "There should be at least one training which meets the filter Ignite 2015");
        }

        /// <summary>
        /// Verify if choosing the filter Build 2015 can get any correct training. 
        /// </summary>
        [TestMethod]
        public void Comps_S14_TC16_CanFindBuildTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            Utility.SelectFilter("Build 2015");
            bool hasFounded = false;

            List<SearchedResult> resultList = Utility.GetFilterResults();
            foreach (SearchedResult resultInfo in resultList)
            {

                if (resultInfo.DetailLink.Contains("Events/Build/2015"))
                {
                    hasFounded = true;
                    break;
                }
            }
            Assert.IsTrue(hasFounded, "There should be at least one training which meets the filter Build 2015");
        }

        /// <summary>
        /// Verify if choosing the filter SAP Gateway for Microsoft can get any correct training. 
        /// </summary>
        [TestMethod]
        public void Comps_S14_TC17_CanFindSAPGatewayforMicrosoftTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            Utility.SelectFilter("SAP Gateway for Microsoft");
            bool hasFounded = false;

            List<SearchedResult> resultList = Utility.GetFilterResults();
            foreach (SearchedResult resultInfo in resultList)
            {
                bool isNameMatched = resultInfo.Name.Contains("SAP");
                bool isDescriptionMatched = resultInfo.Description.Contains("SAP");
                if (isNameMatched || isDescriptionMatched)
                {
                    hasFounded = true;
                    break;
                }
            }
            Assert.IsTrue(hasFounded, "There should be at least one training which meets the filter SAP Gateway for Microsoft");
        }

    }
}
