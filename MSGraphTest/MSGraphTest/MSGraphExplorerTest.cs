﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TestFramework;

namespace MSGraphTest
{
    /// <summary>
    /// Test Class for Microsoft Graph explorer page
    /// </summary>
    [TestClass]
    public class MSGraphExplorerTest
    {
        static string userName;
        static string userPassword;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            GraphBrowser.Initialize();
            string lcn = GraphUtility.GetLCN();
            if (lcn.Equals("zh-cn"))
            {
                userName = GraphUtility.GetConfigurationValue("GraphExplorerInChinaUserName");
                userPassword = GraphUtility.GetConfigurationValue("GraphExplorerInChinaPassword");
            }
            else
            {
                userName = GraphUtility.GetConfigurationValue("GraphExplorerUserName");
                userPassword = GraphUtility.GetConfigurationValue("GraphExplorerPassword");
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            GraphBrowser.Close();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            GraphBrowser.Goto(GraphBrowser.BaseAddress);
        }

        /// <summary>
        /// Verify whether login on Graph explorer page can succeed.
        /// </summary>
        [TestMethod]
        public void Acceptance_Graph_S05_TC01_CanLogin()
        {
            TestHelper.VerifyAndSelectExplorerOnNavBar();
            TestHelper.SigninExplorer(userName, userPassword);

            Assert.IsTrue(GraphUtility.IsLoggedIn(userName), "Login can succeed");
        }

        /// <summary>
        /// Verify whether request GET me can succeed. 
        /// </summary>
        [TestMethod]
        public void Comps_Graph_S05_TC02_CanGetMe()
        {
            TestHelper.VerifyAndSelectExplorerOnNavBar();

            if (!GraphUtility.IsLoggedIn(userName))
            {
                if (GraphUtility.IsLoggedIn())
                {
                    GraphUtility.ClickLogout();
                }
                TestHelper.SigninExplorer(userName, userPassword);
            }

            GraphUtility.InputExplorerQueryString("v1.0", "me" + "\n");
            GraphBrowser.Wait(TimeSpan.FromSeconds(10));
            string response = GraphUtility.GetExplorerResponse();
            string userPrincipalName = GraphUtility.GetProperty(response, "userPrincipalName");
            Assert.AreEqual(
                userName,
                userPrincipalName,
                @"GET ""me"" can obtain the correct response");
        }

        /// <summary>
        /// Verify Whether switching API version can get the correct response.
        /// </summary>
        [TestMethod]
        public void Comps_Graph_S05_TC03_CanSwitchAPIVersion()
        {
            TestHelper.VerifyAndSelectExplorerOnNavBar();
            TestHelper.SigninExplorer(userName, userPassword);

            //v1.0
            GraphUtility.InputExplorerQueryString("v1.0", "me" + "\n");
            GraphBrowser.Wait(TimeSpan.FromSeconds(10));
            string v10Response = GraphUtility.GetExplorerResponse();
            string oDataContext = GraphUtility.GetProperty(v10Response, "@odata.context");
            Assert.IsTrue(oDataContext.Contains("/v1.0"),
                "Setting a v1.0 query string should get a v1.0 response.");

            //vBeta
            GraphUtility.InputExplorerQueryString("beta", "me" + "\n");
            GraphBrowser.Wait(TimeSpan.FromSeconds(10));
            string betaResponse = GraphUtility.GetExplorerResponse();
            oDataContext = GraphUtility.GetProperty(betaResponse, "@odata.context");
            Assert.IsTrue(oDataContext.Contains("/beta"),
                "Setting a vBeta query string should get a vBeta response.");
        }

        /// <summary>
        /// Verify whether request PATCH can succeed.
        /// </summary>
        [TestMethod]
        public void Comps_Graph_S05_TC04_CanPatchMe()
        {
            TestHelper.VerifyAndSelectExplorerOnNavBar();
            TestHelper.SigninExplorer(userName, userPassword);

            //Change the operation from GET to PATCH
            GraphUtility.ClickButton("GET");
            GraphUtility.Click("PATCH");
            string jobTitle = "JobTitle_" + DateTime.Now.ToString("M/d/yyyy/hh/mm/ss");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("jobTitle", jobTitle);
            GraphUtility.InputExplorerJSONBody(dic);
            GraphUtility.InputExplorerQueryString("v1.0", "me" + "\n");
            GraphBrowser.WaitForExploreResponse();
            string patchResponse = GraphUtility.GetExplorerResponse();

            //Change the operation from PATCH to GET
            GraphUtility.ClickButton("PATCH");
            GraphUtility.Click("GET");
            GraphUtility.InputExplorerQueryString("v1.0", "me" + "\n");
            string getResponse = GraphUtility.GetExplorerResponse();
            //The response doesn't change means no GET response is returned.So wait and re-obtain it
            int waitTime = Int32.Parse(GraphUtility.GetConfigurationValue("WaitTime"));
            int retryCount = Int32.Parse(GraphUtility.GetConfigurationValue("RetryCount"));
            int i = 0;

            while (i < retryCount && getResponse == patchResponse)
            {
                GraphBrowser.Wait(TimeSpan.FromSeconds(waitTime));
                getResponse = GraphUtility.GetExplorerResponse();
                i++;
            }
            string newjobTitle = GraphUtility.GetProperty(getResponse, "jobTitle");
            Assert.AreEqual(jobTitle, newjobTitle, "The patched property should be updated accordingly");
        }

        

