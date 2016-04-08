using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            GraphBrowser.Initialize();
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
            if (GraphUtility.IsLoggedIn())
            {
                GraphUtility.ClickLogout();
            }
            GraphUtility.ClickLogin();
            GraphUtility.Login(
                GraphUtility.GetConfigurationValue("GraphExplorerUserName"),
                GraphUtility.GetConfigurationValue("GraphExplorerPassword"));
            Assert.IsTrue(GraphUtility.IsLoggedIn(GraphUtility.GetConfigurationValue("GraphExplorerUserName")), "");
        }

        /// <summary>
        /// Verify whether request GET me can succeed. 
        /// </summary>
        [TestMethod]
        public void Comps_Graph_S05_TC02_CanGetMe()
        {
            TestHelper.VerifyAndSelectExplorerOnNavBar();
            string userName = GraphUtility.GetConfigurationValue("GraphExplorerUserName");

            if (!GraphUtility.IsLoggedIn(userName))
            {
                if (GraphUtility.IsLoggedIn())
                {
                    GraphUtility.ClickLogout();
                }
                GraphUtility.ClickLogin();

                GraphUtility.Login(
                    userName,
                    GraphUtility.GetConfigurationValue("GraphExplorerPassword"));
            }

            GraphUtility.InputExplorerQueryString("https://graph.microsoft.com/v1.0/me" + "\n");
            GraphBrowser.Wait(TimeSpan.FromSeconds(10));
            string response = GraphUtility.GetExplorerResponse();
            string mail = GraphUtility.GetProperty(response, "mail");
            Assert.AreEqual(
                userName,
                mail,
                @"GET ""me"" can obtain the correct response");
        }

        /// <summary>
        /// Verify Whether switching API version can get the correct response.
        /// </summary>
        [TestMethod]
        public void Comps_Graph_S05_TC03_CanSwitchAPIVersion()
        {
            TestHelper.VerifyAndSelectExplorerOnNavBar();
            string userName = GraphUtility.GetConfigurationValue("GraphExplorerUserName");

            if (!GraphUtility.IsLoggedIn())
            {
                GraphUtility.ClickLogin();

                GraphUtility.Login(
                    userName,
                    GraphUtility.GetConfigurationValue("GraphExplorerPassword"));
            }
            //v1.0
            GraphUtility.InputExplorerQueryString("https://graph.microsoft.com/v1.0/me" + "\n");
            GraphBrowser.Wait(TimeSpan.FromSeconds(10));
            string v10Response = GraphUtility.GetExplorerResponse();
            string oDataContext = GraphUtility.GetProperty(v10Response, "@odata.context");
            Assert.IsTrue(oDataContext.Contains("https://graph.microsoft.com/v1.0"),
                "Setting a v1.0 query string should get a v1.0 response.");

            //vBeta
            GraphUtility.InputExplorerQueryString("https://graph.microsoft.com/beta/me" + "\n");
            GraphBrowser.Wait(TimeSpan.FromSeconds(10));
            string betaResponse = GraphUtility.GetExplorerResponse();
            oDataContext = GraphUtility.GetProperty(betaResponse, "@odata.context");
            Assert.IsTrue(oDataContext.Contains("https://graph.microsoft.com/beta"),
                "Setting a vBeta query string should get a vBeta response.");
        }

        /// <summary>
        /// Verify whether request PATCH can succeed.
        /// </summary>
        [TestMethod]
        public void Comps_Graph_S05_TC04_CanPatchMe()
        {
            TestHelper.VerifyAndSelectExplorerOnNavBar();
            string userName = GraphUtility.GetConfigurationValue("GraphExplorerUserName");

            if (!GraphUtility.IsLoggedIn())
            {
                GraphUtility.ClickLogin();

                GraphUtility.Login(
                    userName,
                    GraphUtility.GetConfigurationValue("GraphExplorerPassword"));
            }
            //Change the operation from GET to PATCH
            GraphUtility.ClickButton("GET");
            GraphUtility.Click("PATCH");
            string jobTitle = "JobTitle_" + DateTime.Now.ToString("M/d/yyyy/hh/mm/ss");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("jobTitle", jobTitle);
            GraphUtility.InputExplorerJSONBody(dic);
            GraphUtility.InputExplorerQueryString("https://graph.microsoft.com/v1.0/me" + "\n");
            GraphBrowser.WaitForExploreResponse();
            string patchResponse = GraphUtility.GetExplorerResponse();

            //Change the operation from PATCH to GET
            GraphUtility.ClickButton("PATCH");
            GraphUtility.Click("GET");
            GraphUtility.InputExplorerQueryString("https://graph.microsoft.com/v1.0/me" + "\n");
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

            string userName = GraphUtility.GetConfigurationValue("GraphExplorerUserName");

            if (!GraphUtility.IsLoggedIn())
            {
                GraphUtility.ClickLogin();

                GraphUtility.Login(
                    userName,
                    GraphUtility.GetConfigurationValue("GraphExplorerPassword"));
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
            GraphUtility.InputExplorerQueryString("https://graph.microsoft.com/v1.0/groups" + "\n");
            GraphBrowser.WaitForExploreResponse();
            string postResponse = GraphUtility.GetExplorerResponse();
            string postID = GraphUtility.GetProperty(postResponse, "id");
            string postDisplayName = GraphUtility.GetProperty(postResponse, "displayName");
            // Reload the page to empty the response
            GraphBrowser.Refresh();
            //Check whether the created group can be gotten
            GraphUtility.InputExplorerQueryString("https://graph.microsoft.com/v1.0/groups/" + postID + "\n");
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
            GraphUtility.InputExplorerQueryString("https://graph.microsoft.com/v1.0/groups/" + postID + "\n");
            GraphBrowser.WaitForExploreResponse();
            string deleteResponse = GraphUtility.GetExplorerResponse();

            GraphUtility.Click("DELETE");
            GraphUtility.ClickButton("GET");
            GraphUtility.InputExplorerQueryString("https://graph.microsoft.com/v1.0/groups/" + postID + "\n");
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
    }
}
