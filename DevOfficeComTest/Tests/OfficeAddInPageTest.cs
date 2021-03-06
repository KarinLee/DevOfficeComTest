﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestFramework;
using TestFramework.OfficeAddInPage;

namespace Tests
{
    [TestClass]
    public class OfficeAddInPageTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Browser.SetWaitTime(TimeSpan.FromSeconds(Utility.DefaultWaitTime));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Browser.SwitchBack();
        }

        /// <summary>
        /// Verify whether the navigation item style can be updated when it is chosen or rejected.
        /// </summary>
        //[TestMethod]
        //public void Can_OfficeAddInNavItem_Style_Updated_Accordingly()
        //{
        //    #region Make all the nav items selectable
        //    //Randomly choose a product
        //    Product[] products = Enum.GetValues(typeof(Product)) as Product[];
        //    Random random = new Random();
        //    Product product = products[random.Next(0, products.Length)];
        //    Pages.OfficeAddInPage.CardChooseProduct.ChooseProduct(product);
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
        public void Comps_S10_TC01_CanChooseProduct_PowerPoint()
        {
            Product product = Product.PowerPoint;
            Pages.OfficeAddInPage.CardChooseProduct.ChooseProduct(product);
            Assert.IsTrue(Pages.OfficeAddInPage.CardChooseProduct.IsShowingProductExplore(product), "Failed to choose product {0}.", product.ToString());
            Assert.IsTrue(Pages.OfficeAddInPage.CardChooseProduct.IsShowingVideo(product), "Failed to show video about product {0}.", product.ToString());

        }

        [TestMethod]
        public void Acceptance_S09_TC01_CanGoThroughAddinPage_Excel()
        {
            // Select app
            Product product = Product.Excel;
            Pages.OfficeAddInPage.CardChooseProduct.ChooseProduct(product);
            Assert.IsTrue(Pages.OfficeAddInPage.CardChooseProduct.IsShowingProductExplore(product), "Failed to choose product {0}.", product.ToString());
            Assert.IsTrue(Pages.OfficeAddInPage.CardChooseProduct.IsShowingVideo(product), "Failed to show video about product {0}.", product.ToString());

            //Browser.SaveScreenShot(product.ToString());
            // Explore
            // Pages.OfficeAddInPage.CardExcel.Explore.play();

            // Build
            Pages.OfficeAddInPage.SelectBuildTool();

            // More Resource
            Pages.OfficeAddInPage.CardExcel.MoreResouces.DownLoadStarterSample();
            Assert.IsTrue(Pages.OfficeAddInPage.CardExcel.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open DownLoad Sample page.");
            Pages.OfficeAddInPage.CardExcel.MoreResouces.DesignYourAddIn();
            Assert.IsTrue(Pages.OfficeAddInPage.CardExcel.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Design guidelines page.");
            Pages.OfficeAddInPage.CardExcel.MoreResouces.MoreCodeSamples();
            Assert.IsTrue(Pages.OfficeAddInPage.CardExcel.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Code Samples page.");
            Pages.OfficeAddInPage.CardExcel.MoreResouces.OfficeAddInTypes();
            Assert.IsTrue(Pages.OfficeAddInPage.CardExcel.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Add-in Types page.");
            Pages.OfficeAddInPage.CardExcel.MoreResouces.PublishYourAddIn();
            Assert.IsTrue(Pages.OfficeAddInPage.CardExcel.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Publish AddIn page.");
            Pages.OfficeAddInPage.CardExcel.MoreResouces.ReadTheDocs();
            Assert.IsTrue(Pages.OfficeAddInPage.CardExcel.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Add-in overview page.");
        }

        [TestMethod]
        public void Acceptance_S09_TC02_CanGoThroughAddinPage_Outlook()
        {
            Pages.OfficeGettingStartedPage.OfficeAddInGetStarted();
            Assert.IsTrue(Pages.OfficeAddInPage.IsAtAddinPage(), "Failed to open Office Add-in Getting started page.");
            Assert.IsTrue(Pages.OfficeAddInPage.OnlyDefaultCardsDisplayed(), "Cards in Add-in page are not displayed correctly.");

            // Select app
            Product product = Product.Outlook;
            Pages.OfficeAddInPage.CardChooseProduct.ChooseProduct(product);
            Assert.IsTrue(Pages.OfficeAddInPage.CardChooseProduct.IsShowingProductExplore(product), "Failed to choose product {0}.", product.ToString());
            Assert.IsTrue(Pages.OfficeAddInPage.CardChooseProduct.IsShowingVideo(product), "Failed to choose product {0}.", product.ToString());
            Assert.IsTrue(Pages.OfficeAddInPage.IsCardDisplayed("build"), "Card with id 'build' in Add-in page is not displayed correctly.");
            Pages.OfficeAddInPage.SelectBuildTool(true);
            Assert.IsTrue(Pages.OfficeAddInPage.IsCardDisplayed("more"), "Card with id 'more' in Add-in page is not displayed correctly.");

            // More Resource
            Pages.OfficeAddInPage.CardOutlook.MoreResouces.OutlookDevCenter();
            Assert.IsTrue(Pages.OfficeAddInPage.CardOutlook.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Outlook Dev Center page.");
            Pages.OfficeAddInPage.CardOutlook.MoreResouces.DownLoadStarterSample();
            Assert.IsTrue(Pages.OfficeAddInPage.CardOutlook.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open DownLoad Sample page.");
            Pages.OfficeAddInPage.CardOutlook.MoreResouces.DesignYourAddIn();
            Assert.IsTrue(Pages.OfficeAddInPage.CardOutlook.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Design guidelines page.");
            Pages.OfficeAddInPage.CardOutlook.MoreResouces.MoreCodeSamples();
            Assert.IsTrue(Pages.OfficeAddInPage.CardOutlook.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Code Samples page.");
            Pages.OfficeAddInPage.CardOutlook.MoreResouces.OfficeAddInTypes();
            Assert.IsTrue(Pages.OfficeAddInPage.CardOutlook.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Add-in Types page.");
            Pages.OfficeAddInPage.CardOutlook.MoreResouces.PublishYourAddIn();
            Assert.IsTrue(Pages.OfficeAddInPage.CardOutlook.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Publish AddIn page.");
            Pages.OfficeAddInPage.CardOutlook.MoreResouces.ReadTheDocs();
            Assert.IsTrue(Pages.OfficeAddInPage.CardOutlook.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Add-in overview page.");
        }

        [TestMethod]
        public void Acceptance_S09_TC03_CanGoThroughAddinPage_PowerPoint()
        {
            // Select app
            Product product = Product.PowerPoint;
            Pages.OfficeAddInPage.CardChooseProduct.ChooseProduct(product);
            Assert.IsTrue(Pages.OfficeAddInPage.CardChooseProduct.IsShowingProductExplore(product), "Failed to choose product {0}.", product.ToString());
            Assert.IsTrue(Pages.OfficeAddInPage.CardChooseProduct.IsShowingVideo(product), "Failed to choose product {0}.", product.ToString());
            //Browser.SaveScreenShot(@"E:\\PowerPoint.jpeg");
            //Browser.SaveScreenShot(product.ToString());
            // Explore
            // Pages.OfficeAddInPage.CardPowerPoint.Explore.play();

            // Build
            Pages.OfficeAddInPage.SelectBuildTool(true);
            // More Resource
            Pages.OfficeAddInPage.CardPowerPoint.MoreResouces.DesignYourAddIn();
            Assert.IsTrue(Pages.OfficeAddInPage.CardPowerPoint.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Design guidelines page.");
            Pages.OfficeAddInPage.CardPowerPoint.MoreResouces.MoreCodeSamples();
            Assert.IsTrue(Pages.OfficeAddInPage.CardPowerPoint.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Code Samples page.");
            Pages.OfficeAddInPage.CardPowerPoint.MoreResouces.OfficeAddInTypes();
            Assert.IsTrue(Pages.OfficeAddInPage.CardPowerPoint.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Add-in Types page.");
            Pages.OfficeAddInPage.CardPowerPoint.MoreResouces.PublishYourAddIn();
            Assert.IsTrue(Pages.OfficeAddInPage.CardPowerPoint.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Publish AddIn page.");
            Pages.OfficeAddInPage.CardPowerPoint.MoreResouces.ReadTheDocs();
            Assert.IsTrue(Pages.OfficeAddInPage.CardPowerPoint.MoreResouces.IsShowingCorrectResourcePage(), "Failed to open Add-in overview page.");
        }

        [TestMethod]
        public void Acceptance_S09_TC04_CanGoThroughAddinPage_Word()
        {
            // Select app
            Product product = Product.Word;
            Pages.OfficeAddInPage.CardChooseProduct.ChooseProduct(product);
            Assert.IsTrue(Pages.OfficeAddInPage.CardChooseProduct.IsShowingProductExplore(product), "Failed to choose product {0}.", product.ToString());
            //Assert.IsTrue(Pages.OfficeAddInPage.CardChooseProduct.IsShowingExampleOrVideo(product), "Failed to choose product {0}.", product.ToString());
            //Browser.SaveScreenShot(product.ToString());
            // Explore
            // Pages.OfficeAddInPage.CardWord.Explore.play();

            // Build
            Pages.OfficeAddInPage.SelectBuildTool();

            // More Resource
            Pages.OfficeAddInPage.CardWord.MoreResources.DesignYourAddIn();
            Assert.IsTrue(Pages.OfficeAddInPage.CardWord.MoreResources.IsShowingCorrectResourcePage(), "Failed to open Design guidelines page.");
            Pages.OfficeAddInPage.CardWord.MoreResources.MoreCodeSamples();
            Assert.IsTrue(Pages.OfficeAddInPage.CardWord.MoreResources.IsShowingCorrectResourcePage(), "Failed to open Code Samples page.");
            Pages.OfficeAddInPage.CardWord.MoreResources.OfficeAddInTypes();
            Assert.IsTrue(Pages.OfficeAddInPage.CardWord.MoreResources.IsShowingCorrectResourcePage(), "Failed to open Add-in Types page.");
            Pages.OfficeAddInPage.CardWord.MoreResources.PublishYourAddIn();
            Assert.IsTrue(Pages.OfficeAddInPage.CardWord.MoreResources.IsShowingCorrectResourcePage(), "Failed to open Publish AddIn page.");
            Pages.OfficeAddInPage.CardWord.MoreResources.ReadTheDocs();
            Assert.IsTrue(Pages.OfficeAddInPage.CardWord.MoreResources.IsShowingCorrectResourcePage(), "Failed to open Add-in overview page.");
        }

        [TestMethod]
        public void BVT_S09_TC05_ShowTwoCardsByDefault()
        {
            Pages.OfficeGettingStartedPage.OfficeAddInGetStarted();
            Assert.IsTrue(Pages.OfficeAddInPage.IsAtAddinPage(), "Failed to open Office Add-in Getting started page.");

            // Should show and only show two cards by default 
            Assert.IsTrue(Pages.OfficeAddInPage.OnlyDefaultCardsDisplayed(), "Cards in Add-in page are not displayed correctly.");
        }

        [TestMethod]
        public void BVT_S09_TC06_CanLoadAddinPageImages()
        {
            Product product = Product.Word;
            Pages.OfficeAddInPage.CardChooseProduct.ChooseProduct(product);
            Assert.IsTrue(Pages.OfficeAddInPage.CanLoadImages());
        }

        /// <summary>
        /// Verify whether the steps of building using other tools can appear.
        /// </summary>
        [TestMethod]
        public void BVT_S09_TC07_CanShowContentBuildByOtherTools()
        {
            Array productArrays=Enum.GetValues(typeof(Product));
            int index = new Random().Next(productArrays.Length);

            Product product = (Product)productArrays.GetValue(index);
            Pages.OfficeAddInPage.CardChooseProduct.ChooseProduct(product);

            Pages.OfficeAddInPage.SelectBuildTool();
            Assert.IsTrue(Pages.OfficeAddInPage.BuildOtherToolsStepsExist(),
                "The steps to build using other tools should exist.");

            Assert.IsTrue(Pages.OfficeAddInPage.IsCardDisplayed("more"), "Card 'More Resources' in Add-in page should be displayed");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Browser.Close();
        }
    }
}
