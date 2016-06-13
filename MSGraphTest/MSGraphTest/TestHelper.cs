using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using TestFramework;

namespace MSGraphTest
{
    /// <summary>
    /// 
    /// </summary>
    public static class TestHelper
    {
        public static string[] allowedEnglishSites = { "msgraph-prod-lastgood.azurewebsites.net/en-us",
                                                "msgraph-prod-mirror.azurewebsites.net/en-us",
                                                "msgraph-release.azurewebsites.net/en-us",
                                                "graph.microsoft.io/en-us" };
        /// <summary>
        /// Verify whether the href of nav item "Graph explorer" refers to a specific address
        /// If yes, click it.
        /// </summary>
        public static string VerifyAndSelectExplorerOnNavBar()
        {
            string title = string.Empty;
            try
            {
                title = GraphPages.Navigation.Select("Graph explorer");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("a[contains(@href,'/graph-explorer')]"))
                {
                    Assert.Inconclusive("The link of Try the API is not updated as '/graph-explorer' as the production site");
                }
            }
            return title;
        }

        /// <summary>
        /// Signin on Explorer page
        /// </summary>
        /// <param name="userName">The user name used to sigin</param>
        /// <param name="userPassword">The user password used to signin</param>
        public static void SigninExplorer(string userName, string userPassword)
        {
            string lcn = GraphUtility.GetLCN();
            bool siteValid = false;
            for (int i = 0; i < allowedEnglishSites.Length; i++)
            {
                if (!lcn.Equals("en-us")||GraphBrowser.Url.Replace("/graph-explorer","").EndsWith(allowedEnglishSites[i]))
                {
                    siteValid = true;
                    break;
                }
            }
            if (!siteValid)
            {
                Assert.Inconclusive("{0} should not be used for a signin related test case",
                        GraphBrowser.BaseAddress);
            }

            if (!GraphUtility.IsLoggedIn())
            {
                GraphUtility.ClickLogin();

                GraphUtility.Login(
                    userName,
                    userPassword);
            }
        }
    }
}