        /// <summary>
        /// Verify whether a group can be "Post"ed and "Delete"ed
        /// </summary>
        [TestMethod]
        public void Comps_Graph_S05_TC05_CanPostDeleteGroup()
        {
            TestHelper.VerifyAndSelectExplorerOnNavBar();

            int waitTime = Int32.Parse(GraphUtility.GetConfigurationValue("WaitTime"));
            int retryCount = Int32.Parse(GraphUtility.GetConfigurationValue("RetryCount"));

            if (!GraphUtility.IsLoggedIn())
            {
                GraphUtility.ClickLogin();

                GraphUtility.Login(
                    userName,
                    userPassword);
            }

            //Change the operation from GET to POST
            GraphUtility.ClickButton("GET");
            GraphUtility.Click("POST");

            Dictionary<string, string> postProperties = new Dictionary<string, string>();
            postProperties.Add("description", "A group for test");
            string groupDisplayName = "TestGroup_" + DateTime.Now.ToString("M/d/yyyy/hh/mm/ss");
            postProperties.Add("displayName", groupDisplayName);
            postProperties.Add("mailEnabled", "false");
            postProperties.Add("securityEnabled", "true");
            postProperties.Add("mailNickname", "TestGroupMail");
            GraphUtility.InputExplorerJSONBody(postProperties);
            GraphUtility.InputExplorerQueryString("v1.0", "groups" + "\n");
            GraphBrowser.WaitForExploreResponse();
            string postResponse = GraphUtility.GetExplorerResponse();
            string postID = GraphUtility.GetProperty(postResponse, "id");
            string postDisplayName = GraphUtility.GetProperty(postResponse, "displayName");
            // Reload the page to empty the response
            GraphBrowser.Refresh();
            //Check whether the created group can be gotten
            GraphUtility.InputExplorerQueryString("v1.0", "groups/" + postID + "\n");
            GraphBrowser.WaitForExploreResponse();
            string getResponse = GraphUtility.GetExplorerResponse();
            string getDisplayName = GraphUtility.GetProperty(getResponse, "displayName");
            Assert.AreEqual(
                postDisplayName,
                getDisplayName,
                "The posted group should be able to GET");

            // Reload the page to empty the response
            GraphBrowser.Refresh();
            GraphUtility.ClickButton("GET");
            GraphUtility.Click("DELETE");
            GraphUtility.InputExplorerQueryString("v1.0", "groups/" + postID + "\n");
            GraphBrowser.WaitForExploreResponse();
            string deleteResponse = GraphUtility.GetExplorerResponse();

            GraphUtility.Click("DELETE");
            GraphUtility.ClickButton("GET");
            GraphUtility.InputExplorerQueryString("v1.0", "groups/" + postID + "\n");
            int i = 0;
            do
            {
                GraphBrowser.Wait(TimeSpan.FromSeconds(waitTime));
                getResponse = GraphUtility.GetExplorerResponse();
                i++;
            } while (i < retryCount && getResponse.Equals(deleteResponse));

            Assert.IsTrue(
                getResponse.Contains("Request_ResourceNotFound"),
                "The group should be deleted successfully");
        }

        /// <summary>
        /// Verify whether there is a cooperation note should exist on Chinese explorer page.
        /// </summary>
        [TestMethod]
        public void Comps_Graph_S05_TC06_CanFindNoteOnChineseExplorerPage()
        {
            GraphBrowser.Goto(GraphUtility.RemoveRedundantPartsfromExtractBaseAddress() + "/zh-cn");
            TestHelper.VerifyAndSelectExplorerOnNavBar();
            bool isFound = GraphUtility.FindCHNExplorerNote();
            Assert.IsTrue(isFound, "A graph explorer note for China should exist at the bottom of the page");
        }

        /// <summary>
        /// Verify whether the request list on Chinese explorer page is valid.
        /// </summary>
        [TestMethod]
        public void Comps_Graph_S05_TC07_IsRequestListValidForChinaEndpoint()
        {
            GraphBrowser.Goto(GraphUtility.RemoveRedundantPartsfromExtractBaseAddress() + "/zh-cn");
            TestHelper.VerifyAndSelectExplorerOnNavBar();
            string incorrectRequest;
            bool isValid = GraphUtility.VerifyExplorerRequestListOnCHNEndpoint(out incorrectRequest);
            Assert.IsTrue(isValid,
                "{0} is incorrect on Chinese Explorer page",
                isValid ? "No request" : incorrectRequest);
        }
    }
}
