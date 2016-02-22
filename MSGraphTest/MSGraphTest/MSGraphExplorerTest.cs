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
            GraphBrowser.Goto(GraphUtility.GetConfigurationValue("MSGraphBaseAddress"));
        }

        /// <summary>
        /// Verify whether login on Graph explorer page can succeed.
        /// </summary>
        [TestMethod]
        public void Acceptance_Graph_S05_TC01_CanLogin()
        {
            GraphPages.Navigation.Select("Graph explorer");
            if (GraphUtility.IsLoggedIn())
            {
                GraphUtility.ClickLogout();
                GraphBrowser.Wait(TimeSpan.FromSeconds(5));
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
            GraphPages.Navigation.Select("Graph explorer");
            string userName = GraphUtility.GetConfigurationValue("GraphExplorerUserName");

            if (!GraphUtility.IsLoggedIn(userName))
            {
                if (GraphUtility.IsLoggedIn())
                {
                    GraphUtility.ClickLogout();
                    GraphBrowser.Wait(TimeSpan.FromSeconds(5));
                }
                GraphUtility.ClickLogin();

                GraphUtility.Login(
                    userName,
                    GraphUtility.GetConfigurationValue("GraphExplorerPassword"));
            }

            GraphUtility.InputExplorerQueryString("https://graph.microsoft.com/v1.0/me" + "\n");
            GraphBrowser.Wait(TimeSpan.FromSeconds(10));
            string response = GraphUtility.GetExplorerResponse();

            Assert.IsTrue(
                response.Contains(@"""mail"":""" + userName + @""""),
                @"GET ""me"" can obtain the correct response");
        }

        /// <summary>
        /// Verify Whether switching API version can get the correct response.
        /// </summary>
        [TestMethod]
        public void Comps_Graph_S05_TC03_CanSwitchAPIVersion()
        {
            GraphPages.Navigation.Select("Graph explorer");
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
            Assert.IsTrue(
                 v10Response.Contains(@"""@odata.context"":""https://graph.microsoft.com/v1.0"),
                 "Setting a v1.0 query string should get a v1.0 response.");

            //vBeta
            GraphUtility.InputExplorerQueryString("https://graph.microsoft.com/beta/me" + "\n");
            GraphBrowser.Wait(TimeSpan.FromSeconds(10));
            string betaResponse = GraphUtility.GetExplorerResponse();
            Assert.IsTrue(
                betaResponse.Contains(@"""@odata.context"":""https://graph.microsoft.com/beta"),
                "Setting a vBeta query string should get a vBeta response.");
        }

        /// <summary>
        /// Verify whether request PATCH can succeed.
        /// </summary>
        [TestMethod]
        public void Comps_Graph_S05_TC04_CanPatchMe()
        {
            GraphPages.Navigation.Select("Graph explorer");
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
            GraphBrowser.Wait(TimeSpan.FromSeconds(5));
            string getResponse = GraphUtility.GetExplorerResponse();

            Dictionary<string, string> gottenProperties = GraphUtility.ParseJsonFormatProperties(getResponse);
            Assert.AreEqual(jobTitle, gottenProperties["jobTitle"], "The patched property should be updated accordingly");
        }

        /// <summary>
        /// Verify whether a group can be "Post"ed and "Delete"ed
        /// </summary>
        [TestMethod]
        public void Comps_Graph_S05_TC05_CanPostDeleteGroup()
        {
            GraphPages.Navigation.Select("Graph explorer");
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
            Dictionary<string, string> postResponseProperties = GraphUtility.ParseJsonFormatProperties(postResponse);

            // Reload the page to empty the response
            GraphBrowser.Refresh();
            //Check whether the created group can be gotten
            GraphUtility.InputExplorerQueryString("https://graph.microsoft.com/v1.0/groups/" + postResponseProperties["id"] + "\n");
            GraphBrowser.WaitForExploreResponse();
            string getResponse = GraphUtility.GetExplorerResponse();
            Dictionary<string, string> getResponseProperties = GraphUtility.ParseJsonFormatProperties(getResponse);
            Assert.AreEqual(
                postResponseProperties["displayName"],
                getResponseProperties["displayName"],
                "The posted group should be able to GET");

            //Change the operation from GET to DELETE
            GraphUtility.ClickButton("GET");
            GraphUtility.Click("DELETE");
            GraphUtility.InputExplorerQueryString("https://graph.microsoft.com/v1.0/groups/" + postResponseProperties["id"] + "\n");
            GraphBrowser.Wait(TimeSpan.FromSeconds(4));

            GraphUtility.Click("DELETE");
            GraphUtility.ClickButton("GET");
            GraphUtility.InputExplorerQueryString("https://graph.microsoft.com/v1.0/groups/" + postResponseProperties["id"] + "\n");
            GraphBrowser.Wait(TimeSpan.FromSeconds(5));
            getResponse = GraphUtility.GetExplorerResponse();

            Assert.IsTrue(
                getResponse.Contains("\"code\":\"Request_ResourceNotFound\""),
                "The group should be deleted successfully");
        }
    }
}
