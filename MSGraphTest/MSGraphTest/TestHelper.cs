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
    }
}
